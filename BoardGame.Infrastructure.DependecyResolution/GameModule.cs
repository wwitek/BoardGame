using BoardGame.Domain.Connect4.Factories;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using Ninject.Modules;

namespace BoardGame.Infrastructure.DependecyResolution
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGameFactory>().To<GameFactory>();
            Bind<IBoard>().To<Board>();
        }
    }
}
