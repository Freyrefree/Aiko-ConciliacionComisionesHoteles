<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AmexToExcel
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ColumnaArchivo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnaStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.picConvirtiendo = New System.Windows.Forms.PictureBox()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.lblConvirtiendo = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picConvirtiendo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.DataGridView1)
        Me.GroupBox1.Controls.Add(Me.OK_Button)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(13, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(631, 247)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "AMEX/SANTANDER PDF -> EXCEL "
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnaArchivo, Me.ColumnaStatus})
        Me.DataGridView1.Location = New System.Drawing.Point(6, 88)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(604, 158)
        Me.DataGridView1.TabIndex = 3
        '
        'ColumnaArchivo
        '
        Me.ColumnaArchivo.HeaderText = "ARCHIVO SELECCIONADO"
        Me.ColumnaArchivo.Name = "ColumnaArchivo"
        Me.ColumnaArchivo.ReadOnly = True
        '
        'ColumnaStatus
        '
        Me.ColumnaStatus.HeaderText = "STATUS"
        Me.ColumnaStatus.Name = "ColumnaStatus"
        Me.ColumnaStatus.ReadOnly = True
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK_Button.Image = Global.Conciliacion.My.Resources.Resources.Excel_icon
        Me.OK_Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.OK_Button.Location = New System.Drawing.Point(378, 36)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(233, 46)
        Me.OK_Button.TabIndex = 2
        Me.OK_Button.Text = "CONVERTIR A EXCEL"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Image = Global.Conciliacion.My.Resources.Resources.pdf_icon__1_
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(6, 36)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(337, 46)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "SELECCIONAR REPORTES AMEX/SANTANDER PDF"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Multiselect = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(18, 265)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(605, 91)
        Me.TextBox1.TabIndex = 3
        '
        'picConvirtiendo
        '
        Me.picConvirtiendo.Enabled = False
        Me.picConvirtiendo.Image = Global.Conciliacion.My.Resources.Resources._5
        Me.picConvirtiendo.Location = New System.Drawing.Point(19, 365)
        Me.picConvirtiendo.Name = "picConvirtiendo"
        Me.picConvirtiendo.Size = New System.Drawing.Size(32, 32)
        Me.picConvirtiendo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picConvirtiendo.TabIndex = 4
        Me.picConvirtiendo.TabStop = False
        Me.picConvirtiendo.Visible = False
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Image = Global.Conciliacion.My.Resources.Resources.Log_Out_icon
        Me.Cancel_Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Cancel_Button.Location = New System.Drawing.Point(428, 365)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(197, 34)
        Me.Cancel_Button.TabIndex = 2
        Me.Cancel_Button.Text = "SALIR"
        '
        'lblConvirtiendo
        '
        Me.lblConvirtiendo.AutoSize = True
        Me.lblConvirtiendo.Enabled = False
        Me.lblConvirtiendo.Location = New System.Drawing.Point(57, 376)
        Me.lblConvirtiendo.Name = "lblConvirtiendo"
        Me.lblConvirtiendo.Size = New System.Drawing.Size(122, 13)
        Me.lblConvirtiendo.TabIndex = 5
        Me.lblConvirtiendo.Text = "GENERANDO EXCEL..."
        Me.lblConvirtiendo.Visible = False
        '
        'AmexToExcel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 411)
        Me.Controls.Add(Me.lblConvirtiendo)
        Me.Controls.Add(Me.picConvirtiendo)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AmexToExcel"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CONVERTIR REPORTE AMEX-SANTANDER A EXCEL"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picConvirtiendo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents OK_Button As Button
    Friend WithEvents Cancel_Button As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ColumnaArchivo As DataGridViewTextBoxColumn
    Friend WithEvents ColumnaStatus As DataGridViewTextBoxColumn
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents picConvirtiendo As PictureBox
    Friend WithEvents lblConvirtiendo As Label
End Class
