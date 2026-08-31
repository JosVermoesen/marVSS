using ADODB;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using marVSS2028.SharedForms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.PeppolTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.DailyManagement
{
    public partial class FormBanking : Form
    {
        private int xdaLineCounter;
        private Recordset rsAny;

        private int BeginBalans;
        private string dokumentSleutel = string.Empty;
        private string DefaultRekening = string.Empty;

        private readonly string[] RekeningNummer = new string[10];
        private readonly string[] Uittreksel = new string[10];
        private readonly int[] RecNummer = new int[10];

        private string BekomenKorting = string.Empty;
        private string ToegestaneKorting = string.Empty;
                
        private string[,] xdaLinesDATAArray = new string[0, 0];

        private bool bCtrl;
        private int COUNT_TO;
        private int iOptelControle;

        private string sDatumAanmaak = string.Empty;
        private string sToepassingsCode = string.Empty;
        private string sNaamBestemmeling = string.Empty;

        private string sRekeningNummer = string.Empty;
        private string sUittreksel = string.Empty;
        private decimal cOudSaldo;
        private string sDatumOudSaldo = string.Empty;
        private string sNaamRekeninghouder = string.Empty;
        private string sOmschrijvingRekening = string.Empty;

        private string sRefFinInstelling = string.Empty;
        private string sRefFinInstelling2 = string.Empty;
        private string sDC = string.Empty;
        private decimal cBedrag;
        private string sVerrichting = string.Empty;
        private string sMededeling = string.Empty;
        private string sMDDZone1 = string.Empty;
        private string sMDDZone2 = string.Empty;
        private string sBoekDatum = string.Empty;
        private string sDagAfschriftVolgNummer = string.Empty;
        private string sValutadatum = string.Empty;

        private string sMededeling2 = string.Empty;
        private readonly string[] sRefKlant = new string[2];
        private string sMuntVerrichting = string.Empty;
        private decimal cBedragMunt;

        private string sRekeningTP = string.Empty;
        private string sITcodesTP = string.Empty;
        private string sRekeningTPextra = string.Empty;
        private readonly string[] sNaamEnAdres = new string[3];

        private string sUittreksel2 = string.Empty;
        private string sRekeningNummer2 = string.Empty;
        private decimal cNieuwSaldo;
        private string sDatumNieuwSaldo = string.Empty;

        private int iOptelCtrlCheckUp;
        private decimal cDebetSaldo;
        private decimal cCreditSaldo;

        public FormBanking()
        {
            InitializeComponent();
            WireHighlightEvents(this);
            Shown += FormBanking_Shown;
        }

        private void FormBanking_Load(object sender, EventArgs e)
        {
            xdaLineCounter = 0;
            mfgLijst.Columns[2].Width = 81;
            mfgLijst.Columns[5].Width = 292;

            if (mfgLijst.Rows.Count == 0)
                mfgLijst.Rows.Add();

            if (string.IsNullOrEmpty(LaadTekst("dnnInstellingen", "CodaIOMap")))
                BeWaarTekst("dnnInstellingen", "CodaIOMap", LOCATION_DESKTOP);

            if (PartRight(LOCATION_COMPANYDATA, 5) == "\\098\\" || PartRight(LOCATION_COMPANYDATA, 5) == "\\099\\")
                TextBoxWarningTestCompany.Visible = true;

            LabelInfo2.Text = " Document    TegenR.       Bedrag Omschrijving                    Fin. Kort.";

            DateTime dt;
            if (DateTime.TryParseExact(MIM_GLOBAL_DATE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                Datum.Value = dt;
            else
                Datum.Value = DateTime.Today;

            BeginBalans = (int)DoubleFromString(String99(64));
            ToegestaneKorting = String99(28);
            BekomenKorting = String99(27);
            DefaultRekening = String99(101);

            int[] recs = { 31, 32, 33, 34, 35, 38, 215, 216, 217, 218 };
            int[] rek = { 41, 42, 43, 44, 45, 39, 211, 212, 213, 214 };
            int selected = 0;

            ComboBoxSelectedBancAccount.Items.Clear();
            for (int t = 0; t < 10; t++)
            {
                RecNummer[t] = recs[t];
                RekeningNummer[t] = String99(rek[t]);
                Uittreksel[t] = (String99(recs[t]) ?? string.Empty).Trim();

                if (!string.IsNullOrWhiteSpace(RekeningNummer[t]))
                {
                    BGet(TABLE_LEDGERACCOUNTS, 0, RekeningNummer[t]);
                    string item;
                    if (Ktrl != 0)
                    {
                        item = RekeningNummer[t] + "|Niet aanwezig. Installeer via Setup Boekjaar.";
                    }
                    else
                    {
                        RecordToVeld(TABLE_LEDGERACCOUNTS);
                        item = VSet(RekeningNummer[t], 7) + "|" + (VBibText(TABLE_LEDGERACCOUNTS, "#v020 #") ?? string.Empty).TrimEnd();
                    }

                    ComboBoxSelectedBancAccount.Items.Add(item);
                    if (DefaultRekening == VSet(RekeningNummer[t], 7))
                        selected = ComboBoxSelectedBancAccount.Items.Count - 1;
                }
            }

            if (ComboBoxSelectedBancAccount.Items.Count > 0)
                ComboBoxSelectedBancAccount.SelectedIndex = selected < ComboBoxSelectedBancAccount.Items.Count ? selected : 0;

            // Set initial focus
            ActiveControl = ComboBoxSelectedBancAccount;
        }

        private void FormBanking_Shown(object sender, EventArgs e)
        {
            // Ensure ComboBoxSelectedBancAccount receives focus when the form is fully displayed
            ComboBoxSelectedBancAccount.Focus();
        }

        private string MfgTextMatrix(int row, int col)
        {
            if (row < 0 || row >= mfgLijst.Rows.Count || col < 0 || col >= mfgLijst.Columns.Count)
                return string.Empty;
            return mfgLijst.Rows[row].Cells[col].Value != null ? mfgLijst.Rows[row].Cells[col].Value.ToString() : string.Empty;
        }

        private void MfgSetTextMatrix(int row, int col, object value)
        {
            if (row < 0 || col < 0 || col >= mfgLijst.Columns.Count) return;
            while (mfgLijst.Rows.Count <= row)
                mfgLijst.Rows.Add();
            mfgLijst.Rows[row].Cells[col].Value = value;
        }

        private void MfgAddItem(string line)
        {
            var parts = (line ?? string.Empty).Split('\t');
            var cells = new object[7];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = i < parts.Length ? parts[i] : string.Empty;

            // If the only existing row is blank, reuse it instead of adding a new one
            if (mfgLijst.Rows.Count == 1)
            {
                bool isBlank = true;
                for (int i = 0; i < mfgLijst.Columns.Count; i++)
                {
                    if (mfgLijst.Rows[0].Cells[i].Value != null &&
                        !string.IsNullOrEmpty(mfgLijst.Rows[0].Cells[i].Value.ToString()))
                    {
                        isBlank = false;
                        break;
                    }
                }
                if (isBlank)
                {
                    for (int i = 0; i < cells.Length && i < mfgLijst.Columns.Count; i++)
                        mfgLijst.Rows[0].Cells[i].Value = cells[i];
                    return;
                }
            }

            mfgLijst.Rows.Add(cells);
        }

        private bool ListIsReady()
        {
            int countMissing = 0;
            for (int row = 0; row < mfgLijst.Rows.Count; row++)
            {
                if (MfgTextMatrix(row, 3) == "??????")
                    countMissing++;
            }

            if (countMissing == 0)
            {
                ButtonTransfer.Enabled = true;
                ButtonAssign.Visible = false;
                LabelCounter.Text = string.Empty;
                return true;
            }

            LabelCounter.Text = countMissing.ToString(CultureInfo.InvariantCulture);
            return false;
        }
        
        private void ComboBoxSelectedBancAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxSelectedBancAccount.SelectedIndex < 0) return;

            string locationDesktop = LaadTekstOLD("dnnInstellingen", "CodaIOMap");
            bool hasXda = System.IO.Directory.Exists(locationDesktop) && System.IO.Directory.GetFiles(locationDesktop, "*.xda").Length > 0;
            ButtonReadCamt053.Enabled = hasXda;
            CheckBoxSepaViewer.Visible = hasXda;

            int idx = Math.Min(ComboBoxSelectedBancAccount.SelectedIndex, Uittreksel.Length - 1);
            string u = Uittreksel[idx] ?? string.Empty;

            if (u.Length > 0 && char.IsLetter(u[0]))
                LabelInfo11.Text = (DoubleFromString(PartRight(u, 4)) + 1).ToString(CultureInfo.InvariantCulture);
            else
                LabelInfo11.Text = (DoubleFromString(u) + 1).ToString(CultureInfo.InvariantCulture);

            string key = PartLeft(ComboBoxSelectedBancAccount.Text, 7);
            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(key, 7));
            if (Ktrl != 0) return;

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            int boekjaar = 0;
            if (Application.OpenForms["FormBYPERDAT"] is Form byperdat && byperdat.Controls["Boekjaar"] is ComboBox cb)
                boekjaar = Math.Max(0, cb.SelectedIndex);

            if (BeginBalans == 1)
            {
                if (bhEuro)
                {
                    lblInfo0.Text = DoubleFromString(VBibText(TABLE_LEDGERACCOUNTS, "#e" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #")).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    LabelInfo12.Text = (DoubleFromString(lblInfo0.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    LabelInfo12.Text = DoubleFromString(VBibText(TABLE_LEDGERACCOUNTS, "#v" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #")).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    lblInfo0.Text = (DoubleFromString(LabelInfo12.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (bhEuro)
                {
                    double begin = DoubleFromString(VBibText(TABLE_LEDGERACCOUNTS, "#e" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    double move = DoubleFromString(VBibText(TABLE_LEDGERACCOUNTS, "#e" + (23 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    lblInfo0.Text = (begin + move).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    LabelInfo12.Text = Math.Round(DoubleFromString(lblInfo0.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    double begin = DoubleFromString(VBibText(TABLE_LEDGERACCOUNTS, "#v" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    double move = DoubleFromString(VBibText(TABLE_LEDGERACCOUNTS, "#v" + (23 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    LabelInfo12.Text = (begin + move).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    lblInfo0.Text = (DoubleFromString(LabelInfo12.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
            }

            LabelInfo13.Text = LabelInfo12.Text;
            lblInfo1.Text = lblInfo0.Text;
            ApplySaldoColors();
        }

        private void ApplySaldoColors()
        {
            double begin = DoubleFromString(LabelInfo12.Text);
            if (begin == 0)
            {
                LabelInfo12.BackColor = System.Drawing.Color.Silver;
                lblInfo0.BackColor = System.Drawing.Color.Silver;
            }
            else if (begin > 0)
            {
                LabelInfo12.BackColor = System.Drawing.Color.LightGreen;
                lblInfo0.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                LabelInfo12.BackColor = System.Drawing.Color.LightCyan;
                lblInfo0.BackColor = System.Drawing.Color.LightCyan;
            }

            double end = DoubleFromString(LabelInfo13.Text);
            if (end == 0)
            {
                LabelInfo13.BackColor = System.Drawing.Color.Silver;
                lblInfo1.BackColor = System.Drawing.Color.Silver;
            }
            else if (end > 0)
            {
                LabelInfo13.BackColor = System.Drawing.Color.LightGreen;
                lblInfo1.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                LabelInfo13.BackColor = System.Drawing.Color.LightCyan;
                lblInfo1.BackColor = System.Drawing.Color.LightCyan;
            }
        }

        private void ComboBoxSelectedBankAccount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ComboBoxSelectedBancAccount.Text))
            {
                System.Media.SystemSounds.Beep.Play();
                ComboBoxSelectedBancAccount.Focus();
            }
        }

        private void Datum_Leave(object sender, EventArgs e)
        {
            if (!DateCheck(Datum.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), PERIODAS_TEXT))
            {
                if (Application.OpenForms["FormBYPERDAT"] is Form byperdat)
                {
                    byperdat.WindowState = FormWindowState.Normal;
                    byperdat.Focus();
                }
            }
        }
        
        private void ButtonManual_Click(object sender, EventArgs e)
        {
            using (var detail = new FormDetailInfo())
            {
                GridText = string.Empty;
                detail.ShowDialog(this);
            }

            if (string.IsNullOrEmpty(GridText))
                return;

            ComboBoxSelectedBancAccount.Enabled = false;
            ListBoxFinancialDetail.Items.Add(GridText);

            double bedrag = DoubleFromString(PartMid(GridText, 22, 12));
            bool ontvangst = PartLeft(GridText, 1) == "+";

            if (bhEuro)
            {
                double huidig = DoubleFromString(lblInfo1.Text);
                lblInfo1.Text = (ontvangst ? huidig + bedrag : huidig - bedrag).ToString("#,##0.00", CultureInfo.InvariantCulture);
                LabelInfo13.Text = Math.Round(DoubleFromString(lblInfo1.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                double huidig = DoubleFromString(LabelInfo13.Text);
                LabelInfo13.Text = (ontvangst ? huidig + bedrag : huidig - bedrag).ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblInfo1.Text = (DoubleFromString(LabelInfo13.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }

            ApplySaldoColors();
        }

        private void ListBoxFinancialDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert || e.KeyCode == Keys.Add)
                ButtonManual_Click(sender, e);
        }

        private void ButtonBookIt_Click(object sender, EventArgs e)
        {
            // Validate date against period
            if (!DateCheck(Datum.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), PERIODAS_TEXT))
            {
                System.Media.SystemSounds.Beep.Play();
                Datum.Focus();
                return;
            }

            // Check if there are transactions
            if (ListBoxFinancialDetail.Items.Count == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Verrichtingen inbrengen a.u.b. !!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get ledger account information
            BGet(TABLE_LEDGERACCOUNTS, 0, PartLeft(ComboBoxSelectedBancAccount.Text, 7));
            if (Ktrl != 0)
            {
                MessageBox.Show("onlogische situatie", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);

            // Check for existing statements with higher dates
            string accountPrefix = PartLeft(VBibText(TABLE_LEDGERACCOUNTS, "#v020 #"), 2).ToUpperInvariant();
            string yearSuffix = PartRight(Datum.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), 2);
            int statementNumber = (int)DoubleFromString(LabelInfo11.Text) - 1;
            string previousStatementKey = accountPrefix + yearSuffix + statementNumber.ToString("0000", CultureInfo.InvariantCulture);

            BGet(TABLE_JOURNAL, 2, previousStatementKey);
            if (Ktrl != 0)
            {
                MessageBox.Show("Dit zou het eerste uittreksel binnen het WERKELIJK jaar moeten zijn...  Kontroleer eventueel",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                RecordToVeld(TABLE_JOURNAL);
                string lastStatementDate = VBibText(TABLE_JOURNAL, "#v066 #");
                string currentDate = Datum.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

                if (string.Compare(lastStatementDate, currentDate, StringComparison.Ordinal) > 0)
                {
                    Msg = "Er zijn reeds uittreksels met een hogere datum !" + Environment.NewLine + Environment.NewLine;
                    Msg += "Laatste uittreksel nr. " + previousStatementKey + " dateert van : " + DateText(lastStatementDate) + Environment.NewLine + Environment.NewLine;
                    Msg += "Vervolg.  Bent U zeker ?";

                    DialogResult result = MessageBox.Show(Msg, "Uittreksel afsluiten", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.No)
                        return;
                }
            }

            // Final confirmation message
            string currentStatementKey = accountPrefix + yearSuffix + ((int)DoubleFromString(LabelInfo11.Text)).ToString("0000", CultureInfo.InvariantCulture);

            if (bhEuro)
            {
                Msg = "Datum uittreksel " + Datum.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) +
                      " en bekomen eindsaldo EUR " + lblInfo1.Text + Environment.NewLine + Environment.NewLine +
                      "Hierna wordt de boekhouding bijgewerkt.  Bent U zeker ?";
            }
            else
            {
                Msg = "Datum uittreksel " + Datum.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) +
                      " en bekomen eindsaldo BEF " + LabelInfo13.Text + Environment.NewLine + Environment.NewLine +
                      "Hierna wordt de boekhouding bijgewerkt.  Bent U zeker ?";
            }

            DialogResult confirmResult = MessageBox.Show(Msg, "Uittreksel : " + currentStatementKey,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                BBegin();

                using (var boekingForm = new SharedForms.FormBoeking())
                {
                    if (WegBoekFout(boekingForm))
                    {
                        BAbort();
                        return;
                    }

                    BEnd();

                    // Update counter for statement number
                    string dummySleutel = "s" + RecNummer[ComboBoxSelectedBancAccount.SelectedIndex].ToString("000", CultureInfo.InvariantCulture);
                    BGet(TABLE_COUNTERS, 0, dummySleutel);

                    if (Ktrl != 0)
                    {
                        MessageBox.Show("TellerStop " + dummySleutel + ".  kontakteer R&Vsoft", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        RecordToVeld(TABLE_COUNTERS);
                        FL99_RECORD = ((int)DoubleFromString(LabelInfo11.Text)).ToString(CultureInfo.InvariantCulture);

                        if (BAModus == 1)
                            VBib(TABLE_COUNTERS, FL99_RECORD, "v217 ");
                        else
                            VBib(TABLE_COUNTERS, FL99_RECORD, dummySleutel);

                        BUpdate(TABLE_COUNTERS, 0);

                        if (Ktrl != 0)
                        {
                            MessageBox.Show("Update TellerStop " + dummySleutel + ".  contacteer Vsoft 1985", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Update account last used
                    dummySleutel = "s101";
                    BGet(TABLE_COUNTERS, 0, dummySleutel);

                    if (Ktrl != 0)
                    {
                        MessageBox.Show("TellerStop.  Versieconflict !  Contacteer Vsoft 1985", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        RecordToVeld(TABLE_COUNTERS);
                        FL99_RECORD = PartLeft(ComboBoxSelectedBancAccount.Text, 7);

                        if (BAModus == 1)
                            VBib(TABLE_COUNTERS, FL99_RECORD, "v217 ");
                        else
                            VBib(TABLE_COUNTERS, FL99_RECORD, dummySleutel);

                        BUpdate(TABLE_COUNTERS, 0);

                        if (Ktrl != 0)
                        {
                            MessageBox.Show("UpdateStop Teller. contacteer Vsoft 1985", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    BClose(TABLE_COUNTERS);
                    GridText = string.Empty;
                    Close();
                }
            }
        }

        private void Struktuur_Click(object sender, EventArgs e)
        {
            string referteTxt;
            string dummyText = string.Empty;
            double bedragTekst;
            double bedragKtrl = 0d;
            string defaultKlanten = VSet(String99(9), 7);

            SnelHelpPrint(" ", BL_LOGGING);

            string msg = "Breng mededeling in" + Environment.NewLine
                + "met masker nnnnnnnnnnnn" + Environment.NewLine + Environment.NewLine
                + "Waarbij n staat voor elk" + Environment.NewLine
                + "van de 12 verplichte cijfers" + Environment.NewLine + Environment.NewLine;

            referteTxt = Microsoft.VisualBasic.Interaction.InputBox(msg, "Gestruktureerde betaling");
            if (string.IsNullOrWhiteSpace(referteTxt))
                return;

            string referteDigits = new string(referteTxt.Where(char.IsDigit).ToArray());
            if (referteDigits.Length != 12)
            {
                MessageBox.Show("Ongeldige invoer", "Gebruikersfout", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            double controleBron = DoubleFromString(PartLeft(referteDigits, 10));
            string sPip = ((int)(controleBron - Math.Floor(controleBron / 97d) * 97d)).ToString("00", CultureInfo.InvariantCulture);
            if (sPip == "00") sPip = "97";

            string referteControle = PartRight(referteDigits, 2);
            if (!string.Equals(sPip, referteControle, StringComparison.Ordinal))
            {
                MessageBox.Show(
                    "Ongeldige invoer" + Environment.NewLine + Environment.NewLine
                    + sPip + " <> " + referteControle + Environment.NewLine + Environment.NewLine
                    + "Een gestructureerde referte heeft een kontrolesysteem.  Uw invoer is ongeldig!",
                    "Gebruikersfout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (PartMid(referteDigits, 8, 1) != "0")
            {
                MessageBox.Show("Geen R&V Gestruktureerde mededeling.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BGet(TABLE_CUSTOMERS, 0, PartMid(referteDigits, 4, 4) + PartMid(referteDigits, 9, 2));
            if (Ktrl != 0)
                return;

            RecordToVeld(TABLE_CUSTOMERS);

            msg = "Breng bedrag in voor" + Environment.NewLine
                + "totaal van " + PartLeft(referteDigits, 1) + " kwijtingen" + Environment.NewLine + Environment.NewLine
                + "klant :" + Environment.NewLine + Environment.NewLine
                + (VBibText(TABLE_CUSTOMERS, "#A100 #") + " " + VBibText(TABLE_CUSTOMERS, "#A101 #")).TrimEnd()
                + " "
                + (VBibText(TABLE_CUSTOMERS, "#A125 #") + " " + VBibText(TABLE_CUSTOMERS, "#A127 #")).TrimEnd()
                + Environment.NewLine
                + "Rekeningen:"
                + VBibText(TABLE_CUSTOMERS, "#A170 #") + " " + VBibText(TABLE_CUSTOMERS, "#v251 #");

            string bedragInput = Microsoft.VisualBasic.Interaction.InputBox(msg, "Totaal betaling");
            bedragTekst = DoubleFromString(bedragInput);
            if (bedragTekst == 0d)
                return;

            BGetOrGreater(TABLE_INVOICES, 1, "K" + VBibText(TABLE_CUSTOMERS, "#A110 #"));
            if (Ktrl != 0)
                return;

            RecordToVeld(TABLE_INVOICES);

            string invoicePrefix = VSet("K" + VBibText(TABLE_CUSTOMERS, "#A110 #"), 13);
            if (!string.Equals(VSet(KEY_BUF[TABLE_INVOICES], 13), invoicePrefix, StringComparison.Ordinal))
                return;

            void LijnErBij()
            {
                if (Math.Abs(bedragKtrl - bedragTekst) < 0.0001)
                    return;

                double betaald = DoubleFromString(VBibText(TABLE_INVOICES, "#v037 #"));
                double totaal = DoubleFromString(VBibText(TABLE_INVOICES, "#v249 #"));
                if (Math.Abs(betaald - totaal) < 0.0001)
                    return;

                if (PartLeft(VBibText(TABLE_INVOICES, "#v033 #"), 1) != "Q")
                    return;

                if (Math.Abs(bedragTekst - totaal) >= 0.0001)
                    return;

                double openstaand = totaal - betaald;
                dummyText += VBibText(TABLE_INVOICES, "#v033 #") + "|";
                dummyText += Dec(openstaand, MASK_EURBH) + Environment.NewLine;
                bedragKtrl += openstaand;
            }

            LijnErBij();

            while (true)
            {
                BNext(TABLE_INVOICES);
                if (Ktrl != 0 || !string.Equals(VSet(KEY_BUF[TABLE_INVOICES], 13), invoicePrefix, StringComparison.Ordinal))
                    break;

                RecordToVeld(TABLE_INVOICES);
                LijnErBij();
            }

            if (Math.Abs(bedragKtrl - bedragTekst) >= 0.0001)
            {
                MessageBox.Show("Opzoeking zonder succes, dokumentenstand : " + Environment.NewLine + Environment.NewLine + dummyText);
                return;
            }

            while (!string.IsNullOrEmpty(dummyText))
            {
                if (dummyText.Length < 26)
                    break;

                string lijn = PartLeft(dummyText, 26);
                string bedragLijn = PartMid(lijn, 13, 12);

                if (bedragLijn != new string(' ', 12))
                {
                    GridText = "+" + PartLeft(lijn, 11)
                        + "|" + defaultKlanten
                        + "|" + bedragLijn
                        + "|" + VSet(VBibText(TABLE_CUSTOMERS, "#A100 #"), 29)
                        + "|" + new string(' ', 12);

                    ListBoxFinancialDetail.Items.Add(GridText);

                    if (bhEuro)
                    {
                        double nieuwSaldo = DoubleFromString(lblInfo1.Text) + DoubleFromString(PartMid(GridText, 22, 12));
                        lblInfo1.Text = nieuwSaldo.ToString("#,##0.00", CultureInfo.InvariantCulture);
                        LabelInfo13.Text = Math.Round(DoubleFromString(lblInfo1.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        double nieuwSaldo = DoubleFromString(LabelInfo13.Text) + DoubleFromString(PartMid(GridText, 22, 12));
                        LabelInfo13.Text = nieuwSaldo.ToString("#,##0.00", CultureInfo.InvariantCulture);
                        lblInfo1.Text = (DoubleFromString(LabelInfo13.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    }
                }

                dummyText = PartRight(dummyText, dummyText.Length - 26);
            }

            ApplySaldoColors();
            SnelHelpPrint(GridText + " met succes bijgevoegd !", BL_LOGGING);
        }

        private void ButtonReadCamt053_Click(object sender, EventArgs e)
        {
            string SafeLineValue(int row, int col)
            {
                if (row < 0 || col < 0 || row >= xdaLinesDATAArray.GetLength(0) || col >= xdaLinesDATAArray.GetLength(1))
                    return string.Empty;
                return xdaLinesDATAArray[row, col] ?? string.Empty;
            }

            string FormatVal(string value)
            {
                return DoubleFromString(value).ToString(CultureInfo.InvariantCulture);
            }

            string xdaLocation = (LaadTekstOLD("dnnInstellingen", "CodaIOMap") ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrEmpty(xdaLocation))
                xdaLocation = (LOCATION_DESKTOP ?? string.Empty).ToLowerInvariant();

            string filePath;
            string selectedFileTitle;

            using (var ofd = new OpenFileDialog())
            {
                ofd.FileName = string.Empty;
                ofd.InitialDirectory = xdaLocation;
                ofd.Filter = "Alle bestanden (*.xda)|*.xda";
                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                filePath = (ofd.FileName ?? string.Empty).ToLowerInvariant();
                selectedFileTitle = ofd.SafeFileName ?? string.Empty;

                string selectedDir = (System.IO.Path.GetDirectoryName(ofd.FileName) ?? string.Empty).ToLowerInvariant();
                if (!string.IsNullOrEmpty(selectedDir) && !selectedDir.EndsWith("\\", StringComparison.Ordinal))
                    selectedDir += "\\";

                string baseDir = xdaLocation;
                if (!string.IsNullOrEmpty(baseDir) && !baseDir.EndsWith("\\", StringComparison.Ordinal))
                    baseDir += "\\";

                if (!string.Equals(baseDir, selectedDir, StringComparison.OrdinalIgnoreCase))
                {
                    if (selectedDir.IndexOf("coda\\in", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        MessageBox.Show("Inladen van verwerkte documenten is verboden", string.Empty,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string msg = ".XDA en .XML voor SEPA betaalbestanden locatie staat ingesteld op:" + Environment.NewLine
                        + baseDir + Environment.NewLine + Environment.NewLine
                        + "Mag de standaard locatie vanaf nu gewijzigd worden naar:" + Environment.NewLine
                        + selectedDir;

                    var answer = MessageBox.Show(msg, "Uittreksel afsluiten",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (answer == DialogResult.Yes)
                    {
                        BeWaarTekst("dnnInstellingen", "CodaIOMap", selectedDir);
                        MessageBox.Show("Herstart het inladen van het document", string.Empty,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Probeer opnieuw", string.Empty,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return;
                }
            }

            bool actionResult = ReadCamt053XDA(filePath, CheckBoxSepaViewer.Checked);
            if (!actionResult)
                return;

            xdaOMS = xdaOMS ?? string.Empty;
            xdaDATA = xdaDATA ?? string.Empty;
            xdaLinesOMS = xdaLinesOMS ?? string.Empty;
            xdaLinesDATA = xdaLinesDATA ?? string.Empty;

            xdaOMSArray = xdaOMS.Split('\t');
            xdaDATAArray = xdaDATA.Split('\t');
            xdaLinesOMSArray = xdaLinesOMS.Split('\t');
            xdaParseToArray(xdaLinesDATA);

            int qualityOfBeginning = 0;
            double beginSaldoXDA = xdaDATAArray.Length > 6 ? DoubleFromString(xdaDATAArray[6]) : 0d;
            double cumulSaldo;

            if (xdaDATAArray.Length > 3 && !string.IsNullOrWhiteSpace(xdaDATAArray[3]))
            {
                if (DoubleFromString(LabelInfo11.Text) == DoubleFromString(xdaDATAArray[3]))
                    qualityOfBeginning++;
            }

            if (beginSaldoXDA == DoubleFromString(lblInfo0.Text))
                qualityOfBeginning++;

            bool addOnlyTransactionLines = false;
            if (qualityOfBeginning == 0)
            {
                if (mfgLijst.Rows.Count == 2)
                {
                }
                else if (beginSaldoXDA == DoubleFromString(MfgTextMatrix(mfgLijst.Rows.Count - 2, 6)))
                {
                    cumulSaldo = beginSaldoXDA;
                    addOnlyTransactionLines = true;
                }
                else
                {
                    string msg = "Noch teller (niet essentieel), noch beginsaldo (essentieel)" + Environment.NewLine
                        + "Bij beheer van meerdere rekeningen, duidt eerst de rekening aan voor .XDA import en probeer opnieuw";
                    MessageBox.Show(msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (qualityOfBeginning == 1)
            {
                SnelHelpPrint("Beginsaldo ok", BL_LOGGING);
            }
            else
            {
                SnelHelpPrint("Volgnummer + beginsaldo ok", BL_LOGGING);
            }

            if (!addOnlyTransactionLines && mfgLijst.Rows.Count == 1)
            {
                string startLine = "\t\t\t\t\tBeginsaldo uittreksel\t" + beginSaldoXDA.ToString(CultureInfo.InvariantCulture);
                MfgAddItem(startLine);
            }

            cumulSaldo = beginSaldoXDA;
            SnelHelpPrint("Preparing book list with xdaLinesData", BL_LOGGING);

            int rowCount = xdaLinesDATAArray.GetLength(0);
            for (int t = 0; t < rowCount - 1; t++)
            {
                xdaLineCounter++;

                string lineCode = SafeLineValue(t, 2).Trim();
                string amountToCheck;
                double bedragTransactie;

                if (string.IsNullOrWhiteSpace(SafeLineValue(t, 4)))
                {
                    bedragTransactie = DoubleFromString(SafeLineValue(t, 1));
                    amountToCheck = SafeLineValue(t, 1);
                }
                else
                {
                    bedragTransactie = DoubleFromString(SafeLineValue(t, 4));
                    amountToCheck = SafeLineValue(t, 4);
                }

                string aa = Dec(xdaLineCounter, "000") + "\t" + lineCode + "\t";
                string lineReference;
                string resultReturn;
                string[] resultArray;
                bool isCor;

                switch (lineCode)
                {
                    case "0101000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t\t";
                        break;

                    case "0103000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (SafeLineValue(t, 9).Length == 12)
                        {
                            isCor = true;
                            lineReference = SafeLineValue(t, 9);
                        }
                        else
                        {
                            isCor = false;
                            if (SafeLineValue(t, 10).Length == 0)
                            {
                                MessageBox.Show("Empty Ustrd, Logic?", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lineReference = SafeLineValue(t, 5);
                            }
                            else
                            {
                                lineReference = SafeLineValue(t, 10);
                            }
                        }

                        resultReturn = CtrlDocuments(true, isCor, false, lineReference, SafeLineValue(t, 7), amountToCheck);
                        if (string.IsNullOrEmpty(resultReturn))
                        {
                            aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + lineReference + "\t";
                        }
                        else
                        {
                            resultArray = resultReturn.Split('|');
                            aa += "+" + (resultArray.Length > 0 ? resultArray[0] : string.Empty) + "\t"
                                + String99(9) + "\t"
                                + FormatVal(amountToCheck) + "\t"
                                + (resultArray.Length > 1 ? resultArray[1] : string.Empty) + "\t";
                        }
                        break;

                    case "0107000":
                        bedragTransactie = -bedragTransactie;
                        if (SafeLineValue(t, 9).Length == 12)
                        {
                            isCor = true;
                            lineReference = SafeLineValue(t, 9);
                        }
                        else
                        {
                            isCor = false;
                            if (SafeLineValue(t, 10).Length == 0)
                            {
                                MessageBox.Show("Empty Ustrd, Logic?", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lineReference = "-";
                            }
                            else
                            {
                                lineReference = SafeLineValue(t, 10);
                            }
                        }

                        resultReturn = CtrlDocuments(true, isCor, false, lineReference, SafeLineValue(t, 7), amountToCheck);
                        if (string.IsNullOrEmpty(resultReturn))
                        {
                            MessageBox.Show("Geen resultaat", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(5, 1) + "\t";
                        }
                        else
                        {
                            resultArray = resultReturn.Split('|');
                            aa += "-" + (resultArray.Length > 0 ? resultArray[0] : string.Empty) + "\t"
                                + String99(10) + "\t"
                                + FormatVal(amountToCheck) + "\t"
                                + (resultArray.Length > 1 ? resultArray[1] : string.Empty) + "\t";
                        }
                        break;

                    case "0201000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t\t";
                        break;

                    case "0402000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 10) + "\t";
                        break;

                    case "0404000":
                        MessageBox.Show("Cash Afname", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bedragTransactie = -bedragTransactie;
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 10) + "\t";
                        break;

                    case "0501000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 5) + "\t";
                        break;

                    case "0503000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 6) + "\t";
                        break;

                    case "8022000":
                        bedragTransactie = -bedragTransactie;
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "-\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 10) + "\t";
                        break;

                    case "0150000":
                    case "0250000":
                        MessageBox.Show("stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (SafeLineValue(t, 9).Length == 12)
                        {
                            isCor = true;
                            lineReference = SafeLineValue(t, 9);
                        }
                        else
                        {
                            isCor = false;
                            if (SafeLineValue(t, 10).Length == 0)
                            {
                                MessageBox.Show("Empty Ustrd, Logic?", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lineReference = SafeLineValue(t, 6);
                            }
                            else
                            {
                                lineReference = SafeLineValue(t, 10);
                            }
                        }

                        if (lineReference.Length > 25)
                        {
                            aa += "+\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 6) + "\t";
                        }
                        else
                        {
                            resultReturn = CtrlDocuments(false, isCor, false, lineReference, SafeLineValue(t, 7), amountToCheck);
                            if (string.IsNullOrEmpty(resultReturn))
                            {
                                MessageBox.Show("Geen resultaat", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                aa += "+\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 6) + "\t";
                            }
                            else
                            {
                                resultArray = resultReturn.Split('|');
                                aa += "+" + (resultArray.Length > 0 ? resultArray[0] : string.Empty) + "\t"
                                    + String99(9) + "\t"
                                    + FormatVal(amountToCheck) + "\t"
                                    + (resultArray.Length > 1 ? resultArray[1] : string.Empty) + "\t";
                            }
                        }
                        break;

                    case "0254000":
                        MessageBox.Show("Stop", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        aa += "+\t??????\t" + FormatVal(amountToCheck) + "\t\t";
                        break;

                    case "0550000":
                        MessageBox.Show("Stop VSOFT CDD", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (SafeLineValue(t, 9).Length == 12)
                        {
                            isCor = true;
                            lineReference = SafeLineValue(t, 9);
                        }
                        else
                        {
                            isCor = false;
                            if (SafeLineValue(t, 10).Length == 0)
                            {
                                MessageBox.Show("Empty Ustrd, Logic?", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lineReference = "-";
                            }
                            else
                            {
                                lineReference = SafeLineValue(t, 10);
                            }
                        }

                        if (lineReference.Length > 25)
                        {
                            aa += "+\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 6) + "\t";
                        }
                        else
                        {
                            resultReturn = CtrlDocuments(false, isCor, true, lineReference, SafeLineValue(t, 7), amountToCheck);
                            if (string.IsNullOrEmpty(resultReturn))
                            {
                                MessageBox.Show("Geen resultaat", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                aa += "+\t??????\t" + FormatVal(amountToCheck) + "\t" + SafeLineValue(t, 6) + "\t";
                            }
                            else
                            {
                                resultArray = resultReturn.Split('|');
                                aa += "+" + (resultArray.Length > 0 ? resultArray[0] : string.Empty) + "\t"
                                    + String99(9) + "\t"
                                    + FormatVal(amountToCheck) + "\t"
                                    + (resultArray.Length > 1 ? resultArray[1] : string.Empty) + "\t";
                            }
                        }
                        break;

                    default:
                        MessageBox.Show("BBA buiten controle: " + lineCode, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                cumulSaldo += bedragTransactie;
                aa += cumulSaldo.ToString(CultureInfo.InvariantCulture);
                MfgAddItem(aa);
            }

            MessageBox.Show("TODO: give chance to edit for lines not 100% sure", string.Empty,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("TODO: book or [Esc]!", string.Empty,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ListIsReady();

            // VB6 flow exits here before moving/deleting source file.
            // Keep same behavior for now.
            _ = selectedFileTitle;
        }

        private void ButtonAssign_Click(object sender, EventArgs e)
        {
            if (mfgLijst.CurrentRow == null) return;

            using (var detail = new FormDetailInfo())
            {
                GridText = string.Empty;
                detail.ShowDialog(this);
            }

            if (string.IsNullOrEmpty(GridText))
                return;

            var result = GridText.Split('|');
            if (result.Length < 4) return;

            int row = mfgLijst.CurrentRow.Index;
            MfgSetTextMatrix(row, 2, result[0]);
            MfgSetTextMatrix(row, 3, result[1]);
            MfgSetTextMatrix(row, 4, DoubleFromString(result[2]).ToString(CultureInfo.InvariantCulture));
            MfgSetTextMatrix(row, 5, result[3]);
            ListIsReady();
        }

        private void ButtonTransfer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Overnemen", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool WegBoekFout(FormBoeking boekingForm)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                DKTRL_CUMUL = 0;
                DKTRL_BEF = 0;
                DKTRL_EUR = 0;
                JournaalLocked = false;

                boekingForm.Hide();

                TLB_RECORD[TABLE_JOURNAL] = string.Empty;

                string rekening = PartLeft(ComboBoxSelectedBancAccount.Text ?? string.Empty, 7);
                string datumSleutel = Datum.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

                VBib(TABLE_JOURNAL, rekening, "v019");
                VBib(TABLE_JOURNAL, datumSleutel, "v066");
                VBib(TABLE_JOURNAL, datumSleutel, "v035");

                BGet(TABLE_LEDGERACCOUNTS, 0, rekening);
                if (Ktrl != 0)
                    return true;

                RecordToVeld(TABLE_LEDGERACCOUNTS);
                string rekeningNaam = (VBibText(TABLE_LEDGERACCOUNTS, "#v020 #") ?? string.Empty).ToUpperInvariant();
                dokumentSleutel = PartLeft(rekeningNaam, 2)
                    + Datum.Value.ToString("yy", CultureInfo.InvariantCulture)
                    + ((int)DoubleFromString(LabelInfo11.Text)).ToString("0000", CultureInfo.InvariantCulture);

                VBib(TABLE_JOURNAL, dokumentSleutel, "v038");

                if (bhEuro)
                {
                    double beginSaldo = DoubleFromString(lblInfo0.Text);
                    double eindSaldo = DoubleFromString(lblInfo1.Text);
                    double verschilSaldo = eindSaldo - beginSaldo;

                    VBib(TABLE_JOURNAL,
                        "Sld:" + Dec(beginSaldo, MASK_EURBH) + "/" + Dec(eindSaldo, MASK_EURBH),
                        "v067");
                    VBib(TABLE_JOURNAL, Dec(verschilSaldo, MASK_SY[2]), "v068");
                }
                else
                {
                    double beginSaldo = DoubleFromString(LabelInfo12.Text);
                    double eindSaldo = DoubleFromString(LabelInfo13.Text);

                    VBib(TABLE_JOURNAL,
                        "Sld : " + Dec(beginSaldo, MASK_SY[0]) + " - " + Dec(eindSaldo, MASK_SY[0]),
                        "v067");
                    VBib(TABLE_JOURNAL, Dec(eindSaldo - beginSaldo, MASK_SY[0]), "v068");
                }

                BInsert(TABLE_JOURNAL, 2, boekingForm);
                if (Ktrl != 0)
                    return true;

                VBib(TABLE_JOURNAL, rekening, "v069");

                for (int t = 0; t < ListBoxFinancialDetail.Items.Count; t++)
                {
                    string detailLijn = Convert.ToString(ListBoxFinancialDetail.Items[t]) ?? string.Empty;
                    bool ontvangst = PartLeft(detailLijn, 1) == "+";

                    string docSleutel = PartMid(detailLijn, 2, 11);
                    if (docSleutel == new string(' ', 11))
                    {
                        VBib(TABLE_JOURNAL, " ", "v033");
                        VBib(TABLE_JOURNAL, " ", "v034");
                    }
                    else
                    {
                        BGet(TABLE_INVOICES, 0, docSleutel);
                        if (Ktrl != 0)
                            return true;

                        RecordToVeld(TABLE_INVOICES);
                        VBib(TABLE_JOURNAL, VBibText(TABLE_INVOICES, "#v033 #"), "v033");
                        VBib(TABLE_JOURNAL, VBibText(TABLE_INVOICES, "#v034 #"), "v034");
                    }

                    double totaalBedrag = DoubleFromString(PartMid(detailLijn, 22, 12));
                    VBib(TABLE_JOURNAL, PartMid(detailLijn, 35, 29).Trim(), "v067");

                    double finKort = DoubleFromString(PartMid(detailLijn, 65, 12));
                    if (finKort != 0)
                    {
                        totaalBedrag += finKort;
                        if (ontvangst)
                        {
                            VBib(TABLE_JOURNAL, finKort.ToString(CultureInfo.InvariantCulture), "v068");
                            VBib(TABLE_JOURNAL, ToegestaneKorting, "v019");
                        }
                        else
                        {
                            VBib(TABLE_JOURNAL, (-finKort).ToString(CultureInfo.InvariantCulture), "v068");
                            VBib(TABLE_JOURNAL, BekomenKorting, "v019");
                        }

                        BInsert(TABLE_JOURNAL, 2, boekingForm);
                        if (Ktrl != 0)
                            return true;
                    }

                    VBib(TABLE_JOURNAL, PartMid(detailLijn, 14, 7), "v019");
                    VBib(TABLE_JOURNAL,
                        ontvangst
                            ? (-totaalBedrag).ToString(CultureInfo.InvariantCulture)
                            : totaalBedrag.ToString(CultureInfo.InvariantCulture),
                        "v068");

                    BInsert(TABLE_JOURNAL, 2, boekingForm);
                    if (Ktrl != 0)
                        return true;

                    if (!string.IsNullOrWhiteSpace(VBibText(TABLE_JOURNAL, "#v033 #")))
                    {
                        double reedsBetaald = DoubleFromString(VBibText(TABLE_INVOICES, "#v037 #"));
                        double updateBedrag = XisEuroWisBEF ? Math.Round(totaalBedrag / EURO, 2) : totaalBedrag;

                        VBib(TABLE_INVOICES, (reedsBetaald + updateBedrag).ToString(CultureInfo.InvariantCulture), "v037");
                        VBib(TABLE_INVOICES, VBibText(TABLE_JOURNAL, "#v038 #"), "v038");
                        BUpdate(TABLE_INVOICES, 0);
                        if (Ktrl != 0)
                            return true;
                    }
                }

                DKTRL_CUMUL = DoubleFromString(Dec(DKTRL_CUMUL, MASK_EURBH));
                DKTRL_EUR = DoubleFromString(Dec(DKTRL_EUR, MASK_EURBH));
                DKTRL_BEF = DoubleFromString(Dec(DKTRL_BEF, MASK_BEF));

                if (DKTRL_CUMUL != 0)
                {
                    SetControlEnabledIfPresent(boekingForm, "cmdBoeken", false);
                    MessageBox.Show(
                        "LogikaFout bij vierkantskontrole journaal." + Environment.NewLine + Environment.NewLine
                        + "Deze verrichting wordt geannuleerd.  Controleer zelf eerst en/of raadpleeg ons.",
                        string.Empty,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    boekingForm.ShowDialog(this);
                    return true;
                }

                if (JournaalLocked)
                {
                    SetControlEnabledIfPresent(boekingForm, "cmdBoeken", false);
                    boekingForm.ShowDialog(this);
                    return true;
                }

                switch (Mim != null ? Mim.GetWegBoekModus() : "0")
                {
                    case "0":
                        break;
                    case "1":
                        if (DKTRL_BEF != 0 || DKTRL_EUR != 0)
                            boekingForm.ShowDialog(this);
                        break;
                    case "2":
                        boekingForm.ShowDialog(this);
                        break;
                    default:
                        MessageBox.Show("situatie...", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }

                return DKTRL_CUMUL != 0;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private static void SetControlEnabledIfPresent(Form form, string controlName, bool enabled)
        {
            Control[] matches = form.Controls.Find(controlName, true);
            if (matches.Length > 0)
            {
                matches[0].Enabled = enabled;
            }
        }

        private void mfgLijst_SelectionChanged(object sender, EventArgs e)
        {
            if (mfgLijst.CurrentRow == null)
                return;

            bool needsAssign = MfgTextMatrix(mfgLijst.CurrentRow.Index, 3) == "??????";
            ButtonAssign.Enabled = needsAssign;
            ButtonAssign.Visible = needsAssign;
            if (needsAssign)
                ButtonAssign.Focus();
        }

        private void SSTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SSTab1.SelectedIndex == 0)
            {
                // TODO : Disable the "Volgende" later when importing is 100% checked and ready to book.
                // For now, keep it enabled so manual input is still allowed.
                ButtonManual.Enabled = false;
            }
            else if (SSTab1.SelectedIndex == 1)
            {
                ButtonManual.Enabled = true;
                ButtonManual.Focus();
            }
        }

        private string CtrlDocuments(bool isSeller, bool refIsCor, bool isCDD, string refString, string ibanAccount, string lineAmount)
        {
            string FieldText(Recordset recordset, string fieldName)
            {
                try
                {
                    object value = recordset.Fields[fieldName].Value;
                    return value == null || value == DBNull.Value ? string.Empty : value.ToString().Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }

            refString = refString ?? string.Empty;
            ibanAccount = ibanAccount ?? string.Empty;

            double lineAmountValue = DoubleFromString(lineAmount);
            string formattedAmount = lineAmountValue >= 0d
                ? " " + lineAmountValue.ToString(CultureInfo.InvariantCulture)
                : lineAmountValue.ToString(CultureInfo.InvariantCulture);

            string documentKey = string.Empty;
            int oRefIndex = refString.IndexOf("ORef: ", StringComparison.Ordinal);
            if (oRefIndex >= 0 && oRefIndex + 6 < refString.Length)
                documentKey = refString.Substring(oRefIndex + 6);

            int searchOnRvId = 0;
            string query;

            if (isSeller)
            {
                if (string.IsNullOrEmpty(documentKey))
                {
                    query = "SELECT Leveranciers.A110, Leveranciers.A100, Leveranciers.v259, "
                        + "Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, "
                        + "Dokumenten.v037, Dokumenten.v038, Dokumenten.v039, Dokumenten.v249, "
                        + "Dokumenten.v411, Dokumenten.rvDM, Dokumenten.rvID "
                        + "FROM Leveranciers, Dokumenten "
                        + "WHERE Dokumenten.v034 = 'L' + Leveranciers.A110 "
                        + "AND Dokumenten.v039 = '" + refString + "' "
                        + "AND Leveranciers.v259 = '" + ibanAccount + "' "
                        + "AND Str(Val(Dokumenten.v249)) = '" + formattedAmount + "' "
                        + "ORDER BY Dokumenten.v037 ";
                }
                else
                {
                    query = "SELECT Leveranciers.A110, Leveranciers.A100, Leveranciers.v259, "
                        + "Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, "
                        + "Dokumenten.v037, Dokumenten.v038, Dokumenten.v039, Dokumenten.v249, "
                        + "Dokumenten.v411, Dokumenten.rvDM, Dokumenten.rvID "
                        + "FROM Leveranciers, Dokumenten "
                        + "WHERE Dokumenten.v034 = 'L' + Leveranciers.A110 "
                        + "AND Dokumenten.v033 = '" + documentKey + "' ";
                }
            }
            else
            {
                query = "SELECT Klanten.A110, Klanten.A100, Klanten.v259, Klanten.rvID, "
                    + "Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, "
                    + "Dokumenten.v037, Dokumenten.v038, Dokumenten.v039, Dokumenten.v249, "
                    + "Dokumenten.v411, Dokumenten.rvDM, Dokumenten.rvID, Dokumenten.A000 "
                    + "FROM Klanten, Dokumenten "
                    + "WHERE Dokumenten.v034 = 'K' + Klanten.A110 ";

                if (refIsCor && PartLeft(refString, 1) == "1")
                {
                    string clientNumber = PartMid(refString, 4, 4) + PartMid(refString, 9, 2);
                    query += "AND Klanten.A110 = '" + clientNumber + "' "
                        + "AND Str(Val(Dokumenten.v249)) = '" + formattedAmount + "' ";
                }
                else if (PartMid(refString, 1, 3) == "999")
                {
                    int.TryParse(PartMid(refString, 4, 7).Trim(), out searchOnRvId);
                    query += "AND Klanten.rvID =" + searchOnRvId.ToString(CultureInfo.InvariantCulture) + " "
                        + "AND Dokumenten.v249 <> Dokumenten.v037 ";
                }
                else if (!refIsCor && isCDD)
                {
                    MessageBox.Show("Stop for not refIsCor and isCDD", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    query += "AND Dokumenten.A000 = '" + refString + "' "
                        + "AND Str(Val(Dokumenten.v249)) = '" + formattedAmount + "' ";
                }
                else
                {
                    string groupCode = string.Empty;

                    switch (PartLeft(refString, 1))
                    {
                        case "8":
                            groupCode = "V0";
                            break;
                        case "7":
                            groupCode = "V1";
                            break;
                        case "6":
                            groupCode = "B0";
                            break;
                        case "5":
                            groupCode = "F0";
                            break;
                        case "2":
                            groupCode = "Q0";
                            break;
                        default:
                            MessageBox.Show("onbekende OGM voor klanten?", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }

                    if (string.IsNullOrEmpty(groupCode))
                    {
                        MessageBox.Show("onbekende OGM voor klanten proberen met IBAN van de klant", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        query += "AND Klanten.v259 = '" + ibanAccount + "' ";
                    }
                    else
                    {
                        string referteTxtNoFormat = groupCode + PartMid(refString, 2, 6);
                        _ = referteTxtNoFormat;
                    }
                }

                query += "ORDER BY Dokumenten.v035 DESC ";
            }

            rsAny = new Recordset();
            rsAny.CursorLocation = CursorLocationEnum.adUseClient;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SnelHelpPrint(query, BL_LOGGING);
                rsAny.Open(query, adntDB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);
                Cursor.Current = Cursors.Default;

                if (rsAny.EOF)
                {
                    MessageBox.Show("Geen documenten gevonden", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return string.Empty;
                }

                if (rsAny.RecordCount == 1)
                {
                    rsAny.MoveFirst();
                    return FieldText(rsAny, "v033") + "|" + FieldText(rsAny, "A100") + "|" + FieldText(rsAny, "A110");
                }

                if (PartLeft(refString, 3) == "999" || PartLeft(refString, 3) == "104")
                {
                    rsAny.MoveFirst();
                    return FieldText(rsAny, "v033") + "|" + FieldText(rsAny, "A100") + "|" + FieldText(rsAny, "A110");
                }

                string ctrlDocuments = string.Empty;
                while (!rsAny.EOF)
                {
                    ctrlDocuments = FieldText(rsAny, "v033") + "|" + FieldText(rsAny, "A100") + "|" + FieldText(rsAny, "A110");
                    MessageBox.Show("Stop try to validate with first document found" + Environment.NewLine + Environment.NewLine + ctrlDocuments,
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rsAny.MoveNext();
                }

                return ctrlDocuments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bron:" + Environment.NewLine + ex.Source + Environment.NewLine + Environment.NewLine +
                    "Foutnummer: " + ex.HResult.ToString(CultureInfo.InvariantCulture) + Environment.NewLine + Environment.NewLine +
                    "Detail:" + Environment.NewLine + ex.Message,
                    "CtrlDocuments", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            finally
            {
                try
                {
                    if (rsAny != null && rsAny.State != (int)ObjectStateEnum.adStateClosed)
                        rsAny.Close();
                }
                catch
                {
                }
                finally
                {
                    rsAny = null;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
                
        private void xdaParseToArray(string inputText)
        {
            var lines = (inputText ?? string.Empty).Split(new[] { "\r\n" }, StringSplitOptions.None);
            int maxCols = 0;

            foreach (var line in lines)
            {
                var fields = line.Split('\t');
                if (fields.Length > maxCols)
                    maxCols = fields.Length;
            }

            xdaLinesDATAArray = new string[lines.Length, Math.Max(maxCols, 1)];
            for (int i = 0; i < lines.Length; i++)
            {
                var fields = lines[i].Split('\t');
                for (int j = 0; j < fields.Length; j++)
                    xdaLinesDATAArray[i, j] = fields[j];
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            if (ListBoxFinancialDetail.Items.Count > 0)
            {
                string msg = "Huidige verrichtingen negeren." + Environment.NewLine + Environment.NewLine + "Bent U zeker ?";
                var ans = MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (ans != DialogResult.Yes)
                    return;
            }

            GridText = string.Empty;
            Close();
        }
    }
}

