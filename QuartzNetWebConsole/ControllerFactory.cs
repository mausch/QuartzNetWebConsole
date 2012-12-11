using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniMVC;
using QuartzNetWebConsole.Controllers;

namespace QuartzNetWebConsole {
    public class ControllerFactory : HttpHandlerFactory {
        public override IHttpHandler GetHandler(HttpContextBase context) {
            var lastUrlSegment = context.Request.Url.Segments.Last().Split('.')[0];
            return routes.Where(k => k.Key.Equals(lastUrlSegment, StringComparison.InvariantCultureIgnoreCase))
                .Select(k => k.Value)
                .FirstOrDefault();
        }


        private readonly IEnumerable<KeyValuePair<string, IHttpHandler>> routes =
            new[] {
                KV("jobgroup", JobGroupHandler),
                KV("index", IndexHandler),
                KV("log", LogHandler),
                KV("scheduler", SchedulerHandler),
                KV("static", StaticHandler),
                KV("triggerGroup", TriggerGroupHandler),
                KV("triggersByJob", TriggersByJobHandler),
            };

        private static readonly IHttpHandler IndexHandler = new HttpHandlerWithReadOnlySession(IndexController.Execute);
        private static readonly IHttpHandler JobGroupHandler = new HttpHandlerWithReadOnlySession(JobGroupController.Execute);
        private static readonly IHttpHandler LogHandler = new HttpHandlerWithReadOnlySession(LogController.Execute);
        private static readonly IHttpHandler SchedulerHandler = new HttpHandlerWithReadOnlySession(SchedulerController.Execute);
        private static readonly IHttpHandler StaticHandler = new HttpHandlerWithReadOnlySession(StaticController.Execute);
        private static readonly IHttpHandler TriggerGroupHandler = new HttpHandlerWithReadOnlySession(TriggerGroupController.Execute);
        private static readonly IHttpHandler TriggersByJobHandler = new HttpHandlerWithReadOnlySession(TriggersByJobController.Execute);


        public static KeyValuePair<K, V> KV<K, V>(K key, V value) {
            return new KeyValuePair<K, V>(key, value);
        }
    }
}