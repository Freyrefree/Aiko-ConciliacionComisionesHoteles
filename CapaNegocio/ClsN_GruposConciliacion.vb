Imports System.ComponentModel
Imports CapaDatos.CapaDatos


Namespace CapaNegocio

    Public Class ClsN_GruposConciliacion

        'Objetos
        Private proveedor As ClsProveedores = New ClsProveedores()
        '''''''''''''

        Private _listaGrupos As BindingList(Of String) = New BindingList(Of String)()
        Private _nombreGrupo As String

        Private _listaCondiciones As List(Of Tuple(Of Integer, String)) = New List(Of Tuple(Of Integer, String))()
        Private _nombreCondicion As String

        Private listaCondicionesById As BindingList(Of String) = New BindingList(Of String)()

        Private _idProveedor As Integer

        Private _validarOnyx As Integer

        Private _validarFormatoCityExpress As Integer

        Private _tipoGrupoCondicion As Integer ' 0->Automatico  1->Manual


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Public Property IdProveedor As Integer
            Get
                Return _idProveedor
            End Get
            Set(ByVal value As Integer)
                _idProveedor = value
            End Set
        End Property

        Public Property ValidarFormatoCityExpress As Integer
            Get
                Return _validarFormatoCityExpress
            End Get
            Set(ByVal value As Integer)
                _validarFormatoCityExpress = value
            End Set
        End Property

        Public Property ValidarOnyx As Integer
            Get
                Return _validarOnyx
            End Get
            Set(ByVal value As Integer)
                _validarOnyx = value
            End Set
        End Property

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Property TipoGrupoCondicion As Integer
            Get
                Return _tipoGrupoCondicion
            End Get
            Set(ByVal value As Integer)
                _tipoGrupoCondicion = value
            End Set
        End Property

        Public Property NombreCondicion As String
            Get
                Return _nombreCondicion
            End Get
            Set(ByVal value As String)
                _nombreCondicion = value
            End Set
        End Property


        Public ReadOnly Property ListaCondiciones As List(Of Tuple(Of Integer, String))
            Get
                Return _listaCondiciones
            End Get
        End Property



        Public Sub AddCondicion()

            ListaCondiciones.Add(Tuple.Create(Me._tipoGrupoCondicion, Me._nombreCondicion))

        End Sub



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Public Sub GruposDefault()

            Me.NombreGrupo = "Automatico"
            AddGrupo()
            Me.NombreGrupo = "Manual"
            AddGrupo()

        End Sub


        Public Property NombreGrupo As String
            Get
                Return _nombreGrupo
            End Get
            Set(ByVal value As String)
                _nombreGrupo = value
            End Set
        End Property


        Public ReadOnly Property ListaGrupos As IList(Of String)
            Get
                Return _listaGrupos
            End Get
        End Property



        Public Sub AddGrupo()
            ListaGrupos.Add(Me.NombreGrupo)
        End Sub


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '0 Condiciones Automaticas
        '1 Condiciones Manuales

        Public Sub condicionesAutomaticas()

            Me.TipoGrupoCondicion = 0

            Dim prioridad As Integer
            Dim colAutoCliente As String
            Dim colAutoBCD As String
            Dim cadena As String
            Dim tipoDato As String
            Dim tipoOperacion As String


            Dim lista = proveedor.conciliaCionAutoLista(Me._idProveedor)

            For Each columna In lista

                prioridad = 0

                colAutoCliente = columna.ColumnaAutomatica
                colAutoBCD = columna.ColumnaAutomaticaBDBCD
                tipoDato = columna.TipoDato
                tipoOperacion = columna.TipoOperacion

                Select Case Me.IdProveedor

                    Case 1 'Únicamente Para POSADAS

                        Select Case colAutoCliente
                            Case "clave"
                                prioridad = 2
                            Case "claveGDS"
                                prioridad = 1
                            Case "llegada"
                                prioridad = 5
                            Case "firstName"
                                prioridad = 3
                            Case "lastName"
                                prioridad = 4
                            Case Else
                                prioridad = 0
                        End Select

                        cadena = "[" & colAutoBCD & " <---> " & colAutoCliente & " ][" & tipoDato & "](" & tipoOperacion & ")----" & prioridad

                    Case 2 'Únicamente Para cityexpress


                        'Select Case colAutoCliente
                        '    Case "Reservacion"
                        '        prioridad = 1
                        '    Case "ReferenciaOTA"
                        '        prioridad = 2
                        '    Case Else
                        '        prioridad = 0
                        'End Select

                        'cadena = "[" & colAutoBCD & " <---> " & colAutoCliente & " ][" & tipoDato & "](" & tipoOperacion & ")----" & prioridad

                    Case 3 'Únicamente Para Onyx


                        Select Case colAutoCliente
                            Case "ConformationNo"
                                prioridad = 1
                            Case Else
                                prioridad = 0
                        End Select

                        cadena = "[" & colAutoBCD & " <---> " & colAutoCliente & " ][" & tipoDato & "](" & tipoOperacion & ")----" & prioridad

                    Case 4 'Únicamente Para Tacs

                        Select Case colAutoCliente
                            Case "Confirmation"
                                prioridad = 1
                            Case "FirstName"
                                prioridad = 2
                            Case "LastName"
                                prioridad = 3
                            Case Else
                                prioridad = 0
                        End Select

                        cadena = "[" & colAutoBCD & " <---> " & colAutoCliente & " ][" & tipoDato & "](" & tipoOperacion & ")----" & prioridad

                    Case 19 'Únicamente Para Gestion Commtrack


                        Select Case colAutoCliente
                            Case "Confirmationcode"
                                prioridad = 1
                            Case "FirstName"
                                prioridad = 2
                            Case "LastName"
                                prioridad = 3
                            Case Else
                                prioridad = 0
                        End Select

                        cadena = "[" & colAutoBCD & " <---> " & colAutoCliente & " ][" & tipoDato & "](" & tipoOperacion & ")----" & prioridad

                    Case Else

                        cadena = "[" & colAutoBCD & " <---> " & colAutoCliente & " ][" & tipoDato & "](" & tipoOperacion & ")"

                End Select

                If (cadena <> "") Then
                    Me.NombreCondicion = cadena
                    Me.AddCondicion()
                End If


            Next

        End Sub


        Public Sub condicionesManuales()

            Me.TipoGrupoCondicion = 1

            Select Case Me.IdProveedor

                Case 1 'Únicamente Para POSADAS MANUAL

                    Me.NombreCondicion = "[firstName <---> firstName][TEXTO](CONTIENE)----3"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[lastName <---> lastName][TEXTO](CONTIENE)----2"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateIn <---> llegada][FECHA](IGUALDAD)----4"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateOut <---> salida][FECHA](RANGO)(2)----5"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[HotelName <---> hotel][TEXTO](CONTIENE)----1"
                    Me.AddCondicion()

                Case 2 'Únicamente Para CITYEXPERESS MANUAL Then

                    Me.NombreCondicion = "[firstName <---> firstName][TEXTO](CONTIENE)----3"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[lastName <---> lastName][TEXTO](CONTIENE)----4"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateIn <---> CheckIn][FECHA](IGUALDAD)----1"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateOut <---> CheckOut][FECHA](RANGO)(2)----2"
                    Me.AddCondicion()

                Case 3 'Únicamente Para OnyxPagadas MANUAL

                    Me.NombreCondicion = "[firstName <---> firstName][TEXTO](CONTIENE)----4"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[lastName <---> lastName][TEXTO](CONTIENE)----5"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateIn <---> DateIn][FECHA](IGUALDAD)----2"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateOut <---> DateOut][FECHA](RANGO)(2)----3"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[HotelName <---> HotelName][TEXTO](CONTIENE)----1"
                    Me.AddCondicion()


                Case 4 'Únicamente Para Tacs MANUAL

                    Me.NombreCondicion = "[firstName <---> FirstName][TEXTO](CONTIENE)----3"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[lastName <---> LastName][TEXTO](CONTIENE)----4"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateIn <---> Arrival][FECHA](IGUALDAD)----1"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateOut <---> Departure][FECHA](RANGO)(2)----2"
                    Me.AddCondicion()

                Case 19 'Únicamente Para COMMTRACK MANUAL

                    Me.NombreCondicion = "[firstName <---> First][TEXTO](CONTIENE)----3"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[lastName <---> Last][TEXTO](CONTIENE)----4"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateIn <---> DIN][FECHA](IGUALDAD)----1"
                    Me.AddCondicion()

                    Me.NombreCondicion = "[DateOut <---> OUT][FECHA](RANGO)(2)----2"
                    Me.AddCondicion()

                Case Else



            End Select

        End Sub

        Public Sub condicionesAutoExtras()

            Me.TipoGrupoCondicion = 0

            Select Case Me.IdProveedor

                Case 1 'Únicamente Para POSADAS 



                Case 2 'Únicamente Para CITYEXPERESS  

                    Select Case Me.ValidarFormatoCityExpress
                        Case 1
                            Me.NombreCondicion = "[conformationNo <---> Reservacion][TEXTO](IGUALDAD)----1"
                            Me.AddCondicion()
                            Me.NombreCondicion = "[conformationNo <---> ReferenciaOTA][TEXTO](IGUALDAD)----2"
                            Me.AddCondicion()
                        Case 2
                            Me.NombreCondicion = "[conformationNo <---> conformationNo][TEXTO](IGUALDAD)----1"
                            Me.AddCondicion()
                            'Case Else
                            '    Me.NombreCondicion = "[UniqueBookingID|{LineNo} <---> AgentRef3|{LineNo}][TEXTO](IGUALDAD)----2"
                            '    Me.AddCondicion()
                    End Select


                Case 3 'Únicamente Para OnyxPagadas 

                    Select Case Me.ValidarOnyx
                        Case 1
                            Me.NombreCondicion = "[UniqueBookingID|{LineNo} <---> AgentRef2|{LineNo}][TEXTO](IGUALDAD)----2"
                            Me.AddCondicion()
                        Case 2
                            Me.NombreCondicion = "[AgentRef3|{LineNo} <---> AgentRef3|{LineNo}][TEXTO](IGUALDAD)----2"
                            Me.AddCondicion()
                        Case Else
                            Me.NombreCondicion = "[UniqueBookingID|{LineNo} <---> AgentRef2|{LineNo}][TEXTO](IGUALDAD)----2"
                            Me.AddCondicion()
                    End Select



                Case 4 'Únicamente Para Tacs 


                Case 19 'Únicamente Para COMMTRACK 

                    Me.NombreCondicion = "[UniqueBookingID|{LineNo} <---> Trans|{segnum}][TEXTO](IGUALDAD)----2"
                    Me.AddCondicion()

                Case 20 'Únicamente Para PREPAGO 

                    Me.NombreCondicion = "[UniqueBookingID|{LineNo} <---> numTransaccion|{noSegmento}][TEXTO](IGUALDAD)----1"
                    Me.AddCondicion()

                Case Else

            End Select

        End Sub



    End Class



End Namespace

