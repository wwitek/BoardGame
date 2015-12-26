using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;

namespace BoardGame.Domain.Interfaces
{
    public interface IBot
    {
        IMove MakeMove(BotLevel level, IGame game);
    }
}
