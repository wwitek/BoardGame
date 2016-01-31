using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Factories
{
    public interface IFieldFactory
    {
        IField Create(int row, int column);
    }
}
