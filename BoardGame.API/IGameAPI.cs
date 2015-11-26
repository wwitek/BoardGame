using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.API
{
    public interface IGameAPI
    {
        void StartGame(GameType type);
        IMove NextMove(int clickedRow, int clickedColumn);
    }
}
