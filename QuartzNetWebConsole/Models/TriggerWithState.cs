using Quartz;

namespace QuartzNetWebConsole.Models {
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

        public bool IsPaused {
            get { return state == TriggerState.Paused; }
        }
    }
}