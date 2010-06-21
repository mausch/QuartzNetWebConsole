using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Quartz;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole {
    public class MemoryLogger : AbstractLogger {
        private readonly LimitedList<LogEntry> entries;
        private readonly string partialQuartzConsoleUrl;

        public MemoryLogger(int capacity, string partialQuartzConsoleUrl) : this(capacity) {
            this.partialQuartzConsoleUrl = partialQuartzConsoleUrl;
        }

        public MemoryLogger(int capacity) {
            entries = new LimitedList<LogEntry>(capacity);
        }

        public override void JobScheduled(Trigger trigger) {
            var desc = string.Format("Job {0} scheduled with trigger {1}", DescribeJob(trigger.JobGroup, trigger.JobName), Describe(trigger));
            entries.Add(new LogEntry(desc));
        }

        public override void JobUnscheduled(string triggerName, string triggerGroup) {
            entries.Add(new LogEntry("Trigger removed: " + DescribeTrigger(triggerGroup, triggerName)));
        }

        public override void TriggerFinalized(Trigger trigger) {
            entries.Add(new LogEntry("Trigger finalized: " + Describe(trigger)));
        }

        public override void TriggersPaused(string triggerName, string triggerGroup) {
            entries.Add(new LogEntry("Trigger paused: " + DescribeTrigger(triggerGroup, triggerName)));
        }

        public override void TriggersResumed(string triggerName, string triggerGroup) {
            entries.Add(new LogEntry("Trigger resumed: " + DescribeTrigger(triggerGroup, triggerName)));
        }

        public override void JobsPaused(string jobName, string jobGroup) {
            entries.Add(new LogEntry("Job paused: " + DescribeJob(jobGroup, jobName)));
        }

        public override void JobsResumed(string jobName, string jobGroup) {
            entries.Add(new LogEntry("Job resumed: " + DescribeJob(jobGroup, jobName)));
        }

        public override void SchedulerError(string msg, SchedulerException cause) {
            entries.Add(new LogEntry(string.Format("Scheduler error: {0}\n{1}", msg, cause)));
        }

        public override void SchedulerShutdown() {
            entries.Add(new LogEntry("Scheduler shutdown"));
        }

        public override void JobToBeExecuted(JobExecutionContext context) {
            entries.Add(new LogEntry("Job to be executed: " + Describe(context)));
        }

        public override void JobExecutionVetoed(JobExecutionContext context) {
            entries.Add(new LogEntry("Job execution vetoed: " + Describe(context)));
        }

        public override void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException) {
            var description = "Job was executed: " + Describe(context);
            if (jobException != null)
                description += "\nwith exception: " + jobException;
            entries.Add(new LogEntry(description));
        }

        public override void TriggerFired(Trigger trigger, JobExecutionContext context) {
            entries.Add(new LogEntry("Job fired: " + Describe(context)));
        }

        public override void TriggerMisfired(Trigger trigger) {
            entries.Add(new LogEntry("Job misfired: " + Describe(trigger)));
        }

        public override void TriggerComplete(Trigger trigger, JobExecutionContext context, SchedulerInstruction triggerInstructionCode) {
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
            return Link(string.Format("triggerGroup.ashx?group={0}&highlight={0}.{1}#{0}.{1}", group, name), name);
        }

        private string LinkJob(string group, string name) {
            return Link(string.Format("jobGroup.ashx?group={0}&highlight={0}.{1}#{0}.{1}", group, name), name);
        }

        private string Describe(Trigger trigger) {
            return string.Format("{0}.{1}", LinkTriggerGroup(trigger.Group), LinkTrigger(trigger.Group, trigger.Name));
        }

        private string Describe(JobExecutionContext context) {
            var job = context.JobDetail;
            return string.Format("{0}.{1} (trigger {2})", LinkJobGroup(job.Group), LinkJob(job.Group, job.Name), Describe(context.Trigger));
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