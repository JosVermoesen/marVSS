using System;
using System.Drawing;
using System.Windows.Forms;

using System.Data;
using System.Data.OleDb;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormLedgerBook : Form
    {
        private DataTable JournalDT = new DataTable();

        public string PeriodFrom = PERIOD_FROMTO.Substring(0, 8);
        public string PeriodTo   = PERIOD_FROMTO.Substring(8);
        public string sSQL       = "";
        public string FullLine   = new string('-', 128);

        public double TotalDebit  = 0;
        public double TotalCredit = 0;

        public string   ReportTitle = "";
        public string[] ReportText  = new string[3];
        public string[] ReportField = new string[8];
        public int[]    ReportTab   = new int[8];

        public int    lineCounter;
        public double Ypos;
        public int    PageCounter = 0;

        public FormLedgerBook()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        private void FormLedgerBook_Load(object sender, EventArgs e)
        {
            LineCountTextBox.Text      = "0";
            SelectedPeriodTextBox.Text = FormattedFromTo(PeriodFrom, PeriodTo);
            GetJournalRS();
        }

        private string FormattedFromTo(string From, string To)
            => DateText(From) + " - " + DateText(To);

        public void GetJournalRS()
        {
            Cursor.Current = Cursors.WaitCursor;

            sSQL =
                "SELECT Journalen.v066, Journalen.v019, Rekeningen.v020, " +
                "Journalen.v067, Journalen.v033, Journalen.dece068, Journalen.v069 " +
                "FROM Journalen, Rekeningen " +
                "WHERE Journalen.v019 = Rekeningen.v019 " +
                "AND Journalen.v033 Like 'D0%' " +
                "AND Journalen.v066 >= '" + PeriodFrom + "' " +
                "AND Journalen.v066 <= '" + PeriodTo   + "' " +
                "ORDER BY Journalen.v066";

            JournalDT = new DataTable();

            using (var conn    = new OleDbConnection(oleDbConnect))
            using (var adapter = new OleDbDataAdapter(sSQL, conn))
            {
                adapter.Fill(JournalDT);
            }

            if (JournalDT.Rows.Count == 0)
            {
                LineCountTextBox.Text        = "0";
                ButtonGenerateReport.Enabled = false;
            }
            else
            {
                LineCountTextBox.Text        = JournalDT.Rows.Count.ToString();
                ButtonGenerateReport.Enabled = true;
            }

            Cursor.Current = Cursors.Default;
        }

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

            PageCounter++;
            Ypos = Mim.Report.Print(1,  1,    ReportText[1]);
            Ypos = Mim.Report.Print(17, 1,    "Pagina : " + Dec(PageCounter, "##########"));
            Ypos = Mim.Report.Print(17, Ypos, "Datum  : " + ReportText[0]);
            Ypos = Mim.Report.Print(1,  Ypos, ReportText[2].ToUpper());
            Ypos = Mim.Report.Print(1,  Ypos, FullLine);
            Ypos = Mim.Report.Print(1,  Ypos, ReportTitle);
            Ypos = Mim.Report.Print(1,  Ypos, FullLine);
        }

        private void InitializeFields()
        {
            ReportTitle    = new string(' ', 128);
            ReportField[0] = "Lijn";          ReportTab[0] = 0;
            ReportField[1] = "Datum";         ReportTab[1] = 5;
            ReportField[2] = "Nummer";        ReportTab[2] = 16;
            ReportField[3] = "Naam Rekening"; ReportTab[3] = 24;
            ReportField[4] = "Betreft";       ReportTab[4] = 61;
            ReportField[5] = "      Debet";   ReportTab[5] = 92;
            ReportField[6] = "     Credit";   ReportTab[6] = 103;
            ReportField[7] = "T.Rekening";    ReportTab[7] = 117;

            for (int t = 0; t < 8; t++)
                ReportTitle = ReportTitle.Insert(ReportTab[t], ReportField[t]);

            ReportTitle = ReportTitle.Substring(0, 128);
        }

        private void PrintLine(DataRow row)
        {
            string pdfLine = new string(' ', 128);

            lineCounter++;

            pdfLine = pdfLine.Insert(ReportTab[0], lineCounter.ToString("0000"));
            pdfLine = pdfLine.Insert(ReportTab[1], DateText(row["v066"].ToString()));
            pdfLine = pdfLine.Insert(ReportTab[2], row["v019"].ToString());

            string tempV020 = row["v020"].ToString();
            if (tempV020.Length > 36)
                tempV020 = tempV020.Substring(0, 36);
            pdfLine = pdfLine.Insert(ReportTab[3], tempV020);
            pdfLine = pdfLine.Insert(ReportTab[4], row["v067"].ToString());

            double dcBedrag = Convert.IsDBNull(row["dece068"]) ? 0 : Convert.ToDouble(row["dece068"]);
            if (dcBedrag < 0)
            {
                TotalCredit += dcBedrag;
                pdfLine = pdfLine.Insert(ReportTab[6], Dec(Math.Abs(dcBedrag), "#######0.00"));
            }
            else
            {
                TotalDebit += dcBedrag;
                pdfLine = pdfLine.Insert(ReportTab[5], Dec(dcBedrag, "#######0.00"));
            }

            pdfLine = pdfLine.Insert(ReportTab[7], row["v069"].ToString());
            pdfLine = pdfLine.Substring(0, 128);

            Ypos = Mim.Report.Print(1, Ypos, pdfLine);
            if (Ypos > 27.5)
            {
                Mim.Report.PageBreak();
                ReportPrintHeader();
            }
        }

        private void PrintTotal()
        {
            string pdfLineTotal = new string(' ', 128);

            pdfLineTotal = pdfLineTotal.Insert(ReportTab[0], "Totaal");
            pdfLineTotal = pdfLineTotal.Insert(ReportTab[5], Dec(TotalDebit,            "#######0.00"));
            pdfLineTotal = pdfLineTotal.Insert(ReportTab[6], Dec(Math.Abs(TotalCredit), "#######0.00"));
            pdfLineTotal = pdfLineTotal.Substring(0, 128);

            Ypos = Mim.Report.Print(1, Ypos, FullLine);
            Ypos = Mim.Report.Print(1, Ypos, pdfLineTotal);
            Ypos = Mim.Report.Print(1, Ypos, FullLine);
        }

        private void ButtonClose_Click(object sender, EventArgs e) => Close();

        private void SelectedPeriodTextBox_Leave(object sender, EventArgs e)
        {
            ButtonGenerateReport.Enabled = false;
            string A = SelectedPeriodTextBox.Text;

            if (A.Length != 23)
            {
                MessageBox.Show("Please use format From - To as:\ndd/mm/yyyy - dd/mm/yyyy");
                SelectedPeriodTextBox.Text = FormattedFromTo(PeriodFrom, PeriodTo);
                return;
            }
            if (DateInvalid(A.Substring(0, 10)))
            {
                MessageBox.Show("Invalid date format 'From'");
                SelectedPeriodTextBox.Text = FormattedFromTo(PeriodFrom, PeriodTo);
                SelectedPeriodeLabel.Focus();
                return;
            }
            if (DateInvalid(A.Substring(13)))
            {
                MessageBox.Show("Invalid date format 'To'");
                SelectedPeriodTextBox.Text = FormattedFromTo(PeriodFrom, PeriodTo);
                SelectedPeriodeLabel.Focus();
                return;
            }

            PeriodFrom = A.Substring(6, 4) + A.Substring(3, 2) + A.Substring(0, 2);
            PeriodTo   = A.Substring(19, 4) + A.Substring(16, 2) + A.Substring(13, 2);
            GetJournalRS();
        }

        private void ButtonGenerateReport_Click(object sender, EventArgs e)
        {
            if (Mim.Report.IsOpen())
                Mim.Report.CloseDoc();

            Mim.Report.OpenDoc();
            Mim.Report.Author      = "marIntegraal";
            Mim.Report.GUILanguage = IDEALSoftware.VpeCommunity.GUILanguage.Dutch;
            Mim.Report.Title       = "Diverse Postenboek";

            ReportText[0] = ProcessingDate.Value.ToString("dd/MM/yyyy");

            // Get company info for reportheader 
            string companyName = "";
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                companyName = mim.Text.Substring(mim.Text.IndexOf('[') + 1, mim.Text.IndexOf(']') - mim.Text.IndexOf('[') - 1);
            }
            ReportText[1] = "Diverse Postenboek " + companyName;
            ReportText[2] = SelectedPeriodTextBox.Text;

            lineCounter = 0;
            TotalDebit  = 0;
            TotalCredit = 0;

            InitializeFields();
            ReportPrintHeader();

            foreach (DataRow row in JournalDT.Rows)
                PrintLine(row);

            PrintTotal();

            Mim.Report.WriteDoc(LOCATION_COMPANYDATA);
            Mim.Report.MailSubject = "Diverse Posten bedrijfx";
            Mim.Report.MailText    = "diverseposten bedrijf ix in bijlage.";
            Mim.Report.AddMailReceiver(MailAddressTextBox.Text, IDEALSoftware.VpeCommunity.RecipientClass.To);
            Mim.Report.Preview();
        }  
    }
}
