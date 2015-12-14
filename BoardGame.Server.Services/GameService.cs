using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BoardGame.Server.Contracts;
using BoardGame.Server.BusinessLogic.Interfaces;
using BoardGame.Server.Contracts.Responses;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using System.Threading;
using System.ComponentModel;
using BoardGame.Domain.Logger;

namespace BoardGame.Server.Services
{
    public class GameService : IGameService
    {
        IGameServer Logic { get; set; }
        Random RandomGenerator = new Random();

        public GameService(IGameServer logic)
        {
            Logic = logic;
        }

        private void TempLog(int id, string message)
        {
            Console.WriteLine("Player " + id + ". " + message + ".");
        }

        public async Task<OnlineGameResponse> OnlineGameRequest(int playerId)
        {
            OnlineGameResponse response = null;
            IPlayer rivalPlayer = null;
            IPlayer player = Logic.CreateNewPlayer(playerId);
            TempLog(player.OnlineId, "Request to play");

            Logic.WaitingPlayers.Add(player);
            if (Logic.WaitingPlayers.TryTake(out rivalPlayer, 5000, rp => rp.OnlineId != player.OnlineId))
            {
                TempLog(player.OnlineId, "There is somebody. My rival will be Player" + rivalPlayer.OnlineId);
                List<IPlayer> players = RandomGenerator.Next(2) == 0 ? new List<IPlayer>() { rivalPlayer, player }
                                                                     : new List<IPlayer>() { player, rivalPlayer };
                if (Logic.NewGame(players)) TempLog(player.OnlineId, "New game created");
                
                response = new OnlineGameResponse(player.OnlineId, true);
                TempLog(player.OnlineId, "Response created");
            }
            else
            {
                TempLog(player.OnlineId, "Queue is empty");
                Logic.WaitingPlayers.Remove(player);
                response = new OnlineGameResponse(player.OnlineId, false);
            }
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            IGame game = Logic.GetGameByPlayerId(playerId);
            bool isConfirmed = false;
            bool yourTurn = game.Players[0].OnlineId == playerId;
            TempLog(playerId, "Confrmed. " + (yourTurn ? "My turn" : "His turn"));

            ManualResetEvent waitHandle = new ManualResetEvent(false);
            PropertyChangedEventHandler stateEventHandler = null;
            stateEventHandler = (s, e) =>
            {
                TempLog(playerId, "Online status changed to " + game.State);
                if (game.State.Equals(GameState.Confirmed))
                {
                    game.OnStateChanged -= stateEventHandler;
                    TempLog(playerId, "New waitHandle set"); waitHandle.Set();
                }
            };
            game.OnStateChanged += stateEventHandler;
            Logic.ConfirmPlayer(playerId);
            isConfirmed = waitHandle.WaitOne(10000);

            TempLog(playerId, (isConfirmed ? "Confirmed by other player" : "Timeout! Will wait again"));
            if (!isConfirmed) Logic.RunningGames.Remove(game);

            StartGameResponse response = new StartGameResponse(isConfirmed, playerId, yourTurn);
            TempLog(playerId, "ConfirmToPlay responsed: isConfirmed=" + isConfirmed);
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> GetFirstMove(int playerId)
        {
            TempLog(playerId, "Waiting for the first move..");

            IGame game = Logic.GetGameByPlayerId(playerId);
            bool timeout = game.WaitForNextPlayer(5 * 60 * 1000);

            MoveResponse response = new MoveResponse(game.LastMove, timeout);
            if (!timeout) TempLog(playerId, "Got move in column=" + response.MoveMade.Column);
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> MakeMove(int playerId, int row, int column)
        {
            TempLog(playerId, "Moved in column=" + column);

            IGame game = Logic.GetGameByPlayerId(playerId);
            game.MakeMove(row, column);
            bool timeout = game.WaitForNextPlayer(5 * 60 * 1000);

            MoveResponse response = new MoveResponse(game.LastMove, timeout);
            return await Task.Factory.StartNew(() => response);
        }
    }
}
