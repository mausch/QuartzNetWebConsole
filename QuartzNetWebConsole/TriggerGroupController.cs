using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;

namespace QuartzNetWebConsole {
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
            return new ViewResult(new {triggers, thisUrl, group}, ViewName);
        }

        public struct TriggerWithState {
            private readonly Trigger trigger;
            private readonly TriggerState state;

            public TriggerWithState(Trigger trigger, TriggerState state) {
                this.trigger = trigger;
                this.state = state;
            }

            public Trigger Trigger {
                get { return trigger; }
            }

            public TriggerState State {
                get { return state; }
            }
        }
    }
}