using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class IndexController {
        private static IScheduler scheduler {
            get {
                return Setup.Scheduler();
            }
        }

        public static void Execute(HttpContextBase context) {
            var triggerGroups = scheduler.GetTriggerGroupNames()
                .Select(t => new GroupWithStatus(t, scheduler.IsTriggerGroupPaused(t)))
                .ToArray();

            var jobGroups = scheduler.GetJobGroupNames()
                .Select(j => new GroupWithStatus(j, scheduler.IsJobGroupPaused(j)))
                .ToArray();

            var calendars = scheduler.GetCalendarNames()
                .Select(name => Helpers.KV(name, scheduler.GetCalendar(name).Description))
                .ToArray();


            var jobListeners = scheduler.ListenerManager.GetJobListeners()
                .Select(j => Helpers.KV(j.Name, j.GetType()))
                .ToArray();

            var triggerListeners = scheduler.ListenerManager.GetTriggerListeners()
                .Select(j => Helpers.KV(j.Name, j.GetType()))
                .ToArray();

            var view = Views.Views.IndexPage(scheduler, scheduler.GetMetaData(), triggerGroups, jobGroups, calendars, jobListeners, triggerListeners);
            context.XDocument(Helpers.XHTML(view));
        }
    }
}