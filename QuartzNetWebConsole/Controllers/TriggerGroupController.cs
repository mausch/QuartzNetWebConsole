using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;
using System.Collections.Generic;

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

        public static void Execute(HttpContextBase context, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var highlight = context.Request.QueryString["highlight"];
            var group = context.Request.QueryString["group"];
            var triggers = GetTriggers(scheduler, group);
            var thisUrl = context.Request.RawUrl;
            var paused = scheduler.IsTriggerGroupPaused(group);
            var v = Views.Views.TriggerGroup(group, paused, thisUrl, highlight, triggers);
            context.Response.Html(Helpers.XHTML(v));
        }
    }
}