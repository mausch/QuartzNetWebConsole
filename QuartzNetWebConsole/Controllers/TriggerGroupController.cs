using System;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuartzNetWebConsole.Controllers {
    public class TriggerGroupController {
        private static async Task<IEnumerable<TriggerWithState>> GetTriggers(ISchedulerWrapper scheduler, string group) {
            var triggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(group));
            if (triggerKeys == null)
                return null;

            return await triggerKeys.Traverse(async t =>
            {
                var trigger = await scheduler.GetTrigger(t);
                var state = await scheduler.GetTriggerState(t);
                return new TriggerWithState(trigger, state);
            });
        }

        public static async Task<Response> Execute(RelativeUri url, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var qs = url.ParseQueryString();
            var highlight = qs["highlight"];
            var group = qs["group"];
            var triggers = await GetTriggers(scheduler, group);
            var paused = await scheduler.IsTriggerGroupPaused(group);
            var v = Views.Views.TriggerGroup(group, paused, url.PathAndQuery, highlight, triggers);
            return new Response.XDocumentResponse(Helpers.XHTML(v));
        }
    }
}