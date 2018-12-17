using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SynAP
{
    public class Point : INotifyPropertyChanged
    {
        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double X
        {
            set
            {
                _x = value;
                NotifyPropertyChanged();
            }
            get => _x;
        }
        private double _x;

        public double Y
        {
            set
            {
                _y = value; 
                NotifyPropertyChanged();
            }
            get => _y;
        }
        private double _y;

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
