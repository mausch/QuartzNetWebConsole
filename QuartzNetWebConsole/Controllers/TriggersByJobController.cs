using System;
using System.Linq;
using Quartz;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartzNetWebConsole.Controllers {
    public class TriggersByJobController {
        private static async Task<IEnumerable<TriggerWithState>> GetTriggers(ISchedulerWrapper scheduler, JobKey jobKey) {
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            if (triggers == null)
                return null;

            return await triggers.Traverse(async t =>
            {
                var state = await scheduler.GetTriggerState(t.Key);
                return new TriggerWithState(t, state);
            });
        }

        public static async Task<Response> Execute(RelativeUri url, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var querystring = url.ParseQueryString();
            var highlight = querystring["highlight"];
            var group = querystring["group"];
            var job = querystring["job"];
            var jobKey = new JobKey(job, group);
            var triggers = await GetTriggers(scheduler, jobKey);
            var m = new TriggersByJobModel(triggers, url.PathAndQuery, group, job, highlight);
            return new Response.XDocumentResponse(Helpers.XHTML(Views.Views.TriggersByJob(m)));
        }
    }
}