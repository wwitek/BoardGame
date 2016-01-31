using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Infrastructure.Data.Repository;

namespace BoardGame.Infrastructure.Data.Interfaces
{
    public interface IUnitOfWork
    {
        int Complete();
        IMoveRepository Moves { get; }
    }
}
