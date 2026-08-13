using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using marVSS2028.Classes;

namespace marVSS2028
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run VB6-like startup logic converted to C# (LoadMain.Main)
            LoadMain.Main();

            // Show main form (Mim in VB6, FormMim in .NET)
            Application.Run(new FormMim());
        }
    }
}
