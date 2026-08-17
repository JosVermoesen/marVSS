using ADODB;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.SharedForms.FormXLog;

using marVSS2028.Classes;
using marVSS2028.PublicForms;
using marVSS2028.SharedForms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.PeppolTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.DailyManagement
{
    public partial class FormBuying : Form
    {
        private bool _documentRefIsOGM;
        private string _documentKey = "";
        private string _documentOGM = "";
        private string _documentOGMNoFormat = "";

        private bool _adminNoVat;
        private bool _ifSupplierInsertWarning;

        private Recordset _rsDocuments = new Recordset();
        private Recordset _rsSupplier = new Recordset();

        private string _supplierA110 = "";
        private string _lvLeverancier = "";
        private string _geScanBestand = "";

        private string _priveRekening = "";
        private string _leverancierRekening = "";
        private readonly string[] _grensDetail = new string[4];

        private int _aankoopFlg;
        private readonly string[] _rbtwVak = new string[11];
        private string _sMuntLever = "";
        private string _sIsIntraFlg = "";

        private bool _startBlad;
        private int _ar;

        private string _veldRekening = "";
        private string _veldNaam = "";
        private string _veldBedrag = "";

        private string _supplierCompanyId = "";
        private string _supplierCountryCode = "";
        private string _supplierVatNumber = "";

        private int _positie;
        private bool _suppressMedekontraktantUpdate;

        private readonly RadioButton[] _aankoopOpties;
        private readonly RadioButton[] _obTab;
        private readonly Control[] _tekstInfo;

        public FormBuying()
        {
            InitializeComponent();

            _aankoopOpties = new[] { AankoopOptie0, AankoopOptie1, AankoopOptie2 };
            _tekstInfo = new Control[]
            {
                TekstInfo0, TekstInfo1, TekstInfo2, TekstInfo3, TekstInfo4,
                TekstInfo5, TekstInfo6, TekstInfo7, TekstInfo9, TekstInfo10, TekstInfo12
            };

            BindEvents();
        }

        private void BindEvents()
        {
            Load += FormBuying_Load;
            Resize += FormBuying_Resize;
            WireHighlightEvents(this);

            AankoopDetail.DoubleClick += AankoopDetail_DoubleClick;
            AankoopDetail.Enter += AankoopDetail_Enter;
            AankoopDetail.KeyDown += AankoopDetail_KeyDown;
            AankoopDetail.KeyPress += AankoopDetail_KeyPress;

            for (int i = 0; i < _aankoopOpties.Length; i++)
            {
                int index = i;
                _aankoopOpties[i].CheckedChanged += (sender, e) =>
                {
                    if (_aankoopOpties[index].Checked)
                        AankoopOptie_Click(index);
                };
            }

            ButtonControleIt.Click += ButtonControleIt_Click;
            ButtonBookIt.Click += ButtonBookIt_Click;
            Annuleren.Click += Annuleren_Click;
            ButtonBookIt.Leave += ButtonBookIt_LostFocus;
            ButtonOptimize.Click += ButtonOptimize_Click;
            Schoonvegen.Click += SchoonVegen_Click;
            cbCheckTools.Click += cbCheckTools_Click;
            cbImportUBL.Click += cbImportUBL_Click;
            cmdSQLInfo.Click += cmdSQLInfo_Click;
            cmdXLog.Click += cmdXLog_Click;
            Medekontraktant.CheckedChanged += Medekontraktant_Click;
            StockBeheer.CheckedChanged += StockBeheer_Click;
            TekstInfo5.TextChanged += TekstInfo5_TextChanged;

            ListView1.Click += ListView1_Click;
            ListView1.KeyDown += ListView1_KeyDown;
            SSTab1.SelectedIndexChanged += SSTab1_SelectedIndexChanged;
            SSTab1.KeyDown += SSTab1_KeyDown;

            WireTextField(TekstInfo0, TekstInfo0_KeyDown, TekstInfo0_Enter, TekstInfo0_Leave, TekstInfo0_KeyPress, null);
            WireTextField(TekstInfo1, TekstInfo1_KeyDown, TekstInfo1_Enter, TekstInfo1_Leave, null, null);
            WireTextField(TekstInfo2, TekstInfo2_KeyDown, TekstInfo2_Enter, TekstInfo2_Leave, null, null);
            WireTextField(TekstInfo3, TekstInfo3_KeyDown, TekstInfo3_Enter, TekstInfo3_Leave, TekstInfo3_KeyPress, null);
            WireTextField(TekstInfo4, TekstInfo4_KeyDown, null, TekstInfo4_Leave, null, null);
            WireTextField(TekstInfo5, TekstInfo5_KeyDown, null, TekstInfo5_Leave, null, TekstInfo5_KeyUp);
            WireTextField(TekstInfo6, null, TekstInfo6_Enter, null, null, null);
            WireTextField(TekstInfo7, TekstInfo7_KeyDown, null, null, null, null);
            WireTextField(TekstInfo9, TekstInfo9_KeyDown, null, null, null, null);
            WireTextField(TekstInfo10, TekstInfo10_KeyDown, TekstInfo10_Enter, TekstInfo10_Leave, TekstInfo10_KeyPress, null);
            WireTextField(TekstInfo12, TekstInfo12_KeyDown, null, null, null, null);

            // cbImportUBL.UseMnemonic = true;
            // cbImportUBL.Text = "&UBL B2B IN";
        }

        private void ShowUitwisselingArraysDebug()
        {
            string[] oms = uitwisselingOMSArray ?? (uitwisselingOMS ?? string.Empty).Split('\t');
            string[] data = uitwisselingDATAArray ?? (uitwisselingDATA ?? string.Empty).Split('\t');

            var sb = new StringBuilder();
            sb.AppendLine("uitwisselingOMSArray:");
            for (int i = 0; i < oms.Length; i++)
            {
                sb.AppendLine($"[{i}] = {oms[i]}");
            }

            sb.AppendLine();
            sb.AppendLine("uitwisselingDATAArray:");
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendLine($"[{i}] = {data[i]}");
            }

            MessageBox.Show(sb.ToString(), "UBL debug arrays", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormBuying_Load(object sender, EventArgs e)
        {
            purchasePeppolTODOShowed = false;
            _documentRefIsOGM = String99(100) == "1";
            _adminNoVat = (String99(20) ?? string.Empty).Trim() == "7";

            string companyPath = LOCATION_COMPANYDATA ?? string.Empty;
            if (companyPath.EndsWith("\\098\\", StringComparison.OrdinalIgnoreCase) || companyPath.EndsWith("\\099\\", StringComparison.OrdinalIgnoreCase))
            {
                TextBoxWarningTestCompany.Visible = true;
            }

            SSTab1.SelectedIndex = 1;
            Ktrl = 1;
            if (Ktrl == 0)
            {
                AutoUnLoadCompany();
                Close();
                return;
            }

            ListView1.View = View.Details;
            Cursor.Current = Cursors.WaitCursor;
            Top = 0;
            Left = 0;

            for (int tel = 16; tel <= 19; tel++)
            {
                _rbtwVak[tel - 16] = String99(tel);
                _rbtwVak[tel - 12] = String99(tel + 6);
            }

            _leverancierRekening = String99(10);
            _sIsIntraFlg = String99(200);
            _priveRekening = String99(145);

            _grensDetail[0] = VSet(String99(148), 7) + VSet(String99(149), 7);
            _grensDetail[1] = VSet(String99(146), 7) + VSet(String99(147), 7);
            _grensDetail[2] = VSet(String99(150), 7) + VSet(String99(151), 7);
            _grensDetail[3] = VSet(String99(152), 7) + VSet(String99(153), 7);

            Msg = LaadTekst("DirekteAankoop", "startBlad");
            if (string.IsNullOrWhiteSpace(Msg)) Msg = "True";
            _startBlad = Msg.Trim().Equals("True", StringComparison.OrdinalIgnoreCase) || Msg.Trim() == "-1" || Msg.Trim() == "1";

            SyncDocumentKeyFromOption(0);
            Schoon();
            Cursor.Current = Cursors.Default;

            if (!purchasePeppolTODOShowed)
            {
                cbImportUBL_Click(sender, e);
            }
        }

        private void FormBuying_Resize(object sender, EventArgs e)
        {
            AankoopDetail.Width = Width - 25;
            SSTab1.Focus();
            if (_ifSupplierInsertWarning && BasisB[TABLE_SUPPLIERS] != null)
            {
                BasisB[TABLE_SUPPLIERS].Focus();
            }
        }

        private void WireTextField(Control control, KeyEventHandler keyDown, EventHandler enter, EventHandler leave, KeyPressEventHandler keyPress, KeyEventHandler keyUp)
        {
            if (keyDown != null) control.KeyDown += keyDown;
            if (enter != null) control.Enter += enter;
            if (leave != null) control.Leave += leave;
            if (keyPress != null) control.KeyPress += keyPress;
            if (keyUp != null) control.KeyUp += keyUp;
        }

        private static string FieldText(Recordset rs, string fieldName)
        {
            object value = rs.Fields[fieldName].Value;
            return value == null || value == DBNull.Value ? string.Empty : value.ToString().Trim();
        }

        private static int MsgBoxResult(DialogResult result)
        {
            return result == DialogResult.Yes ? 6 : 7;
        }

        private static string ReplaceDateSeparators(string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }

        private static string NormalizeVatOrCompany(string value)
        {
            return (value ?? string.Empty).Trim().Replace(" ", string.Empty);
        }

        private static double ParseDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return result;
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;
            return 0;
        }

        private static string GetSafeArrayValue(string[] array, int index)
        {
            if (array == null || index < 0 || index >= array.Length)
                return string.Empty;
            return array[index] ?? string.Empty;
        }

        private void FocusByPerDat()
        {
            if (Application.OpenForms["FormBYPERDAT"] is FormBYPERDAT byperdat)
            {
                byperdat.WindowState = FormWindowState.Normal;
                byperdat.BringToFront();
                byperdat.CmbPeriodeBoekjaar.Focus();
            }
        }

        private void ShowPurchaseLineEditor(bool openB2BDetailsOnLoad)
        {
            using (var editor = new FormPurchaseLineEditor())
            {
                editor.StockBeheerChecked = StockBeheer.Checked;
                editor.OpenB2BDetailsOnLoad = openB2BDetailsOnLoad;
                editor.SelectedDocumentLineIndex = Math.Max(0, _positie);
                editor.ShowDialog(this);
            }
        }

        private FormPurchasePeppolMonitor GetPeppolMonitor()
        {
            FormPurchasePeppolMonitor monitor = Application.OpenForms["FormPurchasePeppolMonitor"] as FormPurchasePeppolMonitor;
            if (monitor == null || monitor.IsDisposed)
            {
                monitor = new FormPurchasePeppolMonitor();
            }
            return monitor;
        }

        private string GetSelectedSupplierCode()
        {
            if (!string.IsNullOrWhiteSpace(_supplierA110))
                return _supplierA110;

            string caption = LeverancierInfo.Text ?? string.Empty;
            if (caption.Length >= 12)
                return caption.Substring(0, 12).Trim();

            return string.Empty;
        }

        private string BuildLeverancierInfoCaption()
        {
            string lever = Environment.NewLine + VBibText(TABLE_SUPPLIERS, "#A100 #")
                + Environment.NewLine + VBibText(TABLE_SUPPLIERS, "#A125 #")
                + Environment.NewLine + VBibText(TABLE_SUPPLIERS, "#A104 #")
                + Environment.NewLine + VBibText(TABLE_SUPPLIERS, "#A109 #") + " " + VBibText(TABLE_SUPPLIERS, "#A107 #") + " " + VBibText(TABLE_SUPPLIERS, "#A108 #");

            string landnummer = VBibText(TABLE_SUPPLIERS, "#v149 #");
            string a110 = VSet(VBibText(TABLE_SUPPLIERS, "#A110 #"), 12);

            if (string.IsNullOrWhiteSpace(landnummer))
                return string.Empty;
            if (landnummer == "002")
                return a110 + "* Binnenland * " + lever;
            if ((SISO ?? string.Empty).Contains(landnummer))
                return a110 + "* E.U. Verschuldigde BTW * " + lever;
            return a110 + "* Niet E.U. + BTW ! *" + lever;
        }

        private void ConfigureSupplierFlagsAndCaption()
        {
            string landnummer = VBibText(TABLE_SUPPLIERS, "#v149 #");
            if (string.IsNullOrWhiteSpace(landnummer))
            {
                MessageBox.Show("Landnummer is verplicht !", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (landnummer == "002")
            {
                LeverancierInfo.Text = BuildLeverancierInfoCaption();
                _aankoopFlg = 0;
                Medekontraktant.Enabled = true;
                TekstInfo7.Visible = false;
                TekstInfo7.Text = TekstInfo5.Text;
                Label1_12.Visible = false;
            }
            else if ((SISO ?? string.Empty).Contains(landnummer))
            {
                LeverancierInfo.Text = BuildLeverancierInfoCaption();
                _aankoopFlg = 1;
                Medekontraktant.Enabled = false;
                TekstInfo7.Visible = true;
                TekstInfo7.Text = "0";
                Label1_12.Visible = true;
            }
            else
            {
                LeverancierInfo.Text = BuildLeverancierInfoCaption();
                _aankoopFlg = 2;
                Medekontraktant.Enabled = false;
                TekstInfo7.Visible = true;
                TekstInfo7.Text = "0";
                Label1_12.Visible = true;
            }
        }

        private void EnableDocumentFields(bool enabled)
        {
            TekstInfo0.Enabled = enabled;
            TekstInfo1.Enabled = enabled;
            TekstInfo2.Enabled = enabled;
            TekstInfo3.Enabled = enabled;
            TekstInfo4.Enabled = enabled;
            TekstInfo12.Enabled = enabled;
            TekstInfo5.Enabled = enabled;
            TekstInfo6.Enabled = enabled;
            TekstInfo7.Enabled = enabled;
            TekstInfo10.Enabled = enabled;
        }

        private void ApplySupplierAccountDefaults()
        {
            string leverancierBtwRekening = VBibText(TABLE_SUPPLIERS, "#v162 #");
            if (VSet(leverancierBtwRekening, 3) == "440")
            {
                TekstInfo3.Text = leverancierBtwRekening;
                BGet(TABLE_LEDGERACCOUNTS, 0, VSet(leverancierBtwRekening, 7));
                if (Ktrl != 0)
                {
                    TekstInfo3.Text = _leverancierRekening;
                }
            }
            else
            {
                TekstInfo3.Text = _leverancierRekening;
            }

            TekstInfo10.Text = _rbtwVak[4];
            TekstInfo10.Enabled = true;
        }

        private void ApplyCurrencyDefaults(bool fromUbl)
        {
            _sMuntLever = (VBibText(TABLE_SUPPLIERS, "#vs03 #") ?? string.Empty).ToUpperInvariant();
            BGet(TABLE_VARIOUS, 1, VSet("10" + _sMuntLever, 20));
            if (Ktrl != 0)
            {
                MessageBox.Show("Dagkoers voor muntkode " + _sMuntLever + " niet te vinden !  Eerst aanmaken via gebruikersfiches a.u.b.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _sMuntLever = bhEuro ? "EUR" : "BEF";
                TekstInfo9.Text = Dec(1, "###.########");
            }
            else
            {
                RecordToVeld(TABLE_VARIOUS);
                TekstInfo9.Text = Dec(ParseDouble(VBibText(TABLE_VARIOUS, "#v040 #")), "###.########");
            }

            if (fromUbl)
            {
                if (bhEuro)
                {
                    if (_sMuntLever == "BEF")
                        TekstInfo9.Text = Dec(1 / EURO, "##0.########");
                    else
                        TekstInfo9.Text = Dec(1, "##0.########");
                }
                else
                {
                    if (_sMuntLever == "EUR")
                        TekstInfo9.Text = Dec(1 / EURO, "##0.########");
                    else if (_sMuntLever == "BEF")
                        TekstInfo9.Text = Dec(1, "##0.########");
                }
            }
            else
            {
                TekstInfo9.Text = Dec(1, "##0.########");
            }

            dMuntL = ParseDouble(TekstInfo9.Text);
        }

        private void ApplySupplierFinancialFlags()
        {
            _suppressMedekontraktantUpdate = true;
            Medekontraktant.Checked = VBibText(TABLE_SUPPLIERS, "#v151 #") == "1";
            StockBeheer.Checked = VBibText(TABLE_SUPPLIERS, "#v163 #") == "1";
            _suppressMedekontraktantUpdate = false;
        }

        private void RefreshReference()
        {
            string groupCode = VSet(_documentKey, 2) == "A0" ? "4" : "3";
            string referteTxtNoFormat = groupCode + PartMid(_documentKey, 3, 2) + PartMid(_documentKey, 5, 4) + PartMid(_documentKey, 9, Math.Max(0, _documentKey.Length - 8)) + "xx";
            string referteTxt = "+++" + groupCode + PartMid(_documentKey, 3, 2) + "/" + PartMid(_documentKey, 5, 4) + "/" + PartMid(_documentKey, 9, Math.Max(0, _documentKey.Length - 8)) + "xx+++";

            double dPip = ParseDouble(PartMid(referteTxt, 4, 3) + PartMid(referteTxt, 8, 4) + PartMid(referteTxt, 13, 3));
            string chk = ((int)(dPip - Math.Floor(dPip / 97d) * 97d)).ToString("00");
            if (chk == "00") chk = "97";

            StringBuilder builder = new StringBuilder(referteTxt);
            builder.Remove(15, 2).Insert(15, chk);
            referteTxt = builder.ToString();

            builder = new StringBuilder(referteTxtNoFormat);
            builder.Remove(10, 2).Insert(10, chk);
            referteTxtNoFormat = builder.ToString();

            _documentOGM = referteTxt;
            _documentOGMNoFormat = referteTxtNoFormat;
            LabelDocumentReference.Text = "Referte: " + _documentKey + " of " + _documentOGM;
        }

        private void SyncDocumentKeyFromOption(int index)
        {
            switch (index)
            {
                case 0:
                    _ar = 1;
                    break;
                case 1:
                    _ar = 3;
                    break;
                default:
                    _aankoopOpties[0].Checked = true;
                    _ar = 1;
                    break;
            }

            _documentKey = SleutelDok(_ar);
            RefreshReference();
            Text = "Direkte aankoopverrichting         (" + _documentKey + ")";
        }

        private void ParseToArray(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                Globals.documentLinesDATAArray = new string[0, 0];
                return;
            }

            // Split into lines by CRLF
            string[] lines = inputText.Split(new[] { "\r\n" }, StringSplitOptions.None);

            // Find maximum number of columns
            int maxCols = 0;
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                if (fields.Length > maxCols)
                    maxCols = fields.Length;
            }

            // Dimension result array: rows = number of lines, cols = max fields
            Globals.documentLinesDATAArray = new string[lines.Length, maxCols];

            // Fill the array
            for (int i = 0; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split('\t');
                for (int j = 0; j < fields.Length; j++)
                {
                    Globals.documentLinesDATAArray[i, j] = fields[j] ?? "";
                }
                // Fill remaining cols with empty strings
                for (int j = fields.Length; j < maxCols; j++)
                {
                    Globals.documentLinesDATAArray[i, j] = "";
                }
            }
        }

        private bool MoveToSelectedSupplierRecord(string supplierCode)
        {
            if (string.IsNullOrWhiteSpace(supplierCode))
                return false;

            BGet(TABLE_SUPPLIERS, 0, supplierCode);
            if (Ktrl != 0)
                return false;

            RecordToVeld(TABLE_SUPPLIERS);
            return true;
        }

        private void OpenSupplierSearch(string initialValue)
        {
            aIndex = 1;
            SharedFl = TABLE_SUPPLIERS;
            GridText = initialValue ?? string.Empty;
            using (var sqlSearch = new FormSearchSQL())
            {
                sqlSearch.ShowDialog(this);
            }
        }

        private void SelectAllText(Control control, int selectionLength = -1)
        {
            TextBoxBase textBox = control as TextBoxBase;
            if (textBox == null)
                return;
            textBox.SelectionStart = 0;
            textBox.SelectionLength = selectionLength >= 0 ? Math.Min(selectionLength, textBox.TextLength) : textBox.TextLength;
        }

        private static void SetControlEnabledIfPresent(Form form, string controlName, bool enabled)
        {
            Control[] matches = form.Controls.Find(controlName, true);
            if (matches.Length > 0)
            {
                matches[0].Enabled = enabled;
            }
        }

        private bool CheckSupplierDocuments(string supplierCode)
        {
            bool checkSupplierDocuments = false;
            Recordset rsAny = new Recordset();

            try
            {
                rsAny.CursorLocation = CursorLocationEnum.adUseClient;
                Msg = "SELECT A110, v150, A161, v404, v410, A100 FROM Leveranciers WHERE A110 = '" + supplierCode + "'";
                SnelHelpPrint(Msg, BL_LOGGING);
                Cursor.Current = Cursors.WaitCursor;
                rsAny.Open(Msg, adntDB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);
                Cursor.Current = Cursors.Default;

                if (rsAny.RecordCount == 1)
                {
                    string valueInRecordV410 = FieldText(rsAny, "v410");
                    string v404 = FieldText(rsAny, "v404");

                    if (string.IsNullOrWhiteSpace(valueInRecordV410) || CheckBoxAlwaysPeppolRefresh.Checked)
                    {
                        string checkWithVatNumber = "0208:" + v404;
                        valueInRecordV410 = CheckPeppolRegistration(checkWithVatNumber);
                        if (valueInRecordV410.Length < 300)
                        {
                            Msg = "Gecontroleerd met code: " + checkWithVatNumber + Environment.NewLine + Environment.NewLine;
                            Msg += "Mogelijk geen Peppol Registratie" + Environment.NewLine + Environment.NewLine;
                            Msg += "Tot slot controleren met verouderde 9925:BE";
                            MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            checkWithVatNumber = "9925:BE" + v404;
                            valueInRecordV410 = CheckPeppolRegistration(checkWithVatNumber);
                        }

                        checkSupplierDocuments = valueInRecordV410.Length >= 300;
                        rsAny.Fields["v410"].Value = valueInRecordV410;
                        rsAny.Update();
                    }
                    else if (valueInRecordV410.Length > 500)
                    {
                        checkSupplierDocuments = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Bron:" + Environment.NewLine + ex.Source + Environment.NewLine + Environment.NewLine + "Detail:" + Environment.NewLine + ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                try { if (rsAny.State != (int)ObjectStateEnum.adStateClosed) rsAny.Close(); } catch { }
            }

            return checkSupplierDocuments;
        }

        private void InstalLeverancier()
        {
            cbImportUBL.Enabled = false;
            AankoopDetail.Enabled = true;

            string ibanField = NormalizeVatOrCompany(VBibText(TABLE_SUPPLIERS, "#v259 #"));
            if (ibanField.Length == 16)
            {
                TextInfoSellersIBAN.Text = ibanField.Substring(0, 4) + " " + ibanField.Substring(4, 4) + " " + ibanField.Substring(8, 4) + " " + ibanField.Substring(12, 4);
            }
            else
            {
                TextInfoSellersIBAN.Text = ibanField;
            }

            _supplierA110 = VBibText(TABLE_SUPPLIERS, "#A110 #").Trim();
            Schoonvegen.Enabled = true;

            ConfigureSupplierFlagsAndCaption();

            if (!string.IsNullOrWhiteSpace(VBibText(TABLE_SUPPLIERS, "#A161 #")))
            {
                _supplierCompanyId = VBibText(TABLE_SUPPLIERS, "#v404 #").Trim();
                _supplierCountryCode = VBibText(TABLE_SUPPLIERS, "#v150 #").Trim();
                _supplierVatNumber = VBibText(TABLE_SUPPLIERS, "#A161 #").Trim();
            }

            EnableDocumentFields(true);
            ApplyCurrencyDefaults(false);
            ApplySupplierFinancialFlags();
            ApplySupplierAccountDefaults();

            string v016 = VBibText(TABLE_SUPPLIERS, "#v016 #");
            if (!string.IsNullOrWhiteSpace(v016) && AankoopDetail.Items.Count == 0)
            {
                RasterSchoon();
                GridText = v016 + "||" + "|";
                using (var wijzigen = new FormPurchaseLineEditor())
                    wijzigen.ShowDialog(this);
                if (!string.IsNullOrEmpty(GridText) && GridText.Length >= 7 && RekeningOK(GridText.Substring(0, 7)))
                {
                    AankoopDetail.Items.Add(GridText);
                    InvestKtrl();
                }
            }

            string verval = VBibText(TABLE_SUPPLIERS, "#vs04 #");
            TekstInfo2.Text = VValdag(TekstInfo0.Text, verval);
            ApplyStructuredReferenceMask(true);
            TekstInfo4.Text = VBibText(TABLE_SUPPLIERS, "#v412 #");
            if (!string.IsNullOrWhiteSpace(TekstInfo4.Text))
            {
                TekstInfo4.Enabled = false;
            }

            cmdSQLInfo.Visible = !string.IsNullOrWhiteSpace(VBibText(TABLE_SUPPLIERS, "#v254 #"));
            cmdXLog.Enabled = true;
            LabelInfoXlog.Visible = false;
            SSTab1.SelectedIndex = 0;

            if (!string.IsNullOrWhiteSpace(_supplierCompanyId) && _supplierCountryCode == "BE")
            {
                bool beSupplierPeppolCheck = CheckSupplierDocuments(_supplierA110);
                if (beSupplierPeppolCheck)
                {
                    MessageBox.Show("Deze BE Leverancier dient via Peppol te werken" + Environment.NewLine + Environment.NewLine + "In principe mag je geen aankoopdocumenten MANUEEL inbrengen", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            ButtonControleIt.Enabled = true;
            AankoopDetail.Focus();
        }

        private void InstalSupplierForUBL()
        {
            cbImportUBL.Enabled = false;
            AankoopDetail.Enabled = true;

            _supplierA110 = VBibText(TABLE_SUPPLIERS, "#A110 #").Trim();
            Schoonvegen.Enabled = true;

            ConfigureSupplierFlagsAndCaption();

            if (!string.IsNullOrWhiteSpace(VBibText(TABLE_SUPPLIERS, "#A161 #")))
            {
                _supplierCompanyId = VBibText(TABLE_SUPPLIERS, "#v404 #").Trim();
                _supplierCountryCode = VBibText(TABLE_SUPPLIERS, "#v150 #").Trim();
                _supplierVatNumber = VBibText(TABLE_SUPPLIERS, "#A161 #").Trim();
            }

            EnableDocumentFields(true);
            ApplyCurrencyDefaults(true);
            ApplySupplierFinancialFlags();
            ApplySupplierAccountDefaults();
            ApplyStructuredReferenceMask(false);

            cmdSQLInfo.Visible = !string.IsNullOrWhiteSpace(VBibText(TABLE_SUPPLIERS, "#v254 #"));
            cmdXLog.Enabled = true;
            LabelInfoXlog.Visible = false;
            SSTab1.SelectedIndex = 0;

            TekstInfo1.Text = DateText(ReplaceDateSeparators(GetSafeArrayValue(uitwisselingDATAArray, 2)));
            TekstInfo0.Text = TekstInfo1.Text;
            
            if (uitwisselingDATAArray[3] == "")
            {
                TekstInfo2.Text = TekstInfo0.Text;
            }
            else {
                TekstInfo2.Text = DateText(ReplaceDateSeparators(GetSafeArrayValue(uitwisselingDATAArray, 3)));
            }
            TekstInfo4.Text = GetSafeArrayValue(uitwisselingDATAArray, 14);
            TekstInfo12.Text = GetSafeArrayValue(uitwisselingDATAArray, 1);

            if (_adminNoVat)
            {
                TekstInfo5.Text = "0";
            }
            else if (_aankoopFlg == 0)
            {
                TekstInfo5.Text = (ParseDouble(GetSafeArrayValue(uitwisselingDATAArray, 21)) - ParseDouble(GetSafeArrayValue(uitwisselingDATAArray, 20))).ToString(CultureInfo.CurrentCulture);
                TekstInfo5.Enabled = false;
            }
            else
            {
                MessageBox.Show("EU aankoopdocument. Btw standaard op 21%. Verbeter indien nodig.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                TekstInfo5.Text = Dec(ParseDouble(GetSafeArrayValue(uitwisselingDATAArray, 20)) * 21d / 100d, ".##");
                TekstInfo7.Text = TekstInfo5.Text;
                TekstInfo5.Enabled = true;
                TekstInfo7.Enabled = false;
            }

            TekstInfo6.Text = GetSafeArrayValue(uitwisselingDATAArray, 21);
            TekstInfo1.Enabled = false;
            TekstInfo2.Enabled = false;
            TekstInfo4.Enabled = false;
            TekstInfo12.Enabled = false;
            TekstInfo6.Enabled = false;

            string ibanField = NormalizeVatOrCompany(VBibText(TABLE_SUPPLIERS, "#v259 #"));
            string ibanFieldCheck = NormalizeVatOrCompany(GetSafeArrayValue(uitwisselingDATAArray, 15));
            if (ibanFieldCheck.Length == 16)
            {
                TextInfoSellersIBAN.Text = ibanFieldCheck.Substring(0, 4) + " " + ibanFieldCheck.Substring(4, 4) + " " + ibanFieldCheck.Substring(8, 4) + " " + ibanFieldCheck.Substring(12, 4);
                if (ibanField != ibanFieldCheck)
                {
                    TextWarningIBAN.Visible = true;
                }
            }

            documentLinesOMSArray = (documentLinesOMS ?? string.Empty).Split('\t');
            ParseToArray(documentLinesDATA);

            // Calculate square difference for reconciliation
            double squareCtrl = 0;

            if (Globals.documentLinesDATAArray != null && Globals.documentLinesDATAArray.GetLength(0) > 2)
            {
                int lowerBound = 1;  // Skip header row
                int upperBound = Globals.documentLinesDATAArray.GetLength(0) - 1;  // Skip trailer row

                for (int i = lowerBound; i < upperBound; i++)
                {
                    if (_adminNoVat && Globals.documentLinesDATAArray.GetLength(1) > 8)
                    {
                        double lineAmount = double.TryParse(Globals.documentLinesDATAArray[i, 7], out var amt) ? amt : 0;
                        double vatPercent = double.TryParse(Globals.documentLinesDATAArray[i, 8], out var vat) ? vat : 0;
                        double lineValue = lineAmount * (1 + vatPercent / 100);
                        squareCtrl += lineValue;
                    }
                    else if (Globals.documentLinesDATAArray.GetLength(1) > 7)
                    {
                        double lineAmount = double.TryParse(Globals.documentLinesDATAArray[i, 7], out var amt) ? amt : 0;
                        squareCtrl += lineAmount;
                    }
                }
            }

            string checkSquareStr = _adminNoVat ?
                (GetSafeArrayValue(uitwisselingDATAArray, 21) ?? "0") :
                (GetSafeArrayValue(uitwisselingDATAArray, 20) ?? "0");

            double checkSquare = double.TryParse(checkSquareStr, out var chk) ? chk : 0;
            checkSquare = ParseDouble(Dec(checkSquare, MASK_EURBH));
            squareCtrl = ParseDouble(Dec(squareCtrl, MASK_EURBH));
            double squareDifference = checkSquare - squareCtrl;
            squareDifference = ParseDouble(Dec(squareDifference, MASK_EURBH));


            for (int i = 0; i < uitwisselingDATAArray.Length; i++)
            {
                Console.WriteLine(i + " " + uitwisselingOMSArray[i] + ": " + uitwisselingDATAArray[i]);
            }             

            if (squareDifference != 0)
            {
                string msg = "Rekenverschil bij vierkantscontrole" + Environment.NewLine + Environment.NewLine;
                msg += "Controle der lijnen geeft: " + squareCtrl + Environment.NewLine;

                if (_adminNoVat)
                {
                    msg += "en totaal incl. BTW in UBL document geeft: " + GetSafeArrayValue(uitwisselingDATAArray, 21) + Environment.NewLine + Environment.NewLine;
                    msg += "Administratie zonder BTW plicht" + Environment.NewLine;
                }
                else
                {
                    msg += "en totaal excl. BTW in UBL document geeft: " + GetSafeArrayValue(uitwisselingDATAArray, 20) + Environment.NewLine + Environment.NewLine;
                    msg += "Administratie met BTW plicht" + Environment.NewLine + Environment.NewLine;
                    if (squareDifference > 0.9)
                    {
                        msg += "Het rekenverschil is meer dan een afrondingsverschil." + Environment.NewLine + Environment.NewLine;
                        msg += "Mogelijk: globale korting, globale verzendkost e.d." + Environment.NewLine;
                        msg += "Keer terug en klik XML Tonen voor inzage." + Environment.NewLine + Environment.NewLine;
                        msg += "Boeken op aparte rekeningen? Contacteer ons!" + Environment.NewLine + Environment.NewLine;
                    }                    
                }
                msg += "Verschil wordt verrekend in (eerste) kostenpost.";
                // MessageBox.Show(msg, "Vierkantscontrole der lijnen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Populate AankoopDetail with lines
            string supplierAccount = VBibText(TABLE_SUPPLIERS, "#v016 #").Trim();
            if (string.IsNullOrWhiteSpace(supplierAccount))
            {
                supplierAccount = "604000";
            }

            if (AankoopDetail.Items.Count == 0 && Globals.documentLinesDATAArray != null && Globals.documentLinesDATAArray.GetLength(0) > 2)
            {
                // Clear the grid
                AankoopDetail.Items.Clear();

                int lowerBound = 1;  // Skip header row
                int upperBound = Globals.documentLinesDATAArray.GetLength(0) - 1;  // Skip trailer row

                for (int i = lowerBound; i < upperBound; i++)
                {
                    double lineValue;
                    if (_adminNoVat && Globals.documentLinesDATAArray.GetLength(1) > 8)
                    {
                        double lineAmount = double.TryParse(Globals.documentLinesDATAArray[i, 7], out var amt) ? amt : 0;
                        double vatPercent = double.TryParse(Globals.documentLinesDATAArray[i, 8], out var vat) ? vat : 0;
                        lineValue = lineAmount * (1 + vatPercent / 100);
                    }
                    else if (Globals.documentLinesDATAArray.GetLength(1) > 7)
                    {
                        lineValue = double.TryParse(Globals.documentLinesDATAArray[i, 7], out var amt) ? amt : 0;
                    }
                    else
                    {
                        continue;
                    }

                    if (squareDifference != 0)
                    {
                        lineValue += squareDifference;
                        squareDifference = 0;
                    }

                    string lineStringValue = Dec(lineValue, MASK_EURBH);
                    string fieldName = (Globals.documentLinesDATAArray.GetLength(1) > 3) ? (Globals.documentLinesDATAArray[i, 3] ?? "") : "";

                    if (fieldName != "-" && !string.IsNullOrWhiteSpace(fieldName))
                    {
                        fieldName = (fieldName.Length > 40 ? fieldName.Substring(0, 40) : fieldName.PadRight(40));
                    }
                    else
                    {
                        fieldName = (Globals.documentLinesDATAArray.GetLength(1) > 4) ? (Globals.documentLinesDATAArray[i, 4] ?? "") : "";
                        fieldName = (fieldName.Length > 40 ? fieldName.Substring(0, 40) : fieldName.PadRight(40));
                    }

                    string gridText = (supplierAccount.Length > 7 ? supplierAccount.Substring(0, 7) : supplierAccount.PadRight(7)) + "|" + fieldName + "|" + lineStringValue + "|";

                    if (RekeningOK(supplierAccount))
                    {
                        AankoopDetail.Items.Add(gridText);
                    }
                }
            }

            if (GetSafeArrayValue(uitwisselingDATAArray, 4) == "380")
            {
                // Set document type to Facturering (Invoice)
                try
                {
                    var factureringsControl = this.Controls.Find("Facturering", true);
                    if (factureringsControl.Length > 0 && factureringsControl[0] is RadioButton rb)
                        rb.Checked = true;
                }
                catch { }
            }
            else if (GetSafeArrayValue(uitwisselingDATAArray, 4) == "381")
            {
                // Set document type to Creditnota (Credit Note)
                try
                {
                    var creditnotaControl = this.Controls.Find("Creditnota", true);
                    if (creditnotaControl.Length > 0 && creditnotaControl[0] is RadioButton rb)
                        rb.Checked = true;
                }
                catch { }
            }

            // Enable the control button
            try
            {
                var controleControl = this.Controls.Find("ButtonControleIt", true);
                if (controleControl.Length > 0 && controleControl[0] is Button btn)
                    btn.Enabled = true;
            }
            catch { }

            AankoopDetail.Focus();
        }

        private void InstalleerRecenteCrediteuren()
        {
            Cursor.Current = Cursors.WaitCursor;
            Recordset rsRecent = new Recordset();
            try
            {
                rsRecent.CursorLocation = CursorLocationEnum.adUseClient;
                rsRecent.Open("SELECT DISTINCT Leveranciers.A100 AS [Naam], Leveranciers.A110 AS [idCode] FROM Leveranciers, Dokumenten WHERE  'L'+Leveranciers.A110 =  Dokumenten.v034 AND Dokumenten.v033 Like 'A%' ORDER BY Leveranciers.A100", adntDB, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                ListView1.Items.Clear();
                while (!rsRecent.EOF)
                {
                    ListViewItem item = new ListViewItem(FieldText(rsRecent, "Naam"));
                    item.SubItems.Add(FieldText(rsRecent, "idCode"));
                    ListView1.Items.Add(item);
                    rsRecent.MoveNext();
                }
            }
            finally
            {
                try { if (rsRecent.State != (int)ObjectStateEnum.adStateClosed) rsRecent.Close(); } catch { }
                Cursor.Current = Cursors.Default;
            }

            ListView1.Focus();
        }

        private void cmdXLog_Click(object sender, EventArgs e)
        {
            string supplierCode = GetSelectedSupplierCode();
            if (!MoveToSelectedSupplierRecord(supplierCode))
                return;

            if (!TeleBibClick(TABLE_SUPPLIERS))
            {
                SSTab1.SelectedIndex = 0;
            }
            else
            {
                Msg = "Gegevens bestaande '" + bstNaam[TABLE_SUPPLIERS] + "'-fiche wijzigen.  Bent U zeker ?";
                KtrlBox = MsgBoxResult(MessageBox.Show(Msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
                if (KtrlBox == 6)
                {
                    BUpdate(TABLE_SUPPLIERS, 0);
                }
                SSTab1.SelectedIndex = 0;
                InstalLeverancier();
            }
        }

        private void cmdSQLInfo_Click(object sender, EventArgs e)
        {
            string supplierCode = GetSelectedSupplierCode();
            if (!MoveToSelectedSupplierRecord(supplierCode))
            {
                MessageBox.Show("Onlogica.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmdSQLInfo.Visible = false;
                return;
            }

            string sqlInfo = VBibText(TABLE_SUPPLIERS, "#v254 #");
            if (string.IsNullOrWhiteSpace(sqlInfo))
            {
                cmdSQLInfo.Visible = false;
                return;
            }

            SqlPopUp(sqlInfo, bstNaam[TABLE_SUPPLIERS], "A110", VBibText(TABLE_SUPPLIERS, "#A110 #"));
        }

        private void ListView1_Click(object sender, EventArgs e)
        {
            if (ListView1.SelectedItems.Count > 0)
            {
                _lvLeverancier = ListView1.SelectedItems[0].SubItems.Count > 1 ? ListView1.SelectedItems[0].SubItems[1].Text : string.Empty;
                if (MoveToSelectedSupplierRecord(_lvLeverancier))
                {
                    InstalLeverancier();
                    SSTab1.SelectedIndex = 0;
                }
            }
            else
            {
                KtrlBox = MsgBoxResult(MessageBox.Show("Historiek bestaande leveranciers tonen", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
                if (KtrlBox == 6)
                {
                    InstalleerRecenteCrediteuren();
                }
            }
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListView1_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void SSTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SSTab1.SelectedIndex == 0 && string.IsNullOrWhiteSpace(LeverancierInfo.Text))
            {
                SQLZoekLeverancier();
            }
        }

        private void SSTab1_KeyDown(object sender, KeyEventArgs e)
        {
            if (SSTab1.SelectedIndex == 2)
            {
                if (ListView1.Items.Count == 0 && e.KeyCode == Keys.Enter)
                {
                    KtrlBox = MsgBoxResult(MessageBox.Show("Historiek bestaande leveranciers tonen", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
                    if (KtrlBox == 6)
                    {
                        InstalleerRecenteCrediteuren();
                    }
                }
            }
            else if (e.KeyCode == Keys.Menu || e.KeyCode == Keys.Enter)
            {
                SQLZoekLeverancier();
            }
        }

        private void SQLZoekLeverancier()
        {
            if (!string.IsNullOrWhiteSpace(LeverancierInfo.Text))
            {
                KtrlBox = MsgBoxResult(MessageBox.Show("Andere leverancier aanduiden.  Bent U zeker", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
                if (KtrlBox == 7)
                {
                    AankoopDetail.Focus();
                    return;
                }
            }

            OpenSupplierSearch(string.Empty);
            if (Ktrl == 0)
            {
                InstalLeverancier();
            }
            else
            {
                Schoon();
            }
        }

        private void RasterSchoon()
        {
            AankoopDetail.Items.Clear();
        }

        private bool RekeningOK(string rekeningNummer)
        {
            bool ok = false;
            for (int i = 0; i <= 3; i++)
            {
                string grens = _grensDetail[i] ?? string.Empty;
                string from = VSet(grens, 7);
                string till = grens.Length >= 14 ? VSet(grens.Substring(7, 7), 7) : string.Empty;
                if (string.Compare(rekeningNummer, from, StringComparison.Ordinal) >= 0 && string.Compare(rekeningNummer, till, StringComparison.Ordinal) <= 0)
                {
                    ok = true;
                    break;
                }
            }

            if (!ok)
            {
                Msg = "Uw rekening : " + rekeningNummer + " past niet in de begrenzing." + Environment.NewLine + Environment.NewLine;
                Msg += "Investeringen  : vanaf " + VSet(_grensDetail[0], 7) + " tot " + VSet(_grensDetail[0].Substring(7, 7), 7) + Environment.NewLine;
                Msg += "Schulden/privé : vanaf " + VSet(_grensDetail[1], 7) + " tot " + VSet(_grensDetail[1].Substring(7, 7), 7) + Environment.NewLine;
                Msg += "Handelsgoed    : vanaf " + VSet(_grensDetail[2], 7) + " tot " + VSet(_grensDetail[2].Substring(7, 7), 7) + Environment.NewLine;
                Msg += "Diverse kosten : vanaf " + VSet(_grensDetail[3], 7) + " tot " + VSet(_grensDetail[3].Substring(7, 7), 7);
                MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private int InvestKtrl()
        {
            string fRekNum = VSet(GridText, 7);
            string grens0 = _grensDetail[0] ?? string.Empty;
            if (string.Compare(fRekNum, VSet(grens0, 7), StringComparison.Ordinal) >= 0 && string.Compare(fRekNum, VSet(grens0.Substring(7, 7), 7), StringComparison.Ordinal) <= 0)
            {
                GridText = TekstInfo0.Text + Dec(ParseDouble(PartMid(GridText, 50, 12)), MASK_EURBH);
                using (var investmentSheet = new FormPurchaseInvestmentSheet())
                {
                    investmentSheet.ShowDialog(this);
                }
            }
            return 0;
        }

        private void OpenPurchaseLineEditorForCurrentSelection(bool editingExisting)
        {
            if (editingExisting && _positie < 0)
            {
                MessageBox.Show("Eerst een lijn selecteren !", "Lijn wijzigen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (editingExisting)
                GridText = AankoopDetail.Items[_positie].ToString();
            else
                GridText = string.Empty;

            ShowPurchaseLineEditor(!editingExisting);
            if (string.IsNullOrWhiteSpace(GridText))
            {
                AankoopDetail.Focus();
                return;
            }

            if (!RekeningOK(VSet(GridText, 7)))
            {
                AankoopDetail.Focus();
                return;
            }

            if (editingExisting)
            {
                AankoopDetail.Items[_positie] = GridText;
                InvestKtrl();
                AankoopDetail.SelectedIndex = _positie;
            }
            else
            {
                int insertIndex = Math.Max(0, _positie + 1);
                if (insertIndex >= AankoopDetail.Items.Count)
                    AankoopDetail.Items.Add(GridText);
                else
                    AankoopDetail.Items.Insert(insertIndex, GridText);
                InvestKtrl();
            }

            AankoopDetail.Focus();
        }

        private void AankoopOptie_Click(int index)
        {
            SyncDocumentKeyFromOption(index);
            BGet(TABLE_INVOICES, 0, _documentKey);
            if (Ktrl == 0)
            {
                MessageBox.Show(
                    "Document " + _documentKey + " is reeds aanwezig..." + Environment.NewLine + Environment.NewLine +
                    "Controleer eventueel uw tellerbestand voor het active boekjaar.  Indien U zopas wisselde van boekjaar met het aankoopvenster open, mag U (na controle) deze melding negeren..." + Environment.NewLine + Environment.NewLine +
                    "NIET BOEKJAAR GEWISSELD ZOPAS ?  EERST UW TELLEBESTAND + PROEF- SALDI BALANS CONTROLEREN !!!",
                    string.Empty,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                Ktrl = 1;
            }
        }

        private void Medekontraktant_Click(object sender, EventArgs e)
        {
            if (_suppressMedekontraktantUpdate)
                return;

            if (Medekontraktant.Checked)
            {
                TekstInfo7.Visible = true;
                TekstInfo5.Text = "0";
                TekstInfo6.Text = "0";
                TekstInfo7.Text = "0";
                Label1_12.Visible = true;
            }
            else
            {
                TekstInfo7.Visible = false;
                TekstInfo5.Text = "0";
                TekstInfo6.Text = "0";
                TekstInfo7.Text = "0";
                Label1_12.Visible = false;
            }
        }

        private void StockBeheer_Click(object sender, EventArgs e)
        {
            AankoopDetail.Focus();
        }

        private void AdvanceOnEnter(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void TekstInfo0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                TekstInfo0.Text = TekstInfo1.Text;
            AdvanceOnEnter(e);
        }

        private void TekstInfo0_KeyPress(object sender, KeyPressEventArgs e)
        {
            TekstInfo0.Text = TekstInfo1.Text;
        }

        private void TekstInfo1_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo2_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo12_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo4_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo5_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo7_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo9_KeyDown(object sender, KeyEventArgs e)
        {
            AdvanceOnEnter(e);
        }

        private void TekstInfo10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                aIndex = 0;
                SharedFl = TABLE_LEDGERACCOUNTS;
                GridText = TekstInfo10.Text.Trim();
                using (var sqlSearch = new FormSearchSQL())
                {
                    sqlSearch.ShowDialog(this);
                }
                if (Ktrl == 0)
                    TekstInfo10.Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
                e.Handled = true;
                return;
            }
            AdvanceOnEnter(e);
        }

        private void TekstInfo3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                aIndex = 0;
                SharedFl = TABLE_LEDGERACCOUNTS;
                GridText = TekstInfo3.Text.Trim();
                using (var sqlSearch = new FormSearchSQL())
                {
                    sqlSearch.ShowDialog(this);
                }
                if (Ktrl == 0)
                    TekstInfo3.Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
                e.Handled = true;
                return;
            }
            AdvanceOnEnter(e);
        }

        private void TekstInfo3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TekstInfo10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TekstInfo5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract || e.KeyCode == Keys.Divide)
            {
                if (TekstInfo5.TextLength <= 1)
                    return;

                string result = LineCalculating(TekstInfo5.Text);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    TekstInfo5.Text = Math.Round(ParseDouble(result), 2).ToString(CultureInfo.CurrentCulture);
                }
            }
        }

        private void TekstInfo5_TextChanged(object sender, EventArgs e)
        {
            if (Medekontraktant.Checked)
                TekstInfo7.Text = TekstInfo5.Text;
        }

        private void TekstInfo0_Enter(object sender, EventArgs e)
        {
            SelectAllText(TekstInfo0, 5);
        }

        private void TekstInfo1_Enter(object sender, EventArgs e)
        {
            SelectAllText(TekstInfo1, 5);
        }

        private void TekstInfo2_Enter(object sender, EventArgs e)
        {
            SelectAllText(TekstInfo2, 5);
        }

        private void TekstInfo3_Enter(object sender, EventArgs e)
        {
            SelectAllText(TekstInfo3);
        }

        private void TekstInfo6_Enter(object sender, EventArgs e)
        {
            SelectAllText(TekstInfo6);
        }

        private void TekstInfo10_Enter(object sender, EventArgs e)
        {
            SelectAllText(TekstInfo10);
        }

        private void TekstInfo0_Leave(object sender, EventArgs e)
        {
            if (DateInvalid(TekstInfo0.Text))
            {
                TekstInfo0.Text = MIM_GLOBAL_DATE;
                TekstInfo0.Focus();
            }
            else if (!DateCheck(TekstInfo0.Text, PERIODAS_TEXT))
            {
                FocusByPerDat();
            }
        }

        private void TekstInfo1_Leave(object sender, EventArgs e)
        {
            if (DateInvalid(TekstInfo1.Text))
            {
                TekstInfo1.Text = MIM_GLOBAL_DATE;
                TekstInfo1.Focus();
            }
            else
            {
                TekstInfo2.Text = VValdag(TekstInfo1.Text, VBibText(TABLE_SUPPLIERS, "#vs04 #"));
                TekstInfo0.Text = MIM_GLOBAL_DATE;
            }

            string dateKey = DateKey(TekstInfo1.Text);
            if (string.Compare(dateKey, VSet(BOOKYEAR_FROMTO, 8), StringComparison.Ordinal) < 0 || string.Compare(dateKey, VSet(BOOKYEAR_FROMTO.Substring(8, 8), 8), StringComparison.Ordinal) > 0)
            {
                Msg = "Datum aankoopdocument valt BUITEN het actieve boekjaar." + Environment.NewLine;
                Msg += "De optie 'boekhoudcontrole' in balans leveranciers zal" + Environment.NewLine;
                Msg += "mogelijk niet goed functioneren." + Environment.NewLine + Environment.NewLine;
                Msg += "Controleer eventueel.";
                MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            TekstInfo0.Text = TekstInfo1.Text;
        }

        private void TekstInfo2_Leave(object sender, EventArgs e)
        {
            if (DateInvalid(TekstInfo2.Text))
            {
                TekstInfo2.Text = VValdag(TekstInfo1.Text, VBibText(TABLE_SUPPLIERS, "#vs04 #"));
                TekstInfo2.Focus();
            }
        }

        private void TekstInfo3_Leave(object sender, EventArgs e)
        {
            if (VSet(TekstInfo3.Text, 3) != "440")
            {
                TekstInfo3.Text = _leverancierRekening;
                TekstInfo3.Focus();
                return;
            }

            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(TekstInfo3.Text, 7));
            if (Ktrl != 0)
            {
                TekstInfo3.Text = _leverancierRekening;
                TekstInfo3.Focus();
            }
        }

        private void TekstInfo4_Leave(object sender, EventArgs e)
        {
            if (VBibText(TABLE_SUPPLIERS, "#v017 #") == "1")
            {
                if (!BankOk(TekstInfo4.Text))
                {
                    SnelHelpPrint("Gestructureerde betaalreferte onjuist !", BL_LOGGING);
                    TekstInfo4.Text = "OGM onjuist";
                }
            }
            else if (string.IsNullOrWhiteSpace(TekstInfo4.Text))
            {
                TekstInfo4.Text = TekstInfo12.Text;
            }
        }

        private void TekstInfo5_Leave(object sender, EventArgs e)
        {
            if (_aankoopFlg == 1 && AankoopOptie1.Checked)
            {
                if (ParseDouble(TekstInfo5.Text) != 0)
                {
                    KtrlBox = MsgBoxResult(MessageBox.Show("De aanbeveling door meeste BTW-diensten om bij creditnota E.U. géén B.T.W op te nemen negeren", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                    if (KtrlBox == 6)
                        TekstInfo5.Text = "0";
                }
            }
            else if (_aankoopFlg == 1 && !AankoopOptie1.Checked)
            {
                if (ParseDouble(TekstInfo5.Text) == 0)
                {
                    MessageBox.Show("Respecteer de aanbeveling door de BTW-diensten om bij factuur E.U. de B.T.W. zelf uit te rekenen en zowel AFTREKBAAR als VERSCHULDIGD (0-operatie dus...) het toepasbaar B.T.W. bedrag mee te delen !", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    TekstInfo5.Focus();
                }
                else
                {
                    TekstInfo7.Text = TekstInfo5.Text;
                }
            }
        }

        private void TekstInfo10_Leave(object sender, EventArgs e)
        {
            if (VSet(TekstInfo10.Text, 6) != VSet(_rbtwVak[4], 6))
            {
                TekstInfo10.Text = _rbtwVak[4];
                TekstInfo10.Focus();
                return;
            }

            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(TekstInfo10.Text, 7));
            if (Ktrl != 0)
            {
                TekstInfo10.Text = _rbtwVak[4];
                TekstInfo10.Focus();
            }
        }

        private void ButtonBookIt_LostFocus(object sender, EventArgs e)
        {
            ButtonControleIt.Visible = true;
            ButtonBookIt.Visible = false;
        }

        private void ButtonOptimize_Click(object sender, EventArgs e)
        {
            if (AankoopDetail.Items.Count == 0)
                return;

            Msg = "Buiten producten, meermaals voorkomende lijnen samenvoegen tot één." + Environment.NewLine + Environment.NewLine + "Bent U zeker ?";
            KtrlBox = MsgBoxResult(MessageBox.Show(Msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
            if (KtrlBox != 6)
                return;

            int totalMerged = 0;
            for (int twice = 1; twice <= 2; twice++)
            {
                int tt = 0;
                while (tt < AankoopDetail.Items.Count)
                {
                    string currentLine = AankoopDetail.Items[tt].ToString();
                    if (currentLine.Length == 62)
                    {
                        string huidigeRekening = PartMid(currentLine, 1, 7);
                        int baseIndex = tt;
                        int compareIndex = tt + 1;
                        while (compareIndex < AankoopDetail.Items.Count)
                        {
                            string compareLine = AankoopDetail.Items[compareIndex].ToString();
                            if (huidigeRekening == PartMid(compareLine, 1, 7) && compareLine.Length == 62)
                            {
                                string tempo = AankoopDetail.Items[baseIndex].ToString();
                                double sum = ParseDouble(PartMid(compareLine, 50, 12)) + ParseDouble(PartMid(tempo, 50, 12));
                                string updated = tempo.Substring(0, 49) + Dec(sum, "#########.00").PadLeft(12);
                                AankoopDetail.Items.RemoveAt(compareIndex);
                                AankoopDetail.Items[baseIndex] = updated;
                                totalMerged++;
                            }
                            else
                            {
                                compareIndex++;
                            }
                        }
                    }
                    tt++;
                }
            }

            MessageBox.Show("Einde optimalisatie.  " + totalMerged.ToString(CultureInfo.CurrentCulture) + " lijnen samengevoegd.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string CheckDocument()
        {
            _rsDocuments = new Recordset();
            try
            {
                _rsDocuments.CursorLocation = CursorLocationEnum.adUseClient;
                string payRefClean = GetSafeArrayValue(uitwisselingDATAArray, 14).Replace("/", string.Empty).Replace("+", string.Empty);

                string sSQL = "SELECT Leveranciers.A110, Leveranciers.v404, Dokumenten.v409, Dokumenten.v033, Dokumenten.v034, Dokumenten.v035, Dokumenten.v036, Dokumenten.v037, Dokumenten.v039, Dokumenten.v249 " +
                              "FROM Leveranciers, Dokumenten " +
                              "WHERE Leveranciers.v404 = '" + GetSafeArrayValue(uitwisselingDATAArray, 5) + "' " +
                              "AND Dokumenten.v034 = 'L'+Leveranciers.A110 " +
                              "AND Dokumenten.v409 = '" + GetSafeArrayValue(uitwisselingDATAArray, 1) + "' " +
                              "AND Dokumenten.v039 = '" + payRefClean + "'";

                SnelHelpPrint(Msg, BL_LOGGING);
                Cursor.Current = Cursors.WaitCursor;
                _rsDocuments.Open(sSQL, adntDB, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                Cursor.Current = Cursors.Default;

                if (_rsDocuments.RecordCount == 0)
                    return "In te boeken";

                if (_rsDocuments.RecordCount == 1)
                {
                    XLogKey = string.Empty;
                    bool totalMatches = Math.Abs(ParseDouble(FieldText(_rsDocuments, "v249")) - ParseDouble(GetSafeArrayValue(uitwisselingDATAArray, 21))) < 0.01;
                    bool payRefMatches = payRefClean == FieldText(_rsDocuments, "v039").Trim();
                    bool dateMatches = ReplaceDateSeparators(GetSafeArrayValue(uitwisselingDATAArray, 2)) == FieldText(_rsDocuments, "v035");
                    if (totalMatches && payRefMatches && dateMatches)
                    {
                        string result = "Ingeboekt";
                        XLogKey = FieldText(_rsDocuments, "v033");
                        object openAmount = _rsDocuments.Fields["v037"].Value;
                        if (openAmount == null || openAmount == DBNull.Value || string.IsNullOrWhiteSpace(openAmount.ToString()))
                            return result + "Te betalen";
                        if (Math.Abs(ParseDouble(FieldText(_rsDocuments, "v249")) - ParseDouble(openAmount.ToString())) < 0.01)
                            return result + "Betaald";
                        return result + "Gedeeltelijk openstaand";
                    }
                    return "whatsgoingon?";
                }

                MessageBox.Show("Meerdere documenten gevonden, onlogische situatie.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return string.Empty;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Bron:" + Environment.NewLine + ex.Source + Environment.NewLine + Environment.NewLine + "Detail:" + Environment.NewLine + ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return string.Empty;
            }
        }

        private void cbImportUBL_Click(object sender, EventArgs e)
        {
            int counterToBook = 0;
            int counterBooked = 0;
            string ublFileUrl = string.Empty;

            _ifSupplierInsertWarning = false;

            Cursor.Current = Cursors.WaitCursor;
            FormPurchasePeppolMonitor monitor = GetPeppolMonitor();
            monitor.Hide();
            monitor.ResetToBookGrid();
            monitor.ResetBookedGrid();

            string sPath = LOCATION_COMPANYDATA + @"peppol\in\";
            if (!Directory.Exists(sPath))
            {
                Cursor.Current = Cursors.Default;
                return;
            }

            foreach (string filePath in Directory.GetFiles(sPath, "*.xml"))
            {
                string sFile = Path.GetFileName(filePath);
                try
                {
                    ReadUblDocument(filePath, false, false);
                    if (CheckSupplier())
                    {
                        string result = CheckDocument();
                        string documentCode;
                        switch (GetSafeArrayValue(uitwisselingDATAArray, 4))
                        {
                            case "071":
                            case "084":
                                documentCode = "Debetnota";
                                break;
                            case "380":
                            case "386":
                                documentCode = "Factuur";
                                break;
                            case "381":
                                documentCode = "Creditnota";
                                break;
                            case "575":
                                documentCode = "Verzekering";
                                break;
                            default:
                                documentCode = GetSafeArrayValue(uitwisselingDATAArray, 4);
                                break;
                        }

                        string dateDocument = DateText(ReplaceDateSeparators(GetSafeArrayValue(uitwisselingDATAArray, 2)));
                        string dateExpiringDocument = dateDocument;
                        if (GetSafeArrayValue(uitwisselingDATAArray, 4) == "380")
                        {
                            string expiringDate = DateText(ReplaceDateSeparators(GetSafeArrayValue(uitwisselingDATAArray, 3)));
                            dateExpiringDocument = string.IsNullOrWhiteSpace(expiringDate) ? dateDocument : expiringDate;
                        }

                        if (VSet(result, 9) == "Ingeboekt")
                        {
                            counterBooked++;
                            monitor.AddBookedRow(
                                GetSafeArrayValue(uitwisselingDATAArray, 7),
                                XLogKey,
                                dateDocument,
                                dateExpiringDocument,
                                VSet(result.Substring(9), result.Length - 9),
                                sFile);
                        }
                        else
                        {
                            counterToBook++;
                            monitor.AddToBookRow(
                                GetSafeArrayValue(uitwisselingDATAArray, 7),
                                GetSafeArrayValue(uitwisselingDATAArray, 5),
                                documentCode,
                                dateDocument,
                                dateExpiringDocument,
                                sFile);
                        }
                    }
                }
                catch
                {
                }
            }

            Cursor.Current = Cursors.Default;
            XLogKey = string.Empty;

            if (!purchasePeppolTODOShowed)
            {
                purchasePeppolTODOShowed = true;
                if (monitor.ToBookRowCount == 0)
                {
                    documentLinesDATA = string.Empty;
                    monitor.Close();
                    _ifSupplierInsertWarning = true;
                    return;
                }
                monitor.ShowDialog(this);
            }
            else
            {
                monitor.ShowDialog(this);
            }

            _ifSupplierInsertWarning = true;
            ublFileUrl = (XLogKey ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(ublFileUrl) || !File.Exists(ublFileUrl))
                return;
            if (!ublFileUrl.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Enkel *.xml bestanden zijn toegelaten", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ReadUblDocument(ublFileUrl, false, true);
            uitwisselingOMSArray = (uitwisselingOMS ?? string.Empty).Split('\t');
            uitwisselingDATAArray = (uitwisselingDATA ?? string.Empty).Split('\t');
            // ShowUitwisselingArraysDebug();
            Cursor.Current = Cursors.Default;

            if (CheckSupplier())
            {
                if (!string.IsNullOrWhiteSpace(CheckDocument()))
                {
                    if (MoveToSelectedSupplierRecord(FieldText(_rsSupplier, "A110")))
                    {
                        InstalSupplierForUBL();
                    }
                }
            }
        }

        private void cbCheckTools_Click(object sender, EventArgs e)
        {
            using (var validatingTool = new FormPeppolCheckTool())
            {
                validatingTool.Text = "Leveranciers Peppol Tools";
                SetTextBoxIfPresent(validatingTool, "tbCompanyNumber", _supplierVatNumber);
                SetTextBoxIfPresent(validatingTool, "tbVatNumber", _supplierCountryCode + _supplierVatNumber);
                SetTextBoxIfPresent(validatingTool, "tbPeppolID", "0208:" + _supplierVatNumber);
                validatingTool.ShowDialog(this);
            }
        }

        private static void SetTextBoxIfPresent(Form form, string controlName, string value)
        {
            Control[] matches = form.Controls.Find(controlName, true);
            if (matches.Length > 0 && matches[0] is TextBox textBox)
            {
                textBox.Text = value ?? string.Empty;
            }
        }
        
        private void Schoon()
        {
            TextInfoSellersIBAN.Text = string.Empty;
            TextWarningIBAN.Visible = false;
            ButtonControleIt.Enabled = false;
            uitwisselingOMS = string.Empty;
            uitwisselingDATA = string.Empty;
            documentLinesDATA = string.Empty;
            cbImportUBL.Enabled = true;
            _geScanBestand = string.Empty;

            _lvLeverancier = string.Empty;
            ButtonControleIt.Visible = true;
            AankoopDetail.Enabled = false;
            LeverancierInfo.Text = string.Empty;
            ButtonBookIt.Visible = false;
            Schoonvegen.Enabled = false;
            ApplyStructuredReferenceMask(false);

            TekstInfo0.Enabled = false;
            TekstInfo0.Mask = "00/00/0000";
            TekstInfo0.Text = MIM_GLOBAL_DATE;
            TekstInfo1.Enabled = false;
            TekstInfo1.Mask = "00/00/0000";
            TekstInfo1.Text = MIM_GLOBAL_DATE;
            TekstInfo2.Enabled = false;
            TekstInfo2.Mask = "00/00/0000";
            TekstInfo2.Text = MIM_GLOBAL_DATE;
            TekstInfo3.Enabled = false;
            TekstInfo3.Text = _leverancierRekening;
            TekstInfo4.Enabled = false;
            TekstInfo4.Text = string.Empty;
            TekstInfo12.Enabled = false;
            TekstInfo12.Text = string.Empty;
            TekstInfo5.Enabled = false;
            TekstInfo5.Text = string.Empty;
            TekstInfo6.Enabled = false;
            TekstInfo6.Text = string.Empty;
            TekstInfo7.Enabled = false;
            TekstInfo7.Text = string.Empty;
            TekstInfo9.Enabled = false;
            TekstInfo9.Text = string.Empty;
            TekstInfo10.Enabled = false;
            TekstInfo10.Text = _rbtwVak[4];

            RasterSchoon();
            SSTab1.SelectedIndex = 1;
            cmdXLog.Enabled = false;
            LabelInfoXlog.Visible = true;
            AankoopOptie0.Checked = true;
            _supplierCompanyId = string.Empty;
            _supplierCountryCode = string.Empty;
            _supplierVatNumber = string.Empty;
        }

        private void SchoonVegen_Click(object sender, EventArgs e)
        {
            RasterSchoon();
            Schoon();
            SSTab1.Focus();
        }

        private void Annuleren_Click(object sender, EventArgs e)
        {
            if (AankoopDetail.Items.Count > 0)
            {
                Msg = "Aanwezige bewerkingen negeren !  Bent U zeker ?";
                Ktrl = MsgBoxResult(MessageBox.Show(Msg, "Aankoopverrichtingen sluiten", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
                if (Ktrl != 6)
                    return;
            }

            Close();
        }

        private void AankoopDetail_DoubleClick(object sender, EventArgs e)
        {
            _positie = AankoopDetail.SelectedIndex;
            if (AankoopDetail.SelectedIndex == -1)
                OpenPurchaseLineEditorForCurrentSelection(false);
            else
                OpenPurchaseLineEditorForCurrentSelection(true);
        }

        private void AankoopDetail_Enter(object sender, EventArgs e)
        {
            SnelHelpPrint("[Insert] lijn bijvoegen, [Delete] lijn verwijderen, [spatie],[Enter] om te wijzigen", BL_LOGGING);
            if (_positie >= 0 && _positie < AankoopDetail.Items.Count)
                AankoopDetail.SelectedIndex = _positie;
        }

        private void AankoopDetail_KeyDown(object sender, KeyEventArgs e)
        {
            _positie = AankoopDetail.SelectedIndex;
            if (e.KeyCode == Keys.Insert || e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                OpenPurchaseLineEditorForCurrentSelection(false);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (_positie < 0)
                {
                    MessageBox.Show("Eerst een lijn selecteren !", "Lijn wijzigen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Msg = "Lijn verwijderen !  Bent U zeker ?";
                Ktrl = MsgBoxResult(MessageBox.Show(Msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2));
                if (Ktrl == 6)
                {
                    AankoopDetail.Items.RemoveAt(_positie);
                }
                AankoopDetail.Focus();
                e.Handled = true;
            }
        }

        private void AankoopDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            _positie = AankoopDetail.SelectedIndex;
            if (e.KeyChar == (char)Keys.Enter)
            {
                OpenPurchaseLineEditorForCurrentSelection(true);
                e.Handled = true;
            }
        }

        private void StockBeheer_CheckedChanged(object sender, EventArgs e)
        {
            if (StockBeheer.Checked)
            {
                OMSproduct.Visible = true;
            }
            else
            {
                OMSproduct.Visible = false;
            }
        }

        private void ButtonControleIt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TekstInfo12.Text))
            {
                MessageBox.Show("Referte Document is verplicht", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                TekstInfo12.Focus();
                return;
            }

            if (!DateCheck(TekstInfo0.Text, PERIODAS_TEXT))
            {
                FocusByPerDat();
                return;
            }

            string dateKey = DateKey(TekstInfo1.Text);
            if (string.Compare(dateKey, VSet(BOOKYEAR_FROMTO, 8), StringComparison.Ordinal) < 0 || string.Compare(dateKey, VSet(BOOKYEAR_FROMTO.Substring(8, 8), 8), StringComparison.Ordinal) > 0)
            {
                Msg = "Datum aankoopdocument valt BUITEN het actieve boekjaar." + Environment.NewLine;
                Msg += "De optie 'boekhoudcontrole' in balans leveranciers zal" + Environment.NewLine;
                Msg += "mogelijk niet goed functioneren." + Environment.NewLine + Environment.NewLine;
                Msg += "Controleer eventueel.";
                MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            double bedragTotaal = 0;
            for (int i = 0; i < AankoopDetail.Items.Count; i++)
            {
                string line = AankoopDetail.Items[i].ToString();
                bedragTotaal += ParseDouble(PartMid(line, 50, 12));
            }

            double bedragBtw5 = ParseDouble(TekstInfo5.Text);
            double bedragBtw6 = ParseDouble(TekstInfo6.Text);
            double bedragBtw7 = ParseDouble(TekstInfo7.Text);
            bedragTotaal = bedragTotaal + bedragBtw5 - bedragBtw7;

            if (bedragTotaal == 0)
                return;

            if (Math.Round(bedragTotaal, 2) != Math.Round(bedragBtw6, 2))
            {
                MessageBox.Show("Totaal dokument " + bedragBtw6 + Environment.NewLine + "en volgens berekening is het " + bedragTotaal + Environment.NewLine + Environment.NewLine + "ButtonControleIter a.u.b.!", "Totaalkontrole", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (TekstInfo6.Enabled)
                    TekstInfo6.Focus();
                return;
            }

            ButtonBookIt.Visible = true;
            ButtonBookIt.Enabled = true;
            ButtonControleIt.Visible = false;
            ButtonBookIt.Focus();
        }

        private void ButtonBookIt_Click(object sender, EventArgs e)
        {
            if (AankoopDetail.Items.Count == 0)
                return;

            if (!DateCheck(TekstInfo0.Text, PERIODAS_TEXT))
            {
                TekstInfo0.Focus();
                return;
            }

            string currentKey = SleutelDok(_ar);
            if (_documentKey != currentKey)
            {
                Msg = _documentKey + " <> " + currentKey + Environment.NewLine + Environment.NewLine;
                Msg += "MOGELIJKHEID 1: Teller is identiek, boekjaar is hoger/lager." + Environment.NewLine;
                Msg += "U hebt dus het actief boekjaar of periode gewijzigd tijdens de aanmaak van dit dokument.  Probeer nogmaals NA KONTROLE." + Environment.NewLine + Environment.NewLine;
                Msg += "MOGELIJKHEID 2: Boekjaar is identiek, teller is hoger/lager." + Environment.NewLine;
                Msg += "Een andere gebruiker heeft ondertussen een dokument verwerkt." + Environment.NewLine + Environment.NewLine;
                Msg += "ButtonControleIter eerst eens vooraleer de boeking nogmaals uit te voeren a.u.b. !!!";
                MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                _documentKey = currentKey;
                Text = "Direkte aankoopverrichting         (" + _documentKey + ")";
                return;
            }

            if (MessageBox.Show("Document wegschrijven, boekhouding bijwerken.", _documentKey + " verwerken.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            BClose(TABLE_LEDGERACCOUNTS);
            BClose(TABLE_JOURNAL);
            BClose(TABLE_INVOICES);
            BOpen(TABLE_LEDGERACCOUNTS);
            BOpen(TABLE_JOURNAL);
            BOpen(TABLE_INVOICES);
            BBegin();

            using (var boekingForm = new FormBoeking())
            {
                if (WegBoekFout(boekingForm))
                {
                    BAbort();
                    Focus();
                    return;
                }

                BEnd();
                if (Ktrl != 0)
                {
                    BAbort();
                    return;
                }

                if (_aankoopFlg == 1 && _sIsIntraFlg == "1")
                {
                    Fl = TABLE_SUPPLIERS;
                    aIndex = 19;
                    double dTTwb = ParseDouble(VBibText(TABLE_INVOICES, "#v048 #")) + ParseDouble(VBibText(TABLE_INVOICES, "#v047 #")) + ParseDouble(VBibText(TABLE_INVOICES, "#v046 #")) + ParseDouble(VBibText(TABLE_INVOICES, "#v049 #"));
                    GridText = Dec(dTTwb, MASK_SY[0]) + "\t";
                    using (var intrastat = new FormIntrastat())
                    {
                        intrastat.ShowDialog(this);
                    }
                }

                SS99(SafeRight(_documentKey, 5), _ar);  // s001 or s003
                _documentKey = SleutelDok(_ar);
                Text = "Direkte aankoopverrichting         (" + _documentKey + ")";
                RefreshReference();
                SchoonVegen_Click(sender, EventArgs.Empty);
                SSTab1.Focus();
            }
        }

        private void ApplyStructuredReferenceMask(bool keepExistingValue)
        {
            string current = keepExistingValue ? TekstInfo4.Text : string.Empty;
            if (VBibText(TABLE_SUPPLIERS, "#v017 #") == "1")
            {
                TekstInfo4.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                TekstInfo4.Mask = "\\+\\+\\+000\\/0000\\/00000\\+\\+\\+";
            }
            else
            {
                TekstInfo4.TextMaskFormat = MaskFormat.IncludeLiterals;
                TekstInfo4.Mask = string.Empty;
            }
            TekstInfo4.Text = current;
        }

        private bool WegBoekFout(FormBoeking boekingForm)
        {
            double totaalBedrag = 0;
            double bedrag;
            double dInvest = 0;
            double dPrive = 0;
            double dAlKost = 0;
            double dHandel = 0;

            if (!MoveToSelectedSupplierRecord(GetSelectedSupplierCode()))
                return true;

            dMuntL = ParseDouble(TekstInfo9.Text);
            DKTRL_CUMUL = 0;
            DKTRL_BEF = 0;
            DKTRL_EUR = 0;
            JournaalLocked = false;
            TLB_RECORD[TABLE_JOURNAL] = string.Empty;
            TLB_RECORD[TABLE_INVOICES] = string.Empty;

            if (rsMAR[TABLE_INVOICES].State == (int)ObjectStateEnum.adStateClosed)
            {
                BOpen(TABLE_INVOICES);
            }
            rsMAR[TABLE_INVOICES].AddNew();

            VBib(TABLE_JOURNAL, "L" + VBibText(TABLE_SUPPLIERS, "#A110 #"), "v034");
            VBib(TABLE_JOURNAL, DateKey(TekstInfo0.Text), "v066");
            VBib(TABLE_JOURNAL, _documentKey, "v033");
            VBib(TABLE_JOURNAL, DateKey(TekstInfo1.Text), "v035");
            VBib(TABLE_JOURNAL, TekstInfo3.Text, "v069");

            VBib(TABLE_INVOICES, _documentKey, "v033");
            VBib(TABLE_INVOICES, _documentOGMNoFormat, "v413");
            VBib(TABLE_INVOICES, TekstInfo12.Text, "v409");
            VBib(TABLE_INVOICES, "L" + VBibText(TABLE_SUPPLIERS, "#A110 #"), "v034");
            VBib(TABLE_INVOICES, DateKey(TekstInfo1.Text), "v035");
            VBib(TABLE_INVOICES, DateKey(TekstInfo2.Text), "v036");
            VBib(TABLE_INVOICES, TekstInfo4.Text, "v039");
            VBib(TABLE_INVOICES, Dec(dMuntL, "###.##########"), "v040");
            VBib(TABLE_INVOICES, VBibText(TABLE_SUPPLIERS, "#vs03 #"), "vs03");
            VBib(TABLE_JOURNAL, VBibText(TABLE_SUPPLIERS, "#A100 #"), "v067");

            for (int i = 0; i < AankoopDetail.Items.Count; i++)
            {
                string line = AankoopDetail.Items[i].ToString();
                string lokRekening = VSet(line, 7);
                VBib(TABLE_JOURNAL, lokRekening, "v019");
                BGet(TABLE_LEDGERACCOUNTS, 0, lokRekening);
                if (Ktrl != 0)
                    return true;

                bedrag = ParseDouble(PartMid(line, 50, 12));
                totaalBedrag += bedrag;
                double boekBedrag = AankoopOptie0.Checked ? bedrag : -bedrag;
                VBib(TABLE_JOURNAL, Dec(boekBedrag, MASK_EURBH), "v068");
                VBib(TABLE_JOURNAL, string.Empty, "v102");
                VBib(TABLE_JOURNAL, PartMid(line, 9, 40).Trim(), "v067");

                if (string.Compare(lokRekening, VSet(_grensDetail[0], 7), StringComparison.Ordinal) >= 0 && string.Compare(lokRekening, SafeRight(_grensDetail[0].Substring(7, 7), 7), StringComparison.Ordinal) <= 0)
                    dInvest += bedrag;
                else if (string.Compare(lokRekening, VSet(_grensDetail[1], 7), StringComparison.Ordinal) >= 0 && string.Compare(lokRekening, SafeRight(_grensDetail[1], 7), StringComparison.Ordinal) <= 0)
                    dPrive += bedrag;
                else if (string.Compare(lokRekening, VSet(_grensDetail[2], 7), StringComparison.Ordinal) >= 0 && string.Compare(lokRekening, SafeRight(_grensDetail[2], 7), StringComparison.Ordinal) <= 0)
                    dHandel += bedrag;
                else if (string.Compare(lokRekening, VSet(_grensDetail[3], 7), StringComparison.Ordinal) >= 0 && string.Compare(lokRekening, SafeRight(_grensDetail[3], 7), StringComparison.Ordinal) <= 0)
                    dAlKost += bedrag;
                else
                    MessageBox.Show("Stop in begrenzing", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);

                BInsert(TABLE_JOURNAL, 0, boekingForm);
                if (Ktrl != 0)
                    return true;
            }

            VBib(TABLE_JOURNAL, VBibText(TABLE_SUPPLIERS, "#A100 #"), "v067");
            VBib(TABLE_INVOICES, TekstInfo5.Text, "v045");
            VBib(TABLE_INVOICES, dInvest.ToString(CultureInfo.InvariantCulture), "v048");
            VBib(TABLE_INVOICES, dHandel.ToString(CultureInfo.InvariantCulture), "v046");
            VBib(TABLE_INVOICES, dAlKost.ToString(CultureInfo.InvariantCulture), "v047");

            string dvt99 = (dInvest + dAlKost + dHandel + dPrive).ToString(CultureInfo.InvariantCulture);
            if (Medekontraktant.Checked)
            {
                VBib(TABLE_INVOICES, TekstInfo7.Text, "v043");
                VBib(TABLE_INVOICES, dvt99, _ar == 1 ? "v053" : "v051");
            }
            else if (!Medekontraktant.Checked && _aankoopFlg == 0 && _ar == 3)
            {
                VBib(TABLE_INVOICES, dvt99, "v051");
            }

            if (_aankoopFlg == 1)
            {
                VBib(TABLE_INVOICES, TekstInfo7.Text, "v042");
                VBib(TABLE_INVOICES, dvt99, _ar == 1 ? "v052" : "v050");
            }
            if (_aankoopFlg == 2)
            {
                VBib(TABLE_INVOICES, TekstInfo7.Text, "v044");
                VBib(TABLE_INVOICES, dvt99, _ar == 1 ? "v054" : "v051");
            }

            if (ParseDouble(TekstInfo5.Text) != 0)
            {
                double btwBoeking = _ar == 3 ? -ParseDouble(TekstInfo5.Text) : ParseDouble(TekstInfo5.Text);
                string btwRekening = _ar == 3 ? _rbtwVak[5] : VSet(TekstInfo10.Text, 7);
                VBib(TABLE_JOURNAL, Dec(btwBoeking, MASK_EURBH), "v068");
                VBib(TABLE_JOURNAL, btwRekening, "v019");
                BInsert(TABLE_JOURNAL, 0, boekingForm);
            }

            if (_aankoopFlg != 0 || Medekontraktant.Checked)
            {
                double verschuldigd = _ar == 1 ? -ParseDouble(TekstInfo7.Text) : ParseDouble(TekstInfo7.Text);
                string rek = _aankoopFlg == 2 ? _rbtwVak[3] : (_aankoopFlg == 0 ? _rbtwVak[2] : _rbtwVak[1]);
                VBib(TABLE_JOURNAL, Dec(verschuldigd, MASK_EURBH), "v068");
                VBib(TABLE_JOURNAL, rek, "v019");
                BInsert(TABLE_JOURNAL, 0, boekingForm);
            }

            double leverancierSaldo = ParseDouble(dvt99) + ParseDouble(TekstInfo5.Text) - ParseDouble(TekstInfo7.Text);
            double leverancierBoeking = _ar == 1 ? -leverancierSaldo : leverancierSaldo;
            VBib(TABLE_JOURNAL, Dec(leverancierBoeking, MASK_EURBH), "v068");
            VBib(TABLE_JOURNAL, VSet(TekstInfo3.Text, 7), "v019");
            BInsert(TABLE_JOURNAL, 0, boekingForm);

            if (!XisEuroWisBEF)
                VBib(TABLE_INVOICES, leverancierSaldo.ToString(CultureInfo.InvariantCulture), "v249");
            else
                VBib(TABLE_INVOICES, Math.Round(leverancierSaldo / EURO, 2).ToString(CultureInfo.InvariantCulture), "v249");

            if (!string.IsNullOrWhiteSpace(_geScanBestand) && File.Exists(_geScanBestand))
            {
                FileToBlob(rsMAR[TABLE_INVOICES].Fields["bstBLOB37"], _geScanBestand);
                rsMAR[TABLE_INVOICES].Fields["bstndNaam37"].Value = _geScanBestand;
                int dot = _geScanBestand.LastIndexOf('.');
                rsMAR[TABLE_INVOICES].Fields["typeZending37"].Value = dot >= 0 ? _geScanBestand.Substring(dot + 1) : string.Empty;
            }

            BInsert(TABLE_INVOICES, 0, boekingForm);

            DKTRL_CUMUL = ParseDouble(Dec(DKTRL_CUMUL, MASK_EURBH));
            DKTRL_EUR = ParseDouble(Dec(DKTRL_EUR, MASK_EURBH));
            DKTRL_BEF = ParseDouble(Dec(DKTRL_BEF, MASK_BEF));

            if (DKTRL_CUMUL != 0)
            {
                SetControlEnabledIfPresent(boekingForm, "cmdBoeken", false);
                MessageBox.Show("LogikaFout bij vierkantskontrole journaal." + Environment.NewLine + Environment.NewLine + "Deze verrichting wordt geannuleerd.  Controleer zelf eerst en/of raadpleeg ons.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private bool CheckSupplier()
        {
            bool checkSupplier = false;
            _rsSupplier = new Recordset();

            try
            {
                _rsSupplier.CursorLocation = CursorLocationEnum.adUseClient;

                uitwisselingOMSArray = (uitwisselingOMS ?? string.Empty).Split('\t');
                uitwisselingDATAArray = (uitwisselingDATA ?? string.Empty).Split('\t');

                string searchCountry = GetSafeArrayValue(uitwisselingDATAArray, 11).Trim();
                string searchOnVat;
                if (searchCountry == "BE")
                {
                    searchOnVat = GetSafeArrayValue(uitwisselingDATAArray, 5).Trim();
                }
                else if (searchCountry == "NL")
                {
                    string vatValue = GetSafeArrayValue(uitwisselingDATAArray, 12).Trim();
                    searchOnVat = vatValue.Length > 3 ? vatValue.Substring(2) : vatValue;
                }
                else
                {
                    MessageBox.Show("Nog logica te voorzien voor " + GetSafeArrayValue(uitwisselingDATAArray, 7), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string vatValue = GetSafeArrayValue(uitwisselingDATAArray, 12).Trim();
                    searchOnVat = vatValue.Length > 3 ? vatValue.Substring(2) : vatValue;
                }

                Msg = "SELECT A110, v150, A161, v404, A100, v259 FROM Leveranciers WHERE v150 = '" + searchCountry + "' AND A161 = '" + searchOnVat + "'";
                SnelHelpPrint(Msg, BL_LOGGING);
                Cursor.Current = Cursors.WaitCursor;
                _rsSupplier.Open(Msg, adntDB, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                Cursor.Current = Cursors.Default;

                if (_rsSupplier.RecordCount == 1)
                {
                    string checkUblIBAN = NormalizeVatOrCompany(GetSafeArrayValue(uitwisselingDATAArray, 15));
                    string checkDbIBAN = NormalizeVatOrCompany(FieldText(_rsSupplier, "v259"));
                    if (!string.IsNullOrWhiteSpace(GetSafeArrayValue(uitwisselingDATAArray, 15)) && checkUblIBAN != checkDbIBAN)
                    {
                        Msg = "IBAN rekening is verschillend met database." + Environment.NewLine + Environment.NewLine;
                        Msg += GetSafeArrayValue(uitwisselingDATAArray, 7) + Environment.NewLine + Environment.NewLine;
                        Msg += "* Peppol document: " + checkUblIBAN + Environment.NewLine;
                        Msg += "* Tabel leveranciers: " + checkDbIBAN + Environment.NewLine + Environment.NewLine;
                        Msg += "Wijzig de leveranciersfiche manueel indien nodig.";
                        MessageBox.Show(Msg, "IBAN verschillend", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    checkSupplier = true;
                }
                else if (_rsSupplier.RecordCount == 0)
                {
                    // No supplier found with the company number
                    // Why still ask for confirmation to create a new supplier? Because the user might want to add a new supplier based on the UBL document data.
                    if (_ifSupplierInsertWarning)
                    {
                        Msg = "Geen leverancier gevonden met het ondernemingsnummer " + GetSafeArrayValue(uitwisselingDATAArray, 5) + Environment.NewLine + Environment.NewLine;
                        Msg += "Naam: " + GetSafeArrayValue(uitwisselingDATAArray, 7) + Environment.NewLine;
                        Msg += "Straat: " + GetSafeArrayValue(uitwisselingDATAArray, 8) + Environment.NewLine;
                        Msg += "Plaatsnaam: " + GetSafeArrayValue(uitwisselingDATAArray, 9) + Environment.NewLine + Environment.NewLine;
                        Msg += "Controleer eerst of voeg een nieuwe leverancier toe" + Environment.NewLine + Environment.NewLine;
                        Msg += "Hierna wordt een nieuwe leverancier voorbereid" + Environment.NewLine;
                        Msg += "met gegevens uit het UBL document en ondernemingsnummer als codenummer." + Environment.NewLine + Environment.NewLine;
                        Msg += "Wijzig het codenummer indien nodig, druk enter om de fiche verder te vervolledigen (bij voorkeur eveneens een vaste kostenrekening aanduiden) en te bewaren. " + Environment.NewLine + Environment.NewLine;
                        Msg += "Venster Aankoopverrichtingen wordt geminimaliseerd. Klik onderaan of sneltoetscombinatie [Ctrl][F1] om terug te openen";

                        MessageBox.Show(Msg, "KBO nummer: " + GetSafeArrayValue(uitwisselingDATAArray, 5), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        INSERT_FLAG[TABLE_SUPPLIERS] = 1;
                        PeppolFlag = true;
                        TLB_RECORD[TABLE_SUPPLIERS] = string.Empty;
                        VBib(TABLE_SUPPLIERS, "2", "A10C");
                        VBib(TABLE_SUPPLIERS, searchCountry, "v150");
                        VBib(TABLE_SUPPLIERS, "002", "v149");
                        VBib(TABLE_SUPPLIERS, "B  ", "A109");
                        VBib(TABLE_SUPPLIERS, "EUR", "vs03");
                        VBib(TABLE_SUPPLIERS, "1", "vs07");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 5), "A110");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 5), "v404");

                        if (searchCountry == "BE")
                        {
                            VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 5), "A161");
                        }
                        else
                        {
                            string vatValue = GetSafeArrayValue(uitwisselingDATAArray, 12);
                            if (string.IsNullOrWhiteSpace(vatValue))
                                VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 5), "A161");
                            else
                                VBib(TABLE_SUPPLIERS, vatValue.Length > 3 ? vatValue.Substring(2) : vatValue, "A161");
                        }

                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 7), "A100");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 8), "A104");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 9), "A108");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 10), "A107");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 15), "v259");
                        VBib(TABLE_SUPPLIERS, GetSafeArrayValue(uitwisselingDATAArray, 17), "v260");

                        FormBasicTable supplierForm = BasisB[TABLE_SUPPLIERS] as FormBasicTable;
                        if (supplierForm != null)
                        {
                            // Temporarily disable the resize focus behavior
                            bool savedWarningState = _ifSupplierInsertWarning;
                            _ifSupplierInsertWarning = false;

                            // Minimize FormBuying temporarily to allow supplier form to be on top
                            WindowState = FormWindowState.Minimized;

                            // Restore the warning state
                            _ifSupplierInsertWarning = savedWarningState;

                            supplierForm.WindowState = FormWindowState.Normal;
                            supplierForm.TopMost = true;
                            supplierForm.Show();
                            supplierForm.BringToFront();
                            supplierForm.Activate();
                            supplierForm.TopMost = false;
                            supplierForm.MasketEditBoxInfo.Text = GetSafeArrayValue(uitwisselingDATAArray, 5);
                            supplierForm.MasketEditBoxInfo.SelectionStart = supplierForm.MasketEditBoxInfo.Text.Length;
                            supplierForm.ButtonEdit.Enabled = true;
                            supplierForm.AcceptButton = supplierForm.ButtonEdit;
                            supplierForm.MasketEditBoxInfo.Focus();
                        }
                        return false;
                    }
                    else
                    {
                        // During directory scan phase, allow documents with new suppliers to be added to the list
                        // The supplier will be created later when the user actually selects the document to process
                        checkSupplier = true;
                    }
                }
                else if (_rsSupplier.RecordCount > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Meerdere leveranciers gevonden met ondernemingsnummer ").Append(searchOnVat).Append(". Verbeter eerst").Append(Environment.NewLine).Append(Environment.NewLine);
                    _rsSupplier.MoveFirst();
                    while (!_rsSupplier.EOF)
                    {
                        sb.Append("=> ").Append(FieldText(_rsSupplier, "A110")).Append(" ").Append(FieldText(_rsSupplier, "A100")).Append(Environment.NewLine);
                        _rsSupplier.MoveNext();
                    }
                    MessageBox.Show(sb.ToString(), "KBO nummer: " + GetSafeArrayValue(uitwisselingDATAArray, 5), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Bron:" + Environment.NewLine + ex.Source + Environment.NewLine + Environment.NewLine + "Detail:" + Environment.NewLine + ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            return checkSupplier;
        }
    }
}
