using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class IndexController {

        public static async Task<Response> Execute(Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var triggerGroups = await (await scheduler.GetTriggerGroupNames())
                .Traverse(async t => new GroupWithStatus(t, await scheduler.IsTriggerGroupPaused(t)));

            var jobGroups = await (await scheduler.GetJobGroupNames())
                .Traverse(async j => new GroupWithStatus(j, await scheduler.IsJobGroupPaused(j)));

            var calendars = await (await scheduler.GetCalendarNames())
                .Traverse(async name => Helpers.KV(name, (await scheduler.GetCalendar(name)).Description));

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
                metadata: await scheduler.GetMetaData(),
                triggerGroups: triggerGroups,
                jobGroups: jobGroups,
                calendars: calendars,
                jobListeners: jobListeners,
                triggerListeners: triggerListeners);
            return new Response.XDocumentResponse(Helpers.XHTML(view));
        }
    }
}