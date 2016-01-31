using BoardGame.Domain.Factories;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Interfaces
{
    public interface IBoard
    {
        int Width { get; }
        int Height { get; }
        int WinnerId { get; }
        IMove LastMove { get; }

        void Reset();
        bool IsMoveValid(int row, int column, int playerId);
        IMove InsertChip(int row, int column, int playerId);
        void RemoveChip(int row, int column);
        void ApplyMove(IMove move);
    }
}
