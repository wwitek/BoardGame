using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities
{
    public class Player : IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PlayerType Type { get; set; }
    }
}
