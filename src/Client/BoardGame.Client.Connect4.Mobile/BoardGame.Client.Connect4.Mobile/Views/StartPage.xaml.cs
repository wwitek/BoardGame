using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BoardGame.Client.Connect4.Mobile.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            Debug.WriteLine("StartPage constructor");
        }

        public StartPage(IPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Debug.WriteLine("StartPage constructor 2");
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
            //return base.OnBackButtonPressed();
        }
    }
}
