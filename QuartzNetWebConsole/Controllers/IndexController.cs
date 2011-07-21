using System.Linq;
using System.Web;
using System.Xml.Linq;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class IndexController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            var triggerGroups = scheduler.TriggerGroupNames
                .Select(t => new GroupWithStatus(t, scheduler.IsTriggerGroupPaused(t)))
                .ToArray();
            var jobGroups = scheduler.JobGroupNames
                .Select(j => new GroupWithStatus(j, scheduler.IsJobGroupPaused(j)))
                .ToArray();
            var calendars = scheduler.CalendarNames
                .Select(name => Helpers.KV(name, scheduler.GetCalendar(name).Description))
                .ToArray();

            var jobListeners = scheduler.GlobalJobListeners
                .Cast<IJobListener>()
                .Select(j => Helpers.KV(j.Name, j.GetType()))
                .ToArray();

            var triggerListeners = scheduler.GlobalTriggerListeners
                .Cast<ITriggerListener>()
                .Select(j => Helpers.KV(j.Name, j.GetType()))
                .ToArray();

            var view = Views.Views.IndexPage(scheduler, scheduler.GetMetaData(), triggerGroups, jobGroups, calendars, jobListeners, triggerListeners);

            return new XDocResult(Helpers.XHTML(view));
        }
    }
}