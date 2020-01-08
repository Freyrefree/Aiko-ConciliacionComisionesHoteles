Imports System.IO
Imports System.ComponentModel
Imports OfficeOpenXml
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Imports ClosedXML.Excel
Imports Presentacion
Imports CapaNegocio.CapaNegocio
Imports OfficeOpenXml.Table
Imports System.Threading

Public Class ConciliacionComisionesHoteles

    'Variables Globales
    Private idProveedorGlobal As String = Nothing

    Private mesProveedor As String = Nothing
    Private anioProveedor As String = Nothing

    Private valorRadio As Integer = 0
    Private tipoArchivoCityExpress As Integer = 0
    Private eliminarCancelados As Integer = 0

    WithEvents formularioTipoCambio As Presentacion.Tipo_de_Cambio
    WithEvents fr2 As Presentacion.AgregarNuevaConciliacion
    WithEvents fr3 As Presentacion.AgregarNuevaConciliacion
    WithEvents formObservacionesOnyx As Presentacion.ObservacionesOnyx
    WithEvents frmLoad As Presentacion.objfrmShowProgress
    WithEvents formGrupos As Presentacion.AgregarNuevoGrupo
    WithEvents formGruposUpdate As Presentacion.AgregarNuevoGrupo

    Private ListaGruposConciliaciones As BindingList(Of GrupoConciliaciones)
    Public Event AgregarNuevoGrupoConciliacion As EventHandler(Of AgregarNuevoGrupoConciliacionArgs)

    '**************************  Objetos Clases ***************************

    Private objetoCapaNegocio As ClsN_Proveedores = New ClsN_Proveedores()
    Private objetoCN_BDBCD As ClsN_BDBCD = New ClsN_BDBCD()
    Private objetoCN_Posadas As ClsN_Posadas = New ClsN_Posadas()
    Private objetoCN_Onyx As ClsN_Onyx = New ClsN_Onyx()
    Private objetoCN_CityExpress As ClsN_CityExpress = New ClsN_CityExpress()
    Private objetoCN_GestionCommtrack As ClsN_GestionCommtrack = New ClsN_GestionCommtrack()
    Private objetoCN_Tacs As ClsN_Tacs = New ClsN_Tacs()
    Private objetoCN_Conciliacion As ClsN_Conciliaciones = New ClsN_Conciliaciones()
    Private objetoCN_PrePago As ClsN_PrePago = New ClsN_PrePago()
    Private objetoCN_OnyxRepetido As ClsN_OnyxRepetidos = New ClsN_OnyxRepetidos()

    Private objetoCN_Consultas As ClsN_ConsultasConciliaciones = New ClsN_ConsultasConciliaciones()


    '**************************************************************************************************************************************
    Private actualizacion As ClsN_Actualizaciones = New ClsN_Actualizaciones() 'Objeto Actualizaciones
    '**************************************************************************************************************************************
    '**************************************************************************************************************************************
    Dim Pendiente As ClsN_VerificarPendientes = New ClsN_VerificarPendientes() 'Objeto para Verificar Pendientes
    '**************************************************************************************************************************************

    '**************************************************************************************************************************************
    Dim MatchColumnas As ClsN_MatchColumnas = New ClsN_MatchColumnas() 'Objeto para Match
    Dim listaColumnasCaracteriticas As New List(Of List(Of String))
    '**************************************************************************************************************************************

    '**************************************************************************************************************************************
    Dim condicion As ClsN_GruposConciliacion = New ClsN_GruposConciliacion() 'Objeto para Condiciones
    Dim grupo As ClsN_GruposConciliacion = New ClsN_GruposConciliacion() 'Objeto para grupos
    Private idGrupo As Integer
    Private tuplaIndex As Integer
    '**************************************************************************************************************************************

    '***************************   FORMS  ************************************

    Private formProveedores As FormProveedores = New FormProveedores()
    Private formAddConciliacion As Presentacion.AgregarNuevaConciliacion = New Presentacion.AgregarNuevaConciliacion
    Private objfrmShowProgress As Presentacion.objfrmShowProgress = New Presentacion.objfrmShowProgress()

    '**********************************************************************


    '**********************************************************************

    Public Event ReiniciarFormularioConciliacionCH As EventHandler
    Public Event _reporteIcaavLoaded As EventHandler
    Private ExcelEstadoDeCuentaLoaded As Boolean = False

    'Listas para conciliar  Globales'
    Private valuesList As List(Of Tuple(Of String, String)) = New List(Of Tuple(Of String, String))()

    Private listAutomatico As List(Of Tuple(Of String, String)) = New List(Of Tuple(Of String, String))()
    '**********************************************************************

    Private fullConciliaciones As DataTable = New DataTable()


    Private workbook As IWorkbook
    Private cxml_workbook As XLWorkbook

    Private GridsReportesTabs As Dictionary(Of String, DataGridView)


    Private Sub FillComboProveedores()

        Dim bjetoComboProveedor As ClsN_Proveedores = New ClsN_Proveedores()

        CboProveedores.DataSource = bjetoComboProveedor.CN_DataComboProveedores()
        CboProveedores.DisplayMember = ("nombreProveedor")
        CboProveedores.ValueMember = ("idProveedor")

        If CboProveedores.Items.Count > 0 Then

            CboProveedores.SelectedIndex = 0    ' The first item has index 0 '
            idProveedorGlobal = "0"

        End If

    End Sub


    Private Sub btnBDBCD_Click(sender As Object, e As EventArgs) Handles btnBDBCD.Click

        ExaminarExcel(txt_excel_edo, cmb_hoja_edo)

    End Sub


    Private Sub btnProveedor_Click(sender As Object, e As EventArgs) Handles btnProveedor.Click


        If (idProveedorGlobal <> "0") Then

            Me.ExcelEstadoDeCuentaLoaded = False
            TabNavegacion.SelectedIndex = 1
            Try
                OpenFileDialog2.Filter = "ARCHIVOS DE EXCEL 2007-2013 (.xlsx)|*.xlsx"
                OpenFileDialog2.FilterIndex = 0
                OpenFileDialog2.FileName = ""
                OpenFileDialog2.Multiselect = False
                OpenFileDialog2.RestoreDirectory = True

                If OpenFileDialog2.ShowDialog() = DialogResult.OK Then


                    If OpenFileDialog2.FileName IsNot String.Empty Then

                        StartProgress()

                        txt_excel_icaav.Text = OpenFileDialog2.FileName
                        Dim sheetList As List(Of String) = GetSheetListFromExcel(OpenFileDialog2.FileName)
                        cmb_hoja_icaav.DataSource = sheetList
                        cmb_hoja_icaav.SelectedIndex = -1

                        CloseProgress()

                        If MsgBox("SELECCIONE LA HOJA DE TRABAJO", MsgBoxStyle.Information) = MsgBoxResult.Ok Then
                            cmb_hoja_icaav.Enabled = True
                        End If
                    End If

                Else
                    txt_excel_icaav.Text = ""
                    Exit Sub
                End If
            Catch ex As Exception
                MsgBox("ERROR: " & ex.Message)
            End Try

        Else
            MessageBox.Show("Seleccione Un Proveedor")
        End If

    End Sub

    Private Sub cmb_hoja_edo_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmb_hoja_edo.SelectionChangeCommitted

        StartProgress()
        Dim ruta As String = txt_excel_edo.Text.ToString()
        Dim indexHoja As Integer = cmb_hoja_edo.SelectedIndex

        'Global Canelados
        ClsNGlobales.EliminarCanceladosBCD = eliminarCancelados


        If ClsNGlobales.ActuaizarSegmento = 0 Then

            If ClsNGlobales.EliminarCanceladosBCD = 1 Then

                Dim tabla As DataTable = New DataTable()

                If ruta <> "" And indexHoja <> -1 Then

                    objetoCN_BDBCD.CN_cargaArchivoBDBCDEliminados(ruta, indexHoja)
                    mostrarEliminadosBDBCD()

                Else
                    MessageBox.Show("Verifique los campos Archivo y Hoja")

                End If




            ElseIf ClsNGlobales.EliminarCanceladosBCD = 0 Then

                Dim res As Boolean = cargaDocBDBCD(ruta, indexHoja)

                If (res) Then
                    consultaBDBCD()
                End If



            End If

        ElseIf ClsNGlobales.ActuaizarSegmento = 1 Then

            If ruta <> "" And indexHoja <> -1 Then

                objetoCN_BDBCD.CN_cargaArchivoBDBCDAactualizacionSegmento(ruta, indexHoja)


            Else
                MessageBox.Show("Verifique los campos Archivo y Hoja")

            End If

        End If



        CloseProgress()


    End Sub

    Private Sub cmb_hoja_icaav_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmb_hoja_icaav.SelectionChangeCommitted

        Dim rutaProveedor As String = txt_excel_icaav.Text.ToString()
        Dim indexHojaProveedor As String = cmb_hoja_icaav.SelectedIndex

        Dim proveedor As String = CboProveedores.Text


        If (ClsNGlobales.FechaPagoproveedor <> Nothing) Then

            If (proveedor = "Posadas") Then

                StartProgress()

                'Obtener ultimo ID
                Dim lastId As Int32 = objetoCN_Posadas.CN_ObtenerUltimoId()
                ClsNGlobales.LastID = lastId


                Dim res As Boolean = objetoCN_Posadas.CN_cargaDocPosadas(rutaProveedor, indexHojaProveedor)
                If (res) Then

                    consultaPosadas()
                End If

                CloseProgress()

            ElseIf (proveedor = "CityExpress") Then

                If tipoArchivoCityExpress <> 0 Then

                    ClsNGlobales.TipoPlantillaCityExpress = tipoArchivoCityExpress

                    'Obtener ultimo ID
                    Dim lastId As Int32 = objetoCN_CityExpress.CN_ObtenerUltimoId()
                    'Intancia variables globales

                    'Variables Globales
                    ClsNGlobales.LastID = lastId


                    StartProgress()
                    Dim res As Boolean = objetoCN_CityExpress.CN_cargaDocCityExpress(rutaProveedor, indexHojaProveedor)
                    If (res) Then

                        consultaCityExpress()

                    End If
                    CloseProgress()
                Else

                    MessageBox.Show("Seleccione el tipo de Plantilla")

                End If



            ElseIf (proveedor = "Onyx") Then

                'Obtener ultimo ID
                Dim lastId As Int32 = objetoCN_Onyx.CN_ObtenerUltimoId()

                ClsNGlobales.LastID = lastId

                StartProgress()

                Dim tc As Double
                Try
                    tc = Convert.ToDouble(TxBTC.Text)
                Catch ex As Exception
                    tc = 1.0
                End Try


                If (tc = Nothing) Then

                    tc = 1.0

                End If

                If (tc <> Nothing) Then

                    Dim res As Boolean = objetoCN_Onyx.CN_cargaDocOnyx(rutaProveedor, indexHojaProveedor, tc)
                    If (res) Then

                        consultaOnyx()
                        consultaOnyxPagadas()
                        consultaOnyxObservaciones()
                        consultaOnyxComisionesPendientePago()

                    End If
                Else

                    MessageBox.Show("Ingrese el TC")

                End If


                CloseProgress()

            ElseIf (proveedor = "Gestión Commtrack") Then

                StartProgress()

                'Obtener ultimo ID
                Dim lastId As Int32 = objetoCN_GestionCommtrack.CN_ObtenerUltimoId()

                ClsNGlobales.LastID = lastId


                Dim res As Boolean = objetoCN_GestionCommtrack.CN_cargaDocGestionCommtrack(rutaProveedor, indexHojaProveedor)

                If (res) Then

                    consultaGestionCommtrack()

                End If
                CloseProgress()

            ElseIf (proveedor = "Tacs") Then

                StartProgress()

                'Obtener ultimo ID
                Dim lastId As Int32 = objetoCN_Tacs.CN_ObtenerUltimoId()

                ClsNGlobales.LastID = lastId

                Dim res As Boolean = objetoCN_Tacs.CN_cargaDocTacs(rutaProveedor, indexHojaProveedor)
                If (res) Then

                    consultaTacs()
                    consultaTacsObservaciones()
                    consultaTacsPagadas()


                End If
                CloseProgress()

            ElseIf (proveedor = "Pre Pago") Then

                StartProgress()

                'Obtener ultimo ID
                Dim lastId As Int32 = objetoCN_PrePago.CN_ObtenerUltimoId()

                ClsNGlobales.LastID = lastId

                Dim res As Boolean = objetoCN_PrePago.CN_cargaDocPrePago(rutaProveedor, indexHojaProveedor)
                If (res) Then

                    consultaPrePago()
                    'consultaTacsObservaciones()
                    'consultaTacsPagadas()


                End If
                CloseProgress()

            Else

                MessageBox.Show("Seleccione Un Proveedor")

            End If
        Else

            MessageBox.Show("Selecciona El Mes y Año de Conciliacion")

        End If



    End Sub


    Private Async Sub btnExportarExcel_Click(sender As Object, e As EventArgs) Handles btnExportarExcel.Click

        Dim tablaConciliaciones As DataTable = CType((DataGridView3.DataSource), DataTable)
        Dim tablaPendientesProveedor As DataTable = CType((DGV4.DataSource), DataTable)
        Dim tablaPendientesBDBCD As DataTable = CType((DGVPendientesBDBCD.DataSource), DataTable)

        Dim tablaPagadas As DataTable = CType((DGVPagadas.DataSource), DataTable)
        Dim tablaObservaciones As DataTable = CType((DGVObservaciones.DataSource), DataTable)

        Dim tablaComisionesPendientePago As DataTable = CType((DGVPendientesPago.DataSource), DataTable)

        Dim tablaPartidasRepetidasOnyx As DataTable = CType((DGVRepetidos.DataSource), DataTable)
        Dim tablaPartidasRepetidasPaidCommission As DataTable = CType((DGVPaidCommision.DataSource), DataTable)

        Dim tablaEliminadosBCD As DataTable = CType((DGVEliminadosBCD.DataSource), DataTable)

        Dim bandera_report As Integer = 0

        SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
        SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

            If SaveFileDialog1.FileName <> "" Then
                Dim filename As String = SaveFileDialog1.FileName


                excelConciliacion(filename & ".xlsx", tablaConciliaciones, tablaPendientesProveedor, tablaPendientesBDBCD, tablaPagadas, tablaObservaciones, tablaComisionesPendientePago, tablaPartidasRepetidasOnyx, tablaPartidasRepetidasPaidCommission, tablaEliminadosBCD)


            End If
        Else
            Exit Sub
        End If



    End Sub


    Public Sub excelConciliacion(filename As String, tablaConciliaciones As DataTable, tablaPendientesProveedor As DataTable, tablaPendientesBDBCD As DataTable, tablaPagadas As DataTable, tablaObservaciones As DataTable, tablaComisionesPendientePago As DataTable, tablaPartidasRepetidasOnyx As DataTable, tablaPartidasRepetidasPaidCommission As DataTable, tablaEliminadosBCD As DataTable)


        '''''''Columnas tablaEliminados
        Try
            tablaEliminadosBCD.Columns.Remove("idBDBCD")
            tablaEliminadosBCD.Columns.Remove("id")
        Catch ex As Exception

        End Try

        ''''Columnas tablaPartidasRepetidasOnyxPaidCommission pago 

        Try
            tablaPartidasRepetidasPaidCommission.Columns.Remove("id")

        Catch ex As Exception

        End Try
        ''''Columnas tablaPartidasRepetidasOnyx pago 

        Try
            tablaPartidasRepetidasOnyx.Columns.Remove("id")

        Catch ex As Exception

        End Try

        ''''Columnas pendientes pago 

        Try
            tablaComisionesPendientePago.Columns.Remove("id")
            tablaComisionesPendientePago.Columns.Remove("idBDBCD")
            'tablaComisionesPendientePago.Columns.Remove("mesProveedor")
            tablaComisionesPendientePago.Columns.Remove("estatusConciliado")
            tablaComisionesPendientePago.Columns.Remove("estatusEliminado")
        Catch ex As Exception

        End Try


        ''''''' Columnas BDBCD ''''''''
        Try
            tablaPendientesBDBCD.Columns.Remove("mesProveedor")
            tablaPendientesBDBCD.Columns.Remove("id")
            tablaPendientesBDBCD.Columns.Remove("estatusConciliado")
            tablaPendientesBDBCD.Columns.Remove("proveedor")
        Catch ex As Exception

        End Try


        ''''''' Columnas Resultado Conciliacion ''''''''
        Try
            If (idProveedorGlobal = "1") Then

                tablaConciliaciones.Columns.Remove("idBDBCD")
                tablaConciliaciones.Columns.Remove("idProveedor")

            ElseIf (idProveedorGlobal = "2") Then

                tablaConciliaciones.Columns.Remove("idBDBCD")
                tablaConciliaciones.Columns.Remove("idProveedor")

            ElseIf (idProveedorGlobal = "3") Then

                tablaConciliaciones.Columns.Remove("idBDBCD")
                tablaConciliaciones.Columns.Remove("idProveedor")

            ElseIf (idProveedorGlobal = "4") Then

                tablaConciliaciones.Columns.Remove("idBDBCD")
                tablaConciliaciones.Columns.Remove("idProveedor")

            ElseIf (idProveedorGlobal = "19") Then

                tablaConciliaciones.Columns.Remove("idBDBCD")
                tablaConciliaciones.Columns.Remove("idProveedor")

            End If
        Catch ex As Exception

        End Try

        ''''''' Columnas Pendientes Proveedor ''''''''
        Try

            tablaPendientesProveedor.Columns.Remove("id")
            tablaPendientesProveedor.Columns.Remove("estatusConciliado")

            If (idProveedorGlobal = "1") Then

                tablaPendientesProveedor.Columns.Remove("mesProveedor")
                tablaPendientesProveedor.Columns.Remove("estatusEliminado")
                tablaPendientesProveedor.Columns.Remove("idBDBCD")
                tablaPendientesProveedor.Columns.Remove("idBDBCDManual")

            ElseIf (idProveedorGlobal = "2") Then

                tablaPendientesProveedor.Columns.Remove("mesProveedor")
                tablaPendientesProveedor.Columns.Remove("estatusEliminado")
                tablaPendientesProveedor.Columns.Remove("idBDBCD")
                tablaPendientesProveedor.Columns.Remove("idBDBCDManual")


            ElseIf (idProveedorGlobal = "3") Then

                tablaPendientesProveedor.Columns.Remove("mesProveedor")
                tablaPendientesProveedor.Columns.Remove("estatusEliminado")
                tablaPendientesProveedor.Columns.Remove("FechaCambioTC")
                tablaPendientesProveedor.Columns.Remove("idBDBCD")
                tablaPendientesProveedor.Columns.Remove("idBDBCDManual")


            ElseIf (idProveedorGlobal = "4") Then
                tablaPendientesProveedor.Columns.Remove("idBDBCD")
                tablaPendientesProveedor.Columns.Remove("mesProveedor")
                tablaPendientesProveedor.Columns.Remove("estatusEliminado")
                tablaPendientesProveedor.Columns.Remove("idBDBCDManual")

            ElseIf (idProveedorGlobal = "19") Then
                tablaPendientesProveedor.Columns.Remove("idBDBCD")
                tablaPendientesProveedor.Columns.Remove("mesProveedor")
                tablaPendientesProveedor.Columns.Remove("estatusEliminado")
                tablaPendientesProveedor.Columns.Remove("idBDBCDManual")


            End If
        Catch ex As Exception
        End Try



        ''''''' Columnas Observaciones ''''''''

        Try

            If (idProveedorGlobal = "3") Then

                tablaObservaciones.Columns.Remove("id")
                tablaObservaciones.Columns.Remove("Fechadepago")
                tablaObservaciones.Columns.Remove("firstName")
                tablaObservaciones.Columns.Remove("lastName")
                tablaObservaciones.Columns.Remove("No.trxconcatenada")
                tablaObservaciones.Columns.Remove("estatusConciliado")
                tablaObservaciones.Columns.Remove("TC")
                tablaObservaciones.Columns.Remove("PaidCommissionMXN")
                tablaObservaciones.Columns.Remove("FechaCambioTC")
                tablaObservaciones.Columns.Remove("ClienteTexto")
                tablaObservaciones.Columns.Remove("TarifaSucursal")
                tablaObservaciones.Columns.Remove("estatusELiminado")



            ElseIf (idProveedorGlobal = "4") Then

                tablaPagadas.Columns.Remove("id")
                tablaPagadas.Columns.Remove("mesProveedor")
                tablaPagadas.Columns.Remove("estatusEliminado")


            End If
        Catch ex As Exception
        End Try

        ''''''' Columnas Pagadas ''''''''

        Try
            If (idProveedorGlobal = "3") Then

                tablaPagadas.Columns.Remove("id")
                tablaPagadas.Columns.Remove("idBDBCD")
                tablaPagadas.Columns.Remove("mesProveedor")
                tablaPagadas.Columns.Remove("estatusEliminado")

            ElseIf (idProveedorGlobal = "4") Then

                tablaPagadas.Columns.Remove("id")
                tablaPagadas.Columns.Remove("idBDBCD")
                tablaPagadas.Columns.Remove("mesProveedor")
                tablaPagadas.Columns.Remove("estatusEliminado")

            End If
        Catch ex As Exception
        End Try


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ShowProgress()

        Using p = New ExcelPackage(New MemoryStream())

            If (tablaConciliaciones IsNot Nothing) Then

                If (tablaConciliaciones.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("ResultadoConciliacion")
                    ws.Cells("A1").LoadFromDataTable(tablaConciliaciones, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            If (tablaPendientesProveedor IsNot Nothing) Then

                If (tablaPendientesProveedor.Rows.Count > 0) Then

                    Dim wsB = p.Workbook.Worksheets.Add("ProveedoresPendientes")
                    wsB.Cells("A1").LoadFromDataTable(tablaPendientesProveedor, True, TableStyles.Light13)
                    wsB.Cells.AutoFitColumns()

                End If

            End If


            If (tablaPendientesBDBCD IsNot Nothing) Then

                If (tablaPendientesBDBCD.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("BCDPendientes")
                    wsC.Cells("A1").LoadFromDataTable(tablaPendientesBDBCD, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If

            If (tablaPagadas IsNot Nothing) Then

                If (tablaPagadas.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("Pagadas")
                    wsC.Cells("A1").LoadFromDataTable(tablaPagadas, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If

            If (tablaObservaciones IsNot Nothing) Then

                If (tablaObservaciones.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("Observaciones")
                    wsC.Cells("A1").LoadFromDataTable(tablaObservaciones, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If

            If (tablaComisionesPendientePago IsNot Nothing) Then

                If (tablaComisionesPendientePago.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("ComisionesPendientePago")
                    wsC.Cells("A1").LoadFromDataTable(tablaComisionesPendientePago, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If

            If (tablaPartidasRepetidasOnyx IsNot Nothing) Then

                If (tablaPartidasRepetidasOnyx.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("PartidasRepetidasPagos")
                    wsC.Cells("A1").LoadFromDataTable(tablaPartidasRepetidasOnyx, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If



            If (tablaPartidasRepetidasPaidCommission IsNot Nothing) Then

                If (tablaPartidasRepetidasPaidCommission.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("MontosRepetidosPaidCommission")
                    wsC.Cells("A1").LoadFromDataTable(tablaPartidasRepetidasPaidCommission, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If

            If (tablaEliminadosBCD IsNot Nothing) Then

                If (tablaEliminadosBCD.Rows.Count > 0) Then

                    Dim wsC = p.Workbook.Worksheets.Add("tablaEliminadosBCD")
                    wsC.Cells("A1").LoadFromDataTable(tablaEliminadosBCD, True, TableStyles.Light13)
                    wsC.Cells.AutoFitColumns()

                End If

            End If





            p.Workbook.Worksheets.Add("hoja1")
            'Dim wsC = p.Workbook.Worksheets.Add("VOID")
            'wsC.Cells("A1").LoadFromDataTable(tablaObservaciones, True, TableStyles.Light13)
            'wsC.Cells.AutoFitColumns()

            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            CloseProgress()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub


    Private Function GetSheetListFromExcel(ByVal filePath As String) As List(Of String)
        Dim wb As XSSFWorkbook
        Dim sheetList As List(Of String) = New List(Of String)()
        Dim sheetsNumber As Integer = 0

        Using fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            wb = New XSSFWorkbook(fs)
        End Using
        sheetsNumber = wb.NumberOfSheets

        For i As Integer = 0 To sheetsNumber - 1
            sheetList.Add(wb.GetSheetAt(i).SheetName)
        Next

        Return sheetList
    End Function


    Sub ExaminarExcel(excel As TextBox, hojas As ComboBox)
        Dim sArchivos As String
        Try
            OpenFileDialog1.Filter = "todos los archivos (*.*)|*.*|Excel 2003(.xls) |*.xls|Excel 2007 (.xlsx)|*.xlsx"
            OpenFileDialog1.FilterIndex = 3
            OpenFileDialog1.Multiselect = False
            If (OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then


                StartProgress()

                sArchivos = OpenFileDialog1.FileName
                excel.Text = sArchivos
                hojas.Items.Clear()
                hojas.Text = ""

                Static ban As Integer = 0
                Dim ObjExcel As Object
                ObjExcel = CreateObject("Excel.Application")
                Try
                    hojas.Items.Clear()
                    hojas.Visible = True
                    ObjExcel.Workbooks.Open(excel.Text)
                    For y = 1 To ObjExcel.Sheets.Count
                        hojas.Items.Add(ObjExcel.Sheets(y).Name)
                    Next

                    ObjExcel.DisplayAlerts = False
                    ObjExcel.Visible = False
                    ObjExcel.Quit()
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ObjExcel)
                    ObjExcel = Nothing
                    hojas.Enabled = True

                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                Finally
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                End Try
                hojas.Enabled = True

                CloseProgress()

                If MsgBox("SELECCIONE LA HOJA DE TRABAJO", MsgBoxStyle.Information) = MsgBoxResult.Ok Then
                    cmb_hoja_edo.Enabled = True
                End If

            Else
                excel.Text = ""
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub




    Private Sub MostrarPendientesProveedorOnyx()

        DGV4.DataSource = Nothing
        DGV4.Rows.Clear()
        'DGV4.Rows.Clear()
        DGV4.ForeColor = Color.Black
        DGV4.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGV4.DataSource = objetoCN_Conciliacion.CN_ConsultaPendientesOnyx()
        'DGV4.Refresh()

        DGV4.Columns("id").Visible = False
        DGV4.Columns("estatusConciliado").Visible = False
        DGV4.Columns("mesProveedor").Visible = False
        DGV4.Columns("estatusEliminado").Visible = False
        DGV4.Columns("idBDBCD").Visible = False


        DGV4.Columns("idBDBCDManual").Visible = False

        DGV4.Columns("CondicionOKAuto").HeaderText = "CondicionesAutomaticasCumplidas"
        DGV4.Columns("CondicionNOAuto").HeaderText = "CondicionesAutomaticasNoCumplidas"
        DGV4.Columns("countCumplidoAuto").HeaderText = "ConteoAutomaticasCumplidas"
        DGV4.Columns("countNoCumplidoAuto").HeaderText = "ConteoAutomaticasNOCumplidas"

        DGV4.Columns("CondicionOKManual").HeaderText = "CondicionesManualesCumplidas"
        DGV4.Columns("CondicionNOManual").HeaderText = "CondicionesManualesNoCumplidas"
        DGV4.Columns("countCumplidoManual").HeaderText = "ConteoManualesCumplidas"
        DGV4.Columns("countNoCumplidoManual").HeaderText = "ConteoManualesNOCumplidas"
        DGV4.Columns("CondicionOKManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("CondicionOKAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("countCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells


        LblTotalProveedorPendiente.Text = DGV4.Rows.Count - 1.ToString()

    End Sub

    Private Sub MostrarPendientesProveedorTacs()

        DGV4.DataSource = Nothing
        DGV4.Rows.Clear()
        'DGV4.Rows.Clear()
        DGV4.ForeColor = Color.Black
        DGV4.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGV4.DataSource = objetoCN_Conciliacion.CN_ConsultaPendientesTacs()
        'DGV4.Refresh()

        DGV4.Columns("id").Visible = False
        DGV4.Columns("estatusConciliado").Visible = False
        DGV4.Columns("idBDBCD").Visible = False
        DGV4.Columns("mesProveedor").Visible = False
        DGV4.Columns("estatusEliminado").Visible = False


        DGV4.Columns("idBDBCDManual").Visible = False

        DGV4.Columns("CondicionOKAuto").HeaderText = "CondicionesAutomaticasCumplidas"
        DGV4.Columns("CondicionNOAuto").HeaderText = "CondicionesAutomaticasNoCumplidas"
        DGV4.Columns("countCumplidoAuto").HeaderText = "ConteoAutomaticasCumplidas"
        DGV4.Columns("countNoCumplidoAuto").HeaderText = "ConteoAutomaticasNOCumplidas"

        DGV4.Columns("CondicionOKManual").HeaderText = "CondicionesManualesCumplidas"
        DGV4.Columns("CondicionNOManual").HeaderText = "CondicionesManualesNoCumplidas"
        DGV4.Columns("countCumplidoManual").HeaderText = "ConteoManualesCumplidas"
        DGV4.Columns("countNoCumplidoManual").HeaderText = "ConteoManualesNOCumplidas"
        DGV4.Columns("CondicionOKManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("CondicionOKAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("countCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        LblTotalProveedorPendiente.Text = DGV4.Rows.Count - 1.ToString()

    End Sub


    Private Sub MostrarPendientesProveedorCityExpress()



        DGV4.DataSource = Nothing
        DGV4.Rows.Clear()

        'DGV4.Rows.Clear()
        DGV4.ForeColor = Color.Black
        DGV4.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGV4.DataSource = objetoCN_Conciliacion.CN_ConsultaPendientesCityExpress()
        'DGV4.Refresh()

        DGV4.Columns("id").Visible = False
        DGV4.Columns("estatusConciliado").Visible = False
        DGV4.Columns("idBDBCD").Visible = False
        DGV4.Columns("estatusEliminado").Visible = False
        DGV4.Columns("mesProveedor").Visible = False


        DGV4.Columns("idBDBCDManual").Visible = False

        DGV4.Columns("CondicionOKAuto").HeaderText = "CondicionesAutomaticasCumplidas"
        DGV4.Columns("CondicionNOAuto").HeaderText = "CondicionesAutomaticasNoCumplidas"
        DGV4.Columns("countCumplidoAuto").HeaderText = "ConteoAutomaticasCumplidas"
        DGV4.Columns("countNoCumplidoAuto").HeaderText = "ConteoAutomaticasNOCumplidas"

        DGV4.Columns("CondicionOKManual").HeaderText = "CondicionesManualesCumplidas"
        DGV4.Columns("CondicionNOManual").HeaderText = "CondicionesManualesNoCumplidas"
        DGV4.Columns("countCumplidoManual").HeaderText = "ConteoManualesCumplidas"
        DGV4.Columns("countNoCumplidoManual").HeaderText = "ConteoManualesNOCumplidas"
        DGV4.Columns("CondicionOKManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("CondicionOKAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("countCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells


        LblTotalProveedorPendiente.Text = DGV4.Rows.Count - 1.ToString()

    End Sub

    Private Sub MostrarPendientesGestionCommtrack()



        DGV4.DataSource = Nothing
        DGV4.Rows.Clear()

        'DGV4.Rows.Clear()
        DGV4.ForeColor = Color.Black
        DGV4.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGV4.DataSource = objetoCN_Conciliacion.CN_ConsultaPendientesGestionCommtrack()
        'DGV4.Refresh()

        DGV4.Columns("id").Visible = False
        DGV4.Columns("estatusConciliado").Visible = False
        DGV4.Columns("mesProveedor").Visible = False
        DGV4.Columns("estatusEliminado").Visible = False
        DGV4.Columns("idBDBCD").Visible = False

        DGV4.Columns("idBDBCDManual").Visible = False


        DGV4.Columns("CondicionOKAuto").HeaderText = "CondicionesAutomaticasCumplidas"
        DGV4.Columns("CondicionNOAuto").HeaderText = "CondicionesAutomaticasNoCumplidas"
        DGV4.Columns("countCumplidoAuto").HeaderText = "ConteoAutomaticasCumplidas"
        DGV4.Columns("countNoCumplidoAuto").HeaderText = "ConteoAutomaticasNOCumplidas"

        DGV4.Columns("CondicionOKManual").HeaderText = "CondicionesManualesCumplidas"
        DGV4.Columns("CondicionNOManual").HeaderText = "CondicionesManualesNoCumplidas"
        DGV4.Columns("countCumplidoManual").HeaderText = "ConteoManualesCumplidas"
        DGV4.Columns("countNoCumplidoManual").HeaderText = "ConteoManualesNOCumplidas"
        DGV4.Columns("CondicionOKManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("CondicionOKAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("countCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells


        LblTotalProveedorPendiente.Text = DGV4.Rows.Count - 1.ToString()

    End Sub


    Private Sub MostrarPendientesProveedor()



        DGV4.DataSource = Nothing
        DGV4.Rows.Clear()

        'DGV4.Rows.Clear()
        DGV4.ForeColor = Color.Black
        DGV4.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGV4.DataSource = objetoCN_Conciliacion.CN_ConsultaPendientesPosadas()
        'DGV4.Refresh()
        DGV4.Columns("id").Visible = False
        DGV4.Columns("fechaPago").Visible = False
        DGV4.Columns("estatusConciliado").Visible = False
        DGV4.Columns("percentComision").Visible = False
        DGV4.Columns("idBDBCD").Visible = False
        DGV4.Columns("mesProveedor").Visible = False
        DGV4.Columns("estatusEliminado").Visible = False

        DGV4.Columns("idBDBCDManual").Visible = False

        DGV4.Columns("CondicionOKAuto").HeaderText = "CondicionesAutomaticasCumplidas"
        DGV4.Columns("CondicionNOAuto").HeaderText = "CondicionesAutomaticasNoCumplidas"
        DGV4.Columns("countCumplidoAuto").HeaderText = "ConteoAutomaticasCumplidas"
        DGV4.Columns("countNoCumplidoAuto").HeaderText = "ConteoAutomaticasNOCumplidas"

        DGV4.Columns("CondicionOKManual").HeaderText = "CondicionesManualesCumplidas"
        DGV4.Columns("CondicionNOManual").HeaderText = "CondicionesManualesNoCumplidas"
        DGV4.Columns("countCumplidoManual").HeaderText = "ConteoManualesCumplidas"
        DGV4.Columns("countNoCumplidoManual").HeaderText = "ConteoManualesNOCumplidas"
        DGV4.Columns("CondicionOKManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("CondicionOKAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("CondicionNOAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        DGV4.Columns("countCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoManual").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGV4.Columns("countNoCumplidoAuto").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells



        LblTotalProveedorPendiente.Text = DGV4.Rows.Count - 1.ToString()

    End Sub

    Private Sub MostrarPendientesBDBCD()

        DGV4.DataSource = Nothing
        DGV4.Rows.Clear()

        'DGV4.Rows.Clear()
        DGVPendientesBDBCD.ForeColor = Color.Black
        DGVPendientesBDBCD.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGVPendientesBDBCD.DataSource = objetoCN_Conciliacion.CN_ConsultaPendientesBDBCD()
        'DGV4.Refresh()

        DGVPendientesBDBCD.Columns("id").Visible = False
        DGVPendientesBDBCD.Columns("estatusConciliado").Visible = False
        DGVPendientesBDBCD.Columns("proveedor").Visible = False
        DGVPendientesBDBCD.Columns("mesProveedor").Visible = False


        LblTotalPendientesBDBCD.Text = DGVPendientesBDBCD.Rows.Count - 1.ToString()



    End Sub


    Public Function cargaDocBDBCD(ruta As String, indexHoja As Int16)



        If ruta <> "" And indexHoja <> -1 Then

            Dim respuesta As Boolean = objetoCN_BDBCD.CN_cargaArchivoBDBCD(ruta, indexHoja)

            If (respuesta) Then

                objetoCN_BDBCD.CN_quitarGuion()


                Dim resadd As Boolean = objetoCN_BDBCD.CN_addColumnasBDBCD()


                If (resadd) Then



                    Return True
                Else
                    Return False

                End If
            Else
                Return False

            End If

        Else
            MessageBox.Show("Verifique los campos Archivo y Hoja")
            Return False
        End If

    End Function









    Public Sub consultaBDBCD()

        'objetoCN_BDBCD.CN_SelectBDBCD()
        DataGridView1.DataSource = Nothing
        DataGridView1.Rows.Clear()

        TabNavegacion.SelectedIndex = 0
        DataGridView1.ForeColor = Color.Black
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DataGridView1.DataSource = objetoCN_BDBCD.CN_SelectBDBCD()
        DataGridView1.Columns("id").Visible = False
        DataGridView1.Columns("estatusConciliado").Visible = False
        DataGridView1.Columns("proveedor").Visible = False
        DataGridView1.Columns("mesProveedor").Visible = False

        Dim firstName As DataGridViewColumn = DataGridView1.Columns("firstName")
        firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        firstName.HeaderCell.Style.ForeColor = Color.White

        Dim lastName As DataGridViewColumn = DataGridView1.Columns("lastName")
        lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        lastName.HeaderCell.Style.ForeColor = Color.White

        LblTotalBDBCD.Text = DataGridView1.Rows.Count - 1.ToString()

    End Sub

    Public Sub mostrarEliminadosBDBCD()

        'objetoCN_BDBCD.CN_SelectBDBCD()
        DGVEliminadosBCD.DataSource = Nothing
        DGVEliminadosBCD.Rows.Clear()

        TabNavegacion.SelectedTab = TabPage14
        DGVEliminadosBCD.ForeColor = Color.Black
        DGVEliminadosBCD.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DGVEliminadosBCD.DataSource = objetoCN_BDBCD.CN_ConsultaEliminadosBDBCD()
        DGVEliminadosBCD.Columns("idBDBCD").Visible = False
        DGVEliminadosBCD.Columns("id").Visible = False


        'Dim firstName As DataGridViewColumn = DataGridView1.Columns("firstName")
        'firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        'firstName.HeaderCell.Style.ForeColor = Color.White

        'Dim lastName As DataGridViewColumn = DataGridView1.Columns("lastName")
        'lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        'lastName.HeaderCell.Style.ForeColor = Color.White

        LblEliminadosBCD.Text = DGVEliminadosBCD.Rows.Count - 1.ToString()

    End Sub


    Public Sub consultaPosadas()

        TabNavegacion.SelectedIndex = 1

        DataGridView2.ForeColor = Color.Black
        DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DataGridView2.DataSource = objetoCN_Posadas.CN_SelectPosadas()
        DataGridView2.Columns("id").Visible = False
        DataGridView2.Columns("fechaPago").Visible = False
        DataGridView2.Columns("estatusConciliado").Visible = False


        DataGridView2.Columns("CondicionOKAuto").Visible = False
        DataGridView2.Columns("CondicionNOAuto").Visible = False
        DataGridView2.Columns("countCumplidoAuto").Visible = False
        DataGridView2.Columns("countNoCumplidoAuto").Visible = False
        DataGridView2.Columns("idBDBCD").Visible = False
        DataGridView2.Columns("mesProveedor").Visible = False
        DataGridView2.Columns("estatusEliminado").Visible = False

        DataGridView2.Columns("CondicionOKManual").Visible = False
        DataGridView2.Columns("CondicionNOManual").Visible = False
        DataGridView2.Columns("countCumplidoManual").Visible = False
        DataGridView2.Columns("countNoCumplidoManual").Visible = False
        DataGridView2.Columns("idBDBCDManual").Visible = False


        Dim firstName As DataGridViewColumn = DataGridView2.Columns("firstName")
        firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        firstName.HeaderCell.Style.ForeColor = Color.White

        Dim lastName As DataGridViewColumn = DataGridView2.Columns("lastName")
        lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        lastName.HeaderCell.Style.ForeColor = Color.White

        Dim noNoches As DataGridViewColumn = DataGridView2.Columns("noNoches")
        noNoches.HeaderCell.Style.BackColor = Color.MediumBlue
        noNoches.HeaderCell.Style.ForeColor = Color.White

        Dim totalDeLaReserva As DataGridViewColumn = DataGridView2.Columns("totalDeLaReserva")
        totalDeLaReserva.HeaderCell.Style.BackColor = Color.MediumBlue
        totalDeLaReserva.HeaderCell.Style.ForeColor = Color.White

        LblTotalProveedor.Text = DataGridView2.Rows.Count - 1.ToString()

    End Sub

    Public Sub consultaCityExpress()

        TabNavegacion.SelectedIndex = 1

        DataGridView2.ForeColor = Color.Black
        DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DataGridView2.DataSource = objetoCN_CityExpress.CN_SelectCityExpress()
        DataGridView2.Columns("id").Visible = False
        DataGridView2.Columns("estatusConciliado").Visible = False

        DataGridView2.Columns("CondicionOKAuto").Visible = False
        DataGridView2.Columns("CondicionNOAuto").Visible = False
        DataGridView2.Columns("countCumplidoAuto").Visible = False
        DataGridView2.Columns("countNoCumplidoAuto").Visible = False
        DataGridView2.Columns("idBDBCD").Visible = False
        DataGridView2.Columns("mesProveedor").Visible = False
        DataGridView2.Columns("estatusEliminado").Visible = False

        DataGridView2.Columns("CondicionOKManual").Visible = False
        DataGridView2.Columns("CondicionNOManual").Visible = False
        DataGridView2.Columns("countCumplidoManual").Visible = False
        DataGridView2.Columns("countNoCumplidoManual").Visible = False
        DataGridView2.Columns("idBDBCDManual").Visible = False


        Dim firstName As DataGridViewColumn = DataGridView2.Columns("firstName")
        firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        firstName.HeaderCell.Style.ForeColor = Color.White

        Dim lastName As DataGridViewColumn = DataGridView2.Columns("lastName")
        lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        lastName.HeaderCell.Style.ForeColor = Color.White

        Dim noNoches As DataGridViewColumn = DataGridView2.Columns("NoNoches")
        noNoches.HeaderCell.Style.BackColor = Color.MediumBlue
        noNoches.HeaderCell.Style.ForeColor = Color.White

        LblTotalProveedor.Text = DataGridView2.Rows.Count - 1.ToString()


    End Sub

    Public Sub consultaOnyx()


        TabNavegacion.SelectedIndex = 1

        DataGridView2.ForeColor = Color.Black
        DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        'DataGridView2.DataSource = objetoCN_Posadas.CN_SelectPosadas()
        DataGridView2.DataSource = objetoCN_Onyx.CN_SelectOnyx()

        DataGridView2.Columns("id").Visible = False
        DataGridView2.Columns("estatusConciliado").Visible = False
        DataGridView2.Columns("mesProveedor").Visible = False
        DataGridView2.Columns("estatusEliminado").Visible = False

        Dim firstName As DataGridViewColumn = DataGridView2.Columns("firstName")
        firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        firstName.HeaderCell.Style.ForeColor = Color.White

        Dim lastName As DataGridViewColumn = DataGridView2.Columns("lastName")
        lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        lastName.HeaderCell.Style.ForeColor = Color.White

        Dim Notrxconcatenada As DataGridViewColumn = DataGridView2.Columns("No.trxconcatenada")
        Notrxconcatenada.HeaderCell.Style.BackColor = Color.MediumBlue
        Notrxconcatenada.HeaderCell.Style.ForeColor = Color.White

        Dim observaciones As DataGridViewColumn = DataGridView2.Columns("observaciones")
        observaciones.HeaderCell.Style.BackColor = Color.MediumBlue
        observaciones.HeaderCell.Style.ForeColor = Color.White

        Dim TC As DataGridViewColumn = DataGridView2.Columns("TC")
        TC.HeaderCell.Style.BackColor = Color.MediumBlue
        TC.HeaderCell.Style.ForeColor = Color.White

        Dim PaidCommissionMXN As DataGridViewColumn = DataGridView2.Columns("PaidCommissionMXN")
        PaidCommissionMXN.HeaderCell.Style.BackColor = Color.MediumBlue
        PaidCommissionMXN.HeaderCell.Style.ForeColor = Color.White

        Dim FechaCambioTC As DataGridViewColumn = DataGridView2.Columns("FechaCambioTC")
        FechaCambioTC.HeaderCell.Style.BackColor = Color.MediumBlue
        FechaCambioTC.HeaderCell.Style.ForeColor = Color.White

        Dim BookingStatusCode As DataGridViewColumn = DataGridView2.Columns("BookingStatusCode")
        BookingStatusCode.HeaderCell.Style.BackColor = Color.MediumBlue
        BookingStatusCode.HeaderCell.Style.ForeColor = Color.White

        Dim PaidStatus As DataGridViewColumn = DataGridView2.Columns("PaidStatus")
        PaidStatus.HeaderCell.Style.BackColor = Color.MediumBlue
        PaidStatus.HeaderCell.Style.ForeColor = Color.White

        LblTotalProveedor.Text = DataGridView2.Rows.Count - 1.ToString()

    End Sub

    Public Sub consultaOnyxPagadas()

        DGVPagadas.ForeColor = Color.Black
        DGVPagadas.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DGVPagadas.DataSource = objetoCN_Onyx.CN_SelectOnyxPagadas()

        DGVPagadas.Columns("id").Visible = False
        DGVPagadas.Columns("estatusConciliado").Visible = False

        DGVPagadas.Columns("CondicionOKAuto").Visible = False
        DGVPagadas.Columns("CondicionNOAuto").Visible = False
        DGVPagadas.Columns("countCumplidoAuto").Visible = False
        DGVPagadas.Columns("countNoCumplidoAuto").Visible = False
        DGVPagadas.Columns("idBDBCD").Visible = False
        DGVPagadas.Columns("mesProveedor").Visible = False
        DGVPagadas.Columns("estatusEliminado").Visible = False

        DGVPagadas.Columns("CondicionOKManual").Visible = False
        DGVPagadas.Columns("CondicionNOManual").Visible = False
        DGVPagadas.Columns("countCumplidoManual").Visible = False
        DGVPagadas.Columns("countNoCumplidoManual").Visible = False
        DGVPagadas.Columns("idBDBCDManual").Visible = False

        Dim firstName As DataGridViewColumn = DGVPagadas.Columns("firstName")
        firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        firstName.HeaderCell.Style.ForeColor = Color.White

        Dim lastName As DataGridViewColumn = DGVPagadas.Columns("lastName")
        lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        lastName.HeaderCell.Style.ForeColor = Color.White

        Dim Notrxconcatenada As DataGridViewColumn = DGVPagadas.Columns("No.trxconcatenada")
        Notrxconcatenada.HeaderCell.Style.BackColor = Color.MediumBlue
        Notrxconcatenada.HeaderCell.Style.ForeColor = Color.White

        Dim observaciones As DataGridViewColumn = DGVPagadas.Columns("observaciones")
        observaciones.HeaderCell.Style.BackColor = Color.MediumBlue
        observaciones.HeaderCell.Style.ForeColor = Color.White

        Dim TC As DataGridViewColumn = DGVPagadas.Columns("TC")
        TC.HeaderCell.Style.BackColor = Color.MediumBlue
        TC.HeaderCell.Style.ForeColor = Color.White

        Dim PaidCommissionMXN As DataGridViewColumn = DGVPagadas.Columns("PaidCommissionMXN")
        PaidCommissionMXN.HeaderCell.Style.BackColor = Color.MediumBlue
        PaidCommissionMXN.HeaderCell.Style.ForeColor = Color.White

        Dim FechaCambioTC As DataGridViewColumn = DGVPagadas.Columns("FechaCambioTC")
        FechaCambioTC.HeaderCell.Style.BackColor = Color.MediumBlue
        FechaCambioTC.HeaderCell.Style.ForeColor = Color.White

        Dim BookingStatusCode As DataGridViewColumn = DGVPagadas.Columns("BookingStatusCode")
        BookingStatusCode.HeaderCell.Style.BackColor = Color.MediumBlue
        BookingStatusCode.HeaderCell.Style.ForeColor = Color.White

        Dim PaidStatus As DataGridViewColumn = DGVPagadas.Columns("PaidStatus")
        PaidStatus.HeaderCell.Style.BackColor = Color.MediumBlue
        PaidStatus.HeaderCell.Style.ForeColor = Color.White

        LblTotalPAGADAS.Text = DGVPagadas.Rows.Count - 1.ToString()

    End Sub

    Private Sub consultaOnyxObservaciones()

        DGVObservaciones.ForeColor = Color.Black
        DGVObservaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGVObservaciones.DataSource = objetoCN_Onyx.CN_SelectOnyxObservaciones()
        DGVObservaciones.Columns("id").Visible = False
        DGVObservaciones.Columns("Fechadepago").Visible = False
        DGVObservaciones.Columns("firstName").Visible = False
        DGVObservaciones.Columns("lastName").Visible = False
        DGVObservaciones.Columns("No.trxconcatenada").Visible = False
        DGVObservaciones.Columns("estatusConciliado").Visible = False
        DGVObservaciones.Columns("TC").Visible = False
        DGVObservaciones.Columns("PaidCommissionMXN").Visible = False
        DGVObservaciones.Columns("FechaCambioTC").Visible = False
        DGVObservaciones.Columns("ClienteTexto").Visible = False
        DGVObservaciones.Columns("TarifaSucursal").Visible = False
        DGVObservaciones.Columns("estatusELiminado").Visible = False




        Dim BookingStatusCode As DataGridViewColumn = DGVObservaciones.Columns("BookingStatusCode")
        BookingStatusCode.HeaderCell.Style.BackColor = Color.MediumBlue
        BookingStatusCode.HeaderCell.Style.ForeColor = Color.White

        Dim PaidStatus As DataGridViewColumn = DGVObservaciones.Columns("PaidStatus")
        PaidStatus.HeaderCell.Style.BackColor = Color.MediumBlue
        PaidStatus.HeaderCell.Style.ForeColor = Color.White



        LblTotalOBSERVACIONES.Text = DGVObservaciones.Rows.Count - 1.ToString()


    End Sub

    Public Sub consultaOnyxComisionesPendientePago()

        DGVPendientesPago.ForeColor = Color.Black
        DGVPendientesPago.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DGVPendientesPago.DataSource = objetoCN_Onyx.CN_SelectComisionesPendientePago()

        DGVPendientesPago.Columns("id").Visible = False
        DGVPendientesPago.Columns("estatusConciliado").Visible = False
        DGVPendientesPago.Columns("idBDBCD").Visible = False
        'DGVPendientesPago.Columns("mesProveedor").Visible = False
        DGVPendientesPago.Columns("estatusEliminado").Visible = False

        DGVPendientesPago.Columns("TC").Visible = False
        DGVPendientesPago.Columns("PaidCommissionMXN").Visible = False
        DGVPendientesPago.Columns("FechaCambioTC").Visible = False
        DGVPendientesPago.Columns("CondicionOKAuto").Visible = False
        DGVPendientesPago.Columns("CondicionNOAuto").Visible = False
        DGVPendientesPago.Columns("countCumplidoAuto").Visible = False
        DGVPendientesPago.Columns("countNoCumplidoAuto").Visible = False
        'DGVPendientesPago.Columns("mesproveedor").Visible = False

        Dim BookingStatusCode As DataGridViewColumn = DGVPendientesPago.Columns("BookingStatusCode")
        BookingStatusCode.HeaderCell.Style.BackColor = Color.MediumBlue
        BookingStatusCode.HeaderCell.Style.ForeColor = Color.White

        Dim PaidStatus As DataGridViewColumn = DGVPendientesPago.Columns("PaidStatus")
        PaidStatus.HeaderCell.Style.BackColor = Color.MediumBlue
        PaidStatus.HeaderCell.Style.ForeColor = Color.White

        LblTotalPendientesPago.Text = DGVPendientesPago.Rows.Count - 1.ToString()

    End Sub

    Public Sub consultaOnyxRepetidos()


        TabNavegacion.SelectedIndex = 9

        DGVRepetidos.ForeColor = Color.Black
        DGVRepetidos.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan


        DGVRepetidos.DataSource = objetoCN_OnyxRepetido.CN_DatosOnyxRepetidosMesProveedor

        DGVRepetidos.Columns("id").Visible = False


        Dim mesProveedorAnterior As DataGridViewColumn = DGVRepetidos.Columns("mesProveedorAnterior")
        mesProveedorAnterior.HeaderCell.Style.BackColor = Color.MediumBlue
        mesProveedorAnterior.HeaderCell.Style.ForeColor = Color.White

        Dim mesProveedorActual As DataGridViewColumn = DGVRepetidos.Columns("mesProveedorActual")
        mesProveedorActual.HeaderCell.Style.BackColor = Color.MediumBlue
        mesProveedorActual.HeaderCell.Style.ForeColor = Color.White

        lblTotalRepetidosOnyx.Text = DGVRepetidos.Rows.Count - 1.ToString()

    End Sub



    Public Sub consultaOnyxPaidCommision()


        TabNavegacion.SelectedIndex = 10

        DGVPaidCommision.ForeColor = Color.Black
        DGVPaidCommision.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DGVPaidCommision.DataSource = objetoCN_OnyxRepetido.CN_consultaOnyxPaidCommisionMesProveedor()

        DGVPaidCommision.Columns("id").Visible = False

        Dim mesProveedor As DataGridViewColumn = DGVPaidCommision.Columns("mesProveedor")
        mesProveedor.HeaderCell.Style.BackColor = Color.MediumBlue
        mesProveedor.HeaderCell.Style.ForeColor = Color.White

        LblTotalPaidCommision.Text = DGVPaidCommision.Rows.Count - 1.ToString()

    End Sub



    Public Sub consultaGestionCommtrack()

        objetoCN_GestionCommtrack.CN_Montototaldelareserva()

        TabNavegacion.SelectedIndex = 1

        DataGridView2.ForeColor = Color.Black
        DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DataGridView2.DataSource = objetoCN_GestionCommtrack.CN_SelectGestionCommtrack()
        DataGridView2.Columns("id").Visible = False
        DataGridView2.Columns("estatusConciliado").Visible = False

        DataGridView2.Columns("CondicionOKAuto").Visible = False
        DataGridView2.Columns("CondicionNOAuto").Visible = False
        DataGridView2.Columns("countCumplidoAuto").Visible = False
        DataGridView2.Columns("countNoCumplidoAuto").Visible = False
        DataGridView2.Columns("idBDBCD").Visible = False
        DataGridView2.Columns("mesProveedor").Visible = False
        DataGridView2.Columns("estatusEliminado").Visible = False

        DataGridView2.Columns("CondicionOKManual").Visible = False
        DataGridView2.Columns("CondicionNOManual").Visible = False
        DataGridView2.Columns("countCumplidoManual").Visible = False
        DataGridView2.Columns("countNoCumplidoManual").Visible = False
        DataGridView2.Columns("idBDBCDManual").Visible = False

        Dim firstName As DataGridViewColumn = DataGridView2.Columns("Montototaldelareserva")
        firstName.HeaderCell.Style.BackColor = Color.MediumBlue
        firstName.HeaderCell.Style.ForeColor = Color.White

        Dim lastName As DataGridViewColumn = DataGridView2.Columns("No.trxconcatenada")
        lastName.HeaderCell.Style.BackColor = Color.MediumBlue
        lastName.HeaderCell.Style.ForeColor = Color.White


        LblTotalProveedor.Text = DataGridView2.Rows.Count - 1.ToString()

    End Sub

    Public Sub consultaTacs()

        TabNavegacion.SelectedIndex = 1

        DataGridView2.ForeColor = Color.Black
        DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DataGridView2.DataSource = objetoCN_Tacs.CN_SelectTacs()
        DataGridView2.Columns("id").Visible = False
        DataGridView2.Columns("estatusConciliado").Visible = False
        DataGridView2.Columns("mesProveedor").Visible = False
        DataGridView2.Columns("estatusEliminado").Visible = False

        LblTotalProveedor.Text = DataGridView2.Rows.Count - 1.ToString()

    End Sub


    Public Sub consultaPrePago()

        TabNavegacion.SelectedIndex = 1

        DataGridView2.ForeColor = Color.Black
        DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        DataGridView2.DataSource = objetoCN_PrePago.CN_SelectPrePago()
        DataGridView2.Columns("id").Visible = False
        DataGridView2.Columns("estatusConciliado").Visible = False
        DataGridView2.Columns("mesProveedor").Visible = False
        DataGridView2.Columns("estatusEliminado").Visible = False

        LblTotalProveedor.Text = DataGridView2.Rows.Count - 1.ToString()

    End Sub

    Public Sub consultaTacsPagadas()

        DGVPagadas.ForeColor = Color.Black
        DGVPagadas.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGVPagadas.DataSource = objetoCN_Tacs.CN_SelectTacsPagadas()

        Dim observaciones As DataGridViewColumn = DGVPagadas.Columns("observaciones")
        observaciones.HeaderCell.Style.BackColor = Color.MediumBlue
        observaciones.HeaderCell.Style.ForeColor = Color.White

        Dim PayCurrency As DataGridViewColumn = DGVPagadas.Columns("PayCurrencyTC")
        PayCurrency.HeaderCell.Style.BackColor = Color.MediumBlue
        PayCurrency.HeaderCell.Style.ForeColor = Color.White

        Dim PayCom As DataGridViewColumn = DGVPagadas.Columns("PayComTC")
        PayCom.HeaderCell.Style.BackColor = Color.MediumBlue
        PayCom.HeaderCell.Style.ForeColor = Color.White

        Dim TC As DataGridViewColumn = DGVPagadas.Columns("TC")
        TC.HeaderCell.Style.BackColor = Color.MediumBlue
        TC.HeaderCell.Style.ForeColor = Color.White

        Dim FechaCambioTC As DataGridViewColumn = DGVPagadas.Columns("FechaCambioTC")
        FechaCambioTC.HeaderCell.Style.BackColor = Color.MediumBlue
        FechaCambioTC.HeaderCell.Style.ForeColor = Color.White

        DGVPagadas.Columns("id").Visible = False
        DGVPagadas.Columns("estatusConciliado").Visible = False
        DGVPagadas.Columns("idBDBCD").Visible = False
        DGVPagadas.Columns("mesProveedor").Visible = False
        DGVPagadas.Columns("estatusEliminado").Visible = False

        DGVPagadas.Columns("CondicionOKManual").Visible = False
        DGVPagadas.Columns("CondicionNOManual").Visible = False
        DGVPagadas.Columns("countCumplidoManual").Visible = False
        DGVPagadas.Columns("countNoCumplidoManual").Visible = False
        DGVPagadas.Columns("idBDBCDManual").Visible = False

        DGVPagadas.Columns("CondicionOKAuto").Visible = False
        DGVPagadas.Columns("CondicionNOAuto").Visible = False
        DGVPagadas.Columns("countCumplidoAuto").Visible = False
        DGVPagadas.Columns("countNoCumplidoAuto").Visible = False
        DGVPagadas.Columns("idBDBCD").Visible = False

        LblTotalPAGADAS.Text = DGVPagadas.Rows.Count - 1.ToString()

    End Sub

    Private Sub consultaTacsObservaciones()


        DGVObservaciones.ForeColor = Color.Black
        DGVObservaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGVObservaciones.DataSource = objetoCN_Tacs.CN_SelectTacsObservaciones

        DGVObservaciones.Columns("id").Visible = False
        DGVObservaciones.Columns("estatusConciliado").Visible = False
        DGVObservaciones.Columns("TC").Visible = False
        DGVObservaciones.Columns("FechaCambioTC").Visible = False
        DGVObservaciones.Columns("mesProveedor").Visible = False
        DGVObservaciones.Columns("estatusEliminado").Visible = False


        LblTotalOBSERVACIONES.Text = DGVObservaciones.Rows.Count - 1.ToString()

    End Sub



    Private Sub BtnAddCliente_Click(sender As Object, e As EventArgs) Handles BtnAddCliente.Click
        formProveedores = New FormProveedores
        Show()
    End Sub

    Private Sub AgegarClienteToolStripMenuItem_Click(sender As Object, e As EventArgs)
        formProveedores = New FormProveedores
        Show()
    End Sub





    Private Async Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click



        If IsNothing(lbxGruposConciliacion.SelectedItem) Then

            MessageBox.Show("Selecciona el Tipo de Conciliación")

        Else

            Dim tipoConciliacion As String = lbxGruposConciliacion.SelectedItem.ToString()

            Dim vdateln As String = DateIn.Value.Date.ToShortDateString
            Dim vdateOut As String = DateOut.Value.Date.ToShortDateString

            Dim arrayDateIn() As String
            Dim arrayDateOut() As String

            arrayDateIn = vdateln.Split(New Char() {"/"c})
            arrayDateOut = vdateOut.Split(New Char() {"/"c})

            Dim anioIN As String
            Dim monthIN As String
            Dim dayIN As String


            Dim anioOUT As String
            Dim monthOUT As String
            Dim dayOUT As String


            anioIN = arrayDateIn(2)
            monthIN = arrayDateIn(1)
            dayIN = arrayDateIn(0)

            vdateln = anioIN & "-" & monthIN & "-" & dayIN

            anioOUT = arrayDateOut(2)
            monthOUT = arrayDateOut(1)
            dayOUT = arrayDateOut(0)

            vdateOut = anioOUT & "-" & monthOUT & "-" & dayOUT

            If (ClsNGlobales.FechaPagoproveedor <> Nothing) Then

                If tipoConciliacion <> Nothing Then


                    If (idProveedorGlobal <> "0") Then

                        StartProgress()

                        MatchColumnas.ListaMatchGet = condicion.ListaCondiciones.Where(Function(t) t.Item1 = idGrupo).Select(Function(i) i.Item2).ToList()
                        listaColumnasCaracteriticas.Clear() 'vaciarElementos
                        listaColumnasCaracteriticas = MatchColumnas.MatchColumnas()

                        If (idProveedorGlobal = "1") Then 'Posadas

                            Conciliacion(vdateln, vdateOut, tipoConciliacion)

                        ElseIf (idProveedorGlobal = "2") Then

                            If (ClsNGlobales.TipoPlantillaCityExpress = 1) Then
                                ConciliacionCityExpress(vdateln, vdateOut, tipoConciliacion)
                            ElseIf (ClsNGlobales.TipoPlantillaCityExpress = 2) Then
                                ConciliacionCityExpressPlantillaB(vdateln, vdateOut, tipoConciliacion)
                            Else
                                MessageBox.Show("Seleccione un Formato")
                            End If



                        ElseIf (idProveedorGlobal = "3") Then 'Onyx

                                If (CheckBoxObservaciones.Checked) Then

                                    ConciliacionOnyxObservaciones(vdateln, vdateOut, tipoConciliacion)

                                End If

                                If (CheckBoxComisiones.Checked) Then

                                    ConciliacionOnyxComisionesPendientePago(vdateln, vdateOut, tipoConciliacion)

                                End If

                                If (CheckBoxComisiones.Checked = False And CheckBoxObservaciones.Checked = False) Then

                                    ConciliacionOnyx(vdateln, vdateOut, tipoConciliacion)

                                End If


                            ElseIf (idProveedorGlobal = "4") Then 'Tacs

                                If (CheckBoxObservaciones.Checked) Then

                                    ConciliacionTacsObservaciones(vdateln, vdateOut, tipoConciliacion)

                                End If

                                If (CheckBoxObservaciones.Checked = False) Then

                                    ConciliacionTacs(vdateln, vdateOut, tipoConciliacion)

                                End If

                            ElseIf (idProveedorGlobal = "19") Then 'Commtrack

                                ConciliacionGestionCommtrack(vdateln, vdateOut, tipoConciliacion)

                            ElseIf (idProveedorGlobal = "20") Then 'Commtrack

                                ConciliacionPrePago(vdateln, vdateOut, tipoConciliacion)

                        End If
                        CloseProgress()
                    Else

                        MessageBox.Show("Seleccione Un Proveedor")

                    End If

                Else

                    MessageBox.Show("Selecciona el Tipo de Conciliación")

                End If

            Else
                MessageBox.Show("Seleccione la Fecha del Pago del Proveedor")
            End If


        End If



        DataGridView2.DataSource = Nothing
        DataGridView2.Rows.Clear()


    End Sub

    '*************************************************CONCILIACIONES POSADAS*****************************************************************************************

    Private Sub Conciliacion(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomatico(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then
                    fullConciliaciones = tablaConcAutomatica.Copy()
                End If

            End If

        ElseIf (tipoConciliacion = "MANUAL") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlist(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then
                    fullConciliaciones.Merge(tablaConcManual)
                End If

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedor()

    End Sub

    '*************************************************CONCILIACIONES CITY EXPRESS*****************************************************************************************

    Private Sub ConciliacionCityExpress(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()


        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoCityExpress(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()

                End If

            End If


        ElseIf (tipoConciliacion = "MANUAL") Then


            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualCityExpress(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)

                End If

            End If

        End If



        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedorCityExpress()

    End Sub

    Private Sub ConciliacionCityExpressPlantillaB(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()


        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoCityExpressFormatoB(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()

                End If

            End If


        ElseIf (tipoConciliacion = "MANUAL") Then


            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualCityExpress(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)

                End If

            End If

        End If



        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedorCityExpress()

    End Sub

    '*************************************************CONCILIACIONES ONYX*****************************************************************************************


    Private Sub ConciliacionOnyx(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (tipoConciliacion = "Automatico") Then


            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoOnyx(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()

                End If

            End If

        ElseIf (tipoConciliacion = "MANUAL") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualOnyx(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)

                End If

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedorOnyx()

    End Sub

    '*************************************************CONCILIACIONES ONYX COMISIONES PENDIENTE PAGO*****************************************************************************************

    Private Sub ConciliacionOnyxComisionesPendientePago(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoOnyxComisionesPendientePago(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()

                End If

            End If

        ElseIf (tipoConciliacion = "MANUAL") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualOnyxComisionesPendientePago(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)

                End If

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            objetoCN_Conciliacion.fechaPagoOnyxComisionesPendientePago(fullConciliaciones)

        End If

        consultaOnyxComisionesPendientePago()
        TabNavegacion.SelectedIndex = 8


    End Sub

    '*************************************************CONCILIACIONES ONYX OBSERVACIONES**********************************************************************


    Private Sub ConciliacionOnyxObservaciones(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoOnyxObservaciones(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()

                    If (fullConciliaciones.Rows.Count > 0) Then

                        'Actualizar estatus ONYX OBSERVACIOENS <-----> BDBCD'
                        objetoCN_Conciliacion.estatusOnyxObservaciones(fullConciliaciones)

                    End If



                End If

            End If

        ElseIf (tipoConciliacion = "MANUAL") Then


            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualOnyxObservaciones(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)

                    If (fullConciliaciones.Rows.Count > 0) Then

                        'Actualizar estatus ONYX OBSERVACIOENS <-----> BDBCD'
                        objetoCN_Conciliacion.estatusOnyxObservaciones(fullConciliaciones)

                    End If


                End If

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then



            DataGridView3.DataSource = fullConciliaciones

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedorOnyx()

    End Sub

    '*************************************************CONCILIACIONES TACS**********************************************************************


    Private Sub ConciliacionTacs(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoTacs(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()

                End If

            End If

        ElseIf (tipoConciliacion = "MANUAL") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualTacs(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)

                End If

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedorTacs()

    End Sub

    '*************************************************CONCILIACIONES TACS OBSERVACIONES**********************************************************************


    Private Sub ConciliacionTacsObservaciones(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (tipoConciliacion = "Automatico") Then



            tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoTacsObservaciones(listaColumnasCaracteriticas, vdateln, vdateOut)

            If (tablaConcAutomatica.Rows.Count > 0) Then

                fullConciliaciones = tablaConcAutomatica.Copy()

            End If



        ElseIf (tipoConciliacion = "MANUAL") Then


            Dim multiList As New List(Of List(Of String))
            multiList = objetoCN_Tacs.ListaMatchManualTacs(idProveedorGlobal, valuesList)



            tablaConcManual = objetoCN_Conciliacion.matchlistManualTacsObservaciones(listaColumnasCaracteriticas, vdateln, vdateOut)

            If (tablaConcManual.Rows.Count > 0) Then

                fullConciliaciones.Merge(tablaConcManual)

            End If



        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesProveedorTacs()

    End Sub


    '*************************************************CONCILIACIONES GESTION COMMTRACK**********************************************************************


    Private Sub ConciliacionGestionCommtrack(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()


        If (tipoConciliacion = "Automatico") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoGestionCommtrack(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcAutomatica.Rows.Count > 0) Then

                    fullConciliaciones = tablaConcAutomatica.Copy()
                End If

            End If

        ElseIf (tipoConciliacion = "MANUAL") Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                tablaConcManual = objetoCN_Conciliacion.matchlistManualGestionCommtrack(listaColumnasCaracteriticas, vdateln, vdateOut)

                If (tablaConcManual.Rows.Count > 0) Then

                    fullConciliaciones.Merge(tablaConcManual)



                End If

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones
            DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView3.Columns("idBDBCD").Visible = False
            DataGridView3.Columns("idProveedor").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


        MostrarPendientesBDBCD()
        MostrarPendientesGestionCommtrack()

    End Sub


    '*************************************************CONCILIACIONES PREPAGO**********************************************************************

    Private Sub ConciliacionPrePago(vdateln, vdateOut, tipoConciliacion)

        DataGridView3.DataSource = Nothing
        DataGridView3.Rows.Clear()

        TabNavegacion.SelectedIndex = 3

        DataGridView3.ForeColor = Color.Black
        DataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()


        If (tipoConciliacion = "Automatico") Then

            tablaConcAutomatica = objetoCN_Conciliacion.matchlistAutomaticoPrepago(vdateln, vdateOut)

            If (tablaConcAutomatica.Rows.Count > 0) Then

                fullConciliaciones = tablaConcAutomatica.Copy()

            End If

        End If


        If (fullConciliaciones.Rows.Count > 0) Then

            DataGridView3.DataSource = fullConciliaciones

            DataGridView3.Columns("UUID").Visible = False
            DataGridView3.Columns("UUIDP").Visible = False
            DataGridView3.Columns("id").Visible = False
            DataGridView3.Columns("mesProveedor").Visible = False
            DataGridView3.Columns("proveedor").Visible = False
            DataGridView3.Columns("estatusConciliado").Visible = False

            LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

        End If


    End Sub


    '********************************************* SERIAL NUMBER GRID ****************************************

    Private Sub DataGridView1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint
        Using b As SolidBrush = New SolidBrush(DataGridView1.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub DGVPagadas_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DGVPagadas.RowPostPaint
        Using b As SolidBrush = New SolidBrush(DGVPagadas.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub DataGridView2_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView2.RowPostPaint
        Using b As SolidBrush = New SolidBrush(DataGridView2.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub DataGridView3_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView3.RowPostPaint
        Using b As SolidBrush = New SolidBrush(DataGridView3.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub DGV4_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DGV4.RowPostPaint
        Using b As SolidBrush = New SolidBrush(DGV4.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub DGVPendientesBDBCD_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DGVPendientesBDBCD.RowPostPaint
        Using b As SolidBrush = New SolidBrush(DGVPendientesBDBCD.RowHeadersDefaultCellStyle.ForeColor)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    '*******************************************REINICIAR ESTATUS*********************************************************************************
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click

        If (ClsNGlobales.FechaPagoproveedor <> Nothing) Then

            If idProveedorGlobal <> Nothing And idProveedorGlobal <> "0" Then
                If MessageBox.Show("¿Desea reiniciar las conciliaciones del proveedor seleccionado?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                    objetoCN_Conciliacion.CN_ResetEstatus(idProveedorGlobal)


                    MessageBox.Show("Estatus Reiniciado")

                End If
            Else

                MessageBox.Show("Seleccione Un proveedor")
            End If


        Else

            MessageBox.Show("Selecciona La Fecha de Pago del Proveedor")

        End If

    End Sub


    '****************************************************************************************************************************


    Public Function GetAll(ByVal control As Control, ByVal type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAll(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function


    Private Sub CboProveedores_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboProveedores.SelectedIndexChanged

        idProveedorGlobal = Nothing  'Reiniciar la variable global
        fullConciliaciones.Clear() 'Limpiar tabla de resultado de conciliaciones

        DGVPagadas.DataSource = Nothing
        DGVPagadas.Rows.Clear()

        DGVObservaciones.DataSource = Nothing
        DGVObservaciones.Rows.Clear()

        CheckBox1.Checked = False
        EliminarPrepago.Visible = False

        Dim nombreCliente As String = ""
        Dim idProveedor As String

        nombreCliente = CboProveedores.Text

        If (nombreCliente <> "CapaDatos.Combos" And nombreCliente <> "0" And nombreCliente <> "0" And nombreCliente <> "-- Selecciona Un Proveedor --") Then

            idProveedor = CboProveedores.SelectedValue


            If idProveedor <> Nothing And idProveedor <> "" Then

                Try
                    idProveedor = CboProveedores.SelectedValue
                Catch ex As Exception

                    idProveedor = 0
                End Try

                nombreCliente = nombreCliente.ToUpper

                TabPage2.Text = nombreCliente
                GroupBox2.Text = "REPORTE " & nombreCliente

                idProveedorGlobal = idProveedor    'Asignacion IdProveedor GLOBAL


                CondicionesInterfaz(idProveedorGlobal)
                condicion.ListaCondiciones.Clear() 'limpiar condiciones por proveedor

                'If CheckBoxComisiones.Checked = False Then
                '    condicion.ValidarOnyx = 1 ' Conciliacion automatica Normal
                'Else
                '    condicion.ValidarOnyx = 2 ' Conciliacion automatica para comisiones Pendientes
                'End If

                condicionesPorProveedor()
                ConsultaConciliacionesProveedor()

            End If

        End If

    End Sub

    Private Sub TxBTC_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxBTC.KeyPress

        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso (e.KeyChar <> "."c) Then
            e.Handled = True
        End If

        If (e.KeyChar = "."c) AndAlso ((TryCast(sender, TextBox)).Text.IndexOf("."c) > -1) Then
            e.Handled = True
        End If

    End Sub

    Private Sub BtnConsultaBDBCD_Click(sender As Object, e As EventArgs) Handles BtnConsultaBDBCD.Click
        StartProgress()

        'mostrarEliminadosBDBCD()
        consultaBDBCD()

        CloseProgress()
    End Sub

    Private Sub BtnConsultaProveedor_Click(sender As Object, e As EventArgs) Handles BtnConsultaProveedor.Click

        DataGridView2.DataSource = Nothing
        DataGridView2.Rows.Clear()

        If (ClsNGlobales.FechaPagoproveedor <> Nothing) Then

            If (idProveedorGlobal <> Nothing And idProveedorGlobal <> "" And idProveedorGlobal <> "0") Then

                TabNavegacion.SelectedIndex = 1

                StartProgress()

                If (idProveedorGlobal = "1") Then

                    'Rutinas
                    'ACTUALIZACIÓN
                    objetoCN_Posadas.CN_Actualizacion()
                    objetoCN_Posadas.CN_updateComision()
                    Dim res2 As Boolean = objetoCN_Posadas.CN_addTotalReserva()
                    Dim res3 As Boolean = objetoCN_Posadas.CN_addNoNoches()


                    objetoCN_Posadas.CN_quitarAcentos()
                    '''''''''''''''''''''''''''''''''''
                    consultaPosadas()
                    MostrarPendientesProveedor()

                ElseIf (idProveedorGlobal = "2") Then

                    'objetoCN_CityExpress.CN_quitarAcentos()
                    consultaCityExpress()
                    MostrarPendientesProveedorCityExpress()

                ElseIf (idProveedorGlobal = "3") Then

                    'objetoCN_Onyx.CN_quitarAcentos()
                    consultaOnyx()
                    consultaOnyxPagadas()
                    consultaOnyxObservaciones()
                    consultaOnyxComisionesPendientePago()
                    MostrarPendientesProveedorOnyx()

                ElseIf (idProveedorGlobal = "4") Then

                    objetoCN_Tacs.CN_ModificarFecha()
                    objetoCN_Tacs.CN_ModificarFechaObservaciones()
                    objetoCN_Tacs.CN_ModificarFechaPagadas()
                    consultaTacs()
                    consultaTacsPagadas()
                    consultaTacsObservaciones()
                    MostrarPendientesProveedorTacs()

                ElseIf (idProveedorGlobal = "19") Then

                    objetoCN_GestionCommtrack.CN_ModificarFecha()

                    consultaGestionCommtrack()
                    MostrarPendientesGestionCommtrack()

                ElseIf (idProveedorGlobal = "20") Then

                    consultaPrePago()

                End If


                CloseProgress()

            Else

                MessageBox.Show("Seleccione un Proveedor")

            End If

        Else

            MessageBox.Show("Selecciona El Mes y Año de Conciliacion")

        End If

    End Sub



    Private Sub btnADDTC_Click(sender As Object, e As EventArgs) Handles btnADDTC.Click


        If MessageBox.Show("¿Desea actualizar el Tipo de Cambio?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            Dim tc As Double
            Try
                tc = Convert.ToDouble(TxBTC.Text)
            Catch ex As Exception
                tc = 1.0
            End Try


            If (tc = Nothing) Then

                tc = 1.0

            End If

            If (tc <> Nothing) Then

                StartProgress()

                If (idProveedorGlobal = 4) Then
                    'TACS

                    objetoCN_Tacs.CN_changeTC(tc)

                    consultaTacsPagadas()

                Else

                    'ONYX
                    objetoCN_Onyx.CN_changeTC(tc)
                    objetoCN_Onyx.CN_changeTCPagadas(tc)
                    'objetoCN_Onyx.CN_changeTCPagadasComisionesPendientePago(tc)

                    consultaOnyx()
                    consultaOnyxPagadas()
                    consultaOnyxObservaciones()
                    consultaOnyxComisionesPendientePago()

                End If

                CloseProgress()

            Else

                MessageBox.Show("Ingrese el TC")

            End If

        End If



    End Sub


    Private Sub StartProgress()

        ShowProgress()
    End Sub

    Private Sub CloseProgress()


        frmLoad.Invoke(New Action(AddressOf frmLoad.cerrarLoad))


    End Sub

    Private Sub ShowProgress()

        frmLoad = New Presentacion.objfrmShowProgress()

        Try

            If Me.InvokeRequired Then

                Try
                    frmLoad.ShowDialog()
                Catch ex As Exception
                End Try
            Else
                Dim th As Thread = New Thread(AddressOf ShowProgress)
                th.IsBackground = False
                th.Start()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub DGVObservaciones_DoubleClick(sender As Object, e As EventArgs) Handles DGVObservaciones.DoubleClick

        formObservacionesOnyx = New ObservacionesOnyx

        Dim id As String


        If (DGVObservaciones.SelectedRows.Count > 0) Then


            id = DGVObservaciones.CurrentRow.Cells("id").Value.ToString()
            formObservacionesOnyx.id = id
            formObservacionesOnyx.ShowDialog()

        Else

            MessageBox.Show("Seleccione Una Fila")

        End If




    End Sub


    Private Sub formObservacionOnyx(res As Boolean) Handles formObservacionesOnyx.RetornoForm


        If (res) Then

            TabNavegacion.SelectedIndex = 7
            consultaOnyxObservaciones()

        End If

    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If IsNothing(lbxGruposConciliacion.SelectedItem) Then
            MessageBox.Show("Selecciona el Tipo de Conciliación")
        Else



            Dim vdateln As String = DateIn.Value.Date.ToShortDateString
            Dim vdateOut As String = DateOut.Value.Date.ToShortDateString

            Dim arrayDateIn() As String
            Dim arrayDateOut() As String

            arrayDateIn = vdateln.Split(New Char() {"/"c})
            arrayDateOut = vdateOut.Split(New Char() {"/"c})

            Dim anioIN As String
            Dim monthIN As String
            Dim dayIN As String


            Dim anioOUT As String
            Dim monthOUT As String
            Dim dayOUT As String


            anioIN = arrayDateIn(2)
            monthIN = arrayDateIn(1)
            dayIN = arrayDateIn(0)

            vdateln = anioIN & "-" & monthIN & "-" & dayIN

            anioOUT = arrayDateOut(2)
            monthOUT = arrayDateOut(1)
            dayOUT = arrayDateOut(0)

            vdateOut = anioOUT & "-" & monthOUT & "-" & dayOUT



            If (ClsNGlobales.FechaPagoproveedor <> Nothing) Then

                If (idProveedorGlobal <> "0") Then

                    StartProgress()

                    MatchColumnas.ListaMatchGet = condicion.ListaCondiciones.Where(Function(t) t.Item1 = idGrupo).Select(Function(i) i.Item2).ToList()
                    listaColumnasCaracteriticas.Clear() 'vaciarElementos
                    listaColumnasCaracteriticas = MatchColumnas.MatchColumnas()

                    If (idProveedorGlobal = "1") Then 'Posadas

                        ConciliacionA(vdateln, vdateOut)

                    ElseIf (idProveedorGlobal = "2") Then 'cityExpress

                        ConciliacionB(vdateln, vdateOut)

                    ElseIf (idProveedorGlobal = "3") Then 'onyx

                        ConciliacionC(vdateln, vdateOut)

                    ElseIf (idProveedorGlobal = "4") Then 'Tacs

                        ConciliacionD(vdateln, vdateOut)

                    ElseIf (idProveedorGlobal = "19") Then 'Commtrack

                        ConciliacionE(vdateln, vdateOut)


                    Else
                        MessageBox.Show("No disponible para Éste proveedor")
                    End If
                    CloseProgress()
                Else
                    MessageBox.Show("Seleccione Un Proveedor")
                End If

            Else

                MessageBox.Show("Selecciona La Fecha de Pago del Proveedor")

            End If


        End If


    End Sub


    '*************************************************************************************************************
    Private Sub ConciliacionA(vdateln, vdateOut) 'POSADAS

        If (idGrupo = 0) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.AutomaticoPosadas(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        ElseIf (idGrupo = 1) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.ManualPosadas(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        End If

        MostrarPendientesProveedor()
        TabNavegacion.SelectedIndex = 4

    End Sub

    '*************************************************************************************************************
    Private Sub ConciliacionB(vdateln, vdateOut) 'CITY EXPRESS

        If (idGrupo = 0) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.AutomaticoCityExpress(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        ElseIf (idGrupo = 1) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.ManualCityExpress(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        End If

        MostrarPendientesProveedorCityExpress()
        TabNavegacion.SelectedIndex = 4

    End Sub

    Private Sub ConciliacionD(vdateln, vdateOut)

        If (idGrupo = 0) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.AutomaticoTacs(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        ElseIf (idGrupo = 1) Then


            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.ManualTacs(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        End If

        MostrarPendientesProveedorTacs()
        TabNavegacion.SelectedIndex = 4

    End Sub

    Private Sub ConciliacionC(vdateln, vdateOut)


        Dim tablaConcAutomatica As DataTable = New DataTable()
        Dim tablaConcManual As DataTable = New DataTable()

        If (idGrupo = 0) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.AutomaticoOnyx(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        ElseIf (idGrupo = 1) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.ManualOnyx(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        End If

        MostrarPendientesProveedorOnyx()
        TabNavegacion.SelectedIndex = 4

    End Sub

    Private Sub ConciliacionE(vdateln, vdateOut)

        If (idGrupo = 0) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.AutomaticoGestionCommtrack(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        ElseIf (idGrupo = 1) Then

            If (listaColumnasCaracteriticas.Count > 0) Then

                Pendiente.ManualgestionCommtrack(listaColumnasCaracteriticas, vdateln, vdateOut)

            End If

        End If

        MostrarPendientesGestionCommtrack()
        TabNavegacion.SelectedIndex = 4

    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click


        Dim lastQuery As String = ""

        If (valorRadio <> 0) Then


            If idProveedorGlobal = "1" Then

                Dim idProveedor As Int32
                Dim idBDBCD As Int32

                Dim tabla As DataTable = New DataTable()
                tabla.Clear()


                If MessageBox.Show("¿DESEA CONCILIAR LOS ELEMENTOS MARCADOS?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    StartProgress()

                    For i As Integer = 0 To DGV4.Rows.Count - 1 - 1

                        idProveedor = vbEmpty
                        idBDBCD = vbEmpty

                        Dim row As DataGridViewRow = DGV4.Rows(i)

                        If CBool((row.Cells("ColumnCheck")).Value) OrElse CType(row.Cells("ColumnCheck").Value, CheckState) = CheckState.Checked Then

                            'MessageBox.Show(row.Cells("id").Value.ToString())
                            idProveedor = Convert.ToInt32(row.Cells("id").Value)

                            If (valorRadio = 1) Then
                                idBDBCD = row.Cells("idBDBCD").Value
                                lastQuery = "idBDBCD"
                            ElseIf (valorRadio = 2) Then
                                idBDBCD = row.Cells("idBDBCDManual").Value
                                lastQuery = "idBDBCDManual"
                            End If



                            tabla = objetoCN_Posadas.CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

                            Dim positionRow As Int32 = 0

                            For Each col As DataColumn In tabla.Columns
                                col.[ReadOnly] = False
                            Next

                            For Each rowTable As DataRow In tabla.Rows
                                Dim cellData As Object = rowTable("dim_value")
                                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                                Dim positionCero = cadena(0)

                                Console.WriteLine(positionCero)
                                tabla.Rows(positionRow)("dim_value") = positionCero
                                positionRow = positionRow + 1
                            Next



                            If (fullConciliaciones.Rows.Count > 0) Then

                                For Each dr As DataRow In tabla.Rows

                                    fullConciliaciones.Rows.Add(dr.ItemArray)

                                Next
                            Else

                                fullConciliaciones = tabla

                            End If




                            Console.WriteLine(idProveedor)



                        End If
                    Next




                    If (fullConciliaciones.Rows.Count > 0) Then

                        DataGridView3.DataSource = fullConciliaciones
                        DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        DataGridView3.Columns("idBDBCD").Visible = False
                        DataGridView3.Columns("idProveedor").Visible = False
                        LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

                    End If

                    MostrarPendientesBDBCD()
                    MostrarPendientesProveedor()
                    TabNavegacion.SelectedIndex = 4
                    CloseProgress()

                End If


            ElseIf (idProveedorGlobal = "2") Then

                Dim idProveedor As Int32
                Dim idBDBCD As Int32
                Dim idBDBCDManual As Int32
                Dim tabla As DataTable = New DataTable()
                tabla.Clear()


                If MessageBox.Show("¿DESEA CONCILIAR LOS ELEMENTOS MARCADOS?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    StartProgress()

                    For i As Integer = 0 To DGV4.Rows.Count - 1 - 1

                        idProveedor = vbEmpty
                        idBDBCD = vbEmpty

                        Dim row As DataGridViewRow = DGV4.Rows(i)

                        If CBool((row.Cells("ColumnCheck")).Value) OrElse CType(row.Cells("ColumnCheck").Value, CheckState) = CheckState.Checked Then

                            'MessageBox.Show(row.Cells("id").Value.ToString())
                            idProveedor = Convert.ToInt32(row.Cells("id").Value)

                            If (valorRadio = 1) Then
                                idBDBCD = row.Cells("idBDBCD").Value
                                lastQuery = "idBDBCD"
                            ElseIf (valorRadio = 2) Then
                                idBDBCD = row.Cells("idBDBCDManual").Value
                                lastQuery = "idBDBCDManual"
                            End If


                            tabla = objetoCN_CityExpress.CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

                            Dim positionRow As Int32 = 0

                            For Each col As DataColumn In tabla.Columns
                                col.[ReadOnly] = False
                            Next

                            For Each rowTable As DataRow In tabla.Rows
                                Dim cellData As Object = rowTable("dim_value")
                                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                                Dim positionCero = cadena(0)

                                Console.WriteLine(positionCero)
                                tabla.Rows(positionRow)("dim_value") = positionCero
                                positionRow = positionRow + 1
                            Next



                            If (fullConciliaciones.Rows.Count > 0) Then

                                For Each dr As DataRow In tabla.Rows

                                    fullConciliaciones.Rows.Add(dr.ItemArray)

                                Next
                            Else

                                fullConciliaciones = tabla

                            End If




                            Console.WriteLine(idProveedor)



                        End If
                    Next



                    If (fullConciliaciones.Rows.Count > 0) Then

                        DataGridView3.DataSource = fullConciliaciones
                        DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        DataGridView3.Columns("idBDBCD").Visible = False
                        DataGridView3.Columns("idProveedor").Visible = False
                        LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

                    End If

                    MostrarPendientesBDBCD()
                    MostrarPendientesProveedorCityExpress()

                    CloseProgress()
                End If


            ElseIf (idProveedorGlobal = "3") Then 'Onyx

                Dim idProveedor As Int32
                Dim idBDBCD As Int32
                Dim idBDBCDManual As Int32
                Dim tabla As DataTable = New DataTable()
                tabla.Clear()


                If MessageBox.Show("¿DESEA CONCILIAR LOS ELEMENTOS MARCADOS?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    StartProgress()

                    For i As Integer = 0 To DGV4.Rows.Count - 1 - 1

                        idProveedor = vbEmpty
                        idBDBCD = vbEmpty

                        Dim row As DataGridViewRow = DGV4.Rows(i)

                        If CBool((row.Cells("ColumnCheck")).Value) OrElse CType(row.Cells("ColumnCheck").Value, CheckState) = CheckState.Checked Then

                            'MessageBox.Show(row.Cells("id").Value.ToString())
                            idProveedor = Convert.ToInt32(row.Cells("id").Value)

                            If (valorRadio = 1) Then
                                idBDBCD = row.Cells("idBDBCD").Value
                                lastQuery = "idBDBCD"
                            ElseIf (valorRadio = 2) Then
                                idBDBCD = row.Cells("idBDBCDManual").Value
                                lastQuery = "idBDBCDManual"
                            End If


                            tabla = objetoCN_Onyx.CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

                            Dim positionRow As Int32 = 0

                            For Each col As DataColumn In tabla.Columns
                                col.[ReadOnly] = False
                            Next

                            For Each rowTable As DataRow In tabla.Rows
                                Dim cellData As Object = rowTable("dim_value")
                                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                                Dim positionCero = cadena(0)

                                Console.WriteLine(positionCero)
                                tabla.Rows(positionRow)("dim_value") = positionCero
                                positionRow = positionRow + 1
                            Next



                            If (fullConciliaciones.Rows.Count > 0) Then

                                For Each dr As DataRow In tabla.Rows

                                    fullConciliaciones.Rows.Add(dr.ItemArray)

                                Next
                            Else

                                fullConciliaciones = tabla

                            End If




                            Console.WriteLine(idProveedor)



                        End If
                    Next



                    If (fullConciliaciones.Rows.Count > 0) Then

                        DataGridView3.DataSource = fullConciliaciones
                        DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        DataGridView3.Columns("idBDBCD").Visible = False
                        DataGridView3.Columns("idProveedor").Visible = False
                        LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

                    End If


                    MostrarPendientesBDBCD()
                    MostrarPendientesProveedorOnyx()
                    CloseProgress()
                End If




            ElseIf (idProveedorGlobal = "4") Then 'TACS

                Dim idProveedor As Int32
                Dim idBDBCD As Int32
                Dim idBDBCDManual As Int32
                Dim tabla As DataTable = New DataTable()
                tabla.Clear()


                If MessageBox.Show("¿DESEA CONCILIAR LOS ELEMENTOS MARCADOS?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    StartProgress()

                    For i As Integer = 0 To DGV4.Rows.Count - 1 - 1

                        idProveedor = vbEmpty
                        idBDBCD = vbEmpty

                        Dim row As DataGridViewRow = DGV4.Rows(i)

                        If CBool((row.Cells("ColumnCheck")).Value) OrElse CType(row.Cells("ColumnCheck").Value, CheckState) = CheckState.Checked Then

                            'MessageBox.Show(row.Cells("id").Value.ToString())
                            idProveedor = Convert.ToInt32(row.Cells("id").Value)


                            If (valorRadio = 1) Then
                                idBDBCD = row.Cells("idBDBCD").Value
                                lastQuery = "idBDBCD"
                            ElseIf (valorRadio = 2) Then
                                idBDBCD = row.Cells("idBDBCDManual").Value
                                lastQuery = "idBDBCDManual"
                            End If


                            tabla = objetoCN_Tacs.CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

                            Dim positionRow As Int32 = 0

                            For Each col As DataColumn In tabla.Columns
                                col.[ReadOnly] = False
                            Next

                            For Each rowTable As DataRow In tabla.Rows
                                Dim cellData As Object = rowTable("dim_value")
                                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                                Dim positionCero = cadena(0)

                                Console.WriteLine(positionCero)
                                tabla.Rows(positionRow)("dim_value") = positionCero
                                positionRow = positionRow + 1
                            Next



                            If (fullConciliaciones.Rows.Count > 0) Then

                                For Each dr As DataRow In tabla.Rows

                                    fullConciliaciones.Rows.Add(dr.ItemArray)

                                Next
                            Else

                                fullConciliaciones = tabla

                            End If




                            Console.WriteLine(idProveedor)



                        End If
                    Next



                    If (fullConciliaciones.Rows.Count > 0) Then

                        DataGridView3.DataSource = fullConciliaciones
                        DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        DataGridView3.Columns("idBDBCD").Visible = False
                        DataGridView3.Columns("idProveedor").Visible = False
                        LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

                    End If


                    MostrarPendientesBDBCD()
                    MostrarPendientesProveedorTacs()


                    CloseProgress()
                End If

            ElseIf (idProveedorGlobal = "19") Then 'TACS

                Dim idProveedor As Int32
                Dim idBDBCD As Int32
                Dim idBDBCDManual As Int32
                Dim tabla As DataTable = New DataTable()
                tabla.Clear()


                If MessageBox.Show("¿DESEA CONCILIAR LOS ELEMENTOS MARCADOS?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    StartProgress()

                    For i As Integer = 0 To DGV4.Rows.Count - 1 - 1

                        idProveedor = vbEmpty
                        idBDBCD = vbEmpty

                        Dim row As DataGridViewRow = DGV4.Rows(i)

                        If CBool((row.Cells("ColumnCheck")).Value) OrElse CType(row.Cells("ColumnCheck").Value, CheckState) = CheckState.Checked Then

                            'MessageBox.Show(row.Cells("id").Value.ToString())
                            idProveedor = Convert.ToInt32(row.Cells("id").Value)


                            If (valorRadio = 1) Then
                                idBDBCD = row.Cells("idBDBCD").Value
                                lastQuery = "idBDBCD"
                            ElseIf (valorRadio = 2) Then
                                idBDBCD = row.Cells("idBDBCDManual").Value
                                lastQuery = "idBDBCDManual"
                            End If


                            tabla = objetoCN_GestionCommtrack.CN_ConciliarByID(idProveedor, idBDBCD, lastQuery)

                            Dim positionRow As Int32 = 0

                            For Each col As DataColumn In tabla.Columns
                                col.[ReadOnly] = False
                            Next

                            For Each rowTable As DataRow In tabla.Rows
                                Dim cellData As Object = rowTable("dim_value")
                                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                                Dim positionCero = cadena(0)

                                Console.WriteLine(positionCero)
                                tabla.Rows(positionRow)("dim_value") = positionCero
                                positionRow = positionRow + 1
                            Next



                            If (fullConciliaciones.Rows.Count > 0) Then

                                For Each dr As DataRow In tabla.Rows

                                    fullConciliaciones.Rows.Add(dr.ItemArray)

                                Next
                            Else

                                fullConciliaciones = tabla

                            End If




                            Console.WriteLine(idProveedor)



                        End If
                    Next


                    If (fullConciliaciones.Rows.Count > 0) Then

                        DataGridView3.DataSource = fullConciliaciones
                        DataGridView3.Columns("tipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        DataGridView3.Columns("idBDBCD").Visible = False
                        DataGridView3.Columns("idProveedor").Visible = False
                        LblTotalConciliacion.Text = DataGridView3.Rows.Count - 1.ToString()

                    End If



                    MostrarPendientesBDBCD()
                    MostrarPendientesGestionCommtrack()
                    CloseProgress()
                End If


            Else


                MessageBox.Show("Actualmente ésta funcion no está disponible para éste")

            End If

        Else

            MessageBox.Show("Marque el tipo de condición")

        End If



    End Sub

    Private Async Sub Button3_ClickAsync(sender As Object, e As EventArgs) Handles Button3.Click



        Dim tabla As New DataTable
        tabla.Clear()

        If idProveedorGlobal = "1" Then


            tabla = objetoCN_Posadas.CN_SelectSinConciliar()

            If tabla.Rows.Count > 0 Then


                If MessageBox.Show("¿DESEA ELIMINAR LA INFORMACIÓN SIN CONCILIAR?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                    SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
                    SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
                    If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                        If SaveFileDialog1.FileName <> "" Then
                            Dim filename As String = SaveFileDialog1.FileName

                            StartProgress()

                            Await Task.Factory.StartNew(Sub()


                                                            exportarExcelSinConciliar(filename & ".xlsx", tabla)

                                                        End Sub)
                            CloseProgress()

                        End If
                    Else
                        Exit Sub
                    End If





                End If

            End If

            MostrarPendientesProveedor()

        ElseIf (idProveedorGlobal = "2") Then


            tabla = objetoCN_CityExpress.CN_SelectSinConciliar()

            If tabla.Rows.Count > 0 Then


                If MessageBox.Show("¿DESEA ELIMINAR LA INFORMACIÓN SIN CONCILIAR?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                    SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
                    SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
                    If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                        If SaveFileDialog1.FileName <> "" Then
                            Dim filename As String = SaveFileDialog1.FileName

                            StartProgress()

                            Await Task.Factory.StartNew(Sub()


                                                            exportarExcelSinConciliarCityExpress(filename & ".xlsx", tabla)

                                                        End Sub)
                            CloseProgress()

                        End If
                    Else
                        Exit Sub
                    End If





                End If

            End If

            MostrarPendientesProveedorCityExpress()


        ElseIf (idProveedorGlobal = "3") Then

            tabla = objetoCN_Onyx.CN_SelectSinConciliar()

            If tabla.Rows.Count > 0 Then


                If MessageBox.Show("¿DESEA ELIMINAR LA INFORMACIÓN SIN CONCILIAR?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                    SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
                    SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
                    If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                        If SaveFileDialog1.FileName <> "" Then
                            Dim filename As String = SaveFileDialog1.FileName

                            StartProgress()

                            Await Task.Factory.StartNew(Sub()


                                                            exportarExcelSinConciliarOnyx(filename & ".xlsx", tabla)

                                                        End Sub)
                            CloseProgress()

                        End If
                    Else
                        Exit Sub
                    End If





                End If

            End If

            MostrarPendientesProveedorOnyx()

        ElseIf (idProveedorGlobal = "4") Then 'TACS

            tabla = objetoCN_Tacs.CN_SelectSinConciliar()

            If tabla.Rows.Count > 0 Then


                If MessageBox.Show("¿DESEA ELIMINAR LA INFORMACIÓN SIN CONCILIAR?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                    SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
                    SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
                    If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                        If SaveFileDialog1.FileName <> "" Then
                            Dim filename As String = SaveFileDialog1.FileName

                            StartProgress()

                            Await Task.Factory.StartNew(Sub()


                                                            exportarExcelSinConciliarTacs(filename & ".xlsx", tabla)

                                                        End Sub)
                            CloseProgress()

                        End If
                    Else
                        Exit Sub
                    End If





                End If

            End If

            MostrarPendientesProveedorTacs()

        ElseIf (idProveedorGlobal = "19") Then


            tabla = objetoCN_GestionCommtrack.CN_SelectSinConciliar()

            If tabla.Rows.Count > 0 Then


                If MessageBox.Show("¿DESEA ELIMINAR LA INFORMACIÓN SIN CONCILIAR?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                    SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
                    SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
                    If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                        If SaveFileDialog1.FileName <> "" Then
                            Dim filename As String = SaveFileDialog1.FileName

                            StartProgress()

                            Await Task.Factory.StartNew(Sub()


                                                            exportarExcelSinConciliarGestionCommtrack(filename & ".xlsx", tabla)

                                                        End Sub)
                            CloseProgress()

                        End If
                    Else
                        Exit Sub
                    End If

                End If

            End If

            MostrarPendientesGestionCommtrack()

        Else


            MessageBox.Show("No disponible para este proveedor")

        End If

    End Sub


    Public Sub exportarExcelSinConciliar(filename, tabla)

        objetoCN_Posadas.CN_EliminarPosadas(tabla)


        If (idProveedorGlobal = "1") Then

            tabla.Columns.Remove("id")
            tabla.Columns.Remove("estatusConciliado")
            tabla.Columns.Remove("fechaPago")
            tabla.Columns.Remove("CondicionNOAuto")
            tabla.Columns.Remove("CondicionOKAuto")
            tabla.Columns.Remove("countCumplidoAuto")
            tabla.Columns.Remove("countNoCumplidoAuto")
            tabla.Columns.Remove("idBDBCD")
            tabla.Columns.Remove("mesProveedor")
            tabla.Columns.Remove("estatusEliminado")


        End If

        ' Dim filename = "EliminadosNoConciliados"


        Using p = New ExcelPackage(New MemoryStream())

            If (tabla IsNot Nothing) Then

                If (tabla.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("Datos sin Conciliar")
                    ws.Cells("A1").LoadFromDataTable(tabla, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub


    Public Sub exportarExcelPrepago(filename, tabla)


        objetoCN_Conciliacion.eliminarPrepago(tabla)


        If (idProveedorGlobal = "20") Then

            tabla.Columns.Remove("id")
            tabla.Columns.Remove("UUID")
            tabla.Columns.Remove("UUIDP")
            tabla.Columns.Remove("mesProveedor")
            tabla.Columns.Remove("proveedor")
            tabla.Columns.Remove("estatusConciliado")

        End If

        ' Dim filename = "EliminadosNoConciliados"


        Using p = New ExcelPackage(New MemoryStream())

            If (tabla IsNot Nothing) Then

                If (tabla.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("EliminadosPrepago")
                    ws.Cells("A1").LoadFromDataTable(tabla, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub

    Public Sub exportarExcelSinConciliarCityExpress(filename, tabla)

        objetoCN_CityExpress.CN_EliminarCityExpress(tabla)


        If (idProveedorGlobal = "2") Then

            tabla.Columns.Remove("id")
            tabla.Columns.Remove("estatusConciliado")
            tabla.Columns.Remove("FechadePago")
            tabla.Columns.Remove("CondicionNOAuto")
            tabla.Columns.Remove("CondicionOKAuto")
            tabla.Columns.Remove("countCumplidoAuto")
            tabla.Columns.Remove("countNoCumplidoAuto")
            tabla.Columns.Remove("idBDBCD")
            tabla.Columns.Remove("mesProveedor")
            tabla.Columns.Remove("estatusEliminado")


        End If

        ' Dim filename = "EliminadosNoConciliados"


        Using p = New ExcelPackage(New MemoryStream())

            If (tabla IsNot Nothing) Then

                If (tabla.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("Datos sin Conciliar")
                    ws.Cells("A1").LoadFromDataTable(tabla, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub

    Public Sub exportarExcelSinConciliarOnyx(filename, tabla)

        objetoCN_Onyx.CN_EliminarOnyx(tabla)


        If (idProveedorGlobal = "3") Then

            tabla.Columns.Remove("id")
            tabla.Columns.Remove("estatusConciliado")
            'tabla.Columns.Remove("FechadePago")
            tabla.Columns.Remove("CondicionNOAuto")
            tabla.Columns.Remove("CondicionOKAuto")
            tabla.Columns.Remove("countCumplidoAuto")
            tabla.Columns.Remove("countNoCumplidoAuto")
            tabla.Columns.Remove("idBDBCD")
            tabla.Columns.Remove("mesProveedor")
            tabla.Columns.Remove("estatusEliminado")


        End If

        ' Dim filename = "EliminadosNoConciliados"


        Using p = New ExcelPackage(New MemoryStream())

            If (tabla IsNot Nothing) Then

                If (tabla.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("Datos sin Conciliar")
                    ws.Cells("A1").LoadFromDataTable(tabla, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub

    Public Sub exportarExcelSinConciliarGestionCommtrack(filename, tabla)

        objetoCN_GestionCommtrack.CN_EliminarGestionCommtrack(tabla)


        If (idProveedorGlobal = "19") Then

            tabla.Columns.Remove("id")
            tabla.Columns.Remove("estatusConciliado")
            'tabla.Columns.Remove("FechadePago")
            tabla.Columns.Remove("CondicionNOAuto")
            tabla.Columns.Remove("CondicionOKAuto")
            tabla.Columns.Remove("countCumplidoAuto")
            tabla.Columns.Remove("countNoCumplidoAuto")
            tabla.Columns.Remove("idBDBCD")
            tabla.Columns.Remove("mesProveedor")
            tabla.Columns.Remove("estatusEliminado")


        End If

        ' Dim filename = "EliminadosNoConciliados"

        Using p = New ExcelPackage(New MemoryStream())

            If (tabla IsNot Nothing) Then

                If (tabla.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("Datos sin Conciliar")
                    ws.Cells("A1").LoadFromDataTable(tabla, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub

    Public Sub exportarExcelSinConciliarTacs(filename, tabla)

        objetoCN_Tacs.CN_EliminarTacs(tabla)


        If (idProveedorGlobal = "4") Then

            tabla.Columns.Remove("id")
            tabla.Columns.Remove("estatusConciliado")
            'tabla.Columns.Remove("FechadePago")
            tabla.Columns.Remove("CondicionNOAuto")
            tabla.Columns.Remove("CondicionOKAuto")
            tabla.Columns.Remove("countCumplidoAuto")
            tabla.Columns.Remove("countNoCumplidoAuto")
            tabla.Columns.Remove("idBDBCD")
            tabla.Columns.Remove("mesProveedor")
            tabla.Columns.Remove("estatusEliminado")


        End If

        ' Dim filename = "EliminadosNoConciliados"

        Using p = New ExcelPackage(New MemoryStream())

            If (tabla IsNot Nothing) Then

                If (tabla.Rows.Count > 0) Then

                    Dim ws = p.Workbook.Worksheets.Add("Datos sin Conciliar")
                    ws.Cells("A1").LoadFromDataTable(tabla, True, TableStyles.Light13)
                    ws.Cells.AutoFitColumns()

                End If

            End If


            p.SaveAs(New FileInfo(filename))
            p.Dispose()

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filename)
                If _fi.Exists Then
                    Process.Start(filename)
                End If

            End If

        End Using


    End Sub






    Public Sub marcarTodos()
        For Each row As DataGridViewRow In DGV4.Rows
            Dim cell As DataGridViewCheckBoxCell = row.Cells(0)
            cell.Value = True
        Next
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        Dim s As Boolean = CheckBox1.Checked

        If (s) Then

            For Each row As DataGridViewRow In DGV4.Rows
                Dim cell As DataGridViewCheckBoxCell = row.Cells(0)

                If cell.Selected Then
                    cell.Value = True
                End If


            Next

        Else

            For Each row As DataGridViewRow In DGV4.Rows
                Dim cell As DataGridViewCheckBoxCell = row.Cells(0)
                'cell.Value = False

                If cell.Selected Then
                    'cell.Value = True
                    cell.Value = False
                End If
            Next

        End If


    End Sub

    Private Async Function Button5_ClickAsync(sender As Object, e As EventArgs) As Task Handles Button5.Click

        Dim lista As New List(Of Int32)
        lista.Clear()

        Dim tablaConciliaciones As New DataTable
        Dim tablaPendientesProveedor As New DataTable
        Dim tablaPendientesBDBCD As New DataTable
        Dim tablaPagadas As New DataTable
        Dim tablaObservaciones As New DataTable
        Dim tablaComisionesPendientePago As New DataTable
        Dim tablaPartidasRepetidasOnyx As New DataTable
        Dim tablaPartidasRepetidasPaidCommission As New DataTable
        Dim tablaEliminadosBCD As New DataTable

        tablaConciliaciones.Clear()
        tablaPendientesProveedor.Clear()
        tablaPendientesBDBCD.Clear()
        tablaPagadas.Clear()
        tablaObservaciones.Clear()
        tablaComisionesPendientePago.Clear()
        tablaPartidasRepetidasOnyx.Clear()
        tablaPartidasRepetidasPaidCommission.Clear()
        tablaEliminadosBCD.Clear()

        For Each item As Object In CheckedListBoxExportar.CheckedItems

            Dim index As Integer = CheckedListBoxExportar.Items.IndexOf(item)

            If (index = 0) Then

                'lista.Add(2)
                tablaConciliaciones = CType((DataGridView3.DataSource), DataTable)

            ElseIf (index = 1) Then

                'lista.Add(4)
                tablaPendientesProveedor = CType((DGV4.DataSource), DataTable)

            ElseIf (index = 2) Then
                'lista.Add(5)
                tablaPendientesBDBCD = CType((DGVPendientesBDBCD.DataSource), DataTable)

            ElseIf (index = 3) Then

                tablaPagadas = CType((DGVPagadas.DataSource), DataTable)

            ElseIf (index = 4) Then

                tablaObservaciones = CType((DGVObservaciones.DataSource), DataTable)

            ElseIf (index = 5) Then

                tablaComisionesPendientePago = CType((DGVPendientesPago.DataSource), DataTable)

            ElseIf (index = 6) Then

                tablaPartidasRepetidasOnyx = CType((DGVRepetidos.DataSource), DataTable)
            ElseIf (index = 7) Then

                tablaPartidasRepetidasPaidCommission = CType((DGVPaidCommision.DataSource), DataTable)
            ElseIf (index = 8) Then

                tablaEliminadosBCD = CType((DGVEliminadosBCD.DataSource), DataTable)

            End If



        Next

        SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
        SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

            If SaveFileDialog1.FileName <> "" Then
                Dim filename As String = SaveFileDialog1.FileName

                excelConciliacion(filename & ".xlsx", tablaConciliaciones, tablaPendientesProveedor, tablaPendientesBDBCD, tablaPagadas, tablaObservaciones, tablaComisionesPendientePago, tablaPartidasRepetidasOnyx, tablaPartidasRepetidasPaidCommission, tablaEliminadosBCD)

            End If
        Else
            Exit Function
        End If


    End Function



    Private Sub BtnGuardarConciliacion_Click(sender As Object, e As EventArgs) Handles BtnGuardarConciliacion.Click

        Dim nombreConciliacion As String = DTPFechaProveedor.Value.ToString("yyyy-MM-dd")

        If (nombreConciliacion <> "") Then

            If (idProveedorGlobal <> "0") Then

                StartProgress()

                If (idProveedorGlobal = "1") Then 'Posadas

                    objetoCN_Posadas.idProveedor = idProveedorGlobal
                    objetoCN_Posadas.NombreConciliacionPosadas = nombreConciliacion
                    objetoCN_Posadas.TablaConciliacion = fullConciliaciones
                    objetoCN_Posadas.CN_GuardarConciliacion()


                ElseIf (idProveedorGlobal = "2") Then 'City Express

                    objetoCN_CityExpress.idProveedor = idProveedorGlobal
                    objetoCN_CityExpress.NombreConciliacionCityExpress = nombreConciliacion
                    objetoCN_CityExpress.TablaConciliacion = fullConciliaciones
                    objetoCN_CityExpress.CN_GuardarConciliacion()

                ElseIf (idProveedorGlobal = "3") Then 'Onyx

                    objetoCN_Onyx.idProveedor = idProveedorGlobal
                    objetoCN_Onyx.NombreConciliacionOnyx = nombreConciliacion
                    objetoCN_Onyx.TablaConciliacion = fullConciliaciones
                    objetoCN_Onyx.CN_GuardarConciliacion()


                ElseIf (idProveedorGlobal = "4") Then 'Tacs

                    objetoCN_Tacs.idProveedor = idProveedorGlobal
                    objetoCN_Tacs.NombreConciliacionTacs = nombreConciliacion
                    objetoCN_Tacs.TablaConciliacion = fullConciliaciones
                    objetoCN_Tacs.CN_GuardarConciliacion()


                ElseIf (idProveedorGlobal = "19") Then 'Commtrack

                    objetoCN_GestionCommtrack.idProveedor = idProveedorGlobal
                    objetoCN_GestionCommtrack.NombreConciliacionGestionCommtrack = nombreConciliacion
                    objetoCN_GestionCommtrack.TablaConciliacion = fullConciliaciones
                    objetoCN_GestionCommtrack.CN_GuardarConciliacion()


                End If

            Else

                MessageBox.Show("Seleccione Un Proveedor")

            End If
            CloseProgress()

        Else
            MessageBox.Show("Ingrese el nombre de la conciliación")
        End If


    End Sub

    Private Sub BtnConsultarConciliaciones_Click(sender As Object, e As EventArgs) Handles BtnConsultarConciliaciones.Click


        DGVConciliacionesDetalle.DataSource = Nothing
        DGVConciliacionesDetalle.Rows.Clear()

        DataGridView2.DataSource = Nothing
        DataGridView2.Rows.Clear()

        If (ClsNGlobales.FechaPagoproveedor <> Nothing) Then


            If (idProveedorGlobal <> Nothing And idProveedorGlobal <> "" And idProveedorGlobal <> "0") Then

                TabNavegacion.SelectedIndex = 1

                StartProgress()

                If (idProveedorGlobal = "1") Then


                    'Rutinas

                    'ACTUALIZACIÓN
                    objetoCN_Posadas.CN_Actualizacion()
                    '''''''''''''''''''''''''''''''''''

                    consultaPosadas()
                    MostrarPendientesProveedor()


                ElseIf (idProveedorGlobal = "2") Then

                    'objetoCN_CityExpress.CN_quitarAcentos()
                    consultaCityExpress()
                    MostrarPendientesProveedorCityExpress()

                ElseIf (idProveedorGlobal = "3") Then

                    'objetoCN_Onyx.CN_quitarAcentos()
                    consultaOnyx()
                    consultaOnyxPagadas()
                    consultaOnyxObservaciones()
                    consultaOnyxComisionesPendientePago()
                    MostrarPendientesProveedorOnyx()

                ElseIf (idProveedorGlobal = "4") Then

                    objetoCN_Tacs.CN_ModificarFecha()
                    consultaTacs()
                    consultaTacsPagadas()
                    consultaTacsObservaciones()
                    MostrarPendientesProveedorTacs()

                ElseIf (idProveedorGlobal = "19") Then

                    objetoCN_GestionCommtrack.CN_ModificarFecha()

                    consultaGestionCommtrack()
                    MostrarPendientesGestionCommtrack()

                ElseIf (idProveedorGlobal = "20") Then

                    consultaPrePago()

                End If

                CloseProgress()
            Else

                MessageBox.Show("Seleccione un Proveedor")

            End If
        End If




        If (idProveedorGlobal <> "0") Then

            DGVConciliacionesDetalle.ForeColor = Color.Black
            DGVConciliacionesDetalle.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

            objetoCN_Consultas.idProveedor = idProveedorGlobal

            DGVConciliacionesDetalle.DataSource = objetoCN_Consultas.CN_ConsultaConciliacionesByFechaPagoProveedor()

            DGVConciliacionesDetalle.Columns("id").Visible = False
            DGVConciliacionesDetalle.Columns("idConciliacion").Visible = False
            DGVConciliacionesDetalle.Columns("TipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            TotalDetalleConciliacion.Text = DGVConciliacionesDetalle.Rows.Count - 1.ToString()

        Else

            MessageBox.Show("Seleccione Un Proveedor")

        End If


    End Sub

    Private Sub ConsultaConciliacionesProveedor()

        If (idProveedorGlobal <> "0") Then

            DGVConciliaciones.ForeColor = Color.Black
            DGVConciliaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

            objetoCN_Consultas.idProveedor = idProveedorGlobal
            DGVConciliaciones.DataSource = objetoCN_Consultas.CN_ConsultaConciliacionesByIdProveedor()



            DGVConciliaciones.Columns("id").Visible = False
            DGVConciliaciones.Columns("idProveedor").Visible = False


        Else

            MessageBox.Show("Seleccione Un Proveedor")

        End If

    End Sub

    Private Sub DGVConciliaciones_DoubleClick(sender As Object, e As EventArgs) Handles DGVConciliaciones.DoubleClick


        DGVConciliacionesDetalle.DataSource = Nothing
        DGVConciliacionesDetalle.Rows.Clear()

        If (DGVConciliaciones.SelectedRows.Count > 0) Then

            Dim tablaDetalle As New DataTable

            'txtNombreCliente.Text = DataGridView1.CurrentRow.Cells("Nombre").Value.ToString()
            Dim id As Integer = DGVConciliaciones.CurrentRow.Cells("id").Value.ToString()
            objetoCN_Consultas.idConciliacion = id

            tablaDetalle = objetoCN_Consultas.CN_ConsultaConciliacionesDetalleByIdConciliacion()

            If tablaDetalle.Rows.Count > 0 Then

                DGVConciliacionesDetalle.ForeColor = Color.Black
                DGVConciliacionesDetalle.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan

                DGVConciliacionesDetalle.DataSource = tablaDetalle
                DGVConciliacionesDetalle.Columns("id").Visible = False
                DGVConciliacionesDetalle.Columns("idConciliacion").Visible = False
                DGVConciliacionesDetalle.Columns("TipoConciliacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

                TotalDetalleConciliacion.Text = DGVConciliacionesDetalle.Rows.Count - 1.ToString()

            End If

        Else

            MessageBox.Show("Seleccione Una Fila")

        End If

    End Sub





    Private Sub DTPFechaProveedor_ValueChanged(sender As Object, e As EventArgs) Handles DTPFechaProveedor.ValueChanged

        Dim fechaPagoProveedor As String = DTPFechaProveedor.Value.ToString("yyyy-MM-dd")
        ClsNGlobales.FechaPagoproveedor = fechaPagoProveedor

    End Sub

    Private Sub DTPInicio_ValueChanged(sender As Object, e As EventArgs) Handles DTPInicio.ValueChanged

        Dim fechaProveedorInicio As String = DTPInicio.Value.ToString("yyyy-MM-dd")
        ClsNGlobales.FechaProveedorInicio = fechaProveedorInicio

    End Sub

    Private Sub DTPFin_ValueChanged(sender As Object, e As EventArgs) Handles DTPFin.ValueChanged

        Dim fechaProveedorFin As String = DTPFin.Value.ToString("yyyy-MM-dd")
        ClsNGlobales.FechaProveedorFin = fechaProveedorFin

    End Sub

    Private Sub EliminarPrepago_Click(sender As Object, e As EventArgs) Handles EliminarPrepago.Click


        If (fullConciliaciones.Rows.Count > 0) Then


            If MessageBox.Show("¿DESEA ELIMINAR LA INFORMACIÓN?", "Mensaje!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then


                SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
                SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
                If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

                    If SaveFileDialog1.FileName <> "" Then
                        Dim filename As String = SaveFileDialog1.FileName

                        StartProgress()

                        exportarExcelPrepago(filename & ".xlsx", fullConciliaciones)

                        CloseProgress()

                    End If
                Else
                    Exit Sub
                End If
            End If

        End If

        consultaPrePago()


    End Sub

    Private Sub TabNavegacion_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabNavegacion.DrawItem

        Try

            'Firstly we'll define some parameters.
            Dim CurrentTab As TabPage = TabNavegacion.TabPages(e.Index)
            Dim ItemRect As Rectangle = TabNavegacion.GetTabRect(e.Index)
            Dim FillBrush As New SolidBrush(Color.RoyalBlue)
            Dim TextBrush As New SolidBrush(Color.White)
            Dim sf As New StringFormat
            sf.Alignment = StringAlignment.Center
            sf.LineAlignment = StringAlignment.Center

            If CBool(e.State And DrawItemState.Selected) Then

                FillBrush.Color = Color.White
                TextBrush.Color = Color.Black
                ItemRect.Inflate(2, 2)

            End If


            'Set up rotation for left and right aligned tabs
            If TabNavegacion.Alignment = TabAlignment.Left Or TabNavegacion.Alignment = TabAlignment.Right Then

                Dim RotateAngle As Single = 90
                If TabNavegacion.Alignment = TabAlignment.Left Then RotateAngle = 270
                Dim cp As New PointF(ItemRect.Left + (ItemRect.Width \ 2), ItemRect.Top + (ItemRect.Height \ 2))
                e.Graphics.TranslateTransform(cp.X, cp.Y)
                e.Graphics.RotateTransform(RotateAngle)
                ItemRect = New Rectangle(-(ItemRect.Height \ 2), -(ItemRect.Width \ 2), ItemRect.Height, ItemRect.Width)

            End If

            'Next we'll paint the TabItem with our Fill Brush
            e.Graphics.FillRectangle(FillBrush, ItemRect)

            'Now draw the text.
            e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, RectangleF.op_Implicit(ItemRect), sf)

            'Reset any Graphics rotation
            e.Graphics.ResetTransform()

            'Finally, we should Dispose of our brushes.
            FillBrush.Dispose()
            TextBrush.Dispose()

        Catch ex As Exception

        End Try




    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        valorRadio = 0

        If (RadioButton1.Checked = True) Then

            valorRadio = 1
        Else
            RadioButton2.Checked = False

        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

        valorRadio = 0

        If (RadioButton2.Checked = True) Then

            valorRadio = 2
        Else
            RadioButton1.Checked = False

        End If

    End Sub




    Public Sub CondicionesInterfaz(idCliente As String)

        'Radio CityExpress

        radioFormato1.Visible = False
        radioFormato2.Visible = False


        Try

            CheckedListBoxExportar.Items.RemoveAt(7)
            CheckedListBoxExportar.Items.RemoveAt(6)
            CheckedListBoxExportar.Items.RemoveAt(5)
            CheckedListBoxExportar.Items.RemoveAt(4)
            CheckedListBoxExportar.Items.RemoveAt(3)

        Catch ex As Exception

        End Try


        TabNavegacion.TabPages.Remove(TabPage6)
        TabNavegacion.TabPages.Remove(TabPage7)
        TabNavegacion.TabPages.Remove(TabPage11)
        TabNavegacion.TabPages.Remove(TabPage12)
        TabNavegacion.TabPages.Remove(TabPage13)


        CheckBoxComisiones.Visible = False
        CheckBoxObservaciones.Visible = False

        LblTC.Visible = True
        TxBTC.Visible = True
        btnADDTC.Visible = True


        If (idCliente = 20) Then

            EliminarPrepago.Visible = True
            LblTC.Visible = False
            TxBTC.Visible = False
            btnADDTC.Visible = False


        End If

        If (idCliente = 3) Then 'Únicamente Para ONYX


            CheckedListBoxExportar.Items.Add("Pagadas")
            CheckedListBoxExportar.Items.Add("Observaciones")
            CheckedListBoxExportar.Items.Add("Comisiones Pendiente Pago")
            CheckedListBoxExportar.Items.Add("Partidas Repetidas onyx")
            CheckedListBoxExportar.Items.Add("PaidCommission Onyx")

            CheckBoxComisiones.Visible = True
            CheckBoxObservaciones.Visible = True

            TabNavegacion.TabPages.Insert(6, TabPage6)
            TabNavegacion.TabPages.Insert(7, TabPage7)
            TabNavegacion.TabPages.Insert(8, TabPage11)
            TabNavegacion.TabPages.Insert(9, TabPage12)
            TabNavegacion.TabPages.Insert(10, TabPage13)

            LblTC.Visible = True
            TxBTC.Visible = True
            btnADDTC.Visible = True

        ElseIf (idCliente = 19) Then 'Únicamente Para COMMTRACK AUTO Then

            LblTC.Visible = False
            TxBTC.Visible = False
            btnADDTC.Visible = False

        ElseIf (idCliente = 1) Then 'Únicamente Para POSADAS MANUAL Then

            LblTC.Visible = False
            TxBTC.Visible = False
            btnADDTC.Visible = False

        ElseIf (idCliente = 4) Then 'Únicamente Para Tacs MANUAL

            CheckedListBoxExportar.Items.Add("Pagadas")
            CheckedListBoxExportar.Items.Add("Observaciones")

            TabNavegacion.TabPages.Insert(6, TabPage6)
            TabNavegacion.TabPages.Insert(7, TabPage7)

            CheckBoxObservaciones.Visible = True

            LblTC.Visible = True
            TxBTC.Visible = True
            btnADDTC.Visible = True

        ElseIf (idCliente = 2) Then

            LblTC.Visible = False
            TxBTC.Visible = False
            btnADDTC.Visible = False

            radioFormato1.Visible = True
            radioFormato2.Visible = True

        End If


    End Sub


    Private Sub ConciliacionComisionesHoteles_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Radio CityExpress
        radioFormato1.Visible = False
        radioFormato2.Visible = False
        '''''''''''''''''''''''''''''''''''''

        actualizacion.ActualizarFechaGestion()

        llenarListBoxGruposDefault()

        DGV4.Columns(0).Frozen = True

        EliminarPrepago.Visible = False


        TabNavegacion.TabPages.Remove(TabPage6)
        TabNavegacion.TabPages.Remove(TabPage7)
        TabNavegacion.TabPages.Remove(TabPage11)

        ''''Check para conciliar pestaña extra Onyx
        CheckBoxComisiones.Visible = False
        '''''''''''''''Check para conciliar pestañas Observaciones'''''''''''''''''''
        CheckBoxObservaciones.Visible = False
        ''''Formato Fecha Inicio y Fecha Fin

        Dim MyDate As Date = Now
        Dim DaysInMonth As Integer = Date.DaysInMonth(MyDate.Year, MyDate.Month)
        Dim LastDayInMonthDate As Date = New Date(MyDate.Year, MyDate.Month, DaysInMonth)
        DTPInicio.Value = New Date(Now.Year, Now.Month, 1)
        DTPFin.Value = LastDayInMonthDate

        Dim fechaProveedorInicio As String = DTPInicio.Value.ToString("yyyy-MM-dd")
        Dim fechaProveedorFin As String = DTPFin.Value.ToString("yyyy-MM-dd")

        ClsNGlobales.FechaProveedorInicio = fechaProveedorInicio
        ClsNGlobales.FechaProveedorFin = fechaProveedorFin


        Dim fechaPagoProveedor As String = DTPFechaProveedor.Value.ToString("yyyy-MM-dd")
        ClsNGlobales.FechaPagoproveedor = fechaPagoProveedor


        Dim comboSource As New Dictionary(Of String, String)()
        comboSource.Add("01", "Enero")
        comboSource.Add("02", "Febrero")
        comboSource.Add("03", "Marzo")
        comboSource.Add("04", "Abril")
        comboSource.Add("05", "Mayo")
        comboSource.Add("06", "Junio")
        comboSource.Add("07", "Julio")
        comboSource.Add("08", "Agosto")
        comboSource.Add("09", "Septiembre")
        comboSource.Add("10", "Octubre")
        comboSource.Add("11", "Noviembre")
        comboSource.Add("12", "Diciembre")

        Dim comboSourceB As New Dictionary(Of String, String)()
        comboSourceB.Add("2019", "2019")
        comboSourceB.Add("2018", "2018")
        comboSourceB.Add("2017", "2017")
        comboSourceB.Add("2016", "2016")

        Dim comboSourceC As New Dictionary(Of String, String)()
        comboSourceC.Add("00", "--Mes--")
        comboSourceC.Add("01", "Enero")
        comboSourceC.Add("02", "Febrero")
        comboSourceC.Add("03", "Marzo")
        comboSourceC.Add("04", "Abril")
        comboSourceC.Add("05", "Mayo")
        comboSourceC.Add("06", "Junio")
        comboSourceC.Add("07", "Julio")
        comboSourceC.Add("08", "Agosto")
        comboSourceC.Add("09", "Septiembre")
        comboSourceC.Add("10", "Octubre")
        comboSourceC.Add("11", "Noviembre")
        comboSourceC.Add("12", "Diciembre")

        Dim comboSourceD As New Dictionary(Of String, String)()
        comboSourceD.Add("0000", "--Año--")
        comboSourceD.Add("2019", "2019")
        comboSourceD.Add("2018", "2018")
        comboSourceD.Add("2017", "2017")
        comboSourceD.Add("2016", "2016")

        'dd/mm/yy
        DateIn.Value = "01/01/2018"

        Dim column As DataGridViewColumn = DGV4.Columns(0)
        column.Width = 45

        'DateIn.Format = DateTimePickerFormat.Custom
        'DateIn.CustomFormat = "dd/MM/yyyy"

        'DateOut.Format = DateTimePickerFormat.Custom
        'DateOut.CustomFormat = "dd/MM/yyyy"


        TxBTC.Visible = False
        LblTC.Visible = False
        btnADDTC.Visible = False

        BtnAddCliente.Visible = False
        Button7.Enabled = True
        btnEliminarGrupo.Enabled = True
        btnAgregarNuevaConciliaciion.Enabled = True

        FillComboProveedores()

        DTPFechaProveedor.Value = "01/11/2019"
        DTPInicio.Value = "01/11/2019"
        DTPFin.Value = "01/11/2019"



    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' GRUPOS INICIO '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub mostrarGrupos()
        lbxGruposConciliacion.DataSource = grupo.ListaGrupos
    End Sub

    Private Sub llenarListBoxGruposDefault()
        grupo.GruposDefault()
        mostrarGrupos()
    End Sub

    Private Sub BtnAgregarGrupo_Click(sender As Object, e As EventArgs) Handles BtnAgregarGrupo.Click

        formGrupos = New Presentacion.AgregarNuevoGrupo()

        If idProveedorGlobal <> Nothing And idProveedorGlobal <> "0" Then

            formGrupos.Show()

        Else
            MessageBox.Show("Seleccione un Proveedor")
        End If


    End Sub

    Private Sub formGruposTrasferircadena(nombreGrupo As String) Handles formGrupos.trasferirCadena

        Dim grupos = From grupo In grupo.ListaGrupos Where grupo = nombreGrupo
        If Not grupos.Any() Then
            grupo.NombreGrupo = nombreGrupo
            grupo.AddGrupo()
        Else
            MessageBox.Show("El grupo ya existe")
        End If

        mostrarGrupos()
    End Sub


    Private Sub btnEliminarGrupo_Click(sender As Object, e As EventArgs) Handles btnEliminarGrupo.Click

        If lbxGruposConciliacion.SelectedIndex < 0 Then
            MessageBox.Show("Seleccione un Grupo")
        Else

            idGrupo = lbxGruposConciliacion.SelectedIndex
            eliminarGrupo(idGrupo)

        End If

    End Sub

    Private Sub eliminarGrupo(ByVal idGrupo As Integer)

        If idGrupo <> 0 And idGrupo <> 1 Then

            Dim contador As Integer
            Dim accion As DialogResult

            Dim query As IEnumerable(Of Tuple(Of Integer, String)) = condicion.ListaCondiciones.Where(Function(t) t.Item1 = idGrupo)

            For Each item In query

                contador = contador + 1

            Next

            If contador > 0 Then

                accion = MessageBox.Show("El grupo tiene Condiciones, ¿Desea Eliminarlo?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If accion = vbYes Then
                    eliminarCondicionesPorGrupo(idGrupo)

                    grupo.ListaGrupos.RemoveAt(idGrupo)
                End If

            Else

                accion = MessageBox.Show("¿Desea Eliminar el Grupo?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If accion = vbYes Then
                    grupo.ListaGrupos.RemoveAt(idGrupo)
                End If

            End If

            mostrarGrupos()

        Else
            MessageBox.Show("Este grupo no se puede eliminar")
        End If

    End Sub

    Private Sub eliminarCondicionesPorGrupo(ByVal idGrupo As Integer)

        Dim t = condicion.ListaCondiciones.FirstOrDefault(Function(i) i.Item1 = idGrupo)

        If t IsNot Nothing Then

            condicion.ListaCondiciones.Remove(t)
            mostrarCondiciones(idGrupo)

        End If

    End Sub

    Private Sub condicionesPorProveedor()

        'Dim idGrupo As Integer

        If lbxGruposConciliacion.SelectedIndex < 0 Then

        Else

            condicion.IdProveedor = idProveedorGlobal

            If condicion.ListaCondiciones.Count <= 0 Then

                condicion.condicionesAutomaticas()
                condicion.condicionesAutoExtras()
                condicion.condicionesManuales()

            End If

            idGrupo = lbxGruposConciliacion.SelectedIndex
            mostrarCondiciones(idGrupo)

        End If

    End Sub

    Private Sub mostrarCondiciones(ByVal idGrupo As Integer)

        Dim query As IList(Of Tuple(Of Integer, String)) = condicion.ListaCondiciones.Where(Function(t) t.Item1 = idGrupo).ToList()
        lbxConciliacionesDeGrupo.DataSource = query
        lbxConciliacionesDeGrupo.DisplayMember = "Item2"

    End Sub

    Private Sub lbxGruposConciliacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbxGruposConciliacion.SelectedIndexChanged

        If idProveedorGlobal <> Nothing And idProveedorGlobal <> "0" Then

            condicionesPorProveedor()

        End If

    End Sub


    Private Sub BtnAgregarNuevaConciliaciion_Click(sender As Object, e As EventArgs) Handles btnAgregarNuevaConciliaciion.Click

        fr2 = New Presentacion.AgregarNuevaConciliacion()

        If idProveedorGlobal <> Nothing And idProveedorGlobal <> "0" Then

            fr2.listaGrupos = grupo.ListaGrupos
            fr2.idProveedor = idProveedorGlobal
            fr2.Show()

        Else
            MessageBox.Show("Seleccione un Proveedor")
        End If

    End Sub

    Private Sub pasarCondicion(array() As String) Handles fr2.PasarCondicion

        Dim idGrupo As Integer
        Dim condicionRetorno As String
        Dim grupo As String = ""
        Dim arraySplit() As String
        Dim cadena As String = ""

        If (array(0) <> "" And array(1) <> "") Then

            cadena = array(0)

            arraySplit = cadena.Split(New Char() {","c})

            grupo = arraySplit(4)

            condicionRetorno = array(1)

            If String.IsNullOrWhiteSpace(condicionRetorno) = False Then

                If lbxGruposConciliacion.SelectedIndex < 0 Then

                Else
                    idGrupo = array(2)

                    Dim condiciones =
                    From condicion In condicion.ListaCondiciones Where condicion.Item2 = condicionRetorno AndAlso condicion.Item1 = idGrupo

                    If Not condiciones.Any() Then

                        condicion.TipoGrupoCondicion = idGrupo
                        condicion.NombreCondicion = condicionRetorno
                        condicion.AddCondicion()
                        lbxGruposConciliacion.SelectedIndex = idGrupo

                    Else
                        MessageBox.Show("La condicion ya existe")
                    End If

                    mostrarCondiciones(idGrupo)

                End If

            End If

        End If

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        eliminarCondicion()

    End Sub

    Private Sub eliminarCondicion()

        Dim idGrupo As Integer
        Dim condicionCadena As String

        If lbxGruposConciliacion.SelectedIndex < 0 Then

        Else

            idGrupo = lbxGruposConciliacion.SelectedIndex

            If lbxConciliacionesDeGrupo.SelectedIndex < 0 Then
                MessageBox.Show("Selecciona una Condición")
            Else
                condicionCadena = lbxConciliacionesDeGrupo.[Text]


                Dim condicionesDelete =
                From condicionDelete In condicion.ListaCondiciones Where condicionDelete.Item2 = condicionCadena AndAlso condicionDelete.Item1 = idGrupo

                If condicionesDelete.Any() Then
                    condicion.ListaCondiciones.Remove(condicionesDelete.FirstOrDefault)
                End If

                mostrarCondiciones(idGrupo)

            End If

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim listaGrupos As New BindingList(Of String)

        Dim idLabel As Integer = lbxConciliacionesDeGrupo.SelectedIndex
        Dim cadena As String = lbxConciliacionesDeGrupo.[Text]

        Dim grupo As String = lbxGruposConciliacion.SelectedItem
        Dim idGrupo As String = lbxGruposConciliacion.SelectedIndex

        If (cadena <> "") Then

            tuplaIndex = condicion.ListaCondiciones.FindIndex(Function(t) t.Item1 = idGrupo AndAlso t.Item2 = cadena)

            fr3 = New Presentacion.AgregarNuevaConciliacion

            For Each listBoxItem In lbxGruposConciliacion.Items
                listaGrupos.Add(listBoxItem)
            Next

            fr3.listaGrupos = listaGrupos
            fr3.idProveedor = idProveedorGlobal
            fr3.idLista = idLabel
            fr3.cadenaGet = cadena
            fr3.grupoGet = grupo
            fr3.Show()

        Else
            MessageBox.Show("Selecciona una condición")
        End If

    End Sub

    Private Sub fr3_passvalue(array() As String, id As Int16) Handles fr3.PassvalueUpdate

        Dim ints As Int16 = array.Count
        Dim grupo As String = ""
        Dim arraySplit() As String
        Dim cadena As String = ""

        Dim cadenaActualizada As String
        Dim idGrupo As Integer


        If (array(0) <> "" And array(1) <> "") Then

            cadena = array(0)
            arraySplit = cadena.Split(New Char() {","c})
            grupo = arraySplit(4)

            cadenaActualizada = array(1)

            If String.IsNullOrWhiteSpace(cadenaActualizada) = False Then

                If lbxGruposConciliacion.SelectedIndex < 0 Then

                Else

                    idGrupo = array(2)

                    Dim condiciones =
                    From condicion In condicion.ListaCondiciones Where condicion.Item2 = cadenaActualizada AndAlso condicion.Item1 = idGrupo
                    If Not condiciones.Any() Then


                        condicion.ListaCondiciones(tuplaIndex) = Tuple.Create(idGrupo, cadenaActualizada)

                    Else
                        MessageBox.Show("La condición ya existe")
                    End If

                    mostrarCondiciones(idGrupo)


                End If

            End If

        End If

    End Sub

    Private Sub CheckBoxComisiones_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxComisiones.CheckedChanged

        If CheckBoxComisiones.Checked = False Then
            condicion.ValidarOnyx = 1 ' Conciliacion automatica Normal
        Else
            condicion.ValidarOnyx = 2 ' Conciliacion automatica para comisiones Pendientes
        End If
        condicion.ListaCondiciones.Clear() 'limpiar condiciones por proveedor
        condicionesPorProveedor()

    End Sub

    Private Sub BtnConsultaRepetidos_Click(sender As Object, e As EventArgs) Handles BtnConsultaRepetidos.Click

        consultaOnyxRepetidos()


    End Sub

    Private Sub BtnPaidCommission_Click(sender As Object, e As EventArgs) Handles BtnPaidCommission.Click
        consultaOnyxPaidCommision()
    End Sub

    Private Sub ConfigurarTipoDeCambioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurarTipoDeCambioToolStripMenuItem.Click

        formularioTipoCambio = New Presentacion.Tipo_de_Cambio()

        If idProveedorGlobal <> Nothing And idProveedorGlobal <> "0" Then

            'formularioTipoCambio.idProveedor = idProveedorGlobal
            ClsNGlobales.idProveedor = idProveedorGlobal
            formularioTipoCambio.Show()

        Else
            MessageBox.Show("Seleccione un Proveedor")
        End If

    End Sub

    Private Sub RadioFormato1_CheckedChanged(sender As Object, e As EventArgs) Handles radioFormato1.CheckedChanged

        If (radioFormato1.Checked = True) Then

            tipoArchivoCityExpress = 1
            condicion.ValidarFormatoCityExpress = 1
            ClsNGlobales.TipoPlantillaCityExpress = 1
        Else

            radioFormato2.Checked = False

        End If

        condicion.ListaCondiciones.Clear() 'limpiar condiciones por proveedor
        condicionesPorProveedor()
    End Sub

    Private Sub RadioFormato2_CheckedChanged(sender As Object, e As EventArgs) Handles radioFormato2.CheckedChanged

        If (radioFormato2.Checked = True) Then

            tipoArchivoCityExpress = 2
            condicion.ValidarFormatoCityExpress = 2
            ClsNGlobales.TipoPlantillaCityExpress = 2

        Else

            radioFormato1.Checked = False

        End If

        condicion.ListaCondiciones.Clear() 'limpiar condiciones por proveedor
        condicionesPorProveedor()

    End Sub

    Private Sub CheckBoxEliminarCancelados_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEliminarCancelados.CheckedChanged

        If (CheckBoxEliminarCancelados.Checked = True) Then

            eliminarCancelados = 1

        Else

            eliminarCancelados = 0
        End If

    End Sub

    Private Sub BtnEliminados_Click(sender As Object, e As EventArgs) Handles btnEliminados.Click

        mostrarEliminadosBDBCD()

    End Sub



    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

        If (CheckBox2.Checked = True) Then

            ClsNGlobales.ActuaizarSegmento = 1

        Else
            ClsNGlobales.ActuaizarSegmento = 0

        End If

    End Sub




    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' GRUPOS FIN '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

End Class