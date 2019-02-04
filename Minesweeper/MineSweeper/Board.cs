using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct BoardActionResult
    {
        public bool SteppedOnMine { get; set; }
    }


    public class Board
    {
        private Dictionary<Coordinate, Point> points;
        private int addedMines = 0;

        public int ColumnSize { get; }
        public int RowSize { get; }
        public int NumberOfMines { get; }

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
            ColumnSize = boardColumnSize;
            RowSize = boardRowSize;
            NumberOfMines = numOfMinesInGames;

            randomGenerator = new Random();
            FillPoints();
            AddMines();
        }

        public Board() : this(8, 8, 10)
        {
        }

        //Used for testing
        public Board(int boardColumnSize, int boardRowSize, Dictionary<Coordinate, Point> mockedDict)
        {
            ColumnSize = boardColumnSize;
            RowSize = boardRowSize;

            points = mockedDict;
        }

        private void FillPoints()
        {
            for (int x = 0; x < RowSize; x++)
            {
                for (int y = 0; y < ColumnSize; y++)
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

            while (addedMines < NumberOfMines)
            {
                int randomX = randomGenerator.Next(ColumnSize);
                int randomy = randomGenerator.Next(RowSize);

                var minedCoordinate = new Coordinate(randomX, randomy);
                Point pointCandidate = points[minedCoordinate];

                if (pointCandidate.HasMine == false)
                {
                    pointCandidate.HasMine = true;
                    addedMines++;
                }
            }
        }

        public Point AccessPoint(int x, int y)
        {
            return AccessPoint(new Coordinate(newX: x, newY: y));
        }

        public Point AccessPoint(Coordinate coordinate)
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
                return new BoardActionResult
                {
                    SteppedOnMine = false
                };
            }

            point.AdjacentMines = CalculateAdjacentMines(point.PointCoordinate);

            point.IsOpened = true;

            if (!point.HasMine)
            {

                if (point.AdjacentMines == 0)
                {
                    //Warning, recursion!
                    OpenNeighbourPoints(point);
                }
            }

            return new BoardActionResult
            {
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
                    Point neighbourPoint = AccessPoint(neighbourCoord);
                    if (!neighbourPoint.IsOpened)
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

        private void AddCoordinateIfValid(int pointX, int pointY, List<Coordinate> coordinateList)
        {
            if (pointX >= RowSize || pointY >= ColumnSize
                || pointX < 0 || pointY < 0)
            {
                return;
            }
            coordinateList.Add(new Coordinate(pointX, pointY));
        }


        private List<Coordinate> GetNeighborCoordinates(Coordinate sentralCoordinate)
        {
            int pointX = sentralCoordinate.x;
            int pointY = sentralCoordinate.y;

            var neighbourCoordinates = new List<Coordinate>();

            AddCoordinateIfValid(pointX + 1, pointY, neighbourCoordinates);
            AddCoordinateIfValid(pointX, pointY + 1, neighbourCoordinates);
            AddCoordinateIfValid(pointX + 1, pointY + 1, neighbourCoordinates);

            AddCoordinateIfValid(pointX - 1, pointY, neighbourCoordinates);
            AddCoordinateIfValid(pointX, pointY - 1, neighbourCoordinates);
            AddCoordinateIfValid(pointX - 1, pointY - 1, neighbourCoordinates);

            AddCoordinateIfValid(pointX + 1, pointY - 1, neighbourCoordinates);
            AddCoordinateIfValid(pointX - 1, pointY + 1, neighbourCoordinates);

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
            var centerPoint = AccessPoint(sentralCoordinate);
            return adjacentMineCounter;
        }
    }
}
