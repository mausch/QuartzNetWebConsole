using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;

namespace QuartzNetWebConsole {
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
                .Select(name => new {name, description = scheduler.GetCalendar(name).Description})
                .ToArray();

            var globalJobListeners = scheduler.GlobalJobListeners
                .Cast<IJobListener>()
                .Select(j => new {name = j.Name, type = j.GetType(), global = true});
            var otherJobListeners = scheduler.JobListenerNames
                .Cast<string>()
                .Select(j => scheduler.GetJobListener(j))
                .Select(j => new {name = j.Name, type = j.GetType(), global = false});
            var jobListeners = globalJobListeners.Concat(otherJobListeners).ToArray();

            var globalTriggerListeners = scheduler.GlobalTriggerListeners
                .Cast<ITriggerListener>()
                .Select(l => new {name = l.Name, type = l.GetType(), global = true});
            var otherTriggerListeners = scheduler.TriggerListenerNames
                .Cast<string>()
                .Select(n => scheduler.GetTriggerListener(n))
                .Select(l => new { name = l.Name, type = l.GetType(), global = false });
            var triggerListeners = globalTriggerListeners.Concat(otherTriggerListeners).ToArray();

            return new ViewResult(new {
                scheduler,
                metadata = scheduler.GetMetaData(),
                triggerGroups,
                jobGroups,
                calendars,
                jobListeners,
                triggerListeners,
            }, ViewName);
        }

        public struct GroupWithStatus {
            private readonly string name;
            private readonly bool paused;

            public GroupWithStatus(string name, bool paused) {
                this.name = name;
                this.paused = paused;
            }

            public string Name {
                get { return name; }
            }

            public bool Paused {
                get { return paused; }
            }
        }
    }
}