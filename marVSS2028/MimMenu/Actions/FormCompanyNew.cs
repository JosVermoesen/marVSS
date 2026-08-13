using System;
using System.IO;
using System.Windows.Forms;

using marVSS2028.Classes;
using marVSS2028.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;


namespace marVSS2028.MimMenu.Actions
{
    public partial class FormCompanyNew : Form
    {
        // ── fields (VB6: module-level Dim) ──────────────────────────────────
        private string _bedrijfsNummer;
        private string _van;

        public FormCompanyNew()
        {
            InitializeComponent();
            WireHighlightEvents(this);
        }

        // ── Form_Load ────────────────────────────────────────────────────────
        private void FormCompanyNew_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;
            // Close FormOpenCompany if open (VB6: Unload BedrijfOpenen)
            foreach (Form f in Application.OpenForms)
            {
                if (f is FormOpenCompany)
                {
                    f.Close();
                    break;
                }
            }

            Boekjaar.Text = Globals.MIM_GLOBAL_DATE.Substring(Globals.MIM_GLOBAL_DATE.Length - 4);
            _bedrijfsNummer = VolgendBedrijf();
            this.Text = "Nieuw Bedrijf (" + _bedrijfsNummer + ")";

            CmbBedrijfsType.Items.AddRange(new object[]
            {
                "0: Standaard KMO",
                "1: marIntegraal NT Light",
                "2: Garage met Margeverkoop",
                "3: Auteursrechtadministratie",
                "4: Forfaitaire BTW",
                "5: Verzekeringsbemiddelaar",
                "6: Syndicus administratie",
                "7: BTW vrij(vb. verhuur onroerende goed, vzw)",
                "9: Gezinshuishouding"
            });
            CmbBedrijfsType.SelectedIndex = 0;

            TABLEDEF_ONT[TABLE_COUNTERS] = "00.ONT";
            bstNaam[TABLE_COUNTERS] = "jr" + Boekjaar.Text;

            string strDataLocatie = LaadTekstOLD("BedrijfOpenen", "DataDefault");

            if (strDataLocatie == "server")
            {                
                LocationMarntData.Text = LaadTekstOLD("marIntegraal", "ServerBedrijfsinhoudsopgave") + "\\" + _bedrijfsNummer + "\\";
            }
            else
            {                
                LocationMarntData.Text = LaadTekstOLD("marIntegraal", "Bedrijfsinhoudsopgave2025") + "\\" + _bedrijfsNummer + "\\";
            }             
        }

        // ── BedrijfsNaam_KeyPress ────────────────────────────────────────────
        private void BedrijfsNaam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                SendKeys.Send("{TAB}");
        }

        // ── Boekjaar_Leave (was LostFocus) ──────────────────────────────────
        private void Boekjaar_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(Boekjaar.Text, out int val) && val < 2027 && val > 2024)
                return;

            Boekjaar.Text = MIM_GLOBAL_DATE.Substring(MIM_GLOBAL_DATE.Length - 4);
            Boekjaar.Focus();
            System.Media.SystemSounds.Beep.Play();
        }
        
        // ── Installeren_Click ────────────────────────────────────────────────
        private void Installeren_Click(object sender, EventArgs e)
        {
            Globals.BAModus = 1;
            _van = txtStartMaand.Text + "/" + Boekjaar.Text;

            int.TryParse(Maanden.Text, out int maandenVal);
            string msg = "Installatie Bedrijf " + _bedrijfsNummer + " voor :\r\n\r\n"
                       + BedrijfsNaam.Text + "\r\n\r\n"
                       + maandenVal.ToString("00") + " maanden vanaf : " + _van;

            if (Makelaar.Checked)
                msg += "\r\n\r\nMet programmakeuze voor makelaars !";

            // VB6: MsgBox(A, 292) → YesNo + Question + DefaultButton2
            if (MessageBox.Show(msg, "Bevestiging",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            string destPath = LOCATION_ + _bedrijfsNummer;
            if (!CreatePath(destPath))
                MessageBox.Show("foutmelding bij aanmaak " + destPath);

            LOCATION_COMPANYDATA = LOCATION_ + _bedrijfsNummer + "\\";

            PeriodesMaken();
            LabelMaken();

            string defPath = Application.StartupPath + "\\Content\\Def";            
            if (!CopyFile(defPath, destPath, "LICMARNT.###")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK11.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK12.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK13.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK14.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK21.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK22.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK23.TXT")) goto ErrorSetup;
            if (!CopyFile(defPath, destPath, "DOK24.TXT")) goto ErrorSetup;
            if (!CopyFile(PROGRAM_LOCATION + "MdX", destPath, "MARNT.MDV")) goto ErrorSetup;

            jetConnect = ADOJET_PROVIDER
                       + "Data Source=" + LOCATION_COMPANYDATA + "marnt.mdv;"
                       + "Persist Security Info=False";

            oleDbConnect = OLEDBJET_PROVIDER +
                LOCATION_COMPANYDATA + "marnt.mdv";

            try { BClose(99); } catch { }

            // Create counters table via ADOX (mirrors VB6 ADOX.Catalog block)
            bstNaam[TABLE_COUNTERS] = "jr" + Boekjaar.Text;
            // TODO create table
            // CreateCountersTable(
            //     jetConnect,
            //     bstNaam[TABLE_COUNTERS]);

            adntDB = new ADODB.Connection();
            adntDB.Open(jetConnect);
            
            InitBestanden();
            InstallTellers();
            BClose(99);
            adntDB.Close();
            adntDB = null;

            Negeren_Click(sender, e);   // close form on success
            return;

        ErrorSetup:
            MessageBox.Show(
                "Installatie nieuw bedrijf zonder succes.  Raadpleeg R&VSOFT.",
                "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.Close();
        }

        // ── LabelMaken ──────────────────────────────────────────────────────
        private void LabelMaken()
        {
            string path = LOCATION_COMPANYDATA + "marnt.txt";
            File.WriteAllText(path, BedrijfsNaam.Text);
        }

        // ── Negeren_Click ────────────────────────────────────────────────────
        private void Negeren_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ── PeriodesMaken ────────────────────────────────────────────────────
        private void PeriodesMaken()
        {
            int.TryParse(Maanden.Text, out int aantalMaanden);
            int vanafMaand = int.Parse(_van.Substring(3, 2));
            int vanafJaar = int.Parse(_van.Substring(6, 4));

            // VB6: Open LOCATION_COMPANYDATA + "9999.oct" For Random Len=4
            string octPath = Globals.LOCATION_COMPANYDATA + "9999.oct";
            using (var fs = new FileStream(octPath, FileMode.Create, FileAccess.Write))
            {
                WriteFixedRecord(fs, "0", 4, 1);
                WriteFixedRecord(fs, _van.Substring(6, 4), 4, 2);
                WriteFixedRecord(fs, "1", 4, 3);
            }

            // VB6: Open LOCATION_ + BedrijfsNummer + "/def00.oct" For Random Len=16
            string defOctPath = Globals.LOCATION_ + _bedrijfsNummer + "/def00.oct";
            using (var fs = new FileStream(defOctPath, FileMode.Create, FileAccess.Write))
            {
                int t;
                for (t = 1; t <= aantalMaanden; t++)
                {
                    string b = vanafJaar.ToString("0000")
                             + vanafMaand.ToString("00") + "01"
                             + vanafJaar.ToString("0000")
                             + vanafMaand.ToString("00")
                             + DAYS_IN_MONTH[vanafMaand].ToString("00");

                    WriteFixedRecord(fs, b, 16, t);

                    if (vanafMaand == 12 && t < aantalMaanden)
                    {
                        vanafMaand = 1;
                        vanafJaar++;
                    }
                    else
                    {
                        vanafMaand++;
                    }
                }

                // Pad remaining records up to 99 with empty
                string empty = new string(' ', 16);
                for (int t2 = t; t2 <= 99; t2++)
                    WriteFixedRecord(fs, empty, 16, t2);

                // Record 99 = last period boundary (VB6: Put Fl, 99, b)
                string lastB = vanafJaar.ToString("0000")
                             + (vanafMaand - 1).ToString("00")
                             + DAYS_IN_MONTH[vanafMaand - 1].ToString("00")
                             + vanafJaar.ToString("0000")
                             + (vanafMaand - 1).ToString("00")
                             + DAYS_IN_MONTH[vanafMaand - 1].ToString("00");
                WriteFixedRecord(fs, lastB, 16, 99);
            }
        }

        /// <summary>
        /// Writes a fixed-length ASCII record at a 1-based record position
        /// (mirrors VB6 "Put FileNumber, RecordNumber, FixedLengthString").
        /// </summary>
        private static void WriteFixedRecord(FileStream fs, string value, int recordLen, int recordNumber)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(
                value.PadRight(recordLen).Substring(0, recordLen));
            fs.Seek((long)(recordNumber - 1) * recordLen, SeekOrigin.Begin);
            fs.Write(bytes, 0, bytes.Length);
        }

        // ── VolgendBedrijf ───────────────────────────────────────────────────
        private string VolgendBedrijf()
        {
            for (int t = 1; t <= 999; t++)
            {
                string path = Globals.LOCATION_ + t.ToString("000") + "\\";
                try
                {
                    if (!Directory.Exists(path))
                        return t.ToString("000");
                    if (Directory.GetFileSystemEntries(path).Length == 0)
                        return t.ToString("000");
                }
                catch (DirectoryNotFoundException)
                {
                    return t.ToString("000");
                }
                catch { /* keep searching */ }
            }
            MessageBox.Show("Stop");
            return "000";
        }

        // ── txtStartMaand_Leave (was LostFocus) ──────────────────────────────
        private void txtStartMaand_Leave(object sender, EventArgs e)
        {
            if (txtStartMaand.Text.Length != 5)
            {
                MessageBox.Show("Enkel formaat dd/mm gebruiken a.u.b.");
                txtStartMaand.Text = "01/04";
                txtStartMaand.Focus();
                return;
            }
            if (txtStartMaand.Text[2] != '/')
            {
                MessageBox.Show("Enkel formaat dd/mm gebruiken a.u.b.");
                txtStartMaand.Text = "01/04";
                txtStartMaand.Focus();
            }
        }

        // ── TypeBoekjaar_Click ───────────────────────────────────────────────
        private void TypeBoekjaar_Click(object sender, EventArgs e)
        {
            if (sender == TypeBoekjaar0)
            {
                txtStartMaand.Text = "01/01";
                txtStartMaand.Enabled = false;
            }
            else if (sender == TypeBoekjaar1)
            {
                txtStartMaand.Text = "01/07";
                txtStartMaand.Enabled = false;
            }
            else if (sender == TypeBoekjaar2)
            {
                txtStartMaand.Text = "01/10";
                txtStartMaand.Enabled = false;
            }
            else if (sender == TypeBoekjaar3)
            {
                _van = "01/04/" + Boekjaar.Text;
                txtStartMaand.Text = "01/04";
                txtStartMaand.Enabled = true;
                txtStartMaand.Focus();
            }
        }

        // ── InstallTellers ───────────────────────────────────────────────────
        private void InstallTellers()
        {
            // Boekingen en algemene instellingen
            SS99("EUR", 296);
            SS99(CmbBedrijfsType.Text.Substring(0, 1), 20);
            SS99("1", 299);
            SS99("1", 306);
            SS99("2", 21);
            SS99("2", 291);
            SS99("2", 290);
            SS99("3", 183);
            SS99("2", 200);
            SS99("1", 201);
            SS99("2", 290);

            // Aankoopverrichtingen
            SS99("00000", 1);
            SS99("00000", 2);
            SS99("00000", 3);
            SS99("00000", 4);
            SS99("00000", 15);
            SS99("00000", 205);

            // Verkoopverrichtingen
            SS99("00000", 11);
            SS99("00000", 12);
            SS99("00000", 13);
            SS99("00000", 14);
            SS99("00000", 73);
            SS99("00000", 59);
            SS99("00000", 121);
            SS99("2", 53);
            SS99("0", 54);
            SS99("1001", 181);
            SS99("1", 182);
            SS99("11", 185);
            SS99("000", 186);
            SS99("0", 187);
            SS99("00000", 188);
            SS99("2", 202);
            SS99("1", 203);
            SS99("2", 72);
            SS99("1", 74);
            SS99("1", 75);
            SS99("1", 76);
            SS99("0000.00", 300);

            // BTW Default Rekeningen
            SS99("498054", 16);
            SS99("498055", 17);
            SS99("498056", 18);
            SS99("498057", 19);
            SS99("498059", 22);
            SS99("498063", 23);
            SS99("498064", 24);
            SS99("704000", 25);
            SS99("604000", 77);
            SS99("704100", 78);
            SS99("340000", 79);

            // Default Collectieve Rekeningen
            SS99("400000", 9);
            SS99("440000", 10);
            SS99("4000", 297);
            SS99("4400", 298);
            SS99("756000", 27);
            SS99("656000", 28);
            SS99("489000", 145);
            SS99("455", 146);
            SS99("4899999", 147);
            SS99("18", 148);
            SS99("2999999", 149);
            SS99("60", 150);
            SS99("6089999", 151);
            SS99("61", 152);
            SS99("6559999", 153);

            // Financieel en Rekeningen
            SS99("571000", 41);
            SS99(" 0", 31);
            SS99("561000", 42);
            SS99(" 0", 32);
            SS99("551000", 43);
            SS99(" 0", 33);
            SS99("552000", 44);
            SS99(" 0", 34);
            SS99("553000", 45);
            SS99(" 0", 35);
            SS99("582000", 39);
            SS99(" 0", 38);
            SS99("551000", 101);

            // Bedrijfsinformatie
            SS99("MijnBedrijf bv", 46);
            SS99("MijnStraatEnNummer", 47);
            SS99("PCPC_MijnWoonplaats", 48);
            SS99("Telefoon", 49);
            SS99(" ", 292);
            SS99(" ", 51);
            SS99("IBAN", 293);
            SS99("BIC", 294);
            SS99("Mailadres Onderneming", 295);
            SS99("Contactpersoon", 52);
            SS99("Mailadres Contactpersoon", 50);

            // Status Boekjaar
            SS99("0", 62);
            SS99("0", 63);
            SS99("0", 64);

            // Kassaverkoop
            SS99("OFF", 130);
            SS99("1", 131);
            SS99("0", 132);
        }      
    }
}