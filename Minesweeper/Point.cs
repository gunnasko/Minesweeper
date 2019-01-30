using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct Coordinate
    {
        public int x;
        public int y;
    }
    public class Point
    {
        public bool HasMine { get; }
        public bool IsFlagged { get; set; }
        Coordinate PointCoordinate { get; }
        public bool IsOpenend { get; set; }
        public int NumOfAdjacentMines { get; set; }

        public Point(bool hasMine, Coordinate coordinate)
        {
            PointCoordinate = coordinate;
            HasMine = hasMine;
        }
    }
}
