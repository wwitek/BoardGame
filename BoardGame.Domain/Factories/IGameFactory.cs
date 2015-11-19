using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;

namespace BoardGame.Domain.Factories
{
    public interface IGameFactory
    {
        IGame CreateGame(GameType type, IBoard board);
    }
}
