using System;
using System.Web;
using MiniMVC;
using Quartz;

namespace QuartzNetWebConsole {
    public class IndexController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            return new ViewResult(new {
                scheduler.SchedulerName,
                scheduler.JobGroupNames,
                scheduler.CalendarNames,
                scheduler.InStandbyMode,
                scheduler.IsShutdown,
                scheduler.IsStarted,
                scheduler.JobListenerNames,
                scheduler.TriggerGroupNames,
                scheduler.TriggerListenerNames,
            }, ViewName);
        }
    }
}