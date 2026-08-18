using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using static marVSS2028.Classes.MimEnvironment;

namespace marVSS2028.Classes
{
    internal static class LoadMain // This class is responsible for the initial setup and configuration of the application, including versioning, registry access, and ensuring only one instance of the application is running.
    {
        public static string strConnect;
        public static string strLogFile;

        public static void Main()
        {
            // Set version manually to match ClickOnce publish version format (major.minor.build.revision)
            Globals.MAR_VERSION = "0.1.0.12";
            BeWaarTekst("marIntegraal", "VersionNumber", Globals.MAR_VERSION);
            Globals.IsPreviewMode = true;

            // Determine application version
            var asm = Assembly.GetEntryAssembly();
            string title = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyProductAttribute)))?.Product;
            Globals.appTitleAndVersion = title + " v." + Globals.MAR_VERSION;

            Globals.PeppolFlag = false;
            Globals.DecimalKTRL = false;

            // Get location of marnt\data folder from older registry or use default location
            // Load MimDataLocation from marIntegraal settings
            // Value must contains "\marnt\data"
            string valuePath = LaadTekstOLD("marIntegraal", "Bedrijfsinhoudsopgave2025");
            Globals.LOCATION_MYDOCUMENTS = valuePath;
            BeWaarTekst("marIntegraal", "Bedrijfsinhoudsopgave2025", valuePath);
            BeWaarTekst("marIntegraal", "LOCATION_", Application.StartupPath);

            // Check for previous instance of this process
            bool prevInstance = false;
            try
            {
                string name = Process.GetCurrentProcess().ProcessName;
                var procs = Process.GetProcessesByName(name);
                foreach (var p in procs)
                {
                    if (p.Id != Process.GetCurrentProcess().Id)
                    {
                        prevInstance = true;
                        break;
                    }
                }
            }
            catch { prevInstance = false; }

            if (prevInstance)
            {
                string msg = Application.ProductName + " draait reeds op dit systeem !  Wenst U een bijkomende instantie van marIntegraal te openen voor deze computer én gebruiker (enkel aanbevolen voor netwerktesten)\r\n\r\n" +
                             "Via CTRL+ALT+DEL kan U eventueel het venster TAAKBEHEER opstarten voor extra opties (enkel bij NT/2000/XP/Vista-versies van Windows)\r\n\r\n";

                var res = MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.No)
                {
                    System.Environment.Exit(0);
                    return;
                }
            }

            Globals.BL_LOGGING = false;

            // Initialize ADODB recordset array (10 sets) with null values, as VB6 would do by default  
            for (int i = 0; i <= 9; i++)
            {
                try
                {
                    Globals.rsMAR[i] = new ADODB.Recordset();
                }
                catch
                {
                    Globals.rsMAR[i] = null;
                }
            }

            // Change current directory to application path
            try
            {
                Directory.SetCurrentDirectory(Application.StartupPath);
            }
            catch { }

            // VB6: Set fs = New FileSystemObject -> keep placeholder object
            Globals.fs = new object();
        }
    }
}
