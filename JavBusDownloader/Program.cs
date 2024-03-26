using Data;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JavBusDownloader
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Core.Initialize();

            if (Settings.Debug)
            {
                AllocConsole();
            }

            Core.NewForm(MovieMod.normal, SearchMod.search, "");

            while (Core.fromList.Count != 0)
            {
                Application.DoEvents();
            }
        }
    }
}
