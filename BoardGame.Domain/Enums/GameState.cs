using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Enums
{
    public enum GameState
    {
        Waiting,
        Ready,
        Confirming,
        Confirmed,
        New,
        Running,
        Finished,
        Aborted
    }
}
