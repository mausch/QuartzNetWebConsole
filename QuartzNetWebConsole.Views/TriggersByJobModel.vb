Public Class TriggersByJobModel
    Public ReadOnly Triggers As IEnumerable(Of TriggerWithState)
    Public ReadOnly ThisUrl As String
    Public ReadOnly Group As String
    Public ReadOnly Job As String
    Public ReadOnly Highlight As String

    Public Sub New(triggers As IEnumerable(Of TriggerWithState), thisUrl As String, group As String, job As String, highlight As String)
        Me.Triggers = triggers
        Me.ThisUrl = thisUrl
        Me.Group = group
        Me.Job = job
        Me.Highlight = highlight
    End Sub
End Class
