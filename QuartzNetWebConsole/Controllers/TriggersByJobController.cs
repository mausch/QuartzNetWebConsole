using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Models;

namespace QuartzNetWebConsole.Controllers {
    public class TriggersByJobController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            var group = context.Request.QueryString["group"];
            var job = context.Request.QueryString["job"];
            var thisUrl = context.Request.RawUrl;
            var triggers = scheduler.GetTriggersOfJob(job, group)
                .Select(t => {
                    var state = scheduler.GetTriggerState(t.Name, t.Group);
                    return new TriggerWithState(t, state);
                });
            return new ViewResult(new {triggers, thisUrl, group, job}, ViewName);
        }
    }
}