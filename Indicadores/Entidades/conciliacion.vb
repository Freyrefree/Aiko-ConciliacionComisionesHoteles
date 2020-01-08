Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("conciliacion")>
Partial Public Class conciliacion
    Public Property id As Integer

    <StringLength(50)>
    Public Property nombreConciliacion As String

    Public Property idProveedor As Integer?

    <Column(TypeName:="date")>
    Public Property fechaCreacion As Date?
End Class
