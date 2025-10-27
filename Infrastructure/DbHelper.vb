Imports System.Data
Imports MySql.Data.MySqlClient

Public Module DbHelper

    Public Function ExecuteDataTable(ByVal factory As IConnectionFactory, ByVal sql As String, ByVal ParamArray parameters() As MySqlParameter) As DataTable
        Dim dt As New DataTable()
        Using cn As MySqlConnection = factory.GetOpenConnection()
            Using cmd As New MySqlCommand(sql, cn)
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Using da As New MySqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function


    ' Helper para crear parámetros sin repetir código
    Public Function P(ByVal name As String, ByVal value As Object) As MySqlParameter
        Dim prm As New MySqlParameter(name, value)
        If value Is Nothing Then
            prm.Value = DBNull.Value
        End If
        Return prm
    End Function

End Module
