using marVSS2028.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.ShellHelper;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.Forms
{
    public partial class FormOpenCompany : Form
    {
        public FormOpenCompany()
        {
            InitializeComponent();
            WireHighlightEvents(this);
            Text = "Bedrijf openen";
        }

        private void FormOpenCompany_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;

            // Set up ListView columns (Report view)
            ListViewCompanies.Columns.Clear();
            ListViewCompanies.Columns.Add("Benaming", 491);
            ListViewCompanies.Columns.Add("Map", 35);
            ListViewCompanies.View = View.Details;
            ListViewCompanies.FullRowSelect = true;
            ListViewCompanies.ListViewItemSorter = new ListViewItemComparer(1, SortOrder.Ascending);

            // Restore last data location preference
            string strDataLocatie = LaadTekstOLD("BedrijfOpenen", "DataDefault");
            BeWaarTekst("BedrijfOpenen", "DataDefault", strDataLocatie);

            if (string.IsNullOrWhiteSpace(strDataLocatie))
                strDataLocatie = "lokaal";

            if (strDataLocatie == "server")
            {
                RadioButtonServer.Checked = true;
                TextBoxLocation.Text = LaadTekstOLD("marIntegraal", "ServerBedrijfsinhoudsopgave");
            }
            else
            {
                RadioButtonLocal.Checked = true;
                TextBoxLocation.Text = LaadTekstOLD("marIntegraal", "Bedrijfsinhoudsopgave2025");
            }

            if (string.IsNullOrWhiteSpace(TextBoxLocation.Text))
                TextBoxLocation.Text = LOCATION_MYDOCUMENTS;

            LOCATION_ = TextBoxLocation.Text.TrimEnd('\\') + @"\";
            FillCompanyList();
        }

        private void FillCompanyList()
        {
            ListViewCompanies.Items.Clear();

            string myPath = TextBoxLocation.Text.TrimEnd('\\') + @"\";

            try
            {
                if (!Directory.Exists(myPath))
                    return;

                foreach (string dir in Directory.GetDirectories(myPath))
                {
                    string myName = Path.GetFileName(dir);
                    string marntTxt = Path.Combine(dir, "marnt.txt");

                    if (File.Exists(marntTxt))
                    {
                        string naamDetail;
                        using (var sr = new StreamReader(marntTxt))
                        {
                            naamDetail = sr.ReadLine() ?? myName;
                        }

                        var item = new ListViewItem(naamDetail);
                        item.SubItems.Add(myName);
                        ListViewCompanies.Items.Add(item);
                    }
                }
                if (ListViewCompanies.Items.Count > 0)
                {
                    ListViewCompanies.Items[0].Selected = true;
                    ListViewCompanies.Select();
                }
            }
            catch
            { }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonOpenFolder_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback(TextBoxLocation.Text);
        }

        private void ButtonToggleEditLocation_Click(object sender, EventArgs e)
        {
            TextBoxLocation.Enabled = !TextBoxLocation.Enabled;
            if (TextBoxLocation.Enabled)
            {
                TextBoxLocation.Focus();
            }
            else
            {
                string key = RadioButtonLocal.Checked
                    ? "Bedrijfsinhoudsopgave2025"
                    : "ServerBedrijfsinhoudsopgave";
                string saved = LaadTekst(Application.ProductName, key);

                if (TextBoxLocation.Text != saved)
                {
                    var res = MessageBox.Show(
                        TextBoxLocation.Text + "\r\n\r\nWordt dit de nieuwe opstartinhoudsopgave ?",
                        "Lokatie instellen",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (res == DialogResult.Yes)
                    {
                        BeWaarTekst(Application.ProductName, key, TextBoxLocation.Text);
                        MessageBox.Show("Hierna wordt er afgesloten.  Start het programma opnieuw op.");
                        Application.Exit();
                    }
                    else
                    {
                        TextBoxLocation.Text = saved;
                    }
                }                                
                FillCompanyList();
            }
        }

        private void RadioButtonLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (!RadioButtonLocal.Checked && !RadioButtonServer.Checked)
                return;

            try
            {
                if (RadioButtonServer.Checked)
                {
                    TextBoxLocation.Text = LaadTekstOLD("marIntegraal", "ServerBedrijfsinhoudsopgave");

                    BeWaarTekst("BedrijfOpenen", "DataDefault", "server");
                    ButtonCompact.Enabled = false;
                }
                else
                {
                    TextBoxLocation.Text = LaadTekstOLD("marIntegraal", "Bedrijfsinhoudsopgave2025");
                    BeWaarTekst("BedrijfOpenen", "DataDefault", "lokaal");
                    ButtonCompact.Enabled = true;
                }

                // if (string.IsNullOrWhiteSpace(TextBoxLocation.Text))
                //    TextBoxLocation.Text = LOCATION_MYDOCUMENTS;

                LOCATION_ = TextBoxLocation.Text.TrimEnd('\\') + @"\";
                FillCompanyList();
            }
            catch
            {
                // VB6: On Error Resume Next
            }
        }

        private void ListViewCompanies_DoubleClick(object sender, EventArgs e)
        {
            ButtonOk_Click(sender, e);
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (ListViewCompanies.SelectedItems.Count == 0)
                return;

            string folderName = ListViewCompanies.SelectedItems[0].SubItems[1].Text;
            // TODO: In preview mode we should only accept company foldernames "098" and "099"
            if (IsPreviewMode && folderName != "098" && folderName != "099")
            {
                MessageBox.Show(
                    "MarIntegraal 2028 is in 'Evaluatie Modus'. Ter bescherming van uw gegevens zijn enkel de test-bedrijfsmappen '098' en '099' toegestaan.\n\n" +
                    "Om te testen, kies voor '098' of '099'.\n\nTIP: Maak een kopij van een actieve bedrijfsmap, plak en hernoem deze kopij naar '098' of '099' om te 'testen'.\n\n",                    
                    "Ongeldige Bedrijfskeuze", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string companyName = ListViewCompanies.SelectedItems[0].Text;

            LOCATION_COMPANYDATA = LOCATION_ + folderName + @"\";

            // Update main form caption
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.Text = appTitleAndVersion + " - [" + companyName.Trim() + "]";
            }
            AutoLoadCompany();

            Close();
        }

        private void ButtonCompact_Click(object sender, EventArgs e)
        {
            if (ListViewCompanies.Items.Count == 0)
                return;

            if (ListViewCompanies.SelectedItems.Count == 0)
                return;

            string folderName = ListViewCompanies.SelectedItems[0].SubItems[1].Text;
            string dbPath    = LOCATION_ + folderName + @"\marnt.mdv";
            string tmpPath   = LOCATION_ + folderName + @"\marnt.$$$";

            // Open a temporary connection just to read the ADO version
            Msg = "Huidige database in JetVersie 4.x vernieuwen\r";

            try
            {                
                jetConnect = ADOJET_PROVIDER +
                    "Data Source=" + dbPath + ";" +
                    "Persist Security Info=False";
                                
                adntDB = new ADODB.Connection();
                adntDB.Open(jetConnect);
                Msg += "Microsoft ADO Versie " + adntDB.Version + "\r\r";
                Msg += "LOCATION_ : " + dbPath + "\r\r";
                adntDB.Close();
                adntDB = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij openen database:\r\n" + ex.Message,
                    "Database vernieuwen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Msg += "Regelmatig te gebruiken indien U zelf tabellen, velden, indexen, " +
                   "SQL-opvraagdefinities aanmaakt en/of verwijdert + nadat journalen en " +
                   "dokumenten van een of meer boekjaren werden opgekuist.\r\r";
            Msg += "Onderhoud database.  Bent U zeker ?";

            if (MessageBox.Show(Msg, "Database vernieuwen (compact maken)",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                ListViewCompanies.Focus();
                return;
            }
            
            try
            {
                if (File.Exists(tmpPath))
                    File.Delete(tmpPath);

                SnelHelpPrint("Bezig...", BL_LOGGING);
                UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;                

                // JRO.JetEngine via late-binding COM (no reference needed)
                Type jroType = Type.GetTypeFromProgID("JRO.JetEngine");
                if (jroType == null)
                    throw new InvalidOperationException(
                        "JRO.JetEngine is niet geregistreerd op dit systeem.\r\n" +
                        "Installeer Microsoft Data Access Components (MDAC) of Access Database Engine.");

                object jro = Activator.CreateInstance(jroType);

                string srcConn  = ADOJET_PROVIDER + "Data Source=" + dbPath;
                string destConn = ADOJET_PROVIDER + "Data Source=" + tmpPath +
                                  ";Jet OLEDB:Engine Type=5";

                jroType.InvokeMember("CompactDatabase",
                    System.Reflection.BindingFlags.InvokeMethod, null, jro,
                    new object[] { srcConn, destConn });

                System.Runtime.InteropServices.Marshal.ReleaseComObject(jro);

                File.Delete(dbPath);
                File.Move(tmpPath, dbPath);

                SnelHelpPrint("Klaar!", BL_LOGGING);
            }
            catch (Exception ex)
            {                
                if (ex.Message.Contains("3301"))
                    MessageBox.Show("U kan geen database vernieuwen naar een lagere versie !",
                        "Database vernieuwen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show(ex.Message,
                        "Database vernieuwen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                ListViewCompanies.Focus();
            }
        }

        private void ListViewCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonServer.Checked)
                ButtonCompact.Enabled = false;
            else
                ButtonCompact.Enabled = ListViewCompanies.SelectedItems.Count > 0;
        }
    }
}

