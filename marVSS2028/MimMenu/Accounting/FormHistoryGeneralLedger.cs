using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormHistoryGeneralLedger : Form
    {
        // ── Period / account range ──────────────────────────────────────────────
        private string _periodFrom = BOOKYEAR_FROMTO.Substring(0, 8);
        private string _periodTo   = BOOKYEAR_FROMTO.Substring(8);

        // ── Running totals ──────────────────────────────────────────────────────
        private double _subTotalD;
        private double _subTotalC;
        private double _totalD;
        private double _totalC;
        private double _algTotalD;
        private double _algTotalC;

        // ── Report helpers ──────────────────────────────────────────────────────
        private readonly string   _fullLine    = new string('-', 128);
        private string            _reportTitle = "";
        private string            _subTitle    = "";
        private string            _reportDate  = "";
        private string            _reportHeader = "";
        private string[]          _reportField = new string[9];
        private int[]             _reportTab   = new int[10];
        private double            _ypos;
        private int               _pageCounter;
        private int               _lineCounter;

        // ── Data ────────────────────────────────────────────────────────────────
        private DataTable _journalDT = new DataTable();

        public FormHistoryGeneralLedger()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Load
        // ═══════════════════════════════════════════════════════════════════════
        private void FormHistoryGeneralLedger_Load(object sender, EventArgs e)
        {
            ProcessingDate.Value       = DateTime.Today;
            SelectedPeriodTextBox.Text = FormattedFromTo(_periodFrom, _periodTo);
            AccountFromTextBox.Text    = "1";
            AccountToTextBox.Text      = "7999999";
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

        private void AccountFromTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AccountFromTextBox.Text))
                AccountFromTextBox.Text = "1";
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

        private static string Truncate(string s, int maxLen)
            => s != null && s.Length > maxLen ? s.Substring(0, maxLen) : s ?? "";

        // ═══════════════════════════════════════════════════════════════════════
        // Data retrieval
        // ═══════════════════════════════════════════════════════════════════════
        private void LoadData()
        {
            string acctFrom = AccountFromTextBox.Text.Trim().PadRight(7).Substring(0, 7);
            string acctTo   = AccountToTextBox.Text.Trim().PadRight(7).Substring(0, 7);

            string sql =
                "SELECT j.v019, j.v066, j.v067, j.v033, j.v038, j.dece068, j.v069, " +
                "r.v020 " +
                "FROM Journalen j LEFT JOIN Rekeningen r ON Trim(j.v019) = Trim(r.v019) " +
                "WHERE Trim(j.v019) >= '" + acctFrom.Trim() + "' " +
                "AND Trim(j.v019) <= '" + acctTo.Trim() + "' " +
                "AND j.v066 >= '" + _periodFrom + "' " +
                "AND j.v066 <= '" + _periodTo + "' " +
                "ORDER BY j.v019, j.v066, j.rvID";

            _journalDT = new DataTable();
            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sql, conn))
                adapter.Fill(_journalDT);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Field / column layout
        // ═══════════════════════════════════════════════════════════════════════
        private void InitializeFields()
        {
            _reportTitle  = new string(' ', 128);
            _reportField[0] = "Lijn";           _reportTab[0] = 0;
            _reportField[1] = "Datum";           _reportTab[1] = 7;
            _reportField[2] = "Omschrijving";    _reportTab[2] = 18;
            _reportField[3] = "Document";        _reportTab[3] = 51;
            _reportField[4] = "Fin.Doc.";        _reportTab[4] = 65;
            _reportField[5] = "       Debet";    _reportTab[5] = 76;
            _reportField[6] = "      Credit";    _reportTab[6] = 89;
            _reportField[7] = "T.Reken";         _reportTab[7] = 103;
            _reportField[8] = "VSF.Code";        _reportTab[8] = 111;
            _reportTab[9] = 0;  // sentinel

            for (int t = 0; t < 9; t++)
                _reportTitle = SafeInsert(_reportTitle, _reportTab[t], _reportField[t]);

            _reportTitle = _reportTitle.Substring(0, 128);
        }

        private static string SafeInsert(string s, int pos, string ins)
        {
            if (pos >= s.Length) return s;
            string result = s.Substring(0, pos) + ins + s.Substring(Math.Min(s.Length, pos + ins.Length));
            return result.Length > 128 ? result.Substring(0, 128) : result;
        }

        private string BuildLine(string[] fields)
        {
            string line = new string(' ', 128);
            for (int t = 0; t < 9; t++)
            {
                if (_reportTab[t] == 0 && t > 0) break;
                if (_reportTab[t] >= line.Length) break;
                string val  = fields[t] ?? "";
                int    avail = 128 - _reportTab[t];
                if (val.Length > avail) val = val.Substring(0, avail);
                line = line.Substring(0, _reportTab[t]) + val +
                       line.Substring(Math.Min(128, _reportTab[t] + val.Length));
            }
            return line.Substring(0, 128);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Report header
        // ═══════════════════════════════════════════════════════════════════════
        private void ReportPrintSubHeader()
        {            
            _ypos = Mim.Report.Print(1,  _ypos, _subTitle.ToUpper());
            _ypos = Mim.Report.Print(1,  _ypos, _fullLine);
            _ypos = Mim.Report.Print(1,  _ypos, _reportTitle);
            _ypos = Mim.Report.Print(1,  _ypos, _fullLine);
        }

        private void ReportPrintNewPageHeader()
        {
            Mim.Report.Title = "Historiek Rekeningen";
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
        }

        private void CheckPageBreak()
        {
            if (_ypos > 27.5)
            {
                Mim.Report.PageBreak();
                ReportPrintNewPageHeader();
                ReportPrintSubHeader();
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Subtotal / total lines
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintPeriodTotaal()
        {
            _algTotalD += _subTotalD;
            _algTotalC += _subTotalC;

            if (!PeriodiekeTotalenCheckBox.Checked)
            {
                _subTotalD = 0;
                _subTotalC = 0;
                return;
            }

            var flds = new string[9];
            flds[2] = "Periodiek totaal :";
            flds[5] = Dec(_subTotalD,           MASK_EURBH);
            flds[6] = Dec(Math.Abs(_subTotalC), MASK_EURBH);

            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds));
            _ypos = Mim.Report.Print(1, _ypos, "\n");
            CheckPageBreak();

            _subTotalD = 0;
            _subTotalC = 0;
        }

        private void PrintRekeningTotaal()
        {
            var flds = new string[9];
            flds[2] = "Boekjaar Totalen :";
            flds[5] = Dec(_totalD,           MASK_EURBH);
            flds[6] = Dec(Math.Abs(_totalC), MASK_EURBH);

            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds));
            _ypos = Mim.Report.Print(1, _ypos, "\n");
            CheckPageBreak();

            _lineCounter = 0;
            _subTotalD   = 0;
            _subTotalC   = 0;
            _totalD      = 0;
            _totalC      = 0;
        }

        private void PrintAlgemeenEindTotaal()
        {
            var flds = new string[9];
            flds[2] = "Proef- en Saldi :";
            flds[5] = Dec(_algTotalD,           MASK_EURBH);
            flds[6] = Dec(Math.Abs(_algTotalC), MASK_EURBH);

            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds));
            _ypos = Mim.Report.Print(1, _ypos, _fullLine);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Print one detail line
        // ═══════════════════════════════════════════════════════════════════════
        private void PrintDetailLine(DataRow row)
        {
            _lineCounter++;
            double dc = Convert.IsDBNull(row["dece068"]) ? 0 : Convert.ToDouble(row["dece068"]);

            var flds = new string[9];
            flds[0] = _lineCounter.ToString("00000");
            flds[1] = DateText(row["v066"].ToString());
            flds[2] = Truncate(row["v067"].ToString(), 32);
            flds[3] = Truncate(row["v033"].ToString(), 13);
            flds[4] = Truncate(row["v038"].ToString(), 10);
            flds[7] = Truncate(row["v069"].ToString(), 7);
            flds[8] = "";

            if (dc < 0)
            {
                _subTotalC += dc;
                _totalC    += dc;
                flds[5]    = "";
                flds[6]    = Dec(Math.Abs(dc), MASK_EURBH);
            }
            else
            {
                _subTotalD += dc;
                _totalD    += dc;
                flds[5]    = Dec(dc, MASK_EURBH);
                flds[6]    = "";
            }

            _ypos = Mim.Report.Print(1, _ypos, BuildLine(flds));
            CheckPageBreak();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // VPE report open / close
        // ═══════════════════════════════════════════════════════════════════════
        private void OpenReport()
        {
            if (Mim.Report.IsOpen())
                Mim.Report.CloseDoc();

            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = IDEALSoftware.VpeCommunity.GUILanguage.Dutch;
            _pageCounter = 0;
        }

        private void CloseReport()
        {
            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.MailSubject = "Historiek Rekeningen (Grootboek)";
            Mim.Report.MailText    = "Historiek Rekeningen in bijlage.";
            Mim.Report.AddMailReceiver(MailAddressTextBox.Text, IDEALSoftware.VpeCommunity.RecipientClass.To);
            Mim.Report.Preview();
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Main report generation
        // ═══════════════════════════════════════════════════════════════════════
        private void GenerateReport()
        {
            LoadData();
            if (_journalDT.Rows.Count == 0)
            {
                MessageBox.Show("Geen gegevens gevonden.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenReport();

            // Get company info for reportheader 
            string companyName = "";
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                companyName = mim.Text.Substring(mim.Text.IndexOf('[') + 1, mim.Text.IndexOf(']') - mim.Text.IndexOf('[') - 1);
            }
            _reportHeader = "Historieken " + companyName;
            
            _reportDate   = ProcessingDate.Value.ToString("dd/MM/yyyy");
            
            _lineCounter  = 0;
            _subTotalD    = 0;
            _subTotalC    = 0;
            _totalD       = 0;
            _totalC       = 0;
            _algTotalD    = 0;
            _algTotalC    = 0;

            InitializeFields();

            string prevAcct    = "";
            string prevMonth   = "";
            bool   firstAcct   = true;

            foreach (DataRow row in _journalDT.Rows)
            {
                string acct  = row["v019"].ToString().Trim();
                string date  = row["v066"].ToString();
                string month = date.Length >= 6 ? date.Substring(4, 2) : "  ";

                if (acct != prevAcct)
                {
                    if (firstAcct)
                        ReportPrintNewPageHeader();

                    // Flush previous account
                    if (!firstAcct)
                    {
                        PrintPeriodTotaal();
                        PrintRekeningTotaal();
                    }

                    // Account subtitle
                    string acctName = row["v020"] != DBNull.Value ? row["v020"].ToString() : acct + " rekening reeds vernietigd...";
                    _subTitle = acct.PadRight(7) + " " + acctName;

                    ReportPrintSubHeader();

                    prevAcct  = acct;
                    prevMonth = month;
                    firstAcct = false;
                }
                else if (month != prevMonth)
                {
                    PrintPeriodTotaal();
                    prevMonth = month;
                }

                PrintDetailLine(row);
            }

            // Flush last account
            if (!firstAcct)
            {
                PrintPeriodTotaal();
                PrintRekeningTotaal();
            }

            PrintAlgemeenEindTotaal();
            CloseReport();
        }
    }
}
