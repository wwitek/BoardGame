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

using Xamarin.Forms;

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
            kernel.Load(modules);
            api = kernel.Get<GameAPI>();

            MainPage = new StartPage();
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
