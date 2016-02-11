using BoardGame.Client.Connect4.ViewModels.Pages;

namespace BoardGame.Client.Connect4.ViewModels.Interfaces
{
    public interface INavigationService
    {
        void InjectPage(string pageKey, IPageViewModel viewModel);
        void GoForward();
        void GoBack();
        bool Navigate(string pageKey);
    }
}
