using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Connect4.Entities.Players
{
    public class Human : Player
    {
        public Human(int id)
            : base(id)
        {
            Type = PlayerType.Human;
            Name = "Player " + id;
        }
    }
}