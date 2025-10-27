Imports Reportes
Imports MySql.Data.MySqlClient

Public Class LoginForm

    Private _authRepo As AuthRepository

    Private Sub LoginForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Inicializa repositorio de autenticación
        Dim authFactory As IConnectionFactory = New AuthConnectionFactory()
        _authRepo = New AuthRepository(authFactory)

        lblTitulo.Text = "Ingreso al sistema Reportes"
        lblStatus.Text = ""
        lblStatus.Visible = False  ' oculto si no hay mensaje
        txtUser.Focus()

    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Try
            ShowStatus("", False)

            Dim userId As String = txtUser.Text.Trim()


            If userId = "" Then
                ShowStatus("Ingrese usuario.", True)
                txtUser.Focus()
                txtUser.SelectAll()
                Return
            End If

         

            Dim session As UserSession = _authRepo.Validate(userId)
            If session Is Nothing Then
                ' ← aquí: SOLO limpiamos la contraseña
                txtUser.Clear()
                txtUser.Focus()
                ShowStatus("Credencial inválida o usuario inactivo.", True)
                Return
            End If

            ShowStatus("Acceso correcto. Cargando…", False)
            Dim frm As New MainForm(session)
            frm.Show()
            Me.Hide()

        Catch ex As Exception
            ShowStatus("Error al validar: " & ex.Message, True)
        End Try
    End Sub

    Private Sub txtUser_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtUser.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub
    ' Opcional: cerrar app si se cierra Login y no hay Main abierto
    Private Sub LoginForm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        If Application.OpenForms.Count = 0 Then
            Application.Exit()
        End If
    End Sub

    Private Sub ShowStatus(ByVal message As String, ByVal isError As Boolean)
        lblStatus.Text = message
        lblStatus.Visible = (message IsNot Nothing AndAlso message.Trim() <> "")
        If isError Then
            lblStatus.ForeColor = Color.DarkRed
        Else
            lblStatus.ForeColor = Color.DarkGreen
        End If
    End Sub
    Private Sub txtUser_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUser.TextChanged
        If lblStatus.Visible Then ShowStatus("", False)
    End Sub

    Private Sub grpLogin_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpLogin.Enter

    End Sub
End Class
