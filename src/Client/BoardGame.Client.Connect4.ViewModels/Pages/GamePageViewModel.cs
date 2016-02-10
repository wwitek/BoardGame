using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Domain.Enums;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class GamePageViewModel : BasePageViewModel
    {
        public GamePageViewModel(INavigationService navigationService, GameType type, string level = "") 
            : base(navigationService)
        {
        }
    }
}
