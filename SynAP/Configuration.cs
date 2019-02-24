using System.IO;
using static SynAP.Tools.ReadHelper;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace SynAP
{
    [XmlRoot("SynAP Configuration", IsNullable = true)]
    public class Configuration : INotifyPropertyChanged
    {
        public Configuration()
        {
            Touchpad = new Area();
            Screen = new Area();
        }

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
                _screen = value;
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

        private static XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));

        public void Save(string path)
        {
            TextWriter tw = new StreamWriter(path);
            Serializer.Serialize(tw, this);
        }

        public static Configuration Read(string path)
        {
            using (var sr = new StreamReader(path))
                return (Configuration)Serializer.Deserialize(sr);
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
