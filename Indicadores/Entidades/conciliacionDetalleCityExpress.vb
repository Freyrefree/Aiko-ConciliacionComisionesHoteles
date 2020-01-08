Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("conciliacionDetalleCityExpress")>
Partial Public Class conciliacionDetalleCityExpress
    Public Property id As Integer

    <StringLength(10)>
    Public Property idConciliacion As String

    <StringLength(50)>
    Public Property dim_value As String

    Public Property FechaApp As Date?

    <StringLength(50)>
    Public Property UserSpec As String

    <StringLength(50)>
    Public Property Segmento As String

    <StringLength(50)>
    Public Property CodigoConfirmacion As String

    <StringLength(50)>
    Public Property Comision As String

    <StringLength(50)>
    Public Property Operador As String

    <StringLength(50)>
    Public Property Moneda As String

    <StringLength(50)>
    Public Property CostoTotalDeLaReserva As String

    <StringLength(50)>
    Public Property Noches As String

    <StringLength(50)>
    Public Property ComOrig As String

    <StringLength(50)>
    Public Property TipoConciliacion As String

    <StringLength(50)>
    Public Property SequenceNo As String
End Class
