Imports System.Data
Imports Reportes

Public Class UcRptCirc001
    Inherits UserControl

    Private ReadOnly _session As UserSession
    Private ReadOnly _repo As ReportRepository

    Public Sub New(ByVal session As UserSession)
        InitializeComponent()
        _session = session
        Dim kohaFactory As IConnectionFactory = New KohaConnectionFactory()
        _repo = New ReportRepository(kohaFactory)
    End Sub

    Private Sub UcRptCirc001_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Inicializa filtros
        dtpDesde.Value = Date.Today.AddDays(-7)
        dtpHasta.Value = Date.Today

        cboTipoMov.Items.Clear()
        cboTipoMov.Items.Add("ALL")
        cboTipoMov.Items.Add("ISSUE")
        cboTipoMov.Items.Add("RETURN")
        cboTipoMov.Items.Add("RENEWAL")
        cboTipoMov.SelectedIndex = 0

        If String.Equals(_session.Rol, "FAC_USER", StringComparison.OrdinalIgnoreCase) Then
            cboSede.Items.Clear()
            cboSede.Items.Add("FACULTAD")
            cboSede.Items.Add("INDIVIDUAL")
            cboSede.SelectedIndex = 0
            lblSede.Visible = True
            cboSede.Visible = True
        Else
            lblSede.Visible = False
            cboSede.Visible = False
        End If

        dgvCIRC.AutoGenerateColumns = True
        dgvCIRC.ReadOnly = True
        dgvCIRC.AllowUserToAddRows = False
        dgvCIRC.AllowUserToDeleteRows = False
        dgvCIRC.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCIRC.MultiSelect = False
        dgvCIRC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim desde As DateTime = dtpDesde.Value.Date
            Dim hasta As DateTime = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1) ' incluir todo el día
            Dim tipo As String = If(cboTipoMov.SelectedItem IsNot Nothing, cboTipoMov.SelectedItem.ToString(), "ALL")
            Dim sede As String = "FACULTAD"
            If String.Equals(_session.Rol, "FAC_USER", StringComparison.OrdinalIgnoreCase) AndAlso _
               cboSede.Visible AndAlso cboSede.SelectedItem IsNot Nothing Then
                sede = cboSede.SelectedItem.ToString()
            End If

            Dim dt As DataTable = _repo.GetCirculation(_session, desde, hasta, tipo, sede)
            dgvCIRC.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error al buscar: " & ex.Message, "RPT-CIRC-001", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub dgvCIRC_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCIRC.CellContentClick

    End Sub
End Class
