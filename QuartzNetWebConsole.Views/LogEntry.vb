Public Class LogEntry
    Public ReadOnly Timestamp As DateTimeOffset
    Public ReadOnly Description As String

    Public Sub New(ByVal timestamp As DateTimeOffset, ByVal description As String)
        Me.Timestamp = timestamp
        Me.Description = description
    End Sub

    Public Sub New(ByVal description As String)
        Me.Description = description
        Timestamp = DateTimeOffset.Now
    End Sub
End Class
