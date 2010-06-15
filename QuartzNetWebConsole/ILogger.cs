using System.Collections.Generic;
using Quartz;

namespace QuartzNetWebConsole {
    public interface ILogger: ISchedulerListener, IJobListener, ITriggerListener, IEnumerable<LogEntry> {}
}