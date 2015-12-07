using System;
using System.Collections.Generic;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;
using System.ServiceModel;
using BoardGame.Proxies;
using BoardGame.Server.Contracts;
using BoardGame.Server.Contracts.Responses;

namespace BoardGame.API
{
    public class GameAPI : IGameAPI
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;
        private readonly IGameService Proxy;

        public event EventHandler<MoveEventArgs> OnMoveReceived = null;

        public GameAPI(IGameFactory gameFactory = null, 
                       IPlayerFactory playerFactory = null,
                       IGameService proxy = null)
        {
            GameFactory = gameFactory;
            PlayerFactory = playerFactory;
            Proxy = proxy;
        }

        private void SendMove(IMove move)
        {
            if (OnMoveReceived != null)
            {
                OnMoveReceived(this, new MoveEventArgs { Move = move });
            }
        }

        public async void StartGame(GameType type)
        {
            if (GameFactory == null || PlayerFactory == null) return;
            var players = new List<IPlayer>();

            switch (type)
            {
                case GameType.SinglePlayer:
                    players.Add(PlayerFactory.Create(PlayerType.Human));
                    players.Add(PlayerFactory.Create(PlayerType.Bot));
                    break;
                case GameType.TwoPlayers:
                    players.Add(PlayerFactory.Create(PlayerType.Human));
                    players.Add(PlayerFactory.Create(PlayerType.Human));
                    break;
                case GameType.Online:
                    OnlineGameResponse waitingResponse = new OnlineGameResponse(GameState.WaitingForPlayer, 0);
                    int myId = 0;
                    while (waitingResponse.State.Equals(GameState.WaitingForPlayer))
                    {
                        waitingResponse = await Proxy.OnlineGameRequest(myId);
                        myId = waitingResponse.PlayerId;
                    }
                    if (waitingResponse.State.Equals(GameState.ReadyForOnlineGame))
                    {
                        StartGameResponse startGameResponse = await Proxy.ConfirmToPlay(waitingResponse.PlayerId);
                        if (startGameResponse.YourTurn)
                        {
                            players.Add(PlayerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                            players.Add(PlayerFactory.Create(PlayerType.OnlinePlayer));
                        }
                        else
                        {
                            players.Add(PlayerFactory.Create(PlayerType.OnlinePlayer));
                            players.Add(PlayerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                        }
                    }
                    break;
            }

            CurrentGame = GameFactory.Create(players);
        }

        public void NextMove(int clickedRow, int clickedColumn)
        {
            if (CurrentGame.IsMoveValid(0, clickedColumn))
            {
                SendMove(CurrentGame.MakeMove(0, clickedColumn));

                if (CurrentGame.NextPlayer != null && CurrentGame.NextPlayer.Type.Equals(PlayerType.Bot))
                {
                    // ToDo: Make Bot's move
                    
                    SendMove(CurrentGame.MakeMove(0, 1));
                }
            }
        }
    }
}
