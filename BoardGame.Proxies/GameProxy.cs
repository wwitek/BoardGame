using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using BoardGame.Server.Contracts;
using System.Threading.Tasks;

namespace BoardGame.Proxies
{
    public class GameProxy : ClientBase<IGameService>, IGameService
    {
        public int GetNextMove()
        {
            return Channel.GetNextMove();
        }

        public async Task<int> GetNextMoveAsync()
        {
            return await Channel.GetNextMoveAsync();
        }
    }
}
