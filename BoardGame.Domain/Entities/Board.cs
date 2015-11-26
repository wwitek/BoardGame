using System;
using System.Linq;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;

namespace BoardGame.Domain.Entities
{
    public class Board : IBoard
    {
        private IField[,] Fields;

        public int Width { get; set; }
        public int Height { get; set; }
        public IFieldFactory FieldFactory { get; set; }
        public int WinnerId { get; set; }

        public Board(int width, int height, IFieldFactory fieldFactory)
        {
            Width = width;
            Height = height;
            FieldFactory = fieldFactory;

            Reset();
        }

        public void Reset()
        {
            WinnerId = 0;

            Fields = new IField[Height, Width];
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    Fields[i, j] = FieldFactory.Create(i, j);
                }
            }
        }

        public bool IsMoveValid(int row, int column, int playerId)
        {
            return Fields.Cast<Field>().Take(7).ToList()[column].PlayerId == 0 && WinnerId == 0;
        }

        public IMove InsertChip(int row, int column, int playerId)
        {
            if (column < 0 || column >= Width) throw new Exception("Invalid column!");

            for (int i = Height - 1; i >= 0; i--)
            {
                if (Fields[i, column].PlayerId > 0) continue;
                Fields[i, column].PlayerId = playerId;

                IMove result = GetInsertResult(i, column, playerId, 4);
                
                WinnerId = result.IsConnected ? playerId : 0;

                return result;
            }

            throw new Exception("Cannot add chip here. There is no more room");
        }

        public void RemoveChip(int row, int column)
        {
            if (column < 0 || column >= Width) throw new Exception("Invalid column!");

            for (int i = 0; i < Height; i++)
            {
                if (Fields[i, column].PlayerId == 0) continue;
                Fields[i, column].PlayerId = 0;
                WinnerId = 0;
                return;
            }
        }

        public IMove GetInsertResult(int row, int column, int playerId, int needToWin = 0)
        {
            IMove result = new Move(row, column, playerId, needToWin);

            // Vertically
            // --------------------------------------------
            for (int i = row - 1; i >= 0; i--)
            {
                if (Fields[i, column].PlayerId != playerId) break;
                result.ConnectionVertical.Add(Fields[i, column]);
            }
            result.ConnectionVertical.Add(Fields[row, column]);
            for (int i = row + 1; i < Height; i++)
            {
                if (Fields[i, column].PlayerId != playerId) break;
                result.ConnectionVertical.Add(Fields[i, column]);
            }

            // Horizontally
            // --------------------------------------------
            for (int i = column - 1; i >= 0; i--)
            {
                if (Fields[row, i].PlayerId != playerId) break;
                result.ConnectionHorizontal.Add(Fields[row, i]);
            }
            result.ConnectionHorizontal.Add(Fields[row, column]);
            for (int i = column + 1; i < Width; i++)
            {
                if (Fields[row, i].PlayerId != playerId) break;
                result.ConnectionHorizontal.Add(Fields[row, i]);
            }

            // Diagonally - Northwest to Southeast
            // --------------------------------------------

            // Northwest
            for (int i = 1; i <= Math.Min(row, column); i++)
            {
                if (Fields[row - i, column - i].PlayerId != playerId) break;
                result.ConnectionDescendingDiagonal.Add(Fields[row - i, column - i]);
            }
            result.ConnectionDescendingDiagonal.Add(Fields[row, column]);
            // Southeast
            for (int i = 1; i < Math.Min(Height - row, Width - column); i++)
            {
                if (Fields[row + i, column + i].PlayerId != playerId) break;
                result.ConnectionDescendingDiagonal.Add(Fields[row + i, column + i]);
            }

            // Diagonally - Southwest to Northeast  
            // --------------------------------------------

            // Southwest
            for (int i = 1; i <= Math.Min(Height - row - 1, column); i++)
            {
                if (Fields[row + i, column - i].PlayerId != playerId) break;
                result.ConnectionAscendingDiagonal.Add(Fields[row + i, column - i]);
            }
            result.ConnectionAscendingDiagonal.Add(Fields[row, column]);
            // Northeast
            for (int i = 1; i <= Math.Min(row, Width - column - 1); i++)
            {
                if (Fields[row - i, column + i].PlayerId != playerId) break;
                result.ConnectionAscendingDiagonal.Add(Fields[row - i, column + i]);
            }

            result.IsTie = (!Fields.Cast<IField>().Take(7).Any(f => f.PlayerId == 0)) && !result.IsConnected;
            return result;
        }

    }
}
