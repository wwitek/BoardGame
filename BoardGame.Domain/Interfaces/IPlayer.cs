using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Interfaces
{
    public interface IPlayer
    {
        int Id { get; set; }
        string Name { get; set; }
        PlayerType Type { get; set; }
    }
}