Public Class GroupWithStatus
    Public ReadOnly Name As String
    Public ReadOnly Paused As Boolean

    Public Sub New(ByVal name As String, ByVal paused As Boolean)
        Me.Name = name
        Me.Paused = paused
    End Sub
End Class
