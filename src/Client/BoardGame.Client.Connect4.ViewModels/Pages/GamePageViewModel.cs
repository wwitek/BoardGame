using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels.EventArgs;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Domain.Enums;
using GalaSoft.MvvmLight.Command;
using INavigationService = BoardGame.Client.Connect4.ViewModels.Interfaces.INavigationService;

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

            if (this.api != null)
                this.api.MoveReceived += OnMoveReceived;
        }

        public new ICommand LoadedCommand => new RelayCommand(StartGame);

        private void StartGame()
        {
            Debug.WriteLine("Loaded");
            api?.StartGame(type, level);
        }

        private void OnMoveReceived(object sender, MoveEventArgs e)
        {
        }

        public new ICommand ClickCommand => new RelayCommand<BoardEventArgs>(e => BoardClick(e.Column));
        //public new ICommand ClickCommand => new RelayCommand(() => BoardClick(1));
        private void BoardClick(int column)
        {
            Debug.WriteLine("BoardClick: " + column);
        }
    }
}