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

    [DataContract]
    public struct GameSettings
    {
        [DataMember]
        public int BoardNumberOfColumns { get; set; }
        [DataMember]
        public int BoardNumberOfRows { get; set; }
        [DataMember]
        public int BoardNumberOfMines { get; set; }
        [DataMember]
        public GameDifficulty BoardGameDifficulty { get; set; }

        public GameSettings(int columns, int rows, int mines, GameDifficulty gameDifficulty)
        {
            BoardNumberOfColumns = columns;
            BoardNumberOfRows = rows;
            BoardNumberOfMines = mines;
            BoardGameDifficulty = gameDifficulty;
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
                    return new GameSettings(9, 9, 10, settings);
                case GameDifficulty.Intermediate:
                    return new GameSettings(16, 16, 40, settings);
                case GameDifficulty.Expert:
                    return new GameSettings(16, 30, 99, settings);
                case GameDifficulty.Custom:
                    return new GameSettings(customColumn, customRows, customMines, settings);
            }
            return new GameSettings(customColumn, customRows, customMines, settings);
        }

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
            GameSettings defaultSettings = GetGameSettingsFromDifficulty(GameDifficulty.Beginner);
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
