using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Helpers;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Connect4.Entities
{
    public class Connect4Game : Game
    {
        public Connect4Game(GameType type, CircularQueue<IPlayer> players)
            : base(type, players)
        {
        }
    }
}
