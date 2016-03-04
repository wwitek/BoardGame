using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BoardGame.Contracts.Responses;

namespace BoardGame.Contracts
{
    [ServiceContract]
    public interface IGameServiceAsync
    {
        [OperationContract]
        Task<OnlineGameResponse> OnlineGameRequest(int playerId);

        [OperationContract]
        Task<StartGameResponse> ConfirmToPlay(int playerId);

        [OperationContract]
        Task<MoveResponse> MakeMove(int playerId, int column);

        [OperationContract]
        Task<MoveResponse> GetFirstMove(int playerId);
    }
}
