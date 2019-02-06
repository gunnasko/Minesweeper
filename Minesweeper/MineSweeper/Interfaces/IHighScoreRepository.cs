using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Interfaces
{
    public interface IHighScoreRepository
    {
        void Save(HighScores highscores);
        HighScores Load();
    }
}
