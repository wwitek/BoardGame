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
        private Frame mainFrame;
        int playerId = 1;

        public PageGame(Frame mainFrame)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameBoard board = sender as GameBoard;
            board?.AnimateMove(playerId, 5, board.ColumnClicked);
            playerId = (playerId == 1) ? 2 : 1;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new PageStart(mainFrame);
        }
    }
}
