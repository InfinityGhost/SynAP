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

        public Task Start()
        {
            Output?.Invoke(this, "Starting...");
            API.Device.OnPacket += API_OnPacket;
            Output?.Invoke(this, "Device hooked.");

            ScaleX = ScreenArea.Width / TouchpadArea.Width;
            ScaleY = ScreenArea.Height / TouchpadArea.Height;
            Output?.Invoke(this, "ScaleX,ScaleY:" + $"{ScaleX},{ScaleY}");

            IsActive = true;
            Task.Delay(-1);
            return Task.CompletedTask;
        }

        private double ScaleX { set; get; }
        private double ScaleY { set; get; }

        public Task Stop()
        {
            API.Device.OnPacket -= API_OnPacket;
            Output?.Invoke(this, "Stopped.");

            IsActive = false;
            return Task.CompletedTask;
        }

        private void API_OnPacket()
        {
            try
            {
                int result = API.Device.LoadPacket(API.Packet);
            }
            catch(COMException comex)
            {
                Output?.Invoke(this, comex.ToString());
                return;
            }
            // this tends to throw an exception once in a while

            //Output?.Invoke(this, "Coords:" + $"{API.Packet.X},{API.Packet.Y}");

            // TODO: add cursor positioning

            if (Enum.TryParse(API.Packet.FingerState.ToString(), out API.FingerState))
            {
                if (API.FingerState.HasFlag(SynFingerFlags.SF_FingerTouch))
                {
                    int XPos = Convert.ToInt32((TouchpadDevice.Bounds.Position.X - API.Packet.X - TouchpadArea.Position.X) * ScaleX);
                    int YPos = Convert.ToInt32((API.Packet.Y - TouchpadArea.Position.Y - TouchpadDevice.Bounds.Position.Y) * ScaleY);
                    Cursor.Position = new System.Drawing.Point
                    {
                        X = XPos,
                        Y = YPos,
                    };
                    Status?.Invoke(this, "X,Y:" + "{" + $"{XPos},{YPos}" + "}");
                }
            }
        }

        #endregion

    }
}
