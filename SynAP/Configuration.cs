using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static SynAP.Tools.ReadHelper;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SynAP
{
    public class Configuration : INotifyPropertyChanged
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

        public Area Touchpad
        {
            set
            {
                _touchpad = value;
                NotifyPropertyChanged();
            }
            get => _touchpad;
        }
        private Area _touchpad;

        public Area Screen
        {
            set
            {
                _touchpad = value;
                NotifyPropertyChanged();
            }
            get => _screen;
        }
        private Area _screen;

        public bool LockAspectRatio
        {
            set
            {
                _lockaspectratio = value;
                NotifyPropertyChanged();
            }
            get => _lockaspectratio;
        }
        private bool _lockaspectratio;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
