using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.ComponentModel;
using Minesweeper.Interfaces;

namespace Minesweeper
{
    public class GameScore : INotifyPropertyChanged, IDisposable
    {
        private const double TIMER_INTERVALL_IN_MS = 1000;
        private ITimer _scoreTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Score { get; private set; }

        public GameScore(ITimer scoreTimer)
        {
            ResetScore();
            _scoreTimer = scoreTimer;
            _scoreTimer.Interval = TIMER_INTERVALL_IN_MS;
            _scoreTimer.Elapsed += OnTimedEvent;
        }

        public void Dispose()
        {
            //TODO: Look into proper dispose patterns with try/catch
            _scoreTimer.Dispose();
        }


        public void ResetScore()
        {
            Score = 0;
            OnPropertyChanged("Score");
        }

        public void StartScoreCounter()
        {
            _scoreTimer.Start();
        }

        public void StopScoreCounter()
        {
            _scoreTimer.Stop();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Score++;
            OnPropertyChanged("Score");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
