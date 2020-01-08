Imports Conciliacion

Public Class Conciliacion
    Private _campoEstadoDeCuenta As String
    Private _campoIcaav As String
    Private _tipoDeDatos As TiposDeDatos
    Private _operador As Operadores
    Private _grupo As GrupoConciliaciones
    Public Sub New(ByVal cEstadoDeCuenta As String, ByVal cIcaav As String, ByVal TipoDatos As TiposDeDatos, ByVal Operador As Operadores, ByVal Grupo As GrupoConciliaciones)
        _campoEstadoDeCuenta = cEstadoDeCuenta
        _campoIcaav = cIcaav
        _tipoDeDatos = CType(TipoDatos, TiposDeDatos)
        _operador = CType(Operador, Operadores)
    End Sub

    Property CampoEstadoDeCuenta As String
        Get
            Return _campoEstadoDeCuenta
        End Get
        Set(value As String)
            _campoEstadoDeCuenta = value
        End Set
    End Property

    Property CampoIcaav As String
        Get
            Return _campoIcaav
        End Get
        Set(value As String)
            _campoIcaav = value
        End Set
    End Property

    Property TipoDeDatos As TiposDeDatos
        Get
            Return _tipoDeDatos
        End Get
        Set(value As TiposDeDatos)
            _tipoDeDatos = value
        End Set
    End Property

    Property Operador As Operadores
        Get
            Return _operador
        End Get
        Set(value As Operadores)
            _operador = value
        End Set
    End Property

    Public Property Grupo As GrupoConciliaciones
        Get
            Return _grupo
        End Get
        Set(value As GrupoConciliaciones)
            _grupo = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return "[" & CampoEstadoDeCuenta & " <---> " & CampoIcaav & "]" & " [" & _tipoDeDatos.ToString() & "]"
    End Function
End Class
