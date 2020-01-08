Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data

Namespace CapaDatos

    Public Class ClsOnyx

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
        Public BookingStatusCode As String
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

            Dim query As String = "INSERT INTO conciliacionDetalleOnyx(idConciliacion,dim_value,FechaApp,UserSpec,Segmento,
            CodigoConfirmacion,Comision,Operador,Moneda,CostoTotalDeLaReserva,Noches,ComOrig,SequenceNo,TipoConciliacion,BookingStatusCode)
            VALUES(" & Me.idConciliacion & ",'" & Me.dim_value & "','" & FechaApp & "','" & UserSpec & "','" & Segmento & "',
            '" & CodigoConfirmacion & "','" & Comision & "','" & Operador & "','" & Moneda & "','" & CostoTotalDeLaReserva & "',
            '" & Noches & "',
            '" & ComOrig & "','" & SequenceNo & "','" & TipoConciliacion & "','" & BookingStatusCode & "');"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.ExecuteNonQuery()
                Return lastId

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 152 Onyx")

                Return 0

            Finally

                conexion.CerrarConexion()
            End Try

        End Function


        Public Function CD_DatosOnyx()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM onyx"

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

                MsgBox(ex.Message & " " & "ERROR 001 Onyx")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try



        End Function




        Public Function CD_TruncateOnyxTmp()


            Dim query As String = "TRUNCATE TABLE onyxTmp"

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

                MsgBox(ex.Message & " " & "ERROR 002 Onyx")
                Return False

            Finally

                conexion.CerrarConexion()
            End Try



        End Function


        Public Function CD_InsertarPendientesOnyxTmp(onyx)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.onyxTmp"

                SqlBulkCopy.ColumnMappings.Add("Version", "Version")
                SqlBulkCopy.ColumnMappings.Add("UniqueBookingID", "UniqueBookingID")
                SqlBulkCopy.ColumnMappings.Add("PNR", "PNR")
                SqlBulkCopy.ColumnMappings.Add("SequenceNo", "SequenceNo")
                SqlBulkCopy.ColumnMappings.Add("CreateDate", "CreateDate")
                SqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate")
                SqlBulkCopy.ColumnMappings.Add("LineNo", "[LineNo]")
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
                SqlBulkCopy.ColumnMappings.Add("StatusDateTime", "StatusDateTime")
                SqlBulkCopy.ColumnMappings.Add("BookingStatusCode", "BookingStatusCode")
                SqlBulkCopy.ColumnMappings.Add("ExtraInfoCode", "ExtraInfoCode")
                SqlBulkCopy.ColumnMappings.Add("ConfNoRooms", "ConfNoRooms")
                SqlBulkCopy.ColumnMappings.Add("ConfNoNights", "ConfNoNights")
                SqlBulkCopy.ColumnMappings.Add("ConfDateIn", "ConfDateIn")
                SqlBulkCopy.ColumnMappings.Add("ConfDateOut", "ConfDateOut")
                SqlBulkCopy.ColumnMappings.Add("ConfCommissionPercent", "ConfCommissionPercent")
                SqlBulkCopy.ColumnMappings.Add("ConfCostPrNight", "ConfCostPrNight")
                SqlBulkCopy.ColumnMappings.Add("ConfFixedCommission", "ConfFixedCommission")
                SqlBulkCopy.ColumnMappings.Add("ConfCurrency", "ConfCurrency")
                SqlBulkCopy.ColumnMappings.Add("PaidStatus", "PaidStatus")
                SqlBulkCopy.ColumnMappings.Add("NTCommissionID", "NTCommissionID")
                SqlBulkCopy.ColumnMappings.Add("NTHotelAccount", "NTHotelAccount")
                SqlBulkCopy.ColumnMappings.Add("BookingReferal", "BookingReferal")
                SqlBulkCopy.ColumnMappings.Add("PaymentJournal", "PaymentJournal")
                SqlBulkCopy.ColumnMappings.Add("PaidCommission", "PaidCommission")
                SqlBulkCopy.ColumnMappings.Add("PaidCurrency", "PaidCurrency")
                SqlBulkCopy.ColumnMappings.Add("PaymentPoint", "PaymentPoint")
                SqlBulkCopy.ColumnMappings.Add("PaymentAccount", "PaymentAccount")
                SqlBulkCopy.ColumnMappings.Add("PaymentDate", "PaymentDate")
                SqlBulkCopy.ColumnMappings.Add("OfficeIDBookingAgency", "OfficeIDBookingAgency")
                SqlBulkCopy.ColumnMappings.Add("Invoice_Or_Credit_Number", "Invoice_Or_Credit_Number")
                SqlBulkCopy.ColumnMappings.Add("TC_SavingCode", "TC_SavingCode")
                SqlBulkCopy.ColumnMappings.Add("TC_ATOLCode", "TC_ATOLCode")
                SqlBulkCopy.ColumnMappings.Add("TC_VoucherType", "TC_VoucherType")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference1", "TC_Reference1")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference2", "TC_Reference2")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference3", "TC_Reference3")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference4", "TC_Reference4")
                SqlBulkCopy.ColumnMappings.Add("TC_HotelCode", "TC_HotelCode")
                SqlBulkCopy.ColumnMappings.Add("TC_AddressCode", "TC_AddressCode")
                SqlBulkCopy.ColumnMappings.Add("TC_DurationRackRate", "TC_DurationRackRate")
                SqlBulkCopy.ColumnMappings.Add("TC_DurationRackCurrency", "TC_DurationRackCurrency")
                SqlBulkCopy.ColumnMappings.Add("ConfCommissionVATPercent", "ConfCommissionVATPercent")
                SqlBulkCopy.ColumnMappings.Add("ConfCommissionVAT", "ConfCommissionVAT")
                SqlBulkCopy.ColumnMappings.Add("PaidCommissionBC", "PaidCommissionBC")
                SqlBulkCopy.ColumnMappings.Add("PaidCommissionNTFee", "PaidCommissionNTFee")
                SqlBulkCopy.ColumnMappings.Add("CommissionBookedCurrency", "CommissionBookedCurrency")
                SqlBulkCopy.ColumnMappings.Add("HotelVAT-ID", "[HotelVAT-ID]")
                SqlBulkCopy.ColumnMappings.Add("VAT-Amount-onFeeNTS", "[VAT-Amount-onFeeNTS]")
                SqlBulkCopy.ColumnMappings.Add("VAT-Percentage-onFeeNTS", "[VAT-Percentage-onFeeNTS]")
                SqlBulkCopy.ColumnMappings.Add("ISVATCalculated", "ISVATCalculated")
                SqlBulkCopy.ColumnMappings.Add("PaidGrossCommissionAmount", "PaidGrossCommissionAmount")
                SqlBulkCopy.ColumnMappings.Add("PaidGrossCommissionAmountCurrency", "PaidGrossCommissionAmountCurrency")
                SqlBulkCopy.ColumnMappings.Add("AccountingAmount", "AccountingAmount")
                SqlBulkCopy.ColumnMappings.Add("AccountingCurrency", "AccountingCurrency")
                SqlBulkCopy.ColumnMappings.Add("OnTacsDocument", "OnTacsDocument")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 5 ' 5 minutos
                    SqlBulkCopy.WriteToServer(onyx)
                    Return True

                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 003 Onyx")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function

        Public Function CD_RepetidosOnyxA()

            Dim comandoA As SqlCommand = New SqlCommand()
            Dim comandoB As SqlCommand = New SqlCommand()
            Dim comandoC As SqlCommand = New SqlCommand()
            Dim comandoD As SqlCommand = New SqlCommand()
            Dim comandoE As SqlCommand = New SqlCommand()
            Dim comandoF As SqlCommand = New SqlCommand()
            Dim comandoG As SqlCommand = New SqlCommand()

            Dim leerD As SqlDataReader
            Dim leerH As SqlDataReader
            Dim leerI As SqlDataReader
            Dim leerJ As SqlDataReader
            'Dim leerD As SqlDataReader
            'Dim leerD As SqlDataReader

            Dim respuesta(1) As Integer


            Dim mesProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim idOnyxBase As Integer
            Dim estatusConciliadoOnyxBase As Integer
            Dim mesProveedorOnyxBase As String
            Dim PaidStatus_A As String
            Dim NTCommissionID_A As String
            Dim NTHotelAccount_A As String
            Dim BookingStatusCode_A As String
            Dim PaidCommission_A As String

            Dim queryCuantosA As String
            Dim queryCuantosB As String
            Dim queryCuantosC As String

            Dim cuantosA As Integer = 0
            Dim cuantosB As Integer = 0
            Dim cuantosC As Integer = 0


            '--ONYX
            Dim idOnyxNormal As Integer

            '--PAGADAS
            Dim estatusConciliadoPagadas As Integer
            Dim idOnyxPagadas As Integer


            '--OBSERVACIONES
            Dim estatusConciliadoObservaciones As Integer
            Dim idOnyxObservaciones As Integer


            '--PENDIETES DE PAGO
            Dim estatusConciliadoPendientes As Integer
            Dim idOnyxPendientes As Integer

            '---------------------

            Dim queryonyxA As String
            Dim queryUpdateA As String = ""
            Dim queryUpdateB As String = ""
            Dim queryUpdateBB As String = ""

            Dim idOnyxRepetido As String = ""


            Dim queryA As String = "delete from onyx where mesProveedor = '" & mesProveedor & "'"
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryA
                comando.CommandType = CommandType.Text
                comando.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR  delete")
            Finally
                conexion.CerrarConexion()
            End Try

            Dim tablaOnyx As DataTable = New DataTable()


            Dim queryTabla As String = "SELECT
ony.id,
ony.estatusConciliado,
ony.mesProveedor,
tmp.PaidStatus,
tmp.NTCommissionID,
tmp.NTHotelAccount,
tmp.BookingStatusCode,
ony.PaidCommission

 FROM

(SELECT

	 [Version]
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
	,[StatusDateTime]
	,[BookingStatusCode]
	,[ExtraInfoCode]
	,[ConfNoRooms]
	,[ConfNoNights]
	,[ConfDateIn]
	,[ConfDateOut]
	,[ConfCommissionPercent]
	,[ConfCostPrNight]
	,[ConfFixedCommission]
	,[ConfCurrency]
	,[PaidStatus]
	,[NTCommissionID]
	,[NTHotelAccount]
	,[BookingReferal]
	,[PaymentJournal]
	,[PaidCommission]
	,[PaidCurrency]
	,[PaymentPoint]
	,[PaymentAccount]
	,[PaymentDate]
	,[OfficeIDBookingAgency]
	,[Invoice_Or_Credit_Number]
	,[TC_SavingCode]
	,[TC_ATOLCode]
	,[TC_VoucherType]
	,[TC_Reference1]
	,[TC_Reference2]
	,[TC_Reference3]
	,[TC_Reference4]
	,[TC_HotelCode]
	,[TC_AddressCode]
	,[TC_DurationRackRate]
	,[TC_DurationRackCurrency]
	,[ConfCommissionVATPercent]
	,[ConfCommissionVAT]
	,[PaidCommissionBC]
	,[PaidCommissionNTFee]
	,[CommissionBookedCurrency]
	,[HotelVAT-ID]
	,[VAT-Amount-onFeeNTS]
	,[VAT-Percentage-onFeeNTS]
	,[ISVATCalculated]
	,[PaidGrossCommissionAmount]
	,[PaidGrossCommissionAmountCurrency]
	,[AccountingAmount]
	,[AccountingCurrency]
	,[OnTacsDocument]
	,[Fechadepago]

FROM onyxTMP oTMP

WHERE  EXISTS
(				   
	SELECT * FROM onyx O 
	WHERE O.NTCommissionID = oTMP.NTCommissionID 
	AND O.NTHotelAccount = oTMP.NTHotelAccount
	AND O.PaidStatus = oTMP.PaidStatus

))tmp

INNER JOIN onyx ony 
ON tmp.NTCommissionID = ony.NTCommissionID
AND tmp.NTHotelAccount = ony.NTHotelAccount
AND tmp.PaidStatus = ony.PaidStatus"


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryTabla
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                leer = comando.ExecuteReader()




                While leer.Read()

                    idOnyxBase = vbEmpty
                    estatusConciliadoOnyxBase = vbEmpty
                    mesProveedorOnyxBase = vbEmpty
                    PaidStatus_A = vbEmpty
                    NTCommissionID_A = vbEmpty
                    NTHotelAccount_A = vbEmpty
                    BookingStatusCode_A = vbEmpty
                    PaidCommission_A = vbEmpty

                    queryCuantosA = vbEmpty
                    queryCuantosB = vbEmpty
                    queryCuantosC = vbEmpty

                    cuantosA = vbEmpty
                    cuantosB = vbEmpty
                    cuantosC = vbEmpty

                    queryonyxA = vbEmpty

                    respuesta = Nothing

                    idOnyxBase = leer("id")
                    estatusConciliadoOnyxBase = Convert.ToInt64(If(TypeOf leer("estatusConciliado") Is DBNull, 0, leer("estatusConciliado")))
                    mesProveedorOnyxBase = Convert.ToString(If(TypeOf leer("mesProveedor") Is DBNull, 0, leer("mesProveedor"))) 'leer("mesProveedor")
                    PaidStatus_A = leer("PaidStatus")
                    NTCommissionID_A = leer("NTCommissionID")
                    NTHotelAccount_A = leer("NTHotelAccount")
                    BookingStatusCode_A = leer("BookingStatusCode")
                    PaidCommission_A = Convert.ToString(If(TypeOf leer("PaidCommission") Is DBNull, 0, leer("PaidCommission")))

                    queryCuantosA = "SELECT count(*) as a FROM onyxPagadas WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"
                    queryCuantosB = "SELECT count(*) as b FROM onyxObservaciones WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"
                    queryCuantosC = "SELECT count(*) as c FROM onyxComisionesPendientePago WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"

                    'Obtener id para insertar en onyxRepetidos

                    idOnyxRepetido &= idOnyxBase & " OR id = "
                    'Continue While

                    Try
                        comandoA.Connection = conexion.AbrirConexion()
                        comandoA.CommandText = queryCuantosA
                        comandoA.CommandType = CommandType.Text
                        comandoA.CommandTimeout = 60 * 5 ' 5 minutos
                        cuantosA = Convert.ToInt16(comandoA.ExecuteScalar())
                    Catch ex As Exception
                        MsgBox(ex.Message & " " & " ERROR  cuantosA")
                    End Try

                    Try
                        comandoB.Connection = conexion.AbrirConexion()
                        comandoB.CommandText = queryCuantosB
                        comandoB.CommandType = CommandType.Text
                        comandoB.CommandTimeout = 60 * 5 ' 5 minutos
                        cuantosB = Convert.ToInt16(comandoB.ExecuteScalar())
                    Catch ex As Exception
                        MsgBox(ex.Message & " " & " ERROR  cuantosB")
                    End Try

                    Try
                        comandoC.Connection = conexion.AbrirConexion()
                        comandoC.CommandText = queryCuantosC
                        comandoC.CommandType = CommandType.Text
                        comandoC.CommandTimeout = 60 * 5 ' 5 minutos
                        cuantosC = Convert.ToInt16(comandoC.ExecuteScalar())

                    Catch ex As Exception
                        MsgBox(ex.Message & " " & " ERROR  cuantosC")
                    End Try

                    If (cuantosA = 1 Or cuantosB = 1 Or cuantosC = 1) Then

                        queryonyxA = "SELECT id
	                    FROM onyx 
	                    WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"

                        idOnyxNormal = FuncionidOnyxNormal(queryonyxA)

                        queryUpdateB &= "UPDATE onyx SET BookingStatusCode = '" & BookingStatusCode_A & "', mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxNormal & ";"


                    End If

                    '---BUSQUEDA EN TABLAS 
                    '---PAGADAS

                    If cuantosA > 0 Then

                        Dim queryTA As String = "SELECT 
	                    estatusConciliado,
	                    id
	                    FROM onyxPagadas 
	                    WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"

                        respuesta = estatusID(queryTA)
                        estatusConciliadoPagadas = respuesta(0)
                        idOnyxPagadas = respuesta(1)

                        If estatusConciliadoPagadas = 1 Then
                            queryUpdateB &= "UPDATE onyxPagadas SET BookingStatusCode = '" & BookingStatusCode_A & "', mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxPagadas & ";"

                        Else
                            queryUpdateB &= "UPDATE onyxPagadas SET  mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxPagadas & ";"

                        End If

                    End If

                    '---OBSERVACIONES
                    If cuantosB > 0 Then

                        Dim queryTB As String = "SELECT 
	                    estatusConciliado,
	                    id
	                    FROM onyxObservaciones 
	                    WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"

                        respuesta = estatusID(queryTB)
                        estatusConciliadoObservaciones = respuesta(0)
                        idOnyxObservaciones = respuesta(1)

                        If estatusConciliadoObservaciones = 1 Then
                            queryUpdateB &= "UPDATE onyxObservaciones SET BookingStatusCode = '" & BookingStatusCode_A & "', mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxObservaciones & ";"

                        Else
                            queryUpdateB &= "UPDATE onyxObservaciones SET  mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxObservaciones & ";"

                        End If

                    End If
                    '---COMISIONES PENDIENTE PAGO

                    If cuantosC > 0 Then

                        Dim queryTC As String = "SELECT 
	                    estatusConciliado,
	                    id
	                    FROM onyxComisionesPendientePago 
	                    WHERE NTCommissionID = '" & NTCommissionID_A & "' AND NTHotelAccount = '" & NTHotelAccount_A & "' AND PaidStatus = '" & PaidStatus_A & "'"

                        respuesta = estatusID(queryTC)
                        estatusConciliadoPendientes = respuesta(0)
                        idOnyxPendientes = respuesta(1)

                        If estatusConciliadoPendientes = 1 Then
                            queryUpdateB &= "UPDATE onyxComisionesPendientePago SET BookingStatusCode = '" & BookingStatusCode_A & "', mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxPendientes & ";"

                        Else
                            queryUpdateB &= "UPDATE onyxComisionesPendientePago SET  mesProveedor = '" & mesProveedor & "'  WHERE id = " & idOnyxPendientes & ";"

                        End If

                    End If
                End While


                If queryUpdateB <> "" Then
                    insertarRepetidos(idOnyxRepetido)
                    ready(queryUpdateB)
                End If



            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR  Tabla")
            Finally
                conexion.CerrarConexion()
            End Try




        End Function

        Public Function ready(query)
            Dim comandozzz As SqlCommand = New SqlCommand()

            Try
                comandozzz.Connection = conexion.AbrirConexion()
                comandozzz.CommandText = query
                comandozzz.CommandType = CommandType.Text
                comandozzz.CommandTimeout = 60 * 60 ' 10 minutos
                comandozzz.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR  Update Onyx ")
            End Try


        End Function
        Public Function insertarRepetidos(query)
            query = query.Remove(query.Length - 8)
            Dim mesProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryCompleto As String
            Dim queryInsert As String = "
	INSERT INTO [dbo].[onyxRepetido]
                ([Version]
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
                ,[StatusDateTime]
                ,[BookingStatusCode]
                ,[ExtraInfoCode]
                ,[ConfNoRooms]
                ,[ConfNoNights]
                ,[ConfDateIn]
                ,[ConfDateOut]
                ,[ConfCommissionPercent]
                ,[ConfCostPrNight]
                ,[ConfFixedCommission]
                ,[ConfCurrency]
                ,[PaidStatus]
                ,[NTCommissionID]
                ,[NTHotelAccount]
                ,[BookingReferal]
                ,[PaymentJournal]
                ,[PaidCommission]
                ,[PaidCurrency]
                ,[PaymentPoint]
                ,[PaymentAccount]
                ,[PaymentDate]
                ,[OfficeIDBookingAgency]
                ,[Invoice_Or_Credit_Number]
                ,[TC_SavingCode]
                ,[TC_ATOLCode]
                ,[TC_VoucherType]
                ,[TC_Reference1]
                ,[TC_Reference2]
                ,[TC_Reference3]
                ,[TC_Reference4]
                ,[TC_HotelCode]
                ,[TC_AddressCode]
                ,[TC_DurationRackRate]
                ,[TC_DurationRackCurrency]
                ,[ConfCommissionVATPercent]
                ,[ConfCommissionVAT]
                ,[PaidCommissionBC]
                ,[PaidCommissionNTFee]
                ,[CommissionBookedCurrency]
                ,[HotelVAT-ID]
                ,[VAT-Amount-onFeeNTS]
                ,[VAT-Percentage-onFeeNTS]
                ,[ISVATCalculated]
                ,[PaidGrossCommissionAmount]
                ,[PaidGrossCommissionAmountCurrency]
                ,[AccountingAmount]
                ,[AccountingCurrency]
                ,[OnTacsDocument]
                ,[Fechadepago]
				,[mesProveedorAnterior]
				,[mesProveedorActual]
				)
				
				SELECT
				
				 [Version]
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
                ,[StatusDateTime]
                ,[BookingStatusCode]
                ,[ExtraInfoCode]
                ,[ConfNoRooms]
                ,[ConfNoNights]
                ,[ConfDateIn]
                ,[ConfDateOut]
                ,[ConfCommissionPercent]
                ,[ConfCostPrNight]
                ,[ConfFixedCommission]
                ,[ConfCurrency]
                ,[PaidStatus]
                ,[NTCommissionID]
                ,[NTHotelAccount]
                ,[BookingReferal]
                ,[PaymentJournal]
                ,[PaidCommission]
                ,[PaidCurrency]
                ,[PaymentPoint]
                ,[PaymentAccount]
                ,[PaymentDate]
                ,[OfficeIDBookingAgency]
                ,[Invoice_Or_Credit_Number]
                ,[TC_SavingCode]
                ,[TC_ATOLCode]
                ,[TC_VoucherType]
                ,[TC_Reference1]
                ,[TC_Reference2]
                ,[TC_Reference3]
                ,[TC_Reference4]
                ,[TC_HotelCode]
                ,[TC_AddressCode]
                ,[TC_DurationRackRate]
                ,[TC_DurationRackCurrency]
                ,[ConfCommissionVATPercent]
                ,[ConfCommissionVAT]
                ,[PaidCommissionBC]
                ,[PaidCommissionNTFee]
                ,[CommissionBookedCurrency]
                ,[HotelVAT-ID]
                ,[VAT-Amount-onFeeNTS]
                ,[VAT-Percentage-onFeeNTS]
                ,[ISVATCalculated]
                ,[PaidGrossCommissionAmount]
                ,[PaidGrossCommissionAmountCurrency]
                ,[AccountingAmount]
                ,[AccountingCurrency]
                ,[OnTacsDocument]
                ,[Fechadepago]
				,[mesProveedor]
				,'" & mesProveedor & "'
				from onyx

				where id = 
"

            queryCompleto = queryInsert & query

            Dim comandoRepetido As SqlCommand = New SqlCommand()

            Try
                comandoRepetido.Connection = conexion.AbrirConexion()
                comandoRepetido.CommandText = queryCompleto
                comandoRepetido.CommandType = CommandType.Text
                comandoRepetido.CommandTimeout = 60 * 60 ' 10 minutos
                comandoRepetido.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR  insertar Repetidos Onyx ")
            End Try


        End Function

        Public Function estatusID(query)

            Dim numbers(1) As Integer

            Dim estatusConciliadoObservaciones As Integer
            Dim idOnyxObservaciones As Integer

            Dim comandozz As SqlCommand = New SqlCommand()
            Dim leerzz As SqlDataReader
            Try
                comandozz.Connection = conexion.AbrirConexion()
                comandozz.CommandText = query
                comandozz.CommandType = CommandType.Text
                comandozz.CommandTimeout = 60 * 5 ' 5 minutos

                leerzz = comandozz.ExecuteReader()
                While leerzz.Read()
                    estatusConciliadoObservaciones = Convert.ToInt64(If(TypeOf leerzz(0) Is DBNull, 0, leerzz(0)))
                    idOnyxObservaciones = Convert.ToInt64(If(TypeOf leerzz(1) Is DBNull, 0, leerzz(1)))
                End While
                numbers(0) = estatusConciliadoObservaciones
                numbers(1) = idOnyxObservaciones
                Return numbers

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR  cuantosB")
            End Try
        End Function

        Public Function FuncionidOnyxNormal(query)
            Dim comandoz As SqlCommand = New SqlCommand()
            Dim leerz As SqlDataReader

            Dim id As Integer
            Try
                comandoz.Connection = conexion.AbrirConexion()
                comandoz.CommandText = query
                comandoz.CommandType = CommandType.Text
                comandoz.CommandTimeout = 60 * 5 ' 5 minutos

                leerz = comandoz.ExecuteReader()
                While leerz.Read()
                    id = leerz(0)
                End While

                Return id
            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR  cuantosB")
            End Try

        End Function

        Public Function CD_RepetidosOnyx()

            '            Dim mesProveedor As String = ClsGlobales.FechaPagoproveedor

            '            Dim proceso As String = "

            'delete from onyx where mesProveedor = '" & mesProveedor & "'
            '-- Declaracion de variables para el cursor

            'DECLARE 

            '@idOnyxBase int,
            '@estatusConciliadoOnyxBase int,
            '@mesProveedorOnyxBase varchar(15),
            '@PaidStatus_A varchar(10),
            '@NTCommissionID_A varchar(30),
            '@NTHotelAccount_A varchar(30),
            '@BookingStatusCode_A varchar(30),
            '@PaidCommission_A decimal(18, 3),

            '@cuantosA integer,
            '@cuantosB integer,
            '@cuantosC integer,

            '--ONYX
            '@idOnyxNormal integer,

            '--PAGADAS
            '@estatusConciliadoPagadas integer,
            '@idOnyxPagadas integer,
            '@BookingStatusCodePagadas varchar(30),

            '--OBSERVACIONES
            '@estatusConciliadoObservaciones integer,
            '@idOnyxObservaciones integer,
            '@BookingStatusCodeObservaciones varchar(30),

            '--PENDIETES DE PAGO
            '@estatusConciliadoPendientes integer,
            '@idOnyxPendientes integer,
            '@BookingStatusCodePendientes varchar(30),
            '---------------------
            '@mesProveedor VARCHAR(20) = '" & mesProveedor & "'

            '--Declaración del cursor

            'DECLARE repetidosOnyx CURSOR FOR

            'SELECT
            'ony.id,
            'ony.estatusConciliado,
            'ony.mesProveedor,
            'tmp.PaidStatus,
            'tmp.NTCommissionID,
            'tmp.NTHotelAccount,
            'tmp.BookingStatusCode,
            'ony.PaidCommission

            ' FROM

            '(SELECT

            '	 [Version]
            '	,[UniqueBookingID]
            '	,[PNR]
            '	,[SequenceNo]
            '	,[CreateDate]
            '	,[ModifyDate]
            '	,[LineNo]
            '	,[AgencyIDType]
            '	,[AgencyID]
            '	,[BookingAgent]
            '	,[GuestName]
            '	,[CorporateID]
            '	,[AgentRef1]
            '	,[AgentRef2]
            '	,[AgentRef3]
            '	,[NumberOfRooms]
            '	,[NumberOfNights]
            '	,[DateIn]
            '	,[DateOut]
            '	,[CommissionPercent]
            '	,[CostPrNight]
            '	,[FixedCommission]
            '	,[Currency]
            '	,[RateCode]
            '	,[AccommodationType]
            '	,[ConformationNo]
            '	,[HotelPropertyID]
            '	,[HotelChainID]
            '	,[HotelName]
            '	,[Address1]
            '	,[Address2]
            '	,[City]
            '	,[State]
            '	,[Zip]
            '	,[AirportCityCode]
            '	,[Phone]
            '	,[Fax]
            '	,[CountryCode]
            '	,[StatusDateTime]
            '	,[BookingStatusCode]
            '	,[ExtraInfoCode]
            '	,[ConfNoRooms]
            '	,[ConfNoNights]
            '	,[ConfDateIn]
            '	,[ConfDateOut]
            '	,[ConfCommissionPercent]
            '	,[ConfCostPrNight]
            '	,[ConfFixedCommission]
            '	,[ConfCurrency]
            '	,[PaidStatus]
            '	,[NTCommissionID]
            '	,[NTHotelAccount]
            '	,[BookingReferal]
            '	,[PaymentJournal]
            '	,[PaidCommission]
            '	,[PaidCurrency]
            '	,[PaymentPoint]
            '	,[PaymentAccount]
            '	,[PaymentDate]
            '	,[OfficeIDBookingAgency]
            '	,[Invoice_Or_Credit_Number]
            '	,[TC_SavingCode]
            '	,[TC_ATOLCode]
            '	,[TC_VoucherType]
            '	,[TC_Reference1]
            '	,[TC_Reference2]
            '	,[TC_Reference3]
            '	,[TC_Reference4]
            '	,[TC_HotelCode]
            '	,[TC_AddressCode]
            '	,[TC_DurationRackRate]
            '	,[TC_DurationRackCurrency]
            '	,[ConfCommissionVATPercent]
            '	,[ConfCommissionVAT]
            '	,[PaidCommissionBC]
            '	,[PaidCommissionNTFee]
            '	,[CommissionBookedCurrency]
            '	,[HotelVAT-ID]
            '	,[VAT-Amount-onFeeNTS]
            '	,[VAT-Percentage-onFeeNTS]
            '	,[ISVATCalculated]
            '	,[PaidGrossCommissionAmount]
            '	,[PaidGrossCommissionAmountCurrency]
            '	,[AccountingAmount]
            '	,[AccountingCurrency]
            '	,[OnTacsDocument]
            '	,[Fechadepago]

            'FROM onyxTMP oTMP

            'WHERE  EXISTS
            '(				   
            '	SELECT * FROM onyx O 
            '	WHERE O.NTCommissionID = oTMP.NTCommissionID 
            '	AND O.NTHotelAccount = oTMP.NTHotelAccount
            '	AND O.PaidStatus = oTMP.PaidStatus

            '))tmp

            'INNER JOIN onyx ony 
            'ON tmp.NTCommissionID = ony.NTCommissionID
            'AND tmp.NTHotelAccount = ony.NTHotelAccount
            'AND tmp.PaidStatus = ony.PaidStatus

            '-- Apertura del cursor

            'OPEN repetidosOnyx

            '-- Lectura de la primera fila del cursor
            'FETCH repetidosOnyx INTO    @idOnyxBase, @estatusConciliadoOnyxBase, @mesProveedorOnyxBase,@PaidStatus_A, @NTCommissionID_A, @NTHotelAccount_A, @BookingStatusCode_A,@PaidCommission_A
            'WHILE (@@FETCH_STATUS = 0 )

            'BEGIN
            '--PRINT @Nombre + ' ' + @Apellido1 + ' ' + @Apellido2

            'SELECT @cuantosA = COUNT(*) FROM onyxPagadas WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A
            'SELECT @cuantosB = COUNT(*) FROM onyxObservaciones WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A
            'SELECT @cuantosC = COUNT(*) FROM onyxComisionesPendientePago WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A

            '---En tabla Principal Onyx

            'IF @cuantosA = 1 OR @cuantosB = 1 OR @cuantosC = 1 BEGIN

            '	SELECT 
            '	@idOnyxNormal = id
            '	FROM onyx 
            '	WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A

            '	UPDATE onyx SET BookingStatusCode = @BookingStatusCode_A, mesProveedor = @mesProveedor WHERE id = @idOnyxNormal

            'END

            '---BUSQUEDA EN TABLAS 
            '---PAGADAS

            ' IF @cuantosA > 0 BEGIN

            '	SELECT 
            '	@estatusConciliadoPagadas = estatusConciliado,
            '	@idOnyxPagadas = id
            '	FROM onyxPagadas 
            '	WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A

            '	IF @estatusConciliadoPagadas = 1 BEGIN

            '		UPDATE onyxPagadas SET BookingStatusCode = @BookingStatusCode_A, mesProveedor = @mesProveedor WHERE id = @idOnyxPagadas
            '	END ELSE  BEGIN
            '		UPDATE onyxPagadas SET mesProveedor = @mesProveedor WHERE id = @idOnyxPagadas

            '	END

            ' END

            ' ---OBSERVACIONES

            '  IF @cuantosB > 0 BEGIN

            '	SELECT 
            '	@estatusConciliadoObservaciones = estatusConciliado,
            '	@idOnyxObservaciones = id
            '	FROM onyxObservaciones 
            '	WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A

            '	IF @estatusConciliadoObservaciones = 1 BEGIN

            '		UPDATE onyxObservaciones SET BookingStatusCode = @BookingStatusCode_A, mesProveedor = @mesProveedor WHERE id = @idOnyxObservaciones

            '	END ELSE  BEGIN
            '		UPDATE onyxObservaciones SET mesProveedor = @mesProveedor WHERE id = @idOnyxObservaciones
            '	END

            ' END

            ' ---COMISIONES PENDIENTE PAGO
            '   IF @cuantosC > 0 BEGIN

            '	SELECT 
            '	@estatusConciliadoPendientes = estatusConciliado,
            '	@idOnyxPendientes = id
            '	FROM onyxComisionesPendientePago 
            '	WHERE NTCommissionID = @NTCommissionID_A AND NTHotelAccount = @NTHotelAccount_A AND PaidStatus = @PaidStatus_A

            '	IF @estatusConciliadoPendientes = 1 BEGIN

            '		UPDATE onyxComisionesPendientePago SET BookingStatusCode = @BookingStatusCode_A, mesProveedor = @mesProveedor WHERE id = @idOnyxPendientes
            '	END ELSE  BEGIN
            '		UPDATE onyxComisionesPendientePago SET mesProveedor = @mesProveedor WHERE id = @idOnyxPendientes

            '	END

            ' END
            ' ---------------------------------------------------------------------------------------------------------------------------------------------
            ' ---- Agregar Nuevos
            ' ------------------------------ PAGADAS

            '-- Lectura de la siguiente fila del cursor
            'FETCH repetidosOnyx INTO    @idOnyxBase, @estatusConciliadoOnyxBase, @mesProveedorOnyxBase,@PaidStatus_A, @NTCommissionID_A, @NTHotelAccount_A,@BookingStatusCode_A,@PaidCommission_A

            'END

            '-- Cierre del cursor
            'CLOSE repetidosOnyx
            '-- Liberar los recursos
            'DEALLOCATE repetidosOnyx
            '"

            '            Try
            '                comando.Connection = conexion.AbrirConexion()
            '                comando.CommandText = proceso
            '                comando.CommandType = CommandType.Text
            '                comando.CommandTimeout = 60 * 60 ' 60 minutos
            '                comando.Parameters.Clear()
            '                Dim res As Boolean = comando.ExecuteNonQuery()


            '                res = True

            '                If (res) Then
            '                    'existen registros
            '                    Return True
            '                Else
            '                    'no existen regsitros
            '                    Return False
            '                End If

            '            Catch ex As Exception

            '                MsgBox(ex.Message & " " & "ERROR Proceso repetidos")
            '                Return False

            '            Finally

            '                conexion.CerrarConexion()

            '            End Try



        End Function

        Public Function CD_FaltantesOnyx()

            Dim query As String = "	INSERT INTO [dbo].[onyx]
                ([Version]
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
                ,[StatusDateTime]
                ,[BookingStatusCode]
                ,[ExtraInfoCode]
                ,[ConfNoRooms]
                ,[ConfNoNights]
                ,[ConfDateIn]
                ,[ConfDateOut]
                ,[ConfCommissionPercent]
                ,[ConfCostPrNight]
                ,[ConfFixedCommission]
                ,[ConfCurrency]
                ,[PaidStatus]
                ,[NTCommissionID]
                ,[NTHotelAccount]
                ,[BookingReferal]
                ,[PaymentJournal]
                ,[PaidCommission]
                ,[PaidCurrency]
                ,[PaymentPoint]
                ,[PaymentAccount]
                ,[PaymentDate]
                ,[OfficeIDBookingAgency]
                ,[Invoice_Or_Credit_Number]
                ,[TC_SavingCode]
                ,[TC_ATOLCode]
                ,[TC_VoucherType]
                ,[TC_Reference1]
                ,[TC_Reference2]
                ,[TC_Reference3]
                ,[TC_Reference4]
                ,[TC_HotelCode]
                ,[TC_AddressCode]
                ,[TC_DurationRackRate]
                ,[TC_DurationRackCurrency]
                ,[ConfCommissionVATPercent]
                ,[ConfCommissionVAT]
                ,[PaidCommissionBC]
                ,[PaidCommissionNTFee]
                ,[CommissionBookedCurrency]
                ,[HotelVAT-ID]
                ,[VAT-Amount-onFeeNTS]
                ,[VAT-Percentage-onFeeNTS]
                ,[ISVATCalculated]
                ,[PaidGrossCommissionAmount]
                ,[PaidGrossCommissionAmountCurrency]
                ,[AccountingAmount]
                ,[AccountingCurrency]
                ,[OnTacsDocument]
                ,[Fechadepago])

                SELECT

                [Version]
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
                ,[StatusDateTime]
                ,[BookingStatusCode]
                ,[ExtraInfoCode]
                ,[ConfNoRooms]
                ,[ConfNoNights]
                ,[ConfDateIn]
                ,[ConfDateOut]
                ,[ConfCommissionPercent]
                ,[ConfCostPrNight]
                ,[ConfFixedCommission]
                ,[ConfCurrency]
                ,[PaidStatus]
                ,[NTCommissionID]
                ,[NTHotelAccount]
                ,[BookingReferal]
                ,[PaymentJournal]
                ,[PaidCommission]
                ,[PaidCurrency]
                ,[PaymentPoint]
                ,[PaymentAccount]
                ,[PaymentDate]
                ,[OfficeIDBookingAgency]
                ,[Invoice_Or_Credit_Number]
                ,[TC_SavingCode]
                ,[TC_ATOLCode]
                ,[TC_VoucherType]
                ,[TC_Reference1]
                ,[TC_Reference2]
                ,[TC_Reference3]
                ,[TC_Reference4]
                ,[TC_HotelCode]
                ,[TC_AddressCode]
                ,[TC_DurationRackRate]
                ,[TC_DurationRackCurrency]
                ,[ConfCommissionVATPercent]
                ,[ConfCommissionVAT]
                ,[PaidCommissionBC]
                ,[PaidCommissionNTFee]
                ,[CommissionBookedCurrency]
                ,[HotelVAT-ID]
                ,[VAT-Amount-onFeeNTS]
                ,[VAT-Percentage-onFeeNTS]
                ,[ISVATCalculated]
                ,[PaidGrossCommissionAmount]
                ,[PaidGrossCommissionAmountCurrency]
                ,[AccountingAmount]
                ,[AccountingCurrency]
                ,[OnTacsDocument]
                ,[Fechadepago]

                FROM onyxTMP oTMP

                WHERE NOT EXISTS
                (   SELECT * FROM onyx O 
                    WHERE O.NTCommissionID = oTMP.NTCommissionID 
                    AND O.NTHotelAccount = oTMP.NTHotelAccount
                    AND O.PaidStatus = oTMP.PaidStatus 
                )"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

                res = True

                If (res) Then
                    'existen registros
                    Return True
                Else
                    'no existen regsitros
                    Return False
                End If

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 004 Onyx")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try




        End Function


        Public Function CD_cargaArchivoOnyx(onyx)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            Using SqlBulkCopy

                SqlBulkCopy.DestinationTableName = "dbo.onyx"

                SqlBulkCopy.ColumnMappings.Add("Version", "Version")
                SqlBulkCopy.ColumnMappings.Add("UniqueBookingID", "UniqueBookingID")
                SqlBulkCopy.ColumnMappings.Add("PNR", "PNR")
                SqlBulkCopy.ColumnMappings.Add("SequenceNo", "SequenceNo")
                SqlBulkCopy.ColumnMappings.Add("CreateDate", "CreateDate")
                SqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate")
                SqlBulkCopy.ColumnMappings.Add("LineNo", "[LineNo]")
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
                SqlBulkCopy.ColumnMappings.Add("StatusDateTime", "StatusDateTime")
                SqlBulkCopy.ColumnMappings.Add("BookingStatusCode", "BookingStatusCode")
                SqlBulkCopy.ColumnMappings.Add("ExtraInfoCode", "ExtraInfoCode")
                SqlBulkCopy.ColumnMappings.Add("ConfNoRooms", "ConfNoRooms")
                SqlBulkCopy.ColumnMappings.Add("ConfNoNights", "ConfNoNights")
                SqlBulkCopy.ColumnMappings.Add("ConfDateIn", "ConfDateIn")
                SqlBulkCopy.ColumnMappings.Add("ConfDateOut", "ConfDateOut")
                SqlBulkCopy.ColumnMappings.Add("ConfCommissionPercent", "ConfCommissionPercent")
                SqlBulkCopy.ColumnMappings.Add("ConfCostPrNight", "ConfCostPrNight")
                SqlBulkCopy.ColumnMappings.Add("ConfFixedCommission", "ConfFixedCommission")
                SqlBulkCopy.ColumnMappings.Add("ConfCurrency", "ConfCurrency")
                SqlBulkCopy.ColumnMappings.Add("PaidStatus", "PaidStatus")
                SqlBulkCopy.ColumnMappings.Add("NTCommissionID", "NTCommissionID")
                SqlBulkCopy.ColumnMappings.Add("NTHotelAccount", "NTHotelAccount")
                SqlBulkCopy.ColumnMappings.Add("BookingReferal", "BookingReferal")
                SqlBulkCopy.ColumnMappings.Add("PaymentJournal", "PaymentJournal")
                SqlBulkCopy.ColumnMappings.Add("PaidCommission", "PaidCommission")
                SqlBulkCopy.ColumnMappings.Add("PaidCurrency", "PaidCurrency")
                SqlBulkCopy.ColumnMappings.Add("PaymentPoint", "PaymentPoint")
                SqlBulkCopy.ColumnMappings.Add("PaymentAccount", "PaymentAccount")
                SqlBulkCopy.ColumnMappings.Add("PaymentDate", "PaymentDate")
                SqlBulkCopy.ColumnMappings.Add("OfficeIDBookingAgency", "OfficeIDBookingAgency")
                SqlBulkCopy.ColumnMappings.Add("Invoice_Or_Credit_Number", "Invoice_Or_Credit_Number")
                SqlBulkCopy.ColumnMappings.Add("TC_SavingCode", "TC_SavingCode")
                SqlBulkCopy.ColumnMappings.Add("TC_ATOLCode", "TC_ATOLCode")
                SqlBulkCopy.ColumnMappings.Add("TC_VoucherType", "TC_VoucherType")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference1", "TC_Reference1")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference2", "TC_Reference2")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference3", "TC_Reference3")
                SqlBulkCopy.ColumnMappings.Add("TC_Reference4", "TC_Reference4")
                SqlBulkCopy.ColumnMappings.Add("TC_HotelCode", "TC_HotelCode")
                SqlBulkCopy.ColumnMappings.Add("TC_AddressCode", "TC_AddressCode")
                SqlBulkCopy.ColumnMappings.Add("TC_DurationRackRate", "TC_DurationRackRate")
                SqlBulkCopy.ColumnMappings.Add("TC_DurationRackCurrency", "TC_DurationRackCurrency")
                SqlBulkCopy.ColumnMappings.Add("ConfCommissionVATPercent", "ConfCommissionVATPercent")
                SqlBulkCopy.ColumnMappings.Add("ConfCommissionVAT", "ConfCommissionVAT")
                SqlBulkCopy.ColumnMappings.Add("PaidCommissionBC", "PaidCommissionBC")
                SqlBulkCopy.ColumnMappings.Add("PaidCommissionNTFee", "PaidCommissionNTFee")
                SqlBulkCopy.ColumnMappings.Add("CommissionBookedCurrency", "CommissionBookedCurrency")
                SqlBulkCopy.ColumnMappings.Add("HotelVAT-ID", "[HotelVAT-ID]")
                SqlBulkCopy.ColumnMappings.Add("VAT-Amount-onFeeNTS", "[VAT-Amount-onFeeNTS]")
                SqlBulkCopy.ColumnMappings.Add("VAT-Percentage-onFeeNTS", "[VAT-Percentage-onFeeNTS]")
                SqlBulkCopy.ColumnMappings.Add("ISVATCalculated", "ISVATCalculated")
                SqlBulkCopy.ColumnMappings.Add("PaidGrossCommissionAmount", "PaidGrossCommissionAmount")
                SqlBulkCopy.ColumnMappings.Add("PaidGrossCommissionAmountCurrency", "PaidGrossCommissionAmountCurrency")
                SqlBulkCopy.ColumnMappings.Add("AccountingAmount", "AccountingAmount")
                SqlBulkCopy.ColumnMappings.Add("AccountingCurrency", "AccountingCurrency")
                SqlBulkCopy.ColumnMappings.Add("OnTacsDocument", "OnTacsDocument")


                Try
                    conexion.AbrirConexion()
                    SqlBulkCopy.BulkCopyTimeout = 60 * 5 ' 5 minutos
                    SqlBulkCopy.WriteToServer(onyx)
                    Return True
                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 005 Onyx")
                    Return False
                Finally

                    conexion.CerrarConexion()

                End Try

            End Using

        End Function


        'Public Sub CD_addFirtsNameLastName(id, fisrtName, lastName)
        Public Sub CD_addFirtsNameLastName(queries)


            'Dim query = "UPDATE onyx SET firstName = '" & fisrtName & "', lastName = '" & lastName & "' WHERE id = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queries
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 60 ' 5 minutos
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 006 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Sub CD_addFirtsNameLastNamePagadas(id, fisrtName, lastName)


            Dim query = "UPDATE onyxPagadas SET firstName = '" & fisrtName & "', lastName = '" & lastName & "' WHERE id = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 023 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Sub CD_addFirtsNameLastNameComisionesPendientePago(id, fisrtName, lastName)


            Dim query = "UPDATE onyxComisionesPendientePago SET firstName = '" & fisrtName & "', lastName = '" & lastName & "' WHERE id = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & "ERROR 02332 Onyx")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub


        Public Sub CD_changePaidCommission(tc)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query = "DECLARE @idTemp INT 
DECLARE @PaidCommissionTemp FLOAT
DECLARE @PaidStatus VARCHAR(10)
DECLARE @mesproveedor VARCHAR(100)
DECLARE @TC FLOAT
SET @TC = " & tc & "
SET @mesproveedor = '" & fechaProveedor & "'
 
DECLARE C_A CURSOR

FOR
	SELECT
	id,
	PaidCommission,
	PaidStatus
	FROM onyx
	WHERE mesProveedor = @mesproveedor
	AND estatusEliminado IS NULL;

OPEN C_A;
 
FETCH NEXT FROM C_A INTO 

 @idTemp,
 @PaidCommissionTemp,
 @PaidStatus;
 
WHILE @@FETCH_STATUS = 0
    BEGIN

		IF (@PaidCommissionTemp IS NULL OR @PaidCommissionTemp = '')  BEGIN
			--PRINT @PaidStatus
			UPDATE onyx SET observaciones = @PaidStatus WHERE id = @idTemp
		END
		ELSE BEGIN
			--PRINT @TC
			 UPDATE onyx SET
			 PaidCommissionMXN = (@PaidCommissionTemp * @TC),
			 TC = @TC,
			 FechaCambioTC = GETDATE()
			 WHERE id = @idTemp
		END

        FETCH NEXT FROM C_A INTO @idTemp,@PaidCommissionTemp,@PaidStatus;

    END;
 
CLOSE C_A;
 
DEALLOCATE C_A;"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 058 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_changePaidCommissionPagadas(tc)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query = "DECLARE @idTemp INT 
DECLARE @PaidCommissionTemp FLOAT
DECLARE @PaidStatus VARCHAR(10)
DECLARE @mesproveedor VARCHAR(100)
DECLARE @TC FLOAT
SET @TC = " & tc & "
SET @mesproveedor = '" & fechaProveedor & "'
 
DECLARE C_A CURSOR

FOR
	SELECT
	id,
	PaidCommission,
	PaidStatus
	FROM onyxPagadas
	WHERE mesProveedor = @mesproveedor
	AND estatusEliminado IS NULL;

OPEN C_A;
 
FETCH NEXT FROM C_A INTO 

 @idTemp,
 @PaidCommissionTemp,
 @PaidStatus;
 
WHILE @@FETCH_STATUS = 0
    BEGIN

		IF (@PaidCommissionTemp IS NULL OR @PaidCommissionTemp = '')  BEGIN
			--PRINT @PaidStatus
			UPDATE onyxPagadas SET observaciones = @PaidStatus WHERE id = @idTemp
		END
		ELSE BEGIN
			--PRINT @TC
			 UPDATE onyxPagadas SET
			 PaidCommissionMXN = (@PaidCommissionTemp * @TC),
			 TC = @TC,
			 FechaCambioTC = GETDATE(),
             observaciones = @PaidStatus
			 WHERE id = @idTemp
		END

        FETCH NEXT FROM C_A INTO @idTemp,@PaidCommissionTemp,@PaidStatus;

    END;
 
CLOSE C_A;
 
DEALLOCATE C_A;"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 059 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_changePaidCommissionComisionesPendientePago(id, PaidCommission, tc)

            Dim query = "UPDATE onyxComisionesPendientePago SET PaidCommissionMXN = '" & PaidCommission & "',TC = " & tc & ",FechaCambioTC = GETDATE() WHERE id = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 059.2 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub




        Public Sub CD_agregarObservacionComisionesPendientePago(id, PaidStatus)

            Dim query = "UPDATE onyxComisionesPendientePago SET observaciones = '" & PaidStatus & "' WHERE id = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 086 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_changePaidCurrency(id, PaidCurrency)

            Dim query = "UPDATE onyx SET PaidCurrency = '" & PaidCurrency & "' WHERE id = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 059 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub


        Public Function CD_addTotalReserva()

            Dim procedure As String = "totalDeLaReservaPosadas"

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

                MsgBox(ex.Message & " " & "ERROR 007 Onyx")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try



        End Function

        Public Function CD_addNoNoches()

            Dim procedure As String = "noNochesPosadas"

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

                MsgBox(ex.Message & " " & "ERROR 008 Onyx")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try

        End Function



        Public Function CD_SelectOnyx() As DataTable


            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

            'Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"


            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''



            Dim tablaOnyx As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM onyx" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                leer = comando.ExecuteReader()
                tablaOnyx.Load(leer)

                Return tablaOnyx

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 Onyx")
                Return tablaOnyx

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectOnyxFechaProveedor() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim tablaOnyx As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM onyx " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyx.Load(leer)

                Return tablaOnyx

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0098 Onyx")
                Return tablaOnyx

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Sub CD_quitarGuionComisionesPendientePago()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "UPDATE onyxComisionesPendientePago SET AgentRef3 = LEFT(AgentRef3, CHARINDEX('-', AgentRef3) - 1)
            WHERE AgentRef3 like'%-%'" & queryFechaProveedor & queryEstatusEliminado


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 166322 ONYX")

            Finally

                conexion.CerrarConexion()

            End Try

        End Sub



        Public Sub CD_quitarGuion()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "UPDATE onyx SET AgentRef3 = LEFT(AgentRef3, CHARINDEX('-', AgentRef3) - 1)
            WHERE AgentRef3 like'%-%'" & queryFechaProveedor & queryEstatusEliminado


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 166 ONYX")

            Finally

                conexion.CerrarConexion()

            End Try



        End Sub



        Public Function CD_SelectOnyxPagadas() As DataTable

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "



            Dim tablaOnyxPagadas As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM onyxPagadas" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxPagadas.Load(leer)

                Return tablaOnyxPagadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00323 Onyx")
                Return tablaOnyxPagadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectOnyxComisionesPendientePago() As DataTable

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            '''''''''''''''''''''''''''''''''''''''''' Eliminar Repetidos '''''''''''''''''''''''''''''''''''''''

            Dim tablaOnyxPagadas As DataTable = New DataTable()


            Dim query As String = "SELECT * FROM onyxComisionesPendientePago" '& queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxPagadas.Load(leer)

                Return tablaOnyxPagadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00323 Onyx")
                Return tablaOnyxPagadas

            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_SelectOnyxComisionesPendientePagoTC() As DataTable

            'Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            'Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"


            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            '''''''''''''''''''''''''''''''''''''''''' Eliminar Repetidos '''''''''''''''''''''''''''''''''''''''

            Dim tablaOnyxPagadas As DataTable = New DataTable()


            Dim query As String = "SELECT * FROM onyxComisionesPendientePago" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxPagadas.Load(leer)

                Return tablaOnyxPagadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00323.34 Onyx")
                Return tablaOnyxPagadas

            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_SelectOnyxPagadasTC() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

            Dim tablaOnyxPagadas As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM onyxPagadas" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxPagadas.Load(leer)

                Return tablaOnyxPagadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00323 Onyx")
                Return tablaOnyxPagadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function



        Public Function CD_SelectOnyxPagadasFechaProveedor() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim tablaOnyxPagadas As DataTable = New DataTable()

            'Dim procedure As String = "SelectPosadas"
            Dim query As String = "SELECT * FROM onyxPagadas" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxPagadas.Load(leer)

                Return tablaOnyxPagadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00323 Onyx")
                Return tablaOnyxPagadas

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectOnyxObservaciones()

            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin
            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "



            Dim tablaOnyxObservaciones As DataTable = New DataTable()


            Dim query As String = "SELECT * FROM onyxObservaciones" & queryFechaProveedor & queryEstatusEliminado & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                'comando.CommandType = CommandType.StoredProcedure
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxObservaciones.Load(leer)

                Return tablaOnyxObservaciones

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00324 Onyx")
                Return tablaOnyxObservaciones

            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Sub CD_addtrxconcatenada()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query = "UPDATE onyx SET [No.trxconcatenada] = CONCAT(PaidCommission,' ',PaidCurrency)" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 010 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_addtrxconcatenadaPagadasv2()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query = "UPDATE onyxPagadas SET [No.trxconcatenada] = CONCAT(PaidCommission,' ',PaidCurrency)" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 010 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_addtrxconcatenadaPagadas()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query = "UPDATE onyxPagadas SET [No.trxconcatenada] = CONCAT(PaidCommission,' ',PaidCurrency)" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0134 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Function CD_SelectOnyxPagadasConformationNORepetidos() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


            Dim query As String = "IF NOT OBJECT_ID('TEMPDB..#tblA') IS NULL  DROP TABLE #tblA
IF NOT OBJECT_ID('TEMPDB..#tblC') IS NULL  DROP TABLE #tblC

DECLARE @mesProveedor DATE
SET @mesProveedor = '" & fechaProveedor & "'

CREATE TABLE #tblA(ConformationNO VARCHAR(100) COLLATE Latin1_General_BIN,cuantosRepetidos INT)
CREATE CLUSTERED INDEX ix_tblA ON #tblA (ConformationNO,cuantosRepetidos)

CREATE TABLE #tblC(ConformationNO VARCHAR(100) COLLATE Latin1_General_BIN,sumaPaidCommission DECIMAL(18,3))
CREATE CLUSTERED INDEX ix_tblC ON #tblC (ConformationNO,sumaPaidCommission)


INSERT INTO #tblA
SELECT ConformationNO COLLATE Latin1_General_BIN,count(*) AS cuantosRepetidos FROM onyxPagadas  WHERE mesProveedor =  @mesProveedor
AND ConformationNo IS NOT NULL
GROUP BY ConformationNO 
HAVING COUNT(*)>1;

INSERT INTO onyxRepetidoConformationNO           ([Version]
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
           ,[StatusDateTime]
           ,[BookingStatusCode]
           ,[ExtraInfoCode]
           ,[ConfNoRooms]
           ,[ConfNoNights]
           ,[ConfDateIn]
           ,[ConfDateOut]
           ,[ConfCommissionPercent]
           ,[ConfCostPrNight]
           ,[ConfFixedCommission]
           ,[ConfCurrency]
           ,[PaidStatus]
           ,[NTCommissionID]
           ,[NTHotelAccount]
           ,[BookingReferal]
           ,[PaymentJournal]
           ,[PaidCommission]
           ,[PaidCurrency]
           ,[PaymentPoint]
           ,[PaymentAccount]
           ,[PaymentDate]
           ,[OfficeIDBookingAgency]
           ,[Invoice_Or_Credit_Number]
           ,[TC_SavingCode]
           ,[TC_ATOLCode]
           ,[TC_VoucherType]
           ,[TC_Reference1]
           ,[TC_Reference2]
           ,[TC_Reference3]
           ,[TC_Reference4]
           ,[TC_HotelCode]
           ,[TC_AddressCode]
           ,[TC_DurationRackRate]
           ,[TC_DurationRackCurrency]
           ,[ConfCommissionVATPercent]
           ,[ConfCommissionVAT]
           ,[PaidCommissionBC]
           ,[PaidCommissionNTFee]
           ,[CommissionBookedCurrency]
           ,[HotelVAT-ID]
           ,[VAT-Amount-onFeeNTS]
           ,[VAT-Percentage-onFeeNTS]
           ,[ISVATCalculated]
           ,[PaidGrossCommissionAmount]
           ,[PaidGrossCommissionAmountCurrency]
           ,[AccountingAmount]
           ,[AccountingCurrency]
           ,[OnTacsDocument]
           ,[Fechadepago]
           ,[mesProveedor])

SELECT

	b.[Version]
	,b.UniqueBookingID 
	,b.PNR
	,b.SequenceNo 
	,b.CreateDate
	,b.ModifyDate 
	,b.[LineNo] 
	,b.AgencyIDType 
	,b.AgencyID
	,b.BookingAgent 
	,b.GuestName
	,b.CorporateID 
	,b.AgentRef1 
	,b.AgentRef2 
	,b.AgentRef3 
	,b.NumberOfRooms 
	,b.NumberOfNights 
	,b.DateIn 
	,b.DateOut 
	,b.CommissionPercent 
	,b.CostPrNight 
	,b.FixedCommission 
	,b.Currency 
	,b.RateCode 
	,b.AccommodationType 
	,b.ConformationNo
	,b.HotelPropertyID
	,b.HotelChainID 
	,b.HotelName 
	,b.Address1
	,b.Address2 
	,b.City 
	,b.[State] 
	,b.Zip 
	,b.AirportCityCode
	,b.Phone 
	,b.Fax 
	,b.CountryCode 
	,b.StatusDateTime 
	,b.BookingStatusCode 
	,b.ExtraInfoCode 
	,b.ConfNoRooms 
	,b.ConfNoNights 
	,b.ConfDateIn 
	,b.ConfDateOut 
	,b.ConfCommissionPercent 
	,b.ConfCostPrNight 
	,b.ConfFixedCommission 
	,b.ConfCurrency 
	,b.PaidStatus 
	,b.NTCommissionID 
	,b.NTHotelAccount 
	,b.BookingReferal 
	,b.PaymentJournal
	,b.PaidCommission 
	,b.PaidCurrency 
	,b.PaymentPoint 
	,b.PaymentAccount
	,b.PaymentDate 
	,b.OfficeIDBookingAgency 
	,b.Invoice_Or_Credit_Number 
	,b.TC_SavingCode 
	,b.TC_ATOLCode 
	,b.TC_VoucherType 
	,b.TC_Reference1 
	,b.TC_Reference2 
	,b.TC_Reference3 
	,b.TC_Reference4 
	,b.TC_HotelCode 
	,b.TC_AddressCode 
	,b.TC_DurationRackRate 
	,b.TC_DurationRackCurrency 
	,b.ConfCommissionVATPercent
	,b.ConfCommissionVAT 
	,b.PaidCommissionBC 
	,b.PaidCommissionNTFee 
	,b.CommissionBookedCurrency 
	,b.[HotelVAT-ID] 
	,b.[VAT-Amount-onFeeNTS]
	,b.[VAT-Percentage-onFeeNTS] 
	,b.ISVATCalculated 
	,b.PaidGrossCommissionAmount
	,b.PaidGrossCommissionAmountCurrency 
	,b.AccountingAmount
	,b.AccountingCurrency 
	,b.OnTacsDocument 
	,b.Fechadepago
	,@mesProveedor 

FROM #tblA a  INNER JOIN   onyxPagadas b ON a.ConformationNO = b.ConformationNO COLLATE Latin1_General_BIN
AND b.mesProveedor = @mesProveedor
AND NOT EXISTS
(

SELECT ConformationNO FROM onyxRepetidoConformationNO OP 
WHERE OP.ConformationNO COLLATE Latin1_General_BIN = a.ConformationNO
AND OP.mesProveedor = @mesProveedor
 
)


INSERT INTO #tblC
SELECT a.ConformationNO,sum(cast(b.PaidCommission as decimal(18,3))) AS sumaPaidCommission
FROM #tblA a INNER JOIN onyxPagadas b ON a.ConformationNO = b.ConformationNO COLLATE Latin1_General_BIN
AND b.mesProveedor = @mesProveedor
GROUP BY  a.ConformationNO

--Update Monto
UPDATE onyxPagadas SET onyxPagadas.PaidCommission  = temp.sumaPaidCommission FROM onyxPagadas op
INNER JOIN #tblC temp ON temp.ConformationNO  = op.ConformationNo COLLATE Latin1_General_BIN
AND op.mesProveedor = @mesProveedor; 

--Delete Repetidos
--DELETE FROM onyxPagadas WHERE ConformationNo COLLATE Latin1_General_BIN IN (SELECT ConformationNo FROM #tblC) 
--AND mesProveedor = @mesProveedor

WITH CTE AS 
(SELECT ConformationNo,NUMERO_REPETIDOS=ROW_NUMBER() OVER(PARTITION BY ConformationNo ORDER BY id) 
 FROM onyxPagadas WHERE mesProveedor = @mesProveedor )
--SELECT * FROM CTE WHERE NUMERO_REPETIDOS > 1
DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1 AND ConformationNo IS NOT NULL"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR R50 Onyx")

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        'Public Function CD_SelectOnyxPagadasConformationNORepetidos() As DataTable

        '    Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        '    Dim ConformationNO As String
        '    Dim cuantosRepetidos As Int64

        '    Dim query As String = "SELECT ConformationNO,count(*) AS cuantosRepetidos FROM onyxPagadas  WHERE mesProveedor = '2019-09-05'  
        '    GROUP BY ConformationNO 
        '    HAVING COUNT(*)>1;"

        '    Try
        '        comando.Connection = conexion.AbrirConexion()
        '        comando.CommandText = query
        '        comando.CommandType = CommandType.Text
        '        leer = comando.ExecuteReader()


        '        While leer.Read()

        '            ConformationNO = vbEmpty
        '            cuantosRepetidos = vbEmpty

        '            ConformationNO = Convert.ToString(If(TypeOf leer("ConformationNO") Is DBNull, 0, leer("ConformationNO")))
        '            cuantosRepetidos = Convert.ToInt64(If(TypeOf leer("cuantosRepetidos") Is DBNull, 0, leer("cuantosRepetidos")))





        '        End While




        '    Catch ex As Exception

        '        MsgBox(ex.Message & " " & " ERROR R50 Onyx")

        '    Finally
        '        conexion.CerrarConexion()
        '    End Try


        'End Function

        Public Sub CD_InsertarOnyxPagadas()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND o.mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND o.estatusEliminado IS NULL"


            Dim query = "INSERT INTO [dbo].[onyxPagadas]
            ([Version]
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
            ,[StatusDateTime]
            ,[BookingStatusCode]
            ,[ExtraInfoCode]
            ,[ConfNoRooms]
            ,[ConfNoNights]
            ,[ConfDateIn]
            ,[ConfDateOut]
            ,[ConfCommissionPercent]
            ,[ConfCostPrNight]
            ,[ConfFixedCommission]
            ,[ConfCurrency]
            ,[PaidStatus]
            ,[NTCommissionID]
            ,[NTHotelAccount]
            ,[BookingReferal]
            ,[PaymentJournal]
            ,[PaidCommission]
            ,[PaidCurrency]
            ,[PaymentPoint]
            ,[PaymentAccount]
            ,[PaymentDate]
            ,[OfficeIDBookingAgency]
            ,[Invoice_Or_Credit_Number]
            ,[TC_SavingCode]
            ,[TC_ATOLCode]
            ,[TC_VoucherType]
            ,[TC_Reference1]
            ,[TC_Reference2]
            ,[TC_Reference3]
            ,[TC_Reference4]
            ,[TC_HotelCode]
            ,[TC_AddressCode]
            ,[TC_DurationRackRate]
            ,[TC_DurationRackCurrency]
            ,[ConfCommissionVATPercent]
            ,[ConfCommissionVAT]
            ,[PaidCommissionBC]
            ,[PaidCommissionNTFee]
            ,[CommissionBookedCurrency]
            ,[HotelVAT-ID]
            ,[VAT-Amount-onFeeNTS]
            ,[VAT-Percentage-onFeeNTS]
            ,[ISVATCalculated]
            ,[PaidGrossCommissionAmount]
            ,[PaidGrossCommissionAmountCurrency]
            ,[AccountingAmount]
            ,[AccountingCurrency]
            ,[OnTacsDocument]
            ,[Fechadepago]
            ,[mesProveedor]
            ,[estatusEliminado]
			,[firstName]
			,[lastName]
			,[No.trxconcatenada]
            )

            SELECT
            [Version]
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
            ,[StatusDateTime]
            ,[BookingStatusCode]
            ,[ExtraInfoCode]
            ,[ConfNoRooms]
            ,[ConfNoNights]
            ,[ConfDateIn]
            ,[ConfDateOut]
            ,[ConfCommissionPercent]
            ,[ConfCostPrNight]
            ,[ConfFixedCommission]
            ,[ConfCurrency]
            ,[PaidStatus]
            ,[NTCommissionID]
            ,[NTHotelAccount]
            ,[BookingReferal]
            ,[PaymentJournal]
            ,[PaidCommission]
            ,[PaidCurrency]
            ,[PaymentPoint]
            ,[PaymentAccount]
            ,[PaymentDate]
            ,[OfficeIDBookingAgency]
            ,[Invoice_Or_Credit_Number]
            ,[TC_SavingCode]
            ,[TC_ATOLCode]
            ,[TC_VoucherType]
            ,[TC_Reference1]
            ,[TC_Reference2]
            ,[TC_Reference3]
            ,[TC_Reference4]
            ,[TC_HotelCode]
            ,[TC_AddressCode]
            ,[TC_DurationRackRate]
            ,[TC_DurationRackCurrency]
            ,[ConfCommissionVATPercent]
            ,[ConfCommissionVAT]
            ,[PaidCommissionBC]
            ,[PaidCommissionNTFee]
            ,[CommissionBookedCurrency]
            ,[HotelVAT-ID]
            ,[VAT-Amount-onFeeNTS]
            ,[VAT-Percentage-onFeeNTS]
            ,[ISVATCalculated]
            ,[PaidGrossCommissionAmount]
            ,[PaidGrossCommissionAmountCurrency]
            ,[AccountingAmount]
            ,[AccountingCurrency]
            ,[OnTacsDocument]
            ,[Fechadepago]
            ,[mesProveedor]
            ,[estatusEliminado]
			,[firstName]
			,[lastName]
			,[No.trxconcatenada]

            FROM onyx o

            WHERE NOT EXISTS
            (SELECT * FROM onyxPagadas OP 
            WHERE OP.NTCommissionID = o.NTCommissionID 
            AND OP.NTHotelAccount = o.NTHotelAccount
            AND OP.PaidStatus = o.PaidStatus
            )

            AND (o.PaidStatus = 'PEP' OR  o.PaidStatus ='PNT' OR o.PaidStatus ='PTA') AND o.PaidCommission is not null " & queryFechaProveedor & queryEstatusEliminado


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 011 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Sub CD_InsertarOnyxComisionesPendientePago()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim queryFechaProveedor As String = " AND o.mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND o.estatusEliminado IS NULL"

            Dim query = "INSERT INTO [dbo].[onyxComisionesPendientePago]
            ([Version]
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
            ,[StatusDateTime]
            ,[BookingStatusCode]
            ,[ExtraInfoCode]
            ,[ConfNoRooms]
            ,[ConfNoNights]
            ,[ConfDateIn]
            ,[ConfDateOut]
            ,[ConfCommissionPercent]
            ,[ConfCostPrNight]
            ,[ConfFixedCommission]
            ,[ConfCurrency]
            ,[PaidStatus]
            ,[NTCommissionID]
            ,[NTHotelAccount]
            ,[BookingReferal]
            ,[PaymentJournal]
            ,[PaidCommission]
            ,[PaidCurrency]
            ,[PaymentPoint]
            ,[PaymentAccount]
            ,[PaymentDate]
            ,[OfficeIDBookingAgency]
            ,[Invoice_Or_Credit_Number]
            ,[TC_SavingCode]
            ,[TC_ATOLCode]
            ,[TC_VoucherType]
            ,[TC_Reference1]
            ,[TC_Reference2]
            ,[TC_Reference3]
            ,[TC_Reference4]
            ,[TC_HotelCode]
            ,[TC_AddressCode]
            ,[TC_DurationRackRate]
            ,[TC_DurationRackCurrency]
            ,[ConfCommissionVATPercent]
            ,[ConfCommissionVAT]
            ,[PaidCommissionBC]
            ,[PaidCommissionNTFee]
            ,[CommissionBookedCurrency]
            ,[HotelVAT-ID]
            ,[VAT-Amount-onFeeNTS]
            ,[VAT-Percentage-onFeeNTS]
            ,[ISVATCalculated]
            ,[PaidGrossCommissionAmount]
            ,[PaidGrossCommissionAmountCurrency]
            ,[AccountingAmount]
            ,[AccountingCurrency]
            ,[OnTacsDocument]
            ,[Fechadepago]
            ,[mesProveedor]
            ,[estatusEliminado]
            ,[fechaConfPago]
			,[firstName]
			,[lastName]
			,[No.trxconcatenada]
            )

            SELECT
            [Version]
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
            ,[StatusDateTime]
            ,[BookingStatusCode]
            ,[ExtraInfoCode]
            ,[ConfNoRooms]
            ,[ConfNoNights]
            ,[ConfDateIn]
            ,[ConfDateOut]
            ,[ConfCommissionPercent]
            ,[ConfCostPrNight]
            ,[ConfFixedCommission]
            ,[ConfCurrency]
            ,[PaidStatus]
            ,[NTCommissionID]
            ,[NTHotelAccount]
            ,[BookingReferal]
            ,[PaymentJournal]
            ,[PaidCommission]
            ,[PaidCurrency]
            ,[PaymentPoint]
            ,[PaymentAccount]
            ,[PaymentDate]
            ,[OfficeIDBookingAgency]
            ,[Invoice_Or_Credit_Number]
            ,[TC_SavingCode]
            ,[TC_ATOLCode]
            ,[TC_VoucherType]
            ,[TC_Reference1]
            ,[TC_Reference2]
            ,[TC_Reference3]
            ,[TC_Reference4]
            ,[TC_HotelCode]
            ,[TC_AddressCode]
            ,[TC_DurationRackRate]
            ,[TC_DurationRackCurrency]
            ,[ConfCommissionVATPercent]
            ,[ConfCommissionVAT]
            ,[PaidCommissionBC]
            ,[PaidCommissionNTFee]
            ,[CommissionBookedCurrency]
            ,[HotelVAT-ID]
            ,[VAT-Amount-onFeeNTS]
            ,[VAT-Percentage-onFeeNTS]
            ,[ISVATCalculated]
            ,[PaidGrossCommissionAmount]
            ,[PaidGrossCommissionAmountCurrency]
            ,[AccountingAmount]
            ,[AccountingCurrency]
            ,[OnTacsDocument]
            ,[Fechadepago]
            ,[mesProveedor]
            ,[estatusEliminado]
            ,'" & fechaProveedor & "'
			,[firstName]
			,[lastName]
			,[No.trxconcatenada]


            FROM onyx o

            WHERE NOT EXISTS
            ( SELECT * FROM onyxComisionesPendientePago OP 
            WHERE OP.NTCommissionID = o.NTCommissionID 
            AND OP.NTHotelAccount = o.NTHotelAccount 
            AND OP.PaidStatus = o.PaidStatus
            )

            AND (o.PaidStatus = 'PEP') " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & "ERROR 01165 Onyx")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub


        Public Sub CD_InsertarOnyxObservaciones()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND o.mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND o.estatusEliminado IS NULL"

            Dim query As String = "INSERT INTO [dbo].[onyxObservaciones]
            ([Version]
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
            ,[StatusDateTime]
            ,[BookingStatusCode]
            ,[ExtraInfoCode]
            ,[ConfNoRooms]
            ,[ConfNoNights]
            ,[ConfDateIn]
            ,[ConfDateOut]
            ,[ConfCommissionPercent]
            ,[ConfCostPrNight]
            ,[ConfFixedCommission]
            ,[ConfCurrency]
            ,[PaidStatus]
            ,[NTCommissionID]
            ,[NTHotelAccount]
            ,[BookingReferal]
            ,[PaymentJournal]
            ,[PaidCommission]
            ,[PaidCurrency]
            ,[PaymentPoint]
            ,[PaymentAccount]
            ,[PaymentDate]
            ,[OfficeIDBookingAgency]
            ,[Invoice_Or_Credit_Number]
            ,[TC_SavingCode]
            ,[TC_ATOLCode]
            ,[TC_VoucherType]
            ,[TC_Reference1]
            ,[TC_Reference2]
            ,[TC_Reference3]
            ,[TC_Reference4]
            ,[TC_HotelCode]
            ,[TC_AddressCode]
            ,[TC_DurationRackRate]
            ,[TC_DurationRackCurrency]
            ,[ConfCommissionVATPercent]
            ,[ConfCommissionVAT]
            ,[PaidCommissionBC]
            ,[PaidCommissionNTFee]
            ,[CommissionBookedCurrency]
            ,[HotelVAT-ID]
            ,[VAT-Amount-onFeeNTS]
            ,[VAT-Percentage-onFeeNTS]
            ,[ISVATCalculated]
            ,[PaidGrossCommissionAmount]
            ,[PaidGrossCommissionAmountCurrency]
            ,[AccountingAmount]
            ,[AccountingCurrency]
            ,[OnTacsDocument]
            ,[Fechadepago]
            ,[mesProveedor]
            ,[estatusEliminado]
			,[firstName]
			,[lastName]
			,[No.trxconcatenada]
)

            SELECT
            [Version]
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
            ,[StatusDateTime]
            ,[BookingStatusCode]
            ,[ExtraInfoCode]
            ,[ConfNoRooms]
            ,[ConfNoNights]
            ,[ConfDateIn]
            ,[ConfDateOut]
            ,[ConfCommissionPercent]
            ,[ConfCostPrNight]
            ,[ConfFixedCommission]
            ,[ConfCurrency]
            ,[PaidStatus]
            ,[NTCommissionID]
            ,[NTHotelAccount]
            ,[BookingReferal]
            ,[PaymentJournal]
            ,[PaidCommission]
            ,[PaidCurrency]
            ,[PaymentPoint]
            ,[PaymentAccount]
            ,[PaymentDate]
            ,[OfficeIDBookingAgency]
            ,[Invoice_Or_Credit_Number]
            ,[TC_SavingCode]
            ,[TC_ATOLCode]
            ,[TC_VoucherType]
            ,[TC_Reference1]
            ,[TC_Reference2]
            ,[TC_Reference3]
            ,[TC_Reference4]
            ,[TC_HotelCode]
            ,[TC_AddressCode]
            ,[TC_DurationRackRate]
            ,[TC_DurationRackCurrency]
            ,[ConfCommissionVATPercent]
            ,[ConfCommissionVAT]
            ,[PaidCommissionBC]
            ,[PaidCommissionNTFee]
            ,[CommissionBookedCurrency]
            ,[HotelVAT-ID]
            ,[VAT-Amount-onFeeNTS]
            ,[VAT-Percentage-onFeeNTS]
            ,[ISVATCalculated]
            ,[PaidGrossCommissionAmount]
            ,[PaidGrossCommissionAmountCurrency]
            ,[AccountingAmount]
            ,[AccountingCurrency]
            ,[OnTacsDocument]
            ,[Fechadepago]
            ,[mesProveedor]
            ,[estatusEliminado]
			,[firstName]
			,[lastName]
			,[No.trxconcatenada]

            FROM onyx o

            WHERE NOT EXISTS
            ( SELECT * FROM onyxObservaciones OP 
            WHERE OP.NTCommissionID = o.NTCommissionID 
            AND OP.NTHotelAccount = o.NTHotelAccount 
            AND OP.PaidStatus = o.PaidStatus
            )

            AND (o.PaidStatus ='PNT' OR o.PaidStatus ='PTA' OR o.PaidStatus ='NPD' ) AND PaidCommission IS NULL " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 012 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Function CD_agregarObservacionTblObservaciones(id, observacion)

            Dim query As String = " UPDATE onyxObservaciones SET observaciones = '" & observacion & "' WHERE id  = " & id & " "

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

                If (res) Then

                    Return True
                Else

                    Return False
                End If

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0086 Onyx")
                Return False

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Sub CD_replaceQuotesComisionesPendientePago()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim doubleQuote As String = Chr(34)

            Dim querySingle = "UPDATE onyxComisionesPendientePago SET guestName = REPLACE(guestName,'''','') " & queryFechaProveedor & queryEstatusEliminado
            Dim queryDouble = "UPDATE onyxComisionesPendientePago SET guestName = REPLACE(guestName,'" & doubleQuote & "','') " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = querySingle
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 01253464 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryDouble
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 01253465 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try



        End Sub


        Public Sub CD_replaceQuotesPagadas()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim doubleQuote As String = Chr(34)

            Dim querySingle = "UPDATE onyxPagadas SET guestName = REPLACE(guestName,'''','') " & queryFechaProveedor & queryEstatusEliminado
            Dim queryDouble = "UPDATE onyxPagadas SET guestName = REPLACE(guestName,'" & doubleQuote & "','') " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = querySingle
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 01253 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryDouble
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 017253 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try



        End Sub


        Public Sub CD_replaceQuotes()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            Dim doubleQuote As String = Chr(34)

            Dim querySingle = "UPDATE onyx SET guestName = REPLACE(guestName,'''','') " & queryFechaProveedor & queryEstatusEliminado
            Dim queryDouble = "UPDATE onyx SET guestName = REPLACE(guestName,'" & doubleQuote & "','') " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = querySingle
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 01253 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try


            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryDouble
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 017253 Onyx")


            Finally

                conexion.CerrarConexion()

            End Try



        End Sub


        Public Function CD_ConsultaAcentos()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim tabla As DataTable = New DataTable()

            Dim query As String = "SELECT id, firstName, lastName FROM onyx " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)

                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 123 onyxPagadas")
                Return tabla

            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        'Public Sub CD_QuitarAcentoFirstName(id, firstName)
        Public Sub CD_QuitarAcentoFirstName(queryA)

            'Dim query As String = "UPDATE onyx SET firstName = '" & firstName & "' WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0086 onyxPagadas")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        'Public Sub CD_QuitarAcentoLastName(id, lastNameB)
        Public Sub CD_QuitarAcentoLastName(queryB)

            'Dim query As String = "UPDATE onyx SET lastName = '" & lastNameB & "' WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0087 onyxPagadas")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub



        Public Function CD_SelectSinConciliar() As DataTable


            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM onyxPagadas WHERE CondicionOKAuto IS  NULL AND estatusConciliado IS  NULL AND idBDBCD IS  NULL" & queryFechaProveedor

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)
                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0754 Onyx")
                Return tablaPosadas

            Finally
                conexion.CerrarConexion()
            End Try


        End Function


        Public Sub CD_EliminarOnyx(id)

            'Dim query As String = "DELETE FROM onyxPagadas WHERE id = " & id & ""
            Dim query As String = "UPDATE onyxPagadas SET estatusEliminado = 1 WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 101 onyxPagadas")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Function CD_ConciliarByID(id, idBDBCD, lastQuery)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE onyxPagadas SET estatusConciliado = 1 WHERE id  = " & id & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1265 onyxPagadas")
            Finally
                conexion.CerrarConexion()
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryB As String = "UPDATE BDBCD SET 
            estatusConciliado = 1,
            proveedor = 'onyx',
            mesProveedor = '" & fechaProveedor & "'
            WHERE id  = " & idBDBCD & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1266 onyxPagadas")
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
            CASE WHEN proveedor.PaidCommissionMXN = '' OR proveedor.PaidCommissionMXN IS NULL  THEN proveedor.observaciones ELSE proveedor.PaidCommissionMXN END AS Comision,
            'Onyx' AS Operador,
            proveedor.PaidCurrency AS Moneda,

CASE
WHEN PaidCommissionMXN is not null AND CAST(PaidCommissionMXN AS DECIMAL(18, 3)) > 0 THEN
(CAST(PaidCommissionMXN AS DECIMAL(18, 3)) * 100) / ConfCommissionPercent 
END AS CostoTotalDeLaReserva,


            proveedor.ConfNoNights AS Noches,
            proveedor.PaidCommission As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Adicional' As tipoConciliacion
            FROM  onyxPagadas proveedor
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
                MsgBox(ex.Message & " " & " ERROR 1267 onyx")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Function CD_ObtenerUltimoId()

            Dim lastId As Int64

            Dim queryA As String = "SELECT MAX(id) FROM onyx"

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
                MsgBox(ex.Message & " " & " ERROR 1014 onyx")
            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Sub CD_agregarMesProveedor(ByVal lastId As Int64)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE onyx SET mesProveedor = '" & fechaProveedor & "' WHERE id > " & lastId & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1013 onyx")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub

        Public Function CD_SeleccionIDPendientesOnyx()

            Dim tabla As DataTable = New DataTable()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = " SELECT id FROM onyxPagadas
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

        Public Function CD_DatosOnyxRepetidosMesProveedor()

            Dim MesProveedor = ClsGlobales.FechaPagoproveedor

            Dim tablaOnyxRepetidos As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM onyxRepetido WHERE mesProveedorActual = '" & MesProveedor & "'"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxRepetidos.Load(leer)
                Return tablaOnyxRepetidos

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 10924 Onyx")
                Return tablaOnyxRepetidos

            Finally
                conexion.CerrarConexion()
            End Try



        End Function



        Public Function CD_consultaOnyxPaidCommisionMesProveedor()

            Dim MesProveedor = ClsGlobales.FechaPagoproveedor

            Dim tablaOnyxRepetidos As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM onyxRepetidoConformationNO WHERE mesProveedor = '" & MesProveedor & "' ORDER BY ConformationNO"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaOnyxRepetidos.Load(leer)
                Return tablaOnyxRepetidos

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 10924 Onyx")
                Return tablaOnyxRepetidos

            Finally
                conexion.CerrarConexion()
            End Try



        End Function



    End Class

End Namespace
