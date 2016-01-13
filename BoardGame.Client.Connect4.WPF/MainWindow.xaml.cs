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

namespace BoardGame.Client.Connect4.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IGameAPI gameAPI;

        public MainWindow(IGameAPI gameAPI = null)
        {
            InitializeComponent();

            this.gameAPI = gameAPI;
            MainFrame.Content = new PageStart(MainFrame, this.gameAPI);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
