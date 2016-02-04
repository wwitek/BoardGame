using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BoardGame.API;
using BoardGame.Client.Connect4.WPF.Properties;
using BoardGame.Client.Connect4.WPF.Views;
using BoardGame.Client.Connect4.WPF.Views.Pages;
using BoardGame.Client.Proxies;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Entities.Bots;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;

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

                ViewModelLocator locator = Resources.MergedDictionaries
                    .FirstOrDefault(k => k.Values.OfType<ViewModelLocator>().Any())?
                    .Values.OfType<ViewModelLocator>().FirstOrDefault() ??
                                           Resources.Values.OfType<ViewModelLocator>().FirstOrDefault();

                if (locator == null) throw new NoNullAllowedException("ViewModelLocator cannot be null.");

                MainWindow mainWindow = new MainWindow();
                var navigation = new NavigationService(mainWindow.MainFrame);

                locator.SetGameAPI(api);
                locator.SetNavigationService(navigation);
                navigation.Navigate<StartPage>();

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
