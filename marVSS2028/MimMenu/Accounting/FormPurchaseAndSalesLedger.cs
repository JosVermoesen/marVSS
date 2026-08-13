using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.MdvDataTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormPurchaseAndSalesLedger : Form
    {
        // ── Mode set by caller ───────────────────────────────────────────────
        // _tableIndex must be set before calling Show()
        public int _tableIndex;          // TABLE_SUPPLIERS or TABLE_CUSTOMERS
        private string _ledgerName = "";  // "Aankoopboek" / "Verkoopboek"

        // ── Period ───────────────────────────────────────────────────────────
        private string _beginPeriod = "";
        private string _endPeriod = "";
        private string _typeVATPeriod = "";

        private string _jaar;
        private string _periode;

        // ── Column layout ────────────────────────────────────────────────────
        private const int MAX_VELD = 18;
        private string[] _reportField = new string[MAX_VELD];
        private int[] _reportTab = new int[MAX_VELD];
        private int[] _rapportManier = new int[MAX_VELD];  // 0=raw, 5=date, 9=amount
        private int[] _rapportVeldNr = new int[MAX_VELD];
        private double[] _kolomTotaal = new double[MAX_VELD];
        private int _tMaxVeld;
        private int _ar;  // 1/3 for suppliers, 12/14 for customers

        // ── Report helpers ───────────────────────────────────────────────────
        private readonly string _fullLine = new string('-', 128);
        private string _reportTitle = "";
        private string _reportTitle2 = "";
        private string _reportHeader = "";
        private string _reportDate = "";
        private double _ypos;
        private int _pageCounter;

        // ── Data ─────────────────────────────────────────────────────────────
        private DataTable _docsTable = new DataTable();
        private DataTable _journalTable = new DataTable();

        // ── Square-check accumulator ─────────────────────────────────────────
        // Key = account number (v019, padded), Value = (count v013, amount v068)
        private readonly SortedDictionary<string, (double count, double amount)> _cumulData =
            new SortedDictionary<string, (double count, double amount)>(StringComparer.Ordinal);

        public FormPurchaseAndSalesLedger()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Load
        // ═══════════════════════════════════════════════════════════════════════
        private void FormPurchaseAndSalesLedger_Load(object sender, EventArgs e)
        {
            switch (_tableIndex)
            {
                case TABLE_SUPPLIERS:
                    _ledgerName = "Aankoopboek";
                    break;
                case TABLE_CUSTOMERS:
                    _ledgerName = "Verkoopboek";
                    break;
                default:
                    MessageBox.Show("Ongeldige tabelindex.");
                    Close();
                    return;
            }

            Text = _ledgerName;
            ProcessingDate.Value = DateTime.Today;
            _typeVATPeriod = String99(301);

            string year = PERIOD_FROMTO.Substring(0, 4);
            if (!int.TryParse(year, out int yearNum) || yearNum <= 2025)
            {
                MessageBox.Show(
                    "Werkelijk jaar moet minstens 2026 zijn. " +
                    "2025 en lager zijn uitsluitend te bewerken met de '1995 - 2025' versies van de software.");
                
                Close();
                return;
            }

            switch (_typeVATPeriod)
            {
                case "1":
                    SubTitleTextBox.Text = "Maandelijkse aangifte " +
                        PERIOD_FROMTO.Substring(4, 2) + "/" + year;
                    _beginPeriod = PERIOD_FROMTO.Substring(0, 8);
                    _endPeriod = PERIOD_FROMTO.Substring(8);
                    break;

                case "2":
                case "0":
                    string endMonth = PERIOD_FROMTO.Substring(12, 2);            
                    switch (endMonth)
                    {
                        case "03":
                            _beginPeriod = year + "0101";
                            _endPeriod = PERIOD_FROMTO.Substring(8);
                            SubTitleTextBox.Text = "Kwartaal aangifte 03/" + year;
                            break;
                        case "06":
                            _beginPeriod = year + "0401";
                            _endPeriod = PERIOD_FROMTO.Substring(8);
                            SubTitleTextBox.Text = "Kwartaal aangifte 06/" + year;
                            break;
                        case "09":
                            _beginPeriod = year + "0701";
                            _endPeriod = PERIOD_FROMTO.Substring(8);
                            SubTitleTextBox.Text = "Kwartaal aangifte 09/" + year;
                            break;
                        case "12":
                            _beginPeriod = year + "1001";
                            _endPeriod = PERIOD_FROMTO.Substring(8);
                            SubTitleTextBox.Text = "Kwartaal aangifte 12/" + year;
                            break;
                        default:
                            MessageBox.Show("Selecteer een geldige maand voor kwartaalaangifte",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Close();
                            return;
                    }
                    if (_ledgerName == "Verkoopboek" && _typeVATPeriod == "0")
                    {
                        MessageBox.Show("Volgens Setup BTW geen aangifteplicht. Controleer eventueel.",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                        return;
                    }
                    break;
            }

            DateFromLabel.Text = DateText(_beginPeriod);
            DateToLabel.Text = DateText(_endPeriod);

            UpdateInvoiceCreditnoteState();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Invoice / Creditnote radio buttons
        // ═══════════════════════════════════════════════════════════════════════
        private void RadioFacturen_CheckedChanged(object sender, EventArgs e) => UpdateInvoiceCreditnoteState();
        private void RadioCreditnotas_CheckedChanged(object sender, EventArgs e) => UpdateInvoiceCreditnoteState();
        private void UpdateInvoiceCreditnoteState()
        {
            bool isFactuur = RadioFacturen.Checked;
            int t = _tableIndex == TABLE_SUPPLIERS ? 0 : 10;
            int offset = isFactuur ? 0 : 2;

            int.TryParse(String99(1 + t + offset), out int totNum);
            int.TryParse(String99(2 + t + offset), out int vanNum);

            DocToLabel.Text   = totNum.ToString("D5");
            DocFromLabel.Text = (vanNum < totNum ? vanNum + 1 : vanNum).ToString("D5");
            ButtonGenerateReport.Enabled = vanNum < totNum;

            _ar = _tableIndex == TABLE_SUPPLIERS ? (isFactuur ? 1 : 3) : (isFactuur ? 12 : 14);

            // Check if documents for this period are already included in a VAT declaration
            _jaar    = BOOKYEAR_FROMTO.Substring(0, 4);
            _periode = GetByperdatPeriodNumber().ToString("D2");

            BGet(TABLE_VARIOUS, 1, VSet("17" + _jaar + _periode, 20));
            if (Ktrl != 0) return;

            RecordToVeld(TABLE_VARIOUS);

            // Resolve the two VAT-declaration field codes for this table+type combination
            string vFrom = _tableIndex == TABLE_SUPPLIERS
                ? (isFactuur ? "#v092 #" : "#v094 #")
                : (isFactuur ? "#v096 #" : "#v098 #");
            string vTo   = _tableIndex == TABLE_SUPPLIERS
                ? (isFactuur ? "#v093 #" : "#v095 #")
                : (isFactuur ? "#v097 #" : "#v099 #");

            double getal = ToDouble(VBibText(TABLE_VARIOUS, vFrom))
                         + ToDouble(VBibText(TABLE_VARIOUS, vTo));

            if (getal == 0) return;

            string docFrom = VBibText(TABLE_VARIOUS, vFrom);
            string docTo   = VBibText(TABLE_VARIOUS, vTo);

            MessageBox.Show(
                "Binnen deze periode zijn er reeds dokumenten opgenomen:" + Environment.NewLine +
                "Van: " + docFrom + "  Tot: " + docTo,
                "BTW aangifte kontroleren a.u.b. !",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            DocFromLabel.Text            = docFrom;
            DocToLabel.Text              = docTo;
            ButtonGenerateReport.Enabled = false;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Buttons
        // ═══════════════════════════════════════════════════════════════════════
        private void ButtonClose_Click(object sender, EventArgs e) => Close();

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
                // After report generation, the form remains open to allow the user to
                // add the totals to the VAT declaration for the periode, so we do not close it automatically.
                // We ask first to confirm if they want to close it, without saving the totals to the declaration.

                DialogResult result = MessageBox.Show(                    
                    "Wilt u de totalen opnemen in de BTW-aangifte voor deze periode?\n" + 
                    "(vergeet ook eventuele creditnota's niet)", 
                    "Rapport gegenereerd", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveTotalsToVatDeclaration();
                    // Do not close the form, so they can eventually do the same for credit notes if needed, and then close it manually when done
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Save report totals into the TABLE_VARIOUS VAT declaration record
        // ═══════════════════════════════════════════════════════════════════════
        private void SaveTotalsToVatDeclaration()
        {
            // Derive the VAT period key the same way as UpdateInvoiceCreditnoteState
            string jaar    =  BOOKYEAR_FROMTO.Substring(0, 4);
            string periode = GetByperdatPeriodNumber().ToString("D2");
            string periodeSleutel = jaar + periode;

            BGet(TABLE_VARIOUS, 1, VSet("17" + periodeSleutel, 20));
            if (Ktrl != 0)
            {
                // If there is no record for this period yet, we create a new one
                TLB_RECORD[TABLE_VARIOUS] = "";
                VBib(TABLE_VARIOUS, jaar, "v090");
                VBib(TABLE_VARIOUS, periode, "v091");
                VBib(TABLE_VARIOUS, "17" + VBibText(TABLE_VARIOUS, "#v090 #") + VBibText(TABLE_VARIOUS, "#v091 #"), "v005");
                BInsert(TABLE_VARIOUS, 1);
                BGet(TABLE_VARIOUS, 1, VSet("17" + periodeSleutel, 20));
            }
            // Now we have the record for the period, we update the relevant fields with the totals from the report
            RecordToVeld(TABLE_VARIOUS);
                        
            if (PERIOD_FROMTO.Substring(4, 2) == PERIOD_FROMTO.Substring(12, 2))
            {
                // Maandelijkse periode
                VBib(TABLE_VARIOUS, PERIOD_FROMTO.Substring(4, 2), "i001");  // werkelijke maand
                VBib(TABLE_VARIOUS, PERIOD_FROMTO.Substring(0, 4), "i002");  // werkelijk jaar
            }
            else
            {
                MessageBox.Show(
                    "marIntegraal boekhoudperiodes staan nog altijd op 3-maandelijks. " +
                    "Geen Intervat aangifte mogelijk met deze werkwijze die dateert van 1985-1994 " +
                    "en vermoedelijk overgenomen werd uit marIntegraal DOS periode. " +
                    "Contacteer ons 0475/292255 voor manuele tussenkomst!!)",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            bool isFactuur = RadioFacturen.Checked;
            string docVan = DocFromLabel.Text;
            string docTot = DocToLabel.Text;

            if (_tableIndex == TABLE_SUPPLIERS)
            {
                if (_ar == 1)  // suppliers invoices
                {
                    VBib(TABLE_VARIOUS, _kolomTotaal[16].ToString(), "v045");  // vak 59
                    VBib(TABLE_VARIOUS, _kolomTotaal[9].ToString(),  "v052");  // vak 86
                    VBib(TABLE_VARIOUS, _kolomTotaal[11].ToString(), "v053");  // vak 87
                    VBib(TABLE_VARIOUS, _kolomTotaal[12].ToString(), "v054");  // vak 88

                    VBib(TABLE_VARIOUS, docVan, "v092");
                    VBib(TABLE_VARIOUS, docTot, "v093");
                }
                else if (_ar == 3)  // suppliers creditnotes
                {
                    VBib(TABLE_VARIOUS, _kolomTotaal[16].ToString(), "v100");  // vak 63
                    VBib(TABLE_VARIOUS, _kolomTotaal[7].ToString(),  "v050");  // vak 84
                    VBib(TABLE_VARIOUS, _kolomTotaal[8].ToString(),  "v051");  // vak 85

                    VBib(TABLE_VARIOUS, docVan, "v094");
                    VBib(TABLE_VARIOUS, docTot, "v095");

                    // Negate creditnote totals (VB6: For Tel = 3 To 16: KolomTotaal(Tel) = -KolomTotaal(Tel))
                    for (int tel = 3; tel <= 16; tel++)
                        _kolomTotaal[tel] = -_kolomTotaal[tel];
                }
                else
                {
                    MessageBox.Show("Stop", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Common suppliers fields (accumulated, add to existing)
                VBib(TABLE_VARIOUS, (_kolomTotaal[13] + ToDouble(VBibText(TABLE_VARIOUS, "#v042 #"))).ToString(), "v042");  // vak 55
                VBib(TABLE_VARIOUS, (_kolomTotaal[14] + ToDouble(VBibText(TABLE_VARIOUS, "#v043 #"))).ToString(), "v043");  // vak 56
                VBib(TABLE_VARIOUS, (_kolomTotaal[15] + ToDouble(VBibText(TABLE_VARIOUS, "#v044 #"))).ToString(), "v044");  // vak 57
                VBib(TABLE_VARIOUS, (_kolomTotaal[3]  + ToDouble(VBibText(TABLE_VARIOUS, "#v046 #"))).ToString(), "v046");  // vak 81
                VBib(TABLE_VARIOUS, (_kolomTotaal[4]  + ToDouble(VBibText(TABLE_VARIOUS, "#v047 #"))).ToString(), "v047");  // vak 82
                VBib(TABLE_VARIOUS, (_kolomTotaal[5]  + ToDouble(VBibText(TABLE_VARIOUS, "#v048 #"))).ToString(), "v048");  // vak 83
            }
            else if (_tableIndex == TABLE_CUSTOMERS)
            {
                if (_ar == 12)  // customers invoices
                {
                    VBib(TABLE_VARIOUS, _kolomTotaal[12].ToString(), "v064");  // vak 54
                    VBib(TABLE_VARIOUS, (_kolomTotaal[2] + ToDouble(VBibText(TABLE_VARIOUS, "#v055 #"))).ToString(), "v055");  // vak 00 bijtellen

                    VBib(TABLE_VARIOUS, _kolomTotaal[3].ToString(),  "v056");  // vak 01
                    VBib(TABLE_VARIOUS, _kolomTotaal[4].ToString(),  "v057");  // vak 02
                    VBib(TABLE_VARIOUS, _kolomTotaal[5].ToString(),  "v058");  // vak 03
                    VBib(TABLE_VARIOUS, _kolomTotaal[6].ToString(),  "v059");  // vak 45
                    VBib(TABLE_VARIOUS, _kolomTotaal[7].ToString(),  "v060");  // vak 46
                    VBib(TABLE_VARIOUS, _kolomTotaal[8].ToString(),  "v061");  // vak 47

                    VBib(TABLE_VARIOUS, docVan, "v096");
                    VBib(TABLE_VARIOUS, docTot, "v097");
                }
                else if (_ar == 14)  // customers creditnotes
                {
                    VBib(TABLE_VARIOUS, _kolomTotaal[12].ToString(), "v101");  // vak 64
                    VBib(TABLE_VARIOUS, (ToDouble(VBibText(TABLE_VARIOUS, "#v055 #")) - _kolomTotaal[2]).ToString(), "v055");  // vak 00 aftrekken

                    VBib(TABLE_VARIOUS, _kolomTotaal[10].ToString(), "v062");  // vak 48
                    VBib(TABLE_VARIOUS, _kolomTotaal[11].ToString(), "v063");  // vak 49

                    VBib(TABLE_VARIOUS, docVan, "v098");
                    VBib(TABLE_VARIOUS, docTot, "v099");
                }
                else
                {
                    MessageBox.Show("Stop", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Stop", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (bhEuro)
                VBib(TABLE_VARIOUS, "EUR", "vEUR");

            BUpdate(TABLE_VARIOUS, 1);

            // Persist the "tot" doc number into the setup counter (SS99 slot)
            int ss99Slot = _tableIndex == TABLE_SUPPLIERS
                ? (isFactuur ? 2 : 4)
                : (isFactuur ? 12 : 14);
            SS99(docTot, ss99Slot);

            if (isFactuur)
                RadioCreditnotas.Checked = true;            
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Data retrieval
        // ═══════════════════════════════════════════════════════════════════════
        private bool LoadData()
        {
            bool isFactuur = RadioFacturen.Checked;
            string prefix = _tableIndex == TABLE_SUPPLIERS ? "A" : "V";
            string typeChar = isFactuur ? "0" : "1";
            string docPrefix = prefix + typeChar;

            string sqlDocs =
                "SELECT * FROM Dokumenten " +
                "WHERE Mid(v033,1,2) = '" + docPrefix + "' " +
                "AND v035 >= '" + _beginPeriod + "' " +
                "AND v035 <= '" + _endPeriod + "' " +
                "ORDER BY v033 ASC";

            _docsTable = new DataTable();
            using (var conn = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sqlDocs, conn))
                adapter.Fill(_docsTable);

            if (_docsTable.Rows.Count == 0)
            {
                MessageBox.Show("Er zijn geen dokumenten", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // Update Van/Tot from actual data
            string firstDoc = _docsTable.Rows[0]["v033"].ToString();
            string lastDoc = _docsTable.Rows[_docsTable.Rows.Count - 1]["v033"].ToString();
            DocFromLabel.Text = firstDoc.Length >= 7 ? firstDoc.Substring(6) : firstDoc;
            DocToLabel.Text = lastDoc.Length >= 7 ? lastDoc.Substring(6) : lastDoc;

            string sqlJour =
                "SELECT * FROM Journalen " +
                "WHERE v033 >= '" + firstDoc + "' " +
                "AND v035 >= '" + _beginPeriod + "' " +
                "AND v035 <= '" + _endPeriod + "' " +
                "AND v038 IS NULL " +
                "ORDER BY v033 ASC, v019 ASC";

            _journalTable = new DataTable();
            using (var conn = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sqlJour, conn))
                adapter.Fill(_journalTable);

            return true;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Column layout
        // ═══════════════════════════════════════════════════════════════════════
        private void InitVelden()
        {
            for (int t = 0; t < MAX_VELD; t++)
                _kolomTotaal[t] = 0;

            // Common first two fields
            _rapportVeldNr[0] = 33; _rapportManier[0] = 0;
            _reportField[0] = "Document"; _reportTab[0] = 2;

            _rapportVeldNr[1] = 35; _rapportManier[1] = 5;
            _reportField[1] = "Datum doc."; _reportTab[1] = 14;

            if (_tableIndex == TABLE_SUPPLIERS)
            {
                _rapportVeldNr[2] = 39; _rapportManier[2] = 0;
                _reportField[2] = "Referte"; _reportTab[2] = 25;

                _rapportVeldNr[3] = 46; _rapportManier[3] = 9;
                _reportField[3] = "   VAK 81"; _reportTab[3] = 46;

                _rapportVeldNr[4] = 47; _rapportManier[4] = 9;
                _reportField[4] = "   VAK 82"; _reportTab[4] = 56;

                _rapportVeldNr[5] = 48; _rapportManier[5] = 9;
                _reportField[5] = "   VAK 83"; _reportTab[5] = 66;

                _rapportVeldNr[6] = 49; _rapportManier[6] = 9;
                _reportField[6] = "   DERDEN"; _reportTab[6] = 76;

                _rapportVeldNr[7] = 50; _rapportManier[7] = 9;
                _reportField[7] = "   VAK 84"; _reportTab[7] = 86;

                _rapportVeldNr[8] = 51; _rapportManier[8] = 9;
                _reportField[8] = "   VAK 85"; _reportTab[8] = 96;

                _rapportVeldNr[9] = 52; _rapportManier[9] = 9;
                _reportField[9] = "   VAK 86"; _reportTab[9] = 106;

                _rapportVeldNr[10] = 99; _rapportManier[10] = 1;
                _reportField[10] = "ID.Code/Naam"; _reportTab[10] = 2;

                _rapportVeldNr[11] = 53; _rapportManier[11] = 9;
                _reportField[11] = "   VAK 87"; _reportTab[11] = 56;

                _rapportVeldNr[12] = 54; _rapportManier[12] = 9;
                _reportField[12] = "   VAK 88"; _reportTab[12] = 66;

                _rapportVeldNr[13] = 42; _rapportManier[13] = 9;
                _reportField[13] = "   VAK 55"; _reportTab[13] = 76;

                _rapportVeldNr[14] = 43; _rapportManier[14] = 9;
                _reportField[14] = "   VAK 56"; _reportTab[14] = 86;

                _rapportVeldNr[15] = 44; _rapportManier[15] = 9;
                _reportField[15] = "   VAK 57"; _reportTab[15] = 96;

                _rapportVeldNr[16] = 45; _rapportManier[16] = 9;
                _reportField[16] = _ar == 1 ? "   VAK 59" : "   VAK 63";
                _reportTab[16] = 106;

                _reportTab[17] = 0;
                _tMaxVeld = 16;
            }
            else  // TABLE_CUSTOMERS
            {
                _rapportVeldNr[2] = 55; _rapportManier[2] = 9;
                _reportField[2] = "VAK 00"; _reportTab[2] = 44;

                _rapportVeldNr[3] = 56; _rapportManier[3] = 9;
                _reportField[3] = "VAK 01"; _reportTab[3] = 55;

                _rapportVeldNr[4] = 57; _rapportManier[4] = 9;
                _reportField[4] = "VAK 02"; _reportTab[4] = 66;

                _rapportVeldNr[5] = 58; _rapportManier[5] = 9;
                _reportField[5] = "VAK 03"; _reportTab[5] = 77;

                _rapportVeldNr[6] = 59; _rapportManier[6] = 9;
                _reportField[6] = "VAK 45"; _reportTab[6] = 88;

                _rapportVeldNr[7] = 60; _rapportManier[7] = 9;
                _reportField[7] = "VAK 46"; _reportTab[7] = 99;

                _rapportVeldNr[8] = 61; _rapportManier[8] = 9;
                _reportField[8] = "VAK 47"; _reportTab[8] = 110;

                _rapportVeldNr[9] = 99; _rapportManier[9] = 1;
                _reportField[9] = "ID.Code/Naam"; _reportTab[9] = 2;

                _rapportVeldNr[10] = 62; _rapportManier[10] = 9;
                _reportField[10] = "VAK 48"; _reportTab[10] = 77;

                _rapportVeldNr[11] = 63; _rapportManier[11] = 9;
                _reportField[11] = "VAK 49"; _reportTab[11] = 88;

                _rapportVeldNr[12] = 64; _rapportManier[12] = 9;
                _reportField[12] = _ar == 12 ? "VAK 54" : "VAK 64";
                _reportTab[12] = 99;

                _reportTab[13] = 0;
                _tMaxVeld = 12;
            }

            // Build title lines — split at the wrap-back-to-left (manier==1, ID.Code/Naam)
            _reportTitle = new string(' ', 128);
            _reportTitle2 = new string(' ', 128);
            bool onLine2 = false;
            for (int t = 0; _reportTab[t] != 0 && t <= _tMaxVeld; t++)
            {
                if (_rapportManier[t] == 1)  // wrap point
                    onLine2 = true;
                if (onLine2)
                    _reportTitle2 = SafeInsert(_reportTitle2, _reportTab[t], _reportField[t]);
                else
                    _reportTitle = SafeInsert(_reportTitle, _reportTab[t], _reportField[t]);
            }
            _reportTitle = _reportTitle.Substring(0, 128);
            _reportTitle2 = _reportTitle2.Substring(0, 128);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report helpers
        // ═══════════════════════════════════════════════════════════════════════
        private static string SafeInsert(string s, int pos, string ins)
        {
            if (pos >= s.Length) return s;
            string result = s.Substring(0, pos) + ins + s.Substring(Math.Min(s.Length, pos + ins.Length));
            return result.Length > 128 ? result.Substring(0, 128) : result;
        }

        private static string Truncate(string s, int maxLen)
            => s != null && s.Length > maxLen ? s.Substring(0, maxLen) : s ?? "";

        private static string FieldVal(DataRow row, string colName)
        {
            if (!row.Table.Columns.Contains(colName)) return "";
            return row.IsNull(colName) ? "" : row[colName].ToString();
        }

        private static double FieldDouble(DataRow row, string colName)
        {
            if (!row.Table.Columns.Contains(colName)) return 0;
            if (row.IsNull(colName)) return 0;
            string raw = row[colName].ToString();
            if (string.IsNullOrWhiteSpace(raw)) return 0;
            return Convert.ToDouble(raw);
        }

        private void ReportPrintHeader()
        {
            Mim.Report.SelectFont("Courier New", (int)7.2);
            Mim.Report.TextBold = true;
            Mim.Report.TextColor = ColorTranslator.FromOle(0);
            Mim.Report.nTopMargin = 1;
            Mim.Report.nBottomMargin = 29;
            Mim.Report.nLeftMargin = 0.5;
            Mim.Report.nRightMargin = 0.5;
            Mim.Report.PenSize = 0.01;

            _pageCounter++;
            _ypos = Mim.Report.Print(1, 1, _reportHeader);
            _ypos = Mim.Report.Print(17, 1, "Pagina : " + Dec(_pageCounter, "##########"));
            _ypos = Mim.Report.Print(17, _ypos, "Datum  : " + _reportDate);
            _ypos = Mim.Report.Print(1, _ypos, SubTitleTextBox.Text.ToUpper());
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            _ypos = Mim.Report.Print(1, _ypos, _reportTitle);
            _ypos = Mim.Report.Print(1, _ypos, _reportTitle2);
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

        // ═══════════════════════════════════════════════════════════════════════
        // Print one document header row
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintDocRow(DataRow docRow)
        {
            string line = new string(' ', 128);

            for (int t = 0; _reportTab[t] != 0 && t <= _tMaxVeld; t++)
            {
                string val;
                switch (_rapportManier[t])
                {
                    case 1: // supplier/customer code + name
                        string partyKey = FieldVal(docRow, "v034");
                        if (partyKey.Length > 1) partyKey = partyKey.Substring(1);
                        string partyName = "";
                        if (!string.IsNullOrEmpty(partyKey))
                        {
                            string sqlParty =
                                "SELECT A110, A100 FROM " +
                                (_tableIndex == TABLE_SUPPLIERS ? "Leveranciers" : "Klanten") +
                                " WHERE A110 = '" + partyKey.Trim() + "'";
                            var dt = new DataTable();
                            using (var conn = new OleDbConnection(oleDbConnect))
                            using (var adapter = new OleDbDataAdapter(sqlParty, conn))
                                adapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                                partyName = dt.Rows[0]["A110"].ToString().Trim() + " " +
                                            dt.Rows[0]["A100"].ToString().Trim();
                            else
                                partyName = partyKey + " is niet meer aanwezig";
                        }
                        val = Truncate(partyName, 50);
                        break;

                    case 5: // date
                        val = DateText(FieldVal(docRow, "v" + _rapportVeldNr[t].ToString("D3")));
                        break;

                    case 9: // amount
                        double amount = FieldDouble(docRow, "v" + _rapportVeldNr[t].ToString("D3"));
                        _kolomTotaal[t] += amount;
                        val = Dec(amount, MASK_EURBH);
                        int colW = (t < _tMaxVeld && _reportTab[t + 1] > _reportTab[t])
                            ? _reportTab[t + 1] - _reportTab[t]
                            : 128 - _reportTab[t];
                        if (val.Length > colW) val = val.Substring(val.Length - colW);
                        break;

                    default: // raw
                        val = FieldVal(docRow, "v" + _rapportVeldNr[t].ToString("D3"));
                        break;
                }

                line = SafeInsert(line, _reportTab[t], val);

                // Newline when tab wraps back to left
                if (t < _tMaxVeld && _reportTab[t + 1] < _reportTab[t])
                {
                    _ypos = Mim.Report.Print(1, _ypos, line);
                    line = new string(' ', 128);
                    CheckPageBreak();
                }
            }

            _ypos = Mim.Report.Print(1, _ypos, line);
            _ypos = Mim.Report.Print(1, _ypos, "\n");
            CheckPageBreak();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Print detail journal lines for a document
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintDetailLines(string docNr)
        {
            if (!DetailJournalCheckBox.Checked) return;

            _ypos = Mim.Report.Print(1, _ypos, "");  // blank line before details

            DataRow[] jouRows = _journalTable.Select("v033 = '" + docNr + "'");

            // Layout: two journal entries per line
            // Entry 1: acct@2, name@12 (36 chars), amount@52 (12 chars) → ends @64
            // Entry 2: acct@66, name@76 (36 chars), amount@116 (12 chars) → ends @128
            const int COL2_OFFSET = 64;  // shift for second entry on same line

            string detailLine = null;

            for (int i = 0; i < jouRows.Length; i++)
            {
                DataRow jRow = jouRows[i];
                string acct = FieldVal(jRow, "v019");
                string acctName = "";

                if (acct.Length >= 2 &&
                    (acct.Substring(0, 2) == "40" || acct.Substring(0, 2) == "44"))
                {
                    acctName = FieldVal(jRow, "v067");
                }
                else
                {
                    string sqlAcct =
                        "SELECT v020 FROM Rekeningen WHERE v019 = '" + acct.Trim() + "'";
                    var dt = new DataTable();
                    using (var conn = new OleDbConnection(oleDbConnect))
                    using (var adapter = new OleDbDataAdapter(sqlAcct, conn))
                        adapter.Fill(dt);
                    acctName = dt.Rows.Count > 0 ? dt.Rows[0]["v020"].ToString() : "Rekening vernietigd";
                }

                double dc = FieldDouble(jRow, "v068");
                DetailCumulOleDbFlDummy(acct, dc);

                bool isFirst = (i % 2 == 0);
                if (isFirst)
                    detailLine = new string(' ', 128);

                int baseCol = isFirst ? 0 : COL2_OFFSET;
                detailLine = SafeInsert(detailLine, baseCol + 2, acct);
                detailLine = SafeInsert(detailLine, baseCol + 12, Truncate(acctName, 36));
                detailLine = SafeInsert(detailLine, baseCol + 52, Dec(dc, MASK_EURBH));

                bool isLast = (i == jouRows.Length - 1);
                bool isSecond = !isFirst;
                if (isSecond || isLast)
                {
                    _ypos = Mim.Report.Print(1, _ypos, detailLine.Substring(0, 128));
                    CheckPageBreak();
                    detailLine = null;
                }
            }

            _ypos = Mim.Report.Print(1, _ypos, "\n");  // blank line after details
            CheckPageBreak();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Print totals row
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintTotals()
        {
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);

            string line = new string(' ', 128);
            line = SafeInsert(line, 2, "TOTAAL");

            for (int t = 0; _reportTab[t] != 0 && t <= _tMaxVeld; t++)
            {
                if (_rapportManier[t] == 9)
                {
                    string totVal = Dec(_kolomTotaal[t], MASK_EURBH);
                    int totColW = (t < _tMaxVeld && _reportTab[t + 1] > _reportTab[t])
                        ? _reportTab[t + 1] - _reportTab[t]
                        : 128 - _reportTab[t];
                    if (totVal.Length > totColW) totVal = totVal.Substring(totVal.Length - totColW);
                    line = SafeInsert(line, _reportTab[t], totVal);

                    if (t < _tMaxVeld && _reportTab[t + 1] < _reportTab[t])
                    {
                        _ypos = Mim.Report.Print(1, _ypos, line);
                        line = new string(' ', 128);
                    }
                }
            }

            _ypos = Mim.Report.Print(1, _ypos, line);
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // VPE open / close
        // ═══════════════════════════════════════════════════════════════════════
        private void OpenReport()
        {
            if (Mim.Report.IsOpen())
                Mim.Report.CloseDoc();

            Mim.Report.OpenDoc();
            Mim.Report.Author = "marIntegraal";
            Mim.Report.GUILanguage = IDEALSoftware.VpeCommunity.GUILanguage.Dutch;
            Mim.Report.Title = _ledgerName;
            _pageCounter = 0;
        }

        private void CloseReport()
        {
            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.MailSubject = _ledgerName;
            Mim.Report.MailText = _ledgerName + " in bijlage.";
            Mim.Report.AddMailReceiver(MailAddressTextBox.Text, IDEALSoftware.VpeCommunity.RecipientClass.To);
            Mim.Report.Preview();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Main generation
        // ═══════════════════════════════════════════════════════════════════════
        private void GenerateReport()
        {
            if (!LoadData()) return;

            _cumulData.Clear();

            InitVelden();

            // Build report header string
            string companyName = "";
            int bracketOpen = Mim.Text.IndexOf('[');
            int bracketClose = Mim.Text.IndexOf(']');
            if (bracketOpen >= 0 && bracketClose > bracketOpen)
                companyName = Mim.Text.Substring(bracketOpen + 1, bracketClose - bracketOpen - 1);

            bool isFactuur = RadioFacturen.Checked;
            string docType = isFactuur ? RadioFacturen.Text : RadioCreditnotas.Text;
            _reportHeader = _ledgerName + " " + docType + " " + companyName;
            _reportDate = ProcessingDate.Value.ToString("dd/MM/yyyy");

            OpenReport();
            ReportPrintHeader();

            foreach (DataRow docRow in _docsTable.Rows)
            {
                PrintDocRow(docRow);
                PrintDetailLines(FieldVal(docRow, "v033"));
            }

            PrintTotals();

            // Cumulative totals per ledger account (square check)
            CumulPrint();

            CloseReport();
        }

        private void DetailCumulOleDbFlDummy(string v019, double v068)
        {
            string key = VSet(v019, FLINDEX_LEN[TABLE_LEDGERACCOUNTS, 0]);
            if (_cumulData.TryGetValue(key, out var entry))
                _cumulData[key] = (entry.count + 1, entry.amount + v068);
            else
                _cumulData[key] = (1, v068);
        }

        private void CumulPrint()
        {
            // Start a new page for the cumulative section
            Mim.Report.PageBreak();
            ReportPrintHeader();

            _ypos = Mim.Report.Print(1, _ypos, "\n");
            _ypos = Mim.Report.Print(1, _ypos, "  ** CENTRALISATIE/VIERKANTSCONTROLE **");
            _ypos = Mim.Report.Print(1, _ypos, "\n");

            if (_cumulData.Count == 0) return;

            int tabul = 0;
            string currentLine = new string(' ', 128);
            bool firstEntry = true;

            foreach (var kv in _cumulData)
            {
                string rekeningNaam = GetCumulRekeningNaam(kv.Key);
                string entry = BuildCumulEntry(kv.Key, kv.Value.count, kv.Value.amount, rekeningNaam);

                if (firstEntry)
                {
                    currentLine = SafeInsert(currentLine, 2, entry);
                    firstEntry = false;
                    tabul = 56;
                }
                else if (tabul == 56)
                {
                    // fill right column and flush
                    currentLine = SafeInsert(currentLine, tabul + 2, entry);
                    _ypos = Mim.Report.Print(1, _ypos, currentLine.Substring(0, 128));
                    currentLine = new string(' ', 128);
                    CheckPageBreak();
                    tabul = 0;
                }
                else
                {
                    // left column of a new line
                    currentLine = SafeInsert(currentLine, 2, entry);
                    tabul = 56;
                }
            }

            // Flush any orphaned left-column entry
            if (!firstEntry && tabul == 56)
            {
                _ypos = Mim.Report.Print(1, _ypos, currentLine.Substring(0, 128));
                CheckPageBreak();
            }
        }
            

        // ── Helpers for CumulPrint ───────────────────────────────────────────────────

        private string GetCumulRekeningNaam(string acctKey)
        {
            string sql = "SELECT v020 FROM Rekeningen WHERE v019 LIKE '" + acctKey.Trim() + "%'";
            var dt = new DataTable();
            using (var conn = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sql, conn))
                adapter.Fill(dt);

            return dt.Rows.Count > 0
                ? VSet(dt.Rows[0]["v020"].ToString(), 30)
                : VSet("Rekening reeds vernietigd !!!", 30);
        }

        private string BuildCumulEntry(string acctKey, double count, double amount, string rekeningNaam)
        {
            return Dec(count, "####") + " x " + VSet(acctKey, 7) + " " + rekeningNaam + " " + Dec(amount, MASK_EUR);
        }

        private static double ToDouble(string s) =>
            double.TryParse(s?.Trim(), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double d) ? d : 0.0;

        private static int GetByperdatPeriodNumber()
        {
            foreach (Form f in Application.OpenForms)
                if (f is FormBYPERDAT byp && byp.CmbPeriodeBoekjaar.SelectedItem != null)
                    return byp.CmbPeriodeBoekjaar.SelectedIndex + 1;
            return -1;
        }
    }
}

