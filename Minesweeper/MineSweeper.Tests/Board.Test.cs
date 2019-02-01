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
            for (int i = 0; i < numOfColumns; i++)
            {
                for (int j = 0; j < numOfRows; j++)
                {
                    var coordinate = new Coordinate(i, j);
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
        public void BoardThrowsExceptionIfTryingToAccessUnknownCoordinate()
        {
            int numOfColumns = 8;
            int numOfRows = 8;
            int numOfMines = 10;
            var board = new Board(numOfColumns, numOfRows, numOfMines);

            var outOfBoundCoordinate = new Coordinate(numOfColumns + 1, numOfRows + 1);
            board.PointHasMine(outOfBoundCoordinate);
        }
    }
}
