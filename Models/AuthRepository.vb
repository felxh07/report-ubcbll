Imports System.Data
Imports MySql.Data.MySqlClient
Imports Reportes   ' (ajusta el namespace si aplica)

Public Class AuthRepository
    Private ReadOnly _factory As IConnectionFactory

    Public Sub New(ByVal factory As IConnectionFactory)
        _factory = factory
    End Sub

    Public Function Validate(ByVal userid As String) As UserSession
        Const sql As String = "SELECT userid, nombre, sede, ubicacion, role, is_active " & _
                              "FROM users_app " & _
                              "WHERE userid=@userid AND is_active=1 LIMIT 1;"

        Dim dt As DataTable = DbHelper.ExecuteDataTable(_factory, sql, _
                            DbHelper.P("@userid", userid))

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Return Nothing
        End If

        Dim r As DataRow = dt.Rows(0)
        Dim s As New UserSession()
        s.UserId = Convert.ToString(r("userid"))
        s.Nombre = Convert.ToString(r("nombre"))
        s.Sede = Convert.ToString(r("sede"))
        s.Ubicacion = Convert.ToString(r("ubicacion"))
        s.Rol = Convert.ToString(r("role"))
        Return s
    End Function
End Class
