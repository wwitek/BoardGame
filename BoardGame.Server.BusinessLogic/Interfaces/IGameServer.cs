using BoardGame.Domain.Helpers;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.BusinessLogic.Interfaces
{
    public interface IGameServer
    {
        BlockingPredicateCollection<IPlayer> WaitingPlayers { get; }
        List<IGame> RunningGames { get; }
        ILogger Logger { get; }

        IPlayer CreateNewPlayer(int playerId = 0);
        bool NewGame(List<IPlayer> players);
        IGame GetGameByPlayerId(int id);
        void ConfirmPlayer(int id);
    }
}
