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
    public partial class PageSinglePlayer : Page
    {
        private readonly Frame mainFrame;
        private readonly IGameAPI gameAPI;

        public PageSinglePlayer(Frame mainFrame, IGameAPI gameAPI = null)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            this.gameAPI = gameAPI;
        }
        
        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new PageGame(mainFrame, GameType.SinglePlayer, "Easy", gameAPI);
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new PageGame(mainFrame, GameType.SinglePlayer, "Medium", gameAPI);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.GoBack();
        }
    }
}
