using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.Client.Connect4.ViewModels.Common;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using INavigationService = BoardGame.Client.Connect4.ViewModels.Interfaces.INavigationService;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class SinglePlayerPageViewModel : BasePageViewModel
    {
        public SinglePlayerPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }

        public new ICommand StartEasyGameCommand => new RelayCommand(() => NavigationService.Navigate("EasyGamePage"));
        public new ICommand StartMediumGameCommand => new RelayCommand(() => NavigationService.Navigate("MediumGamePage"));
    }
}
