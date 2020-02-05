Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos
Imports CapaDatos.CapaDatos
Imports System.IO
Imports ExcelDataReader
Imports System.Globalization
Imports System.Text.RegularExpressions

Namespace CapaNegocio

    Public Class ClsN_Tacs

        Public NombreConciliacionTacs As String
        Public TablaConciliacion As New DataTable
        Public idProveedor As Int32


        Private objetoCapaDatos As ClsTacs = New ClsTacs()

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


            If (NombreConciliacionTacs <> "") Then

                objetoCapaDatos.idProveedor = Me.idProveedor
                objetoCapaDatos.NombreConciliacionTacs = Me.NombreConciliacionTacs
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

        Public Function CN_DatosTacs()

            Return objetoCapaDatos.CD_DatosTacs()

        End Function

        Public Function CN_SelectTacs() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectTacs()

            Return tabla

        End Function

        Public Function CN_cargaDocTacs(ruta As String, indexHoja As Int16)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            If ruta <> "" And indexHoja <> -1 Then

                Dim respuesta As Boolean = CN_cargaArchivoTacs(ruta, indexHoja)

                If (respuesta) Then

                    CN_agregarMesProveedor(ClsNGlobales.LastID)

                    CN_ModificarFecha()

                    objetoCapaDatos.CD_InsertarTacsPagadas()
                    objetoCapaDatos.CD_InsertarTacsObservaciones()

                    'Dim res1 As Boolean = CN_addFirtsNameLastName()
                    'Dim res2 As Boolean = CN_addTotalReserva()
                    'Dim res3 As Boolean = CN_addNoNoches()
                    'objetoCapaDatos.CD_updateComision()
                    'CN_updateComision()



                    'If (res1 And res2 And res3) Then
                    Return True
                    'Else
                    'Return False
                    'End If

                Else

                    Return False

                End If

            Else
                MsgBox("Verifique los campos Archivo y Hoja")
            End If


        End Function


        Public Function CN_SelectTacsPagadas() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectTacsPagadas()

            Return tabla

        End Function

        Public Function CN_SelectTacsObservaciones() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectTacsObservaciones()

            Return tabla

        End Function


        Public Function CN_cargaArchivoTacs(ruta, indexHoja)

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()

                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim tacs As DataTable = result(indexHoja)


                    Dim dtCloned As DataTable = tacs.Clone()


                    'dtCloned.Columns(3).DataType = GetType(String)
                    'dtCloned.Columns(4).DataType = GetType(String)
                    'dtCloned.Columns(5).DataType = GetType(String)
                    'dtCloned.Columns(6).DataType = GetType(String)
                    'dtCloned.Columns(7).DataType = GetType(String)

                    'dtCloned.Columns(8).DataType = GetType(Decimal)
                    'dtCloned.Columns(9).DataType = GetType(Decimal)



                    For Each row As DataRow In tacs.Rows
                        dtCloned.ImportRow(row)
                    Next

                    dtCloned.AcceptChanges()



                    If dtCloned.Rows.Count > 0 Then


                        'For Each row As DataRow In dtCloned.Rows
                        '    Dim PayeeIDfromPayor As String
                        '    Dim Iata As String
                        '    Dim AgencyZip As String
                        '    Dim PaymentID As String
                        '    Dim ChequeNumber As String
                        '    Dim PayDate As String
                        '    Dim RoomNights As String

                        '    PayeeIDfromPayor = row("Payee ID from Payor")
                        '    If (PayeeIDfromPayor <> "" Or PayeeIDfromPayor IsNot Nothing) Then
                        '        PayeeIDfromPayor = Regex.Match(PayeeIDfromPayor, "\d+").Value
                        '    End If



                        '    '****************************************************************************
                        '    Iata = Convert.ToInt64(If(TypeOf row("Iata") Is DBNull, 0, row("Iata")))

                        '    If (Iata <> "" Or Iata IsNot Nothing) Then
                        '        Iata = Regex.Match(Iata, "\d+").Value
                        '    End If


                        '    '****************************************************************************
                        '    AgencyZip = Convert.ToInt64(If(TypeOf row("Agency Zip") Is DBNull, 0, row("Agency Zip")))

                        '    If (AgencyZip <> "" Or AgencyZip IsNot Nothing) Then
                        '        AgencyZip = Regex.Match(AgencyZip, "\d+").Value
                        '    End If
                        '    '****************************************************************************
                        '    PaymentID = Convert.ToInt64(If(TypeOf row("Payment ID") Is DBNull, 0, row("Payment ID")))

                        '    If (PaymentID <> "" Or PaymentID IsNot Nothing) Then
                        '        PaymentID = Regex.Match(PaymentID, "\d+").Value
                        '    End If
                        '    '****************************************************************************
                        '    ChequeNumber = Convert.ToInt64(If(TypeOf row("Cheque Number") Is DBNull, 0, row("Cheque Number")))

                        '    If (ChequeNumber <> "" Or ChequeNumber IsNot Nothing) Then
                        '        ChequeNumber = Regex.Match(ChequeNumber, "\d+").Value
                        '    End If
                        '    '****************************************************************************
                        '    PayDate = Convert.ToInt64(If(TypeOf row("Pay Date") Is DBNull, 0, row("Pay Date")))

                        '    If (PayDate <> "" Or PayDate IsNot Nothing) Then
                        '        PayDate = Regex.Match(PayDate, "\d+").Value
                        '    End If
                        '    '****************************************************************************
                        '    RoomNights = Convert.ToInt64(If(TypeOf row("Room Nights") Is DBNull, 0, row("Room Nights")))

                        '    If (RoomNights <> "" Or RoomNights IsNot Nothing) Then
                        '        RoomNights = Regex.Match(RoomNights, "\d+").Value
                        '    End If
                        '    '****************************************************************************

                        '    PayeeIDfromPayor = Convert.ToInt64(PayeeIDfromPayor)
                        '    Iata = Convert.ToInt64(Iata)
                        '    AgencyZip = Convert.ToInt64(AgencyZip)
                        '    PaymentID = Convert.ToInt64(PaymentID)
                        '    ChequeNumber = Convert.ToInt64(ChequeNumber)
                        '    PayDate = Convert.ToInt64(PayDate)
                        '    RoomNights = Convert.ToInt64(RoomNights)

                        '    row("Payee ID from Payor") = PayeeIDfromPayor
                        '    row("Iata") = Iata
                        '    row("Agency Zip") = AgencyZip
                        '    row("Payment ID") = PaymentID
                        '    row("Cheque Number") = ChequeNumber
                        '    row("Pay Date") = PayDate
                        '    row("Room Nights") = RoomNights
                        '    '****************************************************************************



                        'Next


                        If (CN_DatosTacs()) Then


                            Dim res As Boolean = CN_InsertarPendientesTacs(dtCloned)
                            Return res

                        Else

                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoTacs(dtCloned)
                            Return res

                        End If



                    Else
                        MsgBox("El Archivo Del Proveedor No Tiene Datos")
                    End If
                End Using
            End Using

        End Function







        Public Function CN_InsertarPendientesTacs(tacs)

            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncateTacsTmp()
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesTacsTmp(tacs)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesTacs()

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


        Public Sub CN_changeTC(tc)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor


            objetoCapaDatos.CD_changePaidCommission(tc)



        End Sub



        Public Function ListaMatchAutomaticoTacs(idProveedorGlobal, listAutomatico)

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


        Public Function ListaMatchManualTacs(idProveedorGlobal, listAutomatico)

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



        Public Sub CN_ModificarFecha()



            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim dateIn As String
            Dim dateOut As String

            Dim dateInB As String
            Dim dateOutB As String

            Dim year As String
            Dim dd As String
            Dim mm As String



            tabla = objetoCapaDatos.CD_SelectTacs()

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty
                    dateIn = vbEmpty
                    dateOut = vbEmpty

                    Try

                        id = row("id").ToString()
                        dateIn = Trim(row("Arrival"))
                        dateOut = Trim(row("Departure"))

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


                Next

            End If

        End Sub

        Public Sub CN_ModificarFechaPagadas()



            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim dateIn As String
            Dim dateOut As String

            Dim dateInB As String
            Dim dateOutB As String

            Dim year As String
            Dim dd As String
            Dim mm As String



            tabla = objetoCapaDatos.CD_SelectTacsPagadas()

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty
                    dateIn = vbEmpty
                    dateOut = vbEmpty

                    Try

                        id = row("id").ToString()
                        dateIn = Trim(row("Arrival"))
                        dateOut = Trim(row("Departure"))

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


                Next

            End If

        End Sub

        Public Sub CN_ModificarFechaObservaciones()



            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim dateIn As String
            Dim dateOut As String

            Dim dateInB As String
            Dim dateOutB As String

            Dim year As String
            Dim dd As String
            Dim mm As String



            tabla = objetoCapaDatos.CD_SelectTacsObservaciones()

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty
                    dateIn = vbEmpty
                    dateOut = vbEmpty

                    Try

                        id = row("id").ToString()
                        dateIn = Trim(row("Arrival"))
                        dateOut = Trim(row("Departure"))

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


                Next

            End If

        End Sub

        Public Function CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            Return tabla

        End Function

        Public Function CN_SelectSinConciliar() As DataTable

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla = objetoCapaDatos.CD_SelectSinConciliar()

            Return tabla

        End Function

        Public Sub CN_EliminarTacs(tabla)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim id As String

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty

                    id = row("id").ToString()
                    objetoCapaDatos.CD_EliminarTacs(id)

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
