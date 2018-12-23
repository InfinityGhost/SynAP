using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cursor = System.Windows.Forms.Cursor;
using SYNCTRLLib;
using System.Runtime.InteropServices;
using SynAP.Devices;

namespace SynAP
{
    class Driver
    {
        public Driver(API api)
        {
            API = api;
        }

        public Driver(API api, Area screen, Area touchpad) : this(api)
        {
            ScreenArea = screen;
            TouchpadArea = touchpad;
        }

        public Driver(API api, Area screen, Area touchpad, Touchpad device) : this(api, screen, touchpad)
        {
            TouchpadDevice = device;
        }

        public event EventHandler<string> Output;
        public event EventHandler<string> Status;

        public bool IsActive { private set; get; }

        private API API;

        public Area ScreenArea;
        public Area TouchpadArea;
        public Touchpad TouchpadDevice;

        #region Methods

        public void Start()
        {
            if (!API.IsAvailable)
            {
                Output?.Invoke(this, "Starting...");
                API.Device.OnPacket += API_OnPacket;
                Output?.Invoke(this, "Device hooked.");

                ScaleX = ScreenArea.Width / TouchpadArea.Width;
                ScaleY = ScreenArea.Height / TouchpadArea.Height;
                Output?.Invoke(this, "ScaleX,ScaleY:" + $"{ScaleX},{ScaleY}");
                Output?.Invoke(this, "Device Bounds:" + TouchpadDevice);

                IsActive = true;
            }
        }

        private double ScaleX { set; get; }
        private double ScaleY { set; get; }

        public void Stop()
        {
            API.Device.OnPacket -= API_OnPacket;
            Output?.Invoke(this, "Stopped.");

            IsActive = false;
        }

        private void API_OnPacket()
        {
            try
            {
                int result = API.Device.LoadPacket(API.Packet);
            }
            catch(COMException comex)
            {
                Output?.Invoke(this, $"Synaptics API Exception:" + comex.ErrorCode);
                return;
            }
            // this tends to throw an exception once in a while

            if (Enum.TryParse(API.Packet.FingerState.ToString(), out API.FingerState))
            {
                if (API.FingerState.HasFlag(SynFingerFlags.SF_FingerTouch))
                {
                    int XPos = Convert.ToInt32((API.Packet.X - TouchpadDevice.X_Lo) * ScaleX);
                    int YPos = Convert.ToInt32((TouchpadDevice.Y_Hi - API.Packet.Y) * ScaleY);
                    Cursor.Position = new System.Drawing.Point
                    {
                        X = XPos,
                        Y = YPos,
                    };
                    //Status?.Invoke(this, "X,Y:" + "{" + $"{XPos},{YPos}" + "}");
                }
            }
        }

        #endregion

    }
}
