Imports Conciliacion

Public Class AgregarNuevoGrupoConciliacionArgs
    Inherits EventArgs
    Private _GrupoConciliaciones As GrupoConciliaciones

    Public Property GrupoConciliaciones As GrupoConciliaciones
        Get
            Return _GrupoConciliaciones
        End Get
        Set(value As GrupoConciliaciones)
            _GrupoConciliaciones = value
        End Set
    End Property
End Class
