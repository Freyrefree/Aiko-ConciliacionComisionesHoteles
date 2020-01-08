Public Class ConvertirArchivoEventArgs
    Inherits EventArgs
    Private _nombreArchivo As String

    Public Property NombreArchivo()
        Get
            Return _nombreArchivo
        End Get
        Set(value)
            _nombreArchivo = value
        End Set
    End Property
End Class
