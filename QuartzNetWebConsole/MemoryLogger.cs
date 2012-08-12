using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Quartz;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole {
    /// <summary>
    /// Fixed-capacity in-memory logger. 
    /// </summary>
    public class MemoryLogger : AbstractLogger {
        private readonly LimitedList<LogEntry> entries;
        private readonly string partialQuartzConsoleUrl;

        public MemoryLogger(int capacity, string partialQuartzConsoleUrl) : this(capacity) {
            this.partialQuartzConsoleUrl = partialQuartzConsoleUrl;
        }

        public MemoryLogger(int capacity) {
            entries = new LimitedList<LogEntry>(capacity);
        }

        public override void JobScheduled(ITrigger trigger) {
            var desc = string.Format("Job {0} scheduled with trigger {1}", DescribeJob(trigger.JobKey.Group, trigger.JobKey.Name), Describe(trigger));
            entries.Add(new LogEntry(desc));
        }

        public override void TriggerFinalized(ITrigger trigger) {
            entries.Add(new LogEntry("Trigger finalized: " + Describe(trigger)));
        }

        public override void SchedulerError(string msg, SchedulerException cause) {
            entries.Add(new LogEntry(string.Format("Scheduler error: <pre>{0}</pre><br/><pre>{1}</pre>",
                HttpUtility.HtmlEncode(msg),
                HttpUtility.HtmlEncode(cause.ToString()))));
        }

        public override void SchedulerShutdown() {
            entries.Add(new LogEntry("Scheduler shutdown"));
        }

        public override void JobToBeExecuted(IJobExecutionContext context) {
            entries.Add(new LogEntry("Job to be executed: " + Describe(context)));
        }

        public override void JobExecutionVetoed(IJobExecutionContext context) {
            entries.Add(new LogEntry("Job execution vetoed: " + Describe(context)));
        }

        public override void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException) {
            var description = "Job was executed: " + Describe(context);
            if (jobException != null)
                description += string.Format("<br/>with exception: <pre>{0}</pre>", HttpUtility.HtmlEncode(jobException.ToString()));
            entries.Add(new LogEntry(description));
        }

        public override void TriggerFired(ITrigger trigger, IJobExecutionContext context) {
            entries.Add(new LogEntry("Job fired: " + Describe(context)));
        }

        public override void TriggerMisfired(ITrigger trigger) {
            entries.Add(new LogEntry("Job misfired: " + Describe(trigger)));
        }

        public override void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode) {
            entries.Add(new LogEntry("Job complete: " + Describe(context)));
        }

        private string DescribeJob(string group, string name) {
            if (group == null && name == null)
                return "All";
            return string.Format("{0}.{1}", LinkJobGroup(group), LinkJob(group, name));
        }

        private string Link(string href, string text) {
            return string.Format("<a href='{0}{1}'>{2}</a>", partialQuartzConsoleUrl, href, text);
        }

        private string LinkTriggerGroup(string group) {
            return Link(string.Format("triggerGroup.ashx?group={0}", group), group);
        }

        private string LinkJobGroup(string group) {
            return Link(string.Format("jobGroup.ashx?group={0}", group), group);
        }

        private string LinkTrigger(string group, string name) {
            return Link(string.Format("triggerGroup.ashx?group={0}&amp;highlight={0}.{1}#{0}.{1}", group, name), name);
        }

        private string LinkJob(string group, string name) {
            return Link(string.Format("jobGroup.ashx?group={0}&amp;highlight={0}.{1}#{0}.{1}", group, name), name);
        }

        private string Describe(ITrigger trigger) {
            return string.Format("{0}.{1}", LinkTriggerGroup(trigger.Key.Group), LinkTrigger(trigger.Key.Group, trigger.Key.Name));
        }

        private string Describe(IJobExecutionContext context) {
            var job = context.JobDetail;
            return string.Format("{0}.{1} (trigger {2})", LinkJobGroup(job.Key.Group), LinkJob(job.Key.Group, job.Key.Name), Describe(context.Trigger));
        }

        private string DescribeTrigger(string group, string name) {
            if (group == null && name == null)
                return "All";
            return string.Format("{0}.{1}", LinkTriggerGroup(group), LinkTrigger(group, name));
        }


        public override IEnumerator<LogEntry> GetEnumerator() {
            return entries.GetEnumerator();
        }

        public override Expression Expression {
            get { return entries.AsQueryable().Expression; }
        }

        public override Type ElementType {
            get { return typeof (LogEntry); }
        }

        public override IQueryProvider Provider {
            get { return entries.AsQueryable().Provider; }
        }
    }
}