using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using BoardGame.Server.Contracts;
using System.Threading.Tasks;

namespace BoardGame.Proxies
{
    public class GameProxy : DuplexChannelFactory<IGameService>, IGameService
    {
        public GameProxy()
            : base (new InstanceContext(new GameServiceCallback()), "GameCallback")
        {
        }

        public int GetNextMove()
        {
            return CreateChannel().GetNextMove();
        }

        public async Task<int> GetNextMove2Async()
        {
            return await CreateChannel().GetNextMove2Async();
        }

        public void Start()
        {
            CreateChannel().Start();
        }
    }
}
