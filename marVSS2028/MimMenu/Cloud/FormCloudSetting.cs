using System;
using System.IO;
using System.Windows.Forms;
using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.ShellHelper;

namespace marVSS2028.Forms
{
    public partial class FormCloudSetting : Form
    {
        private bool toggleEdit = false;

        public FormCloudSetting()
        {
            InitializeComponent();
        }
                
        private void ToggleProperties(bool toggleSet)
        {
            ButtonDefaultResetForMapMarnt.Visible = toggleSet;
            ButtonDefaultResetForOneDrive.Visible = toggleSet;
            ButtonSave.Visible = toggleSet;

            TextBoxCloudMarnt.Enabled = toggleSet;
            TextBoxCloudMario.Enabled = toggleSet;
            TextBoxCloudArchive.Enabled = toggleSet;
        }

        private void FormCloudSetting_Load(object sender, EventArgs e)
        {
            string strDataLocatie = LaadTekst("BedrijfOpenen", "DataDefault");
            if (strDataLocatie == "server")
                TextBoxMarntDataMap.Text = LaadTekstOLD("marIntegraal", "ServerBedrijfsinhoudsopgave"); // Server anders
            else
                TextBoxMarntDataMap.Text = LaadTekstOLD("marIntegraal", "Bedrijfsinhoudsopgave2025"); // Lokaal is standaard
            toggleEdit = false;
            ToggleProperties(toggleEdit);

            if (!string.IsNullOrEmpty(LOCATION_COMPANYDATA))
            {
                ButtonDefaultResetForOneDrive.Enabled = true;
                ButtonDefaultResetForMapMarnt.Enabled = true;
            }

            if (LaadTekstOLD("dnnInstellingen", "CodaIOMap") == "")
            {
                BeWaarTekst("dnnInstellingen", "CodaIOMap", LOCATION_DESKTOP);
                TextBoxCodaIOMap.Text = LaadTekstOLD("dnnInstellingen", "CodaIOMap");
            }

            if (LaadTekstOLD("dnnInstellingen", "Cloud") == "")
            {
                string bedrijfsLoc = LaadTekstOLD(appTitleAndVersion, "Bedrijfsinhoudsopgave");
                MessageBox.Show(
                    "Nieuwe PC of nog geen instellingen voor Cloud.  Wijzig de volgende standaardwaarden " +
                    "a.u.b. voor uw bedrijf (zie aanbevelingen in onze voorbeeld nota!) of vraag onze " +
                    "gratis bijstand om dit in uw plaats in orde te brengen.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                TextBoxCloudArchive.Text = bedrijfsLoc + @"\cloud\archief"; // "http://localhost/rvDNN"
                TextBoxCloudMarnt.Text  = bedrijfsLoc + @"\cloud";          // "C:\Users\NaamVanGebruiker\SkyDrive"
                TextBoxCloudMario.Text  = bedrijfsLoc + @"\cloud\mario";    // "c:\dotnetnuke\rvDNN\portals\0\documenten\postvak"
            }
            else
            {
                TextBoxCloudArchive.Text = LaadTekstOLD("dnnInstellingen", "Archief");
                TextBoxCloudMarnt.Text   = LaadTekstOLD("dnnInstellingen", "Cloud");
                TextBoxCloudMario.Text   = LaadTekstOLD("dnnInstellingen", "Mario");
                TextBoxCodaIOMap.Text    = LaadTekstOLD("dnnInstellingen", "CodaIOMap");
            }

            // TODO: get cmdWegBoekModus.SelectedIndex = 2 from settings and set the radio buttons accordingly
            radioButtonShowAlwaysBookingsInfo.Checked = true;
            
        }

        private void ButtonCloudArchive_Click(object sender, EventArgs e)
        {
            if (!ShellExecuteWithFallback(TextBoxCloudArchive.Text))
                MessageBox.Show(
                    "Kon " + TextBoxCloudArchive.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonCloudMario_Click(object sender, EventArgs e)
        {
            if (!ShellExecuteWithFallback(TextBoxCloudMario.Text))
                MessageBox.Show(
                    "Kon " + TextBoxCloudMario.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonCloudMarnt_Click(object sender, EventArgs e)
        {
            if (!ShellExecuteWithFallback(TextBoxCloudMarnt.Text))
                MessageBox.Show(
                    "Kon " + TextBoxCloudMarnt.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonCodaIOMap_Click(object sender, EventArgs e)
        {
            if (!ShellExecuteWithFallback(TextBoxCodaIOMap.Text))
                MessageBox.Show(
                    "Kon " + TextBoxCodaIOMap.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonMarntDataMap_Click(object sender, EventArgs e)
        {
            if (!ShellExecuteWithFallback(TextBoxMarntDataMap.Text))
                MessageBox.Show(
                    "Kon " + TextBoxMarntDataMap.Text + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonDefaultResetForMapMarnt_Click(object sender, EventArgs e)
        {
            string marNTLocatie = LaadTekstOLD(appTitleAndVersion, "Bedrijfsinhoudsopgave2025").ToLower()
                                      .Replace(@"\data", string.Empty);

            string serverMap = LaadTekstOLD(appTitleAndVersion, "ServerBedrijfsinhoudsopgave").Trim().ToLower();
            if (serverMap != "")
            {
                MessageBox.Show(
                    "Voor deze PC bestaat al een serverinhoudsopgave:\r\n" +
                    serverMap + "\r\n\r\n" +
                    "Verwijder indien nodig.\r\n\r\n" +
                    "Met serverinstellingen gedraagt deze PC zich als client.\r\n" +
                    "Voor marIntegraal draaiende op locatie (client) dient U\r\n" +
                    "de instellingen manueel in te voeren a.d.h.v. uw server-link.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string msg =
                "Akkoord voor:\r\n" +
                "CLOUD   MARNT: " + marNTLocatie + " (dus dezelfde hoofdmap)\r\n" +
                "CLOUD   MARIO: " + marNTLocatie + @"\manueel" + "\r\n" +
                "CLOUD ARCHIEF: " + marNTLocatie + @"\archief";

            if (MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                TextBoxCloudMarnt.Text  = marNTLocatie;
                TextBoxCloudMario.Text  = marNTLocatie + @"\manueel";
                TextBoxCloudArchive.Text = marNTLocatie + @"\archief";

                EnsureFolder(TextBoxCloudMario.Text);
                EnsureFolder(TextBoxCloudArchive.Text);

                ButtonSave_Click(sender, e);
            }
        }

        private void ButtonDefaultResetForOneDrive_Click(object sender, EventArgs e)
        {
            string marNTLocatie = LaadTekstOLD("marIntegraal", "Bedrijfsinhoudsopgave2025").ToLower()
                                      .Replace(@"\data", string.Empty);

            string serverMap = LaadTekstOLD("marIntegraal", "ServerBedrijfsinhoudsopgave").Trim().ToLower();
            if (serverMap != "")
            {
                MessageBox.Show(
                    "Voor deze PC bestaat al een serverinhoudsopgave:\r\n" +
                    serverMap + "\r\n\r\n" +
                    "Verwijder indien nodig.\r\n\r\n" +
                    "Met serverinstellingen gedraagt deze PC zich als client.\r\n" +
                    "Voor marIntegraal draaiende op locatie (client) dient U\r\n" +
                    "de instellingen manueel in te voeren a.d.h.v. uw server-link.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string systemPersonalDocs = SYSTEM_MYPERSONALDOCUMENTS.ToLower();
            if (systemPersonalDocs.IndexOf("onedrive", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                string msg =
                    "Dit is een toestel met 'OneDrive' Map ideaal voor automatische\r\n" +
                    "archivering naar de CLOUD.\r\n\r\n" +
                    "Akkoord voor:\r\n" +
                    "CLOUD   MARNT: " + systemPersonalDocs + @"\marNT" + "\r\n" +
                    "CLOUD   MARIO: " + systemPersonalDocs + @"\marNT\manueel" + "\r\n" +
                    "CLOUD ARCHIEF: " + systemPersonalDocs + @"\marNT\archief";

                if (MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    TextBoxCloudMarnt.Text   = systemPersonalDocs + @"\marNT";
                    TextBoxCloudMario.Text   = systemPersonalDocs + @"\marNT\manueel";
                    TextBoxCloudArchive.Text = systemPersonalDocs + @"\marNT\archief";

                    EnsureFolder(TextBoxCloudMarnt.Text);
                    EnsureFolder(TextBoxCloudMario.Text);
                    EnsureFolder(TextBoxCloudArchive.Text);

                    ButtonSave_Click(sender, e);
                }
            }
        }

        private void ButtonToggle_Click(object sender, EventArgs e)
        {
            toggleEdit = !toggleEdit;
            ToggleProperties(toggleEdit);
            if (toggleEdit)
                MessageBox.Show(
                    "Wees bedachtzaam bij het wijzigen van deze belangrijke instellingen voor MarIntegraal en MarSync",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            BeWaarTekst("dnnInstellingen", "Archief", TextBoxCloudArchive.Text); // archief cloud
            BeWaarTekst("dnnInstellingen", "Mario",   TextBoxCloudMario.Text);   // mario cloud
            BeWaarTekst("dnnInstellingen", "Cloud",   TextBoxCloudMarnt.Text);   // marnt cloud
            Close();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Helper: replaces VB6 fs.CreateFolder with error swallowed as informational message
        private void EnsureFolder(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch
            {
                MessageBox.Show(
                    "Map bestaat reeds\r\n\r\n" + path,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }        
    }
}
