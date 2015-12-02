using BoardGame.Server.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.BusinessLogic
{
    public class GameServer : IGameServer
    {
        public int a;

        public GameServer()
        {
            a = 0;
        }

        public int GetColumn()
        {
            if (a == 7) a = 0;
            return a++;
        }
    }
}
