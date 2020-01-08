<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Tipo_de_Cambio
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DGVPeriodos = New System.Windows.Forms.DataGridView()
        Me.DGVMonedas = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.labelTotalMonedas = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.labelTotalPeriodos = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.DGVPeriodos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DGVMonedas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DGVPeriodos
        '
        Me.DGVPeriodos.BackgroundColor = System.Drawing.SystemColors.ButtonFace
        Me.DGVPeriodos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVPeriodos.Location = New System.Drawing.Point(12, 29)
        Me.DGVPeriodos.Name = "DGVPeriodos"
        Me.DGVPeriodos.ReadOnly = True
        Me.DGVPeriodos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DGVPeriodos.Size = New System.Drawing.Size(163, 245)
        Me.DGVPeriodos.TabIndex = 7
        '
        'DGVMonedas
        '
        Me.DGVMonedas.BackgroundColor = System.Drawing.SystemColors.ButtonFace
        Me.DGVMonedas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVMonedas.EnableHeadersVisualStyles = False
        Me.DGVMonedas.Location = New System.Drawing.Point(203, 29)
        Me.DGVMonedas.Name = "DGVMonedas"
        Me.DGVMonedas.ReadOnly = True
        Me.DGVMonedas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DGVMonedas.Size = New System.Drawing.Size(812, 245)
        Me.DGVMonedas.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "PERIODOS"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(200, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(113, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "VALOR MONEDAS"
        '
        'labelTotalMonedas
        '
        Me.labelTotalMonedas.AutoSize = True
        Me.labelTotalMonedas.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTotalMonedas.Location = New System.Drawing.Point(897, 277)
        Me.labelTotalMonedas.Name = "labelTotalMonedas"
        Me.labelTotalMonedas.Size = New System.Drawing.Size(19, 19)
        Me.labelTotalMonedas.TabIndex = 86
        Me.labelTotalMonedas.Text = "0"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(827, 277)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 19)
        Me.Label6.TabIndex = 85
        Me.Label6.Text = "TOTAL"
        '
        'labelTotalPeriodos
        '
        Me.labelTotalPeriodos.AutoSize = True
        Me.labelTotalPeriodos.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelTotalPeriodos.Location = New System.Drawing.Point(128, 277)
        Me.labelTotalPeriodos.Name = "labelTotalPeriodos"
        Me.labelTotalPeriodos.Size = New System.Drawing.Size(19, 19)
        Me.labelTotalPeriodos.TabIndex = 88
        Me.labelTotalPeriodos.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(58, 277)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 19)
        Me.Label2.TabIndex = 87
        Me.Label2.Text = "TOTAL"
        '
        'Tipo_de_Cambio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1027, 334)
        Me.Controls.Add(Me.labelTotalPeriodos)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.labelTotalMonedas)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DGVMonedas)
        Me.Controls.Add(Me.DGVPeriodos)
        Me.Name = "Tipo_de_Cambio"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tipo_de_Cambio"
        CType(Me.DGVPeriodos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DGVMonedas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DGVPeriodos As DataGridView
    Friend WithEvents DGVMonedas As DataGridView
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents labelTotalMonedas As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents labelTotalPeriodos As Label
    Friend WithEvents Label2 As Label
End Class
