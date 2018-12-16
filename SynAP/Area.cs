using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SynAP
{
    public class Area
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
        public double Width { set; get; } = 0;

        /// <summary>
        /// Height of the Area.
        /// </summary>
        public double Height { set; get; } = 0;

        /// <summary>
        /// Position of the Area, relative to the top left corner.
        /// </summary>
        public Point Position { set; get; } = new Point(0, 0);

        public override string ToString() => $"{Width},{Height},{Position.X},{Position.Y}";

    }
}
