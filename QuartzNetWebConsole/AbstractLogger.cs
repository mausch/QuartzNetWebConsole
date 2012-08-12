using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole {
    public abstract class AbstractLogger : ILogger {
        public virtual void JobToBeExecuted(IJobExecutionContext context) {
            throw new NotImplementedException();
        }

        public virtual void JobExecutionVetoed(IJobExecutionContext context) {
            throw new NotImplementedException();
        }

        public virtual void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException) {
            throw new NotImplementedException();
        }

        public virtual void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode) {
            throw new NotImplementedException();
        }

        string IJobListener.Name {
            get { return "QuartzNetWebConsole.Logger"; }
        }

        public virtual void TriggerFired(ITrigger trigger, IJobExecutionContext context) {
            throw new NotImplementedException();
        }

        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context) {
            return false;
        }

        public virtual void TriggerMisfired(ITrigger trigger) {
            throw new NotImplementedException();
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
        public virtual void JobScheduled(ITrigger trigger) {
            
        }

        public virtual void JobUnscheduled(TriggerKey triggerKey) {
        }

        public virtual void TriggerFinalized(ITrigger trigger) {
        }

        public virtual void TriggerPaused(TriggerKey triggerKey) {
        }

        public virtual void TriggersPaused(string triggerGroup) {
        }

        public virtual void TriggerResumed(TriggerKey triggerKey) {
        }

        public virtual void TriggersResumed(string triggerGroup) {
        }

        public virtual void JobAdded(IJobDetail jobDetail) {
        }

        public virtual void JobDeleted(JobKey jobKey) {
        }

        public virtual void JobPaused(JobKey jobKey) {
        }

        public virtual void JobsPaused(string jobGroup) {
        }

        public virtual void JobResumed(JobKey jobKey) {
        }

        public virtual void JobsResumed(string jobGroup) {
        }

        public virtual void SchedulerError(string msg, SchedulerException cause) {
        }

        public virtual void SchedulerInStandbyMode() {
        }

        public virtual void SchedulerStarted() {
        }

        public virtual void SchedulerShutdown() {
        }

        public virtual void SchedulerShuttingdown() {
        }

        public virtual void SchedulingDataCleared() {
        }
    }
}