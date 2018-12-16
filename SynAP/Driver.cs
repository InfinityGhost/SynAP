using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYNCTRLLib;

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
            Screen = screen;
            Touchpad = touchpad;
        }

        public event EventHandler<string> Output;

        private API API;

        private Area Screen;
        private Area Touchpad;

        #region Methods

        public Task Start()
        {
            Output?.Invoke(this, "Starting...");
            API.Device.OnPacket += API_OnPacket;
            Output?.Invoke(this, "Device hooked.");

            Task.Delay(-1);
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            API.Device.OnPacket -= API_OnPacket;
            Output?.Invoke(this, "Stopped.");
            
            return Task.CompletedTask;
        }

        private void API_OnPacket()
        {
            var result = API.Device.LoadPacket(API.Packet);
            
            // TODO: add cursor positioning
        }

        #endregion

    }
}
