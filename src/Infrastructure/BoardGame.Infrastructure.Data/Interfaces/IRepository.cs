using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Infrastructure.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IDto
    {
        TEntity Get(int id);
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAll();

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
