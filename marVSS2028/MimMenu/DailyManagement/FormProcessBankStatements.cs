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
    public partial class FormProcessBankStatements : Form
    {
        // VB6 control arrays
        private Label[] _labelInfo;
        private Label[] _lblInfo;
        private ComboBox[] _keuzeInfo;

        // VB6 variables
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

        private bool bCtrl;
        private int COUNT_TO_LOCAL;
        private int iOptelControle;

        // Begin
        private string sDatumAanmaak = string.Empty;
        private string sToepassingsCode = string.Empty;
        private string sNaamBestemmeling = string.Empty;

        // OudSaldo
        private string sRekeningNummer = string.Empty;
        private string sUittreksel = string.Empty;
        private decimal cOudSaldo;
        private string sDatumOudSaldo = string.Empty;
        private string sNaamRekeninghouder = string.Empty;
        private string sOmschrijvingRekening = string.Empty;

        // BewegingsArtikel1
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

        // BewegingsArtikel2
        private string sMededeling2 = string.Empty;
        private readonly string[] sRefKlant = new string[2];
        private string sMuntVerrichting = string.Empty;
        private decimal cBedragMunt;

        // BewegingsArtikel3
        private string sRekeningTP = string.Empty;
        private string sITcodesTP = string.Empty;
        private string sRekeningTPextra = string.Empty;
        private readonly string[] sNaamEnAdres = new string[3];

        // NieuwSaldo
        private string sUittreksel2 = string.Empty;
        private string sRekeningNummer2 = string.Empty;
        private decimal cNieuwSaldo;
        private string sDatumNieuwSaldo = string.Empty;

        // EindOpname
        private int iOptelCtrlCheckUp;
        private decimal cDebetSaldo;
        private decimal cCreditSaldo;

        // XDA arrays
        private string xdaOMS = string.Empty;
        private string xdaDATA = string.Empty;
        private string xdaLinesOMS = string.Empty;
        private string xdaLinesDATA = string.Empty;
        private string[] xdaOMSArray = new string[0];
        private string[] xdaDATAArray = new string[0];
        private string[] xdaLinesOMSArray = new string[0];
        private string[,] xdaLinesDATAArray = new string[0, 0];

        public FormProcessBankStatements()
        {
            InitializeComponent();
            WireHighlightEvents(this);
            InitializeControlArrays();
        }

        private void InitializeControlArrays()
        {
            _labelInfo = new Label[14];
            _labelInfo[0] = LabelInfo0;
            _labelInfo[1] = LabelInfo1;
            _labelInfo[2] = LabelInfo2;
            _labelInfo[3] = LabelInfo3;
            _labelInfo[11] = LabelInfo11;
            _labelInfo[12] = LabelInfo12;
            _labelInfo[13] = LabelInfo13;

            _lblInfo = new Label[8];
            _lblInfo[0] = lblInfo0;
            _lblInfo[1] = lblInfo1;
            _lblInfo[4] = lblInfo4;
            _lblInfo[5] = lblInfo5;
            _lblInfo[6] = lblInfo6;
            _lblInfo[7] = lblInfo7;

            _keuzeInfo = new ComboBox[1];
            _keuzeInfo[0] = KeuzeInfo0;
        }

        private static double Val(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0d;
            double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private static string VbMid(string s, int start, int len)
            => SafeMid(s ?? string.Empty, start, len);

        private static string VbLeft(string s, int len)
            => SafeLeft(s ?? string.Empty, len);

        private static string VbRight(string s, int len)
            => SafeRight(s ?? string.Empty, len);

        // compatibility aliases for direct VB6-style call sites
        private static string Mid(string s, int start, int len) => VbMid(s, start, len);
        private static string Left(string s, int len) => VbLeft(s, len);
        private static string Right(string s, int len) => VbRight(s, len);

        private string MfgTextMatrix(int row, int col)
        {
            if (row < 0 || row >= mfgLijst.Rows.Count || col < 0 || col >= mfgLijst.Columns.Count)
                return string.Empty;
            return mfgLijst.Rows[row].Cells[col].Value?.ToString() ?? string.Empty;
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

        private void MfgInsertItemBeforeLast(string line)
        {
            var parts = (line ?? string.Empty).Split('\t').ToList();
            while (parts.Count < 7) parts.Add(string.Empty);
            int idx = Math.Max(0, mfgLijst.Rows.Count - 1);
            mfgLijst.Rows.Insert(idx, parts.Take(7).ToArray());
        }

        private void EnsureMfgBaseRows()
        {
            if (mfgLijst.Rows.Count == 0)
            {
                mfgLijst.Rows.Add();
                mfgLijst.Rows.Add();
            }
        }

        private int MfgCurrentRow
            => mfgLijst.CurrentCell?.RowIndex ?? 0;

        private bool MfgHasRows
            => mfgLijst.Rows.Count > 0;

        private string CtrlDocuments(bool isSeller, bool refIsCor, bool isCDD, string refString, string ibanAccount, string lineAmount)
        {
            string documentKey = string.Empty;
            int orefIdx = (refString ?? string.Empty).IndexOf("ORef: ", StringComparison.OrdinalIgnoreCase);
            if (orefIdx >= 0)
                documentKey = refString.Substring(orefIdx + 6).Trim();

            string formattedAmount = Val(lineAmount).ToString(CultureInfo.InvariantCulture);

            string sql;
            if (isSeller)
            {
                if (string.IsNullOrEmpty(documentKey))
                {
                    sql = "SELECT Leveranciers.A110, Leveranciers.A100, Leveranciers.v259, " +
                          "Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, " +
                          "Dokumenten.v037, Dokumenten.v038, Dokumenten.v039, Dokumenten.v249, " +
                          "Dokumenten.v411, Dokumenten.rvDM, Dokumenten.rvID " +
                          "FROM Leveranciers, Dokumenten " +
                          "WHERE Dokumenten.v034 = 'L' + Leveranciers.A110 " +
                          "AND Dokumenten.v039 = '" + refString + "' " +
                          "AND Leveranciers.v259 = '" + ibanAccount + "' " +
                          "AND Str(Val(Dokumenten.v249)) = '" + formattedAmount + "' " +
                          "ORDER BY Dokumenten.v037 ";
                }
                else
                {
                    sql = "SELECT Leveranciers.A110, Leveranciers.A100, Leveranciers.v259, " +
                          "Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, " +
                          "Dokumenten.v037, Dokumenten.v038, Dokumenten.v039, Dokumenten.v249, " +
                          "Dokumenten.v411, Dokumenten.rvDM, Dokumenten.rvID " +
                          "FROM Leveranciers, Dokumenten " +
                          "WHERE Dokumenten.v034 = 'L' + Leveranciers.A110 " +
                          "AND Dokumenten.v033 = '" + documentKey + "' ";
                }
            }
            else
            {
                sql = "SELECT Klanten.A110, Klanten.A100, Klanten.v259, Klanten.rvID, " +
                      "Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, " +
                      "Dokumenten.v037, Dokumenten.v038, Dokumenten.v039, Dokumenten.v249, " +
                      "Dokumenten.v411, Dokumenten.rvDM, Dokumenten.rvID, Dokumenten.A000 " +
                      "FROM Klanten, Dokumenten " +
                      "WHERE Dokumenten.v034 = 'K' + Klanten.A110 ";

                if (refIsCor && Left(refString, 1) == "1")
                {
                    string clientNumber = Mid(refString, 4, 4) + Mid(refString, 9, 2);
                    sql += "AND Klanten.A110 = '" + clientNumber + "' ";
                    sql += "AND Str(Val(Dokumenten.v249)) = '" + formattedAmount + "' ";
                }
                else if (Left(refString, 3) == "999")
                {
                    int searchOnRvId = (int)Val(Mid(refString, 4, 7));
                    sql += "AND Klanten.rvID = " + searchOnRvId.ToString(CultureInfo.InvariantCulture) + " ";
                    sql += "AND Dokumenten.v249 <> Dokumenten.v037 ";
                }
                else if (!refIsCor && isCDD)
                {
                    sql += "AND Dokumenten.A000 = '" + refString + "' ";
                    sql += "AND Str(Val(Dokumenten.v249)) = '" + formattedAmount + "' ";
                }
                else
                {
                    sql += "AND Klanten.v259 = '" + ibanAccount + "' ";
                }

                sql += "ORDER BY Dokumenten.v035 DESC ";
            }

            SnelHelpPrint(sql, BL_LOGGING);

            rsAny = new Recordset();
            rsAny.CursorLocation = CursorLocationEnum.adUseClient;

            try
            {
                rsAny.Open(sql, adntDB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

                if (rsAny.RecordCount == 0)
                {
                    MessageBox.Show("Geen documenten gevonden");
                    return string.Empty;
                }

                rsAny.MoveFirst();
                string result = (rsAny.Fields["v033"].Value?.ToString() ?? string.Empty) + "|"
                              + (rsAny.Fields["A100"].Value?.ToString() ?? string.Empty) + "|"
                              + (rsAny.Fields["A110"].Value?.ToString() ?? string.Empty);

                if (rsAny.RecordCount > 1 && Left(refString, 3) != "999" && Left(refString, 3) != "104")
                    MessageBox.Show("Stop try to validate with first document found" + Environment.NewLine + Environment.NewLine + result, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
            finally
            {
                try { if (rsAny.State != 0) rsAny.Close(); } catch { }
                rsAny = null;
            }
        }

        private bool ListIsReady()
        {
            int count = 0;
            for (int row = 0; row < mfgLijst.Rows.Count; row++)
            {
                if (MfgTextMatrix(row, 3) == "??????")
                    count++;
            }

            LabelCounter.Text = count == 0 ? string.Empty : count.ToString(CultureInfo.InvariantCulture);
            bool ready = count == 0;
            ButtonTransfer.Enabled = ready;
            ButtonAssign.Visible = !ready;
            return ready;
        }

        private void xdaParseToArray(string inputText)
        {
            string[] lines = (inputText ?? string.Empty).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int maxCols = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('\t');
                if (fields.Length > maxCols)
                    maxCols = fields.Length;
            }

            if (maxCols == 0)
            {
                xdaLinesDATAArray = new string[0, 0];
                return;
            }

            xdaLinesDATAArray = new string[lines.Length, maxCols];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('\t');
                for (int j = 0; j < fields.Length; j++)
                    xdaLinesDATAArray[i, j] = fields[j];
            }
        }

        private bool fnBeginOpname(string deString)
        {
            sDatumAanmaak = Mid(deString, 6, 6);
            sToepassingsCode = Mid(deString, 15, 2);
            sNaamBestemmeling = Mid(deString, 35, 26);
            return sToepassingsCode == "05";
        }

        private bool fnOudSaldo(string deString)
        {
            sRekeningNummer = Mid(deString, 6, 16);
            sUittreksel = Mid(deString, 3, 3);
            cOudSaldo = (decimal)(Val(Mid(deString, 44, 15)) / 1000d);
            sDatumOudSaldo = Mid(deString, 59, 6);
            sNaamRekeninghouder = Mid(deString, 65, 26);
            sOmschrijvingRekening = Mid(deString, 91, 35);
            iOptelControle++;
            return true;
        }

        private bool fnNieuwSaldo(string deString)
        {
            sRekeningNummer2 = Mid(deString, 5, 12);
            sUittreksel2 = Mid(deString, 2, 3);
            cNieuwSaldo = (decimal)(Val(Mid(deString, 43, 15)) / 1000d);
            sDatumNieuwSaldo = Mid(deString, 58, 6);
            iOptelControle++;
            return true;
        }

        private bool fnEindOpname(string deString)
        {
            iOptelCtrlCheckUp = (int)Val(Mid(deString, 17, 6));
            if (iOptelCtrlCheckUp != iOptelControle)
                SnelHelpPrint("Onlogische situatie", false);

            cDebetSaldo = (decimal)(Val(Mid(deString, 23, 15)) / 1000d);
            cCreditSaldo = (decimal)(Val(Mid(deString, 38, 15)) / 1000d);
            return Mid(deString, 128, 1) != "1";
        }

        private bool fnBeweging(string deString)
        {
            iOptelControle++;
            switch (Mid(deString, 2, 1))
            {
                case "1":
                    sRefFinInstelling = Mid(deString, 11, 21);
                    sDC = Mid(deString, 32, 1);
                    cBedrag = (decimal)(Val(Mid(deString, 33, 15)) / 1000d);
                    sValutadatum = Mid(deString, 48, 6);
                    sVerrichting = Mid(deString, 54, 8);
                    sMededeling = Mid(deString, 62, 1);
                    sMDDZone1 = Mid(deString, 63, 3);
                    sMDDZone2 = Mid(deString, 66, 50);
                    sBoekDatum = Mid(deString, 116, 6);
                    sDagAfschriftVolgNummer = Mid(deString, 122, 3);
                    break;
                case "2":
                    sMededeling2 = Mid(deString, 11, 53);
                    break;
                case "3":
                    sRekeningTP = Mid(deString, 11, 37);
                    sNaamEnAdres[0] = Mid(deString, 48, 35);
                    break;
                default:
                    MessageBox.Show("Onlogische situatie", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            return Mid(deString, 126, 1) != "0";
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

        private void InbrengFinancieel_Load(object sender, EventArgs e)
        {
            xdaLineCounter = 0;
            EnsureMfgBaseRows();

            if (string.IsNullOrEmpty(LaadTekst("dnnInstellingen", "CodaIOMap")))
                BeWaarTekst("dnnInstellingen", "CodaIOMap", LOCATION_DESKTOP);

            if (Right(LOCATION_COMPANYDATA, 5) == "\\098\\" || Right(LOCATION_COMPANYDATA, 5) == "\\099\\")
                TextBoxWarningTestCompany.Visible = true;

            Top = 0;
            base.Left = 0;

            LabelInfo2.Text = " Document    TegenR.       Bedrag Omschrijving                    Fin. Kort.";

            if (DateTime.TryParseExact(MIM_GLOBAL_DATE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                Datum.Value = parsedDate;

            BeginBalans = (int)Val(String99(64));

            ToegestaneKorting = String99(28);
            BekomenKorting    = String99(27);
            DefaultRekening   = String99(101);

            RecNummer[0] = 31;  RekeningNummer[0] = String99(41);  Uittreksel[0] = (String99(31) ?? "").Trim();
            RecNummer[1] = 32;  RekeningNummer[1] = String99(42);  Uittreksel[1] = (String99(32) ?? "").Trim();
            RecNummer[2] = 33;  RekeningNummer[2] = String99(43);  Uittreksel[2] = (String99(33) ?? "").Trim();
            RecNummer[3] = 34;  RekeningNummer[3] = String99(44);  Uittreksel[3] = (String99(34) ?? "").Trim();
            RecNummer[4] = 35;  RekeningNummer[4] = String99(45);  Uittreksel[4] = (String99(35) ?? "").Trim();
            RecNummer[5] = 38;  RekeningNummer[5] = String99(39);  Uittreksel[5] = (String99(38) ?? "").Trim();
            RecNummer[6] = 215; RekeningNummer[6] = String99(211); Uittreksel[6] = (String99(215) ?? "").Trim();
            RecNummer[7] = 216; RekeningNummer[7] = String99(212); Uittreksel[7] = (String99(216) ?? "").Trim();
            RecNummer[8] = 217; RekeningNummer[8] = String99(213); Uittreksel[8] = (String99(217) ?? "").Trim();
            RecNummer[9] = 218; RekeningNummer[9] = String99(214); Uittreksel[9] = (String99(218) ?? "").Trim();

            KeuzeInfo0.Items.Clear();
            int defaultIndex = -1;
            for (int t = 0; t <= 9; t++)
            {
                if (string.IsNullOrWhiteSpace(RekeningNummer[t]))
                    continue;

                BGet(TABLE_LEDGERACCOUNTS, 0, RekeningNummer[t]);
                string itemText;
                if (Ktrl != 0)
                {
                    itemText = RekeningNummer[t] + "|Niet aanwezig. Installeer via Setup Boekjaar.";
                }
                else
                {
                    RecordToVeld(TABLE_LEDGERACCOUNTS);
                    itemText = RekeningNummer[t] + "|" + (VBibText(TABLE_LEDGERACCOUNTS, "#v020 #") ?? string.Empty).TrimEnd();
                }

                KeuzeInfo0.Items.Add(itemText);
                if (DefaultRekening == RekeningNummer[t])
                    defaultIndex = KeuzeInfo0.Items.Count - 1;
            }

            if (KeuzeInfo0.Items.Count > 0)
                KeuzeInfo0.SelectedIndex = defaultIndex >= 0 ? defaultIndex : 0;
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

        private void Annuleren_Click(object sender, EventArgs e)
        {
            if (FinancieelDetail.Items.Count > 0)
            {
                var ans = MessageBox.Show(
                    "Aangeduide verrichtingen negeren." + Environment.NewLine + Environment.NewLine + "Bent U zeker ?",
                    string.Empty,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
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

            double bedrag = Val(Mid(GridText, 22, 12));
            bool ontvangst = Left(GridText, 1) == "+";

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
        }

        private void KeuzeInfo0_SelectedIndexChanged(object sender, EventArgs e)
        {
            string locationDesktop = LaadTekst("dnnInstellingen", "CodaIOMap");
            bool hasXda = !string.IsNullOrEmpty(locationDesktop) && System.IO.Directory.Exists(locationDesktop)
                          && System.IO.Directory.GetFiles(locationDesktop, "*.xda").Length > 0;

            ButtonReadCamt053.Enabled = hasXda;
            CheckBoxSepaViewer.Visible = hasXda;

            if (KeuzeInfo0.SelectedIndex < 0 || KeuzeInfo0.SelectedIndex >= Uittreksel.Length)
                return;

            string utt = (Uittreksel[KeuzeInfo0.SelectedIndex] ?? string.Empty).ToUpperInvariant();
            if (utt.Length > 0 && utt[0] >= 'A' && utt[0] <= 'Z')
                LabelInfo11.Text = (Val(Right(utt, 4)) + 1).ToString(CultureInfo.InvariantCulture);
            else
                LabelInfo11.Text = (Val(utt) + 1).ToString(CultureInfo.InvariantCulture);

            string rekKey = Left(KeuzeInfo0.Text, 7);
            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(rekKey, 7));
            if (Ktrl != 0)
                return;

            RecordToVeld(TABLE_LEDGERACCOUNTS);

            int boekjaarIndex = 0;
            if (Application.OpenForms["FormBYPERDAT"] is global::marVSS2028.FormBYPERDAT bp)
                boekjaarIndex = bp.CmbBoekjaar.SelectedIndex;

            string field22 = "#" + (bhEuro ? "e" : "v") + (22 + boekjaarIndex).ToString("000") + " #";
            string field23 = "#" + (bhEuro ? "e" : "v") + (23 + boekjaarIndex).ToString("000") + " #";

            double beginSaldo;
            if (BeginBalans == 1)
                beginSaldo = Val(VBibText(TABLE_LEDGERACCOUNTS, field22));
            else
                beginSaldo = Val(VBibText(TABLE_LEDGERACCOUNTS, field22)) + Val(VBibText(TABLE_LEDGERACCOUNTS, field23));

            if (bhEuro)
            {
                lblInfo0.Text = beginSaldo.ToString("#,##0.00", CultureInfo.InvariantCulture);
                LabelInfo12.Text = (BeginBalans == 1 ? beginSaldo * EURO : Math.Round(beginSaldo * EURO)).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                LabelInfo12.Text = beginSaldo.ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblInfo0.Text = (beginSaldo / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }

            LabelInfo13.Text = LabelInfo12.Text;
            lblInfo1.Text = lblInfo0.Text;

            UpdateSaldoColor(12);
            UpdateSaldoColor(13);
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
            string datStr = Datum.Value.ToString("dd/MM/yyyy");
            if (!DateCheck(datStr, PERIODAS_TEXT))
            {
                OpenBYPERDAT(this);
            }
        }

        private void FinancieelDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert || e.KeyCode == Keys.Add)
                Volgende_Click(sender, EventArgs.Empty);
        }

        private void mfgLijst_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string selectToEdit = MfgTextMatrix(e.RowIndex, 3);
            if (selectToEdit == "??????")
            {
                ButtonAssign.Enabled = true;
                ButtonAssign.Visible = true;
                ButtonAssign.Focus();
            }
            else
            {
                ButtonAssign.Enabled = false;
            }
        }

        private void mfgLijst_SelectionChanged(object sender, EventArgs e)
        {
            if (mfgLijst.CurrentCell == null) return;
            mfgLijst_CellClick(sender, new DataGridViewCellEventArgs(mfgLijst.CurrentCell.ColumnIndex, mfgLijst.CurrentCell.RowIndex));
        }

        private void UpdateSaldoColor(int labelIndex)
        {
            Label valLabel = _labelInfo[labelIndex];
            Label eurBefLabel = _lblInfo[labelIndex - 12];
            if (valLabel == null || eurBefLabel == null) return;

            double val = Val(valLabel.Text);
            if (val == 0)
            {
                valLabel.BackColor = System.Drawing.Color.Silver;
                eurBefLabel.BackColor = System.Drawing.Color.Silver;
            }
            else if (val > 0)
            {
                valLabel.BackColor = System.Drawing.Color.FromArgb(0x80, 0xFF, 0x80);
                eurBefLabel.BackColor = System.Drawing.Color.FromArgb(0x80, 0xFF, 0x80);
            }
            else
            {
                valLabel.BackColor = System.Drawing.Color.FromArgb(0x80, 0xFF, 0xFF);
                eurBefLabel.BackColor = System.Drawing.Color.FromArgb(0x80, 0xFF, 0xFF);
            }
        }

        private void Afsluiten_Click(object sender, EventArgs e)
        {
            if (!DateCheck(Datum.Value.ToString("dd/MM/yyyy"), PERIODAS_TEXT))
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

            BGet(TABLE_LEDGERACCOUNTS, 0, Left(KeuzeInfo0.Text, 7));
            if (Ktrl != 0)
            {
                MessageBox.Show("onlogische situatie");
                return;
            }
            RecordToVeld(TABLE_LEDGERACCOUNTS);

            string uittrekselSleutel = Left(VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").ToUpperInvariant(), 2)
                                       + Datum.Value.ToString("yy")
                                       + (Val(LabelInfo11.Text) - 1).ToString("0000");

            BGet(TABLE_JOURNAL, 2, uittrekselSleutel);
            if (Ktrl == 0)
            {
                RecordToVeld(TABLE_JOURNAL);
                if (string.CompareOrdinal(VBibText(TABLE_JOURNAL, "#v066 #"), Datum.Value.ToString("yyyyMMdd")) > 0)
                {
                    string msg = "Er zijn reeds uittreksels met een hogere datum !" + Environment.NewLine + Environment.NewLine
                               + "Laatste uittreksel nr. " + uittrekselSleutel
                               + " dateert van : " + DateText(VBibText(TABLE_JOURNAL, "#v066 #"))
                               + Environment.NewLine + Environment.NewLine
                               + "Vervolg.  Bent U zeker ?";
                    if (MessageBox.Show(msg, "Uittreksel afsluiten",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                }
            }

            string confirmMsg = bhEuro
                ? "Datum uittreksel " + Datum.Value + " en bekomen eindsaldo EUR " + lblInfo1.Text
                    + Environment.NewLine + Environment.NewLine
                    + "Hierna wordt de boekhouding bijgewerkt.  Bent U zeker ?"
                : "Datum uittreksel " + Datum.Value + " en bekomen eindsaldo BEF " + LabelInfo13.Text
                    + Environment.NewLine + Environment.NewLine
                    + "Hierna wordt de boekhouding bijgewerkt.  Bent U zeker ?";

            if (MessageBox.Show(confirmMsg,
                "Uittreksel : " + Left(VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").ToUpperInvariant(), 2)
                + Datum.Value.ToString("yy") + Val(LabelInfo11.Text).ToString("0000"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            BBegin();
            using (var boekingForm = new FormBoeking())
            {
                if (WegBoekFout(boekingForm))
                {
                    BAbort();
                    return;
                }

                BEnd();
                if (Ktrl != 0)
                {
                    BAbort();
                    return;
                }

                string dummySleutel = "s" + RecNummer[Math.Max(0, KeuzeInfo0.SelectedIndex)].ToString("000");
                BGet(TABLE_COUNTERS, 0, dummySleutel);
                if (Ktrl != 0)
                {
                    MessageBox.Show("TellerStop " + dummySleutel + ".  kontakteer R&&Vsoft");
                }
                else
                {
                    RecordToVeld(TABLE_COUNTERS);
                    FL99_RECORD = Val(LabelInfo11.Text).ToString(CultureInfo.InvariantCulture);
                    if (BAModus == 1)
                        VBib(TABLE_COUNTERS, FL99_RECORD, "v217 ");
                    else
                        VBib(TABLE_COUNTERS, FL99_RECORD, dummySleutel);
                    BUpdate(TABLE_COUNTERS, 0);
                }

                BGet(TABLE_COUNTERS, 0, "s101");
                if (Ktrl == 0)
                {
                    RecordToVeld(TABLE_COUNTERS);
                    FL99_RECORD = Left(KeuzeInfo0.Text, 7);
                    if (BAModus == 1)
                        VBib(TABLE_COUNTERS, FL99_RECORD, "v217 ");
                    else
                        VBib(TABLE_COUNTERS, FL99_RECORD, "s101");
                    BUpdate(TABLE_COUNTERS, 0);
                }

                BClose(TABLE_COUNTERS);
                GridText = string.Empty;
                Close();
            }
        }

        private bool WegBoekFout(FormBoeking boekingForm)
        {
            DKTRL_CUMUL = 0;
            DKTRL_BEF = 0;
            DKTRL_EUR = 0;

            TLB_RECORD[TABLE_JOURNAL] = string.Empty;
            VBib(TABLE_JOURNAL, Left(KeuzeInfo0.Text, 7), "v019");
            VBib(TABLE_JOURNAL, Datum.Value.ToString("yyyyMMdd"), "v066");
            VBib(TABLE_JOURNAL, Datum.Value.ToString("yyyyMMdd"), "v035");

            BGet(TABLE_LEDGERACCOUNTS, 0, Left(KeuzeInfo0.Text, 7));
            if (Ktrl != 0)
                return true;

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            dokumentSleutel = Left(VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").ToUpperInvariant(), 2)
                              + Datum.Value.ToString("yy")
                              + Val(LabelInfo11.Text).ToString("0000");

            VBib(TABLE_JOURNAL, dokumentSleutel, "v038");

            if (bhEuro)
            {
                double beginSaldo = Val(lblInfo0.Text);
                double eindSaldo = Val(lblInfo1.Text);
                double verschil = eindSaldo - beginSaldo;
                VBib(TABLE_JOURNAL, "Sld:" + Dec(beginSaldo, MASK_EURBH) + "/" + Dec(eindSaldo, MASK_EURBH), "v067");
                VBib(TABLE_JOURNAL, Dec(verschil, MASK_SY[2]), "v068");
            }
            else
            {
                VBib(TABLE_JOURNAL, "Sld : " + Dec(Val(LabelInfo12.Text), MASK_SY[0]) + " - " + Dec(Val(LabelInfo13.Text), MASK_SY[0]), "v067");
                VBib(TABLE_JOURNAL, Dec(Val(LabelInfo13.Text) - Val(LabelInfo12.Text), MASK_SY[0]), "v068");
            }

            BInsert(TABLE_JOURNAL, 2, boekingForm);
            if (Ktrl != 0)
                return true;

            VBib(TABLE_JOURNAL, Left(KeuzeInfo0.Text, 7), "v069");

            for (int t = 0; t < FinancieelDetail.Items.Count; t++)
            {
                string line = FinancieelDetail.Items[t]?.ToString() ?? string.Empty;
                bool ontvangst = Left(line, 1) == "+";

                string doc = Mid(line, 2, 11);
                if (string.IsNullOrWhiteSpace(doc))
                {
                    VBib(TABLE_JOURNAL, " ", "v033");
                    VBib(TABLE_JOURNAL, " ", "v034");
                }
                else
                {
                    BGet(TABLE_INVOICES, 0, doc);
                    if (Ktrl != 0) return true;
                    RecordToVeld(TABLE_INVOICES);
                    VBib(TABLE_JOURNAL, VBibText(TABLE_INVOICES, "#v033 #"), "v033");
                    VBib(TABLE_JOURNAL, VBibText(TABLE_INVOICES, "#v034 #"), "v034");
                }

                double totaalBedrag = Val(Mid(line, 22, 12));
                VBib(TABLE_JOURNAL, Mid(line, 35, 29).Trim(), "v067");

                double finKort = Val(Mid(line, 65, 12));
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
                    if (Ktrl != 0) return true;
                }

                VBib(TABLE_JOURNAL, Mid(line, 14, 7), "v019");
                VBib(TABLE_JOURNAL, (ontvangst ? -totaalBedrag : totaalBedrag).ToString(CultureInfo.InvariantCulture), "v068");

                BInsert(TABLE_JOURNAL, 2, boekingForm);
                if (Ktrl != 0) return true;

                if (!string.IsNullOrWhiteSpace(VBibText(TABLE_JOURNAL, "#v033 #")))
                {
                    double bij = XisEuroWisBEF ? Math.Round(totaalBedrag / EURO, 2) : totaalBedrag;
                    VBib(TABLE_INVOICES,
                        (Val(VBibText(TABLE_INVOICES, "#v037 #")) + bij).ToString(CultureInfo.InvariantCulture),
                        "v037");
                    VBib(TABLE_INVOICES, VBibText(TABLE_JOURNAL, "#v038 #"), "v038");
                    BUpdate(TABLE_INVOICES, 0);
                    if (Ktrl != 0) return true;
                }
            }

            string wegBoekMode = "0";
            if (Application.OpenForms["FormMim"] is FormMim mim)
                wegBoekMode = mim.GetWegBoekModus();

            if (DKTRL_CUMUL != 0)
            {
                MessageBox.Show("LogikaFout bij vierkantskontrole journaal. Deze verrichting wordt geannuleerd.");
                boekingForm.ShowDialog(this);
                return true;
            }

            if (JournaalLocked)
            {
                boekingForm.ShowDialog(this);
                return true;
            }

            if (wegBoekMode == "2" || (wegBoekMode == "1" && (DKTRL_BEF != 0 || DKTRL_EUR != 0)))
                boekingForm.ShowDialog(this);

            return DKTRL_CUMUL != 0;
        }

        private void ButtonReadCamt053_Click(object sender, EventArgs e)
        {
            string xdaLocation = (LaadTekst("dnnInstellingen", "CodaIOMap") ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrEmpty(xdaLocation))
                xdaLocation = (LOCATION_DESKTOP ?? string.Empty).ToLowerInvariant();

            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = xdaLocation;
                ofd.Filter = "Alle bestanden (*.xda)|*.xda";
                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                string filePath = ofd.FileName.ToLowerInvariant();
                string pickedDir = System.IO.Path.GetDirectoryName(filePath)?.ToLowerInvariant() + "\\";

                if (!string.Equals(xdaLocation, pickedDir, StringComparison.OrdinalIgnoreCase))
                {
                    if ((pickedDir ?? string.Empty).Contains("coda\\in"))
                    {
                        MessageBox.Show("Inladen van verwerkte documenten is verboden", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string msg = ".XDA en .XML voor SEPA betaalbestanden locatie staat ingesteld op:" + Environment.NewLine
                               + xdaLocation + Environment.NewLine + Environment.NewLine
                               + "Mag de standaard locatie vanaf nu gewijzigd worden naar:" + Environment.NewLine
                               + pickedDir;
                    if (MessageBox.Show(msg, "Uittreksel afsluiten", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        BeWaarTekst("dnnInstellingen", "CodaIOMap", pickedDir);
                        MessageBox.Show("Herstart het inladen van het document", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Probeer opnieuw", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return;
                }

                bool actionResult = ReadCamt053XDA(filePath, CheckBoxSepaViewer.Checked);
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

                int qualityOfBeginning = 0;
                if (xdaDATAArray.Length > 3 && xdaDATAArray[3] != "" && Val(LabelInfo11.Text) == Val(xdaDATAArray[3]))
                    qualityOfBeginning++;

                double beginSaldoXDA = xdaDATAArray.Length > 6 ? Val(xdaDATAArray[6]) : 0d;
                if (beginSaldoXDA == Val(Dec(Val(lblInfo0.Text), "")))
                    qualityOfBeginning++;

                double cumulSaldo = beginSaldoXDA;
                if (qualityOfBeginning == 0)
                {
                    if (!(mfgLijst.Rows.Count > 2 && beginSaldoXDA == Val(MfgTextMatrix(mfgLijst.Rows.Count - 2, 6))))
                    {
                        MessageBox.Show("Noch teller (niet essentieel), noch beginsaldo (essentieel)" + Environment.NewLine
                                        + "Bij beheer van meerdere rekeningen, duidt eerst de rekening aan voor .XDA import en probeer opnieuw",
                                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else if (mfgLijst.Rows.Count <= 2)
                {
                    MfgInsertItemBeforeLast("\t\t\t\t\t\t" + beginSaldoXDA.ToString(CultureInfo.InvariantCulture));
                }

                for (int t = 0; t < xdaLinesDATAArray.GetLength(0); t++)
                {
                    string bbaCode = (xdaLinesDATAArray[t, 2] ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(bbaCode))
                        continue;

                    xdaLineCounter++;

                    string amountToCheck = !string.IsNullOrWhiteSpace(xdaLinesDATAArray[t, 4])
                        ? xdaLinesDATAArray[t, 4]
                        : xdaLinesDATAArray[t, 1];

                    double bedragTransactie = Val(amountToCheck);
                    bool isCor = !string.IsNullOrWhiteSpace(xdaLinesDATAArray[t, 9]) && xdaLinesDATAArray[t, 9].Length == 12;

                    string lineReference = xdaLinesDATAArray[t, 9];
                    if (string.IsNullOrWhiteSpace(lineReference))
                        lineReference = xdaLinesDATAArray[t, 10];
                    if (string.IsNullOrWhiteSpace(lineReference))
                        lineReference = xdaLinesDATAArray[t, 5];
                    if (string.IsNullOrWhiteSpace(lineReference))
                        lineReference = xdaLinesDATAArray[t, 6];
                    if (string.IsNullOrWhiteSpace(lineReference))
                        lineReference = "-";

                    string doc = string.Empty;
                    string tegenRek = "??????";
                    string oms = lineReference;

                    switch (bbaCode)
                    {
                        case "0101000":
                            bedragTransactie = -Math.Abs(bedragTransactie);
                            doc = "-";
                            oms = string.Empty;
                            break;

                        case "0103000":
                        {
                            bedragTransactie = -Math.Abs(bedragTransactie);
                            string result = CtrlDocuments(true, isCor, false, lineReference, xdaLinesDATAArray[t, 7] ?? string.Empty, amountToCheck);
                            if (string.IsNullOrWhiteSpace(result))
                            {
                                doc = "-";
                                tegenRek = "??????";
                            }
                            else
                            {
                                string[] arr = result.Split('|');
                                doc = "+" + (arr.Length > 0 ? arr[0] : string.Empty);
                                tegenRek = String99(9);
                                oms = arr.Length > 1 ? arr[1] : lineReference;
                            }
                            break;
                        }

                        case "0107000":
                        {
                            bedragTransactie = -Math.Abs(bedragTransactie);
                            string result = CtrlDocuments(true, isCor, false, lineReference, xdaLinesDATAArray[t, 7] ?? string.Empty, amountToCheck);
                            if (string.IsNullOrWhiteSpace(result))
                            {
                                MessageBox.Show("Geen resultaat", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                doc = "-";
                                tegenRek = "??????";
                            }
                            else
                            {
                                string[] arr = result.Split('|');
                                doc = "-" + (arr.Length > 0 ? arr[0] : string.Empty);
                                tegenRek = String99(10);
                                oms = arr.Length > 1 ? arr[1] : lineReference;
                            }
                            break;
                        }

                        case "0201000":
                        case "0402000":
                        case "0404000":
                        case "0501000":
                        case "0503000":
                        case "8022000":
                            bedragTransactie = -Math.Abs(bedragTransactie);
                            doc = "-";
                            tegenRek = "??????";
                            break;

                        case "0150000":
                        case "0250000":
                        {
                            bedragTransactie = Math.Abs(bedragTransactie);
                            if ((lineReference ?? string.Empty).Length > 25)
                            {
                                doc = "+";
                                tegenRek = "??????";
                            }
                            else
                            {
                                string result = CtrlDocuments(false, isCor, false, lineReference, xdaLinesDATAArray[t, 7] ?? string.Empty, amountToCheck);
                                if (string.IsNullOrWhiteSpace(result))
                                {
                                    MessageBox.Show("Geen resultaat", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    doc = "+";
                                    tegenRek = "??????";
                                }
                                else
                                {
                                    string[] arr = result.Split('|');
                                    doc = "+" + (arr.Length > 0 ? arr[0] : string.Empty);
                                    tegenRek = String99(9);
                                    oms = arr.Length > 1 ? arr[1] : lineReference;
                                }
                            }
                            break;
                        }

                        case "0254000":
                            bedragTransactie = Math.Abs(bedragTransactie);
                            doc = "+";
                            tegenRek = "??????";
                            oms = string.Empty;
                            break;

                        case "0550000":
                        {
                            bedragTransactie = Math.Abs(bedragTransactie);
                            if ((lineReference ?? string.Empty).Length > 25)
                            {
                                doc = "+";
                                tegenRek = "??????";
                            }
                            else
                            {
                                string result = CtrlDocuments(false, isCor, true, lineReference, xdaLinesDATAArray[t, 7] ?? string.Empty, amountToCheck);
                                if (string.IsNullOrWhiteSpace(result))
                                {
                                    MessageBox.Show("Geen resultaat", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    doc = "+";
                                    tegenRek = "??????";
                                }
                                else
                                {
                                    string[] arr = result.Split('|');
                                    doc = "+" + (arr.Length > 0 ? arr[0] : string.Empty);
                                    tegenRek = String99(9);
                                    oms = arr.Length > 1 ? arr[1] : lineReference;
                                }
                            }
                            break;
                        }

                        default:
                            MessageBox.Show("BBA buiten controle: " + bbaCode, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            doc = (bedragTransactie < 0 ? "-" : "+");
                            tegenRek = "??????";
                            break;
                    }

                    cumulSaldo += bedragTransactie;
                    string aa = xdaLineCounter.ToString("000") + "\t" + bbaCode + "\t" + doc + "\t"
                              + tegenRek + "\t" + Val(amountToCheck).ToString(CultureInfo.InvariantCulture)
                              + "\t" + oms + "\t" + cumulSaldo.ToString(CultureInfo.InvariantCulture);

                    MfgInsertItemBeforeLast(aa);
                }

                MessageBox.Show("TODO: give chance to edit for lines not 100% sure", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("TODO: book or [Esc]!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ListIsReady();
            }
        }

        private void Struktuur_Click(object sender, EventArgs e)
        {
            SnelHelpPrint(" ", BL_LOGGING);
            string defaultKlanten = String99(9);

            string msg = "Breng mededeling in" + Environment.NewLine
                       + "met masker nnnnnnnnnnnn" + Environment.NewLine + Environment.NewLine
                       + "Waarbij n staat voor elk" + Environment.NewLine
                       + "van de 12 verplichte cijfers" + Environment.NewLine + Environment.NewLine;

            string referteTxt = Microsoft.VisualBasic.Interaction.InputBox(msg, "Gestruktureerde betaling", "");
            if (string.IsNullOrEmpty(referteTxt))
                return;

            double dPip = Val(Left(referteTxt, 10));
            string sPip = ((int)(dPip - Math.Floor(dPip / 97d) * 97d)).ToString("00");
            if (sPip == "00") sPip = "97";
            if (sPip != Right(referteTxt, 2))
            {
                MessageBox.Show("Ongeldige invoer" + Environment.NewLine + Environment.NewLine + sPip + " <> " + Right(referteTxt, 2)
                                + Environment.NewLine + Environment.NewLine
                                + "Een gestructureerde referte heeft een kontrolesysteem.  Uw invoer is ongeldig!",
                                "Gebruikersfout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Mid(referteTxt, 8, 1) != "0")
            {
                MessageBox.Show("Geen R&V Gestruktureerde mededeling.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BGet(TABLE_CUSTOMERS, 0, Mid(referteTxt, 4, 4) + Mid(referteTxt, 9, 2));
            if (Ktrl != 0)
                return;

            RecordToVeld(TABLE_CUSTOMERS);
            string msgAmount = "Breng bedrag in voor" + Environment.NewLine
                             + "totaal van " + Left(referteTxt, 1) + " kwijtingen" + Environment.NewLine + Environment.NewLine
                             + "klant :" + Environment.NewLine + Environment.NewLine
                             + (VBibText(TABLE_CUSTOMERS, "#A100 #") + " " + VBibText(TABLE_CUSTOMERS, "#A101 #")).TrimEnd() + " "
                             + (VBibText(TABLE_CUSTOMERS, "#A125 #") + " " + VBibText(TABLE_CUSTOMERS, "#A127 #")).TrimEnd() + Environment.NewLine
                             + "Rekeningen:" + VBibText(TABLE_CUSTOMERS, "#A170 #") + " " + VBibText(TABLE_CUSTOMERS, "#v251 #");
            double dBedragTekst = Val(Microsoft.VisualBasic.Interaction.InputBox(msgAmount, "Totaal betaling", ""));
            if (dBedragTekst == 0)
                return;

            BGetOrGreater(TABLE_INVOICES, 1, "K" + VBibText(TABLE_CUSTOMERS, "#A110 #"));
            if (Ktrl != 0)
                return;

            RecordToVeld(TABLE_INVOICES);
            if (VSet(KEY_BUF[TABLE_INVOICES], 13) != VSet("K" + VBibText(TABLE_CUSTOMERS, "#A110 #"), 13))
                return;

            double dBedragKtrl = 0;
            string dummyText = string.Empty;

            do
            {
                if (Val(VBibText(TABLE_INVOICES, "#v037 #")) != Val(VBibText(TABLE_INVOICES, "#v249 #"))
                    && Left(VBibText(TABLE_INVOICES, "#v033 #"), 1) == "Q")
                {
                    if (dBedragTekst == Val(VBibText(TABLE_INVOICES, "#v249 #")))
                    {
                        double dTotaal = Val(VBibText(TABLE_INVOICES, "#v249 #")) - Val(VBibText(TABLE_INVOICES, "#v037 #"));
                        dummyText += VBibText(TABLE_INVOICES, "#v033 #") + "|" + Dec(dTotaal, MASK_EURBH) + Environment.NewLine;
                        dBedragKtrl += dTotaal;
                    }
                }

                BNext(TABLE_INVOICES);
                if (Ktrl != 0 || VSet(KEY_BUF[TABLE_INVOICES], 13) != VSet("K" + VBibText(TABLE_CUSTOMERS, "#A110 #"), 13))
                    break;
                RecordToVeld(TABLE_INVOICES);

            } while (true);

            if (dBedragKtrl != dBedragTekst)
            {
                MessageBox.Show("Opzoeking zonder succes, dokumentenstand : " + Environment.NewLine + Environment.NewLine + dummyText);
                return;
            }

            foreach (string line in dummyText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string doc = Left(line, 11);
                string amount = line.Length > 12 ? line.Substring(12) : string.Empty;

                GridText = "+" + doc + "|" + defaultKlanten + "|" + amount + "|"
                         + VSet(VBibText(TABLE_CUSTOMERS, "#A100 #"), 29) + "|" + new string(' ', 12);
                FinancieelDetail.Items.Add(GridText);

                if (bhEuro)
                {
                    lblInfo1.Text = (Val(lblInfo1.Text) + Val(Mid(GridText, 22, 12))).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    LabelInfo13.Text = Math.Round(Val(lblInfo1.Text) * EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    LabelInfo13.Text = (Val(LabelInfo13.Text) + Val(Mid(GridText, 22, 12))).ToString("#,##0.00", CultureInfo.InvariantCulture);
                    lblInfo1.Text = (Val(LabelInfo13.Text) / EURO).ToString("#,##0.00", CultureInfo.InvariantCulture);
                }
            }

            SnelHelpPrint(GridText + " met succes bijgevoegd !", BL_LOGGING);
        }

        private void ButtonAssign_Click(object sender, EventArgs e)
        {
            if (mfgLijst.CurrentCell == null) return;

            using (var detail = new DetailInfo())
            {
                GridText = string.Empty;
                detail.ShowDialog(this);
            }

            if (string.IsNullOrEmpty(GridText))
                return;

            int row = MfgCurrentRow;
            double valueToCheck = Math.Abs(Val(Mid(GridText, 22, 12)));
            double valueLineToCheck = Val(MfgTextMatrix(row, 4));

            if (valueToCheck != valueLineToCheck)
            {
                string msg = "Bedrag van de verrichting mag niet gewijzigd worden." + Environment.NewLine
                           + "Herstart de toewijzing";
                MessageBox.Show(msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string[] resultArray = GridText.Split('|');
            MfgSetTextMatrix(row, 2, resultArray.Length > 0 ? resultArray[0] : string.Empty);
            MfgSetTextMatrix(row, 3, resultArray.Length > 1 ? resultArray[1] : string.Empty);
            MfgSetTextMatrix(row, 4, resultArray.Length > 2 ? Val(resultArray[2]).ToString(CultureInfo.InvariantCulture) : "0");
            MfgSetTextMatrix(row, 5, resultArray.Length > 3 ? resultArray[3] : string.Empty);
            ListIsReady();
        }

        private void ButtonTransfer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Overnemen", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
