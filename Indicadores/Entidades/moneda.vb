Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("moneda")>
Partial Public Class moneda
    Public Property id As Integer

    <StringLength(50)>
    Public Property codigo As String

    <StringLength(50)>
    Public Property nombreMoneda As String
End Class
