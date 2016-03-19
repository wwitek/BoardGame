using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels.EventArguments.Views;
using BoardGame.Client.Connect4.ViewModels.EventArguments.ViewModels;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Domain.Enums;
using GalaSoft.MvvmLight.Command;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Entities;

namespace BoardGame.Client.Connect4.ViewModels.Pages
{
    public class GamePageViewModel : BasePageViewModel
    {
        private readonly GameAPI api;
        private readonly GameType type;
        private readonly string level;
        private EventHandler<MoveReceivedEventArgs> moveReceived;
        private EventHandler<BoardResetEventArgs> boardReset;

        public new ICommand LoadedCommand => new RelayCommand(Load);
        public new ICommand UnloadedCommand => new RelayCommand(Unload);
        public new ICommand ClickedCommand => new RelayCommand<BoardClickEventArgs>(o => Click(o.ColumnClicked));
        public new ICommand ResetCommand => new RelayCommand(Reset);

        public event EventHandler<MoveReceivedEventArgs> MoveReceived
        {
            add
            {
                if (moveReceived == null)
                    moveReceived += value;
            }
            remove
            {
                moveReceived -= value;
            }
        }
        public event EventHandler<BoardResetEventArgs> BoardReset
        {
            add
            {
                if (boardReset == null)
                    boardReset += value;
            }
            remove
            {
                boardReset -= value;
            }
        }

        public GamePageViewModel(INavigationService navigationService, GameAPI api, GameType type, string level = null)
            : base(navigationService)
        {
            this.api = api;
            this.api.MoveReceived += Api_MoveReceived;
            this.type = type;
            this.level = level;
        }

        private void Load()
        {
            TriggerBoardReset();
            Debug.WriteLine("VM: Loaded");
            api?.StartGame(type, level);
        }

        private void Unload()
        {
            Debug.WriteLine("VM: Unload");
            api?.Close();
        }

        private async void Click(int c)
        {
            Debug.WriteLine("VM: Clicked: " + c);
            bool isGameFinished = await api?.NextMove(0, c);
            if (isGameFinished) Reset();
        }

        private void Reset()
        {
            TriggerBoardReset(true);
            api?.StartGame(type, level);
        }

        private void Api_MoveReceived(object sender, MoveEventArgs e)
        {
            Debug.WriteLine("VM: Api_MoveReceived");
            TriggerMove(e.Move);
        }

        private void TriggerMove(IMove move)
        {
            MoveReceivedEventArgs args = new MoveReceivedEventArgs { MoveMade = move };
            moveReceived?.Invoke(this, args);
        }

        private void TriggerBoardReset(bool isAnimate = false)
        {
            BoardResetEventArgs args = new BoardResetEventArgs { IsAnimate = isAnimate };
            boardReset?.Invoke(this, args);
        }
    }
}
