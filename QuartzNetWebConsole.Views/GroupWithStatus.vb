Public Class GroupWithStatus
    Public ReadOnly Name As String
    Public ReadOnly Paused As Boolean?

    Public Sub New(name As String, paused As Boolean?)
        Me.Name = name
        Me.Paused = paused
    End Sub
End Class
