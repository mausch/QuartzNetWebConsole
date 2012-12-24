using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Controllers;
using QuartzNetWebConsole.Utils;
using Route = System.Collections.Generic.KeyValuePair<string, System.Action<System.Web.HttpContextBase>>;

namespace QuartzNetWebConsole {
    public class ControllerFactory : HttpHandlerFactory {
        public override IHttpHandler GetHandler(HttpContextBase context) {
            var lastUrlSegment = context.Request.Url.Segments.Last().Split('.')[0];
            return routes.Where(k => k.Key.Equals(lastUrlSegment, StringComparison.InvariantCultureIgnoreCase))
                .Select(k => new HttpHandlerWithReadOnlySession(k.Value))
                .FirstOrDefault();
        }

        private static ISchedulerWrapper GetSchedulerWrapper() {
            return new SchedulerWrapper(Setup.Scheduler());
        }

        private readonly IEnumerable<Route> routes =
            new[] {
                Route("jobgroup", ctx => JobGroupController.Execute(ctx, GetSchedulerWrapper)),
                Route("index", ctx => IndexController.Execute(ctx, GetSchedulerWrapper)),
                Route("log", LogController.Execute),
                Route("scheduler", ctx => SchedulerController.Execute(ctx, GetSchedulerWrapper)),
                Route("static", StaticController.Execute),
                Route("triggerGroup", ctx => TriggerGroupController.Execute(ctx, Setup.Scheduler)),
                Route("triggersByJob", ctx => TriggersByJobController.Execute(ctx, Setup.Scheduler)),
            };

        public static Route Route(string path, Action<HttpContextBase> action) {
            return new Route(path, action);
        }
    }
}