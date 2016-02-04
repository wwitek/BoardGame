using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Client.Connect4.ViewModels.Interfaces;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class SinglePlayerPageViewModel : BasePageViewModel
    {
        public SinglePlayerPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
        }
    }


}
