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

        [TestMethod]
        public void HighscoreTopTenShouldPrioritizeNewestDateOnEqualScore()
        {
            var highscore = new HighScores();
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sammy",
                Score = 50,
                Date = DateTime.Parse("01/11/2001 07:30:15")
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Ingrid",
                Score = 50,
                Date = DateTime.Parse("01/11/2001 07:30:10")

            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Joe",
                Score = 50,
                Date = DateTime.Parse("02/11/2001 07:30:10")
            }, GameDifficulty.Beginner);
                
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Bob",
                Score = 51,
                Date = DateTime.Parse("02/11/2001 07:25:15")
            }, GameDifficulty.Beginner);

            var highScoreSorted = highscore.GetTopTen(GameDifficulty.Beginner);
            Assert.AreEqual(4, highScoreSorted.Count);
            Assert.AreEqual("Joe", highScoreSorted[0].Name);
            Assert.AreEqual("Sammy", highScoreSorted[1].Name);
            Assert.AreEqual("Ingrid", highScoreSorted[2].Name);
            Assert.AreEqual("Bob", highScoreSorted[3].Name);
        }

        [TestMethod]
        public void CheckingTopTenShouldAlwaysReturnTrueIfTopTenIsNotFull()
        {
            var highscore = new HighScores();
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Andy",
                Score = 5
            }, GameDifficulty.Beginner);

            Assert.IsTrue(highscore.IsScoreTopTenCandidate(11, GameDifficulty.Beginner));
            Assert.IsTrue(highscore.IsScoreTopTenCandidate(1, GameDifficulty.Beginner));
        }

        [TestMethod]
        public void CheckingHighscoreShouldOnlyCheckTopTenInHighscore()
        {
            var highscore = new HighScores();
            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Andy",
                Score = 1
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sam",
                Score = 2
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Nina",
                Score = 3
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Jack",
                Score = 4
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sammy",
                Score = 5
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Ingrid",
                Score = 6
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Sigrid",
                Score = 7
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Jens",
                Score = 8
            }, GameDifficulty.Beginner);

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Elizabeth",
                Score = 9
            }, GameDifficulty.Beginner);

            Assert.IsTrue(highscore.IsScoreTopTenCandidate(9, GameDifficulty.Beginner));
            Assert.IsTrue(highscore.IsScoreTopTenCandidate(10, GameDifficulty.Beginner));

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Frank",
                Score = 10
            }, GameDifficulty.Beginner);

            Assert.IsTrue(highscore.IsScoreTopTenCandidate(10, GameDifficulty.Beginner));
            Assert.IsFalse(highscore.IsScoreTopTenCandidate(11, GameDifficulty.Beginner));

            highscore.AddHighScore(new HighScoreEntry
            {
                Name = "Bob",
                Score = 11
            }, GameDifficulty.Beginner);

            Assert.IsFalse(highscore.IsScoreTopTenCandidate(11, GameDifficulty.Beginner));
            Assert.IsFalse(highscore.IsScoreTopTenCandidate(12, GameDifficulty.Beginner));

            Assert.IsTrue(highscore.IsScoreTopTenCandidate(10, GameDifficulty.Beginner));
            Assert.IsTrue(highscore.IsScoreTopTenCandidate(5, GameDifficulty.Beginner));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldNotBeAbleGetHighscoreFromCustomDifficulty()
        {
            var highscore = new HighScores();
            highscore.GetTopTen(GameDifficulty.Custom);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldNotBeAbleToProbeTopTenFromCustomDifficulty()
        {
            var highscore = new HighScores();
            highscore.IsScoreTopTenCandidate(0, GameDifficulty.Custom);
        }


    }
}
