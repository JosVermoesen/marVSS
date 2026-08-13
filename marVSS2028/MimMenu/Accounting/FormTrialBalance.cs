using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormTrialBalance : Form
    {     
        // ── Period / account range ──────────────────────────────────────────────
        private string _periodFrom  = BOOKYEAR_FROMTO.Substring(0, 8);
        private string _periodTo    = BOOKYEAR_FROMTO.Substring(8);
        
        // ── Running totals ──────────────────────────────────────────────────────
        private double _subTotalD;
        private double _subTotalC;
        private double _cumTotalD;
        private double _cumTotalC;
        private double _totalD;
        private double _totalC;

        // ── Report helpers ──────────────────────────────────────────────────────
        private string   _fullLine    = new string('-', 128);
        private string   _reportTitle = "";
        private string[] _reportText  = new string[4];   // [0]=date [1]=period [2]=header [3]=subtitle
        private string[] _reportField = new string[8];
        private int[]    _reportTab   = new int[8];
        private double   _ypos;
        private int      _pageCounter;
        private int      _lineCounter;

        // ── Data ────────────────────────────────────────────────────────────────
        private DataTable _journalDT   = new DataTable();
        
        public FormTrialBalance()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Load
        // ═══════════════════════════════════════════════════════════════════════
        private void FormTrialBalance_Load(object sender, EventArgs e)
        {
            ProcessingDate.Value = DateTime.Today;

            // Period text box: "dd/mm/yyyy - dd/mm/yyyy"
            SelectedPeriodTextBox.Text = FormattedFromTo(_periodFrom, _periodTo);

            // Account range text boxes
            AccountFromTextBox.Text = "1";
            AccountToTextBox.Text   = "7999999";
        }

        // ═══════════════════════════════════════════════════════════════════════
        // UI event handlers
        // ═══════════════════════════════════════════════════════════════════════        
        private void ButtonClose_Click(object sender, EventArgs e) => Close();

        private void SelectedPeriodTextBox_Leave(object sender, EventArgs e)
        {
            string a = SelectedPeriodTextBox.Text;
            if (a.Length != 23)
            {
                MessageBox.Show("Respecteer :\n\nDD/MM/JJJJ - DD/MM/JJJJ a.u.b. !");
                SelectedPeriodTextBox.Text = FormattedFromTo(_periodFrom, _periodTo);
                return;
            }
            if (DateInvalid(a.Substring(0, 10)))
            {
                MessageBox.Show("Ongeldige 'Van' datum !");
                SelectedPeriodTextBox.Text = FormattedFromTo(_periodFrom, _periodTo);
                SelectedPeriodLabel.Focus();
                return;
            }
            if (DateInvalid(a.Substring(13)))
            {
                MessageBox.Show("Ongeldige 'Tot' datum !");
                SelectedPeriodTextBox.Text = FormattedFromTo(_periodFrom, _periodTo);
                SelectedPeriodLabel.Focus();
                return;
            }
            SetPeriodFromTextBox();
        }

        private void ButtonGenerateReport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (DetailJournalCheckBox.Checked)
                    GenerateDetailJournalReport();
                else
                    GenerateTrialBalanceReport();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════════════════
        private string FormattedFromTo(string from, string to)
            => DateText(from) + " - " + DateText(to);

        private void SetPeriodFromTextBox()
        {
            string a = SelectedPeriodTextBox.Text;
            _periodFrom = a.Substring(6, 4) + a.Substring(3, 2) + a.Substring(0, 2);
            _periodTo   = a.Substring(19, 4) + a.Substring(16, 2) + a.Substring(13, 2);
        }
        
        // ═══════════════════════════════════════════════════════════════════════
        // Data retrieval
        // ═══════════════════════════════════════════════════════════════════════
        private void LoadJournalData()
        {
            Cursor.Current = Cursors.WaitCursor;

            string fieldSoldeOfYear = "r.e" + (ACTIVE_BOOKYEAR + 22).ToString("000");
                        
            string sql =
                "SELECT j.v019, j.v066, j.v067, j.v033, j.dece068, " +
                "r.v020, " +
                fieldSoldeOfYear + " AS rSaldo " +
                "FROM Journalen j LEFT JOIN Rekeningen r ON Trim(j.v019) = Trim(r.v019) " +
                "WHERE j.v066 >= '" + _periodFrom + "' " +
                "AND j.v066 <= '" + _periodTo + "' " +
                "ORDER BY j.v019, j.v066";

            _journalDT = new DataTable();
            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sql, conn))
                adapter.Fill(_journalDT);

            Cursor.Current = Cursors.Default;
        }

        private void LoadDetailJournalData()
        {
            Cursor.Current = Cursors.WaitCursor;

            string sql =
                "SELECT j.v066, j.v019, r.v020, j.v067, j.dece068, j.v033 " +
                "FROM Journalen j LEFT JOIN Rekeningen r ON Trim(j.v019) = Trim(r.v019) " +
                "WHERE j.v066 >= '" + _periodFrom + "' " +
                "AND j.v066 <= '" + _periodTo + "' " +
                "ORDER BY j.v066, j.rvID";

            _journalDT = new DataTable();
            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sql, conn))
                adapter.Fill(_journalDT);

            Cursor.Current = Cursors.Default;
        }
        
        // ═══════════════════════════════════════════════════════════════════════
        // Field / column layout
        // ═══════════════════════════════════════════════════════════════════════
        private void InitializeFields(bool detailMode)
        {
            _reportTitle = new string(' ', 128);

            if (detailMode)
            {
                _reportField[0] = "Nr.Ln";                  _reportTab[0] = 0;
                _reportField[1] = "Datum";                  _reportTab[1] = 7;
                _reportField[2] = "RNummer";                _reportTab[2] = 18;
                _reportField[3] = "Naam Rekening";          _reportTab[3] = 26;
                _reportField[4] = "Boekingsomschrijving";   _reportTab[4] = 67;
                _reportField[5] = "    Debet";              _reportTab[5] = 103;
                _reportField[6] = "   Credit";              _reportTab[6] = 114;
                _reportField[7] = "dokument";               _reportTab[7] = 125;
            }
            else
            {
                _reportField[0] = "Nummer";                 _reportTab[0] = 0;
                _reportField[1] = "Omschrijving Rekening";  _reportTab[1] = 9;
                _reportField[2] = "         Saldo";         _reportTab[2] = 50;
                _reportField[3] = "Maand";                  _reportTab[3] = 65;
                _reportField[4] = "      Debet";            _reportTab[4] = 77;
                _reportField[5] = "     Credit";            _reportTab[5] = 88;
                _reportField[6] = "    Mnd Saldo";          _reportTab[6] = 101;
                _reportField[7] = "    D/C Cumul";          _reportTab[7] = 115;
            }

            for (int t = 0; t < 8; t++)
                _reportTitle = SafeInsert(_reportTitle, _reportTab[t], _reportField[t]);

            _reportTitle = _reportTitle.Substring(0, 128);
        }

        private static string SafeInsert(string s, int pos, string ins)
        {
            if (pos >= s.Length) return s;
            string result = s.Substring(0, pos) + ins + s.Substring(Math.Min(s.Length, pos + ins.Length));
            return result.Length > 128 ? result.Substring(0, 128) : result;
        }

        private static string BuildLine(string[] fields, int[] tabs)
        {
            string line = new string(' ', 128);
            for (int t = 0; t < 8; t++)
            {
                if (tabs[t] >= line.Length) break;
                string val = fields[t] ?? "";
                int avail = 128 - tabs[t];
                if (val.Length > avail) val = val.Substring(0, avail);
                line = line.Substring(0, tabs[t]) + val + line.Substring(Math.Min(128, tabs[t] + val.Length));
            }
            return line.Substring(0, 128);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report header
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
            _ypos = Mim.Report.Print(1,  1,     _reportText[2]);
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

        private static string DutchMonthName(string mm)
        {
            string[] names = { "", "Januari", "Februari", "Maart", "April", "Mei", "Juni",
                                    "Juli", "Augustus", "September", "Oktober", "November", "December" };
            return int.TryParse(mm, out int m) && m >= 1 && m <= 12 ? names[m] : mm;
        }

        private void FlushMonthTotals(string month, string acctNr, string acctName, string openingSaldo, bool showAcct)
        {
            if (_subTotalD == 0 && _subTotalC == 0) return;

            var flds = new string[8];
            if (showAcct)
            {
                flds[0] = acctNr;
                flds[1] = Truncate(acctName, 40);
                flds[2] = openingSaldo;
            }
            flds[3] = DutchMonthName(month);
            flds[4] = Dec(_subTotalD,            "#######0.00");
            flds[5] = Dec(Math.Abs(_subTotalC),  "#######0.00");
            flds[6] = FormatDC(_subTotalD + _subTotalC);
            flds[7] = FormatDC(_cumTotalD + _cumTotalC);

            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds, _reportTab));
            CheckPageBreak();

            _subTotalD = 0;
            _subTotalC = 0;
        }

        private void FlushAccountTotals()
        {
            // _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            CheckPageBreak();
        }

        private static string FormatDC(double v)
        {
            if (v < 0) return "C:" + Dec(Math.Abs(v), "#######0.00");
            if (v > 0) return "D:" + Dec(v,            "#######0.00");
            return "  " + Dec(0,                        "#######0.00");
        }

        private static string Truncate(string s, int maxLen)
            => s != null && s.Length > maxLen ? s.Substring(0, maxLen) : s ?? "";

        // ═══════════════════════════════════════════════════════════════════════
        // Grand total line
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintGrandTotal(bool detailMode)
        {
            var flds = new string[8];
            flds[1] = "Totalen :";

            if (detailMode)
            {
                flds[5] = Dec(_totalD,            "#######0.00");
                flds[6] = Dec(Math.Abs(_totalC),  "#######0.00");
            }
            else
            {
                flds[4] = Dec(_totalD,            "#######0.00");
                flds[5] = Dec(Math.Abs(_totalC),  "#######0.00");
            }

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds, _reportTab));
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);

            if (Math.Round(_totalD, 2) + Math.Round(_totalC, 2) != 0)
                MessageBox.Show(
                    "Katastrofale fout : Algemene cumul Debet <> cumul credit.\n" +
                    "Kontakteer ons 053/21.59.25 !\n" +
                    "Indien U niet beschikt over veiligheidskopij dient recuperatie van bestanden door ons te gebeuren.",
                    "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
                
        // ═══════════════════════════════════════════════════════════════════════
        // Generate report: Trial Balance (Proef- en Saldibalans)
        // ═══════════════════════════════════════════════════════════════════════
        private void GenerateTrialBalanceReport()
        {            
            LoadJournalData();
            if (_journalDT.Rows.Count == 0)
            {
                MessageBox.Show("Geen gegevens gevonden.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenReport("Proef- en Saldibalans");

            _reportText[0] = ProcessingDate.Value.ToString("dd/MM/yyyy");

            // Get company info for reportheader 
            string companyName = "";
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                companyName = mim.Text.Substring(mim.Text.IndexOf('[') + 1, mim.Text.IndexOf(']') - mim.Text.IndexOf('[') - 1);
            }
            _reportText[2] = "Proef- en Saldibalans " + companyName;
            _reportText[3] = "Boekjaar aanvang : " + BOOKYEAR_FROMTO.Substring(0, 4) + ", " + SelectedPeriodTextBox.Text;

            _lineCounter  = 0;
            _totalD       = 0;
            _totalC       = 0;
            _subTotalD    = 0;
            _subTotalC    = 0;
            _cumTotalD    = 0;
            _cumTotalC    = 0;

            InitializeFields(detailMode: false);
            ReportPrintHeader();

            string prevAcct     = "";
            string prevAcctName = "";
            string openingSaldo = "";
            string prevMonth    = "";
            bool   isFirstMonth = true;

            foreach (DataRow row in _journalDT.Rows)
            {
                string acct  = row["v019"].ToString().Trim();
                string date  = row["v066"].ToString();
                string month = date.Length >= 6 ? date.Substring(4, 2) : "  ";

                if (acct != prevAcct)
                {
                    // Flush previous account
                    if (prevAcct != "")
                    {
                        FlushMonthTotals(prevMonth, prevAcct, prevAcctName, openingSaldo, isFirstMonth);
                        FlushAccountTotals();
                    }

                    // New account header info
                    prevAcct     = acct;
                    prevAcctName = row["v020"].ToString();
                    double saldo = (!Convert.IsDBNull(row["rSaldo"]) &&
                                    double.TryParse(row["rSaldo"].ToString().Trim(),
                                                    System.Globalization.NumberStyles.Any,
                                                    System.Globalization.CultureInfo.InvariantCulture,
                                                    out double parsedSaldo))
                                   ? parsedSaldo : 0;
                    openingSaldo = saldo < 0
                        ? "CS:" + Dec(Math.Abs(saldo), "#######0.00")
                        : "DS:" + Dec(saldo, "#######0.00");


                    _cumTotalD  = 0;
                    _cumTotalC  = 0;
                    prevMonth   = month;
                    isFirstMonth = true;
                }
                else if (month != prevMonth)
                {
                    FlushMonthTotals(prevMonth, prevAcct, prevAcctName, openingSaldo, isFirstMonth);
                    isFirstMonth = false;
                    prevMonth    = month;
                }

                // Accumulate
                double dc = (!Convert.IsDBNull(row["dece068"]) &&
                             double.TryParse(row["dece068"].ToString().Trim(),
                                            System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            out double parsedDc))
                            ? parsedDc : 0;
                if (dc < 0)
                {
                    _subTotalC += dc;
                    _cumTotalC += dc;
                    _totalC    += dc;
                }
                else
                {
                    _subTotalD += dc;
                    _cumTotalD += dc;
                    _totalD    += dc;
                }
            }

            // Flush last account
            if (prevAcct != "")
            {
                FlushMonthTotals(prevMonth, prevAcct, prevAcctName, openingSaldo, isFirstMonth);
                FlushAccountTotals();
            }

            PrintGrandTotal(detailMode: false);
            CloseReport("Proef- en Saldibalans");
        }

        // ═══════════════════════════════════════════════════════════════════════
        // VPE report open/close helpers
        // ═══════════════════════════════════════════════════════════════════════
        private void OpenReport(string title)
        {
            if (Mim.Report.IsOpen())
                Mim.Report.CloseDoc();

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
            Mim.Report.AddMailReceiver(MailAddressTextBox.Text, IDEALSoftware.VpeCommunity.RecipientClass.To);
            Mim.Report.Preview();
        }
                
        // ═══════════════════════════════════════════════════════════════════════
        // Print one detail-journal line
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintDetailLine(DataRow row)
        {
            _lineCounter++;
            double dc = Convert.IsDBNull(row["dece068"]) ? 0 : Convert.ToDouble(row["dece068"]);

            var flds = new string[8];
            flds[0] = _lineCounter.ToString("00000");
            flds[1] = DateText(row["v066"].ToString());
            flds[2] = row["v019"].ToString();
            flds[3] = Truncate(row["v020"].ToString(), 40);
            flds[4] = Truncate(row["v067"].ToString(), 35);

            if (dc < 0)
            {
                flds[5] = "";
                flds[6] = Dec(Math.Abs(dc), "#######0.00");
                _totalC += dc;
            }
            else
            {
                flds[5] = Dec(dc, "#######0.00");
                flds[6] = "";
                _totalD += dc;
            }
            flds[7] = row["v033"].ToString();

            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds, _reportTab));
            CheckPageBreak();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Generate report: Detail Journal
        // ═══════════════════════════════════════════════════════════════════════
        private void GenerateDetailJournalReport()
        {
            LoadDetailJournalData();
            if (_journalDT.Rows.Count == 0)
            {
                MessageBox.Show("Geen gegevens gevonden.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenReport("Algemeen Journaal");

            _reportText[0] = ProcessingDate.Value.ToString("dd/MM/yyyy");
            _reportText[2] = "Algemeen Journaal (Systeem OT) " + BOOKYEAR_FROMTO.Substring(0, 4);
            _reportText[3] = "Boekjaar aanvang : " + BOOKYEAR_FROMTO.Substring(0, 4) + ", " + SelectedPeriodTextBox.Text;

            _lineCounter = 0;
            _totalD = 0;
            _totalC = 0;

            InitializeFields(detailMode: true);
            ReportPrintHeader();

            foreach (DataRow row in _journalDT.Rows)
                PrintDetailLine(row);

            PrintGrandTotal(detailMode: true);

            CloseReport("Algemeen Journaal");
        }
    }
}
