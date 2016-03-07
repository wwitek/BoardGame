using BoardGame.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Contracts.Responses;
using System.ServiceModel;
using BoardGame.API.Interfaces;

namespace BoardGame.Client.Proxies
{
    public class GameProxyAsync : IGameProxy
    {
        private IGameServiceAsync proxy = null;

        public GameProxyAsync()
        {
            string address = "http://localhost:9004/GameService";
            Uri addressBase = new Uri(address);
            EndpointAddress endpoint = new EndpointAddress(address);
            BasicHttpBinding bHttp = new BasicHttpBinding();
            ChannelFactory<IGameServiceAsync> channel = new ChannelFactory<IGameServiceAsync>(bHttp, endpoint);
            proxy = channel.CreateChannel(endpoint);
        }

        public async Task<OnlineGameResponse> OnlineGameRequest(int playerId)
        {
            return await proxy.OnlineGameRequest(playerId);
        }

        public async Task<StartGameResponse> ConfirmToPlay(int playerId)
        {
            return await proxy.ConfirmToPlay(playerId);
        }

        public async Task<MoveResponse> MakeMove(int playerId, int column)
        {
            return await proxy.MakeMove(playerId, column);
        }

        public async Task<MoveResponse> GetFirstMove(int playerId)
        {
            return await proxy.GetFirstMove(playerId);
        }
    }
}
