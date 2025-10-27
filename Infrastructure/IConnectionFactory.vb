Imports MySql.Data.MySqlClient

Public Interface IConnectionFactory
    Function GetOpenConnection() As MySqlConnection
End Interface
