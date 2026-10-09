using ADODB;
using System;
using System.Globalization;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.MimMenu.DailyManagement
{
    public partial class FormOGM : Form
    {
        private Recordset rsAny;

        private string[] psTekst = new string[6];
        private long TotaalD;
        private long TotaalC;

        private string[] VeldTXT = new string[18];

        private int TLijnen;
        private object invalidAA;

        // MAIN
        private string bancGuid = string.Empty;
        private string guidId = string.Empty;
        private string creationDateTime = string.Empty;

        private int numberOfTransactions;
        private string controlSum = string.Empty;

        private string initiatingPartyName = string.Empty;
        private string paymentInformationId = string.Empty;
        private string requestedExecutionDate = string.Empty;

        private string debtorName = string.Empty;
        private string debtorIBAN = string.Empty;
        private string debtorBIC = string.Empty;
        private string transactionsList = string.Empty;

        private string manyMemoDates = string.Empty;
        private int memoGroupCounter;

        public FormOGM()
        {
            InitializeComponent();            
        }

        private void FormOGM_Load(object sender, EventArgs e)
        {
            debtorName = String99(46).Trim();
            debtorName = CheckforAmp(debtorName);
            initiatingPartyName = debtorName;

            ComboBoxSelectedBancAccount.Items.Clear();
            BGetOrGreater(TABLE_VARIOUS, 1, "28");
            if (Ktrl == 0 && PartLeft(KEY_BUF[TABLE_VARIOUS], 2) == "28")
            {
                do
                {
                    RecordToVeld(TABLE_VARIOUS);
                    ComboBoxSelectedBancAccount.Items.Add(VBibText(TABLE_VARIOUS, "#v231 #") + ": " +
                        VBibText(TABLE_VARIOUS, "#v232 #") + " [" + VBibText(TABLE_VARIOUS, "#v259 #") + "]");
                    BNext(TABLE_VARIOUS);
                } while (Ktrl == 0 && PartLeft(KEY_BUF[TABLE_VARIOUS], 2) == "28");

                if (ComboBoxSelectedBancAccount.Items.Count > 0)
                    ComboBoxSelectedBancAccount.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Eerst parameters van een financiële instelling inbrengen via 'Diverse Gebruikersfiches' a.u.b.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }

            dtpMemoDatum.MinDate = DateTime.Now;
            creationDateTime = GetCreationDateTime();

            DatumVerwerking.Value = DateTime.Now;
            DTPickerGlobalMemoDate.Value = DatumVerwerking.Value.AddDays(1);

            TextBoxFrom.Text = "0";
            TextBoxTo.Text = new string('z', 12);

            FlexGridPayments.Rows.Clear();
            FlexGridPayments.Columns.Clear();
            AddGridColumns(FlexGridPayments);
            if (FlexGridPayments.Rows.Count == 0)
                FlexGridPayments.Rows.Add();

            FlexGridGhost.Rows.Clear();
            FlexGridGhost.Columns.Clear();
            AddGridColumns(FlexGridGhost);
            if (FlexGridGhost.Rows.Count == 0)
                FlexGridGhost.Rows.Add();            
        }

        private void GhostRefresh(string comboValue)
        {
            double totalAmount = 0;
            int rowCounter;
            int columnCounter;
            string aa = string.Empty;
            string groupValue = PartLeft(comboValue, 10);

            FlexGridGhost.Rows.Clear();
            if (FlexGridGhost.Rows.Count == 0)
                FlexGridGhost.Rows.Add();

            for (rowCounter = 1; rowCounter < FlexGridPayments.Rows.Count; rowCounter++)
            {
                if (ComboGuidLabel.Items.Count > 1)
                {
                    if (groupValue == GetGridCell(FlexGridPayments, rowCounter, 3))
                    {
                        totalAmount += DoubleFromString(GetGridCell(FlexGridPayments, rowCounter, 5));
                        aa = string.Empty;
                        for (columnCounter = 0; columnCounter <= 7; columnCounter++)
                        {
                            aa += GetGridCell(FlexGridPayments, rowCounter, columnCounter) + "\t";
                        }
                        AddGridRow(FlexGridGhost, aa);
                    }
                }
                else
                {
                    aa = GetGridCell(FlexGridPayments, rowCounter, 0) + "\t";
                    aa += GetGridCell(FlexGridPayments, rowCounter, 1) + "\t";
                    aa += GetGridCell(FlexGridPayments, rowCounter, 2) + "\t";
                    aa += groupValue + "\t";
                    aa += GetGridCell(FlexGridPayments, rowCounter, 4) + "\t";
                    aa += GetGridCell(FlexGridPayments, rowCounter, 5) + "\t";
                    aa += GetGridCell(FlexGridPayments, rowCounter, 6) + "\t";
                    aa += GetGridCell(FlexGridPayments, rowCounter, 7);
                    AddGridRow(FlexGridGhost, aa);
                }
            }

            if (ComboGuidLabel.Items.Count > 1)
            {
                lblEUR.Text = Math.Round(totalAmount, 2).ToString("#,###.00", CultureInfo.InvariantCulture);
            }
        }

        private void InitVelden()
        {
            REPORT_FIELD[0] = "Lijn";
            REPORT_TAB[0] = 5;

            REPORT_FIELD[1] = "MemoDatum";
            REPORT_TAB[1] = 10;

            REPORT_FIELD[2] = "    Bedrag";
            REPORT_TAB[2] = 21;

            REPORT_FIELD[3] = "Munt";
            REPORT_TAB[3] = 32;

            REPORT_FIELD[4] = "Begunstigde";
            REPORT_TAB[4] = 37;

            REPORT_FIELD[5] = "Rekeningnr.";
            REPORT_TAB[5] = 68;

            REPORT_FIELD[6] = "OGM/Referte";
            REPORT_TAB[6] = 87;

            REPORT_FIELD[7] = "DocumentID";
            REPORT_TAB[7] = 102;

            REPORT_TAB[8] = 0;
        }

        private void PrintTitel()
        {
            int T = 0;

            PAGE_COUNTER++;

            if (!string.IsNullOrEmpty(usrLicentieInfo))
            {
                // Printer output would go here in full implementation
            }

            while (REPORT_TAB[T] != 0)
            {
                T++;
            }
        }

        private void PrintVelden()
        {
            int T = 0;
            string aa = string.Empty;

            while (REPORT_TAB[T] != 0)
            {
                aa += VeldTXT[T] + "\t";
                T++;
            }
        }

        private void PrintTotaal()
        {
            if (FlexGridPayments.Rows.Count <= 2) return;

            for (int T = 0; T <= 6; T++)
            {
                VeldTXT[T] = string.Empty;
            }

            VeldTXT[1] = "Totaal EUR";
            VeldTXT[2] = Dec(DoubleFromString(lblEUR.Text), MASK_EUR);
            VeldTXT[7] = ComboGuidLabel.Text;
        }

        private void MemoRefresh()
        {
            double totalAmount = 0;
            int Teller;
            string groupValue;

            manyMemoDates = string.Empty;
            memoGroupCounter = 0;

            for (Teller = 1; Teller < FlexGridPayments.Rows.Count; Teller++)
            {
                groupValue = GetGridCell(FlexGridPayments, Teller, 3) + "#";
                if (!manyMemoDates.Contains(groupValue))
                {
                    memoGroupCounter++;
                    manyMemoDates += groupValue;
                }
                totalAmount += DoubleFromString(GetGridCell(FlexGridPayments, Teller, 5));
            }

            lblEUR.Text = Math.Round(totalAmount, 2).ToString("#,###.00", CultureInfo.InvariantCulture);

            ComboGuidLabel.Items.Clear();
            if (CheckBoxForceExecutionDate.Checked)
            {
                LabelMemoGroups.Text = "1";
            }
            else
            {
                LabelMemoGroups.Text = memoGroupCounter.ToString();
            }
            RefreshGroupIds();
        }

        private void RefreshGroupIds()
        {
            string bankCombo = ComboBoxSelectedBancAccount.SelectedItem?.ToString() ?? string.Empty;
            string bankSettingsKey = VSet("28" + bankCombo.Substring(0, Math.Min(3, bankCombo.Length)), 5).Trim();

            BGetOrGreater(TABLE_VARIOUS, 1, bankSettingsKey);

            if (Ktrl != 0 || PartLeft(KEY_BUF[TABLE_VARIOUS], 5) != bankSettingsKey.Trim())
            {
                MessageBox.Show("Eerst financiële instelling parameters inbrengen via 'Diverse Gebruikersfiches' a.u.b.", 
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            RecordToVeld(TABLE_VARIOUS);
            debtorIBAN = VBibText(TABLE_VARIOUS, "#v259 #").Trim();
            debtorBIC = VBibText(TABLE_VARIOUS, "#v260 #").Trim();

            string[] memoArray = manyMemoDates.Split('#');

            if (LabelMemoGroups.Text == "1")
            {
                bancGuid = GetNewGUID().Substring(1, 13);
                string memoDateBuilder = Dec(DTPickerGlobalMemoDate.Value.Day, "00") + "/" +
                                        Dec(DTPickerGlobalMemoDate.Value.Month, "00") + "/" +
                                        Dec(DTPickerGlobalMemoDate.Value.Year, "0000");
                ComboGuidLabel.Items.Add(memoDateBuilder + " : " + 
                    VBibText(TABLE_VARIOUS, "#v237 #").Trim() + "-" + bancGuid);
            }
            else
            {
                for (int T = 1; T <= memoGroupCounter; T++)
                {
                    bancGuid = GetNewGUID().Substring(1, 13);
                    ComboGuidLabel.Items.Add(memoArray[T - 1] + " : " + 
                        VBibText(TABLE_VARIOUS, "#v237 #").Trim() + "-" + bancGuid);
                }
            }

            if (ComboGuidLabel.Items.Count > 0)
                ComboGuidLabel.SelectedIndex = 0;
        }

        private string XmlOGM(string comboValue)
        {
            string ghostDate = PartLeft(comboValue, 10);
            guidId = PartRight(comboValue, 21);

            controlSum = Dec(DoubleFromString(lblEUR.Text), ".00");
            numberOfTransactions = (FlexGridGhost.Rows.Count - 1);
            paymentInformationId = GetNewGUID().Substring(1, 13);

            requestedExecutionDate = PartRight(ghostDate, 4) + "-" +
                                     PartMid(ghostDate, 4, 2) + "-" +
                                     PartLeft(ghostDate, 2);

            // XML template processing would go here
            // This is a placeholder for the complex XML handling

            return string.Empty;
        }

        private string GetGridCell(DataGridView grid, int row, int col)
        {
            if (row >= 0 && row < grid.Rows.Count && col >= 0 && col < grid.Columns.Count)
                return grid.Rows[row].Cells[col].Value?.ToString() ?? string.Empty;
            return string.Empty;
        }

        private void AddGridRow(DataGridView grid, string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var parts = data.Split('\t');
                grid.Rows.Add(parts.Length > 0 ? parts : new object[1]);
            }
        }

        private void ComboBoxSelectedBancAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGroupIds();
        }

        private void CheckBoxForceExecutionDate_CheckedChanged(object sender, EventArgs e)
        {
            if (FlexGridPayments.Rows.Count <= 2) return;

            if (CheckBoxForceExecutionDate.Checked)
            {
                DTPickerGlobalMemoDate.Visible = true;
                DTPickerGlobalMemoDate.Value = DatumVerwerking.Value.AddDays(1);
                FrameChangeMemoDate.Visible = false;
            }
            else
            {
                DTPickerGlobalMemoDate.Visible = false;
                FrameChangeMemoDate.Visible = true;
            }

            MemoRefresh();
        }

        private void CmdEmailNBB_Click(object sender, EventArgs e)
        {
            bool result;
            int volgNR;

            for (int ghost = 0; ghost < ComboGuidLabel.Items.Count; ghost++)
            {
                GhostRefresh(ComboGuidLabel.Items[ghost].ToString());
                XmlOGM(ComboGuidLabel.Items[ghost].ToString());

                for (volgNR = 1; volgNR < FlexGridGhost.Rows.Count; volgNR++)
                {
                    result = ADO_GET(TABLE_INVOICES, 0, "=", GetGridCell(FlexGridGhost, volgNR, 2));
                    if (result)
                    {
                        rsMAR[TABLE_INVOICES].Fields["v411"].Value = guidId;
                        rsMAR[TABLE_INVOICES].Fields["rvDM"].Value = "0";
                        rsMAR[TABLE_INVOICES].Fields["dnnsync"].Value = false;
                        rsMAR[TABLE_INVOICES].Update();
                    }
                }
            }

            string Msg = "In map Manueel vindt U .xda bestand(en) klaar voor import in de toepassing van de aangeduide bank." + 
                  Environment.NewLine + Environment.NewLine +
                  "VERGEET DEZE NIET te verwijderen na succesvol ondertekenen." + Environment.NewLine + Environment.NewLine +
                  "Copijen blijven steeds behouden in uw bedrijfsinhoudsopgave onder submap coda\\out";
            MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Close();
        }

        private void cmdSluiten_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DTPickerGlobalMemoDate_ValueChanged(object sender, EventArgs e)
        {
            if (DTPickerGlobalMemoDate.Value < DatumVerwerking.Value)
            {
                DTPickerGlobalMemoDate.Value = DatumVerwerking.Value.AddDays(1);
            }
            MemoRefresh();
        }
        
        private void AddGridColumns(DataGridView grid)
        {
            grid.Columns.Add("IDCode", "ID Code");
            grid.Columns.Add("Naam", "Naam");
            grid.Columns.Add("Document", "Document");
            grid.Columns.Add("Memodatum", "Memodatum");
            grid.Columns.Add("Munt", "Munt");
            grid.Columns.Add("Bedrag", "Bedrag");
            grid.Columns.Add("SEPARekening", "SEPA Rekening");
            grid.Columns.Add("Referte", "Referte/OGM");
        }

        private void Samenstellen_Click(object sender, EventArgs e)
        {
            if (TextBoxFrom.Text.Trim() == "0")
            {
                BFirst(TABLE_SUPPLIERS, 0);
                TextBoxFrom.Text = KEY_BUF[TABLE_SUPPLIERS];
                BLast(TABLE_SUPPLIERS, 0);
                TextBoxTo.Text = KEY_BUF[TABLE_SUPPLIERS];
            }

            KTRLBalans(TABLE_SUPPLIERS);

            if (FlexGridPayments.Rows.Count > 2)
            {
                MemoRefresh();
                FlexGridPayments.Focus();
                Samenstellen.Enabled = false;
            }
        }

        private void KTRLBalans(int Fl)
        {
            // Complex method with database queries - implementation would follow the VB6 logic
            // This requires substantial translation of the RecordSet logic below
        }

        private string GetCreationDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd\'T\'HH:mm:ss");
        }

        private string CheckforAmp(string input)
        {
            return input?.Replace("&", "&amp;") ?? string.Empty;
        }

        private string GetNewGUID()
        {
            return Guid.NewGuid().ToString("B").ToUpper();
        }
    }
}
