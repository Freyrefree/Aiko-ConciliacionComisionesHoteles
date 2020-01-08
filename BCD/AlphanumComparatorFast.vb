Public Class AlphanumComparator
    Implements IComparer(Of DataRow)

    Public Sub New()

    End Sub

    Public Function Compare(x As DataRow, y As DataRow) As Integer Implements IComparer(Of DataRow).Compare
        Throw New NotImplementedException()
    End Function
End Class
