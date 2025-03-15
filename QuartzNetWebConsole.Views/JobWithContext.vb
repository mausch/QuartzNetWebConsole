Imports Quartz

Public Class JobWithContext
    Public ReadOnly Job As IJobDetail
    Public ReadOnly JobContext As IJobExecutionContext
    Public ReadOnly Interruptible As Boolean

    Public Sub New(job As IJobDetail, jobContext As IJobExecutionContext, interruptible As Boolean)
        If job Is Nothing Then
            Throw New ArgumentNullException("job")
        End If
        Me.Job = job
        Me.JobContext = jobContext
        Me.Interruptible = interruptible
    End Sub
End Class
