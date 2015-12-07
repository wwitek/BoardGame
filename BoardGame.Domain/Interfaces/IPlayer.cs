using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Interfaces
{
    public interface IPlayer
    {
        int OnlineId { get; set; }
        string Name { get; set; }
        PlayerType Type { get; set; }
    }
}