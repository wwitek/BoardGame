using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGame.Contracts;
using BoardGame.Server.Business.Interfaces;
using BoardGame.Contracts.Responses;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using System.Threading;
using System.ComponentModel;
using BoardGame.Domain.Logger;

namespace BoardGame.Server.Services
{
    public class GameServiceAsync : IGameServiceAsync
    {
        private IGameServer Logic { get; }
        private ILogger Logger { get; }
        private Random RandomGenerator { get; } = new Random();

        public GameServiceAsync(IGameServer logic, ILogger logger)
        {
            Logic = logic;
            Logger = logger;
            Logger.Info("GameServiceAsync created.");
        }

        public async Task<OnlineGameResponse> OnlineGameRequest(int playerId)
        {
            OnlineGameResponse response;
            IPlayer rivalPlayer;

            IPlayer player = Logic.NewPlayer(playerId);
            Logger.Info("Player{0} requested to play", player.OnlineId);
            Logic.WaitingPlayers.Add(player);

            if (Logic.WaitingPlayers.TryTake(out rivalPlayer, 5000, rp => rp.OnlineId != player.OnlineId))
            {
                Logger.Info("Rival (Player{0}) was found for Player{1}", rivalPlayer.OnlineId, player.OnlineId);
                List<IPlayer> players = RandomGenerator.Next(2) == 0 ? new List<IPlayer>() { rivalPlayer, player }
                                                                     : new List<IPlayer>() { player, rivalPlayer };
                if (Logic.NewGame(players)) Logger.Info("New game created for: Player{0} (will start) and Player{1}.", players[0].OnlineId, players[1].OnlineId);
                response = new OnlineGameResponse(player.OnlineId, true);
            }
            else
            {
                Logger.Info("Rival wasn't found for Player{0}", player.OnlineId);
                Logic.WaitingPlayers.Remove(player);
                response = new OnlineGameResponse(player.OnlineId, false);
            }
            Logger.Info("OnlineGameResponse was send back to client: Player{0}", player.OnlineId);
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            StartGameResponse response;

            Logger.Info("Player{0} requested to confirm", playerId);
            IGame game = Logic.GetGameByPlayerId(playerId);
            bool isConfirmed = false;
            bool yourTurn = game.Players[0].OnlineId == playerId;

            ManualResetEvent waitHandle = new ManualResetEvent(false);
            PropertyChangedEventHandler stateEventHandler = null;
            stateEventHandler = (s, e) =>
            {
                if (game.State.Equals(GameState.Confirmed))
                {
                    Logger.Info("Player{0} vs Player{1} - game status was changed to Confirmed", game.Players[0].OnlineId, game.Players[1].OnlineId);
                    game.OnStateChanged -= stateEventHandler;
                    waitHandle.Set();
                }
                else
                {
                    Logger.Warning("Player{0} vs Player{1} - game status was changed to {2}. It should be changed only to Confirmed.", null, game.Players[0].OnlineId, game.Players[1].OnlineId, game.State);
                }
            };
            game.OnStateChanged += stateEventHandler;
            Logic.ConfirmPlayer(playerId);
            Logger.Info("Player{0} confirmed", playerId);
            isConfirmed = waitHandle.WaitOne(10000);

            int rivalId = game.Players.SingleOrDefault(p => p.OnlineId != playerId).OnlineId;
            if (isConfirmed)
            {
                Logger.Info("Request confirmed by Player{0}'s rival, Player{1}", playerId, rivalId);
            }
            else
            {
                Logger.Info("Timeout! Player{0} didn't confirm his request to play. " +
                            "Player{1} will be looking for rival again.", rivalId, playerId);
                Logic.RunningGames.Remove(game);
            }
            response = new StartGameResponse(isConfirmed, playerId, yourTurn);
            Logger.Info("StartGameResponse was send back to client: Player{0}", playerId);
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> GetFirstMove(int playerId)
        {
            MoveResponse response;

            Logger.Info("Player{0} is waiting for the rival's first move..", playerId);
            IGame game = Logic.GetGameByPlayerId(playerId);

            bool timeout = game.WaitForNextPlayer(1 * 10 * 1000);

            response = new MoveResponse(game.Board.LastMove, timeout);
            if (!timeout)
            {
                Logger.Info("Timeout! Player{0} move timed out!", 
                            game.Players.SingleOrDefault(p => p.OnlineId != playerId).OnlineId);
            }

            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> MakeMove(int playerId, int column)
        {
            MoveResponse response;

            Logger.Info("Player{0} moved in column {1}", playerId, column);
            IGame game = Logic.GetGameByPlayerId(playerId);
            game.MakeMove(column);
            if (game.Board.LastMove.IsConnected)
            {
                Logger.Info("Player{0} won!", playerId);
                Logic.RunningGames.Remove(game);
                response = new MoveResponse(null);
            }
            else
            {
                bool timeout = game.WaitForNextPlayer(1 * 10 * 1000);
                if (!timeout) Logger.Info("Timeout! Player{0} move timed out!", game.NextPlayer.OnlineId);
                response = new MoveResponse(game.Board.LastMove, timeout);
            }
            return await Task.Factory.StartNew(() => response);
        }
    }
}
