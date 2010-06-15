using System;
using Quartz;

namespace QuartzNetWebConsole {
    public static class Setup {
        public static Func<IScheduler> Scheduler { get; set; }

        public static void SetLogger(ILogger logger) {
            var scheduler = Scheduler();
            if (Logger != null) {
                scheduler.RemoveGlobalJobListener(logger);
                scheduler.RemoveGlobalTriggerListener(logger);
                scheduler.RemoveSchedulerListener(logger);
            }
            scheduler.AddGlobalJobListener(logger);
            scheduler.AddGlobalTriggerListener(logger);
            scheduler.AddSchedulerListener(logger);
            Logger = logger;
        }

        public static ILogger Logger { get; private set; }

        static Setup() {
            Scheduler = () => {
                throw new Exception("Define QuartzNetWebConsole.Setup.Scheduler");
            };
        }
    }
}