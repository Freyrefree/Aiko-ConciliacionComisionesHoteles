
Imports System.ComponentModel

Namespace CapaNegocio

    Public Class ClsN_MatchColumnas


        Public ListaMatchGet As List(Of String) = New List(Of String)()

        Private ListaMatchReturn As List(Of List(Of String)) = New List(Of List(Of String))

        'Public Property ListaMatchGet As IList(Of String)
        '    Get
        '        Return _ListaMatchGet
        '    End Get
        '    Set(ByVal value As String)
        '        _ListaMatchGet = value
        '    End Set
        'End Property

        'Public ReadOnly Property ListaMatch As IList(Of IList(Of String))
        '    Get
        '        Return _ListaMatch
        '    End Get
        'End Property


        Public Function MatchColumnas()

            Dim seccionPrioridad As String() = New String() {"----"}
            Dim prioridad As Integer

            Dim myDelims As String() = New String() {"][", "]("}

            Dim cadena As String = ""

            Dim first As String = ""
            Dim second As String = ""

            Dim third As String = ""
            Dim fourth As String = ""

            Dim fifth As String = ""
            Dim sixth As String = ""

            Dim seventh As String = ""
            Dim eighth As String = ""

            Dim nueve As String = ""
            Dim diez As String = ""

            Dim a As String()
            Dim b As String()

            Dim c As String()
            Dim d As String()

            Dim e As String()
            Dim f As String()

            Dim g As String()
            Dim h As String()

            Dim j As String()

            Dim arrayPrioridad As String()


            Dim columnaBCD As String = ""
            Dim columnaCliente As String = ""
            Dim tipoOperaion As String = ""
            Dim diasRango As String = ""
            Dim tipoDato As String = ""

            Dim i As Int16 = 0

            'Dim stringA As String = ""
            'Dim stringB As String = ""


            For Each item In Me.ListaMatchGet

                'stringA = item.Item1
                'stringB = item.Item2

                cadena = ""
                cadena = item

                'columna BCD
                a = cadena.Split(New Char() {"["c})
                first = Trim(a(1)).ToString()

                b = first.Split(New Char() {"<"c})
                second = Trim(b(0)).ToString()
                '*************************

                'columna Proveedores
                c = cadena.Split(New Char() {">"c})
                third = Trim(c(1)).ToString()

                d = third.Split(New Char() {"]"c})
                fourth = Trim(d(0)).ToString()

                '*************************

                'tipoDato
                j = cadena.Split(myDelims, StringSplitOptions.None) 'cadena.Split(New Char() {"("c})
                nueve = Trim(j(1)).ToString()
                '*************************

                'tipoOperacion
                e = cadena.Split(New Char() {"("c})
                fifth = Trim(e(1)).ToString()

                f = fifth.Split(New Char() {")"c})
                sixth = Trim(f(0)).ToString()

                '*************************

                ''PRIORIDAD
                arrayPrioridad = cadena.Split(seccionPrioridad, StringSplitOptions.None)
                prioridad = Trim(arrayPrioridad(1)).ToString()
                '*************************

                columnaBCD = second
                columnaCliente = fourth
                tipoOperaion = sixth
                tipoDato = nueve

                If (tipoOperaion = "RANGO") Then

                    'Número de Rango de Días

                    g = cadena.Split(New Char() {"("c})
                    seventh = Trim(e(2)).ToString()

                    h = seventh.Split(New Char() {")"c})
                    eighth = Trim(h(0)).ToString()

                    '*************************

                End If

                Me.ListaMatchReturn.Add(New List(Of String))
                Me.ListaMatchReturn(i).Add(columnaBCD)
                Me.ListaMatchReturn(i).Add(columnaCliente)
                Me.ListaMatchReturn(i).Add(tipoOperaion)

                If (tipoOperaion = "RANGO") Then
                    diasRango = eighth
                    Me.ListaMatchReturn(i).Add(diasRango)
                Else
                    Me.ListaMatchReturn(i).Add("0")
                End If

                Me.ListaMatchReturn(i).Add(tipoDato)
                Me.ListaMatchReturn(i).Add(prioridad)
                i = i + 1

            Next

            Return ListaMatchReturn


        End Function

    End Class

End Namespace
