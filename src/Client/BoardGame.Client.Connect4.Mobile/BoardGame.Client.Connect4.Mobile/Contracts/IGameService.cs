using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BoardGame.Client.Connect4.Mobile.Contracts.Responses;

namespace BoardGame.Client.Connect4.Mobile.Contracts
{
    [ServiceContract]
    public interface IGameService
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
