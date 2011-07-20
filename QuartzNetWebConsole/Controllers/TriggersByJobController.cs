using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Views;

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
            var highlight = context.Request.QueryString["highlight"];
            var m = new TriggersByJobModel(triggers, thisUrl, group, job, highlight);
            return new XDocResult(new XDocument(X.XHTML1_0_Transitional, Views.Views.TriggersByJob(m)));
        }
    }
}