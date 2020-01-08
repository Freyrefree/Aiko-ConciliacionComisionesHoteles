Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data

Namespace CapaDatos

    Public Class ClsProveedores



        Private conexion As ClsConexion = New ClsConexion()
        Private leer As SqlDataReader
        Private tabla As DataTable = New DataTable()
        Private comando As SqlCommand = New SqlCommand()


        Public Function Mostrar() As DataTable
            'Ejemplo con Consulta

            'comando.Connection = conexion.AbrirConexion()
            'comando.CommandText = "SELECT * FROM clientes"
            'leer = comando.ExecuteReader()
            'tabla.Load(leer)
            'conexion.CerrarConexion()
            'Return tabla

            'Uso de procedimiento Almacenado

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = "SP_SelectProveedores"
            comando.CommandType = CommandType.StoredProcedure
            leer = comando.ExecuteReader()
            tabla.Load(leer)
            conexion.CerrarConexion()
            Return tabla

        End Function

        Public Sub Insertar(nombre As String)

            'Ejemplo con Consulta

            'comando.Connection = conexion.AbrirConexion()
            'comando.CommandText = "INSERT INTO clientes(nombre)VALUES('" & nombre & "')"
            'comando.CommandType = CommandType.Text
            'comando.ExecuteNonQuery()

            'Uso de procedimiento Almacenado 

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = "SP_AddProveedor"
            comando.CommandType = CommandType.StoredProcedure
            comando.Parameters.Clear()
            comando.Parameters.AddWithValue("@nombre", nombre)
            comando.ExecuteNonQuery()

        End Sub

        Public Sub Editar(nombre As String, id As Int16)

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = "SP_UpdateProveedor"
            comando.CommandType = CommandType.StoredProcedure
            comando.Parameters.Clear()
            comando.Parameters.AddWithValue("@nombre", nombre)
            comando.Parameters.AddWithValue("@id", id)
            comando.ExecuteNonQuery()


        End Sub

        Public Sub Eliminar(id As Int16)

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = "SP_DeleteProveedor"
            comando.CommandType = CommandType.StoredProcedure
            comando.Parameters.Clear()
            comando.Parameters.AddWithValue("@id", id)
            comando.ExecuteNonQuery()


        End Sub

        Public Function CD_LlenarComboClientes() As List(Of Combos)
            Dim lista As List(Of Combos) = New List(Of Combos)()

            Dim query As String = "SELECT id,nombre FROM proveedores WHERE activo = 'si'"

            comando.Connection = conexion.AbrirConexion()
            'comando.CommandText = "SP_FillComboProveedores"
            comando.CommandText = query
            'comando.CommandType = CommandType.StoredProcedure
            comando.CommandType = CommandType.Text
            leer = comando.ExecuteReader()

            Dim item As Combos = New Combos()
            item.idProveedor = 0
            item.nombreProveedor = "-- Selecciona Un Proveedor --"
            lista.Add(item)

            While leer.Read()
                Dim itemB As Combos = New Combos()
                itemB.idProveedor = leer.GetInt32(0)
                itemB.nombreProveedor = leer.GetString(1)
                lista.Add(itemB)
            End While

            conexion.CerrarConexion()

            Return lista

        End Function

        Public Function conciliaCionAutoLista(idProveedor As Int16) As List(Of Listas)

            Dim lista As List(Of Listas) = New List(Of Listas)()

            Dim query As String = "	SELECT CE.*,CB.nombreColumna AS nombreColumnaBDBCD FROM columnasExcel CE
	        INNER JOIN  columnasBDBCD CB ON CE.columnBDBCD = CB.id
	        WHERE idProveedor = " & idProveedor & " AND esAuto = 1"

            Dim tipoDato As Int32
            Dim tipoOperacion As Int32

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.Parameters.Clear()
            comando.CommandType = CommandType.Text

            Try
                leer = comando.ExecuteReader
            Catch ex As Exception

                Return lista

            End Try


            While leer.Read()
                Dim item As Listas = New Listas()
                item.ColumnaAutomatica = leer("nombreColumna")
                item.ColumnaAutomaticaBDBCD = leer("nombreColumnaBDBCD")

                tipoDato = leer("tipoDato")
                tipoOperacion = leer("tipoOperacion")


                Select Case tipoOperacion
                    Case 1
                        item.TipoOperacion = "IGUALDAD"
                    Case 2
                        item.TipoOperacion = "CONTIENE"
                    Case Else
                        item.TipoOperacion = "indefinido"
                End Select


                Select Case tipoDato
                    Case 1
                        item.TipoDato = "NUMÉRICO"
                    Case 2
                        item.TipoDato = "TEXTO"
                    Case 3
                        item.TipoDato = "MONEDA"
                    Case 4
                        item.TipoDato = "FECHA"
                    Case Else
                        item.TipoDato = "indefinido"
                End Select

                lista.Add(item)
            End While

            conexion.CerrarConexion()

            Return lista

        End Function

    End Class

End Namespace


