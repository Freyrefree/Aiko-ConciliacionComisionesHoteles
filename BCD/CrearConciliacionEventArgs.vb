Imports Conciliacion

Public Class CrearConciliacionEventArgs
    Inherits EventArgs
    Private _campoEstadoDeCuenta As String
    Private _campoIcaav As String
    Private _TipoDatos As TiposDeDatos
    Private _Operador As Operadores
    Private _Grupo As GrupoConciliaciones

    Public Property CampoEstadoDeCuenta() As String
        Get
            Return _campoEstadoDeCuenta
        End Get
        Set(value As String)
            _campoEstadoDeCuenta = value
        End Set
    End Property

    Public Property CampoIcaav() As String
        Get
            Return _campoIcaav
        End Get
        Set(value As String)
            _campoIcaav = value
        End Set
    End Property

    Public Property TipoDatos() As TiposDeDatos
        Get
            Return _TipoDatos
        End Get
        Set(value As TiposDeDatos)
            _TipoDatos = value
        End Set
    End Property

    Public Property Operador() As Operadores
        Get
            Return _Operador
        End Get
        Set(value As Operadores)
            _Operador = value
        End Set
    End Property

    Public Property Grupo As GrupoConciliaciones
        Get
            Return _Grupo
        End Get
        Set(value As GrupoConciliaciones)
            _Grupo = value
        End Set
    End Property
End Class
