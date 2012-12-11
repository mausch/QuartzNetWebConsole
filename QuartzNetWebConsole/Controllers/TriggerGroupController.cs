using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using Quartz.Impl.Matchers;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class TriggerGroupController {
        private static IScheduler scheduler {
            get {
                return Setup.Scheduler();
            }
        }

        public static void Execute(HttpContextBase context) {
            var group = context.Request.QueryString["group"];
            var triggerKeys = scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(group));
            var triggers = triggerKeys
                .Select(t => {
                    var trigger = scheduler.GetTrigger(t);
                    var state = scheduler.GetTriggerState(t);
                    return new TriggerWithState(trigger, state);
                });
            var thisUrl = context.Request.RawUrl;
            var paused = scheduler.IsTriggerGroupPaused(group);
            var highlight = context.Request.QueryString["highlight"];
            var v = Views.Views.TriggerGroup(group, paused, thisUrl, highlight, triggers);
            context.XDocument(Helpers.XHTML(v));
        }
    }
}