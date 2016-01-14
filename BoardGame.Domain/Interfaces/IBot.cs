namespace BoardGame.Domain.Interfaces
{
    public interface IBot
    {
        string DisplayName { get; }
        IMove GenerateMove(IGame game);
    }
}
