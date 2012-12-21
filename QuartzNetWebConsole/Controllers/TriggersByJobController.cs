using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class TriggersByJobController {
        private static IScheduler scheduler {
            get {
                return Setup.Scheduler();
            }
        }

        public static void Execute(HttpContextBase context) {
            var group = context.Request.QueryString["group"];
            var job = context.Request.QueryString["job"];
            var jobKey = new JobKey(job, group);
            var thisUrl = context.Request.RawUrl;
            var triggers = scheduler.GetTriggersOfJob(jobKey)
                .Select(t => {
                    var state = scheduler.GetTriggerState(t.Key);
                    return new TriggerWithState(t, state);
                });
            var highlight = context.Request.QueryString["highlight"];
            var m = new TriggersByJobModel(triggers, thisUrl, group, job, highlight);
            context.Response.Html(Helpers.XHTML(Views.Views.TriggersByJob(m)));
        }
    }
}