Module StringUtils
    Public Function ConvertStringToDecimal(ByVal CadenaNumero As String) As Decimal
        Dim Numero As Decimal = 0.0D
        Decimal.TryParse(CadenaNumero, Numero)
        Return Numero
    End Function
End Module
