using System;
using Quartz;

namespace QuartzNetWebConsole {
    public static class Setup {
        /// <summary>
        /// What Quartz.NET scheduler should the web console use.
        /// </summary>
        public static Func<IScheduler> Scheduler { get; set; }

        private static ILogger logger;

        /// <summary>
        /// Optional logger to attach to the web console
        /// </summary>
        public static ILogger Logger {
            get { return logger; }
            set {
                var scheduler = Scheduler();
                if (logger != null) {
                    IJobListener jobListener = logger;
                    ITriggerListener triggerListener = logger;
                    scheduler.ListenerManager.RemoveJobListener(jobListener.Name);
                    scheduler.ListenerManager.RemoveTriggerListener(triggerListener.Name);
                    scheduler.ListenerManager.RemoveSchedulerListener(logger);
                }
                if (value != null) {
                    scheduler.ListenerManager.AddJobListener(value);
                    //scheduler.ListenerManager.AddJobListenerMatcher()
                    scheduler.ListenerManager.AddTriggerListener(value);
                    scheduler.ListenerManager.AddSchedulerListener(value);
                }
                logger = value;
            }
        }

        static Setup() {
            Scheduler = () => { throw new Exception("Define QuartzNetWebConsole.Setup.Scheduler"); };
        }
    }
}