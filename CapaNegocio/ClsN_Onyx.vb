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

    Public Class ClsN_Onyx


        Private objetoCapaDatos As ClsOnyx = New ClsOnyx()

        Public NombreConciliacionOnyx As String
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
            Dim BookingStatusCode As String
            Dim TipoConciliacion As String


            If (NombreConciliacionOnyx <> "") Then

                objetoCapaDatos.idProveedor = Me.idProveedor
                objetoCapaDatos.NombreConciliacionPosadas = Me.NombreConciliacionOnyx
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
                        BookingStatusCode = vbEmpty

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

                        Try
                            objetoCapaDatos.BookingStatusCode = row("BookingStatusCode").ToString()
                        Catch ex As Exception

                        End Try


                        objetoCapaDatos.TipoConciliacion = row("TipoConciliacion").ToString()


                        objetoCapaDatos.CD_GuardarConciliacionDetalle()

                    Next
                End If

            End If

        End Function

        Public Function CN_DatosOnyx()

            Return objetoCapaDatos.CD_DatosOnyx()

        End Function

        Public Function CN_cargaDocOnyx(ruta As String, indexHoja As Int16, tc As Double)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            If ruta <> "" And indexHoja <> -1 Then

                Dim respuesta As Boolean = CN_cargaArchivoOnyx(ruta, indexHoja)

                If (respuesta) Then

                    'Fecha Carga Pago Proveedor
                    CN_agregarMesProveedor(ClsNGlobales.LastID)

                    CN_addFirtsNameLastName()
                    objetoCapaDatos.CD_addtrxconcatenada()
                    objetoCapaDatos.CD_quitarGuion()

                    objetoCapaDatos.CD_InsertarOnyxPagadas()

                    objetoCapaDatos.CD_SelectOnyxPagadasConformationNORepetidos()
                    objetoCapaDatos.CD_addtrxconcatenadaPagadasv2()


                    objetoCapaDatos.CD_InsertarOnyxObservaciones()

                    objetoCapaDatos.CD_InsertarOnyxComisionesPendientePago()

                    Return True
                Else
                    Return False
                End If

            Else
                MsgBox("Verifique los campos Archivo y Hoja")
            End If


        End Function

        Public Function CN_cargaArchivoOnyx(ruta, indexHoja)

            Dim filas As Integer

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    filas = reader.RowCount

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim onyx As DataTable = result(indexHoja)

                    If onyx.Rows.Count > 0 Then

                        If (CN_DatosOnyx()) Then
                            Dim res As Boolean = CN_InsertarPendientesOnyx(onyx)
                            Return res
                        Else
                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoOnyx(onyx)
                            Return res
                        End If

                    Else
                        MsgBox("El Archivo Del Proveedor No Tiene Datos")
                    End If

                End Using
            End Using

        End Function


        Public Function CN_InsertarPendientesOnyx(onyx)

            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncateOnyxTmp()
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesOnyxTmp(onyx)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resG As Boolean = objetoCapaDatos.CD_RepetidosOnyxA()
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesOnyx()


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


        Public Sub CN_addFirtsNameLastName()

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            '' Limpiar guestNAME ''
            objetoCapaDatos.CD_replaceQuotes()

            ''

            Dim id As Integer
            Dim guestName As String
            Dim firstName As String
            Dim lastName As String

            Dim array As String()
            'firtsName, lastName
            'Return objetoCapaDatos.CD_addFirtsNameLastName()

            Dim query As String = ""


            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyxFechaProveedor()

            For Each row As DataRow In tabla.Rows

                id = vbEmpty
                guestName = vbEmpty

                id = row("id").ToString()
                guestName = row("GuestName").ToString()

                If (guestName <> "") Then

                    If (guestName.Contains("/")) Then

                        array = guestName.Split(New Char() {"/"c})

                        firstName = Trim(array(1)).ToString()
                        lastName = Trim(array(0)).ToString()

                        firstName = RemoveDiacritics(firstName)
                        lastName = RemoveDiacritics(lastName)




                    Else

                        firstName = guestName
                        lastName = guestName

                    End If



                    query &= "UPDATE onyx SET firstName = '" & firstName & "', lastName = '" & lastName & "' WHERE id = " & id & "; "



                End If
            Next

            objetoCapaDatos.CD_addFirtsNameLastName(query)


        End Sub

        Public Sub CN_addFirtsNameLastNameComisionesPendientePago()

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            '' Limpiar guestNAME ''
            objetoCapaDatos.CD_replaceQuotesComisionesPendientePago()
            ''

            Dim id As Integer
            Dim guestName As String
            Dim fisrtName As String
            Dim lastName As String

            Dim array As String()
            'firtsName, lastName
            'Return objetoCapaDatos.CD_addFirtsNameLastName()


            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyxComisionesPendientePago()

            For Each row As DataRow In tabla.Rows

                id = vbEmpty
                guestName = vbEmpty

                id = row("id").ToString()
                guestName = row("GuestName").ToString()

                If (guestName <> "") Then

                    If (guestName.Contains("/")) Then

                        array = guestName.Split(New Char() {"/"c})

                        fisrtName = Trim(array(1)).ToString()
                        lastName = Trim(array(0)).ToString()

                    Else

                        fisrtName = guestName
                        lastName = guestName

                    End If


                    objetoCapaDatos.CD_addFirtsNameLastNameComisionesPendientePago(id, fisrtName, lastName)




                End If
            Next

        End Sub

        Public Sub CN_addFirtsNameLastNamePagadas()

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            '' Limpiar guestNAME ''
            objetoCapaDatos.CD_replaceQuotesPagadas()
            ''

            Dim id As Integer
            Dim guestName As String
            Dim fisrtName As String
            Dim lastName As String

            Dim array As String()
            'firtsName, lastName
            'Return objetoCapaDatos.CD_addFirtsNameLastName()


            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyxPagadasFechaProveedor()

            For Each row As DataRow In tabla.Rows

                id = vbEmpty
                guestName = vbEmpty

                id = row("id").ToString()
                guestName = row("GuestName").ToString()

                If (guestName <> "") Then

                    If (guestName.Contains("/")) Then

                        array = guestName.Split(New Char() {"/"c})

                        fisrtName = Trim(array(1)).ToString()
                        lastName = Trim(array(0)).ToString()

                    Else

                        fisrtName = guestName
                        lastName = guestName

                    End If


                    objetoCapaDatos.CD_addFirtsNameLastNamePagadas(id, fisrtName, lastName)




                End If
            Next


        End Sub

        Public Sub CN_changeTC(tc)


            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            objetoCapaDatos.CD_changePaidCommission(tc)


        End Sub



        Public Sub CN_changeTCPagadas(tc)


            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            objetoCapaDatos.CD_changePaidCommissionPagadas(tc)



        End Sub



        Public Function CN_SelectOnyx() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyx()

            Return tabla

        End Function



        Public Function CN_SelectOnyxPagadas() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyxPagadas()

            Return tabla

        End Function

        Public Function CN_SelectOnyxObservaciones() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyxObservaciones()

            Return tabla

        End Function

        Public Function CN_SelectComisionesPendientePago() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectOnyxComisionesPendientePago()

            Return tabla

        End Function




        Public Function CN_agregarObservacion(id, observacion)

            Dim res As Boolean = objetoCapaDatos.CD_agregarObservacionTblObservaciones(id, observacion)

            Return res

        End Function



        Public Sub CN_quitarAcentos()

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()
            Dim id As String
            Dim firstName As String
            Dim lastName As String

            Dim firstNameB As String
            Dim lastNameB As String

            Dim queryA As String = ""
            Dim queryB As String = ""

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


                    'If (firstName <> Nothing) Then

                    firstNameB = RemoveDiacritics(firstName)
                    lastNameB = RemoveDiacritics(lastName)

                    'objetoCapaDatos.CD_QuitarAcentoFirstName(id, firstNameB)
                    queryA &= "UPDATE onyx SET firstName = '" & firstNameB & "', lastName = '" & lastNameB & "' WHERE id = " & id & "; "

                    'End If

                    'If (lastName <> Nothing) Then


                    'objetoCapaDatos.CD_QuitarAcentoLastName(id, lastNameB)

                    'queryB &= "UPDATE onyx SET lastName = '" & lastNameB & "' WHERE id = " & id & "; "

                    'End If




                Next

                'objetoCapaDatos.CD_QuitarAcentoLastName(queryB)
                objetoCapaDatos.CD_QuitarAcentoFirstName(queryA)

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

        Public Sub CN_EliminarOnyx(tabla)


            Dim id As String

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty

                    id = row("id").ToString()
                    objetoCapaDatos.CD_EliminarOnyx(id)

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

            'ClsGlobales.AnioProveedor = ClsNGlobales.AnioProveedor
            'ClsGlobales.MesProveedor = ClsNGlobales.MesProveedor

            objetoCapaDatos.CD_agregarMesProveedor(lastId)

        End Sub





    End Class

End Namespace
