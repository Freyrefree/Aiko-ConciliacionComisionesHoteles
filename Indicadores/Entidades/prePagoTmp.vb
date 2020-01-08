Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("prePagoTmp")>
Partial Public Class prePagoTmp
    Public Property id As Integer

    Public Property numTransaccion As Integer?

    <Column(TypeName:="date")>
    Public Property fechaAppl As Date?

    <StringLength(50)>
    Public Property usrSpec As String

    Public Property noSegmento As Integer?

    <StringLength(50)>
    Public Property confirmationCode As String

    Public Property comisionAplicar As Decimal?

    <StringLength(50)>
    Public Property operador As String

    <StringLength(50)>
    Public Property moneda As String

    Public Property costoTotaldeLaReserva As Decimal?

    Public Property noNoches As Integer?

    Public Property comisionOriginal As Decimal?

    Public Property cupon As Integer?

    <Column(TypeName:="date")>
    Public Property fechadePago As Date?
End Class
