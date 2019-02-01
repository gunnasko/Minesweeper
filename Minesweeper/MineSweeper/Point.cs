using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct Coordinate : IEqualityComparer<Coordinate>
    {
        public int x;
        public int y;
        public Coordinate(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public bool Equals(Coordinate cordA, Coordinate cordB)
        {
            return (cordA.x == cordB.x && cordA.y == cordB.y);
        }

        public int GetHashCode(Coordinate obj)
        {
            return obj.x.GetHashCode() + obj.y.GetHashCode();
        }
    }
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
