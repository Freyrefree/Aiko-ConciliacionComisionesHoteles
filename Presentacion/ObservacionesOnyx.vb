Imports CapaNegocio.CapaNegocio

Public Class ObservacionesOnyx

    Private objetoCapaNegocio As ClsN_Onyx = New ClsN_Onyx()

    Public id As String = Nothing


    Public Event RetornoForm(res As Boolean)

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click



        Dim observacion As String = TxBObservacion.Text.ToString()


        If id <> Nothing Then

            If (observacion <> "") Then

                If (objetoCapaNegocio.CN_agregarObservacion(id, observacion)) Then

                    'Cambiar valor de idCliente a vacío para que no se pueda realzar la operacion más de una vez
                    id = Nothing

                    RaiseEvent RetornoForm(True)
                    Me.Close()


                    Me.Dispose()

                End If



            Else

                MessageBox.Show("Ingrese una observación")

            End If

        Else
            MessageBox.Show("Intente de Nuevo Por Favor")
        End If

    End Sub





End Class