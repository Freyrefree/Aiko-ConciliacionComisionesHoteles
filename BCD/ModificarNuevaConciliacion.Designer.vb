<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ModificarNuevaConciliacion
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lbxColumnaReporte2 = New System.Windows.Forms.ListBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lbxColumnasReporte1 = New System.Windows.Forms.ListBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbGrupoNuevaConciliacion = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbTipoOperacionNuevaConciliacion = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbTipoDatosNuevaConciliacion = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancelarNuevaConciliacion = New System.Windows.Forms.Button()
        Me.btnAgregarNuevaConciliacion = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(787, 81)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.01555!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.21775!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.7667!))
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox4, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox3, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 265.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1093, 265)
        Me.TableLayoutPanel2.TabIndex = 7
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lbxColumnaReporte2)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(715, 3)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(375, 259)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "COLUMNAS SEGUNDO REPORTE"
        '
        'lbxColumnaReporte2
        '
        Me.lbxColumnaReporte2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbxColumnaReporte2.FormattingEnabled = True
        Me.lbxColumnaReporte2.ItemHeight = 16
        Me.lbxColumnaReporte2.Location = New System.Drawing.Point(3, 18)
        Me.lbxColumnaReporte2.Name = "lbxColumnaReporte2"
        Me.lbxColumnaReporte2.Size = New System.Drawing.Size(369, 238)
        Me.lbxColumnaReporte2.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lbxColumnasReporte1)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(332, 259)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "COLUMNAS PRIMER REPORTE"
        '
        'lbxColumnasReporte1
        '
        Me.lbxColumnasReporte1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbxColumnasReporte1.FormattingEnabled = True
        Me.lbxColumnasReporte1.ItemHeight = 16
        Me.lbxColumnasReporte1.Location = New System.Drawing.Point(3, 18)
        Me.lbxColumnasReporte1.Name = "lbxColumnasReporte1"
        Me.lbxColumnasReporte1.Size = New System.Drawing.Size(326, 238)
        Me.lbxColumnasReporte1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancelarNuevaConciliacion)
        Me.Panel1.Controls.Add(Me.btnAgregarNuevaConciliacion)
        Me.Panel1.Controls.Add(Me.cmbGrupoNuevaConciliacion)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbTipoOperacionNuevaConciliacion)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cmbTipoDatosNuevaConciliacion)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(341, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(368, 259)
        Me.Panel1.TabIndex = 2
        '
        'cmbGrupoNuevaConciliacion
        '
        Me.cmbGrupoNuevaConciliacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbGrupoNuevaConciliacion.FormattingEnabled = True
        Me.cmbGrupoNuevaConciliacion.Location = New System.Drawing.Point(31, 165)
        Me.cmbGrupoNuevaConciliacion.Name = "cmbGrupoNuevaConciliacion"
        Me.cmbGrupoNuevaConciliacion.Size = New System.Drawing.Size(324, 24)
        Me.cmbGrupoNuevaConciliacion.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(28, 149)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(131, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "AGREGAR A GRUPO:"
        '
        'cmbTipoOperacionNuevaConciliacion
        '
        Me.cmbTipoOperacionNuevaConciliacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTipoOperacionNuevaConciliacion.FormattingEnabled = True
        Me.cmbTipoOperacionNuevaConciliacion.Location = New System.Drawing.Point(31, 116)
        Me.cmbTipoOperacionNuevaConciliacion.Name = "cmbTipoOperacionNuevaConciliacion"
        Me.cmbTipoOperacionNuevaConciliacion.Size = New System.Drawing.Size(324, 24)
        Me.cmbTipoOperacionNuevaConciliacion.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(28, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(137, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "TIPO DE OPERACIÓN:"
        '
        'cmbTipoDatosNuevaConciliacion
        '
        Me.cmbTipoDatosNuevaConciliacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTipoDatosNuevaConciliacion.FormattingEnabled = True
        Me.cmbTipoDatosNuevaConciliacion.Items.AddRange(New Object() {"NÚMERICO", "TEXTO", "MONEDA", "FECHA"})
        Me.cmbTipoDatosNuevaConciliacion.Location = New System.Drawing.Point(31, 66)
        Me.cmbTipoDatosNuevaConciliacion.Name = "cmbTipoDatosNuevaConciliacion"
        Me.cmbTipoDatosNuevaConciliacion.Size = New System.Drawing.Size(324, 24)
        Me.cmbTipoDatosNuevaConciliacion.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(28, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "TIPO DE DATOS:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(121, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(123, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "CONCILIAR CON"
        '
        'btnCancelarNuevaConciliacion
        '
        Me.btnCancelarNuevaConciliacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelarNuevaConciliacion.Image = Global.Conciliacion.My.Resources.Resources.cancel
        Me.btnCancelarNuevaConciliacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarNuevaConciliacion.Location = New System.Drawing.Point(197, 192)
        Me.btnCancelarNuevaConciliacion.Name = "btnCancelarNuevaConciliacion"
        Me.btnCancelarNuevaConciliacion.Size = New System.Drawing.Size(158, 44)
        Me.btnCancelarNuevaConciliacion.TabIndex = 8
        Me.btnCancelarNuevaConciliacion.Text = "CANCELAR"
        Me.btnCancelarNuevaConciliacion.UseVisualStyleBackColor = True
        '
        'btnAgregarNuevaConciliacion
        '
        Me.btnAgregarNuevaConciliacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAgregarNuevaConciliacion.Image = Global.Conciliacion.My.Resources.Resources.if_Save_131694
        Me.btnAgregarNuevaConciliacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAgregarNuevaConciliacion.Location = New System.Drawing.Point(31, 192)
        Me.btnAgregarNuevaConciliacion.Name = "btnAgregarNuevaConciliacion"
        Me.btnAgregarNuevaConciliacion.Size = New System.Drawing.Size(160, 44)
        Me.btnAgregarNuevaConciliacion.TabIndex = 7
        Me.btnAgregarNuevaConciliacion.Text = "GUARDAR"
        Me.btnAgregarNuevaConciliacion.UseVisualStyleBackColor = True
        '
        'ModificarNuevaConciliacion
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1117, 284)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ModificarNuevaConciliacion"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "AGREGAR NUEVA CONCILIACIÓN"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lbxColumnaReporte2 As ListBox
    Friend WithEvents lbxColumnasReporte1 As ListBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbTipoOperacionNuevaConciliacion As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbTipoDatosNuevaConciliacion As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnCancelarNuevaConciliacion As Button
    Friend WithEvents btnAgregarNuevaConciliacion As Button
    Friend WithEvents cmbGrupoNuevaConciliacion As ComboBox
    Friend WithEvents Label4 As Label
End Class
