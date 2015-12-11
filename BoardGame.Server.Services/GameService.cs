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

            rivalPlayer = Logic.GetAvailablePlayer(player.OnlineId);
            if (rivalPlayer == null)
            {
                Logic.WaitingPlayers.Add(player);


                TempLog(player.OnlineId, "Queue is empty");
                response = new OnlineGameResponse(GameState.Waiting, player.OnlineId);
                await Task.Delay(5000);


            }

            IGame myGame = Logic.GetGameByPlayerId(player.OnlineId);
            if (myGame != null)
            {
                rivalPlayer = myGame.Players.First(p => p.OnlineId != player.OnlineId);
                TempLog(player.OnlineId, "Player"  + rivalPlayer.OnlineId + " added me");

                Logic.WaitingPlayers.Remove(player);
                Logic.WaitingPlayers.Remove(rivalPlayer);
                
                response = new OnlineGameResponse(GameState.Ready, player.OnlineId);
            }
            else
            {
                rivalPlayer = Logic.GetAvailablePlayer(player.OnlineId);
                if (rivalPlayer == null)
                {
                    TempLog(player.OnlineId, "Queue is still empty. Remove from the list.");
                    Logic.WaitingPlayers.Remove(player);
                }
                else
                {
                    TempLog(player.OnlineId, "There is somebody. My rival will be Player" + rivalPlayer.OnlineId);
                    List<IPlayer> players = RandomGenerator.Next(2) == 0 ? new List<IPlayer>() { rivalPlayer, player }
                                                                         : new List<IPlayer>() { player, rivalPlayer };
                    Logic.NewGame(players);
                    response = new OnlineGameResponse(GameState.Ready, player.OnlineId);
                }
            }
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            IGame game = Logic.GetGameByPlayerId(playerId);
            bool yourTurn = game.Players[0].OnlineId == playerId;
            TempLog(playerId, "Confrmed. " + (yourTurn ? "My turn" : "His turn"));

            if (game.State.Equals(GameState.Ready))
            {
                game.State = GameState.Confirmed;
                ManualResetEvent waitHandle = new ManualResetEvent(false);
                PropertyChangedEventHandler stateEventHandler = null;
                stateEventHandler = (s, e) =>
                {
                    game.OnStateChanged -= stateEventHandler;
                    if (game.State.Equals(GameState.New)) waitHandle.Set();
                };
                game.OnStateChanged += stateEventHandler;

                TempLog(playerId, "Waiting for other guy to confim");

                if (!waitHandle.WaitOne(5000))
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
            else if (game.State.Equals(GameState.Confirmed))
            {
                game.State = GameState.New;
            }
            else
            {
                //TODO: Any other state shouldn't happened
            }

            StartGameResponse response = new StartGameResponse(game.State, playerId, yourTurn);
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
            IGame game = Logic.GetGameByPlayerId(playerId);
            bool timeout = game.WaitForNextPlayer(5 * 60 * 1000);

            MoveResponse response = new MoveResponse(game.LastMove, timeout);
            TempLog(playerId, "Got move in column=" + response.MoveMade.Column);
            return await Task.Factory.StartNew(() => response);
        }
    }
}
