using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Entities;

namespace BoardGame.Domain.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(PlayerType type, int id)
        {
            return new Player(type, id);
        }
    }
}
