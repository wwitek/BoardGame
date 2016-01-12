using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace BoardGame.Client.Connect4.WPF.CustomControls
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        public GameBoard()
        {
            InitializeComponent();
        }

        public async void MakeMove(int player, int row, int column)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = (Brush) FindResource("GradientRed");
            ellipse.Margin = new Thickness(109, 9, 209, 259);
            BoardGrid.Children.Insert(0, ellipse);

            for (int i = 0; i < 1000; i++)
            {
                var i1 = i;
                await Task.Factory.StartNew(() => Thread.Sleep(5))
                    .ContinueWith(task => ellipse.Margin = new Thickness(109, 9 + i1, 209, 259 - i1), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
