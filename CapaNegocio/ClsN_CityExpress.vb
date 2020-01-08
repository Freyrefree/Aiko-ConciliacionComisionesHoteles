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

    Public Class ClsN_CityExpress

        Public NombreConciliacionCityExpress As String
        Public TablaConciliacion As New DataTable
        Public idProveedor As Int32

        Private objetoCapaDatos As ClsCityExpress = New ClsCityExpress()

        Public Function CN_DatosCityExpress()

            Return objetoCapaDatos.CD_DatosCityExpress()

        End Function

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


            If (NombreConciliacionCityExpress <> "") Then

                objetoCapaDatos.idProveedor = Me.idProveedor
                objetoCapaDatos.NombreConciliacionPosadas = Me.NombreConciliacionCityExpress
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

        Public Function CN_SelectCityExpress() As DataTable

            'ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            ClsGlobales.FechaProveedorInicio = ClsGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectCityExpress()

            Return tabla

        End Function

        Public Function CN_cargaDocCityExpress(ruta As String, indexHoja As Int16)

            If ruta <> "" And indexHoja <> -1 Then

                Dim respuesta As Boolean = CN_cargaArchivoCityExpress(ruta, indexHoja)

                If (respuesta) Then
                    'Poner fecha proveedor
                    CN_agregarMesProveedor(ClsNGlobales.LastID)


                    If ClsNGlobales.TipoPlantillaCityExpress = 1 Then

                        CN_addFirtsNameLastName()
                        objetoCapaDatos.CD_addNoNoches()
                        CN_quitarAcentos()
                    End If


                    Return True
                Else

                    Return False

                End If

            Else
                MsgBox("Verifique los campos Archivo y Hoja")
            End If


        End Function

        Public Function CN_cargaArchivoCityExpress(ruta, indexHoja)

            ClsGlobales.TipoPlantillaCityExpress = ClsNGlobales.TipoPlantillaCityExpress

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim cityexpress As DataTable = result(indexHoja)

                    If cityexpress.Rows.Count > 0 Then

                        If (CN_DatosCityExpress()) Then

                            Dim res As Boolean = CN_InsertarPendientesCityExpress(cityexpress)
                            Return res

                        Else


                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoCityExpress(cityexpress)
                            Return res

                        End If

                        'Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoCityExpress(cityexpress)
                        'Return res

                    Else
                        MsgBox("El Archivo Del Proveedor No Tiene Datos")
                    End If
                End Using
            End Using

        End Function

        Public Sub CN_addFirtsNameLastName()

            objetoCapaDatos.CD_addFirtsNameLastName()

        End Sub

        Public Function CN_InsertarPendientesCityExpress(cityexpress)



            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncateCityExpressTmp()
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesCityExpressTmp(cityexpress)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesCityExpress()

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


        Public Function ListaMatchAutomaticoCityExpress(idProveedorGlobal, listAutomatico)

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

        Public Function ListaMatchManualCityExpress(idProveedorGlobal, valuesList)

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


        Public Sub CN_quitarAcentos()

            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim firstName As String
            Dim lastName As String

            Dim firstNameB As String
            Dim lastNameB As String

            tabla = objetoCapaDatos.CD_ConsultaAcentos()

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty
                    firstName = vbEmpty
                    lastName = vbEmpty

                    firstNameB = vbEmpty
                    lastNameB = vbEmpty

                    id = row("id").ToString()
                    firstName = row("firstName").ToString()
                    lastName = row("lastName").ToString()


                    If (firstName <> Nothing) Then

                        firstNameB = RemoveDiacritics(firstName)
                        objetoCapaDatos.CD_QuitarAcentoFirstName(id, firstNameB)

                    End If

                    If (lastName <> Nothing) Then

                        lastNameB = RemoveDiacritics(lastName)
                        objetoCapaDatos.CD_QuitarAcentoLastName(id, lastNameB)

                    End If




                Next

            End If




        End Sub

        Public Function RemoveDiacritics(stIn As String) As String

            Dim stFormD As String = stIn.Normalize(NormalizationForm.FormD)
            Dim sb As New StringBuilder()

            For ich As Integer = 0 To stFormD.Length - 1
                Dim uc As UnicodeCategory = CharUnicodeInfo.GetUnicodeCategory(stFormD(ich))
                If uc <> UnicodeCategory.NonSpacingMark Then
                    sb.Append(stFormD(ich))
                End If
            Next

            Return (sb.ToString().Normalize(NormalizationForm.FormC))

        End Function

        Public Function CN_SelectSinConciliar() As DataTable

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla = objetoCapaDatos.CD_SelectSinConciliar()

            Return tabla

        End Function



        Public Sub CN_EliminarCityExpress(tabla)




            Dim id As String

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty

                    id = row("id").ToString()
                    objetoCapaDatos.CD_EliminarCityExpress(id)

                Next

            End If

        End Sub


        Public Function CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor


            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            Return tabla

        End Function

        Public Function CN_ObtenerUltimoId()

            Return objetoCapaDatos.CD_ObtenerUltimoId()

        End Function

        Public Sub CN_agregarMesProveedor(lastId)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            objetoCapaDatos.CD_agregarMesProveedor(lastId)

        End Sub

    End Class

End Namespace
