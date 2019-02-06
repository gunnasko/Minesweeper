using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Minesweeper.Tests
{
    [TestClass]
    public class HighScoreRepositoryTests
    {
        [TestMethod]
        public void HighScoresCanBeSerializedBackAndForth()
        {
            var testHighscores = new HighScores();
            testHighscores.AddHighScore(new HighScoreEntry
            {
                Name = "BeginnerScore",
                Score = 5
            }, GameDifficulty.Beginner);

            testHighscores.AddHighScore(new HighScoreEntry
            {
                Name = "IntermediateScore",
                Score = 10
            }, GameDifficulty.Intermediate);

            testHighscores.AddHighScore(new HighScoreEntry
            {
                Name = "ExpertScore",
                Score = 15
            }, GameDifficulty.Expert);


            var repository = new HighScoreRepository();
            var ms = new MemoryStream();
            repository.SaveToStream(testHighscores, ms);

            ms.Position = 0;
            var loadedHighScores = repository.LoadFromStream(ms);

            VerifyResults(testHighscores, loadedHighScores, GameDifficulty.Beginner);
            VerifyResults(testHighscores, loadedHighScores, GameDifficulty.Intermediate);
            VerifyResults(testHighscores, loadedHighScores, GameDifficulty.Expert);
        }

        private static void VerifyResults(HighScores testHighscores, HighScores loadedHighScores, GameDifficulty difficulty)
        {
            var beginnerList = testHighscores.GetTopTen(difficulty);
            var loadedBegginerList = loadedHighScores.GetTopTen(difficulty);
            CollectionAssert.AreEqual(beginnerList, loadedBegginerList);
        }
    }
}
