Public Class LogEntry
    Public ReadOnly Timestamp As DateTimeOffset
    Public ReadOnly Description As String

    Public Sub New(timestamp As DateTimeOffset, description As String)
        Me.Timestamp = timestamp
        Me.Description = description
    End Sub

    Public Sub New(description As String)
        Me.Description = description
        Timestamp = DateTimeOffset.Now
    End Sub
End Class
