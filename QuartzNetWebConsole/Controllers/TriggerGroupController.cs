using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Models;

namespace QuartzNetWebConsole.Controllers {
    public class TriggerGroupController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            var group = context.Request.QueryString["group"];
            var triggerNames = scheduler.GetTriggerNames(group);
            var triggers = triggerNames
                .Select(t => {
                    var trigger = scheduler.GetTrigger(t, group);
                    var state = scheduler.GetTriggerState(t, group);
                    return new TriggerWithState(trigger, state);
                });
            var thisUrl = context.Request.RawUrl;
            var paused = scheduler.IsTriggerGroupPaused(group);
            return new ViewResult(new {triggers, thisUrl, group, paused}, ViewName);
        }
    }
}