using System;
using Quartz;

namespace SampleApp {
    /// <summary>
    /// A sample dummy global job listener
    /// </summary>
    public class GlobalJobListener : IJobListener {
        public void JobToBeExecuted(IJobExecutionContext context) {
        }

        public void JobExecutionVetoed(IJobExecutionContext context) {
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException) {
        }

        public string Name {
            get {
                return GetType().FullName;
            }
        }
    }
}