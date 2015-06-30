using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using QuartzNetWebConsole.Utils;

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

        public delegate Task AppFunc(IDictionary<string, object> env);

        public static Func<AppFunc, AppFunc> Owin(Func<IScheduler> scheduler) {
            Setup.Scheduler = scheduler;
            return app => env => {
                var uri = env.GetOwinUri();
                var response =
                    Routing.Routes
                        .Where(x => uri.AbsolutePath.Split('.')[0].EndsWith(x.Key, StringComparison.InvariantCultureIgnoreCase))
                        .Select(r => r.Value(uri).EvaluateResponse())
                        .FirstOrDefault();
                return response == null ? app(env) : response(env);
            };
        }
    }
}