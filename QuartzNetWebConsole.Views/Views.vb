Imports MiniMVC
Imports Quartz
Imports System.Web

Public Module Views

    Public Function Log(ByVal logs As IEnumerable(Of LogEntry), ByVal pagination As PaginationInfo, ByVal thisUrl As String) As XElement
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
                        <%= From kv In Index(logs)
                            Let odd = kv.Key Mod 2 = 0
                            Let e = kv.Value
                            Select
                            <tr class=<%= If(Not odd, "alt", "") %>>
                                <td><%= e.Timestamp %></td>
                                <td><%= X.Raw(e.Description) %></td>
                            </tr>
                        %>
                    </table>
                    <br/>
                    <div class="pagination">
                    Showing <%= pagination.FirstItemIndex %> - <%= pagination.LastItemIndex %> of <%= pagination.TotalItemCount %>

                        <%= If(pagination.HasPrevPage,
                            <a href=<%= pagination.PrevPageUrl %>><%= laquo %> Previous</a>,
                            <span class="disabledPage"><%= laquo %> Previous</span>) %>

                        <%= From p In pagination.Pages
                            Select
                            If(p = pagination.CurrentPage,
                            <span class="currentPage"><%= p %></span>,
                            <a href=<%= pagination.PageUrlFor(p) %>><%= p %></a>) %>

                        <%= If(pagination.HasNextPage,
                            <a href=<%= pagination.NextPageUrl %>><%= raquo %> Next</a>,
                            <span class="disabledPage"><%= raquo %> Next</span>) %>

                    </div>
                </body>
            </html>
    End Function

    Public Function LogRSS(ByVal thisUrl As String, ByVal logs As IEnumerable(Of LogEntry)) As XElement
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

    Public Function IndexPage(ByVal scheduler As IScheduler,
                              ByVal metadata As SchedulerMetaData,
                              ByVal triggerGroups As IEnumerable(Of GroupWithStatus),
                              ByVal jobGroups As IEnumerable(Of GroupWithStatus),
                              ByVal calendars As ICollection(Of KeyValuePair(Of String, String)),
                              ByVal jobListeners As ICollection(Of KeyValuePair(Of String, Type)),
                              ByVal triggerListeners As ICollection(Of KeyValuePair(Of String, Type))) As XElement
        Return _
        <html>
            <head>
                <title>Quartz.Net Console</title>
                <%= Stylesheet %>
            </head>
            <body>
                <div class="group">
                    <h2>Scheduler name: <%= scheduler.SchedulerName %></h2>
                    <div style="float: left">
				        Job store: <%= metadata.JobStoreType %><br/>
				        Supports persistence: <%= YesNo(metadata.JobStoreSupportsPersistence) %><br/>
				        Number of jobs executed: <%= metadata.NumJobsExecuted %><br/>
				        Running since: <%= metadata.RunningSince %><br/>
				        Status: <%= If(scheduler.InStandbyMode, "stand-by", "running") %>
                        <br/>
                        <a href="log.ashx">View log</a>
                    </div>
                    <div style="float: right">
                        <%= SimpleForm("scheduler.ashx?method=Shutdown", "Shut down") %>
                        <%= If(scheduler.InStandbyMode,
                            SimpleForm("scheduler.ashx?method=Start", "Start"),
                            SimpleForm("scheduler.ashx?method=Standby", "Stand by")) %>
                        <%= SimpleForm("scheduler.ashx?method=PauseAll", "Pause all triggers") %>
                        <%= SimpleForm("scheduler.ashx?method=ResumeAll", "Resume all triggers") %>
                    </div>
                </div>
                <div class="group">
                    <h2>Scheduler listeners</h2>
                    <table>
                        <tr>
                            <th>Type</th>
                        </tr>
                        <%= From kv In Index(scheduler.SchedulerListeners.Cast(Of Object))
                            Let odd = kv.Key Mod 2 = 0
                            Select
                            <tr class=<%= If(Not odd, "alt", "") %>>
                                <td><%= kv.Value.GetType() %></td>
                            </tr>
                        %>
                    </table>
                </div>
                <div class="group">
                    <h2>Calendars</h2>
                    <%= If(calendars.Count = 0,
                        <span>No calendars</span>,
                        <table>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                            </tr>
                            <%= From kv In Index(calendars)
                                Let odd = kv.Key Mod 2 = 0
                                Let cal = kv.Value
                                Select
                                <tr class=<%= If(Not odd, "alt", "") %>>
                                    <td><%= cal.Key %></td>
                                    <td><%= cal.Value %></td>
                                </tr>
                            %>
                        </table>) %>
                </div>
                <br style="clear:both"/>
                <div class="group">
                    <h2>Job groups</h2>
                    <table>
                        <tr>
                            <th>Name</th>
                            <th>Status</th>
                            <th></th>
                        </tr>
                        <%= From kv In Index(jobGroups)
                            Let odd = kv.Key Mod 2 = 0
                            Let jobg = kv.Value
                            Select
                            <tr class=<%= If(Not odd, "alt", "") %>>
                                <td>
                                    <a href=<%= "jobGroup.ashx?group=" + jobg.Name %>><%= jobg.Name %></a>
                                </td>
                                <td><%= If(jobg.Paused, "Paused", "Started") %></td>
                                <td>
                                    <%= If(jobg.Paused,
                                        SimpleForm("scheduler.ashx?method=ResumeJobGroup&groupName=" + jobg.Name, "Resume"),
                                        SimpleForm("scheduler.ashx?method=PauseJobGroup&groupName=" + jobg.Name, "Pause")) %>
                                </td>
                            </tr>
                        %>
                    </table>
                </div>
                <div class="group">
                    <h2>Trigger groups</h2>
                    <table>
                        <tr>
                            <th>Name</th>
                            <th>Status</th>
                            <th></th>
                        </tr>
                        <%= From kv In Index(triggerGroups)
                            Let odd = kv.Key Mod 2 = 0
                            Let triggerg = kv.Value
                            Select
                            <tr class=<%= If(Not odd, "alt", "") %>>
                                <td>
                                    <a href=<%= "triggerGroup.ashx?group=" + triggerg.Name %>><%= triggerg.Name %></a>
                                </td>
                                <td>
                                    <%= If(triggerg.Paused, "Paused", "Started") %>
                                </td>
                                <td>
                                    <%= If(triggerg.Paused,
                                        SimpleForm("scheduler.ashx?method=ResumeTriggerGroup&groupName=" + triggerg.Name, "Resume"),
                                        SimpleForm("scheduler.ashx?method=PauseTriggerGroup&groupName=" + triggerg.Name, "Pause")) %>
                                </td>
                            </tr>
                        %>
                    </table>
                </div>
                <br style="clear:both"/>
                <div class="group">
                    <h2>Global job listeners</h2>
                    <%= If(jobListeners.Count = 0,
                        <span>No job listeners</span>,
                        <table>
                            <tr>
                                <th>Name</th>
                                <th></th>
                            </tr>
                            <%= From kv In Index(jobListeners)
                                Let odd = kv.Key Mod 2 = 0
                                Let jobl = kv.Value
                                Select
                                <tr class=<%= If(Not odd, "alt", "") %>>
                                    <td><%= jobl.Key %></td>
                                    <td>
                                        <%= SimpleForm("scheduler.ashx?method=RemoveGlobalJobListener&name=" + jobl.Key, "Delete") %>
                                    </td>
                                </tr>
                            %>
                        </table>) %>
                </div>
                <div class="group">
                    <h2>Global trigger listeners</h2>
                    <%= If(triggerListeners.Count = 0,
                        <span>No trigger listeners</span>,
                        <table>
                            <tr>
                                <th>Name</th>
                                <th></th>
                            </tr>
                            <%= From kv In Index(triggerListeners)
                                Let odd = kv.Key Mod 2 = 0
                                Let triggerl = kv.Value
                                Select
                                <tr class=<%= If(Not odd, "alt", "") %>>
                                    <td><%= triggerl.Key %></td>
                                    <td>
                                        <%= SimpleForm("scheduler.ashx?method=RemoveGlobalTriggerListener&name=" + triggerl.Key, "Delete") %>
                                    </td>
                                </tr>
                            %>
                        </table>) %>
                </div>
            </body>
        </html>
    End Function

    Public Function TriggerGroup(ByVal group As String, ByVal paused As Boolean, ByVal thisUrl As String, ByVal highlight As String, ByVal triggers As IEnumerable(Of TriggerWithState)) As XElement
        Dim schedulerOp = Function(method As String) "scheduler.ashx?method=" + method +
                              "&groupName=" + group +
                              "&next=" + HttpUtility.UrlEncode(thisUrl)
        Return _
        <html>
            <head>
                <title>Quartz.Net console - Trigger group <%= group %></title>
                <%= Stylesheet %>
            </head>
            <body>
                <a href="index.ashx">Index</a>
                <h1>Trigger group <%= group %></h1>
		        Status: <%= If(paused, "paused", "started") %>
                <%= If(paused,
                    SimpleForm(schedulerOp("ResumeTriggerGroup"), "Resume this trigger group"),
                    SimpleForm(schedulerOp("PauseTriggerGroup"), "Pause this trigger group")) %>
                <br style="clear:both"/>
                <h2>Triggers</h2>
                <%= TriggerTable(triggers, thisUrl, highlight) %>
            </body>
        </html>
    End Function

    Public Function JobGroup(ByVal group As String, ByVal paused As Boolean, ByVal highlight As String, ByVal thisUrl As String, ByVal jobs As IEnumerable(Of JobWithContext)) As XElement
        Dim schedulerOp = Function(method As String) "scheduler.ashx?method=" + method +
                              "&groupName=" + group +
                              "&next=" + HttpUtility.UrlEncode(thisUrl)
        Return _
            <html>
                <head>
                    <title>Quartz.Net console - Job group <%= group %></title>
                    <%= Stylesheet %>
                </head>
                <body>
                    <a href="index.ashx">Index</a>
                    <h1>Job group <%= group %></h1>
		            Status: <%= If(paused, "paused", "started") %>
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
                            <th>Stateful</th>
                            <th>Volatile</th>
                            <th>Requests recovery</th>
                            <th>Running since</th>
                            <th>Listeners</th>
                            <th></th>
                        </tr>
                        <%= From kv In Index(jobs)
                            Let odd = kv.Key Mod 2 = 0
                            Let j = kv.Value
                            Let op = Function(method As String) "scheduler.ashx?method=" + method +
                            "&jobName=" + j.Job.Name +
                            "&groupName=" + j.Job.Group +
                            "&next=" + HttpUtility.UrlEncode(thisUrl)
                            Select
                            <tr id=<%= j.Job.FullName %>
                                class=<%= If(highlight = j.Job.FullName, "highlight", "") + " " + If(Not odd, "alt", "") %>>
                                <td><%= j.Job.Name %></td>
                                <td><%= j.Job.Description %></td>
                                <td><%= j.Job.JobType %></td>
                                <td><%= YesNo(j.Job.Durable) %></td>
                                <td><%= YesNo(j.Job.Stateful) %></td>
                                <td><%= YesNo(j.Job.Volatile) %></td>
                                <td><%= YesNo(j.Job.RequestsRecovery) %></td>
                                <td><%= j.JobContext.FireTimeUtc %></td>
                                <td><%= j.Job.JobListenerNames.Length %></td>
                                <td>
                                    <a href=<%= "triggersByJob.ashx?group=" + j.Job.Group + "&job=" + j.Job.Name %>>Triggers</a>
                                    <%= SimpleForm(op("DeleteJob"), "Delete") %>
                                    <%= SimpleForm(op("PauseJob"), "Pause") %>
                                    <%= SimpleForm(op("ResumeJob"), "Resume") %>
                                    <%= SimpleForm(op("TriggerJob"), "Trigger") %>
                                    <%= SimpleForm(op("TriggerJobWithVolatileTrigger"), "Trigger volatile") %>
                                    <%= If(j.Interruptible, SimpleForm(op("Interrupt"), "Interrupt"), Nothing) %>
                                </td>
                            </tr>
                        %>
                    </table>
                </body>
            </html>
    End Function

    Public Function TriggersByJob(ByVal model As TriggersByJobModel) As XElement
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

    Public Function TriggerTable(ByVal triggers As IEnumerable(Of TriggerWithState), ByVal thisUrl As String, ByVal highlight As String) As XElement
        Return _
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
                <%= From kv In Index(triggers)
                    Let odd = kv.Key Mod 2 = 0
                    Let high = highlight = kv.Value.Trigger.FullName
                    Let trigger = kv.Value.Trigger
                    Let simpleTrigger = TryCast(trigger, SimpleTrigger)
                    Let cronTrigger = TryCast(trigger, CronTrigger)
                    Select
                    <tr id=<%= trigger.FullName %>
                        class=<%= If(highlight = trigger.FullName, highlight, "") + " " + If(Not odd, "alt", "") %>>
                        <td><%= trigger.Name %></td>
                        <td><%= trigger.Description %></td>
                        <td><%= trigger.Priority %></td>
                        <td>
                            <a href=<%= "jobGroup.ashx?group=" + trigger.JobGroup %>><%= trigger.JobGroup %></a>
                        </td>
                        <td>
                            <a href=<%= "jobGroup.ashx?group=" + trigger.JobGroup +
                                        "&highlight=" + trigger.JobGroup + "." + trigger.JobName +
                                        "#" + trigger.JobGroup + "." + trigger.JobName %>>
                                <%= trigger.JobGroup %>
                            </a>
                        </td>
                        <td><%= trigger.StartTimeUtc %></td>
                        <td><%= trigger.EndTimeUtc %></td>
                        <td><%= trigger.FinalFireTimeUtc %></td>
                        <td><%= trigger.GetNextFireTimeUtc() %></td>
                        <td><%= If(simpleTrigger IsNot Nothing, simpleTrigger.RepeatCount.ToString, "") %></td>
                        <td><%= If(simpleTrigger IsNot Nothing, simpleTrigger.RepeatInterval.ToString, "") %></td>
                        <td><%= If(simpleTrigger IsNot Nothing, simpleTrigger.TimesTriggered.ToString, "") %></td>
                        <td><%= If(cronTrigger IsNot Nothing, SpacesToNbsp(cronTrigger.CronExpressionString), "") %></td>
                        <td><%= trigger.CalendarName %></td>
                        <td><%= kv.Value.State %></td>
                        <td>
                            <%= If(kv.Value.IsPaused,
                                <form method="post"
                                    action=<%= "scheduler.ashx?method=ResumeTrigger&triggerName=" + trigger.Name +
                                               "&groupName=" + trigger.Group +
                                               "&next=" + HttpUtility.UrlEncode(thisUrl) %>>
                                    <input type="submit" value="Resume"/>
                                </form>,
                                <form method="post"
                                    action=<%= "scheduler.ashx?method=PauseTrigger&triggerName=" + trigger.Name +
                                               "&groupName=" + trigger.Group +
                                               "&next=" + HttpUtility.UrlEncode(thisUrl) %>>
                                    <input type="submit" value="Pause"/>
                                </form>) %>
                            <form method="post"
                                action=<%= "scheduler.ashx?method=UnscheduleJob&triggerName=" + trigger.Name +
                                           "&groupName=" + trigger.Group +
                                           "&next=" + HttpUtility.UrlEncode(thisUrl) %>>
                                <input type="submit" value="Delete"/>
                            </form>
                        </td>
                    </tr> %>
            </table>
    End Function
End Module
