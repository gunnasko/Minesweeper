using System;
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
        public void BoardThrowsExceptionIfTryingToReadMineOnUnknownCoordinate()
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
            Board testBoard = CreateCheckerboardTestBoard(4,4);

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
            Board board = CreateSingleMineTopRightBoard(4, 4);
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

        private Board CreateSingleMineTopRightBoard(int numOfRows, int numOfColumns)
        {
            var mockPoints = new Dictionary<Coordinate, Point>();

            for (int x = 0; x < numOfRows; x++)
            {
                for (int y = 0; y < numOfColumns; y++)
                {
                    var coordinate = new Coordinate(newX: x, newY: y);
                    var point = new Point(coordinate);
                    mockPoints[coordinate] = point;
                }
                //Switch between each row for checkerboard effect
            }
            var topRightCoord = new Coordinate(newX: 0, newY: numOfColumns - 1);
            mockPoints[topRightCoord].HasMine = true;
            return new Board(mockPoints);
        }


        /*****
         * Creates checkerboard mined board:
         * |X| |X| |
         * | |X| |X|
         * |X| |X| |
         *****/
        private static Board CreateCheckerboardTestBoard(int numOfRows, int numOfColumns)
        {
            var mockPoints = new Dictionary<Coordinate, Point>();

            bool mineSwitcher = true;

            for (int x = 0; x < numOfRows; x++)
            {
                for (int y = 0; y < numOfColumns; y++)
                {
                    var coordinate = new Coordinate(newX: x, newY: y);
                    var point = new Point(coordinate);
                    if (mineSwitcher)
                    {
                        point.HasMine = true;
                    }
                    mineSwitcher = !mineSwitcher;
                    mockPoints[coordinate] = point;
                }
                //Switch between each row for checkerboard effect
                mineSwitcher = !mineSwitcher;
            }
            return new Board(mockPoints);
        }
    }
}
