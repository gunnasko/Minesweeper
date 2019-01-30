using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Board
    {
        private List<Point> points;
        private int numOfColumns;
        private int numOfRows;
        private int numOfMines;

        public Board(int boardColumnSize, int boardRowSize, int numOfMinesInGames)
        {
            numOfColumns = boardColumnSize;
            numOfRows = boardRowSize;
            numOfMines = numOfMinesInGames;
        }

        public Board() : this(8, 8, 10)
        {
        }

        public void FillPoints()
        {

        }
    }
}
