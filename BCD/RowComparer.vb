Public Class RowComparer
    Implements IEqualityComparer(Of DataRow)

    Public Function Equals(LeftRow As DataRow, RightRow As DataRow) As Boolean Implements IEqualityComparer(Of DataRow).Equals
        If LeftRow Is Nothing Or RightRow Is Nothing Then
            Return False
        End If

        If (LeftRow.RowState = DataRowState.Deleted) Or (RightRow.RowState = DataRowState.Deleted) Then
            Throw New Exception("No se pueden comparar elementos que han sido eliminados.")
        End If

        Dim LeftRowColumnCount As Integer = LeftRow.ItemArray.Count()
        Dim RightRowColumnCount As Integer = RightRow.ItemArray.Count()

        If LeftRowColumnCount <> RightRowColumnCount Then
            Return False
        End If

        For i As Integer = 0 To LeftRowColumnCount - 1
            If Not LeftRow(0).ToString().Equals(RightRow(0).ToString()) Then
                Return False
            End If
        Next

        Return True
    End Function

    Public Function GetHashCode(row As DataRow) As Integer Implements IEqualityComparer(Of DataRow).GetHashCode
        If row.RowState = DataRowState.Deleted Then
            Throw New Exception("No se puede obtener el hashcode de un elemento eliminado.")
        End If

        Dim HashCode As Integer = 0

        If row.ItemArray.Count <= 0 Then
            Return HashCode
        End If

        For i As Integer = 0 To row.ItemArray.Count() - 1
            HashCode = HashCode Xor row(i).ToString().GetHashCode()
        Next
        Return HashCode
    End Function
End Class
