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

namespace BoardGame.Server.Services
{
    public class GameService : IGameService
    {
        IGameServer Logic { get; set; }

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
                TempLog(player.OnlineId, "Times up");
            }

            IGame myGame = Logic.GetGameByPlayerId(player.OnlineId);
            if (myGame != null)
            {
                TempLog(player.OnlineId, "Somebody added me!");
                rivalPlayer = myGame.Players.First(p => p.OnlineId != player.OnlineId);
                TempLog(player.OnlineId, "My rival is Player" + rivalPlayer.OnlineId);

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
                    TempLog(player.OnlineId, "There is somebody!");
                    TempLog(player.OnlineId, "My rival is Player" + rivalPlayer.OnlineId);

                    List<IPlayer> players = new List<IPlayer>()
                    {
                        rivalPlayer,
                        player
                    };

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
            TempLog(playerId, yourTurn ? "My turn!" : "His turn:(");

            game.State++;
            TempLog(playerId, "Confrmed!");
            while (!game.State.Equals(GameState.New))
            {
                TempLog(playerId, "Waiting for other guy to confim");
                await Task.Delay(1000);
                if (game.State.Equals(GameState.New)) break;

                TempLog(playerId, "Still waiting...");
                await Task.Delay(2000);
                if (game.State.Equals(GameState.New)) break;

                TempLog(playerId, "Still waiting...");
                await Task.Delay(3000);
                if (game.State.Equals(GameState.New)) break;

                TempLog(playerId, "Still waiting...");
                await Task.Delay(4000);
                if (game.State.Equals(GameState.New)) break;

                // Something's wrong with another guy. Need to keep waiting for another one
                TempLog(playerId, "Something's wrong with another guy. Need to keep waiting for another one");
                game.State = GameState.Waiting;
                Logic.RunningGames.Remove(game);
                break;
            }

            StartGameResponse response = new StartGameResponse(game.State, playerId, yourTurn);
            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> MakeMove(int playerId, int row, int column)
        {
            MoveResponse response = new MoveResponse(column);
            TempLog(playerId, "Moved in column=" + column);

            IGame game = Logic.GetGameByPlayerId(playerId);
            IMove move = game.MakeMove(row, column);

            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> GetMove(int playerId)
        {
            IGame game = Logic.GetGameByPlayerId(playerId);

            while(!game.NextPlayer.OnlineId.Equals(playerId))
            {
                TempLog(playerId, "Waiting for move...");
                await Task.Delay(2000);
            }
            
            MoveResponse response = new MoveResponse(game.LastMove.Column);
            TempLog(playerId, "Got move in column=" + response.ClickedColumn);

            return await Task.Factory.StartNew(() => response);
        }
    }
}
