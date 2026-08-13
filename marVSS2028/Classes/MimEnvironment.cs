using Microsoft.VisualBasic; // For Environment variables and possibly other VB-specific functions
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.OleDbTools;


using marVSS2028.SharedForms;

namespace marVSS2028.Classes
{
    internal static class MimEnvironment
    {        
        /// <summary>        
        /// Closes the active company: unloads open windows, closes all tables and ADO
        /// connections, resets menu/toolbar states and the company data location.
        /// </summary>
        public static void AutoUnLoadCompany()
        {
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.ToolStripLabel1.Text = "";
                mim.ToolStripLabel2.Text = "";
                mim.ToolStripLabel3.Text = "";
                mim.SystemSubMenu.Enabled = false;
                mim.FilesSubMenu.Enabled = false;
                mim.DailyManagementSubMenu.Enabled = false;
                mim.AccountingSubMenu.Enabled = false;
                mim.ContractsSubMenu.Enabled = false;
                mim.MenuActiesCloseCompany.Enabled = false;
                mim.Cursor = Cursors.WaitCursor;
            }
            Ktrl = 100;

            // Close all MDI child windows except the three BasisB forms or FormBYPERDAT which may be open during company switch
            if (Application.OpenForms["FormMim"] is FormMim mimClose)
            {
                foreach (Form child in mimClose.MdiChildren)
                {
                    if (child is FormBasicTable || child is FormBYPERDAT)
                        continue;
                    child.Close();
                }
            }

            // Close all open tables
            BClose(99);

            // Free ADO connections
            try
            {
                if (rsJournaal != null &&
                    rsJournaal.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                    rsJournaal.Close();
                rsJournaal = null;
            }
            catch { }

            for (int i = TABLE_VARIOUS; i <= TABLE_CONTRACTS; i++)
            {
                try
                {
                    if (rsMAR[i] != null &&
                        rsMAR[i].State != (int)ADODB.ObjectStateEnum.adStateClosed)
                        rsMAR[i].Close();
                }
                catch { }
            }

            // Reset BasisB forms
            for (int t = 1; t <= 3; t++)
            {
                if (BasisB[t] != null)
                {
                    BasisB[t].WindowState = FormWindowState.Minimized;
                    BasisB[t].Enabled = false;
                }
            }

            // Disable FormBYPERDAT
            foreach (Form f in Application.OpenForms)
            {
                if (f is FormBYPERDAT byp)
                {
                    byp.WindowState = FormWindowState.Minimized;
                    byp.Enabled = false;
                    break;
                }
            }

            // Disable shortcut keys for persistent forms
            if (Application.OpenForms["FormMim"] is FormMim mimPersistent)
                mimPersistent.SetPersistentFormsEnabled(false);

            LOCATION_COMPANYDATA = LOCATION_;

            if (Application.OpenForms["FormMim"] is FormMim mimCaption)
                mimCaption.Text = appTitleAndVersion;

            try
            {
                if (adntDB != null &&
                    adntDB.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                    adntDB.Close();
                adntDB = null;
            }
            catch { }

            LOCATION_COMPANYDATA = string.Empty;

            FormBYPERDAT formBYPERDAT = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FormBYPERDAT byp) { formBYPERDAT = byp; break; }
            }
            if (formBYPERDAT != null)
            {
                formBYPERDAT.Enabled = false;
                formBYPERDAT.WindowState = FormWindowState.Minimized;
            }

            if (Application.OpenForms["FormMim"] is FormMim mimCursor)
                mimCursor.Cursor = Cursors.Default;
        }

        // VB6 Static local: ProducentKopij As String * 8 — survives between calls
        private static string _producentKopij = string.Empty;

        /// <summary>        
        /// Opens the active company: reads period/bookyear OCT files, opens the ADO
        /// database connection, runs schema-update checks and initialises table indexes.
        /// UI elements that reference BYPERDAT or Mim menu arrays not yet ported are
        /// marked with TODO comments.
        /// </summary>
        public static void AutoLoadCompany()
        {
            // netVoorbereiden
            Ktrl = NetVoorbereiden() ? 1 : 0;

            // TODO after VB6 version running: check if there is "9999.OCT" found in company data location
            string dest9999 = LOCATION_COMPANYDATA + "9999.OCT";
            if (!File.Exists(dest9999))
                CopyFile(PROGRAM_LOCATION, LOCATION_COMPANYDATA, "9999.OCT");

            // ---- Read 9999.OCT: bookyear list ----
            string ninetyNineOct = LOCATION_COMPANYDATA + "9999.OCT";

            byte[] buf4 = new byte[4];
            byte[] buf16 = new byte[16];
            string aaStr;       // 4-byte record value
            int aktievePeriode = 0;

            // Bookyear items accumulated for period form (replaces BYPERDAT.Boekjaar combo)
            var boekjaarItems = new System.Collections.Generic.List<string>();

            try
            {
                using (var fs9 = new FileStream(ninetyNineOct, FileMode.Open, FileAccess.Read))
                {
                    // Get record 1 (unused result, kept for file-pointer parity)
                    fs9.Seek(0L, SeekOrigin.Begin);
                    fs9.Read(buf4, 0, 4);

                    ACTIVE_BOOKYEAR = 0;

                    // Get record 2 (unused result)
                    fs9.Seek(4L, SeekOrigin.Begin);
                    fs9.Read(buf4, 0, 4);

                    // Build bookyear list from DEFxx.OCT files (descending so index 0 = highest)
                    for (int cnt = 9; cnt >= 0; cnt--)
                    {
                        string octPath = LOCATION_COMPANYDATA + "DEF" + cnt.ToString("D2") + ".OCT";
                        if (File.Exists(octPath))
                        {
                            using (var fsOct = new FileStream(octPath, FileMode.Open, FileAccess.Read))
                            {
                                fsOct.Seek(0L, SeekOrigin.Begin);
                                fsOct.Read(buf16, 0, 16);
                            }
                            string xx = Encoding.Default.GetString(buf16, 0, 4).TrimEnd('\0', ' '); // Left(A, 4)
                            boekjaarItems.Insert(0, xx); // AddItem xx, 0  → insert at front
                        }
                    }

                    // Get record 3: active period index
                    fs9.Seek(8L, SeekOrigin.Begin);
                    fs9.Read(buf4, 0, 4);
                    aaStr = Encoding.Default.GetString(buf4).TrimEnd('\0', ' ');
                    int.TryParse(aaStr, out aktievePeriode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij lezen 9999.OCT:\r\n" + ex.Message);
                return;
            }

            string activeBoekjaarText = boekjaarItems.Count > ACTIVE_BOOKYEAR
                ? boekjaarItems[ACTIVE_BOOKYEAR]
                : string.Empty;
            bstNaam[TABLE_COUNTERS] = "jr" + activeBoekjaarText;

            // ---- Read DEFxx.OCT for period list ----
            string defOctPath = LOCATION_COMPANYDATA
                + "DEF" + ACTIVE_BOOKYEAR.ToString("D2") + ".OCT";
            var periodeItems = new System.Collections.Generic.List<string>();
            string xxPeriod = string.Empty;

            try
            {
                using (var fsDef = new FileStream(defOctPath, FileMode.Open, FileAccess.Read))
                {
                    for (int t = 1; t <= 99; t++)
                    {
                        fsDef.Seek((t - 1) * 16L, SeekOrigin.Begin);
                        int read = fsDef.Read(buf16, 0, 16);
                        if (read < 16)
                            break;

                        string a = Encoding.Default.GetString(buf16);
                        if (a == new string(' ', 16))
                        {
                            // Space record: set BOOKYEAR_FROMTO from first period item
                            string yy = periodeItems.Count > 0 ? periodeItems[0] : string.Empty;
                            // AT = first period text; BOOKYEAR_FROMTO uses XX (last valid a read before space)
                            BOOKYEAR_FROMTO =
                                SafeMid(yy, 7, 4) + SafeMid(yy, 4, 2) + SafeMid(yy, 1, 2)
                                + SafeMid(xxPeriod, 20, 4) + SafeMid(xxPeriod, 17, 2) + SafeMid(xxPeriod, 14, 2);
                            break;
                        }
                        else
                        {
                            // Format: YYYY(0-3) MM(4-5) DD(6-7)  YYYY(8-11) MM(12-13) DD(14-15)
                            string item =
                                a.Substring(6, 2) + "/" + a.Substring(4, 2) + "/" + a.Substring(0, 4)
                                + " - "
                                + a.Substring(14, 2) + "/" + a.Substring(12, 2) + "/" + a.Substring(8, 4);
                            periodeItems.Add(item);
                            xxPeriod = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij lezen periode OCT:\r\n" + ex.Message);
                return;
            }

            if (aktievePeriode - 1 > periodeItems.Count)
            {
                MessageBox.Show(
                    "Het hoogste boekjaar wordt automatisch ingeladen.  " +
                    "Laatste bewerking gebeurde in een boekjaar met meer periodes dan nu mogelijk.  " +
                    "De eerste periode van het hoogste boekjaar wordt hierna automatisch geaktiveerd");
                aktievePeriode = 1;
            }

            int periodeIndex = Math.Max(0, aktievePeriode - 1);
            string at = periodeIndex < periodeItems.Count ? periodeItems[periodeIndex] : string.Empty;
            PERIOD_FROMTO =
                SafeMid(at, 7, 4) + SafeMid(at, 4, 2) + SafeLeft(at, 2)
                + SafeRight(at, 4) + SafeMid(at, 17, 2) + SafeMid(at, 14, 2);

            FormBYPERDAT formBYPERDAT = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FormBYPERDAT byp) { formBYPERDAT = byp; break; }
            }
            if (formBYPERDAT != null)
            {
                formBYPERDAT.DatumVerwerking.Value = DateTime.Now;

                // Suppress SelectedIndexChanged events while populating combos:
                // adntDB is not yet open at this point, so any event-driven call to
                // String99 → BGet → BOpen would fail with a COMException.
                formBYPERDAT.CmbBoekjaar.SelectedIndexChanged -= formBYPERDAT.CmbBoekjaar_SelectedIndexChanged;
                formBYPERDAT.CmbPeriodeBoekjaar.SelectedIndexChanged -= formBYPERDAT.CmbPeriodeBoekjaar_SelectedIndexChanged;
                try
                {
                    formBYPERDAT.CmbBoekjaar.Items.Clear();
                    foreach (string bj in boekjaarItems)
                        formBYPERDAT.CmbBoekjaar.Items.Add(bj);
                    if (boekjaarItems.Count > 0)
                        formBYPERDAT.CmbBoekjaar.SelectedIndex = Math.Min(ACTIVE_BOOKYEAR, boekjaarItems.Count - 1);

                    formBYPERDAT.CmbPeriodeBoekjaar.Items.Clear();
                    foreach (string p in periodeItems)
                        formBYPERDAT.CmbPeriodeBoekjaar.Items.Add(p);
                    if (periodeItems.Count > 0)
                        formBYPERDAT.CmbPeriodeBoekjaar.SelectedIndex = periodeIndex;
                }
                finally
                {
                    formBYPERDAT.CmbBoekjaar.SelectedIndexChanged += formBYPERDAT.CmbBoekjaar_SelectedIndexChanged;
                    formBYPERDAT.CmbPeriodeBoekjaar.SelectedIndexChanged += formBYPERDAT.CmbPeriodeBoekjaar_SelectedIndexChanged;
                }

                formBYPERDAT.Enabled = true;
                formBYPERDAT.WindowState = FormWindowState.Minimized;
                formBYPERDAT.Show();
            }

            XisEuroWisBEF = false;

            // Enable BasisB[] forms created in FormMim_Load()
            for (int t = 1; t <= 3; t++)
            {
                if (BasisB[t] != null)
                {
                    BasisB[t].Enabled = true;
                    BasisB[t].WindowState = FormWindowState.Minimized;
                }
            }

            // Enable shortcut keys for persistent forms
            if (Application.OpenForms["FormMim"] is FormMim mimPersistent)
                mimPersistent.SetPersistentFormsEnabled(true);

            // ---- Open ADO connections ----
            adntDB = new ADODB.Connection();
            adntDBSQLS = new ADODB.Connection();

            // TODO: if (Mim.Instellingen(4).Checked) — SQL Server path not yet ported
            // Jet / MDV path:
            bool updateLengths20250809 = false;
            bool updateLengthsBis20250809 = false;
            bool updateLengths20251025 = false;

            string marntPath = LOCATION_COMPANYDATA + LOCATION_NETDATA + "Marnt.MDV";
            if (File.Exists(marntPath))
            {
                // TabelKontrole();

                BAModus = 1;
                if (Application.OpenForms["FormMim"] is FormMim mimCursor)
                    mimCursor.Cursor = Cursors.WaitCursor;

                oleDbConnect = OLEDBJET_PROVIDER +
                    LOCATION_COMPANYDATA + "marnt.mdv";

                jetConnect = ADOJET_PROVIDER
                    + "Data Source=" + LOCATION_COMPANYDATA + LOCATION_NETDATA
                    + @"\marnt.mdv;"
                    + "Persist Security Info=False";


                try
                {
                    SnelHelpPrint("Database verbinding maken via MS-Jet", BL_LOGGING);
                    adntDB.Open(jetConnect);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij openen database:\r\n" + ex.Message);
                    AutoUnLoadCompany();
                    return;
                }

                // Field-length upgrade checks — using ADODB schema rowsets, no DAO
                try
                {
                    updateLengths20250809 = GetColumnSize(adntDB, "Klanten", "A100") < 50;
                    if (updateLengths20250809)
                        MessageBox.Show(
                            "Een reeks velden rond naam en adres zullen worden uitgebreid " +
                            "in het klanten- en leveranciersbestand",
                            string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    updateLengthsBis20250809 = GetColumnSize(adntDB, "Klanten", "A125") < 50;

                    updateLengths20251025 = GetColumnSize(adntDB, "Dokumenten", "v039") < 35;
                    if (updateLengths20251025)
                        MessageBox.Show(
                            "Een reeks velden rond betaalreferte zullen worden uitgebreid " +
                            "in het dokumentenbestand",
                            string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { }

                // Extend v217 in all jr* journal tables — using ADODB schema rowsets, no DAO
                try
                {
                    // Enumerate all user tables via adSchemaTables
                    ADODB.Recordset rsTables = adntDB.OpenSchema(
                        ADODB.SchemaEnum.adSchemaTables,
                        new object[] { null, null, null, "TABLE" },
                        Type.Missing);

                    var jrTables = new System.Collections.Generic.List<string>();
                    while (!rsTables.EOF)
                    {
                        string tName = rsTables.Fields["TABLE_NAME"].Value?.ToString() ?? string.Empty;
                        if (tName.StartsWith("jr", StringComparison.OrdinalIgnoreCase))
                            jrTables.Add(tName);
                        rsTables.MoveNext();
                    }
                    rsTables.Close();

                    foreach (string tName in jrTables)
                    {
                        if (GetColumnSize(adntDB, tName, "v217") < 50)
                        {
                            BClose(TABLE_COUNTERS);
                            try
                            {
                                string alterSql = "ALTER TABLE " + tName + " ALTER COLUMN v217 TEXT(50)";
                                object rAffected1 = Type.Missing;
                                adntDB.Execute(alterSql, out rAffected1, (int)ADODB.CommandTypeEnum.adCmdText);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(
                                    "Foutmelding bron: " + ex.Source + "\r\n" +
                                    "Foutmelding omschrijving:\r\n" + ex.Message);
                                Application.Exit();
                                return;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch { }

                if (!InitBestanden())
                {
                    MessageBox.Show("(Laat de) DATABASE KONTROLEREN !!!");
                    AutoUnLoadCompany();
                    return;
                }


            }

            // ---- Check for new fields needed ----
            int ktrlfl = 0;
            int ptel = 0;
            for (ktrlfl = TABLE_CUSTOMERS; ktrlfl <= TABLE_JOURNAL; ktrlfl++)
            {
                ptel = 0;
                BClose(ktrlfl);
                BOpen(ktrlfl);
                while (vBC[ktrlfl, ptel] != null &&
                       vBC[ktrlfl, ptel].Length > 0 &&
                       vBC[ktrlfl, ptel][0] != '\0')
                {
                    string fieldName = vBC[ktrlfl, ptel];
                    try
                    {
                        string dummy2 = rsMAR[ktrlfl].Fields[fieldName].Name;
                    }
                    catch
                    {
                        BClose(ktrlfl);
                        if (AdxKolom(bstNaam[ktrlfl], fieldName, 202 /* adVarWChar */, 50))
                            MessageBox.Show("Extra veld : " + fieldName + " met succes bijgevoegd...");
                        else
                            MessageBox.Show("Extra veld : " + fieldName + " NIET MET SUCCES bijgevoegd...");
                        BOpen(ktrlfl);
                    }
                    ptel++;
                }
            }

            // ---- Peppol validations ----            
            string tempoVar = (String99(292) ?? "").Trim();
            string tempovar2 = (String99(51) ?? "").Trim();
            if (!(tempoVar == tempovar2 && tempoVar.Length + tempovar2.Length == 20))
                MessageBox.Show("Breng SPOEDIG Setup Bedrijfsinformatie ...", "Ontbrekende bedrijfsinfo voor Peppol");
            if (!IsValidEmail((String99(295) ?? "").Trim()))
                MessageBox.Show("Mailadres van uw onderneming is ongeldig.", "Ontbrekende bedrijfsinfo voor Peppol");

            // ---- Schema ALTER TABLE upgrades ----
            if (updateLengths20251025)
            {
                BClose(TABLE_INVOICES);
                try
                {
                    object rAff039 = Type.Missing;
                    adntDB.Execute("ALTER TABLE Dokumenten ALTER COLUMN v039 TEXT(35)", out rAff039, (int)ADODB.CommandTypeEnum.adCmdText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Foutmelding omschrijving:\r\n" + ex.Message);
                    Application.Exit();
                    return;
                }
            }

            // ---- Post-load UI / config ----
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.ToolStripLabel1.Text = (FMarBoxText("899", "2",
                    (String99(20) ?? string.Empty).Substring(
                        0, Math.Min(1, (String99(20) ?? string.Empty).Length)))
                    ?? string.Empty) is string _bt && _bt.Length >= 4 ? _bt.Substring(3) : string.Empty;

                mim.ToolStripLabel2.Text = String99(296);
                mim.ToolStripLabel3.Text = "JET4";
                mim.SystemSubMenu.Enabled = true;
                mim.FilesSubMenu.Enabled = true;
                mim.DailyManagementSubMenu.Enabled = true;
                mim.AccountingSubMenu.Enabled = true;
                mim.ContractsSubMenu.Enabled = true;
                mim.MenuActiesCloseCompany.Enabled = true;
                mim.ToolStripBookingDate.Text = MIM_GLOBAL_DATE;
            }

            // TODO: ProducentNummer / VsoftLog Select Case String99(READING,20)
            // (requires String99 to be ported)

            bhEuro = true;

            // ---- Ensure company subdirectories exist ----
            EnsureSubDirectory(LOCATION_COMPANYDATA + "coda");
            EnsureSubDirectory(LOCATION_COMPANYDATA + @"coda\in");
            EnsureSubDirectory(LOCATION_COMPANYDATA + @"coda\out");

            EnsureSubDirectory(LOCATION_COMPANYDATA + "peppol");
            EnsureSubDirectory(LOCATION_COMPANYDATA + @"peppol\in");
            EnsureSubDirectory(LOCATION_COMPANYDATA + @"peppol\out");

            EnsureSubDirectory(LOCATION_COMPANYDATA + "BMP-qr");
            EnsureSubDirectory(LOCATION_COMPANYDATA + "vpeSjbs");
            EnsureSubDirectory(LOCATION_COMPANYDATA + "xlsx-templates");
            EnsureSubDirectory(LOCATION_COMPANYDATA + "vat");

            var repo = new PeppolDocumentRepository(LOCATION_COMPANYDATA + "marnt.mdv");
            repo.EnsurePeppolTableExists(LOCATION_COMPANYDATA + "marnt.mdv");

            // ---- Open journal recordset ----
            BOpen(TABLE_JOURNAL);
            if (usrLicentieInfo?.StartsWith("DemoModus") == true)
            {
                try
                {
                    JournaalLocked = rsMAR[TABLE_JOURNAL].RecordCount > 50;
                }
                catch { }
            }

            for (int countTo = 22; countTo <= 31; countTo++)
                MdvDataTools.VeldOK(TABLE_LEDGERACCOUNTS, "dece" + countTo.ToString("000"), "CURRENCY");

            string destPath = LOCATION_COMPANYDATA + "sjb";
            if (!File.Exists(destPath + "\\sBrief.doc"))
            {
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);
            }

            rsJournaal = new ADODB.Recordset();
            try
            {
                dynamic ntDbDyn3 = ntDB;
                string ntConnect3 = ntDbDyn3?.Connect ?? string.Empty;
                rsJournaal.CursorLocation = !string.IsNullOrEmpty(ntConnect3)
                    ? ADODB.CursorLocationEnum.adUseClient
                    : ADODB.CursorLocationEnum.adUseServer;
                rsJournaal.Open(
                    "SELECT TOP 1 * FROM Journalen",
                    adntDB,
                    ADODB.CursorTypeEnum.adOpenForwardOnly,
                    ADODB.LockTypeEnum.adLockOptimistic,
                    (int)ADODB.CommandTypeEnum.adCmdText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij openen Journalen:\r\n" + ex.Message);
            }

            Cijfermaskers();

            // VB6: If Dir(LOCATION_COMPANYDATA + "DDEF*.txt") = "" Then ...
            // Copy Dok*.txt files to DDEFxx0..5.TXT if no DDEF*.txt files exist yet.
            if (!Directory.EnumerateFiles(LOCATION_COMPANYDATA, "DDEF*.txt").GetEnumerator().MoveNext())
            {
                foreach (string varPath in Directory.EnumerateFiles(LOCATION_COMPANYDATA, "Dok*.txt"))
                {
                    string varString = Path.GetFileName(varPath);
                    // VB6: Mid(VarString, 4, 1) = "2" → skip
                    if (varString.Length >= 4 && varString[3] == '2')
                        continue;

                    string vanString = varPath;
                    for (COUNT_TO = 0; COUNT_TO <= 5; COUNT_TO++)
                    {
                        // VB6: Mid(VarString, 4, 2) extracts 2 chars at position 4 (1-based)
                        string mid42 = varString.Length >= 5 ? varString.Substring(3, 2) : varString.Substring(3);
                        string naarString = LOCATION_COMPANYDATA + "DDEF" + mid42 + COUNT_TO + ".TXT";
                        try { File.Copy(vanString, naarString, overwrite: false); } catch { }
                    }
                }
            }

            // VB6: If Dir(LOCATION_COMPANYDATA + "DDEF125.TXT") = "" Then ...
            // Create DDEF1x5.TXT copies from DDEF1x0.TXT for x = 1..4.
            if (!File.Exists(LOCATION_COMPANYDATA + "DDEF125.TXT"))
            {
                for (COUNT_TO = 1; COUNT_TO <= 4; COUNT_TO++)
                {
                    string vanString = LOCATION_COMPANYDATA + "DDEF1" + COUNT_TO + "0.TXT";
                    string naarString = LOCATION_COMPANYDATA + "DDEF1" + COUNT_TO + "5.TXT";
                    try { File.Copy(vanString, naarString, overwrite: false); } catch { }
                }
            }

            if (Application.OpenForms["FormMim"] is FormMim mimEnd)
                mimEnd.Cursor = Cursors.Default;
        }
        /// <summary>
        /// VB6: Function TeleBibPagina(Fl As Integer) As Boolean
        /// Loads TELEBIB definition fields from .Def files into the TELEBIB_* arrays.
        /// Priority: user-def (xxxU.Def) > base-def (xxx.Def) + optional makelaar-ext (xxxM.Def).
        /// Returns True on success, False on error.
        /// </summary>
        internal static bool TeleBibPagina(int fl)
        {
            int t = 0;

            string lokaalBestand = fl != TABLE_COUNTERS
                ? TABLEDEF_ONT[fl].Substring(0, 3)
                : "00";

            string defDir = PROGRAM_LOCATION + "Content\\Def\\";

            if (!File.Exists(defDir + lokaalBestand + ".Def"))
            {
                MessageBox.Show("Geen VsoftBib definitie " + defDir + lokaalBestand + ".Def");
                return false;
            }

            try
            {
                // --- User preference file (xxxU.Def) ---
                string userDefPath = defDir + lokaalBestand + "U.Def";
                if (File.Exists(userDefPath))
                {
                    t = 0;
                    foreach (string line in File.ReadLines(userDefPath, Encoding.Default))
                    {
                        if (!ParseTelebibLine(line, t, out string code, out string text, out string type, out int length))
                            continue;
                        TELEBIB_CODE[t] = code;
                        TELEBIB_TEXT[t] = text;
                        TELEBIB_TYPE[t] = type;
                        TELEBIB_LENGTH[t] = length;
                        vBC[fl, t] = SafeMid(TELEBIB_CODE[t], 5, 4);
                        if (TELEBIB_TYPE[t] == "D" && DecimalKTRL)
                            JumpToTheBEAT(fl, t);
                        t++;
                    }
                    TELEBIB_CODE[t] = "";
                    TELEBIB_LAST = t - 1;
                    return true;
                }

                // --- Base definition file (xxx.Def) ---
                string baseDefPath = defDir + lokaalBestand + ".Def";
                t = 0;
                foreach (string line in File.ReadLines(baseDefPath, Encoding.Default))
                {
                    if (!ParseTelebibLine(line, t, out string code, out string text, out string type, out int length))
                        continue;
                    TELEBIB_CODE[t] = code;
                    TELEBIB_TEXT[t] = text;
                    TELEBIB_TYPE[t] = type;
                    TELEBIB_LENGTH[t] = length;
                    vBC[fl, t] = SafeMid(TELEBIB_CODE[t], 5, 4);
                    if (TELEBIB_TYPE[t] == "D" && DecimalKTRL)
                        JumpToTheBEAT(fl, t);
                    t++;
                }
                TELEBIB_LAST = t - 1;
                TELEBIB_CODE[t] = "";

                // --- Makelaar extension file (xxxM.Def) ---
                string makelaarDefPath = defDir + lokaalBestand + "M.Def";
                if (ProducentNummer != new string(' ', 8) && File.Exists(makelaarDefPath))
                {
                    foreach (string line in File.ReadLines(makelaarDefPath, Encoding.Default))
                    {
                        if (!ParseTelebibLine(line, t, out string code, out string text, out string type, out int length))
                            continue;
                        TELEBIB_CODE[t] = code;
                        TELEBIB_TEXT[t] = text;
                        TELEBIB_TYPE[t] = type;
                        TELEBIB_LENGTH[t] = length;
                        vBC[fl, t] = SafeMid(TELEBIB_CODE[t], 5, 4);
                        if (TELEBIB_TYPE[t] == "D" && DecimalKTRL)
                            JumpToTheBEAT(fl, t);
                        t++;
                    }
                    TELEBIB_LAST = t - 1;
                    TELEBIB_CODE[t] = "";
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Telebibinlaadfout" + t + " error:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// VB6 GoSub JumpToTheBEAT — checks for a decimal column in the recordset and adds it if missing,
        /// then transfers values from the text field to the decimal field.
        /// </summary>
        private static void JumpToTheBEAT(int fl, int t)
        {
            string decField = "dec" + vBC[fl, t];
            try
            {
                // Try to access the field — if it exists, no action needed
                string _ = rsMAR[fl].Fields[decField].Name;
                DecimalKTRL = false;
            }
            catch
            {
                BClose(fl);
                if (AdxKolom(bstNaam[fl], decField, 6 /* adCurrency */, 0))
                {
                    SnelHelpPrint("Extra SQL Server compatibel Decimal veld : " + decField
                        + " met succes bijgevoegd in tabel : " + bstNaam[fl], BL_LOGGING);
                    Msg = "UPDATE " + bstNaam[fl] + " SET " + decField + "=val(" + vBC[fl, t] + ")";
                    SnelHelpPrint("Cijfers van " + vBC[fl, t] + " worden overgedragen naar " + decField + Msg, BL_LOGGING);
                    DoeDeUpdate(fl, t, decField);
                }
            }
        }

        /// <summary>
        /// VB6 GoSub DoeDeUpdate — iterates all records and copies the text field value
        /// to the corresponding decimal field.
        /// </summary>
        private static void DoeDeUpdate(int fl, int t, string decField)
        {
            BFirst(fl, 0);
            while (Ktrl == 0)
            {
                decimal curBedrag = 0;
                try
                {
                    object raw = rsMAR[fl].Fields[vBC[fl, t]].Value;
                    if (raw != null && raw != DBNull.Value)
                        curBedrag = Convert.ToDecimal(raw, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    SnelHelpPrint(ex.Message + " ter hoogte van "
                        + rsMAR[fl].Fields[0].Value + " in bestand " + bstNaam[fl], BL_LOGGING);
                }
                rsMAR[fl].Fields[decField].Value = curBedrag;
                SnelHelpPrint(rsMAR[fl].Fields[0].Value + " " + curBedrag, BL_LOGGING);
                rsMAR[fl].Update();
                BNext(fl);
            }
        }

        /// <summary>
        /// Parses a single VB6-style CSV line (as produced by Input #) into the four TELEBIB fields.
        /// Returns false when the line cannot be parsed.
        /// </summary>
        internal static bool ParseTelebibLine(string line, int t,
            out string code, out string text, out string type, out int length)
        {
            code = text = type = string.Empty;
            length = 0;

            if (string.IsNullOrWhiteSpace(line))
                return false;

            // VB6 Input # writes quoted strings separated by commas: "code","text","type",length
            string[] parts = line.Split(',');
            if (parts.Length < 4)
                return false;

            code = parts[0].Trim().Trim('"');
            text = parts[1].Trim().Trim('"');
            type = parts[2].Trim().Trim('"');
            int.TryParse(parts[3].Trim().Trim('"'), out length);
            return true;
        }

        public static void InitFirst()
        {
            // FULL_LINE = String$(128, 173)
            FULL_LINE = new string((char)173, 128);

            TABLEDEF_ONT[TABLE_VARIOUS] = "0000000.ONT";        //00
            TABLEDEF_ONT[TABLE_CUSTOMERS] = "0010000.ONT";      //01
            TABLEDEF_ONT[TABLE_SUPPLIERS] = "0020000.ONT";      //02
            TABLEDEF_ONT[TABLE_LEDGERACCOUNTS] = "0030000.ONT"; //03
            TABLEDEF_ONT[TABLE_PRODUCTS] = "0040000.ONT";       //04
            TABLEDEF_ONT[TABLE_JOURNAL] = "0600000.ONT";        //05
            TABLEDEF_ONT[TABLE_INVOICES] = "0200000.ONT";       //06
            TABLEDEF_ONT[TABLE_CONTRACTS] = "0700000.ONT";      //07
            TABLEDEF_ONT[TABLE_DUMMY] = "90DUMMY.ONT";          //08
            TABLEDEF_ONT[TABLE_COUNTERS] = "00.ONT";            //09

            bstNaam[TABLE_VARIOUS] = "Allerlei";
            bstNaam[TABLE_CUSTOMERS] = "Klanten";
            bstNaam[TABLE_SUPPLIERS] = "Leveranciers";
            bstNaam[TABLE_LEDGERACCOUNTS] = "Rekeningen";
            bstNaam[TABLE_PRODUCTS] = "Produkten";
            bstNaam[TABLE_JOURNAL] = "Journalen";
            bstNaam[TABLE_INVOICES] = "dokumenten";
            bstNaam[TABLE_CONTRACTS] = "Polissen";
            bstNaam[TABLE_DUMMY] = "TmpBestand";
            bstNaam[TABLE_COUNTERS] = "Tell";

            DAYS_IN_MONTH[1] = 31;
            DAYS_IN_MONTH[2] = 29; // VB6 used 29 for February leap-safe
            DAYS_IN_MONTH[3] = 31;
            DAYS_IN_MONTH[4] = 30;
            DAYS_IN_MONTH[5] = 31;
            DAYS_IN_MONTH[6] = 30;
            DAYS_IN_MONTH[7] = 31;
            DAYS_IN_MONTH[8] = 31;
            DAYS_IN_MONTH[9] = 30;
            DAYS_IN_MONTH[10] = 31;
            DAYS_IN_MONTH[11] = 30;
            DAYS_IN_MONTH[12] = 31;

            MONTH_AS_TEXT[1] = "Januari  ";
            MONTH_AS_TEXT[2] = "Februari ";
            MONTH_AS_TEXT[3] = "Maart    ";
            MONTH_AS_TEXT[4] = "April    ";
            MONTH_AS_TEXT[5] = "Mei      ";
            MONTH_AS_TEXT[6] = "Juni     ";
            MONTH_AS_TEXT[7] = "Juli     ";
            MONTH_AS_TEXT[8] = "Augustus ";
            MONTH_AS_TEXT[9] = "September";
            MONTH_AS_TEXT[10] = "October  ";
            MONTH_AS_TEXT[11] = "November ";
            MONTH_AS_TEXT[12] = "December ";
        }

        /// <summary>        
        /// Initialises table/index definitions and verifies database indexes.
        /// </summary>
        public static bool InitBestanden()
        {
            bool result = true;

            // TABLE_VARIOUS
            FL_NUMBEROFINDEXEN[TABLE_VARIOUS] = 1;
            JETTABLEUSE_INDEX[TABLE_VARIOUS, 0] = "v004 "; FLINDEX_LEN[TABLE_VARIOUS, 0] = 13; FLINDEX_CAPTION[TABLE_VARIOUS, 0] = "Partij";
            JETTABLEUSE_INDEX[TABLE_VARIOUS, 1] = "v005 "; FLINDEX_LEN[TABLE_VARIOUS, 1] = 20; FLINDEX_CAPTION[TABLE_VARIOUS, 1] = "SPtype";

            // TABLE_CUSTOMERS
            FL_NUMBEROFINDEXEN[TABLE_CUSTOMERS] = 1;
            JETTABLEUSE_INDEX[TABLE_CUSTOMERS, 0] = "A110 "; FLINDEX_LEN[TABLE_CUSTOMERS, 0] = 12; FLINDEX_CAPTION[TABLE_CUSTOMERS, 0] = "Nummer";
            JETTABLEUSE_INDEX[TABLE_CUSTOMERS, 1] = "A100 "; FLINDEX_LEN[TABLE_CUSTOMERS, 1] = 10; FLINDEX_CAPTION[TABLE_CUSTOMERS, 1] = "Bedrijfsnaam";

            // TABLE_SUPPLIERS
            FL_NUMBEROFINDEXEN[TABLE_SUPPLIERS] = 1;
            JETTABLEUSE_INDEX[TABLE_SUPPLIERS, 0] = "A110 "; FLINDEX_LEN[TABLE_SUPPLIERS, 0] = 12; FLINDEX_CAPTION[TABLE_SUPPLIERS, 0] = "Nummer";
            JETTABLEUSE_INDEX[TABLE_SUPPLIERS, 1] = "A100 "; FLINDEX_LEN[TABLE_SUPPLIERS, 1] = 10; FLINDEX_CAPTION[TABLE_SUPPLIERS, 1] = "Bedrijfsnaam";

            // TABLE_LEDGERACCOUNTS
            FL_NUMBEROFINDEXEN[TABLE_LEDGERACCOUNTS] = 1;
            JETTABLEUSE_INDEX[TABLE_LEDGERACCOUNTS, 0] = "v019 "; FLINDEX_LEN[TABLE_LEDGERACCOUNTS, 0] = 7; FLINDEX_CAPTION[TABLE_LEDGERACCOUNTS, 0] = "RekeningNummer";
            JETTABLEUSE_INDEX[TABLE_LEDGERACCOUNTS, 1] = "v020 "; FLINDEX_LEN[TABLE_LEDGERACCOUNTS, 1] = 10; FLINDEX_CAPTION[TABLE_LEDGERACCOUNTS, 1] = "Omschrijving";

            // TABLE_PRODUCTS
            FL_NUMBEROFINDEXEN[TABLE_PRODUCTS] = 1;
            JETTABLEUSE_INDEX[TABLE_PRODUCTS, 0] = "v102 "; FLINDEX_LEN[TABLE_PRODUCTS, 0] = 13; FLINDEX_CAPTION[TABLE_PRODUCTS, 0] = "Artikelkode EAN";
            JETTABLEUSE_INDEX[TABLE_PRODUCTS, 1] = "v105 "; FLINDEX_LEN[TABLE_PRODUCTS, 1] = 10; FLINDEX_CAPTION[TABLE_PRODUCTS, 1] = "Omschrijving";

            // TABLE_JOURNAL
            FL_NUMBEROFINDEXEN[TABLE_JOURNAL] = 4;
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 0] = "v070 "; FLINDEX_LEN[TABLE_JOURNAL, 0] = 15; FLINDEX_CAPTION[TABLE_JOURNAL, 0] = "Rekening Boekdatum";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 1] = "v033 "; FLINDEX_LEN[TABLE_JOURNAL, 1] = 11; FLINDEX_CAPTION[TABLE_JOURNAL, 1] = "Dokumentnummer";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 2] = "v038 "; FLINDEX_LEN[TABLE_JOURNAL, 2] = 8; FLINDEX_CAPTION[TABLE_JOURNAL, 2] = "Betalingsstuk";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 3] = "v041 "; FLINDEX_LEN[TABLE_JOURNAL, 3] = 1; FLINDEX_CAPTION[TABLE_JOURNAL, 3] = "Bewerkingsvlag";
            JETTABLEUSE_INDEX[TABLE_JOURNAL, 4] = "v066 "; FLINDEX_LEN[TABLE_JOURNAL, 4] = 7; FLINDEX_CAPTION[TABLE_JOURNAL, 4] = "Boekdatum";

            // TABLE_INVOICES
            FL_NUMBEROFINDEXEN[TABLE_INVOICES] = 2;
            JETTABLEUSE_INDEX[TABLE_INVOICES, 0] = "v033 "; FLINDEX_LEN[TABLE_INVOICES, 0] = 11; FLINDEX_CAPTION[TABLE_INVOICES, 0] = "DokumentNummer";
            JETTABLEUSE_INDEX[TABLE_INVOICES, 1] = "v034 "; FLINDEX_LEN[TABLE_INVOICES, 1] = 13; FLINDEX_CAPTION[TABLE_INVOICES, 1] = "Partij";
            JETTABLEUSE_INDEX[TABLE_INVOICES, 2] = "A000 "; FLINDEX_LEN[TABLE_INVOICES, 2] = 12; FLINDEX_CAPTION[TABLE_INVOICES, 2] = "KontraktNummer";

            // TABLE_CONTRACTS
            FL_NUMBEROFINDEXEN[TABLE_CONTRACTS] = 3;
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 0] = "A000 "; FLINDEX_LEN[TABLE_CONTRACTS, 0] = 12; FLINDEX_CAPTION[TABLE_CONTRACTS, 0] = "Polisnummer";
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 1] = "A110 "; FLINDEX_LEN[TABLE_CONTRACTS, 1] = 12; FLINDEX_CAPTION[TABLE_CONTRACTS, 1] = "Klantkode";
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 2] = "A010 "; FLINDEX_LEN[TABLE_CONTRACTS, 2] = 4; FLINDEX_CAPTION[TABLE_CONTRACTS, 2] = "Maatschappij";
            JETTABLEUSE_INDEX[TABLE_CONTRACTS, 3] = "v167 "; FLINDEX_LEN[TABLE_CONTRACTS, 3] = 30; FLINDEX_CAPTION[TABLE_CONTRACTS, 3] = "MaandKlantMijPolis";

            // TABLE_COUNTERS
            FL_NUMBEROFINDEXEN[TABLE_COUNTERS] = 0;
            JETTABLEUSE_INDEX[TABLE_COUNTERS, 0] = "v071 "; FLINDEX_LEN[TABLE_COUNTERS, 0] = 5; FLINDEX_CAPTION[TABLE_COUNTERS, 0] = "Setup Parameter";

            // TABLE_DUMMY
            FL_NUMBEROFINDEXEN[TABLE_DUMMY] = 0;
            JETTABLEUSE_INDEX[TABLE_DUMMY, 0] = "v089 "; FLINDEX_LEN[TABLE_DUMMY, 0] = 20; FLINDEX_CAPTION[TABLE_DUMMY, 0] = "Plaatselijk sorteren";

            for (int t = TABLE_VARIOUS; t <= TABLE_COUNTERS; t++)
            {
                BClose(t);
                BOpen(t);
                if (!TeleBibPagina(t))
                {
                    MessageBox.Show("Fout tijdens inladen bestandsdefinities.  Herinstalleer het programma en/of contacteer Vsoft");
                    result = false;
                }

                if (t == TABLE_VARIOUS || t == TABLE_COUNTERS)
                    continue;

                // Fase 1: build semicolon-separated list of current database index names
                // Uses ADODB schema rowset — no DAO required.
                // adSchemaIndexes restrictions: TABLE_CATALOG, TABLE_SCHEMA, INDEX_NAME, TYPE, TABLE_NAME
                string aa = "";
                var seenIndexNames = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
                ADODB.Recordset rsIdx = adntDB.OpenSchema(
                    ADODB.SchemaEnum.adSchemaIndexes,
                    new object[] { null, null, null, null, bstNaam[t] },
                    Type.Missing);
                while (!rsIdx.EOF)
                {
                    string idxName = rsIdx.Fields["INDEX_NAME"].Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(idxName) && seenIndexNames.Add(idxName))
                        aa += idxName + ";";
                    rsIdx.MoveNext();
                }
                rsIdx.Close();

                // Fase 2: verify each standard index is present
                for (int tt = 0; tt <= FL_NUMBEROFINDEXEN[t]; tt++)
                {
                    string caption = FLINDEX_CAPTION[t, tt];
                    int plTT = aa.IndexOf(caption, StringComparison.Ordinal) + 1; // 1-based like VB6 InStr
                    if (plTT > 0)
                    {
                        if (plTT == 1)
                            aa = aa.Substring(caption.Length + 1);
                        else
                            aa = aa.Substring(0, plTT - 1) + aa.Substring(plTT - 1 + caption.Length + 1);
                    }
                    else if (caption != "Boekdatum")
                    {
                        MessageBox.Show("Index '" + caption + "' van tabel '" + bstNaam[t] + "' bestaat niet meer !!!");
                        result = false;
                    }
                }

                // Extra user-added indexes
                if (aa != "")
                {
                    while (aa != "")
                    {
                        FL_NUMBEROFINDEXEN[t]++;
                        int semi = aa.IndexOf(';');
                        FLINDEX_CAPTION[t, FL_NUMBEROFINDEXEN[t]] = aa.Substring(0, semi);
                        string extraCaption = FLINDEX_CAPTION[t, FL_NUMBEROFINDEXEN[t]];

                        // Get columns of this index via ADODB schema rowset
                        // adSchemaIndexes restrictions: TABLE_CATALOG, TABLE_SCHEMA, INDEX_NAME, TYPE, TABLE_NAME
                        var idxColumns = new System.Collections.Generic.List<string>();
                        ADODB.Recordset rsIdxCols = adntDB.OpenSchema(
                            ADODB.SchemaEnum.adSchemaIndexes,
                            new object[] { null, null, extraCaption, null, bstNaam[t] },
                            Type.Missing);
                        while (!rsIdxCols.EOF)
                        {
                            string colName = rsIdxCols.Fields["COLUMN_NAME"].Value?.ToString() ?? string.Empty;
                            if (!string.IsNullOrEmpty(colName))
                                idxColumns.Add(colName);
                            rsIdxCols.MoveNext();
                        }
                        rsIdxCols.Close();

                        int fieldCount = idxColumns.Count;
                        if (fieldCount > 1)
                        {
                            string fieldNames = idxColumns[0];
                            for (int ttt = 1; ttt < fieldCount; ttt++)
                                fieldNames += "+" + idxColumns[ttt];
                            JETTABLEUSE_INDEX[t, FL_NUMBEROFINDEXEN[t]] = fieldNames;
                            MessageBox.Show("Index " + extraCaption + " van tabel " + bstNaam[t] +
                                " is samengesteld uit meerdere velden...\r\n" +
                                "Deze index enkel te gebruiken voor lijsten van " + bstNaam[t] +
                                ".  Bij ge\u00EBndexeerd zoeken wordt enkel het eerste veld opgenomen in het rooster.");
                            FLINDEX_LEN[t, FL_NUMBEROFINDEXEN[t]] = 0;
                        }
                        else
                        {
                            string firstName = idxColumns.Count > 0 ? idxColumns[0] : string.Empty;
                            JETTABLEUSE_INDEX[t, FL_NUMBEROFINDEXEN[t]] = firstName;

                            // Get field size via adSchemaColumns
                            // adSchemaColumns restrictions: TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME
                            int fieldSize = 0;
                            ADODB.Recordset rsCol = adntDB.OpenSchema(
                                ADODB.SchemaEnum.adSchemaColumns,
                                new object[] { null, null, bstNaam[t], firstName.TrimEnd() },
                                Type.Missing);
                            if (!rsCol.EOF)
                            {
                                object sizeVal = rsCol.Fields["CHARACTER_MAXIMUM_LENGTH"].Value;
                                if (sizeVal != null && !(sizeVal is DBNull))
                                    int.TryParse(sizeVal.ToString(), out fieldSize);
                            }
                            rsCol.Close();
                            FLINDEX_LEN[t, FL_NUMBEROFINDEXEN[t]] = fieldSize;
                        }

                        int plTT = aa.IndexOf(extraCaption, StringComparison.Ordinal) + 1;
                        if (plTT == 1)
                            aa = aa.Substring(extraCaption.Length + 1);
                        else
                            aa = aa.Substring(0, plTT - 1) + aa.Substring(plTT - 1 + extraCaption.Length + 1);
                    }
                }
            }

            return result;
        }

        public static bool SaveFormProperties(Form frmWindow)
        {
            try
            {
                SaveSetting(Application.ProductName, frmWindow.Name, "Top", frmWindow.Top.ToString(CultureInfo.InvariantCulture));
                SaveSetting(Application.ProductName, frmWindow.Name, "Links", frmWindow.Left.ToString(CultureInfo.InvariantCulture));
                SaveSetting(Application.ProductName, frmWindow.Name, "Breedte", frmWindow.Width.ToString(CultureInfo.InvariantCulture));
                SaveSetting(Application.ProductName, frmWindow.Name, "Hoogte", frmWindow.Height.ToString(CultureInfo.InvariantCulture));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void LoadFormProperties(Form frmWindow)
        {
            try
            {
                frmWindow.Top = GetIntSetting(Application.ProductName, frmWindow.Name, "Top", frmWindow.Top);
                frmWindow.Left = GetIntSetting(Application.ProductName, frmWindow.Name, "Links", frmWindow.Left);
                frmWindow.Width = GetIntSetting(Application.ProductName, frmWindow.Name, "Breedte", frmWindow.Width);
                frmWindow.Height = GetIntSetting(Application.ProductName, frmWindow.Name, "Hoogte", frmWindow.Height);
            }
            catch
            {
                // Ignore
            }
        }

        public static void BeWaarTekst(string Onderdeel, string SubDeel, string Element)
        {
            SaveSetting(Application.ProductName, Onderdeel, SubDeel, Element);
        }

        public static string LaadTekstOLD(string Onderdeel, string SubDeel)
        {
            try
            {
                if (Onderdeel.Contains(";"))
                {
                    string[] parts = Onderdeel.Split(';');
                    string app = parts[0];
                    string section = parts[1];

                    string valuePath = Interaction.GetSetting(
                           app,       // AppName
                           section,     // Section
                           SubDeel,
                           "" // Default if not found
                           ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

                    return valuePath;
                }
                else
                {
                    string valuePath = Interaction.GetSetting(
                        "marINTEGRAAL",       // AppName
                        Onderdeel,     // Section
                        SubDeel,
                        "" // Default if not found
                        ) ?? ""; // Ensure null-coalescing operator to handle possible null value.

                    return valuePath;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string LaadTekst(string Onderdeel, string SubDeel)
        {
            try
            {
                if (Onderdeel.Contains(";"))
                {
                    string[] parts = Onderdeel.Split(';');
                    string app = parts[0];
                    string section = parts[1];
                    return GetSetting(app, section, SubDeel, string.Empty);
                }
                else
                {
                    return GetSetting(Application.ProductName, Onderdeel, SubDeel, string.Empty);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>        
        /// Displays text in the FormMim status bar (SnelHelp panel).
        /// If text starts with '~', the auto-clear timer is not (re)started.
        /// Appends to Globals.LOG_PRINT when Logging is true.
        /// </summary>
        public static void SnelHelpPrint(string printTekst, bool logging)
        {
            bool startTimer = printTekst.Length == 0 || printTekst[0] != '~';

            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.SetSnelHelp(printTekst, startTimer);
            }

            if (logging)
                LOG_PRINT += printTekst + "\r\n";
        }

        /// <summary>        
        /// Converts .OCT random-access files (fixed 16-byte records) to .OXT text files
        /// for each DEF00..DEF09 set found in LOCATION_COMPANYDATA.
        /// Returns True if at least one file was converted.
        /// </summary>
        public static bool NetVoorbereiden()
        {
            bool result = false;

            for (int netCOUNT_TO = 9; netCOUNT_TO >= 0; netCOUNT_TO--)
            {
                string oxtPath = LOCATION_COMPANYDATA + "DEF" + netCOUNT_TO.ToString("D2") + ".OXT";
                string octPath = LOCATION_COMPANYDATA + "DEF" + netCOUNT_TO.ToString("D2") + ".OCT";

                if (!File.Exists(oxtPath) && File.Exists(octPath))
                {
                    string netdummyLinE = string.Empty;
                    using (var fs = new FileStream(octPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[16];
                        for (int netT = 1; netT <= 99; netT++)
                        {
                            fs.Seek((netT - 1) * 16L, SeekOrigin.Begin);
                            int bytesRead = fs.Read(buffer, 0, 16);
                            if (bytesRead < 16)
                                break;
                            string netA = Encoding.Default.GetString(buffer);
                            if (netA == new string(' ', 16))
                                break;
                            netdummyLinE += "," + netA;
                        }
                    }

                    if (netdummyLinE.Length > 0)
                        netdummyLinE = netdummyLinE.Substring(1);

                    using (var sw = new StreamWriter(oxtPath, false, Encoding.Default))
                    {
                        sw.WriteLine(netdummyLinE);
                    }

                    result = true;
                }
            }

            return result;
        }

        // Helper: create subdirectory if it does not exist (VB6 MkDir)
        private static void EnsureSubDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch { }
        }

        #region Registry helpers

        private static void SaveSetting(string appName, string section, string key, string value)
        {
            try
            {
                using (var baseKey = Registry.CurrentUser.CreateSubKey($"Software\\{appName}\\{section}"))
                {
                    baseKey?.SetValue(key, value ?? string.Empty, RegistryValueKind.String);
                }
            }
            catch
            {
                // ignore
            }
        }

        private static string GetSetting(string appName, string section, string key, string defaultValue)
        {
            try
            {
                using (var baseKey = Registry.CurrentUser.OpenSubKey($"Software\\{appName}\\{section}"))
                {
                    if (baseKey == null) return defaultValue;
                    var val = baseKey.GetValue(key);
                    return val?.ToString() ?? defaultValue;
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        private static int GetIntSetting(string appName, string section, string key, int defaultValue)
        {
            var s = GetSetting(appName, section, key, defaultValue.ToString(CultureInfo.InvariantCulture));
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r))
                return r;
            return defaultValue;
        }

        #endregion        

        /// <summary>
        /// Returns the CHARACTER_MAXIMUM_LENGTH of a column via ADODB schema rowset.
        /// Returns int.MaxValue if the column is not found (so size checks safely fail-open).
        /// </summary>
        private static int GetColumnSize(ADODB.Connection cnn, string tableName, string columnName)
        {
            ADODB.Recordset rs = cnn.OpenSchema(
                ADODB.SchemaEnum.adSchemaColumns,
                new object[] { null, null, tableName, columnName },
                Type.Missing);
            try
            {
                if (!rs.EOF)
                {
                    object val = rs.Fields["CHARACTER_MAXIMUM_LENGTH"].Value;
                    if (val != null && !(val is DBNull) && int.TryParse(val.ToString(), out int size))
                        return size;
                }
            }
            finally
            {
                rs.Close();
            }
            return int.MaxValue;
        }
        /// <summary>
        /// VB6: Sub BalansKontroleWithRecordSet(Fl As Integer)
        /// Shows a balance overview for a customer or supplier in FormXLog,
        /// using an ADODB forward-only recordset over the Dokumenten table.
        /// </summary>
        public static void BalansKontroleWithRecordSet(int fl)
        {
            decimal cumul = 0m;
            decimal dTotaal = 0m;
            decimal dBetaald = 0m;

            // ── Open / prepare FormXLog ────────────────────────────────────────
            if (Application.OpenForms["FormXLog"] is SharedForms.FormXLog xlogExisting)
                xlogExisting.Close();

            // VB6: On Local Error Resume Next — field may not exist on all tables (e.g. A101 absent in Leveranciers).
            string titlePart1 = "";
            string titlePart2 = "";
            try { titlePart1 = ObjectValue(rsMAR[fl].Fields["A100"].Value)?.ToString() ?? ""; } catch { }
            try { titlePart2 = ObjectValue(rsMAR[fl].Fields["A101"].Value)?.ToString() ?? ""; } catch { }

            FormXLog xlog = new SharedForms.FormXLog
            {
                Text = ("Balans voor : " + titlePart1 + " " + titlePart2).TrimEnd()
            };

            SharedScanFl = TABLE_INVOICES;

            // ── Grid headers & layout ──────────────────────────────────────────
            xlog.X.Columns.Clear();
            string[] headers = { "Document", "Totaal", "Datum", "Fin.Stuk", "Betaald", "CumulRest", "Referte" };
            int[] widths = { 115, 111, 98, 101, 97, 119, 220 };
            var rightAlign = new[] { 1, 4, 5 }; // column indices that are right-aligned

            for (int i = 0; i < headers.Length; i++)
            {
                var col = new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    HeaderText = headers[i],
                    Width = widths[i],
                    DefaultCellStyle = { Alignment = Array.IndexOf(rightAlign, i) >= 0
                                ? System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
                                : System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft }
                };
                xlog.X.Columns.Add(col);
            }

            // ── Determine VoorLetter ──────────────────────────────────────────
            string voorLetter = fl == TABLE_CUSTOMERS ? "K"
                               : fl == TABLE_SUPPLIERS ? "L"
                               : "";

            bool enkelOpenstaand = false;
            int rowCount = 0;

        opnieuw:
            cumul = 0m;
            dTotaal = 0m;
            dBetaald = 0m;
            xlog.X.Rows.Clear();
            rowCount = 0;

            // ── Build and run query ───────────────────────────────────────────
            string klantnr = VBibText(fl, "#A110 #").TrimEnd();
            Msg = "SELECT * FROM Dokumenten ";
            Msg += "WHERE v034 = '" + voorLetter + klantnr + "' ";
            Msg += "ORDER BY v035 DESC";

            SnelHelpPrint(Msg, BL_LOGGING);

            var rsLocalAV = new ADODB.Recordset();
            rsLocalAV.CursorLocation = ADODB.CursorLocationEnum.adUseClient;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                rsLocalAV.Open(Msg, adntDB,
                    ADODB.CursorTypeEnum.adOpenForwardOnly,
                    ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText);

                if (rsLocalAV.RecordCount == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Er zijn geen documenten",
                        "", System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Exclamation);
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    return;
                }

                rsLocalAV.MoveFirst();
                while (!rsLocalAV.EOF)
                {
                    // ── AddLine (VB6 GoSub) ────────────────────────────────────
                    string v033 = ObjectValue(rsLocalAV.Fields["v033"].Value)?.ToString() ?? "";
                    dBetaald = 0m;
                    dTotaal = 0m;

                    if (fl == TABLE_CUSTOMERS)
                    {
                        dBetaald = ParseDecimal(ObjectValue(rsLocalAV.Fields["v037"].Value));

                        string firstChar = v033.Length > 0 ? v033.Substring(0, 1) : "";
                        if (firstChar == "V")
                        {
                            dTotaal = ParseDecimal(ObjectValue(rsLocalAV.Fields["v249"].Value));
                            if (v033.Length > 1 && v033.Substring(1, 1) == "1")
                            {
                                dTotaal = -dTotaal;
                                dBetaald = -dBetaald;
                            }
                        }
                        else if (firstChar == "Q")
                        {
                            dTotaal = ParseDecimal(ObjectValue(rsLocalAV.Fields["v249"].Value));
                        }

                        cumul += dTotaal - dBetaald;

                        string v035str = ObjectValue(rsLocalAV.Fields["v035"].Value)?.ToString() ?? "";
                        string v038 = ObjectValue(rsLocalAV.Fields["v038"].Value)?.ToString() ?? "";
                        string a000 = (ObjectValue(rsLocalAV.Fields["A000"].Value)?.ToString() ?? "").Trim();
                        string v039 = ObjectValue(rsLocalAV.Fields["v039"].Value)?.ToString() ?? "";
                        string referte = a000 != "" ? a000 : v039;

                        if (!enkelOpenstaand || dBetaald != dTotaal)
                        {
                            xlog.X.Rows.Add(
                                v033,
                                dTotaal.ToString("#,##0.00"),
                                FormatDateText(v035str),
                                v038,
                                dBetaald.ToString("#,##0.00"),
                                cumul.ToString("#,##0.00"),
                                referte);
                            rowCount++;
                        }
                    }
                    else if (fl == TABLE_SUPPLIERS)
                    {
                        dBetaald = ParseDecimal(ObjectValue(rsLocalAV.Fields["v037"].Value));
                        dTotaal = ParseDecimal(ObjectValue(rsLocalAV.Fields["v249"].Value));

                        if (v033.Length >= 2 && v033.Substring(0, 2) == "A1")
                        {
                            dTotaal = -dTotaal;
                            dBetaald = -dBetaald;
                        }

                        cumul += dTotaal - dBetaald;

                        string v035str = ObjectValue(rsLocalAV.Fields["v035"].Value)?.ToString() ?? "";
                        string v038 = ObjectValue(rsLocalAV.Fields["v038"].Value)?.ToString() ?? "";
                        string v039 =   ObjectValue(rsLocalAV.Fields["v039"].Value)?.ToString() ?? "";

                        if (!enkelOpenstaand || dBetaald != dTotaal)
                        {
                            xlog.X.Rows.Add(
                                v033,
                                dTotaal.ToString("#,##0.00"),
                                FormatDateText(v035str),
                                v038,
                                dBetaald.ToString("#,##0.00"),
                                cumul.ToString("#,##0.00"),
                                v039);
                            rowCount++;
                        }
                    }

                    rsLocalAV.MoveNext();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                Msg = "Er zijn reeds " + rowCount.ToString() + " dokumenten !  Teveel voor het geheugen.  Alleen de openstaande dokumenten weergeven ?";
                KtrlBox = (int)System.Windows.Forms.MessageBox.Show(Msg,
                    "", System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question);
                if (KtrlBox == (int)System.Windows.Forms.DialogResult.Yes)
                {
                    enkelOpenstaand = true;
                    if (rsLocalAV.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                        rsLocalAV.Close();
                    goto opnieuw;
                }
                else
                {
                    SnelHelpPrint(ex.Message, BL_LOGGING);
                    goto opHetScherm;
                }
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                if (rsLocalAV.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                    rsLocalAV.Close();
            }

        opHetScherm:
            if (xlog.X.Rows.Count > 0)
            {
                xlog.X.Rows[0].Selected = false;
                xlog.X.CurrentCell = xlog.X.Rows[0].Cells[0];
            }

            // ── Show FormXLog ─────────────────────────────────────────────────
            xlog.BtnDetailJournaal.Visible = true;
            xlog.BtnWijzigenLijn.Visible = false;
            xlog.BtnAfsluiten.TabStop = false;
            xlog.BtnAfbeelding.Visible = false;
            if (xlog.TabControl1.TabPages.Count > 1)
                xlog.TabControl1.TabPages[1].Text = "Journaal";

            XLogKey = "";
            xlog.ShowDialog();

            SharedScanFl = 0;

            // ── Post-display: handle selected document ────────────────────────
            if (string.IsNullOrEmpty(XLogKey))
                return;

            BGet(TABLE_INVOICES, 0, XLogKey.Length >= 11 ? XLogKey.Substring(0, 11) : XLogKey);
            RecordToVeld(TABLE_INVOICES);
            TeleBibClick(TABLE_INVOICES);

            if (string.IsNullOrEmpty(XLogKey))
                return;

            if (VSF_PRO)
                return;

            BGet(TABLE_JOURNAL, 1,
                VBibText(TABLE_INVOICES, "#" + JETTABLEUSE_INDEX[TABLE_INVOICES, 0] + "#"));

            if (Ktrl != 0)
            {
                // VB6: If Ktrl Then (non-zero = found journal lines → offer delete)
                string docKey = VBibText(TABLE_INVOICES, "#" + JETTABLEUSE_INDEX[TABLE_INVOICES, 0] + "#");
                string deleteMsg = "Ja = alle TYPE-dokumenten DAT jaar vernietigen !" + System.Environment.NewLine
                    + "Nee = enkel DIT dokument verwijderen.";
                int deleteChoice = (int)System.Windows.Forms.MessageBox.Show(
                    deleteMsg,
                    docKey + ": dokument vernietigen !",
                    System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                    System.Windows.Forms.MessageBoxIcon.Question);

                switch (deleteChoice)
                {
                    case (int)System.Windows.Forms.DialogResult.Cancel:
                        // niks
                        break;

                    case (int)System.Windows.Forms.DialogResult.Yes:
                        {
                            RecordToVeld(TABLE_INVOICES);
                            string kontroleString = VBibText(TABLE_INVOICES, "#v035 #").Length >= 4
                                ? VBibText(TABLE_INVOICES, "#v035 #").Substring(0, 4) : "";

                            string bulkMsg = "Onvoorwaardelijk meerdere dokumenten in reeks vernietigen van jaar "
                                + kontroleString + System.Environment.NewLine + System.Environment.NewLine
                                + "Bent U zeker ?";
                            if (System.Windows.Forms.MessageBox.Show(
                                    bulkMsg,
                                    "Opkuis dokumenten jaar " + kontroleString,
                                    System.Windows.Forms.MessageBoxButtons.YesNo,
                                    System.Windows.Forms.MessageBoxIcon.Question,
                                    System.Windows.Forms.MessageBoxDefaultButton.Button2)
                                == System.Windows.Forms.DialogResult.Yes)
                            {
                                BFirst(TABLE_INVOICES, 0);
                                if (Ktrl == 0)
                                {
                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                                    BBegin();
                                    while (true)
                                    {
                                        if (KEY_BUF[TABLE_INVOICES].Length >= 3
                                            && KEY_BUF[TABLE_INVOICES].Substring(2, 1)
                                               == kontroleString.Substring(kontroleString.Length - 1, 1))
                                        {
                                            RecordToVeld(TABLE_INVOICES);
                                            if (kontroleString == VBibText(TABLE_INVOICES, "#v035 #").Substring(0, 4))
                                            {
                                                SnelHelpPrint(KEY_BUF[TABLE_INVOICES], BL_LOGGING);
                                                BDelete(TABLE_INVOICES);
                                            }
                                        }
                                        BNext(TABLE_INVOICES);
                                        if (Ktrl != 0)
                                            break;
                                    }
                                    BEnd();
                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                                }
                            }
                            break;
                        }

                    case (int)System.Windows.Forms.DialogResult.No:
                        {
                            BDelete(TABLE_INVOICES);
                            if (Ktrl != 0)
                                System.Windows.Forms.MessageBox.Show("stop");
                            break;
                        }
                }
            }
            else
            {
                // VB6: Else — no journal lines found
                System.Windows.Forms.MessageBox.Show(
                    "Er zijn nog journaallijnen van het boekjaar desbetreffend dokument beschikbaar !  " +
                    "Verwijderen via menuoptie 'opkuis bestanden' a.u.b.");
            }
        }

        // VB6: Val() equivalent — parses a decimal from an object, returns 0 on failure.
        private static decimal ParseDecimal(object value)
        {
            if (value == null || value is DBNull) return 0m;
            return decimal.TryParse(value.ToString(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal result) ? result : 0m;
        }

        /// <summary>
        /// Formats a date string from a recordset field value into dd/mm/yyyy display format.
        /// The source may be an ISO date string (yyyy-mm-dd), a DateTime, or yyyymmdd text.
        /// </summary>
        private static string FormatDateText(string rawDate)
        {
            if (string.IsNullOrWhiteSpace(rawDate))
                return "";

            if (DateTime.TryParse(rawDate, out DateTime dt))
                return dt.ToString("dd/MM/yyyy");

            // yyyymmdd compact format
            if (rawDate.Length == 8
                && DateTime.TryParseExact(rawDate, "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dt2))
                return dt2.ToString("dd/MM/yyyy");

            return rawDate;
        }


        // ══════════════════════════════════════════════════════════════════════
        // VB6: Function vsfInputBox
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// VB6: Function vsfInputBox — opens FormNTInputbox as a modal dialog.
        /// Returns the entered/selected text, or the original tekstZelf on cancel.
        /// </summary>
        public static string VsfInputBox(string infoTekst, string titel, string tekstZelf, string paswoord)
        {
            ToolDef = new string[4];
            bool isLookup = infoTekst.Length > 0 && infoTekst[0] == '@';
            string caseCode = isLookup && infoTekst.Length >= 3 ? infoTekst.Substring(1, 2) : string.Empty;

            try
            {
                using (var dlg = new FormNTInputbox())
                {
                    VsfInputBox_ConfigureDialog(dlg, titel, infoTekst, tekstZelf, isLookup, caseCode);
                    dlg.ShowDialog();
                    return VsfInputBox_ProcessResult(dlg, infoTekst, tekstZelf, isLookup, caseCode);
                }
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message, "VsfInputBox", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return tekstZelf;
            }
        }

        private static void VsfInputBox_ConfigureDialog(
            FormNTInputbox dlg, string titel, string infoTekst, string tekstZelf, bool isLookup, string caseCode)
        {
            dlg.Text = titel;

            if (isLookup)
            {
                dlg.LookupPanelVisible = true;
                switch (caseCode)
                {
                    case "00":
                        dlg.SQLBevel = "SELECT * FROM ISOLandKodes WHERE ISOLandNummer LIKE '";
                        ToolDef[0] = "00=v149 ";   // Landnummer  ISO kode
                        ToolDef[1] = "01=vs03 ";   // Munteenheid ISO kode
                        ToolDef[2] = "02=v150 ";   // Landkode    ISO kode
                        break;
                    case "01":
                        dlg.SQLBevel = "SELECT * FROM PostKodesWoonplaatsen WHERE PostKode LIKE '";
                        ToolDef[0] = "01=A107 ";   // PostKode volgens Postkantoor
                        ToolDef[1] = "02=A108 ";   // Plaatsnaam
                        break;
                    case "02":
                        dlg.SQLBevel = "SELECT * FROM PostKodesWoonplaatsen WHERE PlaatsNaam LIKE '";
                        ToolDef[0] = "02=A108 ";   // Plaatsnaam
                        ToolDef[1] = "01=A107 ";   // PostKode volgens Postkantoor
                        break;
                }
            }
            else
            {
                dlg.LookupPanelVisible = false;
            }

            // VB6: ntInputbox.Tag = GridText
            string savedGridText = GridText;

            if (GridText == "Edit No")
            {
                dlg.OkVisible = false;
                dlg.SluitenIsDefault = true;
            }
            else
            {
                dlg.OkVisible = true;
                dlg.OkIsDefault = true;
            }

            GridText = infoTekst;
            dlg.StatusText = isLookup
                ? GridText + dlg.SQLBevel + tekstZelf.TrimEnd() + "%';"
                : GridText;

            dlg.InputText = tekstZelf;
        }

        private static string VsfInputBox_ProcessResult(
            FormNTInputbox dlg, string infoTekst, string tekstZelf, bool isLookup, string caseCode)
        {
            // Chr$(255) sentinel = user cancelled
            if (dlg.InputText == "\xFF")
                return tekstZelf;

            if (!isLookup || string.IsNullOrEmpty(dlg.SQLBevel))
                return dlg.InputText;

            // Lookup mode: write related fields back into the active FormXLog grid
            VsfInputBox_UpdateXlogGrid(dlg, caseCode);
            return dlg.InputText;
        }

        private static void VsfInputBox_UpdateXlogGrid(FormNTInputbox dlg, string caseCode)
        {
            // Determine how many extra fields to copy back (VB6: AantalRijen)
            int aantalRijen;
            switch (caseCode)
            {
                case "00": aantalRijen = 2; break;
                case "01":
                case "02": aantalRijen = 1; break;
                default:
                    MessageBox.Show("Stop: onbekende caseCode " + caseCode + " in VsfInputBox_UpdateXlogGrid");
                    return;
            }

            // Find the active FormXLog
            FormXLog xlog = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FormXLog xl) { xlog = xl; break; }
            }
            if (xlog == null) return;

            // VB6: For T = AantalRijen To 0 Step -1
            for (int t = aantalRijen; t >= 0; t--)
            {
                string toolEntry = ToolDef[t] ?? "";
                // VB6: Mid(ToolDef(T), 4) — field code starts at 0-based index 3
                string fieldCode = toolEntry.Length > 3 ? toolEntry.Substring(3).TrimEnd() : string.Empty;

                for (int row = 0; row < xlog.X.Rows.Count; row++)
                {
                    string cell0 = xlog.X.Rows[row].Cells[0].Value?.ToString() ?? "";
                    // VB6: Mid(Xlog.X.Text, 5, 5) = Mid(ToolDef(T), 4)
                    string mid5 = cell0.Length >= 9 ? cell0.Substring(4, 5) : string.Empty;
                    if (mid5 == fieldCode.PadRight(5).Substring(0, 5))
                    {
                        // VB6: Recordset(Val(Mid(ToolDef(T), 1, 2)))
                        int fieldIndex = 0;
                        int.TryParse(toolEntry.Length >= 2 ? toolEntry.Substring(0, 2).Trim() : "0", out fieldIndex);
                        string newValue = dlg.GetRecordsetField(fieldIndex);
                        xlog.X.Rows[row].Cells[2].Value = newValue;
                        break;
                    }
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // VB6: Function TeleBibClick
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// VB6: Function TeleBibClick — loads the appropriate .Def file, shows a
        /// FormXLog grid with all TELEBIB fields for the record, and writes changed
        /// values back via vBib.  Returns true when the user confirmed (XLogKey set).
        /// </summary>
        public static bool TeleBibClick(int fl)
        {
            // ── Clear TELEBIB arrays before loading DEF files ─────────────────
            Array.Clear(TELEBIB_CODE, 0, TELEBIB_CODE.Length);
            Array.Clear(TELEBIB_TEXT, 0, TELEBIB_TEXT.Length);
            Array.Clear(TELEBIB_TYPE, 0, TELEBIB_TYPE.Length);
            Array.Clear(TELEBIB_LENGTH, 0, TELEBIB_LENGTH.Length);
            Array.Clear(TELEBIB_POS, 0, TELEBIB_POS.Length);

            // ── Load .Def file ────────────────────────────────────────────────
            if (fl >= TABLE_CUSTOMERS && fl <= TABLE_CONTRACTS)
            {
                if (!TeleBibPagina(fl))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else if (fl == TABLE_INVOICES)
            {
                string firstChar = FVT[TABLE_INVOICES, 0].Length > 0 ? FVT[TABLE_INVOICES, 0].Substring(0, 1) : "";
                if (!TLBPag2("020" + firstChar))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else if (fl == 10 || fl == 12 || fl == 18 || fl == 21 || fl == 28)
            {
                if (!TLBPag2(fl.ToString("000")))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else if (fl >= 1000 && fl <= 1999)
            {
                if (!TLBPag3("AS1" + (fl - 1000).ToString("000")))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else if (fl >= 2000 && fl <= 2099)
            {
                if (!TLBPag2("GROEP" + (fl - 2000).ToString("00")))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else if (fl >= 3000 && fl <= 3099)
            {
                if (!TLBPag2("SCHADE" + (fl - 3000).ToString("00")))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else if (fl >= 4000 && fl <= 4999)
            {
                if (!TLBPag3("TAK" + (fl - 4000).ToString()))
                { System.Media.SystemSounds.Beep.Play(); return false; }
            }
            else
            {
                MessageBox.Show("stop in TeleBibClick, fl=" + fl.ToString());
                return false;
            }

            // ── Determine log caption and remap fl to TABLE_VARIOUS where needed ──
            string logTekst;
            switch (fl)
            {
                case int f when f == TABLE_CUSTOMERS:
                    logTekst = "BIB voor Klanten"; break;
                case int f when f == TABLE_SUPPLIERS:
                    logTekst = "BIB voor Leveranciers"; break;
                case int f when f == TABLE_LEDGERACCOUNTS:
                    logTekst = "BIB voor Algemene Rekeningen"; break;
                case int f when f == TABLE_PRODUCTS:
                    logTekst = "BIB voor Artikels/Diensten"; break;
                case int f when f == TABLE_CONTRACTS:
                    logTekst = "BIB voor contracten"; break;
                case int f when f == TABLE_INVOICES:
                    logTekst = "BIB voor Aan- en Verkoopdokumenten"; break;
                case int f when f >= 1000 && f <= 1999:
                    fl = TABLE_VARIOUS; logTekst = "BIB AS1/verzoeken"; break;
                case int f when f >= 2000 && f <= 2099:
                    fl = TABLE_VARIOUS; logTekst = "BIB Polis " + VBibText(TABLE_CONTRACTS, "#A000 #"); break;
                case int f when f >= 3000 && f <= 3099:
                    fl = TABLE_VARIOUS; logTekst = "Bib Schade " + VBibText(TABLE_VARIOUS, "#C000 #"); break;
                case int f when f >= 4000 && f <= 4099:
                    fl = TABLE_VARIOUS; logTekst = "BIB DetailPolis " + VBibText(TABLE_CONTRACTS, "#A000 #"); break;
                default:
                    fl = TABLE_VARIOUS; logTekst = " BIB Allerlei"; break;
            }

            // ── Build and show the FormXLog grid ─────────────────────────────
            var xlog = new FormXLog();
            xlog.Text += logTekst;

            // Set up 3-column header row
            xlog.X.Columns.Clear();
            xlog.X.Columns.Add("colKode", "vsfKode");
            xlog.X.Columns.Add("colOmschr", "Veldomschrijving");
            xlog.X.Columns.Add("colGegevens", "Veldgegevens");
            xlog.X.Columns[0].Width = 45;
            xlog.X.Columns[1].Width = 210;
            xlog.X.Columns[2].Width = 463;

            // Invoice-specific button state
            if (fl == TABLE_INVOICES)
            {
                xlog.BtnWijzigenLijn.Enabled = VSF_PRO;
                xlog.BtnAfsluiten.Text = VSF_PRO ? "Speciaal" : "Vernietig!";
                xlog.BtnAfsluiten.Visible = VSF_PRO;
            }

            // ── Populate grid rows from TELEBIB arrays ────────────────────────
            int t = 0;
            while (t < TELEBIB_CODE.Length && TELEBIB_CODE[t] != new string(' ', 10))
            {
                string code = TELEBIB_CODE[t] ?? "";
                // Console.WriteLine("#" + SafeMid(code, 5, 4) + " #");
                string crText = VBibText(fl, "#" + SafeMid(code, 5, 4) + " #");
                string typeCode = SafeMid(code, 2, 2);

                switch (typeCode)
                {
                    case "  ":
                    case "K ":
                    case "L ":
                    case "LC":
                    case "R ":
                    case "R3":
                    case "R4":
                    case "R6":
                    case "R7":
                        break;
                    default:
                        if (code.Length > 0 && code[0] != '@' && crText != "")
                        {
                            string boxMask;
                            if (code[0] == ' ')
                                boxMask = "00";
                            else if (code[0] >= '0' && code[0] <= '9')
                                boxMask = "000";
                            else
                                boxMask = "00";

                            int boxVal = 0;
                            int.TryParse(SafeMid(code, 1, 3), out boxVal);
                            crText = FMarBoxText(boxVal.ToString(boxMask), "2", crText);
                        }
                        break;
                }

                if (SafeMid(code, 10, 1) == "x")
                    crText = fl.ToString() + "{...}";

                if (code != "")
                {
                    xlog.X.Rows.Add(code, TELEBIB_TEXT[t], crText);
                }
                t++;
            }

            xlog.X.Rows[0].Selected = true;

        XLogShow:
            xlog.BtnWijzigenLijn.TabStop = true;
            xlog.BtnAfsluiten.TabStop = true;
            XLogKey = string.Empty;
            xlog.TabControl1.TabPages[1].Text = "- Geen Bijlage";
            xlog.TabControl1.TabPages[1].Visible = false;
            xlog.ShowDialog();
            PeppolFlag = false;

            if (XLogKey == string.Empty)
                return false;

            // ── Write changed values back via vBib ────────────────────────────
            t = 0;
            while (t < TELEBIB_CODE.Length && TELEBIB_CODE[t] != new string(' ', 10))
            {
                string code = TELEBIB_CODE[t] ?? "";
                string crText2 = xlog.X.Rows.Count > t
                    ? xlog.X.Rows[t].Cells[2].Value?.ToString() ?? ""
                    : "";

                if (SafeMid(code, 10, 1) == "*" && crText2 == "")
                {
                    MessageBox.Show(
                        "Invoer voor '" + (TELEBIB_TEXT[t] ?? "").TrimEnd() + "'\r\n\r\nis verplicht !",
                        "Vervolledig a.u.b.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    goto XLogShow;
                }

                if (crText2 != "" && SafeMid(code, 10, 1) != "x")
                {
                    if (SafeMid(code, 2, 2) != "  " && (code.Length == 0 || code[0] != '@'))
                    {
                        int colon = crText2.IndexOf(':');
                        if (colon > 0)
                            crText2 = crText2.Substring(0, colon);
                    }
                    VBib(fl, crText2, SafeMid(code, 5, 5));
                }
                t++;
            }

            // ── "Speciaal" = confirm and update ──────────────────────────────
            if (xlog.BtnAfsluiten.Text == "Speciaal")
            {
                string msg = "Gegevens bestaande fiche wijzigen.  Bent U zeker ?";
                if (MessageBox.Show(msg, string.Empty,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    BUpdate(fl, 0);
            }

            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        // VB6: Function TLBPag3
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// VB6: Function TLBPag3 — loads TELEBIB arrays from a named .Def file that
        /// has 5 fields per record (adds TELEBIB_POS compared with TLBPag2).
        /// Returns true on success.
        /// </summary>
        public static bool TLBPag3(string bsDef)
        {
            string defPath = PROGRAM_LOCATION + @"\Content\Def\" + bsDef + ".Def";
            if (!File.Exists(defPath))
            {
                MessageBox.Show("Geen VsoftBib definitie " + bsDef + ".Def");
                return false;
            }

            int t = 0;
            try
            {
                // VB6: Open … For Input — read CSV-style fields
                using (var sr = new StreamReader(defPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine() ?? "";
                        // VB6: Input #n, f1, f2, f3, f4, f5  (comma-separated, possibly quoted)
                        string[] parts = ParseInputLine(line);
                        if (parts.Length >= 5)
                        {
                            TELEBIB_CODE[t] = parts[0];
                            TELEBIB_TEXT[t] = parts[1];
                            TELEBIB_TYPE[t] = parts[2];
                            TELEBIB_LENGTH[t] = int.TryParse(parts[3], out int len) ? len : 0;
                            TELEBIB_POS[t] = int.TryParse(parts[4], out int pos) ? pos : 0;
                            t++;
                        }
                    }
                }
                TELEBIB_CODE[t] = "";
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Telebibinlaadfout " + t.ToString() + " error: " + ex.Message);
                return false;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // VB6: Function TLBPag2
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>
        /// VB6: Function TLBPag2 — loads TELEBIB arrays from a named .Def file (4
        /// fields per record).  Load order: user override (xxxU.Def) → base (xxx.Def)
        /// → makelaar extension (xxxM.Def).  Returns true on success.
        /// </summary>
        public static bool TLBPag2(string bsDef)
        {
            string baseDir = PROGRAM_LOCATION + @"\Content\Def\";

            if (!File.Exists(baseDir + bsDef + ".Def"))
            {
                MessageBox.Show("Geen VsoftBib definitie " + bsDef + ".Def");
                return false;
            }

            int t = 0;
            try
            {
                // ── User override (xxxU.Def) takes priority ───────────────────
                string userDef = baseDir + bsDef + "U.Def";
                if (File.Exists(userDef))
                {
                    using (var sr = new StreamReader(userDef))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] parts = ParseInputLine(sr.ReadLine() ?? "");
                            if (parts.Length >= 4)
                            {
                                TELEBIB_CODE[t] = parts[0];
                                TELEBIB_TEXT[t] = parts[1];
                                TELEBIB_TYPE[t] = parts[2];
                                TELEBIB_LENGTH[t] = int.TryParse(parts[3], out int len) ? len : 0;
                                t++;
                            }
                        }
                    }
                    TELEBIB_CODE[t] = "";
                    TELEBIB_LAST = t - 1;
                    return true;
                }

                // ── Base definition ───────────────────────────────────────────
                using (var sr = new StreamReader(baseDir + bsDef + ".Def"))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] parts = ParseInputLine(sr.ReadLine() ?? "");
                        if (parts.Length >= 4)
                        {
                            TELEBIB_CODE[t] = parts[0];
                            TELEBIB_TEXT[t] = parts[1];
                            TELEBIB_TYPE[t] = parts[2];
                            TELEBIB_LENGTH[t] = int.TryParse(parts[3], out int len) ? len : 0;
                            t++;
                        }
                    }
                }
                TELEBIB_CODE[t] = "";

                // ── Makelaar extension (xxxM.Def) appended when applicable ────
                if (ProducentNummer != new string(' ', 8))
                {
                    string makelaarDef = baseDir + bsDef + "M.Def";
                    if (File.Exists(makelaarDef))
                    {
                        using (var sr = new StreamReader(makelaarDef))
                        {
                            while (!sr.EndOfStream)
                            {
                                string[] parts = ParseInputLine(sr.ReadLine() ?? "");
                                if (parts.Length >= 4)
                                {
                                    TELEBIB_CODE[t] = parts[0];
                                    TELEBIB_TEXT[t] = parts[1];
                                    TELEBIB_TYPE[t] = parts[2];
                                    TELEBIB_LENGTH[t] = int.TryParse(parts[3], out int len) ? len : 0;
                                    t++;
                                }
                            }
                        }
                        TELEBIB_CODE[t] = "";
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Telebibinlaadfout " + t.ToString() + " error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Parses one VB6-style "Input #n" record line into its comma-separated
        /// fields, honouring double-quoted strings (which may contain commas).
        /// </summary>
        internal static string[] ParseInputLine(string line)
        {
            var fields = new System.Collections.Generic.List<string>();
            int i = 0;
            while (i < line.Length)
            {
                // skip leading whitespace
                while (i < line.Length && line[i] == ' ') i++;

                if (i >= line.Length) break;

                if (line[i] == '"')
                {
                    // quoted field
                    i++;
                    var sb = new StringBuilder();
                    while (i < line.Length)
                    {
                        if (line[i] == '"')
                        {
                            i++;
                            if (i < line.Length && line[i] == '"') { sb.Append('"'); i++; } // escaped quote
                            else break;
                        }
                        else { sb.Append(line[i]); i++; }
                    }
                    fields.Add(sb.ToString());
                }
                else
                {
                    // unquoted field — read up to next comma
                    int start = i;
                    while (i < line.Length && line[i] != ',') i++;
                    fields.Add(line.Substring(start, i - start).Trim());
                }

                // skip comma separator
                if (i < line.Length && line[i] == ',') i++;
            }
            return fields.ToArray();
        }

        /// <summary>
        /// VB6: Function IsXP — returns true when running on Windows XP (major=5, minor=1).
        /// Uses Environment.OSVersion instead of the deprecated GetVersion kernel32 API.
        /// </summary>
        public static bool IsXP()
        {
            Version v = System.Environment.OSVersion.Version;
            return v.Major == 5 && v.Minor == 1;
        }

        // ── Internet connectivity ─────────────────────────────────────────────

        private const string TEST_URL_1 = "http://www.msftconnecttest.com/connecttest.txt";
        private const string TEST_URL_2 = "http://www.gstatic.com/generate_204";
        private const int TIMEOUT_MS = 4000;

        /// <summary>
        /// VB6: Function Internet_IsAvailable — checks connectivity via WinHTTP,
        /// falls back to WebClient download, then opens the browser as a last resort.
        /// </summary>
        public static bool InternetIsAvailable()
        {
            if (CheckWinHttp(TEST_URL_1)) return true;
            if (CheckWinHttp(TEST_URL_2)) return true;
            if (CheckUrlDownload(TEST_URL_1)) return true;
            if (CheckUrlDownload(TEST_URL_2)) return true;

            // Final fallback: open browser so the user can confirm connectivity visually
            ShellExecuteFallback("http://www.msftconnecttest.com/redirect");
            return false;
        }

        /// <summary>
        /// VB6: Check_WinHttp — preferred method using WinHttp.WinHttpRequest.5.1 via COM late-binding.
        /// Returns true when the server responds with HTTP 200 or 204.
        /// </summary>
        private static bool CheckWinHttp(string url)
        {
            try
            {
                Type httpType = Type.GetTypeFromProgID("WinHttp.WinHttpRequest.5.1");
                if (httpType == null) return false;

                object http = Activator.CreateInstance(httpType);
                httpType.InvokeMember("setTimeouts",
                    System.Reflection.BindingFlags.InvokeMethod, null, http,
                    new object[] { TIMEOUT_MS, TIMEOUT_MS, TIMEOUT_MS, TIMEOUT_MS });

                httpType.InvokeMember("Open",
                    System.Reflection.BindingFlags.InvokeMethod, null, http,
                    new object[] { "GET", url, false });

                httpType.InvokeMember("Send",
                    System.Reflection.BindingFlags.InvokeMethod, null, http,
                    new object[] { System.Reflection.Missing.Value });

                int status = (int)httpType.InvokeMember("Status",
                    System.Reflection.BindingFlags.GetProperty, null, http, null);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(http);
                return status == 200 || status == 204;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// VB6: Check_URLDownload — fallback using System.Net.WebClient.
        /// Downloads to a temp file; returns true when the download succeeds.
        /// </summary>
        private static bool CheckUrlDownload(string url)
        {
            string tmp = Path.Combine(Path.GetTempPath(), "netcheck.tmp");
            try
            {
                using (var client = new System.Net.WebClient())
                    client.DownloadFile(url, tmp);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                try { if (File.Exists(tmp)) File.Delete(tmp); } catch { }
            }
        }

        /// <summary>
        /// VB6: ShellExecute_Fallback — opens a URL in the default browser
        /// using the existing ShellHelper infrastructure.
        /// </summary>
        public static void ShellExecuteFallback(string url)
        {
            try { ShellHelper.ShellExecuteWithFallback(url); } catch { }
        }

        /// <summary>        
        /// Validates and posts a journal entry: updates cumulative totals, ledger account
        /// balances, and commits the rsJournaal record.
        /// </summary>
        public static void BookingAddLine(FormBoeking boekingForm, double v068, string sV019, string sV067)
        {
            DKTRL_CUMUL += v068;
            DKTRL_BEF += Math.Round(v068 * EURO, 0);
            DKTRL_EUR += Math.Round(v068, 2);

            // Build grid line (tab-separated)
            string buildLine = sV019 + "\t" + sV067 + "\t";

            if (v068 < 0)
            {
                buildLine += "\t" + (-v068).ToString("#,##0.00");
                buildLine += "\t" + "\t" + Math.Round(-v068 * EURO).ToString("#,##0.00");
            }
            else
            {
                buildLine += v068.ToString("#,##0.00") + "\t" + "";
                buildLine += "\t" + Math.Round(v068 * EURO).ToString("#,##0.00") + "\t" + "";
            }

            // Port of VB6: frmBoeking.mshfBoekLijst.AddItem pipo
            boekingForm?.AddItem(buildLine);
        }
    }
}