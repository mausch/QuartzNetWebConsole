using System;
using System.Threading;
using Quartz;

namespace SampleApp {
    /// <summary>
    /// A sample dummy job
    /// </summary>
    [DisallowConcurrentExecution]
    public class HelloJob : IJob {
        public void Execute(IJobExecutionContext context) {
            Thread.Sleep(5000);
        }
    }
}