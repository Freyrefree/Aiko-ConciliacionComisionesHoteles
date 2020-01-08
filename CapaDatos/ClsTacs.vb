Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data


Namespace CapaDatos


    Public Class ClsTacs

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()
        'Private sqlBulkCopy As New SqlBulkCopy(conexion.Conexion)


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public NombreConciliacionTacs As String
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
            VALUES('" & Me.NombreConciliacionTacs & "'," & Me.idProveedor & ",GETDATE());SELECT SCOPE_IDENTITY()"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                lastId = comando.ExecuteScalar()
                Return lastId

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 158 Tacs")

                Return 0
            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_GuardarConciliacionDetalle()

            Dim lastId As Integer

            Dim query As String = "INSERT INTO conciliacionDetalleTacs(idConciliacion,dim_value,FechaApp,UserSpec,Segmento,
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

                MsgBox(ex.Message & " " & " ERROR 152 Tacs")

                Return 0

            Finally

                conexion.CerrarConexion()
            End Try

        End Function


        Public Function CD_DatosTacs()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM Tacs"

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

                MsgBox(ex.Message & " " & "ERROR 001 Tacs")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try



        End Function

        Public Sub CD_CandenaConciliados(cadenaCumplidas)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "UPDATE tacsPagadas SET CondicionOKAuto = '" & cadenaCumplidas & "' WHERE estatusConciliado = 1" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text

                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 098 tacsPagadas")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Function CD_TruncateTacsTmp()


            Dim query As String = "TRUNCATE TABLE tacsTmp"

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

                MsgBox(ex.Message & " " & "ERROR 002 Tacs")
                Return False

            Finally

                conexion.CerrarConexion()
            End Try



        End Function


        Public Function CD_InsertarPendientesTacsTmp(ByVal tacs As DataTable)

            Dim col = tacs.Columns.Cast(Of DataColumn).Where(Function(x) x.DataType <> GetType(String)).ToList()

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.tacsTmp"

                SqlBulkCopy.ColumnMappings.Add("Record Type", "RecordType")
                SqlBulkCopy.ColumnMappings.Add("TACS Record ID", "TACSRecordID")
                SqlBulkCopy.ColumnMappings.Add("Last Name", "LastName")
                SqlBulkCopy.ColumnMappings.Add("First Name", "FirstName")
                SqlBulkCopy.ColumnMappings.Add("Txn Cd", "TxnCd")
                'Try
                SqlBulkCopy.ColumnMappings.Add("Confirmation", "Confirmation")
                    SqlBulkCopy.ColumnMappings.Add("Arrival", "Arrival")
                'Catch ex As Exception
                'Throw


                'End Try
                SqlBulkCopy.ColumnMappings.Add("Departure", "Departure")
                SqlBulkCopy.ColumnMappings.Add("Report Revenue", "ReportRevenue")
                SqlBulkCopy.ColumnMappings.Add("Report Com", "ReportCom")
                SqlBulkCopy.ColumnMappings.Add("Report Currency", "ReportCurrency")
                SqlBulkCopy.ColumnMappings.Add("Pay Com", "PayCom")
                SqlBulkCopy.ColumnMappings.Add("Pay Currency", "PayCurrency")
                SqlBulkCopy.ColumnMappings.Add("Hotel Group Code", "HotelGroupCode")
                SqlBulkCopy.ColumnMappings.Add("Hotel Group Name", "HotelGroupName")
                SqlBulkCopy.ColumnMappings.Add("Property Code", "PropertyCode")
                SqlBulkCopy.ColumnMappings.Add("Property Name", "PropertyName")
                SqlBulkCopy.ColumnMappings.Add("Property Addr1", "PropertyAddr1")
                SqlBulkCopy.ColumnMappings.Add("Property Addr2", "PropertyAddr2")
                SqlBulkCopy.ColumnMappings.Add("Property City", "PropertyCity")
                SqlBulkCopy.ColumnMappings.Add("Property State Code", "PropertyStateCode")
                SqlBulkCopy.ColumnMappings.Add("Property Postal Code", "PropertyPostalCode")
                SqlBulkCopy.ColumnMappings.Add("Property Country", "PropertyCountry")
                SqlBulkCopy.ColumnMappings.Add("Property tax id", "Propertytaxid")
                SqlBulkCopy.ColumnMappings.Add("Holdback Currency", "HoldbackCurrency")
                SqlBulkCopy.ColumnMappings.Add("Holdback", "Holdback")
                SqlBulkCopy.ColumnMappings.Add("Fee", "Fee")
                SqlBulkCopy.ColumnMappings.Add("Payee ID from Payor", "PayeeIDfromPayor")
                SqlBulkCopy.ColumnMappings.Add("Tacs agency Id", "TacsagencyId")
                SqlBulkCopy.ColumnMappings.Add("Iata", "Iata")
                SqlBulkCopy.ColumnMappings.Add("Arc_num", "Arc_num")
                SqlBulkCopy.ColumnMappings.Add("Agency Legal Name", "AgencyLegalName")
                SqlBulkCopy.ColumnMappings.Add("Agency Name", "AgencyName")
                SqlBulkCopy.ColumnMappings.Add("Agency Attn", "AgencyAttn")
                SqlBulkCopy.ColumnMappings.Add("Agency Addr1", "AgencyAddr1")
                SqlBulkCopy.ColumnMappings.Add("Agency Addr2", "AgencyAddr2")
                SqlBulkCopy.ColumnMappings.Add("Agency City", "AgencyCity")
                SqlBulkCopy.ColumnMappings.Add("Agency State Code", "AgencyStateCode")
                SqlBulkCopy.ColumnMappings.Add("Agency Zip", "AgencyZip")
                SqlBulkCopy.ColumnMappings.Add("Agency Country Code", "AgencyCountryCode")
                SqlBulkCopy.ColumnMappings.Add("Property Phone", "PropertyPhone")
                SqlBulkCopy.ColumnMappings.Add("Payment ID", "PaymentID")
                SqlBulkCopy.ColumnMappings.Add("Cheque Number", "ChequeNumber")
                SqlBulkCopy.ColumnMappings.Add("Pay Date", "PayDate")
                SqlBulkCopy.ColumnMappings.Add("Revenue Report Currency", "RevenueReportCurrency")
                SqlBulkCopy.ColumnMappings.Add("Room Nights", "RoomNights")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(tacs)
                    Return True

                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 003 Tacs")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function


        Public Function CD_FaltantesTacs()

            'Dim procedure As String = "cargaTacsFaltantes"
            Dim query As String = "INSERT INTO [dbo].[tacs]
            ([RecordType]
            ,[TACSRecordID]
            ,[LastName]
            ,[FirstName]
            ,[TxnCd]
            ,[Confirmation]
            ,[Arrival]
            ,[Departure]
            ,[ReportRevenue]
            ,[ReportCom]
            ,[ReportCurrency]
            ,[PayCom]
            ,[PayCurrency]
            ,[HotelGroupCode]
            ,[HotelGroupName]
            ,[PropertyCode]
            ,[PropertyName]
            ,[PropertyAddr1]
            ,[PropertyAddr2]
            ,[PropertyCity]
            ,[PropertyStateCode]
            ,[PropertyPostalCode]
            ,[PropertyCountry]
            ,[Propertytaxid]
            ,[HoldbackCurrency]
            ,[Holdback]
            ,[Fee]
            ,[PayeeIDfromPayor]
            ,[TacsagencyId]
            ,[Iata]
            ,[Arc_num]
            ,[AgencyLegalName]
            ,[AgencyName]
            ,[AgencyAttn]
            ,[AgencyAddr1]
            ,[AgencyAddr2]
            ,[AgencyCity]
            ,[AgencyStateCode]
            ,[AgencyZip]
            ,[AgencyCountryCode]
            ,[PropertyPhone]
            ,[PaymentID]
            ,[ChequeNumber]
            ,[PayDate]
            ,[RevenueReportCurrency]
            ,[RoomNights])

            SELECT 
            [RecordType]
            ,[TACSRecordID]
            ,[LastName]
            ,[FirstName]
            ,[TxnCd]
            ,[Confirmation]
            ,[Arrival]
            ,[Departure]
            ,[ReportRevenue]
            ,[ReportCom]
            ,[ReportCurrency]
            ,[PayCom]
            ,[PayCurrency]
            ,[HotelGroupCode]
            ,[HotelGroupName]
            ,[PropertyCode]
            ,[PropertyName]
            ,[PropertyAddr1]
            ,[PropertyAddr2]
            ,[PropertyCity]
            ,[PropertyStateCode]
            ,[PropertyPostalCode]
            ,[PropertyCountry]
            ,[Propertytaxid]
            ,[HoldbackCurrency]
            ,[Holdback]
            ,[Fee]
            ,[PayeeIDfromPayor]
            ,[TacsagencyId]
            ,[Iata]
            ,[Arc_num]
            ,[AgencyLegalName]
            ,[AgencyName]
            ,[AgencyAttn]
            ,[AgencyAddr1]
            ,[AgencyAddr2]
            ,[AgencyCity]
            ,[AgencyStateCode]
            ,[AgencyZip]
            ,[AgencyCountryCode]
            ,[PropertyPhone]
            ,[PaymentID]
            ,[ChequeNumber]
            ,[PayDate]
            ,[RevenueReportCurrency]
            ,[RoomNights]

            FROM tacsTmp aTMP

            WHERE NOT EXISTS
            (SELECT * FROM tacs A 
            WHERE A.Confirmation = aTMP.Confirmation 
            AND A.Arrival = aTMP.Arrival)"

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
                MsgBox(ex.Message & " " & "ERROR 004 Tacs")
                Return False
            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Sub CD_InsertarTacsPagadas()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND o.mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND o.estatusEliminado IS NULL"


            Dim query = "INSERT INTO tacsPagadas(
            [RecordType]
            ,[TACSRecordID]
            ,[LastName]
            ,[FirstName]
            ,[TxnCd]
            ,[Confirmation]
            ,[Arrival]
            ,[Departure]
            ,[ReportRevenue]
            ,[ReportCom]
            ,[ReportCurrency]
            ,[PayCom]
            ,[PayCurrency]
            ,[HotelGroupCode]
            ,[HotelGroupName]
            ,[PropertyCode]
            ,[PropertyName]
            ,[PropertyAddr1]
            ,[PropertyAddr2]
            ,[PropertyCity]
            ,[PropertyStateCode]
            ,[PropertyPostalCode]
            ,[PropertyCountry]
            ,[Propertytaxid]
            ,[HoldbackCurrency]
            ,[Holdback]
            ,[Fee]
            ,[PayeeIDfromPayor]
            ,[TacsagencyId]
            ,[Iata]
            ,[Arc_num]
            ,[AgencyLegalName]
            ,[AgencyName]
            ,[AgencyAttn]
            ,[AgencyAddr1]
            ,[AgencyAddr2]
            ,[AgencyCity]
            ,[AgencyStateCode]
            ,[AgencyZip]
            ,[AgencyCountryCode]
            ,[PropertyPhone]
            ,[PaymentID]
            ,[ChequeNumber]
            ,[PayDate]
            ,[RevenueReportCurrency]
            ,[RoomNights]
            ,[mesProveedor]
            ,[estatusEliminado])

            SELECT 

            [RecordType]
            ,[TACSRecordID]
            ,[LastName]
            ,[FirstName]
            ,[TxnCd]
            ,[Confirmation]
            ,[Arrival]
            ,[Departure]
            ,[ReportRevenue]
            ,[ReportCom]
            ,[ReportCurrency]
            ,[PayCom]
            ,[PayCurrency]
            ,[HotelGroupCode]
            ,[HotelGroupName]
            ,[PropertyCode]
            ,[PropertyName]
            ,[PropertyAddr1]
            ,[PropertyAddr2]
            ,[PropertyCity]
            ,[PropertyStateCode]
            ,[PropertyPostalCode]
            ,[PropertyCountry]
            ,[Propertytaxid]
            ,[HoldbackCurrency]
            ,[Holdback]
            ,[Fee]
            ,[PayeeIDfromPayor]
            ,[TacsagencyId]
            ,[Iata]
            ,[Arc_num]
            ,[AgencyLegalName]
            ,[AgencyName]
            ,[AgencyAttn]
            ,[AgencyAddr1]
            ,[AgencyAddr2]
            ,[AgencyCity]
            ,[AgencyStateCode]
            ,[AgencyZip]
            ,[AgencyCountryCode]
            ,[PropertyPhone]
            ,[PaymentID]
            ,[ChequeNumber]
            ,[PayDate]
            ,[RevenueReportCurrency]
            ,[RoomNights]
            ,[mesProveedor]
            ,[estatusEliminado]

            FROM tacs o

            WHERE NOT EXISTS
            (SELECT * FROM tacsPagadas A 
            WHERE A.Confirmation = o.Confirmation 
            AND A.Arrival = o.Arrival)

            AND o.TxnCd = 'CB'  AND o.PayCom  <> 0 " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 012 Tacs")


            Finally

                conexion.CerrarConexion()

            End Try



        End Sub

        Public Sub CD_InsertarTacsObservaciones()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND o.mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND o.estatusEliminado IS NULL"

            Dim query = "INSERT INTO tacsObservaciones(
            [RecordType]
            ,[TACSRecordID]
            ,[LastName]
            ,[FirstName]
            ,[TxnCd]
            ,[Confirmation]
            ,[Arrival]
            ,[Departure]
            ,[ReportRevenue]
            ,[ReportCom]
            ,[ReportCurrency]
            ,[PayCom]
            ,[PayCurrency]
            ,[HotelGroupCode]
            ,[HotelGroupName]
            ,[PropertyCode]
            ,[PropertyName]
            ,[PropertyAddr1]
            ,[PropertyAddr2]
            ,[PropertyCity]
            ,[PropertyStateCode]
            ,[PropertyPostalCode]
            ,[PropertyCountry]
            ,[Propertytaxid]
            ,[HoldbackCurrency]
            ,[Holdback]
            ,[Fee]
            ,[PayeeIDfromPayor]
            ,[TacsagencyId]
            ,[Iata]
            ,[Arc_num]
            ,[AgencyLegalName]
            ,[AgencyName]
            ,[AgencyAttn]
            ,[AgencyAddr1]
            ,[AgencyAddr2]
            ,[AgencyCity]
            ,[AgencyStateCode]
            ,[AgencyZip]
            ,[AgencyCountryCode]
            ,[PropertyPhone]
            ,[PaymentID]
            ,[ChequeNumber]
            ,[PayDate]
            ,[RevenueReportCurrency]
            ,[RoomNights]
            ,[mesProveedor]
            ,[estatusEliminado])

            SELECT 

            [RecordType]
            ,[TACSRecordID]
            ,[LastName]
            ,[FirstName]
            ,[TxnCd]
            ,[Confirmation]
            ,[Arrival]
            ,[Departure]
            ,[ReportRevenue]
            ,[ReportCom]
            ,[ReportCurrency]
            ,[PayCom]
            ,[PayCurrency]
            ,[HotelGroupCode]
            ,[HotelGroupName]
            ,[PropertyCode]
            ,[PropertyName]
            ,[PropertyAddr1]
            ,[PropertyAddr2]
            ,[PropertyCity]
            ,[PropertyStateCode]
            ,[PropertyPostalCode]
            ,[PropertyCountry]
            ,[Propertytaxid]
            ,[HoldbackCurrency]
            ,[Holdback]
            ,[Fee]
            ,[PayeeIDfromPayor]
            ,[TacsagencyId]
            ,[Iata]
            ,[Arc_num]
            ,[AgencyLegalName]
            ,[AgencyName]
            ,[AgencyAttn]
            ,[AgencyAddr1]
            ,[AgencyAddr2]
            ,[AgencyCity]
            ,[AgencyStateCode]
            ,[AgencyZip]
            ,[AgencyCountryCode]
            ,[PropertyPhone]
            ,[PaymentID]
            ,[ChequeNumber]
            ,[PayDate]
            ,[RevenueReportCurrency]
            ,[RoomNights]
            ,[mesProveedor]
            ,[estatusEliminado]

            FROM tacs o
            WHERE NOT EXISTS
            (SELECT * FROM tacsObservaciones A 
            WHERE A.Confirmation = o.Confirmation 
            AND A.Arrival = o.Arrival)
            AND (o.TxnCd = 'NA' OR o.TxnCd = 'NS' OR o.TxnCd = 'NC') AND PayCom  = 0" & queryFechaProveedor & queryEstatusEliminado


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & "ERROR 011 Tacs")

            Finally

                conexion.CerrarConexion()
            End Try

        End Sub

        Public Function CD_SelectTacsObservaciones()

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "


            ''''''''''''''''''''''''''''''''''''''''' eliminar repetidos  ''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT *, NUMERO_REPETIDOS = ROW_NUMBER() OVER(PARTITION BY TACSRecordID ORDER BY id) 
            FROM tacsObservaciones)
            --SELECT * FROM CTE WHERE NUMERO_REPETIDOS > 1
            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 10xs1 Tacs")

            Finally

                conexion.CerrarConexion()

            End Try

            '''''''''''''''''''''''''''''''''''''''''  ''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaOnyxObservaciones As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT *  FROM tacsObservaciones" & queryFechaProveedor & queryEstatusEliminado & " AND estatusConciliado IS NULL"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxObservaciones.Load(leer)

                Return tablaOnyxObservaciones

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00324 Tacs")
                Return tablaOnyxObservaciones

            Finally

                conexion.CerrarConexion()

            End Try

        End Function


        Public Function CD_SelectTacsPagadas()

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "


            ''''''''''''''''''''''''''''''''''''''''' eliminar reoetidos  ''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT *, NUMERO_REPETIDOS = ROW_NUMBER() OVER(PARTITION BY TACSRecordID ORDER BY id) 
            FROM tacsPagadas)
            --SELECT * FROM CTE WHERE NUMERO_REPETIDOS > 1
            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 1013r4 Tacs")

            Finally

                conexion.CerrarConexion()

            End Try

            '''''''''''''''''''''''''''''''''''''''''  ''''''''''''''''''''''''''''''''''''''''''''''''



            Dim tablaOnyxObservaciones As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM tacsPagadas" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxObservaciones.Load(leer)

                Return tablaOnyxObservaciones

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00325 Tacs")
                Return tablaOnyxObservaciones

            Finally

                conexion.CerrarConexion()

            End Try

        End Function


        Public Function CD_SelectTacsPagadasFechaPagoProveedor()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            Dim tablaOnyxObservaciones As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM tacsPagadas" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxObservaciones.Load(leer)

                Return tablaOnyxObservaciones

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00325 Tacs")
                Return tablaOnyxObservaciones

            Finally

                conexion.CerrarConexion()

            End Try

        End Function




        Public Function CD_cargaArchivoTacs(tacs)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.tacs"

                SqlBulkCopy.ColumnMappings.Add("Record Type", "RecordType")
                SqlBulkCopy.ColumnMappings.Add("TACS Record ID", "TACSRecordID")
                SqlBulkCopy.ColumnMappings.Add("Last Name", "LastName")
                SqlBulkCopy.ColumnMappings.Add("First Name", "FirstName")
                SqlBulkCopy.ColumnMappings.Add("Txn Cd", "TxnCd")
                Try
                    SqlBulkCopy.ColumnMappings.Add("Confirmation", "Confirmation").ToString()
                    SqlBulkCopy.ColumnMappings.Add("Arrival", "Arrival").ToString()
                Catch ex As Exception

                End Try


                SqlBulkCopy.ColumnMappings.Add("Departure", "Departure")
                SqlBulkCopy.ColumnMappings.Add("Report Revenue", "ReportRevenue")
                SqlBulkCopy.ColumnMappings.Add("Report Com", "ReportCom")
                SqlBulkCopy.ColumnMappings.Add("Report Currency", "ReportCurrency")
                SqlBulkCopy.ColumnMappings.Add("Pay Com", "PayCom")
                SqlBulkCopy.ColumnMappings.Add("Pay Currency", "PayCurrency")
                SqlBulkCopy.ColumnMappings.Add("Hotel Group Code", "HotelGroupCode")
                SqlBulkCopy.ColumnMappings.Add("Hotel Group Name", "HotelGroupName")
                SqlBulkCopy.ColumnMappings.Add("Property Code", "PropertyCode")
                SqlBulkCopy.ColumnMappings.Add("Property Name", "PropertyName")
                SqlBulkCopy.ColumnMappings.Add("Property Addr1", "PropertyAddr1")
                SqlBulkCopy.ColumnMappings.Add("Property Addr2", "PropertyAddr2")
                SqlBulkCopy.ColumnMappings.Add("Property City", "PropertyCity")
                SqlBulkCopy.ColumnMappings.Add("Property State Code", "PropertyStateCode")
                SqlBulkCopy.ColumnMappings.Add("Property Postal Code", "PropertyPostalCode")
                SqlBulkCopy.ColumnMappings.Add("Property Country", "PropertyCountry")
                SqlBulkCopy.ColumnMappings.Add("Property tax id", "Propertytaxid")
                SqlBulkCopy.ColumnMappings.Add("Holdback Currency", "HoldbackCurrency")
                SqlBulkCopy.ColumnMappings.Add("Holdback", "Holdback")
                SqlBulkCopy.ColumnMappings.Add("Fee", "Fee")
                SqlBulkCopy.ColumnMappings.Add("Payee ID from Payor", "PayeeIDfromPayor")
                SqlBulkCopy.ColumnMappings.Add("Tacs agency Id", "TacsagencyId")
                SqlBulkCopy.ColumnMappings.Add("Iata", "Iata")
                SqlBulkCopy.ColumnMappings.Add("Arc_num", "Arc_num")
                SqlBulkCopy.ColumnMappings.Add("Agency Legal Name", "AgencyLegalName")
                SqlBulkCopy.ColumnMappings.Add("Agency Name", "AgencyName")
                SqlBulkCopy.ColumnMappings.Add("Agency Attn", "AgencyAttn")
                SqlBulkCopy.ColumnMappings.Add("Agency Addr1", "AgencyAddr1")
                SqlBulkCopy.ColumnMappings.Add("Agency Addr2", "AgencyAddr2")
                SqlBulkCopy.ColumnMappings.Add("Agency City", "AgencyCity")
                SqlBulkCopy.ColumnMappings.Add("Agency State Code", "AgencyStateCode")
                SqlBulkCopy.ColumnMappings.Add("Agency Zip", "AgencyZip")
                SqlBulkCopy.ColumnMappings.Add("Agency Country Code", "AgencyCountryCode")
                SqlBulkCopy.ColumnMappings.Add("Property Phone", "PropertyPhone")
                SqlBulkCopy.ColumnMappings.Add("Payment ID", "PaymentID")
                SqlBulkCopy.ColumnMappings.Add("Cheque Number", "ChequeNumber")
                SqlBulkCopy.ColumnMappings.Add("Pay Date", "PayDate")
                SqlBulkCopy.ColumnMappings.Add("Revenue Report Currency", "RevenueReportCurrency")
                SqlBulkCopy.ColumnMappings.Add("Room Nights", "RoomNights")



                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                    SqlBulkCopy.WriteToServer(tacs)
                    Return True
                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 005 Tacs")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function




        Public Function CD_SelectTacs() As DataTable


            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "


            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT *, NUMERO_REPETIDOS = ROW_NUMBER() OVER(PARTITION BY TACSRecordID ORDER BY id) 
            FROM tacs)
            --SELECT * FROM CTE WHERE NUMERO_REPETIDOS > 1
            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 101 Tacs")

            Finally

                conexion.CerrarConexion()

            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaTacs As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM tacs" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaTacs.Load(leer)

                Return tablaTacs

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 Tacs")
                Return tablaTacs

            Finally

                conexion.CerrarConexion()

            End Try


        End Function



        Public Function CD_SelectPayCom() As DataTable


            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim tablaTacs As DataTable = New DataTable()

            Dim query As String = "SELECT id,PayCom,PayCurrency,TxnCd FROM tacsPagadas" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaTacs.Load(leer)

                Return tablaTacs

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 010 Tacs")
                Return tablaTacs

            Finally

                conexion.CerrarConexion()

            End Try


        End Function


        Public Sub CD_changePaidCommission(tc)

            Dim query As String = "DECLARE @idTemp INT
DECLARE @PayComTemp FLOAT
DECLARE @TxnCd VARCHAR(10)
DECLARE @mesproveedor VARCHAR(100)
DECLARE @TC FLOAT
SET @TC = " & tc & "
SET @mesproveedor = '" & ClsGlobales.FechaPagoproveedor & "'
 
DECLARE C_A CURSOR

FOR
	SELECT
	id,
	PayCom,
	TxnCd
	FROM tacsPagadas
	WHERE mesProveedor = @mesproveedor
	AND estatusEliminado IS NULL;

OPEN C_A;
 
FETCH NEXT FROM C_A INTO 

 @idTemp,
 @PayComTemp,
 @TxnCd;
 
WHILE @@FETCH_STATUS = 0
    BEGIN

		IF (@PayComTemp IS NULL OR @PayComTemp = '')  BEGIN
			
			UPDATE tacsPagadas SET observaciones = @TxnCd WHERE id = @idTemp
		END
		ELSE BEGIN
			
			 UPDATE tacsPagadas SET
			 PayComTC = (@PayComTemp * @TC),
			 TC = @TC,
			 FechaCambioTC = GETDATE(),
			 PayCurrencyTC = 'MXN'
			 WHERE id = @idTemp
		END

        FETCH NEXT FROM C_A INTO @idTemp,@PayComTemp,@TxnCd;

    END;
 
CLOSE C_A;
 
DEALLOCATE C_A;"


            'Dim query As String = "UPDATE tacsPagadas SET PayComTC = " & PaidCommissionB & ",TC='" & tc & "', PayCurrencyTC = 'MXN', FechaCambioTC=GETDATE() WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 015 Tacs")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub








        Public Sub CD_UpdateDATEIN(id, datein)

            Dim query As String = "UPDATE tacs  SET  Arrival = '" & datein & "' WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()



            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0043 tacs")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub



        Public Sub CD_UpdateDATEOUT(id, dateout)

            Dim query As String = "UPDATE tacs  SET  Departure = '" & dateout & "' WHERE id = " & id & ""


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0044 tacs")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Function CD_ConciliarByID(id, idBDBCD, lastQuery)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE tacsPagadas SET estatusConciliado = 1 WHERE id  = " & id & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1265 tacsPagadas")
            Finally
                conexion.CerrarConexion()
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryB As String = "UPDATE BDBCD SET 
estatusConciliado = 1,
proveedor = 'tacs',
mesProveedor = '" & fechaProveedor & "'
WHERE id  = " & idBDBCD & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1266 tacsPagadas")
            Finally
                conexion.CerrarConexion()
            End Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim tabla As DataTable = New DataTable()

            Dim queryC As String = "
           SELECT
            BD.id AS idBDBCD,
            proveedor.id AS idProveedor,
            BD.UniqueBookingID as dim_value,
            '" & fechaProveedor & "' AS FechaApp,
            BD.HotelPropertyID AS UserSpec,
            BD.[LineNo] AS Segmento,
            BD.ConformationNo AS CodigoConfirmacion,
            CASE WHEN 
            proveedor.PayCom   = 0
            OR proveedor.PayCom IS NULL
            THEN  proveedor.observaciones ELSE CAST(proveedor.PayComTC as varchar(50)) END AS Comision,
            'Tacs' AS Operador,
            proveedor.PayCurrencyTC AS Moneda,
            ReportRevenue AS CostoTotalDeLaReserva,
            proveedor.RoomNights AS Noches,
            proveedor.PayComTC As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Adicional' As tipoConciliacion
            FROM  tacsPagadas proveedor
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
                MsgBox(ex.Message & " " & " ERROR 1267 tacs")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function


        Public Function CD_SelectSinConciliar() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM tacsPagadas WHERE CondicionOKAuto IS  NULL AND estatusConciliado IS  NULL AND idBDBCD IS  NULL" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)
                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0754 Tacs")
                Return tablaPosadas

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Sub CD_EliminarTacs(id)


            'Dim query As String = "DELETE FROM tacsPagadas WHERE id = " & id & ""
            Dim query As String = "UPDATE tacsPagadas SET estatusEliminado = 1 WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 101 tacsPagadas")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub


        Public Function CD_ObtenerUltimoId()

            Dim lastId As Int64

            Dim queryA As String = "SELECT MAX(id) FROM tacs"

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
                MsgBox(ex.Message & " " & " ERROR 1014 tacs")
            Finally
                conexion.CerrarConexion()
            End Try

        End Function


        Public Sub CD_agregarMesProveedor(ByVal lastId As Int64)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE tacs Set mesProveedor = '" & fechaProveedor & "' WHERE id > " & lastId & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1013 tacs")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub

        Public Function CD_SeleccionIDPendientesTacs()

            Dim tabla As DataTable = New DataTable()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = " SELECT id FROM tacsPagadas
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
                MsgBox(ex.Message & " " & " ERROR AAA tacsPagadas")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function






    End Class

End Namespace
