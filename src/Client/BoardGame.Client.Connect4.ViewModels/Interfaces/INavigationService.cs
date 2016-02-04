namespace BoardGame.Client.Connect4.ViewModels.Interfaces
{
    public interface INavigationService
    {
        void GoForward();
        void GoBack();
        bool Navigate(string page);
    }
}
