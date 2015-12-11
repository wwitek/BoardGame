using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace BoardGame.Domain.Entities
{
    public class Game : IGame
    {
        private ManualResetEvent waitForNextMoveHandle = new ManualResetEvent(false);
        private int currentPlayerIndex = 1;
        private GameState state;

        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                if (OnStateChanged != null)
                    OnStateChanged(this, new PropertyChangedEventArgs("State"));
            }
        }
        public IBoard Board { get; }
        public List<IPlayer> Players { get; }
        public IPlayer NextPlayer
        {
            get
            {
                if (Players.Count < currentPlayerIndex) return null;
                return Players[currentPlayerIndex - 1];
            }
        }
        public IMove LastMove { get; set; }

        public event PropertyChangedEventHandler OnStateChanged;

        public Game(IBoard board, IEnumerable<IPlayer> players)
        {
            Board = board;
            Board.Reset();

            Players = players.ToList();
        }

        public bool IsMoveValid(int row, int column)
        {
            if (Players[currentPlayerIndex - 1].Type.Equals(PlayerType.Human))
            {
                return Board.IsMoveValid(row, column, currentPlayerIndex);
            }

            //if current player is Bot or Online player, your move is not allowed
            return false;
        }

        public IMove MakeMove(int row, int column)
        {
            LastMove = Board.InsertChip(row, column, currentPlayerIndex);
            SetNextPlayer();
            return LastMove;
        }

        public void MakeMove(IMove move)
        {
            Board.InsertChip(move);
            LastMove = move;
            SetNextPlayer();
        }

        public bool WaitForNextPlayer(int timeout)
        {
            waitForNextMoveHandle.Reset();
            return waitForNextMoveHandle.WaitOne(timeout);
        }

        // Private methods
        private void SetNextPlayer()
        {
            currentPlayerIndex = currentPlayerIndex == 1 ? 2 : 1;
            if (waitForNextMoveHandle != null)
                waitForNextMoveHandle.Set();
        }
    }
}
