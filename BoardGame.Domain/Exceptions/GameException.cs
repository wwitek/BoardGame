using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Exceptions
{
    public class GameException : Exception
    {
        public GameException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
