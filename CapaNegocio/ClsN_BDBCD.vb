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

    Public Class ClsN_BDBCD

        Private objetoCapaDatos As ClsBDBCD = New ClsBDBCD()

        Public Function CN_DatosBDBCD()

            Return objetoCapaDatos.CD_DatosBDBCD()

        End Function

        Public Function CN_cargaArchivoBDBCD(ruta, indexHoja)

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim DBBCD As DataTable = result(indexHoja)

                    If DBBCD.Rows.Count > 0 Then


                        If (CN_DatosBDBCD()) Then


                            Dim res As Boolean = CN_InsertarPendientes(DBBCD)
                            Return res

                        Else

                            Dim res As Boolean = objetoCapaDatos.CD_cargaArchivoBDBCD(DBBCD)
                            Return res

                        End If



                    Else
                        MsgBox("El Archivo BD BCD No Tiene Datos")
                    End If
                End Using
            End Using

        End Function

        Public Sub CN_cargaArchivoBDBCDEliminados(ruta, indexHoja)

            Dim tabla As DataTable = New DataTable()

            Dim UniqueBookingID As String
            Dim LineNo As String
            Dim query As String = ""
            Dim totalTabla As Integer
            Dim contador As Integer = 0


            'Columna Extra
            Dim Eliminar As String

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim DBBCDEliminar As DataTable = result(indexHoja)

                    If DBBCDEliminar.Columns.Contains("Eliminar") Then

                        If DBBCDEliminar.Rows.Count > 0 Then

                            totalTabla = DBBCDEliminar.Rows.Count


                            For Each row As DataRow In DBBCDEliminar.Rows

                                UniqueBookingID = vbEmpty
                                LineNo = vbEmpty
                                Eliminar = vbEmpty


                                Eliminar = row("Eliminar")

                                If Eliminar = "Cancelado" Then

                                    UniqueBookingID = Trim(row("UniqueBookingID"))
                                    LineNo = Trim(row("LineNo"))


                                    'objetoCapaDatos.CD_SeleccionBDBCDEliminar(UniqueBookingID, LineNo)

                                    If contador + 1 = totalTabla Then
                                        query &= "SELECT * FROM BDBCD WHERE UniqueBookingID = " & UniqueBookingID & " AND [LineNo] =  " & [LineNo] & " " & vbLf & " "
                                    Else
                                        query &= "SELECT * FROM BDBCD WHERE UniqueBookingID = " & UniqueBookingID & " AND [LineNo] =  " & [LineNo] & " UNION " & vbLf & " "
                                    End If

                                End If

                                contador = contador + 1

                            Next

                            objetoCapaDatos.CD_ProcesoCanceladosBDBCD(query)



                        Else
                            MsgBox("El Archivo BD BCD No Tiene Datos")
                        End If
                    Else

                        MsgBox("El archivo no contiene la columna eliminar")

                    End If




                End Using
            End Using

        End Sub

        Public Sub CN_cargaArchivoBDBCDAactualizacionSegmento(ruta, indexHoja)

            Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)

                Using reader = ExcelReaderFactory.CreateReader(stream)

                    Dim watch = System.Diagnostics.Stopwatch.StartNew()
                    Dim result = reader.AsDataSet(New ExcelDataSetConfiguration() With {
                            .ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {
                                .UseHeaderRow = True
                            }
                        }).Tables


                    Dim DBBCD As DataTable = result(indexHoja)

                    If DBBCD.Rows.Count > 0 Then


                        If (objetoCapaDatos.CD_cargaArchivoBDBCDSegmento(DBBCD)) Then

                            objetoCapaDatos.CD_actualizaSegmento()

                        End If



                    Else
                        MsgBox("El Archivo BD BCD No Tiene Datos")
                    End If
                End Using
            End Using

        End Sub


        Public Function CN_InsertarPendientes(BDBCD)

            'Truncar tabla BDBCDTMP
            Dim res As Boolean = objetoCapaDatos.CD_TruncateBDBCDTMP
            If (res) Then

                Dim resI As Boolean = objetoCapaDatos.CD_InsertarPendientesBDBCDTmp(BDBCD)

                If (resI) Then

                    'Agregar faltantes de tabla agressoTMP a agresso
                    Dim resF As Boolean = objetoCapaDatos.CD_FaltantesBDBCD()

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


        Public Function CN_addColumnasBDBCD()


            Dim res As String = objetoCapaDatos.CD_CN_addColumnasBDBCD()
            Return res


        End Function


        Public Function CN_SelectBDBCD() As DataTable

            Dim tabla As DataTable = New DataTable()
            tabla.Rows.Clear()

            tabla = objetoCapaDatos.CD_SelectBDBCD()
            'tabla.Columns.Remove("id")
            Return tabla

        End Function

        Public Function CN_ConsultaEliminadosBDBCD()

            Dim tabla As DataTable = New DataTable()
            tabla.Rows.Clear()

            tabla = objetoCapaDatos.CD_ConsultaEliminadosBDBCD()
            'tabla.Columns.Remove("id")
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

        Public Sub CN_quitarGuion()

            objetoCapaDatos.CD_quitarGuion()

        End Sub


    End Class

End Namespace
