using System;
using BoardGame.Domain.Enums;

namespace BoardGame.API
{
    public interface IGameAPI
    {
        event EventHandler<MoveEventArgs> OnMoveReceived;

        void StartGame(GameType type);
        void NextMove(int clickedRow, int clickedColumn);
    }
}
