Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports CapaNegocio.CapaNegocio

Public Class FormProveedores
    Private objetoCapaNegocio As ClsN_Proveedores = New ClsN_Proveedores()

    Private idCliente As String = Nothing

    Private Sub FormClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MostrarClientes()
        FillComboClientes()

    End Sub


    Private Sub MostrarClientes()
        'Objeto para mostrar registros y actualizarla vista
        Dim bjetoCA As ClsN_Proveedores = New ClsN_Proveedores()

        DataGridView1.DataSource = bjetoCA.MostrarClientes()
    End Sub

    Private Sub FillComboClientes()

        Dim bjetoFillCBCA As ClsN_Proveedores = New ClsN_Proveedores()
        bjetoFillCBCA.CN_DataComboProveedores()

    End Sub

    Private Sub BtnGuardarCliente_Click(sender As Object, e As EventArgs) Handles BtnGuardarCliente.Click
        Dim nombreCliente As String = ""


        nombreCliente = txtNombreCliente.Text

        If nombreCliente <> "" Then

            objetoCapaNegocio.InsertarCliente(nombreCliente)
            MostrarClientes()

        Else
            MessageBox.Show("Ingrese un nombre")
        End If

    End Sub





    Private Sub BtnActualizarCliente_Click(sender As Object, e As EventArgs) Handles BtnActualizarCliente.Click
        Dim nombreCliente As String = ""


        nombreCliente = txtNombreCliente.Text

        If idCliente <> Nothing Then

            objetoCapaNegocio.EditarCliente(nombreCliente, idCliente)
            MostrarClientes()

            'Cambiar valor de idCliente a vacío para que no se pueda realzar la operacion más de una vez
            idCliente = Nothing

        Else

            MessageBox.Show("Intente de Nuevo Por Favor")

        End If



    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick
        If (DataGridView1.SelectedRows.Count > 0) Then

            txtNombreCliente.Text = DataGridView1.CurrentRow.Cells("Nombre").Value.ToString()
            idCliente = DataGridView1.CurrentRow.Cells("id").Value.ToString()

        Else

            MessageBox.Show("Seleccione Una Fila")

        End If
    End Sub

    Private Sub BtnEliminarCliente_Click(sender As Object, e As EventArgs) Handles BtnEliminarCliente.Click



        If (DataGridView1.SelectedRows.Count > 0) Then

            txtNombreCliente.Text = DataGridView1.CurrentRow.Cells("Nombre").Value.ToString()
            idCliente = DataGridView1.CurrentRow.Cells("id").Value.ToString()

            If idCliente <> Nothing Then

                objetoCapaNegocio.EliminarCliente(idCliente)
                MostrarClientes()
                'Cambiar valor de idCliente a vacío para que no se pueda realzar la operacion más de una vez
                idCliente = Nothing

            Else

                MessageBox.Show("Intente de Nuevo Por Favor")

            End If

        Else

            MessageBox.Show("Seleccione Una Fila")

        End If


    End Sub

    Private Sub txtNombreCliente_TextChanged(sender As Object, e As EventArgs) Handles txtNombreCliente.TextChanged

    End Sub
End Class
