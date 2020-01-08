<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.menuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ConciliaciònToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConciliaciònAmexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConciliaciónClientesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.salirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.label1 = New System.Windows.Forms.Label()
        Me.IndicadoresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStrip1.SuspendLayout()
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'menuStrip1
        '
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConciliaciònToolStripMenuItem, Me.salirToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.MinimumSize = New System.Drawing.Size(0, 50)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(985, 50)
        Me.menuStrip1.TabIndex = 2
        Me.menuStrip1.Text = "menuStrip1"
        '
        'ConciliaciònToolStripMenuItem
        '
        Me.ConciliaciònToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConciliaciònAmexToolStripMenuItem, Me.ConciliaciónClientesToolStripMenuItem, Me.IndicadoresToolStripMenuItem})
        Me.ConciliaciònToolStripMenuItem.Image = Global.Conciliacion.My.Resources.Resources.database_table
        Me.ConciliaciònToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ConciliaciònToolStripMenuItem.Name = "ConciliaciònToolStripMenuItem"
        Me.ConciliaciònToolStripMenuItem.Size = New System.Drawing.Size(128, 46)
        Me.ConciliaciònToolStripMenuItem.Text = "Conciliaciones"
        '
        'ConciliaciònAmexToolStripMenuItem
        '
        Me.ConciliaciònAmexToolStripMenuItem.Name = "ConciliaciònAmexToolStripMenuItem"
        Me.ConciliaciònAmexToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.ConciliaciònAmexToolStripMenuItem.Text = "Conciliación General"
        '
        'ConciliaciónClientesToolStripMenuItem
        '
        Me.ConciliaciónClientesToolStripMenuItem.Name = "ConciliaciónClientesToolStripMenuItem"
        Me.ConciliaciónClientesToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.ConciliaciónClientesToolStripMenuItem.Text = "Conciliación Comisiones Hoteles"
        '
        'salirToolStripMenuItem
        '
        Me.salirToolStripMenuItem.Image = Global.Conciliacion.My.Resources.Resources.application_go
        Me.salirToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.salirToolStripMenuItem.Name = "salirToolStripMenuItem"
        Me.salirToolStripMenuItem.Size = New System.Drawing.Size(73, 46)
        Me.salirToolStripMenuItem.Text = "Salir"
        '
        'panel1
        '
        Me.panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel1.AutoSize = True
        Me.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.panel1.Controls.Add(Me.label1)
        Me.panel1.Location = New System.Drawing.Point(0, 388)
        Me.panel1.Name = "panel1"
        Me.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.panel1.Size = New System.Drawing.Size(985, 44)
        Me.panel1.TabIndex = 3
        '
        'label1
        '
        Me.label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.label1.AutoSize = True
        Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label1.ForeColor = System.Drawing.Color.White
        Me.label1.Location = New System.Drawing.Point(316, 15)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(277, 16)
        Me.label1.TabIndex = 0
        Me.label1.Text = "AIKO-Todos los Derechos Reservados"
        '
        'IndicadoresToolStripMenuItem
        '
        Me.IndicadoresToolStripMenuItem.Name = "IndicadoresToolStripMenuItem"
        Me.IndicadoresToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.IndicadoresToolStripMenuItem.Text = "Indicadores"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(985, 430)
        Me.Controls.Add(Me.menuStrip1)
        Me.Controls.Add(Me.panel1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Conciliación General"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.panel1.ResumeLayout(False)
        Me.panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents menuStrip1 As MenuStrip
    Private WithEvents salirToolStripMenuItem As ToolStripMenuItem
    Private WithEvents panel1 As Panel
    Private WithEvents label1 As Label
    Friend WithEvents ConciliaciònToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConciliaciònAmexToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConciliaciónClientesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IndicadoresToolStripMenuItem As ToolStripMenuItem
End Class
