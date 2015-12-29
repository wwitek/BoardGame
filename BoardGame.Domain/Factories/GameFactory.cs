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
        private readonly IList<IBotLevel> botLevels;

        public GameFactory(IBoard board, IList<IBotLevel> botLevels = null)
        {
            this.board = board;
            this.botLevels = botLevels;
        }

        public IGame Create(IList<IPlayer> players, string botLevel)
        {
            if (botLevel?.Length == 0 && players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                //ToDo throw exception
            }

            IBotLevel level = botLevels.FirstOrDefault(b => b.DisplayName.Equals(botLevel));
            if (level == null && players.Any(p => p.Type.Equals(PlayerType.Bot)))
            {
                //ToDo throw exception
            }

            return new Game(board, players, level);
        }
    }
}
