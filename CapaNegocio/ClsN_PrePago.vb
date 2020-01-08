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
    Public Class ClsN_PrePago


        Private objetoCapaDatos As ClsPrePago = New ClsPrePago()

        Public Function CN_ObtenerUltimoId()

            Return objetoCapaDatos.CD_ObtenerUltimoId()

        End Function

        Public Function CN_cargaDocPrePago(ruta As String, indexHoja As Int16)

            If ruta <> "" And indexHoja <> -1 Then

                Dim respuesta As Boolean = CN_cargaArchivoPrePago(ruta, indexHoja)

                If (respuesta) Then

                    'Poner fecha proveedor
                    CN_agregarMesProveedor(ClsNGlobales.LastID)

                    Return True

                Else
                    Return False

                End If

            Else
                MsgBox("Verifique los campos Archivo y Hoja")
            End If


        End Function


        Public Function CN_cargaArchivoPrePago(ruta, indexHoja)

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


                        If (CN_DatosPrePago()) Then


                            Dim res As Boolean = CN_InsertarPendientesGestionCommtrack(GestionCommtrack)
                            Return res

                        Else

                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoPrePago(GestionCommtrack)
                            Return res

                        End If

                        'Aplicar trim

                    Else
                        MsgBox("El Archivo Del Proveedor No Tiene Datos")
                    End If
                End Using
            End Using

        End Function

        Public Function CN_DatosPrePago()

            Return objetoCapaDatos.CD_DatosPrePago()

        End Function


        Public Function CN_InsertarPendientesGestionCommtrack(GestionCommtrack)



            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncatePrePagoTmp()
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesPrePagoTmp(GestionCommtrack)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesPrePago()

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


        Public Sub CN_agregarMesProveedor(lastId)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            objetoCapaDatos.CD_agregarMesProveedor(lastId)

        End Sub

        Public Function CN_SelectPrePago() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            tabla = objetoCapaDatos.CD_SelectPrePago()

            Return tabla

        End Function






    End Class
End Namespace

