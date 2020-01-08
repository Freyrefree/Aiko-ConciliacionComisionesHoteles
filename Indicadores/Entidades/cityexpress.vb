Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("cityexpress")>
Partial Public Class cityexpress
    Public Property id As Integer

    <StringLength(50)>
    Public Property Reservacion As String

    <StringLength(50)>
    Public Property ReferenciaOTA As String

    <Column(TypeName:="date")>
    Public Property CheckIn As Date?

    <Column(TypeName:="date")>
    Public Property CheckOut As Date?

    <StringLength(50)>
    Public Property Monto As String

    <StringLength(50)>
    Public Property Moneda As String

    <StringLength(50)>
    Public Property FormaPago As String

    Public Property Tarifa As Integer?

    <StringLength(50)>
    Public Property Hotel As String

    Public Property IATA As Integer?

    <StringLength(250)>
    Public Property Huesped As String

    <StringLength(50)>
    Public Property Estatus As String

    Public Property Tasa As Decimal?

    <StringLength(50)>
    Public Property Comision As String

    <StringLength(50)>
    Public Property firstName As String

    <StringLength(50)>
    Public Property lastName As String

    Public Property NoNoches As Integer?

    Public Property estatusConciliado As Integer?

    <Column(TypeName:="date")>
    Public Property FechadePago As Date?

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
