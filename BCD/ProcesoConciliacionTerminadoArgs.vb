Imports Conciliacion

Public Class ProcesoConciliacionTerminadoArgs
    Inherits EventArgs
    Private _Grupo As GrupoConciliaciones

    Public Property Grupo As GrupoConciliaciones
        Get
            Return _Grupo
        End Get
        Set(value As GrupoConciliaciones)
            _Grupo = value
        End Set
    End Property
End Class
