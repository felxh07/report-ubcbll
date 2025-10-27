<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UcRptCirc001
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.cboSede = New System.Windows.Forms.ComboBox()
        Me.lblSede = New System.Windows.Forms.Label()
        Me.cboTipoMov = New System.Windows.Forms.ComboBox()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.dgvCIRC = New System.Windows.Forms.DataGridView()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvCIRC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.btnBuscar)
        Me.Panel1.Controls.Add(Me.cboSede)
        Me.Panel1.Controls.Add(Me.lblSede)
        Me.Panel1.Controls.Add(Me.cboTipoMov)
        Me.Panel1.Controls.Add(Me.lblTipo)
        Me.Panel1.Controls.Add(Me.dtpHasta)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.dtpDesde)
        Me.Panel1.Controls.Add(Me.lblDesde)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(860, 100)
        Me.Panel1.TabIndex = 0
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(751, 26)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(75, 23)
        Me.btnBuscar.TabIndex = 8
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'cboSede
        '
        Me.cboSede.FormattingEnabled = True
        Me.cboSede.Items.AddRange(New Object() {"FACULTAD", "INDIVIDUAL"})
        Me.cboSede.Location = New System.Drawing.Point(13, 28)
        Me.cboSede.Name = "cboSede"
        Me.cboSede.Size = New System.Drawing.Size(121, 21)
        Me.cboSede.TabIndex = 7
        '
        'lblSede
        '
        Me.lblSede.AutoSize = True
        Me.lblSede.Location = New System.Drawing.Point(40, 10)
        Me.lblSede.Name = "lblSede"
        Me.lblSede.Size = New System.Drawing.Size(32, 13)
        Me.lblSede.TabIndex = 6
        Me.lblSede.Text = "Sede"
        '
        'cboTipoMov
        '
        Me.cboTipoMov.FormattingEnabled = True
        Me.cboTipoMov.Items.AddRange(New Object() {"ALL", "ISSUE", "RETURN", "RENEWAL"})
        Me.cboTipoMov.Location = New System.Drawing.Point(589, 26)
        Me.cboTipoMov.Name = "cboTipoMov"
        Me.cboTipoMov.Size = New System.Drawing.Size(121, 21)
        Me.cboTipoMov.TabIndex = 5
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(498, 26)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(61, 13)
        Me.lblTipo.TabIndex = 4
        Me.lblTipo.Text = "Movimiento"
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(348, 61)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(91, 20)
        Me.dtpHasta.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(292, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Hasta"
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(348, 23)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(91, 20)
        Me.dtpDesde.TabIndex = 1
        '
        'lblDesde
        '
        Me.lblDesde.AutoSize = True
        Me.lblDesde.Location = New System.Drawing.Point(292, 26)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(38, 13)
        Me.lblDesde.TabIndex = 0
        Me.lblDesde.Text = "Desde"
        '
        'dgvCIRC
        '
        Me.dgvCIRC.AllowUserToAddRows = False
        Me.dgvCIRC.AllowUserToDeleteRows = False
        Me.dgvCIRC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.dgvCIRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCIRC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCIRC.Location = New System.Drawing.Point(0, 100)
        Me.dgvCIRC.MultiSelect = False
        Me.dgvCIRC.Name = "dgvCIRC"
        Me.dgvCIRC.ReadOnly = True
        Me.dgvCIRC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCIRC.Size = New System.Drawing.Size(860, 500)
        Me.dgvCIRC.TabIndex = 1
        '
        'UcRptCirc001
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgvCIRC)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UcRptCirc001"
        Me.Size = New System.Drawing.Size(860, 600)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvCIRC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDesde As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents cboSede As System.Windows.Forms.ComboBox
    Friend WithEvents lblSede As System.Windows.Forms.Label
    Friend WithEvents cboTipoMov As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents dgvCIRC As System.Windows.Forms.DataGridView

End Class
