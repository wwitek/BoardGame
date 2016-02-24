using BoardGame.API;
using BoardGame.Client.Proxies;
using BoardGame.Contracts;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Entities.Bots;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Logger;
using Ninject.Modules;
using System;

namespace BoardGame.Client.Connect4.Mobile.NinjectModules
{
    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFieldFactory>().To<FieldFactory>();
            Bind<IBoard>().To<Board>()
                .WithConstructorArgument("width", 7)
                .WithConstructorArgument("height", 6);

            Bind<IBot>().To<EasyBot>();
            Bind<IBot>().To<MediumBot>();

            Bind<IGameFactory>().To<GameFactory>();
            Bind<IPlayerFactory>().To<PlayerFactory>();
            Bind<IGameService>().To<GameProxyHttp>().
                WhenInjectedExactlyInto<GameAPI>();
            Bind<ILogger>().To<WpfTempLogger>();

            Bind<GameAPI>().ToSelf();
        }
    }

    public class WpfTempLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
