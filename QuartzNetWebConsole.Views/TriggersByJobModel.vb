Public Class TriggersByJobModel
    Public ReadOnly Triggers As IEnumerable(Of TriggerWithState)
    Public ReadOnly ThisUrl As String
    Public ReadOnly Group As String
    Public ReadOnly Job As String
    Public ReadOnly Highlight As String

    Public Sub New(ByVal triggers As IEnumerable(Of TriggerWithState), ByVal thisUrl As String, ByVal group As String, ByVal job As String, ByVal highlight As String)
        Me.Triggers = triggers
        Me.ThisUrl = thisUrl
        Me.Group = group
        Me.Job = job
        Me.Highlight = highlight
    End Sub
End Class
