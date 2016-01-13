using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public PageGame(Frame mainFrame, GameType type, string level = "", IGameAPI gameAPI = null)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            this.gameAPI = gameAPI;
            if (gameAPI != null)
                gameAPI.OnMoveReceived += (s, e) => AnimateMove(e.Move);
            this.gameAPI?.StartGame(type, level);
        }

        private void AnimateMove(IMove result)
        {
            if (result != null && result.Row >= 0)
            {
                GameBoard.AnimateMove(result.PlayerId, result.Row, result.Column);
                if (result.IsConnected) MessageBox.Show("Success! Player " + result.PlayerId + " won!");
            }
            if (result != null && result.IsTie) MessageBox.Show("Game Over!");
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameBoard board = sender as GameBoard;
            if (gameAPI == null || board == null) return;
            gameAPI.NextMove(0, board.ColumnClicked);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.GoBack();
        }
    }
}
