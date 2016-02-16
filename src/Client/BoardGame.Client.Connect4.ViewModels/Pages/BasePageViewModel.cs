using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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

        public ICommand GoBack => new RelayCommand(() => NavigationService.GoBack());
        public ICommand GoForward => new RelayCommand(() => NavigationService.GoForward());
        public ICommand StartSinglePlayerCommand => null;
        public ICommand StartTwoPlayerGameCommand => null;
        public ICommand StartOnlineGameCommand => null;
        public ICommand StartEasyGameCommand => null;
        public ICommand StartMediumGameCommand => null;

        public ICommand LoadedCommand => null;
    }
}