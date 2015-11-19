using BoardGame.Domain.Connect4.Entities;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Helpers;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Connect4.Factories
{
    public class GameFactory : IGameFactory
    {
        public IGame CreateGame(GameType type, IBoard board)
        {
            switch (type)
            {
                case GameType.SinglePlayer:
                    return new Connect4Game(type, board,
                        new CircularQueue<IPlayer> { CreateHuman(1), CreateBot(2) });
                case GameType.TwoPlayers:
                    return new Connect4Game(type, board,
                        new CircularQueue<IPlayer> { CreateHuman(1), CreateHuman(2) });
                case GameType.Online:
                    return new Connect4Game(type, board,
                        new CircularQueue<IPlayer> { CreateHuman(1), CreateOnlinePlayer(2) });

            }
            return null;
        }

        private IPlayer CreateBot(int id)
        {
            return new PlayerFactory().CreatePlayer(PlayerType.Bot, id);
        }

        private IPlayer CreateHuman(int id)
        {
            return new PlayerFactory().CreatePlayer(PlayerType.Human, id);
        }

        private IPlayer CreateOnlinePlayer(int id)
        {
            return new PlayerFactory().CreatePlayer(PlayerType.OnlinePlayer, id);
        }
    }
}
