Imports Quartz

Public Class JobWithContext
    Public ReadOnly Job As JobDetail
    Public ReadOnly JobContext As JobExecutionContext
    Public ReadOnly Interruptible As Boolean

    Public Sub New(ByVal job As JobDetail, ByVal jobContext As JobExecutionContext, ByVal interruptible As Boolean)
        Me.Job = job
        Me.JobContext = jobContext
        Me.Interruptible = interruptible
    End Sub
End Class
