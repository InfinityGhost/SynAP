using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using SynAP;

namespace SynAP.Windows
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();

            DiscordTag.Content = Info.Discord.Tag;
            Version.Content = Info.AssemblyVersion;
            Website.Content = Info.GitHub;
        }

        #region Menu Buttons

        void CloseButton(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region Discord Tag Context Menu

        void CopyTagButton(object sender, RoutedEventArgs e) => Clipboard.SetText((string)DiscordTag.Content);
            
        #endregion

        #region Website Context Menu

        void OpenWebsite(object sender, RoutedEventArgs e) => Process.Start((string)Website.Content);

        #endregion

        #region General Event Handlers

        void OpenContextMenu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Control control)
                if (control.ContextMenu != null)
                    control.ContextMenu.IsOpen = true;
        }

        #endregion

    }
}
