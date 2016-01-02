using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.API
{
    internal static class StringResources
    {
        internal static string TheGameCanNotBeStartedBecauseOfOnMoveReceivedIsNull()
        {
            return @"The Game cannot be started, because OnMoveReceived event handler 
                     has not been set to an instance of an object. Please make sure 
                     the OnMoveReceived handler is registered in UI.";
        }

        internal static string TheGameCanNotBeStartedBecauseOfProxyIsNull()
        {
            return @"The Game cannot be started in online mode, because Proxy object 
                     has not been set to an instance of an object. Please make sure 
                     Proxy object was created and passed in GameAPI constructor.";
        }

        internal static string CanNotPerformOnlineMoveBecauseOfProxyIsNull()
        {
            return @"Cannot perform the next move, becasue it is online player's turn  
                     and Proxy has not been set to an instance of an object. 
                     Please make sure Proxy object was created and passed in GameAPI constructor.";
        }

        internal static string CanNotPerformTheMoveBecauseGameIsNull()
        {
            return @"Cannot perform the move, becasue CurrectGame object is null. 
                     Please make sure CurrentGame was created properly.";
        }

        internal static string CanNotPerformNextMoveBecauseNextPlayerIsNull()
        {
            return @"Cannot perform the next move, becasue NextPlayer object is null.";
        }

        internal static string CanNotPerformBotsMoveBecauseBotWasNotDefined()
        {
            return @"Cannot perform the next move, becasue Bot was not defined.";
        }

        internal static string CommunicationProblemOccured(string where, string exceptionMessage)
        {
            return string.Format(CultureInfo.InvariantCulture,
                @"Communication problem occured in GameAPI's {0} method. Exception: {1}.", where, exceptionMessage);
        }

        internal static string ExceptionOccuredOnServerSide(string where, string exceptionMessage)
        {
            return string.Format(CultureInfo.InvariantCulture,
                @"Server side exception occured in GameAPI's {0} method. Exception: {1}.", where, exceptionMessage);
        }

        internal static string TimeoutExceptionOccured(string where, string exceptionMessage)
        {
            return string.Format(CultureInfo.InvariantCulture,
                @"TimeoutException occured in GameAPI's {0} method. Exception: {1}.", where, exceptionMessage);
        }
    }
}
