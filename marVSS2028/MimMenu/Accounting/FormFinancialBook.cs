using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormFinancialBook : Form
    {
        // ── Period boundaries (YYYYMMDD) ─────────────────────────────────────
        private string _periodFrom = BOOKYEAR_FROMTO.Substring(0, 8);
        private string _periodTo   = BOOKYEAR_FROMTO.Substring(8);

        // ── Running totals ────────────────────────────────────────────────────
        private double _totalDebit;
        private double _totalCredit;

        // ── Report layout ─────────────────────────────────────────────────────
        private readonly string _fullLine  = new string('-', 128);
        private string[] _reportField      = new string[8];
        private int[]    _reportTab        = new int[8];
        private string   _reportTitle      = "";
        private string[] _reportText       = new string[4]; // [0]=printDate [1]=header [2]=subtitle [3]=extra
        private double   _ypos;
        private int      _pageCounter;

        // ── Column data per row ───────────────────────────────────────────────
        private string[] _veldTxt = new string[8];

        // ── Financial account setup (10 bank accounts from String99) ─────────
        private readonly int[]    _recNummer      = new int[10];
        private readonly string[] _rekeningNummer = new string[10];

        // ── Square-check accumulator (in-memory, keyed by account number) ────
        private readonly SortedDictionary<string, (double count, double amount)> _cumulData =
            new SortedDictionary<string, (double count, double amount)>(StringComparer.Ordinal);

        // ── Error log collected during print ─────────────────────────────────
        private string _errorMsg = "";

        public FormFinancialBook()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Load
        // ═══════════════════════════════════════════════════════════════════════
        private void FormFinancialBook_Load(object sender, EventArgs e)
        {
            PeriodTextBox.Text        = FormattedFromTo(_periodFrom, _periodTo);
            ProcessingDateTextBox.Text = MIM_GLOBAL_DATE;

            // Map the 10 financial (bank/cash) accounts from setup String99 records
            // VB6: RecNummer(0..9) and RekeningNummer(0..9) built from String99(READING, 41..45, 39, 211..214)
            int[] setupNrs   = { 41, 42, 43, 44, 45, 39, 211, 212, 213, 214 };
            int[] recNrs     = { 31, 32, 33, 34, 35, 38, 215, 216, 217, 218 };
            for (int i = 0; i < 10; i++)
            {
                _recNummer[i]      = recNrs[i];
                _rekeningNummer[i] = VSet(String99(setupNrs[i]), 7);
            }

            RefreshAccountComboBox();
        }

        private void FormFinancialBook_Shown(object sender, EventArgs e)
        {
            if (ExtractsListBox.Items.Count > 0)
                ExtractsListBox.SelectedIndex = 0;
            ExtractsListBox.Focus();
        }

        // ── Rebuild AccountComboBox from journal within current period ────────
        private void RefreshAccountComboBox()
        {
            AccountComboBox.Items.Clear();

            string sqlAccounts =
                "SELECT DISTINCT j.v070, r.v019, r.v020 " +
                "FROM Journalen j " +
                "LEFT JOIN Rekeningen r ON r.v019 = Left(j.v070, 7) " +
                "WHERE j.v066 >= '" + _periodFrom + "' " +
                "  AND j.v066 <= '" + _periodTo   + "' " +
                "  AND j.v070 IS NOT NULL " +
                "ORDER BY j.v070";

            // Build the set of accounts that are in the setup list and have
            // journal entries in the period — same logic as VB6 Form_Load.
            var dt = new DataTable();
            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sqlAccounts, conn))
                adapter.Fill(dt);

            var addedKeys = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < 10; i++)
            {
                string acctKey = _rekeningNummer[i].TrimEnd();
                if (string.IsNullOrEmpty(acctKey) || addedKeys.Contains(acctKey)) continue;

                // Check whether any journal entry for this account falls within period
                string sqlChk =
                    "SELECT TOP 1 v070 FROM Journalen " +
                    "WHERE v070 >= '" + VSet(acctKey, 7) + _periodFrom + "' " +
                    "  AND v070 <= '" + VSet(acctKey, 7) + _periodTo   + "'";

                string found = "";
                using (var conn = new OleDbConnection(oleDbConnect))
                using (var cmd  = new OleDbCommand(sqlChk, conn))
                {
                    conn.Open();
                    object val = cmd.ExecuteScalar();
                    if (val != null && val != DBNull.Value) found = val.ToString();
                }

                if (string.IsNullOrEmpty(found)) continue;
                if (!found.StartsWith(acctKey, StringComparison.Ordinal)) continue;

                string acctName = GetLedgerAccountName(acctKey);
                AccountComboBox.Items.Add(VSet(acctKey, 7) + " | " + acctName);
                addedKeys.Add(acctKey);
            }

            if (AccountComboBox.Items.Count > 0)
                AccountComboBox.SelectedIndex = 0;

            ButtonGenerateReport.Enabled = ExtractsListBox.Items.Count > 0;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Period text box
        // ═══════════════════════════════════════════════════════════════════════
        private void PeriodTextBox_Enter(object sender, EventArgs e)
            => PeriodTextBox.SelectAll();

        private void PeriodTextBox_Leave(object sender, EventArgs e)
        {
            if (ActiveControl == ExtractsListBox ||
                ActiveControl == AccountComboBox ||
                ActiveControl == ButtonGenerateReport ||
                ActiveControl == ButtonClose) return;

            string a = PeriodTextBox.Text;
            if (a.Length != 23 || DateInvalid(a.Substring(0, 10)) || DateInvalid(a.Substring(13)))
            {
                MessageBox.Show("Respecteer :\n\nDD/MM/JJJJ - DD/MM/JJJJ a.u.b. !");
                PeriodTextBox.Text = FormattedFromTo(_periodFrom, _periodTo);
                PeriodTextBox.Focus();
                return;
            }
            _periodFrom = a.Substring(6, 4) + a.Substring(3, 2) + a.Substring(0, 2);
            _periodTo   = a.Substring(19, 4) + a.Substring(16, 2) + a.Substring(13, 2);
            RefreshAccountComboBox();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Print date text box
        // ═══════════════════════════════════════════════════════════════════════
        private void ProcessingDateTextBox_Enter(object sender, EventArgs e)
            => ProcessingDateTextBox.SelectAll();

        private void ProcessingDateTextBox_Leave(object sender, EventArgs e)
        {
            if (DateInvalid(ProcessingDateTextBox.Text))
            {
                System.Media.SystemSounds.Beep.Play();
                ProcessingDateTextBox.Text = MIM_GLOBAL_DATE;
                ProcessingDateTextBox.Focus();
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Account ComboBox — fill ExtractsListBox when selection changes
        // ═══════════════════════════════════════════════════════════════════════
        private void AccountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountComboBox.SelectedIndex < 0) return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ExtractsListBox.Visible = false;
                ExtractsListBox.Items.Clear();

                string acctKey = SelectedAccountKey();

                // Load all journal records with v070 starting with acctKey in period
                // v070 = account(7) + date(8); v038 = bank extract reference (8 chars)
                string sql =
                    "SELECT v038, v066, v067, v068, v033 " +
                    "FROM Journalen " +
                    "WHERE v070 >= '" + VSet(acctKey, 7) + _periodFrom + "' " +
                    "  AND v070 <= '" + VSet(acctKey, 7) + _periodTo   + "' " +
                    "ORDER BY v070";

                var dt = new DataTable();
                using (var conn    = new OleDbConnection(oleDbConnect))
                using (var adapter = new OleDbDataAdapter(sql, conn))
                    adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    string v038 = FieldVal(row, "v038");
                    if (v038.Length != 8) continue;  // VB6: skip rows without valid extract ref

                    string v066   = FieldVal(row, "v066");
                    string v067   = FieldVal(row, "v067");
                    double v068   = FieldDouble(row, "v068");

                    string line = v038 + " | "
                        + DateText(v066) + " | "
                        + VSet(v067, 30) + "|"
                        + Dec(v068 >= 0 ? v068 : 0,       MASK_EURBH) + "|"
                        + Dec(v068 <  0 ? Math.Abs(v068) : 0, MASK_EURBH);

                    if (!ExtractsListBox.Items.Contains(line))
                        ExtractsListBox.Items.Add(line);
                }

                if (ExtractsListBox.Items.Count > 0)
                    ExtractsListBox.SelectedIndex = 0;

                ExtractsListBox.Visible = true;
                ButtonGenerateReport.Enabled = ExtractsListBox.Items.Count > 0;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // ExtractsListBox events
        // ═══════════════════════════════════════════════════════════════════════
        private void ExtractsListBox_GotFocus(object sender, EventArgs e)
            => SnelHelpPrint("[Enter] of dubbelklikken voor detail.", BL_LOGGING);

        private void ExtractsListBox_DoubleClick(object sender, EventArgs e)
            => BeginInvoke((Action)(() => ShowExtractDetail(ExtractRef(), manualMode: true)));

        private void ExtractsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                BeginInvoke((Action)(() => ShowExtractDetail(ExtractRef(), manualMode: true)));
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Manual journal lookup
        // ═══════════════════════════════════════════════════════════════════════
        private void ButtonManualJournal_Click(object sender, EventArgs e)
        {
            using (var dlg = new SharedForms.FormNTInputbox())
            {
                dlg.Text      = "Financieel journaal kontrole";
                dlg.InputText = "";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                string input = dlg.InputText.Trim();
                if (input.Length != 8)
                {
                    MessageBox.Show(
                        "dokumentnummer bestaat uit 8 tekens !\n\n" +
                        "Voorbeeld:\nRekening 'GB', werkelijk jaar 19'98',\n" +
                        "uittreksel 124, geeft als dokumentnummer:\n\nGB980124");
                    return;
                }
                ShowExtractDetail(input, manualMode: true);
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Buttons
        // ═══════════════════════════════════════════════════════════════════════
        private void ButtonClose_Click(object sender, EventArgs e) => Close();

        private void ButtonCtrl_Click(object sender, EventArgs e)
        {
            // Debug / ctrl button — show Ktrl value (VB6: cmdCKTRL)
            MessageBox.Show("Ktrl = " + Ktrl);
        }

        private void ButtonGenerateReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                GenerateReport();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Detail screen — show journal lines for one extract (VB6: DetailFinancieelStuk)
        // ═══════════════════════════════════════════════════════════════════════
        private void ShowExtractDetail(string extractRef, bool manualMode = false)
        {
            if (string.IsNullOrEmpty(extractRef)) return;

            string acctKey = SelectedAccountKey();

            string sql =
                "SELECT j.v066, j.v019, r.v020, j.v068, j.v067, j.v033, j.v069 " +
                "FROM Journalen j " +
                "LEFT JOIN Rekeningen r ON r.v019 = j.v019 " +
                "WHERE j.v038 = '" + extractRef + "' " +
                "ORDER BY j.v019";

            var dt = new DataTable();
            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sql, conn))
                adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Geen journaallijnen voor " + extractRef);
                return;
            }

            // Build rows for the XLog grid — filter same as VB6 PrintInfo/VolgendeLijn
            var rows = new System.Collections.Generic.List<string[]>();
            foreach (DataRow row in dt.Rows)
            {
                string v019 = FieldVal(row, "v019");
                string v066 = FieldVal(row, "v066");

                // Skip the bank account's own contra line
                if (VSet(v019, 7) == acctKey) continue;

                // In normal mode: only include lines whose date matches the selected extract date
                string extractDate = ExtractDate();
                if (!manualMode && DateText(v066) != extractDate)
                    continue;

                // In normal mode: skip if account is not empty and counter-account ≠ acctKey
                if (!manualMode && !string.IsNullOrWhiteSpace(v019))
                {
                    string v069 = FieldVal(row, "v069");
                    if (VSet(v069, 7) != acctKey) continue;
                }

                string acctName = row.IsNull("v020") ? "//" : row["v020"].ToString();
                rows.Add(new string[]
                {
                    DateText(v066),
                    v019,
                    acctName,
                    FieldVal(row, "v068"),
                    FieldVal(row, "v067"),
                    FieldVal(row, "v033"),
                    FieldVal(row, "v069")
                });
            }

            // Display in a FormXLog-style grid
            using (var xLog = new SharedForms.FormXLog())
            {
                xLog.Text += ", Journaaldetail voor uittreksel : " + extractRef;

                string[] headers = { "Datum (v066)", "Rekening (v019)", "Naam (v020)",
                                     "Bedrag (v068)", "Boekingsomschrijving (v067)",
                                     "AV Dokum. (v033)", "TegenRek. (v069)" };

                foreach (string header in headers)
                    xLog.X.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = header, Name = header });

                foreach (string[] r in rows)
                    xLog.X.Rows.Add(r);

                xLog.ShowDialog(this);
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Column layout
        // ═══════════════════════════════════════════════════════════════════════
        private void InitVelden()
        {
            _reportField[0] = "Datum";          _reportTab[0] = 2;
            _reportField[1] = "Rek.Nm.";        _reportTab[1] = 13;
            _reportField[2] = "Naam/Omschrijving"; _reportTab[2] = 21;
            _reportField[3] = "Betreft";        _reportTab[3] = 62;
            _reportField[4] = "       Debet";   _reportTab[4] = 93;
            _reportField[5] = "      Credit";   _reportTab[5] = 105;
            _reportField[6] = "Document";       _reportTab[6] = 118;
            _reportTab[7] = 0;

            _reportTitle = new string(' ', 128);
            for (int t = 0; _reportTab[t] != 0; t++)
                _reportTitle = SafeInsert(_reportTitle, _reportTab[t], _reportField[t]);
            _reportTitle = _reportTitle.Substring(0, 128);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report — open / close (VPE)
        // ═══════════════════════════════════════════════════════════════════════
        private void OpenReport()
        {
            if (Mim.Report.IsOpen()) Mim.Report.CloseDoc();
            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = IDEALSoftware.VpeCommunity.GUILanguage.Dutch;
            Mim.Report.Title       = "Financieel Boek";
            _pageCounter           = 0;
        }

        private void CloseReport()
        {
            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.Preview();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report — header
        // ═══════════════════════════════════════════════════════════════════════
        private void ReportPrintHeader()
        {
            Mim.Report.SelectFont("Courier New", (int)7.2);
            Mim.Report.TextBold      = true;
            Mim.Report.TextColor     = ColorTranslator.FromOle(0);
            Mim.Report.nTopMargin    = 1;
            Mim.Report.nBottomMargin = 29;
            Mim.Report.nLeftMargin   = 0.5;
            Mim.Report.nRightMargin  = 0.5;
            Mim.Report.PenSize       = 0.01;

            _pageCounter++;
            _ypos = Mim.Report.Print(1,  1,     _reportText[1]);
            _ypos = Mim.Report.Print(17, 1,     "Pagina : " + Dec(_pageCounter, "##########"));
            _ypos = Mim.Report.Print(17, _ypos, "Datum  : " + _reportText[0]);
            _ypos = Mim.Report.Print(1,  _ypos, _reportText[3].ToUpper());
            _ypos = Mim.Report.Print(1,  _ypos, _fullLine);
            _ypos = Mim.Report.Print(1,  _ypos, _reportTitle);
            _ypos = Mim.Report.Print(1,  _ypos, _fullLine);
        }

        private void CheckPageBreak()
        {
            if (_ypos > 27.5)
            {
                Mim.Report.PageBreak();
                ReportPrintHeader();
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report — print one data row (VeldTXT fields at report tab positions)
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintVelden()
        {
            string line = new string(' ', 128);
            for (int t = 0; _reportTab[t] != 0; t++)
                line = SafeInsert(line, _reportTab[t], _veldTxt[t] ?? "");
            _ypos = Mim.Report.Print(1, _ypos, line.Substring(0, 128));
            CheckPageBreak();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report — totals row
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintTotaal()
        {
            for (int t = 0; t < 8; t++) _veldTxt[t] = "";
            _veldTxt[3] = "Periodiek totaal :";
            _veldTxt[4] = Dec(Math.Abs(_totalDebit),  MASK_EURBH);
            _veldTxt[5] = Dec(Math.Abs(_totalCredit), MASK_EURBH);

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            PrintVelden();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report — cumulative / square-check page
        // ═══════════════════════════════════════════════════════════════════════
        private void CumulPrint()
        {
            Mim.Report.PageBreak();
            ReportPrintHeader();

            _ypos = Mim.Report.Print(1, _ypos, "\n");
            _ypos = Mim.Report.Print(1, _ypos, "  ** CENTRALISATIE/VIERKANTSCONTROLE **");
            _ypos = Mim.Report.Print(1, _ypos, "\n");

            if (_cumulData.Count == 0) return;

            bool firstEntry = true;
            int  tabul      = 0;
            string curLine  = new string(' ', 128);

            foreach (var kv in _cumulData)
            {
                string naam  = GetLedgerAccountName(kv.Key.TrimEnd());
                string entry = Dec(kv.Value.count, "####") + " x "
                    + VSet(kv.Key, 7) + " "
                    + VSet(naam, 30)  + " "
                    + Dec(kv.Value.amount, MASK_EURBH);

                if (firstEntry)
                {
                    curLine    = SafeInsert(curLine, 2, entry);
                    firstEntry = false;
                    tabul      = 59;
                }
                else if (tabul == 59)
                {
                    curLine = SafeInsert(curLine, tabul + 2, entry);
                    _ypos   = Mim.Report.Print(1, _ypos, curLine.Substring(0, 128));
                    curLine = new string(' ', 128);
                    CheckPageBreak();
                    tabul   = 0;
                }
                else
                {
                    curLine = SafeInsert(curLine, 2, entry);
                    tabul   = 59;
                }
            }

            // Flush orphaned left-column entry
            if (!firstEntry && tabul == 59)
            {
                _ypos = Mim.Report.Print(1, _ypos, curLine.Substring(0, 128));
                CheckPageBreak();
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Main generation (VB6: Drukken_Click)
        // ═══════════════════════════════════════════════════════════════════════
        private void GenerateReport()
        {
            if (ExtractsListBox.Items.Count == 0) return;

            _totalDebit  = 0;
            _totalCredit = 0;
            _errorMsg    = "";
            _cumulData.Clear();

            InitVelden();

            // Report header texts  (VB6: psTekst(0..3))
            string companyName = "";
            int br = Mim.Text.IndexOf('['), bc = Mim.Text.IndexOf(']');
            if (br >= 0 && bc > br) companyName = Mim.Text.Substring(br + 1, bc - br - 1);

            _reportText[0] = ProcessingDateTextBox.Text;
            _reportText[1] = "Financieel Boek " + (br >= 0 ? Mim.Text.Substring(br) : "");
            _reportText[3] = AccountComboBox.SelectedItem?.ToString() ?? "";

            OpenReport();
            ReportPrintHeader();

            // Iterate each extract line
            for (int i = 0; i < ExtractsListBox.Items.Count; i++)
            {
                string extractLine = ExtractsListBox.Items[i].ToString().PadRight(81);
                string extractRef  = extractLine.Substring(0, 8);

                // Fill _veldTxt from the listbox entry (same layout built in AccountComboBox_SelectedIndexChanged)
                // Layout: v038(0-7) | " | "(8-10) | date(11-20) | " | "(21-23) | desc(24-53) | "|"(54) | deb(55-66) | "|"(67) | crd(68-79)
                for (int t = 0; t < 8; t++) _veldTxt[t] = "";
                _veldTxt[0] = extractLine.Substring(11, 10);     // date DD/MM/YYYY
                _veldTxt[1] = SelectedAccountKey().TrimEnd();     // account
                _veldTxt[2] = extractLine.Substring(24, 30);     // description
                _veldTxt[3] = "DS/CS Saldo van het uittreksel";

                // Parse debet/credit from the fixed layout positions
                string debStr = extractLine.Substring(55, 12).Trim();
                string crdStr = extractLine.Substring(68, 12).Trim();
                double deb    = ParseDecAmount(debStr);
                double crd    = ParseDecAmount(crdStr);

                _veldTxt[4] = deb > 0 ? Dec(deb, MASK_EURBH) : "";
                _veldTxt[5] = crd > 0 ? Dec(crd, MASK_EURBH) : "";
                _veldTxt[6] = extractRef;

                double hetBedrag = deb - crd;
                if (hetBedrag < 0) _totalCredit += Math.Abs(hetBedrag); else _totalDebit += hetBedrag;

                AccumulateCumul(SelectedAccountKey(), hetBedrag);
                PrintVelden();

                // Print journal detail lines for this extract
                bool isLast = (i == ExtractsListBox.Items.Count - 1);
                if (isLast)
                {
                    PrintJournaalDetail(extractRef, extractLine);
                }
                else
                {
                    // Duplicate-extract detection (VB6 ErrorMsg logic)
                    string nextLine = ExtractsListBox.Items[i + 1].ToString();
                    if (extractLine.Substring(0, 21) == nextLine.Substring(0, 21))
                        _errorMsg += extractLine.Substring(0, 21) + " / " + nextLine.Substring(0, 21) + " onlogica.\n";
                    else
                        PrintJournaalDetail(extractRef, extractLine);
                }
            }

            PrintTotaal();
            CumulPrint();

            // Error / balance summary
            bool balanced = Math.Abs(_totalDebit - _totalCredit) < 0.005;
            if (!balanced)
                _errorMsg += "DEBET<>CREDIT : de boekhoudDATABASE instabiel ?  Dient geRESTORED te worden !!!\n\n";

            if (!string.IsNullOrEmpty(_errorMsg))
            {
                string msg = "\n\nEr zijn uittreksels onjuist ingebracht.  De boekhouding blijft gelukkig correct\n\n"
                    + _errorMsg + "\n"
                    + "De gebruiker doet er goed aan steeds te kontroleren :\n"
                    + "* Één Bankuittreksels pér UITTREKSELDATUM !\n"
                    + "* Steeds datum kontroleren vooraleer weg te schrijven !!!\n\n"
                    + "Indien DEBET=CREDIT is de boekhouding toch correct bijgewerkt en hoeft U voor huidige onregelmatigheden niets recht te zetten";

                _ypos = Mim.Report.Print(1, _ypos, msg);
            }

            CloseReport();
            Close();
        }

        // ── Print journal detail lines for one extract ────────────────────────
        private void PrintJournaalDetail(string extractRef, string extractLine)
        {
            string acctKey = SelectedAccountKey();

            string sql =
                "SELECT j.v019, r.v020, j.v066, j.v067, j.v068, j.v033, j.v069 " +
                "FROM Journalen j " +
                "LEFT JOIN Rekeningen r ON r.v019 = j.v019 " +
                "WHERE j.v038 = '" + extractRef + "' " +
                "ORDER BY j.v019";

            var dt = new DataTable();
            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sql, conn))
                adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("onlogische situatie voor " + extractRef);
                return;
            }

            _ypos = Mim.Report.Print(1, _ypos, "\n");   // blank line before details

            foreach (DataRow row in dt.Rows)
            {
                string v019 = FieldVal(row, "v019");

                // Skip the bank account's own contra line
                if (VSet(v019, 7) == acctKey) continue;

                string acctName = row.IsNull("v020") ? "Reeds vernietigd..." : row["v020"].ToString();
                string v067     = FieldVal(row, "v067");
                string v033     = FieldVal(row, "v033");
                double dc       = FieldDouble(row, "v068");

                AccumulateCumul(v019, dc);
                               

        // One detail line per record, aligned with InitVelden column headers:
        //  col 13 = v019, col 21 = v020, col 62 = v067,
        //  col 93 = debit, col 105 = credit, col 118 = v033
        string line = new string(' ', 128);
                line = SafeInsert(line,  13, v019);
                line = SafeInsert(line,  21, acctName.Length > 41 ? acctName.Substring(0, 41) : acctName);
                line = SafeInsert(line,  62, v067.Length     > 31 ? v067.Substring(0, 31)     : v067);
                line = SafeInsert(line,  93, dc > 0 ? Dec(dc,           MASK_EURBH) : "");
                line = SafeInsert(line, 105, dc < 0 ? Dec(Math.Abs(dc), MASK_EURBH) : "");
                line = SafeInsert(line, 118, v033.Length     > 10 ? v033.Substring(0, 10)     : v033);

                _totalDebit += dc > 0 ? dc : 0;
                _totalCredit += dc < 0 ? Math.Abs(dc) : 0;

                _ypos = Mim.Report.Print(1, _ypos, line.Substring(0, 128));
                CheckPageBreak();
            }

            _ypos = Mim.Report.Print(1, _ypos, "\n");  // blank line after details
            CheckPageBreak();
        }

        // ── Square-check accumulator ──────────────────────────────────────────
        private void AccumulateCumul(string account, double amount)
        {
            string key = VSet(account, FLINDEX_LEN[TABLE_LEDGERACCOUNTS, 0]);
            if (_cumulData.TryGetValue(key, out var entry))
                _cumulData[key] = (entry.count + 1, entry.amount + amount);
            else
                _cumulData[key] = (1, amount);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════════════════
        private string FormattedFromTo(string from, string to)
            => DateText(from) + " - " + DateText(to);

        private string SelectedAccountKey()
        {
            string item = AccountComboBox.SelectedItem?.ToString() ?? "";
            return item.Length >= 7 ? item.Substring(0, 7) : VSet(item, 7);
        }

        private string ExtractRef()
        {
            if (ExtractsListBox.SelectedIndex < 0) return "";
            string line = ExtractsListBox.SelectedItem?.ToString() ?? "";
            return line.Length >= 8 ? line.Substring(0, 8) : "";
        }

        private string ExtractDate()
        {
            if (ExtractsListBox.SelectedIndex < 0) return "";
            string line = ExtractsListBox.SelectedItem?.ToString() ?? "";
            return line.Length >= 22 ? line.Substring(12, 10) : "";
        }

        private string GetLedgerAccountName(string acctKey)
        {
            string sql = "SELECT v020 FROM Rekeningen WHERE v019 LIKE '" + acctKey.Trim() + "%'";
            using (var conn = new OleDbConnection(oleDbConnect))
            using (var cmd  = new OleDbCommand(sql, conn))
            {
                conn.Open();
                object val = cmd.ExecuteScalar();
                return val == null || val == DBNull.Value ? "Rekening reeds vernietigd !!!" : val.ToString();
            }
        }

        private static string SafeInsert(string s, int pos, string ins)
        {
            if (pos < 0 || pos >= s.Length || string.IsNullOrEmpty(ins)) return s;
            string result = s.Substring(0, pos) + ins + s.Substring(Math.Min(s.Length, pos + ins.Length));
            return result.Length > 128 ? result.Substring(0, 128) : result;
        }

        private static string FieldVal(DataRow row, string col)
        {
            if (!row.Table.Columns.Contains(col)) return "";
            return row.IsNull(col) ? "" : row[col].ToString();
        }

        private static double FieldDouble(DataRow row, string col)
        {
            if (!row.Table.Columns.Contains(col) || row.IsNull(col)) return 0;
            return double.TryParse(row[col].ToString(), out double d) ? d : 0;
        }

        // VB6: SnelHelpPrint wrapper
        private static void SnelHelpPrint(string msg, bool log)
            => Classes.MimEnvironment.SnelHelpPrint(msg, log);

        // Parses a string produced by Dec(value, MASK_EURBH) back to a double.
        // Dec() outputs BOTH thousands and decimal separators as '.', e.g. "   1.234.56"
        // (it calls .ToString(mask) under the Belgian/Dutch locale which uses ',' as decimal,
        // then replaces all ',' with '.', leaving dots everywhere).
        // The LAST dot is always the decimal separator; all preceding dots are thousands separators.
        private static double ParseDecAmount(string s)
        {
            s = s.Trim();
            if (string.IsNullOrEmpty(s)) return 0;
            int lastDot = s.LastIndexOf('.');
            if (lastDot < 0)
                return double.TryParse(s, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double d0) ? d0 : 0;
            string intPart = s.Substring(0, lastDot).Replace(".", "");
            string decPart = s.Substring(lastDot + 1);
            return double.TryParse(intPart + "." + decPart,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double d) ? d : 0;
        }
    }
}

