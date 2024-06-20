using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoLog
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string LogLevel { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionSource { get; set; }
        public DateTime Created { get; set; }
    }
}
