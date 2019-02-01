using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Tests
{
    public class BoardUtils
    {
        public static Board CreateSingleMineTopRightBoard(int numOfRows, int numOfColumns)
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
            return new Board(numOfColumns ,numOfRows, mockPoints);
        }


        /*****
         * Creates checkerboard mined board:
         * |X| |X| |
         * | |X| |X|
         * |X| |X| |
         *****/
        public static Board CreateCheckerboardTestBoard(int numOfRows, int numOfColumns)
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
            return new Board(numOfColumns, numOfRows, mockPoints);
        }
    }
}
