using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BoardGame.Server.Contracts;

namespace BoardGame.Server.Services
{
    public class GameService : IGameService
    {
        public int GetNextMove()
        {
            return 5;
        }
    }
}
