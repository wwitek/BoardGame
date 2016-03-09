using BoardGame.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Contracts
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginVerifyConnection(int testNumber, AsyncCallback callback, object state);
        int EndVerifyConnection(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginOnlineGameRequest(int playerId, AsyncCallback callback, object state);
        OnlineGameResponse EndOnlineGameRequest(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginConfirmToPlay(int playerId, AsyncCallback callback, object state);
        StartGameResponse EndConfirmToPlay(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetFirstMove(int playerId, AsyncCallback callback, object state);
        MoveResponse EndGetFirstMove(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMakeMove(int playerId, int column, AsyncCallback callback, object state);
        MoveResponse EndMakeMove(IAsyncResult asyncResult);
    }
}
