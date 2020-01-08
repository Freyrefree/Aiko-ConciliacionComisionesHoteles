Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Partial Public Class posadas
    Public Property id As Integer

    <StringLength(255)>
    Public Property hotel As String

    <StringLength(50)>
    Public Property iata As String

    <StringLength(50)>
    Public Property clave As String

    <StringLength(50)>
    Public Property claveGDS As String

    <StringLength(255)>
    Public Property huesped As String

    <Column(TypeName:="date")>
    Public Property llegada As Date?

    <Column(TypeName:="date")>
    Public Property salida As Date?

    <StringLength(50)>
    Public Property comision As String

    <StringLength(50)>
    Public Property moneda As String

    <StringLength(50)>
    Public Property noNoches As String

    <StringLength(50)>
    Public Property firstName As String

    <StringLength(50)>
    Public Property lastName As String

    Public Property totalDeLaReserva As Decimal?

    <StringLength(50)>
    Public Property percentComision As String

    <Column(TypeName:="date")>
    Public Property fechaPago As Date?

    Public Property estatusConciliado As Integer?

    <StringLength(50)>
    Public Property CondicionOkAuto As String

    <StringLength(50)>
    Public Property CondicionNoAuto As String

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
