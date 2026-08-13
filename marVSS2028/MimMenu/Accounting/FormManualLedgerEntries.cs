using ADODB;
using System.Data.OleDb;

using marVSS2028.Classes;
using marVSS2028.PublicForms;
using marVSS2028.SharedForms;
using System;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormManualLedgerEntries : Form
    {
        public Recordset BalanceRS = new Recordset();
        public FormManualLedgerEntries()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ── Form events ────────────────────────────────────────────────────────

        private void FormDiversePosten_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;

            cmbSoortBoeking.Items.Add("0: Diverse post");
            cmbSoortBoeking.Items.Add("1: Afschrijvingspost Eindejaar");
            cmbSoortBoeking.Items.Add("2: Beginbalans");
            cmbSoortBoeking.SelectedIndex = 0;

            dtpDatum.Value = DateTime.TryParseExact(MIM_GLOBAL_DATE, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime d) ? d : DateTime.Today;

            Schoon_Click(null, EventArgs.Empty);

            if (XLogKey == "SchrijfAF!")
                cmbSoortBoeking.SelectedIndex = 1;
        }

        // ── SoortBoeking ───────────────────────────────────────────────────────

        private void CmbSoortBoeking_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                txtOmschrijving.Focus();
        }

        // ── Omschrijving ───────────────────────────────────────────────────────

        private void TxtOmschrijving_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TxtOmschrijving_Leave(object sender, EventArgs e)
        {
            if (txtOmschrijving.Text.Trim() == string.Empty)
                MessageBox.Show("Omschrijving mag niet leeg zijn, ook géén spaties...", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Datum ──────────────────────────────────────────────────────────────

        private void DtpDatum_Leave(object sender, EventArgs e)
        {
            string datStr = dtpDatum.Value.ToString("dd/MM/yyyy");
            if (!DateCheck(datStr, PERIODAS_TEXT))
            {
                OpenBYPERDAT(this);
            }
        }
        // ── DCkeuze / TRvlag

        private void OptDCkeuze_KeyPress(object sender, KeyPressEventArgs e)
        {
            char toets = char.ToUpper(e.KeyChar);
            switch (toets)
            {
                case 'D':
                case '+':
                    optDCkeuze0.Checked = true;
                    txtRekeningNummer.Focus();
                    break;
                case 'C':
                case '-':
                    optDCkeuze1.Checked = true;
                    txtRekeningNummer.Focus();
                    break;
                case 'T':
                case '/':
                    chkTRvlag.Checked = !chkTRvlag.Checked;
                    TRaanUit();
                    txtRekeningNummer.Focus();
                    break;
            }
        }

        private void ChkTRvlag_Click(object sender, EventArgs e)
        {
            TRaanUit();
        }

        private void ChkTRvlag_KeyPress(object sender, KeyPressEventArgs e)
        {
            OptDCkeuze_KeyPress(sender, e);
        }

        // ── RekeningNummer ─────────────────────────────────────────────────────

        private void TxtRekeningNummer_GotFocus(object sender, EventArgs e)
        {
            btnAfsluiten.Enabled = false; // reset default
            SnelHelpPrint("Dubbelklikken of [Ctrl] voor geïndexeerd zoeken", BL_LOGGING);
        }

        private void TxtRekeningNummer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, true);
                return;
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                OpenRekeningZoek();
                e.Handled = true;
            }
        }

        private void TxtRekeningNummer_DoubleClick(object sender, EventArgs e)
        {
            OpenRekeningZoek();
        }

        private void OpenRekeningZoek()
        {
            SharedFl = TABLE_LEDGERACCOUNTS;
            aIndex = 0;
            GridText = txtRekeningNummer.Text;
            using (var sql = new FormSearchSQL())
                sql.ShowDialog(this);
            if (Ktrl != 0)
            {
                lblNaamRekening.Text = string.Empty;
            }
            else
            {
                txtRekeningNummer.Text = RV(rsMAR[TABLE_LEDGERACCOUNTS], "v019")?.ToString() ?? string.Empty;
                lblNaamRekening.Text = RV(rsMAR[TABLE_LEDGERACCOUNTS], "v020")?.ToString() ?? string.Empty;
            }
        }

        private void TxtRekeningNummer_Leave(object sender, EventArgs e)
        {
            if (txtRekeningNummer.Text.Trim() == string.Empty) return;
            if (!ADO_GET(TABLE_LEDGERACCOUNTS, 0, "=", txtRekeningNummer.Text))
            {
                txtRekeningNummer.Text = string.Empty;
                lblNaamRekening.Text = string.Empty;
            }
            else
            {
                lblNaamRekening.Text = RV(rsMAR[TABLE_LEDGERACCOUNTS], "v020")?.ToString() ?? string.Empty;
            }
        }

        // ── Tegenrekening

        private void TxtTegenrekening_GotFocus(object sender, EventArgs e)
        {
            SnelHelpPrint("Dubbelklikken of [Ctrl] voor geïndexeerd zoeken", BL_LOGGING);
        }

        private void TxtTegenrekening_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SelectNextControl((Control)sender, true, true, true, true);
                return;
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                OpenTegenRekeningZoek();
                e.Handled = true;
            }
        }

        private void TxtTegenrekening_DoubleClick(object sender, EventArgs e)
        {
            OpenTegenRekeningZoek();
        }

        private void OpenTegenRekeningZoek()
        {
            SharedFl = TABLE_LEDGERACCOUNTS;
            aIndex = 0;
            GridText = txtTegenrekening.Text;
            using (var sql = new FormSearchSQL())
                sql.ShowDialog(this);
            if (Ktrl != 0)
                lblNaamTegenRekening.Text = string.Empty;
            else
            {
                txtTegenrekening.Text = RV(rsMAR[TABLE_LEDGERACCOUNTS], "v019")?.ToString() ?? string.Empty;
                lblNaamTegenRekening.Text = RV(rsMAR[TABLE_LEDGERACCOUNTS], "v020")?.ToString() ?? string.Empty;
            }
        }

        private void TxtTegenrekening_Leave(object sender, EventArgs e)
        {
            if (txtTegenrekening.Text.Trim() == string.Empty) return;
            if (!ADO_GET(TABLE_LEDGERACCOUNTS, 0, "=", txtTegenrekening.Text))
            {
                txtTegenrekening.Text = string.Empty;
                lblNaamTegenRekening.Text = string.Empty;
            }
            else
            {
                lblNaamTegenRekening.Text = RV(rsMAR[TABLE_LEDGERACCOUNTS], "v020")?.ToString() ?? string.Empty;
            }
        }

        // ── Bedrag

        private void TxtBedrag_TextChanged(object sender, EventArgs e)
        {
            bool hasValue = txtBedrag.Text != string.Empty;
            btnVolgendeLijn.Enabled = hasValue;
            this.AcceptButton = hasValue ? btnVolgendeLijn : null;
        }

        private void TxtBedrag_GotFocus(object sender, EventArgs e)
        {
            // btnAfsluiten.Enabled = double.TryParse(lblSaldo.Text,
            //    System.Globalization.NumberStyles.Any,
            //    System.Globalization.CultureInfo.CurrentCulture, out double saldo) && saldo == 0;
        }

        private void TxtBedrag_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!bhEuro && e.KeyChar == '.')
            {
                System.Media.SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        private void TxtBedrag_Leave(object sender, EventArgs e)
        {
            btnVolgendeLijn.Enabled = false;
        }

        // ── VolgendeLijn ───────────────────────────────────────────────────────

        private void BtnVolgendeLijn_Click(object sender, EventArgs e)
        {
            if (txtRekeningNummer.Text.Trim() == string.Empty)
            {
                System.Media.SystemSounds.Beep.Play();
                txtRekeningNummer.Focus();
                return;
            }
            if (!double.TryParse(txtBedrag.Text, out double lijnBedrag) || lijnBedrag == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                txtBedrag.Focus();
                return;
            }
            if (chkTRvlag.Checked && txtTegenrekening.Text.Trim() == string.Empty)
            {
                System.Media.SystemSounds.Beep.Play();
                txtTegenrekening.Focus();
                return;
            }

            if (!optDCkeuze0.Checked)
                lijnBedrag = -lijnBedrag;

            if (double.TryParse(lblSaldo.Text, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out double currentSaldo))
            {

                currentSaldo += (double)lijnBedrag;
                if (chkTRvlag.Checked)
                    currentSaldo -= (double)lijnBedrag;

                lblSaldo.Text = currentSaldo.ToString("#,##0.00");
            }

            string lijnText = VSet(txtRekeningNummer.Text, 7) + " " +
                              VSet(lblNaamRekening.Text, 40) + " " +
                              Dec(lijnBedrag, MASK_EURBH) + " ";
            lijnText += chkTRvlag.Checked
                ? VSet(txtTegenrekening.Text, 7)
                : new string(' ', 7);
            lstJournaalPost.Items.Add(lijnText);
            // JournaalSaldoKTRL();

            if (double.TryParse(lblSaldo.Text, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out double newSaldo) && newSaldo == 0)
            {
                btnAfsluiten.Enabled = true;
            }
            else
            {
                btnAfsluiten.Enabled = false;
            }

            OpKuisVolgendeLijn();
            optDCkeuze0.Focus();
        }

        // ── Afsluiten (Boeken) ─────────────────────────────────────────────────

        private void BtnAfsluiten_Click(object sender, EventArgs e)
        {
            if (txtOmschrijving.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Omschrijving mag niet leeg zijn.", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOmschrijving.Focus();
                return;
            }

            string datStr = dtpDatum.Value.ToString("dd/MM/yyyy");
            if (!TextTools.DateCheck(datStr, PERIODAS_TEXT))
            {
                System.Media.SystemSounds.Beep.Play();
                dtpDatum.Focus();
                return;
            }


            if (lstJournaalPost.Items.Count == 0)
            {
                return;
            }

            Ktrl = (int)MessageBox.Show(
                "Journaalpost bestaande uit " + lstJournaalPost.Items.Count + " Lijnen wegboeken.  Bent U zeker ?",
                string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (Ktrl == (int)DialogResult.Yes)
            {                
                Cursor = Cursors.WaitCursor;
                if (OleDBBoekFout())
                {   
                    Focus();
                    return;
                }
                else
                {
                    string eerste = cmbSoortBoeking.Text.Length > 0 ? cmbSoortBoeking.Text.Substring(0, 1) : "0";
                    switch (eerste)
                    {
                        case "1":
                            // EindeAfschrijving();
                            break;
                        case "2":
                            SS99("1", 64); // Flag initial bookyear data generated
                            cmbSoortBoeking.SelectedIndex = 0;
                            cmbSoortBoeking.Focus();
                            break;
                        case "0": // Normal boekingstype, no specific post-processing needed
                            break;                        
                    }                 
                    Schoon_Click(null, EventArgs.Empty);
                }
                Cursor = Cursors.Default;
            }
        }

        // ── Sluiten ────────────────────────────────────────────────────────────

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (lstJournaalPost.Items.Count > 0)
            {
                string msg = lstJournaalPost.Items.Count + " Journaallijnen negeren.  Bent U zeker ?";
                Ktrl = (int)MessageBox.Show(msg, "Inbreng Journaalpost",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Ktrl != (int)DialogResult.Yes)
                    return;
            }
            Close();
        }

        // ── Schoon ─────────────────────────────────────────────────────────────

        private void Schoon_Click(object sender, EventArgs e)
        {
            OpKuisVolgendeLijn();
            btnAfsluiten.Enabled = false;
            txtOmschrijving.Text = string.Empty;
            lblSaldo.Text = "0";
            lstJournaalPost.Items.Clear();
            cmbSoortBoeking.Enabled = true;            
            btnSluiten.Enabled = true;

            optDCkeuze0.Enabled = true;
            optDCkeuze1.Enabled = true;
            txtBedrag.Visible = true;
            txtRekeningNummer.Visible = true;
            chkTRvlag.Enabled = true;
            cmbSoortBoeking.Focus();
        }

        // ── JournaalPost listbox ────────────────────────────────────────────────

        private void LstJournaalPost_KeyPress(object sender, KeyPressEventArgs e)
        {
            int pos = lstJournaalPost.SelectedIndex;
            if (e.KeyChar == (char)45 || e.KeyChar == (char)127) // '-' or DEL
            {
                if (pos < 0) return;
                Ktrl = (int)MessageBox.Show(
                    (pos + 1) + "e Journaallijn verwijderen.  Bent U zeker ?",
                    "Journaallijn verwijderen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Ktrl == (int)DialogResult.Yes)
                {
                    lstJournaalPost.Items.RemoveAt(pos);
                    JournaalSaldoKTRL();
                }
            }
        }

        // ── Business logic ─────────────────────────────────────────────────────

        //private static void BoekUserDef() { }

        //private bool AfschrijfBoeking()
        //{
        //    bool succes = true;

        //    if (String99(READING, 63) == "1")
        //    {
        //        MessageBox.Show("Afschrijvingsposten reeds gegenereerd voor dit boekjaar.  Bijkomende posten kunnen uitsluitend via 'Diverse post'-optie ingebracht worden !");
        //        return false;
        //    }
        //    else if (String99(READING, 63) != "0")
        //    {
        //        MessageBox.Show("Setup boekjaar en parameters bevat niet de juiste vlag geboekt of niet geboekt.  Kontroleer");
        //        return false;
        //    }
        //    else if (String99(READING, 64) != "1")
        //    {
        //        MessageBox.Show("Onlogische situatie.  Dit boekjaar bevat nog geen beginbalans ?  De beginbalans dient aanwezig te zijn.  Mogelijk bevindt U zich in het verkeerde boekjaar ?");
        //        return false;
        //    }

        //    BGetOrGreater(TABLE_VARIOUS, 1, VSet("18", 20));
        //    if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("18"))
        //    {
        //        MessageBox.Show("Er zijn geen investeringsfiches !");
        //        return false;
        //    }

        //    AfschrijvingsLijnErBij(ref succes);
        //    while (true)
        //    {
        //        BNext(TABLE_VARIOUS);
        //        if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("18"))
        //            break;
        //        AfschrijvingsLijnErBij(ref succes);
        //    }

        //    if (succes)
        //    {
        //        btnAfsluiten.Enabled = true;
        //        txtOmschrijving.Text = cmbSoortBoeking.Text;
        //        cmbSoortBoeking.Enabled = false;
        //        btnAfsluiten.Focus();
        //    }
        //    optDCkeuze0.Enabled = false;
        //    optDCkeuze1.Enabled = false;
        //    txtBedrag.Visible = false;
        //    txtRekeningNummer.Visible = false;
        //    chkTRvlag.Enabled = false;
        //    return true;
        //}

        //private void AfschrijvingsLijnErBij(ref bool succes)
        //{
        //    string omschrijvingsLijn = string.Empty;
        //    RecordToVeld(TABLE_VARIOUS);

        //    if (VBibText(TABLE_VARIOUS, "#v083 #").Length != 8)
        //    {
        //        omschrijvingsLijn = "Datumformaat onjuist voor " + VBibText(TABLE_VARIOUS, "#v087 #");
        //        MessageBox.Show("dokumentendatum niet in formaat DDMMJJEE\r\n\r\n" +
        //            omschrijvingsLijn + "\r\n\r\n" + VBibText(TABLE_VARIOUS, "#v083 #"));
        //        succes = false;
        //        return;
        //    }

        //    string afschrJaarStr = VBibText(TABLE_VARIOUS, "#v083 #").Length >= 8
        //        ? VBibText(TABLE_VARIOUS, "#v083 #").Substring(4, 4) : "";
        //    string boekJaarStr = BOOKYEAR_FROMTO.Length >= 12
        //        ? BOOKYEAR_FROMTO.Substring(8, 4) : "";
        //    if (string.Compare(afschrJaarStr, boekJaarStr, StringComparison.Ordinal) > 0)
        //        return;

        //    if (double.Parse(VBibText(TABLE_VARIOUS, "#v084 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v084 #")) ==
        //        double.Parse(VBibText(TABLE_VARIOUS, "#v085 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v085 #")))
        //    {
        //        string msgHere = "Alles is afgeschreven voor " + VBibText(TABLE_VARIOUS, "#v087 #") + "\r\n\r\nTotaal : " +
        //            Dec(double.Parse(VBibText(TABLE_VARIOUS, "#v084 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v084 #")), MASK_SY[0]) + "\r\n\r\n";
        //        if (!ADO_GET(TABLE_LEDGERACCOUNTS, 0, "=", VBibText(TABLE_VARIOUS, "#v087 #")))
        //            msgHere += "Rekening bestaat zelfs niet eens...";
        //        else
        //            msgHere += RV(rsMAR[TABLE_LEDGERACCOUNTS], "v020");
        //        MessageBox.Show(msgHere);
        //        return;
        //    }

        //    int ipct = int.Parse(VBibText(TABLE_VARIOUS, "#v082 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v082 #"));
        //    double dbdrg = double.Parse(VBibText(TABLE_VARIOUS, "#v084 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v084 #"));
        //    double dRa = double.Parse(VBibText(TABLE_VARIOUS, "#v085 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v085 #"));

        //    double restKtrl = bhEuro ? 0.5 : 20;
        //    string maskerHier = bhEuro ? MASK_EURBH : MASK_SY[0];

        //    double dasVal = double.Parse(Dec(dbdrg / ipct, maskerHier));
        //    double dRest = dbdrg - (dRa + dasVal);
        //    double das = dRest < restKtrl ? dasVal + dRest : dasVal;

        //    if (!ADO_GET(TABLE_LEDGERACCOUNTS, 0, "=", VBibText(TABLE_VARIOUS, "#v087 #")))
        //        omschrijvingsLijn = "Afschr. op!!!";
        //    if (!ADO_GET(TABLE_LEDGERACCOUNTS, 0, "=", VBibText(TABLE_VARIOUS, "#v088 #")))
        //        omschrijvingsLijn += " Kostrekening !!";
        //    else if (omschrijvingsLijn == string.Empty)
        //        omschrijvingsLijn = "Ok, " + RV(rsMAR[TABLE_LEDGERACCOUNTS], "v020");

        //    if (!omschrijvingsLijn.StartsWith("Ok,"))
        //        succes = false;

        //    string lijnText = VSet(VBibText(TABLE_VARIOUS, "#v088 #"), 7) + " " +
        //                      omschrijvingsLijn + " " +
        //                      Dec(das, MASK_EURBH) + " " +
        //                      VSet(VBibText(TABLE_VARIOUS, "#v087 #"), 7);
        //    lstJournaalPost.Items.Add(lijnText);
        //}

        //private void EindeAfschrijving()
        //{
        //    if (XisEuroWisBEF)
        //        MessageBox.Show("Laatste afschrijving in BEF is geboekt.\r\n\r\nUw hoogste boekjaar is in EUR\r\n\r\nHierna worden alle nog openstaande afschrijvingslijnen eveneens omgerekend naar EUR voor toekomstige bewerkingen",
        //            string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);

        //    for (int t = 0; t < lstJournaalPost.Items.Count; t++)
        //    {
        //        string lijn = lstJournaalPost.Items[t].ToString();
        //        BGetOrGreater(TABLE_VARIOUS, 1, VSet("18" + lijn.Substring(lijn.Length - 7), 20));
        //        if (Ktrl != 0)
        //        {
        //            MessageBox.Show("Stop");
        //            continue;
        //        }
        //        RecordToVeld(TABLE_VARIOUS);
        //        double dRa = double.Parse(VBibText(TABLE_VARIOUS, "#v085 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v085 #")) +
        //                     double.Parse(Mid(lijn, 50, 12).Trim() == string.Empty ? "0" : Mid(lijn, 50, 12).Trim());
        //        VBib(TABLE_VARIOUS, dRa.ToString(), "v085");
        //        if (XisEuroWisBEF)
        //        {
        //            VBib(TABLE_VARIOUS, double.Parse(Dec(dRa / EURO, "########0.00")).ToString(), "v085");
        //            double dTot = double.Parse(VBibText(TABLE_VARIOUS, "#v084 #") == string.Empty ? "0" : VBibText(TABLE_VARIOUS, "#v084 #"));
        //            VBib(TABLE_VARIOUS, double.Parse(Dec(dTot / EURO, "########0.00")).ToString(), "v084");
        //        }
        //        BUpdate(TABLE_VARIOUS, 1);
        //        if (Ktrl != 0) MessageBox.Show("stop tijdens update investeringsfiche");
        //    }
        //    SS99("1", 63);
        //}

        // ── Helpers ────────────────────────────────────────────────────────────

        private static string FormatAmount(double value)
            => value.ToString("#,##0.00");

        private void TRaanUit()
        {
            bool show = chkTRvlag.Checked;
            txtTegenrekening.Visible = show;
            lblNaamTegenRekening.Visible = show;
        }

        private void OpKuisVolgendeLijn()
        {
            chkTRvlag.Checked = false;
            TRaanUit();
            txtRekeningNummer.Text = string.Empty;
            lblNaamRekening.Text = string.Empty;
            txtBedrag.Text = string.Empty;
            txtTegenrekening.Text = string.Empty;
            lblNaamTegenRekening.Text = string.Empty;
            optDCkeuze0.Checked = true;
            btnVolgendeLijn.Enabled = false;
        }

        private void JournaalSaldoKTRL()
        {
            double saldoKTRL = 0;
            for (int i = 0; i < lstJournaalPost.Items.Count; i++)
                saldoKTRL += double.Parse(
                    Mid(lstJournaalPost.Items[i].ToString(), 50, 12).Trim(),
                    System.Globalization.NumberStyles.Any);
            lblSaldo.Text = saldoKTRL.ToString("#,##0.00");
        }

        private static string Mid(string s, int start, int length)
        {
            if (string.IsNullOrEmpty(s) || start < 1 || start > s.Length) return string.Empty;
            int idx = start - 1;
            return s.Substring(idx, Math.Min(length, s.Length - idx));
        }
                
        private void txtOmschrijving_Enter(object sender, EventArgs e)
        {
            if (cmbSoortBoeking.Enabled)
            {
                string bookType = cmbSoortBoeking.Text.Substring(0, 1) ?? "0";
                switch (bookType)
                {
                    case "0":
                        break;

                    case "1":
                        Msg = cmbSoortBoeking.Text + " activeren !\r\n\r\nBent U zeker ?";
                        Ktrl = (int)MessageBox.Show(Msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (Ktrl == (int)DialogResult.Yes)
                        {
                            // AfschrijfBoeking();
                            return;
                        }
                        cmbSoortBoeking.SelectedIndex = 0;
                        cmbSoortBoeking.Focus();

                        break;
                    case "2":

                        Msg = cmbSoortBoeking.Text + " activeren !\r\n\r\nBent U zeker ?";
                        Ktrl = (int)MessageBox.Show(Msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (Ktrl == (int)DialogResult.Yes)
                        {
                            // GenerateOpeningBalance();
                            OleDBGenerateOpeningBalance();
                        }
                        else
                        {
                            cmbSoortBoeking.SelectedIndex = 0;
                            cmbSoortBoeking.Focus();
                        }

                        break;
                    default:
                        MessageBox.Show("Onbekende boekingstype.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            cmbSoortBoeking.Enabled = false;
        }


        // ── OleDB specific methods ───────────────────────────────────────────────
        private bool OleDBGenerateOpeningBalance()
        {
            if (String99(64) == "1")
            {
                MessageBox.Show("Beginbalans reeds gegenereerd voor dit boekjaar.  Bijkomende posten kunnen uitsluitend via 'Diverse post'-optie ingebracht worden !");
                cmbSoortBoeking.SelectedIndex = 0;
                return false;
            }
            else if (String99(64) != "0")
            {
                MessageBox.Show("Setup beginbalans bevat niet de juiste vlag geboekt of niet geboekt.  Kontroleer");
                return false;
            }

            string fieldPrefix = bhEuro ? "e" : "v";
            string fieldNr = (ACTIVE_BOOKYEAR + 23).ToString("000");            
            string field = fieldPrefix + fieldNr;

            Cursor.Current = Cursors.WaitCursor;
            bool checkResult = OleDBGetInitialBookyearData(field);
            Cursor.Current = Cursors.Default;

            if (checkResult)
            {
                btnAfsluiten.Enabled = true;
                txtOmschrijving.Text = cmbSoortBoeking.Text;
                btnAfsluiten.Focus();
            }
            return checkResult;
        }                
        
        public bool OleDBGetInitialBookyearData(string yearSolde)
        {
            double totaalBalans = 0;
            double totaalResultaat = 0;

            string sSQL =
                "SELECT v019, v020, " + yearSolde + " " +
                "FROM Rekeningen " +
                "WHERE v019 >= '1' AND v019 <= '8' " +
                "ORDER BY v019 ASC";

            int recordCount = 0;

            using (var conn = new OleDbConnection(oleDbConnect))
            using (var cmd = new OleDbCommand(sSQL, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        recordCount++;

                        string v019Value = reader["v019"]?.ToString() ?? string.Empty;
                        double amount = 0;

                        object eValue = reader[yearSolde];
                        if (!Convert.IsDBNull(eValue))
                        {
                            string eStr = eValue.ToString().Trim();
                            if (!string.IsNullOrEmpty(eStr))
                            {
                                double.TryParse(eStr,
                                    System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    out amount);
                            }
                        }

                        amount = Math.Round(amount, 4, MidpointRounding.AwayFromZero);
                        if (Math.Abs(amount) < 0.01)
                            amount = 0;

                        if (amount != 0)
                        {
                            if (v019Value.CompareTo("5999999") > 0)
                            {
                                // Resultaatrekening
                                totaalResultaat += amount;
                            }
                            else
                            {
                                // Balansrekening
                                totaalBalans += amount;
                                lstJournaalPost.Items.Add(
                                    VSet(v019Value, 7) + " " +
                                    VSet(reader["v020"]?.ToString() ?? string.Empty, 40) + " " +
                                    Dec(amount, MASK_EURBH) + " " + VSet("", 7));
                            }
                        }
                    }
                }
            }

            if (recordCount == 0)
            {
                MessageBox.Show("Geen balansgegevens gevonden voor het aangeduide boekjaar.");
                return false;
            }

            if (double.Parse(Dec(totaalBalans, MASK_EURBH)) != 0 &&
                double.Parse(Dec(totaalResultaat, MASK_EURBH)) != 0)
            {
                string msgBal = "ACTIVE/PASSIVA verschil   : " + Dec(totaalBalans, MASK_EURBH) + "\r\n";
                msgBal += "Resultatenbalans verschil  : " + Dec(totaalResultaat, MASK_EURBH) + "\r\n";
                MessageBox.Show(msgBal +
                    "\r\nBalansrekeningen Actief <> Passief, Resultaatrekeningen Debet <> Credit" +
                    "\r\n\r\nControleer eerst nog saldo resultaatsverwerking vorig jaar");
            }

            return true;
        }

        private void OleDBAddJournaalEntry(
            string rekening, double bedrag, string tegenrekening,
            OleDbConnection conn, OleDbTransaction tran)
        {
            string datum = dtpDatum.Value.ToString("yyyyMMdd");
            string omschrijving = VSet(txtOmschrijving.Text, 35);

            // STEP 1: Insert the journal entry into the Journalen table
            string sqlLedgerCommand =
                "INSERT INTO Journalen " +
                "(v041, v066, v033, v035, v067, v019, v068, dece068, v069, v070) " +
                "VALUES " +
                "(@v041, @v066, @v033, @v035, @v067, @v019, @v068, @dece068, @v069, @v070)";

            using (var cmd = new OleDbCommand(sqlLedgerCommand, conn, tran))
            {
                cmd.Parameters.AddWithValue("@v041", "0");
                cmd.Parameters.AddWithValue("@v066", datum);
                cmd.Parameters.AddWithValue("@v033", "D0" + datum);
                cmd.Parameters.AddWithValue("@v035", datum);
                cmd.Parameters.AddWithValue("@v067", omschrijving);
                cmd.Parameters.AddWithValue("@v019", rekening);
                cmd.Parameters.AddWithValue("@v068", bedrag.ToString());
                cmd.Parameters.AddWithValue("@dece068", bedrag);
                cmd.Parameters.AddWithValue("@v069", tegenrekening);
                cmd.Parameters.AddWithValue("@v070", rekening + datum);
                cmd.ExecuteNonQuery();
            }

            // STEP 2: Update LedgerAccount solde
            string soldeField = ACTIVE_BOOKYEAR != 0 ? "e023" : "e022";
            string deceField  = ACTIVE_BOOKYEAR != 0 ? "dece023" : "dece022";
            string sqlUpdateLedgerAccount =
                $"UPDATE Rekeningen " +
                $"SET {soldeField} = {soldeField} + @bedragStr, " +
                $"{deceField} = {deceField} + @bedragDbl, " +
                "dnnsync = false " +
                "WHERE v019 = @rekening";

            using (var cmd = new OleDbCommand(sqlUpdateLedgerAccount, conn, tran))
            {
                cmd.Parameters.AddWithValue("@bedragStr", bedrag.ToString("F2"));
                cmd.Parameters.AddWithValue("@bedragDbl", bedrag);
                cmd.Parameters.AddWithValue("@rekening", rekening);
                cmd.ExecuteNonQuery();
            }
        }

        private bool OleDBBoekFout()
        {
            DKTRL_CUMUL = 0; DKTRL_BEF = 0; DKTRL_EUR = 0;

            using (var frmBoeking = new FormBoeking())
            using (var conn = new OleDbConnection(oleDbConnect))
            {
                conn.Open();
                OleDbTransaction tran = conn.BeginTransaction();

                try
                {
                    for (int t = 0; t < lstJournaalPost.Items.Count; t++)
                    {
                        string lijn = lstJournaalPost.Items[t].ToString();
                        string rekening = Mid(lijn, 1, 7);
                        string tegenrekening = Mid(lijn, 63, 7);
                        string bedragRaw = Mid(lijn, 50, 12).Trim();
                        double bedrag = double.Parse(bedragRaw == string.Empty ? "0" : bedragRaw);

                        OleDBAddJournaalEntry(rekening, bedrag, tegenrekening, conn, tran);
                        BookingAddLine(frmBoeking, bedrag, rekening, VSet(txtOmschrijving.Text, 35));

                        if (tegenrekening.Trim() != string.Empty)
                        {
                            OleDBAddJournaalEntry(tegenrekening, -bedrag, rekening, conn, tran);
                            BookingAddLine(frmBoeking, -bedrag, tegenrekening, VSet(txtOmschrijving.Text, 35));
                        }
                    }

                    Cursor = Cursors.Default;

                    DKTRL_CUMUL = double.Parse(Dec(DKTRL_CUMUL, MASK_EURBH));
                    if (DKTRL_CUMUL != 0)
                    {
                        tran.Rollback();
                        MessageBox.Show("Fout bij vierkantskontrole journaal. Controleer!\r\n\r\nDeze verrichting wordt zonder meer genegeerd.");
                        frmBoeking.ShowDialog(this);
                        return true;
                    }

                    string modusFirst = Mim?.GetWegBoekModus() ?? "0";
                    switch (modusFirst)
                    {
                        case "0":
                            // No additional checks — commit
                            break;
                        case "1":
                            // Warn only when EUR <> BEF differences, then commit
                            if (DKTRL_BEF != 0 || DKTRL_EUR != 0)
                                frmBoeking.ShowDialog(this);
                            break;
                        case "2":
                            frmBoeking.ShowDialog(this);
                            if (Globals.DKTRL_CUMUL == 99)
                            {
                                tran.Rollback();
                                return true;
                            }
                            break;
                    }

                    tran.Commit();
                    Schoon_Click(null, EventArgs.Empty);
                    return false;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }        
    }
}

