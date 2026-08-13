using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Filing
{
    public partial class FormBasicTableReporting : Form
    {
        // ── module-level state (VB6 module variables) ────────────────────────
        private int    _tabLijn    = 0;   // running tab position while building report definition
        private int    _flKeuze   = 1;   // currently selected table index (1..5)
        private int    _indexKeuze = 0;  // currently selected sort index

        // PRD files live alongside the other definition files
        private string PrdDir => PROGRAM_LOCATION + @"Content\Prd\";

        // ── constructor ──────────────────────────────────────────────────────
        public FormBasicTableReporting()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ════════════════════════════════════════════════════════════════════
        // Form load
        // ════════════════════════════════════════════════════════════════════
        private void FormBasicTableReporting_Load(object sender, EventArgs e)
        {
            // Table list (VB6 index 1..5 maps to TABLE_CUSTOMERS..TABLE_CONTRACTS)
            cmbTabel.Items.Add("1: Klanten");
            cmbTabel.Items.Add("2: Leveranciers");
            cmbTabel.Items.Add("3: Rekeningen");
            cmbTabel.Items.Add("4: Produkten");
            cmbTabel.Items.Add("5: Polissen");

            // Formatting options (char at position 0 is the format code)
            cmbFormattering.Items.Add("T: Tekst zonder enige bewerking");
            cmbFormattering.Items.Add("D: Van SorteerDatum naar DD/MM/EEJJ");
            cmbFormattering.Items.Add("0: Bedrag met masker ########0");
            cmbFormattering.Items.Add("1: Bedrag met masker ###0");
            cmbFormattering.Items.Add("2: Bedrag met masker ######0.00");
            cmbFormattering.Items.Add("3: Bedrag met masker ##0.00000000");
            cmbFormattering.Items.Add("4: Bedrag met masker #######0.00");
            cmbFormattering.Items.Add("5: Bedrag met masker ##0");
            cmbFormattering.Items.Add("6: Bedrag met masker #0");
            cmbFormattering.Items.Add("7: Bedrag met masker #####0.0");
            cmbFormattering.Items.Add("Z: Rekenformule */+-() via Titel!");

            cmbTabel.SelectedIndex = 0;
        }

        // ════════════════════════════════════════════════════════════════════
        // Table combo — populate sort indexes and PRD list
        // ════════════════════════════════════════════════════════════════════
        private void CmbTabel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTabel.SelectedIndex < 0) return;

            cmbSortering.Items.Clear();
            _flKeuze = int.Parse(cmbTabel.Text.Substring(0, 1));

            for (int t = 0; t <= FL_NUMBEROFINDEXEN[_flKeuze]; t++)
                cmbSortering.Items.Add(t.ToString("D2") + ":" + FLINDEX_CAPTION[_flKeuze, t]);

            cmbSortering.SelectedIndex = 0;

            // Load PRD definitions for this table
            LoadRapportDefinities();
        }

        // ════════════════════════════════════════════════════════════════════
        // Sort combo — update key range defaults
        // ════════════════════════════════════════════════════════════════════
        private void CmbSortering_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSortering.SelectedIndex < 0) return;

            _indexKeuze = int.Parse(cmbSortering.Text.Substring(0, 2));
            int keyLen = FLINDEX_LEN[_flKeuze, _indexKeuze];
            txtKeyLen.Text = keyLen.ToString();
            txtVan.Text    = "0";
            txtTot.Text    = new string('z', keyLen);
            SqlRefresh();
        }

        // ════════════════════════════════════════════════════════════════════
        // Rapport definition combo
        // ════════════════════════════════════════════════════════════════════
        private void CmbRapportDefinitie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRapportDefinitie.SelectedIndex < 0) return;

            string tablePrefix = _flKeuze.ToString("D3");
            string defId       = cmbRapportDefinitie.Text.Substring(0, 2);
            string path        = PrdDir + tablePrefix + defId + ".PRD";

            if (!File.Exists(path))
            {
                MessageBox.Show(tablePrefix + defId + ".PRD bestaat niet meer...");
                return;
            }

            try
            {
                using (var sr = new StreamReader(path))
                {
                    txtRapportnaam.Text = sr.ReadLine() ?? "";
                    lstRapportVelden.Items.Clear();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                            lstRapportVelden.Items.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij lezen PRD: " + ex.Message);
            }

            SqlRefresh();
        }

        private void CmbRapportDefinitie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (cmbRapportDefinitie.SelectedIndex < 0) return;
                string msg = "Rapportdefinitie\n\n" + cmbRapportDefinitie.Text + "\n\nverwijderen.  Bent U zeker ?";
                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string tablePrefix = _flKeuze.ToString("D3");
                    string defId       = cmbRapportDefinitie.Text.Substring(0, 2);
                    string path        = PrdDir + tablePrefix + defId + ".PRD";
                    try { File.Delete(path); } catch { }
                    cmbRapportDefinitie.Items.RemoveAt(cmbRapportDefinitie.SelectedIndex);
                }
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // Afdrukken — build SQL, run query, print via Mim.Report
        // ════════════════════════════════════════════════════════════════════
        private void BtnAfdrukken_Click(object sender, EventArgs e)
        {
            if (lstRapportVelden.Items.Count == 0) return;

            string sql = BuildSql();
            if (string.IsNullOrEmpty(sql)) return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var dt = new DataTable();
                using (var conn    = new OleDbConnection(oleDbConnect))
                using (var adapter = new OleDbDataAdapter(sql, conn))
                    adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Geen gegevens gevonden voor de opgegeven selectie.");
                    return;
                }

                OpenReport();
                PrintTitel(dt);

                foreach (DataRow row in dt.Rows)
                    PrintVelden(row);

                CloseReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij rapport:\n" + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
                
        // ════════════════════════════════════════════════════════════════════
        // Toon SQL
        // ════════════════════════════════════════════════════════════════════
        private void BtnToonSQL_Click(object sender, EventArgs e)
            => MessageBox.Show(BuildSql(), "SQL SELECT Definitie");

        // ════════════════════════════════════════════════════════════════════
        // SQL Overname → FormSQLOperations
        // ════════════════════════════════════════════════════════════════════
        private void BtnSQLOvername_Click(object sender, EventArgs e)
        {
            string sql = BuildSql();
            if (string.IsNullOrEmpty(sql)) return;

            // Find or open FormSQLOperations and pass the SQL
            foreach (Form f in Application.OpenForms)
            {
                if (f is marVSS2028.Forms.FormSQLOperations ops)
                {
                    ops.SetSql(sql);
                    ops.BringToFront();
                    return;
                }
            }
            var newOps = new marVSS2028.Forms.FormSQLOperations();
            newOps.MdiParent = this.MdiParent ?? Mim;
            newOps.SetSql(sql);
            newOps.Show();
        }

        // ════════════════════════════════════════════════════════════════════
        // Definitie — toggle definition-edit mode and save new PRD
        // ════════════════════════════════════════════════════════════════════
        //private void BtnDefinitie_Click(object sender, EventArgs e)
        //{
        //    if (btnAfdrukken.Enabled)
        //    {
        //        // Enter definition mode: load TELEBIB fields for current table
        //        if (!TLBPag2(_flKeuze.ToString("D3")))
        //        {
        //            System.Media.SystemSounds.Beep.Play();
        //            return;
        //        }

        //        lstTabelVelden.Items.Clear();
        //        int t = 0;
        //        while (!string.IsNullOrWhiteSpace(TELEBIB_CODE[t]))
        //        {
        //            lstTabelVelden.Items.Add(
        //                TELEBIB_CODE[t] + " 000 " + TELEBIB_LENGTH[t].ToString("D3") + " T " + TELEBIB_TEXT[t]);
        //            t++;
        //        }

        //        _tabLijn = 0;
        //        SetDefinitionMode(true);
        //        WindowState = FormWindowState.Maximized;
        //        txtRapportnaam.Focus();
        //    }
        //    else
        //    {
        //        // Exit definition mode — save PRD
        //        SetDefinitionMode(false);
        //        WindowState = FormWindowState.Normal;

        //        int volgnummer = cmbRapportDefinitie.Items.Count;
        //        string tablePrefix = _flKeuze.ToString("D3");
        //        string path = PrdDir + tablePrefix + volgnummer.ToString("D2") + ".PRD";

        //        try
        //        {
        //            using (var sw = new StreamWriter(path, false))
        //            {
        //                sw.WriteLine(txtRapportnaam.Text);
        //                foreach (string item in lstRapportVelden.Items)
        //                    sw.WriteLine(item);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Fout bij opslaan definitie: " + ex.Message);
        //            return;
        //        }

        //        string displayEntry = volgnummer.ToString("D2") + ": " + txtRapportnaam.Text;
        //        cmbRapportDefinitie.Items.Add(displayEntry);
        //        cmbRapportDefinitie.SelectedIndex = cmbRapportDefinitie.Items.Count - 1;
        //    }
        //}

        //private void SetDefinitionMode(bool editMode)
        //{
        //    btnAfdrukken.Enabled         = !editMode;
        //    lstTabelVelden.Enabled       = editMode;
        //    lstRapportVelden.Enabled     = editMode;
        //    txtRapportnaam.Enabled       = editMode;
        //    cmbTabel.Enabled             = !editMode;
        //    cmbSortering.Enabled         = !editMode;
        //    cmbRapportDefinitie.Enabled  = !editMode;
        //    btnToevoegen.Enabled         = editMode;
        //    btnTitel.Enabled             = false;
        //    btnFormattering.Enabled      = false;
        //    btnTabPositie.Enabled        = false;
        //}

        // ════════════════════════════════════════════════════════════════════
        // DefEdit buttons
        // ════════════════════════════════════════════════════════════════════
        private void BtnToevoegen_Click(object sender, EventArgs e)
        {
            // Add all selected TabelVelden items to RapportVelden
            for (int i = 0; i < lstTabelVelden.Items.Count; i++)
            {
                if (!lstTabelVelden.GetSelected(i)) continue;

                string tempoString = lstTabelVelden.Items[i].ToString();
                int fieldLen = SafeParseInt(tempoString, 15, 3);

                if (_tabLijn + fieldLen > 128)
                    _tabLijn = 1;
                else
                    _tabLijn += 1;

                // overwrite tabpos in the string (chars 11..13, 0-based)
                tempoString = SetMid(tempoString, 11, 3, _tabLijn.ToString("D3"));
                lstRapportVelden.Items.Add(tempoString);
                _tabLijn += fieldLen;
            }
        }

        private void BtnTitel_Click(object sender, EventArgs e)
        {
            if (lstRapportVelden.SelectedIndex < 0) { System.Media.SystemSounds.Beep.Play(); return; }
            // Pre-fill txtTitelEdit with the current title (chars from position 21+, 0-based)
            string current = lstRapportVelden.SelectedItem.ToString();
            txtTitelEdit.Text = current.Length > 21 ? current.Substring(21) : "";
            OverlayShow(txtTitelEdit);
        }

        private void BtnFormattering_Click(object sender, EventArgs e)
        {
            if (lstRapportVelden.SelectedIndex < 0) { System.Media.SystemSounds.Beep.Play(); return; }
            // Pre-select current format
            string current = lstRapportVelden.SelectedItem.ToString();
            char fmt = current.Length > 19 ? current[19] : 'T';
            for (int t = 0; t < cmbFormattering.Items.Count; t++)
            {
                if (cmbFormattering.Items[t].ToString()[0] == fmt)
                {
                    cmbFormattering.SelectedIndex = t;
                    break;
                }
            }
            OverlayShow(cmbFormattering);
        }

        private void BtnTabPositie_Click(object sender, EventArgs e)
        {
            if (lstRapportVelden.SelectedIndex < 0) { System.Media.SystemSounds.Beep.Play(); return; }
            string current = lstRapportVelden.SelectedItem.ToString();
            txtTabPosEdit.Text = current.Length > 13 ? current.Substring(11, 3).Trim() : "";
            OverlayShow(txtTabPosEdit);
        }

        // ════════════════════════════════════════════════════════════════════
        // TabelVelden listbox
        // ════════════════════════════════════════════════════════════════════
        private void LstTabelVelden_GotFocus(object sender, EventArgs e)
        {
            btnToevoegen.Enabled  = true;
            btnTitel.Enabled      = false;
            btnFormattering.Enabled = false;
            btnTabPositie.Enabled = false;
        }

        private void LstTabelVelden_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTabelVelden.SelectedIndex >= 0)
                SnelHelpPrint(lstTabelVelden.SelectedItem?.ToString() ?? "", BL_LOGGING);
        }

        // ════════════════════════════════════════════════════════════════════
        // RapportVelden listbox
        // ════════════════════════════════════════════════════════════════════
        private void LstRapportVelden_GotFocus(object sender, EventArgs e)
        {
            btnToevoegen.Enabled = false;
            if (lstRapportVelden.Items.Count == 0)
            {
                _tabLijn = 0;
            }
            else
            {
                btnTitel.Enabled        = true;
                btnFormattering.Enabled = true;
                btnTabPositie.Enabled   = true;
            }
        }

        private void LstRapportVelden_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstRapportVelden.Items.Count == 0) { _tabLijn = 0; return; }
            if (e.KeyCode == Keys.Delete && lstRapportVelden.SelectedIndex >= 0)
                lstRapportVelden.Items.RemoveAt(lstRapportVelden.SelectedIndex);
        }

        // ════════════════════════════════════════════════════════════════════
        // Overlay controls — txtTitelEdit, txtTabPosEdit, cmbFormattering
        // ════════════════════════════════════════════════════════════════════
        private void OverlayShow(Control ctrl)
        {
            txtTitelEdit.Visible   = false;
            txtTabPosEdit.Visible  = false;
            cmbFormattering.Visible = false;
            ctrl.Visible = true;
            ctrl.Focus();
        }

        private void TxtTitelEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            int pos = lstRapportVelden.SelectedIndex;
            if (pos < 0) return;
            string current = lstRapportVelden.Items[pos].ToString();
            // Replace from position 21 onward (title field)
            string updated = (current.Length >= 21 ? current.Substring(0, 21) : current.PadRight(21))
                             + txtTitelEdit.Text;
            lstRapportVelden.Items.RemoveAt(pos);
            lstRapportVelden.Items.Insert(pos, updated);
            lstRapportVelden.SelectedIndex = pos;
            lstRapportVelden.Focus();
        }

        private void TxtTitelEdit_LostFocus(object sender, EventArgs e)
            => txtTitelEdit.Visible = false;

        private void TxtTabPosEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            int pos = lstRapportVelden.SelectedIndex;
            if (pos < 0) return;
            string current = lstRapportVelden.Items[pos].ToString();
            int newTab = int.TryParse(txtTabPosEdit.Text, out int p) ? p : 1;
            string updated = SetMid(current, 11, 3, newTab.ToString("D3"));
            lstRapportVelden.Items.RemoveAt(pos);
            lstRapportVelden.Items.Insert(pos, updated);
            lstRapportVelden.SelectedIndex = pos;
            lstRapportVelden.Focus();
        }

        private void TxtTabPosEdit_LostFocus(object sender, EventArgs e)
            => txtTabPosEdit.Visible = false;

        private void CmbFormattering_LostFocus(object sender, EventArgs e)
        {
            int pos = lstRapportVelden.SelectedIndex;
            if (pos >= 0 && cmbFormattering.SelectedIndex >= 0)
            {
                string current = lstRapportVelden.Items[pos].ToString();
                char   newFmt  = cmbFormattering.Text[0];
                string updated = SetMid(current, 19, 1, newFmt.ToString());
                lstRapportVelden.Items.RemoveAt(pos);
                lstRapportVelden.Items.Insert(pos, updated);
                lstRapportVelden.SelectedIndex = pos;
            }
            cmbFormattering.Visible = false;
            lstRapportVelden.Focus();
        }

        // ════════════════════════════════════════════════════════════════════
        // txtVan / txtTot / txtKeyLen events
        // ════════════════════════════════════════════════════════════════════
        private void Txt_GotFocus(object sender, EventArgs e)
        {
            if (sender is TextBox tb) tb.SelectAll();
        }

        private void TxtVanTot_LostFocus(object sender, EventArgs e)
            => SqlRefresh();

        private void TxtKeyLen_LostFocus(object sender, EventArgs e)
        {
            int maxLen = FLINDEX_LEN[_flKeuze, _indexKeuze];
            if (!int.TryParse(txtKeyLen.Text, out int v) || v < 1 || v > maxLen)
            {
                System.Media.SystemSounds.Beep.Play();
                txtKeyLen.Text = maxLen.ToString();
                txtKeyLen.Focus();
                return;
            }
            SqlRefresh();
        }

        // ════════════════════════════════════════════════════════════════════
        // SQL builder
        // ════════════════════════════════════════════════════════════════════
        private string BuildSql()
        {
            if (lstRapportVelden.Items.Count == 0) return "";
            if (cmbSortering.SelectedIndex < 0) return "";

            string indexField = JETTABLEUSE_INDEX[_flKeuze, _indexKeuze].Trim();
            string tableName  = cmbTabel.Text.Length > 3 ? cmbTabel.Text.Substring(3) : cmbTabel.Text;

            var sb = new System.Text.StringBuilder("SELECT");
            for (int i = 0; i < lstRapportVelden.Items.Count; i++)
            {
                string item      = lstRapportVelden.Items[i].ToString();
                // VB6: Trim$(Mid(item, 5, 5)) — field code is at 1-based pos 5..9 = 0-based index 4, len 5
                string fieldCode = item.Length >= 9 ? item.Substring(4, 5).Trim() : item.Trim();
                string title     = item.Length > 21  ? item.Substring(21).Trim()    : fieldCode;

                if (i > 0) sb.Append(",");
                sb.Append(" " + fieldCode + " AS [" + title + "]");
            }

            sb.Append(" FROM " + tableName);
            sb.Append(" WHERE " + indexField + " >= '" + txtVan.Text.Trim() + "'");
            sb.Append(" AND "   + indexField + " <= '" + txtTot.Text.Trim() + "'");
            sb.Append(" ORDER BY " + indexField);

            return sb.ToString();
        }

        // ════════════════════════════════════════════════════════════════════
        // SqlRefresh — count records and update label
        // ════════════════════════════════════════════════════════════════════
        private void SqlRefresh()
        {
            try
            {
                string sql = BuildSql();
                if (string.IsNullOrEmpty(sql)) return;

                SnelHelpPrint(sql, BL_LOGGING);

                int count = 0;
                using (var conn = new OleDbConnection(oleDbConnect))
                using (var cmd  = new OleDbCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read()) count++;
                    }
                }
                lblAantalInSelektie.Text = count.ToString("#,##0");
            }
            catch
            {
                lblAantalInSelektie.Text = "?";
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // Load PRD file list for current table
        // ════════════════════════════════════════════════════════════════════
        private void LoadRapportDefinities()
        {
            cmbRapportDefinitie.Items.Clear();
            string tablePrefix = _flKeuze.ToString("D3");
            string pattern     = tablePrefix + "??.PRD";
            string[] files     = Directory.GetFiles(PrdDir, pattern);

            foreach (string file in files)
            {
                try
                {
                    string defId = Path.GetFileNameWithoutExtension(file).Substring(3, 2);
                    using (var sr = new StreamReader(file))
                    {
                        string title = sr.ReadLine() ?? "";
                        cmbRapportDefinitie.Items.Add(defId + ": " + title);
                    }
                }
                catch { }
            }

            if (cmbRapportDefinitie.Items.Count > 0)
                cmbRapportDefinitie.SelectedIndex = 0;
        }

        // ════════════════════════════════════════════════════════════════════
        // Report — open / close
        // ════════════════════════════════════════════════════════════════════
        private void OpenReport()
        {
            if (Mim.Report.IsOpen()) Mim.Report.CloseDoc();
            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = IDEALSoftware.VpeCommunity.GUILanguage.Dutch;
            Mim.Report.Title       = "SQL Lijstrapport";
        }

        private void CloseReport()
        {
            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.Preview();
        }

        // ════════════════════════════════════════════════════════════════════
        // Report — header
        // ════════════════════════════════════════════════════════════════════
        private double _ypos;
        private int    _pageCounter;
        private string _fullLine = new string('-', 128);

        private void PrintTitel(DataTable dt)
        {
            _pageCounter = 0;
            _ypos = 0;
            ReportHeader(dt);
        }

        private void ReportHeader(DataTable dt)
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
            string header = Mim.Text ?? "";
            int    bracket = header.IndexOf('[');
            if (bracket >= 0) header = header.Substring(bracket);

            _ypos = Mim.Report.Print(1, _ypos, header
                + new string(' ', Math.Max(0, 100 - header.Length))
                + "Pagina : " + _pageCounter.ToString("D3"));
            _ypos = Mim.Report.Print(1, _ypos, "Datum  : " + MIM_GLOBAL_DATE);
            _ypos = Mim.Report.Print(1, _ypos, txtRapportnaam.Text);
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);

            // Column headers from RapportVelden titles at their tab positions
            string headerLine = new string(' ', 128);
            for (int t = 0; t < lstRapportVelden.Items.Count; t++)
            {
                string item  = lstRapportVelden.Items[t].ToString();
                int    tab   = SafeParseInt(item, 11, 3);
                string title = item.Length > 21 ? item.Substring(21) : "";
                headerLine   = SafeInsert(headerLine, tab, title);
            }
            _ypos = Mim.Report.Print(1, _ypos, headerLine.Substring(0, 128));
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
        }

        // ════════════════════════════════════════════════════════════════════
        // Report — print one data row
        // ════════════════════════════════════════════════════════════════════
        private void PrintVelden(DataRow row)
        {
            string line = new string(' ', 128);

            for (int t = 0; t < lstRapportVelden.Items.Count; t++)
            {
                string item     = lstRapportVelden.Items[t].ToString();
                int    tab      = SafeParseInt(item, 11, 3);
                int    fieldLen = SafeParseInt(item, 15, 3);
                char   fmt      = item.Length > 19 ? item[19] : 'T';
                string title    = item.Length > 21 ? item.Substring(21) : "";

                // Resolve field value from DataRow (column alias = title)
                string veldInfo = "";
                try
                {
                    if (row.Table.Columns.Contains(title))
                        veldInfo = row.IsNull(title) ? "" : row[title].ToString();
                }
                catch { }

                switch (fmt)
                {
                    case 'T':
                        veldInfo = veldInfo.Length > fieldLen
                            ? veldInfo.Substring(0, fieldLen)
                            : veldInfo.TrimEnd();
                        break;
                    case 'D':
                        veldInfo = DateText(veldInfo);
                        break;
                    default:
                        if (fmt >= '0' && fmt <= '7')
                        {
                            int maskIdx = fmt - '0';
                            if (double.TryParse(veldInfo, out double d))
                                veldInfo = Dec(d, MASK_SY[maskIdx]);
                        }
                        break;
                }

                if (t == 0)
                    SnelHelpPrint(veldInfo, BL_LOGGING);

                line = SafeInsert(line, tab, veldInfo);
            }

            _ypos = Mim.Report.Print(1, _ypos, line.Substring(0, 128));
            CheckPageBreak();
        }

        private void CheckPageBreak()
        {
            if (_ypos > 27.5)
            {
                Mim.Report.PageBreak();
                _ypos = 0;
                ReportHeader(null);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // Helpers
        // ════════════════════════════════════════════════════════════════════

        // Parse an integer from a fixed-width substring (0-based start, length)
        private static int SafeParseInt(string s, int start, int len)
        {
            if (s == null || start >= s.Length) return 0;
            int available = Math.Min(len, s.Length - start);
            return int.TryParse(s.Substring(start, available).Trim(), out int v) ? v : 0;
        }

        // Replace 'length' characters at 0-based 'start' with 'value' (right-padded/truncated)
        private static string SetMid(string s, int start, int length, string value)
        {
            if (s == null) s = "";
            while (s.Length < start + length) s = s.PadRight(start + length);
            if (value.Length > length) value = value.Substring(0, length);
            else value = value.PadRight(length);
            return s.Substring(0, start) + value + s.Substring(start + length);
        }

        // SafeInsert: write text into a fixed-width line at 1-based column position
        private static string SafeInsert(string line, int col1Based, string text)
        {
            int idx = col1Based - 1;
            if (idx < 0 || idx >= line.Length) return line;
            int maxLen = line.Length - idx;
            if (text.Length > maxLen) text = text.Substring(0, maxLen);
            return line.Substring(0, idx) + text + line.Substring(idx + text.Length);
        }

        private static void SnelHelpPrint(string msg, bool log)
            => Classes.MimEnvironment.SnelHelpPrint(msg, log);

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();   
        }
    }
}

