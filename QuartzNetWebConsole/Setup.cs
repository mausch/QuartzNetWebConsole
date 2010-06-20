using System;
using Quartz;

namespace QuartzNetWebConsole {
    public static class Setup {
        public static Func<IScheduler> Scheduler { get; set; }

        private static ILogger logger;

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