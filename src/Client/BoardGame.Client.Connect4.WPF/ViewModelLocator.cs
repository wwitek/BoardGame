using System.Diagnostics;
using BoardGame.API;
using BoardGame.Client.Connect4.ViewModels;
using BoardGame.Client.Connect4.ViewModels.Pages;
using BoardGame.Client.Connect4.ViewModels.UserControls;
using BoardGame.Domain.Enums;

namespace BoardGame.Client.Connect4.WPF
{
    public class ViewModelLocator
    {
        private NavigationService navigationService;
        private GameAPI gameApi;
        public void SetNavigationService(NavigationService navigation) => navigationService = navigation;
        public void SetGameAPI(GameAPI api) => gameApi = api;

        public StartPageViewModel StartPageViewModel => new StartPageViewModel(navigationService);

        public SinglePlayerPageViewModel SinglePlayerPageViewModel => new SinglePlayerPageViewModel(navigationService);

        public MainWindowViewModel MainWindowViewModel => new MainWindowViewModel();

        public BoardUserControlViewModel BoardUserControlViewModel => new BoardUserControlViewModel();

        public GamePageViewModel EasySinglePlayerGamePageViewModel => 
            new GamePageViewModel(navigationService, GameType.SinglePlayer, "Easy", gameApi);

        public GamePageViewModel MediumSinglePlayerGamePageViewModel => 
            new GamePageViewModel(navigationService, GameType.SinglePlayer, "Medium", gameApi);

        public GamePageViewModel TwoPlayersGamePageViewModel => 
            new GamePageViewModel(navigationService, GameType.TwoPlayers, "", gameApi);

        public GamePageViewModel OnlineGamePageViewModel => 
            new GamePageViewModel(navigationService, GameType.Online, "", gameApi);
    }
}
