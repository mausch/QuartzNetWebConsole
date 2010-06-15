using System;
using System.Collections;
using System.Collections.Generic;
using Quartz;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole {
    public class MemoryLogger : ILogger {
        private readonly LimitedList<LogEntry> entries;
        private readonly string partialQuartzConsoleUrl;

        public MemoryLogger(int capacity, string partialQuartzConsoleUrl): this(capacity) {
            this.partialQuartzConsoleUrl = partialQuartzConsoleUrl;
        }

        public MemoryLogger(int capacity) {
            entries = new LimitedList<LogEntry>(capacity);
        }

        public void JobScheduled(Trigger trigger) {
            var desc = string.Format("Job {0}.{1} scheduled with trigger {2}", LinkJobGroup(trigger.JobGroup), trigger.JobName, Describe(trigger));
            entries.Add(new LogEntry(desc));
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

        private string Describe(Trigger trigger) {
            var cronTrigger = trigger as CronTrigger;
            var cronDesc = cronTrigger == null ? null : cronTrigger.CronExpressionString;

            var simpleTrigger = trigger as SimpleTrigger;
            var simpleDesc = simpleTrigger == null ? null : string.Format("interval {0}, repeat count {1}", simpleTrigger.RepeatInterval, simpleTrigger.RepeatCount);
            return string.Format("{0}.{1} ({2}) {3}{4}", LinkTriggerGroup(trigger.Group), trigger.Name, trigger.GetType(), cronDesc, simpleDesc);
        }

        public void JobUnscheduled(string triggerName, string triggerGroup) {
            entries.Add(new LogEntry(string.Format("Trigger removed: {0}.{1}", LinkTriggerGroup(triggerGroup), triggerName)));
        }

        public void TriggerFinalized(Trigger trigger) {
            entries.Add(new LogEntry(string.Format("Trigger finalized: {0}", Describe(trigger))));
        }

        public void TriggersPaused(string triggerName, string triggerGroup) {
            entries.Add(new LogEntry(string.Format("Trigger paused: {0}.{1}", LinkTriggerGroup(triggerGroup), triggerName)));
        }

        public void TriggersResumed(string triggerName, string triggerGroup) {
            entries.Add(new LogEntry(string.Format("Trigger resumed: {0}.{1}", LinkTriggerGroup(triggerGroup), triggerName)));
        }

        public void JobsPaused(string jobName, string jobGroup) {
            entries.Add(new LogEntry(string.Format("Job paused: {0}.{1}", LinkJobGroup(jobGroup), jobName)));
        }

        public void JobsResumed(string jobName, string jobGroup) {
            entries.Add(new LogEntry(string.Format("Job resumed: {0}.{1}", LinkJobGroup(jobGroup), jobName)));
        }

        public void SchedulerError(string msg, SchedulerException cause) {
            entries.Add(new LogEntry(string.Format("Scheduler error: {0}\n{1}", msg, cause)));
        }

        public void SchedulerShutdown() {
            entries.Add(new LogEntry("Scheduler shutdown"));
        }

        public void JobToBeExecuted(JobExecutionContext context) {
            entries.Add(new LogEntry(string.Format("Job to be executed: {0}", Describe(context))));
        }

        private string Describe(JobExecutionContext context) {
            return string.Format("{0}.{1} (trigger {2})", LinkJobGroup(context.JobDetail.Group), context.JobDetail.Name, Describe(context.Trigger));
        }

        public void JobExecutionVetoed(JobExecutionContext context) {
            entries.Add(new LogEntry(string.Format("Job execution vetoed: {0}", Describe(context))));
        }

        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException) {
            var description = string.Format("Job was executed: {0}", Describe(context));
            if (jobException != null)
                description += "\nwith exception: " + jobException;
            entries.Add(new LogEntry(description));
        }

        public void TriggerFired(Trigger trigger, JobExecutionContext context) {
            entries.Add(new LogEntry(string.Format("Trigger fired: {0}", Describe(context))));
        }

        public bool VetoJobExecution(Trigger trigger, JobExecutionContext context) {
            return false;
        }

        public void TriggerMisfired(Trigger trigger) {
            entries.Add(new LogEntry(string.Format("Trigger misfired: {0}", Describe(trigger))));
        }

        public void TriggerComplete(Trigger trigger, JobExecutionContext context, SchedulerInstruction triggerInstructionCode) {
            entries.Add(new LogEntry(string.Format("Trigger complete: {0}", Describe(context))));
        }

        string ITriggerListener.Name {
            get { return GetType().Name; }
        }

        string IJobListener.Name {
            get { return GetType().Name; }
        }

        public IEnumerator<LogEntry> GetEnumerator() {
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}