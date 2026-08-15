using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using IDEALSoftware.VpeCommunity;

using marVSS2028.Classes;
using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.ShellHelper;
using marVSS2028.Forms;
using marVSS2028.MimMenu.Accounting;
using marVSS2028.MimMenu.Actions;
using marVSS2028.MimMenu.DailyManagement;
using marVSS2028.MimMenu.Filing;
using marVSS2028.PrivateForms;
using marVSS2028.SharedForms;

namespace marVSS2028
{
    public partial class FormMim : Form
    {
        public VpeControl Report = new VpeControl();
        public FormMim()
        {               
            InitializeComponent();            
            Globals.Mim = this;   // register global reference                            
        }

        private void MenuItemBYPERDAT_Click(object sender, EventArgs e)
        {
            foreach (Form child in MdiChildren)
            {
                if (child is FormBYPERDAT existing)
                {
                    existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }
            }
        }

        public void SetPersistentFormsEnabled(bool enabled)
        {
            MenuItemBYPERDAT.Enabled = enabled;
            ToolStripCustomers.Enabled = enabled;
            ToolStripSuppliers.Enabled = enabled;
            ToolStripLedgerAccounts.Enabled = enabled;
        }

        private void ShowSingleMdiChild<T>() where T : Form, new()
        {
            foreach (Form child in MdiChildren)
            {
                if (child is T existing)
                {
                    existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }
            }

            T form = new T
            {
                MdiParent = this
            };
            form.Show();
        }

        private void ToolStripCustomers_Click(object sender, EventArgs e)
        {
            BasisB[1].WindowState = FormWindowState.Normal;
        }

        private void ToolStripSuppliers_Click(object sender, EventArgs e)
        {
            BasisB[2].WindowState = FormWindowState.Normal;
        }

        private void ToolStripLedgerAccounts_Click(object sender, EventArgs e)
        {
            BasisB[3].WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Sets the SnelHelp status bar text.
        /// When startTimer is true the auto-clear timer is (re)started.
        /// Called by MimEnvironment.SnelHelpPrint.
        /// </summary>
        public void SetSnelHelp(string text, bool startTimer)
        {
            SnelHelpLabel.Text = text;
            if (startTimer)
            {
                SnelHelpTijd.Stop();
                SnelHelpTijd.Start();
            }
        }

        /// <summary>Returns the first character of the cmdWegBoekModus selection ("0", "1", or "2").</summary>
        public string GetWegBoekModus()
        {
            string t = cmdWegBoekModus.Text;
            return t.Length > 0 ? t.Substring(0, 1) : "0";
        }

        private void SnelHelpTijd_Tick(object sender, EventArgs e)
        {
            SnelHelpTijd.Stop();
            SnelHelpLabel.Text = string.Empty;
        }

        private void FormMim_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.IsUpgraded)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsUpgraded = true;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.MainTop <= 0)
            {
                Width = 816;
                Height = 489;
                StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                Top = Properties.Settings.Default.MainTop;
                Left = Properties.Settings.Default.MainLeft;
                Width = Properties.Settings.Default.MainWidth;
                Height = Properties.Settings.Default.MainHeight;
            }

            Cursor = Cursors.WaitCursor;

            // Application path
            PROGRAM_LOCATION = Application.StartupPath + @"\";

            // Desktop location
            LOCATION_DESKTOP = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\";

            // Form caption
            Text = appTitleAndVersion;

            usrMailAdres = "demo@rv.be";
            usrPW = "9999";

            // My Documents location
            SYSTEM_MYPERSONALDOCUMENTS = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            if (string.IsNullOrWhiteSpace(LOCATION_MYDOCUMENTS))
            {
                // First-time startup: fall back to My Documents
                LOCATION_MYDOCUMENTS = SYSTEM_MYPERSONALDOCUMENTS;
                MaakBewaarDataFolder();
            }

            MIM_GLOBAL_DATE = DateTime.Now.ToString("dd/MM/yyyy");
            ToolStripBookingDate.Text = MIM_GLOBAL_DATE;

            // cmdWegBoekModus ToolStrip combobox — items already set in designer; set default selection            
            cmdWegBoekModus.Items.Add("0: Geen BoekingsInfo tonen");
            cmdWegBoekModus.Items.Add("1: BoekingsInfo tonen bij EUR <> BEF verschil");
            cmdWegBoekModus.Items.Add("2: Altijd BoekingsInfo tonen");
            cmdWegBoekModus.SelectedIndex = 2;
            cmdWegBoekModus.Enabled = false;

            // Read vsoft.ini
            // In ClickOnce deployments Application.StartupPath points to the app's
            // read-only deployment cache folder, which is also where MdX is published.
            // In development this is the bin\Debug (or bin\Release) folder.
            string vsoftIni = PROGRAM_LOCATION + @"MdX\vsoft.txt";
            if (!File.Exists(vsoftIni))
            {
                MessageBox.Show(
                    "VSOFT.INI niet te vinden.  Installeer korrekt a.u.b.\r\n\r\nGezocht in:\r\n" + vsoftIni,
                    string.Empty,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            foreach (string line in File.ReadLines(vsoftIni))
            {
                string key = line.Length >= 9 ? line.Substring(0, 9).ToLower() : line.ToLower();
                switch (key)
                {
                    case "programma":
                        break;
                    case "assurnet ":
                        int eqIdx = line.IndexOf('=');
                        if (eqIdx >= 0)
                            LOCATION_ASWEB = line.Substring(eqIdx + 1).Trim() + DateTime.Now.ToString("mmss");
                        break;
                    case "producent":
                        int eqIdx2 = line.IndexOf('=');
                        if (eqIdx2 >= 0)
                            ProducentNummer = line.Substring(eqIdx2 + 1).Trim();
                        break;
                }
            }

            LOCATION_ = Globals.LOCATION_MYDOCUMENTS + @"\";
            PERIOD_FROMTO = string.Empty;
                        
            InitFirst();

            // Open splash/licence form
            using (FormSplash splash = new FormSplash())
            {
                splash.ShowDialog(this);
            }

            // Open standaard databases
            try
            {                
                adKBDB = new ADODB.Connection
                {
                    ConnectionString = ADOJET_PROVIDER +
                        "Data Source=" + PROGRAM_LOCATION + @"MdX\Default2022.mdb;"
                };
                adKBDB.Open();

                adTBIB = new ADODB.Connection
                {
                    ConnectionString = ADOJET_PROVIDER +
                        "Data Source=" + PROGRAM_LOCATION + @"MdX\Telebib2.mdb;"
                };
                adTBIB.Open();

                adKBTable = new ADODB.Recordset
                {
                    CursorLocation = ADODB.CursorLocationEnum.adUseServer
                };
                adKBTable.Open("KeuzeBoxData", adKBDB,
                    ADODB.CursorTypeEnum.adOpenKeyset,
                    ADODB.LockTypeEnum.adLockOptimistic,
                    (int)ADODB.CommandTypeEnum.adCmdTableDirect);
                adKBTable.Index = "BestandsNaam";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij openen standaard database:\n\n" + ex.Message,
                    "Standaard database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                        
            // Define Virtual basisForms (BasisB, T = 1 to 3; 4 if product)
            string[] bCaption = new string[4];
            int[] bColor = new int[5];
            bCaption[1] = "Klanten";       bColor[1] = 9;
            bCaption[2] = "Leveranciers";  bColor[2] = 12;
            bCaption[3] = "Rekeningen";    bColor[3] = 15;
            // bCaption[4] = "Artikels";      bColor[4] = 2;

            // Creating Virtual basisForms (BasisB, T = 1 to 3; 4 if product)
            for (int T = 1; T <= 3; T++)
            {
                var basisForm = new FormBasicTable();
                BasisB[T] = basisForm;
                basisForm.MdiParent = this;
                basisForm.Text = bCaption[T];

                if (T == 1)
                    basisForm.BackColor = Color.FromArgb(44, 183, 255);   // from 16758380
                else if (T == 2)
                    basisForm.BackColor = Color.FromArgb(255, 159, 157);  // from 10329599
                else if (T == 3)
                    basisForm.BackColor = Color.FromArgb(255, 255, 234);  // from 15400959
                else
                    basisForm.BackColor = ColorFromQbColor(bColor[T]);

                basisForm.Tag = T.ToString();
                basisForm.Show();
                basisForm.WindowState = FormWindowState.Minimized;
                basisForm.Enabled = false;
            }
                                    
            FormNTInputbox ntInputbox = new FormNTInputbox();
            ntInputbox.Hide();

            var formBYPERDAT = new FormBYPERDAT
            {
                MdiParent = this,                
                Enabled = false
            };
            formBYPERDAT.Show();
                      
            
            AutoUnLoadCompany();

            MenuActionsOpenCompany_Click(sender, e);
            Cursor = Cursors.Default;
        }

        private static void MaakBewaarDataFolder()
        {
            if (string.IsNullOrWhiteSpace(LOCATION_MYDOCUMENTS))
                return;
            try
            {
                Directory.CreateDirectory(LOCATION_MYDOCUMENTS);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij aanmaken map:\n\n" + LOCATION_MYDOCUMENTS + "\n\n" + ex.Message,
                    "Initialisatie", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Maps VB6 QBColor() index to System.Drawing.Color
        private static System.Drawing.Color ColorFromQbColor(int qbIndex)
        {
            System.Drawing.Color[] qbColors =
            {
                System.Drawing.Color.Black,         // 0
                System.Drawing.Color.DarkBlue,      // 1
                System.Drawing.Color.DarkGreen,     // 2
                System.Drawing.Color.DarkCyan,      // 3
                System.Drawing.Color.DarkRed,       // 4
                System.Drawing.Color.DarkMagenta,   // 5
                System.Drawing.Color.DarkGoldenrod, // 6
                System.Drawing.Color.LightGray,     // 7
                System.Drawing.Color.DarkGray,      // 8
                System.Drawing.Color.Blue,          // 9
                System.Drawing.Color.Green,         // 10
                System.Drawing.Color.Cyan,          // 11
                System.Drawing.Color.Red,           // 12
                System.Drawing.Color.Magenta,       // 13
                System.Drawing.Color.Yellow,        // 14
                System.Drawing.Color.White          // 15
            };
            return qbIndex >= 0 && qbIndex < qbColors.Length
                ? qbColors[qbIndex]
                : System.Drawing.Color.White;
        }

        private void FormMim_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainTop = Top;
            Properties.Settings.Default.MainLeft = Left;
            Properties.Settings.Default.MainWidth = Width;
            Properties.Settings.Default.MainHeight = Height;            
            Properties.Settings.Default.Save();
        }
                        
        private void TbSqlSearch_Click(object sender, EventArgs e)
        {
            GridText = string.Empty;
            using (var sqlSearch = new marVSS2028.PublicForms.FormSearchSQL())
                sqlSearch.ShowDialog(this);
        }

        private void TbMarioMap_Click(object sender, EventArgs e)
        {
            string marioPath = LaadTekstOLD("dnnInstellingen", "Mario");
            if (!ShellExecuteWithFallback(marioPath))
                MessageBox.Show(
                    "Kon " + marioPath + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void TbArchiveMap_Click(object sender, EventArgs e)
        {
            string archivePath = LaadTekstOLD("dnnInstellingen", "Archief");
            if (!ShellExecuteWithFallback(archivePath))
                MessageBox.Show(
                    "Kon " + archivePath + " niet openen. Raadpleeg ShellHelper.log voor details.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }        

        private void DatumVerwerking_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void CmdWegBoekModus_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }
        
        private void MenuActiesBedrijfOpenen_Click(object sender, EventArgs e)
        {            
            MessageBox.Show("Bedrijf openen: nog te porteren logica.", "Acties", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuWindowCascade_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void MenuWindowTileVertical_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void MenuWindowTileHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);            
        }

        private void MenuWindowArrangeIcons_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }


        private void MenuCloud4MarInstellingen_Click(object sender, EventArgs e)
        {
            using (FormCloudSetting dlg = new FormCloudSetting())
            {
                dlg.ShowDialog();
            }
        }
                
        private void MenuActionsCloseApp_Click(object sender, EventArgs e)
        {
            if (SystemSubMenu.Enabled)
            {                
                AutoUnLoadCompany();
            }

            foreach (Form child in MdiChildren)
            {
                child.Close();
            }
            Close();
        }
        
        private void TbOpenCompany_Click(object sender, EventArgs e)
        {
            MenuActionsOpenCompany_Click(sender, e);
        }

        private void MenuActiesCloseCompany_Click(object sender, EventArgs e)
        {
            if (SystemSubMenu.Enabled)
            {
                // If SystemSubMenu is enabled, it means a company is already open
                // Ask user if it is ok to close the current company before opening a new one
                // If user cancels, do not proceed with opening a new company
                // Button no is default to prevent accidental clicks on yes
                DialogResult result = MessageBox.Show(
                    "Wilt u dit bedrijf inclusief eventuele open vensters sluiten?",
                    "Bedrijf sluiten",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return;
                }
                AutoUnLoadCompany();
                try
                {
                    if (adntDB != null &&
                        adntDB.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                        adntDB.Close();
                }
                catch { }
                Application.DoEvents();
            }            
        }

        private void MenuActiesManager2009_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = new System.Drawing.Size(1152, 864);
            CenterToScreen();
        }

        private void MenuActiesManager2005_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = new System.Drawing.Size(1024, 768);
            CenterToScreen();
        }

        private void LinkObsidian_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://web24.foxxl.com:8443");
        }

        private void LinkWebmail_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://webmail.rv-services.be");
        }

        private void LinkAccounting_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://www.cbn-cnc.be/nl");            
        }

        private void LinkLicence_Click(object sender, EventArgs e)
        {
            using (FormSplash splash = new FormSplash())
            {
                DialogResult dialogResult = splash.ShowDialog(this);
            }
        }

        private void CommandPrompt_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("cmd");
        }

        private void LinkPeppolValidator_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://peppol-tools.ademico-software.com/ui/document-validator");
        }

        private void LinkPeppolDocs_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://docs.peppol.eu/poacc/billing/3.0/");
        }

        private void LinkMarSyncClickOnce_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://clickonce.vsoft.be/MarSync/publish.htm");
        }      
        
        private void GitHub_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://github.com/JosVermoesen/marVSS");
        }
        
        public void SQLToolMenuItem_Click(object sender, EventArgs e)
        {
            FormSQLOperations sqlForm = new FormSQLOperations
            {
                MdiParent = this
            };
            sqlForm.Show();
        }
                        
        private void MenuActionsMarSync_Click(object sender, EventArgs e)
        {
            DetectClickOnceShortCut();
        }

        private void DetectClickOnceShortCut()
        {
            Application.DoEvents();

            string startMenuPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)
                + @"\Microsoft\Windows\Start Menu\Programs\Vsoft Administratieve Software";

            string[] files = Directory.GetFiles(startMenuPath, "*.appref-ms");

            if (files.Length > 0)
            {
                if (!ShellExecuteWithFallback(files[0]))
                    MessageBox.Show(
                        "Kon bestand niet openen. Raadpleeg ShellHelper.log voor details.",
                        "Fout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(
                    "ClickOnce snelkoppeling voor MarSync niet gevonden in:" + System.Environment.NewLine + System.Environment.NewLine + startMenuPath,
                    "Snelkoppeling Niet Gevonden",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void MenuItemSetup_Click(object sender, EventArgs e)
        {
            FormFiscalYearSettings fiscalYearsettings = new FormFiscalYearSettings
            {
                MdiParent = this
            };
            fiscalYearsettings.Show();
        }
                
        private void ToolStripManualLedgerEntry_Click(object sender, EventArgs e)
        {
            foreach (Form child in MdiChildren)
            {
                if (child is FormManualLedgerEntries existing)
                {
                    existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }
            }

            FormManualLedgerEntries journalEntryForm = new FormManualLedgerEntries
            {
                MdiParent = this
            };
            journalEntryForm.Show();
        }        
        
        private void ToolStripNewFinancialYear_Click(object sender, EventArgs e)
        {
            bool resultNewFinancialYear = MdvDataTools.NewBookYear();
            if (resultNewFinancialYear)
            {
                MessageBox.Show(
                    "Nieuw boekjaar succesvol aangemaakt.",
                    "Nieuw Boekjaar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                MenuActionsOpenCompany_Click(sender, e);
            }            
        }

        private void MenuItemEditCompanyName_Click(object sender, EventArgs e)
        {
            string naam = string.Empty;
            string marntPath = Path.Combine(LOCATION_COMPANYDATA, "marnt.txt");

            try
            {
                if (File.Exists(marntPath))
                    naam = File.ReadLines(marntPath).FirstOrDefault() ?? string.Empty;
            }
            catch { }

            FormNTInputbox dlg = new FormNTInputbox();
            {
                dlg.TekstInfo.Text = naam;
                dlg.Hernieuw.Visible = false;
                dlg.BtnForward.Visible = false;
                dlg.BtnBack.Visible = false;
                dlg.lblInfo.Visible = false;

                dlg.ShowDialog(this);

                if (dlg.TekstInfo.Text != "\xFF")
                {
                    try
                    {
                        File.WriteAllText(marntPath, dlg.TekstInfo.Text + System.Environment.NewLine);
                    }
                    catch { }

                    Mim.Text = $"{appTitleAndVersion} - [{dlg.TekstInfo.Text.Trim()}]";
                }
            }
        }

        private void ToolStripLedgerBook_Click(object sender, EventArgs e)
        {
            using (var ledgerBookForm = new FormLedgerBook())
                ledgerBookForm.ShowDialog();
        }

        private void ToolStripTrialBalance_Click(object sender, EventArgs e)
        {
            using (var trialBalanceForm = new FormTrialBalance())
                trialBalanceForm.ShowDialog();
        }

        private void ToolStripHistoryGeneralLedger_Click(object sender, EventArgs e)
        {
            using (var historyGeneralLedgerForm = new FormHistoryGeneralLedger())
                historyGeneralLedgerForm.ShowDialog();
        }

        private void ToolStripPurchaseLedger_Click(object sender, EventArgs e)
        {
            using (var purchaseLedgerForm = new FormPurchaseAndSalesLedger())
            {
                purchaseLedgerForm._tableIndex = TABLE_SUPPLIERS;
                purchaseLedgerForm.ShowDialog();
            }
        }

        private void ToolStripSalesLedger_Click(object sender, EventArgs e)
        {
            using (var salesLedgerForm = new FormPurchaseAndSalesLedger())
            {
                salesLedgerForm._tableIndex = TABLE_CUSTOMERS;
                salesLedgerForm.ShowDialog();
            }
        }

        private void MenuActionsNewCompany_Click(object sender, EventArgs e)
        {
            if (SystemSubMenu.Enabled)
            {
                // If SystemSubMenu is enabled, it means a company is already open
                // Ask user if it is ok to close the current company before opening a new one
                // If user cancels, do not proceed with opening a new company
                // Button no is default to prevent accidental clicks on yes
                DialogResult result = MessageBox.Show(
                    "Er is nog een bedrijf geopend. Wilt u dit bedrijf sluiten?",
                    "Nieuwe Bedrijfsmap maken",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return;
                }
                AutoUnLoadCompany();
            }

            foreach (Form child in MdiChildren)
            {
                if (child is FormCompanyNew existing)
                {
                    existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }
            }

            FormCompanyNew newCompany = new FormCompanyNew
            {
                MdiParent = this
            };
            newCompany.Show();
        }

        private void MenuActionsOpenCompany_Click(object sender, EventArgs e)
        {
            if (SystemSubMenu.Enabled)
            {
                // If SystemSubMenu is enabled, it means a company is already open
                // Ask user if it is ok to close the current company before opening a new one
                // If user cancels, do not proceed with opening a new company
                // Button no is default to prevent accidental clicks on yes
                DialogResult result = MessageBox.Show(
                    "Er is nog een bedrijf geopend. Wilt u dit bedrijf sluiten en een ander bedrijf openen?",
                    "Bedrijf openen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return;
                }
                AutoUnLoadCompany();
            }

            foreach (Form child in MdiChildren)
            {
                if (child is FormOpenCompany existing)
                {
                    existing.WindowState = FormWindowState.Normal;
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }
            }

            FormOpenCompany openCompany = new FormOpenCompany
            {
                MdiParent = this
            };
            openCompany.Show();
        }

        private void ToolStripFinancialBook_Click(object sender, EventArgs e)
        {
            using (var financialBookForm = new FormFinancialBook())
            {                
                financialBookForm.ShowDialog();
            }
        }

        private void ToolStripLedgerOnScreen_Click(object sender, EventArgs e)
        {
            using (var ledgerOnScreenForm = new FormLedgerSQL())
            {
                ledgerOnScreenForm.ShowDialog();
            }
        }

        private void ToolStripCustomersBalance_Click(object sender, EventArgs e)
        {
            using (var balanceSalesForm = new FormBalancePurchaseAndSales())
            {
                balanceSalesForm._tableIndex = TABLE_CUSTOMERS;
                balanceSalesForm.ShowDialog();
            }
        }

        private void ToolStripSuppliersBalance_Click(object sender, EventArgs e)
        {
            using (var balancePurchaseForm = new FormBalancePurchaseAndSales())
            {
                balancePurchaseForm._tableIndex = TABLE_SUPPLIERS;
                balancePurchaseForm.ShowDialog();
            }
        }

        private void ToolStripVatDeclaration_Click(object sender, EventArgs e)
        {
            using (var vatDeclarationForm = new FormVatDeclaration())
            {
                vatDeclarationForm.ShowDialog();
            }
        }

        private void ToolStripCDD_Click(object sender, EventArgs e)
        {
            ShellExecuteWithFallback("https://cdd.vsoft.be");
        }

        private void ToolStripBasicTableReporting_Click(object sender, EventArgs e)
        {
            FormBasicTableReporting basicTableReporting = new FormBasicTableReporting();
            basicTableReporting.Show();            
        }

        private void MenuItemTemplateVPE_Click(object sender, EventArgs e)
        {
            FormVpeTemplateEditor vpeTemplateEditor = new FormVpeTemplateEditor
            {
                MdiParent = this
            };
            vpeTemplateEditor.Show();
        }

        private void ToolStripVariousDataSheets_Click(object sender, EventArgs e)
        {
            FormVariousDataSheets variousDataSheets = new FormVariousDataSheets
            {
                MdiParent = this
            }; variousDataSheets.Show();
        }

        private void ToolStripProducts_Click(object sender, EventArgs e)
        {
            FormProductBasicTable productBasicTable = new FormProductBasicTable
            {
                // MdiParent = this
            }; productBasicTable.Show();
        }

        private void ToolStripBuying_Click(object sender, EventArgs e)
        {         
            ShowSingleMdiChild<FormBuying>();
            LayoutMdi(MdiLayout.Cascade);
        }

        private void ToolStripFinancial_Click(object sender, EventArgs e)
        {
            // ShowSingleMdiChild<FormProcessBankStatements>();
            // LayoutMdi(MdiLayout.Cascade);
        }
    }
}
