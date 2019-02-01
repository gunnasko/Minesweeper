﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Minesweeper.Tests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void BoardCreatesHasCreatedCorrectNumberOfMines()
        {
            int numOfColumns = 8;
            int numOfRows = 8;
            int numOfMines = 10;
            var board = new Board(numOfColumns, numOfRows, numOfMines);

            int mineCounter = CountNumberOfMines(numOfColumns, numOfRows, board);
            Assert.AreEqual(numOfMines, mineCounter);
        }
        private static int CountNumberOfMines(int numOfColumns, int numOfRows, Board board)
        {
            int mineCounter = 0;
            for (int x = 0; x < numOfRows; x++)
            {
                for (int y = 0; y < numOfColumns; y++)
                {
                    var coordinate = new Coordinate(newX: x, newY: y);
                    if (board.PointHasMine(coordinate))
                    {
                        mineCounter++;
                    }
                }
            }

            return mineCounter;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BoardMustHaveMines()
        {
            int numOfColumns = 8;
            int numOfRows = 8;
            int numOfMines = 0;
            var board = new Board(numOfColumns, numOfRows, numOfMines);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BoardColumnsAndRowsMustBeAtleastTwo()
        {
            int numOfColumns = 1;
            int numOfRows = 1;
            int numOfMines = 1;
            var board = new Board(numOfColumns, numOfRows, numOfMines);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BoardMinesCannotBeMoreThanCreatedPoints()
        {
            int numOfColumns = 4;
            int numOfRows = 4;
            int numOfMines = 17;
            //4*4 = 16 < 17, throw exception
            var board = new Board(numOfColumns, numOfRows, numOfMines);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotReadMineOutsideBoardRange()
        {
            int numOfColumns = 8;
            int numOfRows = 8;
            int numOfMines = 10;
            var board = new Board(numOfColumns, numOfRows, numOfMines);

            var outOfBoundCoordinate = new Coordinate(numOfColumns + 1, numOfRows + 1);
            board.PointHasMine(outOfBoundCoordinate);
        }

        [TestMethod]
        public void OpenedAPointWithoutAMineReturnsAdjancentMines()
        {
            Board testBoard = BoardUtils.CreateCheckerboardTestBoard(4,4);

            //Top row, expect three adjacent mines
            BoardActionResult result = testBoard.OpenPoint(0, 1);
            Assert.AreEqual(3, result.AdjacentMines);
            Assert.AreEqual(false, result.SteppedOnMine);

            //Top right corner, expect 2 adjacent mines
            result = testBoard.OpenPoint(0, 3);
            Assert.AreEqual(2, result.AdjacentMines);
            Assert.AreEqual(false, result.SteppedOnMine);

            //Middle, expect 4 adjacent mines
            result = testBoard.OpenPoint(1, 2);
            Assert.AreEqual(4, result.AdjacentMines);
            Assert.AreEqual(false, result.SteppedOnMine);
        }

        [TestMethod]
        public void OpeningAPointWithNoAdjacentMinesAutoOpensAllNeighboursWithNoAdjacentMines()
        {
            Board board = BoardUtils.CreateSingleMineTopRightBoard(4, 4);
            Assert.AreEqual(true, board.PointHasMine(0, 3));

            //Open a point that has no adjacent mines
            board.OpenPoint(1, 1);
            Assert.AreEqual(true, board.PointIsOpen(1, 1));

            //Expect the points twowards topright to not be open because they have adjacent mines
            Assert.AreEqual(false, board.PointIsOpen(1, 2));
            Assert.AreEqual(false, board.PointIsOpen(0, 2));
            Assert.AreEqual(false, board.PointIsOpen(1, 3));

            //Sample a bunch of other points that are not adjacent to top right mine.
            Assert.AreEqual(true, board.PointIsOpen(0, 0));
            Assert.AreEqual(true, board.PointIsOpen(1, 0));
            Assert.AreEqual(true, board.PointIsOpen(3, 0));
            Assert.AreEqual(true, board.PointIsOpen(3, 3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotOpenAnAlreadyOpenedPoint()
        {
            Board board = BoardUtils.CreateSingleMineTopRightBoard(4, 4);

            board.OpenPoint(1, 1);
            board.OpenPoint(1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotOpenAPointOutsideOfTheBoard()
        {
            Board board = BoardUtils.CreateSingleMineTopRightBoard(4, 4);

            board.OpenPoint(4, 4);
        }

        [TestMethod]
        public void CanFlagAPointOnBoard()
        {
            int numOfColumns = 8;
            int numOfRows = 8;
            int numOfMines = 10;
            var board = new Board(numOfColumns, numOfRows, numOfMines);

            board.FlagPoint(1, 1, true);
            Assert.AreEqual(true, board.PointIsFlagged(1, 1));

            board.FlagPoint(1, 1, false);
            Assert.AreEqual(false, board.PointIsFlagged(1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotFlagCoordinateNotOnBoard()
        {
            int numOfColumns = 8;
            int numOfRows = 8;
            int numOfMines = 10;
            var board = new Board(numOfColumns, numOfRows, numOfMines);

            board.FlagPoint(8, 8, true);
        }







    }
}