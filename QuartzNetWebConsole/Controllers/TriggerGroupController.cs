using System;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;
using System.Collections.Generic;
using System.Xml.Linq;

namespace QuartzNetWebConsole.Controllers {
    public class TriggerGroupController {
        private static IEnumerable<TriggerWithState> GetTriggers(ISchedulerWrapper scheduler, string group) {
            var triggerKeys = scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(group));
            if (triggerKeys == null)
                return null;
            return triggerKeys.Select(t => {
                var trigger = scheduler.GetTrigger(t);
                var state = scheduler.GetTriggerState(t);
                return new TriggerWithState(trigger, state);
            });
        }

        public static Response Execute(Uri url, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var qs = url.ParseQueryString();
            var highlight = qs["highlight"];
            var group = qs["group"];
            var triggers = GetTriggers(scheduler, group);
            var paused = scheduler.IsTriggerGroupPaused(group);
            var v = Views.Views.TriggerGroup(group, paused, url.PathAndQuery, highlight, triggers);
            return new Response.XDocumentResponse(Helpers.XHTML(v));
        }
    }
}