using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using marVSS2028.Classes;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;

namespace marVSS2028.Forms
{
    public partial class FormSQLOperations : Form
    {
        private int[] _grdColWidth = new int[21];
        private bool _allesGesloten = false;
        private ADODB.Recordset _datPrimaryRS = null;
        private string _querySQL = string.Empty;

        public FormSQLOperations()
        {
            InitializeComponent();
            TextTools.WireHighlightEvents(this);
        }

        // ── Form events ────────────────────────────────────────────────────────

        private void FormSQLOperations_Load(object sender, EventArgs e)
        {
            _datPrimaryRS = new ADODB.Recordset
            {
                CursorLocation = ADODB.CursorLocationEnum.adUseClient
            };

            _allesGesloten = false;
            string msg = "Moeten de tabellen gesloten worden (noodzakelijk om wijzigingen aan 'structuur' aan te brengen)";
            if (MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                MessageBox.Show(
                    "De bestanden worden gesloten.  U kan eveneens wijzigingen aan de structuur van tabellen " +
                    "in de database aanbrengen.  Het is aanbevolen straks het bedrijf opnieuw te openen",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _allesGesloten = true;
                BClose(99);
            }
            else
            {
                MessageBox.Show(
                    "De bestanden worden niet gesloten.  U kan enkel de gegevens in de database bewerken.  " +
                    "U kan géén wijzigingen aan de structuur van tabellen in de database aanbrengen.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor = Cursors.WaitCursor;
            AdoLoadDatabase();

            // Insert example query if none exists
            QueryNogEens:
            BGetOrGreater(TABLE_VARIOUS, 1, "29");
            if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("29"))
            {
                string exampleSQL =
                    "SELECT TOP 6\r\n" +
                    "    v019 AS RekNr,\r\n" +
                    "    v020 AS Omschrijving,\r\n" +
                    "    v022 AS [Boekjaar 0],\r\n" +
                    "    v023 AS [Boekjaar -1],\r\n" +
                    "    v024 AS [Boekjaar -2],\r\n" +
                    "    v025 As [Boekjaar -3]\r\n" +
                    "FROM\r\n" +
                    "    Rekeningen\r\n" +
                    "ORDER BY\r\n" +
                    "    v020 DESC";
                TLB_RECORD[TABLE_VARIOUS] = string.Empty;
                VBib(TABLE_VARIOUS, exampleSQL, "v132");
                VBib(TABLE_VARIOUS, "Query voorbeeld", "v250");
                VBib(TABLE_VARIOUS, "29" + VBibText(TABLE_VARIOUS, "#v250 #"), "v005");
                BInsert(TABLE_VARIOUS, 1);
                if (Ktrl == 0)
                    goto QueryNogEens;
            }
            else
            {
                SelectComboVullen();
            }

            AdoRecordset();
            Cursor = Cursors.Default;
        }

        private void FormSQLOperations_FormClosed(object sender, FormClosedEventArgs e)
        {
            try { _datPrimaryRS?.Close(); } catch { }
            _datPrimaryRS = null;

            if (_allesGesloten)
            {
                MessageBox.Show("Bedrijfsdatabase wordt hierna automatisch afgesloten.", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                AutoUnLoadCompany();
            }
        }

        // ── Buttons ────────────────────────────────────────────────────────────

        private void ButtonSQL_Click(object sender, EventArgs e)
        {
            LblRecordCount.Text = string.Empty;
            Refresh();
            if (AdoRecordset())
            {
                // Prevent the grid from auto-overriding column widths
                GridSQL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                foreach (DataGridViewColumn col in GridSQL.Columns)
                    col.Width = 100;

                //for (int i = 0; i < GridSQL.Columns.Count && i < _grdColWidth.Length; i++)
                //{
                //    if (_grdColWidth[i] == 0) break;
                //    GridSQL.Columns[i].Width = _grdColWidth[i];
                //}
            }
        }

        private void ButtonExecute_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                object recAantal = Type.Missing;
                adntDB.Execute(TxtSQL.Text, out recAantal, (int)ADODB.CommandTypeEnum.adCmdText);
                long count = recAantal is int ri ? ri : recAantal is long rl ? rl : 0;
                MessageBox.Show(
                    TxtSQL.Text + "\r\n\rmet succes uitgevoerd.\r\n\r\n" + count + " records met wijzigingen.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Foutmelding bron: " + ex.Source + "\r\n" +
                    "Foutmelding omschrijving:\r\n" + ex.Message);
            }
            finally { Cursor = Cursors.Default; }
        }

        private void ButtonSluiten_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetSql(string sql)
        {
            TxtSQL.Text = sql;
        }

        private void ButtonVersie_Click(object sender, EventArgs e)
        {
            try
            {
                string info =
                    "ADO Versie: "      + adntDB.Version + "\r\n" +
                    "DBMS Naam: "       + adntDB.Properties["DBMS Name"].Value + "\r\n" +
                    "DBMS Versie: "     + adntDB.Properties["DBMS Version"].Value + "\r\n" +
                    "OLE DB Versie: "   + adntDB.Properties["OLE DB Version"].Value + "\r\n" +
                    "Provider Naam: "   + adntDB.Properties["Provider Name"].Value + "\r\n" +
                    "Provider Versie: " + adntDB.Properties["Provider Version"].Value;
                MessageBox.Show(info);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ButtonKopij_Click(object sender, EventArgs e)
        {
            string msg =
                "Kies 'Ja' voor kopij als XML bestand\r\n" +
                "Kies 'Nee' voor kopij naar het klassieke plakbord";

            var result = MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.No)
            {
                // Copy grid content as tab-separated text to clipboard
                try
                {
                    string clip = BuildGridClipText();
                    Clipboard.SetText(clip);
                }
                catch { }
                return;
            }

            // Save as XML
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Alle bestanden (*.xml)|*.xml";
                dlg.FileName = string.Empty;
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    if (File.Exists(dlg.FileName)) File.Delete(dlg.FileName);
                    _datPrimaryRS.Save(dlg.FileName, ADODB.PersistFormatEnum.adPersistXML);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void ButtonOpenXML_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Alle bestanden (*.xml)|*.xml";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    try { _datPrimaryRS.Close(); } catch { }
                    _datPrimaryRS.Open(dlg.FileName, Type.Missing,
                        ADODB.CursorTypeEnum.adOpenForwardOnly,
                        ADODB.LockTypeEnum.adLockReadOnly,
                        (int)ADODB.CommandTypeEnum.adCmdFile);
                    TxtSQL.Text = (string)_datPrimaryRS.Source;
                    LblRecordCount.Text = _datPrimaryRS.RecordCount.ToString();
                    PopulateGrid();
                }
                catch
                {
                    MessageBox.Show("Dit is geen ADO-compatibel XML bestand.", string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ButtonSelectWegschrijven_Click(object sender, EventArgs e)
        {
            BGet(TABLE_VARIOUS, 1, "29" + CmbSelect.Text);
            string colWidths = BuildColWidthString();

            if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("29"))
            {
                // Insert new
                TLB_RECORD[TABLE_VARIOUS] = string.Empty;
                VBib(TABLE_VARIOUS, TxtSQL.Text + colWidths, "v132");
                VBib(TABLE_VARIOUS, CmbSelect.Text, "v250");
                VBib(TABLE_VARIOUS, "29" + VBibText(TABLE_VARIOUS, "#v250 #"), "v005");
                BInsert(TABLE_VARIOUS, 1);
            }
            else if (MessageBox.Show(
                "Bestaande definitie '" + CmbSelect.Text + "' overschrijven ?",
                string.Empty, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                RecordToVeld(TABLE_VARIOUS);
                VBib(TABLE_VARIOUS, TxtSQL.Text + colWidths, "v132");
                VBib(TABLE_VARIOUS, CmbSelect.Text, "v250");
                VBib(TABLE_VARIOUS, "29" + VBibText(TABLE_VARIOUS, "#v250 #"), "v005");
                BUpdate(TABLE_VARIOUS, 1);
            }
            else return;

            SelectComboVullen();
        }

        private void ButtonNet1_Click(object sender, EventArgs e)
        {
            string alterMsg = "ALTER TABLE Journalen DROP COLUMN dece068";
            MessageBox.Show(
                "Alle rekening- en journaalvelden voor cijfers/bedragen dienen vanaf versie 6.5.301 bij voorkeur " +
                "formaat DECIMAL te zijn (voorheen CURRENCY).  Wij zullen zolang als mogelijk een manuele " +
                "hersamenstelling beschikbaar stellen d.m.v. de SQL instructie hierna EN VERVOLGENS opnieuw " +
                "openen van het bedrijf.  Zeker tot aan versie 6.5.500 zal deze functie beschikbaar blijven.\r\n\r\n" +
                "Aarzel NOOIT ons te contacteren voor bijkomende inlichtingen:\r\n\r\n" +
                alterMsg + "\r\n\r\n" +
                "Uiteraard alléén indien U een verkeerde herrekening wenst te verbeteren...\r\n\r\n" +
                "NETWERKERS !  EERST AL UW ANDERE marINTEGRAAL GEBRUIKERSVERBINDINGEN STOPPEN !!!!",
                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonBackup_Click(object sender, EventArgs e)
        {
            string jetConnect = ADOJET_PROVIDER +
                "Data Source=" + LOCATION_COMPANYDATA + @"\marnt.mdv;" +
                "Persist Security Info=False";

            var cnn = new ADODB.Connection();
            try
            {
                cnn.Open(jetConnect);

                string mdbPath = LOCATION_COMPANYDATA + @"\marnt.mdv";
                if (File.Exists(mdbPath)) File.Delete(mdbPath);

                // Copy template
                string src = Path.Combine(PROGRAM_LOCATION, "marnt.mdv");
                if (!File.Exists(src))
                {
                    MessageBox.Show("Bronbestand niet gevonden: " + src);
                    return;
                }
                File.Copy(src, mdbPath);

                string msgResult = string.Empty;
                foreach (string tblName in GetTableNames())
                {
                    try
                    {
                        string sql = "SELECT * INTO [" + mdbPath + "].[" + tblName + "] FROM " + tblName;
                        SnelHelpPrint("Bezig aan tabel " + tblName, BL_LOGGING);
                        Cursor = Cursors.WaitCursor;
                        object affected = Type.Missing;
                        cnn.Execute(sql, out affected, (int)ADODB.CommandTypeEnum.adCmdText);
                        long count = affected is int ri ? ri : affected is long rl ? rl : 0;
                        msgResult += count + " records met succes overgedragen in tabel " + tblName + " / ";
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    finally { Cursor = Cursors.Default; }
                }

                MessageBox.Show("Einde backup database\r\n\r\n" + msgResult, string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Backup nog op veilige plaats bewaren: " + mdbPath, string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                try { cnn.Close(); } catch { }
            }
        }

        // ── ComboBox / ListView events ─────────────────────────────────────────

        private void CmbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            BGet(TABLE_VARIOUS, 1, "29" + CmbSelect.Text);
            if (Ktrl != 0) return;

            RecordToVeld(TABLE_VARIOUS);
            string raw = VBibText(TABLE_VARIOUS, "#v132 #");
            int cwIdx = raw.IndexOf("[Colwidth]", StringComparison.Ordinal);

            TxtSQL.Text = cwIdx >= 0 ? raw.Substring(0, cwIdx) : raw;

            // Parse stored column widths
            if (cwIdx >= 0)
            {
                string cwPart = raw.Substring(cwIdx + 10); // skip "[Colwidth]"
                int col = 0;
                while (cwPart.Contains("\t") && col < _grdColWidth.Length)
                {
                    int tabPos = cwPart.IndexOf('\t');
                    if (int.TryParse(cwPart.Substring(0, tabPos).Trim(), out int w))
                        _grdColWidth[col++] = w;
                    cwPart = cwPart.Substring(tabPos + 1);
                }
                if (col < _grdColWidth.Length) _grdColWidth[col] = 0;
            }
            else
            {
                _grdColWidth[0] = 0;
            }
        }

        private void CmbSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                BGet(TABLE_VARIOUS, 1, "29" + CmbSelect.Text);
                if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("29")) return;

                if (MessageBox.Show(
                    "Bestaande definitie '" + CmbSelect.Text + "' verwijderen ?",
                    string.Empty, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    RecordToVeld(TABLE_VARIOUS);
                    BDelete(TABLE_VARIOUS);
                    SelectComboVullen();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ButtonSelectWegschrijven_Click(sender, e);
            }
        }

        private void CbSQLBevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbSQLBevel.SelectedIndex != 0)
            {
                MessageBox.Show(
                    CbSQLBevel.Text + " opdracht.\r\n\r\n" +
                    "Het is ten zeerste aan te raden om dergelijke\r\n" +
                    "opdrachten BINNENIN een TRANSACTIE uit te voeren\r\n\r\n\r\n" +
                    "BEGIN WORK start een transactie\r\n\r\n" +
                    "ROLLBACK WORK annuleert alle wijziging na 'BEGIN WORK'\r\n" +
                    "(m.a.w. zéér interessant om foutieve 'DELETE/UPDATES/INSERT'\r\n" +
                    "opdrachten teniet te doen...)\r\n\r\n" +
                    "COMMIT WORK ten slotte laat alle 'DELETE/UPDATE/INSERT'\r\n" +
                    "opdrachten doorgaan.\r\n\r\n" +
                    "BEGIN WORK wordt hierna voorgesteld als instructie.  Druk Alt+E om te activeren",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                TxtSQL.Text = "BEGIN WORK";
            }
            else
            {
                QueryPlus();
                QueryChange();
            }
        }

        private void CbVelden_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryPlus();
            QueryChange();
        }

        private void CbOperatie_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryPlus();
            QueryChange();
        }

        private void TxtPLUS_TextChanged(object sender, EventArgs e)
        {
            QueryChange();
        }

        private void TxtWaarde_TextChanged(object sender, EventArgs e)
        {
            QueryPlus();
            QueryChange();
        }

        private void LvDatabase_Click(object sender, EventArgs e)
        {
            if (LvDatabase.SelectedItems.Count > 0)
                TxtSQL.Text = "SELECT * FROM " + LvDatabase.SelectedItems[0].Text;
        }

        private void LvDatabase_DoubleClick(object sender, EventArgs e)
        {
            if (LvDatabase.SelectedItems.Count == 0) return;
            TxtSQL.Text = "SELECT * FROM " + LvDatabase.SelectedItems[0].Text;
            AdoRecordset();

            CbSQLBevel.Enabled = true;
            CbSQLBevel.SelectedIndex = 0;

            CbVelden.Enabled = true;
            CbVelden.Items.Clear();
            try
            {
                for (int i = 0; i < _datPrimaryRS.Fields.Count; i++)
                    CbVelden.Items.Add(_datPrimaryRS.Fields[i].Name);
                if (CbVelden.Items.Count > 0) CbVelden.SelectedIndex = 0;
            }
            catch { }

            CbOperatie.Enabled = true;
            CbOperatie.SelectedIndex = 0;
            TxtWaarde.Enabled = true;
            TxtWaarde.Text = "'%'";
            TxtWaarde.Focus();
        }

        private void GridSQL_DoubleClick(object sender, EventArgs e)
        {
            // Toggle maximised grid
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                GridSQL.Dock = DockStyle.Fill;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                GridSQL.Dock = DockStyle.None;
                GridSQL.SetBounds(0, 0, 614, 218);
            }
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        public void AdoLoadDatabase()
        {
            LvDatabase.Items.Clear();
            foreach (string tblName in GetTableNames())
            {
                var item = new ListViewItem(tblName);
                item.ImageKey = "Tabel";
                LvDatabase.Items.Add(item);
            }
            LvDatabase.View = View.LargeIcon;
        }

        public bool AdoRecordset()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                GridSQL.DataSource = null;
                try { _datPrimaryRS.Close(); } catch { }

                _datPrimaryRS.Open(TxtSQL.Text, adntDB,
                    ADODB.CursorTypeEnum.adOpenStatic,
                    ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText);

                PopulateGrid();
                LblRecordCount.Text = _datPrimaryRS.RecordCount.ToString();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Bron:\r\n" + ex.Source + "\r\n\r\nDetail:\r\n" + ex.Message);
                return false;
            }
            finally { Cursor = Cursors.Default; }
        }

        private void SelectComboVullen()
        {
            CmbSelect.Items.Clear();
            BGetOrGreater(TABLE_VARIOUS, 1, "29");
            if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("29")) return;

            do
            {
                RecordToVeld(TABLE_VARIOUS);
                CmbSelect.Items.Add(VBibText(TABLE_VARIOUS, "#v250 #"));
                BNext(TABLE_VARIOUS);
            }
            while (Ktrl == 0 && KEY_BUF[TABLE_VARIOUS].StartsWith("29"));

            if (CmbSelect.Items.Count > 0) CmbSelect.SelectedIndex = 0;
        }

        private void QueryChange()
        {
            _querySQL = CbSQLBevel.Text + " " + TxtPLUS.Text + " " +
                        CbVelden.Text + " " + CbOperatie.Text + " " + TxtWaarde.Text;
            TxtSQL.Text = _querySQL;
        }

        private void QueryPlus()
        {
            if (LvDatabase.SelectedItems.Count == 0) return;
            string tbl = LvDatabase.SelectedItems[0].Text;

            switch (CbSQLBevel.SelectedIndex)
            {
                case 0:
                    TxtPLUS.Text = " * FROM " + tbl + " WHERE ";
                    TxtPLUS.Enabled = false;
                    break;
                case 1:
                    TxtPLUS.Text = " FROM " + tbl + " WHERE ";
                    TxtPLUS.Enabled = false;
                    break;
                case 2:
                    TxtPLUS.Text = " " + tbl + " SET " + CbVelden.Text + " = ??? WHERE ";
                    TxtPLUS.Enabled = true;
                    break;
                default:
                    MessageBox.Show(CbSQLBevel.Text + " nog niet beschikbaar via snelinstructies",
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtSQL.Text = string.Empty;
                    break;
            }
        }

        // Populate DataGridView from open ADODB.Recordset
        private void PopulateGrid()
        {
            var dt = new System.Data.DataTable();
            int fieldCount = _datPrimaryRS.Fields.Count;
            for (int i = 0; i < fieldCount; i++)
                dt.Columns.Add(_datPrimaryRS.Fields[i].Name);

            if (!_datPrimaryRS.EOF)
            {
                _datPrimaryRS.MoveFirst();

                // Fetch all rows in a single COM call to avoid per-row/per-field
                // cross-apartment context switches that cause ContextSwitchDeadlock.
                // GetRows() returns a column-major 2D array: [columnIndex, rowIndex].
                object[,] rows = (object[,])_datPrimaryRS.GetRows();
                int colCount = rows.GetLength(0);
                int rowCount = rows.GetLength(1);

                for (int r = 0; r < rowCount; r++)
                {
                    var row = dt.NewRow();
                    for (int c = 0; c < colCount; c++)
                    {
                        object val = rows[c, r];
                        row[c] = (val == null || val is DBNull) ? (object)DBNull.Value : val;
                    }
                    dt.Rows.Add(row);
                }
            }

            // Suspend expensive auto-sizing before binding to avoid blocking the STA thread.
            GridSQL.DefaultCellStyle.Padding = Padding.Empty;
            GridSQL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            GridSQL.DataSource = dt;
            // Resize only the visible columns — fast and non-blocking.
            GridSQL.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        // Build clipboard text from DataGridView
        private string BuildGridClipText()
        {
            var sb = new System.Text.StringBuilder();
            foreach (DataGridViewRow row in GridSQL.Rows)
            {
                var cells = new List<string>();
                foreach (DataGridViewCell cell in row.Cells)
                    cells.Add(cell.Value?.ToString() ?? string.Empty);
                sb.AppendLine(string.Join("\t", cells));
            }
            return sb.ToString();
        }

        // Build [Colwidth] suffix for saving query definitions
        private string BuildColWidthString()
        {
            var sb = new System.Text.StringBuilder("[Colwidth]");
            foreach (DataGridViewColumn col in GridSQL.Columns)
                sb.Append(col.Width).Append("\t");
            return sb.ToString();
        }

        // Get all user table names via ADODB schema rowset
        private List<string> GetTableNames()
        {
            var tables = new List<string>();
            try
            {
                ADODB.Recordset rs = adntDB.OpenSchema(
                    ADODB.SchemaEnum.adSchemaTables,
                    new object[] { null, null, null, "TABLE" },
                    Type.Missing);
                while (!rs.EOF)
                {
                    string name = rs.Fields["TABLE_NAME"].Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(name)) tables.Add(name);
                    rs.MoveNext();
                }
                rs.Close();
            }
            catch { }
            return tables;
        }
    }
}
