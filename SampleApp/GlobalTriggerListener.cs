using Quartz;

namespace SampleApp {
    /// <summary>
    /// A sample dummy global trigger listener
    /// </summary>
    public class GlobalTriggerListener : ITriggerListener {
        public void TriggerFired(Trigger trigger, JobExecutionContext context) {}

        public bool VetoJobExecution(Trigger trigger, JobExecutionContext context) {
            return false;
        }

        public void TriggerMisfired(Trigger trigger) {}

        public void TriggerComplete(Trigger trigger, JobExecutionContext context, SchedulerInstruction triggerInstructionCode) {}

        public string Name {
            get { return GetType().FullName; }
        }
    }
}