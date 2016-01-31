using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BoardGame.Infrastructure.Data.Interfaces;

namespace BoardGame.Infrastructure.Data.EF
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IDto
    {
        protected DbContext Context { get; }

        public Repository(DbContext dbContext)
        {
            Context = dbContext;
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
