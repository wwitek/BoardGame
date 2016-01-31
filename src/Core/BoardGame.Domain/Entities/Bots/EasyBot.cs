using System;
using System.Linq;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities.Bots
{
    public class EasyBot : IBot
    {
        public string DisplayName => "Easy";

        public IMove GenerateMove(IGame game)
        {
            int botId = game.Players.SingleOrDefault(p => p.Type.Equals(PlayerType.Bot)).OnlineId;
            Random random = new Random();
            while (true)
            {
                int move = random.Next(0, game.Board.Width);
                if (game.Board.IsMoveValid(-1, move, botId))
                {
                    IMove madeMove = game.MakeMove(-1, move);
                    madeMove.IsBot = true;
                    return madeMove;
                }
            }
        }
    }
}
