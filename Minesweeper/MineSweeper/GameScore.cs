using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.ComponentModel;

namespace Minesweeper
{
    public class GameScore : INotifyPropertyChanged, IDisposable
    {
        private const double TIMER_INTERVALL_IN_MS = 1000;
        private Timer _scoreTimer;
        private int _score;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Score { get { return _score; } }

        public GameScore()
        {
            ResetScore();
            _scoreTimer = new Timer(TIMER_INTERVALL_IN_MS);
            _scoreTimer.Elapsed += OnTimedEvent;
            _scoreTimer.AutoReset = true;
        }

        public void Dispose()
        {
            //TODO: Look into proper dispose patterns with try/catch
            _scoreTimer.Dispose();
        }


        public void ResetScore()
        {
            _score = 0;
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
            _score++;
            OnPropertyChanged("Score");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
