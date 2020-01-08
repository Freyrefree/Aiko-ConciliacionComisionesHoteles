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
Imports System.Text.RegularExpressions

Namespace CapaNegocio
    Public Class ClsN_TipodeCambio

        Private objetoCapaDatos As ClsTipoDeCambio = New ClsTipoDeCambio()



        Public Function CN_DataComboPeriodos()

            Return objetoCapaDatos.CN_DataComboPeriodos()

        End Function

        Public Function CN_DataComboMonedasPeriodo(fecha)

            Return objetoCapaDatos.CD_DataComboMonedasPeriodo(fecha)

        End Function

        Public Sub CN_guardarTipoCambio(idProveedor, fechaProveedor, tipoCambio, moneda)

            objetoCapaDatos.CD_guardarTipoCambio(idProveedor, fechaProveedor, tipoCambio, moneda)

        End Sub

        Public Function consultaPeriodos()

            Return objetoCapaDatos.consultaPeriodos()

        End Function

        Public Function consultaMonedasPeriodo(id, mesProveedor)

            Return objetoCapaDatos.consultaMonedasPeriodo(id, mesProveedor)

        End Function

        Public Function actualizarTipoCambio(id, valorMoneda)

            Return objetoCapaDatos.actualizarTipoCambio(id, valorMoneda)

        End Function

        Public Function cargaPeriodosFaltantes()

            Return objetoCapaDatos.cargaPeriodosFaltantes()

        End Function




    End Class

End Namespace
