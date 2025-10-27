Public Class UserSession
    Public Property UserId As String
    Public Property Nombre As String
    Public Property Sede As String        ' Ej: UBCBLL, BE-FIME, etc.
    Public Property Ubicacion As String   ' Ej: PPS1, SPS1, CP, PP, etc.
    Public Property Rol As String         ' ADMIN | FAC_USER | CENTRAL_SALA | CENTRAL_JEFATURA
    Public ReadOnly Property IsAdmin As Boolean
        Get
            Return String.Equals(Rol, "ADMIN", StringComparison.OrdinalIgnoreCase)
        End Get
    End Property
End Class
