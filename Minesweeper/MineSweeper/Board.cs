using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Board
    {
        private Dictionary<Coordinate, Point> points;
        private int numOfColumns;
        private int numOfRows;
        private int numOfMines;
        private int addedMines = 0;
        Random randomGenerator;

        public Board(int boardColumnSize, int boardRowSize, int numOfMinesInGames)
        {
            points = new Dictionary<Coordinate, Point>();
            numOfColumns = boardColumnSize;
            numOfRows = boardRowSize;
            numOfMines = numOfMinesInGames;

            randomGenerator = new Random();
            FillPoints();
            AddMines();
        }

        public Board() : this(8, 8, 10)
        {
        }

        private void FillPoints()
        {
            for (int x = 0; x < numOfColumns; x++)
            {
                for (int y = 0; y < numOfRows; y++)
                {
                    var coordinate = new Coordinate(newX: x, newY: y);
                    var point = new Point(coordinate);
                    points[coordinate] = (point);
                }
            }
        }

        private void AddMines()
        {
            addedMines = 0;

            while (addedMines < numOfMines)
            {
                int randomX = randomGenerator.Next(numOfColumns);
                int randomy = randomGenerator.Next(numOfRows);

                var minedCoordinate = new Coordinate(randomX, randomy);
                Point pointCandidate = points[minedCoordinate];
                
                if (pointCandidate.HasMine == false)
                {
                    pointCandidate.HasMine = true;
                    addedMines++;
                }
            }
        }

        public bool PointHasMine(Coordinate coordinate)
        {
            try
            {
                var point = points[coordinate];
                return point.HasMine;
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentOutOfRangeException($"Coordinate given does not exist on board!");
            }
        }
    }
}
