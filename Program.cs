using System;
using System.Windows.Forms;
using marVSS2026.Classes;

namespace marVSS2026
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Delegate startup to LoadMain (ported from VB6 aLoadMain.Main)
            LoadMain.Main();
        }
    }
}   