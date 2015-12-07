using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities
{
    public class Move : IMove
    {
        private readonly int NeedToWin;

        public int Row { get; set; }
        public int Column { get; set; }
        public int PlayerId { get; set; }

        public List<IField> ConnectionVertical { get; set; }
        public List<IField> ConnectionHorizontal { get; set; }
        public List<IField> ConnectionDescendingDiagonal { get; set; } // Northwest to Southeast
        public List<IField> ConnectionAscendingDiagonal { get; set; } // Southwest to Northeast  

        public bool IsTie { get; set; }

        public Move(int row, int column, int playerId, int needToWin = 0)
        {
            Row = row;
            Column = column;
            PlayerId = playerId;
            NeedToWin = needToWin;

            ConnectionVertical = new List<IField>();
            ConnectionHorizontal = new List<IField>();
            ConnectionDescendingDiagonal = new List<IField>();
            ConnectionAscendingDiagonal = new List<IField>();
        }

        // Get only properties
        public bool IsConnected =>
            IsVerticallyConnected || IsHorizontallyConnected || IsDiagonallyDescendingConnected || IsDiagonallyAscendingConnected;

        public bool IsVerticallyConnected =>
            ConnectionVertical != null && ConnectionVertical.Count >= NeedToWin;

        public bool IsHorizontallyConnected =>
            ConnectionHorizontal != null && ConnectionHorizontal.Count >= NeedToWin;

        public bool IsDiagonallyDescendingConnected =>
            ConnectionDescendingDiagonal != null && ConnectionDescendingDiagonal.Count >= NeedToWin;

        public bool IsDiagonallyAscendingConnected =>
            ConnectionAscendingDiagonal != null && ConnectionAscendingDiagonal.Count >= NeedToWin;

    }
}
