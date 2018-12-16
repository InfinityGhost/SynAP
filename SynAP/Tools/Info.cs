using System.IO;
using System.Reflection;

namespace SynAP
{
    static class Info
    {
        /// <summary>
        /// Program version.
        /// </summary>
        public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// The default path for the configuration file.
        /// </summary>
        public static string DefaultConfigPath = Directory.GetCurrentDirectory() + @"\" + "default.cfg";

        /// <summary>
        /// This project's Github link.
        /// </summary>
        public static string GitHub = "N/A";

        public class Discord
        {
            /// <summary>
            /// Developer's discord tag.
            /// </summary>
            public static string Tag = "InfinityGhost#7843";
        }
    }
}
