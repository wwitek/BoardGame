using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Interfaces
{
    public interface IBoard
    {
        int Width { get; set; }
        int Height { get; set; }
        IField[,] Fields { get; set; }
        int WinnerId { get; set; }
        bool IsBoardFull { get; }

        void Reset();
        bool IsMoveValid(int row, int column, int playerId);

        IMove InsertChip(int row, int column, int playerId);
        void RemoveChip(int row, int column);
    }
}
