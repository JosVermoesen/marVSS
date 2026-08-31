using System;
using System.Globalization;
using System.Media;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.SharedForms
{
    public partial class FormPurchaseInvestmentSheet : Form
    {
        private readonly TextBox[] _tekstInfo;
        private int _nietAanwezig;

        public FormPurchaseInvestmentSheet()
        {
            InitializeComponent();

            _tekstInfo = new[]
            {
                TekstInfo0,
                TekstInfo1,
                TekstInfo2,
                TekstInfo3,
                TekstInfo4,
                TekstInfo5,
                TekstInfo6,
                TekstInfo7,
                TekstInfo8
            };

            BindTekstInfoEvents();
        }

        private void BindTekstInfoEvents()
        {
            for (int i = 0; i < _tekstInfo.Length; i++)
            {
                _tekstInfo[i].Tag = i;
                _tekstInfo[i].Enter += TekstInfo_Enter;
                _tekstInfo[i].Leave += TekstInfo_Leave;
            }
        }

        private void FormPurchaseInvestmentSheet_Load(object sender, EventArgs e)
        {
            TekstInfo3.Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #").TrimEnd();

            string rekeningTest2 = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #").TrimEnd();
            TekstInfo0.Text = PartMid(GridText ?? string.Empty, 1, 10);
            TekstInfo1.Text = Dec(ParseVbVal(PartMid(GridText ?? string.Empty, 11, 12)), MASK_EURBH);
            TLB_RECORD[TABLE_VARIOUS] = string.Empty;

            bool foundZero = false;
            char[] rekeningChars = rekeningTest2.ToCharArray();
            for (int i = rekeningChars.Length - 1; i >= 0; i--)
            {
                if (rekeningChars[i] == '0')
                {
                    rekeningChars[i] = '9';
                    foundZero = true;
                    break;
                }
            }

            if (!foundZero)
            {
                MessageBox.Show("Onlogika in investeringsrekening !", "Investeringsfiche", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Ok.Enabled = false;
                return;
            }

            string rekeningTest = VSet(new string(rekeningChars), 7);
            TekstInfo4.Text = rekeningTest;

            BGet(TABLE_LEDGERACCOUNTS, 0, rekeningTest);
            if (Ktrl != 0)
            {
                string rekeningNaam = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #").TrimEnd();
                StringBuilder msgBuilder = new StringBuilder();
                msgBuilder.Append("Afschrijving op ").Append(rekeningNaam).Append("\r\n");
                msgBuilder.Append("Rekeningnr. : ").Append(rekeningTest).Append(" bestaat nog niet.\r\n\r\n");
                msgBuilder.Append("Wordt hierna automatisch aangemaakt...");
                MessageBox.Show(msgBuilder.ToString(), "Aanmaak afschrijfrekening", MessageBoxButtons.OK, MessageBoxIcon.Information);

                TLB_RECORD[TABLE_LEDGERACCOUNTS] = string.Empty;
                VBib(TABLE_LEDGERACCOUNTS, rekeningTest, "v019");
                VBib(TABLE_LEDGERACCOUNTS, "Afschrijving op " + rekeningNaam, "v020");
                VBib(TABLE_LEDGERACCOUNTS, "O", "v032");
                BInsert(TABLE_LEDGERACCOUNTS, 0);
            }

            BGet(TABLE_VARIOUS, 1, VSet("18" + rekeningTest, 20));
            _nietAanwezig = Ktrl;

            if (Ktrl != 0)
            {
                TekstInfo6.Text = string.Empty;
                TekstInfo7.Text = Dec(0, MASK_EURBH);
                TekstInfo8.Text = Dec(0, MASK_EURBH);
                TekstInfo2.Text = Dec(5, "###");
                TekstInfo5.Text = "6300000";
                Versneld.Checked = true;
            }
            else
            {
                RecordToVeld(TABLE_VARIOUS);

                TekstInfo2.Text = Dec(ParseVbVal(VBibText(TABLE_VARIOUS, "#v082 #")), "###");
                TekstInfo6.Text = VBibText(TABLE_VARIOUS, "#v083 #").TrimEnd();
                TekstInfo7.Text = Dec(ParseVbVal(VBibText(TABLE_VARIOUS, "#v084 #")), MASK_EURBH);
                TekstInfo8.Text = Dec(ParseVbVal(VBibText(TABLE_VARIOUS, "#v085 #")), MASK_EURBH);
                Versneld.Checked = ParseVbVal(VBibText(TABLE_VARIOUS, "#v086 #")) != 0;
                TekstInfo4.Text = VBibText(TABLE_VARIOUS, "#v087 #").TrimEnd();
                TekstInfo5.Text = VBibText(TABLE_VARIOUS, "#v088 #").TrimEnd();

                double bedrag1 = ParseVbVal(VBibText(TABLE_VARIOUS, "#v084 #"));
                double bedrag2 = ParseVbVal(PartMid(GridText ?? string.Empty, 11, 12));
                
                if (string.Equals(TekstInfo6.Text, TekstInfo0.Text, StringComparison.Ordinal) && bedrag1 == bedrag2)
                {
                    string msg = "Opgelet, laatste bijwerking dezelfde dag én zelfde bedrag\r\n" +
                                 "reeds aanwezig.  Vermijdt dubbele optellingen !\r\n\r\n" +
                                 "Kies Sluiten indien U zopas de fiche reeds bijgewerkt hebt.";
                    MessageBox.Show(msg, "Investeringsfiche zelfde datum en zelfde bedrag", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (!DateCheck(TekstInfo6.Text, BOOKYEARAS_TEXT))
                {
                    string msg = "Opgelet, U probeert een investeringsfiche\r\n" +
                                 "van een ander boekjaar bij te werken !\r\n\r\n" +
                                 "Duidt EERST in de fiche van de leverancier een investeringsrekening aan geldig voor dit boekjaar en probeer daarna opnieuw.";
                    MessageBox.Show(msg, "Boekhoudkundige gebruikersfout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InvestWarning = true;
                    this.Close();                    
                }
            }
        }

        private void FormPurchaseInvestmentSheet_FormClosed(object sender, FormClosedEventArgs e)
        {
            BClose(TABLE_VARIOUS);
        }

        private void Annuleren_Click(object sender, EventArgs e)
        {            
            Msg = "InvesteringFiche negeren, bent U zeker?" + Environment.NewLine + Environment.NewLine +
                  "Opgelet: U dient dan alle lijnen voor dit document over te slaan.";
            DialogResult result = MessageBox.Show(
                Msg,
                "Investeringsfiche overslaan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            VBib(TABLE_VARIOUS, TekstInfo3.Text, "v019");
            VBib(TABLE_VARIOUS, TekstInfo2.Text, "v082");
            VBib(TABLE_VARIOUS, TekstInfo0.Text, "v083");
            VBib(TABLE_VARIOUS, Dec(ParseVbVal(TekstInfo1.Text) + ParseVbVal(TekstInfo7.Text), MASK_EURBH), "v084");
            VBib(TABLE_VARIOUS, (Versneld.Checked ? 1 : 0).ToString("0", CultureInfo.InvariantCulture), "v086");
            VBib(TABLE_VARIOUS, TekstInfo4.Text, "v087");
            VBib(TABLE_VARIOUS, TekstInfo5.Text, "v088");
            VBib(TABLE_VARIOUS, "18" + VBibText(TABLE_VARIOUS, "#v087 #"), "v005");

            string msg = "Informatielijn opslaan...\r\nBent U zeker ?";
            DialogResult result = MessageBox.Show(
                msg,
                "Fiche bijwerken/wegschrijven",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (result != DialogResult.Yes)
            {
                return;
            }

            if (_nietAanwezig != 0)
            {
                BInsert(TABLE_VARIOUS, 1);
            }
            else
            {
                BUpdate(TABLE_VARIOUS, 1);
            }

            Close();
        }

        private void TekstInfo_Enter(object sender, EventArgs e)
        {
            int index = GetTekstInfoIndex(sender);
            _tekstInfo[index].SelectionStart = 0;
            _tekstInfo[index].SelectionLength = _tekstInfo[index].Text.Length;
        }

        private void TekstInfo_Leave(object sender, EventArgs e)
        {
            int index = GetTekstInfoIndex(sender);

            switch (index)
            {
                case 0:
                    if (DateInvalid(_tekstInfo[index].Text))
                    {
                        _tekstInfo[index].Text = PartMid(GridText ?? string.Empty, 1, 10);
                        SystemSounds.Beep.Play();
                    }
                    break;

                case 1:
                case 8:
                    _tekstInfo[index].Text = Dec(ParseVbVal(_tekstInfo[index].Text), MASK_EURBH);
                    break;

                case 2:
                    int tempoBdrg = (int)ParseVbVal(_tekstInfo[index].Text);
                    if (tempoBdrg < 1 || tempoBdrg > 50)
                    {
                        SystemSounds.Beep.Play();
                        tempoBdrg = 5;
                    }

                    _tekstInfo[index].Text = Dec(tempoBdrg, "##0");
                    break;

                case 5:
                    if (!(_tekstInfo[index].Text ?? string.Empty).TrimStart().StartsWith("630", StringComparison.Ordinal))
                    {
                        SystemSounds.Beep.Play();
                        _tekstInfo[index].Text = "6300000";
                        BeginInvoke(new Action(() => _tekstInfo[index].Focus()));
                    }
                    break;
            }
        }

        private static double ParseVbVal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            string trimmed = text.Trim();
            if (double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            if (double.TryParse(trimmed, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
            {
                return value;
            }

            return 0;
        }

        private int GetTekstInfoIndex(object sender)
        {
            return sender is Control c && c.Tag is int idx ? idx : 0;
        }
    }
}
