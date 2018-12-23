using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormsIcon = System.Windows.Forms.NotifyIcon;

namespace SynAP
{
    public class NotifyIcon
    {
        public NotifyIcon()
        {
            string iconPath = @"SynAP.Icon.ico";
            Assembly assembly = Assembly.GetExecutingAssembly();

            Icon.Icon = new System.Drawing.Icon(assembly.GetManifestResourceStream(iconPath));
            Icon.MouseClick += NotifyIcon_Click;
            Icon.Text = "SynAP " + $"v{Info.AssemblyVersion}";
        }

        FormsIcon Icon = new FormsIcon();

        public event EventHandler ShowWindow;

        public bool Visible
        {
            set => Icon.Visible = value;
            get => Icon.Visible;
        }

        private void NotifyIcon_Click(object sender, MouseEventArgs e) => ShowWindow?.Invoke(this, null);
    }
}
