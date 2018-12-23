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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynAP.Controls
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        public Console()
        {
            InitializeComponent();
        }

        public static string Prefix => DateTime.Now.ToLocalTime() + ": ";
        public bool BufferEmpty => Buffer.Text == string.Empty || Buffer.Text == null;

        public event EventHandler<string> Status;

        public Task Log(string text)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (BufferEmpty)
                    Buffer.Text += Prefix + text;
                else
                    Buffer.Text += Environment.NewLine + Prefix + text;
                Status?.Invoke(this, text);
            }));
            return Task.CompletedTask;
        }
        public async void Log(object sender, string text) => await Log(sender.GetType().Name + " - " + text);

        public Task Clear()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Buffer.Text = string.Empty;
            }));
            return Task.CompletedTask;
        }

        public void Copy(object sender, EventArgs e) => Clipboard.SetText(Buffer.Text);
        public async void Clear(object sender, EventArgs e) => await Clear();

    }
}
