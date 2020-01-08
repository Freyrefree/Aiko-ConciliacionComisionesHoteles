Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("columnasBDBCD")>
Partial Public Class columnasBDBCD
    Public Property id As Integer

    <StringLength(50)>
    Public Property nombreColumna As String
End Class
