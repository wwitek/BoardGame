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
using BoardGame.Client.Connect4.WPF.CustomControls;

namespace BoardGame.Client.Connect4.WPF.Pages
{
    /// <summary>
    /// Interaction logic for PageGame.xaml
    /// </summary>
    public partial class PageGame : Page
    {
        public PageGame()
        {
            InitializeComponent();
        }

        private async void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameBoard board = sender as GameBoard;
            int playerId = 1;
            for (int col = 0; col < 7; col++)
            {
                for (int row = 5; row >= 4; row--)
                {
                    playerId = (playerId == 1) ? 2 : 1;
                    board?.AnimateMove(playerId, row, col);
                    await Task.Delay(250);
                }
            }
            await Task.Delay(5000);
            board?.AnimateReset();

        }
    }
}
