Imports System.Data
Imports Reportes

Public Class MainForm

    Private ReadOnly _session As UserSession
    Private ReadOnly _menuRepo As MenuRepository

    Public Sub New(ByVal session As UserSession)
        InitializeComponent()
        _session = session
        Dim authFactory As IConnectionFactory = New AuthConnectionFactory() ' menus viven en misma BD de auth
        _menuRepo = New MenuRepository(authFactory)
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Reportes — UNAC"
        lblBienvenida.Text = String.Format("Bienvenido: {0} — {1} — {2} — {3}", _
                                           _session.Nombre, _session.Sede, _session.Ubicacion, _session.Rol)
        lblFecha.Text = "Hoy: " & Date.Now.ToString("dd/MM/yyyy")
        CargarMenu()
    End Sub

    Private Sub CargarMenu()
        Try
            tvMenu.Nodes.Clear()

            Dim dt As DataTable = _menuRepo.GetMenusByRole(_session.Rol)
            ' Construir árbol: primero padres (parent_id IS NULL), luego hijos
            Dim padres() As DataRow = dt.Select("parent_id IS NULL", "orden ASC")
            For Each p As DataRow In padres
                Dim nodoPadre As New TreeNode(Convert.ToString(p("nombre")))
                nodoPadre.Tag = New MenuTag(Convert.ToInt32(p("id_menu")), Convert.ToString(p("ruta")), True)
                tvMenu.Nodes.Add(nodoPadre)

                ' hijos de este padre
                Dim hijos() As DataRow = dt.Select("parent_id = " & Convert.ToInt32(p("id_menu")), "orden ASC")
                For Each h As DataRow In hijos
                    Dim nodoHijo As New TreeNode(Convert.ToString(h("nombre")))
                    nodoHijo.Tag = New MenuTag(Convert.ToInt32(h("id_menu")), Convert.ToString(h("ruta")), False)
                    nodoPadre.Nodes.Add(nodoHijo)
                Next
            Next

            tvMenu.ExpandAll()
        Catch ex As Exception
            MessageBox.Show("Error al cargar menú: " & ex.Message, "Menú", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub tvMenu_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvMenu.AfterSelect
        Dim tag As MenuTag = TryCast(e.Node.Tag, MenuTag)
        If tag Is Nothing Then Return

        If tag.EsPadre Then
            ' No hacer nada al hacer clic en el grupo
            Return
        End If

        CargarUserControlPorRuta(tag.Ruta)
    End Sub

    Private Sub CargarUserControlPorRuta(ByVal ruta As String)
        Try
            pnlContent.Controls.Clear()

            Select Case ruta
                Case "RPT_CIRC_001"
                    Dim uc As New UcRptCirc001(_session)
                    uc.Dock = DockStyle.Fill
                    pnlContent.Controls.Add(uc)

                Case "RPT_COL_001"
                    Dim uc As New UcRptCol001(_session)
                    uc.Dock = DockStyle.Fill
                    pnlContent.Controls.Add(uc)

                Case Else
                    Dim lbl As New Label()
                    lbl.Text = "Ruta no implementada: " & ruta
                    lbl.AutoSize = True
                    lbl.Location = New Point(12, 12)
                    pnlContent.Controls.Add(lbl)
            End Select

        Catch ex As Exception
            MessageBox.Show("Error al cargar vista: " & ex.Message, "Vista", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Me.Close()
            ' Volver al Login si aún existe
            If Application.OpenForms("LoginForm") IsNot Nothing Then
                Application.OpenForms("LoginForm").Show()
            Else
                Application.Exit()
            End If
        Catch
            Application.Exit()
        End Try
    End Sub

    ' Clase auxiliar para guardar info del nodo
    Private Class MenuTag
        Public ReadOnly Id As Integer
        Public ReadOnly Ruta As String
        Public ReadOnly EsPadre As Boolean
        Public Sub New(ByVal idMenu As Integer, ByVal rutaMenu As String, ByVal esPadreMenu As Boolean)
            Id = idMenu
            Ruta = rutaMenu
            EsPadre = esPadreMenu
        End Sub
    End Class

    Private Sub pnlContent_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlContent.Paint

    End Sub
End Class
