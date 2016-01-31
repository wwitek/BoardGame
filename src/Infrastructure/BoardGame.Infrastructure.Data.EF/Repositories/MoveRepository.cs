using System.Collections.Generic;
using System.Linq;
using BoardGame.Infrastructure.Data.DTO;
using BoardGame.Infrastructure.Data.EF.Contexts;
using BoardGame.Infrastructure.Data.Repository;

namespace BoardGame.Infrastructure.Data.EF.Repositories
{
    public class MoveRepository : Repository<MoveDto>, IMoveRepository
    {
        public MoveRepository(GameContext dbContext) 
            : base(dbContext)
        {
        }

        public IEnumerable<MoveDto> GetMovesByGameId(int gameId)
        {
            return GameContext.Moves.Where(m => m.GameId == gameId);
        }

        private GameContext GameContext => Context as GameContext;
    }
}
