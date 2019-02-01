using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct Coordinate : IComparable<Coordinate>
    {
        public int x;
        public int y;
        public Coordinate(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public int CompareTo(Coordinate other)
        {
            if (this.x == other.x && this.y == other.y)
                return 0;
            else
                return -1;
        }
    }
    public class Point
    {
        public bool HasMine { get; }
        public bool IsFlagged { get; set; }
        public Coordinate PointCoordinate { get; }
        public bool IsOpenend { get; set; }
        public int NumOfAdjacentMines { get; set; }

        public Point(bool hasMine, Coordinate coordinate)
        {
            PointCoordinate = coordinate;
            HasMine = hasMine;
        }
    }
}
