Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos
Imports CapaDatos.CapaDatos
Imports System.IO
Imports ExcelDataReader
Imports System.Globalization

Namespace CapaNegocio


    Public Class ClsN_OnyxRepetidos

        Private objetoCapaDatos As ClsOnyx = New ClsOnyx()
        Public Function CN_DatosOnyxRepetidosMesProveedor()
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            Return objetoCapaDatos.CD_DatosOnyxRepetidosMesProveedor()

        End Function


        Public Function CN_consultaOnyxPaidCommisionMesProveedor()
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            Return objetoCapaDatos.CD_consultaOnyxPaidCommisionMesProveedor()

        End Function



    End Class

End Namespace
