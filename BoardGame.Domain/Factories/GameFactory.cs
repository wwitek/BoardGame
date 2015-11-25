using BoardGame.Domain.Entities;
using BoardGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Domain.Factories
{
    public class GameFactory : IGameFactory
    {
        public IBoard Board { get; set; }
        public GameFactory(IBoard board)
        {
            Board = board;
        }
        public IGame Create(IEnumerable<IPlayer> players)
        {
            return new Game(Board, players);
        }
    }
}
