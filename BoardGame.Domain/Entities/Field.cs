using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities
{
    public class Field : IField
    {
        public int PlayerId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Field(int row, int column)
        {
            Row = row;
            Column = column;
            PlayerId = 0;
        }
    }
}
