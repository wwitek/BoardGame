using System.Collections.Generic;

namespace BoardGame.Domain.Interfaces
{
    public interface IMove
    {
        int Row { get; set; }
        int Column { get; set; }
        int PlayerId { get; set; }

        List<IField> ConnectionVertical { get; set; }
        List<IField> ConnectionHorizontal { get; set; }
        List<IField> ConnectionDescendingDiagonal { get; set; } // Northwest to Southeast
        List<IField> ConnectionAscendingDiagonal { get; set; } // Southwest to Northeast 

        bool IsTie { get; set; }
        bool IsConnected { get; }
    }
}