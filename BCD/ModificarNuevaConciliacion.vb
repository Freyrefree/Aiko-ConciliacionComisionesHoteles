Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms

Public Class ModificarNuevaConciliacion
    Private _ColsPrimerReporte As List(Of String)
    Private _ColsSegundoReporte As List(Of String)
    Private _ListaGrupos As BindingList(Of GrupoConciliaciones)
    Public Event ModificarConciliacionDeGrupo As EventHandler(Of CrearConciliacionEventArgs)
    Private _ErrorList As List(Of String)
    Private _GrupoSeleccionado As GrupoConciliaciones
    Private _Columna1 As String
    Private _Columna2 As String
    Private _TipoDeDatos As TiposDeDatos
    Private _TipoOperacion As Operadores
    Public Sub New(ByVal ColsPrimerReporte As List(Of String), ByVal ColsSegundoReporte As List(Of String), ByVal ListaGrupos As BindingList(Of GrupoConciliaciones), ByVal gruposeleccionado As GrupoConciliaciones, ByVal columna1 As String, ByVal columna2 As String, ByVal tipoDeDatos As TiposDeDatos, ByVal tipoOperacion As Operadores)

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        _ColsPrimerReporte = ColsPrimerReporte
        _ColsSegundoReporte = ColsSegundoReporte
         _ListaGrupos = ListaGrupos
        _ErrorList = New List(Of String)()
        _GrupoSeleccionado = gruposeleccionado
        _Columna1 = columna1
        _Columna2 = columna2
        _TipoDeDatos = tipoDeDatos
        _TipoOperacion = tipoOperacion
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
            RaiseEvent ModificarConciliacionDeGrupo(Me, _crearConciliacionEvtArgs)
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
        lbxColumnasReporte1.SelectedItem = _Columna1
        lbxColumnaReporte2.SelectedItem = _Columna2
        cmbTipoDatosNuevaConciliacion.DataSource = Enumeraciones.ListaTiposDatos
        If _TipoDeDatos = TiposDeDatos.NUMERICO Then
            cmbTipoDatosNuevaConciliacion.SelectedIndex = 0
        End If
        If _TipoDeDatos = TiposDeDatos.TEXTO Then
            cmbTipoDatosNuevaConciliacion.SelectedIndex = 1
        End If
        If _TipoDeDatos = TiposDeDatos.MONEDA Then
            cmbTipoDatosNuevaConciliacion.SelectedIndex = 2
        End If
        If _TipoDeDatos = TiposDeDatos.FECHA Then
            cmbTipoDatosNuevaConciliacion.SelectedIndex = 3
        End If

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
                    If _TipoOperacion = Operadores.NUMERICO_IGUAL Then
                        cmbTipoOperacionNuevaConciliacion.SelectedIndex = 0
                    End If
                Case 1
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresTexto
                    If _TipoOperacion = Operadores.TEXTO_IGUAL Then
                        cmbTipoOperacionNuevaConciliacion.SelectedIndex = 0
                    End If
                    If _TipoOperacion = Operadores.TEXTO_CONTIENE Then
                        cmbTipoOperacionNuevaConciliacion.SelectedIndex = 1
                    End If
                Case 2
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresMoneda
                    If _TipoOperacion = Operadores.MONEDA_IGUAL Then
                        cmbTipoOperacionNuevaConciliacion.SelectedIndex = 0
                    End If
                Case 3
                    cmbTipoOperacionNuevaConciliacion.DataSource = Nothing
                    cmbTipoOperacionNuevaConciliacion.DataSource = ListaTiposOperadoresFecha
                    If _TipoOperacion = Operadores.FECHA_IGUAL Then
                        cmbTipoOperacionNuevaConciliacion.SelectedIndex = 0
                    End If
                Case Else
                    Exit Sub
            End Select

        End If

    End Sub

    Private Sub btnCancelarNuevaConciliacion_Click(sender As Object, e As EventArgs) Handles btnCancelarNuevaConciliacion.Click
        Me.Close()
    End Sub
End Class
