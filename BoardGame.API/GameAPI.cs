using System;
using System.Linq;
using System.Collections.Generic;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;
using BoardGame.Server.Contracts;
using BoardGame.Server.Contracts.Responses;
using BoardGame.Domain.Logger;
using BoardGame.Proxies;
using System.ServiceModel;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Entities.BotLevels;

namespace BoardGame.API
{
    public class GameAPI : IGameAPI
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;
        private readonly IEnumerable<IBotLevel> BotLevels; 
        private readonly IGameService Proxy;
        private readonly ILogger Logger;

        public event EventHandler<MoveEventArgs> OnMoveReceived = null;

        public GameAPI(IGameFactory gameFactory = null, 
                       IPlayerFactory playerFactory = null,
                       IEnumerable<IBotLevel> botLevels = null,
                       IGameService proxy = null,
                       ILogger logger = null)
        {
            GameFactory = gameFactory;
            PlayerFactory = playerFactory;
            BotLevels = botLevels;
            Proxy = proxy;
            Logger = logger;
        }

        private void SendMove(IMove move)
        {
            OnMoveReceived?.Invoke(this, new MoveEventArgs { Move = move });
        }

        private IBotLevel GetBotLevel(string name)
        {
            foreach (var botLevel in BotLevels)
            {
                if (botLevel.DisplayName.Equals(name))
                    return botLevel;
            }
            throw new Exception("There is no " + name + " level registered");
        }

        public async void StartGame(GameType type, string level = "")
        {
            if (GameFactory == null || PlayerFactory == null) return;
            var players = new List<IPlayer>();
            switch (type)
            {
                case GameType.SinglePlayer:
                    players.Add(PlayerFactory.Create(PlayerType.Human, 1));
                    players.Add(PlayerFactory.Create(PlayerType.Bot, 2));
                    break;
                case GameType.TwoPlayers:
                    players.Add(PlayerFactory.Create(PlayerType.Human));
                    players.Add(PlayerFactory.Create(PlayerType.Human));
                    break;
                case GameType.Online:
                    int myId = 0;
                    OnlineGameResponse waitingResponse = new OnlineGameResponse();
                    while (!waitingResponse.IsReady)
                    {
                        waitingResponse = await Proxy.OnlineGameRequest(myId);
                        myId = waitingResponse.PlayerId;
                        if (waitingResponse.IsReady)
                        {
                            StartGameResponse startGameResponse =
                                await Proxy.ConfirmToPlay(waitingResponse.PlayerId);
                            if (startGameResponse.IsConfirmed)
                            {
                                if (startGameResponse.YourTurn)
                                {
                                    players.Add(PlayerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
                                    players.Add(PlayerFactory.Create(PlayerType.OnlinePlayer));
                                }
                                else
                                {
                                    players.Add(PlayerFactory.Create(PlayerType.OnlinePlayer));
                                    players.Add(PlayerFactory.Create(PlayerType.Human, waitingResponse.PlayerId));
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
            CurrentGame = GameFactory.Create(players);

            if (players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                IBotLevel botLevel = GetBotLevel(level);
                if (botLevel == null)
                    throw new GameException("Cannot run single-mode game without any bot levels definded");

                CurrentGame.BotLevel = botLevel;
            }
        }

        private async void GetFirstMove(int playerId)
        {
            MoveResponse moveResponse = await Proxy.GetFirstMove(playerId);
            if (moveResponse?.MoveMade != null)
            {
                CurrentGame.MakeMove(moveResponse.MoveMade);
                SendMove(moveResponse.MoveMade);
            }
        }

        public async void NextMove(int clickedRow, int clickedColumn)
        {
            if (CurrentGame != null && CurrentGame.IsMoveValid(0, clickedColumn))
            {
                SendMove(CurrentGame.MakeMove(0, clickedColumn));

                if (CurrentGame.NextPlayer != null && CurrentGame.NextPlayer.Type.Equals(PlayerType.Bot))
                {
                    IMove move = CurrentGame.BotLevel.GenerateMove(CurrentGame);
                    SendMove(move);
                }
                else if (CurrentGame.NextPlayer != null && CurrentGame.NextPlayer.Type.Equals(PlayerType.OnlinePlayer))
                {
                    int myId = CurrentGame.Players.First(p => p.Type.Equals(PlayerType.Human)).OnlineId;
                    MoveResponse moveResponse = await Proxy.MakeMove(myId, 0, clickedColumn);
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
            ((ICommunicationObject) Proxy).Abort();
        }
    }
}
