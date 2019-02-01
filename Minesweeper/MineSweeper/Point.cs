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
        private bool isOpened;
        public bool HasMine { get; set; }
        public bool IsFlagged { get; set; }
        public Coordinate PointCoordinate { get; }
        public bool IsOpened
        {
            get
            {
                return isOpened;
            }
            set
            {
                isOpened = value;
                OnPropertyChanged("IsOpened");
            }
        }

        public Point(Coordinate coordinate)
        {
            PointCoordinate = coordinate;
            IsOpened = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
