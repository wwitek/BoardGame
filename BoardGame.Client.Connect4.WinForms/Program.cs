using BoardGame.API;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Logger;
using BoardGame.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            var fieldFactory = new FieldFactory();
            var board = new Board(7, 6, fieldFactory);
            var playerFactory = new PlayerFactory();
            var gameFactory = new GameFactory(board);
            var proxy = new GameProxy();
            var logger = new Log4netAdapter("GameAPI");
            var gameAPI = new GameAPI(gameFactory, playerFactory, proxy, logger);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(gameAPI));
        }
    }
}
