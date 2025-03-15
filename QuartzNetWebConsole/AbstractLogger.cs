using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole {
    public abstract class AbstractLogger : ILogger {
        
        public virtual void JobExecutionVetoed(IJobExecutionContext context) {
        }
        
        public virtual void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode) {
        }

        public virtual Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(false);
        }

        public virtual Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        string IJobListener.Name {
            get { return "QuartzNetWebConsole.Logger"; }
        }

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
        public abstract void Add(string msg);

        public virtual Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggersPaused(string? triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task TriggersResumed(string? triggerGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobsPaused(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task JobsResumed(string jobGroup, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulerError(string msg, SchedulerException cause,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulerInStandbyMode(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulerStarted(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulerStarting(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulerShutdown(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulerShuttingdown(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public virtual Task SchedulingDataCleared(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}