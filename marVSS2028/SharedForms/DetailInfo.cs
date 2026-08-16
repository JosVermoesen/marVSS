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
    public partial class DetailInfo : Form
    {
        private string _tegenrekening = string.Empty;
        private string _defaultKlanten = string.Empty;
        private string _defaultLeveranciers = string.Empty;

        public DetailInfo()
        {
            InitializeComponent();
            WireHighlightEvents(this);
            Load += DetailInfo_Load;
        }

        private static double Val(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0d;
            double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
            return result;
        }

        private void DetailInfo_Load(object sender, EventArgs e)
        {
            _defaultKlanten = (String99(9) ?? string.Empty).TrimEnd();
            _defaultLeveranciers = (String99(10) ?? string.Empty).TrimEnd();
            TekstInfo0.Text = _defaultKlanten;
        }

        private void Annuleren_Click(object sender, EventArgs e)
        {
            GridText = string.Empty;
            Hide();

            if (Application.OpenForms["FormBankingTransactions"] is Form inbreng)
                inbreng.Focus();
        }

        private void Balans_Click(object sender, EventArgs e)
        {
            Balans.Text = "Bala&nscontrole";
            SharedFl = Partij.Checked ? TABLE_CUSTOMERS : TABLE_SUPPLIERS;
            GridText = string.Empty;
            aIndex = 1;

            using (var sql = new FormSearchSQL())
                sql.ShowDialog(this);

            if (Ktrl == 0)
            {
                RecordToVeld(SharedFl);
                KTRLBalans(SharedFl);
            }
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
            Dokument.Focus();
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

        private void Dokument_CheckedChanged(object sender, EventArgs e)
        {
            if (Dokument.Checked)
            {
                Dokument.Text = "= dokument";
                Balans.Enabled = true;
                TekstInfo1.Visible = true;
                LabelInfo1.Visible = true;
                TekstInfo5.Visible = true;
                Partij.Visible = true;
                TekstInfo0.Text = Partij.Checked ? _defaultKlanten : _defaultLeveranciers;
            }
            else
            {
                Dokument.Text = "= Geen DOK";
                Partij.Visible = false;
                TekstInfo1.Visible = false;
                LabelInfo1.Visible = false;
                TekstInfo5.Visible = false;
                Balans.Enabled = false;
                TekstInfo0.Text = string.Empty;
                TekstInfo1.Text = string.Empty;
            }
        }

        private void Dokument_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
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
                    MessageBox.Show("dokumentnummer onbekend !!!");
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

            GridText = kolom1 + "|" + kolom2 + "|" + kolom3 + "|" + kolom4 + "|" + kolom5;
            Hide();

            if (Application.OpenForms["FormBankingTransactions"] is Form inbreng)
                inbreng.Focus();
        }

        private void Partij_CheckedChanged(object sender, EventArgs e)
        {
            if (Partij.Checked)
            {
                Partij.Text = "= Klant";
                TekstInfo0.Text = _defaultKlanten;
            }
            else
            {
                Partij.Text = "= Leverancier";
                TekstInfo0.Text = _defaultLeveranciers;
            }
        }

        private void TekstInfo_GotFocus(object sender, EventArgs e)
        {
            Balans.Text = "Bala&nscontrole";

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
            if (ReferenceEquals(sender, TekstInfo0))
            {
                if (e.KeyCode == Keys.ControlKey)
                {
                    SharedFl = TABLE_LEDGERACCOUNTS;
                    aIndex = 0;
                    GridText = TekstInfo0.Text;

                    using (var sql = new FormSearchSQL())
                        sql.ShowDialog(this);

                    TekstInfo0.Text = Ktrl == 0 ? FVT[TABLE_LEDGERACCOUNTS, 0] : string.Empty;
                }
                else if (e.KeyCode == Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    TekstInfo2.Focus();
                }
            }
            else if (ReferenceEquals(sender, TekstInfo2) && e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                TekstInfo3.Focus();
            }
            else if (ReferenceEquals(sender, TekstInfo3) && e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                OK.Focus();
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

            BGet(SharedFl, 0, SafeMid(VBibText(TABLE_INVOICES, "#v034 #"), 2, 999));
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

        private void KTRLBalans(int fl)
        {
            // Full VB6 Xlog grid-flow depends on legacy form internals.
            // Keep behavior functional: load selected party data for current invoice.
            try
            {
                string partyName = VBibText(fl, "#A100 #");
                SnelHelpPrint("Betaalbalans voor : " + partyName, BL_LOGGING);
            }
            catch
            {
                // Keep VB6-like fault tolerance.
            }
        }
    }
}
