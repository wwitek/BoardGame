using System.Windows;
using System.Windows.Controls;

namespace BoardGame.Client.Connect4.WPF.Pages
{
    /// <summary>
    /// Interaction logic for PageStart.xaml
    /// </summary>
    public partial class PageStart : Page
    {
        private readonly INavigationController navigation;

        public PageStart(INavigationController navigation)
        {
            InitializeComponent();
            this.navigation = navigation;
        }

        private void SinglePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate("singlePlayerPage");
        }

        private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate("twoPlayersPage");
        }

        private void OnlineGameButton_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate("onlineGamePage");
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}