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
                IGameServer serverLogic = new GameServer();
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
}
