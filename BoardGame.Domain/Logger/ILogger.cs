using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Logger
{
    public interface ILogger
    {
        void Log(LogEntry entry);
    }
}
