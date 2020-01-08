Imports System.Data.SqlClient


Namespace CapaDatos

    Public Class ClsPosadas

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()
        'Private sqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

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

        'Datos extra
        Public mesProveedor As String
        Public anioProveedor As String





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

            Dim query As String = "INSERT INTO conciliacionDetallePosadas(idConciliacion,dim_value,FechaApp,UserSpec,Segmento,
            CodigoConfirmacion,Comision,Operador,Moneda,CostoTotalDeLaReserva,Noches,ComOrig,SequenceNo,TipoConciliacion)
            VALUES(" & Me.idConciliacion & ",'" & Me.dim_value & "','" & FechaApp & "','" & UserSpec & "','" & Segmento & "',
            '" & CodigoConfirmacion & "','" & Comision & "','" & Operador & "','" & Moneda & "','" & CostoTotalDeLaReserva & "','" & Noches & "',
            '" & ComOrig & "','" & SequenceNo & "','" & TipoConciliacion & "');"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.ExecuteNonQuery()
                Return lastId

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 152 Posadas")

                Return 0

            Finally

                conexion.CerrarConexion()
            End Try

        End Function




        Public Function CD_DatosPosadas()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM posadas"

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

                MsgBox(ex.Message & " " & "ERROR 001 Posadas")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try



        End Function




        Public Function CD_TruncatePosadasTmp()

            Dim query As String = "TRUNCATE TABLE posadasTmp"

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

                MsgBox(ex.Message & " " & "ERROR 002 Posadas")
                Return False

            Finally

                conexion.CerrarConexion()
            End Try

        End Function


        Public Function CD_InsertarPendientesPosadasTmp(posadas)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using sqlBulkCopy

                sqlBulkCopy.DestinationTableName = "dbo.posadasTmp"

                sqlBulkCopy.ColumnMappings.Add("HOTEL", "hotel")
                sqlBulkCopy.ColumnMappings.Add("IATA", "iata")
                sqlBulkCopy.ColumnMappings.Add("CLAVE", "clave")
                sqlBulkCopy.ColumnMappings.Add("CLAVE GDS", "claveGDS")
                sqlBulkCopy.ColumnMappings.Add("HUESPED", "huesped")
                sqlBulkCopy.ColumnMappings.Add("LLEGADA", "llegada")
                sqlBulkCopy.ColumnMappings.Add("SALIDA", "salida")
                sqlBulkCopy.ColumnMappings.Add("COMISION", "comision")
                sqlBulkCopy.ColumnMappings.Add("MONEDA", "moneda")
                SqlBulkCopy.ColumnMappings.Add("% comision", "percentComision")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(posadas)
                    Return True

                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 003 Posadas")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function

        Public Function CD_FaltantesPosadas()

            Dim procedure As String = "cargaPosadasFaltantes"

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

                MsgBox(ex.Message & " " & "ERROR 004 Posadas")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try


        End Function


        Public Function CD_cargaArchivoPosadas(posadas)

            posadas.Columns.Add("mesProveedor")


            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.posadas"

                SqlBulkCopy.ColumnMappings.Add("HOTEL", "hotel")
                SqlBulkCopy.ColumnMappings.Add("IATA", "iata")
                SqlBulkCopy.ColumnMappings.Add("CLAVE", "clave")
                SqlBulkCopy.ColumnMappings.Add("CLAVE GDS", "claveGDS")
                SqlBulkCopy.ColumnMappings.Add("HUESPED", "huesped")
                SqlBulkCopy.ColumnMappings.Add("LLEGADA", "llegada")
                SqlBulkCopy.ColumnMappings.Add("SALIDA", "salida")
                SqlBulkCopy.ColumnMappings.Add("COMISION", "comision")
                SqlBulkCopy.ColumnMappings.Add("MONEDA", "moneda")
                SqlBulkCopy.ColumnMappings.Add("% comision", "percentComision")

                '''''''  MES DE PROVEEDOR  '''''''





                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(posadas)
                    Return True
                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 005 Posadas")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function


        Public Function CD_addFirtsNameLastName()

            'Dim procedure As String = "addFirstAndLastNamePosadas"
            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            Dim query As String = "	UPDATE posadas SET
            firstName =  SUBSTRING(huesped,1,CHARINDEX(' ',huesped)-1),
            lastName =  SUBSTRING(huesped,CHARINDEX(' ',huesped)+1,LEN(huesped))  " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
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

                MsgBox(ex.Message & " " & "ERROR 006 Posadas")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try



        End Function

        Public Function CD_addTotalReserva()

            'Dim procedure As String = "totalDeLaReservaPosadas"
            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "UPDATE posadas 
SET totalDeLaReserva = ((CAST(comision as DECIMAL(18,3)) * 100) / (CAST(percentComision as DECIMAL(18,3)))) " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
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

                MsgBox(ex.Message & " " & "ERROR 007 Posadas")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try



        End Function

        Public Sub CD_EliminarPosadas(id)

            'Dim query As String = "DELETE FROM posadas WHERE id = " & id & ""

            Dim query As String = "UPDATE posadas SET estatusEliminado = 1 WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 008 Posadas")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Function CD_addNoNoches()

            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "	UPDATE posadas SET noNoches = DATEDIFF(day, llegada, salida) " & queryFechaProveedor & queryEstatusEliminado

            'Dim procedure As String = "noNochesPosadas"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
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

                MsgBox(ex.Message & " " & "ERROR 008 Posadas")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Function CD_ConciliarByID(id, idBDBCD, lastQuery)

            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE posadas SET estatusConciliado = 1 WHERE id  = " & id & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1265 Posadas")
            Finally
                conexion.CerrarConexion()
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryB As String = "UPDATE BDBCD SET 
            estatusConciliado = 1,
            proveedor = 'posadas',
            mesProveedor = '" & fechaProveedor & "'
            WHERE id  = " & idBDBCD & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1266 Posadas")
            Finally
                conexion.CerrarConexion()
            End Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim tabla As DataTable = New DataTable()

            Dim queryC As String = "SELECT
            BD.id AS idBDBCD,
            proveedor.id AS idProveedor,
            BD.UniqueBookingID as dim_value,
            '" & fechaProveedor & "' AS FechaApp,
            BD.HotelPropertyID AS UserSpec,
            BD.[LineNo] AS Segmento,
            BD.conformationNo AS CodigoConfirmacion,
            proveedor.comision AS Comision,
            'Posadas' AS Operador,
            proveedor.moneda AS Moneda,
            totalDeLaReserva AS CostoTotalDeLaReserva,
            noNoches AS Noches,
            proveedor.comision As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Adicional' As tipoConciliacion
            FROM  posadas proveedor 
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

        Public Sub CD_CandenaConciliados(cadenaCumplidas)

            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            Dim query As String = "UPDATE posadas SET CondicionOKAuto = '" & cadenaCumplidas & "' WHERE estatusConciliado = 1" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text

                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 098 Posadas")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub



        Public Function CD_SeleccionIDPendientes()

            Dim tabla As DataTable = New DataTable()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = " SELECT id FROM posadas
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
                MsgBox(ex.Message & " " & " ERROR AAA Posadas")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function



        Public Function CD_SelectPosadas() As DataTable


            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

            'Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"


            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT *, NUMERO_REPETIDOS=ROW_NUMBER() OVER(PARTITION BY iata,clave ORDER BY id) 
             FROM posadas)
            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 101 POSADAS")

            Finally

                conexion.CerrarConexion()

            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM posadas " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)

                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 Posadas")
                Return tablaPosadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectPosadasFechaProveedor() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM posadas " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)

                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 1020 Posadas")
                Return tablaPosadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function



        Public Function CD_SelectSinConciliar() As DataTable

            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM posadas WHERE CondicionOKAuto IS  NULL AND estatusConciliado IS  NULL AND idBDBCD IS  NULL" & queryFechaProveedor

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)
                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0754 Posadas")
                Return tablaPosadas

            Finally
                conexion.CerrarConexion()
            End Try


        End Function




        Public Function CD_consultaConciliado()

            Dim query As String = "SELECT TOP 1 CondicionOkAuto FROM posadas WHERE estatusConciliado = 1"
            Dim cadenaConciliado As String

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()

                While leer.Read

                    cadenaConciliado = leer.GetString(0)

                End While

                Return cadenaConciliado

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 123 Posadas")


            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Function CD_ConsultaAcentos()

            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"




            Dim tabla As DataTable = New DataTable()

            Dim query As String = "SELECT id, firstName, lastName FROM posadas " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)

                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 123 Posadas")
                Return tabla

            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Sub CD_QuitarAcentoFirstName(id, firstName)

            Dim query As String = "UPDATE posadas SET firstName = '" & firstName & "' WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0086 Posadas")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_QuitarAcentoLastName(id, lastNameB)
            Dim query As String = "UPDATE posadas SET lastName = '" & lastNameB & "' WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0087 Posadas")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Function CD_UpdateComision(id, percentComision)

            Dim query As String = "UPDATE posadas SET percentComision = '" & percentComision & "' WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

                If (res) Then
                    'existen registros
                    Return True
                Else
                    'no existen regsitros
                    Return False
                End If

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0066 Posadas")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectPosadasComision() As DataTable

            'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "SELECT id,percentComision FROM posadas " & queryFechaProveedor & queryEstatusEliminado

            Dim TblComisionMenor As DataTable = New DataTable()

            'Dim query As String = "UPDATE posadas SET percentComision = (CAST(percentComision AS DECIMAL(18,3)) * 100)"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                TblComisionMenor.Load(leer)

                Return TblComisionMenor

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0029 Posadas")
                Return TblComisionMenor

            Finally

                conexion.CerrarConexion()

            End Try

        End Function


        Public Sub CD_DesconciliarPosadas(id, idBDBCD)

            Dim query As String = "UPDATE posadas SET estatusConciliado = NULL WHERE id  = " & id & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1005 Posadas")
            Finally
                conexion.CerrarConexion()
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryB As String = "UPDATE BDBCD SET estatusConciliado = NULL, proveedor = NULL WHERE id  = " & idBDBCD & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1006 Posadas")
            Finally
                conexion.CerrarConexion()
            End Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        End Sub

        Public Sub CD_agregarMesProveedor(ByVal lastId As Int64)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE posadas Set mesProveedor = '" & fechaProveedor & "' WHERE id > " & lastId & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1013 Posadas")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub

        Public Function CD_ObtenerUltimoId()

            Dim lastId As Int64

            Dim queryA As String = "SELECT MAX(id) FROM posadas"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryA
                comando.CommandType = CommandType.Text

                leer = comando.ExecuteReader()
                While leer.Read()

                    'If (leer(0) IsNot Nothing) Then
                    '    lastId = Convert.ToInt64(leer(0))
                    'Else
                    '    lastId = 0
                    'End If
                    lastId = Convert.ToInt64(If(TypeOf leer(0) Is DBNull, 0, leer(0)))


                    'If (leer(0) <> DBNull.Value) Then
                    '    lastId = Convert.ToInt64(leer(0))
                    'Else
                    '    lastId = 0
                    'End If



                End While



                Return lastId

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1014 Posadas")
            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Sub CD_ActualizacionB()
            Dim queryC As String = "delete from posadas where  clave is null and claveGDS is null"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryC
                comando.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR Actualizacion Posadas")
            End Try
        End Sub

        Public Sub CD_Actualizacion()

            Dim resA As Boolean = False

            Dim query As String = "SELECT idProveedor FROM columnasExcel WHERE idProveedor = 1004"

            Dim tablaA As DataTable = New DataTable()

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                leer = comando.ExecuteReader()
                tablaA.Load(leer)


                If (tablaA.Rows.Count > 0) Then
                    Exit Sub
                Else

                    Dim queryB As String = "Insert Into columnasExcel(idProveedor) values (1004)"
                    Try
                        comando.Connection = conexion.AbrirConexion()
                        comando.CommandText = queryB
                        resA = comando.ExecuteNonQuery()

                        If resA Then

                            'Dim queryC As String = "ALTER TABLE conciliacionDetalleOnyx ADD BookingStatusCode VARCHAR(50) NULL"
                            Dim queryC As String = "delete from posadas where  clave is null and claveGDS is null"

                            Try
                                comando.Connection = conexion.AbrirConexion()
                                comando.CommandText = queryC
                                comando.ExecuteNonQuery()
                            Catch ex As Exception
                                MsgBox(ex.Message & " " & " ERROR ActualizacionC")
                            End Try

                        End If


                    Catch ex As Exception
                        MsgBox(ex.Message & " " & " ERROR ActualizacionB")
                    Finally
                        conexion.CerrarConexion()
                    End Try

                End If


            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR ActualizacionA")
            Finally
                conexion.CerrarConexion()
            End Try





        End Sub


    End Class

End Namespace
