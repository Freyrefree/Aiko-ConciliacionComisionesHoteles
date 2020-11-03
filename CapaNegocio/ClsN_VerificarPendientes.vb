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

    Public Class ClsN_VerificarPendientes

        Private objetoCapaDatos As ClsConciliaciones = New ClsConciliaciones()

        '----------------------------------------POSADAS----------------------------------------------------------------------------

        Public Sub AutomaticoPosadas(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "posadas"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCD = temp.idBcd,
			countCumplidoAuto = temp.suma,
			countNoCumplidoAuto = (@totalCondiciones -  temp.suma),
			CondicionOKAuto = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOAuto =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next



                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD > 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD
                
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        Public Sub ManualPosadas(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "posadas"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCDManual = temp.idBcd,
			countCumplidoManual = temp.suma,
			countNoCumplidoManual = (@totalCondiciones -  temp.suma),
			CondicionOKManual = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOManual =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor



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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next





                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        '------------------------------------------CITY EXPRESS----------------------------------------------------------------------

        Public Sub AutomaticoCityExpress(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "cityexpress"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCD = temp.idBcd,
			countCumplidoAuto = temp.suma,
			countNoCumplidoAuto = (@totalCondiciones -  temp.suma),
			CondicionOKAuto = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOAuto =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next



                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD > 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        Public Sub ManualCityExpress(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "cityexpress"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCDManual = temp.idBcd,
			countCumplidoManual = temp.suma,
			countNoCumplidoManual = (@totalCondiciones -  temp.suma),
			CondicionOKManual = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOManual =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor



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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next





                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        '-------------------------------------------ONYX------------------------------------------------------------------------------

        Public Sub AutomaticoOnyx(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "onyxPagadas"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"."}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCD = temp.idBcd,
			countCumplidoAuto = temp.suma,
			countNoCumplidoAuto = (@totalCondiciones -  temp.suma),
			CondicionOKAuto = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOAuto =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "." & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()
                    Select Case prioridad

                        Case "AgentRef2|{LineNo}"

                            prioridad = "AgentRef2_LineNo"

                        Case Else

                            prioridad = Trim(arrayPrioridad(1)).ToString()

                    End Select

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next



                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""

                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        Select Case elBCD(0)

                            Case "UniqueBookingID|{LineNo}"

                                queryTablaBCDA &= "UniqueBookingID_LineNo" & tipoDato & comaBCD
                                queryTablaBCDB &= "UniqueBookingID_LineNo" & comaBCD
                                queryTablaBCDC &= "CONCAT(UniqueBookingID,[LineNo])" & comaBCD

                            Case Else

                                queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                                queryTablaBCDB &= elBCD(0) & comaBCD
                                queryTablaBCDC &= elBCD(0) & comaBCD

                        End Select




                    End If

                    contadorBCD = contadorBCD + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                            hotel LIKE '%F.I.%',
                            REPLACE( hotel, 'F.I.', '' ),
                            IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        ElseIf elProveedor(1) = "AgentRef2|{LineNo}" Then

                            colExtra = "CONCAT(AgentRef2,[LineNo])"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If

                        Select Case elProveedor(1)

                            Case "AgentRef2|{LineNo}"

                                queryTablaProveedorA &= "AgentRef2_LineNo" & tipoDato & comaProveedor
                                queryTablaProveedorB &= "AgentRef2_LineNo" & comaProveedor
                                columnProveedorBB &= "AgentRef2_LineNo" & bool & comaProveedor

                            Case Else

                                queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                                queryTablaProveedorB &= elProveedor(1) & comaProveedor
                                columnProveedorBB &= elProveedor(1) & bool & comaProveedor

                        End Select



                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then

                        Select Case elConciliados(1)

                            Case "AgentRef2|{LineNo}"

                                queryTablaConciliadosA &= "AgentRef2_LineNo" & bool & int & comaConciliados
                                queryTablaConciliadosB &= "AgentRef2_LineNo" & bool & comaConciliados & puntoyComa

                            Case Else

                                queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                                queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa

                        End Select



                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then

                        Select Case elConciliadosFinal(1)
                            Case "AgentRef2|{LineNo}"

                                queryTablaConciliadosFinalA &= "AgentRef2_LineNo" & bool & int & comaConciliadosFinal
                                queryTablaConciliadosFinalB &= "AgentRef2_LineNo" & bool & comaConciliadosFinal & puntoyComa

                            Case Else

                                queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                                queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa

                        End Select



                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then

                        Select Case elConciliadosResultado(1)
                            Case "AgentRef2|{LineNo}"

                                queryTablaConciliadosResultadoA &= "AgentRef2_LineNo" & bool & int & comaConciliadosResultado
                                queryTablaConciliadosResultadoB &= "AgentRef2_LineNo" & bool & comaConciliadosResultado & puntoyComa

                            Case Else

                                queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                                queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                        End Select

                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then


                        Select Case elConciliadosInsert(1)
                            Case "AgentRef2|{LineNo}"
                                queryInsertConciliadosA &= "AgentRef2_LineNo" & bool & comaConciliadosInsert & puntoyComa
                            Case Else
                                queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                        End Select


                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then

                        Select Case elConciliadosFinalInsert(1)
                            Case "AgentRef2|{LineNo}"

                                queryInsertConciliadosFinalA &= "AgentRef2_LineNo" & bool & comaConciliadosFinalInsert
                                queryInsertConciliadosFinalB &= "SUM(" & "AgentRef2_LineNo" & bool & ") AS " & "AgentRef2_LineNo" & bool & comaConciliadosFinalInsert

                            Case Else

                                queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                                queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert

                        End Select



                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then

                        Select Case elUpdate(1)
                            Case "AgentRef2|{LineNo}"

                                queryUpdateA &= "IIF(temp." & "AgentRef2_LineNo" & bool & " = 1,'" & "AgentRef2|{LineNo}" & ",','')" & comaUpdate & ""
                                queryUpdateB &= "IIF(temp." & "AgentRef2_LineNo" & bool & " = 0,'" & "AgentRef2|{LineNo}" & ",','')" & comaUpdate & ""
                            Case Else
                                queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                                queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        End Select




                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        Select Case columnProveedor
                            Case "AgentRef2|{LineNo}"

                                columnProveedor = "AgentRef2_LineNo"

                            Case Else
                                columnProveedor = el(1)
                        End Select

                        Select Case columnBCD

                            Case "UniqueBookingID|{LineNo}"

                                columnBCD = "UniqueBookingID_LineNo"

                            Case Else
                                columnBCD = el(0)

                        End Select




                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)
                            Select Case selectorColumna
                                Case "AgentRef2|{LineNo}"

                                    selectorColumna = "AgentRef2_LineNo"

                                Case Else
                                    selectorColumna = cc(1)
                            End Select

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        Public Sub ManualOnyx(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "onyxPagadas"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim fechasB As String = " AND ConfDateIn >= '" & vdateln & "'  AND ConfDateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"."}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCDManual = temp.idBcd,
			countCumplidoManual = temp.suma,
			countNoCumplidoManual = (@totalCondiciones -  temp.suma),
			CondicionOKManual = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOManual =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "." & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()
                    Select Case prioridad

                        Case "AgentRef3|{LineNo}"

                            prioridad = "AgentRef3_LineNo"

                        Case Else

                            prioridad = Trim(arrayPrioridad(1)).ToString()

                    End Select

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next



                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""

                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        Select Case elBCD(0)

                            Case "UniqueBookingID|{LineNo}"

                                queryTablaBCDA &= "UniqueBookingID_LineNo" & tipoDato & comaBCD
                                queryTablaBCDB &= "UniqueBookingID_LineNo" & comaBCD
                                queryTablaBCDC &= "CONCAT(UniqueBookingID,[LineNo])" & comaBCD

                            Case Else

                                queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                                queryTablaBCDB &= elBCD(0) & comaBCD
                                queryTablaBCDC &= elBCD(0) & comaBCD

                        End Select




                    End If

                    contadorBCD = contadorBCD + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "HotelName" Then

                            colExtra = "IIF(
                            HotelName LIKE '%F.I.%',
                            REPLACE( HotelName, 'F.I.', '' ),
                            IIF( HotelName LIKE '%F.A.%', REPLACE( HotelName, 'F.A.', '' ), HotelName )) AS HotelName"
                            queryTablaProveedorC &= colExtra & comaProveedor

                        ElseIf elProveedor(1) = "AgentRef3|{LineNo}" Then

                            colExtra = "CONCAT(AgentRef3,[LineNo])"
                            queryTablaProveedorC &= colExtra & comaProveedor

                        ElseIf elProveedor(1) = "DateIn" Then

                            colExtra = "ConfDateIn"
                            queryTablaProveedorC &= colExtra & comaProveedor

                        ElseIf elProveedor(1) = "DateOut" Then

                            colExtra = "ConfDateOut"
                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If

                        Select Case elProveedor(1)

                            Case "AgentRef3|{LineNo}"

                                queryTablaProveedorA &= "AgentRef3_LineNo" & tipoDato & comaProveedor
                                queryTablaProveedorB &= "AgentRef3_LineNo" & comaProveedor
                                columnProveedorBB &= "AgentRef3_LineNo" & bool & comaProveedor

                            Case "DateIn"

                                queryTablaProveedorA &= "ConfDateIn" & tipoDato & comaProveedor
                                queryTablaProveedorB &= "ConfDateIn" & comaProveedor
                                columnProveedorBB &= elProveedor(1) & bool & comaProveedor

                            Case "DateOut"

                                queryTablaProveedorA &= "ConfDateOut" & tipoDato & comaProveedor
                                queryTablaProveedorB &= "ConfDateOut" & comaProveedor
                                columnProveedorBB &= elProveedor(1) & bool & comaProveedor

                            Case Else

                                queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                                queryTablaProveedorB &= elProveedor(1) & comaProveedor
                                columnProveedorBB &= elProveedor(1) & bool & comaProveedor

                        End Select

                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then

                        Select Case elConciliados(1)

                            Case "AgentRef3|{LineNo}"

                                queryTablaConciliadosA &= "AgentRef3_LineNo" & bool & int & comaConciliados
                                queryTablaConciliadosB &= "AgentRef3_LineNo" & bool & comaConciliados & puntoyComa

                            Case Else

                                queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                                queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa

                        End Select



                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then

                        Select Case elConciliadosFinal(1)
                            Case "AgentRef3|{LineNo}"

                                queryTablaConciliadosFinalA &= "AgentRef3_LineNo" & bool & int & comaConciliadosFinal
                                queryTablaConciliadosFinalB &= "AgentRef3_LineNo" & bool & comaConciliadosFinal & puntoyComa

                            Case Else

                                queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                                queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa

                        End Select



                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then

                        Select Case elConciliadosResultado(1)
                            Case "AgentRef3|{LineNo}"

                                queryTablaConciliadosResultadoA &= "AgentRef3_LineNo" & bool & int & comaConciliadosResultado
                                queryTablaConciliadosResultadoB &= "AgentRef3_LineNo" & bool & comaConciliadosResultado & puntoyComa

                            Case Else

                                queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                                queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                        End Select

                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then


                        Select Case elConciliadosInsert(1)
                            Case "AgentRef3|{LineNo}"
                                queryInsertConciliadosA &= "AgentRef3_LineNo" & bool & comaConciliadosInsert & puntoyComa
                            Case Else
                                queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                        End Select


                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then

                        Select Case elConciliadosFinalInsert(1)
                            Case "AgentRef3|{LineNo}"

                                queryInsertConciliadosFinalA &= "AgentRef3_LineNo" & bool & comaConciliadosFinalInsert
                                queryInsertConciliadosFinalB &= "SUM(" & "AgentRef3_LineNo" & bool & ") AS " & "AgentRef3_LineNo" & bool & comaConciliadosFinalInsert

                            Case Else

                                queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                                queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert

                        End Select



                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then

                        Select Case elUpdate(1)
                            Case "AgentRef3|{LineNo}"

                                queryUpdateA &= "IIF(temp." & "AgentRef3_LineNo" & bool & " = 1,'" & "AgentRef3_LineNo" & ",','')" & comaUpdate & ""
                                queryUpdateB &= "IIF(temp." & "AgentRef3_LineNo" & bool & " = 0,'" & "AgentRef3_LineNo" & ",','')" & comaUpdate & ""
                            Case Else
                                queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                                queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        End Select




                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        Select Case columnProveedor
                            Case "AgentRef3|{LineNo}"

                                columnProveedor = "AgentRef3_LineNo"

                            Case "DateIn"
                                columnProveedor = "ConfDateIn"

                            Case "DateOut"
                                columnProveedor = "ConfDateOut"

                            Case Else
                                columnProveedor = el(1)
                        End Select

                        Select Case columnBCD

                            Case "UniqueBookingID|{LineNo}"

                                columnBCD = "UniqueBookingID_LineNo"

                            Case Else
                                columnBCD = el(0)

                        End Select

                        operador = "AND"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = "ON BD.DateIn = proveedor.ConfDateIn " & operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = "ON BD.DateIn = proveedor.ConfDateIn " & operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = "ON BD.DateIn = proveedor.ConfDateIn " & operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If


                        'operador = "ON"
                        'If (tipoOperacion = "IGUALDAD") Then


                        '    condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        'ElseIf (tipoOperacion = "CONTIENE") Then

                        '    condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        'ElseIf (tipoOperacion = "RANGO") Then

                        '    condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        'End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)
                            Select Case selectorColumna
                                Case "AgentRef3|{LineNo}"

                                    selectorColumna = "AgentRef3_LineNo"
                                Case "DateIn"

                                    selectorColumna = "ConfDateIn"
                                Case "DateOut"

                                    selectorColumna = "ConfDateOut"
                                Case Else
                                    selectorColumna = cc(1)
                            End Select

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                Select Case selectorColumna

                                    Case "ConfDateIn"

                                        selectorColumna = "DateIn"
                                    Case "ConfDateOut"

                                        selectorColumna = "DateOut"
                                    Case Else
                                        selectorColumna = columnProveedor
                                End Select

                                selector = 1
                            Else
                                Select Case selectorColumna

                                    Case "ConfDateIn"

                                        selectorColumna = "DateIn"
                                    Case "ConfDateOut"

                                        selectorColumna = "DateOut"
                                        'Case Else
                                        '    selectorColumna
                                End Select
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor & fechasB

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        '-------------------------------------------TACS------------------------------------------------------------------------------
        Public Sub AutomaticoTacs(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            Dim tabla As String = "tacsPagadas"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCD = temp.idBcd,
			countCumplidoAuto = temp.suma,
			countNoCumplidoAuto = (@totalCondiciones -  temp.suma),
			CondicionOKAuto = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOAuto =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor


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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next





                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        Public Sub ManualTacs(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            Dim tabla As String = "tacsPagadas"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCDManual = temp.idBcd,
			countCumplidoManual = temp.suma,
			countNoCumplidoManual = (@totalCondiciones -  temp.suma),
			CondicionOKManual = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOManual =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor



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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next





                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        '-------------------------------------------GESTION COMMTRACK--------------------------------------------------------------------

        Public Sub AutomaticoGestionCommtrack(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)


            Dim tabla As String = "gestionCommtrack"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"."}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCD = temp.idBcd,
			countCumplidoAuto = temp.suma,
			countNoCumplidoAuto = (@totalCondiciones -  temp.suma),
			CondicionOKAuto = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOAuto =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor

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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "." & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()
                    Select Case prioridad

                        Case "Trans|{segnum}"

                            prioridad = "Trans_segnum"

                        Case Else

                            prioridad = Trim(arrayPrioridad(1)).ToString()

                    End Select

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next



                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""

                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        Select Case elBCD(0)

                            Case "UniqueBookingID|{LineNo}"

                                queryTablaBCDA &= "UniqueBookingID_LineNo" & tipoDato & comaBCD
                                queryTablaBCDB &= "UniqueBookingID_LineNo" & comaBCD
                                queryTablaBCDC &= "CONCAT(UniqueBookingID,[LineNo])" & comaBCD

                            Case Else

                                queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                                queryTablaBCDB &= elBCD(0) & comaBCD
                                queryTablaBCDC &= elBCD(0) & comaBCD

                        End Select




                    End If

                    contadorBCD = contadorBCD + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                            hotel LIKE '%F.I.%',
                            REPLACE( hotel, 'F.I.', '' ),
                            IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        ElseIf elProveedor(1) = "Trans|{segnum}" Then

                            colExtra = "CONCAT(Trans,segnum)"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If

                        Select Case elProveedor(1)

                            Case "Trans|{segnum}"

                                queryTablaProveedorA &= "Trans_segnum" & tipoDato & comaProveedor
                                queryTablaProveedorB &= "Trans_segnum" & comaProveedor
                                columnProveedorBB &= "Trans_segnum" & bool & comaProveedor

                            Case Else

                                queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                                queryTablaProveedorB &= elProveedor(1) & comaProveedor
                                columnProveedorBB &= elProveedor(1) & bool & comaProveedor

                        End Select



                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then

                        Select Case elConciliados(1)

                            Case "Trans|{segnum}"

                                queryTablaConciliadosA &= "Trans_segnum" & bool & int & comaConciliados
                                queryTablaConciliadosB &= "Trans_segnum" & bool & comaConciliados & puntoyComa

                            Case Else

                                queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                                queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa

                        End Select



                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then

                        Select Case elConciliadosFinal(1)
                            Case "Trans|{segnum}"

                                queryTablaConciliadosFinalA &= "Trans_segnum" & bool & int & comaConciliadosFinal
                                queryTablaConciliadosFinalB &= "Trans_segnum" & bool & comaConciliadosFinal & puntoyComa

                            Case Else

                                queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                                queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa

                        End Select



                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then

                        Select Case elConciliadosResultado(1)
                            Case "Trans|{segnum}"

                                queryTablaConciliadosResultadoA &= "Trans_segnum" & bool & int & comaConciliadosResultado
                                queryTablaConciliadosResultadoB &= "Trans_segnum" & bool & comaConciliadosResultado & puntoyComa

                            Case Else

                                queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                                queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                        End Select

                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then


                        Select Case elConciliadosInsert(1)
                            Case "Trans|{segnum}"
                                queryInsertConciliadosA &= "Trans_segnum" & bool & comaConciliadosInsert & puntoyComa
                            Case Else
                                queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                        End Select


                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then

                        Select Case elConciliadosFinalInsert(1)
                            Case "Trans|{segnum}"

                                queryInsertConciliadosFinalA &= "Trans_segnum" & bool & comaConciliadosFinalInsert
                                queryInsertConciliadosFinalB &= "SUM(" & "Trans_segnum" & bool & ") AS " & "Trans_segnum" & bool & comaConciliadosFinalInsert

                            Case Else

                                queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                                queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert

                        End Select



                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then

                        Select Case elUpdate(1)
                            Case "Trans|{segnum}"

                                queryUpdateA &= "IIF(temp." & "Trans_segnum" & bool & " = 1,'" & "Trans|{segnum}" & ",','')" & comaUpdate & ""
                                queryUpdateB &= "IIF(temp." & "Trans_segnum" & bool & " = 0,'" & "Trans|{segnum}" & ",','')" & comaUpdate & ""
                            Case Else
                                queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                                queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        End Select




                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        Select Case columnProveedor
                            Case "Trans|{segnum}"

                                columnProveedor = "Trans_segnum"

                            Case Else
                                columnProveedor = el(1)
                        End Select

                        Select Case columnBCD

                            Case "UniqueBookingID|{LineNo}"

                                columnBCD = "UniqueBookingID_LineNo"

                            Case Else
                                columnBCD = el(0)

                        End Select




                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)
                            Select Case selectorColumna
                                Case "Trans|{segnum}"

                                    selectorColumna = "Trans_segnum"

                                Case Else
                                    selectorColumna = cc(1)
                            End Select

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

        Public Sub ManualgestionCommtrack(lista As List(Of List(Of String)), vdateln As String, vdateOut As String)

            Dim tabla As String = "gestionCommtrack"
            Dim puntoyComa As String
            Dim colate As String
            Dim totalListaA As Integer
            totalListaA = lista.Count
            Dim totalLista As Integer
            totalLista = (lista.Count - 1)
            Dim coma As String
            Dim tipoDato As String
            Dim condicion As String
            Dim selectorColumna As String
            Dim selector As Integer
            Dim comaB As String
            Dim j As Integer
            Dim columnProveedorBB As String = ""
            Dim fechasA As String = " DateIn >= '" & vdateln & "'  AND DateOut <= '" & vdateOut & "' "
            Dim queryEstatusConciliado As String = " AND estatusConciliado IS NULL "
            Dim fechasPagoProveedor As String = " AND mesProveedor = '" & ClsNGlobales.FechaPagoproveedor & "'"
            Dim queryEstatusConciliadoP As String = " estatusConciliado IS NULL "
            Dim queryEstatusEliminado As String = " AND estatusEliminado IS NULL "
            Dim queryInsertConciliados As String = ""
            Dim queryInsertConciliadoCC As String = ""
            Dim parentesisA = ""
            Dim parentesisB = ""
            Dim bool As String = "Bool "
            Dim columnaPrioridad As String = ""
            Dim listaPrioridades As New List(Of String)
            Dim arrayPrioridad() As String
            Dim separador As String() = New String() {"|"}
            Dim contadorPrioridad As Integer = 0
            Dim comaPrioridad As String
            Dim colExtra As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoBCD As String = ""
            Dim queryTablaBCDA = " CREATE TABLE #tblBDBCD (id INT, "
            Dim queryTablaBCDB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblBDBCD (id, "
            Dim queryTablaBCDC = " );INSERT INTO #tblBDBCD SELECT id, "
            Dim queryTablaBCDD = " FROM BDBCD WHERE "
            Dim columnaTablaBCD As String = ""
            Dim contadorBCD As Integer = 0
            Dim comaBCD As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim queryCompletoProveedor As String = ""
            Dim queryTablaProveedorA = "

CREATE TABLE #tblProveedor (id INT, "
            Dim queryTablaProveedorB = " )CREATE CLUSTERED INDEX ix_tblBDBCD ON #tblProveedor (id, "
            Dim queryTablaProveedorC = " );INSERT INTO #tblProveedor SELECT id, "
            Dim queryTablaProveedorD = " FROM " & tabla & " WHERE "
            Dim columnaTablaProveedor As String = ""
            Dim contadorProveedor As Integer = 0
            Dim comaProveedor As String


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliados As String = ""
            Dim queryTablaConciliadosA = " 

CREATE TABLE #tblConciliados(suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosB = " )CREATE CLUSTERED INDEX ix_tblConciliados ON #tblConciliados (suma,idBcd,id, "

            Dim int As String = "INT "
            Dim contadorConciliados As Integer = 0
            Dim comaConciliados As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosFinal As String = ""
            Dim queryTablaConciliadosFinalA = "

CREATE TABLE #tblConciliadosFinal (suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosFinalB = " )CREATE CLUSTERED INDEX ix_tblConciliadosFinal ON #tblConciliadosFinal ( "

            Dim intConciliadosFinal As String = "INT "
            Dim contadorConciliadosFinal As Integer = 0
            Dim comaConciliadosFinal As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoConciliadosResultado As String = ""
            Dim queryTablaConciliadosResultadoA = " 

CREATE TABLE #tblConciliadosResultado ( suma INT,idBcd INT,id INT, "
            Dim queryTablaConciliadosResultadoB = " )CREATE CLUSTERED INDEX ix_tblConciliadosResultado ON #tblConciliadosResultado(suma,idBcd,id, "

            Dim intConciliadosResultado As String = "INT "
            Dim contadorConciliadosResultado As Integer = 0
            Dim comaConciliadosResultado As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliados As String = ""
            Dim queryInsertConciliadosA = " 

INSERT INTO #tblConciliados SELECT SUM(conteo) AS suma,IDBCD AS IDBCD_B,id, "

            Dim contadorConciliadosInsert As Integer = 0
            Dim comaConciliadosInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoInsertConciliadosFinal As String = ""
            Dim queryInsertConciliadosFinalA = "

INSERT INTO #tblConciliadosFinal (suma,idBcd,id, "
            Dim queryInsertConciliadosFinalB = ")SELECT SUM(suma) AS conteo,idBcd,id, "
            Dim queryInsertConciliadosFinalC = "FROM #tblConciliados GROUP BY idBcd, id "

            Dim contadorConciliadosFinalInsert As Integer = 0
            Dim comaConciliadosFinalInsert As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim queryCompletoUpdate As String = ""
            Dim queryUpdateA = " 

DECLARE  
			@totalCondiciones INT 
			SET @totalCondiciones = " & totalListaA & " 
			UPDATE " & tabla & " SET
			idBDBCDManual = temp.idBcd,
			countCumplidoManual = temp.suma,
			countNoCumplidoManual = (@totalCondiciones -  temp.suma),
			CondicionOKManual = 
			CONCAT( "
            Dim queryUpdateB = " ),CondicionNOManual =	CONCAT( "
            Dim queryUpdateC = " )FROM  " & tabla & " oP INNER JOIN #tblConciliadosResultado temp	ON oP.id = temp.id "
            Dim contadorUpdate As Integer = 0
            Dim comaUpdate As String
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






            ClsGlobales.FechaPagoproveedor = ClsNGlobales.FechaPagoproveedor



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

                '''''''''''''''''''''''''''''''''''''''''''''''''''''#tblBDBCD''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                For Each prioridad In lista

                    listaPrioridades.Add(prioridad(5) & "|" & prioridad(1))

                Next
                listaPrioridades.Sort()

                For Each prioridad In listaPrioridades

                    If contadorPrioridad = (totalLista) Then
                        comaPrioridad = ""
                    Else
                        comaPrioridad = ","
                    End If

                    arrayPrioridad = prioridad.Split(New Char() {"("c})

                    arrayPrioridad = prioridad.Split(separador, StringSplitOptions.None)
                    prioridad = Trim(arrayPrioridad(1)).ToString()

                    columnaPrioridad &= prioridad & bool & "DESC" & comaPrioridad

                    contadorPrioridad = contadorPrioridad + 1
                Next





                For Each elBCD In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elBCD(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorBCD = (totalLista) Then
                        comaBCD = ""
                        puntoyComa = ");"
                    Else
                        comaBCD = ","
                        puntoyComa = ""
                    End If

                    If (contadorBCD >= 0) Then

                        queryTablaBCDA &= elBCD(0) & tipoDato & comaBCD
                        queryTablaBCDB &= elBCD(0) & comaBCD
                        queryTablaBCDC &= elBCD(0) & comaBCD
                    End If

                    contadorBCD = contadorBCD + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''#tblProveedor''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elProveedor In lista

                    tipoDato = vbEmpty
                    colate = ""
                    Select Case elProveedor(4)
                        Case "TEXTO"
                            tipoDato = " VARCHAR (100) COLLATE Latin1_General_BIN"
                            colate = " COLLATE Latin1_General_BIN"
                        Case "NUMÉRICO"
                            tipoDato = ""
                        Case "FECHA"
                            tipoDato = " DATE"
                        Case "MONEDA"
                            tipoDato = ""
                        Case Else
                            tipoDato = vbEmpty
                    End Select

                    If contadorProveedor = (totalLista) Then
                        comaProveedor = ""
                        puntoyComa = ");"
                    Else
                        comaProveedor = ","
                        puntoyComa = ""
                    End If

                    If (contadorProveedor >= 0) Then

                        If elProveedor(1) = "hotel" Then


                            colExtra = "IIF(
                                 hotel LIKE '%F.I.%',
                                 REPLACE( hotel, 'F.I.', '' ),
                                IIF( hotel LIKE '%F.A.%', REPLACE( hotel, 'F.A.', '' ), hotel )) AS hotel"

                            queryTablaProveedorC &= colExtra & comaProveedor

                        Else
                            queryTablaProveedorC &= elProveedor(1) & comaProveedor
                            colExtra = ""
                        End If


                        queryTablaProveedorA &= elProveedor(1) & tipoDato & comaProveedor
                        queryTablaProveedorB &= elProveedor(1) & comaProveedor
                        columnProveedorBB &= elProveedor(1) & bool & comaProveedor
                    End If

                    contadorProveedor = contadorProveedor + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliados In lista

                    If contadorConciliados = (totalLista) Then
                        comaConciliados = ""
                        puntoyComa = ");"
                    Else
                        comaConciliados = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliados >= 0) Then
                        queryTablaConciliadosA &= elConciliados(1) & bool & int & comaConciliados
                        queryTablaConciliadosB &= elConciliados(1) & bool & comaConciliados & puntoyComa
                    End If

                    contadorConciliados = contadorConciliados + 1
                Next
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinal In lista

                    If contadorConciliadosFinal = (totalLista) Then
                        comaConciliadosFinal = ""
                        puntoyComa = ");"
                    Else
                        comaConciliadosFinal = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosFinal >= 0) Then
                        queryTablaConciliadosFinalA &= elConciliadosFinal(1) & bool & int & comaConciliadosFinal
                        queryTablaConciliadosFinalB &= elConciliadosFinal(1) & bool & comaConciliadosFinal & puntoyComa
                    End If

                    contadorConciliadosFinal = contadorConciliadosFinal + 1
                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''#tblConciliadosResultado'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosResultado In lista

                    If contadorConciliadosResultado = (totalLista) Then
                        comaConciliadosResultado = ""
                        puntoyComa = "); BEGIN "
                    Else
                        comaConciliadosResultado = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosResultado >= 0) Then
                        queryTablaConciliadosResultadoA &= elConciliadosResultado(1) & bool & int & comaConciliadosResultado
                        queryTablaConciliadosResultadoB &= elConciliadosResultado(1) & bool & comaConciliadosResultado & puntoyComa
                    End If

                    contadorConciliadosResultado = contadorConciliadosResultado + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliados'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosInsert In lista

                    If contadorConciliadosInsert = (totalLista) Then
                        comaConciliadosInsert = ""
                        puntoyComa = " FROM( "
                    Else
                        comaConciliadosInsert = ","
                        puntoyComa = ""
                    End If

                    If (contadorConciliadosInsert >= 0) Then
                        queryInsertConciliadosA &= elConciliadosInsert(1) & bool & comaConciliadosInsert & puntoyComa
                    End If

                    contadorConciliadosInsert = contadorConciliadosInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''INSERT #tblConciliadosFinal'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elConciliadosFinalInsert In lista

                    If contadorConciliadosFinalInsert = (totalLista) Then
                        comaConciliadosFinalInsert = ""
                    Else
                        comaConciliadosFinalInsert = ","
                    End If

                    If (contadorConciliadosFinalInsert >= 0) Then
                        queryInsertConciliadosFinalA &= elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                        queryInsertConciliadosFinalB &= "SUM(" & elConciliadosFinalInsert(1) & bool & ") AS " & elConciliadosFinalInsert(1) & bool & comaConciliadosFinalInsert
                    End If

                    contadorConciliadosFinalInsert = contadorConciliadosFinalInsert + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''''''''''''''''''''''''''''''''''''UPDATE'''''''''''''''''''''''''''''''''''''''''''''''''''''

                For Each elUpdate In lista

                    If contadorUpdate = (totalLista) Then
                        comaUpdate = ""
                    Else
                        comaUpdate = ","
                    End If

                    If (contadorUpdate >= 0) Then


                        queryUpdateA &= "IIF(temp." & elUpdate(1) & bool & " = 1,'" & elUpdate(1) & ",','')" & comaUpdate & ""
                        queryUpdateB &= "IIF(temp." & elUpdate(1) & bool & " = 0,'" & elUpdate(1) & ",','')" & comaUpdate & ""

                    End If

                    contadorUpdate = contadorUpdate + 1

                Next

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                For Each el In lista

                    If i = totalLista Then
                        coma = ""
                        puntoyComa = ");"
                    Else
                        coma = ","
                        puntoyComa = ""
                    End If


                    If (i >= 0) Then

                        columnBCD = el(0)
                        columnProveedor = el(1)
                        tipoOperacion = el(2)
                        diasRango = el(3)

                        operador = "ON"
                        If (tipoOperacion = "IGUALDAD") Then


                            condicion = operador & " " & " " & parentesisA & "BD." & columnBCD & " = " & "proveedor." & columnProveedor & parentesisB & " " & " "

                        ElseIf (tipoOperacion = "CONTIENE") Then

                            condicion = operador & "(" & "BD." & columnBCD & " LIKE " & "'%' + " & "proveedor." & columnProveedor & " + '%' OR proveedor." & columnProveedor & " LIKE " & "'%' + " & "BD." & columnBCD & " + '%' )"

                        ElseIf (tipoOperacion = "RANGO") Then

                            condicion = operador & " " & "proveedor." & columnProveedor & " BETWEEN " & "BD." & columnBCD & " AND DATEADD(day, " & diasRango & ",BD. " & columnBCD & ")"

                        End If

                        queryInsertConciliados &= " SELECT
                      proveedor.id,
                      BD.id AS IDBCD,
                      1 AS conteo,
                      '" & columnProveedor & "' AS columnas,"
                        queryInsertConciliadoCC = ""
                        j = 0

                        For Each cc In lista

                            selectorColumna = ""
                            selectorColumna = cc(1)

                            If j = totalLista Then
                                comaB = ""
                            Else
                                comaB = ","
                            End If

                            If (selectorColumna.Contains("{")) Then
                                selectorColumna = selectorColumna.Replace("{", "[")
                                selectorColumna = selectorColumna.Replace("}", "]")
                            End If


                            If selectorColumna = columnProveedor Then
                                selector = 1
                            Else
                                selector = 0
                            End If



                            queryInsertConciliadoCC &= selector & " AS " & selectorColumna & bool & comaB

                            j = j + 1
                        Next



                        queryInsertConciliados &= queryInsertConciliadoCC & "
                         FROM
                          #tblProveedor proveedor
                          INNER JOIN #tblBDBCD BD  
                          " & condicion & " UNION "

                        count = count + 1

                    End If
                    i = i + 1
                Next

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim queryTablas As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosResultado') IS NULL  DROP TABLE #tblConciliadosResultado "

                queryCompletoBCD = queryTablaBCDA & queryTablaBCDB & queryTablaBCDC & queryTablaBCDD & fechasA & queryEstatusConciliado
                queryCompletoProveedor = queryTablaProveedorA & queryTablaProveedorB & queryTablaProveedorC & queryTablaProveedorD & queryEstatusConciliadoP & queryEstatusEliminado & fechasPagoProveedor

                queryCompletoConciliados = queryTablaConciliadosA & queryTablaConciliadosB
                queryCompletoConciliadosFinal = queryTablaConciliadosFinalA & queryTablaConciliadosFinalB

                Dim queryConciliadosAux = "	
                CREATE TABLE #tblConciliadosAux (
                id INT IDENTITY ( 1, 1 ),
                idProv INT )
                CREATE CLUSTERED INDEX ix_tblConciliadosAux 
                ON #tblConciliadosAux ( id, idProv ); "

                queryCompletoConciliadosResultado = queryTablaConciliadosResultadoA & queryTablaConciliadosResultadoB

                queryInsertConciliados &= ") tablaTMP 
                    GROUP BY
                    IDBCD,id," & columnProveedorBB & "
                    HAVING
                     SUM ( conteo ) >= 0 
                     END "

                queryCompletoInsertConciliados = queryInsertConciliadosA & queryInsertConciliados

                queryCompletoInsertConciliados = queryCompletoInsertConciliados.Replace("UNION )", ")")

                queryCompletoInsertConciliadosFinal = queryInsertConciliadosFinalA & queryInsertConciliadosFinalB & queryInsertConciliadosFinalC


                Dim queryConciliadosAuxB = " INSERT INTO #tblConciliadosAux SELECT id FROM #tblConciliadosFinal  GROUP BY id ORDER BY id "

                Dim queryTablaB As String = " IF NOT OBJECT_ID('TEMPDB..#tblBDBCD') IS NULL  DROP TABLE #tblBDBCD 
                IF NOT OBJECT_ID('TEMPDB..#tblProveedor') IS NULL  DROP TABLE #tblProveedor
                IF NOT OBJECT_ID('TEMPDB..#tblConciliados') IS NULL  DROP TABLE #tblConciliados "


                Dim consultaFinal As String = "
            DECLARE @inicio INT
            DECLARE @total INT
            DECLARE @maximo INT
            DECLARE @idProv INT
            DECLARE @maxCondiciones INT
            DECLARE @idBCD INT
            set @inicio = 1
        	SET @maxCondiciones = " & totalListaA & " 
        	SET @total = ( SELECT COUNT ( * ) FROM #tblConciliadosAux )
        WHILE
        		( @inicio <= @total ) BEGIN

        			SET @idProv = ( SELECT idProv FROM #tblConciliadosAux WHERE id = @inicio ) 
        			IF (select count(*) from #tblConciliadosResultado where id = @idProv) = 0
	begin
		set @maximo = (	SELECT top(1) suma FROM #tblConciliadosFinal where id = @idProv order by suma desc)
       
		IF (SELECT COUNT(*) FROM #tblConciliadosFinal where id = @idProv and suma = @maximo) > 1
			begin
				insert into #tblConciliadosResultado
				select top(1) * from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & "

				set @idBcd = (select top(1) idBcd from #tblConciliadosFinal 
				where id = @idProv and suma = @maximo
				order by " & columnaPrioridad & ")
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
		ELSE 
			begin
				insert into #tblConciliadosResultado
				SELECT * FROM #tblConciliadosFinal where id = @idProv and suma = @maximo

				set @idBcd = (SELECT idBcd FROM #tblConciliadosFinal where id = @idProv and suma = @maximo)
				delete from #tblConciliadosFinal where idBcd = @idBcd
			end
	end	
	SELECT @inicio = @inicio + 1



END"

                queryCompletoUpdate = queryUpdateA & queryUpdateB & queryUpdateC


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                Dim queryTablaC As String = "
                If NOT OBJECT_ID('TEMPDB..#tblConciliadosFinal') IS NULL  DROP TABLE #tblConciliadosFinal
                IF NOT OBJECT_ID('TEMPDB..#tblConciliadosAux') IS NULL  DROP TABLE #tblConciliadosAux "


                Dim queyAll As String = queryTablas & queryCompletoBCD & queryCompletoProveedor & queryCompletoConciliados & queryCompletoConciliadosFinal &
                    queryConciliadosAux & queryCompletoConciliadosResultado & queryCompletoInsertConciliados & queryCompletoInsertConciliadosFinal & queryConciliadosAuxB & queryTablaB & consultaFinal &
                    queryCompletoUpdate & queryTablaC

                objetoCapaDatos.CDtemporalAuto(queyAll)

            End If

        End Sub

    End Class

End Namespace

