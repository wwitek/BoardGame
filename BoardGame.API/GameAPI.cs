using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;
using System.Collections.Generic;

namespace BoardGame.API
{
    public class GameAPI : IGameAPI
    {
        private IGame CurrentGame { get; set; }
        private readonly IGameFactory GameFactory;
        private readonly IPlayerFactory PlayerFactory;

        public GameAPI(IGameFactory gameFactory = null, IPlayerFactory playerFactory = null)
        {
            GameFactory = gameFactory;
            PlayerFactory = playerFactory;
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

        public IMove NextMove(int clickedRow, int clickedColumn)
        {
            if (CurrentGame.IsMoveValid(0, clickedColumn))
            {
                return CurrentGame.MakeMove(0, clickedColumn);
            }

            return null;
        }
    }
}
