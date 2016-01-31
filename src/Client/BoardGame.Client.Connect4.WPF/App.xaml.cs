using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BoardGame.API;
using BoardGame.Client.Connect4.WPF.Pages;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Entities.Bots;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using BoardGame.Client.Proxies;
using BoardGame.Server.Contracts;

namespace BoardGame.Client.Connect4.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
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
                //var logger = new Log4netAdapter("GameAPI");
                var api = new GameAPI(gameFactory, playerFactory, proxy);

                MainWindow window = new MainWindow(api);
                window.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(e.ToString());
            }
        }
    }
}
