using ADODB;
using marVSS2028.Classes;
using marVSS2028.PublicForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;

namespace marVSS2028.MimMenu.Filing
{
    public partial class FormProductBasicTable : Form
    {
        private bool ficheIsNieuw;
        private bool isGewijzigd;
        private int iTabIndex;
        private string tempoMilieu = "";

        private Recordset rsGroepenHier;
        private Recordset rsSQLQuery;
        private Recordset rsJourQuery;

        private readonly List<TextBox> txtInfo = new List<TextBox>();
        private readonly Dictionary<TextBox, int> txtInfoIndex = new Dictionary<TextBox, int>();
        private readonly string[] txtInfoTags = new string[]
        {
            "v102", "v105", "v106;004", "v107", "v108;022", "v109", "v110", "v111;002",
            "e112|v112", "e113|v113", "v115", "v116;&R6", "v117;&R7", "v118;&R3",
            "v119", "v120", "v114", "e121|v121", "e122|v122", "e123|v123", "v124;&L",
            "v104", "v125", "v103"
        };

        private readonly string[] txtInfoLabels = new string[]
        {
            "X. Code (EAN)", "A. Omschrijving", "B. Maatstaf", "C. Verpakking",
            "D. Soort", "E. Plaats", "F. Winst %", "G. Btw Code", "H. Verkoop",
            "I. Aankoop", "J. Minimum Stock", "K. Aankooprekening", "L. Verkooprekening",
            "M. Voorraadrekening", "N. Eenheden Aankoop", "O. Eenheden Verkoop", "P. Eenheden Stock", "Q. Bedrag Aankoop",
            "R. Bedrag Verkoop", "S. Bedrag Stock", "T. Leverancier", "T 2e kodenr.", "U. Vlag", "V. Goederencode"
        };

        private CheckBox[] chkFilter;
        private Label[] lblCijfers;

        public FormProductBasicTable()
        {
            InitializeComponent();
            TextTools.WireHighlightEvents(this);
            BuildTxtInfoEditors();
            chkFilter = new[] { chkFilter0, chkFilter1, chkFilter2, chkFilter3 };
            lblCijfers = new[] { lblCijfers0, lblCijfers1, lblCijfers2, lblCijfers3, lblCijfers4, lblCijfers5, lblCijfers6 };
        }

        private void Form_Load(object sender, EventArgs e)
        {
            rsSQLQuery = new Recordset { CursorLocation = CursorLocationEnum.adUseClient };
            rsJourQuery = new Recordset { CursorLocation = CursorLocationEnum.adUseClient };

            // Access check helper is not available in this module yet.

            cmdSwitch.Text = "Ingave in EUR";
            txtSQL.Text = "SELECT * FROM Producten";

            string rekenOpties = OleDbTools.String99(181);
            for (int i = 1; i <= rekenOpties.Length && i <= chkFilter.Length; i++)
            {
                chkFilter[i - 1].Checked = rekenOpties[i - 1] == '1';
            }

            GroepenVullen();
            tabDefault.AutoScroll = true;

            CleanUp();
            Application.DoEvents();
        }

        private void FormProductBasicTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (rsSQLQuery != null && rsSQLQuery.State == (int)ObjectStateEnum.adStateOpen) rsSQLQuery.Close();
            }
            catch { }

            try
            {
                if (rsJourQuery != null && rsJourQuery.State == (int)ObjectStateEnum.adStateOpen) rsJourQuery.Close();
            }
            catch { }

            rsSQLQuery = null;
            rsJourQuery = null;
            rsGroepenHier = null;
        }
        private void Alfa_Click(object sender, EventArgs e)
        {
            SharedFl = TABLE_PRODUCTS;
            aIndex = 1;
            GridText = Txt(1)?.Text ?? "";

            using (var sqlSearch = new FormSearchSQL())
                sqlSearch.ShowDialog(this);

            if (Ktrl == 0)
                FillInTextFields();
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == chkFilter1 && Txt(8) != null)
                Txt(8).Enabled = !chkFilter1.Checked;
        }

        private void ButtonTab_Click(object sender, EventArgs e)
        {
            for (int i = iTabIndex + 1; i < txtInfo.Count; i++)
            {
                if (Txt(i) != null && Txt(i).TabStop && Txt(i).CanFocus)
                {
                    Txt(i).Focus();
                    return;
                }
            }
            Txt(0).Focus();
            ButtonSave.Focus();
        }

        private void ButtonHigher_Click(object sender, EventArgs e)
        {
            BNext(TABLE_PRODUCTS);
            if (Ktrl != 0)
            {
                BLast(TABLE_PRODUCTS, 0);
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("Er zijn nog geen gegevens...");
                    return;
                }
            }
            FillInTextFields();
        }

        private void ButtonLower_Click(object sender, EventArgs e)
        {
            BPrev(TABLE_PRODUCTS);
            if (Ktrl != 0)
            {
                BFirst(TABLE_PRODUCTS, 0);
                if (Ktrl != 0)
                {
                    System.Media.SystemSounds.Beep.Play();
                    MessageBox.Show("Er zijn nog geen gegevens...");
                    return;
                }
            }
            FillInTextFields();
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            Msg = "Kies 'Ja' voor kopij als XML bestand\r\nKies 'Nee' voor kopij naar het klassieke plakbord";
            var result = MessageBox.Show(Msg, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.No)
            {
                Clipboard.Clear();
                Clipboard.SetText(GridToClipboard(msfSQL));
                return;
            }

            try
            {
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "Alle bestanden (*.xml)|*.xml";
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    if (File.Exists(dlg.FileName))
                        File.Delete(dlg.FileName);

                    if (rsSQLQuery != null && rsSQLQuery.State == (int)ObjectStateEnum.adStateOpen)
                        rsSQLQuery.Save(dlg.FileName, PersistFormatEnum.adPersistXML);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static string GridToClipboard(DataGridView grid)
        {
            var sb = new StringBuilder();
            for (int c = 0; c < grid.Columns.Count; c++)
            {
                if (c > 0) sb.Append('\t');
                sb.Append(grid.Columns[c].HeaderText);
            }
            sb.AppendLine();

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                for (int c = 0; c < grid.Columns.Count; c++)
                {
                    if (c > 0) sb.Append('\t');
                    sb.Append(row.Cells[c].Value?.ToString() ?? "");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void cmdRBAcontrole_Click(object sender, EventArgs e)
        {
            tempoMilieu = txtMilieu.Text;
            int telOK = -1;
            string[] parts = (txtMilieu.Text ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parts.Length; i++)
            {
                string item = parts[i].Trim();
                if (item == "") continue;

                BGet(TABLE_PRODUCTS, 0, item);
                if (Ktrl == 0)
                {
                    MessageBox.Show(item + " aanwezig", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    telOK++;
                }
                else
                {
                    MessageBox.Show(item + " NIET aanwezig", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (parts.Length > 0 && telOK != parts.Length - 1)
            {
                tempoMilieu = "";
                MessageBox.Show("Probeer opnieuw");
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            bool teSwitchen = false;

            if (bhEuro)
            {
                if (IsInputBef())
                {
                    SnelHelpPrint("BEF switch voor EURO boekhouding.  Cijfers werden omgewerkt vooraleer weg te schrijven", BL_LOGGING);
                    cmdSwitch_Click(sender, e);
                    teSwitchen = true;
                }
            }
            else if (IsInputEur())
            {
                SnelHelpPrint("EUR switch voor BEF boekhouding.  Cijfers worden omgewerkt vooraleer weg te schrijven", BL_LOGGING);
                cmdSwitch_Click(sender, e);
                teSwitchen = true;
            }

            if (tempoMilieu != "")
            {
                txtMilieu.Text = tempoMilieu;
                tempoMilieu = "";
            }
            FillRecordWithFields();

            string productCode = VBibText(TABLE_PRODUCTS, "#v102 #").Trim();
            if (productCode == "")
            {
                MessageBox.Show("Productcode is verplicht.");
                return;
            }

            if (ficheIsNieuw)
            {
                Msg = "Nieuwe fiche '" + productCode + "' toevoegen.  Bent U zeker ?";
            }else
            {
                Msg = "Gegevens bestaande fiche '" + productCode + "' wijzigen.  Bent U zeker ?";
            }            
            KtrlBox = (int)MessageBox.Show(Msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (KtrlBox == (int)DialogResult.Yes)
            {                
                if (teSwitchen) cmdSwitch_Click(sender, e);

                if (ficheIsNieuw)
                {
                    BInsert(TABLE_PRODUCTS, 0);
                    // cmdSchoon_Click(sender, e);
                    ficheIsNieuw = false;
                }
                else
                {
                    BUpdate(TABLE_PRODUCTS, 0);
                }
            }
            else if (teSwitchen)
            {
                cmdSwitch_Click(sender, e);
            }
        }

        private void ButtonSQL_Click(object sender, EventArgs e)
        {
            AdoRecordset();
        }

        private bool AdoRecordset()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (rsSQLQuery == null)
                {
                    rsSQLQuery = new Recordset { CursorLocation = CursorLocationEnum.adUseClient };
                }

                if (rsSQLQuery.State == (int)ObjectStateEnum.adStateOpen)
                    rsSQLQuery.Close();

                rsSQLQuery.Open(txtSQL.Text, adntDB, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);
                lblRecordCount.Text = rsSQLQuery.RecordCount.ToString(CultureInfo.InvariantCulture);
                msfSQL.DataSource = RecordsetToDataTable(rsSQLQuery);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bron:\r\n" + ex.Source + "\r\n\r\nFoutnummer: " + ex.HResult + "\r\n\r\nDetail:\r\n" + ex.Message);
                msfSQL.Refresh();
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static DataTable RecordsetToDataTable(Recordset rs)
        {
            var dt = new DataTable();
            if (rs == null || rs.State != (int)ObjectStateEnum.adStateOpen) return dt;

            for (int i = 0; i < rs.Fields.Count; i++)
                dt.Columns.Add(rs.Fields[i].Name, typeof(string));

            if (!rs.EOF)
                rs.MoveFirst();

            while (!rs.EOF)
            {
                var row = dt.NewRow();
                for (int i = 0; i < rs.Fields.Count; i++)
                    row[i] = rs.Fields[i].Value == DBNull.Value || rs.Fields[i].Value == null ? "" : rs.Fields[i].Value.ToString();
                dt.Rows.Add(row);
                rs.MoveNext();
            }

            return dt;
        }

        private void cmdSwitch_Click(object sender, EventArgs e)
        {
            if (cmdSwitch.Text == "Ingave in EUR")
            {
                cmdSwitch.Text = "Ingave in BEF";
                for (int i = 0; i < txtInfo.Count; i++)
                {
                    var txt = Txt(i);
                    if (txt == null) continue;
                    string tag = (txt.Tag as string) ?? "";
                    if (tag.IndexOf("|", StringComparison.Ordinal) >= 0)
                        txt.Text = Dec(ToDouble(txt.Text) * EURO, "#######0.0000");
                }
            }
            else
            {
                cmdSwitch.Text = "Ingave in EUR";
                for (int i = 0; i < txtInfo.Count; i++)
                {
                    var txt = Txt(i);
                    if (txt == null) continue;
                    string tag = (txt.Tag as string) ?? "";
                    if (tag.IndexOf("|", StringComparison.Ordinal) >= 0)
                        txt.Text = Dec(ToDouble(txt.Text) / EURO, "#######0.0000");
                }
            }

            RenewTicketPrice();
            RenewStockValue();
        }

        private void cmdTonen_Click(object sender, EventArgs e)
        {
            if ((txtLink.Text ?? "").Trim() == "") return;
            if (!ShellHelper.ShellExecuteWithFallback(txtLink.Text.Trim()))
                MessageBox.Show("Koppeling kan niet geopend worden.");
        }

        private void CmdVerwijderFiche_Click(object sender, EventArgs e)
        {
            string sleutel = Txt(0)?.Text ?? "";
            bool blanco = (TLB_RECORD[TABLE_PRODUCTS] ?? "").Trim() == "" || sleutel.Trim() == "";

            if (blanco)
            {
                Msg = "Nummer met blanco verwijderen.  Bent u zeker";
                if (MessageBox.Show(Msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    BDelete(TABLE_PRODUCTS);
                    ButtonHigher_Click(sender, e);
                }
                return;
            }

            Msg = "Gegevens bestaande '" + bstNaam[TABLE_PRODUCTS] + "'-fiche :" + sleutel.Trim() + " verwijderen.  Bent U zeker ?";
            if (MessageBox.Show(Msg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                BDelete(TABLE_PRODUCTS);
                ButtonHigher_Click(sender, e);
            }
        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            CleanUp();
            Txt(0)?.Focus();
        }
        
        private void Groepen_Click(object sender, EventArgs e)
        {
            using (var f = new FormProductGroups())
                f.ShowDialog(this);
            GroepenVullen();
        }

        private void LijstRap_Click(object sender, EventArgs e)
        {
            using (var f = new FormProductReporting())
                f.ShowDialog(this);
        }

        private void TxtInfo_GotFocus(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null) return;

            int index = txtInfoIndex[txt];
            isGewijzigd = false;
            txt.SelectionStart = 0;
            txt.SelectionLength = txt.Text.Length;
            iTabIndex = index;

            txt.BackColor = Color.FromArgb(0xFF, 0xFF, 0x80);
            string tag = (txt.Tag as string) ?? "";

            if (tag.IndexOf(";", StringComparison.Ordinal) >= 0)
            {
                if (txt.Text.TrimEnd() == "")
                {
                    SnelHelpPrint("Druk [Ctrl] om te kiezen", BL_LOGGING);
                }
                else if (tag.IndexOf("&", StringComparison.Ordinal) >= 0)
                {
                    switch (tag.Substring(tag.IndexOf('&') + 1, 1))
                    {
                        case "K": SharedFl = TABLE_CUSTOMERS; break;
                        case "L": SharedFl = TABLE_SUPPLIERS; break;
                        case "R": SharedFl = TABLE_LEDGERACCOUNTS; break;
                        default: MessageBox.Show("nog niks"); return;
                    }

                    BGet(SharedFl, 0, txt.Text);
                    if (Ktrl != 0)
                    {
                        MessageBox.Show(txt.Text + " bestaat niet (meer) !");
                    }
                    else
                    {
                        RecordToVeld(SharedFl);
                        SnelHelpPrint(FVT[SharedFl, 1] + " Druk [Ctrl] om te wijzigen", BL_LOGGING);
                    }
                }
                else
                {
                    SnelHelpPrint("Druk [Ctrl] om te wijzigen", BL_LOGGING);
                }
            }
        }

        private void TxtInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.ControlKey) return;

            var txt = sender as TextBox;
            if (txt == null) return;
            int index = txtInfoIndex[txt];
            string tag = (txt.Tag as string) ?? "";

            if (tag.IndexOf("&", StringComparison.Ordinal) >= 0)
            {
                aIndex = 0;
                switch (tag.Substring(tag.IndexOf('&') + 1, 1))
                {
                    case "K": SharedFl = TABLE_CUSTOMERS; break;
                    case "L": SharedFl = TABLE_SUPPLIERS; break;
                    case "R": SharedFl = TABLE_LEDGERACCOUNTS; break;
                    default: MessageBox.Show("nog niks"); return;
                }

                GridText = txt.Text;
                using (var sqlSearch = new FormSearchSQL())
                    sqlSearch.ShowDialog(this);

                if (Ktrl == 0)
                    txt.Text = FVT[SharedFl, 0];
            }
            else if (tag.IndexOf(";", StringComparison.Ordinal) >= 0)
            {
                aIndex = (int)ToDouble(tag.Substring(tag.IndexOf(';') + 1));
                aIndex += 1000;

                string dummy = txt.Text;
                GridText = dummy;
                using (var keuze = new FormKeuzeVSF())
                    keuze.ShowDialog(this);

                if (GridText != dummy)
                {
                    txt.Text = GridText;
                    RenewTicketPrice();
                }
            }

            e.Handled = true;
        }

        private void TxtInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null) return;

            string tag = (txt.Tag as string) ?? "";
            if (tag.IndexOf(";", StringComparison.Ordinal) >= 0)
                e.KeyChar = '\0';

            isGewijzigd = true;
        }

        private void TxtInfo_LostFocus(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null) return;

            int index = txtInfoIndex[txt];
            txt.BackColor = Color.White;

            double bedragZonder;

            switch (index)
            {
                case 0:
                    BGet(TABLE_PRODUCTS, 0, txt.Text);
                    if (Ktrl != 0)
                    {
                        string sleutel = txt.Text;
                        CleanUp();
                        ficheIsNieuw = true;
                        txt.Text = sleutel;
                    }
                    else
                    {
                        FillInTextFields();
                    }
                    break;

                case 1:
                    if (!isGewijzigd) return;
                    Text = "ProduktFiche : " + txt.Text;
                    break;

                case 3:
                case 6:
                    if (index == 3)
                    {
                        if (Math.Abs(ToDouble(Txt(3)?.Text)) < 0.0000001) Txt(3).Text = "1";
                        Txt(3).Text = Dec(ToDouble(Txt(3)?.Text), "#####.00");
                    }
                    else
                    {
                        Txt(6).Text = Dec(ToDouble(Txt(6)?.Text), "###");
                        if (chkFilter1.Checked)
                        {
                            if (chkFilter0.Checked)
                                bedragZonder = ToDouble(Txt(9)?.Text);
                            else
                                bedragZonder = ToDouble(Txt(9)?.Text) * ToDouble(Txt(3)?.Text);

                            Txt(8).Text = Dec(bedragZonder + (bedragZonder * ToDouble(Txt(6)?.Text) / 100), "########0.00000");
                        }
                    }
                    RenewTicketPrice();
                    break;

                case 8:
                    if (!isGewijzigd) return;
                    if (chkFilter3.Checked)
                    {
                        double btw = ToDouble(ExtractBtwPercentage(Txt(7)?.Text));
                        Txt(8).Text = Dec(ToDouble(Txt(8)?.Text) * 100 / (100 + btw), "########0.00000");
                    }
                    if (chkFilter0.Checked)
                        Txt(8).Text = Dec(ToDouble(Txt(8)?.Text) / Math.Max(0.000001, ToDouble(Txt(3)?.Text)), "########0.00000");

                    Txt(8).Text = Dec(ToDouble(Txt(8)?.Text), "########0.00000");
                    RenewTicketPrice();
                    break;

                case 9:
                    if (!isGewijzigd) return;
                    if (chkFilter2.Checked)
                    {
                        double btw = ToDouble(ExtractBtwPercentage(Txt(7)?.Text));
                        Txt(9).Text = Dec(ToDouble(Txt(9)?.Text) * 100 / (100 + btw), "########0.00000");
                    }
                    if (chkFilter0.Checked)
                        Txt(9).Text = Dec(ToDouble(Txt(9)?.Text) / Math.Max(0.000001, ToDouble(Txt(3)?.Text)), "########0.00000");

                    if (chkFilter1.Checked)
                    {
                        if (chkFilter0.Checked)
                            bedragZonder = ToDouble(Txt(9)?.Text);
                        else
                            bedragZonder = ToDouble(Txt(9)?.Text) * ToDouble(Txt(3)?.Text);

                        Txt(8).Text = Dec(bedragZonder + (bedragZonder * ToDouble(Txt(6)?.Text) / 100), "########0.00000");
                    }

                    Txt(9).Text = Dec(ToDouble(Txt(9)?.Text), "########0.00000");
                    RenewTicketPrice();
                    RenewStockValue();
                    break;

                case 14:
                case 15:
                case 16:
                    Txt(index).Text = Dec(ToDouble(Txt(index)?.Text), "#####0.000");
                    RenewStockValue();
                    break;

                case 17:
                case 18:
                case 19:
                    RenewStockValue();
                    break;
            }
        }

        private static string ExtractBtwPercentage(string btwText)
        {
            if (string.IsNullOrEmpty(btwText)) return "0";
            int pos = btwText.IndexOf(':');
            if (pos < 0 || pos == btwText.Length - 1) return "0";
            return btwText.Substring(pos + 1);
        }

        private void v_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (v.SelectedTab != tabJournaal) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                string msgQuery = 
                    "SELECT * FROM Journalen " +
                    "WHERE v102 ='" + VBibText(TABLE_PRODUCTS, "#v102 #") + "' " + 
                    "AND v066 >='" + BOOKYEAR_FROMTO.Substring(0,8)  + "' " +
                    "AND v066 <='" + BOOKYEAR_FROMTO.Substring(8, 8) + "' " +
                    "AND v019 >'5' "+ 
                    "ORDER BY v066";

                if (rsJourQuery == null)
                {
                    rsJourQuery = new Recordset { CursorLocation = CursorLocationEnum.adUseClient };
                }

                if (rsJourQuery.State == (int)ObjectStateEnum.adStateOpen)
                    rsJourQuery.Close();

                rsJourQuery.Open(msgQuery, adntDB, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);
                msfJournaal.DataSource = RecordsetToDataTable(rsJourQuery);
                lbJournaal.Text = rsJourQuery.RecordCount.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bron:\r\n" + ex.Source + "\r\n\r\nFoutnummer: " + ex.HResult + "\r\n\r\nDetail:\r\n" + ex.Message);
                msfJournaal.Refresh();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void VerwijderenMogelijk_Click(object sender, EventArgs e)
        {
            VerwijderenMogelijk.Checked = !VerwijderenMogelijk.Checked;
            CmdVerwijderFiche.Enabled = VerwijderenMogelijk.Checked;
        }

        private void GroepenVullen()
        {
            cbCategorie.Items.Clear();
            cbMerk.Items.Clear();

            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();

                    string categorieItems = "";
                    using (var cmd = new OleDbCommand("SELECT GroepItems FROM p_Groepen WHERE GroepsNaam = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("?", "Categorie");
                        object val = cmd.ExecuteScalar();
                        if (val != null && val != DBNull.Value) categorieItems = val.ToString();
                    }

                    if (categorieItems == "")
                    {
                        MessageBox.Show("Gelieve de groepen te initialiseren a.u.b.");
                    }
                    else
                    {
                        foreach (var item in categorieItems.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            cbCategorie.Items.Add(item);
                    }

                    string merkItems = "";
                    using (var cmd = new OleDbCommand("SELECT GroepItems FROM p_Groepen WHERE GroepsNaam = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("?", "Merk");
                        object val = cmd.ExecuteScalar();
                        if (val != null && val != DBNull.Value) merkItems = val.ToString();
                    }

                    if (merkItems == "")
                    {
                        MessageBox.Show("Gelieve de groepen te initialiseren a.u.b.");
                    }
                    else
                    {
                        foreach (var item in merkItems.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            cbMerk.Items.Add(item);
                    }
                }
            }
            catch
            {
                MessageBox.Show("productgroepen worden hierna geïnitialiseerd");
                Groepen_Click(this, EventArgs.Empty);
            }
        }

        private void BuildTxtInfoEditors()
        {
            pnlTxtInfo.SuspendLayout();
            pnlTxtInfo.Controls.Clear();
            pnlTxtInfo.RowStyles.Clear();
            pnlTxtInfo.RowCount = txtInfoTags.Length;
            pnlTxtInfo.AutoSize = false;
            txtInfo.Clear();
            txtInfoIndex.Clear();

            for (int i = 0; i < txtInfoTags.Length; i++)
            {
                pnlTxtInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

                var lbl = new Label
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    Text = txtInfoLabels[i],
                    TextAlign = ContentAlignment.MiddleRight,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(1)
                };

                var txt = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(1),
                    Tag = txtInfoTags[i],
                    Name = "TxtInfo" + i.ToString("D2")
                };

                txt.GotFocus += TxtInfo_GotFocus;
                txt.KeyDown += TxtInfo_KeyDown;
                txt.KeyPress += TxtInfo_KeyPress;
                txt.LostFocus += TxtInfo_LostFocus;

                if (i == 0) txt.MaxLength = 13;
                if (i == 1) txt.MaxLength = 40;

                txtInfo.Add(txt);
                txtInfoIndex[txt] = i;

                pnlTxtInfo.Controls.Add(lbl, 0, i);
                pnlTxtInfo.Controls.Add(txt, 1, i);
            }

            pnlTxtInfo.ResumeLayout();
        }

        private TextBox Txt(int index)
        {
            return index >= 0 && index < txtInfo.Count ? txtInfo[index] : null;
        }

        private static double ToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            double result;
            if (double.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out result)) return result;
            if (double.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out result)) return result;
            return 0;
        }

        private bool IsInputEur()
        {
            return cmdSwitch.Text.IndexOf("EUR", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool IsInputBef()
        {
            return cmdSwitch.Text.IndexOf("BEF", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void FillRecordWithFields()
        {
            for (int tempoTel = 0; tempoTel < txtInfo.Count; tempoTel++)
            {
                var txt = Txt(tempoTel);
                if (txt == null) continue;

                string tag = (txt.Tag as string) ?? "";
                if (tag == "") continue;

                string vBibDef;
                if (tag.IndexOf("|", StringComparison.Ordinal) >= 0)
                {
                    if (IsInputBef())
                    {
                        vBibDef = tag.Substring(0, 4);
                        VBib(TABLE_PRODUCTS, Dec(ToDouble(txt.Text) / EURO, MASK_EURX), vBibDef);
                        vBibDef = tag.Substring(tag.Length - 4, 4);
                    }
                    else
                    {
                        vBibDef = tag.Substring(tag.Length - 4, 4);
                        VBib(TABLE_PRODUCTS, Dec(ToDouble(txt.Text) * EURO, MASK_EURX), vBibDef);
                        vBibDef = tag.Substring(0, 4);
                    }
                }
                else if (tag.IndexOf(";", StringComparison.Ordinal) >= 0)
                {
                    vBibDef = tag.Substring(0, tag.IndexOf(';'));
                }
                else
                {
                    vBibDef = tag;
                }

                VBib(TABLE_PRODUCTS, txt.Text, vBibDef);
            }

            VBib(TABLE_PRODUCTS, txtLink.Text, "v002");
            VBib(TABLE_PRODUCTS, txtMilieu.Text, "v261");
            VBib(TABLE_PRODUCTS, txtEindeReeks.Text, "v300");
            VBib(TABLE_PRODUCTS, cbCategorie.SelectedIndex >= 0 ? cbCategorie.Text : " ", "v221");
            VBib(TABLE_PRODUCTS, cbMerk.SelectedIndex >= 0 ? cbMerk.Text : " ", "v001");
        }

        private void CleanUp()
        {
            for (int i = 0; i < txtInfo.Count; i++)
            {
                var txt = Txt(i);
                if (txt != null) txt.Text = "";
            }

            txtLink.Text = "";
            txtMilieu.Text = "";
            txtEindeReeks.Text = "";
            Text = "ProduktFiche :";

            if (Txt(2) != null) Txt(2).Text = FMarBoxText("004", "2", "0");
            if (Txt(3) != null) Txt(3).Text = Dec(1, "#####.00");
            if (Txt(4) != null) Txt(4).Text = FMarBoxText("022", "2", "N");
            if (Txt(7) != null) Txt(7).Text = FMarBoxText("002", "2", OleDbTools.String99(183));
            if (Txt(11) != null) Txt(11).Text = OleDbTools.String99(77);
            if (Txt(12) != null) Txt(12).Text = OleDbTools.String99(78);
            if (Txt(13) != null) Txt(13).Text = OleDbTools.String99(79);

            RenewStockValue();
            RenewTicketPrice();

            cbCategorie.SelectedIndex = -1;
            cbMerk.SelectedIndex = -1;
        }

        private void RenewStockValue()
        {
            double totaalAantal = ToDouble(Txt(14)?.Text) + ToDouble(Txt(15)?.Text) + ToDouble(Txt(16)?.Text);
            double totaalBeweging = ToDouble(Txt(17)?.Text) + ToDouble(Txt(18)?.Text) + ToDouble(Txt(19)?.Text);

            lblCijfers[0].Text = (ToDouble(Txt(14)?.Text) + ToDouble(Txt(16)?.Text) - ToDouble(Txt(15)?.Text)).ToString("0.000", CultureInfo.InvariantCulture);
            if (Math.Abs(totaalAantal) < 0.0000001)
            {
                lblCijfers[1].Text = "";
            }
            else
            {
                lblCijfers[1].Text = ((totaalBeweging / totaalAantal) * ToDouble(lblCijfers[0].Text)).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }

            lblCijfers[6].Text = (ToDouble(Txt(9)?.Text) * ToDouble(lblCijfers[0].Text)).ToString("#,##0.00", CultureInfo.InvariantCulture);
        }

        private void RenewTicketPrice()
        {
            double btwPerc = 0;
            string btwText = Txt(7)?.Text ?? "";
            int pos = btwText.IndexOf(':');
            if (pos >= 0 && pos < btwText.Length - 1)
                btwPerc = ToDouble(btwText.Substring(pos + 1));

            double bedragBefExcl;
            double bedragBefBtw;
            double bedragEurExcl;
            double bedragEurBtw;

            if (IsInputEur())
            {
                bedragEurExcl = ToDouble(Txt(3)?.Text) * ToDouble(Txt(8)?.Text);
                bedragBefExcl = bedragEurExcl * EURO;

                bedragEurBtw = bedragEurExcl * btwPerc / 100.0;
                bedragBefBtw = bedragBefExcl * btwPerc / 100.0;

                lblCijfers[5].Text = bedragEurExcl.ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblCijfers[3].Text = bedragBefExcl.ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblCijfers[4].Text = (bedragEurExcl + bedragEurBtw).ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblCijfers[2].Text = (bedragBefExcl + bedragBefBtw).ToString("#,##0", CultureInfo.InvariantCulture);
            }
            else
            {
                bedragBefExcl = ToDouble(Txt(3)?.Text) * ToDouble(Txt(8)?.Text);
                bedragEurExcl = bedragBefExcl / EURO;

                bedragBefBtw = bedragBefExcl * btwPerc / 100.0;
                bedragEurBtw = bedragEurExcl * btwPerc / 100.0;

                lblCijfers[3].Text = bedragBefExcl.ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblCijfers[5].Text = bedragEurExcl.ToString("#,##0.00", CultureInfo.InvariantCulture);
                lblCijfers[2].Text = (bedragBefExcl + bedragBefBtw).ToString("#,##0", CultureInfo.InvariantCulture);
                lblCijfers[4].Text = (bedragEurExcl + bedragEurBtw).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }
        }

        private void FillInTextFields()
        {
            ficheIsNieuw = false;
            RecordToVeld(TABLE_PRODUCTS);
            Text = "ProduktFiche : " + VBibText(TABLE_PRODUCTS, "#v105 #").TrimEnd();

            for (int tempoTel = 0; tempoTel < txtInfo.Count; tempoTel++)
            {
                var txt = Txt(tempoTel);
                if (txt == null) continue;

                string tag = (txt.Tag as string) ?? "";
                if (tag == "") continue;

                string vBibDef;
                if (tag.IndexOf("|", StringComparison.Ordinal) >= 0)
                {
                    if (bhEuro)
                    {
                        vBibDef = tag.Substring(0, 4);
                        txt.Text = IsInputEur()
                            ? Dec(ToDouble(VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #")), "#######0.0000")
                            : Dec(ToDouble(VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #")) * EURO, "#######0.0000");
                    }
                    else
                    {
                        vBibDef = tag.Substring(tag.Length - 4, 4);
                        txt.Text = IsInputBef()
                            ? Dec(ToDouble(VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #")), "#######0.0000")
                            : Dec(ToDouble(VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #")) / EURO, "#######0.0000");
                    }
                }
                else if (tag.IndexOf(";", StringComparison.Ordinal) >= 0)
                {
                    vBibDef = tag.Substring(0, tag.IndexOf(';'));
                    if (tag.IndexOf("&", StringComparison.Ordinal) >= 0)
                    {
                        txt.Text = VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #");
                    }
                    else
                    {
                        txt.Text = FMarBoxText(tag.Substring(tag.IndexOf(';') + 1), "2", VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #"));
                    }
                }
                else
                {
                    vBibDef = tag;
                    txt.Text = VBibText(TABLE_PRODUCTS, "#" + vBibDef + " #");
                }
            }

            txtLink.Text = VBibText(TABLE_PRODUCTS, "#v002 #");
            txtEindeReeks.Text = ToDouble(VBibText(TABLE_PRODUCTS, "#v300 #")).ToString(CultureInfo.InvariantCulture);
            txtMilieu.Text = VBibText(TABLE_PRODUCTS, "#v261 #");

            string tmpCategorie = VBibText(TABLE_PRODUCTS, "#v221 #").Trim();
            string tmpMerk = VBibText(TABLE_PRODUCTS, "#v001 #").Trim();

            cbMerk.SelectedIndex = -1;
            cbCategorie.SelectedIndex = -1;

            if (tmpCategorie != "")
            {
                for (int i = 0; i < cbCategorie.Items.Count; i++)
                {
                    if (tmpCategorie == cbCategorie.Items[i].ToString())
                    {
                        cbCategorie.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (tmpMerk != "")
            {
                for (int i = 0; i < cbMerk.Items.Count; i++)
                {
                    if (tmpMerk == cbMerk.Items[i].ToString())
                    {
                        cbMerk.SelectedIndex = i;
                        break;
                    }
                }
            }

            RenewTicketPrice();
            RenewStockValue();
        }
    }
}
