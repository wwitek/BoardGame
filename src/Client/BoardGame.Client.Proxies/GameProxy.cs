using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using BoardGame.Contracts;
using System.Threading.Tasks;
using BoardGame.Contracts.Responses;

namespace BoardGame.Client.Proxies
{
    public class GameProxy : ClientBase<IGameService>, IGameService
    {
        public async Task<OnlineGameResponse> OnlineGameRequest(int playerId)
        {
            return await Channel.OnlineGameRequest(playerId);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            return await Channel.ConfirmToPlay(playerId);
        }

        public async Task<MoveResponse> MakeMove(int playerId, int column)
        {
            return await Channel.MakeMove(playerId, column);
        }

        public async Task<MoveResponse> GetFirstMove(int playerId)
        {
            return await Channel.GetFirstMove(playerId);
        }
    }
}
