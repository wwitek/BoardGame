using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BoardGame.Server.Contracts
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract(IsOneWay = true)]
        void Start();

        [OperationContract]
        int GetNextMove();

        [OperationContract]
        Task<int> GetNextMove2Async();
    }
}
