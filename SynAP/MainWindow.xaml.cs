using SynAP.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SynAP.Tools.ConvertHelper;

namespace SynAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console?.Log("Window loaded.");
            Config = LoadDefaultConfig();
            
            API = new API();
            API.Output += Console.Log;

            Driver = new Driver(API);
            Driver.Output += Console.Log;
            Driver.Status += StatusUpdate;

            Screen = new Screen();
            Touchpad = new Touchpad(API);

            DesktopRes = Screen.Bounds;
            TouchpadRes = Touchpad.Bounds;

            ScreenMapArea.BackgroundArea = DesktopRes;
            TouchpadMapArea.BackgroundArea = TouchpadRes;
            ScreenMapArea.ForegroundArea = Config.Screen;
            TouchpadMapArea.ForegroundArea = Config.Touchpad;
        }

        #region Properties

        private Area DesktopRes { get; set; }
        private Area TouchpadRes { get; set; }

        private API API { get; set; }
        private Driver Driver { get; set; }

        private Screen Screen { get; set; }
        private Touchpad Touchpad { get; set; }

        #endregion

        #region Main Buttons

        private void StartDriverButton(object sender = null, EventArgs e = null)
        {
            if (!Driver.IsActive)
            {
                Driver.ScreenArea = Config.Screen;
                Driver.TouchpadArea = Config.Touchpad;
                Driver.TouchpadDevice = Touchpad;
                Driver.Start();
            }
            else
                Driver.Stop();
        }

        #endregion

        #region Property Updates

        private void UpdateScreen(object sender = null, EventArgs e = null)
        {
            ScreenMapArea.ForegroundArea = Config.Screen;
        }

        private void UpdateTouchpad(object sender = null, EventArgs e = null)
        {
            TouchpadMapArea.ForegroundArea = Config.Touchpad;
            if (Config.LockAspectRatio)
            {
                if (sender == TouchpadHeightBox)
                    Config.Touchpad.Width = Math.Round(Config.Screen.Width / Config.Screen.Height * Config.Touchpad.Height);
                else if (sender == TouchpadWidthBox)
                    Config.Touchpad.Height = Math.Round(Config.Screen.Height / Config.Screen.Width * Config.Touchpad.Width);

            }
        }

        private void CenterArea(object sender = null, EventArgs e = null)
        {
            string tag = (string)(sender as Control).Tag;
            CenterHorizontal(tag);
            CenterVertical(tag);
        }

        private void CenterHorizontal(object sender = null, EventArgs e = null) => CenterHorizontal((string)(sender as Control).Tag);
        private void CenterHorizontal(string tag)
        {
            if (tag == "Screen")
                Config.Screen.Position.X = (Screen.Bounds.Width - Config.Screen.Width) / 2;
            if (tag == "Touchpad")
                Config.Touchpad.Position.X = (Touchpad.Bounds.Width - Config.Touchpad.Width) / 2;
        }

        private void CenterVertical(object sender = null, EventArgs e = null) => CenterVertical((string)(sender as Control).Tag);
        private void CenterVertical(string tag)
        {
            if (tag == "Screen")
                Config.Screen.Position.Y = (Screen.Bounds.Height - Config.Screen.Height) / 2 ;
            if (tag == "Touchpad")
                Config.Touchpad.Position.Y = (Touchpad.Bounds.Height - Config.Touchpad.Height) / 2;
        }

        #endregion

        #region File Management

        public Configuration Config
        {
            set
            {
                _config = value;
                NotifyPropertyChanged();
            }
            get => _config;
        }
        private Configuration _config;

        private Configuration LoadDefaultConfig()
        {
            try
            {
                return new Configuration(Info.DefaultConfigPath);
            }
            catch
            {
                return new Configuration();
            }
        }

        public void SaveDefaultConfig()
        {
            Config.Save(Info.DefaultConfigPath);
        }

        private void LoadDialog(object sender = null, EventArgs e = null)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "Bot configuration files (*.cfg)|*.cfg|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Config = new Configuration(dialog.FileName);
                }
                catch
                {
                    Console?.Log("Error: Invalid configuration file.");
                }
            }
        }

        private void SaveDialog(object sender = null, EventArgs e = null)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "Bot configuration files (*.cfg)|*.cfg|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Config.Save(dialog.FileName);
            }
        }

        #endregion

        #region Misc.

        private void StatusUpdate(object sender, string e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Status.Text = e;
            }));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ScreenMapArea.UpdateCanvas();
                TouchpadMapArea.UpdateCanvas();
            }
        }

        private void ShowAbout(object sender = null, EventArgs e = null)
        {
            new Windows.AboutBox().Show();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
