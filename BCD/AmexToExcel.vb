Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text
Imports HtmlAgilityPack
Imports System.Globalization
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports iTextSharp.text.pdf
Imports System.CodeDom.Compiler
Imports System.CodeDom
Imports System.Runtime.CompilerServices

Public Class AmexToExcel
    Private Event ConversionArchivoEnProgreso As EventHandler
    Private Event ConversionArchivoFinalizado As EventHandler
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AmexToExcel_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DataGridView1.Rows.Clear()
        TextBox1.Text = String.Empty
        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        OpenFileDialog1.RestoreDirectory = True
        OpenFileDialog1.Title = "Seleccionar reportes AMEX/SANTANDER PDF"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.DefaultExt = "pdf"
        OpenFileDialog1.Filter = "Archivos de reporte AMEX/SANTANDER (*.pdf)|*.pdf"
        OpenFileDialog1.FilterIndex = 0
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.CheckPathExists = True
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            If OpenFileDialog1.FileNames.Count > 0 Then
                For Each fileName In OpenFileDialog1.FileNames
                    DataGridView1.Rows.Add(fileName, "PENDIENTE")
                Next
            End If
        End If
    End Sub

    Private Function ConvertirAmexAPDF(ByVal _filePath As String, ByVal _saveFolderPath As String, ByVal rowIndex As Integer) As Boolean
        If File.Exists(_filePath) And Path.GetExtension(_filePath).ToLower() = ".pdf" Then
            Dim _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim _programDirectory = Path.Combine(_path, "DataExtractor")

            '// Si no existe el directorio para los datos del programa lo creamos
            If Not Directory.Exists(_programDirectory) Then
                Directory.CreateDirectory(_programDirectory)
            Else
                Dim di As DirectoryInfo = New DirectoryInfo(_programDirectory)

                For Each _file As FileInfo In di.GetFiles()
                    Try
                        _file.Delete()
                    Catch ex As Exception
                        Console.WriteLine("Error de IO al borrar el archivo.")
                    End Try
                Next
            End If
            Dim exePath As String = Path.Combine(_programDirectory, "pdftohtml.exe")

            ' // Escribimos en el directorio de programa el exe que incluimos como recurso
            File.WriteAllBytes(exePath, My.Resources.pdftohtml)
            File.Copy(_filePath, Path.Combine(_programDirectory, Path.GetFileName(_filePath)))


            Dim startInfo As ProcessStartInfo = New ProcessStartInfo()

            ' // Configuramos los parametros para iniciar el proceso que transforma el pdf a html
            startInfo.Arguments = String.Format("-i -noframes ""{0}"" ""{1}.html""", Path.GetFileName(_filePath), Path.GetFileNameWithoutExtension(_filePath))
            startInfo.UseShellExecute = False
            startInfo.WorkingDirectory = _programDirectory
            startInfo.CreateNoWindow = True
            startInfo.RedirectStandardOutput = True
            startInfo.FileName = exePath

            '// Iniciamos el proceso pasandole como parametro cada uno de los archivos pdf que le hemos pasado como argumento.
            Try
                Using exeProcess As Process = Process.Start(startInfo)
                    DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVIRTIENDO..."
                    RaiseEvent ConversionArchivoEnProgreso(Me, EventArgs.Empty)
                    exeProcess.EnableRaisingEvents = True
                    exeProcess.Start()
                    Dim output = exeProcess.StandardOutput.ReadToEnd()
                    exeProcess.WaitForExit()

                    Dim _document As HtmlDocument = New HtmlDocument()
                    Using fs As FileStream = New FileStream(String.Format("{0}\{1}.html", _programDirectory, Path.GetFileNameWithoutExtension(_filePath)), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        Using sr As StreamReader = New StreamReader(fs, Encoding.GetEncoding("ISO-8859-1"))
                            _document.Load(sr)
                        End Using
                    End Using
                    Dim _root = _document.DocumentNode
                    Dim _sb = New StringBuilder()
                    Dim _sbProcesado = New StringBuilder()
                    For Each node In _root.DescendantsAndSelf()
                        If Not node.HasChildNodes Then
                            Dim text = node.InnerText
                            If Not String.IsNullOrEmpty(text) Then
                                _sb.AppendLine(text.Trim())
                            End If
                        End If
                    Next

                    Dim _fechaFacturacionPatron As String = "(Periodo de facturación: Del )(\d+) de (Enero|Febrero|Marzo|Abril|Mayo|Junio|Julio|Agosto|Septiembre|Octubre|Noviembre|Diciembre) al (\d+) de (Enero|Febrero|Marzo|Abril|Mayo|Junio|Julio|Agosto|Septiembre|Octubre|Noviembre|Diciembre) de (\d+)"
                    Dim _regexFechaFacturacion As Regex = New Regex(_fechaFacturacionPatron)
                    Dim matchesFacturacion As MatchCollection = _regexFechaFacturacion.Matches(_sb.ToString())
                    Dim _fechaMatch = matchesFacturacion.Cast(Of Match)().FirstOrDefault()
                    Dim _primerMesIndex As Integer = 0
                    Dim _segundoMesIndex As Integer = 0
                    Dim _anyofacturacion As Integer = 0

                    If _fechaMatch IsNot Nothing Then
                        _primerMesIndex = Convert.ToInt16(ListaMeses.IndexOf(_fechaMatch.Groups(3).Value.ToString())) + 1
                        _segundoMesIndex = Convert.ToInt16(ListaMeses.IndexOf(_fechaMatch.Groups(5).Value.ToString())) + 1
                        _anyofacturacion = Convert.ToInt16(_fechaMatch.Groups(6).Value)
                    End If
                    File.WriteAllText(_programDirectory & "\texto.txt", _sb.ToString())
                    Using _stream As Stream = GenerateStreamFromString(_sb.ToString())
                        Dim _sreader As StreamReader = New StreamReader(_stream, Encoding.UTF8)
                        Dim descartarLineasSiguientes As Boolean = False
                        Do While _sreader.Peek() >= 0
                            Dim linea As String = _sreader.ReadLine()
                            If (linea.Contains("Corporate Purchasing Card Sam's Club") Or linea.Contains("Corporate Meeting Card") Or linea.Contains(". . . . . .") Or linea.Contains("Total de nuevos cargos y abonos de ")) And descartarLineasSiguientes = False Then
                                descartarLineasSiguientes = True
                            End If

                            If Not descartarLineasSiguientes And Not linea.Contains("Este no es un documento con") Then
                                If Not linea.StartsWith("Tarjeta ") Then
                                    If Not String.IsNullOrEmpty(linea) Then
                                        _sbProcesado.AppendLine(linea)
                                    End If
                                End If
                            End If

                            If linea.Contains("Detalle de nuevos cargos y abonos de ") Or (linea.Contains("(continuación)")) Then
                                descartarLineasSiguientes = False
                            End If
                        Loop
                    End Using
                    Dim dtPDFToExcel As DataTable = New DataTable()
                    dtPDFToExcel.Clear()
                    Dim IdColumn As DataColumn = New DataColumn("ID")
                    IdColumn.DataType = GetType(UInt32)
                    IdColumn.AutoIncrement = True
                    IdColumn.AutoIncrementSeed = 1
                    IdColumn.AutoIncrementStep = 1
                    dtPDFToExcel.Columns.Add(IdColumn)
                    dtPDFToExcel.Columns.Add("FECHA")
                    dtPDFToExcel.Columns.Add("EMPRESA")
                    dtPDFToExcel.Columns.Add("RFC")
                    dtPDFToExcel.Columns.Add("REFERENCIA")
                    dtPDFToExcel.Columns.Add("CLAVE")
                    dtPDFToExcel.Columns.Add("IMPORTE_MON_NAC", GetType(Decimal))
                    dtPDFToExcel.Columns.Add("IMPORTE_MON_EXT", GetType(Decimal))
                    dtPDFToExcel.Columns.Add("TIPO_MON_EXT")
                    dtPDFToExcel.Columns.Add("CR")

                    Dim patron As String = "([0-9]{1,2} de (Ene|Feb|Mar|Abr|May|Jun|Jul|Ago|Sep|Oct|Nov|Dic))(\n|\r\n)([0-9a-zA-Z- &?¿'#\(\):;,.""%$\/_-]+(\n|\r\n)*)(\n|\r\n)((((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?)(\n|\r\n)(((?!Importe)[A-Z0-9]+) \/REF([0-9]+)(\/([A-Z]+))*((\n|\r\n)(CR))*)*(\n|\r\n)*(Importe en moneda extranjera +((((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?) ([A-Za-záéíóú .]+)*((\n|\r\n)(CR))*)*"
                    Dim _regex As Regex = New Regex(patron)
                    Dim sb_html_excaped As String = Web.HttpUtility.HtmlDecode(_sbProcesado.ToString())
                    File.WriteAllText(Path.Combine(_programDirectory, "procesado.txt"), sb_html_excaped)
                    Dim matches As MatchCollection = _regex.Matches(sb_html_excaped)
                    Dim maxReferenciaDigitos As IEnumerable(Of String) = matches.Cast(Of Match)().Select(Function(x) x.Groups.Item(17).ToString())
                    Dim maxReferenciaList As List(Of String) = maxReferenciaDigitos.ToList()
                    Dim MayorString As String = maxReferenciaList.Aggregate("", Function(max, cur) If(max.Length > cur.Length, max, cur))
                    Dim ReferenciaMayorLongitud As Integer = MayorString.Length

                    For Each _match As Match In matches
                        Dim Fecha As DateTime = Nothing
                        Dim Empresa As String = String.Empty
                        Dim RFC As String = String.Empty
                        Dim Referencia As String = String.Empty
                        Dim Clave As String = String.Empty
                        Dim Importe_Mon_Nac As Decimal = 0
                        Dim Importe_Mon_Ext As Decimal = 0
                        Dim Tipo_Mon_Ext As String = String.Empty
                        Dim CR As String = String.Empty

                        Dim _anyoFecha As Integer = 0
                        Dim _mov_mes As Integer = ListaMesesAbreviados.IndexOf(_match.Groups.Item(2).ToString()) + 1

                        If _segundoMesIndex - _primerMesIndex > 0 Then
                            _anyoFecha = _anyofacturacion
                        Else
                            If _mov_mes > _segundoMesIndex Then
                                _anyoFecha = _anyofacturacion - 1
                            Else
                                _anyoFecha = _anyofacturacion
                            End If
                        End If

                        Dim _fechaString = _match.Groups.Item(1).ToString()
                        Dim _fechaDia As Integer = Integer.Parse(Regex.Match(_fechaString, "\d+").Value)
                        Dim _fechaMes As Integer = _mov_mes
                        Dim _fechaAnyo = _anyoFecha

                        Fecha = New DateTime(_fechaAnyo, _fechaMes, _fechaDia)
                        Empresa = _match.Groups.Item(4).ToString()
                        RFC = _match.Groups.Item(16).ToString()
                        Referencia = If(String.IsNullOrEmpty(_match.Groups.Item(17).ToString()), "", _match.Groups.Item(17).ToString().PadLeft(ReferenciaMayorLongitud, "0"))
                        Clave = _match.Groups.Item(19).ToString()
                        If Not String.IsNullOrEmpty(_match.Groups.Item(7).ToString()) Then
                            Importe_Mon_Nac = DecimalParse(_match.Groups.Item(7).ToString())
                        End If
                        If Not String.IsNullOrEmpty(_match.Groups.Item(25).ToString()) Then
                            Importe_Mon_Ext = DecimalParse(_match.Groups.Item(25).ToString())
                        End If
                        Tipo_Mon_Ext = _match.Groups.Item(32).ToString()
                        If _match.Groups.Item(22).ToString() <> String.Empty Then
                            CR = _match.Groups.Item(22).ToString()
                            Importe_Mon_Nac = -Importe_Mon_Nac
                            Importe_Mon_Ext = -Importe_Mon_Ext
                        End If
                        If _match.Groups.Item(35).ToString() <> String.Empty Then
                            CR = _match.Groups.Item(35).ToString()
                            Importe_Mon_Nac = -Importe_Mon_Nac
                            Importe_Mon_Ext = -Importe_Mon_Ext
                        End If
                        dtPDFToExcel.Rows.Add(Nothing, Fecha.ToShortDateString(), Empresa, RFC, Referencia, Clave, Importe_Mon_Nac, If(Importe_Mon_Ext = 0, DBNull.Value, Importe_Mon_Ext), Tipo_Mon_Ext, CR)

                    Next
                    dtPDFToExcel.AcceptChanges()

                    ' // INICIO DE LA EXPORTACION DEL ARCHIVO EXCEL
                    Using objExcelPackage As ExcelPackage = New ExcelPackage()
                        Dim objWorksheet As ExcelWorksheet = objExcelPackage.Workbook.Worksheets.Add("HOJA 1")
                        objWorksheet.Cells("A1").LoadFromDataTable(dtPDFToExcel, True)
                        objWorksheet.Cells.Style.Font.SetFromFont(New Font("Calibri", 12))
                        objWorksheet.Cells.AutoFitColumns()
                        Using objRange As ExcelRange = objWorksheet.Cells(1, 1, 1, dtPDFToExcel.Columns.Count)
                            objRange.Style.Font.Bold = True
                            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center
                            objRange.Style.Fill.PatternType = ExcelFillStyle.Solid
                            objRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                        End Using
                        If File.Exists(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx")) Then
                            If MessageBox.Show("El archivo " & Path.GetFileNameWithoutExtension(_filePath) & ".xlsx" & " ya existe en la ruta de guardado ¿Desea sobreescribirlo?", "El archivo que intenta crear ya existe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                                Try
                                    File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                                Catch ex As Exception
                                    MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Return False
                                End Try
                            End If
                        Else
                            Try
                                File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                            Catch ex As Exception
                                MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return False
                            End Try

                        End If
                        If InvokeRequired Then
                            Invoke(Sub()
                                       DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                                   End Sub)
                        Else
                            DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                        End If
                    End Using
                    ' // FIN DE LA EXPORTACION DEL ARCHIVO EXCEL
                    Return True
                End Using
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
                Console.WriteLine(ex.Message)
                Console.WriteLine(ex.StackTrace)
                Return False
            End Try
        End If
        Return False
    End Function


    Private Function ConvertirIBLAPDF(ByVal _filePath As String, ByVal _saveFolderPath As String, ByVal rowIndex As Integer) As Boolean
        If File.Exists(_filePath) And Path.GetExtension(_filePath).ToLower() = ".pdf" Then
            Dim _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim _programDirectory = Path.Combine(_path, "DataExtractor")
            Dim AnyoFacturacion As Integer = 0

            '// Si no existe el directorio para los datos del programa lo creamos
            If Not Directory.Exists(_programDirectory) Then
                Directory.CreateDirectory(_programDirectory)
            Else
                Dim di As DirectoryInfo = New DirectoryInfo(_programDirectory)

                For Each _file As FileInfo In di.GetFiles()
                    _file.Delete()
                Next
            End If
            Dim exePath As String = Path.Combine(_programDirectory, "pdftohtml.exe")

            ' // Escribimos en el directorio de programa el exe que incluimos como recurso
            File.WriteAllBytes(exePath, My.Resources.pdftohtml)
            File.Copy(_filePath, Path.Combine(_programDirectory, Path.GetFileName(_filePath)))


            Dim startInfo As ProcessStartInfo = New ProcessStartInfo()

            ' // Configuramos los parametros para iniciar el proceso que transforma el pdf a html
            startInfo.Arguments = String.Format("-i -noframes ""{0}"" ""{1}.html""", Path.GetFileName(_filePath), Path.GetFileNameWithoutExtension(_filePath))
            startInfo.UseShellExecute = False
            startInfo.WorkingDirectory = _programDirectory
            startInfo.CreateNoWindow = True
            startInfo.RedirectStandardOutput = True
            startInfo.FileName = exePath

            '// Iniciamos el proceso pasandole como parametro cada uno de los archivos pdf que le hemos pasado como argumento.
            Try
                Using exeProcess As Process = Process.Start(startInfo)
                    DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVIRTIENDO..."
                    RaiseEvent ConversionArchivoEnProgreso(Me, EventArgs.Empty)
                    exeProcess.EnableRaisingEvents = True
                    exeProcess.Start()
                    Dim output = exeProcess.StandardOutput.ReadToEnd()
                    exeProcess.WaitForExit()

                    Dim _document As HtmlDocument = New HtmlDocument()
                    Using fs As FileStream = New FileStream(String.Format("{0}\{1}.html", _programDirectory, Path.GetFileNameWithoutExtension(_filePath)), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        Using sr As StreamReader = New StreamReader(fs, Encoding.GetEncoding("ISO-8859-1"))
                            _document.Load(sr)
                        End Using
                    End Using
                    Dim _root = _document.DocumentNode
                    Dim _sb = New StringBuilder()
                    Dim _sb_procesado As String = String.Empty
                    For Each node In _root.DescendantsAndSelf()
                        If Not node.HasChildNodes Then
                            Dim text = node.InnerText
                            If Not String.IsNullOrEmpty(text) Then
                                _sb.Append(text)
                            End If
                        End If
                    Next

                    _sb_procesado = Regex.Replace(_sb.ToString(), "^\s+$[\r\n]*", "", RegexOptions.Multiline)
                    _sb_procesado = Regex.Replace(_sb_procesado.ToString(), "^\s+", "", RegexOptions.Multiline)

                    Dim _FechaFacturacionPatron As String = "^([0-3][0-9])\/([0-3][0-9])\/(?:(([0-9][0-9])?[0-9][0-9]))\r$"
                    Dim _RegexFechaFacturacion As Regex = New Regex(_FechaFacturacionPatron, RegexOptions.Multiline)
                    Dim _MatchesFacturacion As MatchCollection = _RegexFechaFacturacion.Matches(_sb_procesado.ToString())
                    Dim _FechaMatch As Match = _MatchesFacturacion.Cast(Of Match)().FirstOrDefault()
                    Dim Temp As Date = Nothing
                    Dim _FechaFacturacion As Date = Nothing
                    Dim formatProvider As DateTimeFormatInfo = New DateTimeFormatInfo()
                    formatProvider.Calendar.TwoDigitYearMax = DateTime.Now.Year

                    If _FechaMatch IsNot Nothing Then
                        Temp = Date.ParseExact(_FechaMatch.Value.Trim(), "MM/dd/yy", formatProvider)
                        _FechaFacturacion = Date.ParseExact(Temp.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        AnyoFacturacion = _FechaFacturacion.Year
                    End If
                    Dim lineas() As String = Regex.Split(_sb_procesado, "\r?\n|\r")
                    Dim _sb_limpio As StringBuilder = New StringBuilder()
                    Dim EnBloque As Boolean = False

                    For i As Integer = 0 To lineas.Count - 1
                        If lineas(i).StartsWith("MEMO STATEMENT") Or lineas(i).StartsWith("STATEMENT DATE") Then
                            EnBloque = True
                        End If

                        If lineas(i).StartsWith("Amount") Then
                            EnBloque = False
                        End If

                        If EnBloque = False And (lineas(i).StartsWith("Amount") = False Or lineas(i).StartsWith("PAGE")) Then
                            _sb_limpio.AppendLine(lineas(i))
                        End If
                    Next

                    Dim _sb_limpio_2 As String = Regex.Replace(_sb_limpio.ToString(), "^IBLA Hotel.*$", "", RegexOptions.Multiline)
                    Dim _sb_limpio_3 As String = Regex.Replace(_sb_limpio_2.ToString(), "^PAGE .*$", "", RegexOptions.Multiline)

                    Dim _sb_limpio_final As String = Regex.Replace(_sb_limpio_3.ToString(), "^\s+$[\r\n]*", "", RegexOptions.Multiline)
                    _sb_limpio_final = Regex.Replace(_sb_limpio_final.ToString(), "^\s+", "", RegexOptions.Multiline)

                    Dim dtPDFToExcel As DataTable = New DataTable()
                    dtPDFToExcel.Clear()
                    Dim IdColumn As DataColumn = New DataColumn("ID")
                    IdColumn.DataType = GetType(UInt32)
                    IdColumn.AutoIncrement = True
                    IdColumn.AutoIncrementSeed = 1
                    IdColumn.AutoIncrementStep = 1
                    dtPDFToExcel.Columns.Add(IdColumn)
                    dtPDFToExcel.Columns.Add("POST_DATE")
                    dtPDFToExcel.Columns.Add("TRAN_DATE")
                    dtPDFToExcel.Columns.Add("REF_NUMBER")
                    dtPDFToExcel.Columns.Add("TRAN_DESC")
                    dtPDFToExcel.Columns.Add("MON_NAC", GetType(Decimal))
                    dtPDFToExcel.Columns.Add("MON_EXT", GetType(Decimal))
                    dtPDFToExcel.Columns.Add("MON_EXT_RATE", GetType(Decimal))

                    Dim lineas_sb_limpio() As String = Regex.Split(_sb_limpio_final.ToString(), "\r?\n|\r")

                    For i As Integer = 0 To lineas_sb_limpio.Count - 2 Step 6
                        Dim post_date_month_day As String() = Regex.Split(lineas_sb_limpio(i), "-")
                        Dim post_date As Date = New Date(AnyoFacturacion, Integer.Parse(post_date_month_day(0)), Integer.Parse(post_date_month_day(1)))

                        Dim tran_date_month_day As String() = Regex.Split(lineas_sb_limpio(i + 1), "-")
                        Dim tran_date As Date = New Date(AnyoFacturacion, Integer.Parse(tran_date_month_day(0)), Integer.Parse(tran_date_month_day(1)))

                        Dim ref_number As String = lineas_sb_limpio(i + 2)

                        Dim tran_desc As String = lineas_sb_limpio(i + 3)

                        Dim importe_mon_ext As Decimal = 0

                        Dim pattern As String = "\(FOREIGN CURRENCY\)\s+\$((?<=[^\d,.]|^)\d{1,3}(,(\d{3}))*((?=[,.]\s)|(\.\d+)?(?=[^\d,.]|$))(\-*))\s+[A-Za-z]{3}\s+(\d{2}\/(\d{2}))\s+\(RATE\)\s+((?<=[^\d,.]|^)\d{1,3}(,(\d{3}))*((?=[,.]\s)|(\.\d+)?(?=[^\d,.]|$)))"

                        Dim mMatch As Match = Regex.Match(lineas_sb_limpio(i + 5), pattern, RegexOptions.Multiline)

                        Dim importe_mon_nac As Decimal = 0

                        Dim importe_mon_ext_rate As Decimal = 0

                        If mMatch IsNot Nothing Then
                            If mMatch.Success Then
                                If mMatch.Groups(6).Value = "-" Then
                                    importe_mon_ext = -1.0 * Decimal.Parse(Regex.Replace(lineas_sb_limpio(i + 4), "[A-Za-z]", ""))
                                Else
                                    importe_mon_ext = Decimal.Parse(Regex.Replace(lineas_sb_limpio(i + 4), "[A-Za-z]", ""))
                                End If
                                importe_mon_nac = Decimal.Parse(mMatch.Groups(1).Value)
                                importe_mon_ext_rate = Decimal.Parse(mMatch.Groups(9).Value)
                            End If
                        End If

                        dtPDFToExcel.Rows.Add(Nothing, post_date.ToShortDateString(), tran_date.ToShortDateString(), ref_number, tran_desc, importe_mon_nac, importe_mon_ext, importe_mon_ext_rate)
                    Next
                    dtPDFToExcel.AcceptChanges()

                    ' // INICIO DE LA EXPORTACION DEL ARCHIVO EXCEL
                    Using objExcelPackage As ExcelPackage = New ExcelPackage()
                        Dim objWorksheet As ExcelWorksheet = objExcelPackage.Workbook.Worksheets.Add("HOJA 1")
                        objWorksheet.Cells("A1").LoadFromDataTable(dtPDFToExcel, True)
                        objWorksheet.Cells.Style.Font.SetFromFont(New Font("Calibri", 12))
                        objWorksheet.Cells.AutoFitColumns()
                        Using objRange As ExcelRange = objWorksheet.Cells(1, 1, 1, dtPDFToExcel.Columns.Count)
                            objRange.Style.Font.Bold = True
                            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center
                            objRange.Style.Fill.PatternType = ExcelFillStyle.Solid
                            objRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                        End Using
                        If File.Exists(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx")) Then
                            If MessageBox.Show("El archivo " & Path.GetFileNameWithoutExtension(_filePath) & ".xlsx" & " ya existe en la ruta de guardado ¿Desea sobreescribirlo?", "El archivo que intenta crear ya existe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                                Try
                                    File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                                Catch ex As Exception
                                    MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Return False
                                End Try
                            End If
                        Else
                            Try
                                File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                            Catch ex As Exception
                                MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return False
                            End Try

                        End If

                        If InvokeRequired Then
                            Invoke(Sub()
                                       DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                                   End Sub)
                        Else
                            DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                        End If
                    End Using
                    Return True
                End Using
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
                Console.WriteLine(ex.Message)
                Console.WriteLine(ex.StackTrace)
                Return False
            End Try
        End If
        Return False
    End Function

    Private Function DecimalParse(ByVal s As String) As Decimal
        Return Decimal.Parse(s, NumberStyles.Currency, CultureInfo.InvariantCulture)
    End Function

    Private Function ConvertirSantanderAExcel(ByVal _filePath As String, ByVal _saveFolderPath As String, ByVal rowIndex As Integer) As Boolean
        If File.Exists(_filePath) And Path.GetExtension(_filePath).ToLower() = ".pdf" Then

            Dim dtPDFToExcel As DataTable = New DataTable()
            dtPDFToExcel.Clear()
            dtPDFToExcel.Columns.Add("NUMERO", GetType(Integer))
            dtPDFToExcel.Columns.Add("FECHA")
            dtPDFToExcel.Columns.Add("NOMBRE")
            dtPDFToExcel.Columns.Add("REFERENCIA")
            dtPDFToExcel.Columns.Add("MONEDA")
            dtPDFToExcel.Columns.Add("IMPORTE_MONEDA", GetType(Decimal))
            dtPDFToExcel.Columns.Add("BRUTO_MXN", GetType(Decimal))


            Dim _PDFReader As PdfReader = New PdfReader(_filePath)
            Dim _sbtexto As StringBuilder = New StringBuilder()

            For ipage As Integer = 1 To _PDFReader.NumberOfPages - 2
                Dim numero_transaccion As util.RectangleJ = New util.RectangleJ(12, 121, 35, 318)
                Dim numeros As String = ExtraerTextoDesdeLocalizacion(_filePath, ipage, numero_transaccion)
                Dim lineas = Regex.Split(numeros, "\r\n|\r|\n")
                Dim moneda_index As Integer = -1

                For Each linea In lineas
                    Dim pesoIndex As Integer = linea.IndexOf("MXN")
                    Dim dolarIndex As Integer = linea.IndexOf("USD")
                    Dim euroIndex As Integer = linea.IndexOf("EUR")

                    If pesoIndex > -1 Then
                        moneda_index = pesoIndex
                        Exit For
                    End If

                    If dolarIndex > -1 Then
                        moneda_index = dolarIndex
                        Exit For
                    End If

                    If euroIndex > -1 Then
                        moneda_index = euroIndex
                        Exit For
                    End If
                Next

                For lindex As Integer = 0 To lineas.Count - 2
                    'If Not Regex.IsMatch(linea, "^\s+MXN", RegexOptions.Multiline) Then
                    '_sbtexto.AppendLine(linea.Remove(52, moneda_index - 52))
                    'Else
                    Dim linea_cortada As String = lineas(lindex).Remove(52, moneda_index - 52)
                    Dim linea_siguiente As String = lineas(lindex + 1).Remove(52, moneda_index - 52)
                    If Not Regex.IsMatch(linea_cortada, "^\s+MXN", RegexOptions.Multiline) Then
                        If Not Regex.IsMatch(linea_cortada, "^\s*$", RegexOptions.Multiline) Then
                            If Not Regex.IsMatch(linea_cortada, "^\s+(((((\d{1,3})(\,\d{3})*)|(\d+))\.(\d+)?)[-]*)", RegexOptions.Multiline) Then
                                If Not Regex.IsMatch(linea_cortada, "^\----------", RegexOptions.Multiline) Then
                                    Dim linea1 As String = Regex.Replace(linea_cortada, "^\s+", "", RegexOptions.Multiline)
                                    linea1 = Regex.Replace(linea1, "\r\n", "")
                                    If Regex.IsMatch(linea1, "^[0-9]+[\s]+([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})", RegexOptions.Multiline) Then
                                        _sbtexto.AppendLine(linea1 & linea_siguiente)
                                    End If
                                End If
                            End If
                        End If
                    End If
                    'Console.WriteLine(lineas(lindex).Remove(52, moneda_index - 52))
                    'End If
                Next
            Next

            Dim pattern As String = "^([0-9]+)[\s]+([0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{2})(\s+)([a-zA-Z ,.'-\/]+)((-)*(\d+)*(\d+\-\d+)*(<->)*([A-Z]+)*)(\s+)([A-Z]{3})(\s+)((((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))[-]*)(\s+)(((((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))[-]*)(\s+(A|\+|\*)\s+))*(((((((\d{1,3})(\,\d{3})*)|(\d+))(.\d+)?))[-]*))*(\s+)([A-Z]+|[\-]|[0-9]{2,}|<\->|[0]{1})*"

            Dim options As RegexOptions = RegexOptions.Multiline
            ' Numero es m.Groups(1).Value
            ' Fecha es m.Groups(2).Value
            ' Nombre es m.Groups(4).Value
            ' Referencia es m.Groups(46).Value
            ' Moneda es m.Groups(12).Value
            ' Importe moneda es es m.Groups(14).Value
            ' Importe bruto mxn es m.Groups(36).Value

            For Each m As Match In Regex.Matches(_sbtexto.ToString(), pattern, options)
                Dim _row As DataRow = dtPDFToExcel.NewRow()
                _row("NUMERO") = Integer.Parse(m.Groups(1).Value)
                _row("FECHA") = m.Groups(2).Value
                _row("NOMBRE") = Regex.Replace(m.Groups(4).Value.ToString(), "[\s-]*$", "", RegexOptions.Multiline)
                _row("REFERENCIA") = m.Groups(47).Value
                _row("MONEDA") = m.Groups(12).Value
                _row("IMPORTE_MONEDA") = DecimalParse(m.Groups(14).Value)
                _row("BRUTO_MXN") = DecimalParse(m.Groups(36).Value)
                dtPDFToExcel.Rows.Add(_row)
            Next
            dtPDFToExcel.AcceptChanges()

            ' // INICIO DE LA EXPORTACION DEL ARCHIVO EXCEL
            Using objExcelPackage As ExcelPackage = New ExcelPackage()
                Dim objWorksheet As ExcelWorksheet = objExcelPackage.Workbook.Worksheets.Add("HOJA 1")
                objWorksheet.Cells("A1").LoadFromDataTable(dtPDFToExcel, True)
                objWorksheet.Cells.Style.Font.SetFromFont(New Font("Calibri", 12))
                objWorksheet.Cells.AutoFitColumns()
                Using objRange As ExcelRange = objWorksheet.Cells(1, 1, 1, dtPDFToExcel.Columns.Count)
                    objRange.Style.Font.Bold = True
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center
                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid
                    objRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                End Using
                If File.Exists(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx")) Then
                    If MessageBox.Show("El archivo " & Path.GetFileNameWithoutExtension(_filePath) & ".xlsx" & " ya existe en la ruta de guardado ¿Desea sobreescribirlo?", "El archivo que intenta crear ya existe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                        Try
                            File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                        Catch ex As Exception
                            MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return False
                        End Try
                    End If
                Else
                    Try
                        File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                    Catch ex As Exception
                        MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End Try

                End If
                If InvokeRequired Then
                    Invoke(Sub()
                               DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                           End Sub)
                Else
                    DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                End If

            End Using
            ' // FIN DE LA EXPORTACION DEL ARCHIVO EXCEL
            Return True
        End If
        Return False
    End Function

    Private Function ToLiteral(ByVal input As String) As String
        Using writer = New StringWriter()
            Using provider = CodeDomProvider.CreateProvider("CSharp")
                provider.GenerateCodeFromExpression(New CodePrimitiveExpression(input), writer, Nothing)
                Return writer.ToString()
            End Using
        End Using
    End Function

    Private Function ExtraerTextoDesdeLocalizacion(ByVal filePath As String, ByVal page As Integer, ByVal rect As util.RectangleJ) As String
        ' Despues de que ya tengamos el pdf para trabajar
        Dim reader As PdfReader = New PdfReader(filePath)
        Dim filter As parser.RenderFilter() = {New parser.RegionTextRenderFilter(rect)}
        Dim strategy As parser.ITextExtractionStrategy
        Dim sb As StringBuilder = New StringBuilder()
        sb.Clear()
        strategy = New parser.FilteredTextRenderListener(New parser.LocationTextExtractionStrategy(), filter)
        Dim texto_extraido = parser.PdfTextExtractor.GetTextFromPage(reader, page, strategy)
        reader.Close()
        Return texto_extraido
    End Function

    Public Function GenerateStreamFromString(ByVal s As String) As Stream
        Dim stream As MemoryStream = New MemoryStream()
        Dim writer As StreamWriter = New StreamWriter(stream)
        writer.Write(s)
        writer.Flush()
        stream.Position = 0
        Return stream
    End Function

    Public Function StripHTML(ByVal input As String) As String
        Return Regex.Replace(input, "<.*?>", String.Empty)
    End Function

    Private Function ConvertirAMEX2APDF(ByVal _filePath As String, ByVal _saveFolderPath As String, ByVal rowIndex As Integer) As Boolean
        If File.Exists(_filePath) And Path.GetExtension(_filePath).ToLower() = ".pdf" Then
            Dim _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim _programDirectory = Path.Combine(_path, "DataExtractor")
            Dim AnyoFacturacion As Integer = 0

            '// Si no existe el directorio para los datos del programa lo creamos
            If Not Directory.Exists(_programDirectory) Then
                Directory.CreateDirectory(_programDirectory)
            Else
                Dim di As DirectoryInfo = New DirectoryInfo(_programDirectory)

                For Each _file As FileInfo In di.GetFiles()
                    Try
                        _file.Delete()
                    Catch ex As Exception
                    End Try
                Next
            End If
            Dim exePath As String = Path.Combine(_programDirectory, "pdftohtml.exe")

            ' // Escribimos en el directorio de programa el exe que incluimos como recurso
            File.WriteAllBytes(exePath, My.Resources.pdftohtml)
            File.Copy(_filePath, Path.Combine(_programDirectory, Path.GetFileName(_filePath)))


            Dim startInfo As ProcessStartInfo = New ProcessStartInfo()

            ' // Configuramos los parametros para iniciar el proceso que transforma el pdf a html
            startInfo.Arguments = String.Format("-i -noframes ""{0}"" ""{1}.html""", Path.GetFileName(_filePath), Path.GetFileNameWithoutExtension(_filePath))
            startInfo.UseShellExecute = False
            startInfo.WorkingDirectory = _programDirectory
            startInfo.CreateNoWindow = True
            startInfo.RedirectStandardOutput = True
            startInfo.FileName = exePath

            '// Iniciamos el proceso pasandole como parametro cada uno de los archivos pdf que le hemos pasado como argumento.
            Try
                Using exeProcess As Process = Process.Start(startInfo)
                    DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVIRTIENDO..."
                    RaiseEvent ConversionArchivoEnProgreso(Me, EventArgs.Empty)
                    exeProcess.EnableRaisingEvents = True
                    exeProcess.Start()
                    Dim output = exeProcess.StandardOutput.ReadToEnd()
                    exeProcess.WaitForExit()

                    Dim _document As HtmlDocument = New HtmlDocument()
                    Using fs As FileStream = New FileStream(String.Format("{0}\{1}.html", _programDirectory, Path.GetFileNameWithoutExtension(_filePath)), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        Using sr As StreamReader = New StreamReader(fs, Encoding.GetEncoding("ISO-8859-1"))
                            _document.Load(sr)
                        End Using
                    End Using
                    Dim _root = _document.DocumentNode
                    Dim _sb = New StringBuilder()
                    Dim _sb_procesado As String = String.Empty
                    For Each node In _root.DescendantsAndSelf()
                        If Not node.HasChildNodes Then
                            Dim text = node.InnerText
                            If Not String.IsNullOrEmpty(text) Then
                                _sb.Append(text)
                            End If
                        End If
                    Next
                    'Dim _sb_procesado_no_tags = StripHTML(_sb.ToString())

                    _sb_procesado = Regex.Replace(_sb.ToString(), "^\s+$[\r\n]*", "", RegexOptions.Multiline)
                    _sb_procesado = Regex.Replace(_sb_procesado.ToString(), "^\s+", "", RegexOptions.Multiline)

                    Dim lineas() As String = Regex.Split(_sb_procesado, "\r?\n|\r")
                    Dim _sb_limpio As StringBuilder = New StringBuilder()

                    Using _stream As Stream = GenerateStreamFromString(_sb.ToString())
                        Dim _sreader As StreamReader = New StreamReader(_stream, Encoding.UTF8)
                        Dim descartarLineasSiguientes As Boolean = False
                        Do While _sreader.Peek() >= 0
                            Dim linea As String = _sreader.ReadLine()
                            If linea.Contains("Página:") And descartarLineasSiguientes = False Then
                                descartarLineasSiguientes = True
                            End If

                            If Not descartarLineasSiguientes Then 'And Not linea.Contains("Este no es un documento con") Then
                                'If Not linea.StartsWith("Tarjeta ") Then
                                If Not String.IsNullOrEmpty(linea) Then
                                    _sb_limpio.AppendLine(linea)
                                End If
                                'End If
                            End If

                            If linea.Contains("Importe Total") Then
                                descartarLineasSiguientes = False
                            End If
                        Loop
                    End Using

                    'Dim _sb_limpio2 As StringBuilder = New StringBuilder()
                    'Using _stream As Stream = GenerateStreamFromString(_sb_limpio.ToString())
                    '    Dim _sreader As StreamReader = New StreamReader(_stream, Encoding.UTF8)
                    '    Dim descartarLineasSiguientes As Boolean = False
                    '    Do While _sreader.Peek() >= 0
                    '        Dim linea As String = _sreader.ReadLine()
                    '        If linea.Contains("Orden de Servicio") And descartarLineasSiguientes = False Then
                    '            descartarLineasSiguientes = True
                    '        End If

                    '        If Not descartarLineasSiguientes Then 'And Not linea.Contains("Este no es un documento con") Then
                    '            If Not (linea.Contains("Transacción con información adicional en su estado de cuenta") Or linea.Contains("Continúa en la siguiente página")) Then
                    '                If Not String.IsNullOrEmpty(linea) Then
                    '                    _sb_limpio2.AppendLine(linea)
                    '                End If
                    '            End If
                    '        End If

                    '            If Regex.IsMatch(linea, "^\s*$") Then
                    '            descartarLineasSiguientes = False
                    '        End If
                    '    Loop
                    'End Using

                    Dim EnBloque As Boolean = False
                    For i As Integer = 0 To lineas.Count - 1
                        If Regex.IsMatch(lineas(i), "") Then
                            EnBloque = True
                        End If
                    Next

                    File.WriteAllText(Path.Combine(_programDirectory, "procesado.txt"), _sb_limpio.ToString())

                    '    Dim _FechaFacturacionPatron As String = "^([0-3][0-9])\/([0-3][0-9])\/(?:(([0-9][0-9])?[0-9][0-9]))\r$"
                    '    Dim _RegexFechaFacturacion As Regex = New Regex(_FechaFacturacionPatron, RegexOptions.Multiline)
                    '    Dim _MatchesFacturacion As MatchCollection = _RegexFechaFacturacion.Matches(_sb_procesado.ToString())
                    '    Dim _FechaMatch As Match = _MatchesFacturacion.Cast(Of Match)().FirstOrDefault()
                    '    Dim Temp As Date = Nothing
                    '    Dim _FechaFacturacion As Date = Nothing
                    '    Dim formatProvider As DateTimeFormatInfo = New DateTimeFormatInfo()
                    '    formatProvider.Calendar.TwoDigitYearMax = DateTime.Now.Year

                    '    If _FechaMatch IsNot Nothing Then
                    '        Temp = Date.ParseExact(_FechaMatch.Value.Trim(), "MM/dd/yy", formatProvider)
                    '        _FechaFacturacion = Date.ParseExact(Temp.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    '        AnyoFacturacion = _FechaFacturacion.Year
                    '    End If
                    '    Dim lineas() As String = Regex.Split(_sb_procesado, "\r?\n|\r")
                    '    Dim _sb_limpio As StringBuilder = New StringBuilder()
                    '    Dim EnBloque As Boolean = False

                    '    For i As Integer = 0 To lineas.Count - 1
                    '        If lineas(i).StartsWith("MEMO STATEMENT") Or lineas(i).StartsWith("STATEMENT DATE") Then
                    '            EnBloque = True
                    '        End If

                    '        If lineas(i).StartsWith("Amount") Then
                    '            EnBloque = False
                    '        End If

                    '        If EnBloque = False And (lineas(i).StartsWith("Amount") = False Or lineas(i).StartsWith("PAGE")) Then
                    '            _sb_limpio.AppendLine(lineas(i))
                    '        End If
                    '    Next

                    '    Dim _sb_limpio_2 As String = Regex.Replace(_sb_limpio.ToString(), "^IBLA Hotel.*$", "", RegexOptions.Multiline)
                    '    Dim _sb_limpio_3 As String = Regex.Replace(_sb_limpio_2.ToString(), "^PAGE .*$", "", RegexOptions.Multiline)

                    '    Dim _sb_limpio_final As String = Regex.Replace(_sb_limpio_3.ToString(), "^\s+$[\r\n]*", "", RegexOptions.Multiline)
                    '    _sb_limpio_final = Regex.Replace(_sb_limpio_final.ToString(), "^\s+", "", RegexOptions.Multiline)

                    '    Dim dtPDFToExcel As DataTable = New DataTable()
                    '    dtPDFToExcel.Clear()
                    '    Dim IdColumn As DataColumn = New DataColumn("ID")
                    '    IdColumn.DataType = GetType(UInt32)
                    '    IdColumn.AutoIncrement = True
                    '    IdColumn.AutoIncrementSeed = 1
                    '    IdColumn.AutoIncrementStep = 1
                    '    dtPDFToExcel.Columns.Add(IdColumn)
                    '    dtPDFToExcel.Columns.Add("POST_DATE")
                    '    dtPDFToExcel.Columns.Add("TRAN_DATE")
                    '    dtPDFToExcel.Columns.Add("REF_NUMBER")
                    '    dtPDFToExcel.Columns.Add("TRAN_DESC")
                    '    dtPDFToExcel.Columns.Add("MON_NAC", GetType(Decimal))
                    '    dtPDFToExcel.Columns.Add("MON_EXT", GetType(Decimal))
                    '    dtPDFToExcel.Columns.Add("MON_EXT_RATE", GetType(Decimal))

                    '    Dim lineas_sb_limpio() As String = Regex.Split(_sb_limpio_final.ToString(), "\r?\n|\r")

                    '    For i As Integer = 0 To lineas_sb_limpio.Count - 2 Step 6
                    '        Dim post_date_month_day As String() = Regex.Split(lineas_sb_limpio(i), "-")
                    '        Dim post_date As Date = New Date(AnyoFacturacion, Integer.Parse(post_date_month_day(0)), Integer.Parse(post_date_month_day(1)))

                    '        Dim tran_date_month_day As String() = Regex.Split(lineas_sb_limpio(i + 1), "-")
                    '        Dim tran_date As Date = New Date(AnyoFacturacion, Integer.Parse(tran_date_month_day(0)), Integer.Parse(tran_date_month_day(1)))

                    '        Dim ref_number As String = lineas_sb_limpio(i + 2)

                    '        Dim tran_desc As String = lineas_sb_limpio(i + 3)

                    '        Dim importe_mon_ext As Decimal = 0

                    '        Dim pattern As String = "\(FOREIGN CURRENCY\)\s+\$((?<=[^\d,.]|^)\d{1,3}(,(\d{3}))*((?=[,.]\s)|(\.\d+)?(?=[^\d,.]|$))(\-*))\s+[A-Za-z]{3}\s+(\d{2}\/(\d{2}))\s+\(RATE\)\s+((?<=[^\d,.]|^)\d{1,3}(,(\d{3}))*((?=[,.]\s)|(\.\d+)?(?=[^\d,.]|$)))"

                    '        Dim mMatch As Match = Regex.Match(lineas_sb_limpio(i + 5), pattern, RegexOptions.Multiline)

                    '        Dim importe_mon_nac As Decimal = 0

                    '        Dim importe_mon_ext_rate As Decimal = 0

                    '        If mMatch IsNot Nothing Then
                    '            If mMatch.Success Then
                    '                If mMatch.Groups(6).Value = "-" Then
                    '                    importe_mon_ext = -1.0 * Decimal.Parse(Regex.Replace(lineas_sb_limpio(i + 4), "[A-Za-z]", ""))
                    '                Else
                    '                    importe_mon_ext = Decimal.Parse(Regex.Replace(lineas_sb_limpio(i + 4), "[A-Za-z]", ""))
                    '                End If
                    '                importe_mon_nac = Decimal.Parse(mMatch.Groups(1).Value)
                    '                importe_mon_ext_rate = Decimal.Parse(mMatch.Groups(9).Value)
                    '            End If
                    '        End If

                    '        dtPDFToExcel.Rows.Add(Nothing, post_date.ToShortDateString(), tran_date.ToShortDateString(), ref_number, tran_desc, importe_mon_nac, importe_mon_ext, importe_mon_ext_rate)
                    '    Next
                    '    dtPDFToExcel.AcceptChanges()

                    '    ' // INICIO DE LA EXPORTACION DEL ARCHIVO EXCEL
                    '    Using objExcelPackage As ExcelPackage = New ExcelPackage()
                    '        Dim objWorksheet As ExcelWorksheet = objExcelPackage.Workbook.Worksheets.Add("HOJA 1")
                    '        objWorksheet.Cells("A1").LoadFromDataTable(dtPDFToExcel, True)
                    '        objWorksheet.Cells.Style.Font.SetFromFont(New Font("Calibri", 12))
                    '        objWorksheet.Cells.AutoFitColumns()
                    '        Using objRange As ExcelRange = objWorksheet.Cells(1, 1, 1, dtPDFToExcel.Columns.Count)
                    '            objRange.Style.Font.Bold = True
                    '            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center
                    '            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center
                    '            objRange.Style.Fill.PatternType = ExcelFillStyle.Solid
                    '            objRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue)
                    '        End Using
                    '        If File.Exists(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx")) Then
                    '            If MessageBox.Show("El archivo " & Path.GetFileNameWithoutExtension(_filePath) & ".xlsx" & " ya existe en la ruta de guardado ¿Desea sobreescribirlo?", "El archivo que intenta crear ya existe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                    '                Try
                    '                    File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                    '                Catch ex As Exception
                    '                    MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    '                    Return False
                    '                End Try
                    '            End If
                    '        Else
                    '            Try
                    '                File.WriteAllBytes(Path.Combine(_saveFolderPath, Path.GetFileNameWithoutExtension(_filePath) & ".xlsx"), objExcelPackage.GetAsByteArray())
                    '            Catch ex As Exception
                    '                MessageBox.Show("El programa no puede escribir en el archivo, verifique que no se encuentre abierto y vuelva a presionar el botón ""CONVERTIR A EXCEL"".", "ARCHIVO EN USO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    '                Return False
                    '            End Try

                    '        End If

                    '        If InvokeRequired Then
                    '            Invoke(Sub()
                    '                       DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                    '                   End Sub)
                    '        Else
                    '            DataGridView1.Rows(rowIndex).Cells(1).Value = "CONVERTIDO"
                    '        End If
                    '    End Using
                    '    Return True
                End Using
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
                Console.WriteLine(ex.Message)
                Console.WriteLine(ex.StackTrace)
                Return False
            End Try
        End If
        Return False
    End Function


    Private Async Sub OK_Button_Click_1(sender As Object, e As EventArgs) Handles OK_Button.Click
        FolderBrowserDialog1.Reset()
        FolderBrowserDialog1.Description = "Seleccione el directorio donde desea guardar los archivos Excel"
        FolderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        FolderBrowserDialog1.ShowNewFolderButton = True

        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            If FolderBrowserDialog1.SelectedPath <> "" Then
                For i As Integer = 0 To OpenFileDialog1.FileNames.Length - 1
                    Dim currentDocumentIndex As Integer = i
                    Dim texto_primera_pagina As StringBuilder = New StringBuilder()
                    Dim pdfReader As PdfReader = New PdfReader(OpenFileDialog1.FileNames(i))
                    Dim strategy As parser.ITextExtractionStrategy = New parser.SimpleTextExtractionStrategy()
                    Dim currentText As String = parser.PdfTextExtractor.GetTextFromPage(pdfReader, 1, strategy)
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)))
                    pdfReader.Close()
                    If currentText.ToString().IndexOf("airplus") <> -1 Then
                        picConvirtiendo.Enabled = True
                        picConvirtiendo.Visible = True
                        lblConvirtiendo.Enabled = True
                        lblConvirtiendo.Visible = True

                        Dim resultado_conversion_pdf As Boolean = Await Task.Factory.StartNew(Function() ConvertirSantanderAExcel(OpenFileDialog1.FileNames(currentDocumentIndex), FolderBrowserDialog1.SelectedPath, currentDocumentIndex))

                        picConvirtiendo.Enabled = False
                        picConvirtiendo.Visible = False
                        lblConvirtiendo.Enabled = False
                        lblConvirtiendo.Visible = False

                        If resultado_conversion_pdf Then
                            TextBox1.AppendText("El archivo " & OpenFileDialog1.FileNames(i) & " ha sido convertido satisfactoriamente" & vbCrLf)
                        End If
                    End If

                    If (currentText.ToString().IndexOf("americanexpress") <> -1) Or (currentText.ToString().IndexOf("Central Delivery Cover Page") <> -1) Then

                        picConvirtiendo.Enabled = True
                        picConvirtiendo.Visible = True
                        lblConvirtiendo.Enabled = True
                        lblConvirtiendo.Visible = True

                        Dim resultado_conversion_pdf As Boolean = Await Task.Factory.StartNew(Function() ConvertirAmexAPDF(OpenFileDialog1.FileNames(currentDocumentIndex), FolderBrowserDialog1.SelectedPath, currentDocumentIndex))

                        picConvirtiendo.Enabled = False
                        picConvirtiendo.Visible = False
                        lblConvirtiendo.Enabled = False
                        lblConvirtiendo.Visible = False

                        If resultado_conversion_pdf Then
                            TextBox1.AppendText("El archivo " & OpenFileDialog1.FileNames(i) & " ha sido convertido satisfactoriamente" & vbCrLf)
                        End If
                    End If

                    If currentText.ToString().IndexOf("HOTEL IBO") <> -1 Then

                        picConvirtiendo.Enabled = True
                        picConvirtiendo.Visible = True
                        lblConvirtiendo.Enabled = True
                        lblConvirtiendo.Visible = True

                        Dim resultado_conversion_pdf As Boolean = Await Task.Factory.StartNew(Function() ConvertirIBLAPDF(OpenFileDialog1.FileNames(currentDocumentIndex), FolderBrowserDialog1.SelectedPath, currentDocumentIndex))

                        picConvirtiendo.Enabled = False
                        picConvirtiendo.Visible = False
                        lblConvirtiendo.Enabled = False
                        lblConvirtiendo.Visible = False

                        If resultado_conversion_pdf Then
                            TextBox1.AppendText("El archivo " & OpenFileDialog1.FileNames(i) & " ha sido convertido satisfactoriamente" & vbCrLf)
                        End If
                    End If

                    If currentText.ToString().IndexOf("Estado de Cuenta de EBTA") <> -1 Then

                        picConvirtiendo.Enabled = True
                        picConvirtiendo.Visible = True
                        lblConvirtiendo.Enabled = True
                        lblConvirtiendo.Visible = True

                        Dim resultado_conversion_pdf As Boolean = Await Task.Factory.StartNew(Function() ConvertirAMEX2APDF(OpenFileDialog1.FileNames(currentDocumentIndex), FolderBrowserDialog1.SelectedPath, currentDocumentIndex))

                        picConvirtiendo.Enabled = False
                        picConvirtiendo.Visible = False
                        lblConvirtiendo.Enabled = False
                        lblConvirtiendo.Visible = False

                        If resultado_conversion_pdf Then
                            TextBox1.AppendText("El archivo " & OpenFileDialog1.FileNames(i) & " ha sido convertido satisfactoriamente" & vbCrLf)
                        End If
                    End If
                Next
            End If
        End If
    End Sub
End Class
