using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BoardGame.Domain.Entities
{
    public class Game : IGame
    {
        private int currentPlayerIndex = 1;

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
            IMove result = Board.InsertChip(row, column, currentPlayerIndex);

            // Set next player
            if (!result.IsConnected)
                SetNextPlayer();

            return result;
        }

        // Private methods
        private void SetNextPlayer()
        {
            currentPlayerIndex = currentPlayerIndex == 1 ? 2 : 1;
        }
    }
}
