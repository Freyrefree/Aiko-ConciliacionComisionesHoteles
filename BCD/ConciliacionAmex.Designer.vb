<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ConciliacionAmex
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConciliacionAmex))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_hojaexcel = New System.Windows.Forms.Label()
        Me.cmb_hoja_edo = New System.Windows.Forms.ComboBox()
        Me.btn_edo_cuenta = New System.Windows.Forms.Button()
        Me.txt_excel_edo = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmb_hoja_icaav = New System.Windows.Forms.ComboBox()
        Me.btn_icaav = New System.Windows.Forms.Button()
        Me.txt_excel_icaav = New System.Windows.Forms.TextBox()
        Me.TabNavegacion = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GridEdoCuenta = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GridIcaav = New System.Windows.Forms.DataGridView()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnAgregarNuevaConciliaciion = New System.Windows.Forms.Button()
        Me.lbxConciliacionesDeGrupo = New System.Windows.Forms.ListBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.btnReiniciarGrupo = New System.Windows.Forms.Button()
        Me.btnEliminarGrupo = New System.Windows.Forms.Button()
        Me.btnEditarGrupo = New System.Windows.Forms.Button()
        Me.btnAgregarNuevoGrupo = New System.Windows.Forms.Button()
        Me.lbxGruposConciliacion = New System.Windows.Forms.ListBox()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.chkSelectAllConc2 = New System.Windows.Forms.CheckBox()
        Me.CheckedListBox4 = New System.Windows.Forms.CheckedListBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.chkSelectAllConc1 = New System.Windows.Forms.CheckBox()
        Me.CheckedListBox3 = New System.Windows.Forms.CheckedListBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.cmsDataConverter = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuNuevaConciliacion = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuSalir = New System.Windows.Forms.ToolStripMenuItem()
        Me.UtilidadesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuAmexToPDF = New System.Windows.Forms.ToolStripMenuItem()
        Me.ACERCADEToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuAcercaDe = New System.Windows.Forms.ToolStripMenuItem()
        Me.btn_exportar = New System.Windows.Forms.Button()
        Me.btn_procesar = New System.Windows.Forms.Button()
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
        Me.EventLog1 = New System.Diagnostics.EventLog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabNavegacion.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.GridEdoCuenta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.GridIcaav, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage8.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.cmsDataConverter.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txt_hojaexcel)
        Me.GroupBox1.Controls.Add(Me.cmb_hoja_edo)
        Me.GroupBox1.Controls.Add(Me.btn_edo_cuenta)
        Me.GroupBox1.Controls.Add(Me.txt_excel_edo)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(16, 38)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(525, 153)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "REPORTE BCD"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(210, 16)
        Me.Label2.TabIndex = 62
        Me.Label2.Text = "SELECCIONAR ARCHIVO DE EXCEL:"
        '
        'txt_hojaexcel
        '
        Me.txt_hojaexcel.AutoSize = True
        Me.txt_hojaexcel.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_hojaexcel.Location = New System.Drawing.Point(6, 83)
        Me.txt_hojaexcel.Name = "txt_hojaexcel"
        Me.txt_hojaexcel.Size = New System.Drawing.Size(204, 16)
        Me.txt_hojaexcel.TabIndex = 61
        Me.txt_hojaexcel.Text = "SELECCIONAR HOJA DE TRABAJO:"
        '
        'cmb_hoja_edo
        '
        Me.cmb_hoja_edo.Enabled = False
        Me.cmb_hoja_edo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmb_hoja_edo.FormattingEnabled = True
        Me.cmb_hoja_edo.Location = New System.Drawing.Point(9, 100)
        Me.cmb_hoja_edo.Name = "cmb_hoja_edo"
        Me.cmb_hoja_edo.Size = New System.Drawing.Size(399, 24)
        Me.cmb_hoja_edo.TabIndex = 60
        '
        'btn_edo_cuenta
        '
        Me.btn_edo_cuenta.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_edo_cuenta.Image = Global.Conciliacion.My.Resources.Resources.Open_file_icon
        Me.btn_edo_cuenta.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_edo_cuenta.Location = New System.Drawing.Point(414, 56)
        Me.btn_edo_cuenta.Name = "btn_edo_cuenta"
        Me.btn_edo_cuenta.Size = New System.Drawing.Size(84, 67)
        Me.btn_edo_cuenta.TabIndex = 59
        Me.btn_edo_cuenta.Text = "EXAMINAR"
        Me.btn_edo_cuenta.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn_edo_cuenta.UseVisualStyleBackColor = True
        '
        'txt_excel_edo
        '
        Me.txt_excel_edo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_excel_edo.Location = New System.Drawing.Point(9, 56)
        Me.txt_excel_edo.Name = "txt_excel_edo"
        Me.txt_excel_edo.ReadOnly = True
        Me.txt_excel_edo.Size = New System.Drawing.Size(399, 22)
        Me.txt_excel_edo.TabIndex = 58
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.cmb_hoja_icaav)
        Me.GroupBox2.Controls.Add(Me.btn_icaav)
        Me.GroupBox2.Controls.Add(Me.txt_excel_icaav)
        Me.GroupBox2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(547, 38)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(525, 153)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "REPORTE ESTADO DE CUENTA"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(210, 16)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "SELECCIONAR ARCHIVO DE EXCEL:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(204, 16)
        Me.Label3.TabIndex = 61
        Me.Label3.Text = "SELECCIONAR HOJA DE TRABAJO:"
        '
        'cmb_hoja_icaav
        '
        Me.cmb_hoja_icaav.Enabled = False
        Me.cmb_hoja_icaav.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmb_hoja_icaav.FormattingEnabled = True
        Me.cmb_hoja_icaav.Location = New System.Drawing.Point(9, 100)
        Me.cmb_hoja_icaav.Name = "cmb_hoja_icaav"
        Me.cmb_hoja_icaav.Size = New System.Drawing.Size(399, 24)
        Me.cmb_hoja_icaav.TabIndex = 60
        '
        'btn_icaav
        '
        Me.btn_icaav.Enabled = False
        Me.btn_icaav.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_icaav.Image = Global.Conciliacion.My.Resources.Resources.Open_file_icon
        Me.btn_icaav.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_icaav.Location = New System.Drawing.Point(414, 56)
        Me.btn_icaav.Name = "btn_icaav"
        Me.btn_icaav.Size = New System.Drawing.Size(84, 67)
        Me.btn_icaav.TabIndex = 59
        Me.btn_icaav.Text = "EXAMINAR"
        Me.btn_icaav.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn_icaav.UseVisualStyleBackColor = True
        '
        'txt_excel_icaav
        '
        Me.txt_excel_icaav.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_excel_icaav.Location = New System.Drawing.Point(9, 56)
        Me.txt_excel_icaav.Name = "txt_excel_icaav"
        Me.txt_excel_icaav.ReadOnly = True
        Me.txt_excel_icaav.Size = New System.Drawing.Size(399, 22)
        Me.txt_excel_icaav.TabIndex = 58
        '
        'TabNavegacion
        '
        Me.TabNavegacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabNavegacion.Controls.Add(Me.TabPage1)
        Me.TabNavegacion.Controls.Add(Me.TabPage2)
        Me.TabNavegacion.Controls.Add(Me.TabPage8)
        Me.TabNavegacion.Controls.Add(Me.TabPage6)
        Me.TabNavegacion.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabNavegacion.Location = New System.Drawing.Point(16, 207)
        Me.TabNavegacion.Name = "TabNavegacion"
        Me.TabNavegacion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TabNavegacion.SelectedIndex = 0
        Me.TabNavegacion.Size = New System.Drawing.Size(1190, 364)
        Me.TabNavegacion.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.Transparent
        Me.TabPage1.Controls.Add(Me.GridEdoCuenta)
        Me.TabPage1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1182, 337)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "REPORT.BCD"
        '
        'GridEdoCuenta
        '
        Me.GridEdoCuenta.AllowUserToAddRows = False
        Me.GridEdoCuenta.AllowUserToDeleteRows = False
        Me.GridEdoCuenta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GridEdoCuenta.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader
        Me.GridEdoCuenta.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.GridEdoCuenta.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GridEdoCuenta.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.GridEdoCuenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridEdoCuenta.Location = New System.Drawing.Point(0, 0)
        Me.GridEdoCuenta.Name = "GridEdoCuenta"
        Me.GridEdoCuenta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridEdoCuenta.Size = New System.Drawing.Size(1182, 331)
        Me.GridEdoCuenta.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.GridIcaav)
        Me.TabPage2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1182, 337)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "REPORT.EDO CUENTA"
        '
        'GridIcaav
        '
        Me.GridIcaav.AllowUserToAddRows = False
        Me.GridIcaav.AllowUserToDeleteRows = False
        Me.GridIcaav.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GridIcaav.BackgroundColor = System.Drawing.SystemColors.ControlLightLight
        Me.GridIcaav.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GridIcaav.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GridIcaav.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GridIcaav.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridIcaav.Location = New System.Drawing.Point(0, 0)
        Me.GridIcaav.Name = "GridIcaav"
        Me.GridIcaav.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridIcaav.Size = New System.Drawing.Size(1182, 303)
        Me.GridIcaav.TabIndex = 0
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.GroupBox9)
        Me.TabPage8.Controls.Add(Me.GroupBox7)
        Me.TabPage8.Location = New System.Drawing.Point(4, 23)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8.Size = New System.Drawing.Size(1182, 337)
        Me.TabPage8.TabIndex = 13
        Me.TabPage8.Text = "CONCIL. Y COND."
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.Button3)
        Me.GroupBox9.Controls.Add(Me.Button2)
        Me.GroupBox9.Controls.Add(Me.btnAgregarNuevaConciliaciion)
        Me.GroupBox9.Controls.Add(Me.lbxConciliacionesDeGrupo)
        Me.GroupBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox9.Location = New System.Drawing.Point(609, 13)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(567, 384)
        Me.GroupBox9.TabIndex = 1
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "CONDICIONES"
        '
        'Button3
        '
        Me.Button3.Enabled = False
        Me.Button3.Image = Global.Conciliacion.My.Resources.Resources.delete
        Me.Button3.Location = New System.Drawing.Point(81, 28)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(32, 25)
        Me.Button3.TabIndex = 3
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Image = Global.Conciliacion.My.Resources.Resources.edit
        Me.Button2.Location = New System.Drawing.Point(43, 28)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(32, 25)
        Me.Button2.TabIndex = 3
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnAgregarNuevaConciliaciion
        '
        Me.btnAgregarNuevaConciliaciion.Enabled = False
        Me.btnAgregarNuevaConciliaciion.Image = Global.Conciliacion.My.Resources.Resources.add1__1_
        Me.btnAgregarNuevaConciliaciion.Location = New System.Drawing.Point(6, 28)
        Me.btnAgregarNuevaConciliaciion.Name = "btnAgregarNuevaConciliaciion"
        Me.btnAgregarNuevaConciliaciion.Size = New System.Drawing.Size(32, 25)
        Me.btnAgregarNuevaConciliaciion.TabIndex = 3
        Me.btnAgregarNuevaConciliaciion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAgregarNuevaConciliaciion.UseVisualStyleBackColor = True
        '
        'lbxConciliacionesDeGrupo
        '
        Me.lbxConciliacionesDeGrupo.FormattingEnabled = True
        Me.lbxConciliacionesDeGrupo.ItemHeight = 15
        Me.lbxConciliacionesDeGrupo.Location = New System.Drawing.Point(6, 59)
        Me.lbxConciliacionesDeGrupo.Name = "lbxConciliacionesDeGrupo"
        Me.lbxConciliacionesDeGrupo.Size = New System.Drawing.Size(555, 319)
        Me.lbxConciliacionesDeGrupo.TabIndex = 1
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.btnReiniciarGrupo)
        Me.GroupBox7.Controls.Add(Me.btnEliminarGrupo)
        Me.GroupBox7.Controls.Add(Me.btnEditarGrupo)
        Me.GroupBox7.Controls.Add(Me.btnAgregarNuevoGrupo)
        Me.GroupBox7.Controls.Add(Me.lbxGruposConciliacion)
        Me.GroupBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox7.Location = New System.Drawing.Point(20, 13)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(548, 384)
        Me.GroupBox7.TabIndex = 0
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "CONCILIACIONES"
        '
        'btnReiniciarGrupo
        '
        Me.btnReiniciarGrupo.Enabled = False
        Me.btnReiniciarGrupo.Image = Global.Conciliacion.My.Resources.Resources.if_refresh_49554
        Me.btnReiniciarGrupo.Location = New System.Drawing.Point(82, 23)
        Me.btnReiniciarGrupo.Name = "btnReiniciarGrupo"
        Me.btnReiniciarGrupo.Size = New System.Drawing.Size(32, 25)
        Me.btnReiniciarGrupo.TabIndex = 3
        Me.btnReiniciarGrupo.UseVisualStyleBackColor = True
        '
        'btnEliminarGrupo
        '
        Me.btnEliminarGrupo.Enabled = False
        Me.btnEliminarGrupo.Image = Global.Conciliacion.My.Resources.Resources.delete
        Me.btnEliminarGrupo.Location = New System.Drawing.Point(120, 23)
        Me.btnEliminarGrupo.Name = "btnEliminarGrupo"
        Me.btnEliminarGrupo.Size = New System.Drawing.Size(32, 25)
        Me.btnEliminarGrupo.TabIndex = 2
        Me.btnEliminarGrupo.UseVisualStyleBackColor = True
        '
        'btnEditarGrupo
        '
        Me.btnEditarGrupo.Enabled = False
        Me.btnEditarGrupo.Image = Global.Conciliacion.My.Resources.Resources.edit
        Me.btnEditarGrupo.Location = New System.Drawing.Point(44, 23)
        Me.btnEditarGrupo.Name = "btnEditarGrupo"
        Me.btnEditarGrupo.Size = New System.Drawing.Size(32, 25)
        Me.btnEditarGrupo.TabIndex = 2
        Me.btnEditarGrupo.UseVisualStyleBackColor = True
        '
        'btnAgregarNuevoGrupo
        '
        Me.btnAgregarNuevoGrupo.Enabled = False
        Me.btnAgregarNuevoGrupo.Image = Global.Conciliacion.My.Resources.Resources.link__1_
        Me.btnAgregarNuevoGrupo.Location = New System.Drawing.Point(6, 23)
        Me.btnAgregarNuevoGrupo.Name = "btnAgregarNuevoGrupo"
        Me.btnAgregarNuevoGrupo.Size = New System.Drawing.Size(32, 25)
        Me.btnAgregarNuevoGrupo.TabIndex = 1
        Me.btnAgregarNuevoGrupo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAgregarNuevoGrupo.UseVisualStyleBackColor = True
        '
        'lbxGruposConciliacion
        '
        Me.lbxGruposConciliacion.FormattingEnabled = True
        Me.lbxGruposConciliacion.ItemHeight = 15
        Me.lbxGruposConciliacion.Location = New System.Drawing.Point(6, 59)
        Me.lbxGruposConciliacion.Name = "lbxGruposConciliacion"
        Me.lbxGruposConciliacion.Size = New System.Drawing.Size(536, 319)
        Me.lbxGruposConciliacion.TabIndex = 0
        '
        'TabPage6
        '
        Me.TabPage6.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.TabPage6.Controls.Add(Me.Label6)
        Me.TabPage6.Controls.Add(Me.GroupBox5)
        Me.TabPage6.Controls.Add(Me.GroupBox6)
        Me.TabPage6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage6.Location = New System.Drawing.Point(4, 23)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(1182, 337)
        Me.TabPage6.TabIndex = 6
        Me.TabPage6.Text = "CONF.REP.EXCEL"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Label6.Location = New System.Drawing.Point(395, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label6.Size = New System.Drawing.Size(553, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "SELECCIONE LAS COLUMNAS QUE APARECERÁN EN EL REPORTE DE CONCILIACIÓN"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.chkSelectAllConc2)
        Me.GroupBox5.Controls.Add(Me.CheckedListBox4)
        Me.GroupBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(592, 60)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(580, 268)
        Me.GroupBox5.TabIndex = 3
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "COLUMNAS REPORTE ESTADO DE CUENTA"
        '
        'chkSelectAllConc2
        '
        Me.chkSelectAllConc2.AutoSize = True
        Me.chkSelectAllConc2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectAllConc2.Location = New System.Drawing.Point(18, 26)
        Me.chkSelectAllConc2.Name = "chkSelectAllConc2"
        Me.chkSelectAllConc2.Size = New System.Drawing.Size(249, 19)
        Me.chkSelectAllConc2.TabIndex = 2
        Me.chkSelectAllConc2.Text = "SELECCIONAR TODAS LAS COLUMNAS"
        Me.chkSelectAllConc2.UseVisualStyleBackColor = True
        '
        'CheckedListBox4
        '
        Me.CheckedListBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckedListBox4.CheckOnClick = True
        Me.CheckedListBox4.ColumnWidth = 180
        Me.CheckedListBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckedListBox4.FormattingEnabled = True
        Me.CheckedListBox4.HorizontalScrollbar = True
        Me.CheckedListBox4.Location = New System.Drawing.Point(9, 51)
        Me.CheckedListBox4.MultiColumn = True
        Me.CheckedListBox4.Name = "CheckedListBox4"
        Me.CheckedListBox4.Size = New System.Drawing.Size(565, 196)
        Me.CheckedListBox4.TabIndex = 1
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.chkSelectAllConc1)
        Me.GroupBox6.Controls.Add(Me.CheckedListBox3)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(6, 60)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(580, 268)
        Me.GroupBox6.TabIndex = 2
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "COLUMNAS REPORTE BCD"
        '
        'chkSelectAllConc1
        '
        Me.chkSelectAllConc1.AutoSize = True
        Me.chkSelectAllConc1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectAllConc1.Location = New System.Drawing.Point(18, 26)
        Me.chkSelectAllConc1.Name = "chkSelectAllConc1"
        Me.chkSelectAllConc1.Size = New System.Drawing.Size(249, 19)
        Me.chkSelectAllConc1.TabIndex = 1
        Me.chkSelectAllConc1.Text = "SELECCIONAR TODAS LAS COLUMNAS"
        Me.chkSelectAllConc1.UseVisualStyleBackColor = True
        '
        'CheckedListBox3
        '
        Me.CheckedListBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckedListBox3.CheckOnClick = True
        Me.CheckedListBox3.ColumnWidth = 180
        Me.CheckedListBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckedListBox3.FormattingEnabled = True
        Me.CheckedListBox3.HorizontalScrollbar = True
        Me.CheckedListBox3.Location = New System.Drawing.Point(6, 51)
        Me.CheckedListBox3.MultiColumn = True
        Me.CheckedListBox3.Name = "CheckedListBox3"
        Me.CheckedListBox3.Size = New System.Drawing.Size(557, 196)
        Me.CheckedListBox3.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 574)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1214, 22)
        Me.StatusStrip1.TabIndex = 75
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(38, 17)
        Me.ToolStripStatusLabel1.Text = "LISTO"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.MarqueeAnimationSpeed = 30
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        Me.ToolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ToolStripProgressBar1.Visible = False
        '
        'cmsDataConverter
        '
        Me.cmsDataConverter.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox1})
        Me.cmsDataConverter.Name = "ContextMenuStrip1"
        Me.cmsDataConverter.Size = New System.Drawing.Size(182, 31)
        '
        'ToolStripComboBox1
        '
        Me.ToolStripComboBox1.Items.AddRange(New Object() {"Item1", "Item2", "Item3"})
        Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
        Me.ToolStripComboBox1.Size = New System.Drawing.Size(121, 23)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.UtilidadesToolStripMenuItem, Me.ACERCADEToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1214, 24)
        Me.MenuStrip1.TabIndex = 76
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuNuevaConciliacion, Me.menuSalir})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(70, 20)
        Me.ArchivoToolStripMenuItem.Text = "ARCHIVO"
        '
        'menuNuevaConciliacion
        '
        Me.menuNuevaConciliacion.Name = "menuNuevaConciliacion"
        Me.menuNuevaConciliacion.Size = New System.Drawing.Size(198, 22)
        Me.menuNuevaConciliacion.Text = "NUEVA CONCILIACIÓN"
        '
        'menuSalir
        '
        Me.menuSalir.Name = "menuSalir"
        Me.menuSalir.Size = New System.Drawing.Size(198, 22)
        Me.menuSalir.Text = "SALIR"
        '
        'UtilidadesToolStripMenuItem
        '
        Me.UtilidadesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuAmexToPDF})
        Me.UtilidadesToolStripMenuItem.Name = "UtilidadesToolStripMenuItem"
        Me.UtilidadesToolStripMenuItem.Size = New System.Drawing.Size(82, 20)
        Me.UtilidadesToolStripMenuItem.Text = "UTILIDADES"
        '
        'menuAmexToPDF
        '
        Me.menuAmexToPDF.Name = "menuAmexToPDF"
        Me.menuAmexToPDF.Size = New System.Drawing.Size(255, 22)
        Me.menuAmexToPDF.Text = "AMEX-SANTANDER PDF -> EXCEL"
        '
        'ACERCADEToolStripMenuItem
        '
        Me.ACERCADEToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuAcercaDe})
        Me.ACERCADEToolStripMenuItem.Name = "ACERCADEToolStripMenuItem"
        Me.ACERCADEToolStripMenuItem.Size = New System.Drawing.Size(58, 20)
        Me.ACERCADEToolStripMenuItem.Text = "AYUDA"
        '
        'menuAcercaDe
        '
        Me.menuAcercaDe.Name = "menuAcercaDe"
        Me.menuAcercaDe.Size = New System.Drawing.Size(145, 22)
        Me.menuAcercaDe.Text = "ACERCA DE..."
        '
        'btn_exportar
        '
        Me.btn_exportar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_exportar.Enabled = False
        Me.btn_exportar.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_exportar.Image = Global.Conciliacion.My.Resources.Resources.export_excel
        Me.btn_exportar.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_exportar.Location = New System.Drawing.Point(1078, 123)
        Me.btn_exportar.Name = "btn_exportar"
        Me.btn_exportar.Size = New System.Drawing.Size(124, 58)
        Me.btn_exportar.TabIndex = 73
        Me.btn_exportar.Text = "EXPORTAR"
        Me.btn_exportar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn_exportar.UseVisualStyleBackColor = True
        '
        'btn_procesar
        '
        Me.btn_procesar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_procesar.Enabled = False
        Me.btn_procesar.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_procesar.Image = Global.Conciliacion.My.Resources.Resources.settings_3_icon
        Me.btn_procesar.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_procesar.Location = New System.Drawing.Point(1078, 56)
        Me.btn_procesar.Name = "btn_procesar"
        Me.btn_procesar.Size = New System.Drawing.Size(124, 61)
        Me.btn_procesar.TabIndex = 72
        Me.btn_procesar.Text = "PROCESAR"
        Me.btn_procesar.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn_procesar.UseVisualStyleBackColor = True
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.FileName = "OpenFileDialog2"
        '
        'EventLog1
        '
        Me.EventLog1.SynchronizingObject = Me
        '
        'ConciliacionAmex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1214, 596)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.btn_exportar)
        Me.Controls.Add(Me.btn_procesar)
        Me.Controls.Add(Me.TabNavegacion)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "ConciliacionAmex"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CONCILIACIÓN GENERAL"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabNavegacion.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.GridEdoCuenta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.GridIcaav, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage8.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.cmsDataConverter.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txt_hojaexcel As Label
    Friend WithEvents cmb_hoja_edo As ComboBox
    Friend WithEvents btn_edo_cuenta As Button
    Friend WithEvents txt_excel_edo As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cmb_hoja_icaav As ComboBox
    Friend WithEvents btn_icaav As Button
    Friend WithEvents txt_excel_icaav As TextBox
    Friend WithEvents TabNavegacion As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents GridEdoCuenta As DataGridView
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents GridIcaav As DataGridView
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents btn_exportar As Button
    Friend WithEvents btn_procesar As Button
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents CheckedListBox4 As CheckedListBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents CheckedListBox3 As CheckedListBox
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents Label6 As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents TabPage8 As TabPage
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents btnEliminarGrupo As Button
    Friend WithEvents btnAgregarNuevoGrupo As Button
    Friend WithEvents lbxGruposConciliacion As ListBox
    Friend WithEvents GroupBox9 As GroupBox
    Friend WithEvents lbxConciliacionesDeGrupo As ListBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents btnAgregarNuevaConciliaciion As Button
    Friend WithEvents btnEditarGrupo As Button
    Friend WithEvents cmsDataConverter As ContextMenuStrip
    Friend WithEvents ToolStripComboBox1 As ToolStripComboBox
    Friend WithEvents chkSelectAllConc2 As CheckBox
    Friend WithEvents chkSelectAllConc1 As CheckBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ArchivoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UtilidadesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menuNuevaConciliacion As ToolStripMenuItem
    Friend WithEvents menuSalir As ToolStripMenuItem
    Friend WithEvents menuAmexToPDF As ToolStripMenuItem
    Friend WithEvents ACERCADEToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menuAcercaDe As ToolStripMenuItem
    Friend WithEvents btnReiniciarGrupo As Button
    Friend WithEvents OpenFileDialog2 As OpenFileDialog
    Friend WithEvents EventLog1 As EventLog
End Class
