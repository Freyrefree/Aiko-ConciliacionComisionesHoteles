Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports CapaDatos.CapaDatos

Public Class ClsConciliaciones

    Private conexion As ClsConexion = New ClsConexion()
    Private leer As SqlDataReader
    Private tabla As DataTable = New DataTable()
    Private comando As SqlCommand = New SqlCommand()
    Private ReadOnly objetoCapaDatos As Object

    'Varibles para fecha de Proveedores
    Public mesProveedor As String
    Public anioProveedor As String



    '***************************************************** POSADAS ****************************************************************

    Public Function CD_CriteriosAutomaticosPosadas1() As DataTable

        Dim tablaCrit1 As DataTable = New DataTable()
        tablaCrit1.Rows.Clear()

        Dim procedure As String = "conciliacionCriterio1Posadas"

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = procedure
            comando.CommandType = CommandType.StoredProcedure
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaCrit1.Load(leer)

            Return tablaCrit1

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR 007 Conciliacion")
            Return tablaCrit1

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_CriteriosAutomaticosPosadas2() As DataTable

        Dim tablaCrit2 As DataTable = New DataTable()


        Dim procedure As String = "conciliacionCriterio2Posadas"

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = procedure
            comando.CommandType = CommandType.StoredProcedure
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaCrit2.Load(leer)

            Return tablaCrit2

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR 1 Conciliacion")
            Return tablaCrit2

        Finally

            conexion.CerrarConexion()

        End Try


    End Function


    Public Function CD_EstatusPendientesPosadas()


        Try

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = "estatusCoinciliacionAuto"
            comando.CommandType = CommandType.StoredProcedure
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.Parameters.Clear()
            Dim res As String = comando.ExecuteNonQuery()
            Return res

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR 2 Conciliacion")
            Return False
        Finally

            conexion.CerrarConexion()

        End Try



    End Function

    Public Function CD_ConsultaPendientesPosadas() As DataTable



        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

        'Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
        Dim queryFechaProveedor As String = " AND mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"



        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "



        Dim tablaPen As DataTable = New DataTable()

        Dim query As String = "SELECT * FROM posadas WHERE estatusConciliado IS NULL " & queryFechaProveedor & queryEstatusEliminado

        'Dim procedure As String = "SP_CoinciliacionesPendientesPosadas"

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaPen.Load(leer)

            Return tablaPen

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  3 Conciliacion")
            Return tablaPen

        Finally

            conexion.CerrarConexion()

        End Try

    End Function

    Public Function CD_ConsultaPendientesBDBCD() As DataTable
        Dim tablaPenBDBCD As DataTable = New DataTable()

        Dim query As String = "SELECT * FROM BDBCD WHERE estatusConciliado IS NULL"

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            leer = comando.ExecuteReader()
            tablaPenBDBCD.Load(leer)

            Return tablaPenBDBCD

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  4 Conciliacion")
            Return tablaPenBDBCD

        Finally

            conexion.CerrarConexion()

        End Try

    End Function

    Public Sub CD_Null_CondicionNOAuto()

        'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


        Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

        Dim query As String = "UPDATE posadas SET CondicionNOAuto = NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            'comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  03454 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try

    End Sub
    Public Sub CD_Null_CondicionNOManual()

        'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

        Dim query As String = "UPDATE posadas SET CondicionNOManual = NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            'comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery

        Catch ex As Exception
            MsgBox(ex.Message & " " & "ERROR  03455 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub

    Public Sub CD_Null_CondicionNOAutoCityExpress()

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " WHERE mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL"

        Dim query As String = "UPDATE cityexpress SET CondicionNOAuto = NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            'comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  03455 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try
    End Sub





    Public Sub CD_Null_CondicionNOAutoGestionCommtrack()

        Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " WHERE mesProveedor ='" & fechaproveedor & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

        Dim query As String = "UPDATE gestionCommtrack SET CondicionNOAuto = NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            'comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  03457 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub

    Public Sub CD_testPosadas(pivote, condicionOk, colProv, colBCD, fechas)

        'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "' "
        Dim queryEstatusEliminado As String = " WHERE proveedor.estatusEliminado IS NULL "

        'Dim query As String = "UPDATE posadas
        'SET CondicionOKManual = 
        'CASE WHEN CHARINDEX('" & columnaProveedor & "',CondicionOKManual) = 0 OR CHARINDEX(' " & columnaProveedor & " ',CondicionOKManual) IS NULL  
        'THEN CONCAT(CondicionOKManual,'" & columnaProveedor & "') ELSE replace(CondicionOKManual,'" & columnaProveedor & "','" & columnaProveedor & "') END

        'FROM  posadas proveedor
        'INNER JOIN BDBCD BD " & moreQuery & masqueryLast & fechas

        Dim query As String = "UPDATE posadas
        SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & colProv & "',CondicionNOAuto) = 0 OR CHARINDEX(' " & colProv & " ',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & colProv & "') ELSE replace(CondicionNOAuto,'" & colProv & "','" & colProv & "') END,
        idBDBCD=BD.id
        FROM  posadas proveedor
        INNER JOIN BDBCD BD " & pivote & condicionOk & " AND  proveedor.estatusConciliado  IS NULL " & colBCD & fechas & queryFechaProveedor & queryEstatusEliminado

        Dim dsds As String = ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  876 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try




    End Sub



    Public Function CD_SumaCumplidosByID(queryEnvio)

        Dim tabla As DataTable = New DataTable()

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryEnvio
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10
            leer = comando.ExecuteReader()
            tabla.Load(leer)
            Return tabla

        Catch ex As Exception
            Return tabla
            MsgBox(ex.Message & " " & " ERROR  A1 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Function

    Public Sub CD_VerificarCondicionesPosadasB(columnaProveedor, idBCD, idProveedor)

        Dim query As String = "UPDATE posadas SET camposConciliados = CONCAT(camposConciliados,'[" & idBCD & "|" & columnaProveedor & "]-') WHERE id = " & idProveedor & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10
            comando.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  A2 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub

    Public Function CD_CondicionesCumplidasByID(queryEnvioB, queryBB)

        queryEnvioB &= queryBB
        queryEnvioB = queryEnvioB.Replace("UNION)", ")")

        Dim condicionesCumplidas As Object

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryEnvioB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos

            condicionesCumplidas = comando.ExecuteScalar()

            Return condicionesCumplidas.ToString


        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR A11  Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try





    End Function


    Public Sub CD_CondicionesCumplidasPosadas(queryAll)


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryAll
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  877 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub




    Public Sub CD_CondicionesCumplidasTacs(queryAll)


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryAll
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  877.4 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub


    Public Sub CD_CondicionesCumplidasGestionCommtrack(queryAll)

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryAll
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  877.5 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub

    Public Sub CD_CondicionesCumplidasOnyx(queryAll)


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryAll
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  877.3 Conciliacion")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub

    Public Sub CD_ColumnasCumplidasAuto(id, CondicionNOAuto)

        Dim queryA As String = "UPDATE posadas SET CondicionOKAuto = '" & CondicionNOAuto & "' WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  08764 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub



    Public Sub CD_ResetCondicionNOAuto(id)

        Dim queryA As String = "UPDATE posadas SET CondicionNOAuto = NULL WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  08764 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub



    Public Sub CD_CountFaltantesPosadas(id, count)

        Dim query As String = "UPDATE posadas SET countNoCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  08735 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try



    End Sub



    Public Sub CD_CountCumplidosPosadas(id, count)

        Dim query As String = "UPDATE posadas SET countCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  08745 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try


    End Sub



    Public Sub CD_conlumnaNoConciliada(id, columna)


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim query As String = "UPDATE posadas SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & columna & "',CondicionNOAuto) = 0 OR CHARINDEX('" & columna & "',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & columna & "') ELSE replace(CondicionNOAuto,'" & columna & "','" & columna & "') END
         WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  08765 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try

    End Sub



    Public Sub CD_testPosadas2(count)

        Dim query As String = "update posadas set countNoCumplidoManual = " & count & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  876 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try




    End Sub

    Public Function CD_ConsultasAutomatico(moreQuery, lastPartQuery, masqueryLast, fechas)

        Dim IDBCD As Integer
        Dim idProveedor As Integer

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryFechaProveedor As String = " AND proveedor.mesproveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID AS dim_value,
       '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        proveedor.comision AS Comision,
        'Posadas' AS Operador,
        proveedor.moneda AS Moneda,
        totalDeLaReserva AS CostoTotalDeLaReserva,
        noNoches AS Noches,
        proveedor.comision As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Automatica' As tipoConciliacion
        FROM  posadas proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masqueryLast & fechas & queryFechaProveedor & queryEstatusEliminado & "
        "

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)

            'Dim duplicates = tablaConciliacionAutomatica.AsEnumerable().GroupBy(Function(r) r(1)).Where(Function(gr) gr.Count() > 1)

            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")




            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE posadas SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='posadas',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  55 Conciliacion")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function






    Public Function CD_Consultas(moreQuery, lastPartQuery, fechas)

        Dim IDBCD As Integer
        Dim idProveedor As Integer

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


        Dim queryFechaProveedor As String = " AND proveedor.mesproveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionManual As DataTable = New DataTable()

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.conformationNo AS CodigoConfirmacion,
        proveedor.comision AS Comision,
        'Posadas' AS Operador,
        proveedor.moneda AS Moneda,
        totalDeLaReserva AS CostoTotalDeLaReserva,
        noNoches AS Noches,
        proveedor.comision As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Manual' As tipoConciliacion
        FROM  posadas proveedor 
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionManual.Load(leer)

            '''''''  DUPLICADOS  '''''''
            tablaConciliacionManual = quitarDuplicados(tablaConciliacionManual, "idProveedor")
            tablaConciliacionManual = quitarDuplicados(tablaConciliacionManual, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionManual.Rows.Count > 0) Then

                queryEstatusA = ""
                queryEstatusB = ""

                For Each row As DataRow In tablaConciliacionManual.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty

                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE posadas SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='posadas',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Return tablaConciliacionManual

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion")
            Return tablaConciliacionManual

        Finally

            conexion.CerrarConexion()

        End Try


    End Function





    Public Function CD_ResetEstatusPosadas()

        'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
        Dim resss As Boolean

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryEstatusEliminadoA As String = " AND estatusEliminado IS NULL "
        Dim queryEstatusEliminadoB As String = " AND proveedor.estatusEliminado IS NULL "


        Dim queryA As String = "UPDATE posadas set estatusConciliado = NULL WHERE mesProveedor = '" & fechaProveedor & "'" & queryEstatusEliminadoA

        Dim queryB As String = "update BDBCD set 
        estatusConciliado = null,
        proveedor = null,
        mesProveedor = null
        WHERE proveedor = 'posadas' AND mesProveedor = '" & fechaProveedor & "' "

        Dim queryC As String = "UPDATE posadas SET CondicionOKAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryC &= "UPDATE posadas SET CondicionNOAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryC &= "UPDATE posadas SET countCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryC &= "UPDATE posadas SET countNoCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryC &= "UPDATE posadas SET idBDBCD = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"


        Dim queryH As String = "UPDATE posadas SET CondicionOKManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE posadas SET CondicionNOManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE posadas SET countCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE posadas SET countNoCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE posadas SET idBDBCDManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"



        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryH
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resss = comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryC
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try






        Dim resA As Boolean = False
        Dim resB As Boolean = False




        '''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resA = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 0099 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resB = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 00100 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''''''''''

        If (resA And resB) Then
            Return True
        Else
            Return False

        End If

    End Function


    Public Function CD_ResetEstatusCityExpress()

        'Dim fechaProveedor As String = anioProveedor & "-" & mesProveedor & "-" & "01"
        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryEstatusEliminadoA As String = " AND estatusEliminado IS NULL "



        Dim queryA As String = "UPDATE cityexpress SET estatusConciliado  = NULL WHERE mesProveedor = '" & fechaProveedor & "'" & queryEstatusEliminadoA

        Dim queryB As String = "UPDATE BDBCD SET 
        estatusConciliado  = NULL,
        mesProveedor = NULL,
        proveedor = NULL
        WHERE proveedor = 'cityexpress' AND mesProveedor = '" & fechaProveedor & "' "

        Dim queryC As String = "UPDATE cityexpress SET CondicionOKAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryD As String = "UPDATE cityexpress SET CondicionNOAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryE As String = "UPDATE cityexpress SET countCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryF As String = "UPDATE cityexpress SET countNoCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryG As String = "UPDATE cityexpress SET idBDBCD = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA

        Dim queryH As String = "UPDATE cityexpress SET CondicionOKManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE cityexpress SET CondicionNOManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE cityexpress SET countCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE cityexpress SET countNoCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryH &= "UPDATE cityexpress SET idBDBCDManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"



        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryH
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Dim resA As Boolean = False
        Dim resB As Boolean = False

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryC
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryD
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryE
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryF
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryG
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        '''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resA = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 0099 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resB = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 00100 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''''''''''

        If (resA And resB) Then
            Return True
        Else
            Return False

        End If

    End Function


    Public Function CD_ResetEstatusOnyx()

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryEstatusEliminadoA As String = " AND estatusEliminado IS NULL "

        Dim queryA As String = "UPDATE onyxPagadas SET estatusConciliado  = null WHERE mesProveedor = '" & fechaProveedor & "'" & queryEstatusEliminadoA

        Dim queryB As String = "UPDATE BDBCD  SET
        estatusConciliado  = NULL,
        mesProveedor = NULL,
        proveedor = NULL
        WHERE proveedor = 'onyx' AND mesProveedor = '" & fechaProveedor & "' "

        Dim queryC As String = "UPDATE onyxPagadas SET CondicionOKAuto = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryD As String = "UPDATE onyxPagadas SET CondicionNOAuto = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryE As String = "UPDATE onyxPagadas SET countCumplidoAuto = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryF As String = "UPDATE onyxPagadas SET countNoCumplidoAuto = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryG As String = "UPDATE onyxPagadas SET idBDBCD = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA

        Dim queryH As String = "UPDATE onyxObservaciones SET estatusConciliado = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA

        Dim queryI As String = "UPDATE onyxPagadas SET CondicionOKManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE onyxPagadas SET CondicionNOManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE onyxPagadas SET countCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE onyxPagadas SET countNoCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE onyxPagadas SET idBDBCDManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"



        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryI
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryC
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryD
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryE
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryF
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryG
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryH
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try




        Dim resA As Boolean = False
        Dim resB As Boolean = False

        '''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resA = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 0099 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resB = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 00100 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''''''''''

        If (resA And resB) Then
            Return True
        Else
            Return False

        End If

    End Function


    Public Function CD_ResetEstatusTacs()

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryEstatusEliminadoA As String = " AND estatusEliminado IS NULL "


        Dim queryA As String = "update tacsPagadas set estatusConciliado  = NULL WHERE mesProveedor = '" & fechaProveedor & "'" & queryEstatusEliminadoA
        Dim queryB As String = "update BDBCD set 
estatusConciliado  = null,
proveedor = null,
mesProveedor = null
WHERE proveedor = 'tacs' AND mesProveedor = '" & fechaProveedor & "' "

        Dim queryC As String = "UPDATE tacsPagadas SET CondicionOKAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryD As String = "UPDATE tacsPagadas SET CondicionNOAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryE As String = "UPDATE tacsPagadas SET countCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryF As String = "UPDATE tacsPagadas SET countNoCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryG As String = "UPDATE tacsPagadas SET idBDBCD = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA

        Dim queryH As String = "UPDATE tacsObservaciones SET estatusConciliado = NULL  WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA

        Dim queryI As String = "UPDATE tacsPagadas SET CondicionOKManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE tacsPagadas SET CondicionNOManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE tacsPagadas SET countCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE tacsPagadas SET countNoCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE tacsPagadas SET idBDBCDManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"



        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryI
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryC
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryD
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryE
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryF
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryG
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryH
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try




        Dim resA As Boolean = False
        Dim resB As Boolean = False

        '''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resA = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 0099 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resB = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 00100 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''''''''''

        If (resA And resB) Then
            Return True
        Else
            Return False

        End If

    End Function




    Public Function CD_ResetEstatusGestionCommtrack()

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryEstatusEliminadoA As String = " AND estatusEliminado IS NULL "


        Dim queryA As String = "UPDATE gestionCommtrack set estatusConciliado  = NULL WHERE mesProveedor = '" & fechaProveedor & "'" & queryEstatusEliminadoA

        Dim queryB As String = "UPDATE BDBCD set 
estatusConciliado  = null,
proveedor = null,
mesProveedor = null
WHERE proveedor = 'gestionCommtrack' AND mesProveedor = '" & fechaProveedor & "' "


        Dim queryC As String = "UPDATE gestionCommtrack SET CondicionOKAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryD As String = "UPDATE gestionCommtrack SET CondicionNOAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryE As String = "UPDATE gestionCommtrack SET countCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryF As String = "UPDATE gestionCommtrack SET countNoCumplidoAuto = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA
        Dim queryG As String = "UPDATE gestionCommtrack SET idBDBCD = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA

        Dim queryI As String = "UPDATE gestionCommtrack SET CondicionOKManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE gestionCommtrack SET CondicionNOManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE gestionCommtrack SET countCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE gestionCommtrack SET countNoCumplidoManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"
        queryI &= "UPDATE gestionCommtrack SET idBDBCDManual = NULL WHERE mesProveedor = '" & fechaProveedor & "' " & queryEstatusEliminadoA & ";"



        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryI
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryC
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryD
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryE
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryF
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryG
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            conexion.CerrarConexion()
        End Try







        Dim resA As Boolean = False
        Dim resB As Boolean = False

        '''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resA = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 0099 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            resB = comando.ExecuteNonQuery()

        Catch ex As Exception

            'MsgBox(ex.Message & " " & "ERROR 00100 Conciliacion")

        Finally

            conexion.CerrarConexion()

        End Try
        ''''''''''''''''''''''''''''''''''''''''''''''

        If (resA And resB) Then
            Return True
        Else
            Return False

        End If

    End Function



    '***********************************************************************************************************************

    '***********************************************  ONYX  ****************************************************************


    Public Sub CD_CountCumplidosOnyx(id, count)

        Dim query As String = "UPDATE onyxPagadas SET countCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  304 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try


    End Sub








    Public Sub CD_testOnyx(pivote, condicionOk, colProv, colBCD, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "' "
        Dim queryEstatusEliminado As String = " WHERE proveedor.estatusEliminado IS NULL "

        Dim query As String = "UPDATE onyxPagadas
        SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & colProv & "',CondicionNOAuto) = 0 OR CHARINDEX(' " & colProv & " ',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & colProv & "') ELSE replace(CondicionNOAuto,'" & colProv & "','" & colProv & "') END,
        idBDBCD=BD.id
        FROM  onyxPagadas proveedor
        INNER JOIN BDBCD BD " & pivote & condicionOk & " AND  proveedor.estatusConciliado  IS NULL " & colBCD & fechas & queryFechaProveedor & queryEstatusEliminado

        Dim dsds As String = ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  700 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try




    End Sub

    Public Function CD_ConsultasAutomaticoOnyxComisionesPendientePago(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaFin As String = ClsGlobales.FechaProveedorFin

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND BD.mesProveedor >= '" & fechaInicio & "' AND   BD.mesProveedor <= '" & fechaFin & "' "

        Dim queryFechaProveedorB As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "' "
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim query = "
        SELECT
        BD.mesProveedor,
        proveedor.id AS UUIDP,
        BD.id AS  UUID
        FROM  onyxComisionesPendientePago proveedor
        INNER JOIN onyxPagadas BD
        " & moreQuery & masQueryLast & queryFechaProveedor


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)

            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5.5 Conciliacion ONYX")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try

    End Function




    Public Function CD_ConsultasAutomaticoOnyx(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN proveedor.PaidCommissionMXN = '' OR proveedor.PaidCommissionMXN IS NULL  THEN proveedor.observaciones ELSE proveedor.PaidCommissionMXN END AS Comision,
        'Onyx' AS Operador,
        proveedor.PaidCurrency AS Moneda,
        CASE
        WHEN PaidCommissionMXN is not null AND CAST(PaidCommissionMXN AS DECIMAL(18, 3)) > 0 THEN
        (CAST(PaidCommissionMXN AS DECIMAL(18, 3)) * 100) / ConfCommissionPercent 
        END AS CostoTotalDeLaReserva,
        proveedor.ConfNoNights AS Noches,
        proveedor.PaidCommission As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Automatica' As tipoConciliacion
        FROM  onyxPagadas proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try

            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)




            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE onyxPagadas SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='onyx',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion ONYX")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasManualOnyx(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionManual As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN proveedor.PaidCommissionMXN = '' OR proveedor.PaidCommissionMXN IS NULL  THEN proveedor.observaciones ELSE proveedor.PaidCommissionMXN END AS Comision,
        'Onyx' AS Operador,
        proveedor.PaidCurrency AS Moneda,
        CASE
        WHEN PaidCommissionMXN is not null AND CAST(PaidCommissionMXN AS DECIMAL(18, 3)) > 0 THEN
        (CAST(PaidCommissionMXN AS DECIMAL(18, 3)) * 100) / ConfCommissionPercent 
        END AS CostoTotalDeLaReserva,
        proveedor.ConfNoNights AS Noches,
        proveedor.PaidCommission As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Manual' As tipoConciliacion
        FROM  onyxPagadas proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado & ""


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 15 ' 15 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionManual.Load(leer)



            '''''''  DUPLICADOS  '''''''
            tablaConciliacionManual = quitarDuplicados(tablaConciliacionManual, "idProveedor")
            tablaConciliacionManual = quitarDuplicados(tablaConciliacionManual, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionManual.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionManual.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE onyxPagadas SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='onyx',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            Return tablaConciliacionManual

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  6 Conciliacion ONYX")
            Return tablaConciliacionManual

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasAutomaticoOnyxObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "
        Dim queryBookingStatusCode As String = "
        AND proveedor.BookingStatusCode <> 'OK'
        "

        Dim tablaConciliacionAutomaticaObservaciones As DataTable = New DataTable()

        Dim query = "SELECT
        proveedor.id as idProveedor,
        BD.id as idBDBCD,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN proveedor.PaidCommissionMXN = '' OR proveedor.PaidCommissionMXN IS NULL  THEN proveedor.observaciones ELSE proveedor.PaidCommissionMXN END AS Comision,
        'Onyx' AS Operador,
        proveedor.PaidCurrency AS Moneda,
        CASE
        WHEN PaidCommissionMXN is not null AND CAST(PaidCommissionMXN AS DECIMAL(18, 3)) > 0 THEN
        (CAST(PaidCommissionMXN AS DECIMAL(18, 3)) * 100) / ConfCommissionPercent 
        END AS CostoTotalDeLaReserva,
        proveedor.ConfNoNights AS Noches,
        proveedor.PaidCommission As ComOrig,
        BD.SequenceNo As SequenceNo,
        proveedor.BookingStatusCode As BookingStatusCode,
        'Conciliacion Automatica Observaciones' As tipoConciliacion
        FROM  onyxObservaciones proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado & queryBookingStatusCode


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomaticaObservaciones.Load(leer)

            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomaticaObservaciones = quitarDuplicados(tablaConciliacionAutomaticaObservaciones, "idProveedor")
            tablaConciliacionAutomaticaObservaciones = quitarDuplicados(tablaConciliacionAutomaticaObservaciones, "idBDBCD")


            Return tablaConciliacionAutomaticaObservaciones

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion ONYX")
            Return tablaConciliacionAutomaticaObservaciones

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasManualOnyxObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "
        Dim queryBookingStatusCode As String = "
        AND proveedor.BookingStatusCode <> 'OK'
        "

        Dim tablaConciliacionManualObserva As DataTable = New DataTable()

        Dim query = "SELECT
        proveedor.id as idProveedor,
        BD.id as idBDBCD,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN proveedor.PaidCommissionMXN = '' OR proveedor.PaidCommissionMXN IS NULL  THEN proveedor.observaciones ELSE proveedor.PaidCommissionMXN END AS Comision,
        'Onyx' AS Operador,
        proveedor.PaidCurrency AS Moneda,
        CASE
        WHEN PaidCommissionMXN is not null AND CAST(PaidCommissionMXN AS DECIMAL(18, 3)) > 0 THEN
        CAST(PaidCommissionMXN AS DECIMAL(18, 3)) * 100 / ConfCommissionPercent 
        END AS CostoTotalDeLaReserva,
        proveedor.ConfNoNights AS Noches,
        proveedor.PaidCommission As ComOrig,
        BD.SequenceNo As SequenceNo,
        proveedor.BookingStatusCode As BookingStatusCode,
        'Conciliacion Manual Observaciones' As tipoConciliacion
        FROM  onyxObservaciones proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado &
        queryBookingStatusCode


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionManualObserva.Load(leer)


            '''''''  DUPLICADOS  '''''''
            tablaConciliacionManualObserva = quitarDuplicados(tablaConciliacionManualObserva, "idProveedor")
            tablaConciliacionManualObserva = quitarDuplicados(tablaConciliacionManualObserva, "idBDBCD")



            Return tablaConciliacionManualObserva

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  6 Conciliacion ONYX")
            Return tablaConciliacionManualObserva

        Finally

            conexion.CerrarConexion()

        End Try


    End Function






    Public Function CD_ConsultasManualOnyxComisionesPendientePago(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaFin As String = ClsGlobales.FechaProveedorFin

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND BD.mesProveedor >= '" & fechaInicio & "' AND   BD.mesProveedor <= '" & fechaFin & "' "
        Dim queryFechaProveedorB As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "' "
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionManual As DataTable = New DataTable()

        Dim query = "
        SELECT
        BD.mesProveedor,
        proveedor.id AS UUIDP,
        BD.id AS  UUID
        FROM  onyxComisionesPendientePago proveedor
        INNER JOIN onyxPagadas BD
        " & moreQuery & masQueryLast & queryFechaProveedor & queryFechaProveedorB & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionManual.Load(leer)

            Return tablaConciliacionManual

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  6 Conciliacion ONYX")
            Return tablaConciliacionManual

        Finally

            conexion.CerrarConexion()

        End Try


    End Function


    Public Function CD_ConsultaPendientesOnyx() As DataTable


        Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

        Dim queryFechaProveedor As String = " AND mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "



        Dim tablaPen As DataTable = New DataTable()


        Dim query As String = "SELECT * FROM onyxPagadas WHERE estatusConciliado IS NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaPen.Load(leer)

            Return tablaPen

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  39 Conciliacion Onyx")
            Return tablaPen

        Finally

            conexion.CerrarConexion()

        End Try

    End Function

    Public Function CD_ConsultaPendientesTacs() As DataTable

        Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

        Dim queryFechaProveedor As String = " AND mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

        Dim tablaPen As DataTable = New DataTable()

        Dim query As String = "SELECT * FROM tacsPagadas WHERE estatusConciliado IS NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaPen.Load(leer)

            Return tablaPen

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  39 Conciliacion Tacs")
            Return tablaPen

        Finally

            conexion.CerrarConexion()

        End Try

    End Function






    '***********************************************************************************************************************

    '************************************************ CITY EXPRESS ****************************************************************



    Public Sub CD_testCityExpress(pivote, condicionOk, colProv, colBCD, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor

        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "' "
        Dim queryEstatusEliminado As String = " WHERE proveedor.estatusEliminado IS NULL "

        'Dim query As String = "UPDATE posadas
        'SET CondicionOKManual = 
        'CASE WHEN CHARINDEX('" & columnaProveedor & "',CondicionOKManual) = 0 OR CHARINDEX(' " & columnaProveedor & " ',CondicionOKManual) IS NULL  
        'THEN CONCAT(CondicionOKManual,'" & columnaProveedor & "') ELSE replace(CondicionOKManual,'" & columnaProveedor & "','" & columnaProveedor & "') END

        'FROM  posadas proveedor
        'INNER JOIN BDBCD BD " & moreQuery & masqueryLast & fechas

        Dim query As String = "UPDATE cityexpress
        SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & colProv & "',CondicionNOAuto) = 0 OR CHARINDEX(' " & colProv & " ',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & colProv & "') ELSE replace(CondicionNOAuto,'" & colProv & "','" & colProv & "') END,
        idBDBCD=BD.id
        FROM  cityexpress proveedor
        INNER JOIN BDBCD BD " & pivote & condicionOk & " AND  proveedor.estatusConciliado  IS NULL " & colBCD & fechas & queryFechaProveedor & queryEstatusEliminado

        Dim dsds As String = ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  1 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try




    End Sub

    Public Sub CD_ColumnasCumplidasAutoCityExpress(id, CondicionNOAuto)

        Dim queryA As String = "UPDATE cityexpress SET CondicionOKAuto = '" & CondicionNOAuto & "' WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  101 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub

    Public Sub CD_ResetCondicionNOAutoCityExpress(id)

        Dim queryA As String = "UPDATE cityexpress SET CondicionNOAuto = NULL WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  102 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub

    Public Sub CD_conlumnaNoConciliadaCityExpress(id, columna)


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim query As String = "UPDATE cityexpress SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & columna & "',CondicionNOAuto) = 0 OR CHARINDEX('" & columna & "',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & columna & "') ELSE replace(CondicionNOAuto,'" & columna & "','" & columna & "') END
         WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  103 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try

    End Sub

    Public Sub CD_CountCumplidosCityExpress(id, count)

        Dim query As String = "UPDATE cityexpress SET countCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  104 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try


    End Sub

    Public Sub CD_CountFaltantesCityExpress(id, count)

        Dim query As String = "UPDATE cityexpress SET countNoCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  105 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try



    End Sub

    Public Function CD_ConsultasAutomaticoCityExpressFormatoB(moreQuery, lastPartQuery, masQueryLast, fechas)

        'Dim fechaproveedor As String = ClsGlobales.AnioProveedor & "-" & ClsGlobales.MesProveedor & "-" & "01"
        Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor


        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor ='" & fechaproveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        '' ***************************************************************************************** ''

        Dim query = "
            SELECT
            BD.id AS idBDBCD,
            proveedor.id AS idProveedor,
            BD.UniqueBookingID AS dim_value,
            '" & fechaproveedor & "' AS FechaApp,
            BD.HotelPropertyID AS UserSpec,
            BD.[LineNo] AS Segmento,
            BD.ConformationNo AS CodigoConfirmacion,
            proveedor.Comision AS Comision,
            'CityExpress' AS Operador,
            proveedor.Moneda AS Moneda,
            totalDelIngreso AS CostoTotalDeLaReserva,
            proveedor.NoNoches AS Noches,
            proveedor.Comision As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Automatica' As tipoConciliacion
            FROM  cityexpress proveedor
            INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)


            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE cityexpress SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='cityexpress',
                    mesProveedor ='" & fechaproveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  323 Conciliacion CITY EXPRESS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasAutomaticoCityExpress(moreQuery, lastPartQuery, masQueryLast, fechas)

        'Dim fechaproveedor As String = ClsGlobales.AnioProveedor & "-" & ClsGlobales.MesProveedor & "-" & "01"
        Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor


        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor ='" & fechaproveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        '' ***************************************************************************************** ''

        Dim query = "
            SELECT
            BD.id AS idBDBCD,
            proveedor.id AS idProveedor,
            BD.UniqueBookingID AS dim_value,
            '" & fechaproveedor & "' AS FechaApp,
            BD.HotelPropertyID AS UserSpec,
            BD.[LineNo] AS Segmento,
            BD.ConformationNo AS CodigoConfirmacion,
            proveedor.Comision AS Comision,
            'CityExpress' AS Operador,
            proveedor.Moneda AS Moneda,
            Monto AS CostoTotalDeLaReserva,
            proveedor.NoNoches AS Noches,
            proveedor.Comision As ComOrig,
            BD.SequenceNo As SequenceNo,
            'Conciliacion Automatica' As tipoConciliacion
            FROM  cityexpress proveedor
            INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)


            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE cityexpress SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='cityexpress',
                    mesProveedor ='" & fechaproveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  323 Conciliacion CITY EXPRESS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function



    Public Function CD_ConsultasManualCityExpress(moreQuery, lastPartQuery, masQueryLast, fechas)


        Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor


        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor ='" & fechaproveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "
            SELECT
            BD.id AS idBDBCD,
            proveedor.id AS idProveedor,
            BD.UniqueBookingID AS dim_value,
            '" & fechaproveedor & "' AS FechaApp,
            BD.HotelPropertyID AS UserSpec,
            BD.[LineNo] AS Segmento,
            BD.ConformationNo AS CodigoConfirmacion,
            proveedor.Comision AS Comision,
            'CityExpress' AS Operador,
            proveedor.Moneda AS Moneda,
            Monto AS CostoTotalDeLaReserva,
            proveedor.NoNoches AS Noches,
            proveedor.Comision As ComOrig,
            BD.SequenceNo As SequenceNo,
        'Conciliacion Manual' As tipoConciliacion
        FROM  cityexpress proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado & " "


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)




            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE cityexpress SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='cityexpress',
                    mesProveedor ='" & fechaproveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''







            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  326 Conciliacion CITY EXPRESS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function






    Public Function CD_ConsultaPendientesCityExpress() As DataTable

        Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

        Dim queryFechaProveedor As String = " AND mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

        Dim tablaPen As DataTable = New DataTable()

        Dim query As String = "SELECT * FROM cityexpress WHERE estatusConciliado IS NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaPen.Load(leer)

            Return tablaPen

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  39 Conciliacion  CITY EXPRESS")
            Return tablaPen

        Finally

            conexion.CerrarConexion()

        End Try

    End Function

    '************************************************TASC************************************************************************************
    Public Sub CD_CountFaltantesTacs(id, count)

        Dim query As String = "UPDATE tacsPagadas SET countNoCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  405 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub

    Public Sub CD_CountCumplidosTacs(id, count)

        Dim query As String = "UPDATE tacsPagadas SET countCumplidoAuto = " & count & " WHERE id  = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  404 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try


    End Sub



    Public Sub CD_conlumnaNoConciliadaTacs(id, columna)


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim query As String = "UPDATE tacsPagadas SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & columna & "',CondicionNOAuto) = 0 OR CHARINDEX('" & columna & "',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & columna & "') ELSE replace(CondicionNOAuto,'" & columna & "','" & columna & "') END
         WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  403 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try

    End Sub




    Public Sub CD_ResetCondicionNOAutoTacs(id)

        Dim queryA As String = "UPDATE tacsPagadas SET CondicionNOAuto = NULL WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  402 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub


    Public Sub CD_ColumnasCumplidasAutoTacs(id, CondicionNOAuto)

        Dim queryA As String = "UPDATE tacsPagadas SET CondicionOKAuto = '" & CondicionNOAuto & "' WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  401 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub



    Public Function CD_ConsultasAutomaticoTascObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        '' AS Comision,
        'Tacs' AS Operador,
        '' AS Moneda,
        ReportRevenue AS CostoTotalDeLaReserva,
        proveedor.RoomNights AS Noches,
        '' As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Automatica Observaciones' As tipoConciliacion
        FROM  tacsObservaciones proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)


            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE tacsObservaciones SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='tacs',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion TACS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasManualTascObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        '' AS Comision,
        'Tacs' AS Operador,
        '' AS Moneda,
        ReportRevenue AS CostoTotalDeLaReserva,
        proveedor.RoomNights AS Noches,
        '' As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Manual Observaciones' As tipoConciliacion
        FROM  tacsObservaciones proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)


            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE tacsObservaciones SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='tacs',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  6 Conciliacion TACS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasAutomaticoTasc(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

           Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        '' ***************************************************************************************** ''

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN 
        proveedor.PayCom   = 0
        OR proveedor.PayCom IS NULL
        THEN  proveedor.observaciones ELSE CAST(proveedor.PayComTC as varchar(50)) END AS Comision,
        'Tacs' AS Operador,
        proveedor.PayCurrencyTC AS Moneda,
        ReportRevenue AS CostoTotalDeLaReserva,
        proveedor.RoomNights AS Noches,
        proveedor.PayComTC As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Automatica' As tipoConciliacion
        FROM  tacsPagadas proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)




            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE tacsPagadas SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='tacs',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion TACS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasManualTasc(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN 
        proveedor.PayCom   = 0
        OR proveedor.PayCom IS NULL
        THEN  proveedor.observaciones ELSE CAST(proveedor.PayComTC as varchar(50)) END AS Comision,
        'Tacs' AS Operador,
        proveedor.PayCurrencyTC AS Moneda,
        ReportRevenue AS CostoTotalDeLaReserva,
        proveedor.RoomNights AS Noches,
        proveedor.PayComTC As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Manual' As tipoConciliacion
        FROM  tacsPagadas proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)


            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE tacsPagadas SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='tacs',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  6 Conciliacion TACS")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function





    '***********************************************  Gestion Commtrack  ****************************************************************


    Public Sub CD_ResetCondicionNOAutoGestionCommtrack(id)

        Dim queryA As String = "UPDATE gestionCommtrack SET CondicionNOAuto = NULL WHERE id = " & id & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            leer = comando.ExecuteReader()


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  19003 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try
    End Sub




    Public Sub CD_testGestionCommtrack(pivote, condicionOk, colProv, colBCD, fechas)

        Dim fechaproveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor ='" & fechaproveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL "


        Dim query As String = "UPDATE gestionCommtrack
        SET CondicionNOAuto = 
        CASE WHEN CHARINDEX('" & colProv & "',CondicionNOAuto) = 0 OR CHARINDEX(' " & colProv & " ',CondicionNOAuto) IS NULL  
        THEN CONCAT(CondicionNOAuto,'" & colProv & "') ELSE replace(CondicionNOAuto,'" & colProv & "','" & colProv & "') END,
        idBDBCD=BD.id
        FROM  gestionCommtrack proveedor
        INNER JOIN BDBCD BD " & pivote & condicionOk & " AND  proveedor.estatusConciliado  IS NULL " & colBCD & fechas & queryFechaProveedor & queryEstatusEliminado

        Dim dsds As String = ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 10 minutos
            Dim res As Boolean = comando.ExecuteNonQuery


        Catch ex As Exception

            MsgBox(ex.Message & " " & " ERROR  19001 Conciliacion")


        Finally

            conexion.CerrarConexion()

        End Try




    End Sub

    Public Function CD_ConsultasAutomaticoGestionCommtrack(moreQuery, lastPartQuery, masQueryLast, fechas)


        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

                Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32



        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN proveedor.Observaciones <> '' OR proveedor.Observaciones IS NULL  
THEN proveedor.observaciones ELSE '' END AS Comision,
        'gestionCommtrack' AS Operador,
        proveedor.Curr AS Moneda,
        Montototaldelareserva AS CostoTotalDeLaReserva,
        proveedor.nitec AS Noches,
        proveedor.PAID_AGY As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Automatica' As tipoConciliacion
        FROM  gestionCommtrack proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)

            '''''''  DUPLICADOS  '''''''
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idProveedor")
            tablaConciliacionAutomatica = quitarDuplicados(tablaConciliacionAutomatica, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionAutomatica.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionAutomatica.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE gestionCommtrack SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='gestionCommtrack',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion gestionCommtrack")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function

    Public Function CD_ConsultasManualGestionCommtrack(moreQuery, lastPartQuery, masQueryLast, fechas)

        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionManual As DataTable = New DataTable()

        Dim queryEstatusA As String
        Dim queryEstatusB As String

        Dim IDBCD As Int32
        Dim idProveedor As Int32

        Dim query = "SELECT
        BD.id AS idBDBCD,
        proveedor.id AS idProveedor,
        BD.UniqueBookingID as dim_value,
        '" & fechaProveedor & "' AS FechaApp,
        BD.HotelPropertyID AS UserSpec,
        BD.[LineNo] AS Segmento,
        BD.ConformationNo AS CodigoConfirmacion,
        CASE WHEN proveedor.Observaciones <> '' OR proveedor.Observaciones IS NULL  
THEN proveedor.observaciones ELSE '' END AS Comision,
        'gestionCommtrack' AS Operador,
        proveedor.Curr AS Moneda,
        Montototaldelareserva AS CostoTotalDeLaReserva,
        proveedor.nitec AS Noches,
        proveedor.PAID_AGY As ComOrig,
        BD.SequenceNo As SequenceNo,
        'Conciliacion Manual' As tipoConciliacion
        FROM  gestionCommtrack proveedor
        INNER JOIN BDBCD BD
        " & moreQuery & lastPartQuery & masQueryLast & fechas & queryFechaProveedor & queryEstatusEliminado


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionManual.Load(leer)



            '''''''  DUPLICADOS  '''''''
            tablaConciliacionManual = quitarDuplicados(tablaConciliacionManual, "idProveedor")
            tablaConciliacionManual = quitarDuplicados(tablaConciliacionManual, "idBDBCD")


            ''''''' Cambiar estatus ''''''
            If (tablaConciliacionManual.Rows.Count > 0) Then
                queryEstatusA = ""
                queryEstatusB = ""


                For Each row As DataRow In tablaConciliacionManual.Rows

                    IDBCD = vbEmpty
                    idProveedor = vbEmpty
                    'queryEstatusA = vbEmpty
                    'queryEstatusB = vbEmpty


                    IDBCD = row("idBDBCD")
                    idProveedor = row("idProveedor")

                    queryEstatusA &= "UPDATE gestionCommtrack SET estatusConciliado = 1 WHERE id = " & idProveedor & "; "

                    queryEstatusB &= "UPDATE BDBCD
                    SET estatusConciliado = 1,
                    proveedor ='gestionCommtrack',
                    mesProveedor ='" & fechaProveedor & "'  WHERE id = " & IDBCD & "; "

                Next

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusA
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

                comando.Connection = conexion.AbrirConexion()
                comando.CommandText = queryEstatusB
                comando.CommandType = CommandType.Text
                comando.CommandTimeout = 60 * 10 ' 10 minutos
                comando.ExecuteNonQuery()

            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            Return tablaConciliacionManual

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  5 Conciliacion gestionCommtrack")
            Return tablaConciliacionManual

        Finally

            conexion.CerrarConexion()

        End Try


    End Function




    Public Function CD_ConsultaPendientesGestionCommtrack() As DataTable

        Dim fechaProveedorInicio As String = ClsGlobales.FechaProveedorInicio
        Dim fechaProveedorFin As String = ClsGlobales.FechaProveedorFin

        Dim queryFechaProveedor As String = " AND mesProveedor >= '" & fechaProveedorInicio & "' AND mesProveedor <= '" & fechaProveedorFin & "'"
        Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "

        Dim tablaPen As DataTable = New DataTable()

        Dim query As String = "SELECT * FROM gestionCommtrack WHERE estatusConciliado IS NULL" & queryFechaProveedor & queryEstatusEliminado

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaPen.Load(leer)

            Return tablaPen

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  39 Conciliacion gestionCommtrack")
            Return tablaPen

        Finally

            conexion.CerrarConexion()

        End Try

    End Function


    Public Function CD_ConsultasAutomaticoPrepago(lastPartQuery, fechas)


        Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor
        Dim queryFechaProveedor As String = " AND proveedor.mesProveedor = '" & fechaProveedor & "'"
        Dim queryEstatusEliminado As String = " AND proveedor.estatusEliminado IS NULL"

        Dim tablaConciliacionAutomatica As DataTable = New DataTable()

        Dim query = "SELECT distinct(BD.id) as UUID,
proveedor.id as UUIDP,
        BD.* FROM BDBCD BD
        INNER JOIN prePago proveedor 
        ON CONCAT(proveedor.numTransaccion,proveedor.noSegmento) = CONCAT(BD.UniqueBookingID,BD.[LineNo])"


        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            leer = comando.ExecuteReader()
            tablaConciliacionAutomatica.Load(leer)

            Return tablaConciliacionAutomatica

        Catch ex As Exception

            MsgBox(ex.Message & " " & "ERROR  59876 Conciliacion PrePago")
            Return tablaConciliacionAutomatica

        Finally

            conexion.CerrarConexion()

        End Try


    End Function


    Public Sub elimiarPrepago(idProveedor, idBDBCD)

        Dim queryA As String = "DELETE FROM BDBCD WHERE id = " & idBDBCD & ""
        Dim queryB As String = "DELETE FROM prePago WHERE id = " & idProveedor & ""

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  59876666 Conciliacion PrePago")
        Finally
            conexion.CerrarConexion()
        End Try

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  5987666644 Conciliacion PrePago")
        Finally
            conexion.CerrarConexion()
        End Try




    End Sub

    Public Sub estatusObservacionesOnyx(queryA, queryB)

        'Dim queryA As String = "DELETE FROM BDBCD WHERE id = " & idBDBCD & ""
        'Dim queryB As String = "DELETE FROM prePago WHERE id = " & idProveedor & ""



        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryA
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  59876666 Conciliacion PrePago")
        Finally
            conexion.CerrarConexion()
        End Try

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = queryB
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 10 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  5987666644 Conciliacion PrePago")
        Finally
            conexion.CerrarConexion()
        End Try




    End Sub

    Public Sub actualizarFechaPagoOCPP(idProveedor, mesProveedor)

        Dim anio As String
        Dim mes As String
        Dim dia As String

        Dim fecha As String

        Dim arrayDate() As String

        arrayDate = mesProveedor.Split(New Char() {"/"c})

        anio = arrayDate(2)
        mes = arrayDate(1)
        dia = arrayDate(0)

        fecha = anio & "-" & mes & "-" & dia


        Dim query As String = "UPDATE onyxComisionesPendientePago SET fechaPago = '" & fecha & "' WHERE id = " & idProveedor & " "
        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 5 ' 5 minutos
            comando.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  598FECHAPAGO  onYX")
        Finally
            conexion.CerrarConexion()
        End Try


    End Sub



    '**************************************************************************************************************************



    Public Function ListaColumnBCDByProveedor(idCliente As Int16) As List(Of Listas)

        Dim lista As List(Of Listas) = New List(Of Listas)()

        comando.Connection = conexion.AbrirConexion()
        comando.CommandText = "SP_ColumnsBCDByProveedor"
        comando.Parameters.Clear()
        comando.Parameters.AddWithValue("@idCliente", idCliente)
        comando.CommandType = CommandType.StoredProcedure

        Try
            leer = comando.ExecuteReader
        Catch ex As Exception

            Return lista

        End Try

        While leer.Read()
            Dim item As Listas = New Listas()
            item.idColumaBCDByProveedor = leer("id")
            item.columnBCDByProveedor = leer("nombreColumna")
            item.columnaAuto = leer("columnaAuto")
            lista.Add(item)
        End While

        conexion.CerrarConexion()

        Return lista
    End Function




    Public Function ListaColumnProveedor(idCliente As Int16) As List(Of Listas)

        Dim lista As List(Of Listas) = New List(Of Listas)()


        comando.Connection = conexion.AbrirConexion()
        comando.CommandText = "SP_ColumnsClienteInterfaz"
        comando.Parameters.Clear()
        comando.Parameters.AddWithValue("@idProveedor", idCliente)
        comando.CommandType = CommandType.StoredProcedure

        Try
            leer = comando.ExecuteReader
        Catch ex As Exception
            Return lista
        End Try

        While leer.Read()
            Dim item As Listas = New Listas()
            item.idColumaProveedor = leer("id")
            item.columnProveedor = leer("nombreColumna")
            item.columnaAutoProveedor = leer("esAuto")
            lista.Add(item)
        End While

        conexion.CerrarConexion()

        Return lista

    End Function



    Public Function quitarDuplicados(ByVal dTable As DataTable, ByVal colName As String) As DataTable

        Dim hTable As Hashtable = New Hashtable()
        Dim duplicateList As ArrayList = New ArrayList()

        For Each drow As DataRow In dTable.Rows

            If hTable.Contains(drow(colName)) Then
                duplicateList.Add(drow)
            Else
                hTable.Add(drow(colName), String.Empty)
            End If
        Next

        For Each dRow As DataRow In duplicateList
            dTable.Rows.Remove(dRow)
        Next

        Return dTable

    End Function


    Public Sub CDtemporal(query)

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 20 ' 10 minutos
            comando.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  Pendientes Onix Manual ")
        Finally
            conexion.CerrarConexion()
        End Try


    End Sub

    Public Sub CDtemporalAuto(query)

        Try
            comando.Connection = conexion.AbrirConexion()
            comando.CommandText = query
            comando.CommandType = CommandType.Text
            comando.CommandTimeout = 60 * 60 '1 hora
            comando.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message & " " & " ERROR  0044 ")
        Finally
            conexion.CerrarConexion()
        End Try

    End Sub





End Class
