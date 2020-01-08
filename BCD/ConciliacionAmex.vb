Imports System.Data.OleDb
Imports System.Globalization
Imports System.Text
Imports Microsoft.CodeAnalysis.CSharp.Scripting
Imports Microsoft.CodeAnalysis.Scripting
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Imports LinqExtensions
Imports NPOI.HSSF.UserModel
Imports ClosedXML.Excel

Public Class ConciliacionAmex
    Private selectEdo As New ArrayList
    Private selectIcaav As New ArrayList
    Private arrayEncontroEdo As New ArrayList
    Private arrayEncontroIcaav As New ArrayList
    Private coincidencia As New ArrayList
    Private cuerpoConciliacionIcaav As New DataGridView
    Private arrayEdo As New ArrayList
    Private numFactura As Integer = 0
    Private numOrden As Integer = 0
    Private columna_documemnto As Integer = 0
    Private ExcelEstadoDeCuentaLoaded As Boolean = False
    Private ExcelIcaavLoaded As Boolean = False
    Public Event _reporteEstadoDeCuentaLoaded As EventHandler
    Public Event _reporteIcaavLoaded As EventHandler
    Public Event ProcesoConciliacionIniciado As EventHandler
    Public Event ProcesoConciliacionTerminado As EventHandler(Of ProcesoConciliacionTerminadoArgs)
    Public Event DatosConciliacionProcesadosCorrectamente As EventHandler
    Public Event ProgressSaveFileChanged(sender As Object, e As ProgressSaveFileChangedEventArgs)
    Public Event ProgressSaveFileStart As EventHandler
    Public Event ProgressSaveFileEnd As EventHandler
    Public Event AgregarNuevoGrupoConciliacion As EventHandler(Of AgregarNuevoGrupoConciliacionArgs)
    Public Event ModificarGrupoConciliacion As EventHandler(Of AgregarNuevoGrupoConciliacionArgs)
    Public Event ReiniciarFormularioConciliacion As EventHandler
    Private ListaConciliaciones As List(Of Conciliacion)
    Private condiciones_ec_null_builder As StringBuilder = New StringBuilder()
    Private condiciones_ic_null_builder As StringBuilder = New StringBuilder()
    Private condiciones_ec_numeric_parse_builder As StringBuilder = New StringBuilder()
    Private condiciones_ic_numeric_parse_builder As StringBuilder = New StringBuilder()
    Private condiciones As StringBuilder = New StringBuilder()
    Private DictRowsConciliaciones As Dictionary(Of DataRow, DataRow) = New Dictionary(Of DataRow, DataRow)()
    Private ConflictosEnConciliacion As Boolean = False
    Private ListaGruposConciliaciones As BindingList(Of GrupoConciliaciones)
    Private ReportesTabs As Dictionary(Of String, TabPage)
    Private GridsReportesTabs As Dictionary(Of String, DataGridView)
    Private dtEnumEC As List(Of DataRow)
    Private dtEnumIC As List(Of DataRow)
    Dim dtEstadoDeCuenta As DataTable
    Dim dtIcaav As DataTable

    Private dtIcaavNoEncontrados As DataTable
    Private dtECNoEncontrados As DataTable
    Private dtICResultEnum As DataTable
    Private dtECResultEnum As DataTable

    'nuevo agrgado ismael 230718
    Private dtIcaavNoEncontrados_2 As DataTable
    Private dtECNoEncontrados_2 As DataTable
    Private dtICResultEnum_2 As DataTable
    Private dtECResultEnum_2 As DataTable

    'nuevo agrgado ismael 230718
    Public bandera_c As Integer = 0
    Public bandera_report As Integer = 0

    Public bandera_conflic As Integer = 0


    Dim dtICConflict_1 As DataTable
    Dim dtECConflictos_1 As DataTable

    Dim dtECConflict_1 As DataTable

    Dim dtIcaavConflictos_1 As DataTable

    Private workbook As IWorkbook
    Private cxml_workbook As XLWorkbook



    Private ConciliacionGruposColumnas As Dictionary(Of GrupoConciliaciones, List(Of String))

    Private Sub btn_edo_cuenta_Click(sender As Object, e As EventArgs) Handles btn_edo_cuenta.Click
        Me.ExcelEstadoDeCuentaLoaded = False
        'btnCrearConciliacion.Enabled = False
        TabNavegacion.SelectedIndex = 0
        Try
            OpenFileDialog1.Filter = "ARCHIVOS DE EXCEL 2007-2013 (.xlsx)|*.xlsx"
            OpenFileDialog1.FilterIndex = 0
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Multiselect = False
            OpenFileDialog1.RestoreDirectory = True

            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                If OpenFileDialog1.FileName IsNot String.Empty Then
                    txt_excel_edo.Text = OpenFileDialog1.FileName
                    Dim sheetList As List(Of String) = GetSheetListFromExcel(OpenFileDialog1.FileName)
                    cmb_hoja_edo.DataSource = sheetList
                    cmb_hoja_edo.SelectedIndex = -1
                    If MsgBox("SELECCIONE LA HOJA DE TRABAJO", MsgBoxStyle.Information) = MsgBoxResult.Ok Then
                        cmb_hoja_edo.Enabled = True
                    End If
                End If
            Else
                txt_excel_edo.Text = ""
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("ERROR: " & ex.Message)
        End Try

        'SeleccionaArchivo(txt_excel_edo, cmb_hoja_edo)
    End Sub

    Private Function RemoveBlankColumns(ByVal tbl As DataTable, ParamArray ignoreCols As String())
        Dim tbl_copy As DataTable = tbl.Copy()
        Dim columns = tbl_copy.Columns.Cast(Of DataColumn)().Where(Function(c) Not ignoreCols.Contains(c.ColumnName, StringComparer.OrdinalIgnoreCase))
        Dim rows = tbl_copy.AsEnumerable()
        Dim null_columns = columns.Where(Function(col) rows.All(Function(r) r.IsNull(col))).ToList()
        For Each col_to_remove As DataColumn In null_columns
            tbl_copy.Columns.Remove(col_to_remove)
        Next
        Return tbl_copy
    End Function

    Private Function RemoveEmptyRows(ByVal source As DataTable) As DataTable
        Dim dt1 As DataTable = source.Clone()
        For i As Integer = 0 To source.Rows.Count - 1
            Dim currentRow As DataRow = source.Rows(i)
            For Each colValue In currentRow.ItemArray
                If Not String.IsNullOrEmpty(colValue.ToString()) Then
                    dt1.ImportRow(currentRow)
                    Exit For
                End If
            Next
        Next
        Return dt1
    End Function

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
    Private Function GetDataTableFromExcel(ByVal filePath As String, ByVal sheetIndex As Integer)
        Dim wb As XSSFWorkbook
        Dim sh As XSSFSheet

        Using fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            wb = New XSSFWorkbook(fs)
        End Using
        Dim DT As DataTable = New DataTable()
        DT.Rows.Clear()
        DT.Columns.Clear()

        sh = CType(wb.GetSheetAt(sheetIndex), XSSFSheet)

        If sh.LastRowNum > 0 Then
            Dim headerRow = sh.GetRow(0)
            Dim colCount As Integer = headerRow.LastCellNum

            For c As Integer = 0 To colCount - 1
                If headerRow IsNot Nothing Then
                    If headerRow.GetCell(c) IsNot Nothing Then
                        DT.Columns.Add(headerRow.GetCell(c).ToString())
                    End If
                End If
            Next

            Dim i As Integer = 1
            Dim currentRow = sh.GetRow(i)
            While currentRow IsNot Nothing
                If DT.Columns.Count < sh.GetRow(i).Cells.Count Then
                    For j As Integer = 0 To sh.GetRow(i).Cells.Count - 1
                        DT.Columns.Add("", GetType(String))
                    Next
                End If

                Dim dr = DT.NewRow()
                Dim total_cells = sh.GetRow(0).Cells.Count

                For j As Integer = 0 To total_cells - 1
                    Dim cell As ICell = sh.GetRow(i).GetCell(j)
                    If cell IsNot Nothing Then
                        Select Case cell.CellType
                            Case CellType.Numeric
                                dr(j) = If(DateUtil.IsCellDateFormatted(cell), cell.DateCellValue.ToShortDateString(), cell.NumericCellValue.ToString(CultureInfo.InvariantCulture))
                            Case CellType.String
                                dr(j) = cell.StringCellValue
                            Case CellType.Blank
                                dr(j) = String.Empty
                            Case CellType.Formula
                                dr(j) = cell.NumericCellValue
                            Case CellType.Boolean
                                dr(j) = cell.BooleanCellValue
                            Case Else
                                dr(j) = cell.StringCellValue
                        End Select
                    End If
                Next
                DT.Rows.Add(dr)
                i = i + 1
                currentRow = sh.GetRow(i)
            End While
        End If
        Return DT

    End Function

    Private Sub cmb_hoja_edo_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub

    Private Sub btn_icaav_Click(sender As Object, e As EventArgs) Handles btn_icaav.Click
        Me.ExcelIcaavLoaded = False
        'btnCrearConciliacion.Enabled = False
        TabNavegacion.SelectedIndex = 1

        Try
            OpenFileDialog2.Filter = "ARCHIVOS DE EXCEL 2007-2013 (.xlsx)|*.xlsx"
            OpenFileDialog2.FilterIndex = 0
            OpenFileDialog2.FileName = ""
            OpenFileDialog2.Multiselect = False
            OpenFileDialog2.RestoreDirectory = True

            If OpenFileDialog2.ShowDialog() = DialogResult.OK Then
                If OpenFileDialog2.FileName IsNot String.Empty Then
                    txt_excel_icaav.Text = OpenFileDialog2.FileName
                    Dim sheetList As List(Of String) = GetSheetListFromExcel(OpenFileDialog2.FileName)
                    cmb_hoja_icaav.DataSource = sheetList
                    cmb_hoja_icaav.SelectedIndex = -1
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

        ''SeleccionaArchivo(txt_excel_icaav, cmb_hoja_icaav)
    End Sub

    Private Sub cmb_hoja_icaav_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb_hoja_icaav.SelectedIndexChanged
    End Sub



    Private Async Sub btn_procesar_Click(sender As Object, e As EventArgs) Handles btn_procesar.Click
        Await AplicarConciliacion(ListaGruposConciliaciones)
    End Sub

    Private Sub AplicarEstiloAGrid(ByRef grid As DataGridView)
        grid.ForeColor = Color.Black
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 8D, FontStyle.Bold)
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.BackgroundColor = Color.White
    End Sub

    Private Async Function AplicarConciliacion(ByVal ListaGrupos As BindingList(Of GrupoConciliaciones)) As Task(Of Boolean)
        Dim PrimeraConciliacion As Conciliacion = Nothing
        'nuevo agregado ismael 230718
        Dim valor_obtenido_ica_2 As IEnumerable(Of DataRow)
        Dim valor_obtenido_eca_2 As IEnumerable(Of DataRow)

        Dim ec_no_encontrados_en_icaav_2 = ""
        Dim ic_no_encontrados_en_ec_2 = ""


        For Each _grupoConciliacion In ListaGrupos
            If _grupoConciliacion.YaProcesado = False Then

                ec_no_encontrados_en_icaav_2 = ""
                ic_no_encontrados_en_ec_2 = ""

                Dim _grupoIndex = ListaGrupos.IndexOf(_grupoConciliacion)
                If _grupoIndex = 0 Then
                    dtEstadoDeCuenta = DirectCast(GridEdoCuenta.DataSource, DataTable)
                    dtIcaav = DirectCast(GridIcaav.DataSource, DataTable)
                End If

                dtEnumEC = dtEstadoDeCuenta.Rows.Cast(Of DataRow).ToList()
                dtEnumIC = dtIcaav.Rows.Cast(Of DataRow).ToList()

                If _grupoConciliacion.ListaConciliaciones.Count > 0 Then

                    'nuevo agrgado ismael 230718
                    bandera_c = bandera_c + 1

                    Dim dtAux = dtEstadoDeCuenta.Clone()
                    dtECNoEncontrados = dtEstadoDeCuenta.Clone()

                    Dim dtIcaavConcil = dtIcaav.Clone()
                    dtIcaavNoEncontrados = dtIcaav.Clone()


                    dtIcaavConcil.Clear()
                    dtAux.Clear()
                    dtIcaavNoEncontrados.Clear()
                    condiciones_ic_null_builder.Clear()
                    condiciones_ec_null_builder.Clear()
                    condiciones_ic_numeric_parse_builder.Clear()
                    condiciones_ec_numeric_parse_builder.Clear()
                    condiciones.Clear()


                    'nuevo agregado, para almacenar los dos reportes(cruce) ismael 190718 
                    If Not TabNavegacion.TabPages.ContainsKey("CONC." & _grupoConciliacion.Nombre) Then
                        ReportesTabs("CONC." & _grupoConciliacion.Nombre) = New TabPage()
                        GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre) = New DataGridView()
                        GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre).Name = "_CONC." & _grupoConciliacion.Nombre
                        ReportesTabs("CONC." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre))
                        GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                        ReportesTabs("CONC." & _grupoConciliacion.Nombre).Name = "CONC." & _grupoConciliacion.Nombre
                        ReportesTabs("CONC." & _grupoConciliacion.Nombre).Text = "CONC." & _grupoConciliacion.Nombre
                        AddHandler GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        AplicarEstiloAGrid(GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre))
                        TabNavegacion.TabPages.Add(ReportesTabs("CONC." & _grupoConciliacion.Nombre))

                    End If
                    'fin agrgado ismael 190718

                    'nuevo agrgado ismael 230718
                    If bandera_c = 1 Then

                        Dim dtAux2 = dtEstadoDeCuenta.Clone()
                        dtECNoEncontrados_2 = dtEstadoDeCuenta.Clone()
                        dtECNoEncontrados_2.Clear()

                        Dim dtIcaavConcil2 = dtIcaav.Clone()
                        dtIcaavNoEncontrados_2 = dtIcaav.Clone()
                        dtIcaavNoEncontrados_2.Clear()

                        If Not TabNavegacion.TabPages.ContainsKey("CONC.REPORT.BCD.") Then
                            ReportesTabs("CONC.REPORT.BCD.") = New TabPage()
                            GridsReportesTabs("_CONC.REPORT.BCD.") = New DataGridView()
                            GridsReportesTabs("_CONC.REPORT.BCD.").Name = "_CONC.REPORT.BCD."
                            ReportesTabs("CONC.REPORT.BCD.").Controls.Add(GridsReportesTabs("_CONC.REPORT.BCD."))
                            GridsReportesTabs("_CONC.REPORT.BCD.").Dock = DockStyle.Fill
                            'GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).Parent = Nothing
                            ReportesTabs("CONC.REPORT.BCD.").Name = "CONC.REPORT.BCD."
                            ReportesTabs("CONC.REPORT.BCD.").Text = "CONC.REPORT.BCD."
                            AddHandler GridsReportesTabs("_CONC.REPORT.BCD.").RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                            AplicarEstiloAGrid(GridsReportesTabs("_CONC.REPORT.BCD."))
                            'comntar para que el tab no aparezca
                            'TabNavegacion.TabPages.Add(ReportesTabs("CONC.REP.BCD." & _grupoConciliacion.Nombre))

                        End If

                        If Not TabNavegacion.TabPages.ContainsKey("CONC.REPORT.EDOCTA.") Then
                            ReportesTabs("CONC.REPORT.EDOCTA.") = New TabPage()
                            GridsReportesTabs("_CONC.REPORT.EDOCTA.") = New DataGridView()
                            GridsReportesTabs("_CONC.REPORT.EDOCTA.").Name = "_CONC.REPORT.EDOCTA."
                            ReportesTabs("CONC.REPORT.EDOCTA.").Controls.Add(GridsReportesTabs("_CONC.REPORT.EDOCTA."))
                            GridsReportesTabs("_CONC.REPORT.EDOCTA.").Dock = DockStyle.Fill
                            ReportesTabs("CONC.REPORT.EDOCTA.").Name = "CONC.REPORT.EDOCTA."
                            ReportesTabs("CONC.REPORT.EDOCTA.").Text = "CONC.REPORT.EDOCTA."
                            AddHandler GridsReportesTabs("_CONC.REPORT.EDOCTA.").RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                            AplicarEstiloAGrid(GridsReportesTabs("_CONC.REPORT.EDOCTA."))

                            'comentar para que el tab no aparezca
                            'TabNavegacion.TabPages.Add(ReportesTabs("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre))

                        End If

                        If Not TabNavegacion.TabPages.ContainsKey("NE.REPORT.BCD.") Then
                            ReportesTabs("NE.REPORT.BCD.") = New TabPage()
                            GridsReportesTabs("_NE.REPORT.BCD.") = New DataGridView()
                            GridsReportesTabs("_NE.REPORT.BCD.").Name = "_NE.REPORT.BCD."
                            ReportesTabs("NE.REPORT.BCD.").Controls.Add(GridsReportesTabs("_NE.REPORT.BCD."))
                            GridsReportesTabs("_NE.REPORT.BCD.").Dock = DockStyle.Fill
                            ReportesTabs("NE.REPORT.BCD.").Name = "NE.REPORT.BCD."
                            ReportesTabs("NE.REPORT.BCD.").Text = "NE.REPORT.BCD."
                            AddHandler GridsReportesTabs("_NE.REPORT.BCD.").RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                            AplicarEstiloAGrid(GridsReportesTabs("_NE.REPORT.BCD."))
                            TabNavegacion.TabPages.Add(ReportesTabs("NE.REPORT.BCD."))
                        End If

                        If Not TabNavegacion.TabPages.ContainsKey("NE.REPORT.EDOCTA.") Then
                            ReportesTabs("NE.REPORT.EDOCTA.") = New TabPage()
                            GridsReportesTabs("_NE.REPORT.EDOCTA.") = New DataGridView()
                            GridsReportesTabs("_NE.REPORT.EDOCTA.").Name = "_NE.REPORT.EDOCTA."
                            ReportesTabs("NE.REPORT.EDOCTA.").Controls.Add(GridsReportesTabs("_NE.REPORT.EDOCTA."))
                            GridsReportesTabs("_NE.REPORT.EDOCTA.").Dock = DockStyle.Fill
                            ReportesTabs("NE.REPORT.EDOCTA.").Name = "NE.REPORT.EDOCTA."
                            ReportesTabs("NE.REPORT.EDOCTA.").Text = "NE.REPORT.EDOCTA."
                            AddHandler GridsReportesTabs("_NE.REPORT.EDOCTA.").RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                            AplicarEstiloAGrid(GridsReportesTabs("_NE.REPORT.EDOCTA."))
                            TabNavegacion.TabPages.Add(ReportesTabs("NE.REPORT.EDOCTA."))
                        End If

                        'If Not TabNavegacion.TabPages.ContainsKey("CONFLICT.REPORT.BCD.") Then
                        '    ReportesTabs("CONFLICT.REPORT.BCD.") = New TabPage()
                        '    GridsReportesTabs("_CONFLICT.REPORT.BCD.") = New DataGridView()
                        '    GridsReportesTabs("_CONFLICT.REPORT.BCD.").Name = "_CONFLICT.REPORT.BCD."
                        '    ReportesTabs("CONFLICT.REPORT.BCD.").Controls.Add(GridsReportesTabs("_CONFLICT.REPORT.BCD."))
                        '    GridsReportesTabs("_CONFLICT.REPORT.BCD.").Dock = DockStyle.Fill
                        '    ReportesTabs("CONFLICT.REPORT.BCD.").Name = "CONFLICT.REPORT.BCD."
                        '    ReportesTabs("CONFLICT.REPORT.BCD.").Text = "CONFLICT.REPORT.BCD."
                        '    AddHandler GridsReportesTabs("_CONFLICT.REPORT.BCD.").RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        '    AplicarEstiloAGrid(GridsReportesTabs("_CONFLICT.REPORT.BCD."))
                        '    TabNavegacion.TabPages.Add(ReportesTabs("CONFLICT.REPORT.BCD."))
                        'End If

                        'If Not TabNavegacion.TabPages.ContainsKey("CONFLICT.REPORT.EDOCTA.") Then
                        '    ReportesTabs("CONFLICT.REPORT.EDOCTA.") = New TabPage()
                        '    GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.") = New DataGridView()
                        '    GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").Name = "_CONFLICT.REPORT.EDOCTA."
                        '    ReportesTabs("CONFLICT.REPORT.EDOCTA.").Controls.Add(GridsReportesTabs("_CONFLICT.REPORT.EDOCTA."))
                        '    GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").Dock = DockStyle.Fill
                        '    ReportesTabs("CONFLICT.REPORT.EDOCTA.").Name = "CONFLICT.REPORT.EDOCTA."
                        '    ReportesTabs("CONFLICT.REPORT.EDOCTA.").Text = "CONFLICT.REPORT.EDOCTA."
                        '    AddHandler GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        '    AplicarEstiloAGrid(GridsReportesTabs("_CONFLICT.REPORT.EDOCTA."))
                        '    TabNavegacion.TabPages.Add(ReportesTabs("CONFLICT.REPORT.EDOCTA."))
                        'End If

                    End If


                    If Not TabNavegacion.TabPages.ContainsKey("CONC.REP.BCD." & _grupoConciliacion.Nombre) Then
                        ReportesTabs("CONC.REP.BCD." & _grupoConciliacion.Nombre) = New TabPage()
                        GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre) = New DataGridView()
                        GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).Name = "_CONC.REP.BCD." & _grupoConciliacion.Nombre
                        ReportesTabs("CONC.REP.BCD." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre))
                        GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                        GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).Parent = Nothing
                        ReportesTabs("CONC.REP.BCD." & _grupoConciliacion.Nombre).Name = "CONC.REP.BCD." & _grupoConciliacion.Nombre
                        ReportesTabs("CONC.REP.BCD." & _grupoConciliacion.Nombre).Text = "CONC.REP.BCD." & _grupoConciliacion.Nombre
                        AddHandler GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        AplicarEstiloAGrid(GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre))
                        'comntar para que el tab no aparezca
                        'TabNavegacion.TabPages.Add(ReportesTabs("CONC.REP.BCD." & _grupoConciliacion.Nombre))'modificao ismael 230718

                    End If

                    If Not TabNavegacion.TabPages.ContainsKey("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre) Then
                        ReportesTabs("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre) = New TabPage()
                        GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre) = New DataGridView()
                        GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).Name = "_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre
                        ReportesTabs("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre))
                        GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                        ReportesTabs("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).Name = "CONC.REP.EDOCTA." & _grupoConciliacion.Nombre
                        ReportesTabs("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).Text = "CONC.REP.EDOCTA." & _grupoConciliacion.Nombre
                        AddHandler GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        AplicarEstiloAGrid(GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre))

                        'comentar para que el tab no aparezca
                        'TabNavegacion.TabPages.Add(ReportesTabs("CONC.REP.EDOCTA." & _grupoConciliacion.Nombre))'modificao ismael 230718

                    End If

                    If Not TabNavegacion.TabPages.ContainsKey("NE.REP.BCD." & _grupoConciliacion.Nombre) Then
                        ReportesTabs("NE.REP.BCD." & _grupoConciliacion.Nombre) = New TabPage()
                        GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre) = New DataGridView()
                        GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).Name = "_NE.REP.BCD." & _grupoConciliacion.Nombre
                        ReportesTabs("NE.REP.BCD." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre))
                        GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                        ReportesTabs("NE.REP.BCD." & _grupoConciliacion.Nombre).Name = "NE.REP.BCD." & _grupoConciliacion.Nombre
                        ReportesTabs("NE.REP.BCD." & _grupoConciliacion.Nombre).Text = "NE.REP.BCD." & _grupoConciliacion.Nombre
                        AddHandler GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        AplicarEstiloAGrid(GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre))
                        'TabNavegacion.TabPages.Add(ReportesTabs("NE.REP.BCD." & _grupoConciliacion.Nombre))'modificao ismael 230718
                    End If

                    If Not TabNavegacion.TabPages.ContainsKey("NE.REP.EDOCTA." & _grupoConciliacion.Nombre) Then
                        ReportesTabs("NE.REP.EDOCTA." & _grupoConciliacion.Nombre) = New TabPage()
                        GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre) = New DataGridView()
                        GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).Name = "_NE.REP.EDOCTA." & _grupoConciliacion.Nombre
                        ReportesTabs("NE.REP.EDOCTA." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre))
                        GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                        ReportesTabs("NE.REP.EDOCTA." & _grupoConciliacion.Nombre).Name = "NE.REP.EDOCTA." & _grupoConciliacion.Nombre
                        ReportesTabs("NE.REP.EDOCTA." & _grupoConciliacion.Nombre).Text = "NE.REP.EDOCTA." & _grupoConciliacion.Nombre
                        AddHandler GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                        AplicarEstiloAGrid(GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre))
                        'TabNavegacion.TabPages.Add(ReportesTabs("NE.REP.EDOCTA." & _grupoConciliacion.Nombre))'modificao ismael 230718
                    End If

                    'If Not TabNavegacion.TabPages.ContainsKey("CONFLICT.REP.BCD." & _grupoConciliacion.Nombre) Then
                    '    ReportesTabs("CONFLICT.REP.BCD." & _grupoConciliacion.Nombre) = New TabPage()
                    '    GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre) = New DataGridView()
                    '    GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).Name = "_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre
                    '    ReportesTabs("CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre))
                    '    GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                    '    ReportesTabs("CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).Name = "CONFLICT.REP.BCD." & _grupoConciliacion.Nombre
                    '    ReportesTabs("CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).Text = "CONFLICT.REP.BCD." & _grupoConciliacion.Nombre
                    '    AddHandler GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                    '    AplicarEstiloAGrid(GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre))
                    '    'TabNavegacion.TabPages.Add(ReportesTabs("CONFLICT.REP.BCD." & _grupoConciliacion.Nombre))'modificao ismael 230718
                    'End If

                    'If Not TabNavegacion.TabPages.ContainsKey("CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre) Then
                    '    ReportesTabs("CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre) = New TabPage()
                    '    GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre) = New DataGridView()
                    '    GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).Name = "_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre
                    '    ReportesTabs("CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).Controls.Add(GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre))
                    '    GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).Dock = DockStyle.Fill
                    '    ReportesTabs("CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).Name = "CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre
                    '    ReportesTabs("CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).Text = "CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre
                    '    AddHandler GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).RowPostPaint, AddressOf OnGridCreatedRowPostPaint
                    '    AplicarEstiloAGrid(GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre))
                    '    'TabNavegacion.TabPages.Add(ReportesTabs("CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre))'modificao ismael 230718
                    'End If




                    Dim contador = 0
                    For Each conciliacion In _grupoConciliacion.ListaConciliaciones
                        condiciones_ic_null_builder.Append(String.Format("if (ric[""{0}""].ToString() == """") return false;", conciliacion.CampoIcaav))
                        condiciones_ec_null_builder.Append(String.Format("if (rec[""{0}""].ToString() == """") return false;", conciliacion.CampoEstadoDeCuenta))

                        If conciliacion.TipoDeDatos = TiposDeDatos.NUMERICO Then
                            condiciones_ic_numeric_parse_builder.Append(String.Format("try {{var res_conv = Convert.ToDecimal(ric[""{0}""].ToString());}} catch (System.Exception ex) {{ return false;}}", conciliacion.CampoIcaav))
                            condiciones_ec_numeric_parse_builder.Append(String.Format("try {{var res_conv = Convert.ToDecimal(rec[""{0}""].ToString());}} catch (System.Exception ex) {{ return false;}}", conciliacion.CampoEstadoDeCuenta))
                        End If

                        If conciliacion.TipoDeDatos = TiposDeDatos.TEXTO And conciliacion.Operador = Operadores.TEXTO_CONTIENE Then
                            condiciones_ic_null_builder.Append(String.Format("if (ric[""{0}""].ToString().Length < 5) return false;", conciliacion.CampoIcaav))
                            condiciones_ec_null_builder.Append(String.Format("if (rec[""{0}""].ToString().Length < 5) return false;", conciliacion.CampoEstadoDeCuenta))
                        End If

                        If conciliacion.TipoDeDatos = TiposDeDatos.MONEDA Then
                            condiciones_ic_numeric_parse_builder.Append(String.Format("try {{var res_conv = decimal.Parse(ric[""{0}""].ToString(), NumberStyles.Currency);}} catch (System.Exception ex) {{ return false;}}", conciliacion.CampoIcaav))
                            condiciones_ec_numeric_parse_builder.Append(String.Format("try {{var res_conv = decimal.Parse(rec[""{0}""].ToString(), NumberStyles.Currency);}} catch (System.Exception ex) {{ return false;}}", conciliacion.CampoEstadoDeCuenta))
                        End If

                        If conciliacion.TipoDeDatos = TiposDeDatos.FECHA Then
                            condiciones_ic_numeric_parse_builder.Append(String.Format("try {{var res_conv = DateTime.Parse(ric[""{0}""].ToString());}} catch (System.Exception ex) {{ return false;}}", conciliacion.CampoIcaav))
                            condiciones_ec_numeric_parse_builder.Append(String.Format("try {{var res_conv = DateTime.Parse(rec[""{0}""].ToString());}} catch (System.Exception ex) {{ return false;}}", conciliacion.CampoEstadoDeCuenta))
                        End If

                        If ListaConciliaciones.Count = 1 Then
                            If conciliacion.Operador = Operadores.NUMERICO_IGUAL Then
                                condiciones.Append(String.Format("Convert.ToDecimal(rec[""{0}""].ToString()) == Convert.ToDecimal(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                            ElseIf conciliacion.Operador = Operadores.MONEDA_IGUAL Then
                                condiciones.Append(String.Format("decimal.Parse(rec[""{0}""].ToString(), NumberStyles.Currency) == decimal.Parse(ric[""{1}""].ToString(), NumberStyles.Currency)", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                            ElseIf conciliacion.Operador = Operadores.FECHA_IGUAL Then
                                condiciones.Append(String.Format("DateTime.Parse(rec[""{0}""].ToString()) == DateTime.Parse(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                            ElseIf conciliacion.Operador = Operadores.TEXTO_IGUAL Then
                                condiciones.Append(String.Format("rec[""{0}""].ToString().Equals(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                            ElseIf conciliacion.Operador = Operadores.TEXTO_CONTIENE Then
                                condiciones.Append(String.Format("(rec[""{0}""].ToString().Contains(ric[""{1}""].ToString()) || ric[""{1}""].ToString().Contains(rec[""{0}""].ToString()))", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                            End If
                        Else
                            If contador = 0 Then
                                If conciliacion.Operador = Operadores.NUMERICO_IGUAL Then
                                    condiciones.Append(String.Format("Convert.ToDecimal(rec[""{0}""].ToString()) == Convert.ToDecimal(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.MONEDA_IGUAL Then
                                    condiciones.Append(String.Format("decimal.Parse(rec[""{0}""].ToString(), NumberStyles.Currency) == decimal.Parse(ric[""{1}""].ToString(), NumberStyles.Currency)", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.FECHA_IGUAL Then
                                    condiciones.Append(String.Format("DateTime.Parse(rec[""{0}""].ToString()) == DateTime.Parse(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.TEXTO_IGUAL Then
                                    condiciones.Append(String.Format("rec[""{0}""].ToString().Equals(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.TEXTO_CONTIENE Then
                                    condiciones.Append(String.Format("(rec[""{0}""].ToString().Contains(ric[""{1}""].ToString()) || ric[""{1}""].ToString().Contains(rec[""{0}""].ToString()))", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                End If
                            Else

                                If conciliacion.Operador = Operadores.NUMERICO_IGUAL Then
                                    condiciones.Append(String.Format("&& Convert.ToDecimal(rec[""{0}""].ToString()) == Convert.ToDecimal(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.MONEDA_IGUAL Then
                                    condiciones.Append(String.Format("&& decimal.Parse(rec[""{0}""].ToString(), NumberStyles.Currency) == decimal.Parse(ric[""{1}""].ToString(), NumberStyles.Currency)", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.FECHA_IGUAL Then
                                    condiciones.Append(String.Format("&& DateTime.Parse(rec[""{0}""].ToString()) == DateTime.Parse(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.TEXTO_IGUAL Then
                                    condiciones.Append(String.Format("&& rec[""{0}""].ToString().Equals(ric[""{1}""].ToString())", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                ElseIf conciliacion.Operador = Operadores.TEXTO_CONTIENE Then
                                    condiciones.Append(String.Format("&& (rec[""{0}""].ToString().Contains(ric[""{1}""].ToString()) || ric[""{1}""].ToString().Contains(rec[""{0}""].ToString()))", conciliacion.CampoEstadoDeCuenta, conciliacion.CampoIcaav))
                                End If
                            End If
                            contador = contador + 1
                        End If
                    Next


                    Dim globalData = New GlobalData() With {.DatosEstadoDeCuenta = dtEnumEC, .DatosIcaav = dtEnumIC}
                    Dim query = String.Format("DatosEstadoDeCuenta.AsParallel().Where(rec => {{{0}{1} return DatosIcaav.Any(ric => {{{2}{3} return " + "{4}" + ";}});}}).ToList()", condiciones_ec_null_builder, condiciones_ec_numeric_parse_builder, condiciones_ic_null_builder, condiciones_ic_numeric_parse_builder, condiciones).ToString()
                    Dim query2 = String.Format("DatosIcaav.AsParallel().Where(ric => {{{2}{3} return DatosEstadoDeCuenta.Any(rec => {{{0}{1} return " + "{4}" + ";}});}}).ToList()", condiciones_ec_null_builder, condiciones_ec_numeric_parse_builder, condiciones_ic_null_builder, condiciones_ic_numeric_parse_builder, condiciones).ToString()
                    Dim options = ScriptOptions.Default.WithReferences({GetType(System.Linq.Enumerable).Assembly, GetType(System.Data.DataRowExtensions).Assembly, GetType(System.Convert).Assembly, GetType(NumberStyles).Assembly}).WithImports("System", "System.Linq", "System.Data.DataRowExtensions", "System.Globalization")
                    RaiseEvent ProcesoConciliacionIniciado(Me, EventArgs.Empty)

                    '/////////////////////////////////////////////INICIO NUEVO PROCESO OPTIMIZADO/////////////////////////////////////////
                    Dim resultado_script_eca = Await ProcesarConsultaAsync(query, options, New GlobalData() With {.DatosEstadoDeCuenta = dtEnumEC, .DatosIcaav = dtEnumIC})
                    Dim resultado_script_ica = Await ProcesarConsultaAsync(query2, options, New GlobalData() With {.DatosEstadoDeCuenta = dtEnumEC, .DatosIcaav = dtEnumIC})


                    Dim valor_obtenido_ica As IEnumerable(Of DataRow) = resultado_script_ica.ReturnValue
                    Dim valor_obtenido_eca As IEnumerable(Of DataRow) = resultado_script_eca.ReturnValue

                    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    Dim ec_no_encontrados_en_icaav = dtEnumEC.Except(valor_obtenido_eca)
                    Dim ic_no_encontrados_en_ec = dtEnumIC.Except(valor_obtenido_ica)

                    For Each row In valor_obtenido_eca
                        dtAux.ImportRow(row)
                    Next

                    For Each row In valor_obtenido_ica
                        dtIcaavConcil.ImportRow(row)
                    Next


                    dtECNoEncontrados_2.Clear()
                    For Each row In ec_no_encontrados_en_icaav
                        dtECNoEncontrados.ImportRow(row)

                        'nuevo agrgado ismael 230718
                        'dtECNoEncontrados_2.ImportRow(row)
                    Next

                    dtIcaavNoEncontrados_2.Clear()
                    For Each row In ic_no_encontrados_en_ec
                        dtIcaavNoEncontrados.ImportRow(row)

                        'nuevo agrgeado ismael 230718
                        'dtIcaavNoEncontrados_2.ImportRow(row)
                    Next


                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource = dtAux
                               End Sub)
                    Else
                        GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource = dtAux
                    End If

                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).DataSource = dtECNoEncontrados
                               End Sub)
                    Else
                        GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).DataSource = dtECNoEncontrados
                    End If

                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = dtIcaavConcil
                               End Sub)
                    Else
                        GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = dtIcaavConcil
                    End If

                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = dtIcaavNoEncontrados
                               End Sub)
                    Else
                        GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = dtIcaavNoEncontrados
                    End If

                    Dim evtProcesoTerminadoArgs = New ProcesoConciliacionTerminadoArgs()
                    evtProcesoTerminadoArgs.Grupo = _grupoConciliacion
                    RaiseEvent ProcesoConciliacionTerminado(Me, evtProcesoTerminadoArgs)

                    ' ORDENAMIENTO

                    Dim Conc1DataTable As DataTable = CType(GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim Con1DataTableOrdered As OrderedEnumerableRowCollection(Of DataRow) = Nothing
                    Dim Conc2DataTable As DataTable = CType(GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim Con2DataTableOrdered As OrderedEnumerableRowCollection(Of DataRow) = Nothing
                    Dim NE1DataTable As DataTable = CType(GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim NE1DataTableOrdered As OrderedEnumerableRowCollection(Of DataRow) = Nothing
                    Dim NE2DataTable As DataTable = CType(GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim NE2DataTableOrdered As OrderedEnumerableRowCollection(Of DataRow) = Nothing
                    Dim CONFLICT1DataTable As DataTable = Nothing 'CType(GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim CONFLICT1DataTableOrdered As OrderedEnumerableRowCollection(Of DataRow) = Nothing
                    Dim CONFLICT2DataTable As DataTable = Nothing 'CType(GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim CONFLICT2DataTableOrdered As OrderedEnumerableRowCollection(Of DataRow) = Nothing

                    For i = 0 To _grupoConciliacion.ListaConciliaciones.Count - 1
                        Dim columnIndex = i
                        If i = 0 Then
                            If _grupoConciliacion.ListaConciliaciones(i).TipoDeDatos = TiposDeDatos.NUMERICO Or _grupoConciliacion.ListaConciliaciones(i).TipoDeDatos = TiposDeDatos.MONEDA Then

                                'nuevo agrgado ismael 200718

                                If Conc1DataTable IsNot Nothing Then
                                    Con1DataTableOrdered = Conc1DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                     Dim campo As Decimal = 0
                                                                                                     Decimal.TryParse(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta), campo)
                                                                                                     Return campo
                                                                                                 End Function)
                                End If

                                If Conc2DataTable IsNot Nothing Then
                                    Con2DataTableOrdered = Conc2DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                     Dim campo As Decimal = 0
                                                                                                     Decimal.TryParse(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav), campo)
                                                                                                     Return campo
                                                                                                 End Function)
                                End If

                                If NE1DataTable IsNot Nothing Then
                                    NE1DataTableOrdered = NE1DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                  Dim campo As Decimal = 0
                                                                                                  Decimal.TryParse(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta), campo)
                                                                                                  Return campo
                                                                                              End Function)
                                End If

                                If NE2DataTable IsNot Nothing Then
                                    NE2DataTableOrdered = NE2DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                  Dim campo As Decimal = 0
                                                                                                  Decimal.TryParse(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav), campo)
                                                                                                  Return campo
                                                                                              End Function)
                                End If

                                If CONFLICT1DataTable IsNot Nothing Then
                                    CONFLICT1DataTableOrdered = CONFLICT1DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                              Dim campo As Decimal = 0
                                                                                                              Decimal.TryParse(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta), campo)
                                                                                                              Return campo
                                                                                                          End Function)
                                End If

                                If CONFLICT2DataTable IsNot Nothing Then
                                    CONFLICT2DataTableOrdered = CONFLICT2DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                              Dim campo As Decimal = 0
                                                                                                              Decimal.TryParse(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav), campo)
                                                                                                              Return campo
                                                                                                          End Function)
                                End If

                            ElseIf _grupoConciliacion.ListaConciliaciones(i).TipoDeDatos = TiposDeDatos.FECHA Then
                                If Conc1DataTable IsNot Nothing Then
                                    Con1DataTableOrdered = Conc1DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                     Dim _dateTime As DateTime = Nothing
                                                                                                     Try
                                                                                                         _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta, DateTime)
                                                                                                         Return _dateTime
                                                                                                     Catch ex As Exception
                                                                                                         Return _dateTime
                                                                                                     End Try
                                                                                                 End Function)
                                End If

                                If Conc2DataTable IsNot Nothing Then
                                    Con2DataTableOrdered = Conc2DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                     Dim _dateTime As DateTime = Nothing
                                                                                                     Try
                                                                                                         _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav, DateTime)
                                                                                                         Return _dateTime
                                                                                                     Catch ex As Exception
                                                                                                         Return _dateTime
                                                                                                     End Try
                                                                                                 End Function)
                                End If

                                If NE1DataTable IsNot Nothing Then
                                    NE1DataTableOrdered = NE1DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                  Dim _dateTime As DateTime = Nothing
                                                                                                  Try
                                                                                                      _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta, DateTime)
                                                                                                      Return _dateTime
                                                                                                  Catch ex As Exception
                                                                                                      Return _dateTime
                                                                                                  End Try
                                                                                              End Function)
                                End If

                                If NE2DataTable IsNot Nothing Then
                                    NE2DataTableOrdered = NE2DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                  Dim _dateTime As DateTime = Nothing
                                                                                                  Try
                                                                                                      _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav, DateTime)
                                                                                                      Return _dateTime
                                                                                                  Catch ex As Exception
                                                                                                      Return _dateTime
                                                                                                  End Try
                                                                                              End Function)
                                End If

                                If CONFLICT1DataTable IsNot Nothing Then
                                    CONFLICT1DataTableOrdered = CONFLICT1DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                              Dim _dateTime As DateTime = Nothing
                                                                                                              Try
                                                                                                                  _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta, DateTime)
                                                                                                                  Return _dateTime
                                                                                                              Catch ex As Exception
                                                                                                                  Return _dateTime
                                                                                                              End Try
                                                                                                          End Function)
                                End If

                                If CONFLICT2DataTable IsNot Nothing Then
                                    CONFLICT2DataTableOrdered = CONFLICT2DataTable.AsEnumerable().OrderBy(Function(value)
                                                                                                              Dim _dateTime As DateTime = Nothing
                                                                                                              Try
                                                                                                                  _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav, DateTime)
                                                                                                                  Return _dateTime
                                                                                                              Catch ex As Exception
                                                                                                                  Return _dateTime
                                                                                                              End Try
                                                                                                          End Function)
                                End If

                            Else
                                If Conc1DataTable IsNot Nothing Then
                                    Con1DataTableOrdered = Conc1DataTable.AsEnumerable().OrderBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta)))
                                End If

                                If Conc2DataTable IsNot Nothing Then
                                    Con2DataTableOrdered = Conc2DataTable.AsEnumerable().OrderBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav)))
                                End If

                                If NE1DataTable IsNot Nothing Then
                                    NE1DataTableOrdered = NE1DataTable.AsEnumerable().OrderBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta)))
                                End If

                                If NE2DataTable IsNot Nothing Then
                                    NE2DataTableOrdered = NE2DataTable.AsEnumerable().OrderBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav)))
                                End If

                                If CONFLICT1DataTable IsNot Nothing Then
                                    CONFLICT1DataTableOrdered = CONFLICT1DataTable.AsEnumerable().OrderBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta)))
                                End If

                                If CONFLICT2DataTable IsNot Nothing Then
                                    CONFLICT2DataTableOrdered = CONFLICT2DataTable.AsEnumerable().OrderBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav)))
                                End If
                            End If

                        Else
                            If _grupoConciliacion.ListaConciliaciones(i).TipoDeDatos = TiposDeDatos.NUMERICO Or _grupoConciliacion.ListaConciliaciones(i).TipoDeDatos = TiposDeDatos.MONEDA Then

                                If Con1DataTableOrdered IsNot Nothing Then
                                    Con1DataTableOrdered = Con1DataTableOrdered.ThenBy(Function(value)
                                                                                           Dim campo
                                                                                           Try
                                                                                               campo = Convert.ToDecimal(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta))
                                                                                           Catch ex As Exception
                                                                                               Return Nothing
                                                                                           End Try
                                                                                           Return campo
                                                                                       End Function)
                                End If

                                If Con2DataTableOrdered IsNot Nothing Then
                                    Con2DataTableOrdered = Con2DataTableOrdered.ThenBy(Function(value)
                                                                                           Dim campo
                                                                                           Try
                                                                                               campo = Convert.ToDecimal(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav))
                                                                                           Catch ex As Exception
                                                                                               Return Nothing
                                                                                           End Try
                                                                                           Return campo
                                                                                       End Function)
                                End If

                                If NE1DataTableOrdered IsNot Nothing Then
                                    NE1DataTableOrdered = NE1DataTableOrdered.ThenBy(Function(value)
                                                                                         Dim campo
                                                                                         Try
                                                                                             campo = Convert.ToDecimal(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta))
                                                                                         Catch ex As Exception
                                                                                             Return Nothing
                                                                                         End Try
                                                                                         Return campo
                                                                                     End Function)
                                End If

                                If NE2DataTableOrdered IsNot Nothing Then
                                    NE2DataTableOrdered = NE2DataTableOrdered.ThenBy(Function(value)
                                                                                         Dim campo
                                                                                         Try
                                                                                             campo = Convert.ToDecimal(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav))
                                                                                         Catch ex As Exception
                                                                                             Return Nothing
                                                                                         End Try
                                                                                         Return campo
                                                                                     End Function)
                                End If

                                If CONFLICT1DataTableOrdered IsNot Nothing Then
                                    CONFLICT1DataTableOrdered = CONFLICT1DataTableOrdered.ThenBy(Function(value)
                                                                                                     Dim campo
                                                                                                     Try
                                                                                                         campo = Convert.ToDecimal(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta))
                                                                                                     Catch ex As Exception
                                                                                                         Return Nothing
                                                                                                     End Try
                                                                                                     Return campo
                                                                                                 End Function)
                                End If

                                If CONFLICT2DataTableOrdered IsNot Nothing Then
                                    CONFLICT2DataTableOrdered = CONFLICT2DataTableOrdered.ThenBy(Function(value)
                                                                                                     Dim campo
                                                                                                     Try
                                                                                                         campo = Convert.ToDecimal(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav))
                                                                                                     Catch ex As Exception
                                                                                                         Return Nothing
                                                                                                     End Try
                                                                                                     Return campo
                                                                                                 End Function)
                                End If
                            ElseIf _grupoConciliacion.ListaConciliaciones(i).TipoDeDatos = TiposDeDatos.FECHA Then
                                If Con1DataTableOrdered IsNot Nothing Then
                                    Con1DataTableOrdered = Con1DataTableOrdered.ThenBy(Function(value)
                                                                                           Dim _dateTime As DateTime = Nothing
                                                                                           Try
                                                                                               _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta, DateTime)
                                                                                               Return _dateTime
                                                                                           Catch ex As Exception
                                                                                               Return _dateTime
                                                                                           End Try
                                                                                       End Function)
                                End If

                                If Con2DataTableOrdered IsNot Nothing Then
                                    Con2DataTableOrdered = Con2DataTableOrdered.ThenBy(Function(value)
                                                                                           Dim _dateTime As DateTime = Nothing
                                                                                           Try
                                                                                               _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav, DateTime)
                                                                                               Return _dateTime
                                                                                           Catch ex As Exception
                                                                                               Return _dateTime
                                                                                           End Try
                                                                                       End Function)
                                End If

                                If NE1DataTableOrdered IsNot Nothing Then
                                    NE1DataTableOrdered = NE1DataTableOrdered.ThenBy(Function(value)
                                                                                         Dim _dateTime As DateTime = Nothing
                                                                                         Try
                                                                                             _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta, DateTime)
                                                                                             Return _dateTime
                                                                                         Catch ex As Exception
                                                                                             Return _dateTime
                                                                                         End Try
                                                                                     End Function)
                                End If

                                If NE2DataTableOrdered IsNot Nothing Then
                                    NE2DataTableOrdered = NE2DataTableOrdered.ThenBy(Function(value)
                                                                                         Dim _dateTime As DateTime = Nothing
                                                                                         Try
                                                                                             _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav, DateTime)
                                                                                             Return _dateTime
                                                                                         Catch ex As Exception
                                                                                             Return _dateTime
                                                                                         End Try
                                                                                     End Function)
                                End If

                                If CONFLICT1DataTableOrdered IsNot Nothing Then
                                    CONFLICT1DataTableOrdered = CONFLICT1DataTableOrdered.ThenBy(Function(value)
                                                                                                     Dim _dateTime As DateTime = Nothing
                                                                                                     Try
                                                                                                         _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta, DateTime)
                                                                                                         Return _dateTime
                                                                                                     Catch ex As Exception
                                                                                                         Return _dateTime
                                                                                                     End Try
                                                                                                 End Function)
                                End If

                                If CONFLICT2DataTableOrdered IsNot Nothing Then
                                    CONFLICT2DataTableOrdered = CONFLICT2DataTableOrdered.ThenBy(Function(value)
                                                                                                     Dim _dateTime As DateTime = Nothing
                                                                                                     Try
                                                                                                         _dateTime = CType(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav, DateTime)
                                                                                                         Return _dateTime
                                                                                                     Catch ex As Exception
                                                                                                         Return _dateTime
                                                                                                     End Try
                                                                                                 End Function)
                                End If

                            Else
                                If Con1DataTableOrdered IsNot Nothing Then
                                    Con1DataTableOrdered = Con1DataTableOrdered.ThenBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta)))
                                End If

                                If Con2DataTableOrdered IsNot Nothing Then
                                    Con2DataTableOrdered = Con2DataTableOrdered.ThenBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav)))
                                End If

                                If NE1DataTableOrdered IsNot Nothing Then
                                    NE1DataTableOrdered = NE1DataTableOrdered.ThenBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta)))
                                End If

                                If NE2DataTableOrdered IsNot Nothing Then
                                    NE2DataTableOrdered = NE2DataTableOrdered.ThenBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav)))
                                End If

                                If CONFLICT1DataTableOrdered IsNot Nothing Then
                                    CONFLICT1DataTableOrdered = CONFLICT1DataTableOrdered.ThenBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoEstadoDeCuenta)))
                                End If

                                If CONFLICT2DataTableOrdered IsNot Nothing Then
                                    CONFLICT2DataTableOrdered = CONFLICT2DataTableOrdered.ThenBy(Function(value) Convert.ToString(value(_grupoConciliacion.ListaConciliaciones(columnIndex).CampoIcaav)))
                                End If
                            End If
                        End If
                    Next



                    If Con1DataTableOrdered IsNot Nothing Then
                        If Con1DataTableOrdered.Count > 0 Then
                            GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource = Con1DataTableOrdered.CopyToDataTable()
                        End If
                    End If

                    If Con2DataTableOrdered IsNot Nothing Then
                        If Con2DataTableOrdered.Count > 0 Then
                            GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = Con2DataTableOrdered.CopyToDataTable()
                        End If
                    End If

                    If NE1DataTableOrdered IsNot Nothing Then
                        If NE1DataTableOrdered.Count > 0 Then
                            GridsReportesTabs("_NE.REP.BCD." & _grupoConciliacion.Nombre).DataSource = NE1DataTableOrdered.CopyToDataTable()
                        End If
                    End If

                    If NE2DataTableOrdered IsNot Nothing Then
                        If NE2DataTableOrdered.Count > 0 Then
                            GridsReportesTabs("_NE.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = NE2DataTableOrdered.CopyToDataTable()
                        End If
                    End If

                    If CONFLICT1DataTableOrdered IsNot Nothing Then
                        If CONFLICT1DataTableOrdered.Count > 0 Then
                            'GridsReportesTabs("_CONFLICT.REP.BCD." & _grupoConciliacion.Nombre).DataSource = CONFLICT1DataTableOrdered.CopyToDataTable()
                        End If
                    End If

                    If CONFLICT2DataTableOrdered IsNot Nothing Then
                        If CONFLICT2DataTableOrdered.Count > 0 Then
                            'GridsReportesTabs("_CONFLICT.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource = CONFLICT2DataTableOrdered.CopyToDataTable()
                        End If
                    End If

                    'nuev agregado ismael 200718
                    '====nuevo codigo para armar solo una conciliacion ismael 200717

                    'Dim ColumnasECChecked_p As Integer
                    'Dim ColumnasICChecked_p As Integer

                    Dim dtConciliacionEC As DataTable = Nothing
                    Dim dtConciliacionIC As DataTable = Nothing

                    Dim filasYaProcesas As List(Of DataRow) = New List(Of DataRow)

                    dtConciliacionEC = CType(GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource, DataTable).Copy()
                    dtConciliacionIC = CType(GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource, DataTable).Copy()

                    Dim ECConciliacionNumRows = dtConciliacionEC.Rows.Count
                    Dim ICConciliacionNumRows = dtConciliacionIC.Rows.Count

                    Dim testListaConciliaciones As List(Of Conciliacion) = _grupoConciliacion.ListaConciliaciones
                    Dim CopiaDtConciliacionIC = dtConciliacionIC.Copy()
                    Dim DictRowsEncontradas As Dictionary(Of DataRow, DataRow) = New Dictionary(Of DataRow, DataRow)()
                    Dim rows_encontradas_hash_set = New HashSet(Of DataRow)(CopiaDtConciliacionIC.AsEnumerable)


                    'Await Task.Run(Sub() 

                    'For Each testECRow In dtConciliacionEC.Rows
                    Parallel.ForEach(dtConciliacionEC.Rows.Cast(Of DataRow)(), Sub(testECRow)
                                                                                   Dim rows_encontradas As List(Of DataRow) = Nothing
                                                                                   For t As Integer = 0 To testListaConciliaciones.Count - 1
                                                                                       Dim index_concil As Integer = t
                                                                                       If t = 0 Then

                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO NUMERICO
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.NUMERICO Then

                                                                                               rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)

                                                                                                                                                      Try
                                                                                                                                                          Dim numero1 As Decimal = 0
                                                                                                                                                          Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                          Dim numero2 As Decimal = 0
                                                                                                                                                          Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                                          Decimal.TryParse(num1cadena, numero1)
                                                                                                                                                          Decimal.TryParse(num2cadena, numero2)

                                                                                                                                                          Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                      Catch ex As Exception
                                                                                                                                                          Return False
                                                                                                                                                      End Try
                                                                                                                                                  End Function).ToList()
                                                                                               Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                               If rowSeleccionada IsNot Nothing Then
                                                                                                   If t = testListaConciliaciones.Count - 1 Then
                                                                                                       SyncLock DictRowsEncontradas
                                                                                                           DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                       End SyncLock
                                                                                                       SyncLock DictRowsEncontradas
                                                                                                           filasYaProcesas.Add(rowSeleccionada)
                                                                                                       End SyncLock
                                                                                                   End If
                                                                                               End If
                                                                                           End If

                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO MONEDA
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.MONEDA Then

                                                                                               rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)

                                                                                                                                                      Try
                                                                                                                                                          Dim numero1 As Decimal = Nothing
                                                                                                                                                          Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                          Dim numero2 As Decimal = Nothing
                                                                                                                                                          Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                                          numero1 = Decimal.Parse(num1cadena, NumberStyles.Currency)
                                                                                                                                                          numero2 = Decimal.Parse(num2cadena, NumberStyles.Currency)
                                                                                                                                                          Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                      Catch ex As Exception
                                                                                                                                                          Return False
                                                                                                                                                      End Try
                                                                                                                                                  End Function).ToList()
                                                                                               Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                               If rowSeleccionada IsNot Nothing Then
                                                                                                   If t = testListaConciliaciones.Count - 1 Then
                                                                                                       SyncLock DictRowsEncontradas
                                                                                                           DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                       End SyncLock
                                                                                                       SyncLock DictRowsEncontradas
                                                                                                           filasYaProcesas.Add(rowSeleccionada)
                                                                                                       End SyncLock
                                                                                                   End If
                                                                                               End If
                                                                                           End If


                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO FECHA
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.FECHA Then

                                                                                               rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)

                                                                                                                                                      Try
                                                                                                                                                          Dim fecha1 As DateTime = Nothing
                                                                                                                                                          Dim fecha1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                          Dim fecha2 As DateTime = Nothing
                                                                                                                                                          Dim fecha2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                                          fecha1 = DateTime.Parse(fecha1cadena)
                                                                                                                                                          fecha2 = DateTime.Parse(fecha2cadena)
                                                                                                                                                          Return (fecha1 = fecha2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                      Catch ex As Exception
                                                                                                                                                          Return False
                                                                                                                                                      End Try
                                                                                                                                                  End Function).ToList()
                                                                                               Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                               If rowSeleccionada IsNot Nothing Then
                                                                                                   If t = testListaConciliaciones.Count - 1 Then
                                                                                                       SyncLock DictRowsEncontradas
                                                                                                           DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                       End SyncLock
                                                                                                       SyncLock DictRowsEncontradas
                                                                                                           filasYaProcesas.Add(rowSeleccionada)
                                                                                                       End SyncLock
                                                                                                   End If
                                                                                               End If
                                                                                           End If


                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO TEXTO
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.TEXTO Then
                                                                                               If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_IGUAL Then

                                                                                                   rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)


                                                                                                                                                          Try
                                                                                                                                                              Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                              Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                                              Return (cadena1.Equals(cadena2)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                          Catch ex As Exception
                                                                                                                                                              Return False
                                                                                                                                                          End Try
                                                                                                                                                      End Function).ToList()
                                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                       End If
                                                                                                   End If
                                                                                               End If


                                                                                               ' // PARA TEXTO CONTAIN
                                                                                               If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_CONTIENE Then

                                                                                                   rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)
                                                                                                                                                          Try
                                                                                                                                                              Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                              Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                                              Return (cadena1.Contains(cadena2) Or cadena2.Contains(cadena1)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                          Catch ex As Exception
                                                                                                                                                              Return False
                                                                                                                                                          End Try
                                                                                                                                                      End Function).ToList()
                                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                       End If
                                                                                                   End If
                                                                                               End If
                                                                                           End If

                                                                                           '##
                                                                                       Else

                                                                                           '///////////////////////////////////////////////////////////
                                                                                           ' // SI TIENE MAS DE UNA CONDICION LAS APLICARA

                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO NUMERICO
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.NUMERICO Then
                                                                                               If rows_encontradas IsNot Nothing Then
                                                                                                   rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                                                                                                 Try
                                                                                                                                                     Dim numero1 As Decimal = Nothing
                                                                                                                                                     Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                     Dim numero2 As Decimal = Nothing
                                                                                                                                                     Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                                     numero1 = Convert.ToDecimal(num1cadena)
                                                                                                                                                     numero2 = Convert.ToDecimal(num2cadena)
                                                                                                                                                     Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                 Catch ex As Exception
                                                                                                                                                     Return False
                                                                                                                                                 End Try
                                                                                                                                             End Function).ToList()
                                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                       End If
                                                                                                   End If
                                                                                               End If
                                                                                           End If

                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO MONEDA
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.MONEDA Then
                                                                                               If rows_encontradas IsNot Nothing Then
                                                                                                   rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                                                                                                 Try
                                                                                                                                                     Dim numero1 As Decimal = Nothing
                                                                                                                                                     Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                     Dim numero2 As Decimal = Nothing
                                                                                                                                                     Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                                     numero1 = Decimal.Parse(num1cadena, NumberStyles.Currency)
                                                                                                                                                     numero2 = Decimal.Parse(num2cadena, NumberStyles.Currency)
                                                                                                                                                     Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                 Catch ex As Exception
                                                                                                                                                     Return False
                                                                                                                                                 End Try
                                                                                                                                             End Function).ToList()
                                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                       End If
                                                                                                   End If
                                                                                               End If
                                                                                           End If


                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO FECHA
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.FECHA Then
                                                                                               If rows_encontradas IsNot Nothing Then
                                                                                                   rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                                                                                                 Try
                                                                                                                                                     Dim fecha1 As DateTime = Nothing
                                                                                                                                                     Dim fecha1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                     Dim fecha2 As DateTime = Nothing
                                                                                                                                                     Dim fecha2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                                     fecha1 = DateTime.Parse(fecha1cadena)
                                                                                                                                                     fecha2 = DateTime.Parse(fecha2cadena)
                                                                                                                                                     Return (fecha1 = fecha2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                 Catch ex As Exception
                                                                                                                                                     Return False
                                                                                                                                                 End Try
                                                                                                                                             End Function).ToList()
                                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                           SyncLock DictRowsEncontradas
                                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                                           End SyncLock
                                                                                                       End If
                                                                                                   End If
                                                                                               End If
                                                                                           End If


                                                                                           ' SI LA PRIMERA CONCILIACION ES DE TIPO TEXTO
                                                                                           If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.TEXTO Then
                                                                                               If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_IGUAL Then
                                                                                                   If rows_encontradas IsNot Nothing Then
                                                                                                       rows_encontradas = rows_encontradas.Where(Function(trow)


                                                                                                                                                     Try
                                                                                                                                                         Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                         Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                                         Return (cadena1.Equals(cadena2)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                     Catch ex As Exception
                                                                                                                                                         Return False
                                                                                                                                                     End Try
                                                                                                                                                 End Function).ToList()
                                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                                               SyncLock DictRowsEncontradas
                                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                               End SyncLock
                                                                                                               SyncLock DictRowsEncontradas
                                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                                               End SyncLock
                                                                                                           End If
                                                                                                       End If
                                                                                                   End If
                                                                                               End If


                                                                                               ' // PARA TEXTO CONTAIN
                                                                                               If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_CONTIENE Then
                                                                                                   If rows_encontradas IsNot Nothing Then
                                                                                                       rows_encontradas = rows_encontradas.Where(Function(trow)


                                                                                                                                                     Try
                                                                                                                                                         Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                         Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                                         Return (cadena1.Contains(cadena2) Or cadena2.Contains(cadena1)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                                     Catch ex As Exception
                                                                                                                                                         Return False
                                                                                                                                                     End Try
                                                                                                                                                 End Function).ToList()
                                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                                               SyncLock DictRowsEncontradas
                                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                               End SyncLock
                                                                                                               SyncLock DictRowsEncontradas
                                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                                               End SyncLock
                                                                                                           End If
                                                                                                       End If
                                                                                                   End If
                                                                                               End If
                                                                                           End If

                                                                                           ' /////////////////////////////////////////////


                                                                                       End If
                                                                                   Next
                                                                                   'Next
                                                                               End Sub)


                    Dim dtECReport As DataTable = dtConciliacionEC.Clone()
                    dtECReport.Clear()

                    Dim dtICReport As DataTable = dtConciliacionIC.Clone()

                    For Each column As DataColumn In dtICReport.Columns
                        If dtECReport.Columns.Contains(column.ColumnName) Then
                            dtECReport.Columns.Add(column.ColumnName & "_")
                        Else
                            dtECReport.Columns.Add(column.ColumnName)
                        End If
                    Next

                    dtECReport.AcceptChanges()

                    Dim RowsReporteBCDOriginal As List(Of DataRow) = dtEnumEC.Cast(Of DataRow).ToList()
                    Dim RowsPrimerReporte = DictRowsEncontradas.Keys.ToList()

                    Dim RowsECConciliadas = dtConciliacionEC.Rows.Cast(Of DataRow).ToList()

                    Dim comparer As IEqualityComparer(Of DataRow) = New RowComparer()

                    'Dim RowsReporteBCDOriginalHS As HashSet(Of DataRow) = New HashSet(Of DataRow)(RowsReporteBCDOriginal)
                    'Dim RowsPrimerReporteHS As HashSet(Of DataRow) = New HashSet(Of DataRow)(RowsPrimerReporte)

                    Dim EnReporteBCDNoEnConciliacion = RowsReporteBCDOriginal.IntersectAll(RowsPrimerReporte, comparer).ToList()

                    Dim RowsPendientesBCD = RowsReporteBCDOriginal.ExceptAll(EnReporteBCDNoEnConciliacion, comparer)
                    'RowsPrimerReporte.AddRange(RowsECConciliadas)
                    'Dim bEqual As Boolean = comparer.Equals(RowsPrimerReporte.FirstOrDefault(), RowsPrimerReporte.LastOrDefault())
                    'Dim RowsDistintasPrimerReporte = RowsECConciliadas.Except(RowsPrimerReporte, comparer).ToList()


                    For Each row In RowsPendientesBCD
                        dtECNoEncontrados_2.ImportRow(row)
                    Next

                    Dim RowsReporteEDOCTAOriginal As List(Of DataRow) = dtEnumIC.Cast(Of DataRow).ToList()
                    Dim RowsSegundoReporte = DictRowsEncontradas.Values.ToList()
                    Dim RowsICConciliadas = dtConciliacionIC.Rows.Cast(Of DataRow).ToList()

                    Dim comparer2 = New RowComparer()
                    Dim EnReporteEDOCTANoEnConciliacion = RowsReporteEDOCTAOriginal.IntersectAll(RowsSegundoReporte, comparer2).ToList()

                    Dim RowsPendientesEDOCTA = RowsReporteEDOCTAOriginal.ExceptAll(EnReporteEDOCTANoEnConciliacion, comparer2)

                    For Each row In RowsPendientesEDOCTA
                        dtIcaavNoEncontrados_2.ImportRow(row)
                    Next

                    dtIcaavNoEncontrados_2.AcceptChanges()
                    'Parallel.ForEach(DictRowsEncontradas, Sub(parejaFilasConciliadas As KeyValuePair(Of DataRow, DataRow))
                    For Each parejaFilasConciliadas As KeyValuePair(Of DataRow, DataRow) In DictRowsEncontradas
                        Dim FilaPrimerReporte = CType(parejaFilasConciliadas.Key, DataRow)
                        Dim FilaSegundoReporte = CType(parejaFilasConciliadas.Value, DataRow)

                        Dim filaReporteCombinado As DataRow = dtECReport.NewRow()

                        For g As Integer = 0 To dtECReport.Columns.Count - 1
                            If g < dtConciliacionEC.Columns.Count Then
                                filaReporteCombinado(g) = FilaPrimerReporte(g)
                            Else
                                filaReporteCombinado(g) = FilaSegundoReporte(g - dtConciliacionEC.Columns.Count)

                            End If
                        Next
                        'SyncLock dtECReport
                        dtECReport.Rows.Add(filaReporteCombinado)
                        dtECReport.AcceptChanges()
                        'End SyncLock
                    Next

                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre).DataSource = dtECReport
                               End Sub)
                    Else
                        GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre).DataSource = dtECReport

                    End If

                    ' ORDENAMIENTO
                    'nuevo agrgado ismael 200718
                    Dim Conc1DataTable_1 As DataTable = CType(GridsReportesTabs("_CONC." & _grupoConciliacion.Nombre).DataSource, DataTable)
                    Dim Con1DataTableOrdered_1 As OrderedEnumerableRowCollection(Of DataRow) = Nothing
                    'fin nuevo agregado 

                    '====fin codigo para armar una sola conciliacion  2200718

                End If
                _grupoConciliacion.YaProcesado = True
            End If
        Next

        If bandera_c > 0 Then

            If InvokeRequired Then
                Invoke(Sub()
                           GridsReportesTabs("_NE.REPORT.BCD.").DataSource = Nothing
                       End Sub)
            Else
                GridsReportesTabs("_NE.REPORT.BCD.").DataSource = Nothing
            End If

            If InvokeRequired Then
                Invoke(Sub()
                           GridsReportesTabs("_NE.REPORT.EDOCTA.").DataSource = Nothing
                       End Sub)
            Else
                GridsReportesTabs("_NE.REPORT.EDOCTA.").DataSource = Nothing
            End If



            If Me.InvokeRequired Then
                Invoke(Sub()
                           GridsReportesTabs("_NE.REPORT.BCD.").DataSource = dtECNoEncontrados_2
                           GridsReportesTabs("_NE.REPORT.EDOCTA.").DataSource = dtIcaavNoEncontrados_2
                       End Sub)
            Else
                GridsReportesTabs("_NE.REPORT.BCD.").DataSource = dtECNoEncontrados_2
                GridsReportesTabs("_NE.REPORT.EDOCTA.").DataSource = dtIcaavNoEncontrados_2
            End If

            If Me.InvokeRequired Then
                Invoke(Sub()
                           GridsReportesTabs("_NE.REPORT.EDOCTA.").DataSource = dtIcaavNoEncontrados_2
                       End Sub)
            Else
                GridsReportesTabs("_NE.REPORT.EDOCTA.").DataSource = dtIcaavNoEncontrados_2
            End If

            dtEstadoDeCuenta = dtECNoEncontrados_2.Copy
            dtIcaav = dtIcaavNoEncontrados_2.Copy

            'bandera_c = 0
        End If


        Return True
    End Function

    Public Sub ProcesoConciliacionParalelo(ByRef dtConciliacionEC As DataTable, ByRef dtConciliacionIC As DataTable, ByVal testListaConciliaciones As List(Of Conciliacion), ByRef rows_encontradas_hash_set_p As HashSet(Of DataRow), ByRef filasYaProcesas_p As List(Of DataRow), ByRef DictRowsEncontradas_p As Dictionary(Of DataRow, DataRow))
        Dim rows_encontradas_hash_set = rows_encontradas_hash_set_p
        Dim filasYaProcesas = filasYaProcesas_p
        Dim DictRowsEncontradas = DictRowsEncontradas_p

        Parallel.ForEach(dtConciliacionEC.Rows.Cast(Of DataRow)(), Sub(testECRow)
                                                                       Dim rows_encontradas As List(Of DataRow) = Nothing
                                                                       For t As Integer = 0 To testListaConciliaciones.Count - 1
                                                                           Dim index_concil As Integer = t
                                                                           If t = 0 Then

                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO NUMERICO
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.NUMERICO Then

                                                                                   rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)

                                                                                                                                          Try
                                                                                                                                              Dim numero1 As Decimal = 0
                                                                                                                                              Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                              Dim numero2 As Decimal = 0
                                                                                                                                              Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                              Decimal.TryParse(num1cadena, numero1)
                                                                                                                                              Decimal.TryParse(num2cadena, numero2)

                                                                                                                                              Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                          Catch ex As Exception
                                                                                                                                              Return False
                                                                                                                                          End Try
                                                                                                                                      End Function).ToList()
                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                           SyncLock DictRowsEncontradas
                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                           End SyncLock
                                                                                           SyncLock DictRowsEncontradas
                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                           End SyncLock
                                                                                       End If
                                                                                   End If
                                                                               End If

                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO MONEDA
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.MONEDA Then

                                                                                   rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)

                                                                                                                                          Try
                                                                                                                                              Dim numero1 As Decimal = Nothing
                                                                                                                                              Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                              Dim numero2 As Decimal = Nothing
                                                                                                                                              Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                              numero1 = Decimal.Parse(num1cadena, NumberStyles.Currency)
                                                                                                                                              numero2 = Decimal.Parse(num2cadena, NumberStyles.Currency)
                                                                                                                                              Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                          Catch ex As Exception
                                                                                                                                              Return False
                                                                                                                                          End Try
                                                                                                                                      End Function).ToList()
                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                           SyncLock DictRowsEncontradas
                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                           End SyncLock
                                                                                           SyncLock DictRowsEncontradas
                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                           End SyncLock
                                                                                       End If
                                                                                   End If
                                                                               End If


                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO FECHA
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.FECHA Then

                                                                                   rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)

                                                                                                                                          Try
                                                                                                                                              Dim fecha1 As DateTime = Nothing
                                                                                                                                              Dim fecha1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                              Dim fecha2 As DateTime = Nothing
                                                                                                                                              Dim fecha2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                              fecha1 = DateTime.Parse(fecha1cadena)
                                                                                                                                              fecha2 = DateTime.Parse(fecha2cadena)
                                                                                                                                              Return (fecha1 = fecha2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                          Catch ex As Exception
                                                                                                                                              Return False
                                                                                                                                          End Try
                                                                                                                                      End Function).ToList()
                                                                                   Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                   If rowSeleccionada IsNot Nothing Then
                                                                                       If t = testListaConciliaciones.Count - 1 Then
                                                                                           SyncLock DictRowsEncontradas
                                                                                               DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                           End SyncLock
                                                                                           SyncLock DictRowsEncontradas
                                                                                               filasYaProcesas.Add(rowSeleccionada)
                                                                                           End SyncLock
                                                                                       End If
                                                                                   End If
                                                                               End If


                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO TEXTO
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.TEXTO Then
                                                                                   If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_IGUAL Then

                                                                                       rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)


                                                                                                                                              Try
                                                                                                                                                  Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                  Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                                  Return (cadena1.Equals(cadena2)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                              Catch ex As Exception
                                                                                                                                                  Return False
                                                                                                                                              End Try
                                                                                                                                          End Function).ToList()
                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                               End SyncLock
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                               End SyncLock
                                                                                           End If
                                                                                       End If
                                                                                   End If


                                                                                   ' // PARA TEXTO CONTAIN
                                                                                   If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_CONTIENE Then

                                                                                       rows_encontradas = rows_encontradas_hash_set.Where(Function(trow)
                                                                                                                                              Try
                                                                                                                                                  Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                                  Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                                  Return (cadena1.Contains(cadena2) Or cadena2.Contains(cadena1)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                              Catch ex As Exception
                                                                                                                                                  Return False
                                                                                                                                              End Try
                                                                                                                                          End Function).ToList()
                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                               End SyncLock
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                               End SyncLock
                                                                                           End If
                                                                                       End If
                                                                                   End If
                                                                               End If

                                                                               '##
                                                                           Else

                                                                               '///////////////////////////////////////////////////////////
                                                                               ' // SI TIENE MAS DE UNA CONDICION LAS APLICARA

                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO NUMERICO
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.NUMERICO Then
                                                                                   If rows_encontradas IsNot Nothing Then
                                                                                       rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                                                                                     Try
                                                                                                                                         Dim numero1 As Decimal = Nothing
                                                                                                                                         Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                         Dim numero2 As Decimal = Nothing
                                                                                                                                         Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                         numero1 = Convert.ToDecimal(num1cadena)
                                                                                                                                         numero2 = Convert.ToDecimal(num2cadena)
                                                                                                                                         Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                     Catch ex As Exception
                                                                                                                                         Return False
                                                                                                                                     End Try
                                                                                                                                 End Function).ToList()
                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                               End SyncLock
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                               End SyncLock
                                                                                           End If
                                                                                       End If
                                                                                   End If
                                                                               End If

                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO MONEDA
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.MONEDA Then
                                                                                   If rows_encontradas IsNot Nothing Then
                                                                                       rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                                                                                     Try
                                                                                                                                         Dim numero1 As Decimal = Nothing
                                                                                                                                         Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                         Dim numero2 As Decimal = Nothing
                                                                                                                                         Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                         numero1 = Decimal.Parse(num1cadena, NumberStyles.Currency)
                                                                                                                                         numero2 = Decimal.Parse(num2cadena, NumberStyles.Currency)
                                                                                                                                         Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                     Catch ex As Exception
                                                                                                                                         Return False
                                                                                                                                     End Try
                                                                                                                                 End Function).ToList()
                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                               End SyncLock
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                               End SyncLock
                                                                                           End If
                                                                                       End If
                                                                                   End If
                                                                               End If


                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO FECHA
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.FECHA Then
                                                                                   If rows_encontradas IsNot Nothing Then
                                                                                       rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                                                                                     Try
                                                                                                                                         Dim fecha1 As DateTime = Nothing
                                                                                                                                         Dim fecha1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                         Dim fecha2 As DateTime = Nothing
                                                                                                                                         Dim fecha2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                                                         fecha1 = DateTime.Parse(fecha1cadena)
                                                                                                                                         fecha2 = DateTime.Parse(fecha2cadena)
                                                                                                                                         Return (fecha1 = fecha2) And Not filasYaProcesas.Contains(trow)
                                                                                                                                     Catch ex As Exception
                                                                                                                                         Return False
                                                                                                                                     End Try
                                                                                                                                 End Function).ToList()
                                                                                       Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                       If rowSeleccionada IsNot Nothing Then
                                                                                           If t = testListaConciliaciones.Count - 1 Then
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                               End SyncLock
                                                                                               SyncLock DictRowsEncontradas
                                                                                                   filasYaProcesas.Add(rowSeleccionada)
                                                                                               End SyncLock
                                                                                           End If
                                                                                       End If
                                                                                   End If
                                                                               End If


                                                                               ' SI LA PRIMERA CONCILIACION ES DE TIPO TEXTO
                                                                               If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.TEXTO Then
                                                                                   If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_IGUAL Then
                                                                                       If rows_encontradas IsNot Nothing Then
                                                                                           rows_encontradas = rows_encontradas.Where(Function(trow)


                                                                                                                                         Try
                                                                                                                                             Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                             Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                             Return (cadena1.Equals(cadena2)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                         Catch ex As Exception
                                                                                                                                             Return False
                                                                                                                                         End Try
                                                                                                                                     End Function).ToList()
                                                                                           Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                           If rowSeleccionada IsNot Nothing Then
                                                                                               If t = testListaConciliaciones.Count - 1 Then
                                                                                                   SyncLock DictRowsEncontradas
                                                                                                       DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                   End SyncLock
                                                                                                   SyncLock DictRowsEncontradas
                                                                                                       filasYaProcesas.Add(rowSeleccionada)
                                                                                                   End SyncLock
                                                                                               End If
                                                                                           End If
                                                                                       End If
                                                                                   End If


                                                                                   ' // PARA TEXTO CONTAIN
                                                                                   If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_CONTIENE Then
                                                                                       If rows_encontradas IsNot Nothing Then
                                                                                           rows_encontradas = rows_encontradas.Where(Function(trow)


                                                                                                                                         Try
                                                                                                                                             Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                                                             Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                                                             Return (cadena1.Contains(cadena2) Or cadena2.Contains(cadena1)) And Not filasYaProcesas.Contains(trow)
                                                                                                                                         Catch ex As Exception
                                                                                                                                             Return False
                                                                                                                                         End Try
                                                                                                                                     End Function).ToList()
                                                                                           Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                                                                           If rowSeleccionada IsNot Nothing Then
                                                                                               If t = testListaConciliaciones.Count - 1 Then
                                                                                                   SyncLock DictRowsEncontradas
                                                                                                       DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                                                                                   End SyncLock
                                                                                                   SyncLock DictRowsEncontradas
                                                                                                       filasYaProcesas.Add(rowSeleccionada)
                                                                                                   End SyncLock
                                                                                               End If
                                                                                           End If
                                                                                       End If
                                                                                   End If
                                                                               End If

                                                                               ' /////////////////////////////////////////////


                                                                           End If
                                                                       Next
                                                                       'Next
                                                                   End Sub)
    End Sub

    Private Sub OnGridCreatedRowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Async Function ProcesarConsultaAsync(ByVal query As String, ByVal options As ScriptOptions, ByVal globalData As GlobalData) As Task(Of ScriptState(Of IEnumerable))
        Return Await Task.Run(Async Function()
                                  Dim script_proceso = CSharpScript.Create(Of IEnumerable)(
                    query,
                    options,
                    GetType(GlobalData))
                                  script_proceso.Compile()
                                  Return Await script_proceso.RunAsync(globalData)
                              End Function)
    End Function

    Private Sub DrawRowNumbers(ByRef sender As Object, ByRef e As DataGridViewRowPostPaintEventArgs)
        Dim grid As DataGridView = sender
        Dim rowIdx = (e.RowIndex + 1).ToString()
        Dim centerFormat = New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
        Dim headerBounds = New Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
        e.Graphics.DrawString(rowIdx, Me.Font, SystemBrushes.ControlText, headerBounds, centerFormat)
    End Sub

    Public Sub cuerpoConciliacion()
        TabNavegacion.SelectedIndex = 3
        coincidencia.Clear()

        Dim encontro As Integer = 0
        Dim Int As Integer = 0
        '===========================================================
        For Each valorEdo In arrayEncontroEdo
            My.Application.DoEvents()
            For Each valorIcaav In arrayEncontroIcaav
                My.Application.DoEvents()
                If (valorIcaav = valorEdo) Then
                    coincidencia.Add(valorEdo)
                End If
            Next

        Next
        '===========================================================
        Dim NColGridEdoCuenta As Integer = GridEdoCuenta.ColumnCount
        Dim NRowGridEdoCuenta As Integer = GridEdoCuenta.RowCount
        For Fila As Integer = 0 To NRowGridEdoCuenta - 1
            My.Application.DoEvents()
            If (Fila <> 0) Then
                'GridConciliacion.Rows.Add(1)
            End If
        Next
        '===========================================================
        Dim NColGridIcaav As Integer = GridIcaav.ColumnCount
        Dim NRowGridIcaav As Integer = GridIcaav.RowCount
        For Fila As Integer = 0 To NRowGridEdoCuenta - 1
            My.Application.DoEvents()
            If (Fila <> 0) Then
                cuerpoConciliacionIcaav.Rows.Add(1)
            End If
        Next

        '===========================================================
        Dim numFila As Integer = 0
        For Fila As Integer = 0 To NRowGridEdoCuenta - 1
            encontro = 0
            My.Application.DoEvents()
            For Col As Integer = 0 To NColGridEdoCuenta - 1
                My.Application.DoEvents()
                For Each valorees In selectEdo

                    My.Application.DoEvents()
                    If (String.IsNullOrEmpty(Trim(IIf(GridEdoCuenta.Rows(Fila).Cells(valorees).Value Is DBNull.Value, "", GridEdoCuenta.Rows(Fila).Cells(valorees).Value)))) Then
                        Int = 0
                    Else
                        Int = GridEdoCuenta.Rows(Fila).Cells(valorees).Value
                    End If

                    For Each valores As Integer In coincidencia
                        If (valores = Int) Then
                            'MsgBox(Int)
                            encontro = 1
                        End If
                    Next
                    If (encontro = 1) Then
                        'encontro
                        'GridConciliacion.Rows.Add(1)
                        'GridConciliacion.Rows(numFila).Cells(Col).Value = GridEdoCuenta.Rows(Fila).Cells(Col).Value

                    Else
                        encontro = 0
                        Col = NColGridEdoCuenta - 1
                    End If
                    My.Application.DoEvents()
                Next

            Next

            If (encontro = 1) Then
                numFila += 1
            End If
            My.Application.DoEvents()
        Next
        '========================================================================================

        Dim numFila2 As Integer = 0
        For Fila As Integer = 0 To NRowGridIcaav - 1
            encontro = 0
            My.Application.DoEvents()
            For Col As Integer = 0 To NColGridIcaav - 1
                My.Application.DoEvents()
                For Each valorees In selectIcaav

                    My.Application.DoEvents()
                    If (String.IsNullOrEmpty(Trim(IIf(GridIcaav.Rows(Fila).Cells(valorees).Value Is DBNull.Value, "", GridIcaav.Rows(Fila).Cells(valorees).Value)))) Then
                        Int = 0
                    Else
                        Int = GridIcaav.Rows(Fila).Cells(valorees).Value
                    End If

                    For Each valores As Integer In coincidencia
                        If (valores = Int) Then
                            'MsgBox(Int)
                            encontro = 1
                        End If
                    Next
                    If (encontro = 1) Then
                        cuerpoConciliacionIcaav.Rows(numFila2).Cells(Col).Value = GridIcaav.Rows(Fila).Cells(Col).Value

                    Else
                        encontro = 0
                        Col = NColGridIcaav - 1
                    End If
                    My.Application.DoEvents()
                Next

            Next

            If (encontro = 1) Then
                numFila2 += 1
            End If
            My.Application.DoEvents()
        Next



    End Sub
    Public Sub CuerpoCuenta()
        Dim Int As Integer
        Dim arrayIcaav As New ArrayList
        Dim encontro As Integer = 0

        TabNavegacion.SelectedIndex = 4
        For Each valorees In selectIcaav
            My.Application.DoEvents()
            For value1 As Integer = 0 To GridIcaav.Rows.Count - 2
                My.Application.DoEvents()
                arrayIcaav.Add(GridIcaav.Rows(value1).Cells(valorees).Value)
                ' MsgBox(GridIcaav.Rows(value1).Cells("FACTURA").Value)
            Next
        Next

        Dim NColGridEdoCuenta As Integer = GridEdoCuenta.ColumnCount
        Dim NRowGridEdoCuenta As Integer = GridEdoCuenta.RowCount
        For Fila As Integer = 0 To NRowGridEdoCuenta - 1
            My.Application.DoEvents()
            If (Fila <> 0) Then
                'GridNoEncontrados.Rows.Add(1)
            End If
        Next
        Dim numFila As Integer = 0
        For Fila As Integer = 0 To NRowGridEdoCuenta - 1
            encontro = 0
            My.Application.DoEvents()
            For Col As Integer = 0 To NColGridEdoCuenta - 1
                For Each valorees In selectEdo
                    My.Application.DoEvents()

                    If (String.IsNullOrEmpty(Trim(IIf(GridEdoCuenta.Rows(Fila).Cells(valorees).Value Is DBNull.Value, "", GridEdoCuenta.Rows(Fila).Cells(valorees).Value)))) Then
                        Int = 0
                    Else
                        Int = GridEdoCuenta.Rows(Fila).Cells(valorees).Value
                    End If

                    For Each valores As Integer In arrayIcaav
                        If (valores = Int) Then
                            encontro = 1
                        End If
                    Next
                    If (encontro = 1) Then
                        'MsgBox("Encontro: " & Int)
                        'encontro = 0
                        arrayEncontroEdo.Add(GridEdoCuenta.Rows(Fila).Cells(valorees).Value)
                        My.Application.DoEvents()
                        Col = NColGridEdoCuenta - 1
                    Else
                        encontro = 0
                        'GridNoEncontrados.Rows(numFila).Cells(Col).Value = GridEdoCuenta.Rows(Fila).Cells(Col).Value

                    End If

                Next
                My.Application.DoEvents()
            Next

            If (encontro = 0) Then
                numFila += 1
            End If
            My.Application.DoEvents()
        Next
    End Sub
    Public Sub cuerpoNCredito()
        TabNavegacion.SelectedIndex = 6
        Dim arrayIcaav As New ArrayList
        arrayIcaav.Clear()
        Dim encontro As Integer = 0

        Dim NColGridEdoCuenta As Integer = GridEdoCuenta.ColumnCount
        Dim NRowGridEdoCuenta As Integer = GridEdoCuenta.RowCount
        Dim numFila As Integer = 0
        For Fila As Integer = 0 To NRowGridEdoCuenta - 1
            encontro = 0
            My.Application.DoEvents()
            For Col As Integer = 0 To NColGridEdoCuenta - 1
                My.Application.DoEvents()
                If (IsNumeric(GridEdoCuenta.Rows(Fila).Cells(Col).Value)) Then
                    If (GridEdoCuenta.Rows(Fila).Cells(Col).Value < 0) Then
                        encontro = 1
                    Else
                        'encontro = 0
                    End If
                Else
                    'encontro = 0
                End If
                My.Application.DoEvents()
            Next
            My.Application.DoEvents()
        Next
    End Sub
    Public Sub cuerpoIcaav()
        TabNavegacion.SelectedIndex = 5
        Dim int As Integer

        arrayEdo.Clear()
        Dim encontro As Integer = 0

        For value1 As Integer = 0 To GridEdoCuenta.Rows.Count - 1
            My.Application.DoEvents()
            For Each valorees In selectEdo
                My.Application.DoEvents()
                If (String.IsNullOrEmpty(Trim(IIf(GridEdoCuenta.Rows(value1).Cells(valorees).Value Is DBNull.Value, "", GridEdoCuenta.Rows(value1).Cells(valorees).Value)))) Then
                    'int = 0
                Else
                    int = GridEdoCuenta.Rows(value1).Cells(valorees).Value
                    ' MsgBox(int.ToString)
                    arrayEdo.Add(int)
                End If

            Next
        Next


        Dim NColGridGridIcaav As Integer = GridIcaav.ColumnCount
        Dim NRowGridGridIcaav As Integer = GridIcaav.RowCount
        Dim numFila As Integer = 0

        For Fila As Integer = 0 To NRowGridGridIcaav - 1
            My.Application.DoEvents()
            If (Fila <> 0) Then
                'DataGridNoIcaav.Rows.Add(1)
            End If
        Next
        Dim busqueda As Integer = 0
        For Fila As Integer = 0 To NRowGridGridIcaav - 1
            encontro = 0
            busqueda = 0
            My.Application.DoEvents()
            For Col As Integer = 0 To NColGridGridIcaav - 1
                My.Application.DoEvents()
                If (IsNothing(GridIcaav.Rows(Fila).Cells(Col).Value)) Then
                    Continue For
                Else
                    For Each valorees In selectIcaav
                        My.Application.DoEvents()
                        If (IsNothing(GridIcaav.Rows(Fila).Cells(valorees).Value)) Then
                            Exit For
                        Else
                            If (busqueda = 0) Then

                                For Each valores As String In arrayEdo
                                    busqueda = 1
                                    If (valores = GridIcaav.Rows(Fila).Cells(valorees).Value) Then
                                        encontro = 1

                                        Exit For
                                    End If

                                Next
                            End If
                            If (encontro = 1) Then
                                ' MsgBox("encontro2:" & GridIcaav.Rows(Fila).Cells("FACTURA").Value)
                                arrayEncontroIcaav.Add(GridIcaav.Rows(Fila).Cells(valorees).Value)
                                Col = NColGridGridIcaav - 1
                                Exit For
                            Else
                                encontro = 0
                                'DataGridNoIcaav.Rows(numFila).Cells(Col).Value = GridIcaav.Rows(Fila).Cells(Col).Value
                            End If
                        End If
                        My.Application.DoEvents()

                    Next
                End If
            Next
            If (encontro = 0) Then
                numFila += 1
            End If
            My.Application.DoEvents()
        Next
    End Sub
    Sub CabeceraConciliacion()
        My.Application.DoEvents()
        ' GridConciliacion.Columns.Clear()
        Dim NCol As Integer = GridEdoCuenta.ColumnCount
        For i As Integer = 1 To NCol
            My.Application.DoEvents()
            Dim col1 As New DataGridViewTextBoxColumn
            col1.Name = GridEdoCuenta.Columns(i - 1).Name.ToString()
            If (GridEdoCuenta.Columns(i - 1).Name.ToString() = "NUM DE ORDEN DE SERVICIO") Then
                numOrden = i - 1
            End If
            'GridConciliacion.Columns.Add(col1)
            My.Application.DoEvents()

        Next

        'GridConciliacion.Columns.Add(NCol + 1, "Documento")
        columna_documemnto = NCol + 1


        'GridConciliacion.ForeColor = Color.Black
        'GridConciliacion.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        'GridConciliacion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        'GridConciliacion.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        '==========================================================================================
        My.Application.DoEvents()
        cuerpoConciliacionIcaav.Columns.Clear()
        Dim NColIcaav As Integer = GridIcaav.ColumnCount
        For i As Integer = 1 To NColIcaav
            My.Application.DoEvents()
            Dim col1 As New DataGridViewTextBoxColumn
            col1.Name = GridIcaav.Columns(i - 1).Name.ToString()
            If (GridIcaav.Columns(i - 1).Name.ToString() = "FACTURA") Then
                numFactura = i - 1
            End If
            cuerpoConciliacionIcaav.Columns.Add(col1)
            My.Application.DoEvents()

        Next
        cuerpoConciliacionIcaav.ForeColor = Color.Black
        cuerpoConciliacionIcaav.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        cuerpoConciliacionIcaav.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        cuerpoConciliacionIcaav.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

    End Sub
    Sub cabeceraEdo()
        My.Application.DoEvents()
        'GridNoEncontrados.Columns.Clear()
        Dim NCol As Integer = GridEdoCuenta.ColumnCount
        For i As Integer = 1 To NCol
            My.Application.DoEvents()
            Dim col1 As New DataGridViewTextBoxColumn
            col1.Name = GridEdoCuenta.Columns(i - 1).Name.ToString()
            'GridNoEncontrados.Columns.Add(col1)

        Next

        'GridNoEncontrados.ForeColor = Color.Black
        'GridNoEncontrados.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        'GridNoEncontrados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        'GridNoEncontrados.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    End Sub
    Sub CabeceraIcaav()
        My.Application.DoEvents()
        'DataGridNoIcaav.Columns.Clear()
        Dim NCol As Integer = GridIcaav.ColumnCount
        For i As Integer = 1 To NCol
            My.Application.DoEvents()
            Dim col1 As New DataGridViewTextBoxColumn
            col1.Name = GridIcaav.Columns(i - 1).Name.ToString
            'DataGridNoIcaav.Columns.Add(col1)
        Next


        'DataGridNoIcaav.ForeColor = Color.Black
        'DataGridNoIcaav.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        'DataGridNoIcaav.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        'DataGridNoIcaav.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    End Sub
    Sub cabeceraNCreditos()
        My.Application.DoEvents()
        Dim NCol As Integer = GridEdoCuenta.ColumnCount
    End Sub
    Sub eliminarColumnasVacias(grid As DataGridView)
        My.Application.DoEvents()
        Dim NCol As Integer = grid.ColumnCount
        Dim NRow As Integer = grid.RowCount
        For X = 0 To grid.Columns.Count - 1
            If (IsNothing(grid.Columns(X).ToString)) Then
                grid.Columns.RemoveAt(X)
            End If
        Next

    End Sub

    Sub SeleccionaArchivo(textbox_excel As TextBox, combobox_hoja As ComboBox)
        My.Application.DoEvents()
        Dim sArchivos As String
        Try
            OpenFileDialog1.Filter = "Archivo Excel 2007 (.xlsx)|*.xlsx|Archivo Excel 2003(.xls) |*.xls|Archivos de Microsoft Excel|*.xlsx;*.xls"
            OpenFileDialog1.FilterIndex = 3
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Multiselect = False
            OpenFileDialog1.RestoreDirectory = True

            If (OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
                My.Application.DoEvents()
                sArchivos = OpenFileDialog1.FileName
                textbox_excel.Text = sArchivos

                combobox_hoja.Items.Clear()
                combobox_hoja.Text = ""

                llenarcombobox(textbox_excel, combobox_hoja)
            Else
                textbox_excel.Text = ""
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub llenarcombobox(texbox_excel As TextBox, combobox_hoja As ComboBox)

        My.Application.DoEvents()
        Dim ObjExcel As Object
        ObjExcel = CreateObject("Excel.Application")

        Try
            My.Application.DoEvents()
            combobox_hoja.Items.Clear()
            combobox_hoja.Visible = True
            ObjExcel.Workbooks.Open(texbox_excel.Text)
            For y = 1 To ObjExcel.Sheets.Count
                My.Application.DoEvents()
                combobox_hoja.Items.Add(ObjExcel.Sheets(y).Name)
            Next

            ObjExcel.DisplayAlerts = False
            ObjExcel.Visible = False
            ObjExcel.Quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ObjExcel)
            ObjExcel = Nothing
            combobox_hoja.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()

            MessageBox.Show("POR FAVOR SELECCIONE LA HOJA DE TRABAJO", "SELECCIONE LA HOJA DE TRABAJO", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        combobox_hoja.Enabled = True
    End Sub

    Sub LLenarGrid(textbox_excel As TextBox, combobox_hoja As ComboBox, grid_resultado As DataGridView)
        My.Application.DoEvents()
        Me.Cursor = Cursors.WaitCursor
        If combobox_hoja.Text = "" Then Exit Sub
        Dim this As String = "1"
        leerexcel(this, textbox_excel, combobox_hoja, grid_resultado)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub leerexcel(ByVal cual As String, textbox_excel As TextBox, combobox_hoja As ComboBox, grid_resultado As DataGridView)

        My.Application.DoEvents()
        Dim conexion As New OleDbConnection
        Dim comando As New OleDbCommand
        Dim adaptador As New OleDbDataAdapter
        Dim dsexcel As New DataSet
        Dim ExcelPath As String = textbox_excel.Text.ToLower()
        Dim hoja As String = ""
        Dim num As Integer
        Dim extension As String = ""

        If cual = "1" Then
            My.Application.DoEvents()
            hoja = combobox_hoja.Text
            ExcelPath = textbox_excel.Text.ToLower()
            num = Trim(textbox_excel.Text.ToLower()).Length
            extension = Trim(textbox_excel.Text.ToLower()).Substring(num - 4, 4)
        End If

        If extension = ".xls" Then
            conexion.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ExcelPath & "; Extended Properties= ""Excel 8.0;HDR=YES; IMEX=1"""
        Else
            conexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ExcelPath & ";Extended Properties=" & Chr(34) & "Excel 12.0 Xml;HDR=YES;IMEX=1" & Chr(34)
        End If

        My.Application.DoEvents()
        conexion.Open()
        comando.CommandText = "SELECT * FROM [" & hoja & "$]"
        comando.Connection = conexion
        adaptador.SelectCommand = comando
        conexion.Close()
        adaptador.Fill(dsexcel, "excel")

        quitafilasvacias(dsexcel)
        'quitaColumnasvacias(dsexcel)
        If cual = "1" Then
            My.Application.DoEvents()
            grid_resultado.DataSource = dsexcel.Tables(0)
            grid_resultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            grid_resultado.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            grid_resultado.ForeColor = Color.Black
            grid_resultado.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
            grid_resultado.DataSource = dsexcel.Tables(0)

        End If

    End Sub

    Public Function quitafilasvacias(ByRef dds As DataSet)
        Dim dt As DataTable = dds.Tables(0)

        If dt.Rows.Count > 2 Then
            Dim eliminar As Boolean, c As Integer
            For n As Integer = dt.Rows.Count - 1 To 0 Step -1
                My.Application.DoEvents()
                Dim row As DataRow = dt.Rows(n)
                eliminar = False
                c = 0
                Do
                    My.Application.DoEvents()
                    eliminar = row.Item(c).ToString.Trim = Nothing
                    If Not eliminar Then Exit For
                    c = c + 1
                Loop While c < dt.Columns.Count
                If eliminar Then dt.Rows.Remove(row)
            Next
        End If
        Return dds
    End Function
    Public Function quitaColumnasvacias(ByRef dds As DataSet)
        Dim dt As DataTable = dds.Tables(0)

        If dt.Rows.Count > 2 Then
            Dim c As Integer, row As Integer = 0
            Dim valida As Integer = 0
            '============
            Do
                Dim colum As DataColumn = dt.Columns(c)
                My.Application.DoEvents()
                valida = 0
                row = 0
                Do
                    If (dt.Rows(row)(c).ToString.Trim = Nothing) Then
                        valida = 1
                    Else
                        If (IsDBNull(dt.Rows(row)(c).ToString.Trim)) Then
                            valida = 1
                        Else
                            valida = 0
                        End If
                    End If
                    row += 1
                Loop While row < dt.Rows.Count
                If (valida = 1) Then
                    dt.Columns.RemoveAt(c)
                End If
                c = c + 1
            Loop While c < dt.Columns.Count
            '=======================
            'If eliminar Then dt.Rows.Remove(row)

        End If
        Return dds
    End Function

    Private Shared Function QuitaAcentos(stIn As String) As String

        Dim stFormD As String = stIn.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()

        For ich As Integer = 0 To stFormD.Length - 1
            Dim uc As UnicodeCategory = CharUnicodeInfo.GetUnicodeCategory(stFormD(ich))
            If uc <> UnicodeCategory.NonSpacingMark Then
                sb.Append(stFormD(ich))
            End If
        Next

        Return (sb.ToString().Normalize(NormalizationForm.FormC))

    End Function

    Function validaEdoCuenta() As Boolean

        Dim val_1 As Boolean = False
        Dim val_2 As Boolean = False
        Dim val_3 As Boolean = False
        Dim val_4 As Boolean = False
        Dim val_5 As Boolean = False


        For Each columna As DataGridViewColumn In GridEdoCuenta.Columns

            My.Application.DoEvents()
            Dim col_name As String = QuitaAcentos(columna.Name.ToString.ToUpper)


            If col_name = "REF DE EDO CUENTA" Then
                val_1 = True
            End If
            If col_name = "REF PAGOS CREDITOS APLICADOS" Then
                val_2 = True
            End If
            If col_name = "IND CARGO EN DISPUTA" Then
                val_3 = True
            End If
            If col_name = "MONTO DE PAGO O CREDITO APLI" Then
                val_4 = True
            End If
            If col_name = "NUM DE ORDEN DE SERVICIO" Then
                val_5 = True
            End If
        Next

        If val_1 = False Then
            MsgBox("FALTA COLUMNA (REF DE EDO CUENTA)")
            Return False
            Exit Function
        End If
        If val_2 = False Then
            MsgBox("FALTA COLUMNA (REF PAGOS CREDITOS APLICADOS)")
            Return False
            Exit Function
        End If
        If val_3 = False Then
            MsgBox("FALTA COLUMNA (IND CARGO EN DISPUTA)")
            Return False
            Exit Function
        End If
        If val_4 = False Then
            MsgBox("FALTA COLUMNA (MONTO DE PAGO O CREDITO APLI)")
            Return False
            Exit Function
        End If
        If val_5 = False Then
            MsgBox("FALTA COLUMNA (NUM DE ORDEN DE SERVICIO)")
            Return False
            Exit Function
        End If

        Return True

    End Function

    Function validaIcaav() As Boolean

        Dim col_1, col_2, col_3, col_4, col_5, col_6, col_7 As Integer

        Dim val_1 As Boolean = False
        Dim val_2 As Boolean = False
        Dim val_3 As Boolean = False
        Dim val_4 As Boolean = False
        Dim val_5 As Boolean = False
        Dim val_6 As Boolean = False
        Dim val_7 As Boolean = False

        For Each columna As DataGridViewColumn In GridIcaav.Columns

            My.Application.DoEvents()
            Dim col_name As String = QuitaAcentos(columna.Name.ToString.ToUpper)

            If col_name = "SUCURSAL" Then
                col_1 = columna.Index
                val_1 = True
            End If
            If col_name = "FACTURA" Then
                col_2 = columna.Index
                val_2 = True
            End If
            If col_name = "FECHA SALIDA" Then
                col_3 = columna.Index
                val_3 = True
            End If
            If col_name = "PASAJERO" Then
                col_4 = columna.Index
                val_4 = True
            End If
            If col_name = "TOTAL" Then
                col_5 = columna.Index
                val_5 = True
            End If
            If col_name = "NUMERO BOLETO" Then
                col_6 = columna.Index
                val_6 = True
            End If
            If col_name = "RUTA" Then
                col_7 = columna.Index
                val_7 = True
            End If
        Next

        If val_1 = False Then
            MsgBox("FALTA COLUMNA (SUCURSAL)")
            Return False
            Exit Function
        End If
        If val_2 = False Then
            MsgBox("FALTA COLUMNA (FACTURA)")
            Return False
            Exit Function
        End If
        If val_3 = False Then
            MsgBox("FALTA COLUMNA (FECHA SALIDA)")
            Return False
            Exit Function
        End If
        If val_4 = False Then
            MsgBox("FALTA COLUMNA (PASAJERO)")
            Return False
            Exit Function
        End If
        If val_5 = False Then
            MsgBox("FALTA COLUMNA (TOTAL)")
            Return False
            Exit Function
        End If
        If val_6 = False Then
            MsgBox("FALTA COLUMNA (NUMERO BOLETO)")
            Return False
            Exit Function
        End If
        If val_7 = False Then
            MsgBox("FALTA COLUMNA (RUTA)")
            Return False
            Exit Function
        End If

        Return True

    End Function


    Sub llenarcheckbox()
        CheckedListBox3.Items.Clear()
        'clbEstadoDeCuenta.Items.Clear()
        'clbIcaav.Items.Clear()
        CheckedListBox4.Items.Clear()
        Dim NCol As Integer = GridEdoCuenta.ColumnCount
        For i As Integer = 1 To NCol
            'clbEstadoDeCuenta.Items.Add(GridEdoCuenta.Columns(i - 1).Name.ToString)
            CheckedListBox3.Items.Add(GridEdoCuenta.Columns(i - 1).Name.ToString)
        Next

        'clbEstadoDeCuenta.Visible = True
        Dim NColIcaav As Integer = GridIcaav.ColumnCount
        For i As Integer = 1 To NColIcaav
            'clbIcaav.Items.Add(GridIcaav.Columns(i - 1).Name.ToString)
            CheckedListBox4.Items.Add(GridIcaav.Columns(i - 1).Name.ToString)
        Next

        'clbIcaav.Visible = True


    End Sub

    Function ObtenerColumnasEC() As List(Of String)

        Dim columnasEC = New List(Of String)
        Dim NCol As Integer = GridEdoCuenta.ColumnCount
        For i As Integer = 1 To NCol
            columnasEC.Add(GridEdoCuenta.Columns(i - 1).Name.ToString)
        Next
        Return columnasEC

    End Function

    Function ObtenerColumnasIC() As List(Of String)
        Dim columnasIC = New List(Of String)
        Dim NCol As Integer = GridIcaav.ColumnCount
        For i As Integer = 1 To NCol
            columnasIC.Add(GridIcaav.Columns(i - 1).Name.ToString)
        Next
        Return columnasIC
    End Function


    Private Async Sub btn_exportar_Click(sender As Object, e As EventArgs) Handles btn_exportar.Click
        'nuevo ismael 230718
        bandera_report = 0

        SaveFileDialog1.Filter = "ARCHIVOS EXCEL 2007-2013|*.xlsx"
        SaveFileDialog1.Title = "GUARDAR REPORTE DE CONCILIACIÓN"
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then

            If SaveFileDialog1.FileName <> "" Then
                Dim filename As String = SaveFileDialog1.FileName
                'Await Task.Factory.StartNew(Sub()
                '                                ExportToExcelEPPlus(filename, GridsReportesTabs)
                '                            End Sub)

                Await Task.Factory.StartNew(Sub()
                                                ExportarMejorado(filename, GridsReportesTabs)
                                            End Sub)
            End If
        Else
            Exit Sub
        End If
    End Sub

    Public Sub ExportarMejorado(ByVal filePath As String, ByVal listaGrids As Dictionary(Of String, DataGridView))

        Dim ColumnasECChecked As CheckedListBox.CheckedItemCollection = Nothing
        Dim ColumnasICChecked As CheckedListBox.CheckedItemCollection = Nothing

        If Me.InvokeRequired Then
            Invoke(Sub()
                       ColumnasECChecked = CheckedListBox3.CheckedItems
                       ColumnasICChecked = CheckedListBox4.CheckedItems
                   End Sub)
        Else
            ColumnasECChecked = CheckedListBox3.CheckedItems
            ColumnasICChecked = CheckedListBox4.CheckedItems
        End If

        If ColumnasECChecked.Count() + ColumnasICChecked.Count() < 1 Then
            If MessageBox.Show("DEBE DE SELECCIONAR POR LO MENOS UNA COLUMNA PARA CREAR EL REPORTE DE CONCILIACIÓN", "EXPORTAR A ARCHIVO EXCEL.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) = DialogResult.OK Then
                If Me.InvokeRequired Then
                    Invoke(Sub()
                               TabNavegacion.SelectedTab = TabPage6
                           End Sub)
                Else
                    TabNavegacion.SelectedTab = TabPage6
                End If
                Exit Sub
            End If
        End If

        ' // EMPIEZO PROCESO DE ORDENAMIENTO
        If Me.InvokeRequired Then
            Invoke(Sub()
                       ToolStripStatusLabel1.Text = "EXPORTANDO..."
                       ToolStripProgressBar1.Visible = True
                   End Sub)
        Else
            ToolStripStatusLabel1.Text = "EXPORTANDO..."
            ToolStripProgressBar1.Visible = True
        End If

        Dim TabsConciliaciones = GetAll(Me, GetType(TabPage)).Where(Function(x) x.Text.StartsWith("CONC."))

        Dim TabsReporteBCD = GetAll(Me, GetType(TabPage)).Where(Function(x) x.Text.StartsWith("REPORT.BCD")).FirstOrDefault()
        Dim TabsEstadoDeCuenta = GetAll(Me, GetType(TabPage)).Where(Function(x) x.Text.StartsWith("REPORT.EDO CUENTA")).FirstOrDefault()

        Dim TabsNoEncontradosReporteBCD = GetAll(Me, GetType(TabPage)).Where(Function(x) x.Text.StartsWith("NE.REPORT.BCD")).FirstOrDefault()
        Dim TabsNoEncontradosEstadoDeCuenta = GetAll(Me, GetType(TabPage)).Where(Function(x) x.Text.StartsWith("NE.REPORT.EDOCTA")).FirstOrDefault()

        Dim DtConciliacionUnica As DataTable = Nothing

        workbook = New XSSFWorkbook()
        cxml_workbook = New XLWorkbook()

        cxml_workbook.Style.Font.FontName = "Calibri"
        cxml_workbook.Style.Font.FontSize = 10

        If TabsReporteBCD IsNot Nothing Then
            Dim GridReporteBCD = GetAll(TabsReporteBCD, GetType(DataGridView)).FirstOrDefault()
            If GridReporteBCD IsNot Nothing Then
                Dim DtReporteBCD As DataTable = GetDataTableFromDGV(GridReporteBCD)
                AddSheetToWorkbook(DtReporteBCD, "REPORT.BCD")
            End If
        End If

        If TabsEstadoDeCuenta IsNot Nothing Then
            Dim GridEstadoDeCuenta = GetAll(TabsEstadoDeCuenta, GetType(DataGridView)).FirstOrDefault()
            If GridEstadoDeCuenta IsNot Nothing Then
                Dim DtEstadoDeCuenta As DataTable = GetDataTableFromDGV(GridEstadoDeCuenta)
                AddSheetToWorkbook(DtEstadoDeCuenta, "REPORT.EDO CUENTA")
            End If
        End If

        For i As Integer = 0 To TabsConciliaciones.Count() - 1
            Dim GridConciliacion = GetAll(TabsConciliaciones(i), GetType(DataGridView)).FirstOrDefault()
            If GridConciliacion IsNot Nothing Then
                If (i = 0) Then
                    Dim GridInicialDt As DataTable = GetDataTableFromDGV(GridConciliacion)
                    DtConciliacionUnica = GridInicialDt.Clone()
                    DtConciliacionUnica.Merge(GridInicialDt)
                Else
                    Dim GridConciliacionDt As DataTable = GetDataTableFromDGV(GridConciliacion)
                    DtConciliacionUnica.Merge(GridConciliacionDt)
                End If
            End If

            For c_index As Integer = DtConciliacionUnica.Columns.Count - 1 To 0 Step -1
                If Not (ColumnasECChecked.Contains(DtConciliacionUnica.Columns(c_index).ColumnName) Or ColumnasICChecked.Contains(DtConciliacionUnica.Columns(c_index).ColumnName)) Then
                    DtConciliacionUnica.Columns.Remove(DtConciliacionUnica.Columns(c_index).ColumnName)
                End If
            Next
            DtConciliacionUnica.AcceptChanges()
        Next

        AddSheetToWorkbook(DtConciliacionUnica, "CONCILIACION")

        If TabsNoEncontradosReporteBCD IsNot Nothing Then
            Dim GridNoEncontradosReporteBCD = GetAll(TabsNoEncontradosReporteBCD, GetType(DataGridView)).FirstOrDefault()
            If GridNoEncontradosReporteBCD IsNot Nothing Then
                Dim DtNoEncontradosReporteBCD As DataTable = GetDataTableFromDGV(GridNoEncontradosReporteBCD)
                AddSheetToWorkbook(DtNoEncontradosReporteBCD, "PENDIENTES.REPORT.BCD.")
            End If
        End If

        If TabsNoEncontradosEstadoDeCuenta IsNot Nothing Then
            Dim GridNoEncontradosEstadoDeCuenta = GetAll(TabsNoEncontradosEstadoDeCuenta, GetType(DataGridView)).FirstOrDefault()
            If GridNoEncontradosEstadoDeCuenta IsNot Nothing Then
                Dim DtNoEncontradosEstadoDeCuenta As DataTable = GetDataTableFromDGV(GridNoEncontradosEstadoDeCuenta)
                AddSheetToWorkbook(DtNoEncontradosEstadoDeCuenta, "PENDIENTES.REPORT.EDOCTA.")
            End If
        End If

        Try
            Using fileWriter As FileStream = File.Create(filePath)
                cxml_workbook.SaveAs(fileWriter)
            End Using

            If Me.InvokeRequired Then
                Invoke(Sub()
                           ToolStripStatusLabel1.Text = "LISTO"
                           ToolStripProgressBar1.Visible = False
                       End Sub)
            Else
                ToolStripStatusLabel1.Text = "LISTO"
                ToolStripProgressBar1.Visible = False
            End If

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filePath)
                If _fi.Exists Then
                    Process.Start(filePath)
                End If
            End If
        Catch ex As Exception
            If Me.InvokeRequired Then
                Invoke(Sub()
                           ToolStripStatusLabel1.Text = "LISTO"
                           ToolStripProgressBar1.Visible = False
                       End Sub)
            Else
                ToolStripStatusLabel1.Text = "LISTO"
                ToolStripProgressBar1.Visible = False
            End If
            MsgBox("OCURRIO UN ERROR AL EXPORTAR EL ARCHIVO: " & ex.Message.ToUpper())
            Exit Sub
        End Try
    End Sub

    Public Function GetAll(ByVal control As Control, ByVal type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAll(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function

    Private Function GetDataTableFromDGV(ByRef dgv As DataGridView) As DataTable
        Dim dt = New DataTable()

        For Each column As DataGridViewColumn In dgv.Columns

            If column.Visible Then
                dt.Columns.Add(column.Name)
            End If
        Next

        Dim cellValues As Object() = New Object(dgv.Columns.Count - 1) {}

        For Each row As DataGridViewRow In dgv.Rows

            For i As Integer = 0 To row.Cells.Count - 1
                cellValues(i) = row.Cells(i).Value
            Next

            dt.Rows.Add(cellValues)
        Next

        Return dt
    End Function

    Private Sub ExportToExcelEPPlus(ByVal filePath As String, ByVal listaGrids As Dictionary(Of String, DataGridView))
        workbook = New XSSFWorkbook()
        cxml_workbook = New XLWorkbook()

        cxml_workbook.Style.Font.FontName = "Calibri"
        cxml_workbook.Style.Font.FontSize = 10


        Dim TempMergedDataTable As DataTable = New DataTable()

        Dim ColumnasECChecked As Integer
        Dim ColumnasICChecked As Integer

        If Me.InvokeRequired Then
            Invoke(Sub()
                       ColumnasECChecked = CheckedListBox3.CheckedItems.Count
                       ColumnasICChecked = CheckedListBox4.CheckedItems.Count
                   End Sub)
        Else
            ColumnasECChecked = CheckedListBox3.CheckedItems.Count
            ColumnasICChecked = CheckedListBox4.CheckedItems.Count
        End If



        For Each _grupoConciliacion In ListaGruposConciliaciones

            Dim dtConciliacionEC As DataTable = Nothing
            Dim dtConciliacionIC As DataTable = Nothing
            Dim filasYaProcesas As List(Of DataRow) = New List(Of DataRow)

            If Me.InvokeRequired Then
                Invoke(Sub()
                           dtConciliacionEC = CType(GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource, DataTable).Copy()
                           dtConciliacionIC = CType(GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource, DataTable).Copy()
                       End Sub)
            Else
                dtConciliacionEC = CType(GridsReportesTabs("_CONC.REP.BCD." & _grupoConciliacion.Nombre).DataSource, DataTable).Copy()
                dtConciliacionIC = CType(GridsReportesTabs("_CONC.REP.EDOCTA." & _grupoConciliacion.Nombre).DataSource, DataTable).Copy()
            End If

            Dim ECConciliacionNumRows = dtConciliacionEC.Rows.Count
            Dim ICConciliacionNumRows = dtConciliacionIC.Rows.Count

            If ColumnasECChecked + ColumnasICChecked < 1 Then
                If MessageBox.Show("DEBE DE SELECCIONAR POR LO MENOS UNA COLUMNA PARA CREAR EL REPORTE DE CONCILIACIÓN", "EXPORTAR A ARCHIVO EXCEL.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) = DialogResult.OK Then
                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   TabNavegacion.SelectedTab = TabPage6
                               End Sub)
                    Else
                        TabNavegacion.SelectedTab = TabPage6
                    End If
                    Exit Sub
                End If
            End If



            ' // EMPIEZO PROCESO DE ORDENAMIENTO
            If Me.InvokeRequired Then
                Invoke(Sub()
                           ToolStripStatusLabel1.Text = "EXPORTANDO..."
                           ToolStripProgressBar1.Visible = True
                       End Sub)
            Else
                ToolStripStatusLabel1.Text = "EXPORTANDO..."
                ToolStripProgressBar1.Visible = True
            End If

            Dim testListaConciliaciones As List(Of Conciliacion) = _grupoConciliacion.ListaConciliaciones
            Dim CopiaDtConciliacionIC = dtConciliacionIC.Copy()
            Dim DictRowsEncontradas As Dictionary(Of DataRow, DataRow) = New Dictionary(Of DataRow, DataRow)()

            'Console.WriteLine("Empezando primer ordenamiento")
            For Each testECRow In dtConciliacionEC.Rows.Cast(Of DataRow)
                ''Parallel.ForEach(dtConciliacionEC.Rows.Cast(Of DataRow)(), Sub(testECRow)
                Dim rows_encontradas As EnumerableRowCollection(Of DataRow) = Nothing
                For t As Integer = 0 To testListaConciliaciones.Count - 1
                    Dim index_concil As Integer = t
                    If t = 0 Then

                        ' SI LA PRIMERA CONCILIACION ES DE TIPO NUMERICO
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.NUMERICO Then
                            rows_encontradas = CopiaDtConciliacionIC.AsEnumerable.Where(Function(trow)

                                                                                            Try
                                                                                                Dim numero1 As Decimal = Nothing
                                                                                                Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                Dim numero2 As Decimal = Nothing
                                                                                                Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                numero1 = Convert.ToDecimal(num1cadena)
                                                                                                numero2 = Convert.ToDecimal(num2cadena)
                                                                                                Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                            Catch ex As Exception
                                                                                                Return False
                                                                                            End Try
                                                                                        End Function)
                            Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                            If rowSeleccionada IsNot Nothing Then
                                If t = testListaConciliaciones.Count - 1 Then
                                    SyncLock DictRowsEncontradas
                                        DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                    End SyncLock
                                    SyncLock DictRowsEncontradas
                                        filasYaProcesas.Add(rowSeleccionada)
                                    End SyncLock
                                End If
                            End If
                        End If

                        ' SI LA PRIMERA CONCILIACION ES DE TIPO MONEDA
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.MONEDA Then
                            rows_encontradas = CopiaDtConciliacionIC.AsEnumerable.Where(Function(trow)

                                                                                            Try
                                                                                                Dim numero1 As Decimal = Nothing
                                                                                                Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                Dim numero2 As Decimal = Nothing
                                                                                                Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                numero1 = Decimal.Parse(num1cadena, NumberStyles.Currency)
                                                                                                numero2 = Decimal.Parse(num2cadena, NumberStyles.Currency)
                                                                                                Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                                            Catch ex As Exception
                                                                                                Return False
                                                                                            End Try
                                                                                        End Function)
                            Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                            If rowSeleccionada IsNot Nothing Then
                                If t = testListaConciliaciones.Count - 1 Then
                                    SyncLock DictRowsEncontradas
                                        DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                    End SyncLock
                                    SyncLock DictRowsEncontradas
                                        filasYaProcesas.Add(rowSeleccionada)
                                    End SyncLock
                                End If
                            End If
                        End If


                        ' SI LA PRIMERA CONCILIACION ES DE TIPO FECHA
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.FECHA Then
                            rows_encontradas = CopiaDtConciliacionIC.AsEnumerable.Where(Function(trow)

                                                                                            Try
                                                                                                Dim fecha1 As DateTime = Nothing
                                                                                                Dim fecha1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                Dim fecha2 As DateTime = Nothing
                                                                                                Dim fecha2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                                fecha1 = DateTime.Parse(fecha1cadena)
                                                                                                fecha2 = DateTime.Parse(fecha2cadena)
                                                                                                Return (fecha1 = fecha2) And Not filasYaProcesas.Contains(trow)
                                                                                            Catch ex As Exception
                                                                                                Return False
                                                                                            End Try
                                                                                        End Function)
                            Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                            If rowSeleccionada IsNot Nothing Then
                                If t = testListaConciliaciones.Count - 1 Then
                                    SyncLock DictRowsEncontradas
                                        DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                    End SyncLock
                                    SyncLock DictRowsEncontradas
                                        filasYaProcesas.Add(rowSeleccionada)
                                    End SyncLock
                                End If
                            End If
                        End If


                        ' SI LA PRIMERA CONCILIACION ES DE TIPO TEXTO
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.TEXTO Then
                            If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_IGUAL Then
                                rows_encontradas = CopiaDtConciliacionIC.AsEnumerable.Where(Function(trow)


                                                                                                Try
                                                                                                    Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                    Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                    Return (cadena1.Equals(cadena2)) And Not filasYaProcesas.Contains(trow)
                                                                                                Catch ex As Exception
                                                                                                    Return False
                                                                                                End Try
                                                                                            End Function)
                                Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                If rowSeleccionada IsNot Nothing Then
                                    If t = testListaConciliaciones.Count - 1 Then
                                        SyncLock DictRowsEncontradas
                                            DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                        End SyncLock
                                        SyncLock DictRowsEncontradas
                                            filasYaProcesas.Add(rowSeleccionada)
                                        End SyncLock
                                    End If
                                End If
                            End If


                            ' // PARA TEXTO CONTAIN
                            If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_CONTIENE Then
                                rows_encontradas = CopiaDtConciliacionIC.AsEnumerable.Where(Function(trow)
                                                                                                Try
                                                                                                    Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                                    Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                                    Return (cadena1.Contains(cadena2) Or cadena2.Contains(cadena1)) And Not filasYaProcesas.Contains(trow)
                                                                                                Catch ex As Exception
                                                                                                    Return False
                                                                                                End Try
                                                                                            End Function)
                                Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                If rowSeleccionada IsNot Nothing Then
                                    If t = testListaConciliaciones.Count - 1 Then
                                        SyncLock DictRowsEncontradas
                                            DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                        End SyncLock
                                        SyncLock DictRowsEncontradas
                                            filasYaProcesas.Add(rowSeleccionada)
                                        End SyncLock
                                    End If
                                End If
                            End If
                        End If

                        '##
                    Else

                        '///////////////////////////////////////////////////////////
                        ' // SI TIENE MAS DE UNA CONDICION LAS APLICARA

                        ' SI LA PRIMERA CONCILIACION ES DE TIPO NUMERICO
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.NUMERICO Then
                            If rows_encontradas IsNot Nothing Then
                                rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                              Try
                                                                                  Dim numero1 As Decimal = Nothing
                                                                                  Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                  Dim numero2 As Decimal = Nothing
                                                                                  Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                  numero1 = Convert.ToDecimal(num1cadena)
                                                                                  numero2 = Convert.ToDecimal(num2cadena)
                                                                                  Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                              Catch ex As Exception
                                                                                  Return False
                                                                              End Try
                                                                          End Function)
                                Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                If rowSeleccionada IsNot Nothing Then
                                    If t = testListaConciliaciones.Count - 1 Then
                                        SyncLock DictRowsEncontradas
                                            DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                        End SyncLock
                                        SyncLock DictRowsEncontradas
                                            filasYaProcesas.Add(rowSeleccionada)
                                        End SyncLock
                                    End If
                                End If
                            End If
                        End If

                        ' SI LA PRIMERA CONCILIACION ES DE TIPO MONEDA
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.MONEDA Then
                            If rows_encontradas IsNot Nothing Then
                                rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                              Try
                                                                                  Dim numero1 As Decimal = Nothing
                                                                                  Dim num1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                  Dim numero2 As Decimal = Nothing
                                                                                  Dim num2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                  numero1 = Decimal.Parse(num1cadena, NumberStyles.Currency)
                                                                                  numero2 = Decimal.Parse(num2cadena, NumberStyles.Currency)
                                                                                  Return (numero1 = numero2) And Not filasYaProcesas.Contains(trow)
                                                                              Catch ex As Exception
                                                                                  Return False
                                                                              End Try
                                                                          End Function)
                                Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                If rowSeleccionada IsNot Nothing Then
                                    If t = testListaConciliaciones.Count - 1 Then
                                        SyncLock DictRowsEncontradas
                                            DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                        End SyncLock
                                        SyncLock DictRowsEncontradas
                                            filasYaProcesas.Add(rowSeleccionada)
                                        End SyncLock
                                    End If
                                End If
                            End If
                        End If


                        ' SI LA PRIMERA CONCILIACION ES DE TIPO FECHA
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.FECHA Then
                            If rows_encontradas IsNot Nothing Then
                                rows_encontradas = rows_encontradas.Where(Function(trow)

                                                                              Try
                                                                                  Dim fecha1 As DateTime = Nothing
                                                                                  Dim fecha1cadena As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                  Dim fecha2 As DateTime = Nothing
                                                                                  Dim fecha2cadena As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()

                                                                                  fecha1 = DateTime.Parse(fecha1cadena)
                                                                                  fecha2 = DateTime.Parse(fecha2cadena)
                                                                                  Return (fecha1 = fecha2) And Not filasYaProcesas.Contains(trow)
                                                                              Catch ex As Exception
                                                                                  Return False
                                                                              End Try
                                                                          End Function)
                                Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                If rowSeleccionada IsNot Nothing Then
                                    If t = testListaConciliaciones.Count - 1 Then
                                        SyncLock DictRowsEncontradas
                                            DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                        End SyncLock
                                        SyncLock DictRowsEncontradas
                                            filasYaProcesas.Add(rowSeleccionada)
                                        End SyncLock
                                    End If
                                End If
                            End If
                        End If


                        ' SI LA PRIMERA CONCILIACION ES DE TIPO TEXTO
                        If testListaConciliaciones(index_concil).TipoDeDatos = TiposDeDatos.TEXTO Then
                            If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_IGUAL Then
                                If rows_encontradas IsNot Nothing Then
                                    rows_encontradas = rows_encontradas.Where(Function(trow)


                                                                                  Try
                                                                                      Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                      Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                      Return (cadena1.Equals(cadena2)) And Not filasYaProcesas.Contains(trow)
                                                                                  Catch ex As Exception
                                                                                      Return False
                                                                                  End Try
                                                                              End Function)
                                    Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                    If rowSeleccionada IsNot Nothing Then
                                        If t = testListaConciliaciones.Count - 1 Then
                                            SyncLock DictRowsEncontradas
                                                DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                            End SyncLock
                                            SyncLock DictRowsEncontradas
                                                filasYaProcesas.Add(rowSeleccionada)
                                            End SyncLock
                                        End If
                                    End If
                                End If
                            End If


                            ' // PARA TEXTO CONTAIN
                            If testListaConciliaciones(index_concil).Operador = Operadores.TEXTO_CONTIENE Then
                                If rows_encontradas IsNot Nothing Then
                                    rows_encontradas = rows_encontradas.Where(Function(trow)


                                                                                  Try
                                                                                      Dim cadena1 As String = testECRow(testListaConciliaciones(index_concil).CampoEstadoDeCuenta).ToString()
                                                                                      Dim cadena2 As String = trow(testListaConciliaciones(index_concil).CampoIcaav).ToString()
                                                                                      Return (cadena1.Contains(cadena2) Or cadena2.Contains(cadena1)) And Not filasYaProcesas.Contains(trow)
                                                                                  Catch ex As Exception
                                                                                      Return False
                                                                                  End Try
                                                                              End Function)
                                    Dim rowSeleccionada As DataRow = rows_encontradas.FirstOrDefault()

                                    If rowSeleccionada IsNot Nothing Then
                                        If t = testListaConciliaciones.Count - 1 Then
                                            SyncLock DictRowsEncontradas
                                                DictRowsEncontradas.Add(testECRow, rowSeleccionada)
                                            End SyncLock
                                            SyncLock DictRowsEncontradas
                                                filasYaProcesas.Add(rowSeleccionada)
                                            End SyncLock
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        ' /////////////////////////////////////////////


                    End If
                Next
            Next
            'Console.WriteLine("Terminando primer ordenamiento")
            ''End Sub)
            ' //

            ' // CREAMOS TABLAS CON LA MISMA ESTRUCTURA PERO SIN DATOS
            Dim dtECReport As DataTable = dtConciliacionEC.Clone()
            Dim dtICReport As DataTable = dtConciliacionIC.Clone()

            ' // AGREGAMOS LAS COLUMNAS DE LA SEGUNDA TABLA DENTRO DE LA PRIMERA
            'Console.WriteLine("Combinando Columnas de Ambas tablas")
            For Each column As DataColumn In dtICReport.Columns
                If dtECReport.Columns.Contains(column.ColumnName) Then
                    dtECReport.Columns.Add(column.ColumnName & "_")
                Else
                    dtECReport.Columns.Add(column.ColumnName)
                End If
            Next
            ' // PARA QUE SE VEAN REFLEJADOS LOS CAMBIOS
            dtECReport.AcceptChanges()

            For Each parejaFilasConciliadas As KeyValuePair(Of DataRow, DataRow) In DictRowsEncontradas.AsQueryable()
                'Parallel.ForEach(DictRowsEncontradas, Sub(parejaFilasConciliadas As KeyValuePair(Of DataRow, DataRow))
                Dim FilaPrimerReporte = CType(parejaFilasConciliadas.Key, DataRow)
                Dim FilaSegundoReporte = CType(parejaFilasConciliadas.Value, DataRow)

                Dim filaReporteCombinado As DataRow = dtECReport.NewRow()

                For g As Integer = 0 To dtECReport.Columns.Count - 1
                    If g < dtConciliacionEC.Columns.Count Then
                        filaReporteCombinado(g) = FilaPrimerReporte(g)
                    Else
                        filaReporteCombinado(g) = FilaSegundoReporte(g - dtConciliacionEC.Columns.Count)
                    End If
                Next
                SyncLock dtECReport
                    dtECReport.Rows.Add(filaReporteCombinado)
                    dtECReport.AcceptChanges()
                End SyncLock
            Next
            ''End Sub)
            'Console.WriteLine("Terminando de combinar ambas tablas")

            ' // ESTO ES PARA LAS FILAS SELECCIONADAS
            For itemindex As Integer = 0 To CheckedListBox3.Items.Count - 1
                If Not CheckedListBox3.GetItemChecked(itemindex) Then
                    If dtECReport.Columns.Contains(CheckedListBox3.Items(itemindex).ToString()) Then
                        dtECReport.Columns.Remove(CheckedListBox3.Items(itemindex).ToString())
                    ElseIf dtECReport.Columns.Contains(CheckedListBox3.Items(itemindex).ToString() & "_") Then
                        dtECReport.Columns.Remove(CheckedListBox3.Items(itemindex).ToString() & "_")
                    End If
                End If
            Next

            For itemindex As Integer = 0 To CheckedListBox4.Items.Count - 1
                If Not CheckedListBox4.GetItemChecked(itemindex) Then
                    If dtECReport.Columns.Contains(CheckedListBox4.Items(itemindex).ToString()) Then
                        dtECReport.Columns.Remove(CheckedListBox4.Items(itemindex).ToString())
                    ElseIf dtECReport.Columns.Contains(CheckedListBox4.Items(itemindex).ToString() & "_") Then
                        dtECReport.Columns.Remove(CheckedListBox4.Items(itemindex).ToString() & "_")
                    End If
                End If
            Next

            dtConciliacionEC.AcceptChanges()

            'para imprimir los reportes de estado de cuenta y bcd

            'nuevo agrgado ismael para el reporte de estado de cuenta y bcd 230718
            If bandera_report = 0 Then

                bandera_report = 1
                'Console.WriteLine("Empezando a generar reporte con EPPLUS")

                ' Empieza codigo para exportar grids con NPOI
                ' Parametros Source as DataTable

                Dim dataTableTmp_bcd As DataTable = CType(GridEdoCuenta.DataSource, DataTable)
                Dim sheetName_bcd = "REPORT.BCD"

                AddSheetToWorkbook(dataTableTmp_bcd, sheetName_bcd)

                Dim dataTableTmp_estado As DataTable = CType(GridIcaav.DataSource, DataTable)
                Dim sheetName_es = "REPORT.EDO CUENTA"

                AddSheetToWorkbook(dataTableTmp_estado, sheetName_es)

            End If

            'fin
            If TempMergedDataTable.Columns.Count = 0 Then
                TempMergedDataTable = dtECReport.Clone()
                TempMergedDataTable.Merge(dtECReport)
            Else
                TempMergedDataTable.Merge(dtECReport)
            End If
        Next

        AddSheetToWorkbook(TempMergedDataTable, "CONCILIACION")

        For Each grid In listaGrids
            Dim dataTableTmp As DataTable = CType(grid.Value.DataSource, DataTable)
            'Dim sheetName = grid.Key
            Dim sheetName = String.Empty
            If grid.Key.Contains("_NE") Then
                sheetName = grid.Key.Replace("_NE", "PENDIENTES")
            End If
            If grid.Key.Contains("_CONFLICT") Then
                sheetName = grid.Key.Replace("_CONFLICT", "CONFLICTOS")
            End If

            'NIEVO AGRGADO ISMAEL 230718
            If grid.Key.Contains("_NE") Or grid.Key.Contains("_CONFLICT") Then
                If grid.Value.RowCount > 0 Then
                    AddSheetToWorkbook(dataTableTmp, sheetName)
                End If
            End If
        Next

        Try
            Using fileWriter As FileStream = File.Create(filePath)
                cxml_workbook.SaveAs(fileWriter)
            End Using

            If Me.InvokeRequired Then
                Invoke(Sub()
                           ToolStripStatusLabel1.Text = "LISTO"
                           ToolStripProgressBar1.Visible = False
                       End Sub)
            Else
                ToolStripStatusLabel1.Text = "LISTO"
                ToolStripProgressBar1.Visible = False
            End If

            If MessageBox.Show("EL REPORTE EXCEL SE HA CREADO SATISFACTORIAMENTE. ¿DESEA ABRIR EL REPORTE AHORA?", "REPORTE GENERADO CORRECTAMENTE.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Dim _fi As FileInfo = New FileInfo(filePath)
                If _fi.Exists Then
                    Process.Start(filePath)
                End If
            End If
        Catch ex As Exception
            If Me.InvokeRequired Then
                Invoke(Sub()
                           ToolStripStatusLabel1.Text = "LISTO"
                           ToolStripProgressBar1.Visible = False
                       End Sub)
            Else
                ToolStripStatusLabel1.Text = "LISTO"
                ToolStripProgressBar1.Visible = False
            End If
            MsgBox("OCURRIO UN ERROR AL EXPORTAR EL ARCHIVO: " & ex.Message.ToUpper())
            Exit Sub
        End Try
    End Sub

    Private Sub AddSheetToWorkbook(ByVal dtSource As DataTable, ByVal sheetName As String)
        Dim worksheet = cxml_workbook.Worksheets.Add(sheetName)
        Dim ci As CultureInfo = New CultureInfo("en-US")

        For i As Integer = 0 To dtSource.Columns.Count - 1
            Dim value = dtSource.Columns(i).Caption
            worksheet.Cell(1, i + 1).Value = value
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue
            worksheet.Cell(1, i + 1).Style.Font.Bold = True
        Next

        For i As Integer = 0 To dtSource.Rows.Count - 1
            For j As Integer = 0 To dtSource.Columns.Count - 1
                Dim value = TypeConverter.TryConvert(dtSource.Rows(i).Field(Of String)(j), ci)
                worksheet.Cell(i + 2, j + 1).Value = value

                If value.GetType() = GetType(Double) Then
                    If value Mod 1 > 0 Then
                        'Console.WriteLine("El modulo es " & (value Mod 1).ToString())
                        worksheet.Cell(i + 2, j + 1).Style.NumberFormat.Format = "0.00"
                    ElseIf value Mod 1 = 0 Then
                        worksheet.Cell(i + 2, j + 1).Style.NumberFormat.Format = "0"
                    End If
                End If

                If value.GetType() = GetType(DateTime) Then
                    worksheet.Cell(i + 2, j + 1).Style.DateFormat.Format = "dd/MM/yyyy"
                End If

            Next
        Next
        worksheet.Columns().AdjustToContents()
    End Sub

    Private Async Function VerificarDatosAmbiguos(ByVal grupo As GrupoConciliaciones) As Task(Of Boolean)
        'NUEVO AGREGADO ISMAEL 230718
        bandera_conflic = bandera_conflic + 1


        Dim dtECConcil As DataTable = Nothing

        If InvokeRequired Then
            Invoke(Sub()
                       dtECConcil = GridsReportesTabs("_CONC.REP.BCD." & grupo.Nombre).DataSource
                   End Sub)
        Else
            dtECConcil = GridsReportesTabs("_CONC.REP.BCD." & grupo.Nombre).DataSource
        End If

        Dim dtICaavConcil As DataTable = Nothing

        If InvokeRequired Then
            Invoke(Sub()
                       dtICaavConcil = GridsReportesTabs("_CONC.REP.EDOCTA." & grupo.Nombre).DataSource
                   End Sub)
        Else
            dtICaavConcil = GridsReportesTabs("_CONC.REP.EDOCTA." & grupo.Nombre).DataSource
        End If

        Dim dtEnumECConcil = dtECConcil.AsEnumerable()
        Dim dtEnumICConcil = dtICaavConcil.AsEnumerable()

        Dim ECConciliacionNumRows = dtECConcil.Rows.Count
        Dim ICConciliacionNumRows = dtICaavConcil.Rows.Count
        Dim ListaFilasAmbiguas = New List(Of DataRow)


        'nuevo agregado ismael 230718
        Dim band_r As Integer = 0


        'If Me.InvokeRequired Then
        '    Invoke(Sub()
        '               ToolStripStatusLabel1.Text = "VERIFICANDO ..."
        '               ToolStripProgressBar1.Visible = True
        '           End Sub)
        'Else
        '    ToolStripStatusLabel1.Text = "VERIFICANDO ..."
        '    ToolStripProgressBar1.Visible = True
        'End If

        ListaFilasAmbiguas.Clear()
        DictRowsConciliaciones.Clear()

        If ECConciliacionNumRows >= ICConciliacionNumRows Then

            Dim dtICConflict As DataTable = dtICaavConcil.Clone()

            If dtICConflict IsNot Nothing Then
                dtICConflict.Clear()
            End If

            'nuevo agregado siamel 230718
            If bandera_conflic = 1 Then
                dtICConflict_1 = dtICaavConcil.Clone()
            End If

            Dim globalData = New GlobalECDataRow() 'With {.rec = ecrow, .DatosIcaav = dtEnumICConcil}
            Dim query = String.Format("DatosIcaav.Where((ric) => {0}).ToList()", condiciones).ToString()

            Dim options = ScriptOptions.Default.WithReferences({GetType(System.Linq.Enumerable).Assembly, GetType(System.Data.DataRowExtensions).Assembly, GetType(System.Convert).Assembly, GetType(NumberStyles).Assembly}).WithImports("System", "System.Linq", "System.Data.DataRowExtensions", "System.Globalization")
            Dim script1 = CSharpScript.Create(Of IEnumerable)(
                    query,
                    options,
                    GetType(GlobalECDataRow))

            script1.Compile()


            If dtICConflict_1 IsNot Nothing Then
                dtICConflict_1.Clear()
            End If

            For Each ecrow In dtECConcil.Rows
                Dim resultado_reporte = Await script1.RunAsync(New GlobalECDataRow() With {.rec = ecrow, .DatosIcaav = dtEnumICConcil})
                Dim returned_value As IEnumerable(Of DataRow) = resultado_reporte.ReturnValue
                If returned_value.Count > 1 Then
                    For Each icrowconflictiva In returned_value
                        dtICConflict.ImportRow(icrowconflictiva)

                        'nuevo agrgado ismael 230718
                        dtICConflict_1.ImportRow(icrowconflictiva)

                    Next
                    ListaFilasAmbiguas.Add(ecrow)
                Else
                    Dim resrow = returned_value.FirstOrDefault()
                    If resrow IsNot Nothing Then
                        DictRowsConciliaciones.Add(ecrow, resrow)
                    End If
                End If
            Next

            If dtICConflict IsNot Nothing Then
                dtICResultEnum = dtICConflict.DefaultView.ToTable(True)
            End If

            If dtICConflict_1 IsNot Nothing Then
                dtICResultEnum_2 = dtICConflict_1.DefaultView.ToTable(True) 'nuevo agrgado ismael 230718
            End If


            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               GridsReportesTabs("_CONFLICT.REP.EDOCTA." & grupo.Nombre).DataSource = dtICResultEnum

            '               'nuevo agrgado ismael 230718
            '               GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = ""
            '               GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = dtICResultEnum_2

            '           End Sub)
            'Else
            '    GridsReportesTabs("_CONFLICT.REP.EDOCTA." & grupo.Nombre).DataSource = dtICResultEnum

            '    'NUEVO AGRGADO ISMAEL 230718
            '    GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = ""
            '    GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = dtICResultEnum_2

            'End If

            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REP.EDOCTA." & grupo.Nombre).DataSource = dtICResultEnum
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REP.EDOCTA." & grupo.Nombre).DataSource = dtICResultEnum
            'End If

            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'nuevo agrgado ismael 230718
            '               'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = ""
            '           End Sub)
            'Else
            '    'NUEVO AGRGADO ISMAEL 230718
            '    'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = ""
            'End If

            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = dtICResultEnum_2

            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = dtICResultEnum_2

            'End If


            Dim dtECConflictos As DataTable = dtECConcil.Clone()

            If dtECConflictos IsNot Nothing Then
                dtECConflictos.Clear()
            End If


            'nuevo agregado siamel 230718
            If bandera_conflic = 1 Then
                If dtECConflictos_1 IsNot Nothing Then
                    dtECConflictos_1 = dtECConcil.Clone()
                End If
            End If
            If dtECConflictos_1 IsNot Nothing Then
                dtECConflictos_1.Clear()
            End If

            For Each row In ListaFilasAmbiguas
                'For Each column As DataColumn In dtECConcil.Columns
                'Next
                If dtECConflictos IsNot Nothing Then
                    dtECConflictos.ImportRow(row)
                End If

                If dtECConflictos_1 IsNot Nothing Then
                    dtECConflictos_1.ImportRow(row) 'NUEVO AGREGADO ISMAEL 230718
                End If

            Next

            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REP.BCD." & grupo.Nombre).DataSource = dtECConflictos
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REP.BCD." & grupo.Nombre).DataSource = dtECConflictos
            'End If

            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = ""
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = ""
            'End If

            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = dtECConflictos_1
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = dtECConflictos_1
            'End If

            ' segunda vuelta ec
            If dtECNoEncontrados IsNot Nothing And dtECConflictos IsNot Nothing Then
                Dim dtTemp As DataTable = dtECNoEncontrados.Copy()
                dtTemp.Merge(dtECConflictos)
                dtEnumEC = dtTemp.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
            End If

            If dtECNoEncontrados Is Nothing And dtECConflictos IsNot Nothing Then
                dtEnumEC = dtECConflictos.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
            End If

            If dtECNoEncontrados IsNot Nothing And dtECConflictos Is Nothing Then
                dtEnumEC = dtECNoEncontrados.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
            End If

            ' segunda vuelta icaav
            If dtIcaavNoEncontrados IsNot Nothing And dtICConflict IsNot Nothing Then
                Dim dtTemp As DataTable = dtIcaavNoEncontrados.Copy()
                dtTemp.Merge(dtICConflict)
                dtEnumIC = dtTemp.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
            End If

            If dtIcaavNoEncontrados Is Nothing And dtICConflict IsNot Nothing Then
                dtEnumIC = dtICConflict.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
            End If

            If dtIcaavNoEncontrados IsNot Nothing And dtICConflict Is Nothing Then
                dtEnumIC = dtIcaavNoEncontrados.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
            End If

            If ListaFilasAmbiguas.Count > 0 Then
                Return False
            End If
            Return True

        Else

            Dim dtECConflict As DataTable = dtECConcil.Clone()
            dtECConflict.Clear()

            'NUEVO AGRGADO ISMAEL 230718
            If bandera_conflic = 1 Then
                dtECConflict_1 = dtECConcil.Clone()
            End If


            Dim globalData = New GlobalICDataRow() ''With {.ric = icrow, .DatosEC = dtEnumECConcil}
            Dim query = String.Format("DatosEC.Where((rec) => {0}).ToList()", condiciones).ToString()

            Dim options = ScriptOptions.Default.WithReferences({GetType(System.Linq.Enumerable).Assembly, GetType(System.Data.DataRowExtensions).Assembly, GetType(System.Convert).Assembly, GetType(NumberStyles).Assembly}).WithImports("System", "System.Linq", "System.Data.DataRowExtensions", "System.Globalization")
            Dim script2 = CSharpScript.Create(Of IEnumerable)(
                    query,
                    options,
                    GetType(GlobalICDataRow))

            script2.Compile()

            If dtECConflict_1 IsNot Nothing Then
                dtECConflict_1.Clear()
            End If

            For Each icrow In dtICaavConcil.Rows
                    Dim resultado_reporte = Await script2.RunAsync(New GlobalICDataRow() With {.ric = icrow, .DatosEC = dtEnumECConcil})
                    Dim returned_value As IEnumerable(Of DataRow) = resultado_reporte.ReturnValue
                If returned_value.Count > 1 Then
                    For Each ecrowconflictiva In returned_value
                        dtECConflict.ImportRow(ecrowconflictiva)

                        'nuevo agrgado ismael 2300718
                        dtECConflict_1.ImportRow(ecrowconflictiva)
                    Next
                    ListaFilasAmbiguas.Add(icrow)
                Else
                    Dim ValueRet As IEnumerable(Of DataRow) = returned_value
                    If ValueRet IsNot Nothing And ValueRet.Count > 0 Then
                        Dim resrow = ValueRet.First()
                        DictRowsConciliaciones.Add(icrow, resrow)
                    End If
                End If
            Next

                dtECResultEnum = dtECConflict.DefaultView.ToTable(True)

                dtECResultEnum_2 = dtECConflict_1.DefaultView.ToTable(True) 'nuevo agrgado ismael 230718



            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REP.BCD." & grupo.Nombre).DataSource = dtECResultEnum
            '           End Sub)
            'Else


            '    'GridsReportesTabs("_CONFLICT.REP.BCD." & grupo.Nombre).DataSource = dtECResultEnum
            'End If


            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = ""
            '           End Sub)
            'Else
            '    'nuevo agrgado ismael 2300718
            '    'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = ""
            'End If


            'If Me.InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = dtECResultEnum_2
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REPORT.BCD.").DataSource = dtECResultEnum_2
            'End If


            Dim dtIcaavConflictos As DataTable = dtICaavConcil.Clone()
            If dtIcaavConflictos IsNot Nothing Then
                dtIcaavConflictos.Clear()
            End If

            'NUEVO AGRGADO ISMAEL 230718
            If bandera_conflic = 1 Then
                dtIcaavConflictos_1 = dtICaavConcil.Clone()
            End If

            If dtIcaavConflictos_1 IsNot Nothing Then
                dtIcaavConflictos_1.Clear()
            End If

            For Each row In ListaFilasAmbiguas
                dtIcaavConflictos.ImportRow(row)
                If dtIcaavConflictos_1 IsNot Nothing Then
                    'NUEVO AGRGADO ISMAEL 230718
                    dtIcaavConflictos_1.ImportRow(row)
                End If
            Next

            'If InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REP.EDOCTA." & grupo.Nombre).DataSource = dtIcaavConflictos
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REP.EDOCTA." & grupo.Nombre).DataSource = dtIcaavConflictos
            'End If

            'If InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = ""
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = ""
            'End If

            'If InvokeRequired Then
            '    Invoke(Sub()
            '               'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = dtIcaavConflictos_1
            '           End Sub)
            'Else
            '    'GridsReportesTabs("_CONFLICT.REPORT.EDOCTA.").DataSource = dtIcaavConflictos_1
            'End If






            ' NUEVA VUELTA DE CONCILIACION EC
            If dtECConflict IsNot Nothing And dtECNoEncontrados IsNot Nothing Then
                    Dim dtTemp As DataTable = dtECNoEncontrados.Copy()
                    dtTemp.Merge(dtECConflict)
                    dtEnumEC = dtTemp.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
                End If

                If dtECConflict Is Nothing And dtECNoEncontrados IsNot Nothing Then
                    dtEnumEC = dtECNoEncontrados.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
                End If

                If dtECConflict IsNot Nothing And dtECNoEncontrados Is Nothing Then
                    dtEnumEC = dtECConflict.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
                End If

                ' NUEVA VUELTA DE CONCILIACION IC
                If dtIcaavNoEncontrados IsNot Nothing And dtIcaavConflictos IsNot Nothing Then
                    Dim dtTemp As DataTable = dtIcaavNoEncontrados.Copy()
                    dtTemp.Merge(dtIcaavConflictos)
                    dtEnumIC = dtTemp.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
                End If

                If dtIcaavNoEncontrados Is Nothing And dtIcaavConflictos IsNot Nothing Then
                    dtEnumIC = dtIcaavConflictos.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
                End If

                If dtIcaavNoEncontrados IsNot Nothing And dtIcaavConflictos Is Nothing Then
                    dtEnumIC = dtIcaavNoEncontrados.DefaultView.ToTable(True).Rows.Cast(Of DataRow).ToList()
                End If

                If ListaFilasAmbiguas.Count > 0 Then
                    Return False
                End If
                Return True


            End If



    End Function

    Private Sub OnDatosConciliacionProcesadosCorrectamente(sender As Object, e As EventArgs)
    End Sub



    Private Sub btnCrearConciliacion_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub OnConciliacionCreadaHandler(sender As Object, e As CrearConciliacionEventArgs)
        ListaConciliaciones.Add(New Conciliacion(e.CampoEstadoDeCuenta, e.CampoIcaav, e.TipoDatos, e.Operador, e.Grupo))
        btn_procesar.Enabled = True
    End Sub

    Private Sub ConciliacionAmex_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListaConciliaciones = New List(Of Conciliacion)()
        ListaGruposConciliaciones = New BindingList(Of GrupoConciliaciones)()
        AddHandler _reporteEstadoDeCuentaLoaded, AddressOf OnReporteEstadoDeCuentaLoaded
        AddHandler _reporteIcaavLoaded, AddressOf OnReporteIcaavLoaded
        AddHandler ProcesoConciliacionIniciado, AddressOf OnProcesoConciliacionIniciado
        AddHandler ProcesoConciliacionTerminado, AddressOf OnProcesoConciliacionTerminado
        AddHandler DatosConciliacionProcesadosCorrectamente, AddressOf OnDatosConciliacionProcesadosCorrectamente
        AddHandler ProgressSaveFileChanged, AddressOf OnProgressSaveFileChanged
        AddHandler ProgressSaveFileStart, AddressOf OnProgressSaveFileStart
        AddHandler ProgressSaveFileEnd, AddressOf OnProgressSaveFileEnd
        AddHandler AgregarNuevoGrupoConciliacion, AddressOf OnAgregarNuevoGrupoConciliacion
        lbxGruposConciliacion.DataSource = ListaGruposConciliaciones
        ReportesTabs = New Dictionary(Of String, TabPage)
        GridsReportesTabs = New Dictionary(Of String, DataGridView)
        AplicarEstiloAGrid(GridEdoCuenta)
        AplicarEstiloAGrid(GridIcaav)
        Me.Text = "CONCILIADOR GENERAL BCD 03/01/2019 14:46 pm"
    End Sub

    Private Sub OnAgregarNuevoGrupoConciliacion(sender As Object, e As AgregarNuevoGrupoConciliacionArgs)
        lbxGruposConciliacion.DataSource = Nothing
        ListaGruposConciliaciones.Add(e.GrupoConciliaciones)
        lbxGruposConciliacion.DataSource = ListaGruposConciliaciones
        If lbxGruposConciliacion.Items.Count > 0 Then
            lbxGruposConciliacion.SelectedIndex = -1
            lbxGruposConciliacion.SelectedIndex = 0
        End If
    End Sub

    Private Sub OnProgressSaveFileEnd(sender As Object, e As EventArgs)
        If Me.InvokeRequired Then
            Invoke(Sub()
                       ToolStripStatusLabel1.Text = "LISTO"
                       ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
                       ToolStripProgressBar1.Visible = False
                   End Sub)
        Else
            ToolStripStatusLabel1.Text = "LISTO"
            ToolStripProgressBar1.Style = ProgressBarStyle.Marquee
            ToolStripProgressBar1.Visible = False
        End If

    End Sub

    Private Sub OnProgressSaveFileStart(sender As Object, e As EventArgs)
        If Me.InvokeRequired Then
            Invoke(Sub()
                       ToolStripStatusLabel1.Text = "Generando archivo Excel..."
                       ToolStripProgressBar1.Style = ProgressBarStyle.Blocks
                       ToolStripProgressBar1.Visible = True

                   End Sub)
        Else
            ToolStripStatusLabel1.Text = "Generando archivo Excel..."
            ToolStripProgressBar1.Style = ProgressBarStyle.Blocks
            ToolStripProgressBar1.Visible = True
        End If

    End Sub

    Private Sub OnProgressSaveFileChanged(sender As Object, e As ProgressSaveFileChangedEventArgs)
        If Me.InvokeRequired Then
            Invoke(Sub()
                       ToolStripProgressBar1.Value = e.Progreso
                   End Sub
    )
        Else
            ToolStripProgressBar1.Value = e.Progreso
        End If

    End Sub

    Private Async Sub OnProcesoConciliacionTerminado(sender As Object, e As ProcesoConciliacionTerminadoArgs)
        Dim resultado = Await Task.Factory.StartNew(Async Function()
                                                        Return Await VerificarDatosAmbiguos(e.Grupo)
                                                    End Function)
        If Me.InvokeRequired Then
            Invoke(Sub()
                       ToolStripStatusLabel1.Text = "LISTO"
                       ToolStripProgressBar1.Visible = False
                       btn_edo_cuenta.Enabled = True
                       cmb_hoja_edo.Enabled = True
                       btn_icaav.Enabled = True
                       cmb_hoja_icaav.Enabled = True
                       llenarcheckbox()
                   End Sub)
        Else
            ToolStripStatusLabel1.Text = "LISTO"
            ToolStripProgressBar1.Visible = False
            btn_edo_cuenta.Enabled = True
            cmb_hoja_edo.Enabled = True
            btn_icaav.Enabled = True
            cmb_hoja_icaav.Enabled = True
            llenarcheckbox()
        End If

        If resultado.Result = True Then
            If GridsReportesTabs("_CONC." & e.Grupo.Nombre).RowCount = 0 Then
                If MessageBox.Show("NO SE OBTUVO NINGUN RESULTADO CON LAS CONDICIONES SELECCIONADAS, POR FAVOR VERIFIQUE QUE LOS CAMPOS SELECCIONADOS Y LOS TIPOS DE DATOS DE LAS CONDICIONES SON CORRECTOS.", "NO SE OBTUVIERON RESULTADOS.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) = DialogResult.OK Then
                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   btn_procesar.Enabled = True
                                   btn_exportar.Enabled = True
                               End Sub)
                    Else
                        btn_procesar.Enabled = True
                        btn_exportar.Enabled = True
                    End If
                End If
            Else
                If MessageBox.Show("DATOS CONCILIADOS CORRECTAMENTE, AHORA PUEDE VER LOS RESULTADOS DE LA CONCILIACIÓN O EXPORTARLOS A UN ARCHIVO DE EXCEL.", "PROCESO DE CONCILIACIÓN TERMINADO.", MessageBoxButtons.OK, MessageBoxIcon.Information) = DialogResult.OK Then
                    If Me.InvokeRequired Then
                        Invoke(Sub()
                                   TabNavegacion.SelectedTab = TabPage6
                                   btn_procesar.Enabled = True
                                   ConflictosEnConciliacion = False
                                   btn_exportar.Enabled = True
                               End Sub)
                    Else
                        TabNavegacion.SelectedTab = TabPage6
                        btn_procesar.Enabled = True
                        ConflictosEnConciliacion = False
                        btn_exportar.Enabled = True
                    End If

                End If
            End If
        Else
            MessageBox.Show("NO TODOS LOS ELEMENTOS HAN PODIDO SER CONCILIADOS, AGREGUE OTRO GRUPO DE CONCILIACIÓN Y DIFERENTES CONDICIONES PARA CONCILIAR LOS DATOS RESTANTES.", "CONCILIACIÓN COMPLETADA PARCIALMENTE.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            If Me.InvokeRequired Then
                Invoke(Sub()
                           btn_procesar.Enabled = True
                           ConflictosEnConciliacion = True
                           btn_exportar.Enabled = True
                           'TabNavegacion.SelectedTab = TabPage10
                       End Sub)
            Else
                btn_procesar.Enabled = True
                ConflictosEnConciliacion = True
                btn_exportar.Enabled = True
                'TabNavegacion.SelectedTab = TabPage10
            End If

        End If

    End Sub

    Private Sub OnProcesoConciliacionIniciado(sender As Object, e As EventArgs)
        ToolStripStatusLabel1.Text = "PROCESANDO ..."
        ToolStripProgressBar1.Visible = True
        btn_edo_cuenta.Enabled = False
        cmb_hoja_edo.Enabled = False
        btn_icaav.Enabled = False
        cmb_hoja_icaav.Enabled = False
        btn_procesar.Enabled = False
        btn_exportar.Enabled = False
    End Sub

    Private Sub OnReporteEstadoDeCuentaLoaded(sender As Object, e As EventArgs)
        ExcelEstadoDeCuentaLoaded = True
        If ExcelIcaavLoaded = True Then
            'btnCrearConciliacion.Enabled = True
        End If
    End Sub

    Private Sub OnReporteIcaavLoaded(sender As Object, e As EventArgs)
        ExcelIcaavLoaded = True
        If ExcelEstadoDeCuentaLoaded = True Then
            btnAgregarNuevoGrupo.Enabled = True
        End If
    End Sub

    Private Sub clbEstadoDeCuenta_ItemCheck(sender As Object, e As ItemCheckEventArgs)
        Dim clBox = DirectCast(sender, CheckedListBox)
        For ix = 0 To clBox.Items.Count - 1
            If ix <> e.Index Then
                clBox.SetItemChecked(ix, False)
            End If
        Next
    End Sub

    Private Sub clbIcaav_ItemCheck(sender As Object, e As ItemCheckEventArgs)
        Dim clBox = DirectCast(sender, CheckedListBox)
        For ix = 0 To clBox.Items.Count - 1
            If ix <> e.Index Then
                clBox.SetItemChecked(ix, False)
            End If
        Next
    End Sub

    Private Sub btnEliminarConciliacion_Click(sender As Object, e As EventArgs)
        Dim _button = DirectCast(sender, Button)
        'Dim selectedLbxIndex = lbxConciliaciones.SelectedIndex
        'If selectedLbxIndex <> -1 Then
        'ListaConciliaciones.RemoveAt(selectedLbxIndex)
        'lbxConciliaciones.Items.RemoveAt(selectedLbxIndex)
        'If lbxConciliaciones.Items.Count < 1 Then
        '_button.Enabled = False
        'btn_procesar.Enabled = False
        'End If
        ' Else
        'MessageBox.Show("No esta seleccionada ninguna conciliación.", "Conciliación no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'End If
    End Sub

    Private Sub GridEdoCuenta_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles GridEdoCuenta.RowPostPaint
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub GridIcaav_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles GridIcaav.RowPostPaint
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub GridConciliacion_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub GridIcaavConcil_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub GridNoEncontrados_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub DataGridNoIcaav_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub gridReporteConciliacion_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub gridConflictosEC_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub gridConflictosIcaav_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        DrawRowNumbers(sender, e)
    End Sub

    Private Sub btnAgregarNuevoGrupo_Click(sender As Object, e As EventArgs) Handles btnAgregarNuevoGrupo.Click
        Dim agregarNuevoGrupoFrm As AgregarNuevoGrupo = New AgregarNuevoGrupo(ListaGruposConciliaciones)
        AddHandler agregarNuevoGrupoFrm.AgregarNuevoGrupo, AddressOf OnAgregarGrupoConciliacion
        agregarNuevoGrupoFrm.ShowDialog()
    End Sub

    Private Sub OnAgregarGrupoConciliacion(sender As Object, e As AgregarNuevoGrupoConciliacionArgs)
        RaiseEvent AgregarNuevoGrupoConciliacion(Me, e)
    End Sub

    Private Sub btnEditarGrupo_Click(sender As Object, e As EventArgs) Handles btnEditarGrupo.Click
        Dim _GrupoConciliacionSeleccionado As GrupoConciliaciones = DirectCast(lbxGruposConciliacion.SelectedItem, GrupoConciliaciones)
        Dim _ModificarGrupoConciliaciones As ModificarGrupoConciliaciones = New ModificarGrupoConciliaciones(_GrupoConciliacionSeleccionado)
        AddHandler _ModificarGrupoConciliaciones.ModificarNombreGrupoEvent, AddressOf OnModificarNombreGrupo
        _ModificarGrupoConciliaciones.ShowDialog()

    End Sub

    Private Sub OnModificarNombreGrupo(sender As Object, e As AgregarNuevoGrupoConciliacionArgs)
        Dim IndexGrupoConciliacion = ListaGruposConciliaciones.IndexOf(CType(lbxGruposConciliacion.SelectedItem, GrupoConciliaciones))
        Dim GrupoConciliacion = ListaGruposConciliaciones.ElementAt(IndexGrupoConciliacion)
        GrupoConciliacion.Nombre = e.GrupoConciliaciones.Nombre
    End Sub

    Private Sub lbxGruposConciliacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbxGruposConciliacion.SelectedIndexChanged
        Dim listBox As ListBox = DirectCast(sender, ListBox)
        If listBox.Items.Count > 0 Then
            btnEditarGrupo.Enabled = True
            btnEliminarGrupo.Enabled = True
            btnAgregarNuevaConciliaciion.Enabled = True
            btnReiniciarGrupo.Enabled = True
        Else
            btnEditarGrupo.Enabled = False
            btnEliminarGrupo.Enabled = False
            btnAgregarNuevaConciliaciion.Enabled = False
            btnReiniciarGrupo.Enabled = False
        End If

        Dim GrupoSeleccionado As GrupoConciliaciones = CType(listBox.SelectedItem, GrupoConciliaciones)

        If listBox.SelectedIndex <> -1 Then
            lbxConciliacionesDeGrupo.DataSource = Nothing
            lbxConciliacionesDeGrupo.DataSource = GrupoSeleccionado.ListaConciliaciones

            ''If GrupoSeleccionado.ListaConciliaciones.Count > 0 Then
            ''lbxConciliacionesDeGrupo.SelectedIndex = -1
            ''lbxConciliacionesDeGrupo.SelectedIndex = 0
            ''End If
        End If
    End Sub

    Private Sub btnAgregarNuevaConciliaciion_Click(sender As Object, e As EventArgs) Handles btnAgregarNuevaConciliaciion.Click
        Dim ListaColumnasPrimerReporte = New List(Of String)
        Dim ListaColumnasSegundoReporte = New List(Of String)

        ListaColumnasPrimerReporte.Clear()
        ListaColumnasSegundoReporte.Clear()

        For Each columna As DataGridViewColumn In GridEdoCuenta.Columns
            ListaColumnasPrimerReporte.Add(columna.Name)
        Next

        For Each columna As DataGridViewColumn In GridIcaav.Columns
            ListaColumnasSegundoReporte.Add(columna.Name)
        Next
        Dim grupoSeleccionado As GrupoConciliaciones = CType(lbxGruposConciliacion.SelectedItem, GrupoConciliaciones)
        Dim agregarConcFrm As AgregarNuevaConciliacion = New AgregarNuevaConciliacion(ListaColumnasPrimerReporte, ListaColumnasSegundoReporte, ListaGruposConciliaciones, grupoSeleccionado)
        AddHandler agregarConcFrm.AgregarConciliacionAGrupo, AddressOf OnAgregarConciliacionAGrupo
        agregarConcFrm.ShowDialog()
    End Sub

    Private Sub OnAgregarConciliacionAGrupo(sender As Object, e As CrearConciliacionEventArgs)
        Dim _Conciliacion = New Conciliacion(e.CampoEstadoDeCuenta, e.CampoIcaav, e.TipoDatos, e.Operador, e.Grupo)
        Dim _GrupoConciliacion = ListaGruposConciliaciones.FirstOrDefault(Function(x) x.Nombre = e.Grupo.Nombre)
        If _GrupoConciliacion IsNot Nothing Then
            _GrupoConciliacion.AgregarConciliacionAGrupo(_Conciliacion)
        End If
        btn_procesar.Enabled = True
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub TabNavegacion_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabNavegacion.DrawItem
        Using br As New SolidBrush(Color.LightSalmon)
            e.Graphics.FillRectangle(br, e.Bounds)
            Dim sz = e.Graphics.MeasureString(TabNavegacion.TabPages(e.Index).Text, e.Font)
            e.Graphics.DrawString(TabNavegacion.TabPages(e.Index).Text, e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 2, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1)
            Dim rect As Rectangle = e.Bounds
            rect.Offset(0, 1)
            rect.Inflate(0, -1)
            e.Graphics.DrawRectangle(Pens.DarkGray, rect)
            e.DrawFocusRectangle()
        End Using
    End Sub

    Private Sub GridEdoCuenta_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        cmsDataConverter.Show(Control.MousePosition)

    End Sub

    Private Sub CheckedListBox3_ItemCheck(sender As Object, e As ItemCheckEventArgs)

    End Sub

    Private Sub chkSelectAllConc1_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAllConc1.CheckedChanged
        Dim chkBox As CheckBox = DirectCast(sender, CheckBox)

        If chkBox.Checked Then
            For i = 0 To CheckedListBox3.Items.Count - 1
                CheckedListBox3.SetItemChecked(i, True)
            Next
        Else
            For i = 0 To CheckedListBox3.Items.Count - 1
                CheckedListBox3.SetItemChecked(i, False)
            Next
        End If
    End Sub

    Private Sub chkSelectAllConc2_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAllConc2.CheckedChanged
        Dim chkBox As CheckBox = DirectCast(sender, CheckBox)

        If chkBox.Checked Then
            For i = 0 To CheckedListBox4.Items.Count - 1
                CheckedListBox4.SetItemChecked(i, True)
            Next
        Else
            For i = 0 To CheckedListBox4.Items.Count - 1
                CheckedListBox4.SetItemChecked(i, False)
            Next
        End If
    End Sub

    Private Sub btnEliminarGrupo_Click(sender As Object, e As EventArgs) Handles btnEliminarGrupo.Click
        Dim GrupoSeleccionado = lbxGruposConciliacion.SelectedItem
        If GrupoSeleccionado IsNot Nothing Then
            ListaGruposConciliaciones.Remove(GrupoSeleccionado)
            If ListaGruposConciliaciones.Count = 0 Then
                btnEditarGrupo.Enabled = False
                btnEliminarGrupo.Enabled = False
                btnAgregarNuevaConciliaciion.Enabled = False
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ConciliacionSeleccionada As Conciliacion = CType(lbxConciliacionesDeGrupo.SelectedItem, Conciliacion)
        Dim GrupoSeleccionado As GrupoConciliaciones = CType(lbxGruposConciliacion.SelectedItem, GrupoConciliaciones)
        If GrupoSeleccionado.ListaConciliaciones.Contains(ConciliacionSeleccionada) Then
            GrupoSeleccionado.EliminarConciliacion(ConciliacionSeleccionada)
        End If

        If GrupoSeleccionado.ListaConciliaciones.Count < 1 Then
            Button2.Enabled = False
            Button3.Enabled = False
            btn_procesar.Enabled = False
        End If
    End Sub

    Private Sub lbxConciliacionesDeGrupo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbxConciliacionesDeGrupo.SelectedIndexChanged
        Dim listBox As ListBox = CType(sender, ListBox)
        If listBox.SelectedIndex <> -1 Then
            Button2.Enabled = True
            Button3.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ListaColumnasPrimerReporte = New List(Of String)
        Dim ListaColumnasSegundoReporte = New List(Of String)

        ListaColumnasPrimerReporte.Clear()
        ListaColumnasSegundoReporte.Clear()

        For Each columna As DataGridViewColumn In GridEdoCuenta.Columns
            ListaColumnasPrimerReporte.Add(columna.Name)
        Next

        For Each columna As DataGridViewColumn In GridIcaav.Columns
            ListaColumnasSegundoReporte.Add(columna.Name)
        Next
        Dim grupoSeleccionado As GrupoConciliaciones = CType(lbxGruposConciliacion.SelectedItem, GrupoConciliaciones)
        Dim concilSeleccionada As Conciliacion = CType(lbxConciliacionesDeGrupo.SelectedItem, Conciliacion)
        Dim columnaConcil1 As String = concilSeleccionada.CampoEstadoDeCuenta
        Dim columnaConcil2 As String = concilSeleccionada.CampoIcaav
        Dim modConcilFrm As ModificarNuevaConciliacion = New ModificarNuevaConciliacion(ListaColumnasPrimerReporte, ListaColumnasSegundoReporte, ListaGruposConciliaciones, grupoSeleccionado, concilSeleccionada.CampoEstadoDeCuenta, concilSeleccionada.CampoIcaav, concilSeleccionada.TipoDeDatos, concilSeleccionada.Operador)
        AddHandler modConcilFrm.ModificarConciliacionDeGrupo, AddressOf OnConciliacionModificada
        modConcilFrm.ShowDialog()
    End Sub

    Private Sub OnConciliacionModificada(sender As Object, e As CrearConciliacionEventArgs)
        Dim ConcilNueva As Conciliacion = New Conciliacion(e.CampoEstadoDeCuenta, e.CampoIcaav, e.TipoDatos, e.Operador, e.Grupo)
        Dim ConcilSelected As Conciliacion = CType(lbxConciliacionesDeGrupo.SelectedItem, Conciliacion)
        Dim GrupoSelected As GrupoConciliaciones = CType(lbxGruposConciliacion.SelectedItem, GrupoConciliaciones)
        GrupoSelected.ModificarConciliacionAGrupo(ConcilSelected, ConcilNueva)
    End Sub

    Private Sub menuNuevaConciliacion_Click(sender As Object, e As EventArgs) Handles menuNuevaConciliacion.Click
        RaiseEvent ReiniciarFormularioConciliacion(Me, EventArgs.Empty)
    End Sub

    Private Sub menuSalir_Click(sender As Object, e As EventArgs) Handles menuSalir.Click
        Me.Close()
    End Sub

    Private Sub menuAmexToPDF_Click(sender As Object, e As EventArgs) Handles menuAmexToPDF.Click
        Dim amexToExcelFrm As AmexToExcel = New AmexToExcel()
        amexToExcelFrm.ShowDialog()
    End Sub

    Private Sub menuAcercaDe_Click(sender As Object, e As EventArgs) Handles menuAcercaDe.Click
        Dim AcercaDeFrm = New AcercaDe()
        AcercaDeFrm.ShowDialog()
    End Sub

    Private Sub btnReiniciarGrupo_Click(sender As Object, e As EventArgs) Handles btnReiniciarGrupo.Click
        If lbxGruposConciliacion.SelectedItem IsNot Nothing Then
            Dim grupoSeleccionado As GrupoConciliaciones = lbxGruposConciliacion.SelectedItem
            grupoSeleccionado.YaProcesado = False
        End If
    End Sub

    Private Sub cmb_hoja_edo_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmb_hoja_edo.SelectionChangeCommitted
        Dim cbox As ComboBox = CType(sender, ComboBox)
        Dim DtWithBlankRows As DataTable = GetDataTableFromExcel(OpenFileDialog1.FileName, cmb_hoja_edo.SelectedIndex)
        Dim DtWithoutBlankRows As DataTable = RemoveEmptyRows(DtWithBlankRows)
        Dim DtWithoutBlankColumns As DataTable = RemoveBlankColumns(DtWithoutBlankRows)
        GridEdoCuenta.DataSource = DtWithoutBlankColumns
        TabNavegacion.SelectedIndex = 0
        ''LLenarGrid(txt_excel_edo, cmb_hoja_edo, GridEdoCuenta)
        btn_icaav.Enabled = True
        RaiseEvent _reporteEstadoDeCuentaLoaded(Me, EventArgs.Empty)
    End Sub

    Private Sub cmb_hoja_icaav_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmb_hoja_icaav.SelectionChangeCommitted
        Dim cbox As ComboBox = CType(sender, ComboBox)
        Dim DtWithBlankRows As DataTable = GetDataTableFromExcel(OpenFileDialog2.FileName, cmb_hoja_icaav.SelectedIndex)
        Dim DtWithoutBlankRows As DataTable = RemoveEmptyRows(DtWithBlankRows)
        Dim DtWithoutBlankColumns As DataTable = RemoveBlankColumns(DtWithoutBlankRows)
        GridIcaav.DataSource = DtWithoutBlankColumns

        TabNavegacion.SelectedIndex = 1
        ''LLenarGrid(txt_excel_icaav, cmb_hoja_icaav, GridIcaav)
        'eliminarColumnasVacias(GridIcaav)
        llenarcheckbox()
        lbxGruposConciliacion.DataSource = Nothing
        ListaGruposConciliaciones.Clear()
        ListaGruposConciliaciones.Add(New GrupoConciliaciones() With {.Nombre = "G1"})
        lbxGruposConciliacion.DataSource = ListaGruposConciliaciones
        TabNavegacion.SelectedIndex = 2
        RaiseEvent _reporteIcaavLoaded(Me, EventArgs.Empty)
    End Sub

    Private Sub txt_excel_edo_TextChanged(sender As Object, e As EventArgs) Handles txt_excel_edo.TextChanged

    End Sub

    Private Sub cmb_hoja_edo_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cmb_hoja_edo.SelectedIndexChanged

    End Sub

    Private Sub GridEdoCuenta_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridEdoCuenta.CellContentClick

    End Sub


End Class

Public Class GlobalData
    Public DatosEstadoDeCuenta As List(Of DataRow)
    Public DatosIcaav As List(Of DataRow)
End Class

Public Class GlobalECDataRow
    Public rec As System.Data.DataRow
    Public DatosIcaav As System.Data.EnumerableRowCollection(Of DataRow)
End Class

Public Class GlobalICDataRow
    Public ric As System.Data.DataRow
    Public DatosEC As System.Data.EnumerableRowCollection(Of DataRow)
End Class
