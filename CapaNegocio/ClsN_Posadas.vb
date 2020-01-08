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

    Public Class ClsN_Posadas

        Private objetoCapaDatos As ClsPosadas = New ClsPosadas()

        Public NombreConciliacionPosadas As String
        Public TablaConciliacion As New DataTable
        Public idProveedor As Int32

        Public mesProveedor As String
        Public anioProveedor As String

        Public lastId As Int64

        Public Function CN_DatosPosadas()

            Return objetoCapaDatos.CD_DatosPosadas()


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


            If (NombreConciliacionPosadas <> "") Then

                objetoCapaDatos.idProveedor = Me.idProveedor
                objetoCapaDatos.NombreConciliacionPosadas = Me.NombreConciliacionPosadas
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

        Public Function CN_cargaDocPosadas(ruta As String, indexHoja As Int16)

            'Instancia Fecha Proveedor
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If ruta <> "" And indexHoja <> -1 Then

                Dim respuesta As Boolean = CN_cargaArchivoPosadas(ruta, indexHoja)


                If (respuesta) Then

                    'Poner fecha proveedor
                    CN_agregarMesProveedor(ClsNGlobales.LastID)


                    Dim res1 As Boolean = CN_addFirtsNameLastName()
                    objetoCapaDatos.CD_ActualizacionB()
                    CN_updateComision()
                    Dim res2 As Boolean = CN_addTotalReserva()
                    Dim res3 As Boolean = CN_addNoNoches()


                    CN_quitarAcentos()






                    If (res1 And res2 And res3) Then
                        Return True
                    Else
                        Return False
                    End If

                Else

                    Return False

                End If

            Else
                MsgBox("Verifique los campos Archivo y Hoja")
            End If


        End Function

        Public Sub CN_updateComision()
            'Mes y año del proveedor

            'objetoCapaDatos.anioProveedor = Me.anioProveedor
            'objetoCapaDatos.mesProveedor = Me.mesProveedor
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor


            Dim id As Integer
            Dim percentComision As Decimal


            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectPosadasComision()

            For Each row As DataRow In tabla.Rows

                id = vbEmpty
                percentComision = vbEmpty

                id = row("id").ToString()
                percentComision = row("percentComision").ToString()

                If (percentComision <> Nothing) Then

                    If (percentComision <= 1) Then

                        percentComision = percentComision * 100

                    ElseIf (percentComision >= 100) Then

                        percentComision = percentComision / 100

                    End If
                    objetoCapaDatos.CD_UpdateComision(id, percentComision)


                End If
            Next

        End Sub

        Public Function CN_cargaArchivoPosadas(ruta, indexHoja)

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim posadas As DataTable = result(indexHoja)

                    If posadas.Rows.Count > 0 Then


                        If (CN_DatosPosadas()) Then


                            Dim res As Boolean = CN_InsertarPendientesPosadas(posadas)
                            Return res

                        Else

                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoPosadas(posadas)
                            Return res

                        End If



                    Else
                        MsgBox("El Archivo Del Proveedor No Tiene Datos")
                    End If
                End Using
            End Using

        End Function


        Public Function CN_InsertarPendientesPosadas(posadas)

            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncatePosadasTmp()
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesPosadasTmp(posadas)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesPosadas()

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




        Public Function CN_addFirtsNameLastName()




            Return objetoCapaDatos.CD_addFirtsNameLastName()


        End Function

        Public Function CN_addTotalReserva()

            'totalDeLaReserva
            Return objetoCapaDatos.CD_addTotalReserva()

        End Function

        Public Function CN_addNoNoches()

            'totalDeLaReserva
            Return objetoCapaDatos.CD_addNoNoches()

        End Function

        Public Function CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_ConciliarByID(idProveedor, idBDBCD, lastQuery)

            Return tabla

        End Function

        Public Sub CN_DesconciliarPosadas(idProveedor, idBDBCD)

            Dim tabla As DataTable = New DataTable()
            objetoCapaDatos.CD_DesconciliarPosadas(idProveedor, idBDBCD)

        End Sub



        Public Function CN_SelectPosadas() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectPosadas()

            Return tabla

        End Function

        Public Function CN_SelectSinConciliar() As DataTable
            'instanciar fecha proveedor

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor




            Dim tabla = objetoCapaDatos.CD_SelectSinConciliar()

            Return tabla

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

        Public Sub CN_EliminarPosadas(tabla)

            Dim id As String

            If (tabla.Rows.Count > 0) Then

                For Each row As DataRow In tabla.Rows

                    id = vbEmpty

                    id = row("id").ToString()
                    objetoCapaDatos.CD_EliminarPosadas(id)

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

        Public Sub CN_agregarMesProveedor(lastId)

            objetoCapaDatos.CD_agregarMesProveedor(lastId)

        End Sub

        Public Function CN_ObtenerUltimoId()

            Return objetoCapaDatos.CD_ObtenerUltimoId()

        End Function

        Public Sub CN_Actualizacion()

            objetoCapaDatos.CD_Actualizacion()

        End Sub

    End Class

End Namespace
