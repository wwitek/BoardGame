using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Logger;
using BoardGame.Server.BusinessLogic;
using BoardGame.Server.BusinessLogic.Interfaces;
using BoardGame.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Server.Host.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fieldFactory = new FieldFactory();
                var board = new Board(7, 6, fieldFactory);
                var playerFactory = new PlayerFactory();
                var gameFactory = new GameFactory(board);
                var logger = new MyLogger();

                IGameServer serverLogic = new GameServer(gameFactory, playerFactory, logger);
                IContractBehavior contractBehavior = new GameServiceInstanceProvider(serverLogic);
                ServiceHost host = new GameServiceHost(contractBehavior, typeof(GameService), new Uri("net.tcp://localhost:8002"));
                host.Open();

                Console.WriteLine("Running... Press key to stop");
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }

    public class MyLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
            Console.WriteLine(entry.Message);
        }
    }
}
