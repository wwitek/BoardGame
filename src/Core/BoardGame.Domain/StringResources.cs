using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain
{
    internal static class StringResources
    {
        internal static string TheGameCanNotBeStartedBecauseOfBotHasNotBeenRegistered()
        {
            return @"The Game cannot be started, because no bots were registered.  
                     Please make sure that Game object is created with IBot parameter";
        }

        internal static string TheGameCanNotBeCreatedBecauseBotHasNotBeenRegistered()
        {
            return @"The Game cannot be created, because botName has not beed passed 
                     as argument to GameFactory.  
                     Please make sure that Create method is called with botName parameter";
        }

        internal static string TheGameCanNotBeCreatedBecauseBotHasNotBeenFound(string name)
        {
            return string.Format(CultureInfo.InvariantCulture,
                     @"The Game cannot be created. Bot's name - {0} - has not been found in 
                     GameFactory Bots list.
                     Please make sure that you have created bot with DisplayName {0} and passed it 
                     in GameFactory constructor.", name);
        }

        internal static string TheGameCanNotBeCreatedBecauseOfTooManyBots()
        {
            return @"You cannot create a game with more then one Bot player.";
        }

        internal static string TheGameMustHaveTwoPlayers()
        {
            return @"The Game cannot be created. It has to have two players.";
        }

        internal static string PlayersMustHaveIdGreaterThanZero()
        {
            return @"The Game cannot be created. Players must have id greater than zero in Single Player mode.";
        }

        internal static string ThePlayerCannotBeCreatedBecauseBotMustHaveId()
        {
            return @"The Player cannot be created, because Bot player must have id greater than zero.";
        }

        internal static string ColumnOutsideTheScope(string action, int column)
        {
            return string.Format(CultureInfo.InvariantCulture,
                     @"Cannot perform {0}. The column ({1}) is outside the scope.", action, column);
        }

        internal static string ColumnIsFull(string action, int column)
        {
            return string.Format(CultureInfo.InvariantCulture,
                     @"Cannot perform {0}. The column ({1}) is full already.", action, column);
        }

    }
}
