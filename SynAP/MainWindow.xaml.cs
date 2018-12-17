using SynAP.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Config;
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

        private async void StartDriverButton(object sender = null, EventArgs e = null)
        {
            if (!Driver.IsActive)
            {
                Driver.ScreenArea = Config.Screen;
                Driver.TouchpadArea = Config.Touchpad;
                Driver.TouchpadDevice = Touchpad;
                await Driver.Start();
            }
            else
                await Driver.Stop();
        }

        #endregion

        #region Property Updates

        public void UpdateScreen(object sender = null, EventArgs e = null)
        {
            Config.Screen = new Area
            {
                Width = ScreenWidthBox.GetDouble(),
                Height = ScreenHeightBox.GetDouble(),
                Position = new Point(ScreenXBox.GetDouble(), ScreenYBox.GetDouble())
            };
            ScreenMapArea.ForegroundArea = Config.Screen;
        }

        public void UpdateTouchpad(object sender = null, EventArgs e = null)
        {
            Config.Touchpad = new Area
            {
                Width = TouchpadWidthBox.GetDouble(),
                Height = TouchpadHeightBox.GetDouble(),
                Position = new Point(TouchpadXBox.GetDouble(), TouchpadYBox.GetDouble())
            };
            TouchpadMapArea.ForegroundArea = Config.Touchpad;
        }

        #endregion

        #region File Management

        public Configuration Config { get; set; }

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

        public async void SaveDefaultConfig()
        {
            await Config.Save(Info.DefaultConfigPath);
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

        private async void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsLoaded)
            {
                await ScreenMapArea.UpdateCanvas();
                await TouchpadMapArea.UpdateCanvas();
            }
        }

        private void ShowAbout(object sender = null, EventArgs e = null)
        {
            new Windows.AboutBox().Show();
        }

        #endregion

    }
}
