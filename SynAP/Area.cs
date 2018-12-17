using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SynAP
{
    public class Area : INotifyPropertyChanged
    {
        public Area()
        {
            Width = 0;
            Height = 0;
            Position = new Point(0, 0);
        }

        public Area(double width, double height)
        {
            Width = width;
            Height = height;
            Position = new Point(0, 0);
        }

        public Area(double width, double height, Point pos) : this(height, width)
        {
            Position = pos;
        }

        public Area(string area)
        {
            var properties = area.Split(',');
            Width = Convert.ToDouble(properties[0]);
            Height = Convert.ToDouble(properties[1]);
            var x = Convert.ToDouble(properties[2]);
            var y = Convert.ToDouble(properties[3]);
            Position = new Point(x, y);
        }

        public Area(System.Drawing.Rectangle area) : this(area.Width, area.Height) { }

        public Area(System.Drawing.Rectangle area, Point pos) : this(area)
        {
            Position = pos;
        }

        /// <summary>
        /// Width of the Area.
        /// </summary>
        public double Width
        {
            set
            {
                _width = value;
                NotifyPropertyChanged();
            }
            get => _width;
        }
        private double _width;

        /// <summary>
        /// Height of the Area.
        /// </summary>
        public double Height
        {
            set
            {
                _height = value;
                NotifyPropertyChanged();
            }
            get => _height;
        }
        private double _height;

        /// <summary>
        /// Position of the Area, relative to the top left corner.
        /// </summary>
        public Point Position
        {
            set
            {
                _position = value;
                NotifyPropertyChanged();
            }
            get => _position;
        }
        private Point _position;

        public override string ToString() => $"{Width},{Height},{Position.X},{Position.Y}";

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
