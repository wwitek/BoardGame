using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using BoardGame.Client.Connect4.ViewModels.Interfaces;

namespace BoardGame.Client.Connect4.WPF
{
    public class NavigationService : INavigationService
    {
        private readonly Frame frame;
        private readonly Dictionary<string, IPageViewModel> pageViewModels = new Dictionary<string, IPageViewModel>();
        private readonly Dictionary<IPageViewModel, object> pages = new Dictionary<IPageViewModel, object>();

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        private bool Navigate(IPageViewModel pageViewModel)
        {
            object page;
            if (pages.TryGetValue(pageViewModel, out page))
            {
                return frame.Navigate(page);
            }

            string viewModel = pageViewModel.GetType().Name;
            string pageName = viewModel.Substring(0, viewModel.IndexOf("ViewModel", StringComparison.Ordinal));

            var type = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(a => a.Name.Equals(pageName));
            if (type == null) return false;

            var src = Activator.CreateInstance(type, pageViewModel);
            pages.Add(pageViewModel, src);
            return frame.Navigate(src);
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
            frame.GoBack();
        }

        public void GoForward()
        {
            frame.GoForward();
        }

        public void InjectPage(string pageKey, IPageViewModel viewModel)
        {
            pageViewModels.Add(pageKey, viewModel);
        }
    }
}
