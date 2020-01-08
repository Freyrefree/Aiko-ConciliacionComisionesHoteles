Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Configuration

Namespace CapaDatos

    Public Class ClsConexion

        Public Conexion As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("connConciliacionesHoteles").ConnectionString)

        'Public Conexion As SqlConnection = New SqlConnection("Server=MAQ;Database=conciliacionesProveedores;User Id=sa;Password=server")
        'Public Conexion As SqlConnection = New SqlConnection("Server=DIEGO\SQLEXPRESS;Database=conciliacionesProveedores;User Id=sa;Password=1234")
        'Public Conexion As SqlConnection = New SqlConnection("Server=192.168.234.24\PROVEEDORESBCD;Database=conciliacionesProveedores;User Id=sa;Password=sql14.@iko18**")

        Public Function AbrirConexion() As SqlConnection
            If Conexion.State = ConnectionState.Closed Then Conexion.Open()
            Return Conexion
        End Function

        Public Function CerrarConexion() As SqlConnection
            If Conexion.State = ConnectionState.Open Then Conexion.Close()
            Return Conexion
        End Function

    End Class

End Namespace


