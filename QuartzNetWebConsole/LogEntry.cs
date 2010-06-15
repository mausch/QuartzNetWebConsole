using System;

namespace QuartzNetWebConsole {
    public class LogEntry {
        private readonly DateTimeOffset timestamp;
        private readonly string description;

        public DateTimeOffset Timestamp {
            get { return timestamp; }
        }

        public string Description {
            get { return description; }
        }

        public LogEntry(string description) {
            timestamp = DateTimeOffset.Now;
            this.description = description;
        }
    }
}