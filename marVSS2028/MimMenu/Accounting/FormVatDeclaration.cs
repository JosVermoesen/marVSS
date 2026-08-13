using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using marVSS2028.Classes;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;
using static marVSS2028.Classes.PeppolTools;

namespace marVSS2028.MimMenu.Accounting
{
    public partial class FormVatDeclaration : Form
    {
        // ── Module-level fields (VB6 Dim) ──────────────────────────────────────
        
        private string _vatDeclarationTemplate = "";
        private string _vatGridLineTemplate    = "";
        private string _vatGridLinesList       = "";
        private string _thisVatGridLine        = "";

        private string _myVatNumber        = "";
        private object _periodeFromRecord;
        private object _yearFromRecord;
        private string _myRegistrationName = "";
        private string _myStreetName       = "";
        private string _myPostalZone       = "";
        private string _myCityName         = "";
        private string _myEmail            = "";
        private string _periodeType        = "";

        private string _jaar ;
        private string _periode;

        private string _previousAFFrom; // s002
        private string _previousVFFrom; // s012
        private string _previousANFrom; // s004
        private string _previousVNFrom; // s014

        // ── VAT-box label dictionaries (replace VB6 control arrays) ─────────
        // key = VAT box number (e.g. 54, 55 … 91)
        private readonly Dictionary<int, Label> _lblBEFVak = new Dictionary<int, Label>();
        private readonly Dictionary<int, Label> _lblEURVak = new Dictionary<int, Label>();
        private readonly Dictionary<int, Label> _lblEvak   = new Dictionary<int, Label>();
        // Special "XX" and "YY" labels
        private readonly Label[] _lblEURVakXX = new Label[2];
        private readonly Label[] _lblEURVakYY = new Label[2];
        private readonly Label[] _lblBEFVakXX = new Label[1];
        private readonly Label[] _lblBEFVakYY = new Label[1];

        // ── VAT box numbers used in both the 1999 and 2003 models ───────────
        private static readonly int[] VatBoxes = {
            0, 1, 2, 3, 45, 46, 47, 48, 49,
            54, 55, 56, 57, 59, 61, 62, 63, 64, 65, 66,
            71, 72,
            81, 82, 83, 84, 85, 86, 87, 88, 91
        };

        // Tag → VAT box mapping (same as VB6 lblBEFVak.Tag)
        private static readonly Dictionary<int, string> VatBoxTag = new Dictionary<int, string>
        {
            {  0, "#v055 #"}, {  1, "#v056 #"}, {  2, "#v057 #"}, {  3, "#v058 #"},
            { 45, "#v059 #"}, { 46, "#v060 #"}, { 47, "#v061 #"}, { 48, "#v062 #"}, { 49, "#v063 #"},
            { 54, "#v064 #"}, { 55, "#v042 #"}, { 56, "#v043 #"}, { 57, "#v044 #"}, { 59, "#v045 #"},
            { 61, ""    }, { 62, ""    }, { 63, "#v100 #"}, { 64, "#v101 #"}, { 65, ""    },
            { 66, ""    }, { 71, ""    }, { 72, ""    },
            { 81, "#v046 #"}, { 82, "#v047 #"}, { 83, "#v048 #"}, { 84, "#v050 #"},
            { 85, "#v051 #"}, { 86, "#v052 #"}, { 87, "#v053 #"}, { 88, "#v054 #"}, { 91, ""    }
        };

        public FormVatDeclaration()
        {
            InitializeComponent();
            WireHighlightEvents(this);
            BuildVatBoxLabels();
        }

        // ── Build label grids for both legacy tab pages ──────────────────────
        private void BuildVatBoxLabels()
        {
            // For each VAT box number we create three labels:
            //   BEF value, EUR value (from purchase/sales books), EUR from ledger accounts
            // All are added to tabPage1 (Model 1999) for display-only purposes.
            foreach (int box in VatBoxes)
            {
                var lblBEF = MakeValueLabel();
                var lblEUR = MakeValueLabel();
                var lblE   = MakeValueLabel();
                _lblBEFVak[box] = lblBEF;
                _lblEURVak[box] = lblEUR;
                _lblEvak[box]   = lblE;
                // tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] { lblBEF, lblEUR, lblE });
            }
            // XX/YY special totals
            for (int i = 0; i < 2; i++)
            {
                _lblEURVakXX[i] = MakeValueLabel();
                _lblEURVakYY[i] = MakeValueLabel();
                // tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] { _lblEURVakXX[i], _lblEURVakYY[i] });
            }
            _lblBEFVakXX[0] = MakeValueLabel();
            _lblBEFVakYY[0] = MakeValueLabel();
            // tabPage1.Controls.AddRange(new System.Windows.Forms.Control[] { _lblBEFVakXX[0], _lblBEFVakYY[0] });
        }

        private static Label MakeValueLabel()
        {
            return new Label
            {
                Text        = "",
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign   = ContentAlignment.MiddleRight,
                Size        = new Size(80, 17),
                Visible     = false   // hidden — values shown in richText/XML tabs
            };
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Form_Load
        // ═══════════════════════════════════════════════════════════════════════
        private void FormVatDeclaration_Load(object sender, EventArgs e)
        {            
            // Build TreeView root
            TvwBtwAangiftes.Nodes.Clear();
            var root = TvwBtwAangiftes.Nodes.Add("Btw aangiftes");

            // Read company info from setup (String99 slots same as VB6)
            _myVatNumber        = String99(51).Trim();
            _myRegistrationName = CheckforAmp(String99(46).Trim());
            _myStreetName       = CheckforAmp(String99(47).Trim());

            string adres = String99(48);
            if (adres.Length >= 5 && adres[4] == ' ')
            {
                _myPostalZone = adres.Substring(0, 4);
                _myCityName   = adres.Substring(5).Trim();
            }
            else
            {
                MessageBox.Show(
                    "Controleer setup en parameters voor postcode en plaatsnaam.\r\n\r\n" +
                    "Postcode uit 4 cijfers gevold door een spatie.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            _myEmail = String99(50).Trim();

            // Populate TreeView with existing VAT declaration records (DESC)
            var nodeTexts = new List<string>();
            BGetOrGreater(TABLE_VARIOUS, 1, VSet("17", 20));
            if (Ktrl == 0)
            {
                while (true)
                {
                    RecordToVeld(TABLE_VARIOUS);
                    long dokTot = 0;
                    for (int t = 92; t <= 98; t += 2)
                        dokTot += (long)SafeVal(VBibText(TABLE_VARIOUS, $"#v{t:D3} #"));
                    if (dokTot != 0)
                    {
                        string nodeText = VBibText(TABLE_VARIOUS, "#v090 #") + " " + VBibText(TABLE_VARIOUS, "#v091 #");
                        nodeTexts.Add(nodeText);
                    }
                    BNext(TABLE_VARIOUS);
                    if (Ktrl != 0 || !KEY_BUF[TABLE_VARIOUS].StartsWith("17"))
                        break;
                }
            }
            nodeTexts.Reverse();
            foreach (string nt in nodeTexts)
                root.Nodes.Add(nt);

            root.Expand();

            SnelHelpPrint(DateTime.Now + ":In verdere ontwikkeling voor BEF/EUR/E-mail", BL_LOGGING);

            GetByperdat(out string bookyearStart, out string periodInBookYear);
            // VulDeVelden(bookyearStart, periodInBookYear);

            // Visually select the loaded declaration in the treeview
            string targetText = bookyearStart + " " + periodInBookYear;
            foreach (TreeNode child in root.Nodes)
            {
                if (child.Text.Trim() == targetText.Trim())
                {
                    TvwBtwAangiftes.SelectedNode = child;
                    child.EnsureVisible();
                    break;
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Form_Closed → bClose TABLE_VARIOUS
        // ═══════════════════════════════════════════════════════════════════════
        private void FormVatDeclaration_FormClosed(object sender, FormClosedEventArgs e)
        {
            BClose(TABLE_VARIOUS);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // Helper: get year/period from FormBYPERDAT
        // ═══════════════════════════════════════════════════════════════════════
        private static void GetByperdat(out string jaar, out string periode)
        {
            jaar    = DateTime.Now.Year.ToString();
            periode = "01";
            foreach (Form f in Application.OpenForms)
            {
                if (f is FormBYPERDAT byp)
                {
                    jaar    = byp.CmbBoekjaar.Text;
                    periode = (byp.CmbPeriodeBoekjaar.SelectedIndex + 1).ToString("D2");
                    break;
                }
            }
        }
        
        // ═══════════════════════════════════════════════════════════════════════
        // Buttons
        // ═══════════════════════════════════════════════════════════════════════
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void BtnInitialiseren_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "BTW Aangifte periode initializeren.\nBent U zeker ?",
                "Alle boeken opnieuw uitdrukken !?!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            if (lblDoc0.Text != "00000 - 00000")
                SS99(_previousAFFrom, 2); // s002

            if (lblDoc2.Text != "00000 - 00000")
                SS99(_previousVFFrom, 12); // s012

            if (lblDoc1.Text != "00000 - 00000")
                SS99(_previousANFrom, 4); // s004
            if (lblDoc3.Text != "00000 - 00000")
                SS99(_previousVNFrom, 14); // s014
                        
            // Ensure target period record is removed if exists
            string periodeSleutel = "17" + _jaar + _periode;
            BGet(TABLE_VARIOUS, 1, VSet(periodeSleutel, 20));
            if (Ktrl == 0)
            {
                RecordToVeld(TABLE_VARIOUS);

                BDelete(TABLE_VARIOUS);
            }           
            Close();
        }

        // ── Intervat 2025 email/send ─────────────────────────────────────────
        private void BtnIntervat2025_Click(object sender, EventArgs e)
        {
            string vatFileName = _myVatNumber + " " + _yearFromRecord?.ToString()?.Trim()
                + Dec(Convert.ToDouble(_periodeFromRecord), "00") + ".xml";

            var result = MessageBox.Show(
                "Btwaangifte doormailen voor afhandeling\r\n\r\n" +
                "Kies 'ja' voor doormailen (aanbevolen, ontvangstbewijs volgt), " +
                "'nee' indien U zelf het XML bestand afhandelt",
                "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);

            switch (result)
            {
                case DialogResult.Cancel:
                    if (MessageBox.Show("Taak verlaten zonder enige verwerking. Bent U zeker",
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                        BtnIntervat2025_Click(sender, e);
                    break;

                case DialogResult.Yes:
                    SendByMail2025(vatFileName);
                    break;

                case DialogResult.No:
                    MessageBox.Show(
                        "Klik het INTERVAT tabblad en bewaar het XML bestand (bvb. op uw bureaublad). " +
                        "Start de INTERVAT webapplicatie en bezorg het XML bestand of breng uw cijfers " +
                        "manueel in in dezelfde toepassing\r\n\r\n" +
                        "Voor hulp rond INTERVAT gelieve de website FOD te raadplegen.",
                        "XML Btwaangifte zelf afhandelen via INTERVAT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void SendByMail2025(string vatFileName)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string xml = richTextBox2.Text;
                ScrMaakTekstBestand(xml, vatFileName);

                string subject = "Verzoek tot BTW controle en aangifte";
                string body = tbMailBtw.Text == "info@rv.be"
                    ? $"Formaat:XML bestand\r\nIn bijlage onze aangifte aangemaakt met marIntegraal versie {MAR_VERSION} voor controle en verzending. Graag ontvangstbewijs binnen de 24 uur via mail of onze DNN postbus\r\n\r\n{DateTime.Now}"
                    : $"Formaat:XML bestand\r\nIn bijlage XML btw aangifte gegenereerd door ons boekhoudpakket. Graag de aangifte door uw diensten na de gebruikelijke controles a.u.b.\r\nBezorgt U ons tevens nog ontvangstbevestiging ?\r\n\r\nDank bij voorbaat!\r\n\r\n{DateTime.Now}";

                string mailto = $"mailto:{tbMailBtw.Text}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";
                Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });

                MessageBox.Show(
                    "Zorg ervoor dat uw mailtoepassing effectief kan verzenden nu of straks. " +
                    "U ontvangt later nog ontvangstbevestiging vanwege onze diensten",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij versturen: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        // ── XML 2025 save to file ────────────────────────────────────────────
        private void BtnXml2025_Click(object sender, EventArgs e)
        {
            string vatFileName = _myVatNumber + " " + _yearFromRecord?.ToString()?.Trim()
                + Dec(Convert.ToDouble(_periodeFromRecord), "00") + ".xml";

            using (var dlg = new SaveFileDialog { FileName = vatFileName, Filter = "XML files|*.xml|All files|*.*" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                ScrMaakTekstBestand(richTextBox2.Text, dlg.FileName);
            }
        }

        // ── XML 2008 save to file ────────────────────────────────────────────
        private void BtnCommand1_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog { FileName = "btw.xml", Filter = "XML files|*.xml|All files|*.*" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                ScrMaakTekstBestand(richTextBox1.Text, dlg.FileName);
            }
        }

        // ── Checkbox: vergrendel RTF 2008 ───────────────────────────────────
        private void CbVergrendel_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = cbVergrendel.Checked;
        }

        // ── Checkbox: aanvraag betaalformulieren (2008) ──────────────────────
        private void CbAanvraagBetaalformulieren_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAskTag(richTextBox1,
                cbAanvraagTerugbetaling.Checked ? "YES" : "NO",
                cbAanvraagBetaalformulieren.Checked,
                isPayment: true);
        }

        // ── Checkbox: aanvraag terugbetaling (2008) ──────────────────────────
        private void CbAanvraagTerugbetaling_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAskTag(richTextBox1,
                cbAanvraagBetaalformulieren.Checked ? "YES" : "NO",
                cbAanvraagTerugbetaling.Checked,
                isPayment: false);
        }

        private static void UpdateAskTag(RichTextBox rtb, string otherValue, bool thisNewState, bool isPayment)
        {
            string oldAB = isPayment ? (thisNewState ? "NO" : "YES") : otherValue;
            string newAB = isPayment ? (thisNewState ? "YES" : "NO") : otherValue;
            string oldAT = isPayment ? otherValue : (thisNewState ? "NO" : "YES");
            string newAT = isPayment ? otherValue : (thisNewState ? "YES" : "NO");

            string oldTag = $"<ASK PAYMENT=\"{oldAB}\" RESTITUTION=\"{oldAT}\"/>";
            string newTag = $"<ASK PAYMENT=\"{newAB}\" RESTITUTION=\"{newAT}\"/>";
            rtb.Text = rtb.Text.Replace(oldTag, newTag);
        }

        // ── Checkbox: payment 2025 ───────────────────────────────────────────
        private void CbPayment2025_CheckedChanged(object sender, EventArgs e)
        {
            ReplaceXmlAttribute(richTextBox2, "Payment", cbPayment2025.Checked);
        }

        // ── Checkbox: restitution 2025 ───────────────────────────────────────
        private void CbRestitution2025_CheckedChanged(object sender, EventArgs e)
        {
            ReplaceXmlAttribute(richTextBox2, "Restitution", cbRestitution2025.Checked);
        }

        private static void ReplaceXmlAttribute(RichTextBox rtb, string attr, bool newValue)
        {
            string q = "\"";
            string search  = $"{attr}={q}{(newValue ? "NO"  : "YES")}{q}";
            string replace = $"{attr}={q}{(newValue ? "YES" : "NO" )}{q}";
            rtb.Text = rtb.Text.Replace(search, replace);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // TreeView node click
        // ═══════════════════════════════════════════════════════════════════════
        private void TvwBtwAangiftes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level > 0)   // not the root node
            {
                btnInitialiseren.Enabled = e.Node.Index == 0;
                string nodeText = e.Node.Text;
                _jaar    = nodeText.Length >= 4 ? nodeText.Substring(0, 4) : "";
                _periode = nodeText.Length >= 7 ? nodeText.Substring(5, 2) : "01";
                VulDeVelden(_jaar, _periode);
            }
        }
        
        // ═══════════════════════════════════════════════════════════════════════
        // VulDeVelden — populate all fields from the TABLE_VARIOUS record
        // ═══════════════════════════════════════════════════════════════════════
        private void VulDeVelden(string strStartBookYear, string strPeriodInBookYear)
        {
            BGet(TABLE_VARIOUS, 1, VSet("17" + strStartBookYear + strPeriodInBookYear, 20));
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                tabPage2.Enabled = false;
                return;
            }
            RecordToVeld(TABLE_VARIOUS);

            string year = VBibText(TABLE_VARIOUS, "#i002 #");
            if (!int.TryParse(year, out int yearNum) || yearNum <= 2025)
            {
                // Disable btninitialiseren for years before 2026
                btnInitialiseren.Enabled = false;
            }

            txtPeriodeNr.Text  = strPeriodInBookYear;

            _previousAFFrom = Math.Max(0, (int.TryParse(VBibText(TABLE_VARIOUS, "#v092 #"), out int afFrom) ? afFrom : 0) - 1).ToString();
            _previousVFFrom = Math.Max(0, (int.TryParse(VBibText(TABLE_VARIOUS, "#v096 #"), out int vfFrom) ? vfFrom : 0) - 1).ToString();
            _previousANFrom = Math.Max(0, (int.TryParse(VBibText(TABLE_VARIOUS, "#v094 #"), out int anFrom) ? anFrom : 0) - 1).ToString();
            _previousVNFrom = Math.Max(0, (int.TryParse(VBibText(TABLE_VARIOUS, "#v098 #"), out int vnFrom) ? vnFrom : 0) - 1).ToString();

            lblDoc0.Text = FormatDocRange(VBibText(TABLE_VARIOUS, "#v092 #"), VBibText(TABLE_VARIOUS, "#v093 #"));
            lblDoc2.Text = FormatDocRange(VBibText(TABLE_VARIOUS, "#v096 #"), VBibText(TABLE_VARIOUS, "#v097 #"));
            lblDoc1.Text = FormatDocRange(VBibText(TABLE_VARIOUS, "#v094 #"), VBibText(TABLE_VARIOUS, "#v095 #"));
            lblDoc3.Text = FormatDocRange(VBibText(TABLE_VARIOUS, "#v098 #"), VBibText(TABLE_VARIOUS, "#v099 #"));

            txtPeriodeTot.Text = VBibText(TABLE_VARIOUS, "#i001 #") + "/" + VBibText(TABLE_VARIOUS, "#i002 #");

            // TODO check with real data if these are always the correct mappings (VB6: loop t=1 to 7 step 2, check #v(t+91) and then SS99 with pTrec(t))
            if (VBibText(TABLE_VARIOUS, "#i001 #") == "")
            {
                MessageBox.Show(
                    "Aan- en verkoopboeken voor Intervat nog af te drukken. " +
                    "Intervat tabblad blijft uitgeschakeld. " +
                    "Intervat enkel mogelijk met boeken uitgedrukt via versie 6.6.900 of hoger. " +
                    "TIP: initialiseer fiche en druk de boeken opnieuw uit met versie 900 of hoger",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabPage2.Enabled = false;
                tabPage3.Enabled = false;
                btnIntervat2025.Enabled = false;
                return;
            }

            tabPage2.Enabled = true;
            tabPage3.Enabled = true;
            btnIntervat2025.Enabled = true;

            if (!XmlGenerate()) { MessageBox.Show("Fout bij afhandelen xml bestand", "", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }

            PopulateIntervat2008();

            // Compute XX / YY totals
            decimal xx = TryDec(_lblEURVak[54]) + TryDec(_lblEURVak[55]) + TryDec(_lblEURVak[56])
                       + TryDec(_lblEURVak[57]) + TryDec(_lblEURVak[61]) + TryDec(_lblEURVak[63]);
            decimal yy = TryDec(_lblEURVak[59]) + TryDec(_lblEURVak[62]) + TryDec(_lblEURVak[64]);

            _lblEURVakXX[0].Text = xx.ToString("#,##0.00");
            _lblEURVakXX[1].Text = _lblEURVakXX[0].Text;
            _lblEURVakYY[0].Text = yy.ToString("#,##0.00");
            _lblEURVakYY[1].Text = _lblEURVakYY[0].Text;

            // Determine vak 71 or 72
            decimal diff = xx - yy;
            string tmpVatDecl = _vatDeclarationTemplate;
            if (diff < 0)
            {
                _lblEvak[71].Text = 0.ToString("#,##0.00");
                _lblEvak[72].Text = Math.Abs(diff).ToString("#,##0.00");
                decimal amt72 = Math.Abs(diff);
                _thisVatGridLine = _vatGridLineTemplate
                    .Replace("{amount}", amt72.ToString("0.00"))
                    .Replace("{gridnumber}", "72");
                _vatGridLinesList += _thisVatGridLine;
                tmpVatDecl = tmpVatDecl.Replace("<D71>0</D71>",
                    $"<D72>{(long)(amt72 * 100)}</D72>");
                // cbRestitution2025.Checked = true;
                cbAanvraagTerugbetaling.Checked = true;
            }
            else
            {
                _lblEvak[72].Text = 0.ToString("#,##0.00");
                _lblEvak[71].Text = diff.ToString("#,##0.00");
                _thisVatGridLine = _vatGridLineTemplate
                    .Replace("{amount}", diff.ToString("0.00"))
                    .Replace("{gridnumber}", "71");
                _vatGridLinesList += _thisVatGridLine;
                tmpVatDecl = tmpVatDecl.Replace("<D71>0</D71>",
                    $"<D71>{(long)(diff * 100)}</D71>");
            }

            richTextBox1.Text = _vatDeclarationTemplate;

            // Build 2025 XML
            tmpVatDecl = tmpVatDecl.Replace("<Vsoft>vatDeclareGridLinesList</Vsoft>", _vatGridLinesList);
            richTextBox2.Text = tmpVatDecl;

        }

        private static string FormatDocRange(string from, string to)
        {
            if (!double.TryParse(from, out double f)) f = 0;
            if (!double.TryParse(to,   out double t)) t = 0;
            return $"{f:00000} - {t:00000}";
        }

        private static decimal TryDec(Label lbl)
        {
            if (lbl == null) return 0m;
            string s = lbl.Text.Replace(",", "").Replace(" ", "");
            return decimal.TryParse(s, out decimal d) ? d : 0m;
        }

        // ── XmlGenerate ──────────────────────────────────────────────────────
        private bool XmlGenerate()
        {
            if (!ScrLeesBestandAlleTekst(out _vatDeclarationTemplate,
                PROGRAM_LOCATION + @"Content\xml-templates\vat\be-vatdeclare.txt"))
            {
                MessageBox.Show("Onverwachte situatie", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            _vatGridLineTemplate = "";
            _vatGridLinesList    = "";

            _vatDeclarationTemplate = _vatDeclarationTemplate
                .Replace("{vatNumberDeclarant}",  _myVatNumber)
                .Replace("{nameDeclarant}",        _myRegistrationName)
                .Replace("{streetDeclarant}",      _myStreetName)
                .Replace("{postalCodeDeclarant}",  _myPostalZone)
                .Replace("{cityDeclarant}",        _myCityName)
                .Replace("{emailDeclarant}",       _myEmail);

            _yearFromRecord   = VBibText(TABLE_VARIOUS, "#i002 #").Trim();
            _periodeFromRecord = VBibText(TABLE_VARIOUS, "#i001 #").Trim();

            string vatPeriodeSetup = String99(301);
            switch (vatPeriodeSetup)
            {
                case "2":
                    _periodeType = "Quarter";
                    if (double.TryParse(_periodeFromRecord?.ToString(), out double pQ))
                        _periodeFromRecord = ((int)(pQ / 3)).ToString();
                    break;
                case "1":
                    _periodeType = "Month";
                    break;
                default:
                    MessageBox.Show("Setup BTW instellen a.u.b.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    tabPage2.Enabled = false;
                    tabPage3.Enabled = false;
                    btnIntervat2025.Enabled = false;
                    return false;
            }

            string yearStr    = _yearFromRecord?.ToString() ?? "";
            string periodeStr = _periodeFromRecord?.ToString() ?? "";
            string refStr     = _myVatNumber + "-" + yearStr + Dec(SafeVal(periodeStr), "00");

            _vatDeclarationTemplate = _vatDeclarationTemplate
                .Replace("{periodeType}",        _periodeType)
                .Replace("{period}",             periodeStr)
                .Replace("{fiscalYear}",         yearStr)
                .Replace("{referenceDeclarant}", refStr);

            // Load grid line template
            if (!ScrLeesBestandAlleTekst(out _vatGridLineTemplate,
                PROGRAM_LOCATION + @"Content\xml-templates\vat\be-vatgridline.txt"))
            {
                MessageBox.Show("Onverwachte situatie", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        // ── PopulateIntervat2008 ─────────────────────────────────────────────
        private void PopulateIntervat2008()
        {
            if (!ScrLeesBestandAlleTekst(out string xmlHier, PROGRAM_LOCATION + @"Content\Deprecated\XMLbtwAangifte.txt"))
                xmlHier = "";

            // Reset vak 61 / 62
            SetVakLabel(_lblBEFVak, 61, "0");  SetVakLabel(_lblBEFVak, 62, "0");
            SetVakLabel(_lblEURVak, 61, "0.00"); SetVakLabel(_lblEURVak, 62, "0.00");
            SetVakLabel(_lblEvak,   61, "0.00"); SetVakLabel(_lblEvak,   62, "0.00");

            string isEur = VBibText(TABLE_VARIOUS, "#vEUR #");
            bool bEur    = isEur == "EUR";

            foreach (var kvp in VatBoxTag)
            {
                int box = kvp.Key;
                string tag = kvp.Value;
                if (string.IsNullOrEmpty(tag)) continue;

                double rawVal = SafeVal(VBibText(TABLE_VARIOUS, tag));
                double eurVal;
                double befVal;

                if (bEur)
                {
                    eurVal = rawVal;
                    befVal = rawVal * EURO;
                }
                else
                {
                    eurVal = rawVal / EURO;
                    befVal = rawVal;
                }

                SetVakLabel(_lblBEFVak, box, befVal.ToString("#,##0"));
                SetVakLabel(_lblEURVak, box, eurVal.ToString("#,##0.00"));
                SetVakLabel(_lblEvak,   box, eurVal.ToString("#,##0.00"));

                if (bEur && eurVal != 0)
                {
                    string dTag = $"<D{box}>0</D{box}>";
                    string dVal = $"<D{box}>{(long)(rawVal * 100)}</D{box}>";
                    xmlHier = xmlHier.Replace(dTag, dVal);

                    _thisVatGridLine = _vatGridLineTemplate
                        .Replace("{amount}",     Dec(rawVal, MASK_EUR).Trim())
                        .Replace("{gridnumber}", box.ToString());
                    _vatGridLinesList += _thisVatGridLine + "\r\n";
                }
            }

            // Replace company/period placeholders in 2008 XML
            string vatNum10 = FormatVatNumber10(_myVatNumber);
            string vatNum9  = vatNum10.Length >= 10 ? "0" + vatNum10.Substring(1, 3) + vatNum10.Substring(4, 3) + vatNum10.Substring(7, 3) : "";
            string vatRef9  = vatNum10.Length >= 10 ? vatNum10.Substring(1, 3) + vatNum10.Substring(4, 3) + vatNum10.Substring(7, 3) + "00000" : "";

            xmlHier = ReplaceAll(xmlHier, new Dictionary<string, string>
            {
                { "<VATNUMBER>9999999999</VATNUMBER>", $"<VATNUMBER>{vatNum9}</VATNUMBER>" },
                { "<VATNUMBER>0000000000</VATNUMBER>", $"<VATNUMBER>{vatNum9}</VATNUMBER>" },
                { "<SENDINGREFERENCE>99999999900000</SENDINGREFERENCE>", $"<SENDINGREFERENCE>{vatRef9}</SENDINGREFERENCE>" },
                { "<NAME>Contactpersoon</NAME>",  $"<NAME>{CheckforAmp(String99(52))}</NAME>" },
                { "<NAME>NaamBedrijf</NAME>",     $"<NAME>{CheckforAmp(String99(46))}</NAME>" },
                { "<ADDRESS>StraatContact</ADDRESS>",  $"<ADDRESS>{CheckforAmp(String99(47))}</ADDRESS>" },
                { "<ADDRESS>StraatBedrijf</ADDRESS>",  $"<ADDRESS>{CheckforAmp(String99(47))}</ADDRESS>" },
                { "<POSTCODE>0000</POSTCODE>",    $"<POSTCODE>{_myPostalZone}</POSTCODE>" },
                { "<POSTCODE>9999</POSTCODE>",    $"<POSTCODE>{_myPostalZone}</POSTCODE>" },
                { "<CITY>Plaatscontact</CITY>",   $"<CITY>{_myCityName}</CITY>" },
                { "<CITY>PlaatsBedrijf</CITY>",   $"<CITY>{_myCityName}</CITY>" },
            });

            string vatPeriodeSetup = String99(301);
            string monthOrQuarter  = VBibText(TABLE_VARIOUS, "#i001 #");
            string yearVal         = VBibText(TABLE_VARIOUS, "#i002 #").Trim();

            if (vatPeriodeSetup == "2")
            {
                int q = (int)(SafeVal(monthOrQuarter) / 3);
                xmlHier = xmlHier.Replace("<QUARTERORMONTH>0</QUARTERORMONTH>", $"<QUARTER>{q}</QUARTER>");
            }
            else if (vatPeriodeSetup == "1")
            {
                xmlHier = xmlHier.Replace("<QUARTERORMONTH>0</QUARTERORMONTH>",
                    $"<MONTH>{(int)SafeVal(monthOrQuarter)}</MONTH>");
            }
            else
            {
                MessageBox.Show("Setup BTW instellen a.u.b.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                tabPage2.Enabled = false; tabPage3.Enabled = false;
                btnIntervat2025.Enabled = false;
                return;
            }
            xmlHier = xmlHier.Replace("<YEAR>1985</YEAR>", $"<YEAR>{yearVal}</YEAR>");

            richTextBox1.Text = xmlHier;
        }

        private static void SetVakLabel(Dictionary<int, Label> dict, int box, string text)
        {
            if (dict.ContainsKey(box)) dict[box].Text = text;
        }

        private static string FormatVatNumber10(string vatNum)
        {
            // Ensure 10-digit format starting with 0 or 1
            vatNum = vatNum.Replace(".", "").Replace(" ", "").Replace("BE", "").Replace("be", "");
            return vatNum.Length == 10 ? vatNum : vatNum.PadLeft(10, '0');
        }

        private static string ReplaceAll(string src, Dictionary<string, string> replacements)
        {
            foreach (var kv in replacements)
                src = src.Replace(kv.Key, kv.Value);
            return src;
        }

        private static double SafeVal(string s)
        {
            return double.TryParse(s?.Trim(), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double d) ? d : 0.0;
        }        
    }
}

