using System.Collections.Generic;
using System.Linq;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using BoardGame.Server.BusinessLogic.Interfaces;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Helpers;
using BoardGame.Domain.Logger;
using System;

namespace BoardGame.Server.BusinessLogic
{
    public class GameServer : IGameServer
    {
        private static readonly object NewGameLock = new object();
        private static readonly object ConfirmLock = new object();

        private int nextPlayerId = 1;
        private readonly IGameFactory gameFactory;
        private readonly IPlayerFactory playerFactory;
        private readonly ILogger logger;

        public BlockingPredicateCollection<IPlayer> WaitingPlayers { get; } = new BlockingPredicateCollection<IPlayer>();
        public List<IGame> RunningGames { get; } = new List<IGame>();
        
        public GameServer(IGameFactory gameFactory,
                          IPlayerFactory playerFactory,
                          ILogger logger = null)
        {
            this.gameFactory = gameFactory;
            this.playerFactory = playerFactory;
            this.logger = logger;
        }

        private IGame GetGameByPlayerIdOrDefault(int id)
        {
            return RunningGames.SingleOrDefault(g => g.Players.Contains(g.Players.FirstOrDefault(p => p.OnlineId == id)));
        }

        public IPlayer NewPlayer(int playerId = 0)
        {
            int id = playerId == 0 ? nextPlayerId++ : playerId;
            return playerFactory.Create(PlayerType.OnlinePlayer, id);
        }

        public bool NewGame(List<IPlayer> players)
        {
            lock (NewGameLock)
            {
                if (players == null || players.Count != 2)
                {
                    throw new GameServerException(
                        "Exception occurred while creating new game. NewGame method requires not-null list of 2 players.");
                }

                if (GetGameByPlayerIdOrDefault(players[0].OnlineId) == null &&
                    GetGameByPlayerIdOrDefault(players[1].OnlineId) == null)
                {
                    IGame game = gameFactory.Create(players);
                    game.State = GameState.Ready;
                    RunningGames.Add(game);
                    return true;
                }
                return false;
            }
        }

        public IGame GetGameByPlayerId(int id)
        {
            try
            {
                return RunningGames.Single(g => g.Players.Contains(g.Players.FirstOrDefault(p => p.OnlineId == id)));
            }
            catch(InvalidOperationException ex)
            {
                throw new GameServerException("Exception occurred while getting game by player's id. Either cannot find any game with player's id=" + id + " or there are more games with such id.", ex);
            }
            catch(ArgumentNullException ex)
            {
                throw new GameServerException("Exception occurred in GetGameByPlayerId method. RunningGames property is null.", ex);
            }
        }

        public void ConfirmPlayer(int id)
        {
            lock (ConfirmLock)
            {
                IGame game = GetGameByPlayerId(id);
                IPlayer player = game.Players.Single(p => p.OnlineId == id);
                player.Confirmed = true;

                if (game.Players.All(p => p.Confirmed))
                {
                    game.State = GameState.Confirmed;
                }
            }
        }
    }
}
