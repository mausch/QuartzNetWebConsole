using System;
using Quartz;

namespace QuartzNetWebConsole.Tests {
    public class DummyTrigger : ITrigger {
        public int CompareTo(ITrigger other) {
            throw new NotImplementedException();
        }

        public TriggerBuilder GetTriggerBuilder() {
            throw new NotImplementedException();
        }

        public IScheduleBuilder GetScheduleBuilder() {
            throw new NotImplementedException();
        }

        public bool GetMayFireAgain() {
            throw new NotImplementedException();
        }

        public DateTimeOffset? GetNextFireTimeUtc() {
            return NextFireTimeUtc;
        }

        public DateTimeOffset? GetPreviousFireTimeUtc() {
            throw new NotImplementedException();
        }

        public DateTimeOffset? GetFireTimeAfter(DateTimeOffset? afterTime) {
            throw new NotImplementedException();
        }

        public ITrigger Clone()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? NextFireTimeUtc { get; set; }
        public TriggerKey Key { get; set; }
        public JobKey JobKey { get; set; }
        public string Description { get; set; }
        public string CalendarName { get; set; }
        public JobDataMap JobDataMap { get; set; }
        public DateTimeOffset? FinalFireTimeUtc { get; set; }
        public int MisfireInstruction { get; set; }
        public DateTimeOffset? EndTimeUtc { get; set; }
        public DateTimeOffset StartTimeUtc { get; set; }
        public int Priority { get; set; }
        public bool HasMillisecondPrecision { get; set; }
    }
}