using System.Collections.Generic;
using BoardGame.Domain.Enums;
using System.ComponentModel;
using System;

namespace BoardGame.Domain.Interfaces
{
    public interface IGame
    {
        GameState State { get; set; }
        IBoard Board { get; }
        List<IPlayer> Players { get; }
        IPlayer NextPlayer { get; }
        IMove LastMove { get; set; }

        bool IsMoveValid(int row, int column);
        IMove MakeMove(int row, int column);
        void MakeMove(IMove move);
        bool WaitForNextPlayer(int timeout);

        event PropertyChangedEventHandler StateChanged;
    }
}