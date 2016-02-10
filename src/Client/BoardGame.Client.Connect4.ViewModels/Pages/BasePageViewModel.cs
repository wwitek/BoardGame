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
            Debug.WriteLine("Parameterless " + GetType().Name + " created.");
        }

        public BasePageViewModel(INavigationService navigationService)
        {
            Debug.WriteLine(GetType().Name + " created.");
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

        public ICommand StartSinglePlayerCommand => null;
        public ICommand StartTwoPlayerGameCommand => null;
        public ICommand StartOnlineGameCommand => null;
        public ICommand StartEasyGameCommand => null;
        public ICommand StartMediumGameCommand => null;
    }
}
