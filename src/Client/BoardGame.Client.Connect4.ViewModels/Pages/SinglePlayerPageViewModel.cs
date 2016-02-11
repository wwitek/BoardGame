using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.Client.Connect4.ViewModels.Common;
using BoardGame.Client.Connect4.ViewModels.Interfaces;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class SinglePlayerPageViewModel : BasePageViewModel
    {
        public SinglePlayerPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }

        public new ICommand StartEasyGameCommand => new ActionCommand(x => NavigationService.Navigate("EasyGamePage"));
        public new ICommand StartMediumGameCommand => new ActionCommand(x => NavigationService.Navigate("MediumGamePage"));
    }
}
