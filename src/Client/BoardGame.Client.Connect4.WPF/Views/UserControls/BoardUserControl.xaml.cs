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
using System.Windows.Media.Animation;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Client.Connect4.WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for BoardUserControl.xaml
    /// </summary>
    public partial class BoardUserControl : UserControl
    {
        private bool isAnimating;
        private double HorizontalStep => (ActualWidth - 2 * BorderStrokeThickness) / 7;
        private double VerticalStep => (ActualHeight - 2 * BorderStrokeThickness) / 6;
        private static readonly DependencyProperty StrokeThicknessProperty =
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

        private async void OnBoardReset(object sender, BoardResetEventArgs e)
        {
            Debug.WriteLine("UI: OnBoardReset (IsAnimate=" + e.IsAnimate + ")");

            HideWinner();
            if (e.IsAnimate) await AnimateReset();
            Reset();
        }

        private async void OnMoveReceived(object sender, MoveReceivedEventArgs e)
        {
            Debug.WriteLine("UI: OnMoveReceived");
            IMove move = e?.MoveMade;
            if (move != null)
            {
                await AnimateMove(move.PlayerId, move.Row, move.Column);

                if (move.IsConnected)
                {
                    ShowWinner(move.PlayerId + " won!");
                }
                if (move.IsTie)
                {
                    ShowWinner("Game Over! Nobody won");
                }
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isAnimating) return;

            double x = e.GetPosition(this).X - BorderStrokeThickness;
            double actualCellWidth = (((Grid)sender).ActualWidth - 2 * BorderStrokeThickness) / 7;
            int columnClicked = (int)(x / actualCellWidth);
            if (columnClicked > 6) columnClicked = 6;
            if (columnClicked < 0) columnClicked = 0;

            BoardClickEventArgs args = new BoardClickEventArgs{ ColumnClicked = columnClicked };
            BoardClick?.Invoke(sender, args);
        }

        private async Task<bool> AnimateMove(int player, int row, int column)
        {
            isAnimating = true;

            int firstStepTime = 50 + 50 * Math.Abs(column - 3);
            int secondStepTime = 750;

            double chipHeight = VerticalStep * 0.84;
            double chipWidth = HorizontalStep * 0.84;

            double initChipPositionLeftRight = BorderStrokeThickness + HorizontalStep * 3 + (HorizontalStep - chipWidth) / 2;
            double initChipPositionTop = -chipWidth - 0.36 * VerticalStep;
            double initChipPositionBottom = ActualHeight - initChipPositionTop - chipHeight;

            double firstStepDown = 0.36 * VerticalStep + BorderStrokeThickness + (VerticalStep - chipHeight) / 2;
            double firstRowOffset = -initChipPositionTop + BorderStrokeThickness + (VerticalStep - chipHeight) / 2;

            var chipEllipse = new Ellipse();
            chipEllipse.Fill = (player == 1) ? (Brush)FindResource("GradientRed") : (Brush)FindResource("GradientYellow");
            chipEllipse.Margin = new Thickness(initChipPositionLeftRight, initChipPositionTop, initChipPositionLeftRight, initChipPositionBottom);
            chipEllipse.RenderTransformOrigin = new Point(0.5, 0.5);
            chipEllipse.RenderTransform = new TranslateTransform();
            BoardGrid.Children.Insert(0, chipEllipse);

            EasingDoubleKeyFrame initFrameX = new EasingDoubleKeyFrame();
            initFrameX.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, firstStepTime));
            initFrameX.Value = ((column - 3) * HorizontalStep);
            DoubleAnimationUsingKeyFrames animationX = new DoubleAnimationUsingKeyFrames();
            animationX.KeyFrames.Add(initFrameX);

            Storyboard.SetTarget(animationX, chipEllipse);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            EasingDoubleKeyFrame initFrameY = new EasingDoubleKeyFrame();
            initFrameY.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, firstStepTime));
            initFrameY.Value = firstStepDown;
            EasingDoubleKeyFrame dropDownFrameY = new EasingDoubleKeyFrame();
            dropDownFrameY.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, secondStepTime));
            dropDownFrameY.Value = (row * VerticalStep) + firstRowOffset;
            dropDownFrameY.EasingFunction = new BounceEase() { Bounces = 2, Bounciness = 3, EasingMode = EasingMode.EaseOut };

            DoubleAnimationUsingKeyFrames animationY = new DoubleAnimationUsingKeyFrames();
            animationY.KeyFrames.Add(initFrameY);
            animationY.KeyFrames.Add(dropDownFrameY);

            Storyboard.SetTarget(animationY, chipEllipse);
            Storyboard.SetTargetProperty(animationY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            Storyboard sb = new Storyboard();
            sb.Children.Add(animationX);
            sb.Children.Add(animationY);

            await sb.BeginAsync();

            isAnimating = false;
            return true;
        }

        private async Task<bool> AnimateReset()
        {
            isAnimating = true;
            int dropTime = 1000;
            List<Ellipse> chips = BoardGrid.Children.OfType<Ellipse>().ToList();

            int chipsDeleted = 0;
            foreach (var ellipse in chips)
            {
                EasingDoubleKeyFrame dropDownFrameY = new EasingDoubleKeyFrame();
                dropDownFrameY.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, dropTime));
                dropDownFrameY.Value = ActualHeight + 2 * HorizontalStep;
                dropDownFrameY.EasingFunction = new BounceEase() { Bounces = 3, Bounciness = 4, EasingMode = EasingMode.EaseIn };

                DoubleAnimationUsingKeyFrames animationY = new DoubleAnimationUsingKeyFrames();
                animationY.KeyFrames.Add(dropDownFrameY);

                Storyboard.SetTarget(animationY, ellipse);
                Storyboard.SetTargetProperty(animationY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                Storyboard sb = new Storyboard();
                sb.Children.Add(animationY);
                if (!chipsDeleted++.Equals(chips.Count - 1))
                {
                    sb.Begin();
                    continue;
                }

                await sb.BeginAsync();
            }
            return true;
        }

        private void Reset()
        {
            if (BoardGrid.Children.Count > 3)
                BoardGrid.Children.RemoveRange(0, BoardGrid.Children.Count - 3);

            isAnimating = false;
        }

        private void ShowWinner(string message)
        {
            WinnerMessage.Text = message;
            WinnerGrid.Visibility = Visibility.Visible;
        }

        private void HideWinner()
        {
            WinnerGrid.Visibility = Visibility.Hidden;
        }

    }
}
