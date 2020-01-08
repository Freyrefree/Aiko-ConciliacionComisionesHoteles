Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos
Imports CapaDatos.CapaDatos

Namespace CapaNegocio

    Public Class ClsN_Proveedores

        Private objetoCapaDatos As ClsProveedores = New ClsProveedores()


        Public Function MostrarClientes() As DataTable

            Dim tabla As DataTable = New DataTable()
            tabla = objetoCapaDatos.Mostrar()
            Return tabla

        End Function

        Public Function CN_DataComboProveedores()

            Return objetoCapaDatos.CD_LlenarComboClientes

        End Function


        Public Sub InsertarCliente(nombre As String)

            objetoCapaDatos.Insertar(nombre)

        End Sub

        Public Sub EditarCliente(nombre As String, id As String)

            objetoCapaDatos.Editar(nombre, Convert.ToInt32(id))

        End Sub

        Public Sub EliminarCliente(id As String)

            objetoCapaDatos.Eliminar(Convert.ToInt32(id))

        End Sub















    End Class

End Namespace

