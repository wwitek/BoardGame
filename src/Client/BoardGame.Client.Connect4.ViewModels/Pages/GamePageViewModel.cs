using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels.EventArguments;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Domain.Enums;
using GalaSoft.MvvmLight.Command;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class GamePageViewModel : BasePageViewModel
    {
        private readonly GameAPI api;
        private readonly GameType type;
        private readonly string level;

        public GamePageViewModel(INavigationService navigationService, GameAPI api, GameType type, string level = null)
            : base(navigationService)
        {
            this.api = api;
            this.type = type;
            this.level = level;
        }

        public new ICommand LoadedCommand => new RelayCommand(Loaded);
        private void Loaded()
        {
            Debug.WriteLine("Loaded");
        }

        public new ICommand ClickedCommand => new RelayCommand<BoardEventArgs>(o => Clicked(o.ColumnClicked));
        private void Clicked(int c)
        {
            Debug.WriteLine("Clicked: " + c);
        }
    }
}
