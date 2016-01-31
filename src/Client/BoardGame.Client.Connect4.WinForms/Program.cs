using BoardGame.API;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Logger;
using BoardGame.Client.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoardGame.Domain.Entities.Bots;
using BoardGame.Domain.Interfaces;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BoardGame.Client.Connect4.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var bots = new List<IBot>
                {
                    new MediumBot(),
                    new EasyBot()
                };

                var fieldFactory = new FieldFactory();
                var board = new Board(7, 6, fieldFactory);
                var gameFactory = new GameFactory(board, bots);

                var playerFactory = new PlayerFactory();
                var proxy = new GameProxy();
                var logger = new Log4netAdapter("GameAPI");
                var api = new GameAPI(gameFactory, playerFactory, proxy, logger);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(api));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.ToString());
                Application.Exit();
            }
        }
    }
}
