using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Point
    {
        public bool HasMine { get; set; }
        public bool IsFlagged { get; set; }
        public Coordinate PointCoordinate { get; }
        public bool IsOpened { get; set; }

        public Point(Coordinate coordinate)
        {
            PointCoordinate = coordinate;
            IsOpened = false;
        }

    }
}
