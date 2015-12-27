using System;
using BoardGame.Domain.Enums;

namespace BoardGame.API
{
    public interface IGameAPI
    {
        event EventHandler<MoveEventArgs> OnMoveReceived;

        void StartGame(GameType type, string level = "");
        void NextMove(int clickedRow, int clickedColumn);
        void Close();
    }
}
