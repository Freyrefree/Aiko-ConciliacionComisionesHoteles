Imports System.Windows.Forms

Public Class AgregarConciliacion
    Private _CamposEstadoDeCuenta As List(Of String)
    Private _CamposIcaav As List(Of String)
    Private _ECSelectedIndex As Integer
    Private _ICSelectedIndex As Integer
    Public Event OnConciliacionCreada(sender As Object, e As CrearConciliacionEventArgs)
    Public Sub New(ByVal camposEstadoDeCuenta As List(Of String), ByVal camposIcaav As List(Of String), ByVal ECIndex As Integer, ByVal ICIndex As Integer)

        ' This call is required by the designer.
        InitializeComponent()
        _CamposEstadoDeCuenta = camposEstadoDeCuenta
        _CamposIcaav = camposIcaav
        _ECSelectedIndex = ECIndex
        _ICSelectedIndex = ICIndex
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim conciliacionEventArgs = New CrearConciliacionEventArgs()

        If cmbCamposEstadoDeCuenta.SelectedIndex <> -1 Then
            conciliacionEventArgs.CampoEstadoDeCuenta = cmbCamposEstadoDeCuenta.SelectedItem
        Else
            MessageBox.Show("Debe de seleccionar un campo del reporte Estado de Cuenta", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If cmbTipoDeDatos.SelectedIndex <> -1 Then
            conciliacionEventArgs.TipoDatos = cmbTipoDeDatos.SelectedItem
        Else
            MessageBox.Show("Debe de seleccionar el tipo de datos de la lista", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If cmbOperador.SelectedIndex <> -1 Then
            If cmbTipoDeDatos.SelectedItem = TiposDeDatos.NUMERICO And cmbOperador.SelectedItem = "IGUALDAD" Then
                conciliacionEventArgs.Operador = Operadores.NUMERICO_IGUAL
            End If
            If cmbTipoDeDatos.SelectedItem = TiposDeDatos.TEXTO And cmbOperador.SelectedItem = "IGUALDAD" Then
                conciliacionEventArgs.Operador = Operadores.TEXTO_IGUAL
            End If
            If cmbTipoDeDatos.SelectedItem = TiposDeDatos.TEXTO And cmbOperador.SelectedItem = "CONTIENE" Then
                conciliacionEventArgs.Operador = Operadores.TEXTO_CONTIENE
            End If
            If cmbTipoDeDatos.SelectedItem = TiposDeDatos.FECHA And cmbOperador.SelectedItem = "IGUALDAD" Then
                conciliacionEventArgs.Operador = Operadores.FECHA_IGUAL
            End If
            If cmbTipoDeDatos.SelectedItem = TiposDeDatos.MONEDA And cmbOperador.SelectedItem = "IGUALDAD" Then
                conciliacionEventArgs.Operador = Operadores.MONEDA_IGUAL
            End If
        Else
            MessageBox.Show("Debe de seleccionar el operador para la conciliación", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If cmbCamposIcaav.SelectedIndex <> -1 Then
            conciliacionEventArgs.CampoIcaav = cmbCamposIcaav.SelectedItem
        Else
            MessageBox.Show("Debe de seleccionar un campo del reporte Icaav", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        RaiseEvent OnConciliacionCreada(Me, conciliacionEventArgs)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AgregarConciliacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each campoEstadoDeCuenta In _CamposEstadoDeCuenta
            cmbCamposEstadoDeCuenta.Items.Add(campoEstadoDeCuenta)
        Next

        For Each campoIcaav In _CamposIcaav
            cmbCamposIcaav.Items.Add(campoIcaav)
        Next

        cmbCamposEstadoDeCuenta.SelectedIndex = _ECSelectedIndex
        cmbCamposIcaav.SelectedIndex = _ICSelectedIndex

        LlenarTipoDeDatos()
    End Sub

    Private Sub LlenarTipoDeDatos()
        cmbTipoDeDatos.Items.Clear()
        cmbTipoDeDatos.Items.Add(TiposDeDatos.NUMERICO)
        cmbTipoDeDatos.Items.Add(TiposDeDatos.TEXTO)
        cmbTipoDeDatos.Items.Add(TiposDeDatos.MONEDA)
        cmbTipoDeDatos.Items.Add(TiposDeDatos.FECHA)
        cmbTipoDeDatos.Text = "SELECCIONAR..."
    End Sub
    Private Sub LlenarOperador()
        cmbOperador.Items.Clear()
        If cmbTipoDeDatos.SelectedItem = TiposDeDatos.NUMERICO Then
            cmbOperador.Items.Add("IGUALDAD")
        End If
        If cmbTipoDeDatos.SelectedItem = TiposDeDatos.TEXTO Then
            cmbOperador.Items.Add("IGUALDAD")
            cmbOperador.Items.Add("CONTIENE")
        End If

        If cmbTipoDeDatos.SelectedItem = TiposDeDatos.FECHA Then
            cmbOperador.Items.Add("IGUALDAD")
        End If

        If cmbTipoDeDatos.SelectedItem = TiposDeDatos.MONEDA Then
            cmbOperador.Items.Add("IGUALDAD")
        End If
        cmbOperador.Text = "SELECCIONAR..."
    End Sub

    Private Sub cmbTipoDeDatos_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbTipoDeDatos.SelectionChangeCommitted
        cmbOperador.Enabled = True
        LlenarOperador()
    End Sub
End Class
