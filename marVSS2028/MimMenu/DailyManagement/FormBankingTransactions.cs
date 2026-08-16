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
    public partial class FormBankingTransactions : Form
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

        private string xdaOMS = string.Empty;
        private string xdaDATA = string.Empty;
        private string xdaLinesOMS = string.Empty;
        private string xdaLinesDATA = string.Empty;
        private string[] xdaOMSArray = new string[0];
        private string[] xdaDATAArray = new string[0];
        private string[] xdaLinesOMSArray = new string[0];
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

        public FormBankingTransactions()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        private static double Val(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0d;
            double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
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

        private void FormBankingTransactions_Load(object sender, EventArgs e)
        {
            xdaLineCounter = 0;
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

            BeginBalans = (int)Val(String99(64));
            ToegestaneKorting = String99(28);
            BekomenKorting = String99(27);
            DefaultRekening = String99(101);

            int[] recs = { 31, 32, 33, 34, 35, 38, 215, 216, 217, 218 };
            int[] rek = { 41, 42, 43, 44, 45, 39, 211, 212, 213, 214 };
            int selected = 0;

            KeuzeInfo0.Items.Clear();
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
                        item = VSet(RekeningNummer[t],7) + "|" + (VBibText(TABLE_LEDGERACCOUNTS, "#v020 #") ?? string.Empty).TrimEnd();
                    }

                    KeuzeInfo0.Items.Add(item);
                    if (DefaultRekening == RekeningNummer[t])
                        selected = KeuzeInfo0.Items.Count - 1;
                }
            }

            if (KeuzeInfo0.Items.Count > 0)
                KeuzeInfo0.SelectedIndex = selected < KeuzeInfo0.Items.Count ? selected : 0;
        }

        private void KeuzeInfo0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KeuzeInfo0.SelectedIndex < 0) return;

            string locationDesktop = LaadTekstOLD("dnnInstellingen", "CodaIOMap");
            bool hasXda = System.IO.Directory.Exists(locationDesktop) && System.IO.Directory.GetFiles(locationDesktop, "*.xda").Length > 0;
            ButtonReadCamt053.Enabled = hasXda;
            CheckBoxSepaViewer.Visible = hasXda;

            int idx = Math.Min(KeuzeInfo0.SelectedIndex, Uittreksel.Length - 1);
            string u = Uittreksel[idx] ?? string.Empty;

            if (u.Length > 0 && char.IsLetter(u[0]))
                LabelInfo11.Text = (Val(PartRight(u, 4)) + 1).ToString(CultureInfo.InvariantCulture);
            else
                LabelInfo11.Text = (Val(u) + 1).ToString(CultureInfo.InvariantCulture);

            string key = PartLeft(KeuzeInfo0.Text, 7);
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
                    lblInfo0.Text = Val(VBibText(TABLE_LEDGERACCOUNTS, "#e" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #")).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    LabelInfo12.Text = (Val(lblInfo0.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    LabelInfo12.Text = Val(VBibText(TABLE_LEDGERACCOUNTS, "#v" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #")).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    lblInfo0.Text = (Val(LabelInfo12.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (bhEuro)
                {
                    double begin = Val(VBibText(TABLE_LEDGERACCOUNTS, "#e" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    double move = Val(VBibText(TABLE_LEDGERACCOUNTS, "#e" + (23 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    lblInfo0.Text = (begin + move).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    LabelInfo12.Text = Math.Round(Val(lblInfo0.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    double begin = Val(VBibText(TABLE_LEDGERACCOUNTS, "#v" + (22 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    double move = Val(VBibText(TABLE_LEDGERACCOUNTS, "#v" + (23 + boekjaar).ToString("000", CultureInfo.InvariantCulture) + " #"));
                    LabelInfo12.Text = (begin + move).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    lblInfo0.Text = (Val(LabelInfo12.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
            }

            LabelInfo13.Text = LabelInfo12.Text;
            lblInfo1.Text = lblInfo0.Text;
            ApplySaldoColors();
        }

        private void ApplySaldoColors()
        {
            double begin = Val(LabelInfo12.Text);
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

            double end = Val(LabelInfo13.Text);
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

        private void KeuzeInfo0_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(KeuzeInfo0.Text))
            {
                System.Media.SystemSounds.Beep.Play();
                KeuzeInfo0.Focus();
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

        private void Annuleren_Click(object sender, EventArgs e)
        {
            if (FinancieelDetail.Items.Count > 0)
            {
                string msg = "Aangeduide verrichtingen negeren." + Environment.NewLine + Environment.NewLine + "Bent U zeker ?";
                var ans = MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (ans != DialogResult.Yes)
                    return;
            }

            GridText = string.Empty;
            Close();
        }

        private void Volgende_Click(object sender, EventArgs e)
        {
            using (var detail = new DetailInfo())
            {
                GridText = string.Empty;
                detail.ShowDialog(this);
            }

            if (string.IsNullOrEmpty(GridText))
                return;

            KeuzeInfo0.Enabled = false;
            FinancieelDetail.Items.Add(GridText);

            double bedrag = Val(PartMid(GridText, 22, 12));
            bool ontvangst = PartLeft(GridText, 1) == "+";

            if (bhEuro)
            {
                double huidig = Val(lblInfo1.Text);
                lblInfo1.Text = (ontvangst ? huidig + bedrag : huidig - bedrag).ToString("#,##0.00", CultureInfo.InvariantCulture);
                LabelInfo13.Text = Math.Round(Val(lblInfo1.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                double huidig = Val(LabelInfo13.Text);
                LabelInfo13.Text = (ontvangst ? huidig + bedrag : huidig - bedrag).ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblInfo1.Text = (Val(LabelInfo13.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }

            ApplySaldoColors();
        }

        private void FinancieelDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert || e.KeyCode == Keys.Add)
                Volgende_Click(sender, e);
        }

        private void Afsluiten_Click(object sender, EventArgs e)
        {
            if (!DateCheck(Datum.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), PERIODAS_TEXT))
            {
                System.Media.SystemSounds.Beep.Play();
                Datum.Focus();
                return;
            }

            if (FinancieelDetail.Items.Count == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Verrichtingen inbrengen a.u.b. !!!");
                return;
            }

            MessageBox.Show("Boeken-stroom is geconverteerd tot basisgedrag; verdere journaalboeking blijft projectspecifiek.",
                "Uittreksel afsluiten", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Struktuur_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gestructureerde verrichting is beschikbaar als basisconversie. Gebruik manuele invoer of vul deze flow projectspecifiek aan.",
                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonReadCamt053_Click(object sender, EventArgs e)
        {
            string xdaLocation = (LaadTekstOLD("dnnInstellingen", "CodaIOMap") ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrEmpty(xdaLocation))
                xdaLocation = (LOCATION_DESKTOP ?? string.Empty).ToLowerInvariant();

            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = xdaLocation;
                ofd.Filter = "Alle bestanden (*.xda)|*.xda";
                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                bool actionResult = ReadCamt053XDA(ofd.FileName.ToLowerInvariant(), CheckBoxSepaViewer.Checked);
                if (!actionResult)
                    return;

                xdaOMS = marVSS2028.Classes.Globals.xdaOMS ?? string.Empty;
                xdaDATA = marVSS2028.Classes.Globals.xdaDATA ?? string.Empty;
                xdaLinesOMS = marVSS2028.Classes.Globals.xdaLinesOMS ?? string.Empty;
                xdaLinesDATA = marVSS2028.Classes.Globals.xdaLinesDATA ?? string.Empty;

                xdaOMSArray = xdaOMS.Split('\t');
                xdaDATAArray = xdaDATA.Split('\t');
                xdaLinesOMSArray = xdaLinesOMS.Split('\t');
                xdaParseToArray(xdaLinesDATA);

                MessageBox.Show("XDA data geladen. Controleer en wijs ontbrekende lijnen toe.", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonAssign_Click(object sender, EventArgs e)
        {
            if (mfgLijst.CurrentRow == null) return;

            using (var detail = new DetailInfo())
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
            MfgSetTextMatrix(row, 4, Val(result[2]).ToString(CultureInfo.InvariantCulture));
            MfgSetTextMatrix(row, 5, result[3]);
            ListIsReady();
        }

        private void ButtonTransfer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Overnemen", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Volgende.Enabled = false;
            }
            else if (SSTab1.SelectedIndex == 1)
            {
                Volgende.Enabled = true;
                Volgende.Focus();
            }
        }

        private string CtrlDocuments(bool isSeller, bool refIsCor, bool isCDD, string refString, string ibanAccount, string lineAmount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refString))
                    return string.Empty;

                var amount = Val(lineAmount).ToString(CultureInfo.InvariantCulture);
                string query;

                if (isSeller)
                {
                    query = "SELECT TOP 1 Leveranciers.A110, Leveranciers.A100, Dokumenten.v033 "
                          + "FROM Leveranciers, Dokumenten "
                          + "WHERE Dokumenten.v034 = 'L' + Leveranciers.A110 "
                          + "AND Dokumenten.v039 = '" + refString.Replace("'", "''") + "' "
                          + "AND Str(Val(Dokumenten.v249)) = '" + amount + "' "
                          + "ORDER BY Dokumenten.v037";
                }
                else
                {
                    query = "SELECT TOP 1 Klanten.A110, Klanten.A100, Dokumenten.v033 "
                          + "FROM Klanten, Dokumenten "
                          + "WHERE Dokumenten.v034 = 'K' + Klanten.A110 "
                          + "AND Dokumenten.v249 <> Dokumenten.v037 "
                          + "ORDER BY Dokumenten.v035 DESC";
                }

                rsAny = new Recordset();
                rsAny.CursorLocation = CursorLocationEnum.adUseClient;
                rsAny.Open(query, adntDB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, 0);

                if (rsAny.EOF)
                    return string.Empty;

                var doc = rsAny.Fields["v033"].Value != null ? rsAny.Fields["v033"].Value.ToString() : string.Empty;
                var naam = rsAny.Fields["A100"].Value != null ? rsAny.Fields["A100"].Value.ToString() : string.Empty;
                var nummer = rsAny.Fields["A110"].Value != null ? rsAny.Fields["A110"].Value.ToString() : string.Empty;
                return doc + "|" + naam + "|" + nummer;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (rsAny != null)
                {
                    if (rsAny.State == 1) rsAny.Close();
                    rsAny = null;
                }
            }
        }

        private bool fnBeginOpname(string deString)
        {
            sDatumAanmaak = PartMid(deString, 6, 6);
            sToepassingsCode = PartMid(deString, 15, 2);
            sNaamBestemmeling = PartMid(deString, 35, 26);
            return sToepassingsCode == "05";
        }

        private bool fnOudSaldo(string deString)
        {
            sRekeningNummer = PartMid(deString, 6, 16);
            sUittreksel = PartMid(deString, 3, 3);
            cOudSaldo = (decimal)(Val(PartMid(deString, 44, 15)) / 1000d);
            sDatumOudSaldo = PartMid(deString, 59, 6);
            sNaamRekeninghouder = PartMid(deString, 65, 26);
            sOmschrijvingRekening = PartMid(deString, 91, 35);
            iOptelControle++;
            return true;
        }

        private bool fnNieuwSaldo(string deString)
        {
            sRekeningNummer2 = PartMid(deString, 5, 12);
            sUittreksel2 = PartMid(deString, 2, 3);
            cNieuwSaldo = (decimal)(Val(PartMid(deString, 43, 15)) / 1000d);
            sDatumNieuwSaldo = PartMid(deString, 58, 6);
            iOptelControle++;
            return true;
        }

        private bool fnEindOpname(string deString)
        {
            iOptelCtrlCheckUp = (int)Val(PartMid(deString, 17, 6));
            if (iOptelCtrlCheckUp != iOptelControle)
                SnelHelpPrint("Onlogische situatie", false);

            cDebetSaldo = (decimal)(Val(PartMid(deString, 23, 15)) / 1000d);
            cCreditSaldo = (decimal)(Val(PartMid(deString, 38, 15)) / 1000d);
            return PartMid(deString, 128, 1) != "1";
        }

        private bool fnBeweging(string deString)
        {
            iOptelControle++;
            var deel = PartMid(deString, 2, 1);

            if (deel == "1")
            {
                sRefFinInstelling = PartMid(deString, 11, 21);
                sDC = PartMid(deString, 32, 1);
                cBedrag = (decimal)(Val(PartMid(deString, 33, 15)) / 1000d);
                sValutadatum = PartMid(deString, 48, 6);
                sVerrichting = PartMid(deString, 54, 8);
                sMededeling = PartMid(deString, 62, 1);
                sMDDZone1 = PartMid(deString, 63, 3);
                sMDDZone2 = PartMid(deString, 66, 50);
                sBoekDatum = PartMid(deString, 116, 6);
                sDagAfschriftVolgNummer = PartMid(deString, 122, 3);
            }
            else if (deel == "2")
            {
                sMededeling2 = PartMid(deString, 11, 53);
                sRefKlant[0] = PartMid(deString, 64, 13);
                sRefKlant[1] = PartMid(deString, 77, 13);
                sMuntVerrichting = PartMid(deString, 90, 3);
                cBedragMunt = (decimal)(Val(PartMid(deString, 93, 15)) / 1000d);
            }
            else if (deel == "3")
            {
                sRekeningTP = PartMid(deString, 11, 37);
                sITcodesTP = PartMid(deString, 23, 10);
                sRekeningTPextra = PartMid(deString, 33, 15);
                sNaamEnAdres[0] = PartMid(deString, 48, 35);
            }
            else
            {
                MessageBox.Show("Onlogische situatie", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return PartMid(deString, 126, 1) != "0";
        }

        private void BewegingSchoon()
        {
            sRefFinInstelling = string.Empty;
            sRefFinInstelling2 = string.Empty;
            sDC = string.Empty;
            cBedrag = 0;
            sVerrichting = string.Empty;
            sMededeling = string.Empty;
            sMDDZone1 = string.Empty;
            sMDDZone2 = string.Empty;
            sBoekDatum = string.Empty;
            sDagAfschriftVolgNummer = string.Empty;
            sValutadatum = string.Empty;
            sMededeling2 = string.Empty;
            sRefKlant[0] = string.Empty;
            sRefKlant[1] = string.Empty;
            sMuntVerrichting = string.Empty;
            cBedragMunt = 0;
            sRekeningTP = string.Empty;
            sITcodesTP = string.Empty;
            sRekeningTPextra = string.Empty;
            sNaamEnAdres[0] = string.Empty;
            sNaamEnAdres[1] = string.Empty;
            sNaamEnAdres[2] = string.Empty;
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
    }
}
