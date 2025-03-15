using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace SampleApp {
    /// <summary>
    /// A sample dummy job
    /// </summary>
    [DisallowConcurrentExecution]
    public class HelloJob : IJob {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(5000);
        }
    }
}