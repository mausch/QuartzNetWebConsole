using System;
using Quartz;
using Quartz.Impl.Matchers;

namespace QuartzNetWebConsole.Utils {
    public class SchedulerWrapper {
        private readonly IScheduler scheduler;

        public SchedulerWrapper(IScheduler scheduler) {
            this.scheduler = scheduler;
        }

        public void Shutdown() {
            scheduler.Shutdown();
        }

        public void Start() {
            scheduler.Start();
        }

        public void Standby() {
            scheduler.Standby();
        }

        public void PauseAll() {
            scheduler.PauseAll();
        }

        public void ResumeAll() {
            scheduler.ResumeAll();
        }

        public void ResumeJobGroup(string groupName) {
            scheduler.ResumeJobs(GroupMatcher<JobKey>.GroupEquals(groupName));
        }

        public void ResumeTriggerGroup(string groupName) {
            scheduler.ResumeTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
        }

        public void PauseJobGroup(string groupName) {
            scheduler.PauseJobs(GroupMatcher<JobKey>.GroupEquals(groupName));
        }

        public void PauseTriggerGroup(string groupName) {
            scheduler.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
        }

        public void RemoveGlobalJobListener(string name) {
            scheduler.ListenerManager.RemoveJobListener(name);
        }

        public void RemoveGlobalTriggerListener(string name) {
            scheduler.ListenerManager.RemoveTriggerListener(name);
        }

        public void DeleteJob(string jobName, string groupName) {
            scheduler.DeleteJob(new JobKey(jobName, groupName));
        }

        public void PauseJob(string jobName, string groupName) {
            scheduler.PauseJob(new JobKey(jobName, groupName));
        }

        public void ResumeJob(string jobName, string groupName) {
            scheduler.ResumeJob(new JobKey(jobName, groupName));
        }

        public void TriggerJob(string jobName, string groupName) {
            scheduler.TriggerJob(new JobKey(jobName, groupName));
        }

        public void TriggerJobWithVolatileTrigger(string jobName, string groupName) {
            throw new NotSupportedException();
        }

        public void Interrupt(string jobName, string groupName) {
            scheduler.Interrupt(new JobKey(jobName, groupName));
        }

        public void ResumeTrigger(string triggerName, string groupName) {
            scheduler.ResumeTrigger(new TriggerKey(triggerName, groupName));
        }

        public void PauseTrigger(string triggerName, string groupName) {
            scheduler.PauseTrigger(new TriggerKey(triggerName, groupName));
        }

        public void UnscheduleJob(string triggerName, string groupName) {
            scheduler.UnscheduleJob(new TriggerKey(triggerName, groupName));
        }
    }
}