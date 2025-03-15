using System.Collections.Generic;
using System.Linq;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole {
    public interface ILogger: ISchedulerListener, IJobListener, ITriggerListener, IQueryable<LogEntry> {
        void Add(string msg);
    }
}