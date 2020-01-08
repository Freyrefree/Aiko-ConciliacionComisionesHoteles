Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos
Imports CapaDatos.CapaDatos
Imports System.IO
Imports ExcelDataReader
Imports System.Globalization

Namespace CapaNegocio


    Public Class ClsN_GestionCommtrack

        Private objetoCapaDatos As ClsGestionCommtrack = New ClsGestionCommtrack()

        Public NombreConciliacionGestionCommtrack As String
        Public TablaConciliacion As New DataTable
        Public idProveedor As Int32

        Public Function CN_GuardarConciliacion()

            Dim idConciliacion As Integer
            Dim dim_value As String
            Dim FechaApp As String
            Dim UserSpec As String
            Dim Segmento As String
            Dim CodigoConfirmacion As String
            Dim Comision As String
            Dim Operador As String
            Dim Moneda As String
            Dim CostoTotalDeLaReserva As String
            Dim Noches As String
            Dim ComOrig As String
            Dim SequenceNo As String
            Dim TipoConciliacion As String


            If (NombreConciliacionGestionCommtrack <> "") Then

                objetoCapaDatos.idProveedor = Me.idProveedor
                objetoCapaDatos.NombreConciliacionPosadas = Me.NombreConciliacionGestionCommtrack
                Dim response As Integer = objetoCapaDatos.CD_GuardarConciliacion()

                If (response <> 0) Then
                    For Each row As DataRow In TablaConciliacion.Rows


                        idConciliacion = vbEmpty
                        dim_value = vbEmpty
                        FechaApp = vbEmpty
                        UserSpec = vbEmpty
                        Segmento = vbEmpty
                        CodigoConfirmacion = vbEmpty
                        Comision = vbEmpty
                        Operador = vbEmpty
                        Moneda = vbEmpty
                        CostoTotalDeLaReserva = vbEmpty
                        Noches = vbEmpty
                        ComOrig = vbEmpty
                        SequenceNo = vbEmpty

                        TipoConciliacion = vbEmpty

                        objetoCapaDatos.idConciliacion = response
                        objetoCapaDatos.dim_value = row("dim_value").ToString()
                        objetoCapaDatos.FechaApp = row("FechaApp").ToString()
                        objetoCapaDatos.UserSpec = row("UserSpec").ToString()
                        objetoCapaDatos.Segmento = row("Segmento").ToString()
                        objetoCapaDatos.CodigoConfirmacion = row("CodigoConfirmacion").ToString()
                        objetoCapaDatos.Comision = row("Comision").ToString()
                        objetoCapaDatos.Operador = row("Operador").ToString()
                        objetoCapaDatos.Moneda = row("Moneda").ToString()
                        objetoCapaDatos.CostoTotalDeLaReserva = row("CostoTotalDeLaReserva").ToString()
                        objetoCapaDatos.Noches = row("Noches").ToString()
                        objetoCapaDatos.ComOrig = row("ComOrig").ToString()
                        objetoCapaDatos.SequenceNo = row("SequenceNo").ToString()

                        objetoCapaDatos.TipoConciliacion = row("TipoConciliacion").ToString()

                        objetoCapaDatos.CD_GuardarConciliacionDetalle()

                    Next
                End If

            End If

        End Function

        Public Function CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            Return tabla

        End Function




        Public Function CN_DatosGestionCommtrack()

            Return objetoCapaDatos.CD_DatosGestionCommtrack()

        End Function

        Public Function CN_SelectGestionCommtrack() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectGestionCommtrack()

            Return tabla

        End Function

        Public Function CN_cargaDocGestionCommtrack(ruta As String, indexHoja As Int16)

            If ruta <> "" And indexHoja <> -1 Then

                Dim respuesta As Boolean = CN_cargaArchivoGestionCommtrack(ruta, indexHoja)


                If (respuesta) Then

                    'Poner fecha proveedor
                    CN_agregarMesProveedor(ClsNGlobales.LastID)


                    objetoCapaDatos.CD_CamposTrim()

                    objetoCapaDatos.CD_AddNotrxconcatenada()
                    CN_Montototaldelareserva()
                    CN_ModificarFecha()

                    Return True

                Else
                    Return False

                End If

            Else
                MsgBox("Verifique los campos Archivo y Hoja")
            End If


        End Function

        Public Function CN_cargaArchivoGestionCommtrack(ruta, indexHoja)

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim GestionCommtrack As DataTable = result(indexHoja)

                    If GestionCommtrack.Rows.Count > 0 Then


                        If (CN_DatosGestionCommtrack()) Then


                            Dim res As Boolean = CN_InsertarPendientesGestionCommtrack(GestionCommtrack)
                            Return res

                        Else

                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoGestionCommtrack(GestionCommtrack)
                            Return res

                        End If

                        'Aplicar trim

                    Else
                        MsgBox("El Archivo Del Proveedor No Tiene Datos")
                    End If
                End Using
            End Using

        End Function

        Public Function CN_InsertarPendientesGestionCommtrack(GestionCommtrack)

            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncateGestionCommtrackTmp()
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesGestionCommtrackTmp(GestionCommtrack)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesGestionCommtrack()

                    If (resF) Then
                        Return True
                    End If

                Else

                    Return False

                End If
            Else

                Return False

            End If

        End Function



        Public Function ListaMatchAutomaticoGestionCommtrack(idProveedorGlobal, listAutomatico)

            'Dim list As New List(Of String)

            Dim multiList As New List(Of List(Of String))

            Dim cadena As String = ""

            Dim first As String = ""
            Dim second As String = ""

            Dim third As String = ""
            Dim fourth As String = ""

            Dim fifth As String = ""
            Dim sixth As String = ""

            Dim a As String()
            Dim b As String()

            Dim c As String()
            Dim d As String()

            Dim e As String()
            Dim f As String()

            Dim columnaBCD As String = ""
            Dim columnaCliente As String = ""
            Dim tipoOperaion As String = ""

            Dim i As Int16 = 0

            Dim stringA As String = ""
            Dim stringB As String = ""

            If (idProveedorGlobal <> Nothing) Then

                For Each item In listAutomatico


                    stringA = item.Item1
                    stringB = item.Item2

                    cadena = ""
                    cadena = stringB.ToString()

                    'columna BCD
                    a = cadena.Split(New Char() {"["c})
                    first = Trim(a(1)).ToString()

                    b = first.Split(New Char() {"<"c})
                    second = Trim(b(0)).ToString()
                    '*************************

                    'columna Proveedores
                    c = cadena.Split(New Char() {">"c})
                    third = Trim(c(1)).ToString()

                    d = third.Split(New Char() {"]"c})
                    fourth = Trim(d(0)).ToString()

                    '*************************



                    'tipoOperacion
                    e = cadena.Split(New Char() {"("c})
                    fifth = Trim(e(1)).ToString()

                    f = fifth.Split(New Char() {")"c})
                    sixth = Trim(f(0)).ToString()

                    '*************************

                    columnaBCD = second
                    columnaCliente = fourth
                    tipoOperaion = sixth


                    multiList.Add(New List(Of String))
                    multiList(i).Add(columnaBCD)
                    multiList(i).Add(columnaCliente)
                    multiList(i).Add(tipoOperaion)

                    i = i + 1

                Next

                Return multiList
            Else

                MsgBox("Seleccione un Proveedor")
                Return multiList

            End If


        End Function


        Public Sub CN_Montototaldelareserva()

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim Rate As String
            Dim nitec As String

            Dim total As Int64



            tabla = objetoCapaDatos.CD_SelectGestionCommtrackMontoReserva()

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty
                    Rate = vbEmpty
                    nitec = vbEmpty
                    total = vbEmpty

                    id = row("id").ToString()
                    'Rate = row("Rate")

                    Rate = Convert.ToString(row("Rate"))
                    'nitec = row("nitec")

                    nitec = Convert.ToString(row("nitec"))


                    If (Rate <> Nothing) Then

                        If (Rate.Contains("-")) Then

                        Else

                            If Rate = "" Then
                                Rate = 0
                                Rate = Double.Parse(Rate)
                            Else
                                Rate = Double.Parse(Rate)
                            End If

                            If nitec = "" Then
                                nitec = 0
                                nitec = Double.Parse(nitec)
                            Else
                                nitec = Double.Parse(nitec)
                            End If

                            total = Rate * nitec

                            objetoCapaDatos.CD_AddMontototaldelareserva(id, total.ToString())

                        End If

                    End If

                Next

            End If


        End Sub

        Public Sub CN_ModificarFecha()

            'eliminar dato erroneo'

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            objetoCapaDatos.CD_EliminarErroneo()


            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim dateIn As String
            Dim dateOut As String

            Dim dateInB As String
            Dim dateOutB As String

            Dim year As String
            Dim dd As String
            Dim mm As String



            tabla = objetoCapaDatos.CD_SelectGestionCommtrackMontoReserva()

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty
                    dateIn = vbEmpty
                    dateOut = vbEmpty

                    Try

                        id = row("id").ToString()
                        'dateIn = Trim(row("DIN"))
                        'dateOut = Trim(row("OUT"))
                        dateIn = Convert.ToString(row("DIN"))
                        dateOut = Convert.ToString(row("OUT"))


                        If (dateIn <> "" And dateIn <> Nothing) Then

                            If (dateIn.Length = 8) Then

                                year = dateIn.Substring(0, 4)
                                mm = dateIn.Substring(4, 2)
                                dd = dateIn.Substring(6, 2)

                                dateInB = year & "-" & mm & "-" & dd

                                objetoCapaDatos.CD_UpdateDATEIN(id, dateInB)


                            End If

                        End If

                        If (dateOut <> "" And dateOut <> Nothing) Then

                            If (dateOut.Length = 8) Then

                                year = dateOut.Substring(0, 4)
                                mm = dateOut.Substring(4, 2)
                                dd = dateOut.Substring(6, 2)

                                dateOutB = year & "-" & mm & "-" & dd
                                objetoCapaDatos.CD_UpdateDATEOUT(id, dateOutB)


                            End If

                        End If
                    Catch ex As Exception

                        'MsgBox(ex)


                    End Try








                    'If (Rate.Contains("-")) Then

                    'Else

                    '    total = Rate * nitec



                    '    objetoCapaDatos.CD_AddMontototaldelareserva(id, total.ToString())

                    'End If




                Next

            End If

        End Sub



        Public Function ListaMatchManualGestionCommtrack(idProveedorGlobal, valuesList)



            'Dim list As New List(Of String)

            Dim multiList As New List(Of List(Of String))

            Dim cadena As String = ""

            Dim first As String = ""
            Dim second As String = ""

            Dim third As String = ""
            Dim fourth As String = ""

            Dim fifth As String = ""
            Dim sixth As String = ""

            Dim seventh As String = ""
            Dim eighth As String = ""

            Dim a As String()
            Dim b As String()

            Dim c As String()
            Dim d As String()

            Dim e As String()
            Dim f As String()

            Dim g As String()
            Dim h As String()

            Dim columnaBCD As String = ""
            Dim columnaCliente As String = ""
            Dim tipoOperaion As String = ""
            Dim diasRango As String = ""

            Dim i As Int16 = 0

            Dim stringA As String = ""
            Dim stringB As String = ""

            If (idProveedorGlobal <> Nothing) Then

                For Each item In valuesList



                    stringA = item.Item1
                    stringB = item.Item2

                    cadena = ""

                    cadena = stringB.ToString()

                    'columna BCD
                    a = cadena.Split(New Char() {"["c})
                    first = Trim(a(1)).ToString()

                    b = first.Split(New Char() {"<"c})
                    second = Trim(b(0)).ToString()
                    '*************************

                    'columna Proveedores
                    c = cadena.Split(New Char() {">"c})
                    third = Trim(c(1)).ToString()

                    d = third.Split(New Char() {"]"c})
                    fourth = Trim(d(0)).ToString()

                    '*************************



                    'tipoOperacion
                    e = cadena.Split(New Char() {"("c})
                    fifth = Trim(e(1)).ToString()

                    f = fifth.Split(New Char() {")"c})
                    sixth = Trim(f(0)).ToString()

                    '*************************

                    columnaBCD = second
                    columnaCliente = fourth
                    tipoOperaion = sixth

                    If (tipoOperaion = "RANGO") Then

                        'Número de Rango de Días

                        g = cadena.Split(New Char() {"("c})
                        seventh = Trim(e(2)).ToString()

                        h = seventh.Split(New Char() {")"c})
                        eighth = Trim(h(0)).ToString()

                        '*************************

                    End If


                    multiList.Add(New List(Of String))
                    multiList(i).Add(columnaBCD)
                    multiList(i).Add(columnaCliente)
                    multiList(i).Add(tipoOperaion)

                    If (tipoOperaion = "RANGO") Then

                        diasRango = eighth

                        multiList(i).Add(diasRango)

                    Else
                        multiList(i).Add("0")
                    End If

                    i = i + 1

                Next

                Return multiList
            Else

                MsgBox("Seleccione un Proveedor")
                Return multiList

            End If


        End Function



        Public Function CN_SelectSinConciliar() As DataTable

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            Dim tabla = objetoCapaDatos.CD_SelectSinConciliar()

            Return tabla

        End Function

        Public Sub CN_EliminarGestionCommtrack(tabla)


            Dim id As String

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty

                    id = row("id").ToString()
                    objetoCapaDatos.CD_EliminarGestionCommtrack(id)

                Next

            End If

        End Sub


        Public Function CN_ObtenerUltimoId()

            Return objetoCapaDatos.CD_ObtenerUltimoId()

        End Function

        Public Sub CN_agregarMesProveedor(lastId)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            objetoCapaDatos.CD_agregarMesProveedor(lastId)

        End Sub

    End Class

End Namespace
