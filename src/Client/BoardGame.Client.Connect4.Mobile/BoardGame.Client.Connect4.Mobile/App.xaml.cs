using BoardGame.API;
using BoardGame.Client.Connect4.Mobile.NinjectModules;
using BoardGame.Client.Connect4.Mobile.Views;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using Xamarin.Forms;
using BoardGame.Client.Connect4.ViewModels.Pages;

namespace BoardGame.Client.Connect4.Mobile
{
    public partial class App : Application
    {
        private readonly GameAPI api;

        public App()
        {
            InitializeComponent();

            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new GameModule()
            };

            kernel.Bind<StartPage>().ToSelf().InSingletonScope();
            kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope()
                .WithConstructorArgument("currentPage", kernel.Get<StartPage>());

            kernel.Bind<StartPageViewModel>().To<StartPageViewModel>();
            kernel.Bind<SinglePlayerPageViewModel>().To<SinglePlayerPageViewModel>();
            kernel.Load(modules);

            INavigationService navigation = kernel.Get<INavigationService>();
            navigation.InjectPage("StartPage", kernel.Get<StartPageViewModel>());
            navigation.InjectPage("SinglePlayerPage", kernel.Get<SinglePlayerPageViewModel>());

            api = kernel.Get<GameAPI>();
            MainPage = new NavigationPage(kernel.Get<StartPage>());
            navigation.Navigate("StartPage");
        }

        protected override async void OnStart()
        {
            if (await api.VerifyConnection())
            {
                Debug.WriteLine("Connection verified!");
            }
            else
            {
                Debug.WriteLine("No connection!");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
