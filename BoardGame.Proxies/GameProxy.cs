﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using BoardGame.Server.Contracts;
using System.Threading.Tasks;
using BoardGame.Server.Contracts.Responses;

namespace BoardGame.Proxies
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

        public Task<MoveResponse> MakeMove(int playerId, int row, int column)
        {
            throw new NotImplementedException();
        }
    }
}
