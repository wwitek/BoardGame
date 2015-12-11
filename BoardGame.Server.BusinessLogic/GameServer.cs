using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using BoardGame.Server.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;

namespace BoardGame.Server.BusinessLogic
{
    public class GameServer : IGameServer
    {
        private int NextPlayerId = 1;
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
            return PlayerFactory.Create(PlayerType.OnlinePlayer, id);
        }

        public void NewGame(IEnumerable<IPlayer> players)
        {
            IGame game = GameFactory.Create(players);
            game.State = GameState.Ready;
            RunningGames.Add(game);
        }

        public IGame GetGameByPlayerId(int id)
        {
            return RunningGames.SingleOrDefault(g => g.Players.Contains(g.Players.FirstOrDefault(p => p.OnlineId == id)));
        }

        public IPlayer GetAvailablePlayer(int id)
        {
            return WaitingPlayers.Where(wp => wp.OnlineId != id)
                                 .FirstOrDefault(p => GetGameByPlayerId(p.OnlineId) == null);
        }

    }
}
