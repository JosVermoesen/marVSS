using System;
using System.Data.OleDb;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Filing
{
    public partial class FormLedgerSQL : Form
    {
        // ── VB6 module-level fields ────────────────────────────────────────
        private string _rekeningNummer = "";
        private string _van            = "";
        private string _tot            = "";
        private double _dTotaalSaldo   = 0;
        private bool   _isZoeken       = false;

        // ── constructor ────────────────────────────────────────────────────
        public FormLedgerSQL()
        {
            InitializeComponent();
        }

        // ── Form_Load ──────────────────────────────────────────────────────
        private void FormLedgerSQL_Load(object sender, EventArgs e)
        {            
            txtLijnen.Text   = LaadTekst("HistoriekInScherm", "MaxLijnen");
            tekstLijn.Text   = DateText(BOOKYEAR_FROMTO.Substring(0, 8))
                             + " - "
                             + DateText(BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8));

            rekening.Text    = KEY_BUF[TABLE_LEDGERACCOUNTS];
            this.Text        = "Historiek (" + VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").TrimEnd() + ")";

            // grid columns
            grdJournaalDetail.Columns.Clear();
            grdJournaalDetail.Columns.Add("v033",    "Document");
            grdJournaalDetail.Columns.Add("v035",    "Datum Doc.");
            grdJournaalDetail.Columns.Add("v038",    "Fin.stuk");
            grdJournaalDetail.Columns.Add("v067",    "Omschrijving");
            grdJournaalDetail.Columns.Add("dece068", "EUR");            
            grdJournaalDetail.Columns.Add("v069",    "T.Rek.");

            grdJournaalDetail.Columns[0].Width = 81;
            grdJournaalDetail.Columns[1].Width = 89;
            grdJournaalDetail.Columns[2].Width =  69;
            grdJournaalDetail.Columns[3].Width = 175;
            grdJournaalDetail.Columns[4].Width =  87;            
            grdJournaalDetail.Columns[5].Width =  65;

            grdJournaalDetail.Columns[4].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;            

            Zoeken_Click(sender, e);
        }

        // ── Form_Unload → FormClosed ───────────────────────────────────────
        private void FormLedgerSQL_FormClosed(object sender, FormClosedEventArgs e)
        {
            BeWaarTekst("HistoriekInScherm", "MaxLijnen", txtLijnen.Text);
        }

        // ── GansePeriode_Click ─────────────────────────────────────────────
        private void GansePeriode_Click(object sender, EventArgs e)
        {
            if (gansePeriode.Checked)
                tekstLijn.Text = DateText(BOOKYEAR_FROMTO.Substring(0, 8))
                               + " - "
                               + DateText(BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8));
            else
                tekstLijn.Text = DateText(PERIOD_FROMTO.Substring(0, 8))
                               + " - "
                               + DateText(PERIOD_FROMTO.Substring(PERIOD_FROMTO.Length - 8));

            Zoeken_Click(sender, e);
        }

        // ── Rekening_GotFocus ──────────────────────────────────────────────
        private void Rekening_Enter(object sender, EventArgs e)
        {
            rekening.SelectAll();
            SnelHelpPrint("Dubbelklikken of [Ctrl] voor geïndexeerd zoeken", BL_LOGGING);
        }

        // ── Rekening_KeyDown ───────────────────────────────────────────────
        private void Rekening_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                aIndex   = 0;
                SharedFl = TABLE_LEDGERACCOUNTS;
                GridText = rekening.Text;
                
                using (var sqlSearch = new PublicForms.FormSearchSQL())
                    sqlSearch.ShowDialog(this);

                if (Ktrl == 0)
                {
                    rekening.Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
                    Zoeken_Click(sender, e);
                }
            }
        }

        // ── Rekening_LostFocus ─────────────────────────────────────────────
        private void Rekening_Leave(object sender, EventArgs e)
        {
            _rekeningNummer = rekening.Text;
            BGet(TABLE_LEDGERACCOUNTS, 0, _rekeningNummer);
            if (Ktrl != 0)
                this.Text = "Historiek";
            else
            {
                RecordToVeld(TABLE_LEDGERACCOUNTS);
                this.Text = "Historiek (" + VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").TrimEnd() + ")";
            }
        }

        // ── CmdHoger_Click ─────────────────────────────────────────────────
        private void CmdHoger_Click(object sender, EventArgs e)
        {
            BNext(TABLE_LEDGERACCOUNTS);
            if (Ktrl == 0)
            {
                rekening.Text = KEY_BUF[TABLE_LEDGERACCOUNTS];
                Zoeken_Click(sender, e);
            }
        }

        // ── CmdLager_Click ─────────────────────────────────────────────────
        private void CmdLager_Click(object sender, EventArgs e)
        {
            BPrev(TABLE_LEDGERACCOUNTS);
            if (Ktrl == 0)
            {
                rekening.Text = KEY_BUF[TABLE_LEDGERACCOUNTS];
                Zoeken_Click(sender, e);
            }
        }

        // ── TekstLijn_LostFocus ────────────────────────────────────────────
        private void TekstLijn_Leave(object sender, EventArgs e)
        {
            string t = tekstLijn.Text;
            string resetVal = gansePeriode.Checked
                ? DateText(BOOKYEAR_FROMTO.Substring(0, 8)) + " - " + DateText(BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8))
                : DateText(PERIOD_FROMTO.Substring(0, 8))   + " - " + DateText(PERIOD_FROMTO.Substring(PERIOD_FROMTO.Length - 8));

            if (DateInvalid(t.Length >= 13 ? t.Substring(t.Length - 10) : "") || t.Length != 23)
            {
                MessageBox.Show("Respecteer :\r\n\r\nDD/MM/EEJJ - DD/MM/EEJJ a.u.b. !");
                tekstLijn.Text = resetVal;
                tekstLijn.Focus();
                return;
            }
            Zoeken_Click(sender, e);
        }

        // ── cbKlembord_Click ───────────────────────────────────────────────
        private void CbKlembord_Click(object sender, EventArgs e)
        {
            try
            {
                var data = grdJournaalDetail.GetClipboardContent();
                if (data == null)
                {
                    MessageBox.Show("Eerst selecteren a.u.b. !");
                    return;
                }
                Cursor = Cursors.WaitCursor;
                this.Refresh();
                Clipboard.SetDataObject(data);
                Cursor = Cursors.Default;
            }
            catch
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    "Kopiëren naar het klembord was onvolledig (afhankelijk van het gebruikte " +
                    "besturingssysteem & werkgeheugen). Verklein de selectie en probeer opnieuw.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // ── Sluiten_Click ──────────────────────────────────────────────────
        private void Sluiten_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ── Zoeken_Click ───────────────────────────────────────────────────
        private void Zoeken_Click(object sender, EventArgs e)
        {
            if (_isZoeken) return;
            _isZoeken = true;
            try
            {
            SnelHelpPrint("Bezig...", BL_LOGGING);

            grdJournaalDetail.Rows.Clear();
            lblSaldo.Text  = "";
            _dTotaalSaldo  = 0;

            // parse max lines
            int maxLijn = 0;
            if (!int.TryParse(txtLijnen.Text.Trim(), out maxLijn) || maxLijn <= 0)
            {
                txtLijnen.Text = "300";
                maxLijn        = 300;
            }
            maxLijn += 2;   // matches VB6: maxLijn = TxtLijnen + 2

            // build Van / Tot keys: rekeningNummer + YYYYMMDD
            _rekeningNummer = rekening.Text.Trim();
            string t        = tekstLijn.Text;
            _van = VSet(_rekeningNummer, 7)
                 + t.Substring(6, 4) + t.Substring(3, 2) + t.Substring(0, 2);   // DD/MM/YYYY → YYYYMMDD
            _tot = VSet(_rekeningNummer, 7)
                 + t.Substring(19, 4) + t.Substring(16, 2) + t.Substring(13, 2);

            // resolve account
            BGet(TABLE_LEDGERACCOUNTS, 0, _rekeningNummer);
            if (Ktrl != 0)
                this.Text = "Historiek";
            else
            {
                RecordToVeld(TABLE_LEDGERACCOUNTS);
                this.Text = "Historiek (" + VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").TrimEnd() + ")";
            }

            grdJournaalDetail.Visible = false;
            this.Refresh();

            // ── OleDb query (ADODB equivalent) ────────────────────────────
            string sql =
                "SELECT * FROM Journalen " +
                "WHERE v070 >= '" + _van + "' AND v070 <= '" + _tot + "' " +
                "ORDER BY v035";

            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand(sql, conn))
                    using (var rs  = cmd.ExecuteReader())
                    {
                        if (!rs.HasRows)
                        {
                            grdJournaalDetail.Rows.Add("", "", "", "", "", "", "");
                        }
                        else
                        {
                            // mirror VB6: update rekening/caption from first record
                            bool first = true;
                            bool zoekverder = false;

                            while (rs.Read())
                            {
                                if (first)
                                {
                                    rekening.Text = rs["v019"]?.ToString() ?? _rekeningNummer;

                                    // narrow Van/Tot to the account in the first record (VB6: Mid(Van,1,7) = ...)
                                    string v070_7 = (rs["v070"]?.ToString() ?? "").Substring(0, Math.Min(7, (rs["v070"]?.ToString() ?? "").Length));
                                    if (v070_7.Length == 7)
                                    {
                                        _van = v070_7 + _van.Substring(7);
                                        _tot = v070_7 + _tot.Substring(7);
                                    }

                                    BGet(TABLE_LEDGERACCOUNTS, 0, rs["v019"]?.ToString() ?? "");
                                    if (Ktrl != 0)
                                        this.Text = "Historiek";
                                    else
                                    {
                                        RecordToVeld(TABLE_LEDGERACCOUNTS);
                                        this.Text = "Historiek (" + VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").TrimEnd() + ")";
                                    }
                                    first = false;
                                }

                                // ── LijnErBij (VB6 GoSub inlined) ────────
                                string v033    = rs["v033"]?.ToString() ?? "";
                                string v035    = DateText(rs["v035"]?.ToString() ?? "");
                                string v038    = rs["v038"]?.ToString() ?? "";
                                string v067    = rs["v067"]?.ToString() ?? "";
                                double dece068 = 0;
                                double.TryParse(rs["dece068"]?.ToString(), out dece068);
                                string col4    = dece068.ToString("#,##0.00");                                
                                string v069    = rs["v069"]?.ToString() ?? "";

                                _dTotaalSaldo += dece068;

                                grdJournaalDetail.Rows.Add(v033, v035, v038, v067, col4, v069);

                                // ── tussenstop (VB6 maxLijn check) ───────
                                if (grdJournaalDetail.Rows.Count >= maxLijn && !zoekverder)
                                {
                                    string msg2 = "Meer dan " + (maxLijn - 2) +
                                                  " journaallijnen.  Enkel de eerste " +
                                                  (maxLijn - 2) + " tonen ?";
                                    if (MessageBox.Show(msg2, "", MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question,
                                            MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                        break;
                                    else
                                        zoekverder = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Bron:\r\n" + ex.Source +
                    "\r\n\r\nDetail:\r\n"   + ex.Message,
                    "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                grdJournaalDetail.Visible = true;
                return;
            }

            lblSaldo.Text             = _dTotaalSaldo.ToString("#,##0.00");
            grdJournaalDetail.Visible = true;

            try { grdJournaalDetail.Focus(); } catch { }
            }
            finally
            {
                _isZoeken = false;
            }
        }
    }
}
