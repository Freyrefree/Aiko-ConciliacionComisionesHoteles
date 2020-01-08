Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data



Namespace CapaDatos

    Public Class ClsBDBCD

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()
        'Private sqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

        Public Function CD_DatosBDBCD()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM BDBCD"

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

                MsgBox(ex.Message & " " & "ERROR 001 BDBCD")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try



        End Function



        Public Function CD_ConsultaEliminadosBDBCD() As DataTable

            Dim comandoB As SqlCommand = New SqlCommand()
            Dim tabla As DataTable = New DataTable()

            Dim query As String = " SELECT * FROM BDBCDCancelados"

            Try
                comandoB.Connection = conexion.AbrirConexion()
                comandoB.CommandText = query
                comandoB.CommandType = CommandType.Text
                leer = comandoB.ExecuteReader()
                tabla.Load(leer)

                Return tabla


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 9086 BDBCD")
                Return tabla
            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Sub CD_ProcesoCanceladosBDBCD(query)


            Dim queryA As String = "
INSERT INTO BDBCDCancelados
(
[idBDBCD]
      ,[Version]
      ,[UniqueBookingID]
      ,[PNR]
      ,[SequenceNo]
      ,[CreateDate]
      ,[ModifyDate]
      ,[LineNo]
      ,[AgencyIDType]
      ,[AgencyID]
      ,[BookingAgent]
      ,[GuestName]
      ,[CorporateID]
      ,[AgentRef1]
      ,[AgentRef2]
      ,[AgentRef3]
      ,[NumberOfRooms]
      ,[NumberOfNights]
      ,[DateIn]
      ,[DateOut]
      ,[CommissionPercent]
      ,[CostPrNight]
      ,[FixedCommission]
      ,[Currency]
      ,[RateCode]
      ,[AccommodationType]
      ,[ConformationNo]
      ,[HotelPropertyID]
      ,[HotelChainID]
      ,[HotelName]
      ,[Address1]
      ,[Address2]
      ,[City]
      ,[State]
      ,[Zip]
      ,[AirportCityCode]
      ,[Phone]
      ,[Fax]
      ,[CountryCode]
      ,[AgentStatusCode]
      ,[AgentPaymentCode]
      ,[FechaAplicacion]
      ,[CodigoConfirmacion]
      ,[Comision]
      ,[Operardor]
      ,[ClienteTexto]
      ,[TarifaSucursal]
)
SELECT [id]
      ,[Version]
      ,[UniqueBookingID]
      ,[PNR]
      ,[SequenceNo]
      ,[CreateDate]
      ,[ModifyDate]
      ,[LineNo]
      ,[AgencyIDType]
      ,[AgencyID]
      ,[BookingAgent]
      ,[GuestName]
      ,[CorporateID]
      ,[AgentRef1]
      ,[AgentRef2]
      ,[AgentRef3]
      ,[NumberOfRooms]
      ,[NumberOfNights]
      ,[DateIn]
      ,[DateOut]
      ,[CommissionPercent]
      ,[CostPrNight]
      ,[FixedCommission]
      ,[Currency]
      ,[RateCode]
      ,[AccommodationType]
      ,[ConformationNo]
      ,[HotelPropertyID]
      ,[HotelChainID]
      ,[HotelName]
      ,[Address1]
      ,[Address2]
      ,[City]
      ,[State]
      ,[Zip]
      ,[AirportCityCode]
      ,[Phone]
      ,[Fax]
      ,[CountryCode]
      ,[AgentStatusCode]
      ,[AgentPaymentCode]
      ,[FechaAplicacion]
      ,[CodigoConfirmacion]
      ,[Comision]
      ,[Operardor]
      ,[ClienteTexto]
      ,[TarifaSucursal] 

FROM
(
"

            Dim queryB As String = "
) tmp 
 WHERE estatusConciliado IS NULL
 DELETE FROM BDBCD WHERE id IN (SELECT idBDBCD FROM BDBCDCancelados)

"

            Dim queryCompleto As String = queryA & query & queryB

            Dim tabla As DataTable = New DataTable()

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryCompleto
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 00123 BDBCD")
            Finally

                conexion.CerrarConexion()

            End Try

        End Sub


        Public Function CD_cargaArchivoBDBCD(BDBCD)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using sqlBulkCopy

                sqlBulkCopy.DestinationTableName = "dbo.BDBCD"
                SqlBulkCopy.ColumnMappings.Add("Version", "Version")
                SqlBulkCopy.ColumnMappings.Add("UniqueBookingID", "UniqueBookingID").ToString()
                SqlBulkCopy.ColumnMappings.Add("PNR", "PNR")
                SqlBulkCopy.ColumnMappings.Add("SequenceNo", "SequenceNo")
                SqlBulkCopy.ColumnMappings.Add("CreateDate", "CreateDate")
                SqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate")
                SqlBulkCopy.ColumnMappings.Add("LineNo", "LineNo")
                SqlBulkCopy.ColumnMappings.Add("AgencyIDType", "AgencyIDType")
                SqlBulkCopy.ColumnMappings.Add("AgencyID", "AgencyID")
                SqlBulkCopy.ColumnMappings.Add("BookingAgent", "BookingAgent")
                SqlBulkCopy.ColumnMappings.Add("GuestName", "GuestName")
                SqlBulkCopy.ColumnMappings.Add("CorporateID", "CorporateID")
                SqlBulkCopy.ColumnMappings.Add("AgentRef1", "AgentRef1")
                SqlBulkCopy.ColumnMappings.Add("AgentRef2", "AgentRef2")
                SqlBulkCopy.ColumnMappings.Add("AgentRef3", "AgentRef3")
                SqlBulkCopy.ColumnMappings.Add("NumberOfRooms", "NumberOfRooms")
                SqlBulkCopy.ColumnMappings.Add("NumberOfNights", "NumberOfNights")
                SqlBulkCopy.ColumnMappings.Add("DateIn", "DateIn")
                SqlBulkCopy.ColumnMappings.Add("DateOut", "DateOut")
                SqlBulkCopy.ColumnMappings.Add("CommissionPercent", "CommissionPercent")
                SqlBulkCopy.ColumnMappings.Add("CostPrNight", "CostPrNight")
                SqlBulkCopy.ColumnMappings.Add("FixedCommission", "FixedCommission")
                SqlBulkCopy.ColumnMappings.Add("Currency", "Currency")
                SqlBulkCopy.ColumnMappings.Add("RateCode", "RateCode")
                SqlBulkCopy.ColumnMappings.Add("AccommodationType", "AccommodationType")
                SqlBulkCopy.ColumnMappings.Add("ConformationNo", "ConformationNo")
                SqlBulkCopy.ColumnMappings.Add("HotelPropertyID", "HotelPropertyID")
                SqlBulkCopy.ColumnMappings.Add("HotelChainID", "HotelChainID")
                SqlBulkCopy.ColumnMappings.Add("HotelName", "HotelName")
                SqlBulkCopy.ColumnMappings.Add("Address1", "Address1")
                SqlBulkCopy.ColumnMappings.Add("Address2", "Address2")
                SqlBulkCopy.ColumnMappings.Add("City", "City")
                SqlBulkCopy.ColumnMappings.Add("State", "State")
                SqlBulkCopy.ColumnMappings.Add("Zip", "Zip")
                SqlBulkCopy.ColumnMappings.Add("AirportCityCode", "AirportCityCode")
                SqlBulkCopy.ColumnMappings.Add("Phone", "Phone")
                SqlBulkCopy.ColumnMappings.Add("Fax", "Fax")
                SqlBulkCopy.ColumnMappings.Add("CountryCode", "CountryCode")
                SqlBulkCopy.ColumnMappings.Add("AgentStatusCode", "AgentStatusCode")
                SqlBulkCopy.ColumnMappings.Add("AgentPaymentCode", "AgentPaymentCode")
                SqlBulkCopy.ColumnMappings.Add("FechaAplicacion", "FechaAplicacion")
                SqlBulkCopy.ColumnMappings.Add("CodigoConfirmacion", "CodigoConfirmacion")
                SqlBulkCopy.ColumnMappings.Add("Comision", "Comision")
                SqlBulkCopy.ColumnMappings.Add("Operardor", "Operardor")
                SqlBulkCopy.ColumnMappings.Add("ClienteTexto", "ClienteTexto")
                SqlBulkCopy.ColumnMappings.Add("TarifaSucursal", "TarifaSucursal")

                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 500 ' 5 minutos
                    SqlBulkCopy.WriteToServer(BDBCD)
                    Return True
                Catch ex As Exception
                    MsgBox(ex.Message & " " & "ERROR 002 BDBCD")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try


            End Using



        End Function

        Public Function CD_actualizaSegmento()

            Dim query As String = "
IF NOT OBJECT_ID('TEMPDB..#tblBDBCDTemporal') IS NULL  DROP TABLE #tblBDBCDTemporal

create table #tblBDBCDTemporal(estado varchar(50),segmentoNuevo int, segmentoViejo int, 
idTemoral int ,idOriginal int)

INSERT INTO #tblBDBCDTemporal 

SELECT * FROM 

 (SELECT IIF(temporal.segmentoNuevo = temporal.segmentoViejo, 'IGUAL', 'DIFERENTE') AS estado,
temporal.segmentoNuevo,
temporal.segmentoViejo,
temporal.idTemoral,
temporal.idOriginal
 FROM
(SELECT
bdbcdTemp.id as idTemoral,
BDBCD.id as idOriginal,
bdbcdTemp.[LineNo] as segmentoNuevo,
BDBCD.[LineNo] as segmentoViejo,


bdbcdTemp.Version,
bdbcdTemp.UniqueBookingID,
bdbcdTemp.PNR,
bdbcdTemp.SequenceNo,--
bdbcdTemp.CreateDate,
bdbcdTemp.ModifyDate,
bdbcdTemp.AgencyIDType,
bdbcdTemp.AgencyID,
bdbcdTemp.BookingAgent,
bdbcdTemp.GuestName,
bdbcdTemp.CorporateID,
bdbcdTemp.AgentRef1,
bdbcdTemp.AgentRef2,
bdbcdTemp.AgentRef3,
bdbcdTemp.NumberOfRooms,
bdbcdTemp.NumberOfNights,
bdbcdTemp.DateIn,
bdbcdTemp.DateOut,
bdbcdTemp.CommissionPercent,
bdbcdTemp.CostPrNight,
bdbcdTemp.FixedCommission,
bdbcdTemp.Currency,
bdbcdTemp.RateCode,
bdbcdTemp.AccommodationType,
bdbcdTemp.ConformationNo,
bdbcdTemp.HotelPropertyID,
bdbcdTemp.HotelChainID,
bdbcdTemp.HotelName,
bdbcdTemp.Address1,
bdbcdTemp.Address2,
bdbcdTemp.City,
bdbcdTemp.State,
bdbcdTemp.Zip,
bdbcdTemp.AirportCityCode,
bdbcdTemp.Phone,
bdbcdTemp.Fax,
bdbcdTemp.CountryCode,
bdbcdTemp.AgentStatusCode,
bdbcdTemp.AgentPaymentCode,
bdbcdTemp.FechaAplicacion,
bdbcdTemp.CodigoConfirmacion,
bdbcdTemp.Comision,
bdbcdTemp.Operardor,
bdbcdTemp.ClienteTexto,
bdbcdTemp.TarifaSucursal

FROM BDBCDSegmento bdbcdTemp
INNER JOIN BDBCD bdbcd
ON bdbcdTemp.Version = BDBCD.Version
AND bdbcdTemp.UniqueBookingID = BDBCD.UniqueBookingID
AND bdbcdTemp.PNR = BDBCD.PNR
AND bdbcdTemp.SequenceNo = BDBCD.SequenceNo
AND bdbcdTemp.CreateDate = BDBCD.CreateDate
AND bdbcdTemp.ModifyDate = BDBCD.ModifyDate
AND bdbcdTemp.AgencyIDType = BDBCD.AgencyIDType
AND bdbcdTemp.AgencyID = BDBCD.AgencyID
AND bdbcdTemp.BookingAgent = BDBCD.BookingAgent
AND bdbcdTemp.GuestName = BDBCD.GuestName
--AND bdbcdTemp.CorporateID = BDBCD.CorporateID
AND bdbcdTemp.AgentRef1 = BDBCD.AgentRef1
AND bdbcdTemp.AgentRef2 = BDBCD.AgentRef2
--AND bdbcdTemp.AgentRef3 = BDBCD.AgentRef3
AND bdbcdTemp.NumberOfRooms = BDBCD.NumberOfRooms
AND bdbcdTemp.NumberOfNights = BDBCD.NumberOfNights
AND bdbcdTemp.DateIn = BDBCD.DateIn
AND bdbcdTemp.DateOut = BDBCD.DateOut
AND bdbcdTemp.CommissionPercent = BDBCD.CommissionPercent
AND bdbcdTemp.CostPrNight = BDBCD.CostPrNight
--AND bdbcdTemp.FixedCommission = BDBCD.FixedCommission
AND bdbcdTemp.Currency = BDBCD.Currency
--AND bdbcdTemp.RateCode = BDBCD.RateCode
AND bdbcdTemp.AccommodationType = BDBCD.AccommodationType
AND bdbcdTemp.ConformationNo = BDBCD.ConformationNo
AND bdbcdTemp.HotelPropertyID = BDBCD.HotelPropertyID
AND bdbcdTemp.HotelChainID = BDBCD.HotelChainID
AND bdbcdTemp.HotelName = BDBCD.HotelName
AND bdbcdTemp.Address1 = BDBCD.Address1
AND bdbcdTemp.Address2 = BDBCD.Address2
AND bdbcdTemp.City = BDBCD.City
AND bdbcdTemp.State = BDBCD.State
AND bdbcdTemp.Zip = BDBCD.Zip
AND bdbcdTemp.AirportCityCode = BDBCD.AirportCityCode
AND bdbcdTemp.Phone = BDBCD.Phone
AND bdbcdTemp.Fax = BDBCD.Fax
AND bdbcdTemp.CountryCode = BDBCD.CountryCode
AND bdbcdTemp.AgentStatusCode = BDBCD.AgentStatusCode
AND bdbcdTemp.AgentPaymentCode = BDBCD.AgentPaymentCode
AND bdbcdTemp.FechaAplicacion = BDBCD.FechaAplicacion
--AND bdbcdTemp.CodigoConfirmacion = BDBCD.CodigoConfirmacion
AND bdbcdTemp.Comision = BDBCD.Comision
--AND bdbcdTemp.Operardor = BDBCD.Operardor
AND bdbcdTemp.ClienteTexto = BDBCD.ClienteTexto
AND bdbcdTemp.TarifaSucursal = BDBCD.TarifaSucursal

AND BDBCD.estatusConciliado IS NULL)  temporal) temporalB

WHERE temporalB.estado = 'DIFERENTE'

UPDATE tablaA 
SET tablaA.[LineNo] = tablaB.segmentoNuevo
FROM BDBCD tablaA 
INNER JOIN #tblBDBCDTemporal tablaB
ON tablaA.id = tablaB.idOriginal"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim rows_Affected As Integer

                comando.CommandTimeout = 60 * 5 ' 5 minutos
                'comando.ExecuteNonQuery()
                rows_Affected = comando.ExecuteNonQuery()
                Console.WriteLine(rows_Affected)
            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 001567 BDBCD")
            Finally

                conexion.CerrarConexion()

            End Try



        End Function

        Public Function CD_cargaArchivoBDBCDSegmento(BDBCD)

            Dim query As String = "TRUNCATE TABLE BDBCDSegmento"
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 001567 BDBCD")
            Finally

                conexion.CerrarConexion()

            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.BDBCDSegmento"
                SqlBulkCopy.ColumnMappings.Add("Version", "Version")
                SqlBulkCopy.ColumnMappings.Add("UniqueBookingID", "UniqueBookingID").ToString()
                SqlBulkCopy.ColumnMappings.Add("PNR", "PNR")
                SqlBulkCopy.ColumnMappings.Add("SequenceNo", "SequenceNo")
                SqlBulkCopy.ColumnMappings.Add("CreateDate", "CreateDate")
                SqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate")
                SqlBulkCopy.ColumnMappings.Add("LineNo", "LineNo")
                SqlBulkCopy.ColumnMappings.Add("AgencyIDType", "AgencyIDType")
                SqlBulkCopy.ColumnMappings.Add("AgencyID", "AgencyID")
                SqlBulkCopy.ColumnMappings.Add("BookingAgent", "BookingAgent")
                SqlBulkCopy.ColumnMappings.Add("GuestName", "GuestName")
                SqlBulkCopy.ColumnMappings.Add("CorporateID", "CorporateID")
                SqlBulkCopy.ColumnMappings.Add("AgentRef1", "AgentRef1")
                SqlBulkCopy.ColumnMappings.Add("AgentRef2", "AgentRef2")
                SqlBulkCopy.ColumnMappings.Add("AgentRef3", "AgentRef3")
                SqlBulkCopy.ColumnMappings.Add("NumberOfRooms", "NumberOfRooms")
                SqlBulkCopy.ColumnMappings.Add("NumberOfNights", "NumberOfNights")
                SqlBulkCopy.ColumnMappings.Add("DateIn", "DateIn")
                SqlBulkCopy.ColumnMappings.Add("DateOut", "DateOut")
                SqlBulkCopy.ColumnMappings.Add("CommissionPercent", "CommissionPercent")
                SqlBulkCopy.ColumnMappings.Add("CostPrNight", "CostPrNight")
                SqlBulkCopy.ColumnMappings.Add("FixedCommission", "FixedCommission")
                SqlBulkCopy.ColumnMappings.Add("Currency", "Currency")
                SqlBulkCopy.ColumnMappings.Add("RateCode", "RateCode")
                SqlBulkCopy.ColumnMappings.Add("AccommodationType", "AccommodationType")
                SqlBulkCopy.ColumnMappings.Add("ConformationNo", "ConformationNo")
                SqlBulkCopy.ColumnMappings.Add("HotelPropertyID", "HotelPropertyID")
                SqlBulkCopy.ColumnMappings.Add("HotelChainID", "HotelChainID")
                SqlBulkCopy.ColumnMappings.Add("HotelName", "HotelName")
                SqlBulkCopy.ColumnMappings.Add("Address1", "Address1")
                SqlBulkCopy.ColumnMappings.Add("Address2", "Address2")
                SqlBulkCopy.ColumnMappings.Add("City", "City")
                SqlBulkCopy.ColumnMappings.Add("State", "State")
                SqlBulkCopy.ColumnMappings.Add("Zip", "Zip")
                SqlBulkCopy.ColumnMappings.Add("AirportCityCode", "AirportCityCode")
                SqlBulkCopy.ColumnMappings.Add("Phone", "Phone")
                SqlBulkCopy.ColumnMappings.Add("Fax", "Fax")
                SqlBulkCopy.ColumnMappings.Add("CountryCode", "CountryCode")
                SqlBulkCopy.ColumnMappings.Add("AgentStatusCode", "AgentStatusCode")
                SqlBulkCopy.ColumnMappings.Add("AgentPaymentCode", "AgentPaymentCode")
                SqlBulkCopy.ColumnMappings.Add("FechaAplicacion", "FechaAplicacion")
                SqlBulkCopy.ColumnMappings.Add("CodigoConfirmacion", "CodigoConfirmacion")
                SqlBulkCopy.ColumnMappings.Add("Comision", "Comision")
                SqlBulkCopy.ColumnMappings.Add("Operardor", "Operardor")
                SqlBulkCopy.ColumnMappings.Add("ClienteTexto", "ClienteTexto")
                SqlBulkCopy.ColumnMappings.Add("TarifaSucursal", "TarifaSucursal")

                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 500 ' 500 minutos
                    SqlBulkCopy.WriteToServer(BDBCD)
                    Return True
                Catch ex As Exception
                    MsgBox(ex.Message & " " & " ERROR 001568 BDBCD")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try


            End Using



        End Function

        Public Function CD_TruncateBDBCDTMP()


            Dim query As String = "TRUNCATE TABLE BDBCDTmp"

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

                MsgBox(ex.Message & " " & "ERROR 003 BDBCD")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try



        End Function

        Public Function CD_InsertarPendientesBDBCDTmp(BDBCD)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.BDBCDTmp"
                SqlBulkCopy.ColumnMappings.Add("Version", "Version")
                SqlBulkCopy.ColumnMappings.Add("UniqueBookingID", "UniqueBookingID").ToString()
                SqlBulkCopy.ColumnMappings.Add("PNR", "PNR")
                SqlBulkCopy.ColumnMappings.Add("SequenceNo", "SequenceNo")
                SqlBulkCopy.ColumnMappings.Add("CreateDate", "CreateDate")
                SqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate")
                SqlBulkCopy.ColumnMappings.Add("LineNo", "LineNo")
                SqlBulkCopy.ColumnMappings.Add("AgencyIDType", "AgencyIDType")
                SqlBulkCopy.ColumnMappings.Add("AgencyID", "AgencyID")
                SqlBulkCopy.ColumnMappings.Add("BookingAgent", "BookingAgent")
                SqlBulkCopy.ColumnMappings.Add("GuestName", "GuestName")
                SqlBulkCopy.ColumnMappings.Add("CorporateID", "CorporateID")
                SqlBulkCopy.ColumnMappings.Add("AgentRef1", "AgentRef1")
                SqlBulkCopy.ColumnMappings.Add("AgentRef2", "AgentRef2")
                SqlBulkCopy.ColumnMappings.Add("AgentRef3", "AgentRef3")
                SqlBulkCopy.ColumnMappings.Add("NumberOfRooms", "NumberOfRooms")
                SqlBulkCopy.ColumnMappings.Add("NumberOfNights", "NumberOfNights")
                SqlBulkCopy.ColumnMappings.Add("DateIn", "DateIn")
                SqlBulkCopy.ColumnMappings.Add("DateOut", "DateOut")
                SqlBulkCopy.ColumnMappings.Add("CommissionPercent", "CommissionPercent")
                SqlBulkCopy.ColumnMappings.Add("CostPrNight", "CostPrNight")
                SqlBulkCopy.ColumnMappings.Add("FixedCommission", "FixedCommission")
                SqlBulkCopy.ColumnMappings.Add("Currency", "Currency")
                SqlBulkCopy.ColumnMappings.Add("RateCode", "RateCode")
                SqlBulkCopy.ColumnMappings.Add("AccommodationType", "AccommodationType")
                SqlBulkCopy.ColumnMappings.Add("ConformationNo", "ConformationNo")
                SqlBulkCopy.ColumnMappings.Add("HotelPropertyID", "HotelPropertyID")
                SqlBulkCopy.ColumnMappings.Add("HotelChainID", "HotelChainID")
                SqlBulkCopy.ColumnMappings.Add("HotelName", "HotelName")
                SqlBulkCopy.ColumnMappings.Add("Address1", "Address1")
                SqlBulkCopy.ColumnMappings.Add("Address2", "Address2")
                SqlBulkCopy.ColumnMappings.Add("City", "City")
                SqlBulkCopy.ColumnMappings.Add("State", "State")
                SqlBulkCopy.ColumnMappings.Add("Zip", "Zip")
                SqlBulkCopy.ColumnMappings.Add("AirportCityCode", "AirportCityCode")
                SqlBulkCopy.ColumnMappings.Add("Phone", "Phone")
                SqlBulkCopy.ColumnMappings.Add("Fax", "Fax")
                SqlBulkCopy.ColumnMappings.Add("CountryCode", "CountryCode")
                SqlBulkCopy.ColumnMappings.Add("AgentStatusCode", "AgentStatusCode")
                SqlBulkCopy.ColumnMappings.Add("AgentPaymentCode", "AgentPaymentCode")
                SqlBulkCopy.ColumnMappings.Add("FechaAplicacion", "FechaAplicacion")
                SqlBulkCopy.ColumnMappings.Add("CodigoConfirmacion", "CodigoConfirmacion")
                SqlBulkCopy.ColumnMappings.Add("Comision", "Comision")
                SqlBulkCopy.ColumnMappings.Add("Operardor", "Operardor")
                SqlBulkCopy.ColumnMappings.Add("ClienteTexto", "ClienteTexto")
                SqlBulkCopy.ColumnMappings.Add("TarifaSucursal", "TarifaSucursal")

                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 500 ' 5 minutos
                    SqlBulkCopy.WriteToServer(BDBCD)
                    Return True
                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 004 BDBCD")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function


        Public Function CD_FaltantesBDBCD()

            Dim procedure As String = "cargaBDBCDFaltantes"
            'Dim procedure As String = ""

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

                MsgBox(ex.Message & " " & "ERROR 005 BDBCD")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_CN_addColumnasBDBCD()

            Dim procedure As String = "addFirstAndLastNameBDBCD"

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

                MsgBox(ex.Message & " " & "ERROR 006 BDBCD")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Function CD_SeleccionIDPendientes() As DataTable

            Dim tabla As DataTable = New DataTable()

            Dim query = "SELECT
            id 
            FROM
            BDBCD 
            WHERE
            estatusConciliado IS NULL"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)

                Return tabla

            Catch ex As Exception
                MsgBox(ex.Message & " " & "ERROR A.34 BDBCD")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try

        End Function


        Public Function CD_SelectBDBCD() As DataTable


            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''

            Dim queryRepetidos = "WITH CTE AS 
            (SELECT ConformationNo, NUMERO_REPETIDOS=ROW_NUMBER() OVER(PARTITION BY UniqueBookingID, [LineNo] ORDER BY id) 
             FROM BDBCD)
            DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryRepetidos
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 101 BDBCD")

            Finally

                conexion.CerrarConexion()

            End Try


            '''''''''''''''''''''''''''''' Retornar Select''''''''''''''''''''''''''''''''''''''''''

            Dim tablaBDBCD As DataTable = New DataTable()
            tablaBDBCD.Rows.Clear()

            Dim procedure As String = "SelectBDBCD"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = procedure
                comando.CommandType = CommandType.StoredProcedure
                leer = comando.ExecuteReader()
                tablaBDBCD.Load(leer)

                Return tablaBDBCD

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 007 BDBCD")
                Return tablaBDBCD

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_ConsultaAcentos()

            Dim tabla As DataTable = New DataTable()

            Dim query As String = "SELECT id, firstName, lastName FROM BDBCD"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
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

                MsgBox(ex.Message & " " & "ERROR 0086 BDBCD")


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

                MsgBox(ex.Message & " " & "ERROR 0087 BDBCD")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Sub CD_quitarGuion()

            Dim query As String = "UPDATE BDBCD SET UniqueBookingID = LEFT(UniqueBookingID, CHARINDEX('-', UniqueBookingID) - 1)
            WHERE UniqueBookingID like'%-%'"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 166 BDBCD")

            Finally

                conexion.CerrarConexion()

            End Try

        End Sub



    End Class

End Namespace
