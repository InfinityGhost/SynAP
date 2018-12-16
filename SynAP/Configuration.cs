using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static SynAP.Tools.ReadHelper;

namespace SynAP
{
    public class Configuration 
    {
        public Configuration()
        {
            Touchpad = new Area();
            Screen = new Area();
        }

        public Configuration(string path) => Load(path);

        public Configuration(Area touchpad, Area screen)
        {
            Touchpad = touchpad;
            Screen = screen;
        }

        public Area Touchpad { set; get; }
        public Area Screen { set; get; }

        public bool LockAspectRatio;

        #region File Management

        private readonly string Splitter = ":";

        public Task Load(string path)
        {
            var file = File.ReadAllLines(path);
            Touchpad = new Area(file.GetProperty("Touchpad"));
            Screen = new Area(file.GetProperty("Desktop"));
            LockAspectRatio = file.GetProperty("LockAspectRatio").ToBool();
            return Task.CompletedTask;
        }

        public Task Save(string path)
        {
            string[] vs =
            {
                "Touchpad" + Splitter + Touchpad,
                "Desktop" + Splitter + Screen,
                "LockAspectRatio" + Splitter + LockAspectRatio,
            };
            File.WriteAllLines(path, vs);
            return Task.CompletedTask;
        }

        #endregion
    }
}
