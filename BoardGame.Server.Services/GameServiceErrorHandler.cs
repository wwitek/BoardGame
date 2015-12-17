using BoardGame.Domain.Logger;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace BoardGame.Server.Services
{
    public class GameServiceErrorHandler : IErrorHandler
    {
        private ILogger Logger { get; }

        public GameServiceErrorHandler(ILogger logger)
        {
            Logger = logger;
        }

        public bool HandleError(Exception error)
        {
            Logger.Error(error.Message, error);
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            FaultException fe = new FaultException();
            MessageFault message = fe.CreateMessageFault();
            fault = Message.CreateMessage(version, message, null);
        }
    }
}
