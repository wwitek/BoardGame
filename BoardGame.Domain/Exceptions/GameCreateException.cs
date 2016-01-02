using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Exceptions
{
    public class GameCreateException : Exception
    {
        internal GameCreateException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
