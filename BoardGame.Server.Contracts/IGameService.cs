using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BoardGame.Server.Contracts.Responses;

namespace BoardGame.Server.Contracts
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        Task<OnlineGameResponse> OnlineGameRequest(int playerId);

        [OperationContract]
        Task<StartGameResponse> ConfirmToPlay(int playerId);

        [OperationContract]
        Task<MoveResponse> MakeMove(int playerId, int row, int column);

        [OperationContract]
        Task<MoveResponse> GetMove(int playerId);
    }
}
