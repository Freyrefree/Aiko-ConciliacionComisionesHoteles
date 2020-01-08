Public Class VisualizationTypeEventArgs
    Inherits System.EventArgs

    Public visualizationType As VISUALIZATION_TYPE

    Public Sub New(ByVal visType As VISUALIZATION_TYPE)
        MyBase.New()
        visualizationType = visType
    End Sub
End Class
