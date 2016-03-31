using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;
using BoardGame.Contracts;
using BoardGame.Contracts.Responses;
using BoardGame.Domain.Logger;
using System.ServiceModel;
using System.Threading.Tasks;
using BoardGame.API.Exceptions;
using BoardGame.API.Interfaces;

namespace BoardGame.API
{
    public class GameAPI
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory gameFactory;
        private readonly IPlayerFactory playerFactory;
        private IGameProxy proxy;
        private readonly ILogger logger;
        private int myOnlineId;
        private bool isRequestingOnlineGame;
        private bool isOnlineGameRequestCancelled;

        public bool IsOnlineAvailable => proxy != null;
        public event EventHandler<MoveEventArgs> MoveReceived;

        public GameAPI(IGameFactory gameFactory,
                       IPlayerFactory playerFactory)
        {
            Requires.IsNotNull(gameFactory, "gameFactory");
            Requires.IsNotNull(playerFactory, "playerFactory");

            this.gameFactory = gameFactory;
            this.playerFactory = playerFactory;
        }

        public GameAPI(IGameFactory gameFactory,
                       IPlayerFactory playerFactory,
                       ILogger logger)
        {
            Requires.IsNotNull(gameFactory, "gameFactory");
            Requires.IsNotNull(playerFactory, "playerFactory");

            this.gameFactory = gameFactory;
            this.playerFactory = playerFactory;
            this.logger = logger;
        }

        public GameAPI(IGameFactory gameFactory, 
                       IPlayerFactory playerFactory,
                       ILogger logger,
                       IGameProxy proxy)
        {
            Requires.IsNotNull(gameFactory, "gameFactory");
            Requires.IsNotNull(playerFactory, "playerFactory");

            this.gameFactory = gameFactory;
            this.playerFactory = playerFactory;
            this.proxy = proxy;
            this.logger = logger;
        }

        public async Task<bool> VerifyConnection()
        {
            try
            {
                var sampleTestValue = 123;
                var verifyConnection = proxy?.VerifyConnection(sampleTestValue);
                if (verifyConnection != null && await verifyConnection == sampleTestValue)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger?.Error(StringResources.VerifyConnectionFailed(ex.Message));
                logger?.Error(ex);
            }
            proxy = null;
            return false;
        }

        public async void StartGame(GameType type, string level = "")
        {
            logger.Info("Game started");
            try
            { 
                if (MoveReceived == null)
                {
                    logger?.Error("InvalidOperationException: " + StringResources.TheGameCanNotBeStartedBecauseOfMoveReceivedIsNull());
                    throw new InvalidOperationException(
                        StringResources.TheGameCanNotBeStartedBecauseOfMoveReceivedIsNull());
                }

                var players = new List<IPlayer>();
                switch (type)
                {
                    case GameType.SinglePlayer:
                        players.Add(playerFactory.Create(PlayerType.Human, 1));
                        players.Add(playerFactory.Create(PlayerType.Bot, 2));
                        break;
                    case GameType.TwoPlayers:
                        players.Add(playerFactory.Create(PlayerType.Human, 0));
                        players.Add(playerFactory.Create(PlayerType.Human, 0));
                        break;
                    case GameType.Online:
                        if (proxy == null)
                        {
                            logger?.Error("InvalidOperationException: " +
                                          StringResources.TheGameCanNotBeStartedBecauseOfProxyIsNull());
                            throw new InvalidOperationException(
                                StringResources.TheGameCanNotBeStartedBecauseOfProxyIsNull());
                        }
                        isOnlineGameRequestCancelled = false;

                        if (!isRequestingOnlineGame)
                        {
                            players = await Task.Factory.StartNew(RequestOnlinePlayer).Result;
                        }

                        if (players.Count != 2) return;

                        if (players[0].Type.Equals(PlayerType.OnlinePlayer))
                        {
                            GetFirstMove(myOnlineId);
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
            catch (TimeoutException ex)
            {
                string exceptionMessage = StringResources.TimeoutExceptionOccured("StartGame", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
            catch (FaultException ex)
            {
                string exceptionMessage = StringResources.ExceptionOccuredOnServerSide("StartGame", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
            catch (CommunicationException ex)
            {
                string exceptionMessage = StringResources.CommunicationProblemOccured("StartGame", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
        }

        private async Task<List<IPlayer>> RequestOnlinePlayer()
        {
            var players = new List<IPlayer>();
            OnlineGameResponse waitingResponse = new OnlineGameResponse();
            isRequestingOnlineGame = true;
            while (!waitingResponse.IsReady && !isOnlineGameRequestCancelled)
            {
                waitingResponse = await proxy.OnlineGameRequest(myOnlineId);
                myOnlineId = waitingResponse.PlayerId;

                //Quit requesting online game when isOnlineGameRequestCancelled is true 
                //(user is not in online game page)
                if (isOnlineGameRequestCancelled) break;
                if (waitingResponse.IsReady)
                {
                    StartGameResponse startGameResponse =
                        await proxy.ConfirmToPlay(waitingResponse.PlayerId);
                    if (startGameResponse.IsConfirmed)
                    {
                        if (startGameResponse.YourTurn)
                        {
                            players.Add(playerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                            players.Add(playerFactory.Create(PlayerType.OnlinePlayer, 0));
                        }
                        else
                        {
                            players.Add(playerFactory.Create(PlayerType.OnlinePlayer, 0));
                            players.Add(playerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                        }
                    }
                    else
                    {
                        waitingResponse.IsReady = false;
                    }
                }
            }
            isRequestingOnlineGame = false;
            return players;
        }

        public async Task<bool> NextMove(int clickedRow, int clickedColumn)
        {
            try
            {
                if (CurrentGame == null)
                {
                    logger?.Warning("InvalidOperationException: " + StringResources.CanNotPerformTheMoveBecauseGameIsNull());
                    //throw new InvalidOperationException(
                    //    StringResources.CanNotPerformTheMoveBecauseGameIsNull());
                    return false;
                }

                if (CurrentGame.State == GameState.Finished || CurrentGame.State == GameState.Aborted)
                {
                    return true;
                }

                if (CurrentGame.IsMoveValid(clickedColumn))
                {
                    SendMove(CurrentGame.MakeMove(clickedColumn));

                    if (CurrentGame.NextPlayer == null)
                    {
                        logger?.Error("InvalidOperationException: " +
                                      StringResources.CanNotPerformNextMoveBecauseNextPlayerIsNull());
                        throw new InvalidOperationException(
                            StringResources.CanNotPerformNextMoveBecauseNextPlayerIsNull());
                    }

                    if (CurrentGame.State.Equals(GameState.Running) &&
                        CurrentGame.NextPlayer.Type.Equals(PlayerType.Bot))
                    {
                        if (CurrentGame.Bot == null)
                        {
                            logger?.Error("InvalidOperationException: " +
                                          StringResources.CanNotPerformBotsMoveBecauseBotWasNotDefined());
                            throw new InvalidOperationException(
                                StringResources.CanNotPerformBotsMoveBecauseBotWasNotDefined());
                        }

                        Stopwatch moveStopwatch = new Stopwatch();
                        moveStopwatch.Reset();
                        moveStopwatch.Start();

                        int botMove = CurrentGame.Bot.GenerateMove(CurrentGame);

                        int elapsed = (int)moveStopwatch.ElapsedMilliseconds;
                        if (elapsed < 750)
                        {
                            await Task.Delay(1200 - elapsed);
                        }
                        moveStopwatch.Stop();
                        SendMove(CurrentGame.MakeMove(botMove));
                    }
                    else if (CurrentGame.NextPlayer.Type.Equals(PlayerType.OnlinePlayer))
                    {
                        if (proxy == null)
                        {
                            logger?.Error("InvalidOperationException: " +
                                          StringResources.CanNotPerformOnlineMoveBecauseOfProxyIsNull());
                            throw new InvalidOperationException(
                                StringResources.CanNotPerformOnlineMoveBecauseOfProxyIsNull());
                        }

                        int myId = CurrentGame.Players.First(p => p.Type.Equals(PlayerType.Human)).OnlineId;
                        MoveResponse moveResponse = await proxy.MakeMove(myId, clickedColumn);
                        if (moveResponse?.MoveMade != null)
                        {
                            CurrentGame.MakeMove(moveResponse.MoveMade);
                            SendMove(moveResponse.MoveMade);
                        }
                    }
                }
                return false;
            }
            catch (TimeoutException ex)
            {
                string exceptionMessage = StringResources.TimeoutExceptionOccured("NextMove", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
            catch (FaultException ex)
            {
                string exceptionMessage = StringResources.ExceptionOccuredOnServerSide("NextMove", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
            catch (CommunicationException ex)
            {
                string exceptionMessage = StringResources.CommunicationProblemOccured("NextMove", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
        }

        public void Close()
        {
            CurrentGame = null;
            isOnlineGameRequestCancelled = true;
            logger?.Info("Closing GameAPI. Server connection will be aborted");
        }

        private void SendMove(IMove move)
        {
            Requires.IsNotNull(MoveReceived, "MoveReceived");

            MoveReceived?.Invoke(this, new MoveEventArgs { Move = move });
        }

        private async void GetFirstMove(int playerId)
        {
            try
            {
                MoveResponse moveResponse = await proxy.GetFirstMove(playerId);
                if (moveResponse?.MoveMade != null)
                {
                    CurrentGame.MakeMove(moveResponse.MoveMade);
                    SendMove(moveResponse.MoveMade);
                }
            }
            catch (TimeoutException ex)
            {
                string exceptionMessage = StringResources.TimeoutExceptionOccured("GetFirstMove", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
            catch (FaultException ex)
            {
                string exceptionMessage = StringResources.ExceptionOccuredOnServerSide("GetFirstMove", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
            catch (CommunicationException ex)
            {
                string exceptionMessage = StringResources.CommunicationProblemOccured("GetFirstMove", ex.Message);
                logger?.Error(exceptionMessage);

                throw new GameServerException(exceptionMessage, ex);
            }
        }
    }
}
