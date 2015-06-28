using System;
using System.Linq;
using Quartz;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;

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

        public static Response Execute(Uri url, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var querystring = url.ParseQueryString();
            var highlight = querystring["highlight"];
            var group = querystring["group"];
            var job = querystring["job"];
            var jobKey = new JobKey(job, group);
            var triggers = GetTriggers(scheduler, jobKey);
            var m = new TriggersByJobModel(triggers, url.PathAndQuery, group, job, highlight);
            return new Response.XDocumentResponse(Helpers.XHTML(Views.Views.TriggersByJob(m)));
        }
    }
}