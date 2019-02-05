using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public struct HighScoreEntry
    {
        public string Name { get; set; }
        public int Score { get; set; }
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

        public List<HighScoreEntry> GetTopTen(GameDifficulty highScoreDifficulty)
        {
            var query = from h in _highScores[highScoreDifficulty] orderby h.Score select h;
            return query.Take(10).ToList();
        }

    }
}
