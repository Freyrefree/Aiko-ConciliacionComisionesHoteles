Public Class Form1
    Private ConciliacionForm As ConciliacionAmex
    Private ConciliacionFormCH As ConciliacionComisionesHoteles

    Private ConciliacionFormI1 As Indicadores.Indicadores

    Private Sub salirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles salirToolStripMenuItem.Click
        Dim x As DialogResult = MessageBox.Show("Esta seguro de salir de la aplicación", "Conciliación", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

        If x = DialogResult.Yes Then
            Application.[Exit]()
        End If
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub OnReiniciarFormularioConciliacion(ByVal sender As Object, e As EventArgs)
        Dim frm As ConciliacionAmex = CType(sender, ConciliacionAmex)
        frm.Close()
        frm.Dispose()
        Dim frm2 As ConciliacionAmex = New ConciliacionAmex()
        AddHandler frm2.ReiniciarFormularioConciliacion, AddressOf OnReiniciarFormularioConciliacion
        frm2.ShowDialog(Me)
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
    End Sub


    Private Sub ConciliaciònAmexToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConciliaciònAmexToolStripMenuItem.Click
        ConciliacionForm = New ConciliacionAmex()
        AddHandler ConciliacionForm.ReiniciarFormularioConciliacion, AddressOf OnReiniciarFormularioConciliacion
        ConciliacionForm.ShowDialog(Me)
    End Sub

    Private Sub ConciliaciònEdoCuentaToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'Dim conciliacion_edo As New ConciliacionEdoCuenta
        'conciliacion_edo.ShowDialog(Me)
    End Sub

    Private Sub ConciliaciònToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConciliaciònToolStripMenuItem.Click

    End Sub



    Private Sub ConciliaciónClientesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConciliaciónClientesToolStripMenuItem.Click

        ConciliacionFormCH = New ConciliacionComisionesHoteles()
        AddHandler ConciliacionFormCH.ReiniciarFormularioConciliacionCH, AddressOf ReiniciarFormularioConciliacionCH
        ConciliacionFormCH.ShowDialog(Me)


    End Sub

    Private Sub ReiniciarFormularioConciliacionCH(ByVal sender As Object, e As EventArgs)
        Dim frm As ConciliacionComisionesHoteles = CType(sender, ConciliacionComisionesHoteles)
        frm.Close()
        frm.Dispose()
        Dim frm2 As ConciliacionComisionesHoteles = New ConciliacionComisionesHoteles()
        AddHandler frm2.ReiniciarFormularioConciliacionCH, AddressOf ReiniciarFormularioConciliacionCH
        frm2.ShowDialog(Me)
    End Sub

    Private Sub ConciliaciónIndicadoresToolStripMenuItem_Click(sender As Object, e As EventArgs)

        'ConciliacionFormI = New ConciliacionComisionesHotelesI()
        'AddHandler ConciliacionFormI.ReiniciarFormularioConciliacionFormI, AddressOf ReiniciarFormularioConciliacionFormI
        'ConciliacionFormI.ShowDialog(Me)

    End Sub

    Private Sub IndicadoresToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IndicadoresToolStripMenuItem.Click

        ConciliacionFormI1 = New Indicadores.Indicadores()
        ConciliacionFormI1.ShowDialog(Me)

    End Sub


End Class
