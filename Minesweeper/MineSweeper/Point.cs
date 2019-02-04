using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class Point : INotifyPropertyChanged
    {
        private bool _isOpened;
        private bool _isFlagged;
        public bool IsFlagged
        {
            get
            {
                return _isFlagged;
            }
            set
            {
                if (_isFlagged != value)
                {
                    _isFlagged = value;
                    OnPropertyChanged("IsFlagged");
                }
            }
        }
        public bool HasMine { get; set; }
        public Coordinate PointCoordinate { get; }
        public bool IsOpened
        {
            get
            {
                return _isOpened;
            }
            set
            {
                if (_isOpened != value)
                {
                    _isOpened = value;
                    OnPropertyChanged("IsOpened");
                }
            }
        }

        public Point(Coordinate coordinate)
        {
            PointCoordinate = coordinate;
            IsOpened = false;
        }

        public int AdjacentMines { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
