Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data

Namespace CapaDatos

    Public Class ClsGestionCommtrack

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()

        Public NombreConciliacionPosadas As String
        Public idProveedor As Int32

        'Columnas Tabla ConciliacionDetalle
        Public idConciliacion As Integer
        Public dim_value As String
        Public FechaApp As String
        Public UserSpec As String
        Public Segmento As String
        Public CodigoConfirmacion As String
        Public Comision As String
        Public Operador As String
        Public Moneda As String
        Public CostoTotalDeLaReserva As String
        Public Noches As String
        Public ComOrig As String
        Public SequenceNo As String
        Public TipoConciliacion As String

        Public Function CD_GuardarConciliacion()

            Dim lastId As Integer

            Dim query As String = "INSERT INTO conciliacion(nombreConciliacion,idProveedor,fechaCreacion)
            VALUES('" & Me.NombreConciliacionPosadas & "'," & Me.idProveedor & ",GETDATE());SELECT SCOPE_IDENTITY()"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                lastId = comando.ExecuteScalar()
                Return lastId

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 151 Posadas")

                Return 0
            Finally

                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_GuardarConciliacionDetalle()

            Dim lastId As Integer

            Dim query As String = "INSERT INTO conciliacionDetalleGestionCommtrack(idConciliacion,dim_value,FechaApp,UserSpec,Segmento,
            CodigoConfirmacion,Comision,Operador,Moneda,CostoTotalDeLaReserva,Noches,ComOrig,SequenceNo,TipoConciliacion)
            VALUES(" & Me.idConciliacion & ",'" & Me.dim_value & "','" & FechaApp & "','" & UserSpec & "','" & Segmento & "',
            '" & CodigoConfirmacion & "','" & Comision & "','" & Operador & "','" & Moneda & "','" & CostoTotalDeLaReserva & "','" & Noches & "',
            '" & ComOrig & "', '" & SequenceNo & "' ,'" & TipoConciliacion & "');"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.ExecuteNonQuery()
                Return lastId

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 153 GestionCommtrack")

                Return 0

            Finally

                conexion.CerrarConexion()
            End Try

        End Function


        Public Function CD_DatosGestionCommtrack()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM gestionCommtrack"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                'comando.Parameters.Clear()
                countResult = Convert.ToInt32(comando.ExecuteScalar())


                If (countResult > 0) Then
                    'existen registros
                    Return True
                Else
                    'no existen regsitros
                    Return False
                End If

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 001 gestionCommtrack")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try

        End Function




        Public Function CD_cargaArchivoGestionCommtrack(ByVal GestionCommtrack As DataTable)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)


            Dim Usr_spec As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Usr spec")).FirstOrDefault().ColumnName
            Dim Trans_ As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Trans #")).FirstOrDefault().ColumnName
            Dim SuppID As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("SuppID")).FirstOrDefault().ColumnName
            Dim Supplier As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Supplier")).FirstOrDefault().ColumnName
            Dim DIN As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("DIN")).FirstOrDefault().ColumnName
            Dim OUT As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("OUT")).FirstOrDefault().ColumnName
            Dim PAID_AGY As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("PAID_AGY")).FirstOrDefault().ColumnName
            Dim Confirmation_code As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Confirmation code")).FirstOrDefault().ColumnName
            Dim Curr As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Curr")).FirstOrDefault().ColumnName
            Dim Rate As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Rate")).FirstOrDefault().ColumnName
            Dim First As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("First")).FirstOrDefault().ColumnName
            Dim IATA As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("IATA")).FirstOrDefault().ColumnName
            Dim Last As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Last")).FirstOrDefault().ColumnName
            Dim nitec As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("nitec")).FirstOrDefault().ColumnName
            Dim Phone As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Phone")).FirstOrDefault().ColumnName
            Dim PNR As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("PNR")).FirstOrDefault().ColumnName
            Dim Remark As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Remark")).FirstOrDefault().ColumnName
            Dim Address_1 As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Address 1")).FirstOrDefault().ColumnName
            Dim Address_2 As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Address 2")).FirstOrDefault().ColumnName
            Dim VenType As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("VenType")).FirstOrDefault().ColumnName
            Dim segnum As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("segnum")).FirstOrDefault().ColumnName
            Dim Observaciones As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Observaciones")).FirstOrDefault().ColumnName

            Using SqlBulkCopy
                'archivo->BD

                SqlBulkCopy.DestinationTableName = "dbo.gestionCommtrack"

                SqlBulkCopy.ColumnMappings.Add(Usr_spec, "Usrspec")
                SqlBulkCopy.ColumnMappings.Add(Trans_, "Trans")
                SqlBulkCopy.ColumnMappings.Add(SuppID, "SuppID")
                SqlBulkCopy.ColumnMappings.Add(Supplier, "Supplier")
                SqlBulkCopy.ColumnMappings.Add(DIN, "DIN")
                SqlBulkCopy.ColumnMappings.Add(OUT, "OUT")
                SqlBulkCopy.ColumnMappings.Add(PAID_AGY, "PAID_AGY")
                SqlBulkCopy.ColumnMappings.Add(Confirmation_code, "Confirmationcode")
                SqlBulkCopy.ColumnMappings.Add(Curr, "Curr")
                SqlBulkCopy.ColumnMappings.Add(Rate, "Rate")
                SqlBulkCopy.ColumnMappings.Add(First, "First")
                SqlBulkCopy.ColumnMappings.Add(IATA, "IATA")
                SqlBulkCopy.ColumnMappings.Add(Last, "Last")
                SqlBulkCopy.ColumnMappings.Add(nitec, "nitec")
                SqlBulkCopy.ColumnMappings.Add(Phone, "Phone")
                SqlBulkCopy.ColumnMappings.Add(PNR, "PNR")
                SqlBulkCopy.ColumnMappings.Add(Remark, "Remark")
                SqlBulkCopy.ColumnMappings.Add(Address_1, "Address1")
                SqlBulkCopy.ColumnMappings.Add(Address_2, "Address2")
                SqlBulkCopy.ColumnMappings.Add(VenType, "VenType")
                SqlBulkCopy.ColumnMappings.Add(segnum, "segnum")
                SqlBulkCopy.ColumnMappings.Add(Observaciones, "Observaciones")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(GestionCommtrack)
                    Return True
                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 005 GestionCommtrackTmp")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function

        Public Function CD_TruncateGestionCommtrackTmp()


            Dim query As String = "TRUNCATE TABLE gestionCommtrackTmp"

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

                MsgBox(ex.Message & " " & "ERROR 002 GestionCommtrack")
                Return False

            Finally

                conexion.CerrarConexion()
            End Try



        End Function

        Public Function CD_InsertarPendientesGestionCommtrackTmp(ByVal GestionCommtrack As DataTable)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Dim Usr_spec As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Usr spec")).FirstOrDefault().ColumnName
            Dim Trans_ As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Trans #")).FirstOrDefault().ColumnName
            Dim SuppID As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("SuppID")).FirstOrDefault().ColumnName
            Dim Supplier As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Supplier")).FirstOrDefault().ColumnName
            Dim DIN As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("DIN")).FirstOrDefault().ColumnName
            Dim OUT As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("OUT")).FirstOrDefault().ColumnName
            Dim PAID_AGY As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("PAID_AGY")).FirstOrDefault().ColumnName
            Dim Confirmation_code As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Confirmation code")).FirstOrDefault().ColumnName
            Dim Curr As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Curr")).FirstOrDefault().ColumnName
            Dim Rate As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Rate")).FirstOrDefault().ColumnName
            Dim First As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("First")).FirstOrDefault().ColumnName
            Dim IATA As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("IATA")).FirstOrDefault().ColumnName
            Dim Last As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Last")).FirstOrDefault().ColumnName
            Dim nitec As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("nitec")).FirstOrDefault().ColumnName
            Dim Phone As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Phone")).FirstOrDefault().ColumnName
            Dim PNR As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("PNR")).FirstOrDefault().ColumnName
            Dim Remark As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Remark")).FirstOrDefault().ColumnName
            Dim Address_1 As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Address 1")).FirstOrDefault().ColumnName
            Dim Address_2 As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Address 2")).FirstOrDefault().ColumnName
            Dim VenType As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("VenType")).FirstOrDefault().ColumnName
            Dim segnum As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("segnum")).FirstOrDefault().ColumnName
            Dim Observaciones As String = GestionCommtrack.Columns.Cast(Of DataColumn).Where(Function(x) x.ColumnName.Contains("Observaciones")).FirstOrDefault().ColumnName

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.gestionCommtrackTmp"

                SqlBulkCopy.ColumnMappings.Add(Usr_spec, "Usrspec")
                SqlBulkCopy.ColumnMappings.Add(Trans_, "Trans")
                SqlBulkCopy.ColumnMappings.Add(SuppID, "SuppID")
                SqlBulkCopy.ColumnMappings.Add(Supplier, "Supplier")
                SqlBulkCopy.ColumnMappings.Add(DIN, "DIN")
                SqlBulkCopy.ColumnMappings.Add(OUT, "OUT")
                SqlBulkCopy.ColumnMappings.Add(PAID_AGY, "PAID_AGY")
                SqlBulkCopy.ColumnMappings.Add(Confirmation_code, "Confirmationcode")
                SqlBulkCopy.ColumnMappings.Add(Curr, "Curr")
                SqlBulkCopy.ColumnMappings.Add(Rate, "Rate")
                SqlBulkCopy.ColumnMappings.Add(First, "First")
                SqlBulkCopy.ColumnMappings.Add(IATA, "IATA")
                SqlBulkCopy.ColumnMappings.Add(Last, "Last")
                SqlBulkCopy.ColumnMappings.Add(nitec, "nitec")
                SqlBulkCopy.ColumnMappings.Add(Phone, "Phone")
                SqlBulkCopy.ColumnMappings.Add(PNR, "PNR")
                SqlBulkCopy.ColumnMappings.Add(Remark, "Remark")
                SqlBulkCopy.ColumnMappings.Add(Address_1, "Address1")
                SqlBulkCopy.ColumnMappings.Add(Address_2, "Address2")
                SqlBulkCopy.ColumnMappings.Add(VenType, "VenType")
                SqlBulkCopy.ColumnMappings.Add(segnum, "segnum")
                SqlBulkCopy.ColumnMappings.Add(Observaciones, "Observaciones")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(GestionCommtrack)
                    Return True

                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 003 GestionCommtrack")
                    Return False

                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function

        Public Function CD_FaltantesGestionCommtrack()

            Dim procedure As String = "cargaGestionCommtrackFaltantes"

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

                MsgBox(ex.Message & " " & "ERROR 004 GestionCommtrack")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectGestionCommtrack() As DataTable

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT *, NUMERO_REPETIDOS=ROW_NUMBER() OVER(PARTITION BY Trans,ConfirmationCode ORDER BY id) 
            FROM gestionCommtrack)

            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 101 gestionCommtrack")

            Finally

                conexion.CerrarConexion()

            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM gestionCommtrack" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)

                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 gestionCommtrack")
                Return tablaPosadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectGestionCommtrackPagoFechaProveedor() As DataTable

            Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor

            Dim queryFechaProveedor As String = " WHERE mesProveedor ='" & fechaproveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM gestionCommtrack" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)

                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 gestionCommtrack")
                Return tablaPosadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectGestionCommtrackMontoReserva() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM gestionCommtrack" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)

                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 gestionCommtrack")
                Return tablaPosadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function


        Public Sub CD_CamposTrim()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            Dim query As String = "UPDATE gestionCommtrack SET [First] =  ltrim(RTRIM([First])) " & queryFechaProveedor & queryEstatusEliminado & ";
            UPDATE gestionCommtrack SET Supplier =  ltrim(RTRIM([Supplier])) " & queryFechaProveedor & queryEstatusEliminado & ";
            UPDATE gestionCommtrack SET [Last] =  ltrim(RTRIM([Last])) " & queryFechaProveedor & queryEstatusEliminado & " ;"
            'Dim query2 As String = "update gestionCommtrack set Supplier =  ltrim(RTRIM(Supplier));"
            'Dim query3 As String = "update gestionCommtrack set Supplier =  ltrim(RTRIM(Last));"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()



            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0453 GestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try




        End Sub

        Public Sub CD_AddNotrxconcatenada()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "UPDATE gestionCommtrack SET [No.trxconcatenada] = CONCAT(Trans,segnum)" & queryFechaProveedor & queryEstatusEliminado


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()



            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0458 GestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_AddMontototaldelareserva(id, total)

            Dim query As String = "UPDATE gestionCommtrack  SET  Montototaldelareserva = " & total & " WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()



            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0458 GestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_UpdateDATEIN(id, datein)

            Dim query As String = "UPDATE gestionCommtrack  SET  DIN = '" & datein & "' WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()



            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0043 GestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_EliminarErroneo()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "delete from gestionCommtrack where Usrspec = '----------' " & queryFechaProveedor & queryEstatusEliminado & ";
delete from gestionCommtrack where Usrspec = '' and Trans = '' " & queryFechaProveedor & queryEstatusEliminado & " ;"


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()



            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0049 GestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub


        Public Sub CD_UpdateDATEOUT(id, dateout)

            Dim query As String = "UPDATE gestionCommtrack  SET  OUT = '" & dateout & "' WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0044 GestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub




        Public Function CD_SelectSinConciliar() As DataTable

            Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor

            Dim queryFechaProveedor As String = " AND mesProveedor ='" & fechaproveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM gestionCommtrack WHERE CondicionOKAuto IS  NULL AND estatusConciliado IS  NULL AND idBDBCD IS  NULL" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)
                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 100 gestionCommtrack")
                Return tablaPosadas

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Sub CD_EliminarGestionCommtrack(id)

            'Dim query As String = "DELETE FROM gestionCommtrack WHERE id = " & id & ""
            Dim query As String = "UPDATE  gestionCommtrack SET estatusEliminado=1 WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 101 gestionCommtrack")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Function CD_ConciliarByID(id, idBDBCD, lastQuery)

            Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE gestionCommtrack SET estatusConciliado = 1 WHERE id  = " & id & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1265 gestionCommtrack")
            Finally
                conexion.CerrarConexion()
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryB As String = "UPDATE BDBCD SET 
            estatusConciliado = 1,
            proveedor = 'gestionCommtrack',
            mesProveedor = '" & fechaproveedor & "'
            WHERE id  = " & idBDBCD & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1266 gestionCommtrack")
            Finally
                conexion.CerrarConexion()
            End Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim tabla As DataTable = New DataTable()

            Dim queryC As String = "SELECT
            BD.id AS idBDBCD,
            proveedor.id AS idProveedor,
            BD.UniqueBookingID as dim_value,
            '" & fechaproveedor & "' AS FechaApp,
            BD.HotelPropertyID AS UserSpec,
            BD.[LineNo] AS Segmento,
            BD.ConformationNo AS CodigoConfirmacion,
            CASE WHEN proveedor.Observaciones <> '' OR proveedor.Observaciones IS NULL  
THEN proveedor.observaciones ELSE '' END AS Comision,
            'gestionCommtrack' AS Operador,
            proveedor.Curr AS Moneda,
            Montototaldelareserva AS CostoTotalDeLaReserva,
            proveedor.nitec AS Noches,
            proveedor.PAID_AGY As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Adicional' As tipoConciliacion
            FROM  gestionCommtrack proveedor 
            INNER JOIN BDBCD BD on proveedor." & lastQuery & " = BD.id
            AND proveedor.id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryC
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)

                Return tabla
            Catch ex As Exception
                MsgBox(ex.Message & " " & "ERROR 1267 Posadas")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Function CD_ObtenerUltimoId()

            Dim lastId As Int64

            Dim queryA As String = "SELECT MAX(id) FROM gestionCommtrack"

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
                MsgBox(ex.Message & " " & " ERROR 1014 gestionCommtrack")
            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Sub CD_agregarMesProveedor(ByVal lastId As Int64)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE gestionCommtrack Set mesProveedor = '" & fechaProveedor & "' WHERE id > " & lastId & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1013 gestionCommtrack")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub

        Public Function CD_SeleccionIDPendientesGestionCommtrack()

            Dim tabla As DataTable = New DataTable()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = " SELECT id FROM gestionCommtrack
	        WHERE mesProveedor = '" & fechaProveedor & "'
	        AND estatusConciliado IS NULL
	        AND estatusEliminado IS NULL "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)

                Return tabla

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR AAA gestionCommtrack")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function


    End Class

End Namespace
