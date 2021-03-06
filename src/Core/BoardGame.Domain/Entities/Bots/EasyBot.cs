﻿using System;
using System.Linq;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Exceptions;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Entities.Bots
{
    public class EasyBot : IBot
    {
        public string DisplayName => "Easy";

        public int GenerateMove(IGame game)
        {
            if (!game.NextPlayer.Type.Equals(PlayerType.Bot))
            {
                throw new GenerateMoveException(
                    StringResources.CannotGenerateMoveNextPlayerIsNotBot());
            }

            int botId = game.Players.SingleOrDefault(p => p.Type.Equals(PlayerType.Bot)).OnlineId;
            Random random = new Random();
            while (true)
            {
                int move = random.Next(0, game.Board.Width);
                return move;
            }
        }
    }
}
