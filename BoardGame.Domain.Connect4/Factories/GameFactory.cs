using BoardGame.Domain.Connect4.Entities;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Helpers;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Connect4.Factories
{
    public class GameFactory : IGameFactory
    {
        public IGame CreateGame(GameType type)
        {
            switch (type)
            {
                case GameType.SinglePlayer:
                    return new Connect4Game(type,
                        new CircularQueue<IPlayer> { CreateHuman(), CreateBot() });
                case GameType.TwoPlayers:
                    return new Connect4Game(type,
                        new CircularQueue<IPlayer> { CreateHuman(), CreateHuman() });
                case GameType.Online:
                    return new Connect4Game(type,
                        new CircularQueue<IPlayer> { CreateHuman(), CreateOnlinePlayer() });

            }
            return null;
        }

        private IPlayer CreateBot()
        {
            return new PlayerFactory().CreatePlayer(PlayerType.Bot);
        }

        private IPlayer CreateHuman()
        {
            return new PlayerFactory().CreatePlayer(PlayerType.Human);
        }

        private IPlayer CreateOnlinePlayer()
        {
            return new PlayerFactory().CreatePlayer(PlayerType.OnlinePlayer);
        }
    }
}
