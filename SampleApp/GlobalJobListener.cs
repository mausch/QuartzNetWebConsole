using System;
using Quartz;

namespace SampleApp {
    public class GlobalJobListener : IJobListener {
        public void JobToBeExecuted(JobExecutionContext context) {
        }

        public void JobExecutionVetoed(JobExecutionContext context) {
        }

        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException) {
        }

        public string Name {
            get {
                return GetType().FullName;
            }
        }
    }
}