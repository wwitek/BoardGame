﻿using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;

namespace BoardGame.API
{
    public class GameAPI
    {
        public IGame CurrentGame { get; set; }
        public IGameFactory GameFactory { get; set; }

        public IGame CreateGame(GameType type)
        {
            CurrentGame = GameFactory.CreateGame(type);
            return CurrentGame;
        }

        public IMove MakeMove(int row, int column)
        {
            if (CurrentGame == null) return null;
            IPlayer player = CurrentGame.CurrentPlayer;

            if (player.Type == PlayerType.Human)
            {
                return CurrentGame.MakeMove(row, column, player.Id);
            }

            return null;
        }
    }
}
