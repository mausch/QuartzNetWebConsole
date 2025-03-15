Imports System.Net
Imports Quartz
Imports Quartz.Impl.Triggers

Public Module Views
    Public Function Pager(pagination As PaginationInfo) As XElement
        Return _
            <div class="pagination">
                Showing <%= pagination.FirstItemIndex %> - <%= pagination.LastItemIndex %> of <%= pagination.TotalItemCount %>

                <%= If(pagination.HasPrevPage,
                    <a href=<%= pagination.PrevPageUrl %>><%= X.laquo %> Previous</a>,
                    <span class="disabledPage"><%= X.laquo %> Previous</span>) %>

                <%= From p In pagination.Pages
                    Select
                    If(p = pagination.CurrentPage,
                    <span class="currentPage"><%= p %></span>,
                    <a href=<%= pagination.PageUrlFor(p) %>><%= p %></a>) %>

                <%= If(pagination.HasNextPage,
                    <a href=<%= pagination.NextPageUrl %>><%= X.raquo %> Next</a>,
                    <span class="disabledPage"><%= X.raquo %> Next</span>) %>
            </div>
    End Function

    Public Function Log(logs As IEnumerable(Of LogEntry), pagination As PaginationInfo, thisUrl As String) As XElement
        Return _
            <html>
                <head>
                    <title>Quartz.Net Console - Log</title>
                    <%= Stylesheet %>
                    <link rel="alternate" type="application/rss+xml" title="RSS" href="log.ashx?rss=1"/>
                </head>
                <body>
                    <a href="index.ashx">Index</a>
                    <h1>Scheduler log</h1>
                    <table>
                        <tr>
                            <th>Date / Time</th>
                            <th>Description</th>
                        </tr>
                        <%= From e In logs
                            Select
                            <tr>
                                <td class="datetime"><%= e.Timestamp %></td>
                                <td><%= X.Raw(e.Description) %></td>
                            </tr>
                        %>
                    </table>
                    <br/>
                    <%= Pager(pagination) %>
                </body>
            </html>
    End Function

    Public Function LogRSS(thisUrl As String, logs As IEnumerable(Of LogEntry)) As XElement
        Return _
            <rss version="2.0">
                <channel>
                    <title>Quartz.NET log</title>
                    <description>Quartz.NET log</description>
                    <link><%= thisUrl %></link>
                    <pubDate><%= DateTimeOffset.Now.ToString("R") %></pubDate>
                    <%= From r In logs
                        Select
                        <item>
                            <title><%= r.Timestamp %></title>
                            <description><%= r.Description %></description>
                            <pubDate><%= r.Timestamp.ToString("R") %></pubDate>
                        </item>
                    %>
                </channel>
            </rss>
    End Function

    Public Function SchedulerStatus(schedulerName As String, inStandby As Boolean, metadata As SchedulerMetaData) As XElement
        Return _
            <div class="group">
                <h2>Scheduler name: <%= schedulerName %></h2>
                <div style="float: left">
				    Job store: <%= metadata.JobStoreType %><br/>
				    Supports persistence: <%= YesNo(metadata.JobStoreSupportsPersistence) %><br/>
				    Number of jobs executed: <%= metadata.NumberOfJobsExecuted %><br/>
				    Running since: <span class="datetime"><%= metadata.RunningSince %></span><br/>
				    Status: <%= If(inStandby, "stand-by", "running") %>
                    <br/>
                    <a href="log.ashx">View log</a>
                </div>
                <div style="float: right">
                    <%= SimpleForm("scheduler.ashx?method=Shutdown", "Shut down") %>
                    <%= If(inStandby,
                        SimpleForm("scheduler.ashx?method=Start", "Start"),
                        SimpleForm("scheduler.ashx?method=Standby", "Stand by")) %>
                    <%= SimpleForm("scheduler.ashx?method=PauseAll", "Pause all triggers") %>
                    <%= SimpleForm("scheduler.ashx?method=ResumeAll", "Resume all triggers") %>
                </div>
            </div>

    End Function

    Public Function SchedulerListeners(listeners As IEnumerable(Of ISchedulerListener)) As XElement
        Return _
            <div class="group">
                <h2>Scheduler listeners</h2>
                <table>
                    <tr>
                        <th>Type</th>
                    </tr>
                    <%= From l In listeners
                        Select
                        <tr>
                            <td><%= l.GetType() %></td>
                        </tr>
                    %>
                </table>
            </div>
    End Function

    Public Function SchedulerCalendars(calendars As ICollection(Of KeyValuePair(Of String, String))) As XElement
        Return _
            <div class="group">
                <h2>Calendars</h2>
                <%= If(calendars.Count = 0,
                    <span>No calendars</span>,
                    <table>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                        </tr>
                        <%= From cal In calendars
                            Select
                            <tr>
                                <td><%= cal.Key %></td>
                                <td><%= cal.Value %></td>
                            </tr>
                        %>
                    </table>) %>
            </div>

    End Function

    Public Function SchedulerEntityGroups(entity As String) As Func(Of IEnumerable(Of GroupWithStatus), XElement)
        Return Function(groups) _
                    <div class="group">
                        <h2><%= entity %> groups</h2>
                        <table>
                            <tr>
                                <th>Name</th>
                                <th>Status</th>
                                <th></th>
                            </tr>
                            <%= From group In groups
                                Select
                                <tr>
                                    <td>
                                        <a href=<%= entity & "Group.ashx?group=" & group.Name %>><%= group.Name %></a>
                                    </td>
                                    <td><%= IfNullable(group.Paused, ifNull:="N/A", ifTrue:="Paused", ifFalse:="Started") %></td>
                                    <td>
                                        <%= IfNullable(group.Paused,
                                            ifNull:=<span></span>,
                                            ifTrue:=SimpleForm("scheduler.ashx?method=Resume" & entity & "Group&groupName=" + group.Name, "Resume"),
                                            ifFalse:=SimpleForm("scheduler.ashx?method=Pause" & entity & "Group&groupName=" + group.Name, "Pause")) %>
                                    </td>
                                </tr>
                            %>
                        </table>
                    </div>
    End Function

    Public ReadOnly SchedulerJobGroups As Func(Of IEnumerable(Of GroupWithStatus), XElement) = SchedulerEntityGroups("Job")

    Public ReadOnly SchedulerTriggerGroups As Func(Of IEnumerable(Of GroupWithStatus), XElement) = SchedulerEntityGroups("Trigger")

    Public Function GlobalEntityListeners(entity As String) As Func(Of ICollection(Of KeyValuePair(Of String, Type)), XElement)
        Return Function(listeners) _
                <div class="group">
                    <h2>Global <%= entity %> listeners</h2>
                    <%= If(listeners.Count = 0,
                        <span>No <%= entity %> listeners</span>,
                        <table>
                            <tr>
                                <th>Name</th>
                                <th></th>
                            </tr>
                            <%= From listener In listeners
                                Select
                                <tr>
                                    <td><%= listener.Key %></td>
                                    <td>
                                        <%= SimpleForm("scheduler.ashx?method=RemoveGlobal" & entity & "Listener&name=" & listener.Key, "Delete") %>
                                    </td>
                                </tr>
                            %>
                        </table>) %>
                </div>
    End Function

    Public ReadOnly GlobalJobListeners As Func(Of ICollection(Of KeyValuePair(Of String, Type)), XElement) = GlobalEntityListeners("Job")

    Public ReadOnly GlobalTriggerListeners As Func(Of ICollection(Of KeyValuePair(Of String, Type)), XElement) = GlobalEntityListeners("Trigger")

    Public Function IndexPage(schedulerName As String,
                              inStandby As Boolean,
                              listeners As IReadOnlyCollection(Of ISchedulerListener),
                              metadata As SchedulerMetaData,
                              triggerGroups As IReadOnlyCollection(Of GroupWithStatus),
                              jobGroups As IReadOnlyCollection(Of GroupWithStatus),
                              calendars As IReadOnlyCollection(Of KeyValuePair(Of String, String)),
                              jobListeners As IReadOnlyCollection(Of KeyValuePair(Of String, Type)),
                              triggerListeners As IReadOnlyCollection(Of KeyValuePair(Of String, Type))) As XElement
        Return _
        <html>
            <head>
                <title>Quartz.Net Console</title>
                <%= Stylesheet %>
            </head>
            <body>
                <%= SchedulerStatus(schedulerName, inStandby, metadata) %>
                <%= SchedulerListeners(listeners) %>
                <%= SchedulerCalendars(calendars) %>
                <br style="clear:both"/>
                <%= SchedulerJobGroups(jobGroups) %>
                <%= SchedulerTriggerGroups(triggerGroups) %>
                <br style="clear:both"/>
                <%= GlobalJobListeners(jobListeners) %>
                <%= GlobalTriggerListeners(triggerListeners) %>
            </body>
        </html>
    End Function

    Public Function TriggerGroup(group As String, paused As Boolean?, thisUrl As String, highlight As String, triggers As IEnumerable(Of TriggerWithState)) As XElement
        Dim schedulerOp = Function(method As String) "scheduler.ashx?method=" + method +
                              "&groupName=" + group +
                              "&next=" + WebUtility.UrlEncode(thisUrl)
        Return _
        <html>
            <head>
                <title>Quartz.Net console - Trigger group <%= group %></title>
                <%= Stylesheet %>
            </head>
            <body>
                <a href="index.ashx">Index</a>
                <h1>Trigger group <%= group %></h1>
		        Status: <%= IfNullable(paused, ifNull:="N/A", ifTrue:="paused", ifFalse:="started") %>
                <%= IfNullable(paused,
                    ifNull:=<span></span>,
                    ifTrue:=SimpleForm(schedulerOp("ResumeTriggerGroup"), "Resume this trigger group"),
                    ifFalse:=SimpleForm(schedulerOp("PauseTriggerGroup"), "Pause this trigger group")) %>
                <br style="clear:both"/>
                <h2>Triggers</h2>
                <%= TriggerTable(triggers, thisUrl, highlight) %>
            </body>
        </html>
    End Function

    Public Function JobGroup(group As String, paused As Boolean?, highlight As String, thisUrl As String, jobs As IEnumerable(Of JobWithContext)) As XElement
        Dim schedulerOp = Function(method As String) "scheduler.ashx?method=" + method +
                              "&groupName=" + group +
                              "&next=" + WebUtility.UrlEncode(thisUrl)
        Return _
            <html>
                <head>
                    <title>Quartz.Net console - Job group <%= group %></title>
                    <%= Stylesheet %>
                </head>
                <body>
                    <a href="index.ashx">Index</a>
                    <h1>Job group <%= group %></h1>
		            Status: <%= IfNullable(paused, ifNull:="N/A", ifTrue:="paused", ifFalse:="started") %>
                    <%= If(paused,
                        SimpleForm(schedulerOp("ResumeJobGroup"), "Resume this job group"),
                        SimpleForm(schedulerOp("PauseJobGroup"), "Pause this job group")) %>
                    <br style="clear:both"/>
                    <h2>Jobs</h2>
                    <table>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                            <th>Type</th>
                            <th>Durable</th>
                            <th>Persist data after execution</th>
                            <th>Concurrent execution disallowed</th>
                            <th>Requests recovery</th>
                            <th>Running since</th>
                            <th></th>
                        </tr>
                        <%= From j In jobs
                            Let op = Function(method As String) "scheduler.ashx?method=" + method +
                            "&jobName=" + j.Job.Key.Name +
                            "&groupName=" + j.Job.Key.Group +
                            "&next=" + WebUtility.UrlEncode(thisUrl)
                            Select
                            <tr id=<%= j.Job.Key.ToString() %>
                                class=<%= If(highlight = j.Job.Key.ToString(), "highlight", "") %>>
                                <td><%= j.Job.Key.Name %></td>
                                <td><%= j.Job.Description %></td>
                                <td><%= j.Job.JobType %></td>
                                <td><%= YesNo(j.Job.Durable) %></td>
                                <td><%= YesNo(j.Job.PersistJobDataAfterExecution) %></td>
                                <td><%= YesNo(j.Job.ConcurrentExecutionDisallowed) %></td>
                                <td><%= YesNo(j.Job.RequestsRecovery) %></td>
                                <td class="datetime"><%= If(j.JobContext IsNot Nothing, j.JobContext.FireTimeUtc, Nothing) %></td>
                                <td>
                                    <a href=<%= "triggersByJob.ashx?group=" + j.Job.Key.Group + "&job=" + j.Job.Key.Name %>>Triggers</a>
                                    <%= SimpleForm(op("DeleteJob"), "Delete") %>
                                    <%= SimpleForm(op("PauseJob"), "Pause") %>
                                    <%= SimpleForm(op("ResumeJob"), "Resume") %>
                                    <%= SimpleForm(op("TriggerJob"), "Trigger") %>
                                    <%= If(j.Interruptible, SimpleForm(op("Interrupt"), "Interrupt"), Nothing) %>
                                </td>
                            </tr>
                        %>
                    </table>
                </body>
            </html>
    End Function

    Public Function TriggersByJob(model As TriggersByJobModel) As XElement
        Return _
<html>
    <head>
        <title>Quartz.Net console - Triggers for job <%= model.Group %>.<%= model.Job %></title>
        <%= Stylesheet %>
    </head>
    <body>
        <a href="index.ashx">Index</a>
        <h1>Triggers for job <%= model.Group %>.<%= model.Job %></h1>
        <br style="clear:both"/>
        <h2>Triggers</h2>
        <%= TriggerTable(model.Triggers, model.ThisUrl, model.Highlight) %>
    </body>
</html>
    End Function

    Public Function TriggerTable(triggers As IEnumerable(Of TriggerWithState), thisUrl As String, highlight As String) As XElement
        Return _
            If(triggers Is Nothing,
               <span>Not available</span>,
               <table>
                   <tr>
                       <th>Name</th>
                       <th>Description</th>
                       <th>Priority</th>
                       <th>Job group</th>
                       <th>Job name</th>
                       <th>Start time UTC</th>
                       <th>End time UTC</th>
                       <th>Final fire time UTC</th>
                       <th>Next fire time UTC</th>
                       <th>Repeat count</th>
                       <th>Repeat interval</th>
                       <th>Times triggered</th>
                       <th>Cron</th>
                       <th>Calendar</th>
                       <th>State</th>
                       <th></th>
                   </tr>
                   <%= From tr In triggers
                    Let trigger = tr.Trigger
                    Let high = highlight = trigger.Key.ToString()
                    Let simpleTrigger = TryCast(trigger, SimpleTriggerImpl)
                    Let cronTrigger = TryCast(trigger, CronTriggerImpl)
                    Let op = Function(method As String) "scheduler.ashx?method=" & method &
                       "&triggerName=" + trigger.Key.Name +
                       "&groupName=" + trigger.Key.Group +
                       "&next=" + WebUtility.UrlEncode(thisUrl)
                    Select
                    <tr id=<%= trigger.Key.ToString() %>
                        class=<%= If(highlight = trigger.Key.ToString(), "highlight", "") %>>
                        <td><%= trigger.Key.Name %></td>
                        <td><%= trigger.Description %></td>
                        <td><%= trigger.Priority %></td>
                        <td>
                            <a href=<%= "jobGroup.ashx?group=" + trigger.JobKey.Group %>><%= trigger.JobKey.Group %></a>
                        </td>
                        <td>
                            <a href=<%= "jobGroup.ashx?group=" + trigger.JobKey.Group +
                                        "&highlight=" + trigger.JobKey.Group + "." + trigger.JobKey.Name +
                                        "#" + trigger.JobKey.Group + "." + trigger.JobKey.Name %>>
                                <%= trigger.JobKey.Group %>
                            </a>
                        </td>
                        <td class="datetime"><%= trigger.StartTimeUtc %></td>
                        <td class="datetime"><%= trigger.EndTimeUtc %></td>
                        <td class="datetime"><%= trigger.FinalFireTimeUtc %></td>
                        <td class="datetime"><%= trigger.GetNextFireTimeUtc() %></td>
                        <td><%= If(simpleTrigger IsNot Nothing, simpleTrigger.RepeatCount.ToString, "") %></td>
                        <td><%= If(simpleTrigger IsNot Nothing, simpleTrigger.RepeatInterval.ToString, "") %></td>
                        <td><%= If(simpleTrigger IsNot Nothing, simpleTrigger.TimesTriggered.ToString, "") %></td>
                        <td><%= If(cronTrigger IsNot Nothing, X.SpacesToNbsp(cronTrigger.CronExpressionString), "") %></td>
                        <td><%= trigger.CalendarName %></td>
                        <td><%= tr.State %></td>
                        <td>
                            <%= If(tr.IsPaused,
                                SimpleForm(op("ResumeTrigger"), "Resume"),
                                SimpleForm(op("PauseTrigger"), "Pause")) %>

                            <%= SimpleForm(op("UnscheduleJob"), "Delete") %>
                        </td>
                    </tr> %>
               </table>)
    End Function

End Module
