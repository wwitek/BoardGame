﻿using System;
using System.Linq;
using System.Text;
using BoardGame.Domain.Exceptions;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Factories;

namespace BoardGame.Domain.Entities
{
    public class Board : IBoard
    {
        private IField[,] fields;
        private readonly IFieldFactory fieldFactory;

        public int Width { get; }
        public int Height { get; }
        public IMove LastMove { get; private set; }
        public int WinnerId => (LastMove == null) ? 0 : (LastMove.IsConnected) ? LastMove.PlayerId : 0;

        public Board(int width, int height, IFieldFactory fieldFactory)
        {
            Width = width;
            Height = height;
            this.fieldFactory = fieldFactory;

            Reset();
        }

        public void Reset()
        {
            LastMove = null;

            fields = new IField[Height, Width];
            for (int i = 0; i < fields.GetLength(0); i++)
            {
                for (int j = 0; j < fields.GetLength(1); j++)
                {
                    fields[i, j] = fieldFactory.Create(i, j);
                }
            }
        }

        public bool IsColumnValid(int column)
        {
            if (column < 0 || column >= Width)
            {
                return false;
            }
            return fields.Cast<IField>().Take(7).ToList()[column].PlayerId == 0 && WinnerId == 0;
        }

        public IMove InsertInColumn(int column, int playerId)
        {
            if (column < 0 || column >= Width)
            {
                throw new InvalidColumnException(
                    StringResources.ColumnOutsideTheScope("InsertInColumn", column));
            }

            for (int i = Height - 1; i >= 0; i--)
            {
                if (fields[i, column].PlayerId > 0) continue;
                fields[i, column].PlayerId = playerId;

                LastMove = PerformMove(i, column, playerId, 4);
                return LastMove;
            }

            throw new InvalidColumnException(
                StringResources.ColumnIsFull("InsertInColumn", column));
        }

        public void ApplyMove(IMove move)
        {
            Requires.IsNotNull(move, "move");

            if (move.Column < 0 || move.Column >= Width)
            {
                throw new InvalidColumnException(
                    StringResources.ColumnOutsideTheScope("ApplyMove", move.Column));
            }

            if (fields[move.Row, move.Column].PlayerId == 0)
            {
                fields[move.Row, move.Column].PlayerId = move.PlayerId;
                LastMove = move;
                return;
            }

            throw new InvalidColumnException(
                StringResources.ColumnIsFull("ApplyMove", move.Column));
        }

        public void RemoveChip(int column)
        {
            if (column < 0 || column >= Width)
            {
                throw new InvalidColumnException(
                    StringResources.ColumnOutsideTheScope("RemoveChip", column));
            }

            for (int i = 0; i < Height; i++)
            {
                if (fields[i, column].PlayerId == 0) continue;
                fields[i, column].PlayerId = 0;
                LastMove = null;
                return;
            }
        }

        private IMove PerformMove(int row, int column, int playerId, int needToWin = 0)
        {
            IMove result = new Move(row, column, playerId, needToWin);

            // Vertically
            // --------------------------------------------
            for (int i = row - 1; i >= 0; i--)
            {
                if (fields[i, column].PlayerId != playerId) break;
                result.ConnectionVertical.Add(fields[i, column]);
            }
            result.ConnectionVertical.Add(fields[row, column]);
            for (int i = row + 1; i < Height; i++)
            {
                if (fields[i, column].PlayerId != playerId) break;
                result.ConnectionVertical.Add(fields[i, column]);
            }

            // Horizontally
            // --------------------------------------------
            for (int i = column - 1; i >= 0; i--)
            {
                if (fields[row, i].PlayerId != playerId) break;
                result.ConnectionHorizontal.Add(fields[row, i]);
            }
            result.ConnectionHorizontal.Add(fields[row, column]);
            for (int i = column + 1; i < Width; i++)
            {
                if (fields[row, i].PlayerId != playerId) break;
                result.ConnectionHorizontal.Add(fields[row, i]);
            }

            // Diagonally - Northwest to Southeast
            // --------------------------------------------

            // Northwest
            for (int i = 1; i <= Math.Min(row, column); i++)
            {
                if (fields[row - i, column - i].PlayerId != playerId) break;
                result.ConnectionDescendingDiagonal.Add(fields[row - i, column - i]);
            }
            result.ConnectionDescendingDiagonal.Add(fields[row, column]);
            // Southeast
            for (int i = 1; i < Math.Min(Height - row, Width - column); i++)
            {
                if (fields[row + i, column + i].PlayerId != playerId) break;
                result.ConnectionDescendingDiagonal.Add(fields[row + i, column + i]);
            }

            // Diagonally - Southwest to Northeast  
            // --------------------------------------------

            // Southwest
            for (int i = 1; i <= Math.Min(Height - row - 1, column); i++)
            {
                if (fields[row + i, column - i].PlayerId != playerId) break;
                result.ConnectionAscendingDiagonal.Add(fields[row + i, column - i]);
            }
            result.ConnectionAscendingDiagonal.Add(fields[row, column]);
            // Northeast
            for (int i = 1; i <= Math.Min(row, Width - column - 1); i++)
            {
                if (fields[row - i, column + i].PlayerId != playerId) break;
                result.ConnectionAscendingDiagonal.Add(fields[row - i, column + i]);
            }

            result.IsTie = (!fields.Cast<IField>().Take(7).Any(f => f.PlayerId == 0)) && !result.IsConnected;
            return result;
        }

        public override string ToString()
        {
            string output = string.Empty;
            for (int i = 0; i < Height; i++)
            {
                string line = "";
                for (int j = 0; j < Width; j++)
                {
                    line += fields[i, j].PlayerId + " ";
                }
                output += line + Environment.NewLine;
            }
            return output;
        }
    }
}
