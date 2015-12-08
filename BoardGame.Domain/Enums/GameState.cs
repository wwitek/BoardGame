using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Enums
{
    public enum GameState
    {
        Waiting = 0,
        Ready = 1,
        Confirmed = 2,
        New = 3,
        Running = 4,
        Finished = 5,
        Aborted = 6
    }
}
