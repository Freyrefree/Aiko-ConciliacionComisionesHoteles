Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("posadasTmp")>
Partial Public Class posadasTmp
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
    Public Property percentComision As String

    <Column(TypeName:="date")>
    Public Property fechaPago As Date?
End Class
