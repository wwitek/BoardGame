using BoardGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.BusinessLogic.Interfaces
{
    public interface IGameServer
    {
        List<IPlayer> WaitingPlayers { get; }

        IPlayer CreateNewPlayer(int playerId = 0);
        IGame CreateNewGame(IEnumerable<IPlayer> players);
    }
}
