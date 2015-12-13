using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities
{
    public class Player : IPlayer
    {
        public int OnlineId { get; set; }
        public string Name { get; set; }
        public PlayerType Type { get; set; }
        public bool Confirmed { get; set; }

        public Player(PlayerType type, int onlineId = 0)
        {
            OnlineId = onlineId;
            Type = type;
            Confirmed = false;
        }
    }
}
