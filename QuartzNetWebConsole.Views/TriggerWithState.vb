Imports Quartz

Public Class TriggerWithState
    Public ReadOnly Trigger As Trigger
    Public ReadOnly State As TriggerState

    Public Sub New(ByVal trigger As Trigger, ByVal state As TriggerState)
        Me.Trigger = trigger
        Me.State = state
    End Sub

    Public ReadOnly Property IsPaused As Boolean
        Get
            Return State = TriggerState.Paused
        End Get
    End Property
End Class
