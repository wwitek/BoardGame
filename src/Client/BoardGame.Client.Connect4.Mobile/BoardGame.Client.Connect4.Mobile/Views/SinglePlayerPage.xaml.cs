using BoardGame.Client.Connect4.ViewModels.Interfaces;
using BoardGame.Client.Connect4.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BoardGame.Client.Connect4.Mobile.Views
{
    public partial class SinglePlayerPage : ContentPage
    {
        public SinglePlayerPage(IPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            BasePageViewModel viewModel = (BasePageViewModel)BindingContext;

            if (viewModel.GoBack.CanExecute(null))
                viewModel.GoBack.Execute(null);

            return base.OnBackButtonPressed();
        }
    }
}
