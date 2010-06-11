using System;
using System.Threading;
using Quartz;

namespace SampleApp {
    public class HelloJob : IStatefulJob {
        public void Execute(JobExecutionContext context) {
            Thread.Sleep(5000);
        }
    }
}