Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("tipoCambioDetalle")>
Partial Public Class tipoCambioDetalle
    Public Property id As Integer

    Public Property idTipoCambio As Integer?

    Public Property idMoneda As Integer?

    Public Property valorMoneda As Decimal?

    Public Property tipo As Integer?

    Public Property idOnyxPendiente As Integer?

    Public Property fechaActualizacion As Date?
End Class
