using System.ServiceModel;

namespace BoardGame.Server.Contracts
{
    public interface IGameServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCallback();
    }
}
