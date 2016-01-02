using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Exceptions
{
    public class BotNotRegisteredException : Exception
    {
        internal BotNotRegisteredException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
