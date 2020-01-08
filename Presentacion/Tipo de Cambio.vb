Imports CapaNegocio.CapaNegocio

Public Class Tipo_de_Cambio

    'OBJETOS
    Private objetoTipoCambio As ClsN_TipodeCambio = New ClsN_TipodeCambio()
    Private objetoGlobales As ClsNGlobales = New ClsNGlobales()
    'PROPIEDADES

    Private fechaPeriodoGlobal As String
    Public idProveedor As String


    Private idDetalle As String
    Private montoMoneda As String

    Private Sub Tipo_de_Cambio_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cargaPeriodosFaltantes()

        consultaPeriodos()

        DGVPeriodos.Font = New Font("Tahoma", 12)
        DGVMonedas.Font = New Font("Tahoma", 12)

    End Sub

    Private Sub cargaPeriodosFaltantes()

        objetoTipoCambio.cargaPeriodosFaltantes()

    End Sub


    'Private Sub TxtTipoCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    Dim keyChar = e.KeyChar

    '    If Char.IsControl(keyChar) Then
    '        'Allow all control characters.
    '    ElseIf Char.IsDigit(keyChar) OrElse keyChar = "."c Then
    '        Dim text = txtTipoCambio.Text
    '        Dim selectionStart = txtTipoCambio.SelectionStart
    '        Dim selectionLength = txtTipoCambio.SelectionLength

    '        text = text.Substring(0, selectionStart) & keyChar & text.Substring(selectionStart + selectionLength)

    '        If Integer.TryParse(text, New Integer) AndAlso text.Length > 16 Then
    '            'Reject an integer that is longer than 16 digits.
    '            e.Handled = True
    '        ElseIf Double.TryParse(text, New Double) AndAlso text.IndexOf("."c) < text.Length - 4 Then
    '            'Reject a real number with two many decimal places.
    '            e.Handled = True
    '        End If
    '    Else
    '        'Reject all other characters.
    '        e.Handled = True
    '    End If
    'End Sub

    Private Sub CboPeriodos_SelectedIndexChanged(sender As Object, e As EventArgs)


        Dim fechas As String = ""
        Dim fecha As String

        If fechas.Length = 10 Then

            Dim arrayFecha() As String
            arrayFecha = fechas.Split(New Char() {"/"c})

            fecha = arrayFecha(2) & "-" & arrayFecha(1) & "-" & arrayFecha(0)

            fechaPeriodoGlobal = ""
            fechaPeriodoGlobal = fecha



        End If


    End Sub


    Public Sub consultaPeriodos()

        DGVPeriodos.ForeColor = Color.Black
        DGVPeriodos.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGVPeriodos.DataSource = objetoTipoCambio.consultaPeriodos()

        DGVPeriodos.Columns("id").Visible = False
        DGVPeriodos.Columns("idProveedor").Visible = False

        DGVPeriodos.Columns("fechaPeriodo").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        labelTotalPeriodos.Text = DGVPeriodos.Rows.Count - 1.ToString()

    End Sub

    Public Sub consultaMonedasPeriodo(idPeriodo, mesProveedor)

        DGVMonedas.ForeColor = Color.Black
        DGVMonedas.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan
        DGVMonedas.DataSource = objetoTipoCambio.consultaMonedasPeriodo(idPeriodo, mesProveedor)

        DGVMonedas.Columns("id").Visible = False
        DGVMonedas.Columns("idTipoCambio").Visible = False
        DGVMonedas.Columns("idMoneda").Visible = False
        DGVMonedas.Columns("nombreMoneda").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGVMonedas.Columns("fechaActualizacion").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        DGVMonedas.Columns("valorMoneda").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        Dim valorMoneda As DataGridViewColumn = DGVMonedas.Columns("valorMoneda")
        valorMoneda.HeaderCell.Style.BackColor = Color.MediumBlue
        valorMoneda.HeaderCell.Style.ForeColor = Color.White

        labelTotalMonedas.Text = DGVMonedas.Rows.Count - 1.ToString()

    End Sub

    Private Sub DGVPeriodos_DoubleClick(sender As Object, e As EventArgs) Handles DGVPeriodos.DoubleClick

        consultaMonedasPeriodo()

    End Sub

    Public Sub consultaMonedasPeriodo()

        Dim id As String
        Dim mesProveedor As String
        Dim arrayFecha() As String

        If (DGVPeriodos.SelectedRows.Count > 0) Then

            id = DGVPeriodos.CurrentRow.Cells("id").Value.ToString()
            mesProveedor = DGVPeriodos.CurrentRow.Cells("fechaPeriodo").Value.ToString()
            arrayFecha = mesProveedor.Split(New Char() {"/"c})
            mesProveedor = arrayFecha(2).Substring(0, 4) & "-" & arrayFecha(1) & "-" & arrayFecha(0)

            consultaMonedasPeriodo(id, mesProveedor)

        Else

            MessageBox.Show("Seleccione Una Fila")

        End If

    End Sub

    Private Sub DGVMonedas_DoubleClick(sender As Object, e As EventArgs) Handles DGVMonedas.DoubleClick


        Dim id As String
        Dim valorMoneda As String

        If (DGVMonedas.SelectedRows.Count > 0) Then

            id = DGVMonedas.CurrentRow.Cells("id").Value.ToString()
            valorMoneda = DGVMonedas.CurrentRow.Cells("valorMoneda").Value.ToString()
            idDetalle = id

        Else

            MessageBox.Show("Seleccione Una Fila")

        End If

    End Sub



    Private Sub DGVMonedas_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVMonedas.CellDoubleClick

        'DGVMonedas.ReadOnly = False
        If e.ColumnIndex = DGVMonedas.Columns("valorMoneda").Index Then

            DGVMonedas.ReadOnly = False
        Else

            DGVMonedas.ReadOnly = True
        End If


    End Sub

    Private Sub DGVMonedas_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DGVMonedas.CellValueChanged


        Dim id As String
        Dim valorMoneda As String
        Dim respuesta As Boolean


        If e.RowIndex = -1 Then Exit Sub ' PARA SABER SI HAY ROW EN EDICION

        id = DGVMonedas.CurrentRow.Cells("id").Value.ToString()
        valorMoneda = DGVMonedas.CurrentRow.Cells("valorMoneda").Value.ToString()

        If valorMoneda = "" Then

            MessageBox.Show("Ingrese Un Valor")
        Else

            Dim result As Double = 0.0



            If Double.TryParse(valorMoneda, result) Then
                ' valid entry
                respuesta = objetoTipoCambio.actualizarTipoCambio(id, result)

                If (respuesta) Then

                    idDetalle = Nothing
                    'consultaPeriodos()
                    consultaMonedasPeriodo()

                End If
            Else
                MessageBox.Show("Ingrese Un Valor Correcto")
            End If







        End If







    End Sub

    Private Sub DGVMonedas_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DGVMonedas.DataError


        Dim oDGVC As DataGridViewColumn = DGVMonedas.Columns(e.ColumnIndex)
        Dim sTextoMensaje As String
        sTextoMensaje = "ERROR EN LA COLUMNA: " & oDGVC.DataPropertyName & vbLf + e.Exception.Message
        MessageBox.Show(sTextoMensaje, "ERROR DE EDICIÓN", MessageBoxButtons.OK)
        'e.Cancel = [False]


    End Sub
End Class