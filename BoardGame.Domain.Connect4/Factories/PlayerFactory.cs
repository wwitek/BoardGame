﻿using BoardGame.Domain.Connect4.Entities.Players;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Connect4.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer CreatePlayer(PlayerType type)
        {
            switch (type)
            {
                    case PlayerType.Bot:
                        return new Bot();
                    case PlayerType.Human:
                        return new Human();
                    case PlayerType.OnlinePlayer:
                        return new OnlinePlayer();
            }

            return null;
        }
    }
}
