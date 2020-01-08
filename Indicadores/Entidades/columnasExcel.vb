Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("columnasExcel")>
Partial Public Class columnasExcel
    Public Property id As Integer

    Public Property idProveedor As Integer?

    <StringLength(50)>
    Public Property nombreColumna As String

    Public Property esAuto As Integer?

    Public Property columnBDBCD As Integer?

    Public Property mostrarInterfaz As Integer?

    Public Property tipoOperacion As Integer?

    Public Property tipoDato As Integer?
End Class
