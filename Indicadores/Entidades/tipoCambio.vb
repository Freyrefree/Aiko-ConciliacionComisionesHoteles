Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tipoCambio")>
Partial Public Class tipoCambio
    Public Property id As Integer

    Public Property idProveedor As Integer?

    <Column(TypeName:="date")>
    Public Property fechaPeriodo As Date?
End Class
