Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos
Imports CapaDatos.CapaDatos
Imports System.Windows.Forms


Namespace CapaNegocio

    Public Class ClsN_ConsultasConciliaciones

        Private objetoCapaDatos As ClsConsultasConciliaciones = New ClsConsultasConciliaciones()

        Public idProveedor As Integer
        Public idConciliacion As Integer

        Public Function CN_ConsultaConciliacionesByIdProveedor() As DataTable

            Dim tablaConciliaciones As DataTable = New DataTable()

            objetoCapaDatos.idProveedor = Me.idProveedor

            tablaConciliaciones = objetoCapaDatos.CD_SelectConciliacionesByProveedor()

            Return tablaConciliaciones

        End Function

        Public Function CN_ConsultaConciliacionesByFechaPagoProveedor() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tablaConciliaciones As DataTable = New DataTable()

            objetoCapaDatos.idProveedor = Me.idProveedor

            tablaConciliaciones = objetoCapaDatos.CD_ConsultaConciliacionesByFechaPagoProveedor()

            Return tablaConciliaciones

        End Function

        Public Function CN_ConsultaConciliacionesDetalleByIdConciliacion() As DataTable

            Dim tablaConciliacionesDetalle As DataTable = New DataTable()

            objetoCapaDatos.idConciliacion = Me.idConciliacion

            tablaConciliacionesDetalle = objetoCapaDatos.CD_ConsultaConciliacionesDetalleByIdConciliacion()

            Return tablaConciliacionesDetalle

        End Function

    End Class

End Namespace
