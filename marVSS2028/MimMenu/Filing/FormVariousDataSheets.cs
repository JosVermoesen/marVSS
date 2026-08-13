using System;
using System.Drawing;
using System.Windows.Forms;

using IDEALSoftware.VpeCommunity;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Filing
{
    public partial class FormVariousDataSheets : Form
    {
        // ── State ──────────────────────────────────────────────────────────────
        private string[] _veldTXT  = new string[18];
        private double   _dTas;
        private int      _tMaxVeld;

        // ── Report layout ──────────────────────────────────────────────────────
        private readonly string _fullLine = new string('-', 128);
        private string[] _rptField        = new string[24];
        private int[]    _rptTab          = new int[24];
        private string[] _psText          = new string[6];  // [0]=date [2]=title [3]=subtitle
        private double   _ypos;
        private int      _pageCounter;

        public FormVariousDataSheets()
        {
            InitializeComponent();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Form events
        // ═══════════════════════════════════════════════════════════════════════

        private void FormVariousDataSheets_Load(object sender, EventArgs e)
        {
            CmbDokumentType.Items.Add("18: Investeringsfiches");
            CmbDokumentType.Items.Add("10: MuntKodes, dagkoersen");
            CmbDokumentType.Items.Add("12: Logboek Artikels");
            CmbDokumentType.Items.Add("21: Forfaitaire Btw cumulators");
            CmbDokumentType.Items.Add("28: Financiële Instellingen");
            CmbDokumentType.SelectedIndex = 0;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // ComboBox
        // ═══════════════════════════════════════════════════════════════════════

        private void CmbDokumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtKey.Text      = "";
            BtnEdit.Enabled  = false;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Key (TxtKey) events
        // ═══════════════════════════════════════════════════════════════════════

        private void TxtKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                BtnNewSheet.Focus();
                e.Handled = true;
            }
        }

        private void TxtKey_Leave(object sender, EventArgs e)
        {
            if (TxtKey.Text.Trim().Length == 0) return;

            BGet(TABLE_VARIOUS, 1, DocTypePrefix() + VSet(TxtKey.Text.Trim(), 18));
            if (Ktrl == 0)
            {
                RecordNaarFiche();
                INSERT_FLAG[TABLE_VARIOUS] = 0;
                BtnSave.Enabled = true;
            }
            else
            {
                BtnSave.Enabled = false;
            }

            if (INSERT_FLAG[TABLE_VARIOUS] == 1)
                FillInsertRecord();

            BtnEdit.Enabled = true;
            BtnEdit.Focus();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Buttons
        // ═══════════════════════════════════════════════════════════════════════

        private void BtnPrint_Click(object sender, EventArgs e)   // Knop(2)
        {
            string prefix = DocTypePrefix();
            switch (prefix)
            {
                case "18":
                    if (AfschrijvingenLijstOk())
                    {
                        // report was already generated inside AfschrijvingenLijstOk
                    }
                    break;
                default:
                    MessageBox.Show("Nog geen afdrukdefinitie beschikbaar voor " + CmbLabel(),
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void BtnNewSheet_Click(object sender, EventArgs e) // Knop(3)
        {
            NieuweFiche();
            TxtKey.Focus();
        }

        private void BtnEdit_Click(object sender, EventArgs e)    // Knop(5)
        {
            if (!TeleBibClick(int.Parse(DocTypePrefix())))
            {
                BtnSave.Enabled = false;
            }
            else
            {
                BtnSave.Enabled = true;
                BtnSave.Focus();
                FicheNaarRecord();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)    // Knop(1)
        {
            FicheNaarRecord();
            NieuweFiche();
            TxtKey.Focus();
        }

        private void BtnPrev_Click(object sender, EventArgs e)    // Knop(6)
        {
            BPrev(TABLE_VARIOUS);
            if (Ktrl == 9)
            {
                BFirst(TABLE_VARIOUS, 1);
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    BtnEdit.Enabled = false;
                }
            }
            else if (DocTypePrefix() == KEY_BUF[TABLE_VARIOUS].Substring(0, 2))
            {
                INSERT_FLAG[TABLE_VARIOUS] = 0;
                RecordNaarFiche();
                BtnEdit.Enabled = true;
                return;
            }

            if (Ktrl == 0)
            {
                BGetOrGreater(TABLE_VARIOUS, 1, VSet(DocTypePrefix(), 20));
                if (Ktrl == 0 && KEY_BUF[TABLE_VARIOUS].Length >= 2 &&
                    KEY_BUF[TABLE_VARIOUS].Substring(0, 2) == DocTypePrefix())
                {
                    INSERT_FLAG[TABLE_VARIOUS] = 0;
                    RecordNaarFiche();
                    BtnEdit.Enabled = true;
                }
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)    // Knop(7)
        {
            BNext(TABLE_VARIOUS);
            if (Ktrl == 9)
            {
                BLast(TABLE_VARIOUS, 1);
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    BtnEdit.Enabled = false;
                }
            }
            else
            {
                if (KEY_BUF[TABLE_VARIOUS].Length >= 2 &&
                    KEY_BUF[TABLE_VARIOUS].Substring(0, 2) == DocTypePrefix())
                {
                    INSERT_FLAG[TABLE_VARIOUS] = 0;
                    RecordNaarFiche();
                    BtnEdit.Enabled = true;
                }
            }
        }

        private void BtnMinimize_Click(object sender, EventArgs e) // Knop(8)
        {
            WindowState = FormWindowState.Minimized;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Delete via Delete key (VB6 Knop_KeyDown with KeyCode=46)
        // ═══════════════════════════════════════════════════════════════════════

        private void FormVariousDataSheets_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RecordToVeld(TABLE_VARIOUS);
                string prefix = DocTypePrefix();
                string fvt1   = FVT[TABLE_VARIOUS, 1];
                if (fvt1.Length >= 3 &&
                    prefix == fvt1.Substring(0, 2) &&
                    TxtKey.Text.TrimEnd() == fvt1.Substring(2).TrimEnd())
                {
                    if (MessageBox.Show("Bestaande fiche verwijderen.  Bent U zeker ?",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        BDelete(TABLE_VARIOUS);
                        NieuweFiche();
                    }
                }
                e.Handled = true;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Internal record helpers
        // ═══════════════════════════════════════════════════════════════════════

        private void NieuweFiche()
        {
            TxtKey.Text                = "";
            TLB_RECORD[TABLE_VARIOUS]  = "";
            INSERT_FLAG[TABLE_VARIOUS] = 1;
            BtnSave.Enabled            = false;
            BtnEdit.Enabled            = false;
            TxtKey.Enabled             = true;
        }

        private void RecordNaarFiche()
        {
            TLB_RECORD[TABLE_VARIOUS] = "";
            if (Ktrl != 0)
            {
                MessageBox.Show("stop", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            RecordToVeld(TABLE_VARIOUS);
            string raw = VBibText(TABLE_VARIOUS, "#v005 #");
            TxtKey.Text                = raw.Length >= 3 ? raw.Substring(2) : "";
            INSERT_FLAG[TABLE_VARIOUS] = 0;
        }

        private void FicheNaarRecord()
        {
            BGet(TABLE_VARIOUS, 1, VSet(
                VBibText(TABLE_VARIOUS, "#" + JETTABLEUSE_INDEX[TABLE_VARIOUS, 1] + "#"),
                FLINDEX_LEN[TABLE_VARIOUS, 1]));

            if (Ktrl == 0)
            {
                if (MessageBox.Show("Gegevens bestaande fiche wijzigen.  Bent U zeker ?",
                        "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FillKeyFields();
                    BUpdate(TABLE_VARIOUS, 1);
                }
            }
            else
            {
                BInsert(TABLE_VARIOUS, 1);
            }
        }

        private void FillInsertRecord()
        {
            switch (DocTypePrefix())
            {
                case "10":
                    VBib(TABLE_VARIOUS, TxtKey.Text, "vs03");
                    VBib(TABLE_VARIOUS, DocTypePrefix() + VBibText(TABLE_VARIOUS, "#vs03 #"), "v005");
                    break;
                case "12":
                    VBib(TABLE_VARIOUS, TxtKey.Text, "v152");
                    VBib(TABLE_VARIOUS, DocTypePrefix() + VBibText(TABLE_VARIOUS, "#v152 #"), "v005");
                    break;
                case "18":
                    VBib(TABLE_VARIOUS, TxtKey.Text, "v087");
                    VBib(TABLE_VARIOUS, DocTypePrefix() + VBibText(TABLE_VARIOUS, "#v087 #"), "v005");
                    break;
                case "21":
                    VBib(TABLE_VARIOUS, TxtKey.Text, "v216");
                    VBib(TABLE_VARIOUS, DocTypePrefix() + VBibText(TABLE_VARIOUS, "#v216 #"), "v005");
                    break;
                case "28":
                    VBib(TABLE_VARIOUS, TxtKey.Text, "v231");
                    VBib(TABLE_VARIOUS, DocTypePrefix() + VBibText(TABLE_VARIOUS, "#v231 #"), "v005");
                    break;
                default:
                    MessageBox.Show("stop", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
            }
        }

        private void FillKeyFields()
        {
            // same field mapping used in both FicheNaarRecord and TxtKey_Leave insert path
            FillInsertRecord();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════════════════

        private string DocTypePrefix()
        {
            string txt = CmbDokumentType.Text;
            return txt.Length >= 2 ? txt.Substring(0, 2) : "  ";
        }

        private string CmbLabel()
            => CmbDokumentType.Text.Length >= 4 ? CmbDokumentType.Text.Substring(4) : CmbDokumentType.Text;

        // ═══════════════════════════════════════════════════════════════════════
        // Report: depreciation list (AfschrijvingenLijstOk)
        // ═══════════════════════════════════════════════════════════════════════

        private bool AfschrijvingenLijstOk()
        {
            bool bhKontrole = false;

            string flag63 = String99(63);
            if (flag63 == "1")
            {
                string msg =
                    "Afschrijvingsposten reeds gegenereerd voor dit boekjaar !\r\n\r\n" +
                    "Bijkomende posten kunnen uitsluitend via 'Diverse post'-optie\r\n" +
                    "ingebracht worden !\r\n\r\n" +
                    "Wenst U boekhoudkundige kontrolelijst ?";
                if (MessageBox.Show(msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    bhKontrole = true;
                }
                else
                {
                    return false;
                }
            }
            else if (flag63 != "0")
            {
                MessageBox.Show("Setup boekjaar en parameters bevat niet de juiste vlag geboekt of niet geboekt.  Kontroleer",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (String99(64) != "1")
            {
                MessageBox.Show("Onlogische situatie.  Dit boekjaar bevat nog geen beginbalans ?  De beginbalans dient aanwezig te zijn.  Mogelijk bevindt U zich in het verkeerde boekjaar ?",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _dTas = 0;
            BGetOrGreater(TABLE_VARIOUS, 1, VSet("18", 20));
            if (Ktrl != 0 || KEY_BUF[TABLE_VARIOUS].Substring(0, 2) != "18")
            {
                MessageBox.Show("Er zijn geen investeringsfiches !",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            Enabled        = false;

            // Open Mim.Report
            if (Mim.Report.IsOpen()) Mim.Report.CloseDoc();
            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = GUILanguage.Dutch;
            Mim.Report.Title       = "Investeringsfiches";
            _pageCounter = 0;

            _psText[0] = MIM_GLOBAL_DATE;
            _psText[2] = CmbLabel() + " " + GetCompanyBracket();

            InitVelden();
            ReportPrintHeader();

            AddDepreciationLine(bhKontrole);
            do
            {
                BNext(TABLE_VARIOUS);
                if (Ktrl != 0 || KEY_BUF[TABLE_VARIOUS].Substring(0, 2) != "18") break;
                AddDepreciationLine(bhKontrole);
            } while (true);

            PrintTotaal();

            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.Preview();

            Enabled        = true;
            Cursor.Current = Cursors.Default;

            return !bhKontrole;
        }

        // ── AddDepreciationLine (VB6 AfschrijvingsLijnErBij GoSub) ─────────────

        private void AddDepreciationLine(bool bhKontrole)
        {
            RecordToVeld(TABLE_VARIOUS);

            string rawDate = VBibText(TABLE_VARIOUS, "#v083 #");
            if (rawDate.Trim().Length != 8)
            {
                MessageBox.Show("Datumformaat '" + rawDate.Trim() + "' onjuist voor " +
                    VBibText(TABLE_VARIOUS, "#v087 #"),
                    "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Convert yyyymmdd → dd/mm/yyyy
            string datumJaarInvest =
                rawDate.Substring(0, 2) + "/" +
                rawDate.Substring(2, 2) + "/" +
                rawDate.Substring(4);

            if (DateKey(datumJaarInvest).CompareTo(BOOKYEAR_FROMTO.Substring(8)) > 0)
                return;

            int    ipct  = int.Parse(VBibText(TABLE_VARIOUS, "#v082 #").Trim().Length > 0
                               ? VBibText(TABLE_VARIOUS, "#v082 #") : "0");
            double dbdrg = ParseDouble(VBibText(TABLE_VARIOUS, "#v084 #"));

            for (int i = 0; i < _veldTXT.Length; i++) _veldTXT[i] = "";

            _veldTXT[4] = datumJaarInvest;
            _veldTXT[5] = Dec(dbdrg, MASK_2002);
            _veldTXT[7] = Dec(ipct,  "#####");

            // Investment account
            _veldTXT[3] = VBibText(TABLE_VARIOUS, "#v019 #").Trim();
            BGet(TABLE_LEDGERACCOUNTS, 0, _veldTXT[3]);
            _veldTXT[0] = Ktrl != 0 ? "Niet (meer) aanwezig..." : LedgerName(TABLE_LEDGERACCOUNTS);

            // Depreciation account
            _veldTXT[6] = VBibText(TABLE_VARIOUS, "#v087 #").Trim();
            BGet(TABLE_LEDGERACCOUNTS, 0, _veldTXT[6]);
            _veldTXT[1] = Ktrl != 0 ? "Niet (meer) aanwezig..." : LedgerName(TABLE_LEDGERACCOUNTS);

            // Depreciation cost account
            _veldTXT[9] = VBibText(TABLE_VARIOUS, "#v088 #").Trim();
            BGet(TABLE_LEDGERACCOUNTS, 0, _veldTXT[9]);
            _veldTXT[2] = Ktrl != 0 ? "Niet (meer) aanwezig..." : LedgerName(TABLE_LEDGERACCOUNTS);

            double das;
            if (bhKontrole)
            {
                das = CalcDepreciationFromJournal(_veldTXT[6], out double dRa);
                _veldTXT[8]  = Dec(dRa,  MASK_2002);
                _veldTXT[10] = Dec(das,  MASK_2002);
            }
            else
            {
                double dRa = ParseDouble(VBibText(TABLE_VARIOUS, "#v085 #"));
                if (dbdrg == ParseDouble(VBibText(TABLE_VARIOUS, "#v085 #")))
                {
                    das = 0;
                }
                else
                {
                    double portion = Math.Truncate(dbdrg / ipct);
                    double dRest   = dbdrg - (dRa + portion);
                    das = Math.Abs(dRest) < 20 ? portion + dRest : portion;
                }
                _veldTXT[8]  = Dec(dRa,  MASK_2002);
                _veldTXT[10] = Dec(das,  MASK_2002);
            }

            _dTas += das;
            PrintVelden();
        }

        private string LedgerName(int table)
        {
            RecordToVeld(table);
            return VBibText(table, "#v020 #");
        }

        // ── CalcDepreciationFromJournal (bhSPECIAAL + OverLOOPJournaal GoSubs) ─

        private double CalcDepreciationFromJournal(string rekKontrole, out double dRa)
        {
            string fromKey = rekKontrole + BOOKYEAR_FROMTO.Substring(0, 8);
            string toKey   = rekKontrole + BOOKYEAR_FROMTO.Substring(8);

            double bedragBegin = 0;
            double das         = 0;
            string dummyDatum  = "";

            BGetOrGreater(TABLE_JOURNAL, 0, fromKey);
            if (Ktrl == 0 && KEY_BUF[TABLE_JOURNAL].CompareTo(toKey) <= 0)
            {
                do
                {
                    RecordToVeld(TABLE_JOURNAL);
                    string jDatum = VBibText(TABLE_JOURNAL, "#v066 #");
                    double jBedrag = ParseDouble(VBibText(TABLE_JOURNAL, "#v068 #"));

                    if (dummyDatum.Trim().Length == 0)
                    {
                        dummyDatum   = jDatum;
                        bedragBegin  = jBedrag;
                    }
                    else if (dummyDatum == jDatum)
                    {
                        bedragBegin += jBedrag;
                    }
                    else
                    {
                        das += jBedrag;
                    }

                    BNext(TABLE_JOURNAL);
                    if (Ktrl != 0 || KEY_BUF[TABLE_JOURNAL].CompareTo(toKey) > 0) break;
                } while (true);

                if (das == 0)
                {
                    double orig = ParseDouble(VBibText(TABLE_VARIOUS, "#v084 #"));
                    double rest = ParseDouble(VBibText(TABLE_VARIOUS, "#v085 #"));
                    das = orig != rest ? Math.Abs(bedragBegin) : 0;
                }
                else
                {
                    das = Math.Abs(das);
                }
            }

            dRa = ParseDouble(VBibText(TABLE_VARIOUS, "#v085 #")) - das;
            return das;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report helpers
        // ═══════════════════════════════════════════════════════════════════════

        private void InitVelden()
        {
            switch (DocTypePrefix())
            {
                case "18":
                    _rptField[0]  = "Investeringsrekening";     _rptTab[0]  = 2;
                    _rptField[1]  = "Rekening Afschrijvingen";  _rptTab[1]  = 43;
                    _rptField[2]  = "Afschrijvingskosten Rekening"; _rptTab[2] = 84;
                    _rptField[3]  = "Nummer";                   _rptTab[3]  = 2;
                    _rptField[4]  = "AankpDatum";               _rptTab[4]  = 10;
                    _rptField[5]  = "Tot.Bedrag";               _rptTab[5]  = 21;
                    _rptField[6]  = "Nummer";                   _rptTab[6]  = 43;
                    _rptField[7]  = "Jaren";                    _rptTab[7]  = 51;
                    _rptField[8]  = "Reeds afg.";               _rptTab[8]  = 57;
                    _rptField[9]  = "Nummer";                   _rptTab[9]  = 84;
                    _rptField[10] = " Bedrag Af";               _rptTab[10] = 92;
                    _tMaxVeld = 10;
                    break;
                default:
                    MessageBox.Show("Stop, nog niets voorzien.",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
            }
        }

        private void ReportPrintHeader()
        {
            Mim.Report.SelectFont("Courier New", 7);
            Mim.Report.TextBold      = true;
            Mim.Report.TextColor     = ColorTranslator.FromOle(0);
            Mim.Report.nTopMargin    = 1;
            Mim.Report.nBottomMargin = 29;
            Mim.Report.nLeftMargin   = 0.5;
            Mim.Report.nRightMargin  = 0.5;
            Mim.Report.PenSize       = 0.01;

            _pageCounter++;
            _ypos = Mim.Report.Print(1,  1,     _psText[2]);
            _ypos = Mim.Report.Print(17, 1,     "Pagina : " + Dec(_pageCounter, "##########"));
            _ypos = Mim.Report.Print(17, _ypos, "Datum  : " + _psText[0]);

            // Column headers — row 1 (account titles)
            string line1 = BuildReportLine(new[] { _rptField[0], _rptField[1], _rptField[2] },
                                           new[] { _rptTab[0],   _rptTab[1],   _rptTab[2] });
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            _ypos = Mim.Report.Print(1, _ypos, line1);

            // Column headers — row 2 (detail fields)
            string[] hdr2  = new string[11];
            int[]    tab2  = new int[11];
            for (int i = 3; i <= 10; i++) { hdr2[i - 3] = _rptField[i]; tab2[i - 3] = _rptTab[i]; }
            _ypos = Mim.Report.Print(1, _ypos, BuildReportLine(hdr2, tab2));
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
        }

        private void CheckPageBreak()
        {
            if (_ypos > 27.5)
            {
                Mim.Report.PageBreak();
                ReportPrintHeader();
            }
        }

        private void PrintVelden()
        {
            // Row 1: account names
            string line1 = BuildReportLine(
                new[] { _veldTXT[0], _veldTXT[1], _veldTXT[2] },
                new[] { _rptTab[0],  _rptTab[1],  _rptTab[2] });
            _ypos = Mim.Report.Print(1, _ypos, line1);

            // Row 2: numeric / code detail
            string[] det  = new string[8];
            int[]    dtab = new int[8];
            for (int i = 3; i <= 10; i++) { det[i - 3] = _veldTXT[i]; dtab[i - 3] = _rptTab[i]; }
            _ypos = Mim.Report.Print(1, _ypos, BuildReportLine(det, dtab));

            CheckPageBreak();
        }

        private void PrintTotaal()
        {
            string[] tot  = new string[11];
            tot[1]  = "Totaal :";
            tot[10] = Dec(_dTas, MASK_2002);

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            string line2 = BuildReportLine(
                new[] { tot[3], tot[4], tot[5], tot[6], tot[7], tot[8], tot[9], tot[10] },
                new[] { _rptTab[3], _rptTab[4], _rptTab[5], _rptTab[6], _rptTab[7], _rptTab[8], _rptTab[9], _rptTab[10] });
            _ypos = Mim.Report.Print(1, _ypos, tot[1]);  // "Totaal :" at left
            _ypos = Mim.Report.Print(1, _ypos, line2);
        }

        private static string BuildReportLine(string[] fields, int[] tabs)
        {
            string line = new string(' ', 128);
            for (int i = 0; i < fields.Length && i < tabs.Length; i++)
            {
                if (tabs[i] <= 0 || tabs[i] >= line.Length) continue;
                string val   = fields[i] ?? "";
                int    avail = 128 - tabs[i];
                if (val.Length > avail) val = val.Substring(0, avail);
                line = line.Substring(0, tabs[i]) + val +
                       line.Substring(Math.Min(128, tabs[i] + val.Length));
            }
            return line.Length > 128 ? line.Substring(0, 128) : line;
        }

        private static double ParseDouble(string s)
        {
            double.TryParse(s.Trim(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double v);
            return v;
        }

        private static string GetCompanyBracket()
        {
            string caption = Mim.Text;
            int s = caption.IndexOf('[');
            return s >= 0 ? caption.Substring(s) : "";
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}

