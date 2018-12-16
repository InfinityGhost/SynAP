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
                int xLo = api.GetProperty(SynDeviceProperty.SP_XLoSensor);
                int xHi = api.GetProperty(SynDeviceProperty.SP_XHiSensor);
                int yLo = api.GetProperty(SynDeviceProperty.SP_YLoSensor);
                int yHi = api.GetProperty(SynDeviceProperty.SP_YHiSensor);
                Bounds = new Area(xHi - xLo, yHi - yLo);
            }
            else
                Bounds = new Area(6143, 6143);
        }

        public Area Bounds;
    }
}
