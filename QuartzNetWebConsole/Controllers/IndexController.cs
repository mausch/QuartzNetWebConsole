using System;
using System.Linq;
using System.Web;
using MiniMVC;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class IndexController {

        public static void Execute(HttpContextBase context, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
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

            var view = Views.Views.IndexPage(
                schedulerName: scheduler.SchedulerName,
                inStandby: scheduler.InStandbyMode,
                listeners: scheduler.ListenerManager.GetSchedulerListeners(),
                metadata:scheduler.GetMetaData(),
                triggerGroups: triggerGroups,
                jobGroups: jobGroups,
                calendars: calendars,
                jobListeners: jobListeners,
                triggerListeners: triggerListeners);
            context.Response.Html(Helpers.XHTML(view));
        }
    }
}