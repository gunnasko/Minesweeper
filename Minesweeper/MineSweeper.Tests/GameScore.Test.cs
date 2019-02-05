using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Minesweeper.Interfaces;
using System.ComponentModel;

namespace Minesweeper.Tests
{
    [TestClass]
    public class GameScoreTests
    {

        [TestMethod]
        public void GameScoreShouldIncrementWhenTimerElapses()
        {
            var timerMocker = new Mock<ITimer>();
            var gameScore = new GameScore(timerMocker.Object);

            gameScore.StartScoreCounter();
            //Verify start has been run.
            timerMocker.Verify(t => t.Start(), Times.Once());

            timerMocker.Raise(t => t.Elapsed += null, new EventArgs() as System.Timers.ElapsedEventArgs);
            Assert.AreEqual(1, gameScore.Score);

            timerMocker.Raise(t => t.Elapsed += null, new EventArgs() as System.Timers.ElapsedEventArgs);
            Assert.AreEqual(2, gameScore.Score);
        }

        [TestMethod]
        public void ShouldBeAbleToResetGamerScore()
        {
            var timerMocker = new Mock<ITimer>();
            var gameScore = new GameScore(timerMocker.Object);

            gameScore.StartScoreCounter();
            //Verify start has been run.
            timerMocker.Verify(t => t.Start(), Times.Once());

            timerMocker.Raise(s => s.Elapsed += null, new EventArgs() as System.Timers.ElapsedEventArgs);
            Assert.AreEqual(1, gameScore.Score);

            gameScore.ResetScore();
            Assert.AreEqual(0, gameScore.Score);

            timerMocker.Raise(s => s.Elapsed += null, new EventArgs() as System.Timers.ElapsedEventArgs);
            Assert.AreEqual(1, gameScore.Score);
        }

        [TestMethod]
        public void ShouldBeAbleToStopTimer()
        {
            var timerMocker = new Mock<ITimer>();
            var gameScore = new GameScore(timerMocker.Object);
           
            gameScore.StopScoreCounter();
            timerMocker.Verify(t => t.Stop(), Times.Once());
        }

        [TestMethod]
        public void TimerIntervallShouldBe1Second()
        {
            var timerMocker = new Mock<ITimer>();
            var gameScore = new GameScore(timerMocker.Object);
            timerMocker.VerifySet(t => t.Interval = 1000);
        }

        [TestMethod]
        public void GameScoreShouldNotifyWhenScoreIsIncrementedAndReset()
        {
            var timerMocker = new Mock<ITimer>();
            var gameScore = new GameScore(timerMocker.Object);
            List<string> receivedEvents = new List<string>();
            gameScore.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                receivedEvents.Add(e.PropertyName);
            };

            //Increment score
            timerMocker.Raise(s => s.Elapsed += null, new EventArgs() as System.Timers.ElapsedEventArgs);

            //Reset score
            gameScore.ResetScore();

            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual("Score", receivedEvents[0]);
            Assert.AreEqual("Score", receivedEvents[1]);
        }


    }
}
