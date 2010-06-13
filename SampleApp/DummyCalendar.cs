using System;
using Quartz;

namespace SampleApp {
    public class DummyCalendar : ICalendar {
        public bool IsTimeIncluded(DateTime timeUtc) {
            return false;
        }

        public DateTime GetNextIncludedTimeUtc(DateTime timeUtc) {
            return timeUtc;
        }

        public string Description { get; set; }

        public ICalendar CalendarBase { get; set; }
    }
}