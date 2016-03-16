namespace BoardGame.Domain.Interfaces
{
    public interface IBot
    {
        string DisplayName { get; }
        int GenerateMove(IGame game);
    }
}
