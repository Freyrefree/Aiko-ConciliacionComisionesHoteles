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

    Public Class ClsN_Conciliaciones

        Private objetoCapaDatos As ClsConciliaciones = New ClsConciliaciones()
        Private objetoCapaDatosPosadas As ClsPosadas = New ClsPosadas()
        Private objetoCapaDatosCityExpress As ClsCityExpress = New ClsCityExpress()
        Private objetoCapaDatoOnyx As ClsOnyx = New ClsOnyx()
        Private objetoCapaDatosTacs As ClsTacs = New ClsTacs()
        Private objetoCapaDatosGestionCommtrack As ClsGestionCommtrack = New ClsGestionCommtrack()

        Private objetoCapaDatosBDBCD As ClsBDBCD = New ClsBDBCD()


        'Varibles para fecha de Proveedores
        Public mesProveedor As String
        Public anioProveedor As String


        '******************************************************************* POSADAS ******************************************************

        Public Function CN_CriteriosAutomaticosPosadas() As DataTable

            Dim tablaCrit1 As DataTable = New DataTable()
            Dim tablaCrit2 As DataTable = New DataTable()
            Dim tablaConbine1_2 As DataTable = New DataTable()
            Dim positionRow As Int32 = 0

            tablaCrit1.Rows.Clear()
            tablaCrit2.Rows.Clear()
            tablaConbine1_2.Rows.Clear()

            tablaCrit1 = objetoCapaDatos.CD_CriteriosAutomaticosPosadas1()
            tablaCrit2 = objetoCapaDatos.CD_CriteriosAutomaticosPosadas2()


            tablaConbine1_2 = tablaCrit1.Copy()
            tablaConbine1_2.Merge(tablaCrit2)

            For Each row As DataRow In tablaConbine1_2.Rows
                Dim cellData As Object = row("dim_value")
                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                Dim positionCero = cadena(0)

                Console.WriteLine(positionCero)
                tablaConbine1_2.Rows(positionRow)("dim_value") = positionCero
                positionRow = positionRow + 1
            Next


            Return tablaConbine1_2

        End Function


        Public Function CN_EstatusPendientesPosadas()

            Dim res As String = objetoCapaDatos.CD_EstatusPendientesPosadas()
            If (res) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function CN_ConsultaPendientesPosadas() As DataTable

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tablaP As DataTable = New DataTable()
            tablaP = objetoCapaDatos.CD_ConsultaPendientesPosadas()
            Return tablaP

        End Function

        Public Function CN_ConsultaPendientesBDBCD() As DataTable

            Dim tablaPBDBCD As DataTable = New DataTable()
            tablaPBDBCD = objetoCapaDatos.CD_ConsultaPendientesBDBCD()
            Return tablaPBDBCD

        End Function

        Public Function matchlist(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (
                                (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%' OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )
                                OR
                                (" & "proveedor." & columnProveedor & " LIKE " & "'%' + " & "REPLACE(BD." & columnBCD & ", 'F.I.' , '') + '%' OR " & "proveedor." & columnProveedor & " LIKE " & "'%' + " & "REPLACE(BD." & columnBCD & ", 'F.A.' , '') + '%' )
                                )"

                            Else

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"



                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_Consultas(moreQuery, lastPartQuery, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function


        Public Function matchlistAutomatico(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()



            Dim listaColumnasBDBCD As New List(Of String)


            'Dim countArray As Int32

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim moreQuery = ""
            Dim moreQueryB = ""
            Dim masqueryLast = ""
            Dim columnaProveedor As String = ""

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "

            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""
                    parentesisA = ""
                    parentesisB = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)

                        masqueryLast &= " AND BD." & columnBCD & "<> '' "
                        columnaProveedor &= columnProveedor & ","


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"
                                parentesisA = "("

                            End If

                            If (count = 1) Then

                                operador = "OR"
                                parentesisB = ")"

                            End If


                            moreQuery &= operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"

                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next

                tabla = objetoCapaDatos.CD_ConsultasAutomatico(moreQuery, lastPartQuery, masqueryLast, fechas)

                For Each col As DataColumn In tabla.Columns
                    col.[ReadOnly] = False
                Next

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function








        Public Sub condicionesPosadasAutomatico(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)




            'elimianar primer index de listaColumnasBDBCD
            listaColumnasBDBCD.RemoveAt(0)
            'Dim ejemplo As String = listaColumnasBDBCD.Item(0)

            Dim conteo As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""


            For Each colPorveedor In listaColumnasProveedor

                If (countB <= 1) Then
                    colProv &= colPorveedor & ","
                Else
                    colProv = colPorveedor & ","
                End If

                If (countB >= 1) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

                colProv = colPorveedor & ","

                'If (countB >= 0) Then
                '    listaColActualizada.Add(colProv)
                'End If

                'countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = (arrayUltimo.Count + 1)
            Dim condicionesTolerancia As Integer = (totalCondiciones - 1)

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatosPosadas.CD_SeleccionIDPendientes()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            conteo = 1
                            condicionOk = " ON " & condicion.Replace("|", "")
                        Else
                            conteo = 2
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        'If (listaColActualizada.Item(count) = 0) Then



                        'End If

                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & Trim(listaColActualizada.Item(count).Replace(",", ",").Substring(0, listaColActualizada.Item(count).Replace(",", ",").Length - 1) & " "c) & "' AS columnas 
	                        FROM
		                        posadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & Trim(listaColActualizada.Item(count).Replace(",", ",").Substring(0, listaColActualizada.Item(count).Replace(",", ",").Length - 1) & " "c) & "' AS columnas
	                        FROM
		                        posadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")


                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                'contadorCondicion = suma

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If


                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If

                    For Each item In valuesList



                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColumnasProveedor

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                        cadenaNoCumplida = Trim(cadenaNoCumplida)

                        If (elegirCadenaPosadasAuto(cadenaRetorno, 1)) Then



                            queryAll &= "UPDATE posadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaPosadasAuto(cadenaRetorno, 2)) Then


                            queryAll &= "UPDATE posadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For

                        ElseIf (elegirCadenaPosadasAuto(cadenaRetorno, 4)) Then


                            queryAll &= "UPDATE posadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        End If



                    Next


                Next
                objetoCapaDatos.CD_CondicionesCumplidasPosadas(queryAll)
            End If



        End Sub

        Public Sub condicionesPosadasManual(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)

            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""




            For Each colPorveedor In listaColumnasProveedor

                colProv = colPorveedor & ","

                If (countB >= 0) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = arrayUltimo.Count
            Dim condicionesTolerancia As Integer = totalCondiciones - 1

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatosPosadas.CD_SeleccionIDPendientes()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            condicionOk = " ON " & condicion.Replace("|", "")
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        1 AS conteo,
		                        '" & listaColActualizada.Item(count).Replace(",", "") & "' AS columnas 
	                        FROM
		                        posadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        1 AS conteo,
		                        '" & listaColActualizada.Item(count).Replace(",", "") & "' AS columnas 
	                        FROM
		                        posadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")


                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                'contadorCondicion = suma

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If


                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If



                    For Each item In valuesList



                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColActualizada

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                        cadenaNoCumplida = Trim(cadenaNoCumplida)

                        If (elegirCadenaPosadasManual(cadenaRetorno, 1)) Then



                            queryAll &= "UPDATE posadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaPosadasManual(cadenaRetorno, 2)) Then


                            queryAll &= "UPDATE posadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaPosadasManual(cadenaRetorno, 3)) Then


                            queryAll &= "UPDATE posadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaPosadasManual(cadenaRetorno, 4)) Then


                            queryAll &= "UPDATE posadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        End If



                    Next


                Next
                objetoCapaDatos.CD_CondicionesCumplidasPosadas(queryAll)
            End If



        End Sub

        Public Function elegirCadenaOnyxAuto(condicionesCumplidas, opc)

            Dim stringArray As String() = {"AgentRef3|{LineNo}"}
            Dim stringArrayB As String() = {"ConformationNo"}



            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else

                        For Each x As String In stringArrayB
                            If condicionesCumplidas.Contains(x) Then
                                Return True
                            Else
                                Return False
                            End If
                        Next

                    End If
                Next

            End If


        End Function





        Public Function elegirCadenaGestionCommtrackAuto(condicionesCumplidas, opc)

            Dim stringArray As String() = {"Trans|{segnum}"}
            Dim stringArrayB As String() = {"Confirmationcode"}

            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else

                        For Each x As String In stringArrayB
                            If condicionesCumplidas.Contains(x) Then
                                Return True
                            Else
                                Return False
                            End If
                        Next

                    End If
                Next

            End If


        End Function

        Public Function elegirCadenaGestionCommtrackManual(condicionesCumplidas, opc)

            Dim stringArray As String() = {"DIN"}
            Dim stringArrayB As String() = {"OUT"}
            Dim stringArrayC As String() = {"First"}
            Dim stringArrayD As String() = {"Last"}

            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else
                        Return False

                    End If
                Next

            ElseIf (opc = 2) Then

                For Each x As String In stringArrayB
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 3) Then

                For Each x As String In stringArrayC
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            ElseIf (opc = 4) Then

                For Each x As String In stringArrayD
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            End If


        End Function

        Public Function elegirCadenaTacsAuto(condicionesCumplidas, opc)

            Dim stringArray As String() = {"Confirmation"}
            Dim stringArrayB As String() = {"FirstName"}
            Dim stringArrayC As String() = {"LastName"}
            Dim bandera As Integer = 0


            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else
                        Return False

                    End If
                Next

            ElseIf (opc = 2) Then

                For Each x As String In stringArrayB
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 3) Then

                For Each x As String In stringArrayC
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next
            End If

        End Function

        Public Function elegirCadenaTacsManual(condicionesCumplidas, opc)

            Dim stringArray As String() = {"FirstName"}
            Dim stringArrayB As String() = {"LastName"}
            Dim stringArrayC As String() = {"Arrival"}
            Dim stringArrayD As String() = {"Departure"}

            Dim bandera As Integer = 0


            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else
                        Return False

                    End If
                Next

            ElseIf (opc = 2) Then

                For Each x As String In stringArrayB
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 3) Then

                For Each x As String In stringArrayC
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            ElseIf (opc = 4) Then

                For Each x As String In stringArrayD
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            End If



        End Function



        Public Function elegirCadenaPosadasAuto(condicionesCumplidas, opc)

            Dim stringArray As String() = {"clave", "claveGDS"}
            Dim stringArrayB As String() = {"llegada"}
            Dim stringArrayD As String() = {"firstName", "lastName"}
            Dim bandera As Integer = 0


            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else
                        Return False

                    End If
                Next

            ElseIf (opc = 2) Then

                For Each x As String In stringArrayB
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            ElseIf (opc = 4) Then

                For Each x As String In stringArrayD
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            End If

        End Function

        Public Function elegirCadenaPosadasManual(condicionesCumplidas, opc)

            Dim stringArray As String() = {"hotel"}
            Dim stringArrayB As String() = {"llegada"}
            Dim stringArrayC As String() = {"salida"}
            Dim stringArrayD As String() = {"firstName", "lastName"}
            Dim bandera As Integer = 0


            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else
                        Return False

                    End If
                Next

            ElseIf (opc = 2) Then

                For Each x As String In stringArrayB
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 3) Then

                For Each x As String In stringArrayC
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 4) Then

                For Each x As String In stringArrayD
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next


            End If

        End Function

        Public Function elegirCadenaCityexpressAuto(condicionesCumplidas, opc)

            Dim stringArray As String() = {"Reservacion"}
            Dim stringArrayB As String() = {"ReferenciaOTA"}

            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else

                        For Each b As String In stringArrayB
                            If condicionesCumplidas.Contains(b) Then
                                Return True
                            Else

                                Return False

                            End If
                        Next

                    End If
                Next

            End If

        End Function



        Public Function elegirCadenaCityExpressManual(condicionesCumplidas, opc)

            Dim stringArray As String() = {"checkIn"}
            Dim stringArrayB As String() = {"checkOut"}
            Dim stringArrayC As String() = {"firstName"}
            Dim stringArrayD As String() = {"lastName"}


            If (opc = 1) Then

                For Each a As String In stringArray
                    If condicionesCumplidas.Contains(a) Then
                        Return True
                    Else
                        Return False

                    End If
                Next

            ElseIf (opc = 2) Then

                For Each x As String In stringArrayB
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 3) Then

                For Each x As String In stringArrayC
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            ElseIf (opc = 4) Then

                For Each x As String In stringArrayD
                    If condicionesCumplidas.Contains(x) Then
                        Return True
                    Else
                        Return False
                    End If
                Next

            End If

        End Function






        Public Function CN_ResetEstatus(idProveedorGlobal)

            'Instanciar Fechaproveedor
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            If (idProveedorGlobal = "1") Then

                Return objetoCapaDatos.CD_ResetEstatusPosadas()

            ElseIf (idProveedorGlobal = "2") Then

                Return objetoCapaDatos.CD_ResetEstatusCityExpress()

            ElseIf (idProveedorGlobal = "3") Then

                Return objetoCapaDatos.CD_ResetEstatusOnyx()

            ElseIf (idProveedorGlobal = "4") Then

                Return objetoCapaDatos.CD_ResetEstatusTacs()

            ElseIf (idProveedorGlobal = "19") Then

                Return objetoCapaDatos.CD_ResetEstatusGestionCommtrack()
            Else



            End If



        End Function


        '***********************************************************************************************************************************

        '****************************************** ONYX ***********************************************************************************










        Public Sub descomponerCondicionesOnyxAuto(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)

            'elimianar primer index de listaColumnasBDBCD
            'listaColumnasBDBCD.RemoveAt(0)
            'Dim ejemplo As String = listaColumnasBDBCD.Item(0)

            Dim conteo As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""


            For Each colPorveedor In listaColumnasProveedor

                'If (countB <= 1) Then
                '    colProv &= colPorveedor & ","
                'Else
                '    colProv = colPorveedor & ","
                'End If

                'If (countB >= 1) Then
                '    listaColActualizada.Add(colProv)
                'End If

                'countB = countB + 1

                colProv = colPorveedor


                If (countB >= 0) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = (arrayUltimo.Count)
            Dim condicionesTolerancia As Integer = (totalCondiciones - 1)

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatoOnyx.CD_SeleccionIDPendientesOnyx()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            conteo = 1
                            condicionOk = " ON " & condicion.Replace("|", "")
                        Else
                            conteo = 1
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        'If (listaColActualizada.Item(count) = 0) Then



                        'End If

                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas 
	                        FROM
		                        onyxPagadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                " & listaColumnasBDBCD(count) & "
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas
	                        FROM
		                        onyxPagadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL 
                                " & listaColumnasBDBCD(count) & "
		                       " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")

                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If

                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If

                    For Each item In valuesList

                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColumnasProveedor

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        If (cadenaNoCumplida <> "") Then

                            cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                            cadenaNoCumplida = Trim(cadenaNoCumplida)
                        End If


                        If (elegirCadenaOnyxAuto(cadenaRetorno, 1)) Then

                            queryAll &= "UPDATE onyxPagadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                        End If

                    Next

                Next

                If (queryAll <> "") Then
                    objetoCapaDatos.CD_CondicionesCumplidasOnyx(queryAll)
                End If

            End If

        End Sub


        Public Function matchlistAutomaticoOnyxComisionesPendientePago(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0
            Dim tablaA As String = ""
            Dim tablaB As String = ""
            Dim masQueryLast As String = ""



            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""


            Dim concatA = ""
            Dim concatB = ""

            Dim moreQuery As String = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""
                    concatA = ""
                    concatB = ""

                    If (i >= 0) Then


                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("|")) Then

                            columnBCD = columnBCD.Replace("|", ",BD.")
                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("|")) Then

                            columnProveedor = columnProveedor.Replace("|", ",proveedor.")
                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)


                        'If (count = 0) Then
                        If (count = 1) Then

                                concatA = "CONCAT("
                                concatB = ")"


                                masQueryLast &= " AND " & concatA & columnBCD & concatB & "<> '' "

                            If (masQueryLast.Contains("(")) Then

                                masQueryLast = masQueryLast.Replace("(", "(BD.")

                            End If

                        Else

                                masQueryLast &= " AND BD." & columnBCD & "<> '' "
                            concatA = ""
                            concatB = ""

                        End If



                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            If (count = 1) Then
                                    concatA = "CONCAT("
                                    concatB = ")"
                                    tablaA = "BD."
                                    tablaB = "proveedor."

                                Else
                                    concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            If (count = 1) Then
                                    concatA = "CONCAT("
                                    concatB = ")"
                                    tablaA = "BD."
                                    tablaB = "proveedor."
                                Else
                                    concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasAutomaticoOnyxComisionesPendientePago(moreQuery, lastPartQuery, masQueryLast, fechas)

                'For Each row As DataRow In tabla.Rows
                '    Dim cellData As Object = row("dim_value")
                '    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                '    Dim positionCero = cadena(0)

                '    Console.WriteLine(positionCero)
                '    tabla.Rows(positionRow)("dim_value") = positionCero
                '    positionRow = positionRow + 1
                'Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function

        Public Function matchlistAutomaticoOnyxObservaciones(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0
            Dim tablaA As String = ""
            Dim tablaB As String = ""
            Dim masQueryLast As String = ""



            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""


            Dim concatA = ""
            Dim concatB = ""

            Dim moreQuery As String = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""
                    concatA = ""
                    concatB = ""

                    If (i >= 0) Then


                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("|")) Then

                            columnBCD = columnBCD.Replace("|", ",BD.")
                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("|")) Then

                            columnProveedor = columnProveedor.Replace("|", ",proveedor.")
                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)


                        'If (count = 0) Then
                        If (count = 1) Then


                            concatA = "CONCAT("
                            concatB = ")"


                            masQueryLast &= " AND " & concatA & columnBCD & concatB & "<> '' "

                            If (masQueryLast.Contains("(")) Then

                                masQueryLast = masQueryLast.Replace("(", "(BD.")


                            End If

                        Else

                            masQueryLast &= " AND BD." & columnBCD & "<> '' "
                            concatA = ""
                            concatB = ""

                        End If



                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then


                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            If (count = 1) Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."

                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            If (count = 1) Then
                                'If (count = 0) Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."
                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasAutomaticoOnyxObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function





        Public Function matchlistAutomaticoOnyx(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            Dim tabla As DataTable = New DataTable()
            Dim tablaB As DataTable = New DataTable()
            Dim tablafull As DataTable = New DataTable()
            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            Dim queries(1) As String

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "
            Dim lastPartQuery = " 
            AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL "
            Dim moreQuery As String
            Dim masQueryLast As String
            Dim positionRow As Int32 = 0

            'Validar condicion especial
            Dim condicionBool As Boolean
            Dim condicionEspecial As String = "AgentRef2|{LineNo}"
            Dim listaB As List(Of List(Of String))
            Dim listaC As New List(Of List(Of String))
            listaC.AddRange(lista)
            'Dim listaB As List(Of String)
            'listaB = lista.Find(Function(x) x(1).Contains(condicionEspecial))

            condicionBool = lista.Exists(Function(x) x(1) = condicionEspecial)

            If (condicionBool) Then

                'Aplicar Conciliación sólo para AgentRef3
                listaC.RemoveAt(1)
                listaB = lista
                listaB.RemoveAt(0)

                queries = armarQueryOnyxAutomatico(listaB, vdateln, vdateOut)
                masQueryLast = queries(0)
                moreQuery = queries(1)


                'lista.RemoveAt(1)

                tablafull = objetoCapaDatos.CD_ConsultasAutomaticoOnyx(moreQuery, lastPartQuery, masQueryLast, fechas)
            End If

            queries = armarQueryOnyxAutomatico(listaC, vdateln, vdateOut)
            masQueryLast = queries(0)
            moreQuery = queries(1)

            tablaB = objetoCapaDatos.CD_ConsultasAutomaticoOnyx(moreQuery, lastPartQuery, masQueryLast, fechas)
            tablafull.Merge(tablaB)

            For Each row As DataRow In tablafull.Rows
                Dim cellData As Object = row("dim_value")
                Dim cadena As String() = cellData.Split(New Char() {"-"c})

                Dim positionCero = cadena(0)

                Console.WriteLine(positionCero)
                tablafull.Rows(positionRow)("dim_value") = positionCero
                positionRow = positionRow + 1
            Next


            Return tablafull



        End Function

        Public Function armarQueryOnyxAutomatico(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)
            Dim queries(1) As String

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim columnProveedorB As String = ""
            Dim tipoOperacion As String = ""
            Dim tablaA As String = ""
            Dim tablaB As String = ""
            Dim masQueryLast As String

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************
            Dim operador = ""
            Dim concatA = ""
            Dim concatB = ""
            Dim moreQuery As String = ""

            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""
                    concatA = ""
                    concatB = ""

                    If (i >= 0) Then


                        columnBCD = el(0)
                        columnProveedor = el(1)
                        columnProveedorB = el(1)


                        If (columnBCD.Contains("|")) Then

                            columnBCD = columnBCD.Replace("|", ",BD.")
                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("|")) Then

                            columnProveedor = columnProveedor.Replace("|", ",proveedor.")
                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)


                        'If (count = 0) Then
                        'If (count = 1) Then
                        If (columnProveedorB = "AgentRef2|{LineNo}") Then


                            concatA = "CONCAT("
                            concatB = ")"


                            masQueryLast &= " AND " & concatA & columnBCD & concatB & "<> '' "

                            If (masQueryLast.Contains("(")) Then

                                masQueryLast = masQueryLast.Replace("(", "(BD.")


                            End If

                        Else

                            masQueryLast &= " AND BD." & columnBCD & "<> '' "
                            concatA = ""
                            concatB = ""

                        End If



                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            'If (count = 1) Then
                            If (columnProveedorB = "AgentRef2|{LineNo}") Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."

                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            'If (count = 1) Then
                            If (columnProveedorB = "AgentRef2|{LineNo}") Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."
                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next

                queries(0) = masQueryLast
                queries(1) = moreQuery
                Return queries


            End If


        End Function

        Public Function matchlistManualOnyxComisionesPendientePago(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("{")) Then


                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("{")) Then


                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If

                            'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"

                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasManualOnyxComisionesPendientePago(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each col As DataColumn In tabla.Columns
                    col.[ReadOnly] = False
                Next

                'For Each row As DataRow In tabla.Rows
                '    Dim cellData As Object = row("dim_value")
                '    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                '    Dim positionCero = cadena(0)

                '    Console.WriteLine(positionCero)
                '    tabla.Rows(positionRow)("dim_value") = positionCero
                '    positionRow = positionRow + 1
                'Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function

        Public Function matchlistManualOnyxObservaciones(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("{")) Then


                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("{")) Then


                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If

                            'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"

                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasManualOnyxObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each col As DataColumn In tabla.Columns
                    col.[ReadOnly] = False
                Next

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function

        Public Function matchlistManualOnyx(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("{")) Then


                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("{")) Then
                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")
                        End If

                        Select Case columnProveedor
                            Case "DateIn"
                                columnProveedor = "ConfDateIn"
                            Case "DateOut"
                                columnProveedor = "ConfDateOut"
                            Case Else
                                columnProveedor = columnProveedor
                        End Select





                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (
                                (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%' OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )
                                OR
                                (" & "proveedor." & columnProveedor & " LIKE " & "'%' + " & "REPLACE(BD." & columnBCD & ", 'F.I.' , '') + '%' OR " & "proveedor." & columnProveedor & " LIKE " & "'%' + " & "REPLACE(BD." & columnBCD & ", 'F.A.' , '') + '%' )
                                )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If

                            'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"

                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasManualOnyx(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each col As DataColumn In tabla.Columns
                    col.[ReadOnly] = False
                Next

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function



        Public Function CN_ConsultaPendientesOnyx() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tablaP As DataTable = New DataTable()
            tablaP = objetoCapaDatos.CD_ConsultaPendientesOnyx()
            Return tablaP

        End Function

        Public Function CN_ConsultaPendientesTacs() As DataTable

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tablaP As DataTable = New DataTable()
            tablaP = objetoCapaDatos.CD_ConsultaPendientesTacs()
            Return tablaP

        End Function


        '***********************************************************************************************************************************
        '***********************************************************   CITY EXPRESS  ************************************************************************

        Public Function matchlistAutomaticoCityExpress(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim moreQuery = ""
            Dim masQueryLast = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""
                    parentesisA = ""
                    parentesisB = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"
                                parentesisA = "("

                            End If

                            If (count = 1) Then

                                operador = "OR"
                                parentesisB = ")"

                            End If
                            If (count = 2) Then

                            End If

                            moreQuery &= operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next

                'If ClsNGlobales.TipoPlantillaCityExpress = 1 Then
                tabla = objetoCapaDatos.CD_ConsultasAutomaticoCityExpress(moreQuery, lastPartQuery, masQueryLast, fechas)
                    'ElseIf ClsNGlobales.TipoPlantillaCityExpress = 2 Then
                    'tabla = objetoCapaDatos.CD_ConsultasAutomaticoCityExpressFormatoB(moreQuery, lastPartQuery, masQueryLast, fechas)
                    'End If


                    For Each col As DataColumn In tabla.Columns
                    col.[ReadOnly] = False
                Next

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If

        End Function

        Public Function matchlistAutomaticoCityExpressFormatoB(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim moreQuery = ""
            Dim masQueryLast = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""
                    parentesisA = ""
                    parentesisB = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"
                                'parentesisA = "("
                                parentesisA = ""

                            End If

                            If (count = 1) Then

                                'operador = "OR"
                                operador = ""
                                'parentesisB = ")"
                                parentesisB = ""

                            End If
                            If (count = 2) Then

                            End If

                            moreQuery &= operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasAutomaticoCityExpressFormatoB(moreQuery, lastPartQuery, masQueryLast, fechas)



                    For Each col As DataColumn In tabla.Columns
                    col.[ReadOnly] = False
                Next

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If

        End Function



        Public Function matchlistManualCityExpress(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
        AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista

                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("{")) Then


                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("{")) Then


                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "


                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                            moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"


                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasManualCityExpress(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function






        Public Function CN_ConsultaPendientesCityExpress() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tablaP As DataTable = New DataTable()
            tablaP = objetoCapaDatos.CD_ConsultaPendientesCityExpress()
            Return tablaP

        End Function
        '***************************************************************************************************************************************



        '****************************************** Gestion Commtrack ***********************************************************************************


        Public Function testGestionCommtrack(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim listaCondiciones As New List(Of String)
            Dim moreQueryB = ""
            Dim listaColumnasProveedor As New List(Of String)
            Dim listaColumnasBDBCD As New List(Of String)
            Dim listQuery As New List(Of String)
            Dim columnaBCD As String

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0
            Dim tablaA As String = ""
            Dim tablaB As String = ""
            Dim masQueryLast As String = ""



            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""


            Dim concatA = ""
            Dim concatB = ""

            Dim moreQuery As String = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""
                    concatA = ""
                    concatB = ""

                    If (i >= 0) Then


                        columnBCD = el(0)
                        columnProveedor = el(1)

                        listQuery.Add(columnProveedor)
                        listaColumnasProveedor.Add(columnProveedor)


                        If (columnBCD.Contains("|")) Then

                            columnBCD = columnBCD.Replace("|", ",BD.")
                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("|")) Then

                            columnProveedor = columnProveedor.Replace("|", ",proveedor.")
                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)


                        If (count = 0) Then


                            concatA = "CONCAT("
                            concatB = ")"


                            masQueryLast &= " AND " & concatA & columnBCD & concatB & "<> '' "
                            columnaBCD = " AND " & concatA & columnBCD & concatB & "<> '' "

                            If (masQueryLast.Contains("(")) Then

                                masQueryLast = masQueryLast.Replace("(", "(BD.")


                            End If
                            If (columnaBCD.Contains("(")) Then

                                columnaBCD = columnaBCD.Replace("(", "(BD.")


                            End If

                        Else

                            masQueryLast &= " AND BD." & columnBCD & "<> '' "
                            columnaBCD = " AND BD." & columnBCD & "<> '' "
                            concatA = ""
                            concatB = ""

                        End If
                        listaColumnasBDBCD.Add(columnaBCD)



                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            If (count = 0) Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."

                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "
                            moreQueryB &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " " & "|"
                            listaCondiciones.Add(moreQueryB)

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            If (count = 0) Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."
                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "
                            moreQueryB &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " " & "|"
                            listaCondiciones.Add(moreQueryB)
                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next

                descomponerCondicionesGestionCommtrackAuto(listaCondiciones, listaColumnasProveedor, listaColumnasBDBCD, fechas)

            End If

        End Function

        Public Function testGestionCommtrackManual(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim listaCondiciones As New List(Of String)
            Dim moreQueryB = ""
            Dim listaColumnasProveedor As New List(Of String)
            Dim listaColumnasBDBCD As New List(Of String)
            Dim listQuery As New List(Of String)
            Dim columnaBCD As String

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)
                        listaColumnasProveedor.Add(columnProveedor)
                        listaColumnasBDBCD.Add(columnBCD)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "
                            moreQueryB &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " " & "|"
                            listaCondiciones.Add(moreQueryB)


                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                                moreQueryB &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )" & "|"
                                listaCondiciones.Add(moreQueryB)

                            Else

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                                moreQueryB &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )" & "|"
                                listaCondiciones.Add(moreQueryB)

                            End If

                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"
                            moreQueryB &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")" & "|"
                            listaCondiciones.Add(moreQueryB)

                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next

                descomponerCondicionesGestionCommtrackManual(listaCondiciones, listaColumnasProveedor, listaColumnasBDBCD, fechas)

            End If

        End Function

        Public Sub descomponerCondicionesGestionCommtrackManual(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)

            'elimianar primer index de listaColumnasBDBCD
            'listaColumnasBDBCD.RemoveAt(0)
            'Dim ejemplo As String = listaColumnasBDBCD.Item(0)

            Dim conteo As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""


            For Each colPorveedor In listaColumnasProveedor

                'If (countB <= 1) Then
                '    colProv &= colPorveedor & ","
                'Else
                '    colProv = colPorveedor & ","
                'End If

                'If (countB >= 1) Then
                '    listaColActualizada.Add(colProv)
                'End If

                'countB = countB + 1

                colProv = colPorveedor


                If (countB >= 0) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = (arrayUltimo.Count)
            Dim condicionesTolerancia As Integer = (totalCondiciones - 1)

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatosGestionCommtrack.CD_SeleccionIDPendientesGestionCommtrack()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            conteo = 1
                            condicionOk = " ON " & condicion.Replace("|", "")
                        Else
                            conteo = 1
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        'If (listaColActualizada.Item(count) = 0) Then



                        'End If

                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas 
	                        FROM
		                        gestionCommtrack proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                 AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas
	                        FROM
		                        gestionCommtrack proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL 
                               AND BD." & listaColumnasBDBCD(count) & " <> ''
		                        " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")

                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If

                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If

                    For Each item In valuesList

                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColumnasProveedor

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        If (cadenaNoCumplida <> "") Then

                            cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                            cadenaNoCumplida = Trim(cadenaNoCumplida)
                        End If


                        If (elegirCadenaGestionCommtrackManual(cadenaRetorno, 1)) Then

                            queryAll &= "UPDATE gestionCommtrack
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"
                            Continue For
                        ElseIf (elegirCadenaGestionCommtrackManual(cadenaRetorno, 2)) Then

                            queryAll &= "UPDATE gestionCommtrack
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"
                            Continue For
                        ElseIf (elegirCadenaGestionCommtrackAuto(cadenaRetorno, 3)) Then

                            queryAll &= "UPDATE gestionCommtrack
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"
                            Continue For
                        ElseIf (elegirCadenaGestionCommtrackAuto(cadenaRetorno, 4)) Then

                            queryAll &= "UPDATE gestionCommtrack
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"
                            Continue For
                        End If

                    Next

                Next

                If (queryAll <> "") Then
                    objetoCapaDatos.CD_CondicionesCumplidasGestionCommtrack(queryAll)
                End If

            End If

        End Sub


        Public Sub descomponerCondicionesGestionCommtrackAuto(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)

            'elimianar primer index de listaColumnasBDBCD
            'listaColumnasBDBCD.RemoveAt(0)
            'Dim ejemplo As String = listaColumnasBDBCD.Item(0)

            Dim conteo As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""


            For Each colPorveedor In listaColumnasProveedor

                'If (countB <= 1) Then
                '    colProv &= colPorveedor & ","
                'Else
                '    colProv = colPorveedor & ","
                'End If

                'If (countB >= 1) Then
                '    listaColActualizada.Add(colProv)
                'End If

                'countB = countB + 1

                colProv = colPorveedor


                If (countB >= 0) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = (arrayUltimo.Count)
            Dim condicionesTolerancia As Integer = (totalCondiciones - 1)

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatosGestionCommtrack.CD_SeleccionIDPendientesGestionCommtrack()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            conteo = 1
                            condicionOk = " ON " & condicion.Replace("|", "")
                        Else
                            conteo = 1
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        'If (listaColActualizada.Item(count) = 0) Then



                        'End If

                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas 
	                        FROM
		                        gestionCommtrack proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                " & listaColumnasBDBCD(count) & "
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas
	                        FROM
		                        gestionCommtrack proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL 
                                " & listaColumnasBDBCD(count) & "
		                        " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")

                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If

                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If

                    For Each item In valuesList

                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColumnasProveedor

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        If (cadenaNoCumplida <> "") Then

                            cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                            cadenaNoCumplida = Trim(cadenaNoCumplida)
                        End If


                        If (elegirCadenaGestionCommtrackAuto(cadenaRetorno, 1)) Then

                            queryAll &= "UPDATE gestionCommtrack
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                        End If

                    Next

                Next

                If (queryAll <> "") Then
                    objetoCapaDatos.CD_CondicionesCumplidasGestionCommtrack(queryAll)
                End If

            End If

        End Sub

        Public Function matchlistAutomaticoGestionCommtrack(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim positionRow As Int32 = 0
            Dim tablaA As String = ""
            Dim tablaB As String = ""
            Dim masQueryLast As String = ""



            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""


            Dim concatA = ""
            Dim concatB = ""

            Dim moreQuery As String = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""
                    concatA = ""
                    concatB = ""

                    If (i >= 0) Then


                        columnBCD = el(0)
                        columnProveedor = el(1)


                        If (columnBCD.Contains("|")) Then

                            columnBCD = columnBCD.Replace("|", ",BD.")
                            columnBCD = columnBCD.Replace("{", "[")
                            columnBCD = columnBCD.Replace("}", "]")

                        End If

                        If (columnProveedor.Contains("|")) Then

                            columnProveedor = columnProveedor.Replace("|", ",proveedor.")
                            columnProveedor = columnProveedor.Replace("{", "[")
                            columnProveedor = columnProveedor.Replace("}", "]")

                        End If


                        tipoOperacion = el(2)


                        'If (count = 0) Then
                        If (count = 1) Then

                            concatA = "CONCAT("
                            concatB = ")"


                            masQueryLast &= " AND " & concatA & columnBCD & concatB & "<> '' "

                            If (masQueryLast.Contains("(")) Then

                                masQueryLast = masQueryLast.Replace("(", "(BD.")


                            End If

                        Else

                            masQueryLast &= " AND BD." & columnBCD & "<> '' "
                            concatA = ""
                            concatB = ""

                        End If



                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            If (count = 1) Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."

                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then

                                operador = "AND"

                            Else

                                operador = "ON"

                            End If

                            'If (count = 0) Then
                            If (count = 1) Then

                                concatA = "CONCAT("
                                concatB = ")"
                                tablaA = "BD."
                                tablaB = "proveedor."
                            Else
                                concatA = ""
                                concatB = ""

                            End If

                            moreQuery &= operador & " " & concatA & " " & "BD." & columnBCD & concatB & " = " & concatA & "proveedor." & columnProveedor & concatB & " "

                        End If

                        count = count + 1

                    End If
                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasAutomaticoGestionCommtrack(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function



        Public Function matchlistManualGestionCommtrack(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)


                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasManualGestionCommtrack(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function




        Public Function CN_ConsultaPendientesGestionCommtrack() As DataTable

            ClsGlobales.FechaProveedorInicio = ClsNGlobales.FechaProveedorInicio
            ClsGlobales.FechaProveedorFin = ClsNGlobales.FechaProveedorFin

            Dim tablaP As DataTable = New DataTable()
            tablaP = objetoCapaDatos.CD_ConsultaPendientesGestionCommtrack()
            Return tablaP

        End Function

        '*********************************************************************************************************

        '********************************************** TACS *****************************************************
        Public Function testTacsManual(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim listaCondiciones As New List(Of String)
            Dim moreQueryB = ""
            Dim listaColumnasProveedor As New List(Of String)
            Dim listaColumnasBDBCD As New List(Of String)
            Dim listQuery As New List(Of String)


            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        listaColumnasProveedor.Add(columnProveedor)
                        listaColumnasBDBCD.Add(columnBCD)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "
                            moreQueryB &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " " & "|"
                            listaCondiciones.Add(moreQueryB)


                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                                moreQueryB &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )" & "|"
                                listaCondiciones.Add(moreQueryB)

                            Else

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"
                                moreQueryB &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )" & "|"
                                listaCondiciones.Add(moreQueryB)

                            End If

                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"
                            moreQueryB &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")" & "|"
                            listaCondiciones.Add(moreQueryB)

                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next

            End If

            descomponerCondicionesTacsManual(listaCondiciones, listaColumnasProveedor, listaColumnasBDBCD, fechas)


        End Function

        Public Sub descomponerCondicionesTacsManual(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)

            'elimianar primer index de listaColumnasBDBCD
            'listaColumnasBDBCD.RemoveAt(0)
            'Dim ejemplo As String = listaColumnasBDBCD.Item(0)

            Dim conteo As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""


            For Each colPorveedor In listaColumnasProveedor

                'If (countB <= 1) Then
                '    colProv &= colPorveedor & ","
                'Else
                '    colProv = colPorveedor & ","
                'End If

                'If (countB >= 1) Then
                '    listaColActualizada.Add(colProv)
                'End If

                'countB = countB + 1

                colProv = colPorveedor


                If (countB >= 0) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = (arrayUltimo.Count)
            Dim condicionesTolerancia As Integer = (totalCondiciones - 1)

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatosTacs.CD_SeleccionIDPendientesTacs()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            conteo = 1
                            condicionOk = " ON " & condicion.Replace("|", "")
                        Else
                            conteo = 1
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        'If (listaColActualizada.Item(count) = 0) Then



                        'End If

                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas 
	                        FROM
		                        tacsPagadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas
	                        FROM
		                        tacsPagadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL 
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")

                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If

                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If

                    For Each item In valuesList

                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColumnasProveedor

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        If (cadenaNoCumplida <> "") Then

                            cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                            cadenaNoCumplida = Trim(cadenaNoCumplida)
                        End If


                        If (elegirCadenaTacsManual(cadenaRetorno, 1)) Then

                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaTacsManual(cadenaRetorno, 2)) Then


                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaTacsManual(cadenaRetorno, 3)) Then

                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaTacsManual(cadenaRetorno, 4)) Then

                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKManual = '" & cadenaRetorno & "',
                            idBDBCDManual =" & itemB & ",
                            countCumplidoManual =" & itemA & ",
                            countNoCumplidoManual = " & cantidadNOCumplidas & ",
                            CondicionNOManual = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        End If

                    Next

                Next

                If (queryAll <> "") Then
                    objetoCapaDatos.CD_CondicionesCumplidasTacs(queryAll)
                End If

            End If

        End Sub

        Public Function testTacs(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim listaCondiciones As New List(Of String)
            Dim moreQueryB = ""
            Dim listaColumnasProveedor As New List(Of String)
            Dim listaColumnasBDBCD As New List(Of String)
            Dim listQuery As New List(Of String)
            Dim columnaBCD As String

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)
                        listQuery.Add(columnProveedor)
                        listaColumnasProveedor.Add(columnProveedor)


                        masQueryLast &= " AND BD." & columnBCD & "<> '' "
                        columnaBCD = " AND BD." & columnBCD & "<> '' "
                        listaColumnasBDBCD.Add(columnBCD)

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "
                            moreQueryB &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " " & "|"
                            listaCondiciones.Add(moreQueryB)

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                                moreQueryB &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )" & "|"
                                listaCondiciones.Add(moreQueryB)
                            Else


                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"
                                moreQueryB &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )" & "|"
                                listaCondiciones.Add(moreQueryB)
                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            moreQueryB &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")" & "|"
                            listaCondiciones.Add(moreQueryB)



                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next

                descomponerCondicionesTacsAuto(listaCondiciones, listaColumnasProveedor, listaColumnasBDBCD, fechas)

            End If

        End Function


        Public Sub descomponerCondicionesTacsAuto(ByVal listaCondiciones As List(Of String), ByVal listaColumnasProveedor As List(Of String), ByVal listaColumnasBDBCD As List(Of String), ByVal fechas As String)

            'elimianar primer index de listaColumnasBDBCD
            'listaColumnasBDBCD.RemoveAt(0)
            'Dim ejemplo As String = listaColumnasBDBCD.Item(0)

            Dim conteo As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim tablaAB As DataTable = New DataTable()
            Dim suma As Integer
            Dim IDBCD_B As Integer
            ''''''''''''''''''''''''''''''''''''''
            Dim queryEnvio As String = ""
            Dim queryEnvioB As String = ""
            Dim queryAll As String = ""

            Dim cadenaRetorno As String
            Dim cadenaNoCumplida As String = ""


            Dim idProveedor As Integer


            Dim tablaProveedor As DataTable = New DataTable()

            Dim valuesList As List(Of Tuple(Of Integer, Integer)) = New List(Of Tuple(Of Integer, Integer))()


            Dim listaColActualizada As New List(Of String)

            Dim myDelims As String() = New String() {"|AND", "| AND"}
            'Dim myDelims As String() = New String() {"|AND"}
            Dim Ultimo = listaCondiciones(listaCondiciones.Count - 1)
            Dim count As Int32 = 0
            Dim countB As Int32 = 0

            Dim condicionOk As String


            Dim contadorCondicion As Int32

            Dim arrayUltimo() As String

            Dim colProv As String = ""


            For Each colPorveedor In listaColumnasProveedor

                'If (countB <= 1) Then
                '    colProv &= colPorveedor & ","
                'Else
                '    colProv = colPorveedor & ","
                'End If

                'If (countB >= 1) Then
                '    listaColActualizada.Add(colProv)
                'End If

                'countB = countB + 1

                colProv = colPorveedor


                If (countB >= 0) Then
                    listaColActualizada.Add(colProv)
                End If

                countB = countB + 1

            Next

            arrayUltimo = Ultimo.Split(myDelims, StringSplitOptions.None)


            Dim totalCondiciones As Integer = (arrayUltimo.Count)
            Dim condicionesTolerancia As Integer = (totalCondiciones - 1)

            Dim itemB As Integer
            Dim itemA As Integer
            Dim cantidadNOCumplidas As Integer

            Dim queryBB As String = ""

            'Obtener Lista  de ID Proveedores Pendientes'
            tablaProveedor = objetoCapaDatosTacs.CD_SeleccionIDPendientesTacs()
            If (tablaProveedor.Rows.Count > 0) Then

                For Each rowB As DataRow In tablaProveedor.Rows

                    'queryAll = ""

                    count = 0
                    valuesList.Clear()

                    idProveedor = vbEmpty
                    idProveedor = rowB("id")

                    queryEnvio = ""
                    queryEnvio &= "SELECT SUM
	                        ( conteo ) suma,
	                        IDBCD AS IDBCD_B 
                        FROM
	                        ("

                    queryEnvioB = ""
                    queryEnvioB &= "SELECT ColumnasCumplidas = STUFF((
                    SELECT ',' + columnas
                    FROM
	                    ("

                    For Each condicion In arrayUltimo

                        condicionOk = condicion.Replace("|", "")

                        If (count > 0) Then
                            conteo = 1
                            condicionOk = " ON " & condicion.Replace("|", "")
                        Else
                            conteo = 1
                        End If


                        '''''''''''''''''''''''''''''''''SE ARMA QUERY'''''''''''''''''''''''''''''''''''''''''
                        'If (listaColActualizada.Item(count) = 0) Then



                        'End If

                        queryEnvio &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas 
	                        FROM
		                        tacsPagadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        queryEnvioB &= "
	                        SELECT
		                        proveedor.id,
		                        BD.id AS IDBCD,
		                        " & conteo & " AS conteo,
		                        '" & listaColActualizada.Item(count) & "' AS columnas
	                        FROM
		                        tacsPagadas proveedor
		                        INNER JOIN BDBCD BD " & condicionOk & "
	                        WHERE
		                        proveedor.id = " & idProveedor & " 
		                        AND BD.estatusConciliado IS NULL 
                                AND BD." & listaColumnasBDBCD(count) & " <> ''
		                       " & fechas & " UNION"

                        '''''''''''''''''''''''''''''''''SE TERMINA ARMAR QUERY'''''''''''''''''''''''''''''''''''''''''


                        count = count + 1
                    Next

                    queryEnvio &= ") tablaTMP 
                        GROUP BY
	                        IDBCD 
                        HAVING
	                        SUM ( conteo ) > 0"
                    queryEnvio = queryEnvio.Replace("UNION)", ")")



                    tablaAB = objetoCapaDatos.CD_SumaCumplidosByID(queryEnvio)
                    contadorCondicion = 0

                    If (tablaAB.Rows.Count > 0) Then

                        For Each rowC As DataRow In tablaAB.Rows

                            IDBCD_B = rowC("IDBCD_B")
                            suma = rowC("suma")

                            If (suma <= totalCondiciones Or suma <= condicionesTolerancia) Then

                                If (suma > contadorCondicion) Then

                                    contadorCondicion = suma
                                    valuesList.Clear()
                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                ElseIf (suma = totalCondiciones Or suma = condicionesTolerancia) Then

                                    valuesList.Add(Tuple.Create(suma, IDBCD_B))

                                End If

                            End If

                            IDBCD_B = vbEmpty
                            suma = vbEmpty

                        Next

                    End If

                    For Each item In valuesList

                        cantidadNOCumplidas = vbEmpty
                        itemA = vbEmpty
                        itemB = vbEmpty
                        queryBB = ""
                        cadenaNoCumplida = ""


                        itemA = item.Item1
                        cantidadNOCumplidas = totalCondiciones - itemA

                        itemB = item.Item2

                        queryBB = ") tablaTMP
		                WHERE IDBCD = " & itemB & "
	                            FOR XML PATH('')
                         ), 1, 1, '')"

                        cadenaRetorno = objetoCapaDatos.CD_CondicionesCumplidasByID(queryEnvioB, queryBB)



                        For Each columna In listaColumnasProveedor

                            columna = columna.Replace(",", "")

                            If Not Regex.IsMatch(cadenaRetorno.Replace(",", " "), "\b" + Regex.Escape(columna) + "\b") Then

                                cadenaNoCumplida &= columna & ","

                            End If

                        Next

                        If (cadenaNoCumplida <> "") Then

                            cadenaNoCumplida = cadenaNoCumplida.Substring(0, cadenaNoCumplida.Length - 1) & " "c
                            cadenaNoCumplida = Trim(cadenaNoCumplida)
                        End If


                        If (elegirCadenaTacsAuto(cadenaRetorno, 1)) Then

                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaTacsAuto(cadenaRetorno, 2)) Then


                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        ElseIf (elegirCadenaTacsAuto(cadenaRetorno, 3)) Then

                            queryAll &= "UPDATE tacsPagadas
                            SET CondicionOKAuto = '" & cadenaRetorno & "',
                            idBDBCD =" & itemB & ",
                            countCumplidoAuto =" & itemA & ",
                            countNoCumplidoAuto = " & cantidadNOCumplidas & ",
                            CondicionNOAuto = '" & cadenaNoCumplida & "'
                            WHERE id = " & idProveedor & ";"

                            Continue For
                        End If

                    Next

                Next

                If (queryAll <> "") Then
                    objetoCapaDatos.CD_CondicionesCumplidasTacs(queryAll)
                End If

            End If

        End Sub



        Public Function matchlistAutomaticoTacs(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)


                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasAutomaticoTasc(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function

        Public Function matchlistAutomaticoTacsObservaciones(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)


                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next


                tabla = objetoCapaDatos.CD_ConsultasAutomaticoTascObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function

        Public Function matchlistManualTacs(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)


                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next






                tabla = objetoCapaDatos.CD_ConsultasManualTasc(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function

        Public Function matchlistManualTacsObservaciones(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()

            Dim columnBCD As String = ""
            Dim columnProveedor As String = ""
            Dim tipoOperacion As String = ""
            Dim diasRango As String = ""
            Dim positionRow As Int32 = 0
            Dim masQueryLast As String = ""

            'SE DEBEN CAMBIAR ESTOS VALORES'
            Dim i As Int32 = 0
            Dim count As Int32 = 0
            '*********************

            Dim operador = ""
            Dim moreQuery = ""
            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            If lista.Count >= 0 Then

                For Each el In lista


                    operador = ""

                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        masQueryLast &= " AND BD." & columnBCD & "<> '' "

                        If (tipoOperacion = "IGUALDAD") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & " "



                        ElseIf (tipoOperacion = "CONTIENE") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If


                            If (columnBCD = "HotelName") Then

                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.I.' , '') + '%'
                                OR " & "BD." & columnBCD & " LIKE " & "'%' + " & "REPLACE(proveedor." & columnProveedor & ", 'F.A.' , '') + '%' )"

                            Else

                                'moreQuery &= operador & " " & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%'"
                                moreQuery &= operador & " (" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                            End If




                        ElseIf (tipoOperacion = "RANGO") Then

                            If (count >= 1) Then
                                operador = "AND"
                            Else
                                operador = "ON"
                            End If

                            moreQuery &= operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                            'proveedor.salida  BETWEEN BD.dateout AND DATEADD(day, 3, BD.dateout)


                        End If

                        count = count + 1

                    End If

                    i = i + 1
                Next

                tabla = objetoCapaDatos.CD_ConsultasManualTascObservaciones(moreQuery, lastPartQuery, masQueryLast, fechas)

                For Each row As DataRow In tabla.Rows
                    Dim cellData As Object = row("dim_value")
                    Dim cadena As String() = cellData.Split(New Char() {"-"c})

                    Dim positionCero = cadena(0)

                    Console.WriteLine(positionCero)
                    tabla.Rows(positionRow)("dim_value") = positionCero
                    positionRow = positionRow + 1
                Next



                Return tabla

            Else

                'Tabla Vacía
                Return tabla

            End If


        End Function



        ''************************************************* PREPAGO ************************************************
        Public Function matchlistAutomaticoPrepago(vdateln As String, vdateOut As String)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

            Dim tabla As DataTable = New DataTable()
            Dim positionRow As Int32 = 0

            Dim lastPartQuery = " AND  proveedor.estatusConciliado IS NULL
            AND BD.estatusConciliado IS NULL"

            Dim fechas As String = " AND BD.DateIn >= '" & vdateln & "'  AND BD.DateOut <= '" & vdateOut & "' "


            tabla = objetoCapaDatos.CD_ConsultasAutomaticoPrepago(lastPartQuery, fechas)

            'For Each row As DataRow In tabla.Rows
            '    Dim cellData As Object = row("dim_value")
            '    Dim cadena As String() = cellData.Split(New Char() {"-"c})

            '    Dim positionCero = cadena(0)

            '    Console.WriteLine(positionCero)
            '    tabla.Rows(positionRow)("dim_value") = positionCero
            '    positionRow = positionRow + 1
            'Next


            Return tabla

        End Function


        Public Sub eliminarPrepago(prepagos)

            Dim idBDBCD As Integer
            Dim idProveedor As Integer

            For Each row As DataRow In prepagos.Rows
                idBDBCD = vbEmpty
                idProveedor = vbEmpty

                idBDBCD = row("UUID")
                idProveedor = row("UUIDP")

                objetoCapaDatos.elimiarPrepago(idProveedor, idBDBCD)

            Next

        End Sub

        Public Sub fechaPagoOnyxComisionesPendientePago(tablaFechaPagos)

            Dim idBDBCD As Integer
            Dim idProveedor As Integer
            Dim mesProveedor As String

            For Each row As DataRow In tablaFechaPagos.Rows

                'idBDBCD = vbEmpty
                idProveedor = vbEmpty
                mesProveedor = vbEmpty


                'idBDBCD = row("UUID")
                idProveedor = row("UUIDP")
                mesProveedor = row("mesProveedor")



                objetoCapaDatos.actualizarFechaPagoOCPP(idProveedor, mesProveedor)

            Next

        End Sub

        Public Function estatusOnyxObservaciones(tablaResultadoConciliacion)

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor
            Dim fechaProveedor As String = ClsGlobales.FechaPagoproveedor


            Dim idBDBCD As Integer
            Dim idProveedor As Integer

            Dim queryA As String = ""
            Dim queryB As String = ""

            For Each row As DataRow In tablaResultadoConciliacion.Rows

                idBDBCD = vbEmpty
                idProveedor = vbEmpty

                idBDBCD = row("idBDBCD")
                idProveedor = row("idProveedor")

                queryA &= "UPDATE onyxObservaciones SET estatusConciliado = 1 WHERE id  = " & idProveedor & ";"

                queryB &= "UPDATE BDBCD SET estatusConciliado = 1, proveedor='onyx',mesProveedor='" & fechaProveedor & "' WHERE id  = " & idBDBCD & ";"

            Next

            objetoCapaDatos.estatusObservacionesOnyx(queryA, queryB)

        End Function




        '***********************************************************************************************************

        Public Function columnasBCDByProveedor(idCliente As Int32)

            Return objetoCapaDatos.ListaColumnBCDByProveedor(idCliente)

        End Function

        Public Function columnasProveedorInterfaz(idCliente As Int32)

            Return objetoCapaDatos.ListaColumnProveedor(idCliente)

        End Function

    End Class

End Namespace
