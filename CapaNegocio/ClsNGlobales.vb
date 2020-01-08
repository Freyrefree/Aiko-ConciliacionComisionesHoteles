Namespace CapaNegocio

    Public Class ClsNGlobales

        Private Shared v_FechaPagoProveedor As String = ""
        Private Shared v_fechaProveedorInicio As String = ""
        Private Shared v_fechaProveedorFin As String = ""

        Private Shared v_mesProveedor As String = ""
        Private Shared v_anioProveedor As String = ""
        Private Shared v_lastId As Int64 = 0
        Private Shared v_idProveedor As String = ""
        Private Shared v_tipoPlantillaCityExpress As Integer

        Private Shared v_eliminarCanceladosBCD As Integer

        Private Shared v_ActuaizarSegmento As Integer

        Public Shared Property ActuaizarSegmento As Integer

            Get
                Return v_ActuaizarSegmento
            End Get
            Set(ByVal value As Integer)
                v_ActuaizarSegmento = value
            End Set

        End Property

        Public Shared Property EliminarCanceladosBCD As String

            Get
                Return v_eliminarCanceladosBCD
            End Get
            Set(ByVal value As String)
                v_eliminarCanceladosBCD = value
            End Set

        End Property

        Public Shared Property TipoPlantillaCityExpress As String

            Get
                Return v_tipoPlantillaCityExpress
            End Get
            Set(ByVal value As String)
                v_tipoPlantillaCityExpress = value
            End Set

        End Property


        Public Shared Property idProveedor As String

            Get
                Return v_idProveedor
            End Get
            Set(ByVal value As String)
                v_idProveedor = value
            End Set

        End Property

        Public Shared Property MesProveedor As String

            Get
                Return v_mesProveedor
            End Get
            Set(ByVal value As String)
                v_mesProveedor = value
            End Set

        End Property


        Public Shared Property AnioProveedor As String

            Get
                Return v_anioProveedor
            End Get
            Set(ByVal value As String)
                v_anioProveedor = value
            End Set

        End Property

        Public Shared Property LastID As String

            Get
                Return v_lastId
            End Get
            Set(ByVal value As String)
                v_lastId = value
            End Set

        End Property

        Public Shared Property FechaPagoproveedor As String

            Get
                Return v_FechaPagoProveedor
            End Get
            Set(ByVal value As String)
                v_FechaPagoProveedor = value
            End Set

        End Property

        Public Shared Property FechaProveedorInicio As String

            Get
                Return v_fechaProveedorInicio
            End Get
            Set(ByVal value As String)
                v_fechaProveedorInicio = value
            End Set

        End Property

        Public Shared Property FechaProveedorFin As String

            Get
                Return v_fechaProveedorFin
            End Get
            Set(ByVal value As String)
                v_fechaProveedorFin = value
            End Set

        End Property



    End Class

End Namespace
