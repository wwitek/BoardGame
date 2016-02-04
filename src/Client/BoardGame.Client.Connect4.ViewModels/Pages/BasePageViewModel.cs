using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Client.Connect4.ViewModels.Common;
using BoardGame.Client.Connect4.ViewModels.Interfaces;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public abstract class BasePageViewModel : ObservableObject
    {
        protected readonly INavigationService NavigationService;

        protected BasePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public ActionCommand GoBack
        {
            get { return new ActionCommand(x => NavigationService.GoBack()); }
        }

        public ActionCommand GoForward
        {
            get { return new ActionCommand(x => NavigationService.GoForward()); }
        }
    }
}
