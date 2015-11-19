using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Factories
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(PlayerType type, int id);
    }
}