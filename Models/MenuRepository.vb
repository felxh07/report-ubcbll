Imports System.Data
Imports MySql.Data.MySqlClient

Public Class MenuRepository
    Private ReadOnly _factory As IConnectionFactory

    Public Sub New(ByVal factory As IConnectionFactory)
        _factory = factory
    End Sub

    ' Devuelve menús (padres e hijos) visibles para el rol, ordenados
    Public Function GetMenusByRole(ByVal rol As String) As DataTable
        Const sql As String = _
            "SELECT m.id_menu, m.nombre, m.ruta, m.parent_id, m.orden " & _
            "FROM menus m " & _
            "JOIN menu_roles mr ON mr.menu_id = m.id_menu " & _
            "WHERE mr.role = @rol AND m.is_active = 1 " & _
            "ORDER BY COALESCE(m.parent_id, m.id_menu), m.orden, m.id_menu;"

        Return DbHelper.ExecuteDataTable(_factory, sql, DbHelper.P("@rol", rol))
    End Function
End Class
