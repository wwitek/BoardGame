using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Entities.Bots;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using NUnit.Framework;

namespace BoardGame.Tests
{
    [TestFixture]
    public class BotTests
    {
        [Test]
        public void GenerateEasyMoveTest()
        {
            IBot bot = new EasyBot();
            IGame game = TestHelper.CreateGame(PlayerType.Bot, bot);
            game.MakeMove(1);

            IMove move = bot.GenerateMove(game);
        }

        [Test]
        public void GenerateMediumMoveTest()
        {
            IBot bot = new MediumBot();
            IGame game = TestHelper.CreateGame(PlayerType.Bot, bot);
            game.MakeMove(1);

            //TODO Make it faster
            //IMove move = bot.GenerateMove(game);
        }
    }
}
