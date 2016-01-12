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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity.Media;

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

        public void MakeMove(int player, int row, int column)
        {
            ControlStoryboardAction action = new ControlStoryboardAction();
            action.Storyboard = (Storyboard)FindResource("MoveChipFirstColumn");
            action.Storyboard.Begin();
        }
    }
}
