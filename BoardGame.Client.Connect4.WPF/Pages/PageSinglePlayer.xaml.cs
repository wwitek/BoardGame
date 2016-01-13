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

namespace BoardGame.Client.Connect4.WPF.Pages
{
    /// <summary>
    /// Interaction logic for PageStart.xaml
    /// </summary>
    public partial class PageSinglePlayer : Page
    {
        private Frame mainFrame;

        public PageSinglePlayer(Frame mainFrame)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
        }
        
        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new PageGame(mainFrame);
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new PageGame(mainFrame);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new PageStart(mainFrame);
        }
    }
}
