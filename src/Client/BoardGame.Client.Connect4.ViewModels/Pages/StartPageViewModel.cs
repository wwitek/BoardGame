using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.Client.Connect4.ViewModels.Common;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Domain.Enums;
using GalaSoft.MvvmLight.Command;
using INavigationService = BoardGame.Client.Connect4.ViewModels.Interfaces.INavigationService;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class StartPageViewModel : BasePageViewModel
    {
        public StartPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }

        public new ICommand StartSinglePlayerCommand => new RelayCommand(() => NavigationService.Navigate("SinglePlayerPage"));
        public new ICommand StartTwoPlayerGameCommand => new RelayCommand(() => NavigationService.Navigate("TwoPlayerGamePage"));
        public new ICommand StartOnlineGameCommand => new RelayCommand(() => NavigationService.Navigate("OnlineGamePage"));
    }
}