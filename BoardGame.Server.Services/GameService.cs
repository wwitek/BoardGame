using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BoardGame.Server.Contracts;
using BoardGame.Server.BusinessLogic.Interfaces;

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

        public int GetNextMove()
        {
            return Logic.GetColumn();
        }

        public async Task<int> GetNextMove2Async()
        {
            await Task.Delay(2000);
            return await Task.Factory.StartNew(() => Logic.GetColumn());
        }
    }
}
