Public Class ProgressSaveFileChangedEventArgs
    Inherits EventArgs

    Private _progreso As Integer

    Public Property Progreso() As Integer
        Get
            Return _progreso
        End Get
        Set(value As Integer)
            _progreso = value
        End Set
    End Property
End Class
