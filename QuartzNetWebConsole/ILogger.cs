using System.Collections.Generic;
using System.Linq;
using Quartz;

namespace QuartzNetWebConsole {
    public interface ILogger: ISchedulerListener, IJobListener, ITriggerListener, IQueryable<LogEntry> {}
}