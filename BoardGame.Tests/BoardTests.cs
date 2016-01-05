using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BoardGame.Tests
{
    [TestFixture]
    class BoardTests
    {
        private IBoard board;

        private static IBoard CreateBoard(int[,] ids)
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

        [SetUp]
        public void RunBeforeAnyTests()
        {
            board = CreateBoard(new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0}
            });
        }

        [Test]
        public void IsMoveValidTest()
        {
            for (int j = 0; j < board.Width; j++)
            {
                Assert.IsTrue(board.IsMoveValid(board.Height - 1, j, 1));
            }

            for (int i = 0; i < board.Height - 1; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    Assert.IsFalse(board.IsMoveValid(i, j, 1));
                }
            }
        }

        [Test, Description("Insert chip into full cell")]
        public void InsertChipIntoFullCell_IsMoveValidTest()
        {
            int lastRow = board.Height - 1;
            board.InsertChip(lastRow, 0, 1);
            Assert.IsFalse(board.IsMoveValid(lastRow, 0, 1));
        }

        [Test, Description("Board with chip on the bottom. Insert chip into the same column")]
        public void InsertChipOnTheTop_IsMoveValidTest()
        {
            int lastRow = board.Height - 1;
            board.InsertChip(lastRow, 0, 1);
            Assert.IsTrue(board.IsMoveValid(-1, 0, 1));
        }

        [Test, Description("Insert chip into full column")]
        public void InsertChipIntoFullColumn_IsMoveValidTest()
        {
            int lastRow = board.Height - 1;
            for (int row = lastRow; row >= 0; row--)
            {
                board.InsertChip(row, 0, (row%2 == 0) ? 1 : 2);
            }
            Assert.IsFalse(board.IsMoveValid(-1, 0, 1));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void InsertFirstChipTest(int column)
        {
            IMove result = board.InsertChip(-1, column, 1);

            Assert.AreEqual(1, result.ConnectionHorizontal.Count);
            Assert.AreEqual(1, result.ConnectionVertical.Count);
            Assert.AreEqual(1, result.ConnectionDescendingDiagonal.Count);
            Assert.AreEqual(1, result.ConnectionAscendingDiagonal.Count);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ApplyMoveTest(int column)
        {
            var move = Substitute.For<IMove>();
            move.Column = column;
            move.Row = 5;
            move.PlayerId = 1;

            board.ApplyMove(move);
        }

        [Test]
        public void ApplyNullMoveTest()
        {
            Assert.Throws<ArgumentNullException>(() => board.ApplyMove(null));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void InsertFourWinningVerticalChipsTest(int column)
        {
            board.InsertChip(-1, column, 1);
            board.InsertChip(-1, column, 1);
            board.InsertChip(-1, column, 1);
            IMove result = board.InsertChip(-1, column, 1);

            Assert.AreEqual(4, result.ConnectionVertical.Count);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void InsertFourWinningHorizontalChipsTest(int columnOffset)
        {
            board.InsertChip(-1, 0 + columnOffset, 1);
            board.InsertChip(-1, 1 + columnOffset, 1);
            board.InsertChip(-1, 2 + columnOffset, 1);
            IMove result = board.InsertChip(-1, 3 + columnOffset, 1);

            Assert.AreEqual(4, result.ConnectionHorizontal.Count);
        }

        [TestCaseSource(nameof(DescendingBoardTestSet))]
        public void InsertFourWinningDescendingChipsTest(Dictionary<int, IBoard> input)
        {
            int start = input.FirstOrDefault().Key;
            board = input.FirstOrDefault().Value;

            IMove result = null;
            for (int col = start; col < start + 4; col++)
            {
                result = board.InsertChip(-1, col, 1);
            }
            Assert.AreEqual(4, result.ConnectionDescendingDiagonal.Count);
        }

        [TestCaseSource(nameof(AscendingBoardTestSet))]
        public void InsertFourWinningAscendingChipsTest(Dictionary<int, IBoard> input)
        {
            int start = input.FirstOrDefault().Key;
            board = input.FirstOrDefault().Value;

            IMove result = null;
            for (int col = start; col < start + 4; col++)
            {
                result = board.InsertChip(-1, col, 1);
            }
            Assert.AreEqual(4, result.ConnectionAscendingDiagonal.Count);
        }

        [TestCaseSource(nameof(FullBoardTestSet))]
        public void AllDirectionsConnectedTest(IBoard input)
        {
            board = input;
            IMove result = board.InsertChip(2, 3, 1);

            Assert.AreEqual(7, result.ConnectionHorizontal.Count);
            Assert.AreEqual(4, result.ConnectionVertical.Count);
            Assert.AreEqual(6, result.ConnectionDescendingDiagonal.Count);
            Assert.AreEqual(6, result.ConnectionAscendingDiagonal.Count);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsConnectedTest(int column)
        {
            board.InsertChip(-1, column, 1);
            board.InsertChip(-1, column, 1);
            board.InsertChip(-1, column, 1);
            IMove result = board.InsertChip(-1, column, 1);

            Assert.IsTrue(result.IsConnected);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsNotConnectedTest(int column)
        {
            board.InsertChip(-1, column, 1);
            board.InsertChip(-1, column, 1);
            IMove result = board.InsertChip(-1, column, 1);

            Assert.IsFalse(result.IsConnected);
        }

        [Test]
        public void RemoveChipTest()
        {
            board = CreateBoard(new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {1, 0, 0, 0, 0, 0, 0}
            });

            board.RemoveChip(5, 0);

            Assert.IsTrue(board.IsMoveValid(5, 0, 1));
        }

        // Source
        // ------------------------------------------------
        #region Source

        private static Dictionary<int, IBoard> CreateBoardWithInteger(int start, int[,] ids)
        {
            var boardDct = new Dictionary<int, IBoard>();
            boardDct.Add(start, CreateBoard(ids));
            return boardDct;
        }

        static readonly object[] AscendingBoardTestSet = {
            CreateBoardWithInteger(0, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 2, 0, 0, 0},
                {0, 0, 2, 2, 0, 0, 0},
                {0, 2, 2, 2, 0, 0, 0}
            }),
            CreateBoardWithInteger(1, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 2, 0, 0},
                {0, 0, 0, 1, 2, 0, 0},
                {0, 0, 2, 2, 1, 0, 0},
                {0, 2, 2, 2, 1, 0, 0}
            }),
            CreateBoardWithInteger(2, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 2, 0},
                {0, 0, 0, 0, 1, 1, 0},
                {0, 0, 0, 2, 2, 2, 0},
                {0, 0, 2, 2, 1, 2, 0},
                {0, 0, 2, 1, 1, 2, 0}
            }),
            CreateBoardWithInteger(3, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 2},
                {0, 0, 0, 0, 0, 2, 2},
                {0, 0, 0, 0, 2, 1, 2},
                {0, 0, 0, 1, 2, 1, 1},
                {0, 0, 0, 2, 2, 1, 1}
            })
        };

        static readonly object[] DescendingBoardTestSet = {
            CreateBoardWithInteger(0, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {2, 0, 0, 0, 0, 0, 0},
                {2, 2, 0, 0, 0, 0, 0},
                {2, 2, 2, 0, 0, 0, 0}
            }),
            CreateBoardWithInteger(1, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 1, 0, 0, 0, 0, 0},
                {0, 1, 2, 0, 0, 0, 0},
                {0, 2, 1, 1, 0, 0, 0},
                {0, 2, 2, 2, 1, 0, 0}
            }),
            CreateBoardWithInteger(2, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0, 0},
                {0, 0, 1, 2, 0, 0, 0},
                {0, 0, 2, 1, 1, 0, 0},
                {0, 0, 2, 1, 2, 1, 0},
                {0, 0, 2, 1, 1, 2, 0}
            }),
            CreateBoardWithInteger(3, new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 2, 0, 0, 0},
                {0, 0, 0, 2, 2, 0, 0},
                {0, 0, 0, 2, 2, 2, 0}
            })
        };

        static readonly object[] FullBoardTestSet =
        {
            CreateBoard(new int[,]
            {
                {2, 1, 2, 0, 2, 1, 2}, //2 - 4
                {2, 2, 1, 0, 1, 2, 2}, //2 - 4
                {1, 1, 1, 0, 1, 1, 1}, //6 - 0
                {2, 2, 1, 1, 1, 2, 2}, //3 - 4
                {2, 1, 2, 1, 2, 1, 2}, //3 - 4
                {1, 2, 2, 1, 2, 2, 1}  //3 - 4
            })
        };

        #endregion
    }
}
