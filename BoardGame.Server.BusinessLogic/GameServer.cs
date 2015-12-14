using System.Collections.Generic;
using System.Linq;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using BoardGame.Server.BusinessLogic.Interfaces;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Helpers;
using BoardGame.Domain.Logger;

namespace BoardGame.Server.BusinessLogic
{
    public class GameServer : IGameServer
    {
        private readonly object newGameLock = new object();
        private readonly object confirmLock = new object();

        private int NextPlayerId = 1;
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;

        public BlockingPredicateCollection<IPlayer> WaitingPlayers { get; } = new BlockingPredicateCollection<IPlayer>();
        public List<IGame> RunningGames { get; } = new List<IGame>();
        public ILogger Logger { get; }

        public GameServer(IGameFactory gameFactory,
                          IPlayerFactory playerFactory,
                          ILogger logger = null)
        {
            GameFactory = gameFactory;
            PlayerFactory = playerFactory;
            Logger = logger;
        }

        public IPlayer CreateNewPlayer(int playerId = 0)
        {
            int id = playerId == 0 ? NextPlayerId++ : playerId;
            return PlayerFactory.Create(PlayerType.OnlinePlayer, id);
        }

        public bool NewGame(List<IPlayer> players)
        {
            lock (newGameLock)
            {
                if (players == null || players.Count != 2)
                {
                    // TODO throw an excpetion
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
