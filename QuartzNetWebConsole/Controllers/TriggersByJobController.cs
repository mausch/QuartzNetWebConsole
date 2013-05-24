using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;
using System.Collections.Generic;

namespace QuartzNetWebConsole.Controllers {
    public class TriggersByJobController {
        private static IEnumerable<TriggerWithState> GetTriggers(ISchedulerWrapper scheduler, JobKey jobKey) {
            var triggers = scheduler.GetTriggersOfJob(jobKey);
            if (triggers == null)
                return null;
            return triggers.Select(t => {
                var state = scheduler.GetTriggerState(t.Key);
                return new TriggerWithState(t, state);
            });
        }

        public static void Execute(HttpContextBase context, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var highlight = context.Request.QueryString["highlight"];
            var group = context.Request.QueryString["group"];
            var job = context.Request.QueryString["job"];
            var jobKey = new JobKey(job, group);
            var triggers = GetTriggers(scheduler, jobKey);
            var thisUrl = context.Request.RawUrl;
            var m = new TriggersByJobModel(triggers, thisUrl, group, job, highlight);
            context.Response.Html(Helpers.XHTML(Views.Views.TriggersByJob(m)));
        }
    }
}