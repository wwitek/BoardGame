using System;
using System.Linq;
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
                    int myId = 0;
                    OnlineGameResponse waitingResponse = new OnlineGameResponse(GameState.Waiting, 0);

                    while (waitingResponse.State.Equals(GameState.Waiting))
                    {
                        waitingResponse = await Proxy.OnlineGameRequest(myId);
                        myId = waitingResponse.PlayerId;

                        if (waitingResponse.State.Equals(GameState.Ready))
                        {
                            StartGameResponse startGameResponse = await Proxy.ConfirmToPlay(waitingResponse.PlayerId);
                            if (startGameResponse.State.Equals(GameState.Waiting))
                            {
                                waitingResponse = new OnlineGameResponse(GameState.Waiting, 0);
                            }

                            if (startGameResponse.YourTurn)
                            {
                                players.Add(PlayerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                                players.Add(PlayerFactory.Create(PlayerType.OnlinePlayer));
                            }
                            else
                            {
                                players.Add(PlayerFactory.Create(PlayerType.OnlinePlayer));
                                players.Add(PlayerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                                GetMove(waitingResponse.PlayerId);
                            }
                        }
                    }
                    break;
            }

            CurrentGame = GameFactory.Create(players);
        }

        private async void GetMove(int playerId)
        {
            MoveResponse moveResponse = await Proxy.GetMove(playerId);
            SendMove(CurrentGame.MakeMove(0, moveResponse.ClickedColumn));
        }

        public async void NextMove(int clickedRow, int clickedColumn)
        {
            if (CurrentGame != null && CurrentGame.IsMoveValid(0, clickedColumn))
            {
                SendMove(CurrentGame.MakeMove(0, clickedColumn));

                if (CurrentGame.NextPlayer != null && CurrentGame.NextPlayer.Type.Equals(PlayerType.Bot))
                {
                    // ToDo: Make Bot's move

                    SendMove(CurrentGame.MakeMove(0, 1));
                }
                else if (CurrentGame.NextPlayer != null && CurrentGame.NextPlayer.Type.Equals(PlayerType.OnlinePlayer))
                {
                    int myId = CurrentGame.Players.First(p => p.Type.Equals(PlayerType.Human)).OnlineId;
                    MoveResponse moveResponse = await Proxy.MakeMove(myId, 0, clickedColumn);

                    GetMove(myId);
                }
            }
        }


    }
}
