using System;
using System.Drawing;
using System.Windows.Forms;

using IDEALSoftware.VpeCommunity;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Filing
{
    public partial class FormProductReporting : Form
    {
        // ── State ──────────────────────────────────────────────────────────────
        private string[] _psTekst  = new string[6];   // [0]=date [2]=title [3]=subtitle
        private string[] _veldTXT  = new string[18];
        private long     _tLijnen;
        private int      _indexNR;
        private int      _tMaxVeld;

        // ── Report layout ──────────────────────────────────────────────────────
        private readonly string _fullLine = new string('-', 128);
        private string[] _rptField        = new string[24];
        private int[]    _rptTab          = new int[24];
        private double   _ypos;
        private int      _pageCounter;

        public FormProductReporting()
        {
            InitializeComponent();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Form events
        // ═══════════════════════════════════════════════════════════════════════

        private void FormProductReporting_Load(object sender, EventArgs e)
        {
            TekstLijn0.Text = "";
            TekstLijn1.Text = MIM_GLOBAL_DATE;

            CmbLijstType.Items.Add("Lijst Verkoopstock");
            CmbLijstType.Items.Add("Lijst Te Bestellen");
            CmbLijstType.Items.Add("Lijst Aankoop stockwaarde");
            CmbLijstType.SelectedIndex = 0;

            GetAllIndexes(bstNaam[TABLE_PRODUCTS], Sortering);
            for (int t = 0; t < Sortering.Items.Count; t++)
            {
                if (Sortering.Items[t].ToString().IndexOf(FLINDEX_CAPTION[TABLE_PRODUCTS, 0],
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Sortering.SelectedIndex = t;
                    break;
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Button events
        // ═══════════════════════════════════════════════════════════════════════

        private void Annuleren_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Drukken_Click(object sender, EventArgs e)
        {
            string beginSleutel = TekstInfo0.Text;
            string eindSleutel  = TekstInfo1.Text;
            _tLijnen = 0;

            _psTekst[2] = CmbLijstType.Text + " " + GetCompanyBracket();
            _psTekst[0] = TekstLijn1.Text;
            _psTekst[3] = TekstLijn0.Text;

            InitVelden();
            BClose(TABLE_PRODUCTS);
            BFirst(TABLE_PRODUCTS, _indexNR);
            BGetOrGreater(TABLE_PRODUCTS, _indexNR, beginSleutel);

            if (Ktrl != 0 ||
                string.Compare(KEY_BUF[TABLE_PRODUCTS].ToUpperInvariant(),
                               eindSleutel.ToUpperInvariant(), StringComparison.Ordinal) > 0)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Enabled        = false;

            if (Mim.Report.IsOpen()) Mim.Report.CloseDoc();
            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = GUILanguage.Dutch;
            Mim.Report.Title       = CmbLijstType.Text;

            _pageCounter = 0;
            PrintTitel();
            PrintInfo();

            do
            {
                BNext(TABLE_PRODUCTS);
                if (Ktrl != 0 ||
                    string.Compare(KEY_BUF[TABLE_PRODUCTS].Trim().ToUpperInvariant(),
                                   eindSleutel.ToUpperInvariant(), StringComparison.Ordinal) > 0)
                    break;
                PrintInfo();
            }
            while (true);

            PrintTotaal();

            Mim.Report.WriteDoc(Classes.Globals.LOCATION_COMPANYDATA);
            Mim.Report.Preview();

            Cursor.Current = Cursors.Default;
            Enabled        = true;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // ComboBox events
        // ═══════════════════════════════════════════════════════════════════════

        private void CmbLijstType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbLijstType.SelectedIndex > 1)
            {
                MessageBox.Show("Voorlopig " + DateTime.Now + " enkel eerste lijst mogelijk !");
                CmbLijstType.SelectedIndex = 0;
            }
        }

        private void Sortering_SelectedIndexChanged(object sender, EventArgs e)
        {
            _indexNR = -1;
            for (int t = 0; t < Sortering.Items.Count; t++)
            {
                if (Sortering.Text.IndexOf(FLINDEX_CAPTION[TABLE_PRODUCTS, t],
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _indexNR = t;
                    break;
                }
            }
            if (_indexNR < 0)
            {
                MessageBox.Show("Indexen zijn ondertussen vernieuwd ?" +
                    "\r\n\r\nOm van de nieuwe indexen gebruik te kunnen maken " +
                    "dient U het bedrijf te heropenen !");
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // TekstLijn focus events
        // ═══════════════════════════════════════════════════════════════════════

        private void TekstLijn0_GotFocus(object sender, EventArgs e)
        {
            TekstLijn0.SelectAll();
        }

        private void TekstLijn0_Leave(object sender, EventArgs e)
        {
            // no validation needed for subtitle line
        }

        private void TekstLijn1_GotFocus(object sender, EventArgs e)
        {
            TekstLijn1.SelectAll();
        }

        private void TekstLijn1_Leave(object sender, EventArgs e)
        {
            if (DateInvalid(TekstLijn1.Text))
            {
                System.Media.SystemSounds.Beep.Play();
                TekstLijn1.Text = MIM_GLOBAL_DATE;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report helpers
        // ═══════════════════════════════════════════════════════════════════════

        private void InitVelden()
        {
            for (int i = 0; i < _rptField.Length; i++) { _rptField[i] = ""; _rptTab[i] = 0; }

            switch (CmbLijstType.SelectedIndex)
            {
                case 0:
                    _rptField[0] = "Nummer";      _rptTab[0] = 5;
                    _rptField[1] = "Omschrijving"; _rptTab[1] = 19;
                    _rptField[2] = "VK EUR Ex.";  _rptTab[2] = 60;
                    _rptField[3] = "B";            _rptTab[3] = 73;
                    _rptField[4] = "Verpak";       _rptTab[4] = 75;
                    _rptField[5] = "Maat";         _rptTab[5] = 83;
                    _rptField[6] = "EUR incl";     _rptTab[6] = 89;
                    _rptField[7] = " Stock";       _rptTab[7] = 98;
                    _rptField[8] = "Plaats";       _rptTab[8] = 107;
                    _rptField[9] = "Vlag";         _rptTab[9] = 120;
                    _tMaxVeld = 9;
                    break;

                case 1:
                    _rptField[0] = "Nummer";      _rptTab[0] = 5;
                    _rptField[1] = "Omschrijving"; _rptTab[1] = 19;
                    _rptField[2] = "AK EUR Ex.";  _rptTab[2] = 60;
                    _rptField[3] = "B";            _rptTab[3] = 73;
                    _rptField[4] = "Verpak";       _rptTab[4] = 75;
                    _rptField[5] = "Maat";         _rptTab[5] = 83;
                    _rptField[6] = "Min.Stock";    _rptTab[6] = 89;
                    _rptField[7] = " Stock";       _rptTab[7] = 98;
                    _rptField[8] = "Bestellen";    _rptTab[8] = 107;
                    _rptField[9] = "Vlag";         _rptTab[9] = 120;
                    _tMaxVeld = 9;
                    break;

                default:
                    MessageBox.Show("Stop");
                    break;
            }
        }

        private void PrintTitel()
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
            _ypos = Mim.Report.Print(1,  1,     _psTekst[2]);
            _ypos = Mim.Report.Print(17, 1,     "Pagina : " + Dec(_pageCounter, "##########"));
            _ypos = Mim.Report.Print(17, _ypos, "Datum  : " + _psTekst[0]);

            if (!string.IsNullOrEmpty(usrLicentieInfo))
                _ypos = Mim.Report.Print(1, _ypos, usrLicentieInfo);

            if (!string.IsNullOrEmpty(_psTekst[3]))
                _ypos = Mim.Report.Print(1, _ypos, _psTekst[3].ToUpperInvariant());

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);

            // Column header line
            string headerLine = BuildReportLine(_rptField, _rptTab, _tMaxVeld + 1);
            _ypos = Mim.Report.Print(1, _ypos, headerLine);
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
        }

        private void PrintInfo()
        {
            RecordToVeld(TABLE_PRODUCTS);

            double basisbedrag;
            double basisAantal;
            string veldTXT3;
            double ticketEUR2005;

            switch (CmbLijstType.SelectedIndex)
            {
                case 0:
                    _veldTXT[0] = VBibText(TABLE_PRODUCTS, "#v102 #");
                    _veldTXT[1] = VBibText(TABLE_PRODUCTS, "#v105 #");

                    basisbedrag  = ParseDouble(VBibText(TABLE_PRODUCTS, "#e112 #"));
                    _veldTXT[2]  = Dec(basisbedrag, MASK_EUR + "00");

                    _veldTXT[3] = VBibText(TABLE_PRODUCTS, "#v111 #");
                    veldTXT3    = MidStr(FMarBoxText("002", "2", _veldTXT[3]), 4);

                    basisAantal  = ParseDouble(VBibText(TABLE_PRODUCTS, "#v107 #"));
                    _veldTXT[4]  = RightStr(Dec(basisAantal, MASK_SY[7]), 6);

                    _veldTXT[5]  = MidStr(FMarBoxText("004", "2", VBibText(TABLE_PRODUCTS, "#v106 #")), 4);

                    ticketEUR2005 = (basisbedrag * basisAantal) +
                                    (basisbedrag * basisAantal * ParseDouble(veldTXT3) / 100.0);
                    _veldTXT[6]  = RightStr(Dec(ticketEUR2005, MASK_EUR), 8);

                    _veldTXT[7]  = RightStr(Dec(
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v114 #")) +
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v119 #")) -
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v120 #")),
                        MASK_SY[7]), 6);

                    _veldTXT[8]  = VBibText(TABLE_PRODUCTS, "#v109 #");
                    _veldTXT[9]  = VBibText(TABLE_PRODUCTS, "#v125 #");
                    break;

                case 1:
                    _veldTXT[0] = VBibText(TABLE_PRODUCTS, "#v102 #");
                    _veldTXT[1] = VBibText(TABLE_PRODUCTS, "#v105 #");

                    basisbedrag  = ParseDouble(VBibText(TABLE_PRODUCTS, "#e113 #"));
                    _veldTXT[2]  = Dec(basisbedrag, MASK_EUR + "00");

                    _veldTXT[3] = VBibText(TABLE_PRODUCTS, "#v111 #");
                    veldTXT3    = MidStr(FMarBoxText("002", "2", _veldTXT[3]), 4);

                    basisAantal  = ParseDouble(VBibText(TABLE_PRODUCTS, "#v107 #"));
                    _veldTXT[4]  = RightStr(Dec(basisAantal, MASK_SY[7]), 6);
                    _veldTXT[5]  = MidStr(FMarBoxText("004", "2", VBibText(TABLE_PRODUCTS, "#v106 #")), 4);

                    _veldTXT[6]  = RightStr(Dec(
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v115 #")), MASK_SY[7]), 6);

                    _veldTXT[7]  = RightStr(Dec(
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v114 #")) +
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v119 #")) -
                        ParseDouble(VBibText(TABLE_PRODUCTS, "#v120 #")),
                        MASK_SY[7]), 6);

                    _veldTXT[8]  = Dec(ParseDouble(_veldTXT[6]) - ParseDouble(_veldTXT[7]), MASK_SY[7]);
                    _veldTXT[9]  = VBibText(TABLE_PRODUCTS, "#v125 #");
                    break;

                default:
                    return;
            }

            PrintVelden();
        }

        private void PrintVelden()
        {
            // Case 1: bestellijst — only print when needed (VeldTXT[9] > 0 means flag set)
            if (CmbLijstType.SelectedIndex == 1)
            {
                if (ParseDouble(_veldTXT[9]) <= 0) return;
            }

            _tLijnen++;
            string line = BuildReportLine(_veldTXT, _rptTab, _tMaxVeld + 1);
            _ypos = Mim.Report.Print(1, _ypos, line);

            CheckPageBreak();
        }

        private void PrintTotaal()
        {
            for (int t = 0; t <= _tMaxVeld; t++) _veldTXT[t] = "";
            _veldTXT[1] = "Totaal aantal lijnen : " + _tLijnen.ToString();

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            string line = BuildReportLine(_veldTXT, _rptTab, _tMaxVeld + 1);
            _ypos = Mim.Report.Print(1, _ypos, line);
        }

        private void CheckPageBreak()
        {
            if (_ypos > 27.5)
            {
                Mim.Report.PageBreak();
                PrintTitel();
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Static helpers
        // ═══════════════════════════════════════════════════════════════════════

        private static string BuildReportLine(string[] fields, int[] tabs, int count)
        {
            char[] line = new string(' ', 128).ToCharArray();
            for (int i = 0; i < count && i < fields.Length && i < tabs.Length; i++)
            {
                int col = tabs[i];
                if (col <= 0 || col >= line.Length) continue;
                string val = fields[i] ?? "";
                int avail = line.Length - col;
                if (val.Length > avail) val = val.Substring(0, avail);
                for (int j = 0; j < val.Length; j++)
                    line[col + j] = val[j];
            }
            return new string(line);
        }

        private static double ParseDouble(string s)
        {
            double.TryParse((s ?? "").Trim(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double v);
            return v;
        }

        /// <summary>VB6 Mid(s, start) — 1-based, returns "" when start beyond length.</summary>
        private static string MidStr(string s, int start)
        {
            if (string.IsNullOrEmpty(s) || start > s.Length) return "";
            return s.Substring(start - 1);
        }

        /// <summary>VB6 Right(s, len).</summary>
        private static string RightStr(string s, int len)
        {
            if (string.IsNullOrEmpty(s) || len <= 0) return "";
            return s.Length <= len ? s : s.Substring(s.Length - len);
        }

        private static string GetCompanyBracket()
        {
            string caption = Mim.Text;
            int s = caption.IndexOf('[');
            return s >= 0 ? caption.Substring(s) : "";
        }
    }
}

