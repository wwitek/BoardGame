using BoardGame.Domain.Entities;
using BoardGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Exceptions;

namespace BoardGame.Domain.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IBoard board;
        private readonly IList<IBot> bots;

        public GameFactory(IBoard board, IList<IBot> bots = null)
        {
            this.board = board;
            this.bots = bots;
        }

        public IGame Create(IList<IPlayer> players, string botName)
        {
            #region Exception handling
            if (players.Count != 2)
            {
                throw new GameCreateException(
                    StringResources.TheGameMustHaveTwoPlayers());
            }

            if (players.Count(p => p.Type == PlayerType.Human) == 0)
            {
                throw new GameCreateException(
                    StringResources.TheGameMustHaveAHumanPlayer());
            }

            if (players.Count(p => p.Type == PlayerType.Bot) > 1)
            {
                throw new GameCreateException(
                    StringResources.TheGameCanNotBeCreatedBecauseOfTooManyBots());
            }

            if (players.Count(p => p.Type == PlayerType.OnlinePlayer) > 1)
            {
                throw new GameCreateException(
                    StringResources.TheGameCanNotBeCreatedBecauseOfTooManyOnlinePlayers());
            }

            if (string.IsNullOrEmpty(botName) && players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                throw new GameCreateException(
                    StringResources.TheGameCanNotBeCreatedBecauseBotHasNotBeenRegistered());
            }

            if (players.Any(p => p.Type.Equals(PlayerType.Bot)) 
                && players.Count(p => p.Type.Equals(PlayerType.Human) && p.OnlineId == 0) > 0)
            {
                throw new GameCreateException(
                    StringResources.PlayersMustHaveIdGreaterThanZero());
            }

            #endregion

            IBot bot = bots.FirstOrDefault(b => b.DisplayName.Equals(botName));

            #region Exception handling
            if (bot == null && players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                throw new GameCreateException(
                    StringResources.TheGameCanNotBeCreatedBecauseBotHasNotBeenFound(botName));
            }
            #endregion

            return new Game(board, players, bot);
        }
    }
}