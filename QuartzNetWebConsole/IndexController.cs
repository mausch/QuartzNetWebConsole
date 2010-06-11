using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;

namespace QuartzNetWebConsole {
    public class IndexController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            var pausedTriggerGroups = scheduler.GetPausedTriggerGroups();
            var triggerGroups = scheduler.TriggerGroupNames
                .Select(t => new TriggerGroupWithStatus(t, pausedTriggerGroups.Contains(t)))
                .ToArray();
            return new ViewResult(new {
                scheduler,
                metadata = scheduler.GetMetaData(),
                triggerGroups,
            }, ViewName);
        }

        public struct TriggerGroupWithStatus {
            private readonly string name;
            private readonly bool paused;

            public TriggerGroupWithStatus(string name, bool paused) {
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