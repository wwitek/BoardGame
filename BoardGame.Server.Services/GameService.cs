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

namespace BoardGame.Server.Services
{
    public class GameService : IGameService
    {
        IGameServer Logic { get; set; }
        Random RandomGenerator = new Random();

        public GameService(IGameServer logic)
        {
            Logic = logic;
            Console.WriteLine("GameService with logic created...");
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
                TempLog(player.OnlineId, "Players created");

                if (Logic.NewGame(players))
                {
                    TempLog(player.OnlineId, "New game created");
                }

                if (Logic.GetGameByPlayerId(player.OnlineId).Equals(Logic.GetGameByPlayerId(rivalPlayer.OnlineId)))
                {
                    TempLog(player.OnlineId, "Game created correctly");
                }

                response = new OnlineGameResponse(GameState.Ready, player.OnlineId);
                TempLog(player.OnlineId, "Response created");
            }
            else
            {
                TempLog(player.OnlineId, "Queue is empty");
                Logic.WaitingPlayers.Remove(player);
                response = new OnlineGameResponse(GameState.Waiting, player.OnlineId);
            }
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            TempLog(playerId, "ConfirmToPlay");
            IGame game = null;
            game = Logic.GetGameByPlayerId(playerId);

            bool yourTurn = game.Players[0].OnlineId == playerId;
            TempLog(playerId, "Confrmed. " + (yourTurn ? "My turn" : "His turn"));

            if (game.State.Equals(GameState.Ready))
            {
                TempLog(playerId, "Game was Ready. Will change it to Confirming");
                game.State = GameState.Confirming;
                TempLog(playerId, "Game is Confirming state");

                ManualResetEvent waitHandle = new ManualResetEvent(false);
                PropertyChangedEventHandler stateEventHandler = null;
                stateEventHandler = (s, e) =>
                {
                    TempLog(playerId, "State changed");
                    game.OnStateChanged -= stateEventHandler;
                    if (game.State.Equals(GameState.New)) TempLog(playerId, "New waitHandle set"); waitHandle.Set();
                };
                game.OnStateChanged += stateEventHandler;
                TempLog(playerId, "Waiting for other guy to confim..");

                game.State = GameState.Confirmed;
                if (!waitHandle.WaitOne(60000))
                {
                    TempLog(playerId, "Timeout");
                    game.State = GameState.Waiting;
                    Logic.RunningGames.Remove(game);
                }
                else
                {
                    TempLog(playerId, "Confirmed by other player");
                }
            }
            else if (game.State.Equals(GameState.Confirming))
            {
                TempLog(playerId, "Game is still in Confirming process. Will wait till it's finished");
                ManualResetEvent waitHandle = new ManualResetEvent(false);
                PropertyChangedEventHandler stateEventHandler = null;
                stateEventHandler = (s, e) =>
                {
                    TempLog(playerId, "State changed");
                    game.OnStateChanged -= stateEventHandler;
                    if (game.State.Equals(GameState.Confirmed)) TempLog(playerId, "Confirmed waitHandle set"); waitHandle.Set();
                };
                game.OnStateChanged += stateEventHandler;

                if (game.State.Equals(GameState.Confirmed))
                {
                    game.State = GameState.Confirmed;
                }

                if (!waitHandle.WaitOne(60000))
                {
                    TempLog(playerId, "Timeout");
                    game.State = GameState.Waiting;
                    Logic.RunningGames.Remove(game);
                }
                else
                {
                    TempLog(playerId, "Confirmed by other player");
                }
                game.State = GameState.New;
            }
            else if (game.State.Equals(GameState.Confirmed))
            {
                TempLog(playerId, "Game was Confirmed. Will change it to New");
                game.State = GameState.New;
            }
            else
            {
                //TODO: Any other state shouldn't happened
            }

            StartGameResponse response = new StartGameResponse(game.State, playerId, yourTurn);
            TempLog(playerId, "ConfirmToPlay responsed: " + response.State);
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

        public async Task<MoveResponse> GetFirstMove(int playerId)
        {
            TempLog(playerId, "Waiting for the first move..");

            IGame game = Logic.GetGameByPlayerId(playerId);
            bool timeout = game.WaitForNextPlayer(5 * 60 * 1000);

            MoveResponse response = new MoveResponse(game.LastMove, timeout);
            if (!timeout) TempLog(playerId, "Got move in column=" + response.MoveMade.Column);
            return await Task.Factory.StartNew(() => response);
        }
    }
}
