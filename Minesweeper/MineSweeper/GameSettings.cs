using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace Minesweeper
{
    public enum GameDifficulty
    {
        Beginner,
        Intermediate,
        Expert,
        Custom
    }

    public struct BoardSettings
    {
        public int BoardNumberOfColumns { get; set; }
        public int BoardNumberOfRows { get; set; }
        public int BoardNumberOfMines { get; set; }

        public BoardSettings(int columns, int rows, int mines)
        {
            BoardNumberOfColumns = columns;
            BoardNumberOfRows = rows;
            BoardNumberOfMines = mines;
        }
    }

    [DataContract]
    public class GameSettings
    {
        [DataMember]
        public int CustomBoardNumberOfColumns { get; set; }
        [DataMember]
        public int CustomBoardNumberOfRows { get; set; }
        [DataMember]
        public int CustomBoardNumberOfMines { get; set; }
        [DataMember]
        public GameDifficulty BoardGameDifficulty { get; set; }

        public GameSettings(int columns, int rows, int mines, GameDifficulty gameDifficulty)
        {
            CustomBoardNumberOfColumns = columns;
            CustomBoardNumberOfRows = rows;
            CustomBoardNumberOfMines = mines;
            BoardGameDifficulty = gameDifficulty;
        }

        public BoardSettings CreateBoardSettings()
        {
            switch (BoardGameDifficulty)
            {
                case GameDifficulty.Beginner:
                    return new BoardSettings(9, 9, 10);
                case GameDifficulty.Intermediate:
                    return new BoardSettings(16, 16, 40);
                case GameDifficulty.Expert:
                    return new BoardSettings(16, 30, 99);
                case GameDifficulty.Custom:
                    return new BoardSettings(CustomBoardNumberOfColumns, CustomBoardNumberOfRows, CustomBoardNumberOfMines);
            }
            return new BoardSettings(9, 9, 10);
        }

        public GameSettings() : this(9, 9, 10, GameDifficulty.Beginner)
        {

        }
    }

    public class GameSettingsUtils
    {
        const string GAME_SETTINGS_FILE_NAME = "settings.xml";
   
        public static void Save(GameSettings settingsToSave)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                using (IsolatedStorageFileStream file = isoStore.OpenFile(GAME_SETTINGS_FILE_NAME, System.IO.FileMode.Create))
                {
                    var serializer = new DataContractSerializer(typeof(GameSettings));
                    serializer.WriteObject(file, settingsToSave);
                }
            }
        }

        public static GameSettings Load()
        {
            GameSettings defaultSettings = new GameSettings();
            GameSettings loadedSettings = defaultSettings;

            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    using (IsolatedStorageFileStream file = isoStore.OpenFile(GAME_SETTINGS_FILE_NAME, System.IO.FileMode.Open))
                    {
                        var serializer = new DataContractSerializer(typeof(GameSettings));
                        loadedSettings = (GameSettings)serializer.ReadObject(file);
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    //Continue with default settings
                    loadedSettings = defaultSettings;
                }
                catch (SerializationException)
                {
                    loadedSettings = defaultSettings;
                }
            }

            return loadedSettings;
        }

    }
}
