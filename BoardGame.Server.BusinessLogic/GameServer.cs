using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using BoardGame.Server.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Helpers;

namespace BoardGame.Server.BusinessLogic
{
    public class GameServer : IGameServer
    {
        private readonly object newgameLock = new object();
        private readonly object confirmLock = new object();
        private int NextPlayerId = 1;
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;

        public BlockingPredicateCollection<IPlayer> WaitingPlayers { get; } = new BlockingPredicateCollection<IPlayer>();
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

        public bool NewGame(List<IPlayer> players)
        {
            lock (newgameLock)
            {
                if (players == null || players.Count != 2)
                {
                    // TODO Logging...
                    return false;
                }

                if (GetGameByPlayerId(players[0].OnlineId) == null && GetGameByPlayerId(players[1].OnlineId) == null)
                {
                    IGame game = GameFactory.Create(players);
                    game.State = GameState.Ready;
                    RunningGames.Add(game);
                    return true;
                }

                return false;
            }
        }

        public IGame GetGameByPlayerId(int id)
        {
            return RunningGames.SingleOrDefault(g => g.Players.Contains(g.Players.FirstOrDefault(p => p.OnlineId == id)));
        }

        public void ConfirmPlayer(int id)
        {
            lock (confirmLock)
            {
                IGame game = GetGameByPlayerId(id);
                IPlayer player = game.Players.SingleOrDefault(p => p.OnlineId == id);
                player.Confirmed = true;

                if (game.Players.All(p => p.Confirmed))
                {
                    game.State = GameState.Confirmed;
                }
            }
        }
    }
}
