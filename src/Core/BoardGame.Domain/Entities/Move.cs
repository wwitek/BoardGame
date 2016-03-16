using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Interfaces;
using System.Runtime.Serialization;

namespace BoardGame.Domain.Entities
{
    [DataContract]
    public class Move : IMove
    {
        [DataMember]
        private readonly int NeedToWin;

        [DataMember]
        public int Row { get; set; }

        [DataMember]
        public int Column { get; set; }

        [DataMember]
        public int PlayerId { get; set; }

        [DataMember]
        public List<IField> ConnectionVertical { get; set; }

        [DataMember]
        public List<IField> ConnectionHorizontal { get; set; }

        [DataMember]
        public List<IField> ConnectionDescendingDiagonal { get; set; } // Northwest to Southeast

        [DataMember]
        public List<IField> ConnectionAscendingDiagonal { get; set; } // Southwest to Northeast  

        [DataMember]
        public bool IsTie { get; set; }

        internal Move(int row, int column, int playerId, int needToWin = 0)
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
