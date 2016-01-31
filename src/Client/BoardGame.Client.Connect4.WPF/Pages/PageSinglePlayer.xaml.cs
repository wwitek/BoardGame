using System.Windows;
using System.Windows.Controls;

namespace BoardGame.Client.Connect4.WPF.Pages
{
    /// <summary>
    /// Interaction logic for PageStart.xaml
    /// </summary>
    public partial class PageSinglePlayer : Page
    {
        private readonly INavigationController navigation;

        public PageSinglePlayer(INavigationController navigation)
        {
            InitializeComponent();
            this.navigation = navigation;
        }
        
        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate("easyPage");
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            navigation.Navigate("mediumPage");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            navigation.GameFrame.GoBack();
        }
    }
}
