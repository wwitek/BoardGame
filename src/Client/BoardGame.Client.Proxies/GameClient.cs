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
    public class GameClient : IGameService
    {
        private IGameService proxy = null;

        public GameClient()
        {
            string address = "http://localhost:9003/GameService";
            Uri addressBase = new Uri(address);
            EndpointAddress endpoint = new EndpointAddress(address);
            BasicHttpBinding bHttp = new BasicHttpBinding();
            ChannelFactory<IGameService> channel = new ChannelFactory<IGameService>(bHttp, endpoint);
            proxy = channel.CreateChannel(endpoint);
        }

        public IAsyncResult BeginConfirmToPlay(int playerId, AsyncCallback callback, object state)
        {
            return proxy.BeginConfirmToPlay(playerId, callback, state);
        }

        public IAsyncResult BeginGetFirstMove(int playerId, AsyncCallback callback, object state)
        {
            return proxy.BeginGetFirstMove(playerId, callback, state);
        }

        public IAsyncResult BeginMakeMove(int playerId, int column, AsyncCallback callback, object state)
        {
            return proxy.BeginMakeMove(playerId, column, callback, state);
        }

        public IAsyncResult BeginOnlineGameRequest(int playerId, AsyncCallback callback, object state)
        {
            return proxy.BeginOnlineGameRequest(playerId, callback, state);
        }

        public IAsyncResult BeginVerifyConnection(int testNumber, AsyncCallback callback, object state)
        {
            return proxy.BeginVerifyConnection(testNumber, callback, state);
        }

        public StartGameResponse EndConfirmToPlay(IAsyncResult asyncResult)
        {
            return proxy.EndConfirmToPlay(asyncResult);
        }

        public MoveResponse EndGetFirstMove(IAsyncResult asyncResult)
        {
            return proxy.EndGetFirstMove(asyncResult);
        }

        public MoveResponse EndMakeMove(IAsyncResult asyncResult)
        {
            return proxy.EndMakeMove(asyncResult);
        }

        public OnlineGameResponse EndOnlineGameRequest(IAsyncResult asyncResult)
        {
            return proxy.EndOnlineGameRequest(asyncResult);
        }

        public int EndVerifyConnection(IAsyncResult asyncResult)
        {
            return proxy.EndVerifyConnection(asyncResult);
        }
    }
}