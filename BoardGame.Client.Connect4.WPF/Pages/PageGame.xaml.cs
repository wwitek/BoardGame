using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BoardGame.API;
using BoardGame.Client.Connect4.WPF.CustomControls;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Client.Connect4.WPF.Pages
{
    /// <summary>
    /// Interaction logic for PageGame.xaml
    /// </summary>
    public partial class PageGame : Page
    {
        private readonly Frame mainFrame;
        private readonly IGameAPI gameAPI;
        private readonly GameType type;
        private readonly string level;
        private static readonly Stopwatch MoveStopwatch = new Stopwatch();
        private bool isAnimating;

        public PageGame(Frame mainFrame, GameType type, string level = "", IGameAPI gameAPI = null)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            this.gameAPI = gameAPI;
            this.type = type;
            this.level = level;
        }

        private void PageGame_OnLoaded(object sender, RoutedEventArgs rea)
        {
            if (gameAPI != null)
                gameAPI.OnMoveReceived += GameAPIOnOnMoveReceived;

            GameBoard.Reset();
            gameAPI?.StartGame(type, level);
        }

        private async void GameAPIOnOnMoveReceived(object sender, MoveEventArgs moveEventArgs)
        {
            isAnimating = true;
            if (MoveStopwatch.IsRunning && moveEventArgs.Move.IsBot)
            {
                int elapsed = (int)MoveStopwatch.ElapsedMilliseconds;
                if (elapsed < 1000)
                {
                    await Task.Delay(1500 - elapsed);
                }
            }
            MoveStopwatch.Reset();
            MoveStopwatch.Start();
            await AnimateMove(moveEventArgs.Move);
            isAnimating = false;
        }

        private async Task<bool> AnimateMove(IMove result)
        {
            if (result != null && result.Row >= 0)
            {
                await GameBoard.AnimateMove(result.PlayerId, result.Row, result.Column, result.IsBot);
                if (result.IsConnected) MessageBox.Show("Success! Player " + result.PlayerId + " won!");
            }
            if (result != null && result.IsTie) MessageBox.Show("Game Over!");
            return true;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameBoard board = sender as GameBoard;
            if (gameAPI == null || board == null || isAnimating) return;
            gameAPI.NextMove(0, board.ColumnClicked);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            gameAPI.OnMoveReceived -= GameAPIOnOnMoveReceived;
            GameBoard.Reset();
            mainFrame.GoBack();
        }

        private async void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            await GameBoard.AnimateReset();
            gameAPI?.StartGame(type, level);
        }
    }
}
