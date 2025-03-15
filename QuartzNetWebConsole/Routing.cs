using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuartzNetWebConsole.Controllers;
using QuartzNetWebConsole.Utils;

using Route = System.Collections.Generic.KeyValuePair<string, System.Func<QuartzNetWebConsole.Utils.RelativeUri, System.Threading.Tasks.Task<QuartzNetWebConsole.Utils.Response>>>;

namespace QuartzNetWebConsole {
    public class Routing {

        private static ISchedulerWrapper GetSchedulerWrapper() {
            return new SchedulerWrapper(Setup.Scheduler());
        }

        public static readonly IReadOnlyList<Route> Routes =
            new[] {
                Route("jobgroup", url => JobGroupController.Execute(url, GetSchedulerWrapper)),
                Route("index", _ => IndexController.Execute(GetSchedulerWrapper)),
                Route("log", LogController.Execute),
                Route("scheduler", ctx => SchedulerController.Execute(ctx, GetSchedulerWrapper)),
                Route("static", StaticController.Execute),
                Route("triggerGroup", ctx => TriggerGroupController.Execute(ctx, GetSchedulerWrapper)),
                Route("triggersByJob", ctx => TriggersByJobController.Execute(ctx, GetSchedulerWrapper)),

            };

        private static Route Route(string path, Func<RelativeUri, Task<Response>> action) {
            return new Route(path, action);
        }

    }
}
