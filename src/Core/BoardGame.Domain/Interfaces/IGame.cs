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
        IBot Bot { get; }

        bool IsMoveValid(int column);
        IMove MakeMove(int column);
        void MakeMove(IMove move);

        bool WaitForNextPlayer(int timeout);

        event PropertyChangedEventHandler OnStateChanged;
    }
}