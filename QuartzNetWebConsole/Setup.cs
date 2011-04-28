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
                    scheduler.RemoveGlobalJobListener(logger);
                    scheduler.RemoveGlobalTriggerListener(logger);
                    scheduler.RemoveSchedulerListener(logger);
                }
                if (value != null) {
                    scheduler.AddGlobalJobListener(value);
                    scheduler.AddGlobalTriggerListener(value);
                    scheduler.AddSchedulerListener(value);
                }
                logger = value;
            }
        }

        static Setup() {
            Scheduler = () => { throw new Exception("Define QuartzNetWebConsole.Setup.Scheduler"); };
        }
    }
}