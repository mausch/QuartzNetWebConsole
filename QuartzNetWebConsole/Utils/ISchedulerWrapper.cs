using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
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
        Task<IReadOnlyCollection<TriggerKey>> GetTriggerKeys(GroupMatcher<TriggerKey> matcher);
        Task<IReadOnlyCollection<JobKey>> GetJobKeys(GroupMatcher<JobKey> matcher);
        Task<bool> IsJobGroupPaused(string groupName);
        Task<bool> IsTriggerGroupPaused(string groupName);
        Task<IReadOnlyCollection<IJobExecutionContext>> GetCurrentlyExecutingJobs();
        Task<IJobDetail> GetJobDetail(JobKey key);
        Task<IReadOnlyCollection<string>> GetTriggerGroupNames();
        Task<IReadOnlyCollection<string>> GetJobGroupNames();
        Task<IReadOnlyCollection<string>> GetCalendarNames();
        IListenerManager ListenerManager { get; }
        string SchedulerName { get; }
        bool InStandbyMode { get; }
        Task<ICalendar> GetCalendar(string name);
        Task<SchedulerMetaData> GetMetaData();
        Task<IReadOnlyCollection<ITrigger>> GetTriggersOfJob(JobKey jobKey);
        Task<ITrigger> GetTrigger(TriggerKey triggerKey);
        Task<TriggerState> GetTriggerState(TriggerKey triggerKey);
    }
}