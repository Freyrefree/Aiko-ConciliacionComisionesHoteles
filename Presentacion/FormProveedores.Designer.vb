<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormProveedores
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNombreCliente = New System.Windows.Forms.TextBox()
        Me.BtnGuardarCliente = New System.Windows.Forms.Button()
        Me.BtnActualizarCliente = New System.Windows.Forms.Button()
        Me.BtnEliminarCliente = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(12, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(383, 150)
        Me.DataGridView1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(401, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Nombre"
        '
        'txtNombreCliente
        '
        Me.txtNombreCliente.Location = New System.Drawing.Point(451, 9)
        Me.txtNombreCliente.Name = "txtNombreCliente"
        Me.txtNombreCliente.Size = New System.Drawing.Size(100, 20)
        Me.txtNombreCliente.TabIndex = 2
        '
        'BtnGuardarCliente
        '
        Me.BtnGuardarCliente.Location = New System.Drawing.Point(401, 120)
        Me.BtnGuardarCliente.Name = "BtnGuardarCliente"
        Me.BtnGuardarCliente.Size = New System.Drawing.Size(100, 42)
        Me.BtnGuardarCliente.TabIndex = 3
        Me.BtnGuardarCliente.Text = "Guardar"
        Me.BtnGuardarCliente.UseVisualStyleBackColor = True
        '
        'BtnActualizarCliente
        '
        Me.BtnActualizarCliente.Location = New System.Drawing.Point(507, 120)
        Me.BtnActualizarCliente.Name = "BtnActualizarCliente"
        Me.BtnActualizarCliente.Size = New System.Drawing.Size(100, 42)
        Me.BtnActualizarCliente.TabIndex = 5
        Me.BtnActualizarCliente.Text = "Actualizar"
        Me.BtnActualizarCliente.UseVisualStyleBackColor = True
        '
        'BtnEliminarCliente
        '
        Me.BtnEliminarCliente.Location = New System.Drawing.Point(613, 120)
        Me.BtnEliminarCliente.Name = "BtnEliminarCliente"
        Me.BtnEliminarCliente.Size = New System.Drawing.Size(100, 42)
        Me.BtnEliminarCliente.TabIndex = 6
        Me.BtnEliminarCliente.Text = "Eliminar"
        Me.BtnEliminarCliente.UseVisualStyleBackColor = True
        '
        'FormProveedores
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(725, 169)
        Me.Controls.Add(Me.BtnEliminarCliente)
        Me.Controls.Add(Me.BtnActualizarCliente)
        Me.Controls.Add(Me.BtnGuardarCliente)
        Me.Controls.Add(Me.txtNombreCliente)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "FormProveedores"
        Me.Text = "Proveedores"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents txtNombreCliente As TextBox
    Friend WithEvents BtnGuardarCliente As Button
    Friend WithEvents BtnActualizarCliente As Button
    Friend WithEvents BtnEliminarCliente As Button
End Class
