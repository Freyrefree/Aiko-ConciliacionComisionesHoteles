Imports System.Configuration
Imports System.Text
Imports ClosedXML.Excel

Public Class GeneradorReporte
    Private _wb As IXLWorkbook
    Private _filePath As String
    Private Shared conciliacionesProvRepository As conciliacionesProveedores

    Public Sub New(ByVal filePath As String)
        Me._filePath = filePath
        Me._wb = New XLWorkbook()

        ' Add any initialization after the InitializeComponent() call.
        Dim configMap As ExeConfigurationFileMap = New ExeConfigurationFileMap() With {.ExeConfigFilename = "Indicadores.config"}
        Dim configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None)
        Dim connStringsSection = configuration.ConnectionStrings
        Dim connString = connStringsSection.ConnectionStrings("conciliacionesProveedores").ConnectionString
        conciliacionesProvRepository = New conciliacionesProveedores(connString)
    End Sub

    Public Async Function ExportarReporteAsync() As Task
        Await Task.Run(New Action(Sub()
                                      ' Primer hoja
                                      Dim ws As IXLWorksheet = _wb.Worksheets.Add("COM_PEND_PAGO")
                                      ws.ShowGridLines = False
                                      'Headers
                                      ws.Cell(1, 1).Value = "Proveedor"
                                      ws.Cell(1, 2).Value = "FechaConfPago"
                                      ws.Cell(1, 3).Value = "FechaActual"
                                      ws.Cell(1, 4).Value = "Dias"
                                      ws.Cell(1, 5).Value = "Transacciones"
                                      ws.Cell(1, 6).Value = "Monto de comision pendiente"
                                      'Proveedor
                                      ws.Cell(2, 1).Value = "Onyx"

                                      'Periodos
                                      Dim ListaPeriodos = conciliacionesProvRepository.onyxComisionesPendientePago.Where(Function(x) x.fechaConfPago IsNot Nothing).GroupBy(Function(x) x.fechaConfPago).ToList()
                                      Dim TrCollection As StringBuilder = New StringBuilder()
                                      For i As Integer = 0 To ListaPeriodos.Count() - 1
                                          Dim Periodo As Date = ListaPeriodos(i).Key.Value
                                          Dim PeriodoTransacciones As Integer = ListaPeriodos(i).Count()
                                          Dim MontoComisionPendiente As Decimal = 0

                                          Dim AgrupacionTransaccionesMoneda = ListaPeriodos(i).GroupBy(Function(x) x.ConfCurrency).ToList()

                                          For j As Integer = 0 To AgrupacionTransaccionesMoneda.Count() - 1
                                              Dim sumaMonedas As Decimal = AgrupacionTransaccionesMoneda(j).ToList().Sum(Function(x) ConvertStringToDecimal(x.ConfCostPrNight))

                                              Dim TipoMoneda As String = AgrupacionTransaccionesMoneda(j).Key
                                              Dim MonedaInfo As moneda = conciliacionesProvRepository.moneda.Where(Function(x) x.codigo.ToUpper() = TipoMoneda.ToUpper()).FirstOrDefault()

                                              If MonedaInfo IsNot Nothing Then
                                                  Dim TipoCambio = conciliacionesProvRepository.tipoCambio.Where(Function(x) x.fechaPeriodo = Periodo And x.idProveedor = 3).FirstOrDefault()
                                                  If TipoCambio IsNot Nothing Then
                                                      Dim TipoCambioDetalle = conciliacionesProvRepository.tipoCambioDetalle.Where(Function(x) x.idTipoCambio = TipoCambio.id And x.idMoneda = MonedaInfo.id).FirstOrDefault()
                                                      If TipoCambioDetalle IsNot Nothing Then
                                                          MontoComisionPendiente = MontoComisionPendiente + (AgrupacionTransaccionesMoneda(j).Sum(Function(x) x.ConfCostPrNight * TipoCambioDetalle.valorMoneda * x.ConfNoNights * 0.1))
                                                      End If
                                                  End If
                                              End If
                                          Next

                                          Dim FechaActual As Date = DateTime.Now

                                          ws.Cell(2 + i, 2).Value = Periodo.ToString("yyyy-MM-dd")
                                          ws.Cell(2 + i, 3).Value = FechaActual.ToString("yyyy-MM-dd")
                                          ws.Cell(2 + i, 4).Value = CInt((FechaActual - Periodo).TotalDays)
                                          ws.Cell(2 + i, 5).Value = PeriodoTransacciones
                                          ws.Cell(2 + i, 6).Value = MontoComisionPendiente.ToString()
                                      Next
                                      ws.Range(1, 1, ListaPeriodos.Count + 1, 6).CreateTable()
                                      ws.Columns(6).Style.Font.Bold = True
                                      ws.Columns(6).Style.NumberFormat.Format = "$#,##0.00"
                                      ws.Columns(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                                      ws.Columns.AdjustToContents()


                                      ' Segunda hoja

                                      For i As Integer = 0 To ListaPeriodos.Count() - 1
                                          Dim Periodo As Date = ListaPeriodos(i).Key.Value
                                          Dim wsReportePeriodo As IXLWorksheet = _wb.Worksheets.Add(Periodo.ToString("yyyy-MM-dd"))
                                          Dim PeriodoTransacciones As List(Of onyxComisionesPendientePago) = ListaPeriodos(i).ToList()

                                          Dim NumeroColumnas As Integer = GetType(onyxComisionesPendientePago).GetProperties().Count()
                                          Dim NumeroFilas As Integer = PeriodoTransacciones.Count

                                          wsReportePeriodo.Cell(1, 1).InsertTable(PeriodoTransacciones.AsEnumerable())
                                          wsReportePeriodo.Columns().AdjustToContents()
                                          wsReportePeriodo.Cell(PeriodoTransacciones.Count + 4, 1).Value = "TOTAL COMISIONES POR PAGAR"

                                          wsReportePeriodo.Cell(1, NumeroColumnas + 1).Value = "TipoDeCambio"
                                          wsReportePeriodo.Cell(1, NumeroColumnas + 2).Value = "Comision"

                                          For fila As Integer = 2 To NumeroFilas + 1
                                              Dim filaIndex As Integer = fila
                                              Dim MonedaInfo As moneda = conciliacionesProvRepository.moneda.ToList().Where(Function(x) x.codigo.ToUpper() = CStr(wsReportePeriodo.Cell(filaIndex, 50).Value).ToUpper()).FirstOrDefault()

                                              If MonedaInfo IsNot Nothing Then
                                                  Dim TipoCambio = conciliacionesProvRepository.tipoCambio.Where(Function(x) x.fechaPeriodo = Periodo And x.idProveedor = 3).FirstOrDefault()
                                                  If TipoCambio IsNot Nothing Then
                                                      Dim TipoCambioDetalle = conciliacionesProvRepository.tipoCambioDetalle.Where(Function(x) x.idTipoCambio = TipoCambio.id And x.idMoneda = MonedaInfo.id).FirstOrDefault()
                                                      If TipoCambioDetalle IsNot Nothing Then
                                                          wsReportePeriodo.Cell(filaIndex, NumeroColumnas + 1).Value = TipoCambioDetalle.valorMoneda
                                                      End If
                                                  End If
                                              End If

                                              wsReportePeriodo.Cell(filaIndex, NumeroColumnas + 2).FormulaR1C1 = String.Format("R{0}C{1} * R{2}C{3} * R{4}C{5} * 0.1", filaIndex, 48, filaIndex, NumeroColumnas + 1, filaIndex, 44)
                                          Next
                                          wsReportePeriodo.Tables.Remove(0)
                                          wsReportePeriodo.Range(1, 1, NumeroFilas + 1, NumeroColumnas + 2).CreateTable()
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 1).Value = "Total:"
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#0F243E")
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 1).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 1).Style.Font.Bold = True
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 2).FormulaR1C1 = String.Format("SUM(R{0}C{1}:R{2}C{3})", 2, NumeroColumnas + 2, NumeroFilas + 1, NumeroColumnas + 2)
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#0F243E")
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 2).Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")
                                          wsReportePeriodo.Cell(NumeroFilas + 2, NumeroColumnas + 2).Style.Font.Bold = True
                                          wsReportePeriodo.Columns(107).Style.NumberFormat.Format = "$#,##0.00"
                                          wsReportePeriodo.Columns(107).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
                                          wsReportePeriodo.Columns.AdjustToContents()
                                      Next

                                      _wb.SaveAs(_filePath)
                                  End Sub))
    End Function
End Class
