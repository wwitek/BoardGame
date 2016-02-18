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
using BoardGame.Client.Connect4.ViewModels.EventArguments.Views;
using BoardGame.Client.Connect4.ViewModels.EventArguments.ViewModels;
using System.Diagnostics;
using BoardGame.Client.Connect4.ViewModels.Pages;

namespace BoardGame.Client.Connect4.WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for BoardUserControl.xaml
    /// </summary>
    public partial class BoardUserControl : UserControl
    {
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("BorderStrokeThickness", typeof(int), typeof(BoardUserControl));

        public int BorderStrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public event EventHandler<BoardClickEventArgs> BoardClick;

        public BoardUserControl()
        {
            InitializeComponent();
            DataContextChanged += (s, e) =>
            {
                if (DataContext.GetType().Equals(typeof(GamePageViewModel)))
                {
                    ((GamePageViewModel)DataContext).MoveReceived += OnMoveReceived;
                    ((GamePageViewModel)DataContext).BoardReset += OnBoardReset;
                }
            };
        }

        private void OnBoardReset(object sender, BoardResetEventArgs e)
        {
            Debug.WriteLine("UI: OnBoardReset");
        }

        private void OnMoveReceived(object sender, MoveReceivedEventArgs e)
        {
            Debug.WriteLine("UI: OnMoveReceived");
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(this).X - BorderStrokeThickness;
            double actualCellWidth = (((Grid)sender).ActualWidth - 2 * BorderStrokeThickness) / 7;
            int columnClicked = (int)(x / actualCellWidth);
            if (columnClicked > 6) columnClicked = 6;
            if (columnClicked < 0) columnClicked = 0;

            BoardClickEventArgs args = new BoardClickEventArgs{ ColumnClicked = columnClicked };
            BoardClick?.Invoke(sender, args);
        }
    }
}
