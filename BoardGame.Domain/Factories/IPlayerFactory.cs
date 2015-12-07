using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Factories
{
    public interface IPlayerFactory
    {
        IPlayer Create(PlayerType type, int id = 0);
    }
}