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
                int X_Lo = api.GetProperty(SynDeviceProperty.SP_XLoSensor);
                int X_Hi = api.GetProperty(SynDeviceProperty.SP_XHiSensor);
                int Y_Lo = api.GetProperty(SynDeviceProperty.SP_YLoSensor);
                int Y_Hi = api.GetProperty(SynDeviceProperty.SP_YHiSensor);
                Bounds = new Area(X_Hi - X_Lo, Y_Hi - Y_Lo, new System.Windows.Point(X_Lo, Y_Lo));
            }
            else
                Bounds = new Area(6143, 6143);
        }

        public Area Bounds;
    }
}
