using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAP.Devices
{
    public class Screen
    {
        public Screen()
        {
            Bounds = new Area(System.Windows.Forms.Screen.PrimaryScreen.Bounds);
        }

        public Area Bounds;
    }
}
