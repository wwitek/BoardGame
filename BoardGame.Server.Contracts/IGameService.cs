using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BoardGame.Server.Contracts
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        int GetNextMove();

        [OperationContract]
        Task<int> GetNextMove2Async();
    }
}
