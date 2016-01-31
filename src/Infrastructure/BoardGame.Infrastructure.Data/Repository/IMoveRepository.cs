using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Infrastructure.Data.DTO;
using BoardGame.Infrastructure.Data.Interfaces;

namespace BoardGame.Infrastructure.Data.Repository
{
    public interface IMoveRepository : IRepository<MoveDto>
    {
        IEnumerable<MoveDto> GetMovesByGameId(int gameId);
    }
}
