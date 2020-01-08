Imports System.Globalization

Module TypeConverter
    Function TryConvert(ByVal value As String, ByVal cinfo As CultureInfo) As Object
        If String.IsNullOrEmpty(value) Then
            Return String.Empty
        End If

        Dim doubleValue As Double = 0

        If Double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, doubleValue) Then
            Return doubleValue
        End If

        Dim floatValue As Single = 0

        If Single.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, floatValue) Then
            Return floatValue
        End If

        Dim longValue As Long = 0

        If Long.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, longValue) Then
            Return longValue
        End If

        Dim intValue As Integer = 0

        If Integer.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, intValue) Then
            Return intValue
        End If

        Dim boolValue As Boolean = False

        If Boolean.TryParse(value, boolValue) Then
            Return boolValue
        End If

        Dim dateTimeValue As DateTime = DateTime.MinValue

        If DateTime.TryParseExact(value, "dd/MM/yyyy", cinfo, DateTimeStyles.None, dateTimeValue) Then
            Return dateTimeValue.ToString("dd/MM/yyyy", cinfo)
        End If

        Return (value)
    End Function
End Module
