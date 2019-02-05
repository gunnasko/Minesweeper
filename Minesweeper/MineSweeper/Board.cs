using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct BoardActionResult
    {
        public bool SteppedOnMine { get; set; }
    }

    public class Board : INotifyPropertyChanged
    {
        private Dictionary<Coordinate, Point> _points;
        private int _addedMines = 0;

        public int ColumnSize { get; } //Y
        public int RowSize { get; } //X
        public int NumberOfMines { get; }
        public int NumberOfFlagsLeft { get; private set; } = 0;
        Random _randomGenerator;

        public event PropertyChangedEventHandler PropertyChanged;

        public Board(int boardColumnSize, int boardRowSize, int numOfMinesInGames, Dictionary<Coordinate, Point> points)
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

            ColumnSize = boardColumnSize;
            RowSize = boardRowSize;
            NumberOfMines = numOfMinesInGames;
            NumberOfFlagsLeft = NumberOfMines;
            _points = points;
        }

        public Board(int boardColumnSize, int boardRowSize, int numOfMinesInGames)
            : this(boardColumnSize, boardRowSize, numOfMinesInGames, new Dictionary<Coordinate, Point>())
        {
            _randomGenerator = new Random();
            FillPoints();
            AddMines();
        }

        public Board() : this(8, 8, 10)
        {
        }

        public Board(BoardSettings boardSettings) :this(
            boardSettings.BoardNumberOfColumns,
            boardSettings.BoardNumberOfRows,
            boardSettings.BoardNumberOfMines)
        {
        }

        private void FillPoints()
        {
            for (int x = 0; x < RowSize; x++)
            {
                for (int y = 0; y < ColumnSize; y++)
                {
                    var coordinate = new Coordinate(newX: x, newY: y);
                    var point = new Point(coordinate);
                    _points[coordinate] = (point);
                }
            }
        }

        private void AddMines()
        {
            _addedMines = 0;

            while (_addedMines < NumberOfMines)
            {
                int randomX = _randomGenerator.Next(RowSize);
                int randomY = _randomGenerator.Next(ColumnSize);

                var minedCoordinate = new Coordinate(randomX, randomY);
                Point pointCandidate = _points[minedCoordinate];

                if (pointCandidate.HasMine == false)
                {
                    pointCandidate.HasMine = true;
                    _addedMines++;
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
                return _points[coordinate];
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
            if (AccessPoint(coordinate).IsFlagged == setFlag)
            {
                return;
            }

            if (setFlag)
            {
                NumberOfFlagsLeft--;
            }
            else
            {
                if (NumberOfFlagsLeft == NumberOfMines)
                {
                    //This exception is so rare I cannot create a test that exposes this.
                    throw new InvalidOperationException("Cannot add more flags then mines!");
                }
                NumberOfFlagsLeft++;
            }

            OnPropertyChanged("NumberOfFlagsLeft");

            AccessPoint(coordinate).IsFlagged = setFlag;
        }

        public BoardActionResult OpenPoint(int x, int y)
        {
            return OpenPoint(new Coordinate(newX: x, newY: y));
        }

        public bool HasWonGame()
        {
            bool hasOpenedMine = (from p in _points where (p.Value.IsOpened == true && p.Value.HasMine == true) select p).Count() > 0;
            if (hasOpenedMine)
            {
                return false;
            }

            int numOfUnopenedPoints = (from p in _points where p.Value.IsOpened == false select p).Count();
            return (numOfUnopenedPoints <= NumberOfMines);
        }

        private void ShowAllMines()
        {
            var query = from p in _points where p.Value.HasMine == true select p;
            foreach (var val in query)
            {
                Point point = val.Value;
                point.IsOpened = true;
            }
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
            //Lost the game, show all mines!
            else
            {
                ShowAllMines();
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
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
