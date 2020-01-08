Imports ClosedXML.Excel

Public Class ExportadorReporte
    Private _workbook As IXLWorkbook

    Public Sub New()
        _workbook = New XLWorkbook()
    End Sub

    Public Sub GenerarHojaTabulador()
        Dim _worksheetTabulador As IXLWorksheet = _workbook.Worksheets.Add("COMIS_PEND_PAGO")
    End Sub


End Class
