Imports System.Windows.Forms

Public Class ModificarGrupoConciliaciones
    Private _GrupoConciliaciones As GrupoConciliaciones
    Public Event ModificarNombreGrupoEvent As EventHandler(Of AgregarNuevoGrupoConciliacionArgs)
    Public Sub New(ByVal GConciliaciones As GrupoConciliaciones)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _GrupoConciliaciones = GConciliaciones
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If txtNombreGrupo.Text IsNot String.Empty Then
            _GrupoConciliaciones.Nombre = txtNombreGrupo.Text
            Dim evtArgs As AgregarNuevoGrupoConciliacionArgs = New AgregarNuevoGrupoConciliacionArgs()
            evtArgs.GrupoConciliaciones = _GrupoConciliaciones
            RaiseEvent ModificarNombreGrupoEvent(Me, evtArgs)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            If MessageBox.Show("EL CAMPO NOMBRE DE GRUPO NO PUEDE ESTAR EN BLANCO", "ERROR DE VALIDACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) = DialogResult.OK Then
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ModificarGrupoConciliaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtNombreGrupo.Text = _GrupoConciliaciones.Nombre
    End Sub
End Class
