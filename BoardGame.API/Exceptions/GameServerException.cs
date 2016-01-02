using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.API.Exceptions
{
    public class GameServerException : Exception
    {
        public GameServerException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
