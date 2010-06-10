using System;
using Quartz;

namespace QuartzNetWebConsole {
    public static class Setup {
        public static Func<IScheduler> Scheduler { get; set;}

        static Setup() {
            Scheduler = () => {
                throw new Exception("Define QuartzNetWebConsole.Setup.Scheduler");
            };
        }
    }
}