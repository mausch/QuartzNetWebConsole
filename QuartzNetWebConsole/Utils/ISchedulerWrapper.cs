using System.Collections.Generic;
using Quartz;
using Quartz.Collection;
using Quartz.Impl.Matchers;

namespace QuartzNetWebConsole.Utils {
    public interface ISchedulerWrapper {
        void Shutdown();
        void Start();
        void Standby();
        void PauseAll();
        void ResumeAll();
        void ResumeJobGroup(string groupName);
        void ResumeTriggerGroup(string groupName);
        void PauseJobGroup(string groupName);
        void PauseTriggerGroup(string groupName);
        void RemoveGlobalJobListener(string name);
        void RemoveGlobalTriggerListener(string name);
        void DeleteJob(string jobName, string groupName);
        void PauseJob(string jobName, string groupName);
        void ResumeJob(string jobName, string groupName);
        void TriggerJob(string jobName, string groupName);
        void Interrupt(string jobName, string groupName);
        void ResumeTrigger(string triggerName, string groupName);
        void PauseTrigger(string triggerName, string groupName);
        void UnscheduleJob(string triggerName, string groupName);
        bool? IsJobGroupPaused(string groupName);
        bool? IsTriggerGroupPaused(string groupName);
        IEnumerable<IJobExecutionContext> GetCurrentlyExecutingJobs();
        Quartz.Collection.ISet<JobKey> GetJobKeys(GroupMatcher<JobKey> key);
        IJobDetail GetJobDetail(JobKey key);
        IEnumerable<string> GetTriggerGroupNames();
        IEnumerable<string> GetJobGroupNames();
        IEnumerable<string> GetCalendarNames();
        IListenerManager ListenerManager { get; }
        string SchedulerName { get; }
        bool InStandbyMode { get; }
        ICalendar GetCalendar(string name);
        SchedulerMetaData GetMetaData();
        IEnumerable<ITrigger> GetTriggersOfJob(JobKey jobKey);
        Quartz.Collection.ISet<TriggerKey> GetTriggerKeys(GroupMatcher<TriggerKey> matcher);
        ITrigger GetTrigger(TriggerKey triggerKey);
        TriggerState GetTriggerState(TriggerKey triggerKey);
    }
}