using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;

namespace QuartzNetWebConsole.Utils {
    public class SchedulerWrapper : ISchedulerWrapper {
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

        public async Task<IReadOnlyCollection<TriggerKey>> GetTriggerKeys(GroupMatcher<TriggerKey> matcher)
        {
            return await scheduler.GetTriggerKeys(matcher);
        }

        public async Task<IReadOnlyCollection<JobKey>> GetJobKeys(GroupMatcher<JobKey> matcher)
        {
            return await scheduler.GetJobKeys(matcher);
        }

        public async Task<bool> IsJobGroupPaused(string groupName) {
            try {
                return await scheduler.IsJobGroupPaused(groupName);
            } catch (NotImplementedException) {
                return false;
            }
        }

        public async Task<bool> IsTriggerGroupPaused(string groupName) {
            try {
                return await scheduler.IsTriggerGroupPaused(groupName);
            } catch (NotImplementedException) {
                return false;
            }
        }

        public async Task<IReadOnlyCollection<IJobExecutionContext>> GetCurrentlyExecutingJobs() {
            return await scheduler.GetCurrentlyExecutingJobs();
        }

        public async Task<IJobDetail> GetJobDetail(JobKey key) {
            return await scheduler.GetJobDetail(key);
        }

        public async Task<IReadOnlyCollection<string>> GetTriggerGroupNames() {
            return await scheduler.GetTriggerGroupNames();
        }

        public async Task<IReadOnlyCollection<string>> GetJobGroupNames() {
            return await scheduler.GetJobGroupNames();
        }

        public async Task<IReadOnlyCollection<string>> GetCalendarNames() {
            return await scheduler.GetCalendarNames();
        }

        public IListenerManager ListenerManager {
            get {
                return scheduler.ListenerManager;
            }
        }

        public async Task<ICalendar> GetCalendar(string name) {
            return await scheduler.GetCalendar(name);
        }

        public async Task<SchedulerMetaData> GetMetaData() {
            return await scheduler.GetMetaData();
        }

        public async Task<IReadOnlyCollection<ITrigger>> GetTriggersOfJob(JobKey jobKey) {
            try {
                return await scheduler.GetTriggersOfJob(jobKey);
            } catch (NotImplementedException) {
                return null;
            }
        }

        public async Task<ITrigger> GetTrigger(TriggerKey triggerKey) {
            return await scheduler.GetTrigger(triggerKey);
        }

        public async Task<TriggerState> GetTriggerState(TriggerKey triggerKey) {
            return await scheduler.GetTriggerState(triggerKey);
        }

        public string SchedulerName {
            get {
                return scheduler.SchedulerName;
            }
        }

        public bool InStandbyMode {
            get {
                return scheduler.InStandbyMode;
            }
        }
    }
}