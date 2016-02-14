using System.Threading;
using System.Threading.Tasks;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Exceptions;
using BoardGame.Domain.Interfaces;
using NUnit.Framework;

namespace BoardGame.Tests
{
    [TestFixture]
    class GameTests
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {   
        }

        [Test]
        public void BotNotRegisteredExceptionTest()
        {
            Assert.Throws<BotNotRegisteredException>(() => TestHelper.CreateGame(PlayerType.Bot));
        }

        [Test]
        public void BotNotRegisteredException_TwoBotsTest()
        {
            IBot bot = TestHelper.CreateBot();
            Assert.Throws<GameCreateException>(() => TestHelper.CreateGame(PlayerType.Bot, PlayerType.Bot, bot));
        }

        [Test]
        public void BotDoesNotThrowNotRegisteredExceptionTest()
        {
            IBot bot = TestHelper.CreateBot();
            Assert.DoesNotThrow(() => TestHelper.CreateGame(PlayerType.Bot, bot));
        }

        [Test]
        public void BotDoesNotThrowNotRegisteredException_TwoOnlineTest()
        {
            Assert.DoesNotThrow(() => TestHelper.CreateGame(PlayerType.OnlinePlayer, PlayerType.OnlinePlayer));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsMoveValid_OnTopTest(int column)
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human);
            Assert.IsTrue(game.IsMoveValid(-1, column));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsMoveValid_LastRowTest(int column)
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human);
            Assert.IsTrue(game.IsMoveValid(5, column));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsMoveValid_InvalidTest(int column)
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human);
            for (int i = 0; i < 5; i++)
            {
                Assert.IsFalse(game.IsMoveValid(i, column));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsMoveValid_NotHumanMoveTest(int column)
        {
            IBot bot = TestHelper.CreateBot();
            IGame game = TestHelper.CreateGame(PlayerType.Bot, PlayerType.Human, bot);
            Assert.IsFalse(game.IsMoveValid(-1, column));
        }

        [Test]
        public void GameStateNewTest()
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);
            Assert.AreEqual(GameState.New, game.State);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void GameStateRunningAfterMoveTest(int column)
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);
            game.MakeMove(-1, column);
            Assert.AreEqual(GameState.Running, game.State);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void MakeMoveTest(int column)
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);
            IMove move = TestHelper.CreateMove(5, column, 1);
            game.MakeMove(move);
        }

        [Test]
        public void WaitForNextPlayer_TimeoutTest()
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);

            bool timeout = game.WaitForNextPlayer(10);
            Assert.IsFalse(timeout);
        }

        [Test]
        public void WaitForNextPlayer_NoTimeoutTest()
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);

            Task.Run(() =>
            {
                Thread.Sleep(10);
                game.MakeMove(-1, 0);
            });

            bool timeout = game.WaitForNextPlayer(500);
            Assert.IsTrue(timeout);
        }

        [Test]
        public void OnStateChangedWasCalledTest()
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);
            bool wasCalled = false;
            game.OnStateChanged += (o, e) => wasCalled = true;
            game.State = GameState.Running;
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void NextPlayerTest()
        {
            IGame game = TestHelper.CreateGame(PlayerType.Human, PlayerType.Human);

            var player1 = game.NextPlayer;
            game.MakeMove(-1, 0);
            var player2 = game.NextPlayer;
            Assert.AreNotEqual(player1.OnlineId,player2.OnlineId);
        }
    }
}