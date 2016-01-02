using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using BoardGame.Domain.Exceptions;

namespace BoardGame.Domain.Entities
{
    public class Game : IGame
    {
        private readonly object lockObject = new object();
        private readonly ManualResetEvent waitForNextMoveHandle = new ManualResetEvent(false);
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
                lock(lockObject)
                {
                    state = value;
                    OnStateChanged?.Invoke(this, new PropertyChangedEventArgs("State"));
                }
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
        public IBot Bot { get; }

        public event PropertyChangedEventHandler OnStateChanged;

        internal Game(IBoard board, IEnumerable<IPlayer> players, IBot bot = null)
        {
            Board = board;
            Board.Reset();

            Players = players.ToList();
            Bot = bot;

            if (bot == null && Players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                throw new BotNotRegisteredException(
                    StringResources.TheGameCanNotBeStartedBecauseOfBotHasNotBeenRegistered());
            }
        }

        public bool IsMoveValid(int row, int column)
        {
            if (Players[currentPlayerIndex - 1].Type.Equals(PlayerType.Human))
            {
                return Board.IsMoveValid(row, column, currentPlayerIndex);
            }
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

        /// <summary>
        /// Method used on server side. Blocks the current thread until the currentPlayerIndex 
        /// is chnaged (SetNextPlayer is called), using a 32-bit signed integer to specify 
        /// the time interval in milliseconds.
        /// </summary>
        /// <param name="timeout">The number of milliseconds to wait, or Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <returns>True if the current instance receives a signal; otherwise, false</returns>
        public bool WaitForNextPlayer(int timeout)
        {
            waitForNextMoveHandle.Reset();
            return waitForNextMoveHandle.WaitOne(timeout);
        }

        // Private methods
        private void SetNextPlayer()
        {
            currentPlayerIndex = currentPlayerIndex == 1 ? 2 : 1;
            waitForNextMoveHandle?.Set();
        }
    }
}
