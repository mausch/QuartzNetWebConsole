using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniMVC;
using QuartzNetWebConsole.Controllers;
using Route = System.Collections.Generic.KeyValuePair<string, System.Action<System.Web.HttpContextBase>>;

namespace QuartzNetWebConsole {
    public class ControllerFactory : HttpHandlerFactory {
        public override IHttpHandler GetHandler(HttpContextBase context) {
            var lastUrlSegment = context.Request.Url.Segments.Last().Split('.')[0];
            return routes.Where(k => k.Key.Equals(lastUrlSegment, StringComparison.InvariantCultureIgnoreCase))
                .Select(k => new HttpHandlerWithReadOnlySession(k.Value))
                .FirstOrDefault();
        }

        private readonly IEnumerable<Route> routes =
            new[] {
                Route("jobgroup", JobGroupController.Execute),
                Route("index", IndexController.Execute),
                Route("log", LogController.Execute),
                Route("scheduler", SchedulerController.Execute),
                Route("static", StaticController.Execute),
                Route("triggerGroup", TriggerGroupController.Execute),
                Route("triggersByJob", TriggersByJobController.Execute),
            };

        public static KeyValuePair<string, Action<HttpContextBase>> Route(string path, Action<HttpContextBase> action) {
            return new KeyValuePair<string, Action<HttpContextBase>>(path, action);
        }
    }
}