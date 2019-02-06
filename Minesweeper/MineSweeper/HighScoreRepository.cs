﻿using Minesweeper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class HighScoreRepository : IHighScoreRepository
    {
        const string HIGH_SCORE_PATH = "highscores.xml";
        public HighScores Load()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                using (IsolatedStorageFileStream file = isoStore.OpenFile(HIGH_SCORE_PATH, System.IO.FileMode.Open))
                {
                    return LoadFromStream(file);
                }
            }
        }

        public void Save(HighScores highscores)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                using (IsolatedStorageFileStream file = isoStore.OpenFile(HIGH_SCORE_PATH, System.IO.FileMode.Create))
                {
                    SaveToStream(highscores ,file);
                }
            }
        }

        public HighScores LoadFromStream(Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(HighScores));
            return (HighScores)serializer.ReadObject(stream);
        }

        public void SaveToStream(HighScores highscores, Stream stream)
        {
            var serializer = new DataContractSerializer(typeof(HighScores));
            serializer.WriteObject(stream, highscores);
        }
    }
}
