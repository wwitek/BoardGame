using System;
using BoardGame.Infrastructure.Data.EF.Contexts;
using BoardGame.Infrastructure.Data.EF.Repositories;
using BoardGame.Infrastructure.Data.Interfaces;
using BoardGame.Infrastructure.Data.Repository;

namespace BoardGame.Infrastructure.Data.EF
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected GameContext Context { get; }
        public IMoveRepository Moves { get; }

        public UnitOfWork(GameContext context)
        {
            Context = context;
            Moves = new MoveRepository(Context);
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
