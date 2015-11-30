using BoardGame.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
                ServiceHost host = new ServiceHost(typeof(GameService), new Uri("net.tcp://localhost:8002"));
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
