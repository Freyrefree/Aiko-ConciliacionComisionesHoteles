Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Partial Public Class proveedores
    Public Property id As Integer

    <StringLength(50)>
    Public Property nombre As String

    <StringLength(10)>
    Public Property activo As String
End Class
