using marVSS2028.MimMenu.Filing;
using System;
using System.Globalization;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028
{
    public partial class FormBasicTable : Form
    {
        private int _t;
        private readonly string[,] _voorkeurQuick = new string[11, 11];

        public FormBasicTable()
        {
            InitializeComponent();
            
            ComboBoxSearchOn.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FormBasicFile_Activated(object sender, EventArgs e)
        {
            try
            {
                int fl = int.Parse(Tag?.ToString() ?? "0");
                if (string.IsNullOrEmpty(LOCATION_COMPANYDATA)) return;

                if (ComboBoxSearchOn.Items.Count == 0)
                {
                    for (_t = 0; _t <= FL_NUMBEROFINDEXEN[fl]; _t++)
                    {
                        ComboBoxSearchOn.Items.Add(
                            _t.ToString("D2") + ":" +
                            FLINDEX_CAPTION[fl, _t] + " (" +
                            JETTABLEUSE_INDEX[fl, _t].Trim() + ")");
                    }
                    ComboBoxSearchOn.SelectedIndex = 1;
                    
                }
                NieuweFiche(fl);
            }
            catch { }
        }

        private void FormBasisFiche_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;
        }

        private void ComboBoxSearchOn_GotFocus(object sender, EventArgs e)
        {
            AcceptButton = ButtonSearchOn;  // ButtonSearchOn.Default = True
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ButtonFirst_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            Mim.InfoData.Visible = false;

            //Move First
            BFirst(fl, 0);
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                ButtonEdit.Enabled = false;
                MaskedTextBoxDescription.Text = "";
            }
            else
            {
                INSERT_FLAG[fl] = 0;
                RecordNaarFiche(fl);
                ButtonEdit.Enabled = true;
            }
        }

        private void ButtonLast_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            Mim.InfoData.Visible = false;

            //Move Last
            BLast(fl, 0);
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                ButtonEdit.Enabled = false;
                MaskedTextBoxDescription.Text = "";
            }
            else
            {
                INSERT_FLAG[fl] = 0;
                RecordNaarFiche(fl);
                ButtonEdit.Enabled = true;
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            Mim.InfoData.Visible = false;

            //Move Next
            BNext(fl);
            if (Ktrl == 9)
            {
                BLast(fl, 0);
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    ButtonEdit.Enabled = false;
                    MaskedTextBoxDescription.Text = "";
                }
            }
            if (Ktrl == 0)
            {
                INSERT_FLAG[fl] = 0;
                RecordNaarFiche(fl);
                ButtonEdit.Enabled = true;
            }
        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            Mim.InfoData.Visible = false;

            //Move Previous
            BPrev(fl);
            if (Ktrl == 9)
            {
                BFirst(fl, 0);
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    ButtonEdit.Enabled = false;
                    MaskedTextBoxDescription.Text = "";
                }
            }
            if (Ktrl == 0)
            {
                INSERT_FLAG[fl] = 0;
                RecordNaarFiche(fl);
                ButtonEdit.Enabled = true;
            }
        }

        private void ButtonRelating_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            Mim.InfoData.Visible = false;

            //Ledger or balance buyer and seller
            if (fl >= TABLE_CUSTOMERS && fl <= TABLE_SUPPLIERS)
            {                
                BalansKontroleWithRecordSet(fl);
            }
            else if (fl == TABLE_LEDGERACCOUNTS)
            {
                // TODO
                FormLedgerSQL HistoriekSQL = new FormLedgerSQL();
                HistoriekSQL.ShowDialog();
            }
            else
            {
                MessageBox.Show("boekhoudkontrole (nog) niet aanwezig");
            }

            INSERT_FLAG[fl] = 0;
            RecordNaarFiche(fl);
            ButtonEdit.Enabled = true;
        }

        private void ButtonSearchOn_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");

            if (fl == TABLE_CONTRACTS)
            {
                // TODO
                // Venster.ShowDialog();
            }
            else
            {
                SharedFl = fl;
                aIndex = int.Parse(ComboBoxSearchOn.Text.Substring(0, 2));
                GridText = MasketEditBoxInfo.Text;

                using (var sqlSearch = new marVSS2028.PublicForms.FormSearchSQL())
                    sqlSearch.ShowDialog(this);
            }

            if (Ktrl == 0)
            {
                MasketEditBoxInfo.Text = VBibText(fl, "#" + JETTABLEUSE_INDEX[fl, 0] + "#");
                INSERT_FLAG[fl] = 0;
                RecordNaarFiche(fl);
                ButtonEdit.Enabled = true;
                AcceptButton = ButtonEdit;  // ButtonEdit.Default = True
                MasketEditBoxInfo.Focus();
            }
            else
            {
                ButtonEdit.Enabled = false;
                MasketEditBoxInfo.Text = string.Empty;
                INSERT_FLAG[fl] = 1;
                MaskedTextBoxDescription.Text = "";
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            if (INSERT_FLAG[fl] != 0) return;

            string msg = "Bestaande fiche " + Text + " verwijderen.  Bent U zeker ?";
            var result = MessageBox.Show(msg, MasketEditBoxInfo.Text,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                BDelete(fl);
            // Knop_Click 3 was for new Record
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            int fl = int.Parse(Tag?.ToString() ?? "0");
            Mim.InfoData.Visible = false;

            //EDIT
            if (PeppolFlag && fl == TABLE_SUPPLIERS)
            {
                VBib(TABLE_SUPPLIERS, MasketEditBoxInfo.Text, "A110");  //Uniek Codenummer
            }
            else
            {
                string teZoeken = MasketEditBoxInfo.Text.Trim();
                if (teZoeken == "") { System.Media.SystemSounds.Beep.Play(); return; }

                BGet(fl, 0, MasketEditBoxInfo.Text);
                if (Ktrl == 0)
                {
                    INSERT_FLAG[fl] = 0;
                    RecordNaarFiche(fl);
                    ButtonEdit.Enabled = true;
                }
                else
                {
                    NieuweFiche(fl);
                    MasketEditBoxInfo.Text = teZoeken;
                    MaskedTextBoxDescription.Text = "";
                }

                if (fl == TABLE_LEDGERACCOUNTS)
                    DbKontrole(MasketEditBoxInfo.Text.Trim(), TABLE_LEDGERACCOUNTS);

                if (INSERT_FLAG[fl] == 1)
                {
                    if (fl == TABLE_CUSTOMERS || fl == TABLE_SUPPLIERS)
                        VBib(fl, MasketEditBoxInfo.Text, "A110");   //Klant/Levnummer
                    else if (fl == TABLE_LEDGERACCOUNTS)
                        VBib(fl, MasketEditBoxInfo.Text, "v019");   //Rekeningnummer
                    else
                        MessageBox.Show("Stop");
                }
            }

            if (TeleBibClick(fl))
                FicheNaarRecord(fl);

            Mim.InfoData.Visible = false;
            INSERT_FLAG[fl] = 0;
            RecordNaarFiche(fl);
            ButtonEdit.Enabled = true;
        }

        private void FicheNaarRecord(int fl)
        {
            BGet(fl, 0, VSet(MasketEditBoxInfo.Text, FLINDEX_LEN[fl, 0]));
            if (Ktrl == 0)
                BUpdate(fl, 0);
            else
                BInsert(fl, 0);
            //Knop_Click 3 was for new Record
        }

        private void NieuweFiche(int fl)
        {
            if (PeppolFlag) return;

            DaoBlankoRecord(fl);
            MasketEditBoxInfo.Text = string.Empty;
            INSERT_FLAG[fl] = 1;
            ButtonEdit.Enabled = false;
            AcceptButton = ButtonEdit;   // ButtonEdit.Default = True
            MasketEditBoxInfo.Enabled = true;

            try { MasketEditBoxInfo.Focus(); } catch { }
        }

        private void RecordNaarFiche(int fl)
        {
            try
            {
                TLB_RECORD[fl] = string.Empty;
                if (Ktrl == 0)
                    RecordToVeld(fl);

                MasketEditBoxInfo.Text = VBibText(fl, "#" + JETTABLEUSE_INDEX[fl, 0] + "#");
                Application.DoEvents();

                string msg = string.Empty;
                for (_t = 0; _t <= 1; _t++)
                    msg += FVT[fl, _t].TrimEnd() + " ";

                MaskedTextBoxDescription.Text = msg;
                INSERT_FLAG[fl] = 0;
            }
            catch { }
        }
    }
}
