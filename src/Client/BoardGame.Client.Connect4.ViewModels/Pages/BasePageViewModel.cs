using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.Client.Connect4.ViewModels.Common;
using BoardGame.Client.Connect4.ViewModels.Interfaces;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class BasePageViewModel : ObservableObject, IPageViewModel
    {
        protected readonly INavigationService NavigationService;

        public BasePageViewModel()
        {
        }
        public BasePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public ICommand GoBack => new ActionCommand(x => NavigationService.GoBack());
        public ICommand GoForward => new ActionCommand(x => NavigationService.GoForward());
        public ICommand StartSinglePlayerCommand => null;
        public ICommand StartTwoPlayerGameCommand => null;
        public ICommand StartOnlineGameCommand => null;
        public ICommand StartEasyGameCommand => null;
        public ICommand StartMediumGameCommand => null;

        public ICommand LoadedCommand => null;
        public ICommand BoardClickCommand => null;
    }
}