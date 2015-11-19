using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Connect4.Entities.Players
{
    public class Bot : Player
    {
        public Bot(int id)
            : base(id)
        {
            Type = PlayerType.Bot;
            Name = "Bot";
        }
    }
}
