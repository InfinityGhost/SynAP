using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SYNCTRLLib;

namespace SynAP
{
    class API
    {   
        public API()
        {
            try
            {
                APICtrl = new SynAPICtrl();
                Device = new SynDeviceCtrl();
                Packet = new SynPacketCtrl();
                IsAvailable = true;
                Init();
            }
            catch (COMException)
            {
                IsAvailable = false;
                Output?.Invoke(this, "API unavailable. Please install Synaptics Touchpad drivers.");
            }
        }

        public event EventHandler<string> Output;

        public SynDeviceCtrl Device;
        public SynAPICtrl APICtrl;
        public SynPacketCtrl Packet;

        public bool IsAvailable { private set; get; }
        public int DeviceHandle;

        public Task Init()
        {
            APICtrl.Initialize();
            APICtrl.Activate();
            DeviceHandle = APICtrl.FindDevice(SynConnectionType.SE_ConnectionAny, SynDeviceType.SE_DeviceTouchPad, -1);
            Device.Select(DeviceHandle);
            Device.Activate();

            Output?.Invoke(this, "API successfully initialized.");
            return Task.CompletedTask;
        }

        public int GetProperty(SynDeviceProperty property)
        {
            return Device.GetLongProperty(property);
        }

    }
}
