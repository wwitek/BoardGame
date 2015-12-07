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

        public async Task<OnlineGameResponse> OnlineGameRequest(int playerId)
        {
            OnlineGameResponse response = null;
            IPlayer player = Logic.CreateNewPlayer(playerId);
            Logic.WaitingPlayers.Add(player);


            Console.WriteLine("New Player" + player.OnlineId);
            if (Logic.WaitingPlayers.Count(p => p.OnlineId != player.OnlineId) == 0)
            {
                Console.WriteLine("Player" + player.OnlineId + ". Nobody here. Will have to wait.");
                response = new OnlineGameResponse(GameState.WaitingForPlayer, player.OnlineId);
                await Task.Delay(5000);
                Console.WriteLine("Times up for Player" + player.OnlineId);
            }

            if (Logic.WaitingPlayers.Count(p => p.OnlineId != player.OnlineId) == 0)
            {
                Console.WriteLine("Player" + player.OnlineId + ". Still nobody here. Remove from the list.");
                Logic.WaitingPlayers.Remove(player);
            }
            else
            {
                Console.WriteLine("Player" + player.OnlineId + ". There is somebody!");
                response = new OnlineGameResponse(GameState.ReadyForOnlineGame, player.OnlineId);
            }

            return await Task.Factory.StartNew(() => response);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            StartGameResponse response = new StartGameResponse(GameState.New, 1, true);



            return await Task.Factory.StartNew(() => response);
        }

        public async Task<MoveResponse> MakeMove(int playerId, int row, int column)
        {
            MoveResponse response = new MoveResponse(null);



            return await Task.Factory.StartNew(() => response);
        }
    }
}
