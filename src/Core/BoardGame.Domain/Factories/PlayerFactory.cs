using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Exceptions;

namespace BoardGame.Domain.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(PlayerType type, int id)
        {
            if (type.Equals(PlayerType.Bot) && id < 1)
            {
                throw new PlayerCreateException(
                    StringResources.ThePlayerCannotBeCreatedBecauseBotMustHaveId());
            }

            return new Player(type, id);
        }
    }
}
