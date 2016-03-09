using BoardGame.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.API.Interfaces
{
    public interface IGameProxy
    {
        Task<int> VerifyConnection(int testNumber);
        Task<OnlineGameResponse> OnlineGameRequest(int playerId);
        Task<StartGameResponse> ConfirmToPlay(int playerId);
        Task<MoveResponse> MakeMove(int playerId, int column);
        Task<MoveResponse> GetFirstMove(int playerId);
    }
}
