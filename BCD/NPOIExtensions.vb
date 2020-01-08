Imports System.Runtime.CompilerServices
Imports NPOI.SS.UserModel

Module NPOIExtensions
    <Extension()>
    Sub SetCellValue(ByVal cell As ICell, ByVal value As Object)
        Dim type = value.[GetType]()

        If value.[GetType]() = GetType(Boolean) Then
            cell.SetCellValue(CBool(value))
            Return
        End If

        If value.[GetType]() = GetType(DateTime) Then
            cell.SetCellValue(CType(value, DateTime))
            Return
        End If

        If value.[GetType]() = GetType(Double) OrElse value.[GetType]() = GetType(Single) OrElse value.[GetType]() = GetType(Long) OrElse value.[GetType]() = GetType(Integer) Then
            cell.SetCellValue(CDbl(value))
            Return
        End If

        cell.SetCellValue(value.ToString())
    End Sub
End Module
