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

namespace BoardGame.Client.Connect4.WPF
{
    public class GameModule : NinjectModule
    {
        public GameModule()
        {
            Debug.WriteLine(GetType().Name + " created.");
        }

        public override void Load()
        {
            Bind<MainWindow>().ToSelf().InSingletonScope();

            MainWindow mainWindow = Kernel.Get<MainWindow>();
            Bind<INavigationService>().To<NavigationService>().InSingletonScope()
                .WithConstructorArgument("frame", mainWindow.MainFrame);
            
            INavigationService navigationService = Kernel.Get<INavigationService>();

            Bind<StartPageViewModel>().To<StartPageViewModel>()
                .WithConstructorArgument("navigationService", navigationService);

            Bind<SinglePlayerPageViewModel>().To<SinglePlayerPageViewModel>()
                .WithConstructorArgument("navigationService", navigationService);

            Bind<GamePageViewModel>().To<GamePageViewModel>()
                .WithConstructorArgument("navigationService", navigationService)
                .WithConstructorArgument("type", GameType.SinglePlayer)
                .WithConstructorArgument("level", "Easy");
        }
    }
}
