using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using BoardGame.Domain.Enums;

namespace BoardGame.Client.Connect4.WPF.Pages
{
    /// <summary>
    /// Interaction logic for PageStart.xaml
    /// </summary>
    public partial class PageStart : Page
    {
        private readonly Frame mainFrame;
        private readonly IGameAPI gameAPI;

        public PageStart(Frame mainFrame, IGameAPI gameAPI = null)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            this.gameAPI = gameAPI;
        }

        private void SinglePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new PageSinglePlayer(mainFrame, gameAPI);
        }

        private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new PageGame(mainFrame, GameType.TwoPlayers, "", gameAPI));
        }

        private void OnlineGameButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new PageGame(mainFrame, GameType.Online, "", gameAPI));
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
