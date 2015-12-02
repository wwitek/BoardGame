using System;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace BoardGame.Server.Services
{
    public class GameServiceHost : ServiceHost
    {
        public GameServiceHost(IContractBehavior contractBehavior, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            ImplementedContracts.Values
                .First(c => c.Name == "IGameService")
                .ContractBehaviors.Add(contractBehavior);
        }
    }
}
