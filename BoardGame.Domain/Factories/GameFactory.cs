using BoardGame.Domain.Entities;
using BoardGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;

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
            if (botName?.Length == 0 && players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                //ToDo throw exception
            }

            IBot bot = bots.FirstOrDefault(b => b.DisplayName.Equals(botName));

            if (bot == null && players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                //ToDo throw exception
            }

            return new Game(board, players, bot);
        }
    }
}