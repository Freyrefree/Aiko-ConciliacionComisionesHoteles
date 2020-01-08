Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
Imports CapaNegocio.CapaNegocio


Public Class AgregarNuevaConciliacion


    Private objetoCapaNegocio As ClsN_Conciliaciones = New ClsN_Conciliaciones()

    Public Property idProveedor As String
    Public Property listaGrupos As New BindingList(Of String)
    Public Property idLista As Int32 = -1
    Public Property cadenaGet As String
    Public Property grupoGet As String



    Public Event PasarCondicion(text() As String)

    Public Event PassvalueUpdate(text() As String, idFila As Integer)

    Private Sub fillComboGrupos()

        Dim position As Int32 = 0

        For Each grupo In listaGrupos

            If (position >= 0) Then
                cmbGrupoNuevaConciliacion.Items.Add(grupo)
            End If

            position = position + 1
        Next

    End Sub

    Private Sub AgregarNuevaConciliacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        BtnActualizar.Visible = False




        If (idLista >= 0) Then

            Dim columnaBCD As String
            Dim columnaProveedor As String

            Dim arrayCadena() As String
            Dim arrayCadenaB() As String
            Dim arrayC As String
            Dim arrayD() As String
            Dim arrayE() As String

            Dim arrayPrioridad() As String
            Dim separadorPrioridad As String() = New String() {"----"}
            Dim prioridad As Integer



            arrayPrioridad = cadenaGet.Split(separadorPrioridad, StringSplitOptions.None)
            prioridad = Trim(arrayPrioridad(1))


            arrayCadena = cadenaGet.Split(New Char() {"["c})
            arrayCadenaB = arrayCadena(1).Split(New Char() {"]"c})

            arrayC = arrayCadenaB(0)

            arrayD = arrayC.Split(New Char() {"<"c})
            arrayE = arrayC.Split(New Char() {">"c})

            columnaBCD = Trim(arrayD(0))
            columnaProveedor = Trim(arrayE(1))

            LblDias.Visible = False
            inputDias.Visible = False

            fillComboGrupos()
            CargaColumnasBCDByProveedor()
            CargaColumnasProveedor()

            BtnActualizar.Visible = True

            NumPrioridad.Value = prioridad



            If (cadenaGet.Contains("NUMÉRICO")) Then

                cmbTipoDatosNuevaConciliacion.SelectedIndex = cmbTipoDatosNuevaConciliacion.FindStringExact("NUMÉRICO")

                If (cadenaGet.Contains("IGUALDAD")) Then

                    cmbTipoOperacionNuevaConciliacion.SelectedIndex = cmbTipoOperacionNuevaConciliacion.FindStringExact("IGUALDAD")

                End If


            ElseIf (cadenaGet.Contains("TEXTO")) Then

                cmbTipoDatosNuevaConciliacion.SelectedIndex = cmbTipoDatosNuevaConciliacion.FindStringExact("TEXTO")

                If (cadenaGet.Contains("CONTIENE")) Then
                    cmbTipoOperacionNuevaConciliacion.SelectedIndex = cmbTipoOperacionNuevaConciliacion.FindStringExact("CONTIENE")

                ElseIf (cadenaGet.Contains("IGUALDAD")) Then
                    cmbTipoOperacionNuevaConciliacion.SelectedIndex = cmbTipoOperacionNuevaConciliacion.FindStringExact("IGUALDAD")

                End If


            ElseIf (cadenaGet.Contains("MONEDA")) Then

                cmbTipoDatosNuevaConciliacion.SelectedIndex = cmbTipoDatosNuevaConciliacion.FindStringExact("MONEDA")

                If (cadenaGet.Contains("IGUALDAD")) Then

                    cmbTipoOperacionNuevaConciliacion.SelectedIndex = cmbTipoOperacionNuevaConciliacion.FindStringExact("IGUALDAD")


                End If


            ElseIf (cadenaGet.Contains("FECHA")) Then

                cmbTipoDatosNuevaConciliacion.SelectedIndex = cmbTipoDatosNuevaConciliacion.FindStringExact("FECHA")

                If (cadenaGet.Contains("IGUALDAD")) Then
                    cmbTipoOperacionNuevaConciliacion.SelectedIndex = cmbTipoOperacionNuevaConciliacion.FindStringExact("IGUALDAD")


                ElseIf (cadenaGet.Contains("RANGO")) Then

                    'Dim dias As String = cadenaGet.Substring(cadenaGet.LastIndexOf("(") + 1)
                    'dias = dias.Replace(")", "")

                    Dim arrayDias() As String
                    Dim separadorDias As String() = New String() {")(", ")"}
                    Dim dias As Integer

                    arrayDias = cadenaGet.Split(separadorDias, StringSplitOptions.None)
                    dias = Trim(arrayDias(1))

                    dias = Convert.ToInt32(dias)



                    inputDias.Value = dias
                    LblDias.Visible = True
                    inputDias.Visible = True

                    cmbTipoOperacionNuevaConciliacion.Items.Remove("CONTIENE")
                    cmbTipoOperacionNuevaConciliacion.Items.Remove("RANGO")
                    cmbTipoOperacionNuevaConciliacion.Items.Add("RANGO")

                    cmbTipoOperacionNuevaConciliacion.SelectedIndex = cmbTipoOperacionNuevaConciliacion.FindStringExact("RANGO")


                End If


            End If

            lbxColumnasReporte1.SelectedIndex = lbxColumnasReporte1.FindStringExact(columnaBCD)
            lbxColumnaReporte2.SelectedIndex = lbxColumnaReporte2.FindStringExact(columnaProveedor)

            cmbGrupoNuevaConciliacion.SelectedIndex = cmbGrupoNuevaConciliacion.FindStringExact(grupoGet)

        Else


            LblDias.Visible = False
            inputDias.Visible = False


            fillComboGrupos()
            CargaColumnasBCDByProveedor()
            CargaColumnasProveedor()


        End If





    End Sub

    Private Sub cmbTipoOperacionNuevaConciliacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoOperacionNuevaConciliacion.SelectedIndexChanged

        If cmbTipoOperacionNuevaConciliacion.Text = "RANGO" Then

            LblDias.Visible = True
            inputDias.Visible = True
            'NUDDias.Value = 0

        Else
            LblDias.Visible = False
            inputDias.Visible = False
            'NUDDias.Value = 0
        End If

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub


    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnCancelarNuevaConciliacion_Click(sender As Object, e As EventArgs) Handles btnCancelarNuevaConciliacion.Click
        Me.Close()
    End Sub

    Private Sub btnAgregarNuevaConciliacion_Click(sender As Object, e As EventArgs) Handles btnAgregarNuevaConciliacion.Click

        Dim array() As String
        array = retornacadena()


        If array.Count > 0 Then
            RaiseEvent PasarCondicion(array)
            Me.Close()

        End If

    End Sub

    Private Sub BtnActualizar_Click(sender As Object, e As EventArgs) Handles BtnActualizar.Click

        'ACTUALIZAR

        Dim array() As String
        array = retornacadena()


        If array.Count > 0 Then
            RaiseEvent PassvalueUpdate(array, idLista)
            Me.Close()

        End If

    End Sub

    Private Sub cmbTipoDatosNuevaConciliacion_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbTipoDatosNuevaConciliacion.SelectionChangeCommitted

        Dim tipoDato As Int32

        tipoDato = cmbTipoDatosNuevaConciliacion.SelectedIndex

        Select Case tipoDato

            Case 0
                cmbTipoOperacionNuevaConciliacion.Items.Remove("CONTIENE")
                cmbTipoOperacionNuevaConciliacion.Items.Remove("RANGO")
            Case 1
                cmbTipoOperacionNuevaConciliacion.Items.Remove("CONTIENE")
                cmbTipoOperacionNuevaConciliacion.Items.Add("CONTIENE")
                cmbTipoOperacionNuevaConciliacion.Items.Remove("RANGO")
            Case 2
                cmbTipoOperacionNuevaConciliacion.Items.Remove("CONTIENE")
                cmbTipoOperacionNuevaConciliacion.Items.Remove("RANGO")
            Case 3
                cmbTipoOperacionNuevaConciliacion.Items.Remove("CONTIENE")
                cmbTipoOperacionNuevaConciliacion.Items.Add("RANGO")
            Case Else
                'Debug.WriteLine("Not between 1 and 10, inclusive")
        End Select


    End Sub


    Public Function retornacadena()

        Dim datosConciliacion(2) As String

        Dim columnBCD As String = ""
        Dim columnProveedor As String = ""
        Dim cadena As String = ""
        Dim valoresConciliacion As String = ""

        Dim tipoDato As String = ""
        Dim tipoOperacion As String = ""
        Dim grupo As String = ""
        Dim idGrupo As String = ""
        Dim prioridad As Integer



        tipoDato = cmbTipoDatosNuevaConciliacion.SelectedItem
        tipoOperacion = cmbTipoOperacionNuevaConciliacion.SelectedItem
        grupo = cmbGrupoNuevaConciliacion.SelectedItem
        idGrupo = cmbGrupoNuevaConciliacion.SelectedIndex

        columnBCD = lbxColumnasReporte1.SelectedItem
        columnProveedor = lbxColumnaReporte2.SelectedItem

        prioridad = NumPrioridad.Value

        If prioridad > 0 Then

            If columnBCD <> Nothing And columnProveedor <> Nothing And prioridad <> Nothing Then

                If (columnBCD.Contains("[")) Then

                    columnBCD = columnBCD.Replace("[", "{")
                    columnBCD = columnBCD.Replace("]", "}")

                End If

                If (columnProveedor.Contains("[")) Then

                    columnProveedor = columnProveedor.Replace("[", "{")
                    columnProveedor = columnProveedor.Replace("]", "}")

                End If


                If tipoDato <> Nothing And tipoOperacion <> Nothing And grupo <> Nothing Then

                    'para Visualizar

                    valoresConciliacion = columnBCD & "," & columnProveedor & "," & tipoDato & "," & tipoOperacion & "," & grupo

                    If (inputDias.Value > 0) Then
                        cadena = "[" & columnBCD & " <---> " & columnProveedor & "][" & tipoDato & "](" & tipoOperacion & ")(" & inputDias.Value & ")----" & prioridad
                    Else
                        cadena = "[" & columnBCD & " <---> " & columnProveedor & "][" & tipoDato & "](" & tipoOperacion & ")----" & prioridad
                    End If



                    'cadenaR = cadena

                    datosConciliacion(0) = valoresConciliacion
                    datosConciliacion(1) = cadena
                    datosConciliacion(2) = idGrupo
                    Return datosConciliacion




                Else
                    MessageBox.Show("Seleccione Los Campos Requeridos")
                    'cadena = "nada"
                    Return datosConciliacion

                End If

            Else
                MessageBox.Show("Seleccione una Columna de Cada Archivo")
                'cadena = "nada"
                Return datosConciliacion

            End If
        Else

            MessageBox.Show("Seleccione  la prioridad")

            Return datosConciliacion


        End If


    End Function



    Private Sub CargaColumnasBCDByProveedor()
        Dim id As Int32
        Dim nombreColumna As String
        id = idProveedor

        lbxColumnasReporte1.Items.Clear()



        Dim lista = objetoCapaNegocio.columnasBCDByProveedor(id)
        If lista.Count > 0 Then

            For Each el In lista
                nombreColumna = el.columnBCDByProveedor
                lbxColumnasReporte1.Items.Add(nombreColumna)
            Next

        End If

    End Sub

    Private Sub CargaColumnasProveedor()

        Dim id As Int32
        Dim nombreColumna As String
        id = idProveedor

        lbxColumnaReporte2.Items.Clear()



        Dim lista = objetoCapaNegocio.columnasProveedorInterfaz(id)
        If lista.Count > 0 Then

            For Each el In lista
                nombreColumna = el.columnProveedor
                lbxColumnaReporte2.Items.Add(nombreColumna)
            Next

        End If

    End Sub

    Private Sub cmbTipoOperacionNuevaConciliacion_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbTipoOperacionNuevaConciliacion.SelectionChangeCommitted

        Dim value As String = ""
    End Sub

    Private Sub NUDDias_ValueChanged(sender As Object, e As EventArgs)

    End Sub
End Class
