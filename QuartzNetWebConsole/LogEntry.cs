using System;

namespace QuartzNetWebConsole {
    public class LogEntry {
        private readonly DateTime timestamp;
        private readonly string description;

        public DateTime Timestamp {
            get { return timestamp; }
        }

        public string Description {
            get { return description; }
        }

        public LogEntry(string description) {
            timestamp = DateTime.Now;
            this.description = description;
        }
    }
}