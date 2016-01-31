using System;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using BoardGame.Domain.Logger;

namespace BoardGame.Server.Services
{
    public class GameServiceHost : ServiceHost
    {
        private readonly IErrorHandler errorHandler;

        public GameServiceHost(IContractBehavior contractBehavior, IErrorHandler errorHandler, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            this.errorHandler = errorHandler;
            ImplementedContracts.Values
                .First(c => c.Name == "IGameService")
                .ContractBehaviors.Add(contractBehavior);
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new ErrorHandlerBehaviour(errorHandler));
            base.OnOpening();
        }

        class ErrorHandlerBehaviour : IServiceBehavior, IErrorHandler
        {
            private readonly IErrorHandler errorHandler;

            public ErrorHandlerBehaviour(IErrorHandler errorHandler)
            {
                this.errorHandler = errorHandler;
            }

            bool IErrorHandler.HandleError(Exception error)
            {
                return errorHandler.HandleError(error);
            }

            void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
            {
                errorHandler.ProvideFault(error, version, ref fault);
            }

            void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
            {
            }

            void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
            {
                foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
                {
                    channelDispatcher.ErrorHandlers.Add(this);
                }
            }

            void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
            {
            }
        }

    }
}
