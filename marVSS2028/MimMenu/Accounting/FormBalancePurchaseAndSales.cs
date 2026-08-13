using System;
using System.Collections.Generic;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormBalancePurchaseAndSales : Form
    {
        // ── Module-level fields ────────────────────────────────────────────────
        public  int    _tableIndex;   // set by caller before ShowDialog (TABLE_CUSTOMERS / TABLE_SUPPLIERS)

        private string[] _psTekst    = new string[6];
        private string   _lijstNaam  = string.Empty;
        private string[] _veldTXT    = new string[21];
        private int      _flPartij;

        private double _totaalBTW, _totaalGOED, _totaalALBETAALD;
        private double _totaalNOGTEBETALEN, _totaalVOOR, _totaalNA;
        private double _dttot, _dtbtw, _dTrb;

        private string _plGrensVan = string.Empty;   // YYYYMMDD
        private string _plGrensTot = string.Empty;   // YYYYMMDD

        private int    _aantalBovenPeriode, _aantalOnderPeriode;
        private double _totaalDokBovenPeriode, _totaalDokOnderPeriode;

        // Replaces VB6 invisible BetalingenVoorNa(0-3) ListBox control arrays
        private readonly List<string>[] _betalingenVoorNa =
        {
            new List<string>(), new List<string>(),
            new List<string>(), new List<string>()
        };

        // Report layout (mirrors VB6 REPORT_FIELD / REPORT_TAB globals locally)
        private string[] _reportField = new string[12];
        private int[]    _reportTab   = new int[12];
        private string   _fullLine    = new string('-', 128);
        private double   _ypos;
        private int      _pageCounter;

        // ── Constructor ────────────────────────────────────────────────────────
        public FormBalancePurchaseAndSales()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ══════════════════════════════════════════════════════════════════════
        // Form Load
        // ══════════════════════════════════════════════════════════════════════
        private void FormBalancePurchaseAndSales_Load(object sender, EventArgs e)
        {
            _plGrensVan = BOOKYEAR_FROMTO.Substring(0, 8);
            _plGrensTot = BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8);

            CheckBoxExclude1994.Checked = true;
            // Restore saved settings
            CheckBoxLedgerDetail.Checked        = LaadTekst("DocumentsBalance", "BetalingsKontrole")           == "1";
            CheckBoxExpiryDate.Checked        = LaadTekst("DocumentsBalance", "KontroleVervaldag")            == "1";
            CheckBoxExcludeOutOfPeriod.Checked        = LaadTekst("DocumentsBalance", "GeenBetalingHogerBoekjaar")    == "1";
            CheckBoxOnlyThisPeriod.Checked        = LaadTekst("DocumentsBalance", "PeriodeBegrenzen")             == "1";
            CheckBoxNotPaid.Checked        = LaadTekst("DocumentsBalance", "EnkelNietBetaaldedokumenten")  == "1";            
            CheckBoxFinanceDetail.Checked = LaadTekst("DocumentsBalance", "FinancieelDetailViaJournaal")  == "1";
            
            txtDatum.Text   = MIM_GLOBAL_DATE;
            txtPeriode.Text = DateText(BOOKYEAR_FROMTO.Substring(0, 8))
                            + " - "
                            + DateText(BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8));

            switch (_tableIndex)
            {
                case TABLE_SUPPLIERS:
                    _lijstNaam = "Balans Leveranciers";
                    _flPartij  = TABLE_SUPPLIERS;
                    break;
                case TABLE_CUSTOMERS:
                    _lijstNaam = "Balans Klanten";
                    _flPartij  = TABLE_CUSTOMERS;
                    break;
                default:
                    MessageBox.Show("stop balans partijen!");
                    Close();
                    return;
            }

            if (XisEuroWisBEF)
                _lijstNaam += " (Speciale modus: Alle cijfers in BEF !)";

            Text        = _lijstNaam;
            txtVan.Text = "0";
            txtTot.Text = new string('z', 12);
        }
                
        private void ChkFinancieelDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (!CheckBoxFinanceDetail.Checked)
            {
                CheckBoxExcludeOutOfPeriod.Checked = false;
                CheckBoxLedgerDetail.Checked = false;
                CheckBoxLedgerDetail.Enabled = false;
            }
            else
            {
                CheckBoxLedgerDetail.Enabled = true;
            }
        }

        private void CmdBewaar_Click(object sender, EventArgs e)
        {
            BeWaarTekst("DocumentsBalance", "KontroleVervaldag",           CheckBoxExpiryDate.Checked        ? "1" : "0");
            BeWaarTekst("DocumentsBalance", "GeenBetalingHogerBoekjaar",   CheckBoxExcludeOutOfPeriod.Checked        ? "1" : "0");
            BeWaarTekst("DocumentsBalance", "PeriodeBegrenzen",            CheckBoxOnlyThisPeriod.Checked        ? "1" : "0");
            BeWaarTekst("DocumentsBalance", "EnkelNietBetaaldedokumenten", CheckBoxNotPaid.Checked        ? "1" : "0");
            BeWaarTekst("DocumentsBalance", "BetalingsKontrole",           CheckBoxLedgerDetail.Checked        ? "1" : "0");
            BeWaarTekst("DocumentsBalance", "FinancieelDetailViaJournaal", CheckBoxFinanceDetail.Checked ? "1" : "0");            
        }

        private void CmdStandaard_Click(object sender, EventArgs e)
        {
            CheckBoxExpiryDate.Checked        = false;
            _plGrensVan                 = BOOKYEAR_FROMTO.Substring(0, 8);
            _plGrensTot                 = BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8);
            txtPeriode.Text             = DateText(_plGrensVan) + " - " + DateText(_plGrensTot);
            CheckBoxOnlyThisPeriod.Checked        = true;
            CheckBoxExcludeOutOfPeriod.Checked        = true;
            CheckBoxNotPaid.Checked        = true;
            CheckBoxFinanceDetail.Checked = true;
            CheckBoxLedgerDetail.Checked        = true;
            txtSubTitel.Text            = "Boekhoudcontrole " + txtPeriode.Text;
        }

        private void CmdStandaardBetaling_Click(object sender, EventArgs e)
        {
            CheckBoxExpiryDate.Checked = true;
            CheckBoxOnlyThisPeriod.Checked = false;
            CheckBoxExcludeOutOfPeriod.Checked = false;
            CheckBoxNotPaid.Checked = true;
            CheckBoxFinanceDetail.Checked = false;            
            txtDatum.Text               = MIM_GLOBAL_DATE;
            txtSubTitel.Text            = "Betalingscontrole";
        }

        // ── cmdEuroCheck: body intentionally empty (VB6 had Exit Sub at top) ─
        private void CmdEuroCheck_Click(object sender, EventArgs e) { }

        // ── Selektie(0): update datum when vervaldag-check toggled ────────────
        private void ChkSelektie0_CheckedChanged(object sender, EventArgs e)
        {
            txtDatum.Text = CheckBoxExpiryDate.Checked
                ? DateText(BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8))
                : MIM_GLOBAL_DATE;
        }

        // ── Selektie(4): warn when "exclude pre-1994 docs" is activated ───────
        private void ChkSelektie4_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxExclude1994.Checked)
                MessageBox.Show(
                    "Schakel uitsluitend aan indien U problemen ondervindt met sommige " +
                    "geimporteerde DOS-dokumenten van voor 1994 (o.a. BTW 33 %, 8 % luxetaks).\r\n\r\n" +
                    "Indien U alle mogelijkheden van marIntegraal Windows versie met uw oude data " +
                    "wenst te benutten, gelieve ons pér bedrijf een veiligheidskopij te bezorgen.\r\n\r\n" +
                    "Binnen uw servicecontract werken wij deze kosteloos om in onze lokalen.");
        }

        // ── TextBox Enter: select all ─────────────────────────────────────────
        private void TekstLijn_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox tb) tb.SelectAll();
        }

        // ── txtDatum Leave ────────────────────────────────────────────────────
        private void TxtDatum_Leave(object sender, EventArgs e)
        {
            if (DateInvalid(txtDatum.Text))
            {
                System.Media.SystemSounds.Beep.Play();
                txtDatum.Text = MIM_GLOBAL_DATE;
            }
        }

        // ── txtPeriode Leave ──────────────────────────────────────────────────
        private void TxtPeriode_Leave(object sender, EventArgs e)
        {
            string t = txtPeriode.Text;
            string resetVal = DateText(BOOKYEAR_FROMTO.Substring(0, 8))
                            + " - "
                            + DateText(BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8));

            if (t.Length != 23 || DateInvalid(t.Substring(t.Length - 10)))
            {
                MessageBox.Show("Respecteer :\r\n\r\nDD/MM/EEJJ - DD/MM/EEJJ a.u.b. !");
                txtPeriode.Text = resetVal;
                txtPeriode.Focus();
                return;
            }

            _plGrensVan = t.Substring(6, 4) + t.Substring(3, 2) + t.Substring(0, 2);
            _plGrensTot = t.Substring(19, 4) + t.Substring(16, 2) + t.Substring(13, 2);

            bool fullBookYear = BOOKYEAR_FROMTO == _plGrensVan + _plGrensTot;
            CheckBoxLedgerDetail.Checked = fullBookYear;
            CheckBoxLedgerDetail.Visible = fullBookYear;
        }

        // ══════════════════════════════════════════════════════════════════════
        // Drukken (print / report)
        // ══════════════════════════════════════════════════════════════════════
        private void Drukken_Click(object sender, EventArgs e)
        {
            string beginSleutel, eindSleutel;
            switch (_flPartij)
            {
                case TABLE_SUPPLIERS:
                    beginSleutel = "L" + txtVan.Text;
                    eindSleutel  = "L" + txtTot.Text;
                    break;
                default: // TABLE_CUSTOMERS
                    beginSleutel = "K" + txtVan.Text;
                    eindSleutel  = "K" + txtTot.Text;
                    break;
            }

            string rdtemp = DateKey(txtDatum.Text);

            // Compose report header info
            string companyBracket = string.Empty;
            if (Application.OpenForms["FormMim"] is FormMim mimRef)
            {
                int idx = mimRef.Text.IndexOf('[');
                if (idx >= 0) companyBracket = mimRef.Text.Substring(idx);
            }
            _psTekst[2] = Text + " " + companyBracket;
            _psTekst[0] = txtDatum.Text;
            _psTekst[3] = txtSubTitel.Text;

            InitVelden();

            // Reset totals
            _totaalBTW            = 0;
            _totaalGOED           = 0;
            _totaalALBETAALD      = 0;
            _totaalNOGTEBETALEN   = 0;
            _totaalVOOR           = 0;
            _totaalNA             = 0;
            _dttot = _dtbtw = _dTrb = 0;
            _betalingenVoorNa[2].Clear();
            _betalingenVoorNa[3].Clear();

            string tds = "Geen journalen voor : \r\n";

            BGetOrGreater(TABLE_INVOICES, 1, beginSleutel);
            if (Ktrl != 0
                || string.Compare(VSet(KEY_BUF[TABLE_INVOICES], 13), eindSleutel, StringComparison.OrdinalIgnoreCase) > 0)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            if (!string.Equals(KEY_BUF[TABLE_INVOICES].Substring(0, 1), beginSleutel.Substring(0, 1), StringComparison.Ordinal))
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            Cursor = Cursors.WaitCursor;
            Enabled = false;

            bool merkOp = false;
            if (_plGrensVan == BOOKYEAR_FROMTO.Substring(0, 8)
                && _plGrensTot == BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8)
                && _plGrensVan.Substring(0, 4) == _plGrensTot.Substring(0, 4))
                merkOp = true;

            OpenReport(_lijstNaam);
            _pageCounter = 0;
            PrintTitel();

            // Per-document run state
            double dTOT = 0, drb = 0, dTnt = 0, dBTW = 0;
            bool   reedsMetBetalingen = false;
            string kopBuf  = string.Empty;
            string syMasker;

            // ── Main document loop ────────────────────────────────────────────
            while (true)
            {
                Application.DoEvents();
                RecordToVeld(TABLE_INVOICES);

                // ── KontroleVoorwaarden (inlined) ─────────────────────────────
                bool skip = false;

                if (CheckBoxExpiryDate.Checked)
                {
                    if (string.CompareOrdinal(VBibText(TABLE_INVOICES, "#v036 #"), rdtemp) > 0)
                        skip = true;
                }

                if (!skip && CheckBoxExclude1994.Checked)
                {
                    if (string.CompareOrdinal(VBibText(TABLE_INVOICES, "#v035 #").Substring(0, Math.Min(4, VBibText(TABLE_INVOICES, "#v035 #").Length)), "1994") < 0)
                        skip = true;
                }

                if (!skip && CheckBoxOnlyThisPeriod.Checked)
                {
                    string v035 = VBibText(TABLE_INVOICES, "#v035 #");
                    string v033 = VBibText(TABLE_INVOICES, "#v033 #");
                    if (merkOp && v035.Length >= 4 && v033.Length >= 6
                        && v035.Substring(0, 4) != v033.Substring(2, 4))
                        MessageBox.Show(
                            "Opgelet, noteer/controleer a.u.b.:\r\nDatum document: " + v035 +
                            " onlogisch voor document nummer " + v033,
                            string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    if (string.CompareOrdinal(v035, _plGrensVan) < 0
                        || string.CompareOrdinal(v035, _plGrensTot) > 0)
                        skip = true;
                }

                if (!skip && CheckBoxLedgerDetail.Checked && _flPartij == TABLE_SUPPLIERS)
                {
                    BGet(TABLE_JOURNAL, 1, VBibText(TABLE_INVOICES, "#v033 #"));
                    if (Ktrl != 0)
                    {
                        skip = true;
                    }
                    else
                    {
                        RecordToVeld(TABLE_JOURNAL);
                        string jv035 = VBibText(TABLE_JOURNAL, "#v035 #");
                        string iv035 = VBibText(TABLE_INVOICES, "#v035 #");
                        if (string.CompareOrdinal(jv035, _plGrensVan) < 0
                            || string.CompareOrdinal(iv035, _plGrensTot) > 0)
                        {
                            MessageBox.Show(
                                "dokumentdatum (" + iv035 + ") <> boekdatum journaal (" + jv035 + ")\r\n\r\n" +
                                "Wordt uit boekhoudcontrole geweerd.  Kontroleer eventueel manueel",
                                VBibText(TABLE_INVOICES, "#v033 #"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            skip = true;
                        }
                    }
                }

                if (!skip)
                {
                    dTOT = SafeVal(VBibText(TABLE_INVOICES, "#v249 #"));
                    if (XisEuroWisBEF) dTOT = Math.Round(dTOT * EURO);

                    syMasker = MASK_EUR;
                    string v033Chr2 = VBibText(TABLE_INVOICES, "#v033 #");
                    bool isCreditNote = v033Chr2.Length >= 2 && v033Chr2[1] == '1';
                    if (isCreditNote)
                    {
                        dTOT = -dTOT;
                        drb  = XisEuroWisBEF
                            ? Math.Round(-SafeVal(VBibText(TABLE_INVOICES, "#v037 #")) * EURO)
                            : -SafeVal(VBibText(TABLE_INVOICES, "#v037 #"));
                    }
                    else
                    {
                        drb = XisEuroWisBEF
                            ? Math.Round(SafeVal(VBibText(TABLE_INVOICES, "#v037 #")) * EURO)
                            : SafeVal(VBibText(TABLE_INVOICES, "#v037 #"));
                    }

                    _veldTXT[1] = VBibText(TABLE_INVOICES, "#v033 #");
                    _veldTXT[2] = DateText(VBibText(TABLE_INVOICES, "#v035 #"));
                    _veldTXT[3] = VBibText(TABLE_INVOICES, "#vs03 #");
                    _veldTXT[4] = Dec(dTOT / SafeVal(VBibText(TABLE_INVOICES, "#v040 #")), syMasker);
                    _veldTXT[5] = string.Empty;
                    _veldTXT[9] = DateText(VBibText(TABLE_INVOICES, "#v036 #"));

                    string v034 = VBibText(TABLE_INVOICES, "#v034 #");
                    if (v034.Trim() != kopBuf)
                    {
                        dTnt   = 0;
                        string partijKey = VSet(v034.Length >= 2 ? v034.Substring(1, Math.Min(12, v034.Length - 1)) : string.Empty, 12);
                        BGet(_tableIndex, 0, partijKey);
                        kopBuf = v034.Trim();
                        _veldTXT[0] = (v034.Length >= 2 ? v034.Substring(1, Math.Min(12, v034.Length - 1)) : string.Empty).TrimEnd();
                        if (Ktrl != 0)
                        {
                            _veldTXT[0] += " * niet meer aanwezig *";
                        }
                        else
                        {
                            RecordToVeld(_tableIndex);
                            string naam = (_veldTXT[0] + " " + VBibText(_tableIndex, "#A100 #").TrimEnd() + " " + VBibText(_tableIndex, "#A101 #").TrimEnd());
                            _veldTXT[0] = naam.Length > 27 ? naam.Substring(0, 27) : naam;
                        }
                        SnelHelpPrint(_veldTXT[0], BL_LOGGING);
                    }

                    BGet(TABLE_JOURNAL, 1, VBibText(TABLE_INVOICES, "#v033 #"));
                    if (Ktrl != 0 || !CheckBoxFinanceDetail.Checked)
                    {
                        // No journal for this document
                        if (CheckBoxFinanceDetail.Checked)
                            tds += VBibText(TABLE_INVOICES, "#v033 #") + "  ...  "
                                 + VBibText(_flPartij, "#A110 #") + " " + VBibText(_flPartij, "#A100 #") + "\r\n";

                        bool fullyPaid = Math.Abs(dTOT - drb) < 0.001;
                        if (!(fullyPaid && CheckBoxNotPaid.Checked))
                        {
                            bool isFirstOfPartij = _veldTXT[0].StartsWith(
                                v034.Length >= 2 ? v034.Substring(1) : string.Empty,
                                StringComparison.Ordinal);

                            if (isFirstOfPartij)
                            {
                                // Blank line before new party block
                                _ypos = Mim.Report.Print(1, _ypos, string.Empty);
                                CheckPageBreak();
                            }

                            dTnt += dTOT - drb;
                            _veldTXT[6]  = Dec(drb, MASK_2002);
                            _veldTXT[7]  = VBibText(TABLE_INVOICES, "#v038 #");
                            _veldTXT[8]  = string.Empty;
                            _veldTXT[10] = Dec(dTnt, MASK_2002);

                            _totaalGOED      += dTOT;
                            _totaalALBETAALD += drb;
                            PrintVelden();
                            _veldTXT[0] = string.Empty;
                        }
                    }
                    else
                    {
                        AfdrukDetailReedsBetaald(ref dTOT, ref drb, ref dTnt, ref dBTW,
                                                 ref reedsMetBetalingen, ref tds, kopBuf, v034);
                    }
                }

                BNext(TABLE_INVOICES);
                if (Ktrl != 0
                    || string.Compare(VSet(KEY_BUF[TABLE_INVOICES], 13), eindSleutel, StringComparison.OrdinalIgnoreCase) > 0)
                    break;
            }

            _totaalNOGTEBETALEN = _totaalGOED - _totaalALBETAALD;
            EindTotaal();

            Cursor  = Cursors.Default;
            Enabled = true;
            
            // ── Mededeling ontbrekende journalen ──────────────────────────────
            if (tds.Length > 24)
            {
                tds += "\r\nDe betalingen voor bovenvermelde dokumenten kunnen dus niet\r\n"
                     + "in detail weergegeven worden.  Enkel het algemeen totaal\r\n"
                     + "van het dokument en laatste financieel uittreksel...";
                MessageBox.Show(tds);
            }

            // ── Betalingsdetail voor/na boekjaar ─────────────────────────────
            int totalBVN = 0;
            foreach (var lst in _betalingenVoorNa) totalBVN += lst.Count;

            if (totalBVN != 0)
            {
                if (MessageBox.Show(
                    "Betalingsdetail dokumenten lagere/hogere boekjaren...\r\nOp papier (nieuw rapport)?",
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    string[] bhHeader =
                    {
                        "Detail betalingen dokumenten van lagere boekjaren in dit boekjaar\r\n\r\n",
                        "Detail betalingen dokumenten van hogere boekjaren in dit boekjaar\r\n\r\n",
                        "Detail betalingen dokumenten van dit boekjaar in lagere boekjaren\r\n\r\n",
                        "Detail betalingen dokumenten van dit boekjaar in hogere boekjaren\r\n\r\n"
                    };
                    double[] bhTotals =
                    {
                        _totaalDokOnderPeriode,
                        _totaalDokBovenPeriode,
                        _totaalVOOR,
                        _totaalNA
                    };

                    Mim.Report.PageBreak();
                    // OpenReport(_lijstNaam + " – betalingsdetail");
                    // _pageCounter = 0;

                    for (int tel = 0; tel < 4; tel++)
                    {
                        if (_betalingenVoorNa[tel].Count == 0) continue;
                        Mim.Report.SelectFont("Courier New", (int)7.2);
                        _ypos = Mim.Report.Print(1, 1, bhHeader[tel]);
                        foreach (string line in _betalingenVoorNa[tel])
                            _ypos = Mim.Report.Print(1, _ypos, line);
                        _ypos = Mim.Report.Print(1, _ypos, string.Empty);
                        _ypos = Mim.Report.Print(1, _ypos,
                            "Totaal financiële bewegingen: " + Dec(bhTotals[tel], MASK_2002));
                        Mim.Report.PageBreak();
                    }                    
                }
            }

            SnelHelpPrint("Klaar", BL_LOGGING);
            CloseReport(_lijstNaam);
            Close();
        }

        // ══════════════════════════════════════════════════════════════════════
        // AfdrukDetailReedsBetaald  (VB6 GoSub inlined as method)
        // ══════════════════════════════════════════════════════════════════════
        private void AfdrukDetailReedsBetaald(
            ref double dTOT, ref double drb, ref double dTnt, ref double dBTW,
            ref bool reedsMetBetalingen, ref string tds, string kopBuf, string v034)
        {
            // First compute the total-paid for this invoice (TotaalBETAALD sub)
            drb = ComputeTotaalBetaald();

            bool fullyPaid = Math.Abs(Math.Round(dTOT, 2) - Math.Round(drb, 2)) < 0.001;
            if (fullyPaid && CheckBoxNotPaid.Checked) return;

            // Re-open journal from first record for this invoice
            BGet(TABLE_JOURNAL, 1, VBibText(TABLE_INVOICES, "#v033 #"));

            dTnt += dTOT;
            reedsMetBetalingen = false;
            drb = 0;

            while (true)
            {
                RecordToVeld(TABLE_JOURNAL);
                string jv038 = VBibText(TABLE_JOURNAL, "#v038 #").Trim();
                if (jv038 != string.Empty)
                {
                    string jv019 = VBibText(TABLE_JOURNAL, "#v019 #");
                    if (jv019.Length > 0 && jv019[0] == '4')
                    {
                        reedsMetBetalingen = true;
                        BetalingErBij(ref drb, ref dTnt, ref dBTW, dTOT, kopBuf, v034);
                    }
                }
                BNext(TABLE_JOURNAL);
                if (Ktrl != 0
                    || KEY_BUF[TABLE_JOURNAL].Trim() != VBibText(TABLE_INVOICES, "#v033 #").Trim())
                    break;
            }

            if (!reedsMetBetalingen)
            {
                drb = 0;
                VBib(TABLE_JOURNAL, "0", "v068");
                BetalingErBij(ref drb, ref dTnt, ref dBTW, dTOT, kopBuf, v034);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // BetalingErBij  (VB6 GoSub inlined as method)
        // ══════════════════════════════════════════════════════════════════════
        private void BetalingErBij(
            ref double drb, ref double dTnt, ref double dBTW,
            double dTOT, string kopBuf, string v034)
        {
            double betaaldBedrag = SafeVal(VBibText(TABLE_JOURNAL, "#v068 #"));
            if (_flPartij == TABLE_CUSTOMERS) betaaldBedrag = -betaaldBedrag;

            if (CheckBoxExcludeOutOfPeriod.Checked)
            {
                string jv066 = VBibText(TABLE_JOURNAL, "#v066 #");
                if (string.CompareOrdinal(jv066, _plGrensVan) < 0)
                {
                    _totaalVOOR += betaaldBedrag;
                    _betalingenVoorNa[2].Add(BuildBetalingLine(betaaldBedrag));
                    betaaldBedrag = 0;
                }
                else if (string.CompareOrdinal(jv066, _plGrensTot) > 0)
                {
                    _totaalNA += betaaldBedrag;
                    _betalingenVoorNa[3].Add(BuildBetalingLine(betaaldBedrag));
                    betaaldBedrag = 0;
                }
            }

            // New-party separator line
            string invoicePart = v034.Length >= 2 ? v034.Substring(1) : string.Empty;
            bool isFirstOfPartij = kopBuf.StartsWith(invoicePart, StringComparison.Ordinal)
                                   || _veldTXT[0].StartsWith(invoicePart, StringComparison.Ordinal);
            if (_veldTXT[0].StartsWith(invoicePart, StringComparison.Ordinal))
            {
                dTnt = dTOT;
                drb  = 0;
                _ypos = Mim.Report.Print(1, _ypos, string.Empty);
                CheckPageBreak();
            }

            drb   += betaaldBedrag;
            dTnt  -= betaaldBedrag;
            _dTrb += drb;
            _dttot += dTOT - dBTW;
            _dtbtw += dBTW;

            _veldTXT[6]  = Dec(betaaldBedrag, MASK_2002);
            _veldTXT[7]  = VBibText(TABLE_JOURNAL, "#v038 #");
            _veldTXT[8]  = Dec(dTOT - drb, MASK_2002);
            _veldTXT[10] = Dec(dTnt, MASK_2002);

            if (SafeVal(_veldTXT[4]) + SafeVal(_veldTXT[5]) != 0)
            {
                _totaalGOED           += dTOT;
                _totaalNOGTEBETALEN   += dTOT;
            }
            _totaalALBETAALD += betaaldBedrag;
            PrintVelden();

            // Clear per-payment fields for next iteration
            _veldTXT[0] = _veldTXT[1] = _veldTXT[2] =
            _veldTXT[3] = _veldTXT[4] = _veldTXT[5] =
            _veldTXT[9] = string.Empty;
        }

        // ══════════════════════════════════════════════════════════════════════
        // ComputeTotaalBetaald  (VB6 TotaalBETAALD GoSub)
        // ══════════════════════════════════════════════════════════════════════
        private double ComputeTotaalBetaald()
        {
            double drb = 0;
            BGet(TABLE_JOURNAL, 1, VBibText(TABLE_INVOICES, "#v033 #"));
            if (Ktrl != 0)
            {
                MessageBox.Show("onlogische situatie");
                return drb;
            }

            while (true)
            {
                RecordToVeld(TABLE_JOURNAL);
                if (VBibText(TABLE_JOURNAL, "#v038 #").Trim() != string.Empty)
                {
                    string jv019 = VBibText(TABLE_JOURNAL, "#v019 #");
                    if (jv019.Length > 0 && jv019[0] == '4')
                    {
                        double betaaldBedrag = SafeVal(VBibText(TABLE_JOURNAL, "#v068 #"));
                        if (_flPartij == TABLE_CUSTOMERS) betaaldBedrag = -betaaldBedrag;

                        if (XisEuroWisBEF
                            && string.CompareOrdinal(VBibText(TABLE_JOURNAL, "#v066 #"),
                                                     BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8)) > 0)
                            betaaldBedrag = Math.Round(betaaldBedrag * EURO);

                        if (CheckBoxExcludeOutOfPeriod.Checked)
                        {
                            string jv066 = VBibText(TABLE_JOURNAL, "#v066 #");
                            if (string.CompareOrdinal(jv066, _plGrensVan) >= 0
                                && string.CompareOrdinal(jv066, _plGrensTot) <= 0)
                                drb += betaaldBedrag;
                        }
                        else
                        {
                            drb += betaaldBedrag;
                        }
                    }
                }

                BNext(TABLE_JOURNAL);
                if (Ktrl != 0
                    || KEY_BUF[TABLE_JOURNAL].Trim() != VBibText(TABLE_INVOICES, "#v033 #").Trim())
                    break;
            }
            return drb;
        }

        // ══════════════════════════════════════════════════════════════════════
        // EindTotaal
        // ══════════════════════════════════════════════════════════════════════
        private void EindTotaal()
        {
            for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
            _veldTXT[0]  = "Totalen :";
            _veldTXT[4]  = Dec(_totaalGOED,          MASK_2002);
            _veldTXT[5]  = string.Empty;
            _veldTXT[6]  = Dec(_totaalALBETAALD,     MASK_2002);
            _veldTXT[10] = Dec(_totaalNOGTEBETALEN,  MASK_2002);

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            PrintRegel();

            if (!CheckBoxLedgerDetail.Checked) return;

            // ── Subtotals before / after period ──────────────────────────────
            if (_totaalVOOR != 0)
            {
                for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
                _veldTXT[0] = "Dok. reeds betaald voor :";
                _veldTXT[2] = txtPeriode.Text.Length >= 10 ? txtPeriode.Text.Substring(0, 10) : txtPeriode.Text;
                _veldTXT[6] = Dec(_totaalVOOR, MASK_2002);
                PrintRegel();
            }
            if (_totaalNA != 0)
            {
                for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
                _veldTXT[0] = "Dok. reeds betaald na :";
                _veldTXT[2] = txtPeriode.Text.Length >= 10 ? txtPeriode.Text.Substring(txtPeriode.Text.Length - 10) : txtPeriode.Text;
                _veldTXT[6] = Dec(_totaalNA, MASK_2002);
                PrintRegel();
            }

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);

            // ── Groep rekeningen analyse (betalingsjournaal) ──────────────────
            double totaalBedragGroep    = 0;
            int    aantalInGroep        = 0;
            double bedragZonderdokument = 0;
            int    aantalZonderdokument = 0;

            _aantalBovenPeriode    = 0;
            _aantalOnderPeriode    = 0;
            _totaalDokBovenPeriode = 0;
            _totaalDokOnderPeriode = 0;
            _betalingenVoorNa[0].Clear();
            _betalingenVoorNa[1].Clear();

            string groepSelektie = String99(_flPartij + 296);
            if (groepSelektie.Trim() == string.Empty)
                groepSelektie = String99(_flPartij + 8).Substring(0, Math.Min(4, String99(_flPartij + 8).Length)) + "999";
            if (groepSelektie.Length > 7)
            {
                MessageBox.Show("Groep bestaat uit meer dan 7 tekens: " + groepSelektie,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                groepSelektie = groepSelektie.Substring(0, 7);
            }
            else while (groepSelektie.Length < 7)
                groepSelektie += "9";

            BGetOrGreater(TABLE_LEDGERACCOUNTS, 0, VSet(String99(_flPartij + 8), 7));
            if (Ktrl != 0)
            {
                MessageBox.Show("onlogika");
                return;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            while (string.CompareOrdinal(KEY_BUF[TABLE_LEDGERACCOUNTS], groepSelektie) <= 0)
            {
                string groepRekening4 = KEY_BUF[TABLE_LEDGERACCOUNTS];
                SnelHelpPrint("Journalen boekjaar voor rek. " + KEY_BUF[TABLE_LEDGERACCOUNTS] + " worden gekontroleerd.  Ogenblik a.u.b.", BL_LOGGING);
                aantalInGroep++;

                string saldoField = bhEuro
                    ? "#e" + (22 + ACTIVE_BOOKYEAR).ToString("000") + " #"
                    : "#v" + (22 + ACTIVE_BOOKYEAR).ToString("000") + " #";
                totaalBedragGroep += SafeVal(VBibText(TABLE_LEDGERACCOUNTS, saldoField));

                BGetOrGreater(TABLE_JOURNAL, 0, VSet(KEY_BUF[TABLE_LEDGERACCOUNTS], 7) + _plGrensVan);
                if (Ktrl != 0)
                {
                    MessageBox.Show("Geen journalen voor deze periode...");
                }
                else
                {
                    RecordToVeld(TABLE_JOURNAL);
                    if (string.CompareOrdinal(KEY_BUF[TABLE_JOURNAL].Substring(0, Math.Min(7, KEY_BUF[TABLE_JOURNAL].Length)), groepSelektie) > 0)
                        break;

                    while (KEY_BUF[TABLE_JOURNAL].Length >= 15
                           && string.CompareOrdinal(KEY_BUF[TABLE_JOURNAL].Substring(KEY_BUF[TABLE_JOURNAL].Length - 8), _plGrensTot) <= 0)
                    {
                        SnelHelpPrint("Alle journalen voor rekening " + KEY_BUF[TABLE_LEDGERACCOUNTS]
                            + " worden gekontroleerd.  Bezig aan :" + KEY_BUF[TABLE_JOURNAL].Substring(KEY_BUF[TABLE_JOURNAL].Length - 8), BL_LOGGING);
                        Application.DoEvents();

                        string jv033 = VBibText(TABLE_JOURNAL, "#v033 #").Trim();
                        if (jv033 == string.Empty || (jv033.Length > 0 && jv033[0] == 'D'))
                        {
                            aantalZonderdokument++;
                            bedragZonderdokument += SafeVal(VBibText(TABLE_JOURNAL, "#v068 #"));
                        }
                        else if (VBibText(TABLE_JOURNAL, "#v038 #").TrimEnd() != string.Empty)
                        {
                            BGet(TABLE_INVOICES, 0, VBibText(TABLE_JOURNAL, "#v033 #"));
                            if (Ktrl == 0)
                            {
                                RecordToVeld(TABLE_INVOICES);
                                string iv035 = VBibText(TABLE_INVOICES, "#v035 #");
                                if (string.CompareOrdinal(iv035, _plGrensVan) < 0)
                                {
                                    double bb = SafeVal(VBibText(TABLE_JOURNAL, "#v068 #"));
                                    if (_flPartij == TABLE_CUSTOMERS) bb = -bb;
                                    if (XisEuroWisBEF && string.CompareOrdinal(VBibText(TABLE_JOURNAL, "#v066 #"), BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8)) > 0)
                                        bb = Math.Round(bb * EURO);
                                    _totaalDokOnderPeriode += bb;
                                    _aantalOnderPeriode++;
                                    _betalingenVoorNa[0].Add(BuildBetalingLineFromInvoice(bb));
                                }
                                else if (string.CompareOrdinal(iv035, _plGrensTot) > 0)
                                {
                                    double bb = SafeVal(VBibText(TABLE_JOURNAL, "#v068 #"));
                                    if (_flPartij == TABLE_CUSTOMERS) bb = -bb;
                                    if (XisEuroWisBEF && string.CompareOrdinal(VBibText(TABLE_JOURNAL, "#v066 #"), BOOKYEAR_FROMTO.Substring(BOOKYEAR_FROMTO.Length - 8)) > 0)
                                        bb = Math.Round(bb * EURO);
                                    _totaalDokBovenPeriode += bb;
                                    _aantalBovenPeriode++;
                                    _betalingenVoorNa[1].Add(BuildBetalingLineFromInvoice(bb));
                                }
                            }
                        }

                        BNext(TABLE_JOURNAL);
                        if (Ktrl != 0
                            || string.CompareOrdinal(KEY_BUF[TABLE_JOURNAL], groepRekening4 + _plGrensTot) > 0)
                            break;
                        RecordToVeld(TABLE_JOURNAL);
                    }
                }

                BNext(TABLE_LEDGERACCOUNTS);
                if (Ktrl != 0 || string.CompareOrdinal(KEY_BUF[TABLE_LEDGERACCOUNTS], groepSelektie) > 0) break;
                RecordToVeld(TABLE_LEDGERACCOUNTS);
            }

            // Print group summary lines
            for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
            _veldTXT[0] = "Stand " + aantalInGroep.ToString("00") + " " + groepSelektie + "-rekeningen";
            _veldTXT[6] = Dec(totaalBedragGroep, MASK_2002);
            PrintRegel();

            for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
            _veldTXT[0] = aantalZonderdokument.ToString("00") + " verr. zonder dokument";
            _veldTXT[6] = Dec(bedragZonderdokument, MASK_2002);
            PrintRegel();

            for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
            _veldTXT[0] = _aantalOnderPeriode.ToString() + " betalingen dok. < boekjaar";
            _veldTXT[6] = Dec(_totaalDokOnderPeriode, MASK_2002);
            PrintRegel();

            for (int t = 0; t <= 10; t++) _veldTXT[t] = string.Empty;
            _veldTXT[0] = _aantalBovenPeriode.ToString() + " betalingen dok. > boekjaar";
            _veldTXT[6] = Dec(_totaalDokBovenPeriode, MASK_2002);
            PrintRegel();
        }

        // ══════════════════════════════════════════════════════════════════════
        // Report helpers
        // ══════════════════════════════════════════════════════════════════════
        private void InitVelden()
        {
            _reportField[0]  = "Identiteit";    _reportTab[0]  = 1;
            _reportField[1]  = "Document";      _reportTab[1]  = 29;
            _reportField[2]  = "Datum";         _reportTab[2]  = 41;
            _reportField[3]  = "Mdoc";          _reportTab[3]  = 52;

            _reportField[4]  = XisEuroWisBEF ? " Goed(BEF)" : (bhEuro ? " Goed(EUR)" : " Goed(BEF)");
            _reportTab[4]    = 56;

            _reportField[5]  = XisEuroWisBEF ? "  BTW(BEF)" : (bhEuro ? "  BTW(EUR)" : "  BTW(BEF)");
            _reportTab[5]    = 67;

            _reportField[6]  = "   Betaald";    _reportTab[6]  = 78;
            _reportField[7]  = "Fin.stuk";      _reportTab[7]  = 89;
            _reportField[8]  = "      Rest";    _reportTab[8]  = 98;
            _reportField[9]  = "Vervaldag";     _reportTab[9]  = 109;

            _reportField[10] = XisEuroWisBEF ? " Cum.(BEF)" : (bhEuro ? " Cum.(EUR)" : " Cum.(BEF)");
            _reportTab[10]   = 119;

            _reportTab[11]   = 0;   // sentinel
        }

        private void OpenReport(string title)
        {
            if (Mim.Report.IsOpen()) Mim.Report.CloseDoc();
            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = IDEALSoftware.VpeCommunity.GUILanguage.Dutch;
            Mim.Report.Title       = title;
            _pageCounter = 0;
        }

        private void CloseReport(string mailSubject)
        {
            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.MailSubject = mailSubject;
            Mim.Report.MailText    = mailSubject + " in bijlage.";
            Mim.Report.Preview();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PrintTitel()
        {
            Mim.Report.SelectFont("Courier New", (int)7.2);
            Mim.Report.TextBold      = true;
            Mim.Report.TextColor     = System.Drawing.ColorTranslator.FromOle(0);
            Mim.Report.nTopMargin    = 1;
            Mim.Report.nBottomMargin = 29;
            Mim.Report.nLeftMargin   = 0.5;
            Mim.Report.nRightMargin  = 0.5;
            Mim.Report.PenSize       = 0.01;

            _pageCounter++;
            _ypos = Mim.Report.Print(1,  1,     _psTekst[2]);
            _ypos = Mim.Report.Print(17, 1,     "Pagina : " + Dec(_pageCounter, "##########"));
            _ypos = Mim.Report.Print(17, _ypos, "Datum  : " + _psTekst[0]);
            _ypos = Mim.Report.Print(1,  _ypos, _psTekst[3].ToUpper());
            _ypos = Mim.Report.Print(1,  _ypos, _fullLine);

            // Column headers
            string header = BuildTabLine(_reportField, _reportTab);
            _ypos = Mim.Report.Print(1, _ypos, header);
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
        }

        private void CheckPageBreak()
        {
            if (_ypos > 27.5)
            {
                Mim.Report.PageBreak();
                PrintTitel();
            }
        }

        private void PrintVelden()
        {
            string line = BuildTabLine(_veldTXT, _reportTab);
            _ypos = Mim.Report.Print(1, _ypos, line);
            CheckPageBreak();
        }

        private void PrintRegel()
        {
            string line = BuildTabLine(_veldTXT, _reportTab);
            _ypos = Mim.Report.Print(1, _ypos, line);
            CheckPageBreak();
        }

        // ── Build a fixed-width line by placing values at their tab positions ─
        private static string BuildTabLine(string[] fields, int[] tabs)
        {
            string line = new string(' ', 128);
            for (int t = 0; t < fields.Length; t++)
            {
                if (t >= tabs.Length || tabs[t] == 0 && t > 0) break;
                int pos = tabs[t] > 0 ? tabs[t] - 1 : 0;   // VB6 Tab() is 1-based
                if (pos >= line.Length) break;
                string val = fields[t] ?? string.Empty;
                int avail = line.Length - pos;
                if (val.Length > avail) val = val.Substring(0, avail);
                line = line.Substring(0, pos) + val + line.Substring(Math.Min(line.Length, pos + val.Length));
            }
            return line.TrimEnd();
        }

        // ── Helpers ────────────────────────────────────────────────────────────
        private static double SafeVal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            double.TryParse(s.Trim(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double result);
            return result;
        }

        private string BuildBetalingLine(double betaaldBedrag)
        {
            return VSet(VBibText(TABLE_INVOICES, "#v034 #").Length >= 2
                        ? VBibText(TABLE_INVOICES, "#v034 #").Substring(1) : string.Empty, 12) + " "
                 + VSet(VBibText(TABLE_JOURNAL, "#v067 #"), 20) + " "
                 + DateText(VBibText(TABLE_INVOICES, "#v035 #")) + " "
                 + VBibText(TABLE_INVOICES, "#v033 #") + " "
                 + Dec(betaaldBedrag, MASK_2002) + " "
                 + VSet(VBibText(TABLE_JOURNAL, "#v019 #"), 7) + " "
                 + DateText(VBibText(TABLE_JOURNAL, "#v066 #")) + " "
                 + VBibText(TABLE_JOURNAL, "#v038 #") + " "
                 + VBibText(TABLE_JOURNAL, "#v069 #");
        }

        private string BuildBetalingLineFromInvoice(double bedrag)
        {
            return VSet(VBibText(TABLE_INVOICES, "#v034 #").Length >= 2
                        ? VBibText(TABLE_INVOICES, "#v034 #").Substring(1) : string.Empty, 12) + " "
                 + VSet(VBibText(TABLE_JOURNAL, "#v067 #"), 20) + " "
                 + DateText(VBibText(TABLE_INVOICES, "#v035 #")) + " "
                 + VBibText(TABLE_INVOICES, "#v033 #") + " "
                 + Dec(bedrag, MASK_2002) + " "
                 + VSet(VBibText(TABLE_JOURNAL, "#v019 #"), 7) + " "
                 + DateText(VBibText(TABLE_JOURNAL, "#v066 #")) + " "
                 + VBibText(TABLE_JOURNAL, "#v038 #") + " "
                 + VBibText(TABLE_JOURNAL, "#v069 #");
        }
    }
}
