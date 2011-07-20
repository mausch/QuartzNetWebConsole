using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole {
    public abstract class AbstractLogger : ILogger {
        public virtual void JobScheduled(Trigger trigger) {}

        public virtual void JobUnscheduled(string triggerName, string triggerGroup) {}

        public virtual void TriggerFinalized(Trigger trigger) {}

        public virtual void TriggersPaused(string triggerName, string triggerGroup) {}

        public virtual void TriggersResumed(string triggerName, string triggerGroup) {}

        public virtual void JobsPaused(string jobName, string jobGroup) {}

        public virtual void JobsResumed(string jobName, string jobGroup) {}

        public virtual void SchedulerError(string msg, SchedulerException cause) {}

        public virtual void SchedulerShutdown() {}

        public virtual void JobToBeExecuted(JobExecutionContext context) {}

        public virtual void JobExecutionVetoed(JobExecutionContext context) {}

        public virtual void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException) {}

        string IJobListener.Name {
            get { return "QuartzNetWebConsole.Logger"; }
        }

        public virtual void TriggerFired(Trigger trigger, JobExecutionContext context) {}

        public bool VetoJobExecution(Trigger trigger, JobExecutionContext context) {
            return false;
        }

        public virtual void TriggerMisfired(Trigger trigger) {}

        public virtual void TriggerComplete(Trigger trigger, JobExecutionContext context, SchedulerInstruction triggerInstructionCode) {}

        string ITriggerListener.Name {
            get { return "QuartzNetWebConsole.Logger"; }
        }

        public abstract IEnumerator<LogEntry> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public abstract Expression Expression { get; }
        public abstract Type ElementType { get; }
        public abstract IQueryProvider Provider { get; }
    }
}