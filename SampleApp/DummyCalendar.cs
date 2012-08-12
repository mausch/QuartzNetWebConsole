using System;
using Quartz;

namespace SampleApp {
    public class DummyCalendar : ICalendar {
        public bool IsTimeIncluded(DateTimeOffset timeUtc) {
            return false;
        }

        public DateTimeOffset GetNextIncludedTimeUtc(DateTimeOffset timeUtc) {
            return timeUtc;
        }

        public string Description { get; set; }

        public ICalendar CalendarBase { get; set; }
        public object Clone() {
            return this;
        }
    }
}