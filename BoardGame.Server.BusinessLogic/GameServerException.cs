using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.BusinessLogic
{
    [Serializable]
    public class GameServerException : Exception
    {
        public GameServerException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
