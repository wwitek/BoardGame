using BoardGame.Client.Connect4.ViewModels.Pages;

namespace BoardGame.Client.Connect4.ViewModels.Interfaces
{
    public interface INavigationService
    {
        void GoForward();
        void GoBack();
        bool Navigate(string page);
        bool Navigate(BasePageViewModel page);
    }
}
