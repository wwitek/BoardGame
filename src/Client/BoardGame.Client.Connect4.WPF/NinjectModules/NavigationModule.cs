using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Client.Connect4.ViewModels.Pages;
using BoardGame.Client.Connect4.WPF.Views;
using BoardGame.Domain.Enums;
using Ninject;
using Ninject.Modules;
using INavigationService = BoardGame.Client.Connect4.ViewModels.Interfaces.INavigationService;
using NavigationService = BoardGame.Client.Connect4.WPF.NavigationService;

namespace BoardGame.Client.Connect4.WPF.NinjectModules
{
    public class NavigationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindow>().ToSelf().InSingletonScope();
            MainWindow mainWindow = Kernel.Get<MainWindow>();

            Bind<INavigationService>().To<NavigationService>().InSingletonScope()
                .WithConstructorArgument("frame", mainWindow.MainFrame);

            Bind<StartPageViewModel>().To<StartPageViewModel>();
            Bind<SinglePlayerPageViewModel>().To<SinglePlayerPageViewModel>();

            Bind<GamePageViewModel>().To<GamePageViewModel>()
                .Named("EasyGamePage")
                .WithConstructorArgument("type", GameType.SinglePlayer)
                .WithConstructorArgument("level", "Easy");

            Bind<GamePageViewModel>().To<GamePageViewModel>()
                .Named("MediumGamePage")
                .WithConstructorArgument("type", GameType.SinglePlayer)
                .WithConstructorArgument("level", "Medium");

            Bind<GamePageViewModel>().To<GamePageViewModel>()
                .Named("TwoPlayerGamePage")
                .WithConstructorArgument("type", GameType.TwoPlayers)
                .WithConstructorArgument("level", "");

            Bind<GamePageViewModel>().To<GamePageViewModel>()
                .Named("OnlineGamePage")
                .WithConstructorArgument("type", GameType.Online)
                .WithConstructorArgument("level", "");
        }
    }
}
