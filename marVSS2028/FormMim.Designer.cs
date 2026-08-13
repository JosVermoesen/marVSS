using ADODB;
using System.Drawing;
using System.Windows.Forms;

namespace marVSS2028
{
    partial class FormMim
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem ActionsSubMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuActionsOpenCompany;
        private System.Windows.Forms.ToolStripMenuItem MenuActionsNewCompany;
        public System.Windows.Forms.ToolStripMenuItem MenuActiesCloseCompany;
        private System.Windows.Forms.ToolStripMenuItem MenuActionsMarSync;
        private System.Windows.Forms.ToolStripMenuItem MenuActiesManager2009;
        private System.Windows.Forms.ToolStripMenuItem MenuActiesManager2005;
        private System.Windows.Forms.ToolStripMenuItem MenuActiesXmlRekenbladen;
        private System.Windows.Forms.ToolStripMenuItem MenuActionsCloseApp;
        public System.Windows.Forms.ToolStripMenuItem SystemSubMenu;
        public System.Windows.Forms.ToolStripMenuItem WindowSubMenu;
        public System.Windows.Forms.ToolStripMenuItem FilesSubMenu;
        public System.Windows.Forms.ToolStripMenuItem DailyManagementSubMenu;
        public System.Windows.Forms.ToolStripMenuItem AccountingSubMenu;
        public System.Windows.Forms.ToolStripMenuItem ContractsSubMenu;
        private System.Windows.Forms.ToolStripMenuItem CloudSubMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuCloud4MarInstellingen;
        private System.Windows.Forms.ToolStripMenuItem MenuCloud4MarWebsite;
        private System.Windows.Forms.ToolStripMenuItem SettingsSubMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpSubMenu;
        private System.Windows.Forms.ToolStripMenuItem HostingInfo;
        private System.Windows.Forms.ToolStripMenuItem LinkAccounting;
        private System.Windows.Forms.ToolStripMenuItem LinkLicence;
        private System.Windows.Forms.ToolStripMenuItem CommandPrompt;
        private System.Windows.Forms.ToolStripMenuItem PeppolInfo;
        private System.Windows.Forms.ToolStripMenuItem LinkPeppolValidator;
        private System.Windows.Forms.ToolStripMenuItem LinkPeppolDocs;
        private System.Windows.Forms.ToolStripMenuItem LinkMarSyncClickOnce;
        private System.Windows.Forms.ToolStripMenuItem MenuCascadeOpenForms;
        private System.Windows.Forms.ToolStripMenuItem MenuTileOpenFormsVertical;
        private System.Windows.Forms.ToolStripMenuItem MenuTileOpenFormsHorizontal;
        private System.Windows.Forms.ToolStripMenuItem MenuArrangeOpenForms;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMim));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.ActionsSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuActionsOpenCompany = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuActionsNewCompany = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuActiesCloseCompany = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuActionsMarSync = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuActiesManager2009 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuActiesManager2005 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuActiesXmlRekenbladen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuActionsCloseApp = new System.Windows.Forms.ToolStripMenuItem();
            this.SystemSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemBYPERDAT = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemTemplateVPE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemEditCompanyName = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCascadeOpenForms = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTileOpenFormsVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTileOpenFormsHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuArrangeOpenForms = new System.Windows.Forms.ToolStripMenuItem();
            this.FilesSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripDashBoard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripCustomers = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSuppliers = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripLedgerAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripProducts = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripVariousDataSheets = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripBasicTableReporting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLedgerOnScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.DailyManagementSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripBuying = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSelling = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripFinancial = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripCashRegister = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripCDD = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountingSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripManualLedgerEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripPurchaseLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSalesLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripFinancialBook = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripLedgerBook = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripEUVatListingQuarterly = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolstripBEVATListingYearly = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripIntrastat19 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripIntrastat29 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripVatDeclaration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripProductInventoryCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripHistoryGeneralLedger = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripCustomersBalance = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripCustomersTopdown = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSuppliersBalance = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSuppliersTopdown = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripTrialBalance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripFinalReport = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripTransitionProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripNewFinancialYear = new System.Windows.Forms.ToolStripMenuItem();
            this.ContractsSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CloudSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCloud4MarInstellingen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCloud4MarWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpSubMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HostingInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkObsidian = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkWebmail = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkAccounting = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkLicence = new System.Windows.Forms.ToolStripMenuItem();
            this.CommandPrompt = new System.Windows.Forms.ToolStripMenuItem();
            this.PeppolInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkPeppolValidator = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkPeppolDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkMarSyncClickOnce = new System.Windows.Forms.ToolStripMenuItem();
            this.GitHub = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.ToolStripBookingDate = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelBoekModus = new System.Windows.Forms.ToolStripLabel();
            this.cmdWegBoekModus = new System.Windows.Forms.ToolStripComboBox();
            this.TbOpenCompany = new System.Windows.Forms.ToolStripButton();
            this.TbArchiveMap = new System.Windows.Forms.ToolStripButton();
            this.TbMarioMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.SnelHelp = new System.Windows.Forms.StatusStrip();
            this.ToolStripLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.SnelHelpLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SnelHelpTijd = new System.Windows.Forms.Timer(this.components);
            this.InfoData = new System.Windows.Forms.PictureBox();
            this.mainMenu.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SnelHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoData)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActionsSubMenu,
            this.SystemSubMenu,
            this.WindowSubMenu,
            this.FilesSubMenu,
            this.DailyManagementSubMenu,
            this.AccountingSubMenu,
            this.ContractsSubMenu,
            this.CloudSubMenu,
            this.SettingsSubMenu,
            this.HelpSubMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(831, 24);
            this.mainMenu.TabIndex = 11;
            this.mainMenu.Text = "MenuStrip";
            // 
            // ActionsSubMenu
            // 
            this.ActionsSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuActionsOpenCompany,
            this.MenuActionsNewCompany,
            this.MenuActiesCloseCompany,
            this.toolStripSeparator1,
            this.MenuActionsMarSync,
            this.toolStripSeparator2,
            this.MenuActiesManager2009,
            this.MenuActiesManager2005,
            this.toolStripSeparator3,
            this.MenuActiesXmlRekenbladen,
            this.MenuActionsCloseApp});
            this.ActionsSubMenu.Name = "ActionsSubMenu";
            this.ActionsSubMenu.Size = new System.Drawing.Size(51, 20);
            this.ActionsSubMenu.Text = "&Acties";
            // 
            // MenuActionsOpenCompany
            // 
            this.MenuActionsOpenCompany.Name = "MenuActionsOpenCompany";
            this.MenuActionsOpenCompany.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuActionsOpenCompany.Size = new System.Drawing.Size(210, 22);
            this.MenuActionsOpenCompany.Text = "Bedrijf &Openen";
            this.MenuActionsOpenCompany.Click += new System.EventHandler(this.MenuActionsOpenCompany_Click);
            // 
            // MenuActionsNewCompany
            // 
            this.MenuActionsNewCompany.Name = "MenuActionsNewCompany";
            this.MenuActionsNewCompany.Size = new System.Drawing.Size(210, 22);
            this.MenuActionsNewCompany.Text = "&Nieuw Bedrijf installeren";
            this.MenuActionsNewCompany.Click += new System.EventHandler(this.MenuActionsNewCompany_Click);
            // 
            // MenuActiesCloseCompany
            // 
            this.MenuActiesCloseCompany.Enabled = false;
            this.MenuActiesCloseCompany.Name = "MenuActiesCloseCompany";
            this.MenuActiesCloseCompany.Size = new System.Drawing.Size(210, 22);
            this.MenuActiesCloseCompany.Text = "&Bedrijf Sluiten";
            this.MenuActiesCloseCompany.Click += new System.EventHandler(this.MenuActiesCloseCompany_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // MenuActionsMarSync
            // 
            this.MenuActionsMarSync.Name = "MenuActionsMarSync";
            this.MenuActionsMarSync.Size = new System.Drawing.Size(210, 22);
            this.MenuActionsMarSync.Text = "MarSync Starten";
            this.MenuActionsMarSync.Click += new System.EventHandler(this.MenuActionsMarSync_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // MenuActiesManager2009
            // 
            this.MenuActiesManager2009.Name = "MenuActiesManager2009";
            this.MenuActiesManager2009.Size = new System.Drawing.Size(210, 22);
            this.MenuActiesManager2009.Text = "Manager Standaard 2009";
            this.MenuActiesManager2009.Click += new System.EventHandler(this.MenuActiesManager2009_Click);
            // 
            // MenuActiesManager2005
            // 
            this.MenuActiesManager2005.Name = "MenuActiesManager2005";
            this.MenuActiesManager2005.Size = new System.Drawing.Size(210, 22);
            this.MenuActiesManager2005.Text = "Manager Standaard 2005";
            this.MenuActiesManager2005.Click += new System.EventHandler(this.MenuActiesManager2005_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(207, 6);
            // 
            // MenuActiesXmlRekenbladen
            // 
            this.MenuActiesXmlRekenbladen.Enabled = false;
            this.MenuActiesXmlRekenbladen.Name = "MenuActiesXmlRekenbladen";
            this.MenuActiesXmlRekenbladen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.MenuActiesXmlRekenbladen.Size = new System.Drawing.Size(210, 22);
            this.MenuActiesXmlRekenbladen.Text = "&XML Rekenbladen";
            // 
            // MenuActionsCloseApp
            // 
            this.MenuActionsCloseApp.Name = "MenuActionsCloseApp";
            this.MenuActionsCloseApp.Size = new System.Drawing.Size(210, 22);
            this.MenuActionsCloseApp.Text = "&Afsluiten (Alt+F4)";
            this.MenuActionsCloseApp.Click += new System.EventHandler(this.MenuActionsCloseApp_Click);
            // 
            // SystemSubMenu
            // 
            this.SystemSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemSetup,
            this.MenuItemBYPERDAT,
            this.MenuItemTemplateVPE,
            this.toolStripSeparator4,
            this.MenuItemSQL,
            this.toolStripSeparator6,
            this.MenuItemEditCompanyName});
            this.SystemSubMenu.Name = "SystemSubMenu";
            this.SystemSubMenu.Size = new System.Drawing.Size(63, 20);
            this.SystemSubMenu.Text = "&Systeem";
            // 
            // MenuItemSetup
            // 
            this.MenuItemSetup.Name = "MenuItemSetup";
            this.MenuItemSetup.Size = new System.Drawing.Size(259, 22);
            this.MenuItemSetup.Text = "Set-Up Boekjaar En Parameters";
            this.MenuItemSetup.Click += new System.EventHandler(this.MenuItemSetup_Click);
            // 
            // MenuItemBYPERDAT
            // 
            this.MenuItemBYPERDAT.Name = "MenuItemBYPERDAT";
            this.MenuItemBYPERDAT.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.MenuItemBYPERDAT.Size = new System.Drawing.Size(259, 22);
            this.MenuItemBYPERDAT.Text = "Datum / Periode / Boekjaar";
            this.MenuItemBYPERDAT.Click += new System.EventHandler(this.MenuItemBYPERDAT_Click);
            // 
            // MenuItemTemplateVPE
            // 
            this.MenuItemTemplateVPE.Name = "MenuItemTemplateVPE";
            this.MenuItemTemplateVPE.Size = new System.Drawing.Size(259, 22);
            this.MenuItemTemplateVPE.Text = "Lay-Out Uitgaand Document";
            this.MenuItemTemplateVPE.Click += new System.EventHandler(this.MenuItemTemplateVPE_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(256, 6);
            // 
            // MenuItemSQL
            // 
            this.MenuItemSQL.Name = "MenuItemSQL";
            this.MenuItemSQL.Size = new System.Drawing.Size(259, 22);
            this.MenuItemSQL.Text = "SQL Bewerkingen";
            this.MenuItemSQL.Click += new System.EventHandler(this.SQLToolMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(256, 6);
            // 
            // MenuItemEditCompanyName
            // 
            this.MenuItemEditCompanyName.Name = "MenuItemEditCompanyName";
            this.MenuItemEditCompanyName.Size = new System.Drawing.Size(259, 22);
            this.MenuItemEditCompanyName.Text = "Bedrijfsnaam wijzigen";
            this.MenuItemEditCompanyName.Click += new System.EventHandler(this.MenuItemEditCompanyName_Click);
            // 
            // WindowSubMenu
            // 
            this.WindowSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuCascadeOpenForms,
            this.MenuTileOpenFormsVertical,
            this.MenuTileOpenFormsHorizontal,
            this.MenuArrangeOpenForms});
            this.WindowSubMenu.Name = "WindowSubMenu";
            this.WindowSubMenu.Size = new System.Drawing.Size(57, 20);
            this.WindowSubMenu.Text = "&Venster";
            // 
            // MenuCascadeOpenForms
            // 
            this.MenuCascadeOpenForms.Name = "MenuCascadeOpenForms";
            this.MenuCascadeOpenForms.Size = new System.Drawing.Size(202, 22);
            this.MenuCascadeOpenForms.Text = "&Trapsgewijs";
            this.MenuCascadeOpenForms.Click += new System.EventHandler(this.MenuWindowCascade_Click);
            // 
            // MenuTileOpenFormsVertical
            // 
            this.MenuTileOpenFormsVertical.Name = "MenuTileOpenFormsVertical";
            this.MenuTileOpenFormsVertical.Size = new System.Drawing.Size(202, 22);
            this.MenuTileOpenFormsVertical.Text = "&Onder elkaar";
            this.MenuTileOpenFormsVertical.Click += new System.EventHandler(this.MenuWindowTileVertical_Click);
            // 
            // MenuTileOpenFormsHorizontal
            // 
            this.MenuTileOpenFormsHorizontal.Name = "MenuTileOpenFormsHorizontal";
            this.MenuTileOpenFormsHorizontal.Size = new System.Drawing.Size(202, 22);
            this.MenuTileOpenFormsHorizontal.Text = "&Naast elkaar";
            this.MenuTileOpenFormsHorizontal.Click += new System.EventHandler(this.MenuWindowTileHorizontal_Click);
            // 
            // MenuArrangeOpenForms
            // 
            this.MenuArrangeOpenForms.Name = "MenuArrangeOpenForms";
            this.MenuArrangeOpenForms.Size = new System.Drawing.Size(202, 22);
            this.MenuArrangeOpenForms.Text = "&Pictogrammen schikken";
            this.MenuArrangeOpenForms.Click += new System.EventHandler(this.MenuWindowArrangeIcons_Click);
            // 
            // FilesSubMenu
            // 
            this.FilesSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripDashBoard,
            this.toolStripSeparator10,
            this.ToolStripCustomers,
            this.ToolStripSuppliers,
            this.ToolStripLedgerAccounts,
            this.ToolStripProducts,
            this.ToolStripVariousDataSheets,
            this.toolStripSeparator11,
            this.ToolStripBasicTableReporting,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripSeparator12,
            this.ToolStripLedgerOnScreen});
            this.FilesSubMenu.Name = "FilesSubMenu";
            this.FilesSubMenu.Size = new System.Drawing.Size(52, 20);
            this.FilesSubMenu.Text = "&Fiches";
            // 
            // ToolStripDashBoard
            // 
            this.ToolStripDashBoard.Enabled = false;
            this.ToolStripDashBoard.Name = "ToolStripDashBoard";
            this.ToolStripDashBoard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.ToolStripDashBoard.Size = new System.Drawing.Size(272, 22);
            this.ToolStripDashBoard.Text = "Dashboard";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(269, 6);
            // 
            // ToolStripCustomers
            // 
            this.ToolStripCustomers.Name = "ToolStripCustomers";
            this.ToolStripCustomers.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.ToolStripCustomers.Size = new System.Drawing.Size(272, 22);
            this.ToolStripCustomers.Text = "Klanten";
            this.ToolStripCustomers.Click += new System.EventHandler(this.ToolStripCustomers_Click);
            // 
            // ToolStripSuppliers
            // 
            this.ToolStripSuppliers.Name = "ToolStripSuppliers";
            this.ToolStripSuppliers.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.ToolStripSuppliers.Size = new System.Drawing.Size(272, 22);
            this.ToolStripSuppliers.Text = "Leveranciers";
            this.ToolStripSuppliers.Click += new System.EventHandler(this.ToolStripSuppliers_Click);
            // 
            // ToolStripLedgerAccounts
            // 
            this.ToolStripLedgerAccounts.Name = "ToolStripLedgerAccounts";
            this.ToolStripLedgerAccounts.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ToolStripLedgerAccounts.Size = new System.Drawing.Size(272, 22);
            this.ToolStripLedgerAccounts.Text = "Algemene Rekeningen";
            this.ToolStripLedgerAccounts.Click += new System.EventHandler(this.ToolStripLedgerAccounts_Click);
            // 
            // ToolStripProducts
            // 
            this.ToolStripProducts.Name = "ToolStripProducts";
            this.ToolStripProducts.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.ToolStripProducts.Size = new System.Drawing.Size(272, 22);
            this.ToolStripProducts.Text = "Artikel/Product/Dienst";
            this.ToolStripProducts.Click += new System.EventHandler(this.ToolStripProducts_Click);
            // 
            // ToolStripVariousDataSheets
            // 
            this.ToolStripVariousDataSheets.Name = "ToolStripVariousDataSheets";
            this.ToolStripVariousDataSheets.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.ToolStripVariousDataSheets.Size = new System.Drawing.Size(272, 22);
            this.ToolStripVariousDataSheets.Text = "Diverse Gebruikerfiches";
            this.ToolStripVariousDataSheets.Click += new System.EventHandler(this.ToolStripVariousDataSheets_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(269, 6);
            // 
            // ToolStripBasicTableReporting
            // 
            this.ToolStripBasicTableReporting.Name = "ToolStripBasicTableReporting";
            this.ToolStripBasicTableReporting.Size = new System.Drawing.Size(272, 22);
            this.ToolStripBasicTableReporting.Text = "Lijstrapportage";
            this.ToolStripBasicTableReporting.Click += new System.EventHandler(this.ToolStripBasicTableReporting_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Enabled = false;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(272, 22);
            this.toolStripMenuItem5.Text = "Bestanden Importeren";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Enabled = false;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(272, 22);
            this.toolStripMenuItem6.Text = "Grafische voorstelling";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(269, 6);
            // 
            // ToolStripLedgerOnScreen
            // 
            this.ToolStripLedgerOnScreen.Name = "ToolStripLedgerOnScreen";
            this.ToolStripLedgerOnScreen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.ToolStripLedgerOnScreen.Size = new System.Drawing.Size(272, 22);
            this.ToolStripLedgerOnScreen.Text = "Historiek Grootboek rekening";
            this.ToolStripLedgerOnScreen.Click += new System.EventHandler(this.ToolStripLedgerOnScreen_Click);
            // 
            // DailyManagementSubMenu
            // 
            this.DailyManagementSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripBuying,
            this.ToolStripSelling,
            this.ToolStripFinancial,
            this.toolStripSeparator13,
            this.ToolStripCashRegister,
            this.toolStripSeparator14,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator15,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.ToolStripCDD});
            this.DailyManagementSubMenu.Name = "DailyManagementSubMenu";
            this.DailyManagementSubMenu.Size = new System.Drawing.Size(75, 20);
            this.DailyManagementSubMenu.Text = "&Document";
            // 
            // ToolStripBuying
            // 
            this.ToolStripBuying.Name = "ToolStripBuying";
            this.ToolStripBuying.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F1)));
            this.ToolStripBuying.Size = new System.Drawing.Size(282, 22);
            this.ToolStripBuying.Text = "Aankoop";
            this.ToolStripBuying.Click += new System.EventHandler(this.ToolStripBuying_Click);
            // 
            // ToolStripSelling
            // 
            this.ToolStripSelling.Enabled = false;
            this.ToolStripSelling.Name = "ToolStripSelling";
            this.ToolStripSelling.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F2)));
            this.ToolStripSelling.Size = new System.Drawing.Size(282, 22);
            this.ToolStripSelling.Text = "Verkoop";
            // 
            // ToolStripFinancial
            // 
            this.ToolStripFinancial.Enabled = false;
            this.ToolStripFinancial.Name = "ToolStripFinancial";
            this.ToolStripFinancial.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F3)));
            this.ToolStripFinancial.Size = new System.Drawing.Size(282, 22);
            this.ToolStripFinancial.Text = "Financiëel";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(279, 6);
            // 
            // ToolStripCashRegister
            // 
            this.ToolStripCashRegister.Enabled = false;
            this.ToolStripCashRegister.Name = "ToolStripCashRegister";
            this.ToolStripCashRegister.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F6)));
            this.ToolStripCashRegister.Size = new System.Drawing.Size(282, 22);
            this.ToolStripCashRegister.Text = "Kassaverkoop";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(279, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(282, 22);
            this.toolStripMenuItem1.Text = "Rekeninguittreksel";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(282, 22);
            this.toolStripMenuItem2.Text = "Betalingsbestand";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(279, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Enabled = false;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(282, 22);
            this.toolStripMenuItem3.Text = "Standaardkostprijskaart";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Enabled = false;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItem4.Size = new System.Drawing.Size(282, 22);
            this.toolStripMenuItem4.Text = "Briefwisseling";
            // 
            // ToolStripCDD
            // 
            this.ToolStripCDD.Name = "ToolStripCDD";
            this.ToolStripCDD.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.ToolStripCDD.Size = new System.Drawing.Size(282, 22);
            this.ToolStripCDD.Text = "Domiciliëring Schuldeiser";
            this.ToolStripCDD.Click += new System.EventHandler(this.ToolStripCDD_Click);
            // 
            // AccountingSubMenu
            // 
            this.AccountingSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripManualLedgerEntry,
            this.toolStripSeparator5,
            this.ToolStripPurchaseLedger,
            this.ToolStripSalesLedger,
            this.ToolStripFinancialBook,
            this.ToolStripLedgerBook,
            this.toolStripSeparator7,
            this.ToolStripEUVatListingQuarterly,
            this.ToolstripBEVATListingYearly,
            this.ToolStripIntrastat19,
            this.ToolStripIntrastat29,
            this.ToolStripVatDeclaration,
            this.toolStripSeparator8,
            this.ToolStripProductInventoryCheck,
            this.ToolStripHistoryGeneralLedger,
            this.ToolStripCustomersBalance,
            this.ToolStripCustomersTopdown,
            this.ToolStripSuppliersBalance,
            this.ToolStripSuppliersTopdown,
            this.ToolStripTrialBalance,
            this.toolStripSeparator9,
            this.ToolStripFinalReport,
            this.ToolStripTransitionProgram});
            this.AccountingSubMenu.Name = "AccountingSubMenu";
            this.AccountingSubMenu.Size = new System.Drawing.Size(90, 20);
            this.AccountingSubMenu.Text = "&Boekhouding";
            // 
            // ToolStripManualLedgerEntry
            // 
            this.ToolStripManualLedgerEntry.Name = "ToolStripManualLedgerEntry";
            this.ToolStripManualLedgerEntry.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.ToolStripManualLedgerEntry.Size = new System.Drawing.Size(300, 22);
            this.ToolStripManualLedgerEntry.Text = "Diverse Posten";
            this.ToolStripManualLedgerEntry.Click += new System.EventHandler(this.ToolStripManualLedgerEntry_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(297, 6);
            // 
            // ToolStripPurchaseLedger
            // 
            this.ToolStripPurchaseLedger.Name = "ToolStripPurchaseLedger";
            this.ToolStripPurchaseLedger.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1)));
            this.ToolStripPurchaseLedger.Size = new System.Drawing.Size(300, 22);
            this.ToolStripPurchaseLedger.Text = "Aankoopboek";
            this.ToolStripPurchaseLedger.Click += new System.EventHandler(this.ToolStripPurchaseLedger_Click);
            // 
            // ToolStripSalesLedger
            // 
            this.ToolStripSalesLedger.Name = "ToolStripSalesLedger";
            this.ToolStripSalesLedger.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F2)));
            this.ToolStripSalesLedger.Size = new System.Drawing.Size(300, 22);
            this.ToolStripSalesLedger.Text = "Verkoopboek";
            this.ToolStripSalesLedger.Click += new System.EventHandler(this.ToolStripSalesLedger_Click);
            // 
            // ToolStripFinancialBook
            // 
            this.ToolStripFinancialBook.Name = "ToolStripFinancialBook";
            this.ToolStripFinancialBook.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F3)));
            this.ToolStripFinancialBook.Size = new System.Drawing.Size(300, 22);
            this.ToolStripFinancialBook.Text = "Financiëel Grootboek";
            this.ToolStripFinancialBook.Click += new System.EventHandler(this.ToolStripFinancialBook_Click);
            // 
            // ToolStripLedgerBook
            // 
            this.ToolStripLedgerBook.Name = "ToolStripLedgerBook";
            this.ToolStripLedgerBook.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.ToolStripLedgerBook.Size = new System.Drawing.Size(300, 22);
            this.ToolStripLedgerBook.Text = "Diverse Posten Grootboek";
            this.ToolStripLedgerBook.Click += new System.EventHandler(this.ToolStripLedgerBook_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(297, 6);
            // 
            // ToolStripEUVatListingQuarterly
            // 
            this.ToolStripEUVatListingQuarterly.Enabled = false;
            this.ToolStripEUVatListingQuarterly.Name = "ToolStripEUVatListingQuarterly";
            this.ToolStripEUVatListingQuarterly.Size = new System.Drawing.Size(300, 22);
            this.ToolStripEUVatListingQuarterly.Text = "EU Btw Kwartaallisting";
            // 
            // ToolstripBEVATListingYearly
            // 
            this.ToolstripBEVATListingYearly.Enabled = false;
            this.ToolstripBEVATListingYearly.Name = "ToolstripBEVATListingYearly";
            this.ToolstripBEVATListingYearly.Size = new System.Drawing.Size(300, 22);
            this.ToolstripBEVATListingYearly.Text = "BTW Jaarlisting Binnenland";
            // 
            // ToolStripIntrastat19
            // 
            this.ToolStripIntrastat19.Enabled = false;
            this.ToolStripIntrastat19.Name = "ToolStripIntrastat19";
            this.ToolStripIntrastat19.Size = new System.Drawing.Size(300, 22);
            this.ToolStripIntrastat19.Text = "Intrastat 19";
            // 
            // ToolStripIntrastat29
            // 
            this.ToolStripIntrastat29.Enabled = false;
            this.ToolStripIntrastat29.Name = "ToolStripIntrastat29";
            this.ToolStripIntrastat29.Size = new System.Drawing.Size(300, 22);
            this.ToolStripIntrastat29.Text = "Intrastat 29";
            // 
            // ToolStripVatDeclaration
            // 
            this.ToolStripVatDeclaration.Name = "ToolStripVatDeclaration";
            this.ToolStripVatDeclaration.Size = new System.Drawing.Size(300, 22);
            this.ToolStripVatDeclaration.Text = "Stand Btw Aangifte";
            this.ToolStripVatDeclaration.Click += new System.EventHandler(this.ToolStripVatDeclaration_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(297, 6);
            // 
            // ToolStripProductInventoryCheck
            // 
            this.ToolStripProductInventoryCheck.Enabled = false;
            this.ToolStripProductInventoryCheck.Name = "ToolStripProductInventoryCheck";
            this.ToolStripProductInventoryCheck.Size = new System.Drawing.Size(300, 22);
            this.ToolStripProductInventoryCheck.Text = "Inventaris Producten Controle";
            // 
            // ToolStripHistoryGeneralLedger
            // 
            this.ToolStripHistoryGeneralLedger.Name = "ToolStripHistoryGeneralLedger";
            this.ToolStripHistoryGeneralLedger.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
            this.ToolStripHistoryGeneralLedger.Size = new System.Drawing.Size(300, 22);
            this.ToolStripHistoryGeneralLedger.Text = "Historiek Algemene Rekenignen";
            this.ToolStripHistoryGeneralLedger.Click += new System.EventHandler(this.ToolStripHistoryGeneralLedger_Click);
            // 
            // ToolStripCustomersBalance
            // 
            this.ToolStripCustomersBalance.Name = "ToolStripCustomersBalance";
            this.ToolStripCustomersBalance.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F7)));
            this.ToolStripCustomersBalance.Size = new System.Drawing.Size(300, 22);
            this.ToolStripCustomersBalance.Text = "Balans Klanten";
            this.ToolStripCustomersBalance.Click += new System.EventHandler(this.ToolStripCustomersBalance_Click);
            // 
            // ToolStripCustomersTopdown
            // 
            this.ToolStripCustomersTopdown.Enabled = false;
            this.ToolStripCustomersTopdown.Name = "ToolStripCustomersTopdown";
            this.ToolStripCustomersTopdown.Size = new System.Drawing.Size(300, 22);
            this.ToolStripCustomersTopdown.Text = "Top-Down Klanten";
            // 
            // ToolStripSuppliersBalance
            // 
            this.ToolStripSuppliersBalance.Name = "ToolStripSuppliersBalance";
            this.ToolStripSuppliersBalance.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F8)));
            this.ToolStripSuppliersBalance.Size = new System.Drawing.Size(300, 22);
            this.ToolStripSuppliersBalance.Text = "Balans Leveranciers";
            this.ToolStripSuppliersBalance.Click += new System.EventHandler(this.ToolStripSuppliersBalance_Click);
            // 
            // ToolStripSuppliersTopdown
            // 
            this.ToolStripSuppliersTopdown.Enabled = false;
            this.ToolStripSuppliersTopdown.Name = "ToolStripSuppliersTopdown";
            this.ToolStripSuppliersTopdown.Size = new System.Drawing.Size(300, 22);
            this.ToolStripSuppliersTopdown.Text = "Top-Down Leveranciers";
            // 
            // ToolStripTrialBalance
            // 
            this.ToolStripTrialBalance.Name = "ToolStripTrialBalance";
            this.ToolStripTrialBalance.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F12)));
            this.ToolStripTrialBalance.Size = new System.Drawing.Size(300, 22);
            this.ToolStripTrialBalance.Text = "Proef- en Saldibalans";
            this.ToolStripTrialBalance.Click += new System.EventHandler(this.ToolStripTrialBalance_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(297, 6);
            // 
            // ToolStripFinalReport
            // 
            this.ToolStripFinalReport.Enabled = false;
            this.ToolStripFinalReport.Name = "ToolStripFinalReport";
            this.ToolStripFinalReport.Size = new System.Drawing.Size(300, 22);
            this.ToolStripFinalReport.Text = "Eindrapportage";
            // 
            // ToolStripTransitionProgram
            // 
            this.ToolStripTransitionProgram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripNewFinancialYear});
            this.ToolStripTransitionProgram.Name = "ToolStripTransitionProgram";
            this.ToolStripTransitionProgram.Size = new System.Drawing.Size(300, 22);
            this.ToolStripTransitionProgram.Text = "Overgangsprogramma\'s";
            // 
            // ToolStripNewFinancialYear
            // 
            this.ToolStripNewFinancialYear.Name = "ToolStripNewFinancialYear";
            this.ToolStripNewFinancialYear.Size = new System.Drawing.Size(156, 22);
            this.ToolStripNewFinancialYear.Text = "Nieuw Boekjaar";
            this.ToolStripNewFinancialYear.Click += new System.EventHandler(this.ToolStripNewFinancialYear_Click);
            // 
            // ContractsSubMenu
            // 
            this.ContractsSubMenu.Name = "ContractsSubMenu";
            this.ContractsSubMenu.Size = new System.Drawing.Size(101, 20);
            this.ContractsSubMenu.Text = "&Contractbeheer";
            // 
            // CloudSubMenu
            // 
            this.CloudSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuCloud4MarInstellingen,
            this.MenuCloud4MarWebsite});
            this.CloudSubMenu.Name = "CloudSubMenu";
            this.CloudSubMenu.Size = new System.Drawing.Size(75, 20);
            this.CloudSubMenu.Text = "CloudData";
            // 
            // MenuCloud4MarInstellingen
            // 
            this.MenuCloud4MarInstellingen.Name = "MenuCloud4MarInstellingen";
            this.MenuCloud4MarInstellingen.Size = new System.Drawing.Size(156, 22);
            this.MenuCloud4MarInstellingen.Text = "Instellingen";
            this.MenuCloud4MarInstellingen.Click += new System.EventHandler(this.MenuCloud4MarInstellingen_Click);
            // 
            // MenuCloud4MarWebsite
            // 
            this.MenuCloud4MarWebsite.Name = "MenuCloud4MarWebsite";
            this.MenuCloud4MarWebsite.Size = new System.Drawing.Size(156, 22);
            this.MenuCloud4MarWebsite.Text = "https://vsoft.be";
            // 
            // SettingsSubMenu
            // 
            this.SettingsSubMenu.Name = "SettingsSubMenu";
            this.SettingsSubMenu.Size = new System.Drawing.Size(80, 20);
            this.SettingsSubMenu.Text = "&Instellingen";
            // 
            // HelpSubMenu
            // 
            this.HelpSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HostingInfo,
            this.LinkAccounting,
            this.LinkLicence,
            this.CommandPrompt,
            this.PeppolInfo,
            this.GitHub});
            this.HelpSubMenu.Name = "HelpSubMenu";
            this.HelpSubMenu.Size = new System.Drawing.Size(24, 20);
            this.HelpSubMenu.Text = "&?";
            // 
            // HostingInfo
            // 
            this.HostingInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LinkObsidian,
            this.LinkWebmail});
            this.HostingInfo.Name = "HostingInfo";
            this.HostingInfo.Size = new System.Drawing.Size(280, 22);
            this.HostingInfo.Text = "Plesk Hosting";
            // 
            // LinkObsidian
            // 
            this.LinkObsidian.Name = "LinkObsidian";
            this.LinkObsidian.Size = new System.Drawing.Size(176, 22);
            this.LinkObsidian.Text = "Obsidian 18.0.65";
            this.LinkObsidian.Click += new System.EventHandler(this.LinkObsidian_Click);
            // 
            // LinkWebmail
            // 
            this.LinkWebmail.Name = "LinkWebmail";
            this.LinkWebmail.Size = new System.Drawing.Size(176, 22);
            this.LinkWebmail.Text = "Webmail rvServices";
            this.LinkWebmail.Click += new System.EventHandler(this.LinkWebmail_Click);
            // 
            // LinkAccounting
            // 
            this.LinkAccounting.Name = "LinkAccounting";
            this.LinkAccounting.Size = new System.Drawing.Size(280, 22);
            this.LinkAccounting.Text = "Commissie Boekhoudkundige Normen";
            this.LinkAccounting.Click += new System.EventHandler(this.LinkAccounting_Click);
            // 
            // LinkLicence
            // 
            this.LinkLicence.Name = "LinkLicence";
            this.LinkLicence.Size = new System.Drawing.Size(280, 22);
            this.LinkLicence.Text = "&Licentie toewijzing";
            this.LinkLicence.Click += new System.EventHandler(this.LinkLicence_Click);
            // 
            // CommandPrompt
            // 
            this.CommandPrompt.Name = "CommandPrompt";
            this.CommandPrompt.Size = new System.Drawing.Size(280, 22);
            this.CommandPrompt.Text = "Command Prompt";
            this.CommandPrompt.Click += new System.EventHandler(this.CommandPrompt_Click);
            // 
            // PeppolInfo
            // 
            this.PeppolInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LinkPeppolValidator,
            this.LinkPeppolDocs,
            this.LinkMarSyncClickOnce});
            this.PeppolInfo.Name = "PeppolInfo";
            this.PeppolInfo.Size = new System.Drawing.Size(280, 22);
            this.PeppolInfo.Text = "Peppol";
            // 
            // LinkPeppolValidator
            // 
            this.LinkPeppolValidator.Name = "LinkPeppolValidator";
            this.LinkPeppolValidator.Size = new System.Drawing.Size(180, 22);
            this.LinkPeppolValidator.Text = "Document Validator";
            this.LinkPeppolValidator.Click += new System.EventHandler(this.LinkPeppolValidator_Click);
            // 
            // LinkPeppolDocs
            // 
            this.LinkPeppolDocs.Name = "LinkPeppolDocs";
            this.LinkPeppolDocs.Size = new System.Drawing.Size(180, 22);
            this.LinkPeppolDocs.Text = "BIS Billing 3.0";
            this.LinkPeppolDocs.Click += new System.EventHandler(this.LinkPeppolDocs_Click);
            // 
            // LinkMarSyncClickOnce
            // 
            this.LinkMarSyncClickOnce.Name = "LinkMarSyncClickOnce";
            this.LinkMarSyncClickOnce.Size = new System.Drawing.Size(180, 22);
            this.LinkMarSyncClickOnce.Text = "ClickOnce MarSync";
            this.LinkMarSyncClickOnce.Click += new System.EventHandler(this.LinkMarSyncClickOnce_Click);
            // 
            // GitHub
            // 
            this.GitHub.Name = "GitHub";
            this.GitHub.Size = new System.Drawing.Size(280, 22);
            this.GitHub.Text = "SourceCode on GitHub";
            this.GitHub.Click += new System.EventHandler(this.GitHub_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.AutoSize = false;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripBookingDate,
            this.toolStripSep1,
            this.toolStripLabelBoekModus,
            this.cmdWegBoekModus,
            this.TbOpenCompany,
            this.TbArchiveMap,
            this.TbMarioMap,
            this.toolStripSep2});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 24);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(831, 44);
            this.mainToolStrip.TabIndex = 12;
            this.mainToolStrip.Text = "mainToolStrip";
            // 
            // ToolStripBookingDate
            // 
            this.ToolStripBookingDate.Name = "ToolStripBookingDate";
            this.ToolStripBookingDate.Size = new System.Drawing.Size(77, 41);
            this.ToolStripBookingDate.Text = "dd/MM/yyyy";
            this.ToolStripBookingDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ToolStripBookingDate.ToolTipText = "Datum verwerking";
            // 
            // toolStripSep1
            // 
            this.toolStripSep1.Name = "toolStripSep1";
            this.toolStripSep1.Size = new System.Drawing.Size(6, 44);
            // 
            // toolStripLabelBoekModus
            // 
            this.toolStripLabelBoekModus.Name = "toolStripLabelBoekModus";
            this.toolStripLabelBoekModus.Size = new System.Drawing.Size(73, 41);
            this.toolStripLabelBoekModus.Text = "BoekModus:";
            // 
            // cmdWegBoekModus
            // 
            this.cmdWegBoekModus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdWegBoekModus.Name = "cmdWegBoekModus";
            this.cmdWegBoekModus.Size = new System.Drawing.Size(180, 44);
            this.cmdWegBoekModus.ToolTipText = "Boekingsmodus selecteren";
            this.cmdWegBoekModus.SelectedIndexChanged += new System.EventHandler(this.CmdWegBoekModus_SelectedIndexChanged);
            // 
            // TbOpenCompany
            // 
            this.TbOpenCompany.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TbOpenCompany.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbOpenCompany.Image = global::marVSS2028.Properties.Resources.OPENFOLD_LARGE;
            this.TbOpenCompany.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TbOpenCompany.Name = "TbOpenCompany";
            this.TbOpenCompany.Size = new System.Drawing.Size(61, 41);
            this.TbOpenCompany.Text = "Bedrijf";
            this.TbOpenCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TbOpenCompany.ToolTipText = "Bedrijf openen (Ctrl+O)";
            this.TbOpenCompany.Click += new System.EventHandler(this.TbOpenCompany_Click);
            // 
            // TbArchiveMap
            // 
            this.TbArchiveMap.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TbArchiveMap.Image = global::marVSS2028.Properties.Resources.WebFolderOpened;
            this.TbArchiveMap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TbArchiveMap.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TbArchiveMap.Name = "TbArchiveMap";
            this.TbArchiveMap.Size = new System.Drawing.Size(65, 41);
            this.TbArchiveMap.Text = "Archief";
            this.TbArchiveMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TbArchiveMap.ToolTipText = "Cloud Map Archief";
            this.TbArchiveMap.Click += new System.EventHandler(this.TbArchiveMap_Click);
            // 
            // TbMarioMap
            // 
            this.TbMarioMap.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TbMarioMap.Image = global::marVSS2028.Properties.Resources.WebFolderOpened;
            this.TbMarioMap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TbMarioMap.Name = "TbMarioMap";
            this.TbMarioMap.Size = new System.Drawing.Size(58, 41);
            this.TbMarioMap.Text = "Mario";
            this.TbMarioMap.ToolTipText = "Cloud Map Manueel";
            this.TbMarioMap.Click += new System.EventHandler(this.TbMarioMap_Click);
            // 
            // toolStripSep2
            // 
            this.toolStripSep2.Name = "toolStripSep2";
            this.toolStripSep2.Size = new System.Drawing.Size(6, 44);
            // 
            // SnelHelp
            // 
            this.SnelHelp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabel1,
            this.ToolStripLabel2,
            this.ToolStripLabel3,
            this.SnelHelpLabel});
            this.SnelHelp.Location = new System.Drawing.Point(0, 455);
            this.SnelHelp.Name = "SnelHelp";
            this.SnelHelp.Size = new System.Drawing.Size(831, 24);
            this.SnelHelp.TabIndex = 20;
            // 
            // ToolStripLabel1
            // 
            this.ToolStripLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.ToolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripLabel1.Name = "ToolStripLabel1";
            this.ToolStripLabel1.Size = new System.Drawing.Size(94, 19);
            this.ToolStripLabel1.Tag = "";
            this.ToolStripLabel1.Text = "Standaard KMO";
            this.ToolStripLabel1.ToolTipText = "Bedrijfstype";
            // 
            // ToolStripLabel2
            // 
            this.ToolStripLabel2.AutoSize = false;
            this.ToolStripLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.ToolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripLabel2.Name = "ToolStripLabel2";
            this.ToolStripLabel2.Size = new System.Drawing.Size(28, 19);
            this.ToolStripLabel2.Text = "EUR";
            this.ToolStripLabel2.ToolTipText = "Munt van het boekjaar";
            // 
            // ToolStripLabel3
            // 
            this.ToolStripLabel3.AutoSize = false;
            this.ToolStripLabel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.ToolStripLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.ToolStripLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripLabel3.Name = "ToolStripLabel3";
            this.ToolStripLabel3.Size = new System.Drawing.Size(30, 19);
            this.ToolStripLabel3.Text = "JET4";
            this.ToolStripLabel3.ToolTipText = "JET4 of SQL Server";
            // 
            // SnelHelpLabel
            // 
            this.SnelHelpLabel.AutoSize = false;
            this.SnelHelpLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.SnelHelpLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.SnelHelpLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SnelHelpLabel.Name = "SnelHelpLabel";
            this.SnelHelpLabel.Size = new System.Drawing.Size(664, 19);
            this.SnelHelpLabel.Spring = true;
            this.SnelHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SnelHelpLabel.ToolTipText = "Info, tips en hulp";
            // 
            // SnelHelpTijd
            // 
            this.SnelHelpTijd.Interval = 5000;
            this.SnelHelpTijd.Tick += new System.EventHandler(this.SnelHelpTijd_Tick);
            // 
            // InfoData
            // 
            this.InfoData.Location = new System.Drawing.Point(33, 137);
            this.InfoData.Name = "InfoData";
            this.InfoData.Size = new System.Drawing.Size(100, 50);
            this.InfoData.TabIndex = 22;
            this.InfoData.TabStop = false;
            this.InfoData.Visible = false;
            // 
            // FormMim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 479);
            this.Controls.Add(this.InfoData);
            this.Controls.Add(this.SnelHelp);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "FormMim";
            this.Text = "marIntegraal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMim_FormClosing);
            this.Load += new System.EventHandler(this.FormMim_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.SnelHelp.ResumeLayout(false);
            this.SnelHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InfoData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem ToolStripCustomers;
        private System.Windows.Forms.ToolStripMenuItem ToolStripSuppliers;
        private System.Windows.Forms.ToolStripMenuItem ToolStripLedgerAccounts;
        private System.Windows.Forms.ToolStripMenuItem LinkObsidian;
        private System.Windows.Forms.ToolStripMenuItem LinkWebmail;

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton TbOpenCompany;
        private System.Windows.Forms.ToolStripButton TbMarioMap;
        private System.Windows.Forms.ToolStripButton TbArchiveMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSep1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelBoekModus;
        private System.Windows.Forms.ToolStripComboBox cmdWegBoekModus;
        private System.Windows.Forms.ToolStripSeparator toolStripSep2;
        public System.Windows.Forms.ToolStripLabel ToolStripBookingDate;
        private System.Windows.Forms.ToolStripMenuItem GitHub;
        public System.Windows.Forms.ToolStripMenuItem MenuItemSQL;
        internal System.Windows.Forms.StatusStrip SnelHelp;        
        internal System.Windows.Forms.ToolStripStatusLabel SnelHelpLabel;
        internal System.Windows.Forms.Timer SnelHelpTijd;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripLabel2;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripLabel1;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripLabel3;
        public System.Windows.Forms.PictureBox InfoData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSetup;
        private System.Windows.Forms.ToolStripMenuItem MenuItemBYPERDAT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTemplateVPE;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem ToolStripManualLedgerEntry;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem ToolStripPurchaseLedger;
        private System.Windows.Forms.ToolStripMenuItem ToolStripSalesLedger;
        private System.Windows.Forms.ToolStripMenuItem ToolStripFinancialBook;
        private System.Windows.Forms.ToolStripMenuItem ToolStripLedgerBook;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;        
        private ToolStripMenuItem ToolStripEUVatListingQuarterly;
        private ToolStripMenuItem ToolstripBEVATListingYearly;
        private ToolStripMenuItem ToolStripIntrastat19;
        private ToolStripMenuItem ToolStripIntrastat29;
        private ToolStripMenuItem ToolStripVatDeclaration;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem ToolStripProductInventoryCheck;
        private ToolStripMenuItem ToolStripHistoryGeneralLedger;
        private ToolStripMenuItem ToolStripCustomersBalance;
        private ToolStripMenuItem ToolStripCustomersTopdown;
        private ToolStripMenuItem ToolStripSuppliersBalance;
        private ToolStripMenuItem ToolStripSuppliersTopdown;
        private ToolStripMenuItem ToolStripTrialBalance;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem ToolStripFinalReport;
        private ToolStripMenuItem ToolStripTransitionProgram;
        private ToolStripMenuItem ToolStripNewFinancialYear;
        private ToolStripMenuItem MenuItemEditCompanyName;
        private ToolStripMenuItem ToolStripDashBoard;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem ToolStripProducts;
        private ToolStripMenuItem ToolStripVariousDataSheets;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem ToolStripBasicTableReporting;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripMenuItem ToolStripLedgerOnScreen;
        private ToolStripMenuItem ToolStripBuying;
        private ToolStripMenuItem ToolStripSelling;
        private ToolStripMenuItem ToolStripFinancial;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripMenuItem ToolStripCashRegister;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem ToolStripCDD;
    }
}

