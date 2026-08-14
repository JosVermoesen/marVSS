using System;
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
        // VB6: Dim Tegenrekening As String * 7
        private string _tegenrekening = string.Empty;
        private string _defaultKlanten = string.Empty;
        private string _defaultLeveranciers = string.Empty;

        public DetailInfo()
        {
            InitializeComponent();
            InitializeControlArrays();
            WireHighlightEvents(this);
        }

        private void InitializeControlArrays()
        {
            TekstInfo = new[] { _tekst0, _tekst1, _tekst2, _tekst3, _tekst4, _tekst5 };
            LabelInfo = new[] { _label0, _label1, _label2, _label3, _label4, _label5, _label6 };
            tbBank    = new[] { _tbBank0, _tbBank1 };
        }

        // ── Form_Load ────────────────────────────────────────────────────────
        private void DetailInfo_Load(object sender, EventArgs e)
        {
            _defaultKlanten      = (String99(9)  ?? string.Empty).TrimEnd();
            _defaultLeveranciers = (String99(10) ?? string.Empty).TrimEnd();

            TekstInfo[0].Text = _defaultKlanten;
        }

        // ── Annuleren (Sluiten) ───────────────────────────────────────────────
        private void Annuleren_Click(object sender, EventArgs e)
        {
            GridText = string.Empty;
            Hide();

            if (Application.OpenForms["FormInbrengFinancieel"] is Form inbreng)
                inbreng.Focus();
        }

        // ── Balanscontrole ────────────────────────────────────────────────────
        private void Balans_Click(object sender, EventArgs e)
        {
            BtnBalans.Text = "Bala&nscontrole"; // keep Default visual
            SharedFl = Partij.Checked ? TABLE_CUSTOMERS : TABLE_SUPPLIERS;

            GridText = string.Empty;
            aIndex   = 1;

            using (var sql = new FormSearchSQL())
                sql.ShowDialog(this);

            if (Ktrl == 0)
            {
                RecordToVeld(SharedFl);
                BalansKontroleWithRecordSet(SharedFl);
            }
        }

        // ── Bewerking ─────────────────────────────────────────────────────────
        private void Bewerking_CheckedChanged(object sender, EventArgs e)
        {
            Bewerking.Text = Bewerking.Checked ? "= Ontvangst" : "= Uitgave";
            Partij.Checked = Bewerking.Checked;
            Partij_CheckedChanged(sender, e);
            Dokument.Focus();
        }

        // ── cmdBank ───────────────────────────────────────────────────────────
        private void CmdBank_Click(object sender, EventArgs e)
        {
            var btn   = (Button)sender;
            int index = (int)btn.Tag;
            if (!string.IsNullOrEmpty(tbBank[index].Text))
                MessageBox.Show("test voor sepa webservice", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Dokument ──────────────────────────────────────────────────────────
        private void Dokument_CheckedChanged(object sender, EventArgs e)
        {
            if (Dokument.Checked)
            {
                Dokument.Text             = "= dokument";
                BtnBalans.Enabled         = true;
                TekstInfo[1].Visible      = true;
                LabelInfo[1].Visible      = true;
                TekstInfo[5].Visible      = true;
                Partij.Visible            = true;
                TekstInfo[0].Text         = Partij.Checked ? _defaultKlanten : _defaultLeveranciers;
            }
            else
            {
                Dokument.Text             = "= Geen DOK";
                Partij.Visible            = false;
                TekstInfo[1].Visible      = false;
                LabelInfo[1].Visible      = false;
                TekstInfo[5].Visible      = false;
                BtnBalans.Enabled         = false;
                TekstInfo[0].Text         = string.Empty;
                TekstInfo[1].Text         = string.Empty;
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

        // ── Partij ────────────────────────────────────────────────────────────
        private void Partij_CheckedChanged(object sender, EventArgs e)
        {
            if (Partij.Checked)
            {
                Partij.Text       = "= Klant";
                TekstInfo[0].Text = _defaultKlanten;
            }
            else
            {
                Partij.Text       = "= Leverancier";
                TekstInfo[0].Text = _defaultLeveranciers;
            }
        }

        // ── TekstInfo GotFocus ────────────────────────────────────────────────
        private void TekstInfo_GotFocus(object sender, EventArgs e)
        {
            var tb    = (TextBox)sender;
            int index = (int)tb.Tag;

            BtnBalans.Enabled = false;

            tb.SelectAll();

            switch (index)
            {
                case 0:
                    SnelHelpPrint("[Ctrl] voor geïndexeerd zoeken", BL_LOGGING);
                    Bewerking.Enabled = false;
                    Dokument.Enabled  = false;
                    Partij.Enabled    = false;
                    break;
                case 5:
                    BtnZoekDokument.Enabled = true;
                    BtnZoekDokument.Focus();
                    break;
            }
        }

        // ── TekstInfo KeyDown ─────────────────────────────────────────────────
        private void TekstInfo_KeyDown(object sender, KeyEventArgs e)
        {
            var tb    = (TextBox)sender;
            int index = (int)tb.Tag;

            switch (index)
            {
                case 0:
                    if (e.KeyCode == Keys.ControlKey)
                    {
                        SharedFl  = TABLE_LEDGERACCOUNTS;
                        aIndex    = 0;
                        GridText  = TekstInfo[0].Text;
                        using (var sql = new FormSearchSQL())
                            sql.ShowDialog(this);
                        TekstInfo[0].Text = Ktrl == 0 ? FVT[TABLE_LEDGERACCOUNTS, 0] : string.Empty;
                    }
                    else if (e.KeyCode == Keys.Return)
                    {
                        e.SuppressKeyPress = true;
                        TekstInfo[2].Focus();
                    }
                    break;
                case 2:
                    if (e.KeyCode == Keys.Return)
                    {
                        e.SuppressKeyPress = true;
                        TekstInfo[3].Focus();
                    }
                    break;
                case 3:
                    if (e.KeyCode == Keys.Return)
                    {
                        e.SuppressKeyPress = true;
                        BtnOk.Focus();
                    }
                    break;
            }
        }

        // ── TekstInfo KeyPress ────────────────────────────────────────────────
        private void TekstInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            var tb    = (TextBox)sender;
            int index = (int)tb.Tag;

            if (index != 5) return;

            const string allowed = "*.0123456789\b";
            if (allowed.IndexOf(e.KeyChar) >= 0)
            {
                BtnOk.Enabled = false;
            }
            else
            {
                e.Handled = true;
                var msg = "Syntax : [*] . xxxxx . [eejj]" + Environment.NewLine + Environment.NewLine
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
                MessageBox.Show(msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        // ── TekstInfo LostFocus ───────────────────────────────────────────────
        private void TekstInfo_LostFocus(object sender, EventArgs e)
        {
            var tb    = (TextBox)sender;
            int index = (int)tb.Tag;

            switch (index)
            {
                case 0:
                    if (!string.IsNullOrEmpty(TekstInfo[0].Text))
                    {
                        BGet(TABLE_LEDGERACCOUNTS, 0, VSet(TekstInfo[0].Text, 7));
                        if (Ktrl != 0)
                        {
                            System.Media.SystemSounds.Beep.Play();
                            TekstInfo[0].Text  = string.Empty;
                            LabelInfo[6].Text  = string.Empty;
                        }
                        else
                        {
                            RecordToVeld(TABLE_LEDGERACCOUNTS);
                            LabelInfo[6].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
                        }
                    }
                    break;

                case 1:
                    if (double.TryParse(TekstInfo[1].Text, out double korting) && korting != 0d)
                    {
                        string msg = "Financiele korting van bedrag aftrekken.";
                        int antwoord = (int)MessageBox.Show(msg, string.Empty,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (antwoord == (int)DialogResult.Yes)
                        {
                            double bedrag = 0d;
                            double.TryParse(TekstInfo[2].Text, out bedrag);
                            TekstInfo[2].Text         = (bedrag - korting).ToString();
                            TekstInfo[1].Enabled      = false;
                            TekstInfo[2].Enabled      = false;
                        }
                        else
                        {
                            TekstInfo[1].Text = string.Empty;
                        }
                    }
                    break;

                case 5:
                    BtnZoekDokument.Enabled = false;
                    break;
            }
        }

        // ── Ok ────────────────────────────────────────────────────────────────
        private void Ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TekstInfo[0].Text))
            {
                MessageBox.Show("Tegenrekening aanduiden a.u.b. !");
                TekstInfo[0].Focus();
                return;
            }

            if (Dokument.Checked)
            {
                BGet(TABLE_INVOICES, 0, VSet(TekstInfo[5].Text, 11));
                if (Ktrl != 0)
                {
                    MessageBox.Show("dokumentnummer onbekend !!!");
                    return;
                }
            }

            double.TryParse(TekstInfo[1].Text, out double t1);
            double.TryParse(TekstInfo[2].Text, out double t2);
            if (t1 + t2 == 0d)
            {
                MessageBox.Show("Bedrag - of + inbrengen a.u.b. !!!");
                return;
            }

            string kVlag  = Bewerking.Checked ? "+" : "-";
            string kolom1 = Dokument.Checked
                ? VSet(kVlag + TekstInfo[5].Text, 12)
                : VSet(kVlag, 12);
            string kolom2 = VSet(TekstInfo[0].Text, 7);
            string kolom3 = VSet(Dec(t2, MASK_EURBH), 12);
            string kolom4 = VSet(TekstInfo[3].Text, 29);
            string kolom5 = VSet(Dec(t1, MASK_EURBH), 12);

            GridText = kolom1 + "|" + kolom2 + "|" + kolom3 + "|" + kolom4 + "|" + kolom5;

            Hide();

            if (Application.OpenForms["FormInbrengFinancieel"] is Form inbreng)
                inbreng.Focus();
        }

        // ── ZoekDokument ──────────────────────────────────────────────────────
        private void ZoekDokument_Click(object sender, EventArgs e)
        {
            string kontroleTekst = TekstInfo[5].Text.Trim();
            if (string.IsNullOrEmpty(kontroleTekst)) return;

            string dokType;
            if (kontroleTekst.StartsWith("*"))
            {
                dokType       = "Q0";
                kontroleTekst = kontroleTekst.Substring(1);
                if (kontroleTekst.StartsWith("."))
                    kontroleTekst = kontroleTekst.Substring(1);
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
                double.TryParse(kontroleTekst.Substring(0, dotPos), out double numVal);
                nummer = numVal.ToString("00000");
                jaar   = kontroleTekst.Length >= dotPos + 5
                    ? kontroleTekst.Substring(dotPos + 1, 4)
                    : kontroleTekst.Substring(dotPos + 1);
            }
            else
            {
                double.TryParse(kontroleTekst, out double numVal);
                nummer = numVal.ToString("00000");
                jaar   = MIM_GLOBAL_DATE.Length >= 4
                    ? MIM_GLOBAL_DATE.Substring(MIM_GLOBAL_DATE.Length - 4)
                    : MIM_GLOBAL_DATE;
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
                TekstInfo[1].Text = string.Empty;
                TekstInfo[2].Text = string.Empty;
                TekstInfo[3].Text = string.Empty;
                TekstInfo[5].Text = string.Empty;
                SnelHelpPrint(dokType + jaar + nummer + " niet gevonden...", BL_LOGGING);
                return;
            }

            RecordToVeld(TABLE_INVOICES);
            TekstInfo[1].Text = string.Empty;

            double dBetaald = 0d;
            double.TryParse(VBibText(TABLE_INVOICES, "#v037 #"), out dBetaald);
            if (XisEuroWisBEF)
            {
                dBetaald = Math.Round(dBetaald * EURO);
                MessageBox.Show("CTRLstop");
            }

            double dTotaal = 0d;
            double.TryParse(VBibText(TABLE_INVOICES, "#v249 #"), out dTotaal);
            if (XisEuroWisBEF)
            {
                MessageBox.Show("CTRLstop");
                dTotaal = Math.Round(dTotaal * EURO);
            }

            if (dTotaal - dBetaald != 0d)
            {
                TekstInfo[2].Text = (dTotaal - dBetaald).ToString();
            }
            else
            {
                string msg = "dokument reeds volledig betaald." + Environment.NewLine
                    + "Bedragen die nu bijgeteld worden," + Environment.NewLine
                    + "einde van het boekjaar rechtzetten !!!";
                MessageBox.Show(msg, "Dubbele betaling...", MessageBoxButtons.OK, MessageBoxIcon.None);
            }

            SharedFl = VBibText(TABLE_INVOICES, "#v033 #").StartsWith("A")
                ? TABLE_SUPPLIERS
                : TABLE_CUSTOMERS;

            string partijKey = VBibText(TABLE_INVOICES, "#v034 #");
            if (partijKey.Length > 1)
                partijKey = partijKey.Substring(1);

            BGet(SharedFl, 0, partijKey);
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
            }
            else
            {
                RecordToVeld(SharedFl);
                TekstInfo[3].Text = VBibText(SharedFl, "#A100 #");
                tbBank[0].Text    = VBibText(SharedFl, "#A170 #");
                tbBank[1].Text    = VBibText(SharedFl, "#v251 #");
            }

            TekstInfo[5].Text    = FVT[TABLE_INVOICES, 0];
            Bewerking.Enabled    = false;
            Dokument.Enabled     = false;
            Partij.Enabled       = false;
            BtnOk.Enabled        = true;
            BtnOk.Focus();
        }
    }
}
