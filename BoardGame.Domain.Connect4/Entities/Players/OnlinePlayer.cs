using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Connect4.Entities.Players
{
    public class OnlinePlayer : Player
    {
        public OnlinePlayer(int id)
            : base(id)
        {
            Type = PlayerType.OnlinePlayer;
            Name = "Player " + id;
        }
    }
}