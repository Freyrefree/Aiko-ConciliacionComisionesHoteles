Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("gestionCommtrack")>
Partial Public Class gestionCommtrack
    Public Property id As Integer

    <StringLength(50)>
    Public Property Usrspec As String

    <StringLength(50)>
    Public Property Trans As String

    <StringLength(50)>
    Public Property SuppID As String

    <StringLength(255)>
    Public Property Supplier As String

    <StringLength(50)>
    Public Property DIN As String

    <StringLength(50)>
    Public Property OUT As String

    <StringLength(50)>
    Public Property PAID_AGY As String

    <StringLength(50)>
    Public Property Confirmationcode As String

    <StringLength(50)>
    Public Property Curr As String

    <StringLength(50)>
    Public Property Rate As String

    <StringLength(255)>
    Public Property First As String

    <StringLength(50)>
    Public Property IATA As String

    <StringLength(255)>
    Public Property Last As String

    <StringLength(50)>
    Public Property nitec As String

    <StringLength(50)>
    Public Property Phone As String

    <StringLength(50)>
    Public Property PNR As String

    <StringLength(50)>
    Public Property Remark As String

    <StringLength(255)>
    Public Property Address1 As String

    <StringLength(255)>
    Public Property Address2 As String

    <StringLength(255)>
    Public Property VenType As String

    <StringLength(255)>
    Public Property segnum As String

    <StringLength(255)>
    Public Property Observaciones As String

    <Column(TypeName:="date")>
    Public Property Fechadepago As Date?

    Public Property Montototaldelareserva As Decimal?

    Public Property estatusConciliado As Integer?

    <Column("No.trxconcatenada")>
    <StringLength(50)>
    Public Property No_trxconcatenada As String

    <StringLength(50)>
    Public Property CondicionOKAuto As String

    <StringLength(50)>
    Public Property CondicionNOAuto As String

    Public Property countCumplidoAuto As Integer?

    Public Property countNoCumplidoAuto As Integer?

    Public Property idBDBCD As Integer?

    <Column(TypeName:="date")>
    Public Property mesProveedor As Date?

    Public Property estatusEliminado As Integer?

    <StringLength(50)>
    Public Property CondicionOKManual As String

    <StringLength(50)>
    Public Property CondicionNOManual As String

    Public Property countCumplidoManual As Integer?

    Public Property countNoCumplidoManual As Integer?

    Public Property idBDBCDManual As Integer?
End Class
