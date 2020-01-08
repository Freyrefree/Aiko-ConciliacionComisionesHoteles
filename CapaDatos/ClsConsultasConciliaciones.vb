Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data


Namespace CapaDatos

    Public Class ClsConsultasConciliaciones

        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()


        Public idProveedor As Integer
        Public idConciliacion As Integer

        Public Function CD_SelectConciliacionesByProveedor() As DataTable

            Dim tabla As DataTable = New DataTable()
            Dim query As String = "SELECT * FROM conciliacion WHERE idProveedor = " & Me.idProveedor & ""

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)
                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 001 ConsultaConciliacion")
                Return tabla

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Function CD_ConsultaConciliacionesDetalleByIdConciliacion() As DataTable

            Dim tabla As DataTable = New DataTable()
            Dim query As String
            'Obtener Tabla id de proveedor
            Dim queryA As String = "SELECT idProveedor FROM conciliacion WHERE id = " & Me.idConciliacion & ""
            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryA
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()

                'Dim idProveedor As Int64 = leer.GetInt64(0)

                While leer.Read()
                    Dim idProveedor As Int64 = leer(0)
                    'Console.WriteLine("{0}" & vbTab & "{1}", reader.GetInt32(0), reader.GetString(1))
                End While


            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 009 ConsultaConciliacion")


            Finally
                conexion.CerrarConexion()
            End Try

            If (idProveedor = 1) Then

                query = "SELECT * FROM conciliacionDetallePosadas WHERE idConciliacion = " & Me.idConciliacion & ""
            ElseIf (idProveedor = 2) Then
                query = "SELECT * FROM conciliacionDetalleCityExpress WHERE idConciliacion = " & Me.idConciliacion & ""
            ElseIf (idProveedor = 3) Then
                query = "SELECT * FROM conciliacionDetalleOnyx WHERE idConciliacion = " & Me.idConciliacion & ""
            ElseIf (idProveedor = 4) Then
                query = "SELECT * FROM conciliacionDetalleTacs WHERE idConciliacion = " & Me.idConciliacion & ""
            ElseIf (idProveedor = 19) Then
                query = "SELECT * FROM conciliacionDetalleGestionCommtrack WHERE idConciliacion = " & Me.idConciliacion & ""

            End If




            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)
                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 002 ConsultaConciliacion")
                Return tabla

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

        Public Function CD_ConsultaConciliacionesByFechaPagoProveedor() As DataTable

            Dim fechaInicio As String = ClsGlobales.FechaProveedorInicio
            Dim fechaFin As String = ClsGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()
            Dim query As String = ""
            'Dim query As String = "SELECT * FROM conciliacion WHERE idProveedor = " & Me.idProveedor & ""

            If (idProveedor = 1) Then
                query = "SELECT * FROM conciliacionDetallePosadas WHERE FechaApp >= '" & fechaInicio & "' AND  FechaApp <= '" & fechaFin & "' "
            ElseIf (idProveedor = 2) Then
                query = "SELECT * FROM conciliacionDetalleCityExpress WHERE FechaApp >= '" & fechaInicio & "' AND  FechaApp <= '" & fechaFin & "' "
            ElseIf (idProveedor = 3) Then
                query = "SELECT * FROM conciliacionDetalleOnyx WHERE FechaApp >= '" & fechaInicio & "' AND FechaApp <= '" & fechaFin & "' "
            ElseIf (idProveedor = 4) Then
                query = "SELECT * FROM conciliacionDetalleTacs WHERE FechaApp >= '" & fechaInicio & "' AND  FechaApp <= '" & fechaFin & "' "
            ElseIf (idProveedor = 19) Then
                query = "SELECT * FROM conciliacionDetalleGestionCommtrack WHERE FechaApp >= '" & fechaInicio & "' AND FechaApp <= '" & fechaFin & "' "

            End If

            Try
                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = query
                comando.CommandType = CommandType.Text
                leer = comando.ExecuteReader()
                tabla.Load(leer)
                Return tabla

            Catch ex As Exception

                MsgBox(ex.Message & " " & " ERROR 003 ConsultaConciliacion")
                Return tabla

            Finally
                conexion.CerrarConexion()
            End Try


        End Function

    End Class

End Namespace
