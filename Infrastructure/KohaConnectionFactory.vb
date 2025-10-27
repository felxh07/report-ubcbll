Imports System.Configuration
Imports MySql.Data.MySqlClient

Public Class KohaConnectionFactory
    Implements IConnectionFactory

    Private ReadOnly _connString As String
    Public Sub New()
        _connString = ConfigurationManager.ConnectionStrings("KohaMySql").ConnectionString
    End Sub
    Public Function GetOpenConnection() As MySqlConnection Implements IConnectionFactory.GetOpenConnection
        Dim cn As New MySqlConnection(_connString)
        cn.Open()
        Return cn
    End Function
End Class
