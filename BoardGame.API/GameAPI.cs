using System;
using System.Linq;
using System.Collections.Generic;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;
using BoardGame.Server.Contracts;
using BoardGame.Server.Contracts.Responses;
using BoardGame.Domain.Logger;
using System.ServiceModel;

namespace BoardGame.API
{
    public class GameAPI : IGameAPI
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory gameFactory;
        private readonly IPlayerFactory playerFactory;
        private readonly IGameService proxy;
        private readonly ILogger logger;

        public event EventHandler<MoveEventArgs> OnMoveReceived;

        public GameAPI(IGameFactory gameFactory, 
                       IPlayerFactory playerFactory,
                       IGameService proxy = null,
                       ILogger logger = null)
        {
            Requires.IsNotNull(gameFactory, "gameFactory");
            Requires.IsNotNull(playerFactory, "playerFactory");

            this.gameFactory = gameFactory;
            this.playerFactory = playerFactory;
            this.proxy = proxy;
            this.logger = logger;
        }

        private void SendMove(IMove move)
        {
            Requires.IsNotNull(OnMoveReceived, "OnMoveReceived");

            OnMoveReceived?.Invoke(this, new MoveEventArgs { Move = move });
        }

        public async void StartGame(GameType type, string level = "")
        {
            if (OnMoveReceived == null)
            {
                logger?.Error("InvalidOperationException: " + StringResources.TheGameCanNotBeStartedBecauseOfOnMoveReceivedIsNull());
                throw new InvalidOperationException(
                    StringResources.TheGameCanNotBeStartedBecauseOfOnMoveReceivedIsNull());
            }

            var players = new List<IPlayer>();
            switch (type)
            {
                case GameType.SinglePlayer:
                    players.Add(playerFactory.Create(PlayerType.Human, 1));
                    players.Add(playerFactory.Create(PlayerType.Bot, 2));
                    break;
                case GameType.TwoPlayers:
                    players.Add(playerFactory.Create(PlayerType.Human));
                    players.Add(playerFactory.Create(PlayerType.Human));
                    break;
                case GameType.Online:
                    int myId = 0;

                    if (proxy == null)
                    {
                        logger?.Error("InvalidOperationException: " + StringResources.TheGameCanNotBeStartedBecauseOfProxyIsNull());
                        throw new InvalidOperationException(
                            StringResources.TheGameCanNotBeStartedBecauseOfProxyIsNull());
                    }

                    OnlineGameResponse waitingResponse = new OnlineGameResponse();
                    while (!waitingResponse.IsReady)
                    {
                        waitingResponse = await proxy.OnlineGameRequest(myId);
                        myId = waitingResponse.PlayerId;
                        if (waitingResponse.IsReady)
                        {
                            StartGameResponse startGameResponse =
                                await proxy.ConfirmToPlay(waitingResponse.PlayerId);
                            if (startGameResponse.IsConfirmed)
                            {
                                if (startGameResponse.YourTurn)
                                {
                                    players.Add(playerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                                    players.Add(playerFactory.Create(PlayerType.OnlinePlayer));
                                }
                                else
                                {
                                    players.Add(playerFactory.Create(PlayerType.OnlinePlayer));
                                    players.Add(playerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                                    GetFirstMove(waitingResponse.PlayerId);
                                }
                            }
                            else
                            {
                                waitingResponse.IsReady = false;
                            }
                        }
                    }
                    break;
                case GameType.Bluetooth:
                    break;
                case GameType.Wifi:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            CurrentGame = gameFactory.Create(players, level);
        }

        private async void GetFirstMove(int playerId)
        {
            MoveResponse moveResponse = await proxy.GetFirstMove(playerId);
            if (moveResponse?.MoveMade != null)
            {
                CurrentGame.MakeMove(moveResponse.MoveMade);
                SendMove(moveResponse.MoveMade);
            }
        }

        public async void NextMove(int clickedRow, int clickedColumn)
        {
            if (CurrentGame == null)
            {
                logger?.Error("InvalidOperationException: " + StringResources.CanNotPerformTheMoveBecauseGameIsNull());
                throw new InvalidOperationException(
                    StringResources.CanNotPerformTheMoveBecauseGameIsNull());
            }

            if (CurrentGame.IsMoveValid(0, clickedColumn))
            {
                SendMove(CurrentGame.MakeMove(0, clickedColumn));

                if (CurrentGame.NextPlayer == null)
                {
                    logger?.Error("InvalidOperationException: " + StringResources.CanNotPerformNextMoveBecauseNextPlayerIsNull());
                    throw new InvalidOperationException(
                        StringResources.CanNotPerformNextMoveBecauseNextPlayerIsNull());
                }
                if (CurrentGame.NextPlayer.Type.Equals(PlayerType.Bot))
                {
                    if (CurrentGame.Bot == null)
                    {
                        logger?.Error("InvalidOperationException: " + StringResources.CanNotPerformBotsMoveBecauseBotWasNotDefined());
                        throw new InvalidOperationException(
                            StringResources.CanNotPerformBotsMoveBecauseBotWasNotDefined());
                    }
                    IMove move = CurrentGame.Bot.GenerateMove(CurrentGame);
                    SendMove(move);
                }
                else if (CurrentGame.NextPlayer.Type.Equals(PlayerType.OnlinePlayer))
                {
                    if (proxy == null)
                    {
                        logger?.Error("InvalidOperationException: " + StringResources.CanNotPerformOnlineMoveBecauseOfProxyIsNull());
                        throw new InvalidOperationException(
                            StringResources.CanNotPerformOnlineMoveBecauseOfProxyIsNull());
                    }

                    int myId = CurrentGame.Players.First(p => p.Type.Equals(PlayerType.Human)).OnlineId;
                    MoveResponse moveResponse = await proxy.MakeMove(myId, 0, clickedColumn);
                    if (moveResponse?.MoveMade != null)
                    {
                        CurrentGame.MakeMove(moveResponse.MoveMade);
                        SendMove(moveResponse.MoveMade);
                    }
                }
            }
        }

        public void Close()
        {
            ((ICommunicationObject) proxy).Abort();
        }
    }
}
