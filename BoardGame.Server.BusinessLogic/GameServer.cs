using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
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
        private int NextPlayerId = 0;
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;

        public List<IPlayer> WaitingPlayers { get; } = new List<IPlayer>();
        public List<IGame> RunningGames { get; } = new List<IGame>();

        public GameServer(IGameFactory gameFactory, IPlayerFactory playerFactory)
        {
            GameFactory = gameFactory;
            PlayerFactory = playerFactory;
        }

        public IPlayer CreateNewPlayer(int playerId = 0)
        {
            int id = playerId == 0 ? NextPlayerId++ : playerId;
            return PlayerFactory.Create(Domain.Enums.PlayerType.OnlinePlayer, id);
        }

        public IGame CreateNewGame(IEnumerable<IPlayer> players)
        {
            return GameFactory.Create(players);
        }
    }
}
