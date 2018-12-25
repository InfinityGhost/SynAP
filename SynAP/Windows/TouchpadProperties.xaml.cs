using SynAP.Devices;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SynAP.Windows
{
    /// <summary>
    /// Interaction logic for TouchpadProperties.xaml
    /// </summary>
    public partial class TouchpadProperties : Window
    {
        public TouchpadProperties(Touchpad touchpad, API api)
        {
            InitializeComponent();
            device = touchpad;
            API = api;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var bounds = device.Bounds;
            Width.Content = bounds.Width + " px";
            Height.Content = bounds.Height + " px";
        }

        private API API;
        private Touchpad device;
    }
}
