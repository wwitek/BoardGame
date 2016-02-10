using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Client.Connect4.ViewModels.Pages;

namespace BoardGame.Client.Connect4.WPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            Debug.WriteLine("Parameterless " + GetType().Name + " created.");
            InitializeComponent();
        }

        public GamePage(IPageViewModel viewModel)
        {
            Debug.WriteLine(GetType().Name + " created.");
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
