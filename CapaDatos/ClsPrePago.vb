Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data


Namespace CapaDatos
    Public Class ClsPrePago

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()

        Public Function CD_ObtenerUltimoId()

            Dim lastId As Int64

            Dim queryA As String = "SELECT MAX(id) FROM prePago"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryA
                comando.CommandType = CommandType.Text

                leer = comando.ExecuteReader()
                While leer.Read()


                    lastId = Convert.ToInt64(If(TypeOf leer(0) Is DBNull, 0, leer(0)))

                End While

                Return lastId

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 20001 prePago")
            Finally
                conexion.CerrarConexion()
            End Try

        End Function


        Public Function CD_DatosPrePago()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM prePago"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                countResult = Convert.ToInt32(comando.ExecuteScalar())

                If (countResult > 0) Then
                    'existen registros
                    Return True
                Else
                    'no existen regsitros
                    Return False
                End If

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 20002 prePago")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_TruncatePrePagoTmp()


            Dim query As String = "TRUNCATE TABLE prePagoTmp"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                'comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


                If (res) Then
                    'existen registros
                    Return True
                Else
                    'no existen regsitros
                    Return False
                End If

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 20003 prePago")
                Return False

            Finally

                conexion.CerrarConexion()
            End Try



        End Function

        Public Function CD_InsertarPendientesPrePagoTmp(posadas)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.prePagoTmp"

                SqlBulkCopy.ColumnMappings.Add("Núm Transacción", "numTransaccion")
                SqlBulkCopy.ColumnMappings.Add("Fecha Appl", "fechaAppl")
                SqlBulkCopy.ColumnMappings.Add("Usr Spec", "usrSpec")
                SqlBulkCopy.ColumnMappings.Add("No Segmento", "noSegmento")
                SqlBulkCopy.ColumnMappings.Add("Confirmation Code", "confirmationCode")
                SqlBulkCopy.ColumnMappings.Add("Comision Aplicar", "comisionAplicar")
                SqlBulkCopy.ColumnMappings.Add("Operador", "operador")
                SqlBulkCopy.ColumnMappings.Add("Moneda", "moneda")
                SqlBulkCopy.ColumnMappings.Add("Costo total de la reserva", "costoTotaldeLaReserva")
                SqlBulkCopy.ColumnMappings.Add("# Noches", "noNoches")
                SqlBulkCopy.ColumnMappings.Add("Comision Original", "comisionOriginal")
                SqlBulkCopy.ColumnMappings.Add("Cupon", "cupon")
                SqlBulkCopy.ColumnMappings.Add("Fecha de pago", "fechadePago")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(posadas)
                    Return True

                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 20004 prePago")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function

        Public Function CD_FaltantesPrePago()

            Dim procedure As String = "cargaprePagoFaltantes"
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = procedure
                comando.CommandType = CommandType.StoredProcedure
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

                If (res) Then
                    'existen registros
                    Return True
                Else
                    'no existen regsitros
                    Return False
                End If

            Catch ex As Exception


                MsgBox(ex.Message & " " & "ERROR 20005 prePago")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_cargaArchivoPrePago(posadas)

            posadas.Columns.Add("mesProveedor")


            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.prePago"

                SqlBulkCopy.ColumnMappings.Add("Núm Transacción", "numTransaccion")
                SqlBulkCopy.ColumnMappings.Add("Fecha Appl", "fechaAppl")
                SqlBulkCopy.ColumnMappings.Add("Usr Spec", "usrSpec")
                SqlBulkCopy.ColumnMappings.Add("No Segmento", "noSegmento")
                SqlBulkCopy.ColumnMappings.Add("Confirmation Code", "confirmationCode")
                SqlBulkCopy.ColumnMappings.Add("Comision Aplicar", "comisionAplicar")
                SqlBulkCopy.ColumnMappings.Add("Operador", "operador")
                SqlBulkCopy.ColumnMappings.Add("Moneda", "moneda")
                SqlBulkCopy.ColumnMappings.Add("Costo total de la reserva", "costoTotaldeLaReserva")
                SqlBulkCopy.ColumnMappings.Add("# Noches", "noNoches")
                SqlBulkCopy.ColumnMappings.Add("Comision Original", "comisionOriginal")
                SqlBulkCopy.ColumnMappings.Add("Cupon", "cupon")
                SqlBulkCopy.ColumnMappings.Add("Fecha de pago", "fechadePago")

                '''''''  MES DE PROVEEDOR  '''''''

                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(posadas)
                    Return True
                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 20006 prePago")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function

        Public Sub CD_agregarMesProveedor(ByVal lastId As Int64)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE prePago Set mesProveedor = '" & fechaProveedor & "' WHERE id > " & lastId & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & "ERROR 20007 prePago")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub


        Public Function CD_SelectPrePago() As DataTable

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT *, NUMERO_REPETIDOS=ROW_NUMBER() OVER(PARTITION BY numTransaccion,comisionAplicar ORDER BY id) 
            FROM prePago)
            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 20008 prePago")

            Finally

                conexion.CerrarConexion()

            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM prePago" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)

                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 20009 prePago")
                Return tablaPosadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function


    End Class
End Namespace



