Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos.CapaDatos
Namespace CapaDatos

    Public Class ClsActualizaciones
        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()

        Public Sub ActualizarFechaGestion()
            Dim query As String = "
                DECLARE @totalRegistros INTEGER
                DECLARE @longitud AS INTEGER = 10
                SELECT @totalRegistros = COUNT(*) FROM  gestionCommtrack WHERE  LEN(DIN) > @longitud

                IF @totalRegistros > 0 BEGIN

                UPDATE gestionCommtrack SET 
                DIN =CONCAT(SUBSTRING(DIN,7,4),'-',SUBSTRING(DIN,4,2),'-',SUBSTRING(DIN,1,2)),
                OUT =CONCAT(SUBSTRING(OUT,7,4),'-',SUBSTRING(OUT,4,2),'-',SUBSTRING(OUT,1,2))
                WHERE  LEN(DIN) >  @longitud

                END

ALTER TABLE conciliacionDetalleOnyx ALTER COLUMN FechaApp date;
ALTER TABLE conciliacionDetalleCityExpress ALTER COLUMN FechaApp date;
ALTER TABLE conciliacionDetalleGestionCommtrack ALTER COLUMN FechaApp date;
ALTER TABLE conciliacionDetallePosadas ALTER COLUMN FechaApp date;
ALTER TABLE conciliacionDetalleTacs ALTER COLUMN FechaApp date;

"

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 5 ' 5 minutos
                comando.ExecuteNonQuery()

            Catch ex As Exception

                MsgBox(ex.Message & " " & "ERROR Actualizacion 1")
            Finally
                conexion.CerrarConexion()

            End Try


        End Sub



    End Class
End Namespace