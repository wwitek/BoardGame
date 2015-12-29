using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities.BotLevels
{
    public class EasyBotLevel : IBotLevel
    {
        public string DisplayName => "Easy";

        public IMove GenerateMove(IGame game)
        {
            int botId = game.Players.SingleOrDefault(p => p.Type.Equals(PlayerType.Bot)).OnlineId;
            Random random = new Random();
            while (true)
            {
                int move = random.Next(0, game.Board.Width);
                if (game.Board.IsMoveValid(0, move, botId))
                {
                    return game.MakeMove(0, move);
                }
            }
        }
    }
}
