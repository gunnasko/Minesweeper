using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public enum GameDifficulty
    {
        Beginner,
        Intermediate,
        Expert,
        Custom
    }

    public struct GameSettings
    {
        public int BoardNumberOfColumns { get; set; }
        public int BoardNumberOfRows { get; set; }
        public int BoardNumberOfMines { get; set; }

        public GameSettings(int columns, int rows, int mines)
        {
            BoardNumberOfColumns = columns;
            BoardNumberOfRows = rows;
            BoardNumberOfMines = mines;
        }
    }

    public class GameSettingsUtils
    {
        public static GameSettings GetGameSettingsFromDifficulty(GameDifficulty settings, int customColumn = 9, int customRows = 9, int customMines = 10)
        {
            switch (settings)
            {
                case GameDifficulty.Beginner:
                    return new GameSettings(9, 9, 10);
                case GameDifficulty.Intermediate:
                    return new GameSettings(16, 16, 40);
                case GameDifficulty.Expert:
                    return new GameSettings(16, 30, 99);
                case GameDifficulty.Custom:
                    return new GameSettings(customColumn, customRows, customMines);
            }
            return new GameSettings(customColumn, customRows, customMines);
        }
    }
}
