using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

using marVSS2028.Classes;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;

namespace marVSS2028.PublicForms
{
    public partial class FormSearchSQL : Form
    {
        private int _indexNR;
        private int[] _grdColWidth = new int[21];
        private DataTable _datPrimaryRS;
        private bool _gridUpdating;

        public FormSearchSQL()
        {
            InitializeComponent();
            TextTools.WireHighlightEvents(this);
        }

        // ── Helpers ────────────────────────────────────────────────────────────
                
        private static int InStr(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle)) return 0;
            int idx = haystack.IndexOf(needle, StringComparison.Ordinal);
            return idx < 0 ? 0 : idx + 1;
        }

        private static int InStr(int start, string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle) || start > haystack.Length) return 0;
            int from = Math.Max(0, start - 1);
            int idx = haystack.IndexOf(needle, from, StringComparison.Ordinal);
            return idx < 0 ? 0 : idx + 1;
        }

        // ── GetAllIndexes: fill a ComboBox with index entries for table fl ─────
        private static void GetAllIndexes(string tableName, ComboBox combo)
        {
            combo.Items.Clear();
            for (int t = 0; t <= NUMBER_TABLES; t++)
            {
                if (string.Compare(bstNaam[t], tableName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    for (int i = 0; i <= FL_NUMBEROFINDEXEN[t]; i++)
                    {
                        string field = JETTABLEUSE_INDEX[t, i].TrimEnd();
                        string caption = FLINDEX_CAPTION[t, i];
                        // Format: " FIELD; Caption"  — mirrors VB6 GetAllIndexes convention
                        combo.Items.Add(" " + field + "; " + caption);
                    }
                    return;
                }
            }
        }

        // ── SQLVernieuwTekst ───────────────────────────────────────────────────

        private void SQLVernieuwTekst(string comboTekst)
        {
            string sorteerIndex = string.Empty;
            string sorteerOrde = string.Empty;
            string sleuteltje;

            _grdColWidth[0] = 0;

            if (!chkExterneDatabase.Checked)
                sleuteltje = "marSQL" + SharedFl.ToString("00") + comboTekst.Substring(0, InStr(comboTekst, ";") - 1);
            else
                sleuteltje = "marEDB" + SharedFl.ToString("00") + comboTekst.Substring(0, InStr(comboTekst, ";") - 1);

            int telOrde = 0;
            while (true)
            {
                int countTo = InStr(telOrde + 1, comboTekst, ";") - 1;
                if (countTo < 0)
                    break;

                sorteerIndex = SafeMid(comboTekst, countTo - 3, 4);
                sorteerOrde = SafeMid(comboTekst, countTo - 3, 4);
                sorteerOrde += SafeMid(comboTekst, countTo - 4, 1) == "+" ? " ASC" : " ASC"; // " DESC";
                
                telOrde = countTo + 1;
            }

            BGet(TABLE_VARIOUS, 1, "29" + sleuteltje);
            if (Ktrl != 0)
            {
                if (chkExterneDatabase.Checked)
                {
                    MessageBox.Show("Onjuiste SQL zoekopdracht voor externe database", string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                InitSQL(sorteerIndex, sorteerOrde);
            }
            else
            {
                RecordToVeld(TABLE_VARIOUS);
                string msg = VBibText(TABLE_VARIOUS, "#v132 #");
                if (InStr(msg.ToUpper(), "WHERE") != 0)
                {
                    string upperMsg = msg.ToUpper();
                    msg = msg.Substring(0, InStr(upperMsg, " WHERE ") - 1);
                    msg += " WHERE " + sorteerIndex + " Like \"" + txtTeZoeken.Text + "\"";
                    msg += " ORDER BY " + sorteerOrde;
                    rtbSQLTekst.Text = msg;

                    string colPart = VBibText(TABLE_VARIOUS, "#v132 #");
                    int colWidthIdx = InStr(colPart, "[Colwidth]");
                    string colMsg = colWidthIdx > 0
                        ? SafeMid(colPart, colWidthIdx + 10)
                        : string.Empty;

                    if (string.IsNullOrEmpty(colMsg))
                    {
                        _grdColWidth[0] = 0;
                    }
                    else
                    {
                        int countTo = 0;
                        while (!string.IsNullOrEmpty(colMsg))
                        {
                            int tabPos = InStr(colMsg, "\t");
                            if (tabPos != 0)
                            {
                                if (int.TryParse(colMsg.Substring(0, tabPos - 1), out int w))
                                    _grdColWidth[countTo] = w;
                                colMsg = SafeMid(colMsg, tabPos + 1);
                                countTo++;
                            }
                            else
                                break;
                        }
                        _grdColWidth[countTo] = 0;
                    }
                }
                else
                {
                    InitSQL(sorteerIndex, sorteerOrde);
                }
            }
        }

        private void InitSQL(string sorteerIndex, string sorteerOrde)
        {
            string msg = "SELECT";
            bool deLaatste = false;

            // Ensure first (primary) index field is included first
            for (int i = 0; i < Sortering.Items.Count; i++)
            {
                string item = Sortering.Items[i].ToString();
                string field = SafeMid(item, 2, InStr(item, ";") - 2);
                if (string.Compare(JETTABLEUSE_INDEX[SharedFl, 0].TrimEnd(), field.Trim(), StringComparison.Ordinal) == 0)
                {
                    string caption = SafeMid(item, InStr(item, ";") + 2);
                    msg += " " + field + " AS [" + caption + "],";
                    if (i == Sortering.Items.Count - 1) deLaatste = true;
                    break;
                }
            }

            if (msg == "SELECT")
            {
                string firstItem = Sortering.Items.Count > 0 ? Sortering.Items[0].ToString() : string.Empty;
                string field = SafeMid(firstItem, 2, InStr(firstItem, ";") - 2);
                MessageBox.Show("Hoofdindex " + field + " bestaat niet (meer)", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Add remaining fields
            for (int i = 0; i < Sortering.Items.Count; i++)
            {
                string item = Sortering.Items[i].ToString();
                string field = SafeMid(item, 2, InStr(item, ";") - 2);
                if (string.Compare(JETTABLEUSE_INDEX[SharedFl, 0].TrimEnd(), field.Trim(), StringComparison.Ordinal) == 0)
                    continue;

                string caption = SafeMid(item, InStr(item, ";") + 2);
                msg += " " + field + " AS [" + caption + "]";
                bool isLast = deLaatste
                    ? (i == Sortering.Items.Count - 2)
                    : (i < Sortering.Items.Count - 1);
                if (isLast) msg += ",";
            }

            msg += " FROM " + bstNaam[SharedFl];
            msg += " WHERE " + sorteerIndex + " Like \"" + txtTeZoeken.Text + "\"";
            msg += " ORDER BY " + sorteerOrde;
            rtbSQLTekst.Text = msg;
        }

        // ── Form events ────────────────────────────────────────────────────────

        private void FormSearchSQL_Load(object sender, EventArgs e)
        {
            Text = Text + ": " + bstNaam[SharedFl];
            VulcmbSortering();

            if (InStr(GridText, "@Beperk@") != 0)
            {
                txtTeZoeken.Text = GridText.Substring(0, 2) + "%";
                cmdZoeken_Click(sender, e);
            }
            else if (!string.IsNullOrEmpty(GridText))
            {
                txtTeZoeken.Text = GridText + "%";
                cmdZoeken_Click(sender, e);
            }
            else
            {
                txtTeZoeken.Text = "%";
            }
        }

        // ── Controls ───────────────────────────────────────────────────────────

        private void chkExterneDatabase_Click(object sender, EventArgs e)
        {
            if (chkExterneDatabase.Checked)
            {
                Sortering.Visible = false;
                cmbExternedatabase.Visible = true;
                VulcmbExterneDatabase();
            }
            else
            {
                cmbExternedatabase.Visible = false;
                VulcmbSortering();
                Sortering.Visible = true;
            }
        }

        private void cmbExterneDatabase_Click(object sender, EventArgs e)
        {
            cmdZoeken.Text = "Zoeken";
            SQLVernieuwTekst(cmbExternedatabase.Text);
            Schoon();
        }

        private void cmdBewaar_Click(object sender, EventArgs e)
        {
            //string sleuteltje;
            //if (!chkExterneDatabase.Checked)
            //    sleuteltje = "marSQL" + SharedFl.ToString("00") + Sortering.Text.Substring(0, InStr(Sortering.Text, ";") - 1);
            //else
            //    sleuteltje = "marEDB" + SharedFl.ToString("00") + cmbExternedatabase.Text.Substring(0, InStr(cmbExternedatabase.Text, ";") - 1);

            //BGet(TABLE_VARIOUS, 1, "29" + sleuteltje);
            //if (Ktrl != 0)
            //{
            //    string colWidths = BuildColWidthString();
            //    string saveMsg = rtbSQLTekst.Text + "[Colwidth]" + colWidths;

            //    TLB_RECORD[TABLE_VARIOUS] = string.Empty;
            //    VBib(TABLE_VARIOUS, saveMsg, "v132");
            //    VBib(TABLE_VARIOUS, sleuteltje, "v250");
            //    VBib(TABLE_VARIOUS, "29" + sleuteltje, "v005");
            //    BInsert(TABLE_VARIOUS, 1);
            //}
            //else if (MessageBox.Show("Bestaande definitie '" + sleuteltje + "' overschrijven ?",
            //    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            //    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //{
            //    RecordToVeld(TABLE_VARIOUS);
            //    string colWidths = BuildColWidthString();
            //    string saveMsg = rtbSQLTekst.Text + "[Colwidth]" + colWidths;

            //    VBib(TABLE_VARIOUS, saveMsg, "v132");
            //    VBib(TABLE_VARIOUS, sleuteltje, "v250");
            //    VBib(TABLE_VARIOUS, "29" + sleuteltje, "v005");
            //    BUpdate(TABLE_VARIOUS, 1);
            //}
        }

        private string BuildColWidthString()
        {
            var sb = new System.Text.StringBuilder();
            foreach (DataGridViewColumn col in mfgLijst.Columns)
                sb.Append(col.Width).Append('\t');
            return sb.ToString();
        }

        private void cmdSluiten_Click(object sender, EventArgs e)
        {
            Ktrl = 99;
            Close();
        }

        private void mfgLijst_Click(object sender, EventArgs e)
        {
            if (_gridUpdating) return;
            _gridUpdating = true;
            try
            {
                if (_datPrimaryRS == null || _datPrimaryRS.Rows.Count == 0) return;
                if (mfgLijst.Rows.Count == 0 || mfgLijst.CurrentCell == null) return;
                int row = mfgLijst.CurrentCell.RowIndex;
                if (_datPrimaryRS.Rows.Count == 0 || row >= _datPrimaryRS.Rows.Count) return;

                string fieldName = SafeMid(Sortering.Text, InStr(Sortering.Text, ";") + 2);
                try
                {
                    txtTeZoeken.Text = _datPrimaryRS.Rows[row][fieldName]?.ToString() ?? string.Empty;
                }
                catch
                {
                    SnelHelpPrint(fieldName + " ontbreekt in SELECT !!", false);
                }
            }
            catch { }
            finally
            {
                _gridUpdating = false;
            }
        }

        private void mfgLijst_DblClick(object sender, EventArgs e)
        {
            cmdZoeken_Click(sender, e);
        }

        private void mfgLijst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                cmdZoeken_Click(sender, EventArgs.Empty);
            }
        }

        private void mfgLijst_GotFocus(object sender, EventArgs e)
        {
            cmdZoeken.Text = "Ok";
        }
        
        private void rtbSQLTekst_KeyPress(object sender, KeyPressEventArgs e)
        {
            cmdBewaar.Enabled = true;
        }

        private void Sortering_Click(object sender, EventArgs e)
        {
            try
            {
                cmdZoeken.Text = "Zoeken";
                SQLVernieuwTekst(Sortering.Text);
                txtTeZoeken.Text = "%";
                Schoon();
            }
            catch { }
        }

        private void Sortering_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down)
                e.SuppressKeyPress = true;
        }

        private void Sortering_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtTeZoeken_Change(object sender, EventArgs e)
        {
            if (txtTeZoeken.Text.Length <= 1 && !txtTeZoeken.Text.Contains("%"))
            {
                int sel = txtTeZoeken.Text.Length;
                txtTeZoeken.Text += "%";
                txtTeZoeken.SelectionStart = sel;
            }
        }

        private void txtTeZoeken_GotFocus(object sender, EventArgs e)
        {
            cmdZoeken.Text = "Zoeken";
            txtTeZoeken.SelectAll();
        }

        private void txtTeZoeken_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode >= Keys.Up && e.KeyCode <= Keys.Down)
                {
                    mfgLijst.Focus();
                    if (mfgLijst.Rows.Count > 0)
                        mfgLijst.CurrentCell = mfgLijst.Rows[0].Cells[_indexNR < mfgLijst.Columns.Count ? _indexNR : 0];
                }
            }
            catch { }
        }

        private void txtTeZoeken_KeyPress(object sender, KeyPressEventArgs e)
        {
            cmdZoeken.Text = "Zoeken";
        }

        private void cmdZoeken_Click(object sender, EventArgs e)
        {
            if (cmdZoeken.Text == "Ok")
            {
                if (Sortering.Visible)
                {
                    // Get the value from column 0 of the selected row
                    int rowIdx = mfgLijst.CurrentCell?.RowIndex ?? -1;
                    if (rowIdx < 0) return;
                    string key = mfgLijst.Rows[rowIdx].Cells.Count > 0
                        ? mfgLijst.Rows[rowIdx].Cells[0].Value?.ToString() ?? string.Empty
                        : string.Empty;
                    XLogKey = key;
                    BGet(SharedFl, 0, key);
                    if (Ktrl != 0)
                    {
                        System.Media.SystemSounds.Beep.Play();
                        txtTeZoeken.Focus();
                        return;
                    }
                    RecordToVeld(SharedFl);
                }
                else
                {
                    int rowIdx = mfgLijst.CurrentCell?.RowIndex ?? -1;
                    if (rowIdx < 0) return;
                    string cellText = mfgLijst.Rows[rowIdx].Cells.Count > 0
                        ? mfgLijst.Rows[rowIdx].Cells[0].Value?.ToString() ?? string.Empty
                        : string.Empty;

                    var choice = MessageBox.Show(
                        "Info uit externe database toevoegen aan bedrijfsdatabase.  Bent U zeker?\r\n\r\n" +
                        "Kies 'Ja' om enkel geselecteerde lijn in te voegen.\r\n" +
                        "Kies 'Nee' om alle lijnen in te voegen.",
                        cellText, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button3);

                    if (choice == DialogResult.Yes)
                    {
                        VoegExterneLijnErBij(rowIdx);
                        chkExterneDatabase.Checked = false;
                        txtTeZoeken.Text = string.Empty;
                        txtTeZoeken.Focus();
                        return;
                    }
                    else if (choice == DialogResult.No)
                    {
                        if (MessageBox.Show("Alle lijnen invoegen.  Bent U zeker?",
                            string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            VoegExterneLijnErBij(-1);
                            chkExterneDatabase.Checked = false;
                            txtTeZoeken.Text = string.Empty;
                            txtTeZoeken.Focus();
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                Close();
            }
            else
            {
                if (!chkExterneDatabase.Checked)
                    SQLVernieuwTekst(Sortering.Text);
                else
                    SQLVernieuwTekst(cmbExternedatabase.Text);

                Cursor = Cursors.WaitCursor;
                try
                {
                    string sSQL = rtbSQLTekst.Text;

                    _datPrimaryRS = new DataTable();

                    try
                    {
                        using (var conn = new OleDbConnection(adntDB.ConnectionString))
                        using (var adapter = new OleDbDataAdapter(sSQL, conn))
                            adapter.Fill(_datPrimaryRS);
                        PopulateGrid();
                    }
                    catch (Exception ex)
                    {
                        Schoon();
                        MessageBox.Show("Bron:\r\n" + ex.Source + "\r\n\r\nFoutnummer: " + ex.HResult +
                            "\r\n\r\nDetail:\r\n" + ex.Message);
                        lblTekst1.Text = string.Empty;
                        goto Done;
                    }

                    lblTekst1.Text = _datPrimaryRS.Rows.Count.ToString();

                    for (int i = 0; i < mfgLijst.Columns.Count; i++)
                    {
                        if (_grdColWidth[i] == 0) break;
                        mfgLijst.Columns[i].Width = _grdColWidth[i];
                    }

                    if (mfgLijst.Rows.Count > 0)
                    {
                        mfgLijst.CurrentCell = mfgLijst.Rows[0].Cells[0];
                        mfgLijst.Focus();
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }

            Done:;
        }

        private void VoegExterneLijnErBij(long rijInteVoegen)
        {
            string firstHeader = mfgLijst.Columns.Count > 0
                ? mfgLijst.Columns[0].HeaderText : string.Empty;

            if (string.Compare(JETTABLEUSE_INDEX[SharedFl, 0].TrimEnd(), firstHeader.Trim(),
                StringComparison.OrdinalIgnoreCase) != 0)
            {
                MessageBox.Show("Eerste veldnaam komt niet overeen");
                return;
            }

            if (rijInteVoegen == -1)
            {
                Cursor = Cursors.WaitCursor;
                for (int r = 0; r < mfgLijst.Rows.Count; r++)
                {
                    string key = mfgLijst.Rows[r].Cells.Count > 0
                        ? mfgLijst.Rows[r].Cells[0].Value?.ToString() ?? string.Empty
                        : string.Empty;

                    BGet(SharedFl, 0, key);
                    bool isNieuw = Ktrl != 0;
                    if (isNieuw)
                        DaoBlankoRecord(SharedFl);
                    else
                    {
                        stbSnelHelpLabel.Text = "Bestaat reeds";
                        RecordToVeld(SharedFl);
                    }

                    for (int c = 0; c < mfgLijst.Columns.Count; c++)
                    {
                        string fieldName = mfgLijst.Columns[c].HeaderText;
                        string val = mfgLijst.Rows[r].Cells[c].Value?.ToString() ?? string.Empty;
                        VBib(SharedFl, val, fieldName);
                    }

                    if (isNieuw)
                        BInsert(SharedFl, 0);
                    else
                        BUpdate(SharedFl, 0);
                }
                Cursor = Cursors.Default;
            }
            else
            {
                int r = (int)rijInteVoegen;
                string key = mfgLijst.Rows[r].Cells.Count > 0
                    ? mfgLijst.Rows[r].Cells[0].Value?.ToString() ?? string.Empty
                    : string.Empty;

                BGet(SharedFl, 0, key);
                bool isNieuw = Ktrl != 0;
                if (isNieuw)
                    DaoBlankoRecord(SharedFl);
                else
                {
                    stbSnelHelpLabel.Text = "Bestaat reeds";
                    RecordToVeld(SharedFl);
                }

                for (int c = 0; c < mfgLijst.Columns.Count; c++)
                {
                    string fieldName = mfgLijst.Columns[c].HeaderText;
                    string val = mfgLijst.Rows[r].Cells[c].Value?.ToString() ?? string.Empty;
                    VBib(SharedFl, val, fieldName);
                }

                if (isNieuw)
                    BInsert(SharedFl, 0);
                else
                    BUpdate(SharedFl, 0);
            }
        }

        // ── Grid population ────────────────────────────────────────────────────

        private void PopulateGrid()
        {
            mfgLijst.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            mfgLijst.DataSource = _datPrimaryRS;
            mfgLijst.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        // ── Fill helpers ───────────────────────────────────────────────────────

        private void VulcmbSortering()
        {
            GetAllIndexes(bstNaam[SharedFl], Sortering);
            _indexNR = 0;
            for (int i = 0; i < Sortering.Items.Count; i++)
            {
                string item = Sortering.Items[i].ToString();
                string caption = SafeMid(item, InStr(item, ";") + 2);
                if (string.Compare(caption, FLINDEX_CAPTION[SharedFl, aIndex], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    _indexNR = i;
                    break;
                }
            }

            if (Sortering.Items.Count > 0)
                Sortering.SelectedIndex = _indexNR;
        }

        private void VulcmbExterneDatabase()
        {
            cmbExternedatabase.Items.Clear();
            string sleutelHier = "marEDB" + SharedFl.ToString("00");
            BGetOrGreater(TABLE_VARIOUS, 1, "29" + sleutelHier);

            if (Ktrl != 0 || string.Compare(SafeMid(KEY_BUF[TABLE_VARIOUS], 3, 8), sleutelHier,
                StringComparison.OrdinalIgnoreCase) != 0)
            {
                MessageBox.Show("Er bestaan (nog) geen definities met voorvoegsel: " + sleutelHier,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                chkExterneDatabase.Checked = false;
            }
            else
            {
                while (true)
                {
                    RecordToVeld(TABLE_VARIOUS);
                    string veldString = SafeMid(KEY_BUF[TABLE_VARIOUS], 11, 5);
                    cmbExternedatabase.Items.Add(veldString + ";" + SafeMid(KEY_BUF[TABLE_VARIOUS], 3));
                    BNext(TABLE_VARIOUS);
                    if (Ktrl != 0 || string.Compare(SafeMid(KEY_BUF[TABLE_VARIOUS], 3, 8), sleutelHier,
                        StringComparison.OrdinalIgnoreCase) != 0)
                        break;
                }
                if (cmbExternedatabase.Items.Count > 0)
                    cmbExternedatabase.SelectedIndex = 0;
            }
        }

        private void Schoon()
        {
            mfgLijst.DataSource = null;
            mfgLijst.Columns.Clear();
            mfgLijst.Rows.Clear();
        }

        private static string SafeMid(string s, int start, int length)
        {
            if (string.IsNullOrEmpty(s) || start < 1 || start > s.Length) return string.Empty;
            int idx = start - 1;
            return s.Substring(idx, Math.Min(length, s.Length - idx));
        }

        private static string SafeMid(string s, int start)
        {
            if (string.IsNullOrEmpty(s) || start < 1 || start > s.Length) return string.Empty;
            return s.Substring(start - 1);
        }
    }
}

