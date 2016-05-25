using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using BoardGame.Client.Connect4.ViewModels.Interfaces;

namespace BoardGame.Client.Connect4.Mobile
{
    public class NavigationService : INavigationService
    {
        public Page CurrentPage { get; private set; }
        private readonly Dictionary<string, IPageViewModel> pageViewModels = new Dictionary<string, IPageViewModel>();
        private readonly Dictionary<IPageViewModel, object> pages = new Dictionary<IPageViewModel, object>();

        public NavigationService(Page currentPage)
        {
            CurrentPage = currentPage;
        }

        private bool Navigate(IPageViewModel pageViewModel)
        {
            object page;
            if (pages.TryGetValue(pageViewModel, out page))
            {
                CurrentPage.Navigation.PushAsync((Page)page);
                return true;
            }

            string viewModel = pageViewModel.GetType().Name;
            string pageName = viewModel.Substring(0, viewModel.IndexOf("ViewModel", StringComparison.Ordinal));

            var typeInfo = GetType().GetTypeInfo().Assembly.DefinedTypes
                .SingleOrDefault(a => a.Name.Equals(pageName));
            var type = typeInfo.AsType();
            if (type == null) return false;

            Page src = (Page)Activator.CreateInstance(type, pageViewModel);
            pages.Add(pageViewModel, src);
            CurrentPage.Navigation.PushAsync(src);
            CurrentPage = src;
            return true;
        }

        public bool Navigate(string pageKey)
        {
            IPageViewModel viewModel;
            if (pageViewModels.TryGetValue(pageKey, out viewModel))
            {
                return Navigate(viewModel);
            }
            return false;
        }

        public void GoBack()
        {
            string viewModel = CurrentPage.BindingContext.GetType().Name;
            string pageName = viewModel.Substring(0, viewModel.IndexOf("ViewModel", StringComparison.Ordinal));

            switch(pageName)
            {
                case "SinglePlayerPage":
                    IPageViewModel previousViewModel;
                    if (pageViewModels.TryGetValue("StartPage", out previousViewModel))
                    {
                        object page;
                        if (pages.TryGetValue(previousViewModel, out page))
                        {
                            CurrentPage = (Page)page;
                        }
                    }
                    break;
            }
            CurrentPage.SendBackButtonPressed();
        }

        public void GoForward()
        {
            //frame.GoForward();
        }

        public void InjectPage(string pageKey, IPageViewModel viewModel)
        {
            pageViewModels.Add(pageKey, viewModel);
        }
    }
}
