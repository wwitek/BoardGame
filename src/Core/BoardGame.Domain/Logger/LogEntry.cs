using BoardGame.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Logger
{
    public class LogEntry
    {
        public readonly LoggingEventType Severity;
        public readonly string Message;
        public readonly Exception Exception;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            //Requires.IsNotNullOrEmpty(message, "message");
            //Requires.IsValidEnum(severity, "severity");
            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }
}
