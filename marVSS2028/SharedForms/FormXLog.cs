using ADODB;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;

using marVSS2028.PublicForms;

namespace marVSS2028.SharedForms
{
    public partial class FormXLog : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            X.StandardTab = true;
            if (X.Rows.Count > 0 && X.Columns.Count > 0)
                X.CurrentCell = X.Rows[0].Cells[0];
            X.Focus();
        }

        private string _bstPDFofTIF = string.Empty;
        private int _atLijn;
        private int _rowIdx = -1;

        private string _crText  = string.Empty;
        private string _crText2 = string.Empty;

        // Tracks "Selecteren mogelijk" menu toggle (VB6: Kopie(1).Checked)
        private bool _selectieActief;

        public FormXLog()
        {
            InitializeComponent();
        }
        
        private void FormXLog_Load(object sender, EventArgs e)
        {
            LoadFormProperties(this);
            foreach (DataGridViewColumn col in X.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            
        }

        private void FormXLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveFormProperties(this);
        }

        private void FormXLog_Resize(object sender, EventArgs e)
        {
            try
            {
                TabControl1.Top    = 0;
                TabControl1.Left   = 0;
                TabControl1.Width  = Width - 16;
                TabControl1.Height = Height - 78;

                X.Width  = TabControl1.Width  - 6;
                X.Height = TabControl1.Height - 28;

                int btnTop = Height - 66;
                BtnAfsluiten.Top       = btnTop;
                BtnAnnuleren.Top       = btnTop;
                BtnAfbeelding.Top      = btnTop;
                BtnWijzigenLijn.Top    = btnTop;
                BtnDetailJournaal.Top  = btnTop;
            }
            catch { }
        }
                
        private void BtnAfsluiten_Click(object sender, EventArgs e)
        {
            _rowIdx = X.CurrentCell?.RowIndex ?? -1;
            if (_rowIdx < 0)
            {
                if (Text.StartsWith("Schade", StringComparison.OrdinalIgnoreCase))
                    XLogKey = "Nieuw";
            }
            else
            {
                string col0 = X.Rows[_rowIdx].Cells.Count > 0
                    ? X.Rows[_rowIdx].Cells[0].Value?.ToString() ?? "" : "";
                string col1 = X.Rows[_rowIdx].Cells.Count > 1
                    ? X.Rows[_rowIdx].Cells[1].Value?.ToString() ?? "" : "";

                if (string.IsNullOrEmpty(col0))
                {
                    if (Text.StartsWith("Schade", StringComparison.OrdinalIgnoreCase))
                        XLogKey = "Nieuw";
                }
                else
                {
                    XLogKey = col0 + "\r\n" + col1;
                }
            }
            Hide();
        }
                
        private void BtnAnnuleren_Click(object sender, EventArgs e)
        {
            XLogKey     = string.Empty;
            GridText    = string.Empty;
            WindowState = FormWindowState.Normal;
            Hide();
        }
                
        private void BtnAfbeelding_Click(object sender, EventArgs e)
        {
            string msg = "Grafische afdruk van het venster !  Bent U zeker ?\r\n\r\n"
                       + "Kies 'Nee' voor gewone afdruk van alle ingevulde tekstlijnen.\r\n"
                       + "Hiermee voldoet U aan uw GPRD verplichtingen indien uw partij zijn/haar gegevens opvraagt";

            DialogResult choice = MessageBox.Show(msg, string.Empty,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);

            if (choice == DialogResult.Yes)
            {
                try { PrintFormAsBitmap(); } catch { }
            }
            else if (choice == DialogResult.No)
            {
                PrintTextLines();
            }
        }
                
        private void BtnWijzigenLijn_Click(object sender, EventArgs e)
        {
            try
            {
                _rowIdx = X.CurrentCell?.RowIndex ?? -1;
                if (_rowIdx < 0) return;

                string telebibCode = _rowIdx < TELEBIB_CODE.Length ? TELEBIB_CODE[_rowIdx] ?? "" : "";
                string telebibText = _rowIdx < TELEBIB_TEXT.Length ? TELEBIB_TEXT[_rowIdx] ?? "" : "";

                if (SafeMid(telebibCode, 2, 2) != "  " && SafeMid(telebibCode, 1, 1) != "@")
                {
                    // VB6: X_KeyDown 17, 0
                    X_KeyDown_Ctrl(_rowIdx);
                    // MoveToNextRow();
                    X.Focus();
                }
                else
                {
                    string editMsg = string.Empty;
                    string pos10   = SafeMid(telebibCode, 10, 1);

                    if (pos10 == "-" || pos10 == "x")
                    {
                        editMsg  = "Deze informatie kan niet gewijzigd worden...";
                        GridText = "Edit No";
                    }
                    else
                    {
                        GridText = "Edit Yes";
                    }

                    if (SafeMid(telebibCode, 1, 1) == "@")
                        editMsg = SafeMid(telebibCode, 1, 3);
                    else
                        editMsg = editMsg + QuickHelp(telebibCode.Length >= 3 ? telebibCode.Substring(0, 3) : telebibCode);

                    int col2 = Math.Min(2, X.Columns.Count - 1);
                    string currentValue = X.Rows[_rowIdx].Cells[col2].Value?.ToString() ?? "";
                    _atLijn = 0;
                    int.TryParse(pos10, out _atLijn);

                    if (pos10 == "x")
                    {
                        int flIdx = 0;
                        var flIdxMatch = System.Text.RegularExpressions.Regex.Match(currentValue, @"\d+");
                        if (flIdxMatch.Success) int.TryParse(flIdxMatch.Value, out flIdx);
                        string fieldName = SafeMid(telebibCode, 5, 4);
                        try
                        {
                            object fieldVal = rsMAR[flIdx]?.Fields[fieldName]?.Value;
                            if (fieldVal != null && fieldVal != DBNull.Value)
                            {
                                var dlg = new FormReactionsDialog();
                                dlg.TextBoxReactions.Text = fieldVal.ToString();
                                dlg.Text = flIdx <= 2 ? "Ondersteunde documenten" : "Bevestigingen en reacties";
                                dlg.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Geen gegevens geregistreerd", string.Empty,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Geen gegevens geregistreerd", string.Empty,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        string newVal = VsfInputBox(editMsg, telebibText.TrimEnd(), currentValue, string.Empty);
                        X.Rows[_rowIdx].Cells[col2].Value = newVal;
                        // MoveToNextRow();
                        X.Focus();
                    }
                }
            }
            catch { }
        }
                
        private void BtnDetailJournaal_Click(object sender, EventArgs e)
        {
            int rowIdx = X.CurrentCell?.RowIndex ?? -1;
            if (rowIdx < 0) return;

            string deString = X.Rows[rowIdx].Cells[0].Value?.ToString() ?? "";

            BGet(TABLE_JOURNAL, 1, VSet(deString, 11));
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Geen journaallijnen voor " + deString);
                return;
            }

            // Build result rows
            var rows = new System.Collections.Generic.List<string[]>();
            Cursor = Cursors.WaitCursor;
            try
            {
                rows.Add(BuildJournaalRij());
                do
                {
                    BNext(TABLE_JOURNAL);
                    if (Ktrl != 0 || KEY_BUF[TABLE_JOURNAL] != deString)
                        break;
                    rows.Add(BuildJournaalRij());
                } while (true);
            }
            finally { Cursor = Cursors.Default; }

            // Show in a simple dialog with a DataGridView
            using (var dlg = new Form())
            {
                dlg.Text     = "Journaaldetail voor dokument: " + deString;
                dlg.Size     = new Size(900, 400);
                dlg.MinimizeBox = false;
                var grid = new DataGridView
                {
                    Dock            = DockStyle.Fill,
                    ReadOnly        = true,
                    AllowUserToAddRows    = false,
                    AllowUserToDeleteRows = false,
                    RowHeadersVisible     = false,
                    AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.AllCells
                };
                dlg.Controls.Add(grid);
                string[] headers = { "Datum #v066", "Rekening #v019", "Naam #v020", "Bedrag #v068",
                                     "Boekingsomschrijving #v067", "Fin. Stuk #v038", "TegenRek. #v069", "vsfRecord" };
                foreach (string h in headers)
                    grid.Columns.Add(h, h);
                foreach (string[] row in rows)
                    grid.Rows.Add(row);
                dlg.ShowDialog(this);
            }
        }
                
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowIdx = X.CurrentCell?.RowIndex ?? -1;
            string rowKey = rowIdx >= 0 ? X.Rows[rowIdx].Cells[0].Value?.ToString() ?? "" : "";

            // Update label/button state based on current _bstPDFofTIF before tab switch
            if (string.IsNullOrEmpty(_bstPDFofTIF))
            {
                LblLabel1.Text      = rowKey;
                BtnCommand1.Enabled = false;
                BtnCommand3.Enabled = false;
            }
            else
            {
                LblLabel1.Text  = rowKey;
                LblLabel2.Text  = _bstPDFofTIF;
                string ext = GetExtension(_bstPDFofTIF);
                BtnCommand1.Enabled = ext == "pdf";
                BtnCommand3.Enabled = ext != "pdf";
            }

            switch (TabControl1.SelectedIndex)
            {
                case 0:
                    _bstPDFofTIF = string.Empty;
                    TabControl1.TabPages[1].Text = "- Geen Bijlage";
                    LblLabel1.Text = "- leeg -";
                    LblLabel2.Text = string.Empty;
                    BtnAfbeelding.Font = new Font(BtnAfbeelding.Font, FontStyle.Regular);
                    break;

                case 1:
                    if (TabControl1.TabPages[1].Text.StartsWith("+"))
                    {
                        try
                        {
                            string strType = rsMAR[SharedScanFl].Fields["typeZending37"].Value?.ToString() ?? "";
                            _bstPDFofTIF   = LaadTekst("dnnInstellingen", "Mario") + "\\ioDocument." + strType;
                            LblLabel2.Text = _bstPDFofTIF;
                            Cursor = Cursors.WaitCursor;
                            if (File.Exists(_bstPDFofTIF)) File.Delete(_bstPDFofTIF);
                            BlobToFile(rsMAR[SharedScanFl].Fields["bstBLOB37"], _bstPDFofTIF);
                            Application.DoEvents();
                            Cursor = Cursors.Default;

                            BtnCommand1.Text    = "Pdf Wijzigen"; BtnCommand1.Enabled = true;
                            BtnCommand2.Enabled = false;
                            BtnCommand3.Text    = "Tif Wijzigen"; BtnCommand3.Enabled = true;
                            BtnCommand4.Enabled = false;
                            LblLabel3.Visible   = true;
                        }
                        catch { Cursor = Cursors.Default; }
                    }
                    else
                    {
                        BtnCommand1.Text    = "Pdf Openen"; BtnCommand1.Enabled = true;
                        BtnCommand2.Enabled = false;
                        BtnCommand3.Text    = "Tif Openen"; BtnCommand3.Enabled = true;
                        BtnCommand4.Enabled = false;
                        LblLabel3.Visible   = false;
                    }
                    break;
            }
        }
                
        private void BtnCommand1_Click(object sender, EventArgs e)
        {
            BtnCommand3.Enabled = false;
            BtnCommand4.Enabled = false;
            BtnCommand2.Enabled = false;

            if (BtnCommand1.Text == "Pdf Openen")
            {
                string initDir = LaadTekst("dnnInstellingen", "Mario");
                if (string.IsNullOrEmpty(initDir))
                {
                    MessageBox.Show(
                        "Nieuwe PC of nog geen instellingen voor PDF Postvak In.  " +
                        "Aanbevolen in te stellen a.u.b. via submenu DotNetNuke.",
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    initDir = LOCATION_COMPANYDATA;
                }

                using (var dlg = new OpenFileDialog { InitialDirectory = initDir,
                                                      Filter = "Acrobat PDF bestand (*.pdf)|*.pdf",
                                                      FileName = string.Empty })
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    _bstPDFofTIF = dlg.FileName;
                }

                if (!_bstPDFofTIF.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    MessageBox.Show("Uitsluitend Pdf bestanden selecteren a.u.b.", string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                LblLabel2.Text      = _bstPDFofTIF;
                BtnCommand2.Enabled = true;
            }
            else if (BtnCommand1.Text == "Pdf Wijzigen")
            {
                Application.DoEvents();
                BtnCommand2_Click(sender, e);
            }
            else if (BtnCommand1.Text == "Pdf Vernieuwen")
            {
                BtnCommand2.Enabled = true;
                BtnCommand1.Text    = "Pdf Wijzigen";
            }
        }
                
        private void BtnCommand2_Click(object sender, EventArgs e)
        {
            try
            {
                FileToBlob(rsMAR[SharedScanFl].Fields["bstBLOB37"], _bstPDFofTIF);
                rsMAR[SharedScanFl].Fields["bstndNaam37"].Value  = _bstPDFofTIF;
                rsMAR[SharedScanFl].Fields["typeZending37"].Value = GetExtension(_bstPDFofTIF);
                rsMAR[SharedScanFl].Update();
                TabControl1.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
                
        private void BtnCommand3_Click(object sender, EventArgs e)
        {
            BtnCommand1.Enabled = false;
            BtnCommand2.Enabled = false;
        }
                
        private void X_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_selectieActief || e.RowIndex < 0) return;
            X.ClearSelection();
            X.Rows[e.RowIndex].Selected = true;
        }

        private void X_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (BtnAfsluiten.TabStop)
                BtnWijzigenLijn_Click(sender, e);
            else
                BtnAfsluiten_Click(sender, e);
        }

        private void X_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnWijzigenLijn_Click(sender, e);
                return;
            }
            if (e.KeyCode != Keys.ControlKey) return;
            _rowIdx = X.CurrentCell?.RowIndex ?? -1;
            if (_rowIdx >= 0) X_KeyDown_Ctrl(_rowIdx);
            e.Handled = true;
        }

        /// <summary>VB6: X_KeyDown with KeyCode=17 — Ctrl lookup logic.</summary>
        private void X_KeyDown_Ctrl(int rowIdx)
        {
            if (rowIdx < 0 || rowIdx >= TELEBIB_CODE.Length) return;
            string telebibCode = TELEBIB_CODE[rowIdx] ?? "";

            // TODO: continue Text.StartsWith("Log" or Text.StartsWith("Set"

            if (!Text.StartsWith("Log", StringComparison.OrdinalIgnoreCase) &&
                !Text.StartsWith("Set", StringComparison.OrdinalIgnoreCase)) return;

            int lastCol = X.Columns.Count - 1;
            int col2 = Math.Min(2, lastCol);
            string dummyText = (X.Rows[rowIdx].Cells[lastCol].Value?.ToString() ?? "").TrimEnd();

            switch (SafeMid(telebibCode, 2, 2))
            {
                case "K ":
                case "L ":
                case "LC":
                case "R ":
                case "R3":
                case "R4":
                case "R6":
                case "R7":
                    aIndex = 0;
                    switch (SafeMid(telebibCode, 2, 1))
                    {
                        case "K": SharedFl = TABLE_CUSTOMERS; break;
                        case "L": SharedFl = TABLE_SUPPLIERS; break;
                        case "R": SharedFl = TABLE_LEDGERACCOUNTS; break;
                        default: MessageBox.Show("nog niks"); return;
                    }
                    GridText = string.Empty;
                    if (SafeMid(telebibCode, 3, 2) != "  ")
                    {
                        if (!string.IsNullOrEmpty(dummyText))
                            GridText = SharedFl == TABLE_SUPPLIERS && SafeMid(telebibCode, 3, 2) == "CO"
                                ? "CO" + dummyText : dummyText;
                        else
                            GridText = SafeMid(telebibCode, 3, 2) + "@Beperk@";
                    }
                    else
                    {
                        GridText = dummyText;
                    }
                    // Open FormSearchSQL for indexed field lookup
                    using (var sqlSearch = new marVSS2028.PublicForms.FormSearchSQL())
                        sqlSearch.ShowDialog(this);
                    if (Ktrl == 0)
                    {
                        string val = SharedFl == TABLE_SUPPLIERS && SafeMid(telebibCode, 3, 2) == "CO"
                            ? (FVT[SharedFl, 0].Length > 2 ? FVT[SharedFl, 0].Substring(2).TrimEnd() : "")
                            : FVT[SharedFl, 0];
                        X.Rows[rowIdx].Cells[lastCol].Value = val;
                    }
                    break;

                case "  ":
                    break;

                default:
                    int boxType = telebibCode.Length > 0 && telebibCode[0] >= '0' && telebibCode[0] <= '9' ? 1 : 0;
                    int.TryParse(SafeMid(telebibCode, 1, 3), out aIndex);
                    if (boxType == 1) aIndex += 1000;

                    string currentVal = X.Rows[rowIdx].Cells[col2].Value?.ToString() ?? "";
                    GridText = currentVal;
                    using (var keuzeVSF = new FormKeuzeVSF())
                        keuzeVSF.ShowDialog(this);
                    if (GridText != currentVal)
                        X.Rows[rowIdx].Cells[col2].Value = GridText;
                    break;
            }
        }

        private void X_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '!' || e.KeyChar > '~') return;
            _rowIdx = X.CurrentCell?.RowIndex ?? -1;
            if (_rowIdx < 0) return;
            int col2 = Math.Min(2, X.Columns.Count - 1);
            string clip = X.Rows[_rowIdx].Cells[col2].Value?.ToString() ?? "";
            if (col2 == 2 && SafeMid(clip, 2, 2) == "  ")
            {
                X.Rows[_rowIdx].Cells[col2].Value = e.KeyChar + clip;
                BtnWijzigenLijn_Click(sender, e);
            }
        }

        // VB6: X_KeyUp → X_Click
        private void X_KeyUp(object sender, KeyEventArgs e)
        {
            if (X.CurrentCell == null) return;
            X_CellClick(sender, new DataGridViewCellEventArgs(
                X.CurrentCell.ColumnIndex,
                X.CurrentCell.RowIndex));
        }

        /// <summary>VB6: X_RowColChange — updates attachment tab caption.</summary>
        private void X_SelectionChanged(object sender, EventArgs e)
        {
            TabControl1.TabPages[1].Text = "- Geen Bijlage";
            _rowIdx = X.CurrentCell?.RowIndex ?? -1;
            if (SharedScanFl == 0 || _rowIdx < 0) return;

            string keyVal = X.Rows[_rowIdx].Cells[0].Value?.ToString()?.Trim() ?? "";
            if (string.IsNullOrEmpty(keyVal)) return;

            bool found = ADO_GET(SharedScanFl, 0, "=", keyVal);
            if (!found) return;

            try
            {
                LblLabel1.Text = keyVal;
                object blob = rsMAR[SharedScanFl].Fields["bstBLOB37"].Value;
                if (blob == null || blob == DBNull.Value)
                    LblLabel1.Text = "- leeg -";
                else
                    TabControl1.TabPages[1].Text = "+ Met Bijlage";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // ── Menu (Bewerken / Kopie) ────────────────────────────────────────────

        private void MenuKopieren_Click(object sender, EventArgs e)        => KopieHandler(0);
        private void MenuSelectie_Click(object sender, EventArgs e)        => KopieHandler(1);
        private void MenuGrafischAfdruk_Click(object sender, EventArgs e)  => KopieHandler(2);
        private void MenuHPPrint_Click(object sender, EventArgs e)         => KopieHandler(4);
        private void MenuIBMPrint_Click(object sender, EventArgs e)        => KopieHandler(5);
        private void MenuPuurTekst_Click(object sender, EventArgs e)       => KopieHandler(6);
        private void MenuBewaarAls_Click(object sender, EventArgs e)       => KopieHandler(8);
        private void MenuStandaardGrootte_Click(object sender, EventArgs e) => KopieHandler(10);

        private void KopieHandler(int index)
        {
            switch (index)
            {
                case 0:
                    string clip = GetGridClipText();
                    if (string.IsNullOrEmpty(clip))
                    {
                        MessageBox.Show("Eerst selecteren a.u.b. !");
                        return;
                    }
                    try
                    {
                        Clipboard.Clear();
                        Clipboard.SetText(clip);
                    }
                    catch
                    {
                        MessageBox.Show("Kopiëren naar het klembord was onvolledig.  " +
                            "Max. 64000 tekens aan tekst kan geknipt worden.  " +
                            "Uw toestel beschikt mogelijk over onvoldoende geheugen.  " +
                            "Sluit overbodige toepassingen en probeer eventueel opnieuw.");
                    }
                    break;

                case 1:
                    _selectieActief      = !_selectieActief;
                    MenuSelectie.Checked = _selectieActief;
                    break;

                case 2:
                    try { PrintFormAsBitmap(); } catch { }
                    break;

                case 4:
                    PrintToFile(useDefFile: PROGRAM_LOCATION + @"Content\Def\hpj.def", puurTekst: false);
                    break;

                case 5:
                    PrintToFile(useDefFile: PROGRAM_LOCATION + @"Content\Def\ibm.def", puurTekst: false);
                    break;

                case 6:
                    PrintToFile(useDefFile: null, puurTekst: true);
                    break;

                case 8:
                    BewaarAls();
                    break;

                case 10:
                    WindowState = FormWindowState.Normal;
                    Height = 405;   // VB6: 6465 twips
                    Width  = 585;   // VB6: 9315 twips
                    break;
            }
        }

        // ── Private helpers ────────────────────────────────────────────────────

        /// <summary>VB6: BewaarAls — save grid content to a delimited text file.</summary>
        private void BewaarAls()
        {
            string lijstSep;
            try
            {
                lijstSep = LaadTekst("c:\\windows\\win.ini;intl", "sList");
                if (string.IsNullOrEmpty(lijstSep)) lijstSep = ";";
            }
            catch { lijstSep = ";"; }

            Cursor = Cursors.WaitCursor;
            try
            {
                string fileName;
                using (var dlg = new SaveFileDialog { Filter = "Alle bestanden (*.*)|*.*", FileName = string.Empty })
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    fileName = dlg.FileName;
                }

                using (var sw = new StreamWriter(fileName, false, Encoding.Default))
                {
                    for (int r = 0; r < X.Rows.Count; r++)
                    {
                        var sb = new StringBuilder();
                        for (int c = 0; c < X.Columns.Count; c++)
                        {
                            if (c > 0) sb.Append(lijstSep);
                            sb.Append(X.Rows[r].Cells[c].Value?.ToString() ?? "");
                        }
                        sw.WriteLine(sb.ToString());
                    }
                }
            }
            catch { }
            finally { Cursor = Cursors.Default; }
        }

        /// <summary>VB6: PrintForm — renders the form window to the default printer.</summary>
        private void PrintFormAsBitmap()
        {
            var bmp = new Bitmap(Width, Height);
            DrawToBitmap(bmp, new Rectangle(0, 0, Width, Height));
            var pd = new PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                ev.Graphics.DrawImage(bmp, 0, 0);
                ev.HasMorePages = false;
            };
            pd.Print();
        }

        /// <summary>VB6: StelLijstSamen / PrintDeTITEL GoSub blocks.</summary>
        private void PrintToFile(string useDefFile, bool puurTekst)
        {
            string linesStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Aantal lijnen per blad", "Paginalengte", "72");
            if (string.IsNullOrEmpty(linesStr)) return;
            int linesPerPage = int.TryParse(linesStr, out int lp) ? lp : 72;

            string printFile = "printbst.txt";
            try
            {
                if (File.Exists(printFile)) File.Delete(printFile);
                if (!string.IsNullOrEmpty(useDefFile) && File.Exists(useDefFile))
                    File.Copy(useDefFile, printFile);

                int pageCounter = 0;
                int countTo     = 0;

                using (var sw = new StreamWriter(printFile, true, Encoding.Default))
                {
                    Action printTitle = () =>
                    {
                        pageCounter++;
                        countTo = 12;
                        string mimCaption = Application.OpenForms["FormMim"]?.Text ?? "";
                        int bi = mimCaption.IndexOf('[');
                        string mimPart = bi >= 0 ? mimCaption.Substring(bi) : mimCaption;
                        sw.WriteLine(mimPart + " DosBoxPrint\t\t\t\t\t\t\t\t\t\t\t\tPagina : " + pageCounter);
                        sw.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tDatum  : " + MIM_GLOBAL_DATE + "\r\n");
                        sw.WriteLine(Text);
                        sw.WriteLine(new string('-', 128));
                        // header row
                        for (int c = 0; c < X.Columns.Count; c++)
                            sw.Write(X.Columns[c].HeaderText.PadRight(REPORT_TAB[c]));
                        sw.WriteLine("\r\n" + new string('-', 128) + "\r\n");
                    };

                    printTitle();

                    for (int r = 0; r < X.Rows.Count; r++)
                    {
                        for (int c = 0; c < X.Columns.Count; c++)
                            sw.Write((X.Rows[r].Cells[c].Value?.ToString() ?? "").PadRight(REPORT_TAB[c]));
                        countTo++;
                        sw.WriteLine();

                        if (countTo >= linesPerPage)
                        {
                            if (!puurTekst) sw.Write((char)12);
                            printTitle();
                        }
                    }
                }

                System.Diagnostics.Process.Start("notepad.exe", "\"" + printFile + "\"");
            }
            catch { }
        }

        /// <summary>VB6: PrintTextLines — printer output of grid rows col1 + col2.</summary>
        private void PrintTextLines()
        {
            try
            {
                var pd = new PrintDocument();
                pd.PrinterSettings.PrinterName =
                    System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > LijstPrinterNr
                    ? System.Drawing.Printing.PrinterSettings.InstalledPrinters[LijstPrinterNr]
                    : pd.PrinterSettings.PrinterName;

                var fontNormal = new Font("Courier New", 12, FontStyle.Regular);
                var fontBold   = new Font("Courier New", 12, FontStyle.Bold);
                int rowIndex   = 0;
                bool firstPage = true;

                pd.PrintPage += (s, e) =>
                {
                    float y  = e.MarginBounds.Top;
                    float x1 = e.MarginBounds.Left;
                    float x2 = x1 + 300;

                    if (firstPage)
                    {
                        firstPage = false;
                        string mimCaption = Application.OpenForms["FormMim"]?.Text ?? "";
                        int bi = mimCaption.IndexOf('[');
                        string mimPart = bi >= 0 ? mimCaption.Substring(bi).ToUpper() : "";
                        e.Graphics.DrawString(mimPart, fontBold, Brushes.Black, x1, y); y += fontBold.Height * 2;
                        e.Graphics.DrawString("marINTEGRAAL NT : " + Text, fontBold, Brushes.Black, x1, y); y += fontBold.Height;
                        e.Graphics.DrawString(FULL_LINE.Substring(0, Math.Min(80, FULL_LINE.Length)), fontNormal, Brushes.Black, x1, y); y += fontNormal.Height;
                    }

                    while (rowIndex < X.Rows.Count)
                    {
                        string col1 = X.Rows[rowIndex].Cells.Count > 1
                            ? X.Rows[rowIndex].Cells[1].Value?.ToString()?.TrimEnd() ?? "" : "";
                        string col2 = X.Rows[rowIndex].Cells.Count > 2
                            ? X.Rows[rowIndex].Cells[2].Value?.ToString()?.TrimEnd() ?? "" : "";
                        rowIndex++;

                        if (string.IsNullOrEmpty(col2)) continue;
                        e.Graphics.DrawString(col1, fontNormal, Brushes.Black, x1, y);
                        if (col2.Length > 40)
                        {
                            y += fontNormal.Height;
                            e.Graphics.DrawString(col2, fontBold, Brushes.Black, x1, y);
                        }
                        else
                        {
                            e.Graphics.DrawString(col2, fontBold, Brushes.Black, x2, y);
                        }
                        y += fontNormal.Height;
                        if (y > e.MarginBounds.Bottom) { e.HasMorePages = true; return; }
                    }

                    e.Graphics.DrawString(
                        "Datum en uur vandaag             : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        fontBold, Brushes.Black, x1, y + fontNormal.Height * 2);
                    e.HasMorePages = false;
                    fontNormal.Dispose();
                    fontBold.Dispose();
                };
                pd.Print();
            }
            catch { }
        }

        /// <summary>VB6: QuickHelp function.</summary>
        private string QuickHelp(string infoString)
        {
            if (string.IsNullOrEmpty(infoString)) return string.Empty;

            string result = string.Empty;
            switch (infoString[0])
            {
                case '1': result = "Naam of adres"; break;
                case '2': result = "Beschrijving, tekst of omschrijving"; break;
                case '3': result = "Een Bedrag in + of -"; break;
                case '4': result = "Een hoeveelheid (+)"; break;
                case '5': result = "Kode (1 of meer tekens)"; break;
                case '6': result = "Index (000.00)"; break;
                case '7': result = "Referentie"; break;
                case '8': result = "Percentage (max. 999)"; break;
                case '9': result = "Datum (DDMMEEJJ)"; break;
                case 'A': result = "Communicatiekanalen (telefoon, fax...)"; break;
                case 'B': result = "Financiële rekening (xxx-xxxxxxx-xx)"; break;
                case 'b': result = "Btw Nummer of Nationaal nummer (xxx.xxx.xxx)"; break;
                case 'c':
                case 'd': result = "Geldige bestandsnaam a.u.b"; break;
                case 'z': result = "Volledige datum als sleutel"; break;
            }

            if (infoString.Length >= 3 && infoString.Substring(1, 2) != "  ")
                SnelHelpPrint(" [Ctrl] voor Venster keuzeopties)", BL_LOGGING);

            return result;
        }

        /// <summary>Builds one journal detail row for CmdDetailJournaal.</summary>
        private string[] BuildJournaalRij()
        {
            RecordToVeld(TABLE_JOURNAL);

            string datum    = VBibText(TABLE_JOURNAL, "#v066 #");
            string rekening = VBibText(TABLE_JOURNAL, "#v019 #");

            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(rekening, 7));
            string naam;
            if (Ktrl != 0)
            {
                naam = "//";
            }
            else
            {
                RecordToVeld(TABLE_LEDGERACCOUNTS);
                naam = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
            }

            string bedrag   = rsMAR[TABLE_JOURNAL].Fields["dece068"].Value?.ToString() ?? "";
            string omschr   = rsMAR[TABLE_JOURNAL].Fields["v067"].Value?.ToString() ?? "";
            string finStuk  = rsMAR[TABLE_JOURNAL].Fields["v038"].Value?.ToString() ?? "";
            string tegenRek = rsMAR[TABLE_JOURNAL].Fields["v069"].Value?.ToString() ?? "";
            string record   = TLB_RECORD[TABLE_JOURNAL];

            return new[] { datum, rekening, naam, bedrag, omschr, finStuk, tegenRek, record };
        }

        /// <summary>Advances grid selection to the next row.</summary>
        private void MoveToNextRow()
        {
            int next = _rowIdx + 1;
            if (next < X.Rows.Count)
            {
                X.CurrentCell = X.Rows[next].Cells[0];
                if (next > 6)
                    X.FirstDisplayedScrollingRowIndex = Math.Max(0, next - 5);
            }
        }

        /// <summary>Returns clipboard text from selected rows, tab-separated.</summary>
        private string GetGridClipText()
        {
            if (X.GetCellCount(DataGridViewElementStates.Selected) == 0) return string.Empty;
            var sb = new StringBuilder();
            foreach (DataGridViewRow row in X.SelectedRows)
            {
                bool first = true;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (!first) sb.Append('\t');
                    sb.Append(cell.Value?.ToString() ?? "");
                    first = false;
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>Returns the file extension without the dot, lower-case.</summary>
        private static string GetExtension(string path)
        {
            int dot = path.LastIndexOf('.');
            return dot >= 0 ? path.Substring(dot + 1).ToLower() : string.Empty;
        }

        // ── BLOB helpers ───────────────────────────────────────────────────────

        /// <summary>VB6: BlobToFile — copies a BLOB field to a binary file.</summary>
        public static void BlobToFile(Field fld, string filename, long chunkSize = 8192)
        {
            if ((fld.Attributes & (int)FieldAttributeEnum.adFldLong) == 0)
                throw new InvalidOperationException("Field doesn't support the GetChunk method.");
            if (File.Exists(filename)) File.Delete(filename);
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                long bytesLeft = fld.ActualSize;
                while (bytesLeft > 0)
                {
                    int bytes = (int)Math.Min(bytesLeft, chunkSize);
                    byte[] tmp = (byte[])fld.GetChunk(bytes);
                    fs.Write(tmp, 0, tmp.Length);
                    bytesLeft -= bytes;
                }
            }
            Application.DoEvents();
        }

        /// <summary>VB6: FileToBlob — copies a binary file into a BLOB field.</summary>
        public static void FileToBlob(Field fld, string filename, long chunkSize = 8192)
        {
            if ((fld.Attributes & (int)FieldAttributeEnum.adFldLong) == 0)
                throw new InvalidOperationException("Field doesn't support the GetChunk method.");
            if (!File.Exists(filename))
                throw new FileNotFoundException("File not found", filename);
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                long bytesLeft = fs.Length;
                while (bytesLeft > 0)
                {
                    int bytes = (int)Math.Min(bytesLeft, chunkSize);
                    byte[] tmp = new byte[bytes];
                    fs.Read(tmp, 0, bytes);
                    fld.AppendChunk(tmp);
                    bytesLeft -= bytes;
                }
            }
        }               
    }
}

