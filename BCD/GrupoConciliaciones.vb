Imports System.ComponentModel

Public Class GrupoConciliaciones
    Implements INotifyPropertyChanged

    Private _Nombre As String
    Private _ListaConciliaciones As List(Of Conciliacion)
    Private _YaProcesado As Boolean
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
    Public Sub New()
        _ListaConciliaciones = New List(Of Conciliacion)
    End Sub

    Public Sub AgregarConciliacionAGrupo(ByVal concil As Conciliacion)
        _ListaConciliaciones.Add(concil)
        NotifyPropertyChanged("ListaConciliaciones")
    End Sub

    Public Sub ModificarConciliacionAGrupo(ByVal concilAntigua As Conciliacion, ByVal concilNueva As Conciliacion)
        Dim cAntigua As Conciliacion = _ListaConciliaciones.ElementAtOrDefault(_ListaConciliaciones.IndexOf(concilAntigua))
        If cAntigua IsNot Nothing Then
            cAntigua.CampoEstadoDeCuenta = concilNueva.CampoEstadoDeCuenta
            cAntigua.CampoIcaav = concilNueva.CampoIcaav
            cAntigua.TipoDeDatos = concilNueva.TipoDeDatos
            cAntigua.Operador = concilNueva.Operador
            cAntigua.Grupo = concilNueva.Grupo
        End If
        NotifyPropertyChanged("ListaConciliaciones")
    End Sub

    Public Sub EliminarConciliacion(ByVal concil As Conciliacion)
        If _ListaConciliaciones.Contains(concil) Then
            _ListaConciliaciones.Remove(concil)
        End If
        NotifyPropertyChanged("ListaConciliaciones")
    End Sub

    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
            NotifyPropertyChanged("Nombre")
        End Set
    End Property

    Public Property ListaConciliaciones() As List(Of Conciliacion)
        Get
            Return _ListaConciliaciones
        End Get
        Set(value As List(Of Conciliacion))
            _ListaConciliaciones = value
        End Set
    End Property

    Public Property YaProcesado As Boolean
        Get
            Return _YaProcesado
        End Get
        Set(value As Boolean)
            _YaProcesado = value
            NotifyPropertyChanged("YaProcesado")
        End Set
    End Property

    Public Overrides Function ToString() As String
        If Me.YaProcesado Then
            Return Nombre & (" (YA PROCESADO)")
        Else
            Return Nombre & (" (SIN PROCESAR)")
        End If
    End Function

End Class
