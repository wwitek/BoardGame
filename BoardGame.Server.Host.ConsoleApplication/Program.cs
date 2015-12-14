using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Logger;
using BoardGame.Server.BusinessLogic;
using BoardGame.Server.BusinessLogic.Interfaces;
using BoardGame.Server.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
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

                ILogger serverLogicLogger = new Log4netAdapter("GameServer");
                ILogger gameServiceLogger = new Log4netAdapter("GameService");

                IGameServer serverLogic = new GameServer(gameFactory, playerFactory, serverLogicLogger);
                IContractBehavior contractBehavior = new GameServiceInstanceProvider(serverLogic, gameServiceLogger);
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

    public class Log4netAdapter : ILogger
    {
        private readonly ILog log;

        public Log4netAdapter(string loggerName)
        {
            log = LogManager.GetLogger(loggerName);
        }

        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Info)
                log.Info(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Warning)
                log.Warn(entry.Message, entry.Exception);
            else if (entry.Severity == LoggingEventType.Error)
                log.Error(entry.Message, entry.Exception);
            else
                log.Fatal(entry.Message, entry.Exception);
        }
    }
}
