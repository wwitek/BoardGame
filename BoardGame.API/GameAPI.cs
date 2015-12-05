using System;
using System.Collections.Generic;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;
using System.ServiceModel;
using BoardGame.Proxies;

namespace BoardGame.API
{
    public class GameAPI : IGameAPI
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;

        public event EventHandler<MoveEventArgs> OnMoveReceived = null;

        public GameAPI(IGameFactory gameFactory = null, IPlayerFactory playerFactory = null)
        {
            GameFactory = gameFactory;
            PlayerFactory = playerFactory;
        }

        private void SendMove(IMove move)
        {
            if (OnMoveReceived != null)
            {
                OnMoveReceived(this, new MoveEventArgs { Move = move });
            }
        }

        public void StartGame(GameType type)
        {
            if (GameFactory == null || PlayerFactory == null) return;

            var players = new List<IPlayer> {
                PlayerFactory.Create(PlayerType.Human, 1)
            };

            switch (type)
            {
                case GameType.SinglePlayer:
                    players.Add(PlayerFactory.Create(PlayerType.Bot, 2));
                    break;
                case GameType.TwoPlayers:
                    players.Add(PlayerFactory.Create(PlayerType.Human, 2));
                    break;
            }

            CurrentGame = GameFactory.Create(players);
        }

        public void NextMove(int clickedRow, int clickedColumn)
        {
            if (CurrentGame.IsMoveValid(0, clickedColumn))
            {
                SendMove(CurrentGame.MakeMove(0, clickedColumn));

                if (CurrentGame.NextPlayer.Type.Equals(PlayerType.Bot))
                {
                    // ToDo: Make Bot's move

                    SendMove(CurrentGame.MakeMove(0, 1));
                }
            }
        }
    }
}
