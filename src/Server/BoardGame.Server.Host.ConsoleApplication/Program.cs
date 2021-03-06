﻿using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Logger;
using BoardGame.Server.Business;
using BoardGame.Server.Business.Interfaces;
using BoardGame.Server.Services;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BoardGame.Server.Host.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Uri address = new Uri("net.tcp://localhost:8002");
                Uri address = new Uri("http://localhost:9003/GameService");
                Uri addressAsync = new Uri("http://localhost:9004/GameService");

                var fieldFactory = new FieldFactory();
                var board = new Board(7, 6, fieldFactory);
                var playerFactory = new PlayerFactory();
                var gameFactory = new GameFactory(board);

                ILogger serverLogicLogger = new Log4netAdapter("GameServer");
                ILogger gameServiceLogger = new Log4netAdapter("GameService");
                IGameServer serverLogic = new GameServer(gameFactory, playerFactory, serverLogicLogger);

                IContractBehavior contractBehavior = new GameServiceInstanceProvider(typeof(GameService), serverLogic, gameServiceLogger);
                IErrorHandler errorHandler = new GameServiceErrorHandler(gameServiceLogger);
                ServiceHost host = new GameServiceHost(contractBehavior, errorHandler, "IGameService", typeof(GameService), address);
                host.Open();

                IContractBehavior contractBehaviorAsync = new GameServiceInstanceProvider(typeof(GameServiceAsync), serverLogic, gameServiceLogger);
                using (ServiceHost hostAsync =
                    new GameServiceHost(contractBehaviorAsync, errorHandler, "IGameServiceAsync", typeof(GameServiceAsync), addressAsync))
                {
                    hostAsync.Open();

                    Console.WriteLine("Running... Press key to stop");
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
