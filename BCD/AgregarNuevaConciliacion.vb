Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms

Public Class AgregarNuevaConciliacion
    Private _ColsPrimerReporte As List(Of String)
    Private _ColsSegundoReporte As List(Of String)
    Private _ListaGrupos As BindingList(Of GrupoConciliaciones)
    Public Event AgregarConciliacionAGrupo As EventHandler(Of CrearConciliacionEventArgs)
    Private _ErrorList As List(Of String)
    Private _GrupoSeleccionado As GrupoConciliaciones

    Public Sub New(ByVal ColsPrimerReporte As List(Of String), ByVal ColsSegundoReporte As List(Of String), ByVal ListaGrupos As BindingList(Of GrupoConciliaciones), ByVal gruposeleccionado As GrupoConciliaciones)

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        _ColsPrimerReporte = ColsPrimerReporte
        _ColsSegundoReporte = ColsSegundoReporte
        _ListaGrupos = ListaGrupos
        _ErrorList = New List(Of String)()
        _GrupoSeleccionado = gruposeleccionado
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAgregarNuevaConciliacion_Click(sender As Object, e As EventArgs) Handles btnAgregarNuevaConciliacion.Click
        If _ErrorList.Count > 0 Then
            Dim errorBuilder = New StringBuilder()
            errorBuilder.Append("SE HAN DETECTADO LOS SIGUIENTES ERRORES DE VALIDACIÓN:" & vbCrLf)
            For Each MensajeError In _ErrorList
                errorBuilder.Append(" - " & MensajeError & vbCrLf)
            Next
            If MessageBox.Show(errorBuilder.ToString(), "ERROR DE VALIDACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) = DialogResult.OK Then
                Exit Sub
            End If
        Else
            Dim _crearConciliacionEvtArgs = New CrearConciliacionEventArgs
            _crearConciliacionEvtArgs.CampoEstadoDeCuenta = lbxColumnasReporte1.SelectedItem.ToString()
            _crearConciliacionEvtArgs.CampoIcaav = lbxColumnaReporte2.SelectedItem.ToString()
            Select Case cmbTipoDatosNuevaConciliacion.SelectedIndex
                Case 0
                    _crearConciliacionEvtArgs.TipoDatos = TiposDeDatos.NUMERICO
                    If cmbTipoOperacionNuevaConciliacion.SelectedItem = "IGUALDAD" Then
                        _crearConciliacionEvtArgs.Operador = Operadores.NUMERICO_IGUAL
                    End If
                Case 1
                    _crearConciliacionEvtArgs.TipoDatos = TiposDeDatos.TEXTO
                    If cmbTipoOperacionNuevaConciliacion.SelectedItem = "IGUALDAD" Then
                        _crearConciliacionEvtArgs.Operador = Operadores.TEXTO_IGUAL
                    End If
                    If cmbTipoOperacionNuevaConciliacion.SelectedItem = "CONTIENE" Then
                        _crearConciliacionEvtArgs.Operador = Operadores.TEXTO_CONTIENE
                    End If
                Case 2
                    _crearConciliacionEvtArgs.TipoDatos = TiposDeDatos.MONEDA
                    If cmbTipoOperacionNuevaConciliacion.SelectedItem = "IGUALDAD" Then
                        _crearConciliacionEvtArgs.Operador = Operadores.MONEDA_IGUAL
                    End If
                Case 3
                    _crearConciliacionEvtArgs.TipoDatos = TiposDeDatos.FECHA
                    If cmbTipoOperacionNuevaConciliacion.SelectedItem = "IGUALDAD" Then
                        _crearConciliacionEvtArgs.Operador = Operadores.FECHA_IGUAL
                    End If
                Case Else
                    Exit Select
            End Select

            _crearConciliacionEvtArgs.Grupo = CType(cmbGrupoNuevaConciliacion.SelectedItem, GrupoConciliaciones)
            RaiseEvent AgregarConciliacionAGrupo(Me, _crearConciliacionEvtArgs)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If

    End Sub

    Public Sub ValidarCampos()
        _ErrorList.Clear()
        If lbxColumnasReporte1.SelectedIndex = -1 Then
            _ErrorList.Add("NO SE HA SELECCIONADO LA COLUMNA DEL PRIMER REPORTE")
        End If

        If lbxColumnaReporte2.SelectedIndex = -1 Then
            _ErrorList.Add("NO SE HA SELECCIONADO LA COLUMNA DEL SEGUNDO REPORTE")
        End If

        If cmbTipoDatosNuevaConciliacion.SelectedIndex = -1 Then
            _ErrorList.Add("NO SE HA SELECCIONADO EL TIPO DE DATOS PARA LA CONCILIACIÓN")
        End If

        If cmbTipoOperacionNuevaConciliacion.SelectedIndex = -1 Then
            _ErrorList.Add("NO SE HA SELECCIONADO EL GRUPO AL CUAL PERTENECERA LA CONCILIACIÓN")
        End If
    End Sub

    Private Sub AgregarNuevaConciliacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbxColumnasReporte1.DataSource = _ColsPrimerReporte
        lbxColumnaReporte2.DataSource = _ColsSegundoReporte
        cmbTipoDatosNuevaConciliacion.DataSource = Enumeraciones.ListaTiposDatos
        cmbGrupoNuevaConciliacion.DataSource = _ListaGrupos
        cmbGrupoNuevaConciliacion.SelectedItem = _GrupoSeleccionado

        If lbxColumnasReporte1.Items.Count <= 0 Or lbxColumnaReporte2.Items.Count <= 0 Then
            btnAgregarNuevaConciliacion.Enabled = False
        Else
            btnAgregarNuevaConciliacion.Enabled = True
        End If
    End Sub

    Private Sub cmbTipoDatosNuevaConciliacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoDatosNuevaConciliacion.SelectedIndexChanged
        Dim _comboBox As ComboBox = DirectCast(sender, ComboBox)
        If _comboBox.SelectedIndex <> -1 Then
            Select Case _comboBox.SelectedIndex
                Case 0
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresNumerico
                Case 1
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresTexto
                Case 2
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresMoneda
                Case 3
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresFecha
                Case Else
                    Exit Sub
            End Select
        End If

    End Sub

    Private Sub btnCancelarNuevaConciliacion_Click(sender As Object, e As EventArgs) Handles btnCancelarNuevaConciliacion.Click
        Me.Close()
    End Sub
End Class
