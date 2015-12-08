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
        List<IGame> RunningGames { get; }

        IPlayer CreateNewPlayer(int playerId = 0);
        void NewGame(IEnumerable<IPlayer> players);
        IGame GetGameByPlayerId(int id);
        IPlayer GetAvailablePlayer(int id);
    }
}
