Imports System.Data
Imports Reportes

Public Class UcRptCol001
    Inherits UserControl

    Private ReadOnly _session As UserSession
    Private ReadOnly _repo As ReportRepository

    Public Sub New(ByVal session As UserSession)
        InitializeComponent()
        _session = session
        Dim kohaFactory As IConnectionFactory = New KohaConnectionFactory()
        _repo = New ReportRepository(kohaFactory)
    End Sub

    Private Sub UcRptCol001_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cboEstado.Items.Clear()
        cboEstado.Items.Add("ALL")
        cboEstado.Items.Add("Disponible")
        cboEstado.Items.Add("Prestado")
        cboEstado.Items.Add("Perdido")
        cboEstado.Items.Add("Dañado")
        cboEstado.Items.Add("No prestable")
        cboEstado.SelectedIndex = 0

        dgvCOL.AutoGenerateColumns = True
        dgvCOL.ReadOnly = True
        dgvCOL.AllowUserToAddRows = False
        dgvCOL.AllowUserToDeleteRows = False
        dgvCOL.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCOL.MultiSelect = False
        dgvCOL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Dim estado As String = If(cboEstado.SelectedItem IsNot Nothing, cboEstado.SelectedItem.ToString(), "ALL")
            Dim dt As DataTable = _repo.GetCollectionByStatus(_session, estado)
            dgvCOL.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Error al buscar: " & ex.Message, "RPT-COL-001", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
