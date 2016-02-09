using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Client.Connect4.ViewModels.Common;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Domain.Enums;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class StartPageViewModel : BasePageViewModel
    {
        public StartPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {

        }

        public ActionCommand StartSinglePlayerCommand
        {
            get
            {
                return new ActionCommand(x => NavigationService.Navigate("SinglePlayerPage"));
            }
        }

        public ActionCommand StartTwoPlayerCommand
        {
            get { return new ActionCommand(x => NavigationService.Navigate(new GamePageViewModel(NavigationService, GameType.TwoPlayers))); }
        }
    }
}
