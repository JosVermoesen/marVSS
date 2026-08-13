using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace marVSSNET.Classes
{
    internal class LoadMain
    {
        public static string? strConnect;
        public static string? strLogFile;

        public static void Initialize()
        {
            // Set version manually to match ClickOnce publish version format (major.minor.build.revision)
            Globals.MAR_VERSION = "0.2.2.3";
            //TODO:  BeWaarTekst("marIntegraal", "VersionNumber", Globals.MAR_VERSION);
            Globals.IsPreviewMode = true;

            // Determine application version
            var asm = Assembly.GetEntryAssembly();
            Globals.appTitleAndVersion = (asm?.GetName().Name ?? "marVSS") + " v." + Globals.MAR_VERSION;

            Globals.PeppolFlag = false;
            Globals.DecimalKTRL = false;

        }
    }
}