using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Minesweeper
{
    public enum GameDifficulty
    {
        Beginner,
        Intermediate,
        Expert,
        Custom
    }

    [DataContract]
    public struct GameSettings
    {
        [DataMember]
        public int BoardNumberOfColumns { get; set; }
        [DataMember]
        public int BoardNumberOfRows { get; set; }
        [DataMember]
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
        const string GAME_SETTINGS_FILE_NAME = "settings.xml";
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

        public static void Save(GameSettings settingsToSave)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                using (IsolatedStorageFileStream file = isoStore.OpenFile(GAME_SETTINGS_FILE_NAME, System.IO.FileMode.OpenOrCreate))
                {
                    var serializer = new DataContractSerializer(typeof(GameSettings));
                    serializer.WriteObject(file, settingsToSave);
                }
            }
        }
    }
}
