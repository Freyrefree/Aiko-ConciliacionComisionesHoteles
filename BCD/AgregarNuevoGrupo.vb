Imports System.ComponentModel
Imports System.Windows.Forms

Public Class AgregarNuevoGrupo
    Private _ListaGruposExtistentes As BindingList(Of GrupoConciliaciones)
    Public Event AgregarNuevoGrupo As EventHandler(Of AgregarNuevoGrupoConciliacionArgs)

    Public Sub New(ByVal ListaGruposExistentes)

        ' This call is required by the designer.
        InitializeComponent()
        _ListaGruposExtistentes = ListaGruposExistentes
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If txtNombreGrupo.Text IsNot String.Empty Then
            Dim GrupoConciliacionExistente = _ListaGruposExtistentes.FirstOrDefault(Function(x) x.Nombre = txtNombreGrupo.Text)
            If GrupoConciliacionExistente IsNot Nothing Then
                If MessageBox.Show("YA EXISTE UN GRUPO DE CONCILIACIONES CON EL MISMO NOMBRE", "ERROR DE VALIDACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) = DialogResult.OK Then
                    Exit Sub
                End If
            Else
                Dim _AgregarNuevoGrupoConciliacionArgs = New AgregarNuevoGrupoConciliacionArgs()
                Dim _grupoConciliaciones = New GrupoConciliaciones()
                _grupoConciliaciones.Nombre = txtNombreGrupo.Text
                _AgregarNuevoGrupoConciliacionArgs.GrupoConciliaciones = _grupoConciliaciones
                RaiseEvent AgregarNuevoGrupo(Me, _AgregarNuevoGrupoConciliacionArgs)
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If

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
