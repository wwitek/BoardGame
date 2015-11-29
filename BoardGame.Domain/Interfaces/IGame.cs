using System.Collections.Generic;
using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Interfaces
{
    public interface IGame
    {
        IBoard Board { get; }
        List<IPlayer> Players { get; }
        IPlayer NextPlayer { get; }

        bool IsMoveValid(int row, int column);
        IMove MakeMove(int row, int column);
    }
}