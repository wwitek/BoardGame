﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGame.Domain.Entities;
using BoardGame.Domain.Factories;
using BoardGame.Domain.Interfaces;
using NUnit.Framework;

namespace BoardGame.Tests
{
    [TestFixture]
    class BoardTests
    {
        [Test]
        public void CreationTest()
        {
            var fieldFactory = new FieldFactory();
            IBoard board = new Board(7, 6, fieldFactory);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void AddFirstChipTest(int column)
        {
            var fieldFactory = new FieldFactory();
            IBoard board = new Board(7, 6, fieldFactory);

            IMove result = board.InsertChip(0, column, 1);

            Assert.AreEqual(1, result.ConnectionHorizontal.Count);
            Assert.AreEqual(1, result.ConnectionVertical.Count);
            Assert.AreEqual(1, result.ConnectionDescendingDiagonal.Count);
            Assert.AreEqual(1, result.ConnectionAscendingDiagonal.Count);
        }
    }
}