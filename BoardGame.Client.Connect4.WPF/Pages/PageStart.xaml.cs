﻿using System;
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
    public partial class PageStart : Page
    {
        private Frame mainFrame;

        public PageStart(Frame mainFrame)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
        }

        private void SinglePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new PageSinglePlayer(mainFrame);
        }

        private void TwoPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new PageGame(mainFrame);
        }

        private void OnlineGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new PageGame(mainFrame);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
