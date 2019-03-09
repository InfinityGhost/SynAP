using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAP.Controls.Resource
{
    class ScaleTool
    {
        public ScaleTool(double frameWidth, double frameHeight, double objectWidth, double objectHeight)
        {
            ValueX = frameWidth / objectWidth;
            ValueY = frameHeight / objectHeight;
        }

        private double ValueX;
        private double ValueY;

        public double Value
        {
            get
            {
                if (ValueX > ValueY)
                    return ValueY;
                else
                    return ValueX;
            }
        }
    }
}
