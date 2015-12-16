using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace BoardGame.Server.Services
{
    public class GameServiceErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
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
