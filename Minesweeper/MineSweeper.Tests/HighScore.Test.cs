using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Minesweeper.Tests
{
    [TestClass]
    public class HighScoreTests
    {
        [TestMethod]
        public void ShouldBeAbleToAddScoresToAllDifficultiesExceptCustom()
        {
            var highscore = new HighScores();
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "BeginnerScore",
                Score = 5
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "IntermediateScore",
                Score = 10
            }, GameDifficulty.Intermediate);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "ExpertScore",
                Score = 15
            }, GameDifficulty.Expert);

            var beginnerList = highscore.GetTopTen(GameDifficulty.Beginner);
            var intermediateList = highscore.GetTopTen(GameDifficulty.Intermediate);
            var expertList = highscore.GetTopTen(GameDifficulty.Expert);

            Assert.AreEqual(5, beginnerList[0].Score);
            Assert.AreEqual(10, intermediateList[0].Score);
            Assert.AreEqual(15, expertList[0].Score);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldNotBeAbleToAddCustomDifficultyHighscore()
        {
            var highscore = new HighScores();
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "CustomScore",
                Score = 5
            }, GameDifficulty.Custom);
        }

        [TestMethod]
        public void HighscoreShouldOnlyReturnTopTenInDescendingValue()
        {
            var highscore = new HighScores();
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Andy",
                Score = 10
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sam",
                Score = 5
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Nina",
                Score = 8
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Jack",
                Score = 2
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sammy",
                Score = 110
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Ingrid",
                Score = 0
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sigrid",
                Score = 44
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Jens",
                Score = 22
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Elizabeth",
                Score = 45
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Frank",
                Score = 63
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Bob",
                Score = 6
            }, GameDifficulty.Beginner);



            var highScoreSorted = highscore.GetTopTen(GameDifficulty.Beginner);

            Assert.AreEqual(10, highScoreSorted.Count);
            Assert.AreEqual("Ingrid", highScoreSorted[0].Name);
            Assert.AreEqual("Jack", highScoreSorted[1].Name);
            Assert.AreEqual("Sam", highScoreSorted[2].Name);
            //Frank is last on the list, since Sammy is so low she should not appear on it.
            Assert.AreEqual("Frank", highScoreSorted[9].Name);
        }


    }
}
