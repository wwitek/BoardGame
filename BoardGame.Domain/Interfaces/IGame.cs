using System.Collections.Generic;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Helpers;

namespace BoardGame.Domain.Interfaces
{
    public interface IGame
    {
        GameState State { get; }
        GameType Type { get; }
        IBoard Board { get; }
        CircularQueue<IPlayer> Players { get; }
        IPlayer CurrentPlayer { get; }
        IMove MakeMove(int row, int column, int playerId);
    }
}