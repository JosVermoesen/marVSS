using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using IDEALSoftware.VpeCommunity;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.VPETools;

namespace marVSS2028.Forms
{
    public partial class FormVpeTemplateEditor : Form
    {
        // ── State ─────────────────────────────────────────────────────────────
        private const int KopVoet  = 1;   // always 1 (VB6: set in Load, never changed)
        private int _taalKode  = 2;       // 1=Frans 2=NL 3=EN 4=DE
        private int _docuType  = 0;       // 0=Factuur … 6=Kwijting

        // ── Document-type menu items (for checked-one-at-a-time) ──────────────
        private ToolStripMenuItem[] _dokMenuItems;

        // ── Taal menu items (for checked-one-at-a-time) ───────────────────────
        private ToolStripMenuItem[] _taalMenuItems;

        public FormVpeTemplateEditor()
        {
            InitializeComponent();
        }

        // ═════════════════════════════════════════════════════════════════════
        // Form events
        // ═════════════════════════════════════════════════════════════════════

        private void FormVpeTemplateEditor_Load(object sender, EventArgs e)
        {
            _dokMenuItems = new[]
            {
                MenuDokFactuur, MenuDokLevering, MenuDokBestel,
                MenuDokOfferte, MenuDokBrief,    MenuDokRekening, MenuDokKwijting
            };

            _taalMenuItems = new[]
            {
                null,                // index 0 unused (VB6 was 1-based)
                MenuTaalFrans,
                MenuTaalNederlands,
                MenuTaalEngels,
                MenuTaalDuits
            };

            // Initialise the demo-label colour to its current BackColor (VB6: txtKleur = Label1.BackColor)
            TxtKleur.Text = ColorTranslator.ToOle(LblDemoTekst.BackColor).ToString();
        }

        private void FormVpeTemplateEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Mim.Report.IsOpen())
            {
                MessageBox.Show("Sluit eerst het PDF venster a.u.b.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // Menu – Bestand
        // ═════════════════════════════════════════════════════════════════════

        private void MenuBestandOpenen_Click(object sender, EventArgs e)
        {
            Inladen(KopVoet.ToString() + _taalKode.ToString() + _docuType.ToString());
        }

        private void MenuBestandSluiten_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ═════════════════════════════════════════════════════════════════════
        // Menu – Via kladblok
        // ═════════════════════════════════════════════════════════════════════

        private void MenuViaKBTekst_Click(object sender, EventArgs e)
        {
            OpenInNotepad(0);
        }

        private void OpenInNotepad(int index)
        {
            string typeKey = KopVoet.ToString() + _taalKode.ToString() + _docuType.ToString();
            string suffix  = index == 0 ? ".Txt" : "G.Txt";
            string bestand = LOCATION_COMPANYDATA + @"vpeSjbs\pdfDDEF" + typeKey + suffix;

            if (!File.Exists(bestand))
            {
                if (index != 0)
                {
                    MessageBox.Show(bestand + " is niet aanwezig", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show(
                    LOCATION_COMPANYDATA + @"vpeSjbs\pdfDDEF" + typeKey + ".Txt" +
                    " niet gevonden in de bedrijfsinhoudsopgave. Hierna wordt een voorbeelddocument vanuit de programmainhoudsopgave gekopieerd",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string src = AppDomain.CurrentDomain.BaseDirectory + @"VpeSjbs\";
                if (!CopyFile(src, LOCATION_COMPANYDATA + @"vpeSjbs\", "pdfDDEF" + typeKey + ".Txt"))
                {
                    MessageBox.Show(src + "pdfDDEF" + typeKey + ".Txt kan als voorbeelddocument niet gekopieerd worden. Probeer eventueel manueel",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                CopyFile(src, LOCATION_COMPANYDATA, "demo-compagny-logo.bmp");
                // retry
                bestand = LOCATION_COMPANYDATA + @"vpeSjbs\pdfDDEF" + typeKey + ".Txt";
            }

            System.Diagnostics.Process.Start("notepad.exe", "\"" + bestand + "\"");
        }

        // ═════════════════════════════════════════════════════════════════════
        // Menu – Taal  (checked-one-at-a-time, Tag holds 1-based index)
        // ═════════════════════════════════════════════════════════════════════

        private void MenuTaal_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item)) return;
            if (!int.TryParse(item.Tag?.ToString(), out int idx)) return;

            for (int t = 1; t <= 4; t++)
                if (_taalMenuItems[t] != null)
                    _taalMenuItems[t].Checked = false;

            item.Checked = true;
            _taalKode = idx;
        }

        // ═════════════════════════════════════════════════════════════════════
        // Menu – Document type (checked-one-at-a-time, Tag holds 0-based index)
        // ═════════════════════════════════════════════════════════════════════

        private void MenuDokType_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item)) return;
            if (!int.TryParse(item.Tag?.ToString(), out int idx)) return;

            foreach (var m in _dokMenuItems)
                m.Checked = false;

            item.Checked = true;
            _docuType = idx;
        }

        // ═════════════════════════════════════════════════════════════════════
        // Controls
        // ═════════════════════════════════════════════════════════════════════

        private void TxtDemoTekst_TextChanged(object sender, EventArgs e)
        {
            LblDemoTekst.Text = TxtDemoTekst.Text;
        }

        private void BtnKleurKiezen_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog())
            {
                dlg.AnyColor  = true;
                dlg.FullOpen  = true;
                dlg.Color     = LblDemoTekst.ForeColor;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LblDemoTekst.ForeColor = dlg.Color;
                    TxtKleur.Text          = ColorTranslator.ToOle(dlg.Color).ToString();
                }
            }
        }

        private void BtnFont_Click(object sender, EventArgs e)
        {
            using (var dlg = new FontDialog())
            {
                dlg.ShowEffects = true;
                dlg.ShowColor   = false;
                dlg.Font        = LblDemoTekst.Font;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LblDemoTekst.Font = dlg.Font;

                    string bold      = dlg.Font.Bold      ? "1" : "0";
                    string italic    = dlg.Font.Italic    ? "1" : "0";
                    string underline = dlg.Font.Underline ? "1" : "0";

                    TxtFont.Text =
                        dlg.Font.Size.ToString("0.##") + "," +
                        "\"" + dlg.Font.Name + "\"" + "," +
                        ColorTranslator.ToOle(LblDemoTekst.ForeColor).ToString() + "," +
                        bold + "," + italic + "," + underline;
                }
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // Inladen — opens and previews the VPE template file
        // ═════════════════════════════════════════════════════════════════════

        private void Inladen(string typeEnTaal)
        {
            string defFile = LOCATION_COMPANYDATA + @"vpeSjbs\pdfDDEF" + typeEnTaal + ".Txt";

            if (Mim.Report.IsOpen())
                Mim.Report.CloseDoc();

        VpeTest:
            if (!File.Exists(defFile))
            {
                System.Media.SystemSounds.Beep.Play();
                Mim.Report.CloseDoc();
                MessageBox.Show(
                    @"vpeSjbs\pdfDDEF" + typeEnTaal + ".Txt niet gevonden in de bedrijfsinhoudsopgave. " +
                    "Hierna wordt een voorbeelddocument vanuit de programmainhoudsopgave gekopieerd",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string src = AppDomain.CurrentDomain.BaseDirectory + @"VpeSjbs\";
                if (!CopyFile(src, LOCATION_COMPANYDATA + @"vpeSjbs\", "pdfDDEF" + typeEnTaal + ".Txt"))
                {
                    MessageBox.Show(src + "pdfDDEF" + typeEnTaal + ".Txt kan als voorbeelddocument niet gekopieerd worden. Probeer eventueel manueel",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                CopyFile(src, LOCATION_COMPANYDATA, "demo-compagny-logo.bmp");
                goto VpeTest;
            }

            Mim.Report.OpenDoc();
            Mim.Report.Author      = String99(46).Trim();
            Mim.Report.GUILanguage = GUILanguage.Dutch;
            Mim.Report.Title       = "marIntegraal Rapport";
            Mim.Report.nTopMargin  = 1;
            Mim.Report.nLeftMargin = 1;
            Mim.Report.nRightMargin  = 1;
            Mim.Report.GridVisible = true;

            PdfPrintUserDef(typeEnTaal, 0);

            Mim.Report.WriteDoc(PROGRAM_LOCATION + "marrapport.pdf");
            Mim.Report.Preview();
        }
    }
}

