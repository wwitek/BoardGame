using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using NSubstitute;

namespace BoardGame.Tests
{
    internal static class TestHelper
    {
        internal static IGame CreateGame(PlayerType rivalPlayerType, IBot bot = null)
        {
            var board = CreateBoard(new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0}
            });

            return new Game(board, CreatePlayers(PlayerType.Human, rivalPlayerType), bot);
        }

        internal static IGame CreateGame(PlayerType first, PlayerType second, IBot bot = null)
        {
            var board = CreateBoard(new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0}
            });

            return new Game(board, CreatePlayers(first,second), bot);
        }

        internal static IBoard CreateBoard(int[,] ids)
        {
            var fieldFactory = Substitute.For<IFieldFactory>();
            fieldFactory.Create(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x =>
                {
                    var field = Substitute.For<IField>();
                    field.Row = (int)x[0];
                    field.Column = (int)x[1];
                    field.PlayerId = ids[field.Row, field.Column];
                    return field;
                });
            return new Board(7, 6, fieldFactory);
        }

        internal static IList<IPlayer> CreatePlayers(PlayerType first, PlayerType second)
        {
            var player1 = Substitute.For<IPlayer>();
            player1.Name = "Player1";
            player1.OnlineId = 1;
            player1.Type = first;

            var player2 = Substitute.For<IPlayer>();
            player2.Name = "Player2";
            player2.OnlineId = 2;
            player2.Type = second;

            var players = new List<IPlayer>()
            {
                player1,
                player2
            };
            return players;
        }

        internal static IBot CreateBot(IGame game)
        {
            var bot = Substitute.For<IBot>();
            bot.GenerateMove(game).Returns(x => null);

            return bot;
        }

        internal static IMove CreateMove(int row, int column, int playerId)
        {
            var move = Substitute.For<IMove>();
            move.Column = column;
            move.Row = 5;
            move.PlayerId = 1;
            return move;
        }
    }
}
