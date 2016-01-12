using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
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
        public int ColumnClicked { get; private set; }
        public int BorderStrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("BorderStrokeThickness", typeof(int), typeof(GameBoard));

        private double HorizontalStep => (ActualWidth - 2 * BorderStrokeThickness) / 7;
        private double VerticalStep => (ActualHeight - 2 * BorderStrokeThickness) / 6;

        private bool resetInProgress;

        public GameBoard()
        {
            InitializeComponent();
        }

        public bool AnimateMove(int player, int row, int column)
        {
            if (resetInProgress) return false;

            double chipHeight = VerticalStep * 0.84;
            double chipWidth = HorizontalStep * 0.84;

            double initChipPositionLeftRight = BorderStrokeThickness + HorizontalStep * 3 + (HorizontalStep - chipWidth) / 2;
            double initChipPositionTop = -chipWidth - 0.36 * VerticalStep;
            double initChipPositionBottom = ActualHeight - initChipPositionTop - chipHeight;

            double firstStepDown = 0.36 * VerticalStep + BorderStrokeThickness + (VerticalStep - chipHeight) / 2;
            double firstRowOffset = -initChipPositionTop + BorderStrokeThickness + (VerticalStep - chipHeight) / 2;

            var chipEllipse = new Ellipse();
            chipEllipse.Fill = (player == 1) ? (Brush) FindResource("GradientRed") : (Brush)FindResource("GradientYellow");
            chipEllipse.Margin = new Thickness(initChipPositionLeftRight, initChipPositionTop, initChipPositionLeftRight, initChipPositionBottom);
            chipEllipse.RenderTransformOrigin = new Point(0.5,0.5);
            chipEllipse.RenderTransform = new TranslateTransform();
            BoardGrid.Children.Insert(0, chipEllipse);

            EasingDoubleKeyFrame initFrameX = new EasingDoubleKeyFrame();
            initFrameX.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 500));
            initFrameX.Value = ((column - 3) * HorizontalStep);
            DoubleAnimationUsingKeyFrames animationX = new DoubleAnimationUsingKeyFrames();
            animationX.KeyFrames.Add(initFrameX);

            Storyboard.SetTarget(animationX, chipEllipse);
            Storyboard.SetTargetProperty(animationX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            EasingDoubleKeyFrame initFrameY = new EasingDoubleKeyFrame();
            initFrameY.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 500));
            initFrameY.Value = firstStepDown;
            EasingDoubleKeyFrame dropDownFrameY = new EasingDoubleKeyFrame();
            dropDownFrameY.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 1, 500));
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
            sb.Begin();

            return true;
        }

        public void AnimateReset()
        {
            resetInProgress = true;

            List<Ellipse> chips = BoardGrid.Children.OfType<Ellipse>().ToList();
            int chipsCompleted = 0;
            foreach (var ellipse in chips)
            {
                EasingDoubleKeyFrame dropDownFrameY = new EasingDoubleKeyFrame();
                dropDownFrameY.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 1));
                dropDownFrameY.Value = ActualHeight + 2 * HorizontalStep;
                dropDownFrameY.EasingFunction = new BounceEase() { Bounces = 3, Bounciness = 4, EasingMode = EasingMode.EaseIn };

                DoubleAnimationUsingKeyFrames animationY = new DoubleAnimationUsingKeyFrames();
                animationY.KeyFrames.Add(dropDownFrameY);

                Storyboard.SetTarget(animationY, ellipse);
                Storyboard.SetTargetProperty(animationY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                Storyboard sb = new Storyboard();
                sb.Children.Add(animationY);
                sb.Completed += (o, e) => { if (chipsCompleted++.Equals(chips.Count - 1)) Reset(); };
                sb.Begin();
            }
        }

        public void Reset()
        {
            if (BoardGrid.Children.Count > 2)
                BoardGrid.Children.RemoveRange(0, BoardGrid.Children.Count - 2);

            resetInProgress = false;
        }

        private void BoardGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(this).X - BorderStrokeThickness;
            double actualCellWidth = (((Grid) sender).ActualWidth - 2 * BorderStrokeThickness) / 7;
            ColumnClicked = (int)(x / actualCellWidth);
            if (ColumnClicked > 6) ColumnClicked = 6;
            if (ColumnClicked < 0) ColumnClicked = 0;
        }
    }
}
