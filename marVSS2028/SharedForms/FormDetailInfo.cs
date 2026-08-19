using System;
using System.Globalization;
using System.Windows.Forms;

using marVSS2028.PublicForms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.SharedForms
{
    public partial class FormDetailInfo : Form
    {
        private string _tegenrekening = string.Empty;
        private string _defaultKlanten = string.Empty;
        private string _defaultLeveranciers = string.Empty;

        public FormDetailInfo()
        {
            InitializeComponent();
            WireHighlightEvents(this);
            Load += DetailInfo_Load;
            Shown += DetailInfo_Shown;
        }
        
        private void DetailInfo_Load(object sender, EventArgs e)
        {
            _defaultKlanten = (String99(9) ?? string.Empty).TrimEnd();
            _defaultLeveranciers = (String99(10) ?? string.Empty).TrimEnd();
            TekstInfo0.Text = _defaultKlanten;
            Bewerking.Focus();
        }

        private void DetailInfo_Shown(object sender, EventArgs e)
        {
            Bewerking.Focus();
        }

        private void Balans_Click(object sender, EventArgs e)
        {
            // Balans.Text = "Bala&nscontrole";
            SharedFl = Partij.Checked ? TABLE_CUSTOMERS : TABLE_SUPPLIERS;
            GridText = string.Empty;
            aIndex = 1;

            using (FormSearchSQL sqlSearch = new FormSearchSQL())
                sqlSearch.ShowDialog(this);

            if (Ktrl == 0)
            {
                RecordToVeld(SharedFl);
                KTRLBalans(SharedFl);
            }
        }
        
        private void CmdBank_Click(object sender, EventArgs e)
        {
            if (ReferenceEquals(sender, cmdBank0))
            {
                if (!string.IsNullOrEmpty(tbBank0.Text))
                    MessageBox.Show("test voor sepa webservice", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!string.IsNullOrEmpty(tbBank1.Text))
                    MessageBox.Show("test voor sepa webservice", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
                
        private void Ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TekstInfo0.Text))
            {
                MessageBox.Show("Tegenrekening aanduiden a.u.b. !");
                TekstInfo0.Focus();
                return;
            }

            if (Dokument.Checked)
            {
                BGet(TABLE_INVOICES, 0, VSet(TekstInfo5.Text ?? string.Empty, 11));
                if (Ktrl != 0)
                {
                    MessageBox.Show("Documentnummer onbekend !!!");
                    Dokument.Focus();
                    return;
                }
            }

            if (Val(TekstInfo1.Text) + Val(TekstInfo2.Text) == 0)
            {
                MessageBox.Show("Bedrag - of + inbrengen a.u.b. !!!");
                return;
            }

            string kVlag = Bewerking.Checked ? "+" : "-";
            string kolom1 = Dokument.Checked ? kVlag + (TekstInfo5.Text ?? string.Empty) : kVlag;
            string kolom2 = (TekstInfo0.Text ?? string.Empty);
            string kolom3 = Dec(Val(TekstInfo2.Text), MASK_EURBH);
            string kolom4 = (TekstInfo3.Text ?? string.Empty);
            string kolom5 = Dec(Val(TekstInfo1.Text), MASK_EURBH);

            GridText = VSet(kolom1,12) + "|" + VSet(kolom2,7) + "|" + VSet(kolom3,12) + "|" + VSet(kolom4, 29) + "|" + VSet(kolom5,12);
            Hide();

            if (Application.OpenForms["FormBankingTransactions"] is Form inbreng)
                inbreng.Focus();
        }        

        private void TekstInfo_GotFocus(object sender, EventArgs e)
        {
            // Balans.Text = "Bala&nscontrole";

            var tb = sender as TextBox;
            if (tb != null)
                tb.SelectAll();

            if (ReferenceEquals(sender, TekstInfo0))
            {
                SnelHelpPrint("[Ctrl] voor geïndexeerd zoeken", BL_LOGGING);
                Bewerking.Enabled = false;
                Dokument.Enabled = false;
                Partij.Enabled = false;                
            }
            else if (ReferenceEquals(sender, TekstInfo5))
            {
                ZoekDokument.Enabled = true;
                ZoekDokument.Focus();
            }
        }

        private void TekstInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (ReferenceEquals(sender, TekstInfo0) && e.KeyCode == Keys.ControlKey)
            {
                SharedFl = TABLE_LEDGERACCOUNTS;
                aIndex = 0;
                GridText = TekstInfo0.Text;

                using (var sql = new FormSearchSQL())
                    sql.ShowDialog(this);

                TekstInfo0.Text = Ktrl == 0 ? FVT[TABLE_LEDGERACCOUNTS, 0] : string.Empty;
                return;
            }

            if (sender is TextBox && e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TekstInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!ReferenceEquals(sender, TekstInfo5))
                return;

            string allowed = "*.0123456789" + (char)8;
            if (allowed.IndexOf(e.KeyChar) >= 0)
            {
                OK.Enabled = false;
                return;
            }

            e.Handled = true;
            string msg = "Syntax : [*] . xxxxx . [eejj]" + Environment.NewLine + Environment.NewLine
                       + "*" + Environment.NewLine
                       + "'Q0'-Kwijting klanten (makelaars), programma maakt" + Environment.NewLine
                       + "anders 'V0'-sleutel, ontvangst van klantfaktuur, 'V1'" + Environment.NewLine
                       + "uitgave creditnota, 'A0' faktuur leverancier of 'A1'" + Environment.NewLine
                       + "creditnota leverancier" + Environment.NewLine + Environment.NewLine
                       + "xxxxx" + Environment.NewLine
                       + "dokument volgnummer (verplicht) van 1 tot 99999." + Environment.NewLine + Environment.NewLine
                       + "eejj" + Environment.NewLine
                       + "Eeuw dokument van 1900 tot max 2099 (optioneel)" + Environment.NewLine + Environment.NewLine
                       + "Opties door '.' scheiden van elkaar a.u.b !" + Environment.NewLine + Environment.NewLine
                       + "Vb. *.542.1992 = kwijting Q0199200542 van '1992'" + Environment.NewLine
                       + "Vb. 542 zijnde aan- of verkoopdokument ??????00542 van 'huidig jaar'";
            MessageBox.Show(msg);
        }

        private void TekstInfo_LostFocus(object sender, EventArgs e)
        {
            if (ReferenceEquals(sender, TekstInfo0))
            {
                if (!string.IsNullOrWhiteSpace(TekstInfo0.Text))
                {
                    BGet(TABLE_LEDGERACCOUNTS, 0, VSet(TekstInfo0.Text, 7));
                    if (Ktrl != 0)
                    {
                        System.Media.SystemSounds.Beep.Play();
                        TekstInfo0.Text = string.Empty;
                        LabelInfo6.Text = string.Empty;
                    }
                    else
                    {
                        RecordToVeld(TABLE_LEDGERACCOUNTS);
                        LabelInfo6.Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
                    }
                }
            }
            else if (ReferenceEquals(sender, TekstInfo1))
            {
                if (Val(TekstInfo1.Text) != 0)
                {
                    const string msg = "Financiele korting van bedrag aftrekken.";
                    var result = MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        TekstInfo2.Text = (Val(TekstInfo2.Text) - Val(TekstInfo1.Text)).ToString(CultureInfo.InvariantCulture);
                        TekstInfo1.Enabled = false;
                        TekstInfo2.Enabled = false;
                    }
                    else
                    {
                        TekstInfo1.Text = string.Empty;
                    }
                }
            }
            else if (ReferenceEquals(sender, TekstInfo5))
            {
                ZoekDokument.Enabled = false;
            }
        }

        private void ZoekDokument_Click(object sender, EventArgs e)
        {
            string kontroleTekst = (TekstInfo5.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(kontroleTekst))
                return;

            string dokType;
            if (PartLeft(kontroleTekst, 1) == "*")
            {
                dokType = "Q0";
                kontroleTekst = PartRight(kontroleTekst, kontroleTekst.Length - 1);
                if (PartLeft(kontroleTekst, 1) == ".")
                    kontroleTekst = PartRight(kontroleTekst, kontroleTekst.Length - 1);
            }
            else
            {
                dokType = string.Empty;
            }

            string nummer;
            string jaar;
            int dotPos = kontroleTekst.IndexOf('.');
            if (dotPos >= 0)
            {
                nummer = Val(PartLeft(kontroleTekst, dotPos)).ToString("00000", CultureInfo.InvariantCulture);
                jaar = PartRight(kontroleTekst, 4);
            }
            else
            {
                nummer = Val(kontroleTekst).ToString("00000", CultureInfo.InvariantCulture);
                jaar = PartRight(MIM_GLOBAL_DATE, 4);
            }

            if (dokType != "Q0")
            {
                if (Partij.Checked)
                    dokType = Bewerking.Checked ? "V0" : "V1";
                else
                    dokType = Bewerking.Checked ? "A1" : "A0";
            }

            BGet(TABLE_INVOICES, 0, dokType + jaar + nummer);
            if (Ktrl != 0)
            {
                TekstInfo1.Text = string.Empty;
                TekstInfo2.Text = string.Empty;
                TekstInfo3.Text = string.Empty;
                TekstInfo5.Text = string.Empty;
                SnelHelpPrint(dokType + jaar + nummer + " niet gevonden...", BL_LOGGING);
                return;
            }

            RecordToVeld(TABLE_INVOICES);
            TekstInfo1.Text = string.Empty;

            double dBetaald = Val(VBibText(TABLE_INVOICES, "#v037 #"));
            double dTotaal = Val(VBibText(TABLE_INVOICES, "#v249 #"));

            if (XisEuroWisBEF)
            {
                dBetaald = Math.Round(dBetaald * EURO);
                dTotaal = Math.Round(dTotaal * EURO);
            }

            if (dTotaal - dBetaald != 0)
            {
                TekstInfo2.Text = (dTotaal - dBetaald).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                string msg = "Document reeds volledig betaald." + Environment.NewLine
                           + "Bedragen die nu bijgeteld worden," + Environment.NewLine
                           + "einde van het boekjaar rechtzetten !!!";
                MessageBox.Show(msg, "Dubbele betaling...");
            }

            SharedFl = PartLeft(FVT[TABLE_INVOICES, 0], 1) == "A" ? TABLE_SUPPLIERS : TABLE_CUSTOMERS;

            BGet(SharedFl, 0, PartMid(VBibText(TABLE_INVOICES, "#v034 #"), 2, 999));
            if (Ktrl == 0)
            {
                RecordToVeld(SharedFl);
                TekstInfo3.Text = VBibText(SharedFl, "#A100 #");
                tbBank0.Text = VBibText(SharedFl, "#A170 #");
                tbBank1.Text = VBibText(SharedFl, "#v251 #");
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
            }

            TekstInfo5.Text = FVT[TABLE_INVOICES, 0];
            Bewerking.Enabled = false;
            Dokument.Enabled = false;
            Partij.Enabled = false;
            OK.Enabled = true;
            OK.Focus();
        }
        
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            GridText = string.Empty;
            Hide();

            // if (Application.OpenForms["FormBankingTransactions"] is Form inbreng)
            //     inbreng.Focus();
        }
        
        private void Bewerking_CheckedChanged(object sender, EventArgs e)
        {
            if (Bewerking.Checked)
            {
                Bewerking.Text = "= Ontvangst";
                Partij.Checked = true;
            }
            else
            {
                Bewerking.Text = "= Uitgave";
                Partij.Checked = false;
            }

            Partij_CheckedChanged(sender, e);
        }

        private void Dokument_CheckedChanged(object sender, EventArgs e)
        {
            if (Dokument.Checked)
            {
                Dokument.Text = "= Document";
                Balans.Enabled = true;
                TekstInfo1.Visible = true;
                LabelInfo1.Visible = true;
                TekstInfo5.Visible = true;
                Partij.Visible = true;
                TekstInfo0.Text = Partij.Checked ? _defaultKlanten : _defaultLeveranciers;
            }
            else
            {
                Dokument.Text = "= Geen Document";
                Partij.Visible = false;
                TekstInfo1.Visible = false;
                LabelInfo1.Visible = false;
                TekstInfo5.Visible = false;
                Balans.Enabled = false;
                TekstInfo0.Text = string.Empty;
                TekstInfo1.Text = string.Empty;
            }
        }

        private void Partij_CheckedChanged(object sender, EventArgs e)
        {
            if (Partij.Checked)
            {
                Partij.Text = "= &Klant";
                TekstInfo0.Text = _defaultKlanten;
            }
            else
            {
                Partij.Text = "= &Leverancier";
                TekstInfo0.Text = _defaultLeveranciers;
            }
        }

        private static double Val(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0d;
            double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private void KTRLBalans(int fl)
        {
            double cumul = 0d;
            int lijnenOpenstaand = 0;
            string voorLetter = string.Empty;

            switch (fl)
            {
                case TABLE_CUSTOMERS:
                    voorLetter = "K";
                    break;
                case TABLE_SUPPLIERS:
                    voorLetter = "L";
                    break;
            }

            BClose(TABLE_INVOICES);

            string partyName = (VBibText(fl, "#A100 #") ?? string.Empty).TrimEnd();
            string sleutelPrefix = voorLetter + (VBibText(fl, "#A110 #") ?? string.Empty);
            string sleutel13 = VSet(sleutelPrefix, 13);

            using (var xlog = new FormXLog())
            {
                xlog.Text = "Betaalbalans voor : " + partyName;
                xlog.X.Columns.Clear();
                xlog.X.Columns.Add("colDok", "Document");
                xlog.X.Columns.Add("colTot", "Totaal");
                xlog.X.Columns.Add("colDat", "Datum");
                xlog.X.Columns.Add("colFin", "Fin.Stuk");
                xlog.X.Columns.Add("colBet", "Betaald");
                xlog.X.Columns.Add("colCum", "CumulRest");

                BGetOrGreater(TABLE_INVOICES, 1, VSet(sleutelPrefix, FLINDEX_LEN[TABLE_INVOICES, 1]));
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    return;
                }

                RecordToVeld(TABLE_INVOICES);
                if (VSet(KEY_BUF[TABLE_INVOICES], 13) != sleutel13)
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("Geen documenten voor " + partyName);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    AddVolgendeLijn(xlog);

                    while (true)
                    {
                        BNext(TABLE_INVOICES);
                        if (Ktrl != 0 || VSet(KEY_BUF[TABLE_INVOICES], 13) != sleutel13)
                            break;

                        RecordToVeld(TABLE_INVOICES);
                        AddVolgendeLijn(xlog);
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                if (lijnenOpenstaand == 0)
                {
                    MessageBox.Show("Alles is betaald voor/door" + Environment.NewLine + partyName, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                xlog.X.Columns[0].Width = 103; // 1215 / 15;
                xlog.X.Columns[1].Width = 96;  // 1125 / 15;
                xlog.X.Columns[2].Width = 90;  // 930 / 15;
                xlog.X.Columns[3].Width = 82;  // 1005 / 15;
                xlog.X.Columns[4].Width = 102; // 975 / 15;
                xlog.X.Columns[5].Width = 103; // 1185 / 15;

                xlog.X.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                xlog.X.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                xlog.X.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                xlog.AcceptButton = xlog.BtnAfsluiten;
                xlog.BtnWijzigenLijn.Visible = false;
                xlog.BtnAfsluiten.TabStop = false;
                xlog.BtnAfbeelding.Visible = false;
                XLogKey = string.Empty;
                if (xlog.TabControl1.TabPages.Count > 1)
                    xlog.TabControl1.TabPages[1].Visible = false;

                xlog.ShowDialog(this);
            }

            if (string.IsNullOrEmpty(XLogKey))
                return;

            int keySep = XLogKey.IndexOf('\r');
            string docKey = keySep > 0 ? PartLeft(XLogKey, keySep) : XLogKey;

            BGet(TABLE_INVOICES, 0, docKey);
            if (Ktrl != 0)
                return;

            RecordToVeld(TABLE_INVOICES);
            TekstInfo1.Text = string.Empty;

            if (XisEuroWisBEF)
            {
                double saldo = Math.Round(Val(VBibText(TABLE_INVOICES, "#v249 #")) * EURO)
                             - Math.Round(Val(VBibText(TABLE_INVOICES, "#v037 #")) * EURO);
                TekstInfo2.Text = saldo.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                double totaalBedrag = Val(VBibText(TABLE_INVOICES, "#v249 #"));
                double totaalBetaald = Val(VBibText(TABLE_INVOICES, "#v037 #"));
                TekstInfo2.Text = Dec(totaalBedrag - totaalBetaald, string.Empty);
            }

            TekstInfo3.Text = partyName;
            tbBank0.Text = VBibText(fl, "#A170 #");
            tbBank1.Text = VBibText(fl, "#v251 #");
            TekstInfo5.Text = VBibText(TABLE_INVOICES, "#v033 #");
            TekstInfo5.Enabled = false;
            Bewerking.Enabled = false;
            Dokument.Enabled = false;
            Partij.Enabled = false;
            Balans.Enabled = false;
            if (!OK.Enabled)
                OK.Enabled = true;
            OK.Focus();

            void AddVolgendeLijn(FormXLog xlogForm)
            {
                string v033 = VBibText(TABLE_INVOICES, "#v033 #") ?? string.Empty;
                string type2 = PartLeft(v033, 2);

                switch (type2)
                {
                    case "A0":
                    case "V1":
                        if (Bewerking.Checked)
                            return;
                        break;
                    case "A1":
                    case "V0":
                        if (!Bewerking.Checked)
                            return;
                        break;
                }

                double dBetaald = Val(VBibText(TABLE_INVOICES, "#v037 #"));
                if (XisEuroWisBEF)
                    dBetaald = Math.Round(dBetaald * EURO);

                double dTotaal = 0d;

                if (fl == TABLE_CUSTOMERS)
                {
                    string type1 = PartLeft(v033, 1);

                    if (type1 == "V")
                    {
                        dTotaal = Val(VBibText(TABLE_INVOICES, "#v249 #"));
                        if (XisEuroWisBEF)
                        {
                            MessageBox.Show("CTRLstop");
                            dTotaal = Math.Round(dTotaal * EURO);
                        }

                        if (PartMid(v033, 2, 1) == "1")
                        {
                            dTotaal = -dTotaal;
                            dBetaald = -dBetaald;
                        }
                    }
                    else if (type1 == "Q")
                    {
                        dTotaal = Val(VBibText(TABLE_INVOICES, "#v249 #"));
                        if (XisEuroWisBEF)
                            dTotaal = Math.Round(dTotaal * EURO);
                    }
                }
                else if (fl == TABLE_SUPPLIERS)
                {
                    dTotaal = Val(VBibText(TABLE_INVOICES, "#v249 #"));
                    if (XisEuroWisBEF)
                        dTotaal = Math.Round(dTotaal * EURO);

                    if (type2 == "A1")
                    {
                        dTotaal = -dTotaal;
                        dBetaald = -dBetaald;
                    }
                }

                cumul += dTotaal - dBetaald;
                if (dBetaald == dTotaal)
                    return;

                lijnenOpenstaand++;
                xlogForm.X.Rows.Add(
                    v033,
                    dTotaal.ToString("#,##0.00", CultureInfo.InvariantCulture),
                    DateText(VBibText(TABLE_INVOICES, "#v035 #")),
                    VBibText(TABLE_INVOICES, "#v038 #"),
                    dBetaald.ToString("#,##0.00", CultureInfo.InvariantCulture),
                    cumul.ToString("#,##0.00", CultureInfo.InvariantCulture));
            }
        }
    }    
}
