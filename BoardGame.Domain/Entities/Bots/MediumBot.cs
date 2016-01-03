﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities.Bots
{
    public class MediumBot : IBot
    {
        private const int MaxDepth = 8;
        private const int WinValue = 1;
        private const int LoseValue = -1;
        private const int DrawValue = 0;

        private IBoard board;
        private int botId;
        private int humanId;

        public string DisplayName => "Medium";

        public IMove GenerateMove(IGame game)
        {
            board = game.Board;
            humanId = game.Players.SingleOrDefault(p => p.Type.Equals(PlayerType.Human)).OnlineId;
            botId = game.Players.SingleOrDefault(p => p.Type.Equals(PlayerType.Bot)).OnlineId;

            List<int> moves = new List<int>();
            double max = -1;

            for (int i = 0; i < board.Width; i++)
            {
                if (board.IsMoveValid(0, i, botId))
                {
                    double value = GetMoveValue(i);
                    Debug.WriteLine("Move {0}: {1}", i, value);
                    if (value > max)
                    {
                        max = value;
                        moves.Clear();
                        moves.Add(i);
                    }
                    else if (value == max)
                    {
                        moves.Add(i);
                    }
                }
            }

            Random random = new Random();
            int move = moves[random.Next(0, moves.Count)];
            return game.MakeMove(0, move);
        }

        private double GetMoveValue(int column)
        {
            IMove result = board.InsertChip(0, column, botId);
            double value = AlfaBeta(false, MaxDepth, int.MinValue, int.MaxValue);
            board.RemoveChip(0, column);
            return value;
        }

        private double AlfaBeta(bool isMax, int depth, double alfa, double beta)
        {
            if (board.WinnerId > 0 || depth == 0)
            {
                double score = 0;

                if (board.WinnerId > 0)
                {
                    score = (board.WinnerId == botId) ? WinValue : LoseValue;
                }
                else if (depth == 0)
                {
                    score = DrawValue;
                }

                return score / (MaxDepth - depth + 1);
            }

            if (isMax)
            {
                for (int i = 0; i < board.Width; i++)
                {
                    if (board.IsMoveValid(0, i, botId))
                    {
                        IMove result = board.InsertChip(0, i, botId);
                        double alfabeta = AlfaBeta(false, depth - 1, alfa, beta);
                        alfa = Math.Max(alfa, alfabeta);
                        board.RemoveChip(0, i);
                        if (beta <= alfa) break;
                    }
                }
                return alfa;
            }
            else
            {
                for (int i = 0; i < board.Width; i++)
                {
                    if (board.IsMoveValid(0, i, humanId))
                    {
                        IMove result = board.InsertChip(0, i, humanId);
                        double alfabeta = AlfaBeta(true, depth - 1, alfa, beta);
                        beta = Math.Min(beta, alfabeta);
                        board.RemoveChip(0, i);
                        if (beta <= alfa) break;
                    }
                }
                return beta;
            }
        }
    }
}