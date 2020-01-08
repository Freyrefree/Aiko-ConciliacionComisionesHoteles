Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data


Namespace CapaDatos


    Public Class ClsTipoDeCambio

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()


        Public Function CN_DataComboPeriodos()


            Dim lista As List(Of Combos) = New List(Of Combos)()


            Dim query As String = "SELECT DISTINCT mesProveedor FROM onyx ORDER BY mesProveedor"

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            leer = comando.ExecuteReader()

            Dim item As Combos = New Combos()
            'item.idProveedor = 0
            item.fechaPeriodo = "-- Selecciona Un Periodo --"
            lista.Add(item)

            While leer.Read()
                Dim itemB As Combos = New Combos()
                'itemB.idProveedor = leer.GetInt32(0)
                itemB.fechaPeriodo = leer.GetDateTime(0)
                lista.Add(itemB)
            End While

            conexion.CerrarConexion()

            Return lista

        End Function



        Public Function CD_DataComboMonedasPeriodo(fecha)


            Dim lista As List(Of Combos) = New List(Of Combos)()


            Dim query As String = "SELECT DISTINCT op.ConfCurrency,m.nombreMoneda,
            CONCAT(op.ConfCurrency,'-',m.nombreMoneda) as monedas FROM onyxComisionesPendientePago op
            INNER JOIN moneda m ON m.codigo = op.ConfCurrency
            WHERE mesProveedor = '" & fecha & "'"



            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            leer = comando.ExecuteReader()

            Dim item As Combos = New Combos()
            'item.idProveedor = 0
            item.moneda = "-- Selecciona Una moneda --"
            lista.Add(item)

            While leer.Read()
                Dim itemB As Combos = New Combos()
                itemB.codigoMoneda = leer.GetString(0)
                itemB.moneda = leer.GetString(2)
                lista.Add(itemB)
            End While

            conexion.CerrarConexion()

            Return lista

        End Function

        Public Sub CD_guardarTipoCambio(idProveedor, fechaProveedor, tipoCambio, moneda)

            'Dim query As String = "INSERT INTO tipoCambio(idProveedor,fechaPeriodo) VALUES (" & idProveedor & ",'" & fechaProveedor & "')"

            Dim query As String = "
DECLARE @mesProveedor DATE
DECLARE @idProveedor INT
DECLARE @idMoneda INT
DECLARE @codigoMoneda VARCHAR(10)
DECLARE @valorMoneda DECIMAL(18,3)
---------------------------------
DECLARE @cuantos INT
DECLARE @idTipoCambio INT
DECLARE @cuantosTipoCambioDetalle INT
-----------------------------------
SET @mesProveedor = '" & fechaProveedor & "'
SET @idProveedor = " & idProveedor & "
SET @valorMoneda = " & tipoCambio & "
SET @codigoMoneda = '" & moneda & "'
---------------------------------------------------------------
--TIPO 1 = Sin registro de Onyx
--TIPO 2 = Con registro de onyx

SELECT  @cuantos = COUNT(*) FROM tipoCambio WHERE idProveedor = @idProveedor AND fechaPeriodo = @mesProveedor

--Obtener ID MONEDA
SELECT @idMoneda = id FROM moneda WHERE codigo = @codigoMoneda

IF @cuantos > 0 BEGIN

--print('si hay')
SELECT  @idTipoCambio = id FROM  tipoCambio WHERE idProveedor = @idProveedor AND fechaPeriodo = @mesProveedor

--Validar que no exista ya una moneda en ese periodo
SELECT @cuantosTipoCambioDetalle = COUNT(*) FROM tipoCambioDetalle WHERE idTipoCambio = @idTipoCambio AND idMoneda = @idMoneda

	IF @cuantosTipoCambioDetalle = 0 BEGIN

		INSERT INTO tipoCambioDetalle(idTipoCambio,idMoneda,valorMoneda,tipo,fechaActualizacion) 
		VALUES (@idTipoCambio,@idMoneda,@valorMoneda,1,GETDATE())

	END

END ELSE  BEGIN

--print('no hay')
INSERT INTO tipoCambio(idProveedor,fechaPeriodo) VALUES (@idProveedor,@mesProveedor)

SELECT  @idTipoCambio = id FROM  tipoCambio WHERE idProveedor = @idProveedor AND fechaPeriodo = @mesProveedor

--Validar que no exista ya una moneda en ese periodo
SELECT @cuantosTipoCambioDetalle = COUNT(*) FROM tipoCambioDetalle WHERE idTipoCambio = @idTipoCambio AND idMoneda = @idMoneda

	IF @cuantosTipoCambioDetalle = 0 BEGIN

		INSERT INTO tipoCambioDetalle(idTipoCambio,idMoneda,valorMoneda,tipo,fechaActualizacion) 
		VALUES (@idTipoCambio,@idMoneda,@valorMoneda,1,GETDATE())

	END

END"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 033 tipoCambio")


            Finally

                conexion.CerrarConexion()
            End Try

        End Sub


        Public Function consultaPeriodos()

            Dim tabla As DataTable = New DataTable()
            Dim query As String = "SELECT * FROM tipoCambio"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)
                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR Tipo de cambio 0003")
                Return tabla

            Finally
                conexion.CerrarConexion()
            End Try

        End Function

        Public Function consultaMonedasPeriodo(id, mesProveedor)

            Dim res As Boolean = False


            Dim queryA As String = "
DECLARE @idTipoCambio INT
DECLARE @mesProveedor varchar(30)
SET @idTipoCambio = " & id & "
SET @mesProveedor = '" & mesProveedor & "'

INSERT INTO tipoCambioDetalle(idMoneda,idTipoCambio,tipo)
SELECT DISTINCT 
m.id AS idMoneda,
@idTipoCambio AS idTipoCambio,
1 AS tipo
FROM 
onyxComisionesPendientePago op
INNER JOIN 
moneda m ON m.codigo = op.ConfCurrency
WHERE mesProveedor = @mesProveedor
AND m.id NOT IN (SELECT idMoneda FROM tipoCambioDetalle WHERE idTipoCambio = @idTipoCambio)"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryA
                comando.CommandType = CommandType.Text
                res = comando.ExecuteNonQuery()
                'res = True

            Catch ex As Exception
                res = False

                MsgBox(ex.Message & " " & " ERROR Tipo de cambio 0010")


            Finally
                conexion.CerrarConexion()
            End Try





            Dim tabla As DataTable = New DataTable()

                Dim query As String = "SELECT tcd.id,tcd.idTipoCambio,tcd.idMoneda,m.codigo,m.nombreMoneda,tcd.valorMoneda,tcd.fechaActualizacion FROM tipoCambioDetalle tcd
            INNER JOIN moneda m ON m.id = tcd.idMoneda AND tcd.idTipoCambio = " & id & ""

                Try
                    comando.Connection = conexion.AbrirConexion()
                    comando.CommandText = query
                    comando.CommandType = CommandType.Text
                    leer = comando.ExecuteReader()
                    tabla.Load(leer)
                    Return tabla

                Catch ex As Exception

                    MsgBox(ex.Message & " " & " ERROR Tipo de cambio 0004")
                    Return tabla

                Finally
                    conexion.CerrarConexion()
                End Try






        End Function


        Public Function actualizarTipoCambio(id, valorMoneda)

            Dim res As Boolean = False


            Dim query As String = "UPDATE tipoCambioDetalle SET valorMoneda = " & valorMoneda & ", fechaActualizacion = GETDATE() WHERE id = " & id & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                res = comando.ExecuteNonQuery()
                'res = True
                Return res
            Catch ex As Exception
                res = False

                MsgBox(ex.Message & " " & " ERROR Tipo de cambio 0005")
                Return res

            Finally
                conexion.CerrarConexion()
            End Try

        End Function


        Public Function cargaPeriodosFaltantes()

            Dim res As Boolean = False


            Dim query As String = "INSERT INTO tipoCambio(fechaPeriodo,idProveedor)
SELECT DISTINCT mesProveedor,3 AS idProveedor FROM ONYX
WHERE mesProveedor NOT IN (SELECT fechaPeriodo FROM tipoCambio WHERE idProveedor = 3)"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                res = comando.ExecuteNonQuery()

                Return res
            Catch ex As Exception
                res = False

                MsgBox(ex.Message & " " & " ERROR Tipo de cambio 0006")
                Return res

            Finally
                conexion.CerrarConexion()
            End Try

        End Function




    End Class

End Namespace
