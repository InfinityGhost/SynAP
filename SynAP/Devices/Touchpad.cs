using SYNCTRLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAP.Devices
{
    class Touchpad
    {
        public Touchpad()
        {
            Bounds = new Area(6143, 6143);
        }

        public Touchpad(API api)
        {
            if (api.IsAvailable)
            {
                X_Lo = api.GetProperty(SynDeviceProperty.SP_XLoSensor);
                X_Hi = api.GetProperty(SynDeviceProperty.SP_XHiSensor);
                Y_Lo = api.GetProperty(SynDeviceProperty.SP_YLoSensor);
                Y_Hi = api.GetProperty(SynDeviceProperty.SP_YHiSensor);
                Bounds = new Area(X_Hi - X_Lo, Y_Hi - Y_Lo, new Point(X_Lo, Y_Lo));
            }
            else
                Bounds = new Area(6143, 6143);
        }

        public Area Bounds;

        public int X_Lo;
        public int X_Hi;
        public int Y_Lo;
        public int Y_Hi;

        public override string ToString() => $"[{Bounds}],[{X_Lo},{X_Hi}|{Y_Lo},{Y_Hi}]";
    }
}
