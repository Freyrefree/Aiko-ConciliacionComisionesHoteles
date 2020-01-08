Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data


Namespace CapaDatos

    Public Class ClsCityExpress

        ''Fecha Proveedor
        Public Property mesProveedor As String
        Public Property anioProveedor As String

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

                MsgBox(ex.Message & " " & " ERROR 151 cityExpress")

                Return 0
            Finally

                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_GuardarConciliacionDetalle()

            Dim lastId As Integer

            Dim query As String = "INSERT INTO conciliacionDetalleCityExpress(idConciliacion,dim_value,FechaApp,UserSpec,Segmento,
            CodigoConfirmacion,Comision,Operador,Moneda,CostoTotalDeLaReserva,Noches,ComOrig,SequenceNo,TipoConciliacion)
            VALUES(" & Me.idConciliacion & ",'" & Me.dim_value & "','" & FechaApp & "','" & UserSpec & "','" & Segmento & "',
            '" & CodigoConfirmacion & "','" & Comision & "','" & Operador & "','" & Moneda & "','" & CostoTotalDeLaReserva & "','" & Noches & "',
            '" & ComOrig & "', '" & SequenceNo & "', '" & TipoConciliacion & "');"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.ExecuteNonQuery()
                Return lastId

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 152 cityExpress")

                Return 0

            Finally

                conexion.CerrarConexion()
            End Try

        End Function

        Public Function CD_DatosCityExpress()


            Dim countResult As Int32
            Dim query As String = "SELECT COUNT(*) FROM cityexpress"

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

                MsgBox(ex.Message & " " & "ERROR 001 cityexpress")
                Return False
            Finally

                conexion.CerrarConexion()
            End Try



        End Function




        Public Function CD_TruncateCityExpressTmp()


            Dim query As String = "TRUNCATE TABLE cityexpressTmp"

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

                MsgBox(ex.Message & " " & "ERROR 002 cityexpress")
                Return False

            Finally

                conexion.CerrarConexion()
            End Try



        End Function


        Public Function CD_InsertarPendientesCityExpressTmp(cityexpress)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            If ClsGlobales.TipoPlantillaCityExpress = 1 Then

                Using SqlBulkCopy

                    SqlBulkCopy.DestinationTableName = "dbo.cityexpressTmp"

                    SqlBulkCopy.ColumnMappings.Add("Reservación", "Reservacion")
                    SqlBulkCopy.ColumnMappings.Add("Referencia OTA", "ReferenciaOTA")
                    SqlBulkCopy.ColumnMappings.Add("CheckIn", "CheckIn")
                    SqlBulkCopy.ColumnMappings.Add("CheckOut", "CheckOut")
                    SqlBulkCopy.ColumnMappings.Add("Monto", "Monto")
                    SqlBulkCopy.ColumnMappings.Add("Moneda", "Moneda")
                    SqlBulkCopy.ColumnMappings.Add("Forma Pago", "FormaPago")
                    SqlBulkCopy.ColumnMappings.Add("Tarifa", "Tarifa")
                    SqlBulkCopy.ColumnMappings.Add("Hotel", "Hotel")
                    SqlBulkCopy.ColumnMappings.Add("IATA", "IATA")
                    SqlBulkCopy.ColumnMappings.Add("Huesped", "Huesped")
                    SqlBulkCopy.ColumnMappings.Add("Estatus", "Estatus")
                    SqlBulkCopy.ColumnMappings.Add("Tasa", "Tasa")
                    SqlBulkCopy.ColumnMappings.Add("Comisión", "Comision")


                    Try
                        conexion.AbrirConexion()
                        SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                        SqlBulkCopy.WriteToServer(cityexpress)
                        Return True

                    Catch ex As Exception

                        MsgBox(ex.Message & " " & "ERROR 003 cityexpress")
                        Return False
                    Finally

                        conexion.CerrarConexion()

                    End Try

                End Using

            ElseIf ClsGlobales.TipoPlantillaCityExpress = 2 Then

                Dim count As Integer = 0

                Dim dateIn As String
                Dim dateOut As String

                Dim fechaInicio As String
                Dim fechaFin As String

                Dim tasa As Double
                Dim IATA As Int32

                Dim dtCloned As DataTable = cityexpress.Clone()
                dtCloned.Columns(4).DataType = GetType(String)
                dtCloned.Columns(5).DataType = GetType(String)
                dtCloned.Columns(6).DataType = GetType(String)
                dtCloned.Columns(12).DataType = GetType(Double)
                'dtCloned.Columns(15).DataType = GetType(Int32)
                For Each row As DataRow In cityexpress.Rows
                    dtCloned.ImportRow(row)
                Next






                For Each dr As DataRow In dtCloned.Rows

                    dateIn = vbEmpty
                    dateOut = vbEmpty
                    fechaInicio = vbEmpty
                    fechaFin = vbEmpty
                    tasa = Nothing
                    IATA = Nothing



                    dateIn = Trim(dr("Fecha de Entrada").ToString())
                    dateOut = Trim(dr("Fecha de Salida").ToString())
                    tasa = dr("Tasa de Comisión")
                    'IATA = Convert.ToInt32(If(TypeOf dr("Número de IATA") Is DBNull, 0, dr("Número de IATA")))
                    'IATA = Convert.ToInt32(If(TypeOf dr("Número de IATA") Is , 0, dr("Número de IATA")))
                    'IATA = dr("Número de IATA")
                    If (dr("Número de IATA") <> "") Then
                        IATA = dr("Número de IATA")
                    Else
                        IATA = 0
                    End If

                    'If count = 626 Then
                    'Console.WriteLine("here")
                    Console.WriteLine(count & "-" & dateIn & "-" & dateOut)
                    'End If

                    dr("Número de IATA") = IATA

                    tasa = tasa * 100
                    dr("Tasa de Comisión") = tasa


                    If dateIn.Length = 8 Then
                        fechaInicio = dateIn.Substring(0, 4) & "-" & dateIn.Substring(4, 2) & "-" & dateIn.Substring(6, 2)
                        dr("Fecha de Entrada") = fechaInicio
                    Else
                        dr("Fecha de Entrada") = ""
                    End If

                    If dateOut.Length = 8 Then
                        fechaFin = dateOut.Substring(0, 4) & "-" & dateOut.Substring(4, 2) & "-" & dateOut.Substring(6, 2)
                        dr("Fecha de Salida") = fechaFin
                    Else
                        dr("Fecha de Salida") = ""
                    End If

                    count = count + 1

                Next


                Using SqlBulkCopy

                    SqlBulkCopy.DestinationTableName = "dbo.cityexpressTmp"

                    SqlBulkCopy.ColumnMappings.Add("Marca", "marca")
                    SqlBulkCopy.ColumnMappings.Add("Clave del Hotel", "Hotel")
                    SqlBulkCopy.ColumnMappings.Add("Nombre del Hotel", "nombreDelHotel")
                    SqlBulkCopy.ColumnMappings.Add("Zona", "zona")
                    SqlBulkCopy.ColumnMappings.Add("Número de Confirmación", "conformationNo")
                    SqlBulkCopy.ColumnMappings.Add("Fecha de Entrada", "CheckIn")
                    SqlBulkCopy.ColumnMappings.Add("Fecha de Salida", "CheckOut")
                    SqlBulkCopy.ColumnMappings.Add("Número de Noches", "NoNoches")
                    SqlBulkCopy.ColumnMappings.Add("Codigo de Tarífa", "Tarifa")

                    SqlBulkCopy.ColumnMappings.Add("Monto de la Tarífa", "Monto")
                    SqlBulkCopy.ColumnMappings.Add("Moneda", "Moneda")
                    SqlBulkCopy.ColumnMappings.Add("Total de Ingreso", "totalDelIngreso")
                    SqlBulkCopy.ColumnMappings.Add("Tasa de Comisión", "Tasa")
                    SqlBulkCopy.ColumnMappings.Add("Monto de Comisión", "Comision")

                    SqlBulkCopy.ColumnMappings.Add("Forma de Pago de Estancia", "FormaPago")
                    SqlBulkCopy.ColumnMappings.Add("Número de IATA", "IATA")

                    SqlBulkCopy.ColumnMappings.Add("Nombre de Agencia", "nombreDeAgencia")
                    SqlBulkCopy.ColumnMappings.Add("Agencia Descuenta Comisión", "agenciaDescuentaComision")
                    SqlBulkCopy.ColumnMappings.Add("Canal", "canal")
                    SqlBulkCopy.ColumnMappings.Add("Convenio", "convenio")


                    Try
                        conexion.AbrirConexion()
                        SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                        SqlBulkCopy.WriteToServer(dtCloned)
                        Return True
                    Catch ex As Exception

                        MsgBox(ex.Message & " " & "ERROR 026 cityexpress")
                        Return False
                    Finally

                        conexion.CerrarConexion()

                    End Try

                End Using



            End If



        End Function



        Public Function CD_FaltantesCityExpress()


            If ClsGlobales.TipoPlantillaCityExpress = 1 Then

                Dim procedure As String = "cargaCityExpressFaltantes"

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

                    MsgBox(ex.Message & " " & "ERROR 004 cityexpress")
                    Return False

                Finally

                    conexion.CerrarConexion()

                End Try


            ElseIf ClsGlobales.TipoPlantillaCityExpress = 2 Then

                Dim query As String = "INSERT INTO [dbo].[cityexpress]
                (marca,Hotel,nombreDelHotel,zona,
                conformationNo,CheckIn,CheckOut,NoNoches,
                Tarifa,Monto,Moneda,totalDelIngreso,Tasa,Comision,
                FormaPago,IATA,NombreDeAgencia,
                agenciaDescuentaComision,canal,convenio)

                select marca,Hotel,nombreDelHotel,zona,
                conformationNo,CheckIn,CheckOut,NoNoches,
                Tarifa,Monto,Moneda,totalDelIngreso,Tasa,Comision,
                FormaPago,IATA,NombreDeAgencia,
                agenciaDescuentaComision,canal,convenio
                from cityexpresstmp atmp

                WHERE NOT EXISTS
                (SELECT * FROM cityexpress A 
                WHERE A.IATA = aTMP.IATA 
                AND A.conformationNo = aTMP.conformationNo)"

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
                        Return True
                    End If

                Catch ex As Exception

                    MsgBox(ex.Message & " " & "ERROR 070 cityexpress")
                    Return False

                Finally

                    conexion.CerrarConexion()

                End Try


            End If




        End Function


        Public Function CD_cargaArchivoCityExpress(cityexpress)

            Dim SqlBulkCopy As New SqlBulkCopy(conexion.Conexion)

            If ClsGlobales.TipoPlantillaCityExpress = 1 Then



                Using SqlBulkCopy

                    SqlBulkCopy.DestinationTableName = "dbo.cityexpress"

                    SqlBulkCopy.ColumnMappings.Add("Reservación", "Reservacion")
                    SqlBulkCopy.ColumnMappings.Add("Referencia OTA", "ReferenciaOTA")
                    SqlBulkCopy.ColumnMappings.Add("CheckIn", "CheckIn")
                    SqlBulkCopy.ColumnMappings.Add("CheckOut", "CheckOut")
                    SqlBulkCopy.ColumnMappings.Add("Monto", "Monto")
                    SqlBulkCopy.ColumnMappings.Add("Moneda", "Moneda")
                    SqlBulkCopy.ColumnMappings.Add("Forma Pago", "FormaPago")
                    SqlBulkCopy.ColumnMappings.Add("Tarifa", "Tarifa")
                    SqlBulkCopy.ColumnMappings.Add("Hotel", "Hotel")
                    SqlBulkCopy.ColumnMappings.Add("IATA", "IATA")
                    SqlBulkCopy.ColumnMappings.Add("Huesped", "Huesped")
                    SqlBulkCopy.ColumnMappings.Add("Estatus", "Estatus")
                    SqlBulkCopy.ColumnMappings.Add("Tasa", "Tasa")
                    SqlBulkCopy.ColumnMappings.Add("Comisión", "Comision")


                    Try
                        conexion.AbrirConexion()
                        SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                        SqlBulkCopy.WriteToServer(cityexpress)
                        Return True
                    Catch ex As Exception

                        MsgBox(ex.Message & " " & "ERROR 005 cityexpress")
                        Return False
                    Finally

                        conexion.CerrarConexion()

                    End Try

                End Using

            ElseIf ClsGlobales.TipoPlantillaCityExpress = 2 Then

                Dim count As Integer = 0

                Dim dateIn As String
                Dim dateOut As String

                Dim fechaInicio As String
                Dim fechaFin As String

                Dim tasa As Double
                Dim IATA As Int32

                Dim dtCloned As DataTable = cityexpress.Clone()
                dtCloned.Columns(4).DataType = GetType(String)
                dtCloned.Columns(5).DataType = GetType(String)
                dtCloned.Columns(6).DataType = GetType(String)
                dtCloned.Columns(12).DataType = GetType(Double)
                'dtCloned.Columns(15).DataType = GetType(Int32)
                For Each row As DataRow In cityexpress.Rows
                    dtCloned.ImportRow(row)
                Next






                For Each dr As DataRow In dtCloned.Rows

                    dateIn = vbEmpty
                    dateOut = vbEmpty
                    fechaInicio = vbEmpty
                    fechaFin = vbEmpty
                    tasa = Nothing
                    IATA = Nothing



                    dateIn = Trim(dr("Fecha de Entrada").ToString())
                    dateOut = Trim(dr("Fecha de Salida").ToString())
                    tasa = dr("Tasa de Comisión")
                    'IATA = Convert.ToInt32(If(TypeOf dr("Número de IATA") Is DBNull, 0, dr("Número de IATA")))
                    'IATA = Convert.ToInt32(If(TypeOf dr("Número de IATA") Is , 0, dr("Número de IATA")))
                    'IATA = dr("Número de IATA")
                    If (dr("Número de IATA") <> "") Then
                        IATA = dr("Número de IATA")
                    Else
                        IATA = 0
                    End If

                    'If count = 626 Then
                    'Console.WriteLine("here")
                    Console.WriteLine(count & "-" & dateIn & "-" & dateOut)
                    'End If

                    dr("Número de IATA") = IATA

                    tasa = tasa * 100
                    dr("Tasa de Comisión") = tasa


                    If dateIn.Length = 8 Then
                        fechaInicio = dateIn.Substring(0, 4) & "-" & dateIn.Substring(4, 2) & "-" & dateIn.Substring(6, 2)
                        dr("Fecha de Entrada") = fechaInicio
                    Else
                        dr("Fecha de Entrada") = ""
                    End If

                    If dateOut.Length = 8 Then
                        fechaFin = dateOut.Substring(0, 4) & "-" & dateOut.Substring(4, 2) & "-" & dateOut.Substring(6, 2)
                        dr("Fecha de Salida") = fechaFin
                    Else
                        dr("Fecha de Salida") = ""
                    End If

                    count = count + 1

                Next


                Using SqlBulkCopy

                    SqlBulkCopy.DestinationTableName = "dbo.cityexpress"

                    SqlBulkCopy.ColumnMappings.Add("Marca", "marca")
                    SqlBulkCopy.ColumnMappings.Add("Clave del Hotel", "Hotel")
                    SqlBulkCopy.ColumnMappings.Add("Nombre del Hotel", "nombreDelHotel")
                    SqlBulkCopy.ColumnMappings.Add("Zona", "zona")
                    SqlBulkCopy.ColumnMappings.Add("Número de Confirmación", "conformationNo")
                    SqlBulkCopy.ColumnMappings.Add("Fecha de Entrada", "CheckIn")
                    SqlBulkCopy.ColumnMappings.Add("Fecha de Salida", "CheckOut")
                    SqlBulkCopy.ColumnMappings.Add("Número de Noches", "NoNoches")
                    SqlBulkCopy.ColumnMappings.Add("Codigo de Tarífa", "Tarifa")

                    SqlBulkCopy.ColumnMappings.Add("Monto de la Tarífa", "Monto")
                    SqlBulkCopy.ColumnMappings.Add("Moneda", "Moneda")
                    SqlBulkCopy.ColumnMappings.Add("Total de Ingreso", "totalDelIngreso")
                    SqlBulkCopy.ColumnMappings.Add("Tasa de Comisión", "Tasa")
                    SqlBulkCopy.ColumnMappings.Add("Monto de Comisión", "Comision")

                    SqlBulkCopy.ColumnMappings.Add("Forma de Pago de Estancia", "FormaPago")
                    SqlBulkCopy.ColumnMappings.Add("Número de IATA", "IATA")

                    SqlBulkCopy.ColumnMappings.Add("Nombre de Agencia", "nombreDeAgencia")
                    SqlBulkCopy.ColumnMappings.Add("Agencia Descuenta Comisión", "agenciaDescuentaComision")
                    SqlBulkCopy.ColumnMappings.Add("Canal", "canal")
                    SqlBulkCopy.ColumnMappings.Add("Convenio", "convenio")


                    Try
                        conexion.AbrirConexion()
                        SqlBulkCopy.BulkCopyTimeout = 60 * 50 ' 5 minutos
                        SqlBulkCopy.WriteToServer(dtCloned)
                        Return True
                    Catch ex As Exception

                        MsgBox(ex.Message & " " & "ERROR 025 cityexpress")
                        Return False
                    Finally

                        conexion.CerrarConexion()

                    End Try

                End Using


                'MsgBox("Formato2")

            End If

        End Function


        Public Sub CD_addFirtsNameLastName()

            'Dim fechaProveedor As String = ClsGlobales.AnioProveedor & "-" & ClsGlobales.MesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"

            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            Dim query = "UPDATE cityexpress SET
firstName =  SUBSTRING(huesped,1,CHARINDEX(' ',huesped)-1),
lastName =  SUBSTRING(huesped,CHARINDEX(' ',huesped)+1,LEN(huesped))" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 00678 cityexpress")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Sub CD_addNoNoches()

            'Dim fechaProveedor As String = ClsGlobales.AnioProveedor & "-" & ClsGlobales.MesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query = "UPDATE cityexpress SET 
            NoNoches = DATEDIFF(day, CheckIn, CheckOut)" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0089 cityexpress")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub





        Public Function CD_SelectCityExpress() As DataTable


            Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin


            Dim queryFechaProveedor As String = " WHERE mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"

            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"


            ''''''''''''''''''''''''''''''''''''''''''  Eliminar Repetidos  '''''''''''''''''''''''''''''''''''''''
            If ClsGlobales.TipoPlantillaCityExpress = 1 Then

                '    Dim queryRepetidos = "WITH CTE AS 
                '(SELECT *, NUMERO_REPETIDOS=ROW_NUMBER() OVER(PARTITION BY 
                '[IATA]
                ',[Reservacion]
                'ORDER BY id) 
                'FROM cityexpress)
                'DELETE FROM CTE WHERE NUMERO_REPETIDOS > 1"

                '    Try
                '        comando.Connection = conexion.AbrirConexion()
                '        comando.CommandText = queryRepetidos
                '        comando.CommandType = CommandType.Text
                '        Dim res As Boolean = comando.ExecuteNonQuery

                '    Catch ex As Exception

                '        MsgBox(ex.Message & " " & "ERROR 101 CITY Express")

                '    Finally

                '        conexion.CerrarConexion()

                '    End Try

            End If




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaCityExpress As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM cityexpress " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaCityExpress.Load(leer)

                Return tablaCityExpress

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 009 cityexpress")
                Return tablaCityExpress

            Finally

                conexion.CerrarConexion()

            End Try


        End Function

        Public Function CD_SelectCityExpressFechaPagoProveedor() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"

            Dim queryEstatusEliminado As String = " And estatusEliminado Is NULL"

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaCityExpress As DataTable = New DataTable()

            Dim query As String = "Select * FROM cityexpress " & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaCityExpress.Load(leer)

                Return tablaCityExpress

            Catch ex As Exception

                MsgBox(ex.Message & " " & " Error 0098 cityexpress")
                Return tablaCityExpress

            Finally

                conexion.CerrarConexion()

            End Try


        End Function


        Public Function CD_ConsultaAcentos()


            'Dim fechaProveedor As String = ClsGlobales.AnioProveedor & "-" & ClsGlobales.MesProveedor & "-" & "01"
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"

            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim tabla As DataTable = New DataTable()

            Dim query As String = "SELECT id, firstName, lastName FROM cityexpress" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)

                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 123 cityexpress")
                Return tabla

            Finally

                conexion.CerrarConexion()

            End Try

        End Function

        Public Sub CD_QuitarAcentoFirstName(id, firstName)

            Dim query As String = "UPDATE cityexpress SET firstName = '" & firstName & "' WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0086 cityexpress")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub

        Public Sub CD_QuitarAcentoLastName(id, lastNameB)
            Dim query As String = "UPDATE cityexpress SET lastName = '" & lastNameB & "' WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR 0087 cityexpress")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub

        Public Sub CD_CandenaConciliados(cadenaCumplidas)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

            Dim query As String = "UPDATE cityexpress SET CondicionOKAuto = '" & cadenaCumplidas & "' WHERE estatusConciliado = 1" & queryFechaProveedor & queryEstatusEliminado

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text

                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 098 cityexpress")


            Finally

                conexion.CerrarConexion()

            End Try


        End Sub


        Public Function CD_SelectSinConciliar() As DataTable

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
            Dim queryFechaProveedor As String = " AND mesProveedor = '" & fechaProveedor & "'"

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim tablaPosadas As DataTable = New DataTable()

            Dim query As String = "SELECT * FROM cityexpress WHERE CondicionOKAuto IS  NULL AND estatusConciliado IS  NULL AND idBDBCD IS  NULL" & queryFechaProveedor

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tablaPosadas.Load(leer)
                Return tablaPosadas

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 100 cityexpress")
                Return tablaPosadas

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Sub CD_EliminarCityExpress(id)

            'Dim query As String = "DELETE FROM cityexpress WHERE id = " & id & ""

            Dim query As String = "UPDATE  cityexpress SET estatusEliminado=1 WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.Parameters.Clear()
                Dim res As Boolean = comando.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 101 cityexpress")


            Finally

                conexion.CerrarConexion()

            End Try

        End Sub




        Public Function CD_ConciliarByID(id, idBDBCD, lastQuery)

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE cityexpress
            SET estatusConciliado = 1
            WHERE id  = " & id & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1265 cityexpress")
            Finally
                conexion.CerrarConexion()
            End Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryB As String = "UPDATE BDBCD SET 
estatusConciliado = 1,
proveedor = 'cityexpress',
mesProveedor = '" & fechaProveedor & "'
WHERE id  = " & idBDBCD & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryB
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1266 cityexpress")
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
            BD.conformationNo AS CodigoConfirmacion,
            proveedor.Comision AS Comision,
            'CityExpress' AS Operador,
            proveedor.Moneda AS Moneda,
            Monto AS CostoTotalDeLaReserva,
            proveedor.noNoches AS Noches,
            proveedor.Comision As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Adicional' As tipoConciliacion
            FROM  cityexpress proveedor 
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
                MsgBox(ex.Message & " " & "ERROR 1267 cityexpress")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function


        Public Function CD_ObtenerUltimoId()

            Dim lastId As Int64

            Dim queryA As String = "SELECT MAX(id) FROM cityexpress"

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
                MsgBox(ex.Message & " " & " ERROR 1014 cityexpress")
            Finally
                conexion.CerrarConexion()
            End Try

        End Function


        Public Sub CD_agregarMesProveedor(ByVal lastId As Int64)

            'Dim fechaProveedor As String = ClsGlobales.AnioProveedor & "-" & ClsGlobales.MesProveedor & "-" & "01"

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = "UPDATE cityExpress Set mesProveedor = '" & fechaProveedor & "' WHERE id > " & lastId & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                Dim res As Boolean = comando.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox(ex.Message & " " & " ERROR 1013 cityExpress")
            Finally
                conexion.CerrarConexion()
            End Try


        End Sub

        Public Function CD_SeleccionIDPendientesCityExpress()

            Dim tabla As DataTable = New DataTable()

            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

            Dim query As String = " SELECT id FROM cityexpress
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
                MsgBox(ex.Message & " " & " ERROR AAA cityexpress")
                Return tabla
            Finally
                conexion.CerrarConexion()
            End Try


        End Function






    End Class

End Namespace
