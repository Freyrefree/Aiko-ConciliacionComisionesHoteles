Imports System.Reflection
Imports System.Windows.Forms

Public Class AcercaDe

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AcercaDe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim _assembly As Assembly = Assembly.GetExecutingAssembly()
        Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(_assembly.Location)
        Dim Version As String = fvi.FileVersion
        Label2.Text = Version
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://www.aiko.com.mx")
    End Sub
End Class
