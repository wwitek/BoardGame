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
using BoardGame.Client.Connect4.WPF.Pages;
using BoardGame.Domain.Enums;

namespace BoardGame.Client.Connect4.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IGameAPI gameAPI = null)
        {
            InitializeComponent();

            INavigationController navigation = new NavigationController(MainFrame);
            navigation.Pages.Add("singlePlayerPage", new PageSinglePlayer(navigation));
            navigation.Pages.Add("twoPlayersPage", new PageGame(MainFrame, GameType.TwoPlayers, "", gameAPI));
            navigation.Pages.Add("onlineGamePage", new PageGame(MainFrame, GameType.Online, "", gameAPI));
            navigation.Pages.Add("easyPage", new PageGame(MainFrame, GameType.SinglePlayer, "Easy", gameAPI));
            navigation.Pages.Add("mediumPage", new PageGame(MainFrame, GameType.SinglePlayer, "Medium", gameAPI));

            MainFrame.Content = new PageStart(navigation);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
