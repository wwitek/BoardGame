using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Client.Connect4.ViewModels.Pages;
using BoardGame.Client.Connect4.WPF.Properties;
using BoardGame.Client.Connect4.WPF.Views;
using BoardGame.Client.Connect4.WPF.Views.Pages;
using BoardGame.Client.Proxies;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Entities.Bots;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using Ninject;
using Ninject.Modules;

namespace BoardGame.Client.Connect4.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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

                IKernel kernel = new StandardKernel();
                var modules = new List<INinjectModule>
                {
                    new GameModule()
                };
                kernel.Load(modules);

                MainWindow mainWindow = kernel.Get<MainWindow>();
                INavigationService navigation = kernel.Get<INavigationService>();

                navigation.InjectPage("StartPage", kernel.Get<StartPageViewModel>());
                navigation.InjectPage("SinglePlayerPage", kernel.Get<SinglePlayerPageViewModel>());
                navigation.InjectPage("TwoPlayerGamePage", kernel.Get<GamePageViewModel>());
                navigation.InjectPage("OnlineGamePage", kernel.Get<GamePageViewModel>());
                navigation.InjectPage("EasyGamePage", kernel.Get<GamePageViewModel>());
                navigation.InjectPage("MediumGamePage", kernel.Get<GamePageViewModel>());

                navigation.Navigate("StartPage");
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(e.ToString());
            }
        }
    }
}
