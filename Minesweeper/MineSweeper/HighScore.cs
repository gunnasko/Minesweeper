using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    [DataContract]
    public struct HighScoreEntry
    {
        public int Placement { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }

    [DataContract]
    public class HighScores
    {
        [DataMember]
        private Dictionary<GameDifficulty, List<HighScoreEntry>> _highScores;

        public HighScores()
        {
            _highScores = new Dictionary<GameDifficulty, List<HighScoreEntry>>();

            _highScores[GameDifficulty.Beginner] = new List<HighScoreEntry>();
            _highScores[GameDifficulty.Intermediate] = new List<HighScoreEntry>();
            _highScores[GameDifficulty.Expert] = new List<HighScoreEntry>();
        }

        public void AddHighScore(HighScoreEntry entry, GameDifficulty scoreDifficulty)
        {
            try
            {
                _highScores[scoreDifficulty].Add(entry);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Game difficulty cannot be custom!");
            }
        }

        public bool IsScoreInTopTen(int scoreCandidate, GameDifficulty scoreDifficulty)
        {
            try
            {
                var topTen = GetTopTen(scoreDifficulty);
                return scoreCandidate <= topTen.Last().Score;
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Game difficulty cannot be custom!");
            }
        }

        public List<HighScoreEntry> GetTopTen(GameDifficulty highScoreDifficulty)
        {
            var query = from h in _highScores[highScoreDifficulty] orderby h.Score select h;
            HighScoreEntry [] topTenList = query.Take(10).ToArray();
            //Sett placement as one indexed
            for (int i = 0; i < topTenList.Length; i++)
            {
                topTenList[i].Placement = i + 1;
            }
            return new List<HighScoreEntry>(topTenList);
        }

    }
}
