Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Text
Imports CapaNegocio.CapaNegocio





Public Class AgregarNuevoGrupo


    Public Event trasferirCadena(text As String)

    Private objetoCN_Conciliacion As ClsN_Conciliaciones = New ClsN_Conciliaciones()


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim nombreGrupo = CType(txtNombreGrupo.Text, String)

        If nombreGrupo IsNot String.Empty Then



            RaiseEvent trasferirCadena(nombreGrupo)
            Me.Close()

        Else

            If MessageBox.Show("EL CAMPO NOMBRE DE GRUPO NO PUEDE ESTAR EN BLANCO", "ERROR DE VALIDACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) Then
                Exit Sub
            End If

        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtNombreGrupo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNombreGrupo.KeyPress
        If Char.IsLetterOrDigit(e.KeyChar) Or e.KeyChar = Convert.ToChar(Keys.Back) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
End Class
