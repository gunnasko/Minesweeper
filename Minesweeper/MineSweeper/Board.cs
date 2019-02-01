using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct BoardActionResult
    {
        public int AdjacentMines { get; set; }
        public bool SteppedOnMine { get; set; }
    }


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
            if (numOfMinesInGames <= 0)
            {
                throw new ArgumentOutOfRangeException("Board must have atleast one mine");
            }

            if (boardColumnSize < 2 || boardRowSize < 2)
            {
                throw new ArgumentOutOfRangeException("Board size must be atleast two in columns and rows");
            }

            if (numOfMinesInGames > boardColumnSize * boardRowSize)
            {
                throw new ArgumentOutOfRangeException("Number of mines cannot be larger than possible number of points");
            }

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

        //Used for testing
        public Board(Dictionary<Coordinate, Point> mockedDict)
        {
            points = mockedDict;
        }

        private void FillPoints()
        {
            for (int x = 0; x < numOfRows; x++)
            {
                for (int y = 0; y < numOfColumns; y++)
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

        private Point AccessPoint(Coordinate coordinate)
        {
            try
            {
                return points[coordinate];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentOutOfRangeException($"Coordinate given does not exist on board!");
            }
        }

        public bool PointHasMine(int x, int y)
        {
            return PointHasMine(new Coordinate(newX: x, newY: y));
        }

        public bool PointHasMine(Coordinate coordinate)
        {
            return AccessPoint(coordinate).HasMine;
        }

        public bool PointIsOpen(int x, int y)
        {
            return PointIsOpen(new Coordinate(newX: x, newY: y));
        }

        public bool PointIsOpen(Coordinate coordinate)
        {
            return AccessPoint(coordinate).IsOpened;
        }

        public bool PointIsFlagged(int x, int y)
        {
            return PointIsFlagged(new Coordinate(newX: x, newY: y));
        }

        public bool PointIsFlagged(Coordinate coordinate)
        {
            return AccessPoint(coordinate).IsFlagged;
        }

        public void FlagPoint(int x, int y, bool setFlag)
        {
            FlagPoint(new Coordinate(newX: x, newY: y), setFlag);
        }

        public void FlagPoint(Coordinate coordinate, bool setFlag)
        {
            AccessPoint(coordinate).IsFlagged = setFlag;
        }

        public BoardActionResult OpenPoint(int x, int y)
        {
            return OpenPoint(new Coordinate(newX: x, newY: y));
        }


        public BoardActionResult OpenPoint(Coordinate coordinate)
        {
            Point point = AccessPoint(coordinate);

            if (point.IsOpened)
            {
                throw new ArgumentException("Cannot open point that is already opened!");
            }

            point.IsOpened = true;

            int adjacentMines = 0;
            if (!point.HasMine)
            {
                adjacentMines = CalculateAdjacentMines(point.PointCoordinate);
                if (adjacentMines == 0)
                {
                    //Warning, recursion!
                    OpenNeighbourPoints(point);
                }
            }

            return new BoardActionResult
            {
                AdjacentMines = adjacentMines,
                SteppedOnMine = point.HasMine
            };
        }

        private void OpenNeighbourPoints(Point point)
        {
            List<Coordinate> neighborCoordinates = GetNeighborCoordinates(point.PointCoordinate);
            foreach (var neighbourCoord in neighborCoordinates)
            {
                try
                {
                    int adjacentMines = CalculateAdjacentMines(neighbourCoord);
                    if (adjacentMines == 0)
                    {
                        OpenPoint(neighbourCoord);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    //Do nothing.
                }
                catch (ArgumentException)
                {
                    //Do nothing, thrown if already opened.
                }
            }
        }

        private List<Coordinate> GetNeighborCoordinates(Coordinate sentralCoordinate)
        {
            int pointX = sentralCoordinate.x;
            int pointY = sentralCoordinate.y;

            var neighbourCoordinates = new List<Coordinate>();

            neighbourCoordinates.Add(new Coordinate(pointX + 1, pointY));
            neighbourCoordinates.Add(new Coordinate(pointX, pointY + 1));
            neighbourCoordinates.Add(new Coordinate(pointX + 1, pointY + 1));

            neighbourCoordinates.Add(new Coordinate(pointX - 1, pointY));
            neighbourCoordinates.Add(new Coordinate(pointX, pointY - 1));
            neighbourCoordinates.Add(new Coordinate(pointX - 1, pointY - 1));

            neighbourCoordinates.Add(new Coordinate(pointX + 1, pointY - 1));
            neighbourCoordinates.Add(new Coordinate(pointX - 1, pointY + 1));

            return neighbourCoordinates;
        }

        private int CalculateAdjacentMines(Coordinate sentralCoordinate)
        {
            int adjacentMineCounter = 0;
            var candidatePoints = GetNeighborCoordinates(sentralCoordinate);

            foreach (var neighbourCoord in candidatePoints)
            {
                try
                {
                    Point neighbourPoint = AccessPoint(neighbourCoord);
                    if (neighbourPoint.HasMine)
                    {
                        adjacentMineCounter++;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    //Do nothing. We just do not want to update mine counter.
                }
            }

            return adjacentMineCounter;
        }
    }
}
