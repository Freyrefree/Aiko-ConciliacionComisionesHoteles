
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports CapaNegocio.CapaNegocio
Imports DevExpress.Utils
Imports DevExpress.XtraCharts
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports Indicadores
Imports MoreLinq

Public Enum VISUALIZATION_TYPE
    SIN_SELECCIONAR = 0
    PORCENTAJE = 1
    CANTIDAD = 2
    MONTO = 3
End Enum

Public Class Indicadores

    Private Shared conciliacionesProvRepository As conciliacionesProveedores
    Public VisualizationTypeSelected As VISUALIZATION_TYPE
    Public Event OnVisualizationTypeChange As EventHandler(Of VisualizationTypeEventArgs)
    Private exportFilePath As String

    Dim seriesPorConciliacion As New Series("% DE CONCILIACIÓN", ViewType.Pie)
    Dim vistaPorConciliacion As PieSeriesView = CType(seriesPorConciliacion.View, PieSeriesView)

    Dim seriesConciliadas As New Series("RESERVAS CONCILIADAS", ViewType.Pie)
    Dim vistaConciliadas As PieSeriesView = CType(seriesConciliadas.View, PieSeriesView)

    Dim seriesNoConciliadasAutomatico As New Series("RESERVAS NO COINCILIADAS AUTOMATICO", ViewType.Pie)
    Dim vistaNoConciliadasAutomatico As PieSeriesView = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)

    Dim seriesNoConciliadasManual As New Series("RESERVAS NO CONCILIADAS MANUAL", ViewType.Pie)
    Dim vistaNoConciliadasManual As PieSeriesView = CType(seriesNoConciliadasManual.View, PieSeriesView)

    Dim seriesOnyxRepProvComisionesPagadas As New Series("ONYX - COMISIONES PAGADAS", ViewType.Pie)
    Dim vistaOnyxRepProvComisionesPagadas As PieSeriesView = CType(seriesOnyxRepProvComisionesPagadas.View, PieSeriesView)

    Dim seriesOnyxRepProvComisionesPendientesPago As New Series("ONYX - COMISIONES POR PAGAR", ViewType.Pie)
    Dim vistaOnyxRepProvComisionesPendientesPago As PieSeriesView = CType(seriesOnyxRepProvComisionesPendientesPago.View, PieSeriesView)

    Dim seriesOnyxRepProvComisionesConObservaciones As New Series("ONYX - CON OBSERVACIONES", ViewType.Pie)
    Dim vistaOnyxRepProvComisionesConObservaciones As PieSeriesView = CType(seriesOnyxRepProvComisionesConObservaciones.View, PieSeriesView)

    Dim seriesOnyxComPagReservacionesConciliadas As New Series("ONYX PAGADAS - RESERVACIONES CONCILIADAS", ViewType.Pie)
    Dim vistaOnyxComPagReservacionesConciliadas As PieSeriesView = CType(seriesOnyxComPagReservacionesConciliadas.View, PieSeriesView)

    Dim seriesOnyxComPagReservacionesNoConciliadas As New Series("ONYX PAGADAS - RESERVACIONES NO CONCILIADAS", ViewType.Pie)
    Dim vistaOnyxComPagReservacionesNoConciliadas As PieSeriesView = CType(seriesOnyxComPagReservacionesNoConciliadas.View, PieSeriesView)

    Dim seriesOnyxComPagPTA As New Series("ONYX PAGADAS - PTA", ViewType.Pie)
    Dim vistaOnyxComPagPTA As PieSeriesView = CType(seriesOnyxComPagPTA.View, PieSeriesView)

    Dim seriesOnyxComisionesConObservaciones As New Series("ONYX - CON OBSERVACIONES", ViewType.Pie)
    Dim vistaOnyxComisionesConObservaciones As PieSeriesView = CType(seriesOnyxComisionesConObservaciones.View, PieSeriesView)

    Dim seriesTacsComisionesConObservaciones As New Series("TACS - CON OBSERVACIONES", ViewType.Pie)
    Dim vistaTacsComisionesConObservaciones As PieSeriesView = CType(seriesTacsComisionesConObservaciones.View, PieSeriesView)

    Dim seriesOnyxComisionesPorPagarConfirmadas As New Series("ONYX - POR PAGAR-CONFIRMADAS", ViewType.Pie)
    Dim vistaOnyxComisionesPorPagarConfirmadas As PieSeriesView = CType(seriesOnyxComisionesPorPagarConfirmadas.View, PieSeriesView)

    Dim seriesTacsRepProvComisionesPagadas As New Series("TACS - COMISIONES PAGADAS", ViewType.Pie)
    Dim vistaTacsRepProvComisionesPagadas As PieSeriesView = CType(seriesTacsRepProvComisionesPagadas.View, PieSeriesView)

    Dim seriesTacsRepProvComisionesConObservaciones As New Series("TACS - CON OBSERVACIONES", ViewType.Pie)
    Dim vistaTacsRepProvComisionesConObservaciones As PieSeriesView = CType(seriesTacsRepProvComisionesConObservaciones.View, PieSeriesView)

    Dim seriesTacsComPagReservacionesConciliadas As New Series("TACS PAGADAS - RESERVACIONES CONCILIADAS", ViewType.Pie)
    Dim vistaTacsComPagReservacionesConciliadas As PieSeriesView = CType(seriesTacsComPagReservacionesConciliadas.View, PieSeriesView)

    Private objetoCN_Consultas As ClsN_ConsultasConciliaciones = New ClsN_ConsultasConciliaciones()


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        exportFilePath = String.Empty
        Dim configMap As ExeConfigurationFileMap = New ExeConfigurationFileMap() With {.ExeConfigFilename = "Indicadores.config"}
        Dim configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None)
        Dim connStringsSection = configuration.ConnectionStrings
        Dim connString = connStringsSection.ConnectionStrings("conciliacionesProveedores").ConnectionString
        conciliacionesProvRepository = New conciliacionesProveedores(connString)
    End Sub

    Private Sub Indicadores_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dateNow = DateTime.Now
        cmbProveedores.Properties.Items.AddRange(ENUM_PROVEEDORES.GetNames(GetType(ENUM_PROVEEDORES)))
        txtFechaInicio.DateTime = dateNow
        txtFechaFin.DateTime = dateNow

        VisualizationTypeSelected = VISUALIZATION_TYPE.PORCENTAJE

        AddHandler OnVisualizationTypeChange, New EventHandler(Of VisualizationTypeEventArgs)(AddressOf OnVisualizationTypeChangeEventHandler)

    End Sub

    Private Sub OnVisualizationTypeChangeEventHandler(sender As Object, e As VisualizationTypeEventArgs)
        VisualizationTypeSelected = e.visualizationType
    End Sub

    Private Sub ConsultaConciliacionesProveedor()
        Dim idProveedorGlobal = "1"
        objetoCN_Consultas.idProveedor = idProveedorGlobal
        gridListadoConciliaciones.DataSource = objetoCN_Consultas.CN_ConsultaConciliacionesByIdProveedor()
        gridDetalleConciliaciones.DataSource = objetoCN_Consultas.CN_ConsultaConciliacionesByIdProveedor()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs)
        If ChartPortentajeConciliacion.Series.Count = 0 Then
            Return
        End If
        vistaPorConciliacion.ExplodeMode = PieExplodeMode.UseFilters.MaxValue
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs)
        If ChartPortentajeConciliacion.Series.Count = 0 Then
            Return
        End If
        vistaPorConciliacion.ExplodeMode = PieExplodeMode.UseFilters.MinValue
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs)
        If ChartPortentajeConciliacion.Series.Count = 0 Then
            Return
        End If
        vistaPorConciliacion.ExplodeMode = PieExplodeMode.UseFilters.None
    End Sub

    Private Sub CheckEdit12_CheckedChanged(sender As Object, e As EventArgs)
        Dim visualizationArgs As VisualizationTypeEventArgs = New VisualizationTypeEventArgs(VISUALIZATION_TYPE.PORCENTAJE)
        RaiseEvent OnVisualizationTypeChange(Me, visualizationArgs)
    End Sub

    'Private Async Function UpdateInfoChart() As Task
    '    Dim ComboBoxSelectedEnumItem As ENUM_PROVEEDORES = Nothing
    '    Dim CheckCantidadCtrl As DevExpress.XtraEditors.CheckEdit = CType(sender, DevExpress.XtraEditors.CheckEdit)

    '    System.Enum.TryParse(CType(cmbProveedores.SelectedItem, String), ComboBoxSelectedEnumItem)

    '    If txtFechaInicio.DateTime > txtFechaFin.DateTime Then
    '        If MessageBox.Show("La fecha final no puede ser menor a la fecha inicial", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
    '            'Exit Function
    '        End If
    '    ElseIf ComboBoxSelectedEnumItem = 0 Then
    '        If MessageBox.Show("No se ha seleccionado ningun proveedor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
    '            Exit Function
    '        End If
    '    Else
    '        btnProcesar.Enabled = False
    '        CheckCantidadCtrl.Enabled = False

    '        If GeneralPorcentajeConciliacion.Checked = True Then
    '            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionPosadas(True)
    '                Else
    '                    Await ConciliacionPosadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then

    '                If checkPorCant.Checked Then
    '                    Await ConciliacionCityExpress(True)
    '                Else
    '                    Await ConciliacionCityExpress(False)
    '                End If

    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionGestionCommtrack(True)
    '                Else
    '                    Await ConciliacionGestionCommtrack(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyx(True)
    '                Else
    '                    Await ConciliacionOnyx(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionTacs(True)
    '                Else
    '                    Await ConciliacionTacs(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionGeneral(True)
    '                Else
    '                    Await ConciliacionGeneral(False)
    '                End If
    '            End If
    '        End If

    '        If GeneralReservasConciliadas.Checked = True Then
    '            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionPosadasReservasConciliadas(True)
    '                Else
    '                    Await ConciliacionPosadasReservasConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionCityExpressReservasConciliadas(True)
    '                Else
    '                    Await ConciliacionCityExpressReservasConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionGestionCommtrackReservasConciliadas(True)
    '                Else
    '                    Await ConciliacionGestionCommtrackReservasConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyxReservasConciliadas(True)
    '                Else
    '                    Await ConciliacionOnyxReservasConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionTacsReservasConciliadas(True)
    '                Else
    '                    Await ConciliacionTacsReservasConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionGeneralReservasConciliadas(True)
    '                Else
    '                    Await ConciliacionGeneralReservasConciliadas(False)
    '                End If
    '            End If
    '        End If

    '        If GeneralReservasNoConciliadas.Checked = True Then
    '            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionPosadasReservasNoConciliadas(True)
    '                Else
    '                    Await ConciliacionPosadasReservasNoConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionCityExpressReservasNoConciliadas(True)
    '                Else
    '                    Await ConciliacionCityExpressReservasNoConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionGestionCommtrackReservasNoConciliadas(True)
    '                Else
    '                    Await ConciliacionGestionCommtrackReservasNoConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyxReservasNoConciliadas(True)
    '                Else
    '                    Await ConciliacionOnyxReservasNoConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionTacsReservasNoConciliadas(True)
    '                Else
    '                    Await ConciliacionTacsReservasNoConciliadas(False)
    '                End If
    '            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionGeneralReservasNoConciliadas(True)
    '                Else
    '                    Await ConciliacionGeneralReservasNoConciliadas(False)
    '                End If

    '            End If
    '        End If

    '        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
    '            If OnyxReportadasPorProveedor.Checked Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyxReportadasPorProveedorComisionesPagadas(True)
    '                    Await ConciliacionOnyxReportadasPorProveedorComisionesPorPagar(True)
    '                    Await ConciliacionOnyxReportadasPorProveedorComisionesConObservaciones(True)
    '                Else
    '                    Await ConciliacionOnyxReportadasPorProveedorComisionesPagadas(False)
    '                    Await ConciliacionOnyxReportadasPorProveedorComisionesPorPagar(False)
    '                    Await ConciliacionOnyxReportadasPorProveedorComisionesConObservaciones(False)
    '                End If
    '            End If
    '        End If

    '        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
    '            If OnyxComisionesPagadas.Checked = True Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyxComPagReservacionesConciliadas(True)
    '                    Await ConciliacionOnyxComPagPTA(True)
    '                Else
    '                    Await ConciliacionOnyxComPagReservacionesConciliadas(False)
    '                    Await ConciliacionOnyxComPagPTA(False)
    '                End If
    '            End If

    '            If OnyxComisionesConObservaciones.Checked = True Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyxConObservaciones(True)
    '                Else
    '                    Await ConciliacionOnyxConObservaciones(False)
    '                End If
    '            End If

    '            If OnyxComisionesConfirmadas.Checked = True Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionOnyxComPorPagarConfirmadas(True)
    '                Else
    '                    Await ConciliacionOnyxComPorPagarConfirmadas(False)
    '                End If
    '            End If
    '        End If

    '        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
    '            If TacsReportadasPorProveedor.Checked = True Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionTacsReportadasPorProveedorComisionesPagadas(True)
    '                    Await ConciliacionTacsReportadasPorProveedorComisionesConObservaciones(True)
    '                Else
    '                    Await ConciliacionTacsReportadasPorProveedorComisionesPagadas(False)
    '                    Await ConciliacionTacsReportadasPorProveedorComisionesConObservaciones(False)
    '                End If
    '            End If

    '            If TacsComisionesPagadas.Checked = True Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionTacsComPagReservacionesConciliadas(True)
    '                Else
    '                    Await ConciliacionTacsComPagReservacionesConciliadas(False)
    '                End If
    '            End If

    '            If TacsComisionesConObservaciones.Checked = True Then
    '                If checkPorCant.Checked Then
    '                    Await ConciliacionTacsConObservaciones(True)
    '                Else
    '                    Await ConciliacionTacsConObservaciones(False)
    '                End If
    '            End If
    '        End If

    '        btnProcesar.Enabled = True
    '        CheckCantidadCtrl.Enabled = True
    '    End If
    'End Function

    Private Async Sub btnProcesar_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnProcesar.ItemClick

        Dim ComboBoxSelectedEnumItem As ENUM_PROVEEDORES = Nothing
        System.Enum.TryParse(CType(cmbProveedores.SelectedItem, String), ComboBoxSelectedEnumItem)

        If RadioRangoTiempo.Checked = True Then
            If txtFechaInicio.DateTime > txtFechaFin.DateTime Then
                If MessageBox.Show("La fecha final no puede ser menor a la fecha inicial", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
                    Exit Sub
                End If
            End If
        ElseIf ComboBoxSelectedEnumItem = 0 Then
            If MessageBox.Show("No se ha seleccionado ningun proveedor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
                Exit Sub
            End If
        ElseIf RadioPeriodo.Checked = True Then
            If Convert.ToString(CmbPeriodos.SelectedItem).Equals(String.Empty) Then
                If MessageBox.Show("Es necesario que seleccione un periodo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
                    Exit Sub
                End If
            End If
        End If

        btnProcesar.Enabled = True
        RadioRangoTiempo.Enabled = False
        txtFechaInicio.Enabled = False
        txtFechaFin.Enabled = False
        RadioPeriodo.Enabled = False
        CmbPeriodos.Enabled = False
        cmbProveedores.Enabled = False
        RadioPorcentaje.Enabled = False
        RadioCantidad.Enabled = False
        RadioMonto.Enabled = False
        RadioPendientesAuto.Enabled = False
        RadioPendientesManual.Enabled = False
        RadioPendientesAmbas.Enabled = False

        ProgressIndicator.Visible = True

        If GeneralPorcentajeConciliacion.Checked = True Then
            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
                Await ConciliacionPosadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
                Await ConciliacionCityExpress(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                Await ConciliacionGestionCommtrack(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
                Await ConciliacionOnyx(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
                Await ConciliacionTacs(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
                Await ConciliacionGeneral(VisualizationTypeSelected)
            End If
        End If

        If GeneralReservasConciliadas.Checked = True Then
            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
                Await ConciliacionPosadasReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
                Await ConciliacionCityExpressReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                Await ConciliacionGestionCommtrackReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
                Await ConciliacionOnyxReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
                Await ConciliacionTacsReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
                Await ConciliacionGeneralReservasConciliadas(VisualizationTypeSelected)
            End If
        End If

        If GeneralReservasNoConciliadas.Checked = True Then
            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
                Await ConciliacionPosadasReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
                Await ConciliacionCityExpressReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                Await ConciliacionGestionCommtrackReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
                Await ConciliacionOnyxReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
                Await ConciliacionTacsReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
                Await ConciliacionGeneralReservasNoConciliadas(VisualizationTypeSelected)
            End If
        End If

        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
            If OnyxReportadasPorProveedor.Checked = True Then
                Await ConciliacionOnyxReportadasPorProveedorComisionesPagadas(VisualizationTypeSelected)
                Await ConciliacionOnyxReportadasPorProveedorComisionesPorPagar(VisualizationTypeSelected)
            End If

            If OnyxComisionesPagadas.Checked = True Then
                Await ConciliacionOnyxComPagReservacionesConciliadas(VisualizationTypeSelected)
                Await ConciliacionOnyxComPagPTA(VisualizationTypeSelected)
            End If

            If OnyxComisionesConObservaciones.Checked = True Then
                Await ConciliacionOnyxConObservaciones(VisualizationTypeSelected)
            End If

            If OnyxComisionesConfirmadas.Checked = True Then
                Await ConciliacionOnyxComPorPagarConfirmadas(VisualizationTypeSelected)
            End If
        End If

        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
            If TacsReportadasPorProveedor.Checked = True Then
                Await ConciliacionTacsReportadasPorProveedorComisionesPagadas(VisualizationTypeSelected)
                Await ConciliacionTacsReportadasPorProveedorComisionesConObservaciones(VisualizationTypeSelected)
            End If

            If TacsComisionesPagadas.Checked = True Then
                Await ConciliacionTacsComPagReservacionesConciliadas(VisualizationTypeSelected)
            End If

            If TacsComisionesConObservaciones.Checked = True Then
                Await ConciliacionTacsConObservaciones(VisualizationTypeSelected)
            End If
        End If

        ProgressIndicator.Visible = False
        btnProcesar.Enabled = True
        RadioRangoTiempo.Enabled = True
        txtFechaInicio.Enabled = True
        txtFechaFin.Enabled = True
        RadioPeriodo.Enabled = True
        CmbPeriodos.Enabled = True
        cmbProveedores.Enabled = True
        RadioPorcentaje.Enabled = True
        RadioCantidad.Enabled = True
        RadioMonto.Enabled = True
        RadioPendientesAuto.Enabled = True
        RadioPendientesManual.Enabled = True
        RadioPendientesAmbas.Enabled = True
    End Sub

    Private Async Function ConciliacionOnyxReportadasPorProveedorComisionesPagadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IEnumerable(Of onyxPagadas) = Nothing
                           Dim TotalRegistros As IEnumerable(Of onyxPagadas) = Nothing

                           Dim TotalRegistrosGeneral As IEnumerable(Of onyx) = Nothing

                           Dim RegistrosEnFechaMonto As IEnumerable(Of conciliacionDetalleOnyx) = Nothing
                           Dim TotalRegistrosMonto As IEnumerable(Of conciliacionDetalleOnyx) = Nothing

                           Dim RegistrosEnFechaConObservaciones As IEnumerable(Of onyxObservaciones) = Nothing
                           Dim TotalRegistrosConObservaciones As IEnumerable(Of onyxObservaciones) = Nothing
                           Dim RegistrosEnFechaConObservacionesMonto As IEnumerable(Of onyxObservaciones) = Nothing
                           Dim TotalRegistrosConObservacionesMonto As IEnumerable(Of onyxObservaciones) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.Fechadepago <= FechaFinal).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.Fechadepago <= FechaFinal).ToList()
                               TotalRegistrosGeneral = conciliacionesProvRepository.onyx.Where(Function(x) x.mesProveedor >= FechaInicial And x.Fechadepago <= FechaFinal).ToList()
                               RegistrosEnFechaMonto = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()
                               TotalRegistrosMonto = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()

                               RegistrosEnFechaConObservaciones = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                               TotalRegistrosConObservaciones = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                               RegistrosEnFechaConObservacionesMonto = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                               TotalRegistrosConObservacionesMonto = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               TotalRegistrosGeneral = conciliacionesProvRepository.onyx.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               RegistrosEnFechaMonto = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp = Periodo).ToList()
                               TotalRegistrosMonto = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp = Periodo).ToList()

                               RegistrosEnFechaConObservaciones = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               TotalRegistrosConObservaciones = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               RegistrosEnFechaConObservacionesMonto = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               TotalRegistrosConObservacionesMonto = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo).ToList()
                           End If


                           If TotalRegistros.Count() = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosReportadosPorProveedor As Integer = RegistrosEnFecha.Count()
                           Dim RegistrosReportadosPorProveedorConObservaciones As Integer = RegistrosEnFechaConObservaciones.Count()

                           Dim PorcentajeRegistrosReportadosPorProveedor As Double = (RegistrosReportadosPorProveedor * 100D) / CType(TotalRegistrosGeneral.Count(), Double)
                           Dim PorcentajeTotal As Double = 100D - PorcentajeRegistrosReportadosPorProveedor

                           Dim MontosRegistrosReportadosPorProveedor As Double = RegistrosEnFechaMonto.ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision))
                           Dim MontosRegistrosTotales As Double = TotalRegistrosMonto.ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision))

                           Dim RegistrosConObservaciones As Integer = RegistrosEnFechaConObservaciones.Count()

                           Dim PorcentajeRegistrosConObservaciones As Double = (RegistrosConObservaciones * 100D) / CType(TotalRegistrosGeneral.Count(), Double)

                           Dim MontosRegistrosConObservaciones As Double = RegistrosEnFechaConObservacionesMonto.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim MontosRegistrosConObservacionesTotales As Double = TotalRegistrosConObservacionesMonto.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartOnyxRepProvComisionesPagadas.Series.Clear()
                                                     seriesOnyxRepProvComisionesPagadas.Points.Clear()
                                                     vistaOnyxRepProvComisionesPagadas.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", PorcentajeRegistrosReportadosPorProveedor))
                                                         seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX -COMISIONES CON OBSERVACIONES", PorcentajeRegistrosConObservaciones))
                                                         seriesOnyxRepProvComisionesPagadas.Label.TextPattern = "{A}: {VP:p2}"

                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", RegistrosReportadosPorProveedor))
                                                         seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES CON OBSERVACIONES", RegistrosConObservaciones))
                                                         seriesOnyxRepProvComisionesPagadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", MontosRegistrosReportadosPorProveedor))
                                                         seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES CON OBSERVACIONES", MontosRegistrosConObservaciones))
                                                         seriesOnyxRepProvComisionesPagadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesOnyxRepProvComisionesPagadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesOnyxRepProvComisionesPagadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaOnyxRepProvComisionesPagadas = CType(seriesOnyxRepProvComisionesPagadas.View, PieSeriesView)
                                                     vistaOnyxRepProvComisionesPagadas.Titles.Add(New SeriesTitle())
                                                     vistaOnyxRepProvComisionesPagadas.Titles(0).Text = seriesOnyxRepProvComisionesPagadas.Name
                                                     ChartOnyxRepProvComisionesPagadas.Legend.Visibility = DefaultBoolean.True
                                                     ChartOnyxRepProvComisionesPagadas.Series.Add(seriesOnyxRepProvComisionesPagadas)
                                                 End Sub))
                           Else
                               ChartOnyxRepProvComisionesPagadas.Series.Clear()
                               seriesOnyxRepProvComisionesPagadas.Points.Clear()
                               vistaOnyxRepProvComisionesPagadas.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", PorcentajeRegistrosReportadosPorProveedor))
                                   seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - TOTALES", PorcentajeTotal))
                                   seriesOnyxRepProvComisionesPagadas.Label.TextPattern = "{A}: {VP:p2}"

                                   seriesOnyxRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", PorcentajeRegistrosReportadosPorProveedor))
                                   seriesOnyxRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("ONYX - TOTALES", PorcentajeTotal))
                                   seriesOnyxRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", RegistrosReportadosPorProveedor))
                                   seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - TOTALES", TotalRegistros))
                                   seriesOnyxRepProvComisionesPagadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - COMISIONES POR PAGAR", MontosRegistrosReportadosPorProveedor))
                                   seriesOnyxRepProvComisionesPagadas.Points.Add(New SeriesPoint("ONYX - TOTALES", MontosRegistrosTotales))
                                   seriesOnyxRepProvComisionesPagadas.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesOnyxRepProvComisionesPagadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesOnyxRepProvComisionesPagadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaOnyxRepProvComisionesPagadas = CType(seriesOnyxRepProvComisionesPagadas.View, PieSeriesView)
                               vistaOnyxRepProvComisionesPagadas.Titles.Add(New SeriesTitle())
                               vistaOnyxRepProvComisionesPagadas.Titles(0).Text = seriesOnyxRepProvComisionesPagadas.Name
                               ChartOnyxRepProvComisionesPagadas.Legend.Visibility = DefaultBoolean.True
                               ChartOnyxRepProvComisionesPagadas.Series.Add(seriesOnyxRepProvComisionesPagadas)
                           End If

                       End Sub)
    End Function


    Private Async Function ConciliacionTacsReportadasPorProveedorComisionesPagadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IEnumerable(Of tacsPagadas) = Nothing
                           Dim TotalRegistros As Integer = 0
                           Dim MontoTotal As Decimal = 0.0D

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                               TotalRegistros = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).Count()
                               MontoTotal = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).Where(Function(x) x.PayCom IsNot Nothing).ToList().Sum(Function(x) ConvertStringToDecimal(x.PayCom.Value))
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               TotalRegistros = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor = Periodo).Count()
                               MontoTotal = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor = Periodo And x.PayCom IsNot Nothing).Sum(Function(x) x.PayCom.Value)
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosReportadosPorProveedor As Integer = RegistrosEnFecha.Count()

                           Dim PorcentajeRegistrosReportadosPorProveedor As Double = (RegistrosReportadosPorProveedor * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeTotal As Double = 100D - PorcentajeRegistrosReportadosPorProveedor

                           Dim RegistrosReportadosPorProveedorMonto As Double = RegistrosEnFecha.Sum(Function(x) x.PayComTC)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartTacsRepProvComisionesPagadas.Series.Clear()
                                                     seriesTacsRepProvComisionesPagadas.Points.Clear()
                                                     vistaTacsRepProvComisionesPagadas.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - COMISIONES PAGADAS", PorcentajeRegistrosReportadosPorProveedor))
                                                         seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - TOTALES", PorcentajeTotal))
                                                         seriesTacsRepProvComisionesPagadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - COMISIONES PAGADAS", RegistrosReportadosPorProveedor))
                                                         seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - TOTALES", TotalRegistros))
                                                         seriesTacsRepProvComisionesPagadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - COMISIONES PAGADAS", RegistrosReportadosPorProveedorMonto))
                                                         seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - TOTALES", MontoTotal))
                                                         seriesTacsRepProvComisionesPagadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesTacsRepProvComisionesPagadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesTacsRepProvComisionesPagadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaTacsRepProvComisionesPagadas = CType(seriesTacsRepProvComisionesPagadas.View, PieSeriesView)
                                                     vistaTacsRepProvComisionesPagadas.Titles.Add(New SeriesTitle())
                                                     vistaTacsRepProvComisionesPagadas.Titles(0).Text = seriesTacsRepProvComisionesPagadas.Name
                                                     ChartTacsRepProvComisionesPagadas.Legend.Visibility = DefaultBoolean.True
                                                     ChartTacsRepProvComisionesPagadas.Series.Add(seriesTacsRepProvComisionesPagadas)
                                                 End Sub))
                           Else
                               ChartTacsRepProvComisionesPagadas.Series.Clear()
                               seriesTacsRepProvComisionesPagadas.Points.Clear()
                               vistaTacsRepProvComisionesPagadas.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - COMISIONES PAGADAS", PorcentajeRegistrosReportadosPorProveedor))
                                   seriesTacsRepProvComisionesPagadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - COMISIONES PAGADAS", RegistrosReportadosPorProveedor))
                                   seriesTacsRepProvComisionesPagadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesTacsRepProvComisionesPagadas.Points.Add(New SeriesPoint("TACS - TOTALES", MontoTotal))
                                   seriesTacsRepProvComisionesPagadas.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesTacsRepProvComisionesPagadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesTacsRepProvComisionesPagadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaTacsRepProvComisionesPagadas = CType(seriesTacsRepProvComisionesPagadas.View, PieSeriesView)
                               vistaTacsRepProvComisionesPagadas.Titles.Add(New SeriesTitle())
                               vistaTacsRepProvComisionesPagadas.Titles(0).Text = seriesTacsRepProvComisionesPagadas.Name
                               ChartTacsRepProvComisionesPagadas.Legend.Visibility = DefaultBoolean.True
                               ChartTacsRepProvComisionesPagadas.Series.Add(seriesTacsRepProvComisionesPagadas)
                           End If

                       End Sub)
    End Function

    Private Sub AppendHtmlRow(ByRef html As StringBuilder, ByVal proveedor As String, ByVal fechaConfPago As String, ByVal fechaActual As String, ByVal dias As String, ByVal transacciones As String, ByVal montoDeComisionPendiente As String)
        html.Append(String.Format("<tr>" &
                                    "<td style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;"">{0}</td>" &
                                    "<td style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;"">{1}</td>" &
                                    "<td style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;"">{2}</td>" &
                                    "<td style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;"">{3}</td>" &
                                    "<td style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;"">{4}</td>" &
                                    "<td style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black; font-weight: bold; text-align: right;"">{5}</td>" &
                                  "</tr>",
                                  proveedor, fechaConfPago, fechaActual, dias, transacciones, montoDeComisionPendiente))
    End Sub

    Private Async Function ConciliacionOnyxReportadasPorProveedorComisionesPorPagar(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()

                           Dim ListaPeriodos = conciliacionesProvRepository.onyxComisionesPendientePago.Where(Function(x) x.fechaConfPago IsNot Nothing).GroupBy(Function(x) x.fechaConfPago).ToList()
                           Dim TrCollection As StringBuilder = New StringBuilder()
                           For i As Integer = 0 To ListaPeriodos.Count() - 1
                               Dim Periodo As Date = ListaPeriodos(i).Key.Value
                               Dim PeriodoTransacciones As Integer = ListaPeriodos(i).Count()
                               Dim MontoComisionPendiente As Decimal = 0

                               Dim AgrupacionTransaccionesMoneda = ListaPeriodos(i).GroupBy(Function(x) x.ConfCurrency).ToList()

                               For Each moneda In AgrupacionTransaccionesMoneda
                                   Dim sumaMoneas As Decimal = moneda.ToList().Sum(Function(x) ConvertStringToDecimal(x.ConfCostPrNight))

                                   Dim TipoMoneda As String = moneda.Key
                                   Dim MonedaInfo As moneda = conciliacionesProvRepository.moneda.Where(Function(x) x.codigo.ToUpper() = TipoMoneda.ToUpper()).FirstOrDefault()

                                   If MonedaInfo IsNot Nothing Then
                                       Dim TipoCambio = conciliacionesProvRepository.tipoCambio.Where(Function(x) x.fechaPeriodo = Periodo And x.idProveedor = 3).FirstOrDefault()
                                       If TipoCambio IsNot Nothing Then
                                           Dim TipoCambioDetalle = conciliacionesProvRepository.tipoCambioDetalle.Where(Function(x) x.idTipoCambio = TipoCambio.id And x.idMoneda = MonedaInfo.id).FirstOrDefault()
                                           If TipoCambioDetalle IsNot Nothing Then
                                               MontoComisionPendiente = MontoComisionPendiente + (moneda.Sum(Function(x) x.ConfCostPrNight * TipoCambioDetalle.valorMoneda * x.ConfNoNights * 0.1))
                                           End If
                                       End If
                                   End If
                               Next

                               Dim FechaActual As Date = DateTime.Now
                               If i = 0 Then
                                   AppendHtmlRow(TrCollection, "Onyx", Periodo.ToString("yyyy-MM-dd"), FechaActual.ToString("yyyy-MM-dd"), CInt((FechaActual - Periodo).TotalDays), PeriodoTransacciones, MontoComisionPendiente.ToString("C"))
                               Else
                                   AppendHtmlRow(TrCollection, "", Periodo.ToString("yyyy-MM-dd"), FechaActual.ToString("yyyy-MM-dd"), CInt((FechaActual - Periodo).TotalDays), PeriodoTransacciones, MontoComisionPendiente.ToString("C"))
                               End If
                           Next

                           Dim Template As String = "<html>" &
                                                        "<body>" &
                                                        "<table style=""border-collapse: collapse;border: 1px solid black;"">" &
                                                        "<tbody>" &
                                                        "<tr>" &
                                                        "<td nowrap style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;background: #BFBFBF; font-weight: bold;"">Proveedor</td>" &
                                                        "<td nowrap style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;background: #BFBFBF; font-weight: bold;"">FechaConfPago</td>" &
                                                        "<td nowrap style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;background: #BFBFBF; font-weight: bold;"">Fecha Actual</td>" &
                                                        "<td nowrap style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;background: #BFBFBF; font-weight: bold;"">Dias</td>" &
                                                        "<td nowrap style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;background: #BFBFBF; font-weight: bold;"">Transacciones</td>" &
                                                        "<td nowrap style=""padding-left: 5px; padding-right: 5px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif; border: 1px solid black;background: #BFBFBF; font-weight: bold;"">Monto de comision pendiente</td>" &
                                                        "</tr>" &
                                                        TrCollection.ToString() &
                                                        "</tbody>" &
                                                        "</table>" &
                                                        "</body>" &
                                                        "</html>"

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     TabuladorWebBrowser.DocumentText = Template
                                                 End Sub))
                           Else
                               TabuladorWebBrowser.DocumentText = Template
                           End If

                       End Sub)
    End Function

    Private Async Function ConciliacionTacsReportadasPorProveedorComisionesConObservaciones(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IEnumerable(Of tacsObservaciones) = Nothing
                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                               TotalRegistros = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor = Periodo).ToList()
                               TotalRegistros = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor = Periodo).Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosReportadosPorProveedor As Integer = RegistrosEnFecha.Count()
                           Dim RegistrosReportadosPorProveedorMonto As Decimal = RegistrosEnFecha.Sum(Function(x) x.PayCom)

                           Dim PorcentajeRegistrosReportadosPorProveedor As Double = (RegistrosReportadosPorProveedor * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeTotal As Double = (TotalRegistros * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartTacsRepProvConObservaciones.Series.Clear()
                                                     seriesTacsRepProvComisionesConObservaciones.Points.Clear()
                                                     vistaTacsRepProvComisionesConObservaciones.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesTacsRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("TACS - COMISIONES CON OBSERVACIONES", PorcentajeRegistrosReportadosPorProveedor))
                                                         seriesTacsRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesTacsRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("TACS - COMISIONES CON OBSERVACIONES", RegistrosReportadosPorProveedor))
                                                         seriesTacsRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesTacsRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("TACS - COMISIONES CON OBSERVACIONES", RegistrosReportadosPorProveedorMonto))
                                                         seriesTacsRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesTacsRepProvComisionesConObservaciones.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesTacsRepProvComisionesConObservaciones.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaTacsRepProvComisionesConObservaciones = CType(seriesTacsRepProvComisionesConObservaciones.View, PieSeriesView)
                                                     vistaTacsRepProvComisionesConObservaciones.Titles.Add(New SeriesTitle())
                                                     vistaTacsRepProvComisionesConObservaciones.Titles(0).Text = seriesTacsRepProvComisionesConObservaciones.Name
                                                     ChartTacsRepProvConObservaciones.Legend.Visibility = DefaultBoolean.True
                                                     ChartTacsRepProvConObservaciones.Series.Add(seriesTacsRepProvComisionesConObservaciones)
                                                 End Sub))
                           Else
                               ChartTacsRepProvConObservaciones.Series.Clear()
                               seriesTacsRepProvComisionesConObservaciones.Points.Clear()
                               vistaTacsRepProvComisionesConObservaciones.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesTacsRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("TACS - COMISIONES CON OBSERVACIONES", PorcentajeRegistrosReportadosPorProveedor))
                                   seriesTacsRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesTacsRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("TACS - COMISIONES CON OBSERVACIONES", RegistrosReportadosPorProveedor))
                                   seriesTacsRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesTacsRepProvComisionesConObservaciones.Points.Add(New SeriesPoint("TACS - COMISIONES CON OBSERVACIONES", RegistrosReportadosPorProveedorMonto))
                                   seriesTacsRepProvComisionesConObservaciones.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesTacsRepProvComisionesConObservaciones.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesTacsRepProvComisionesConObservaciones.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaTacsRepProvComisionesConObservaciones = CType(seriesTacsRepProvComisionesConObservaciones.View, PieSeriesView)
                               vistaTacsRepProvComisionesConObservaciones.Titles.Add(New SeriesTitle())
                               vistaTacsRepProvComisionesConObservaciones.Titles(0).Text = seriesTacsRepProvComisionesConObservaciones.Name
                               ChartTacsRepProvConObservaciones.Legend.Visibility = DefaultBoolean.True
                               ChartTacsRepProvConObservaciones.Series.Add(seriesTacsRepProvComisionesConObservaciones)
                           End If

                       End Sub)
    End Function



    Private Async Function ConciliacionOnyxComPagReservacionesConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosConciliadosEnFecha As IEnumerable(Of onyxPagadas) = Nothing
                           Dim RegistrosNoConciliadosEnFecha As IEnumerable(Of onyxPagadas) = Nothing
                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosConciliadosEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado = 1).ToList()
                               RegistrosNoConciliadosEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosConciliadosEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado = 1).ToList()
                               RegistrosNoConciliadosEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo).Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim CantidadRegistrosConciliados As Integer = RegistrosConciliadosEnFecha.Count()
                           Dim CantidadRegistrosNoConciliados As Integer = RegistrosNoConciliadosEnFecha.Count()

                           Dim CantidadRegistrosConciliadosMonto As Integer = RegistrosConciliadosEnFecha.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim CantidadRegistrosNoConciliadosMonto As Integer = RegistrosNoConciliadosEnFecha.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))


                           Dim PorcentajeRegistrosConciliados As Double = (CantidadRegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (CantidadRegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartOnyxComPagReservacionesConciliadas.Series.Clear()
                                                     seriesOnyxComPagReservacionesConciliadas.Points.Clear()
                                                     vistaOnyxComPagReservacionesConciliadas.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesOnyxComPagReservacionesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES CONCILIADAS", CantidadRegistrosConciliados))
                                                         seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES NO CONCILIADAS", CantidadRegistrosNoConciliados))
                                                         seriesOnyxComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES CONCILIADAS", CantidadRegistrosConciliadosMonto))
                                                         seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES NO CONCILIADAS", CantidadRegistrosNoConciliadosMonto))
                                                         seriesOnyxComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesOnyxComPagReservacionesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesOnyxComPagReservacionesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaOnyxComPagReservacionesConciliadas = CType(seriesOnyxComPagReservacionesConciliadas.View, PieSeriesView)
                                                     vistaOnyxComPagReservacionesConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaOnyxComPagReservacionesConciliadas.Titles(0).Text = seriesOnyxComPagReservacionesConciliadas.Name
                                                     ChartOnyxComPagReservacionesConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     ChartOnyxComPagReservacionesConciliadas.Series.Add(seriesOnyxComPagReservacionesConciliadas)
                                                 End Sub))
                           Else
                               ChartOnyxComPagReservacionesConciliadas.Series.Clear()
                               seriesOnyxComPagReservacionesConciliadas.Points.Clear()
                               vistaOnyxComPagReservacionesConciliadas.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesOnyxComPagReservacionesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES CONCILIADAS", CantidadRegistrosConciliados))
                                   seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES NO CONCILIADAS", CantidadRegistrosNoConciliados))
                                   seriesOnyxComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES CONCILIADAS", CantidadRegistrosConciliadosMonto))
                                   seriesOnyxComPagReservacionesConciliadas.Points.Add(New SeriesPoint("RESERVACIONES NO CONCILIADAS", CantidadRegistrosNoConciliadosMonto))
                                   seriesOnyxComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesOnyxComPagReservacionesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesOnyxComPagReservacionesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaOnyxComPagReservacionesConciliadas = CType(seriesOnyxComPagReservacionesConciliadas.View, PieSeriesView)
                               vistaOnyxComPagReservacionesConciliadas.Titles.Add(New SeriesTitle())
                               vistaOnyxComPagReservacionesConciliadas.Titles(0).Text = seriesOnyxComPagReservacionesConciliadas.Name
                               ChartOnyxComPagReservacionesConciliadas.Legend.Visibility = DefaultBoolean.True
                               ChartOnyxComPagReservacionesConciliadas.Series.Add(seriesOnyxComPagReservacionesConciliadas)
                           End If

                       End Sub)
    End Function



    ' TACS RESERVAS CONCILIADAS

    Private Async Function ConciliacionTacsComPagReservacionesConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosConciliadosEnFecha As IEnumerable(Of tacsPagadas) = Nothing
                           Dim RegistrosNoConciliadosEnFecha As IEnumerable(Of tacsPagadas) = Nothing
                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosConciliadosEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado = 1).ToList()
                               RegistrosNoConciliadosEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosConciliadosEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado = 1).ToList()
                               RegistrosNoConciliadosEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor = Periodo).Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim CantidadRegistrosConciliados As Integer = RegistrosConciliadosEnFecha.Count()
                           Dim CantidadRegistrosNoConciliados As Integer = RegistrosNoConciliadosEnFecha.Count()

                           Dim CantidadRegistrosConciliadosMonto As Integer = RegistrosConciliadosEnFecha.Sum(Function(x) x.PayComTC)
                           Dim CantidadRegistrosNoConciliadosMonto As Integer = RegistrosNoConciliadosEnFecha.Sum(Function(x) x.PayComTC)

                           Dim PorcentajeRegistrosConciliados As Double = (CantidadRegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (CantidadRegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartTacsComPagReservacionesConciliadas.Series.Clear()
                                                     seriesTacsComPagReservacionesConciliadas.Points.Clear()
                                                     vistaTacsComPagReservacionesConciliadas.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS CONCILIADOS", PorcentajeRegistrosConciliados))
                                                         seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS NO CONCILIADOS", PorcentajeRegistrosNoConciliados))
                                                         seriesTacsComPagReservacionesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS CONCILIADOS", CantidadRegistrosConciliados))
                                                         seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS NO CONCILIADOS", CantidadRegistrosNoConciliados))
                                                         seriesTacsComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS CONCILIADOS", CantidadRegistrosConciliadosMonto))
                                                         seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS NO CONCILIADOS", CantidadRegistrosNoConciliadosMonto))
                                                         seriesTacsComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesTacsComPagReservacionesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesTacsComPagReservacionesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaTacsComPagReservacionesConciliadas = CType(seriesTacsComPagReservacionesConciliadas.View, PieSeriesView)
                                                     vistaTacsComPagReservacionesConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaTacsComPagReservacionesConciliadas.Titles(0).Text = seriesTacsComPagReservacionesConciliadas.Name
                                                     ChartTacsComPagReservacionesConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     ChartTacsComPagReservacionesConciliadas.Series.Add(seriesTacsComPagReservacionesConciliadas)
                                                 End Sub))
                           Else
                               ChartTacsComPagReservacionesConciliadas.Series.Clear()
                               seriesTacsComPagReservacionesConciliadas.Points.Clear()
                               vistaTacsComPagReservacionesConciliadas.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS CONCILIADOS", PorcentajeRegistrosConciliados))
                                   seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS NO CONCILIADOS", PorcentajeRegistrosNoConciliados))
                                   seriesTacsComPagReservacionesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS CONCILIADOS", CantidadRegistrosConciliados))
                                   seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS NO CONCILIADOS", CantidadRegistrosNoConciliados))
                                   seriesTacsComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS CONCILIADOS", CantidadRegistrosConciliadosMonto))
                                   seriesTacsComPagReservacionesConciliadas.Points.Add(New SeriesPoint("REGISTROS NO CONCILIADOS", CantidadRegistrosNoConciliadosMonto))
                                   seriesTacsComPagReservacionesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesTacsComPagReservacionesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesTacsComPagReservacionesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaTacsComPagReservacionesConciliadas = CType(seriesTacsComPagReservacionesConciliadas.View, PieSeriesView)
                               vistaTacsComPagReservacionesConciliadas.Titles.Add(New SeriesTitle())
                               vistaTacsComPagReservacionesConciliadas.Titles(0).Text = seriesTacsComPagReservacionesConciliadas.Name
                               ChartTacsComPagReservacionesConciliadas.Legend.Visibility = DefaultBoolean.True
                               ChartTacsComPagReservacionesConciliadas.Series.Add(seriesTacsComPagReservacionesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionOnyxComPagPTA(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaPEP As IEnumerable(Of onyxPagadas) = Nothing
                           Dim RegistrosEnFechaPNT As IEnumerable(Of onyxPagadas) = Nothing
                           Dim RegistrosEnFechaPTA As IEnumerable(Of onyxPagadas) = Nothing

                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaPEP = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidStatus = "PEP" And x.PaidCommission IsNot Nothing).ToList()
                               RegistrosEnFechaPNT = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidStatus = "PNT" And x.PaidCommission IsNot Nothing).ToList()
                               RegistrosEnFechaPTA = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidStatus = "PTA" And x.PaidCommission IsNot Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidCommission IsNot Nothing).Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaPEP = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo And x.PaidStatus = "PEP" And x.PaidCommission IsNot Nothing).ToList()
                               RegistrosEnFechaPNT = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo And x.PaidStatus = "PNT" And x.PaidCommission IsNot Nothing).ToList()
                               RegistrosEnFechaPTA = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo And x.PaidStatus = "PTA" And x.PaidCommission IsNot Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo And x.PaidCommission IsNot Nothing).Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosPagadosPEP As Integer = RegistrosEnFechaPEP.Count()
                           Dim RegistrosPagadosPNT As Integer = RegistrosEnFechaPNT.Count()
                           Dim RegistrosPagadosPTA As Integer = RegistrosEnFechaPTA.Count()

                           Dim RegistrosPagadosPEPMonto As Integer = RegistrosEnFechaPEP.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim RegistrosPagadosPNTMonto As Integer = RegistrosEnFechaPNT.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim RegistrosPagadosPTAMonto As Integer = RegistrosEnFechaPTA.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))


                           Dim PorcentajePagadosPEP As Double = (RegistrosPagadosPEP * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosPNT As Double = (RegistrosPagadosPNT * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosPTA As Double = (RegistrosPagadosPTA * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosTotal As Double = (TotalRegistros * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartOnyxComPagPTA.Series.Clear()
                                                     seriesOnyxComPagPTA.Points.Clear()
                                                     vistaOnyxComPagPTA.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PEP", PorcentajePagadosPEP))
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PNT", PorcentajePagadosPNT))
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PTA", PorcentajePagadosPTA))
                                                         seriesOnyxComPagPTA.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PEP", RegistrosPagadosPEP))
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PNT", RegistrosPagadosPNT))
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PTA", RegistrosPagadosPTA))
                                                         seriesOnyxComPagPTA.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PEP", RegistrosPagadosPEPMonto))
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PNT", RegistrosPagadosPNTMonto))
                                                         seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PTA", RegistrosPagadosPTAMonto))
                                                         seriesOnyxComPagPTA.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesOnyxComPagPTA.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesOnyxComPagPTA.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaOnyxComPagPTA = CType(seriesOnyxComPagPTA.View, PieSeriesView)
                                                     vistaOnyxComPagPTA.Titles.Add(New SeriesTitle())
                                                     vistaOnyxComPagPTA.Titles(0).Text = seriesOnyxComPagPTA.Name
                                                     ChartOnyxComPagPTA.Legend.Visibility = DefaultBoolean.True
                                                     ChartOnyxComPagPTA.Series.Add(seriesOnyxComPagPTA)
                                                 End Sub))
                           Else
                               ChartOnyxComPagPTA.Series.Clear()
                               seriesOnyxComPagPTA.Points.Clear()
                               vistaOnyxComPagPTA.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PEP", PorcentajePagadosPEP))
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PNT", PorcentajePagadosPNT))
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PTA", PorcentajePagadosPTA))
                                   seriesOnyxComPagPTA.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PEP", RegistrosPagadosPEP))
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PNT", RegistrosPagadosPNT))
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PTA", RegistrosPagadosPTA))
                                   seriesOnyxComPagPTA.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PEP", RegistrosPagadosPEPMonto))
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PNT", RegistrosPagadosPNTMonto))
                                   seriesOnyxComPagPTA.Points.Add(New SeriesPoint("PAGADAS - PTA", RegistrosPagadosPTAMonto))
                                   seriesOnyxComPagPTA.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesOnyxComPagPTA.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesOnyxComPagPTA.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaOnyxComPagPTA = CType(seriesOnyxComPagPTA.View, PieSeriesView)
                               vistaOnyxComPagPTA.Titles.Add(New SeriesTitle())
                               vistaOnyxComPagPTA.Titles(0).Text = seriesOnyxComPagPTA.Name
                               ChartOnyxComPagPTA.Legend.Visibility = DefaultBoolean.True
                               ChartOnyxComPagPTA.Series.Add(seriesOnyxComPagPTA)
                           End If

                       End Sub)
    End Function


    Private Async Function ConciliacionOnyxConObservaciones(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNPD As IEnumerable(Of onyxObservaciones) = Nothing
                           Dim RegistrosEnFechaPNT As IEnumerable(Of onyxObservaciones) = Nothing
                           Dim RegistrosEnFechaPTA As IEnumerable(Of onyxObservaciones) = Nothing

                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNPD = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidStatus = "NPD" And x.PaidCommission Is Nothing).ToList()
                               RegistrosEnFechaPNT = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidStatus = "PNT" And x.PaidCommission Is Nothing).ToList()
                               RegistrosEnFechaPTA = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidStatus = "PTA" And x.PaidCommission Is Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.PaidCommission IsNot Nothing).Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNPD = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.PaidStatus = "NPD" And x.PaidCommission Is Nothing).ToList()
                               RegistrosEnFechaPNT = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.PaidStatus = "PNT" And x.PaidCommission Is Nothing).ToList()
                               RegistrosEnFechaPTA = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.PaidStatus = "PTA" And x.PaidCommission Is Nothing).ToList()
                               TotalRegistros = conciliacionesProvRepository.onyxObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.PaidCommission Is Nothing).Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosPagadosNPD As Integer = RegistrosEnFechaNPD.Count()
                           Dim RegistrosPagadosPNT As Integer = RegistrosEnFechaPNT.Count()
                           Dim RegistrosPagadosPTA As Integer = RegistrosEnFechaPTA.Count()

                           Dim RegistrosPagadosNPDMonto As Decimal = RegistrosEnFechaNPD.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim RegistrosPagadosPNTMonto As Decimal = RegistrosEnFechaPNT.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim RegistrosPagadosPTAMonto As Decimal = RegistrosEnFechaPTA.ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))

                           Dim PorcentajePagadosNPD As Double = (RegistrosPagadosNPD * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosPNT As Double = (RegistrosPagadosPNT * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosPTA As Double = (RegistrosPagadosPTA * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartOnyxConObservaciones.Series.Clear()
                                                     seriesOnyxComisionesConObservaciones.Points.Clear()
                                                     vistaOnyxComisionesConObservaciones.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("NPD", PorcentajePagadosNPD))
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PNT", PorcentajePagadosPNT))
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PTA", PorcentajePagadosPTA))
                                                         seriesOnyxComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("NPD", RegistrosPagadosNPD))
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PNT", RegistrosPagadosPNT))
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PTA", RegistrosPagadosPTA))
                                                         seriesOnyxComisionesConObservaciones.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("NPD", RegistrosPagadosNPDMonto))
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PNT", RegistrosPagadosPNTMonto))
                                                         seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PTA", RegistrosPagadosPTAMonto))
                                                         seriesOnyxComisionesConObservaciones.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesOnyxComisionesConObservaciones.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesOnyxComisionesConObservaciones.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaOnyxComisionesConObservaciones = CType(seriesOnyxComisionesConObservaciones.View, PieSeriesView)
                                                     vistaOnyxComisionesConObservaciones.Titles.Add(New SeriesTitle())
                                                     vistaOnyxComisionesConObservaciones.Titles(0).Text = seriesOnyxComisionesConObservaciones.Name
                                                     ChartOnyxConObservaciones.Legend.Visibility = DefaultBoolean.True
                                                     ChartOnyxConObservaciones.Series.Add(seriesOnyxComisionesConObservaciones)
                                                 End Sub))
                           Else
                               ChartOnyxConObservaciones.Series.Clear()
                               seriesOnyxComisionesConObservaciones.Points.Clear()
                               vistaOnyxComisionesConObservaciones.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("NPD", PorcentajePagadosNPD))
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PNT", PorcentajePagadosPNT))
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PTA", PorcentajePagadosPTA))
                                   seriesOnyxComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("NPD", RegistrosPagadosNPD))
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PNT", RegistrosPagadosPNT))
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PTA", RegistrosPagadosPTA))
                                   seriesOnyxComisionesConObservaciones.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("NPD", RegistrosPagadosNPDMonto))
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PNT", RegistrosPagadosPNTMonto))
                                   seriesOnyxComisionesConObservaciones.Points.Add(New SeriesPoint("PTA", RegistrosPagadosPTAMonto))
                                   seriesOnyxComisionesConObservaciones.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesOnyxComisionesConObservaciones.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesOnyxComisionesConObservaciones.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaOnyxComisionesConObservaciones = CType(seriesOnyxComisionesConObservaciones.View, PieSeriesView)
                               vistaOnyxComisionesConObservaciones.Titles.Add(New SeriesTitle())
                               vistaOnyxComisionesConObservaciones.Titles(0).Text = seriesOnyxComisionesConObservaciones.Name
                               ChartOnyxConObservaciones.Legend.Visibility = DefaultBoolean.True
                               ChartOnyxConObservaciones.Series.Add(seriesOnyxComisionesConObservaciones)
                           End If

                       End Sub)
    End Function



    Private Async Function ConciliacionTacsConObservaciones(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNS As IEnumerable(Of tacsObservaciones) = Nothing
                           Dim RegistrosEnFechaNC As IEnumerable(Of tacsObservaciones) = Nothing
                           Dim RegistrosEnFechaNA As IEnumerable(Of tacsObservaciones) = Nothing

                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNS = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.TxnCd = "NS").ToList()
                               RegistrosEnFechaNC = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.TxnCd = "NC").ToList()
                               RegistrosEnFechaNA = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.TxnCd = "NA").ToList()
                               TotalRegistros = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNS = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.TxnCd = "NS").ToList()
                               RegistrosEnFechaNC = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.TxnCd = "NC").ToList()
                               RegistrosEnFechaNA = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor = Periodo And x.TxnCd = "NA").ToList()
                               TotalRegistros = conciliacionesProvRepository.tacsObservaciones.Where(Function(x) x.mesProveedor = Periodo).Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosPagadosNS As Integer = RegistrosEnFechaNS.Count()
                           Dim RegistrosPagadosNC As Integer = RegistrosEnFechaNC.Count()
                           Dim RegistrosPagadosNA As Integer = RegistrosEnFechaNA.Count()

                           Dim RegistrosPagadosNSMonto As Integer = RegistrosEnFechaNS.Sum(Function(x) x.PayCom)
                           Dim RegistrosPagadosNCMonto As Integer = RegistrosEnFechaNC.Sum(Function(x) x.PayCom)
                           Dim RegistrosPagadosNAMonto As Integer = RegistrosEnFechaNA.Sum(Function(x) x.PayCom)

                           Dim PorcentajePagadosNS As Double = (RegistrosPagadosNS * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosNC As Double = (RegistrosPagadosNC * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajePagadosNA As Double = (RegistrosPagadosNA * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartTacsConObservaciones.Series.Clear()
                                                     seriesTacsComisionesConObservaciones.Points.Clear()
                                                     vistaTacsComisionesConObservaciones.Titles.Clear()

                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NS", PorcentajePagadosNS))
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NC", PorcentajePagadosNC))
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NA", PorcentajePagadosNA))
                                                         seriesTacsComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NS", RegistrosPagadosNS))
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NC", RegistrosPagadosNC))
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NA", RegistrosPagadosNA))
                                                         seriesTacsComisionesConObservaciones.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NS", RegistrosPagadosNSMonto))
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NC", RegistrosPagadosNCMonto))
                                                         seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NA", RegistrosPagadosNAMonto))
                                                         seriesTacsComisionesConObservaciones.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesTacsComisionesConObservaciones.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesTacsComisionesConObservaciones.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaTacsComisionesConObservaciones = CType(seriesTacsComisionesConObservaciones.View, PieSeriesView)
                                                     vistaTacsComisionesConObservaciones.Titles.Add(New SeriesTitle())
                                                     vistaTacsComisionesConObservaciones.Titles(0).Text = seriesTacsComisionesConObservaciones.Name
                                                     ChartTacsConObservaciones.Legend.Visibility = DefaultBoolean.True
                                                     ChartTacsConObservaciones.Series.Add(seriesTacsComisionesConObservaciones)
                                                 End Sub))
                           Else
                               ChartTacsConObservaciones.Series.Clear()
                               seriesTacsComisionesConObservaciones.Points.Clear()
                               vistaTacsComisionesConObservaciones.Titles.Clear()

                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NS", PorcentajePagadosNS))
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NC", PorcentajePagadosNC))
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NA", PorcentajePagadosNA))
                                   seriesTacsComisionesConObservaciones.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NS", RegistrosPagadosNS))
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NC", RegistrosPagadosNC))
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NA", RegistrosPagadosNA))
                                   seriesTacsComisionesConObservaciones.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NS", RegistrosPagadosNSMonto))
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NC", RegistrosPagadosNCMonto))
                                   seriesTacsComisionesConObservaciones.Points.Add(New SeriesPoint("NA", RegistrosPagadosNAMonto))
                                   seriesTacsComisionesConObservaciones.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesTacsComisionesConObservaciones.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesTacsComisionesConObservaciones.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaTacsComisionesConObservaciones = CType(seriesTacsComisionesConObservaciones.View, PieSeriesView)
                               vistaTacsComisionesConObservaciones.Titles.Add(New SeriesTitle())
                               vistaTacsComisionesConObservaciones.Titles(0).Text = seriesTacsComisionesConObservaciones.Name
                               ChartTacsConObservaciones.Legend.Visibility = DefaultBoolean.True
                               ChartTacsConObservaciones.Series.Add(seriesTacsComisionesConObservaciones)
                           End If

                       End Sub)
    End Function


    Private Async Function ConciliacionOnyxComPorPagarConfirmadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IEnumerable(Of onyxComisionesPendientePago) = Nothing

                           Dim TotalRegistros As Integer = 0

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.onyxComisionesPendientePago.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.Fechadepago Is Nothing).ToList()
                               TotalRegistros = RegistrosEnFecha.Count()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.onyxComisionesPendientePago.Where(Function(x) x.mesProveedor = Periodo And x.Fechadepago Is Nothing).ToList()
                               TotalRegistros = RegistrosEnFecha.Count()
                           End If


                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           ' Obtenemos distinct de periodos

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartOnyxComPorPagarConfirmadas.Series.Clear()
                                                     seriesOnyxComisionesPorPagarConfirmadas.Points.Clear()
                                                     vistaOnyxComisionesPorPagarConfirmadas.Titles.Clear()
                                                 End Sub))
                           Else
                               ChartOnyxComPorPagarConfirmadas.Series.Clear()
                               seriesOnyxComisionesPorPagarConfirmadas.Points.Clear()
                               vistaOnyxComisionesPorPagarConfirmadas.Titles.Clear()
                           End If

                           Dim ListaPeriodos = RegistrosEnFecha.Where(Function(x) x.fechaConfPago IsNot Nothing).GroupBy(Function(x) x.fechaConfPago.Value)

                           For Each mPeriodo In ListaPeriodos
                               Dim PeriodoString = mPeriodo.Key.ToString("yyyy-MM-dd")
                               Dim CantidadRegistrosPeriodo = mPeriodo.Count()
                               Dim CantidadRegistrosPeriodoMonto As Decimal = mPeriodo.Sum(Function(x) x.PaidCommissionMXN)
                               Dim PorcentajeRegistrosPeriodo As Double = (CantidadRegistrosPeriodo * 100D) / CType(TotalRegistros, Double)
                               Dim DiasDeRetraso As Integer = (DateTime.Now - mPeriodo.Key).TotalDays
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesOnyxComisionesPorPagarConfirmadas.Points.Add(New SeriesPoint(String.Format("{0} con {1} dias de antiguedad", PeriodoString, DiasDeRetraso.ToString()), PorcentajeRegistrosPeriodo))
                                                             seriesOnyxComisionesPorPagarConfirmadas.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesOnyxComisionesPorPagarConfirmadas.Points.Add(New SeriesPoint(String.Format("{0} con {1} dias de antiguedad", PeriodoString, DiasDeRetraso.ToString()), CantidadRegistrosPeriodo))
                                                             seriesOnyxComisionesPorPagarConfirmadas.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesOnyxComisionesPorPagarConfirmadas.Points.Add(New SeriesPoint(String.Format("{0} con {1} dias de antiguedad", PeriodoString, DiasDeRetraso.ToString()), CantidadRegistrosPeriodoMonto))
                                                             seriesOnyxComisionesPorPagarConfirmadas.Label.TextPattern = "{A}: {V:c2}"
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesOnyxComisionesPorPagarConfirmadas.Points.Add(New SeriesPoint(String.Format("{0} con {1} dias de antiguedad", PeriodoString, DiasDeRetraso.ToString()), PorcentajeRegistrosPeriodo))
                                       seriesOnyxComisionesPorPagarConfirmadas.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesOnyxComisionesPorPagarConfirmadas.Points.Add(New SeriesPoint(String.Format("{0} con {1} dias de antiguedad", PeriodoString, DiasDeRetraso.ToString()), CantidadRegistrosPeriodo))
                                       seriesOnyxComisionesPorPagarConfirmadas.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesOnyxComisionesPorPagarConfirmadas.Points.Add(New SeriesPoint(String.Format("{0} con {1} dias de antiguedad", PeriodoString, DiasDeRetraso.ToString()), CantidadRegistrosPeriodoMonto))
                                       seriesOnyxComisionesPorPagarConfirmadas.Label.TextPattern = "{A}: {V:c2}"
                                   End If
                               End If

                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CType(seriesOnyxComisionesPorPagarConfirmadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesOnyxComisionesPorPagarConfirmadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaOnyxComisionesPorPagarConfirmadas = CType(seriesOnyxComisionesPorPagarConfirmadas.View, PieSeriesView)
                                                     vistaOnyxComisionesPorPagarConfirmadas.Titles.Add(New SeriesTitle())
                                                     vistaOnyxComisionesPorPagarConfirmadas.Titles(0).Text = seriesOnyxComisionesPorPagarConfirmadas.Name
                                                     ChartOnyxComPorPagarConfirmadas.Legend.Visibility = DefaultBoolean.True
                                                     ChartOnyxComPorPagarConfirmadas.Series.Add(seriesOnyxComisionesPorPagarConfirmadas)
                                                 End Sub))
                           Else
                               CType(seriesOnyxComisionesPorPagarConfirmadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesOnyxComisionesPorPagarConfirmadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaOnyxComisionesPorPagarConfirmadas = CType(seriesOnyxComisionesPorPagarConfirmadas.View, PieSeriesView)
                               vistaOnyxComisionesPorPagarConfirmadas.Titles.Add(New SeriesTitle())
                               vistaOnyxComisionesPorPagarConfirmadas.Titles(0).Text = seriesOnyxComisionesPorPagarConfirmadas.Name
                               ChartOnyxComPorPagarConfirmadas.Legend.Visibility = DefaultBoolean.True
                               ChartOnyxComPorPagarConfirmadas.Series.Add(seriesOnyxComisionesPorPagarConfirmadas)
                           End If

                       End Sub)
    End Function

    Private Async Function ConciliacionGeneral(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim RegistrosPosadasEnFecha = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                           Dim RegistrosOnyxEnFecha = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                           Dim RegistrosCityExpressEnFecha = conciliacionesProvRepository.cityexpress.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                           Dim RegistrosCityTacsEnFecha = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                           Dim RegistrosCityGestionCommtrackEnFecha = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()

                           Dim TotalRegistros As Integer = RegistrosPosadasEnFecha.Count() + RegistrosOnyxEnFecha.Count() + RegistrosCityExpressEnFecha.Count() + RegistrosCityTacsEnFecha.Count() + RegistrosCityGestionCommtrackEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If


                           Dim RegistrosConciliados As Integer = RegistrosPosadasEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1) +
                                                                 RegistrosOnyxEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1) +
                                                                 RegistrosCityExpressEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1) +
                                                                 RegistrosCityTacsEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1) +
                                                                 RegistrosCityGestionCommtrackEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1)


                           Dim RegistrosConciliadosMonto As Decimal = RegistrosPosadasEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).Sum(Function(x) ConvertStringToDecimal(x.comision)) +
                                                                 RegistrosOnyxEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN)) +
                                                                 RegistrosCityExpressEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision)) +
                                                                 RegistrosCityTacsEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).Sum(Function(x) ConvertStringToDecimal(x.PayComTC)) +
                                                                 RegistrosCityGestionCommtrackEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))


                           Dim RegistrosNoConciliados As Integer = RegistrosPosadasEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Count() +
                                                                 RegistrosOnyxEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Count() +
                                                                 RegistrosCityExpressEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Count() +
                                                                 RegistrosCityTacsEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Count() +
                                                                 RegistrosCityGestionCommtrackEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Count()

                           Dim RegistrosNoConciliadosMonto As Decimal = RegistrosPosadasEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Sum(Function(x) ConvertStringToDecimal(x.comision)) +
                                                                 RegistrosOnyxEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).ToList().ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN)) +
                                                                 RegistrosCityExpressEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision)) +
                                                                 RegistrosCityTacsEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Sum(Function(x) ConvertStringToDecimal(x.PayComTC)) +
                                                                 RegistrosCityGestionCommtrackEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))

                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)


                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:$0.00}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                   seriesPorConciliacion.Label.TextPattern = "{A}:{V:$0.00}"
                               End If

                               CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                               vistaPorConciliacion.Titles.Add(New SeriesTitle())
                               vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                               ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                               ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionGeneralReservasConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim RegistrosPosadasEnFecha = conciliacionesProvRepository.conciliacionDetallePosadas.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()
                           Dim RegistrosOnyxEnFecha = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()
                           Dim RegistrosCityExpressEnFecha = conciliacionesProvRepository.conciliacionDetalleCityExpress.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()
                           Dim RegistrosTacsEnFecha = conciliacionesProvRepository.conciliacionDetalleTacs.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()
                           Dim RegistrosGestionCommtrackEnFecha = conciliacionesProvRepository.conciliacionDetalleGestionCommtrack.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal).ToList()

                           Dim TotalRegistros As Integer = RegistrosPosadasEnFecha.Count() + RegistrosOnyxEnFecha.Count() + RegistrosCityExpressEnFecha.Count() + RegistrosTacsEnFecha.Count() + RegistrosGestionCommtrackEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = False
                               End If
                           End If

                           'Buscamos los distintos tipos de conciliaciones
                           Dim TiposConciliaciones As List(Of String) = New List(Of String)()
                           TiposConciliaciones.AddRange(RegistrosPosadasEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList())
                           TiposConciliaciones.AddRange(RegistrosOnyxEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList())
                           TiposConciliaciones.AddRange(RegistrosCityExpressEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList())
                           TiposConciliaciones.AddRange(RegistrosTacsEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList())
                           TiposConciliaciones.AddRange(RegistrosGestionCommtrackEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList())

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartConciliadas.Series.Clear()
                                                     seriesConciliadas.Points.Clear()
                                                     vistaConciliadas.Titles.Clear()
                                                 End Sub))
                           Else
                               chartConciliadas.Series.Clear()
                               seriesConciliadas.Points.Clear()
                               vistaConciliadas.Titles.Clear()
                           End If

                           For Each TipoConciliacion In TiposConciliaciones.GroupBy(Function(x) x)
                               Dim ConciliadosPosadasConTipoDeConciliacionCount As Integer = RegistrosPosadasEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Count()
                               Dim ConciliadosOnyxConTipoDeConciliacionCount As Integer = RegistrosOnyxEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Count()
                               Dim ConciliadosCityExpressConTipoDeConciliacionCount As Integer = RegistrosCityExpressEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Count()
                               Dim ConciliadosTacsConTipoDeConciliacionCount As Integer = RegistrosTacsEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Count()
                               Dim ConciliadosGestionCommtrackConTipoDeConciliacionCount As Integer = RegistrosGestionCommtrackEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Count()


                               Dim ConciliadosPosadasConTipoDeConciliacionMonto As Integer = RegistrosPosadasEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).ToList().Sum(Function(x) ConvertStringToDecimal(x.ComOrig))
                               Dim ConciliadosOnyxConTipoDeConciliacionMonto As Integer = RegistrosOnyxEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).ToList().Sum(Function(x) ConvertStringToDecimal(x.ComOrig))
                               Dim ConciliadosCityExpressConTipoDeConciliacionMonto As Integer = RegistrosCityExpressEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Sum(Function(x) ConvertStringToDecimal(x.ComOrig))
                               Dim ConciliadosTacsConTipoDeConciliacionMonto As Integer = RegistrosTacsEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Sum(Function(x) ConvertStringToDecimal(x.ComOrig))
                               Dim ConciliadosGestionCommtrackConTipoDeConciliacionMonto As Integer = RegistrosGestionCommtrackEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion.Key).Sum(Function(x) ConvertStringToDecimal(x.ComOrig))

                               Dim ConciliadosConTipoDeConciliacionCount As Integer = ConciliadosPosadasConTipoDeConciliacionCount + ConciliadosOnyxConTipoDeConciliacionCount + ConciliadosCityExpressConTipoDeConciliacionCount + ConciliadosTacsConTipoDeConciliacionCount + ConciliadosGestionCommtrackConTipoDeConciliacionCount
                               Dim ConciliadosConTipoDeConciliacionMonto As Integer = ConciliadosPosadasConTipoDeConciliacionMonto + ConciliadosOnyxConTipoDeConciliacionMonto + ConciliadosCityExpressConTipoDeConciliacionMonto + ConciliadosTacsConTipoDeConciliacionMonto + ConciliadosGestionCommtrackConTipoDeConciliacionMonto
                               Dim ConciliadosConTipoDeConciliacionPorcentaje As Double = ((ConciliadosPosadasConTipoDeConciliacionCount + ConciliadosOnyxConTipoDeConciliacionCount + ConciliadosCityExpressConTipoDeConciliacionCount + ConciliadosTacsConTipoDeConciliacionCount + ConciliadosGestionCommtrackConTipoDeConciliacionCount) * 100.0R) / (TotalRegistros)

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion.Key, ConciliadosConTipoDeConciliacionPorcentaje))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion.Key, ConciliadosConTipoDeConciliacionCount))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion.Key, ConciliadosConTipoDeConciliacionMonto))
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion.Key, ConciliadosConTipoDeConciliacionPorcentaje))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion.Key, ConciliadosConTipoDeConciliacionCount))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion.Key, ConciliadosConTipoDeConciliacionMonto))
                                   End If
                               End If

                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesConciliadas.Label.TextPattern = "{A}:{V:$0.00}"
                                                     End If
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                                                     vistaConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                                                     chartConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     chartConciliadas.Series.Add(seriesConciliadas)
                                                 End Sub))
                           Else
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesConciliadas.Label.TextPattern = "{A}:{V:$0.00}"
                               End If
                               CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                               vistaConciliadas.Titles.Add(New SeriesTitle())
                               vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                               chartConciliadas.Legend.Visibility = DefaultBoolean.True
                               chartConciliadas.Series.Add(seriesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionGeneralReservasNoConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim RegistrosPosadasEnFechaNoConciliados = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           Dim RegistrosCityExpressEnFechaNoConciliados = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           Dim RegistrosOnyxEnFechaNoConciliados = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           Dim RegistrosGestionCommtrackEnFechaNoConciliados = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           Dim RegistrosTacsEnFechaNoConciliados = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()

                           Dim RegistrosTotales As List(Of Object) = New List(Of Object)()
                           RegistrosTotales.AddRange(RegistrosPosadasEnFechaNoConciliados)
                           RegistrosTotales.AddRange(RegistrosCityExpressEnFechaNoConciliados)
                           RegistrosTotales.AddRange(RegistrosOnyxEnFechaNoConciliados)
                           RegistrosTotales.AddRange(RegistrosGestionCommtrackEnFechaNoConciliados)
                           RegistrosTotales.AddRange(RegistrosTacsEnFechaNoConciliados)

                           Dim TotalRegistrosNoConciliados As Integer = RegistrosPosadasEnFechaNoConciliados.Count + RegistrosCityExpressEnFechaNoConciliados.Count + RegistrosOnyxEnFechaNoConciliados.Count + RegistrosGestionCommtrackEnFechaNoConciliados.Count + RegistrosTacsEnFechaNoConciliados.Count

                           Dim ConditionsNoAutomaticoHashSet As HashSet(Of String) = New HashSet(Of String)()
                           Dim ConditionsNoManualHashSet As HashSet(Of String) = New HashSet(Of String)()

                           Dim CondicionesNoAutomaticoUnicas As List(Of String) = New List(Of String)()
                           CondicionesNoAutomaticoUnicas.AddRange(RegistrosPosadasEnFechaNoConciliados.Where(Function(x) x.CondicionNoAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNoAuto).Select(Function(x) x.CondicionNoAuto).ToList())
                           CondicionesNoAutomaticoUnicas.AddRange(RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) x.CondicionNoAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNoAuto).Select(Function(x) x.CondicionNoAuto).ToList())
                           CondicionesNoAutomaticoUnicas.AddRange(RegistrosOnyxEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList())
                           CondicionesNoAutomaticoUnicas.AddRange(RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList())
                           CondicionesNoAutomaticoUnicas.AddRange(RegistrosTacsEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList())


                           Dim CondicionesNoManualUnicas As List(Of String) = New List(Of String)()
                           CondicionesNoManualUnicas.AddRange(RegistrosPosadasEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList())
                           CondicionesNoManualUnicas.AddRange(RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList())
                           CondicionesNoManualUnicas.AddRange(RegistrosOnyxEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList())
                           CondicionesNoManualUnicas.AddRange(RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList())
                           CondicionesNoManualUnicas.AddRange(RegistrosTacsEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList())


                           Dim CondicionesNoAutomaticasRadio As Boolean = False
                           Dim CondicionesNoManualRadio As Boolean = False
                           Dim CondicionesNoAmbasRadio As Boolean = False

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                                                     CondicionesNoManualRadio = RadioPendientesManual.Checked
                                                     CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                                                 End Sub))
                           Else
                               CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                               CondicionesNoManualRadio = RadioPendientesManual.Checked
                               CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                           End If


                           For Each CadenaCondiciones In CondicionesNoAutomaticoUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoAutomaticoHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           For Each CadenaCondiciones In CondicionesNoManualUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoManualHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           'ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartNoConciliadas.Series.Clear()
                                                     seriesNoConciliadasManual.Points.Clear()
                                                     vistaNoConciliadasManual.Titles.Clear()
                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                 End Sub))
                           Else
                               chartNoConciliadas.Series.Clear()
                               seriesNoConciliadasManual.Points.Clear()
                               vistaNoConciliadasManual.Titles.Clear()
                               seriesNoConciliadasAutomatico.Points.Clear()
                               vistaNoConciliadasAutomatico.Titles.Clear()
                           End If

                           If CondicionesNoAutomaticasRadio Then

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionPosadasCount As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionCityExpressCount As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionOnyxCount As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackCount As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionTacsCount As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Count()

                                   Dim ElementosNoConciliadosConCondicionPosadasMonto As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionCityExpressMonto As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionOnyxMonto As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackMonto As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionTacsMonto As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))

                                   Dim ElementosNoConciliadosConCondicionCount = ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionTacsCount + ElementosNoConciliadosConCondicionGestionCommtrackCount
                                   Dim ElementosNoConciliadosConCondicionMonto = ElementosNoConciliadosConCondicionPosadasMonto + ElementosNoConciliadosConCondicionCityExpressMonto + ElementosNoConciliadosConCondicionOnyxMonto + ElementosNoConciliadosConCondicionTacsMonto + ElementosNoConciliadosConCondicionGestionCommtrackMonto
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = ((ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionGestionCommtrackCount + ElementosNoConciliadosConCondicionTacsCount) * 100.0R) / (TotalRegistrosNoConciliados)

                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V:$0.00}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V:$0.00}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If


                           If CondicionesNoManualRadio Then
                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionPosadasCount As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionCityExpressCount As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionOnyxCount As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackCount As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionTacsCount As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()


                                   Dim ElementosNoConciliadosConCondicionPosadasMonto As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionCityExpressMonto As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionOnyxMonto As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackMonto As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionTacsMonto As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))

                                   Dim ElementosNoConciliadosConCondicionCount = ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionTacsCount + ElementosNoConciliadosConCondicionGestionCommtrackCount
                                   Dim ElementosNoConciliadosConCondicionMonto = ElementosNoConciliadosConCondicionPosadasMonto + ElementosNoConciliadosConCondicionCityExpressMonto + ElementosNoConciliadosConCondicionOnyxMonto + ElementosNoConciliadosConCondicionTacsMonto + ElementosNoConciliadosConCondicionGestionCommtrackMonto
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = ((ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionGestionCommtrackCount + ElementosNoConciliadosConCondicionTacsCount) * 100.0R) / (TotalRegistrosNoConciliados)

                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}:{VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}:{V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}:{V:$0.00}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}:{VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}:{V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}:{V:$0.00}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If
                           End If

                           If CondicionesNoAmbasRadio Then
                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionPosadasCount As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionCityExpressCount As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionOnyxCount As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackCount As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionTacsCount As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Count()

                                   Dim ElementosNoConciliadosConCondicionPosadasMonto As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionCityExpressMonto As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionOnyxMonto As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackMonto As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionTacsMonto As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))

                                   Dim ElementosNoConciliadosConCondicionCount = ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionTacsCount + ElementosNoConciliadosConCondicionGestionCommtrackCount
                                   Dim ElementosNoConciliadosConCondicionMonto = ElementosNoConciliadosConCondicionPosadasMonto + ElementosNoConciliadosConCondicionCityExpressMonto + ElementosNoConciliadosConCondicionOnyxMonto + ElementosNoConciliadosConCondicionTacsMonto + ElementosNoConciliadosConCondicionGestionCommtrackMonto
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = ((ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionGestionCommtrackCount + ElementosNoConciliadosConCondicionTacsCount) * 100.0R) / (TotalRegistrosNoConciliados)

                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V:$0.00}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}:{V:$0.00}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If



                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionPosadasCount As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionCityExpressCount As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionOnyxCount As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackCount As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionTacsCount As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()

                                   Dim ElementosNoConciliadosConCondicionPosadasMonto As Integer = RegistrosPosadasEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionCityExpressMonto As Integer = RegistrosCityExpressEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.comision))
                                   Dim ElementosNoConciliadosConCondicionOnyxMonto As Integer = RegistrosOnyxEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                                   Dim ElementosNoConciliadosConCondicionGestionCommtrackMonto As Integer = RegistrosGestionCommtrackEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionTacsMonto As Integer = RegistrosTacsEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))

                                   Dim ElementosNoConciliadosConCondicionCount = ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionTacsCount + ElementosNoConciliadosConCondicionGestionCommtrackCount
                                   Dim ElementosNoConciliadosConCondicionMonto = ElementosNoConciliadosConCondicionPosadasMonto + ElementosNoConciliadosConCondicionCityExpressMonto + ElementosNoConciliadosConCondicionOnyxMonto + ElementosNoConciliadosConCondicionTacsMonto + ElementosNoConciliadosConCondicionGestionCommtrackMonto
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = ((ElementosNoConciliadosConCondicionPosadasCount + ElementosNoConciliadosConCondicionCityExpressCount + ElementosNoConciliadosConCondicionOnyxCount + ElementosNoConciliadosConCondicionGestionCommtrackCount + ElementosNoConciliadosConCondicionTacsCount) * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}:{VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}:{V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}:{V:$0.00}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}:{VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}:{V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}:{V:$0.00}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If

                           End If

                       End Sub)
    End Function

    Private Async Function ConciliacionPosadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of posadas) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor = Periodo)
                           End If
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If


                           Dim RegistrosConciliados As IEnumerable(Of posadas) = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1)
                           Dim RegistrosNoConciliados As IEnumerable(Of posadas) = RegistrosEnFecha.Where(Function(x) x.estatusConciliado Is Nothing)

                           Dim RegistrosConciliadosCount As Integer = RegistrosConciliados.Count()
                           Dim RegistrosNoConciliadosCount As Integer = RegistrosNoConciliados.Count()

                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliadosCount * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliadosCount * 100D) / CType(TotalRegistros, Double)

                           Dim MontosRegistrosConciliados As Double = RegistrosConciliados.Sum(Function(x) ConvertStringToDecimal(x.comision))
                           Dim MontosRegistrosNoConciliados As Double = RegistrosNoConciliados.Sum(Function(x) ConvertStringToDecimal(x.comision))

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados.Count()))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados.Count()))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", MontosRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", MontosRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", MontosRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", MontosRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                               vistaPorConciliacion.Titles.Add(New SeriesTitle())
                               vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                               ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                               ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                           End If



                       End Sub)
    End Function

    Private Async Function ConciliacionPosadasReservasConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of conciliacionDetallePosadas) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetallePosadas.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetallePosadas.Where(Function(x) x.FechaApp = Periodo)
                           End If

                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = False
                               End If
                           End If

                           'Buscamos los distintos tipos de conciliaciones
                           Dim TiposConciliaciones As List(Of String) = New List(Of String)()
                           TiposConciliaciones = RegistrosEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList()

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartConciliadas.Series.Clear()
                                                     seriesConciliadas.Points.Clear()
                                                     vistaConciliadas.Titles.Clear()
                                                 End Sub))
                           Else
                               chartConciliadas.Series.Clear()
                               seriesConciliadas.Points.Clear()
                               vistaConciliadas.Titles.Clear()
                           End If

                           For Each TipoConciliacion As String In TiposConciliaciones
                               Dim ConciliadosConTipoDeConciliacionCount As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).Count()
                               Dim ConciliadosConTipoDeConciliacionPorcentaje As Double = (ConciliadosConTipoDeConciliacionCount * 100.0R) / (TotalRegistros)

                               Dim ConciliadosConTipoDeConciliacionMonto As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision))

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                   End If
                               End If

                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                                                     vistaConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                                                     chartConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     chartConciliadas.Series.Add(seriesConciliadas)
                                                 End Sub))
                           Else
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If
                               CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                               vistaConciliadas.Titles.Add(New SeriesTitle())
                               vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                               chartConciliadas.Legend.Visibility = DefaultBoolean.True
                               chartConciliadas.Series.Add(seriesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionPosadasReservasNoConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)


                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNoConciliados As IEnumerable(Of posadas) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                           End If

                           Dim TotalRegistrosNoConciliados As Integer = RegistrosEnFechaNoConciliados.Count()

                           Dim ConditionsNoAutomaticoHashSet As HashSet(Of String) = New HashSet(Of String)()
                           Dim ConditionsNoManualHashSet As HashSet(Of String) = New HashSet(Of String)()

                           Dim CondicionesNoAutomaticoUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNoAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNoAuto).Select(Function(x) x.CondicionNoAuto).ToList()
                           Dim CondicionesNoManualUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList()

                           Dim CondicionesNoAutomaticasRadio As Boolean = False
                           Dim CondicionesNoManualRadio As Boolean = False
                           Dim CondicionesNoAmbasRadio As Boolean = False

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                                                     CondicionesNoManualRadio = RadioPendientesManual.Checked
                                                     CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                                                 End Sub))
                           Else
                               CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                               CondicionesNoManualRadio = RadioPendientesManual.Checked
                               CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                           End If


                           For Each CadenaCondiciones In CondicionesNoAutomaticoUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoAutomaticoHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           For Each CadenaCondiciones In CondicionesNoManualUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoManualHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           'ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartNoConciliadas.Series.Clear()
                                                     seriesNoConciliadasManual.Points.Clear()
                                                     vistaNoConciliadasManual.Titles.Clear()
                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                 End Sub))
                           Else
                               chartNoConciliadas.Series.Clear()
                               seriesNoConciliadasManual.Points.Clear()
                               vistaNoConciliadasManual.Titles.Clear()
                               seriesNoConciliadasAutomatico.Points.Clear()
                               vistaNoConciliadasAutomatico.Titles.Clear()
                           End If

                           If CondicionesNoAutomaticasRadio Then

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 If InvokeRequired Then
                                                                     Invoke(New Action(Sub()
                                                                                           chartNoConciliadas.Series.Clear()
                                                                                           seriesNoConciliadasManual.Points.Clear()
                                                                                           vistaNoConciliadasManual.Titles.Clear()
                                                                                           seriesNoConciliadasAutomatico.Points.Clear()
                                                                                           vistaNoConciliadasAutomatico.Titles.Clear()
                                                                                       End Sub))
                                                                 Else
                                                                     chartNoConciliadas.Series.Clear()
                                                                     seriesNoConciliadasManual.Points.Clear()
                                                                     vistaNoConciliadasManual.Titles.Clear()
                                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                                 End If
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           If InvokeRequired Then
                                               Invoke(New Action(Sub()
                                                                     chartNoConciliadas.Series.Clear()
                                                                     seriesNoConciliadasManual.Points.Clear()
                                                                     vistaNoConciliadasManual.Titles.Clear()
                                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                                 End Sub))
                                           Else
                                               chartNoConciliadas.Series.Clear()
                                               seriesNoConciliadasManual.Points.Clear()
                                               vistaNoConciliadasManual.Titles.Clear()
                                               seriesNoConciliadasAutomatico.Points.Clear()
                                               vistaNoConciliadasAutomatico.Titles.Clear()
                                           End If
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If


                           If CondicionesNoManualRadio Then
                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 If InvokeRequired Then
                                                                     Invoke(New Action(Sub()
                                                                                           chartNoConciliadas.Series.Clear()
                                                                                           seriesNoConciliadasManual.Points.Clear()
                                                                                           vistaNoConciliadasManual.Titles.Clear()
                                                                                       End Sub))
                                                                 Else
                                                                     chartNoConciliadas.Series.Clear()
                                                                     seriesNoConciliadasManual.Points.Clear()
                                                                     vistaNoConciliadasManual.Titles.Clear()
                                                                 End If
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           If InvokeRequired Then
                                               Invoke(New Action(Sub()
                                                                     chartNoConciliadas.Series.Clear()
                                                                     seriesNoConciliadasManual.Points.Clear()
                                                                     vistaNoConciliadasManual.Titles.Clear()
                                                                 End Sub))
                                           Else
                                               chartNoConciliadas.Series.Clear()
                                               seriesNoConciliadasManual.Points.Clear()
                                               vistaNoConciliadasManual.Titles.Clear()
                                           End If
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If
                           End If

                           If CondicionesNoAmbasRadio Then

                               ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNoAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 If InvokeRequired Then
                                                                     Invoke(New Action(Sub()
                                                                                           chartNoConciliadas.Series.Clear()
                                                                                           seriesNoConciliadasAutomatico.Points.Clear()
                                                                                           vistaNoConciliadasAutomatico.Titles.Clear()
                                                                                       End Sub))
                                                                 Else
                                                                     chartNoConciliadas.Series.Clear()
                                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                                 End If
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           If InvokeRequired Then
                                               Invoke(New Action(Sub()
                                                                     chartNoConciliadas.Series.Clear()
                                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                                 End Sub))
                                           Else
                                               chartNoConciliadas.Series.Clear()
                                               seriesNoConciliadasAutomatico.Points.Clear()
                                               vistaNoConciliadasAutomatico.Titles.Clear()
                                           End If
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If

                       End Sub)
    End Function

    Private Async Function ConciliacionCityExpress(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of cityexpress) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.cityexpress.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.cityexpress.Where(Function(x) x.mesProveedor = Periodo)
                           End If

                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosConciliados As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1)
                           Dim RegistrosNoConciliados As Integer = RegistrosEnFecha.Count(Function(x) x.estatusConciliado Is Nothing)

                           Dim RegistrosConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision))
                           Dim RegistrosNoConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision))


                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"

                                   CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                   vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                   vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                   ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                   ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                               End If
                           End If


                       End Sub)
    End Function

    Private Async Function ConciliacionCityExpressReservasConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of conciliacionDetalleCityExpress) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleCityExpress.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleCityExpress.Where(Function(x) x.FechaApp = Periodo)
                           End If

                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = False
                               End If
                           End If

                           'Buscamos los distintos tipos de conciliaciones
                           Dim TiposConciliaciones As List(Of String) = New List(Of String)()
                           TiposConciliaciones = RegistrosEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList()

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartConciliadas.Series.Clear()
                                                     seriesConciliadas.Points.Clear()
                                                     vistaConciliadas.Titles.Clear()
                                                 End Sub))
                           Else
                               chartConciliadas.Series.Clear()
                               seriesConciliadas.Points.Clear()
                               vistaConciliadas.Titles.Clear()
                           End If

                           For Each TipoConciliacion As String In TiposConciliaciones
                               Dim ConciliadosConTipoDeConciliacionCount As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).Count()
                               Dim ConciliadosConTipoDeConciliacionMonto As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).ToList().Sum(Function(x) ConvertStringToDecimal(x.Comision))
                               Dim ConciliadosConTipoDeConciliacionPorcentaje As Double = (ConciliadosConTipoDeConciliacionCount * 100.0R) / (TotalRegistros)

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                   End If
                               End If

                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                                                     vistaConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                                                     chartConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     chartConciliadas.Series.Add(seriesConciliadas)
                                                 End Sub))
                           Else
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If
                               CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                               vistaConciliadas.Titles.Add(New SeriesTitle())
                               vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                               chartConciliadas.Legend.Visibility = DefaultBoolean.True
                               chartConciliadas.Series.Add(seriesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionCityExpressReservasNoConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNoConciliados As IEnumerable(Of cityexpress) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.cityexpress.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.cityexpress.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                           End If

                           Dim TotalRegistrosNoConciliados As Integer = RegistrosEnFechaNoConciliados.Count()

                           Dim ConditionsNoAutomaticoHashSet As HashSet(Of String) = New HashSet(Of String)()
                           Dim ConditionsNoManualHashSet As HashSet(Of String) = New HashSet(Of String)()

                           Dim CondicionesNoAutomaticoUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList()
                           Dim CondicionesNoManualUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList()

                           Dim CondicionesNoAutomaticasRadio As Boolean = False
                           Dim CondicionesNoManualRadio As Boolean = False
                           Dim CondicionesNoAmbasRadio As Boolean = False

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                                                     CondicionesNoManualRadio = RadioPendientesManual.Checked
                                                     CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                                                 End Sub))
                           Else
                               CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                               CondicionesNoManualRadio = RadioPendientesManual.Checked
                               CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                           End If


                           For Each CadenaCondiciones In CondicionesNoAutomaticoUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoAutomaticoHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           For Each CadenaCondiciones In CondicionesNoManualUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoManualHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           'ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartNoConciliadas.Series.Clear()
                                                     seriesNoConciliadasManual.Points.Clear()
                                                     vistaNoConciliadasManual.Titles.Clear()
                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                 End Sub))
                           Else
                               chartNoConciliadas.Series.Clear()
                               seriesNoConciliadasManual.Points.Clear()
                               vistaNoConciliadasManual.Titles.Clear()
                               seriesNoConciliadasAutomatico.Points.Clear()
                               vistaNoConciliadasAutomatico.Titles.Clear()
                           End If

                           If CondicionesNoAutomaticasRadio Then

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.Comision))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If


                           If CondicionesNoManualRadio Then
                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.Comision))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If
                           End If

                           If CondicionesNoAmbasRadio Then
                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.Comision))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If



                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.Comision))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If

                           End If

                       End Sub)
    End Function


    Private Async Function ConciliacionGestionCommtrack(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of gestionCommtrack) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor = Periodo)
                           End If
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosConciliados As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1)
                           Dim RegistrosNoConciliados As Integer = RegistrosEnFecha.Count(Function(x) x.estatusConciliado Is Nothing)

                           Dim RegistrosConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).ToList().Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                           Dim RegistrosNoConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).ToList().Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))

                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                               vistaPorConciliacion.Titles.Add(New SeriesTitle())
                               vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                               ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                               ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                           End If



                       End Sub)
    End Function

    Private Async Function ConciliacionGestionCommtrackReservasConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of conciliacionDetalleGestionCommtrack) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleGestionCommtrack.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleGestionCommtrack.Where(Function(x) x.FechaApp = Periodo)
                           End If
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = False
                               End If
                           End If

                           'Buscamos los distintos tipos de conciliaciones
                           Dim TiposConciliaciones As List(Of String) = New List(Of String)()
                           TiposConciliaciones = RegistrosEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList()

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartConciliadas.Series.Clear()
                                                     seriesConciliadas.Points.Clear()
                                                     vistaConciliadas.Titles.Clear()
                                                 End Sub))
                           Else
                               chartConciliadas.Series.Clear()
                               seriesConciliadas.Points.Clear()
                               vistaConciliadas.Titles.Clear()
                           End If

                           For Each TipoConciliacion As String In TiposConciliaciones
                               Dim ConciliadosConTipoDeConciliacionCount As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).Count()
                               Dim ConciliadosConTipoDeConciliacionMonto As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).ToList().Sum(Function(x) ConvertStringToDecimal(x.ComOrig))
                               Dim ConciliadosConTipoDeConciliacionPorcentaje As Double = (ConciliadosConTipoDeConciliacionCount * 100.0R) / (TotalRegistros)

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                   End If
                               End If
                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                                                     vistaConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                                                     chartConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     chartConciliadas.Series.Add(seriesConciliadas)
                                                 End Sub))
                           Else
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If
                               CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                               vistaConciliadas.Titles.Add(New SeriesTitle())
                               vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                               chartConciliadas.Legend.Visibility = DefaultBoolean.True
                               chartConciliadas.Series.Add(seriesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionGestionCommtrackReservasNoConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNoConciliados As IEnumerable(Of gestionCommtrack) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.gestionCommtrack.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                           End If

                           Dim TotalRegistrosNoConciliados As Integer = RegistrosEnFechaNoConciliados.Count()

                           Dim ConditionsNoAutomaticoHashSet As HashSet(Of String) = New HashSet(Of String)()
                           Dim ConditionsNoManualHashSet As HashSet(Of String) = New HashSet(Of String)()

                           Dim CondicionesNoAutomaticoUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList()
                           Dim CondicionesNoManualUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList()

                           Dim CondicionesNoAutomaticasRadio As Boolean = False
                           Dim CondicionesNoManualRadio As Boolean = False
                           Dim CondicionesNoAmbasRadio As Boolean = False

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                                                     CondicionesNoManualRadio = RadioPendientesManual.Checked
                                                     CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                                                 End Sub))
                           Else
                               CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                               CondicionesNoManualRadio = RadioPendientesManual.Checked
                               CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                           End If


                           For Each CadenaCondiciones In CondicionesNoAutomaticoUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoAutomaticoHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           For Each CadenaCondiciones In CondicionesNoManualUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoManualHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           'ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartNoConciliadas.Series.Clear()
                                                     seriesNoConciliadasManual.Points.Clear()
                                                     vistaNoConciliadasManual.Titles.Clear()
                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                 End Sub))
                           Else
                               chartNoConciliadas.Series.Clear()
                               seriesNoConciliadasManual.Points.Clear()
                               vistaNoConciliadasManual.Titles.Clear()
                               seriesNoConciliadasAutomatico.Points.Clear()
                               vistaNoConciliadasAutomatico.Titles.Clear()
                           End If

                           If CondicionesNoAutomaticasRadio Then

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PAID_AGY)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If


                           If CondicionesNoManualRadio Then
                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If
                           End If

                           If CondicionesNoAmbasRadio Then
                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If



                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) ConvertStringToDecimal(x.PAID_AGY))
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If

                           End If

                       End Sub)
    End Function

    Private Async Function ConciliacionOnyx(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of onyx) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.onyx.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.onyx.Where(Function(x) x.mesProveedor = Periodo)
                           End If
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosConciliados As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1)
                           Dim RegistrosNoConciliados As Integer = RegistrosEnFecha.Count(Function(x) x.estatusConciliado Is Nothing)

                           Dim RegistrosConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))
                           Dim RegistrosNoConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).ToList().Sum(Function(x) ConvertStringToDecimal(x.PaidCommissionMXN))

                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                               vistaPorConciliacion.Titles.Add(New SeriesTitle())
                               vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                               ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                               ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                           End If



                       End Sub)
    End Function

    Private Async Function ConciliacionOnyxReservasConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of conciliacionDetalleOnyx) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleOnyx.Where(Function(x) x.FechaApp = Periodo)
                           End If

                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = False
                               End If
                           End If

                           'Buscamos los distintos tipos de conciliaciones
                           Dim TiposConciliaciones As List(Of String) = New List(Of String)()
                           TiposConciliaciones = RegistrosEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList()

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartConciliadas.Series.Clear()
                                                     seriesConciliadas.Points.Clear()
                                                     vistaConciliadas.Titles.Clear()
                                                 End Sub))
                           Else
                               chartConciliadas.Series.Clear()
                               seriesConciliadas.Points.Clear()
                               vistaConciliadas.Titles.Clear()
                           End If

                           For Each TipoConciliacion As String In TiposConciliaciones
                               Dim ConciliadosConTipoDeConciliacionCount As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).Count()
                               Dim ConciliadosConTipoDeConciliacionMonto As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).ToList().Sum(Function(x) ConvertStringToDecimal(x.ComOrig))
                               Dim ConciliadosConTipoDeConciliacionPorcentaje As Double = (ConciliadosConTipoDeConciliacionCount * 100.0R) / (TotalRegistros)

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                   End If
                               End If

                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                                                     vistaConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                                                     chartConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     chartConciliadas.Series.Add(seriesConciliadas)
                                                 End Sub))
                           Else
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If
                               CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                               vistaConciliadas.Titles.Add(New SeriesTitle())
                               vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                               chartConciliadas.Legend.Visibility = DefaultBoolean.True
                               chartConciliadas.Series.Add(seriesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionOnyxReservasNoConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)

                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNoConciliados As IEnumerable(Of onyxPagadas) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.onyxPagadas.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                           End If

                           Dim TotalRegistrosNoConciliados As Integer = RegistrosEnFechaNoConciliados.Count()

                           Dim ConditionsNoAutomaticoHashSet As HashSet(Of String) = New HashSet(Of String)()
                           Dim ConditionsNoManualHashSet As HashSet(Of String) = New HashSet(Of String)()

                           Dim CondicionesNoAutomaticoUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList()
                           Dim CondicionesNoManualUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList()

                           Dim CondicionesNoAutomaticasRadio As Boolean = False
                           Dim CondicionesNoManualRadio As Boolean = False
                           Dim CondicionesNoAmbasRadio As Boolean = False

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                                                     CondicionesNoManualRadio = RadioPendientesManual.Checked
                                                     CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                                                 End Sub))
                           Else
                               CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                               CondicionesNoManualRadio = RadioPendientesManual.Checked
                               CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                           End If


                           For Each CadenaCondiciones In CondicionesNoAutomaticoUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoAutomaticoHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           For Each CadenaCondiciones In CondicionesNoManualUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoManualHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           'ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartNoConciliadas.Series.Clear()
                                                     seriesNoConciliadasManual.Points.Clear()
                                                     vistaNoConciliadasManual.Titles.Clear()
                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                 End Sub))
                           Else
                               chartNoConciliadas.Series.Clear()
                               seriesNoConciliadasManual.Points.Clear()
                               vistaNoConciliadasManual.Titles.Clear()
                               seriesNoConciliadasAutomatico.Points.Clear()
                               vistaNoConciliadasAutomatico.Titles.Clear()
                           End If

                           If CondicionesNoAutomaticasRadio Then

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PaidCommissionMXN)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If


                           If CondicionesNoManualRadio Then
                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PaidCommissionMXN)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If
                           End If

                           If CondicionesNoAmbasRadio Then
                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PaidCommissionMXN)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If



                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PaidCommissionMXN)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If

                           End If

                       End Sub)
    End Function

    Private Async Function ConciliacionTacs(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IEnumerable(Of tacs) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.tacs.Where(Function(x) x.mesProveedor = Periodo).ToList()
                           End If
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosConciliados As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1)
                           Dim RegistrosNoConciliados As Integer = RegistrosEnFecha.Count(Function(x) x.estatusConciliado Is Nothing)

                           Dim RegistrosConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).Sum(Function(x) x.PayCom)
                           Dim RegistrosNoConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Sum(Function(x) x.PayCom)


                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                               vistaPorConciliacion.Titles.Add(New SeriesTitle())
                               vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                               ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                               ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                           End If



                       End Sub)
    End Function

    Private Async Function ConciliacionTacsReservasConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFecha As IQueryable(Of conciliacionDetalleTacs) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleTacs.Where(Function(x) x.FechaApp >= FechaInicial And x.FechaApp <= FechaFinal)
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFecha = conciliacionesProvRepository.conciliacionDetalleTacs.Where(Function(x) x.FechaApp = Periodo)
                           End If
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinReservasConciliadasEncontradas.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinReservasConciliadasEncontradas.Visible = False
                               End If
                           End If

                           'Buscamos los distintos tipos de conciliaciones
                           Dim TiposConciliaciones As List(Of String) = New List(Of String)()
                           TiposConciliaciones = RegistrosEnFecha.DistinctBy(Function(x) x.TipoConciliacion).Select(Function(x) x.TipoConciliacion).ToList()

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartConciliadas.Series.Clear()
                                                     seriesConciliadas.Points.Clear()
                                                     vistaConciliadas.Titles.Clear()
                                                 End Sub))
                           Else
                               chartConciliadas.Series.Clear()
                               seriesConciliadas.Points.Clear()
                               vistaConciliadas.Titles.Clear()
                           End If

                           For Each TipoConciliacion As String In TiposConciliaciones
                               Dim ConciliadosConTipoDeConciliacionCount As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).Count()
                               Dim ConciliadosConTipoDeConciliacionMonto As Integer = RegistrosEnFecha.Where(Function(x) x.TipoConciliacion = TipoConciliacion).ToList().Where(Function(x) x.Comision IsNot Nothing).Sum(Function(x) ConvertStringToDecimal(x.Comision))
                               Dim ConciliadosConTipoDeConciliacionPorcentaje As Double = (ConciliadosConTipoDeConciliacionCount * 100.0R) / (TotalRegistros)

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                                         End If
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionPorcentaje))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionCount))
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesConciliadas.Points.Add(New SeriesPoint(TipoConciliacion, ConciliadosConTipoDeConciliacionMonto))
                                   End If
                               End If

                           Next

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                                                     End If
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                                                     vistaConciliadas.Titles.Add(New SeriesTitle())
                                                     vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                                                     chartConciliadas.Legend.Visibility = DefaultBoolean.True
                                                     chartConciliadas.Series.Add(seriesConciliadas)
                                                 End Sub))
                           Else
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesConciliadas.Label.TextPattern = "{A}: {V:c2}"
                               End If
                               CType(seriesConciliadas.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesConciliadas.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaConciliadas = CType(seriesConciliadas.View, PieSeriesView)
                               vistaConciliadas.Titles.Add(New SeriesTitle())
                               vistaConciliadas.Titles(0).Text = seriesConciliadas.Name
                               chartConciliadas.Legend.Visibility = DefaultBoolean.True
                               chartConciliadas.Series.Add(seriesConciliadas)
                           End If
                       End Sub)
    End Function

    Private Async Function ConciliacionTacsReservasNoConciliadas(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)


                           Dim Periodo As DateTime = DateTime.MinValue

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                                                 End Sub))
                           Else
                               Periodo = CType(CmbPeriodos.SelectedItem, DateTime)
                           End If

                           Dim RegistrosEnFechaNoConciliados As IEnumerable(Of tacsPagadas) = Nothing

                           If RadioRangoTiempo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal And x.estatusConciliado Is Nothing).ToList()
                           ElseIf RadioPeriodo.Checked Then
                               RegistrosEnFechaNoConciliados = conciliacionesProvRepository.tacsPagadas.Where(Function(x) x.mesProveedor = Periodo And x.estatusConciliado Is Nothing).ToList()
                           End If

                           Dim TotalRegistrosNoConciliados As Integer = RegistrosEnFechaNoConciliados.Count()

                           Dim ConditionsNoAutomaticoHashSet As HashSet(Of String) = New HashSet(Of String)()
                           Dim ConditionsNoManualHashSet As HashSet(Of String) = New HashSet(Of String)()

                           Dim CondicionesNoAutomaticoUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOAuto IsNot Nothing).DistinctBy(Function(x) x.CondicionNOAuto).Select(Function(x) x.CondicionNOAuto).ToList()
                           Dim CondicionesNoManualUnicas As List(Of String) = RegistrosEnFechaNoConciliados.Where(Function(x) x.CondicionNOManual IsNot Nothing).DistinctBy(Function(x) x.CondicionNOManual).Select(Function(x) x.CondicionNOManual).ToList()

                           Dim CondicionesNoAutomaticasRadio As Boolean = False
                           Dim CondicionesNoManualRadio As Boolean = False
                           Dim CondicionesNoAmbasRadio As Boolean = False

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                                                     CondicionesNoManualRadio = RadioPendientesManual.Checked
                                                     CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                                                 End Sub))
                           Else
                               CondicionesNoAutomaticasRadio = RadioPendientesAuto.Checked
                               CondicionesNoManualRadio = RadioPendientesManual.Checked
                               CondicionesNoAmbasRadio = RadioPendientesAmbas.Checked
                           End If


                           For Each CadenaCondiciones In CondicionesNoAutomaticoUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoAutomaticoHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           For Each CadenaCondiciones In CondicionesNoManualUnicas
                               Dim CondicionesArray As String() = CadenaCondiciones.Split(","c)
                               For i As Integer = 0 To CondicionesArray.Length - 1
                                   If Not String.IsNullOrEmpty(CondicionesArray(i)) Then
                                       ConditionsNoManualHashSet.Add(CondicionesArray(i))
                                   End If
                               Next
                           Next

                           'ConditionsNoAutomaticoHashSet.UnionWith(ConditionsNoManualHashSet)

                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     chartNoConciliadas.Series.Clear()
                                                     seriesNoConciliadasManual.Points.Clear()
                                                     vistaNoConciliadasManual.Titles.Clear()
                                                     seriesNoConciliadasAutomatico.Points.Clear()
                                                     vistaNoConciliadasAutomatico.Titles.Clear()
                                                 End Sub))
                           Else
                               chartNoConciliadas.Series.Clear()
                               seriesNoConciliadasManual.Points.Clear()
                               vistaNoConciliadasManual.Titles.Clear()
                               seriesNoConciliadasAutomatico.Points.Clear()
                               vistaNoConciliadasAutomatico.Titles.Clear()
                           End If

                           If CondicionesNoAutomaticasRadio Then

                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PayComTC)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If
                           End If


                           If CondicionesNoManualRadio Then
                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PayComTC)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If
                           End If

                           If CondicionesNoAmbasRadio Then
                               For Each Condicion As String In ConditionsNoAutomaticoHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PayComTC)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasAutomatico.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                                         vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasAutomatico.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasAutomatico.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasAutomatico = CType(seriesNoConciliadasAutomatico.View, PieSeriesView)
                                   vistaNoConciliadasAutomatico.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasAutomatico.Titles(0).Text = seriesNoConciliadasAutomatico.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasAutomatico)
                               End If



                               For Each Condicion As String In ConditionsNoManualHashSet
                                   Dim RegexCondicionPattern As String = "[,]*(" + Condicion + ")(?!\w+)[\,]*"

                                   Dim ElementosNoConciliadosConCondicionCount As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Count()
                                   Dim ElementosNoConciliadosConCondicionMonto As Integer = RegistrosEnFechaNoConciliados.Where(Function(x) Regex.IsMatch(If(x.CondicionNOAuto, ""), RegexCondicionPattern) Or Regex.IsMatch(If(x.CondicionNOManual, ""), RegexCondicionPattern)).Sum(Function(x) x.PayComTC)
                                   Dim ElementosNoConciliadosConCondicionPorcentaje As Double = (ElementosNoConciliadosConCondicionCount * 100.0R) / (TotalRegistrosNoConciliados)


                                   If InvokeRequired Then
                                       Invoke(New Action(Sub()
                                                             If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                                             ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                                 seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                                             End If
                                                         End Sub))
                                   Else
                                       If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionPorcentaje))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionCount))
                                       ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                           seriesNoConciliadasManual.Points.Add(New SeriesPoint(Condicion, ElementosNoConciliadosConCondicionMonto))
                                       End If
                                   End If
                               Next

                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                                         ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                             seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                                         End If

                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                         CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                         vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                                         vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                                         vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                                         chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                                         chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                                                     End Sub))
                               Else
                                   If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {VP:p2}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V}"
                                   ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                       seriesNoConciliadasManual.Label.TextPattern = "{A}: {V:c2}"
                                   End If

                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                   CType(seriesNoConciliadasManual.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                   vistaNoConciliadasManual = CType(seriesNoConciliadasManual.View, PieSeriesView)
                                   vistaNoConciliadasManual.Titles.Add(New SeriesTitle())
                                   vistaNoConciliadasManual.Titles(0).Text = seriesNoConciliadasManual.Name
                                   chartNoConciliadas.Legend.Visibility = DefaultBoolean.True
                                   chartNoConciliadas.Series.Add(seriesNoConciliadasManual)
                               End If

                           End If

                       End Sub)
    End Function

    Private Sub HabilitarDeshabilitarIndicadoresOnyx()
        OnyxReportadasPorProveedor.Enabled = Not OnyxReportadasPorProveedor.Enabled
    End Sub

    Private Sub chartPorConciliacion_SizeChanged(sender As Object, e As EventArgs) Handles ChartPortentajeConciliacion.SizeChanged
        Dim x As Integer = ChartPortentajeConciliacion.Location.X
        Dim y As Integer = ChartPortentajeConciliacion.Location.Y
        Dim w As Integer = ChartPortentajeConciliacion.Size.Width
        Dim h As Integer = ChartPortentajeConciliacion.Size.Height

        Dim w_p As Integer = w - 10
        Dim h_p As Integer = 50
        Dim x_p As Integer = x + 5
        Dim y_p As Integer = ((y + h) / 2.0R) - (h_p / 2.0R)

        PanelSinRegistrosEncontrados.Size = New System.Drawing.Size(w_p, h_p)
        PanelSinRegistrosEncontrados.Location = New System.Drawing.Point(x_p, y_p)
    End Sub

    Private Sub GeneralReservasNoConciliadas_CheckedChanged(sender As Object, e As EventArgs) Handles GeneralReservasNoConciliadas.CheckedChanged
        Dim CheckCtrl As DevExpress.XtraEditors.CheckEdit = CType(sender, DevExpress.XtraEditors.CheckEdit)
        If CheckCtrl.Checked Then
            GroupBox1.Enabled = True
        End If
    End Sub

    Private Async Function ObtenerPeriodosProveedor(ByVal proveedor As ENUM_PROVEEDORES) As Task(Of List(Of Date?))
        Dim periodos = Await Task.Run(Function()
                                          If proveedor = ENUM_PROVEEDORES.POSADAS Then
                                              Dim PeriodosPosadas = conciliacionesProvRepository.posadas.GroupBy(Function(x) x.mesProveedor).Select(Function(x) x.Key)
                                              Return PeriodosPosadas.ToList()
                                          ElseIf proveedor = ENUM_PROVEEDORES.CITY_EXPRESS Then
                                              Dim PeriodosCityExpress = conciliacionesProvRepository.cityexpress.GroupBy(Function(x) x.mesProveedor).Select(Function(x) x.Key)
                                              Return PeriodosCityExpress.ToList()
                                          ElseIf proveedor = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                                              Dim PeriodosGestionCommtrack = conciliacionesProvRepository.gestionCommtrack.GroupBy(Function(x) x.mesProveedor).Select(Function(x) x.Key)
                                              Return PeriodosGestionCommtrack.ToList()
                                          ElseIf proveedor = ENUM_PROVEEDORES.ONYX Then
                                              Dim PeriodosOnyx = conciliacionesProvRepository.onyx.GroupBy(Function(x) x.mesProveedor).Select(Function(x) x.Key)
                                              Return PeriodosOnyx.ToList()
                                          ElseIf proveedor = ENUM_PROVEEDORES.TACS Then
                                              Dim PeriodosTacs = conciliacionesProvRepository.tacs.GroupBy(Function(x) x.mesProveedor).Select(Function(x) x.Key)
                                              Return PeriodosTacs.ToList()
                                          Else
                                              Return New List(Of Date?)()
                                          End If
                                      End Function)
        Return periodos
    End Function

    Private Async Sub cmbProveedores_SelectedValueChangedAsync(sender As Object, e As EventArgs) Handles cmbProveedores.SelectedValueChanged
        Dim ComboEdit As DevExpress.XtraEditors.ComboBoxEdit = CType(sender, DevExpress.XtraEditors.ComboBoxEdit)

        ComboEdit.Enabled = False
        ProgressIndicator.Visible = True

        If ComboEdit.SelectedItem.ToLower().Equals("posadas") Then

            CmbPeriodos.Enabled = False
            RadioPeriodo.Enabled = False

            XtraTabPage1.PageVisible = True
            XtraTabPage2.PageVisible = True
            XtraTabPage3.PageVisible = True
            XtraTabPage4.PageVisible = True
            XtraTabPage5.PageVisible = True


            Dim Periodos = Await ObtenerPeriodosProveedor(ENUM_PROVEEDORES.POSADAS)
            CmbPeriodos.DataSource = Periodos

            CmbPeriodos.Enabled = True
            RadioPeriodo.Enabled = True
        ElseIf ComboEdit.SelectedItem.ToLower().Equals("city_express") Then

            CmbPeriodos.Enabled = False
            RadioPeriodo.Enabled = False

            XtraTabPage1.PageVisible = True
            XtraTabPage2.PageVisible = True
            XtraTabPage3.PageVisible = True
            XtraTabPage4.PageVisible = True
            XtraTabPage5.PageVisible = True


            Dim Periodos = Await ObtenerPeriodosProveedor(ENUM_PROVEEDORES.CITY_EXPRESS)

            CmbPeriodos.DataSource = Periodos

            CmbPeriodos.Enabled = True
            RadioPeriodo.Enabled = True

        ElseIf ComboEdit.SelectedItem.ToLower().Equals("gestion_commtrack") Then

            Dim ControlHabilitado As Boolean = CmbPeriodos.Enabled

            CmbPeriodos.Enabled = False
            RadioPeriodo.Enabled = False

            XtraTabPage1.PageVisible = True
            XtraTabPage2.PageVisible = True
            XtraTabPage3.PageVisible = True
            XtraTabPage4.PageVisible = True
            XtraTabPage5.PageVisible = True


            Dim Periodos = Await ObtenerPeriodosProveedor(ENUM_PROVEEDORES.GESTION_COMMTRACK)

            CmbPeriodos.DataSource = Periodos
            CmbPeriodos.Enabled = True
            RadioPeriodo.Enabled = True

        ElseIf ComboEdit.SelectedItem.ToLower().Equals("onyx") Then


            CmbPeriodos.Enabled = False
            RadioPeriodo.Enabled = False

            XtraTabPage1.PageVisible = False
            XtraTabPage2.PageVisible = False
            XtraTabPage3.PageVisible = False
            XtraTabPage4.PageVisible = True
            XtraTabPage5.PageVisible = False

            Dim Periodos = Await ObtenerPeriodosProveedor(ENUM_PROVEEDORES.ONYX)

            CmbPeriodos.DataSource = Periodos
            CmbPeriodos.Enabled = True
            RadioPeriodo.Enabled = True

        ElseIf ComboEdit.SelectedItem.ToLower().Equals("tacs") Then

            CmbPeriodos.Enabled = False
            RadioPeriodo.Enabled = False

            XtraTabPage1.PageVisible = True
            XtraTabPage2.PageVisible = True

            XtraTabPage1.PageVisible = False
            XtraTabPage2.PageVisible = False
            XtraTabPage3.PageVisible = False
            XtraTabPage4.PageVisible = False
            XtraTabPage5.PageVisible = True

            Dim Periodos = Await ObtenerPeriodosProveedor(ENUM_PROVEEDORES.TACS)

            CmbPeriodos.DataSource = Periodos
            CmbPeriodos.Enabled = True
            RadioPeriodo.Enabled = True


        ElseIf ComboEdit.SelectedItem.ToLower().Equals("general") Then

            Dim ControlHabilitado As Boolean = CmbPeriodos.Enabled

            CmbPeriodos.Enabled = False
            RadioPeriodo.Enabled = False
            RadioPeriodo.Checked = False
            CmbPeriodos.DataSource = Nothing


            XtraTabPage1.PageVisible = True
            XtraTabPage2.PageVisible = True
            XtraTabPage3.PageVisible = True
            XtraTabPage4.PageVisible = True
            XtraTabPage5.PageVisible = True

            Dim Periodos = Await ObtenerPeriodosProveedor(ENUM_PROVEEDORES.GENERAL)
            CmbPeriodos.DataSource = Periodos

            RadioRangoTiempo.Checked = True

        End If

        If ComboEdit.SelectedItem.ToLower().Equals("onyx") Then
            XtraTabPage4.PageEnabled = True
            OnyxReportadasPorProveedor.Enabled = True
            OnyxComisionesPagadas.Enabled = True
            OnyxComisionesConObservaciones.Enabled = True
            OnyxComisionesConfirmadas.Enabled = True
        Else
            XtraTabPage4.PageEnabled = False
            OnyxReportadasPorProveedor.Enabled = False
            OnyxComisionesPagadas.Enabled = False
            OnyxComisionesConObservaciones.Enabled = False
            OnyxComisionesConfirmadas.Enabled = False
        End If

        If ComboEdit.SelectedItem.ToLower().Equals("tacs") Then
            XtraTabPage5.PageEnabled = True
            TacsReportadasPorProveedor.Enabled = True
            TacsComisionesPagadas.Enabled = True
            TacsComisionesConObservaciones.Enabled = True
        Else
            XtraTabPage5.PageEnabled = False
            TacsReportadasPorProveedor.Enabled = False
            TacsComisionesPagadas.Enabled = False
            TacsComisionesConObservaciones.Enabled = False
        End If

        ProgressIndicator.Visible = False
        ComboEdit.Enabled = True
    End Sub

    Private Async Function OnyxConciliacionReportadasPorProveedor(ByVal visualizationType As VISUALIZATION_TYPE) As Task
        Await Task.Run(Sub()
                           Dim FechaInicial As DateTime = New DateTime(txtFechaInicio.DateTime.Year, txtFechaInicio.DateTime.Month, txtFechaInicio.DateTime.Day, 0, 0, 0)
                           Dim FechaFinal As DateTime = New DateTime(txtFechaFin.DateTime.Year, txtFechaFin.DateTime.Month, txtFechaFin.DateTime.Day, 23, 59, 59)
                           Dim RegistrosEnFecha = conciliacionesProvRepository.posadas.Where(Function(x) x.mesProveedor >= FechaInicial And x.mesProveedor <= FechaFinal)
                           Dim TotalRegistros As Integer = RegistrosEnFecha.Count()

                           If TotalRegistros = 0 Then
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = True
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = True
                               End If
                           Else
                               If InvokeRequired Then
                                   Invoke(New Action(Sub()
                                                         PanelSinRegistrosEncontrados.Visible = False
                                                     End Sub))
                               Else
                                   PanelSinRegistrosEncontrados.Visible = False
                               End If
                           End If

                           Dim RegistrosConciliados As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Count(Function(x) x.estatusConciliado = 1)
                           Dim RegistrosNoConciliados As Integer = RegistrosEnFecha.Count(Function(x) x.estatusConciliado Is Nothing)

                           Dim RegistrosConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado IsNot Nothing).Where(Function(x) x.estatusConciliado = 1).Sum(Function(x) ConvertStringToDecimal(x.comision))
                           Dim RegistrosNoConciliadosMonto As Integer = RegistrosEnFecha.Where(Function(x) x.estatusConciliado Is Nothing).Sum(Function(x) ConvertStringToDecimal(x.comision))


                           Dim PorcentajeRegistrosConciliados As Double = (RegistrosConciliados * 100D) / CType(TotalRegistros, Double)
                           Dim PorcentajeRegistrosNoConciliados As Double = (RegistrosNoConciliados * 100D) / CType(TotalRegistros, Double)


                           If InvokeRequired Then
                               Invoke(New Action(Sub()
                                                     ChartPortentajeConciliacion.Series.Clear()
                                                     seriesPorConciliacion.Points.Clear()
                                                     vistaPorConciliacion.Titles.Clear()
                                                     If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                                                     ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                                         seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                                         seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                                                     End If

                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                                                     CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                                                     vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                                                     vistaPorConciliacion.Titles.Add(New SeriesTitle())
                                                     vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                                                     ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                                                     ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                                                 End Sub))
                           Else
                               ChartPortentajeConciliacion.Series.Clear()
                               seriesPorConciliacion.Points.Clear()
                               vistaPorConciliacion.Titles.Clear()
                               If visualizationType = VISUALIZATION_TYPE.PORCENTAJE Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", PorcentajeRegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", PorcentajeRegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {VP:p2}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.CANTIDAD Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliados))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliados))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V}"
                               ElseIf visualizationType = VISUALIZATION_TYPE.MONTO Then
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS CONCILIADAS", RegistrosConciliadosMonto))
                                   seriesPorConciliacion.Points.Add(New SeriesPoint("RESERVAS NO CONCILIADAS", RegistrosNoConciliadosMonto))
                                   seriesPorConciliacion.Label.TextPattern = "{A}: {V:c2}"
                               End If

                               CType(seriesPorConciliacion.Label, PieSeriesLabel).Position = PieSeriesLabelPosition.TwoColumns
                               CType(seriesPorConciliacion.Label, PieSeriesLabel).ResolveOverlappingMode = ResolveOverlappingMode.Default
                               vistaPorConciliacion = CType(seriesPorConciliacion.View, PieSeriesView)
                               vistaPorConciliacion.Titles.Add(New SeriesTitle())
                               vistaPorConciliacion.Titles(0).Text = seriesPorConciliacion.Name
                               ChartPortentajeConciliacion.Legend.Visibility = DefaultBoolean.True
                               ChartPortentajeConciliacion.Series.Add(seriesPorConciliacion)
                           End If

                       End Sub)
    End Function

    Private Sub RadioRangoTiempo_CheckedChanged(sender As Object, e As EventArgs) Handles RadioRangoTiempo.CheckedChanged
        Dim CheckedControl As RadioButton = CType(sender, RadioButton)
        If CheckedControl.Checked = True Then
            ChartPortentajeConciliacion.Enabled = True
            LblFechaFin.Enabled = True
            txtFechaInicio.Enabled = True
            txtFechaFin.Enabled = True
        Else
            ChartPortentajeConciliacion.Enabled = False
            LblFechaFin.Enabled = False
            txtFechaInicio.Enabled = False
            txtFechaFin.Enabled = False
        End If
    End Sub

    Private Sub RadioPeriodo_CheckedChanged(sender As Object, e As EventArgs) Handles RadioPeriodo.CheckedChanged
        Dim CheckedControl As RadioButton = CType(sender, RadioButton)
        If CheckedControl.Checked = True Then
            LblPeriodo.Enabled = True
            CmbPeriodos.Enabled = True
        Else
            LblPeriodo.Enabled = False
            CmbPeriodos.Enabled = False
        End If
    End Sub

    Private Sub RadioPorcentaje_CheckedChanged(sender As Object, e As EventArgs) Handles RadioPorcentaje.CheckedChanged
        Dim RadioControl As RadioButton = CType(sender, RadioButton)
        If RadioControl.Checked = True Then
            Dim visualizationArgs As VisualizationTypeEventArgs = New VisualizationTypeEventArgs(VISUALIZATION_TYPE.PORCENTAJE)
            XtraTabPage3.PageEnabled = True
            RaiseEvent OnVisualizationTypeChange(Me, visualizationArgs)
            'UpdateChartInfo()
        End If
    End Sub

    Private Sub RadioCantidad_CheckedChanged(sender As Object, e As EventArgs) Handles RadioCantidad.CheckedChanged
        Dim RadioControl As RadioButton = CType(sender, RadioButton)
        If RadioControl.Checked = True Then
            Dim visualizationArgs As VisualizationTypeEventArgs = New VisualizationTypeEventArgs(VISUALIZATION_TYPE.CANTIDAD)
            XtraTabPage3.PageEnabled = True
            RaiseEvent OnVisualizationTypeChange(Me, visualizationArgs)
            'UpdateChartInfo()
        End If
    End Sub

    Private Sub RadioMonto_CheckedChanged(sender As Object, e As EventArgs) Handles RadioMonto.CheckedChanged
        Dim RadioControl As RadioButton = CType(sender, RadioButton)
        If RadioControl.Checked = True Then
            Dim visualizationArgs As VisualizationTypeEventArgs = New VisualizationTypeEventArgs(VISUALIZATION_TYPE.MONTO)
            XtraTabPage3.PageEnabled = False
            RaiseEvent OnVisualizationTypeChange(Me, visualizationArgs)
            'UpdateChartInfo()
        End If
    End Sub

    Private Async Sub UpdateChartInfo()
        Dim ComboBoxSelectedEnumItem As ENUM_PROVEEDORES = Nothing
        System.Enum.TryParse(CType(cmbProveedores.SelectedItem, String), ComboBoxSelectedEnumItem)

        If RadioRangoTiempo.Checked = True Then
            If txtFechaInicio.DateTime > txtFechaFin.DateTime Then
                Exit Sub
            End If
        ElseIf ComboBoxSelectedEnumItem = 0 Then
            Exit Sub
        ElseIf RadioPeriodo.Checked = True Then
            If Convert.ToString(CmbPeriodos.SelectedItem).Equals(String.Empty) Then
                Exit Sub
            End If
        End If

        btnProcesar.Enabled = False
        RadioPorcentaje.Enabled = False

        If GeneralPorcentajeConciliacion.Checked = True Then
            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
                Await ConciliacionPosadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
                Await ConciliacionCityExpress(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                Await ConciliacionGestionCommtrack(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
                Await ConciliacionOnyx(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
                Await ConciliacionTacs(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
                Await ConciliacionGeneral(VisualizationTypeSelected)
            End If
        End If

        If GeneralReservasConciliadas.Checked = True Then
            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
                Await ConciliacionPosadasReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
                Await ConciliacionCityExpressReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                Await ConciliacionGestionCommtrackReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
                Await ConciliacionOnyxReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
                Await ConciliacionTacsReservasConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
                Await ConciliacionGeneralReservasConciliadas(VisualizationTypeSelected)
            End If
        End If

        If GeneralReservasNoConciliadas.Checked = True Then
            If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.POSADAS Then
                Await ConciliacionPosadasReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.CITY_EXPRESS Then
                Await ConciliacionCityExpressReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GESTION_COMMTRACK Then
                Await ConciliacionGestionCommtrackReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
                Await ConciliacionOnyxReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
                Await ConciliacionTacsReservasNoConciliadas(VisualizationTypeSelected)
            ElseIf ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.GENERAL Then
                Await ConciliacionGeneralReservasNoConciliadas(VisualizationTypeSelected)
            End If
        End If

        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.ONYX Then
            If OnyxReportadasPorProveedor.Checked = True Then
                Await ConciliacionOnyxReportadasPorProveedorComisionesPagadas(VisualizationTypeSelected)
                Await ConciliacionOnyxReportadasPorProveedorComisionesPorPagar(VisualizationTypeSelected)
            End If

            If OnyxComisionesPagadas.Checked = True Then
                Await ConciliacionOnyxComPagReservacionesConciliadas(VisualizationTypeSelected)
                Await ConciliacionOnyxComPagPTA(VisualizationTypeSelected)
            End If

            If OnyxComisionesConObservaciones.Checked = True Then
                Await ConciliacionOnyxConObservaciones(VisualizationTypeSelected)
            End If

            If OnyxComisionesConfirmadas.Checked = True Then
                Await ConciliacionOnyxComPorPagarConfirmadas(VisualizationTypeSelected)
            End If
        End If

        If ComboBoxSelectedEnumItem = ENUM_PROVEEDORES.TACS Then
            If TacsReportadasPorProveedor.Checked = True Then
                Await ConciliacionTacsReportadasPorProveedorComisionesPagadas(VisualizationTypeSelected)
                Await ConciliacionTacsReportadasPorProveedorComisionesConObservaciones(VisualizationTypeSelected)
            End If

            If TacsComisionesPagadas.Checked = True Then
                Await ConciliacionTacsComPagReservacionesConciliadas(VisualizationTypeSelected)
            End If

            If TacsComisionesConObservaciones.Checked = True Then
                Await ConciliacionTacsConObservaciones(VisualizationTypeSelected)
            End If
        End If

        btnProcesar.Enabled = True
        RadioPorcentaje.Enabled = True
    End Sub

    Private Async Sub BarButtonItem2_ItemClickAsync(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnExportar.ItemClick
        Dim exportFileFDialog As SaveFileDialog = New SaveFileDialog()
        exportFileFDialog.Filter = "Archivo de Microsoft Excel 2007-2016 (*.xlsx)|*.xlsx"

        If exportFileFDialog.ShowDialog() = DialogResult.OK Then
            Me.exportFilePath = exportFileFDialog.FileName
            Dim genReporte As GeneradorReporte = New GeneradorReporte(Me.exportFilePath)

            If InvokeRequired Then
                Invoke(New Action(Sub()
                                      ProgressIndicator.Visible = True
                                      ProgressIndicator.Caption = "Exportando Reporte..."
                                  End Sub))
            Else
                ProgressIndicator.Visible = True
                ProgressIndicator.Caption = "Exportando Reporte..."
            End If

            Try
                If InvokeRequired Then
                    Invoke(New Action(Sub()
                                          btnExportar.Enabled = False
                                          btnProcesar.Enabled = False
                                          cmbProveedores.Enabled = False
                                      End Sub))
                Else
                    btnExportar.Enabled = False
                    btnProcesar.Enabled = False
                    cmbProveedores.Enabled = False
                End If

                Await genReporte.ExportarReporteAsync()

                If InvokeRequired Then
                    Invoke(New Action(Sub()
                                          btnExportar.Enabled = True
                                          btnProcesar.Enabled = True
                                          cmbProveedores.Enabled = True
                                      End Sub))
                Else
                    btnExportar.Enabled = True
                    btnProcesar.Enabled = True
                    cmbProveedores.Enabled = True
                End If

                If InvokeRequired Then
                    Invoke(New Action(Sub()
                                          ProgressIndicator.Visible = False
                                          ProgressIndicator.Caption = "Obteniendo Información..."
                                      End Sub))
                Else
                    ProgressIndicator.Visible = False
                    ProgressIndicator.Caption = "Obteniendo Información..."
                End If

                If MessageBox.Show("El reporte se exportado correctamente ¿Desea abrirlo ahora?", "Operación exitosa", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
                    Dim fileExportedInfo As FileInfo = New FileInfo(Me.exportFilePath)
                    If fileExportedInfo.Exists Then
                        System.Diagnostics.Process.Start(fileExportedInfo.FullName)
                    End If
                End If
            Catch ex As Exception

                MessageBox.Show(ex.ToString(), "Error al exportar", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                If InvokeRequired Then
                    Invoke(New Action(Sub()
                                          ProgressIndicator.Visible = False
                                          ProgressIndicator.Caption = "Obteniendo Información..."
                                      End Sub))
                Else
                    ProgressIndicator.Visible = False
                    ProgressIndicator.Caption = "Obteniendo Información..."
                End If
            End Try
        End If
    End Sub
End Class