using System.Windows.Forms;

namespace marVSS2028.MimMenu.DailyManagement
{
    partial class FormBuying
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.LabelDocumentReference = new System.Windows.Forms.Label();
            this.OMSproduct = new System.Windows.Forms.Label();
            this.TextBoxWarningTestCompany = new System.Windows.Forms.TextBox();
            this.CheckBoxAlwaysPeppolRefresh = new System.Windows.Forms.CheckBox();
            this.ButtonOptimize = new System.Windows.Forms.Button();
            this.cbCheckTools = new System.Windows.Forms.Button();
            this.cbImportUBL = new System.Windows.Forms.Button();
            this.Schoonvegen = new System.Windows.Forms.Button();
            this.ButtonControleIt = new System.Windows.Forms.Button();
            this.ButtonBookIt = new System.Windows.Forms.Button();
            this.Medekontraktant = new System.Windows.Forms.CheckBox();
            this.Annuleren = new System.Windows.Forms.Button();
            this.StockBeheer = new System.Windows.Forms.CheckBox();
            this.cmdSQLInfo = new System.Windows.Forms.Button();
            this.AankoopOptie2 = new System.Windows.Forms.RadioButton();
            this.AankoopOptie1 = new System.Windows.Forms.RadioButton();
            this.AankoopOptie0 = new System.Windows.Forms.RadioButton();
            this.AankoopDetail = new System.Windows.Forms.ListBox();
            this.TekstInfo4 = new System.Windows.Forms.MaskedTextBox();
            this.TekstInfo3 = new System.Windows.Forms.TextBox();
            this.TekstInfo6 = new System.Windows.Forms.TextBox();
            this.TekstInfo10 = new System.Windows.Forms.TextBox();
            this.TekstInfo2 = new System.Windows.Forms.MaskedTextBox();
            this.TekstInfo1 = new System.Windows.Forms.MaskedTextBox();
            this.TekstInfo0 = new System.Windows.Forms.MaskedTextBox();
            this.TekstInfo9 = new System.Windows.Forms.TextBox();
            this.TekstInfo7 = new System.Windows.Forms.TextBox();
            this.TekstInfo5 = new System.Windows.Forms.TextBox();
            this.TekstInfo12 = new System.Windows.Forms.TextBox();
            this.TextInfoSellersIBAN = new System.Windows.Forms.TextBox();
            this.SSTab1 = new System.Windows.Forms.TabControl();
            this.TabPageLeverancier = new System.Windows.Forms.TabPage();
            this.LeverancierInfo = new System.Windows.Forms.Label();
            this.TabPageBewerken = new System.Windows.Forms.TabPage();
            this.cmdXLog = new System.Windows.Forms.Button();
            this.LabelInfoXlog = new System.Windows.Forms.Label();
            this.TabPageHistoriek = new System.Windows.Forms.TabPage();
            this.ListView1 = new System.Windows.Forms.ListView();
            this.columnHeaderNaam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIdCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Label1_0 = new System.Windows.Forms.Label();
            this.Label1_1 = new System.Windows.Forms.Label();
            this.OMSLabel = new System.Windows.Forms.Label();
            this.Label1_3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1_5 = new System.Windows.Forms.Label();
            this.Label1_7 = new System.Windows.Forms.Label();
            this.Label1_8 = new System.Windows.Forms.Label();
            this.Label1_11 = new System.Windows.Forms.Label();
            this.Label1_12 = new System.Windows.Forms.Label();
            this.Label1_14 = new System.Windows.Forms.Label();
            this.TextWarningIBAN = new System.Windows.Forms.Label();
            this.SSTab1.SuspendLayout();
            this.TabPageLeverancier.SuspendLayout();
            this.TabPageBewerken.SuspendLayout();
            this.TabPageHistoriek.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelDocumentReference
            // 
            this.LabelDocumentReference.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelDocumentReference.Location = new System.Drawing.Point(14, 511);
            this.LabelDocumentReference.Name = "LabelDocumentReference";
            this.LabelDocumentReference.Size = new System.Drawing.Size(404, 13);
            this.LabelDocumentReference.TabIndex = 49;
            this.LabelDocumentReference.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OMSproduct
            // 
            this.OMSproduct.AutoSize = true;
            this.OMSproduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OMSproduct.Location = new System.Drawing.Point(452, 141);
            this.OMSproduct.Name = "OMSproduct";
            this.OMSproduct.Size = new System.Drawing.Size(120, 13);
            this.OMSproduct.TabIndex = 94;
            this.OMSproduct.Text = "Artikelnr.    Omschrijving";
            this.OMSproduct.Visible = false;
            // 
            // TextBoxWarningTestCompany
            // 
            this.TextBoxWarningTestCompany.BackColor = System.Drawing.Color.Red;
            this.TextBoxWarningTestCompany.Enabled = false;
            this.TextBoxWarningTestCompany.ForeColor = System.Drawing.Color.Black;
            this.TextBoxWarningTestCompany.Location = new System.Drawing.Point(415, 443);
            this.TextBoxWarningTestCompany.Name = "TextBoxWarningTestCompany";
            this.TextBoxWarningTestCompany.Size = new System.Drawing.Size(177, 20);
            this.TextBoxWarningTestCompany.TabIndex = 86;
            this.TextBoxWarningTestCompany.TabStop = false;
            this.TextBoxWarningTestCompany.Text = "Opgelet: Dit is een testbedrijf!";
            this.TextBoxWarningTestCompany.Visible = false;
            // 
            // CheckBoxAlwaysPeppolRefresh
            // 
            this.CheckBoxAlwaysPeppolRefresh.AutoSize = true;
            this.CheckBoxAlwaysPeppolRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxAlwaysPeppolRefresh.Location = new System.Drawing.Point(198, 16);
            this.CheckBoxAlwaysPeppolRefresh.Name = "CheckBoxAlwaysPeppolRefresh";
            this.CheckBoxAlwaysPeppolRefresh.Size = new System.Drawing.Size(93, 17);
            this.CheckBoxAlwaysPeppolRefresh.TabIndex = 90;
            this.CheckBoxAlwaysPeppolRefresh.TabStop = false;
            this.CheckBoxAlwaysPeppolRefresh.Text = "Check Peppol";
            this.CheckBoxAlwaysPeppolRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxAlwaysPeppolRefresh.UseVisualStyleBackColor = true;
            // 
            // ButtonOptimize
            // 
            this.ButtonOptimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOptimize.Location = new System.Drawing.Point(483, 341);
            this.ButtonOptimize.Name = "ButtonOptimize";
            this.ButtonOptimize.Size = new System.Drawing.Size(109, 24);
            this.ButtonOptimize.TabIndex = 89;
            this.ButtonOptimize.TabStop = false;
            this.ButtonOptimize.Text = "Dubbels Vermijden";
            this.ButtonOptimize.UseVisualStyleBackColor = true;
            // 
            // cbCheckTools
            // 
            this.cbCheckTools.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckTools.Location = new System.Drawing.Point(377, 71);
            this.cbCheckTools.Name = "cbCheckTools";
            this.cbCheckTools.Size = new System.Drawing.Size(95, 32);
            this.cbCheckTools.TabIndex = 85;
            this.cbCheckTools.TabStop = false;
            this.cbCheckTools.Text = "Check Tools";
            this.cbCheckTools.UseVisualStyleBackColor = true;
            // 
            // cbImportUBL
            // 
            this.cbImportUBL.Enabled = false;
            this.cbImportUBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbImportUBL.Location = new System.Drawing.Point(376, 106);
            this.cbImportUBL.Name = "cbImportUBL";
            this.cbImportUBL.Size = new System.Drawing.Size(96, 28);
            this.cbImportUBL.TabIndex = 53;
            this.cbImportUBL.TabStop = false;
            this.cbImportUBL.Text = "&UBL B2B IN";
            this.cbImportUBL.UseVisualStyleBackColor = true;
            // 
            // Schoonvegen
            // 
            this.Schoonvegen.Enabled = false;
            this.Schoonvegen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Schoonvegen.Location = new System.Drawing.Point(483, 472);
            this.Schoonvegen.Name = "Schoonvegen";
            this.Schoonvegen.Size = new System.Drawing.Size(109, 24);
            this.Schoonvegen.TabIndex = 78;
            this.Schoonvegen.TabStop = false;
            this.Schoonvegen.Text = "Sch&oon";
            this.Schoonvegen.UseVisualStyleBackColor = true;
            // 
            // ButtonControleIt
            // 
            this.ButtonControleIt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonControleIt.Location = new System.Drawing.Point(483, 374);
            this.ButtonControleIt.Name = "ButtonControleIt";
            this.ButtonControleIt.Size = new System.Drawing.Size(109, 24);
            this.ButtonControleIt.TabIndex = 71;
            this.ButtonControleIt.TabStop = false;
            this.ButtonControleIt.Text = "&Controle";
            this.ButtonControleIt.UseVisualStyleBackColor = true;
            this.ButtonControleIt.Click += new System.EventHandler(this.ButtonControleIt_Click);
            // 
            // ButtonBookIt
            // 
            this.ButtonBookIt.Enabled = false;
            this.ButtonBookIt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonBookIt.Location = new System.Drawing.Point(483, 371);
            this.ButtonBookIt.Name = "ButtonBookIt";
            this.ButtonBookIt.Size = new System.Drawing.Size(109, 24);
            this.ButtonBookIt.TabIndex = 72;
            this.ButtonBookIt.TabStop = false;
            this.ButtonBookIt.Text = "&Boeken";
            this.ButtonBookIt.UseVisualStyleBackColor = true;
            this.ButtonBookIt.Visible = false;
            this.ButtonBookIt.Click += new System.EventHandler(this.ButtonBookIt_Click);
            // 
            // Medekontraktant
            // 
            this.Medekontraktant.AutoSize = true;
            this.Medekontraktant.Enabled = false;
            this.Medekontraktant.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Medekontraktant.Location = new System.Drawing.Point(179, 138);
            this.Medekontraktant.Name = "Medekontraktant";
            this.Medekontraktant.Size = new System.Drawing.Size(107, 17);
            this.Medekontraktant.TabIndex = 77;
            this.Medekontraktant.TabStop = false;
            this.Medekontraktant.Text = "&Medecontractant";
            this.Medekontraktant.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Medekontraktant.UseVisualStyleBackColor = true;
            // 
            // Annuleren
            // 
            this.Annuleren.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Annuleren.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Annuleren.Location = new System.Drawing.Point(483, 499);
            this.Annuleren.Name = "Annuleren";
            this.Annuleren.Size = new System.Drawing.Size(109, 25);
            this.Annuleren.TabIndex = 76;
            this.Annuleren.TabStop = false;
            this.Annuleren.Text = "Sluiten";
            this.Annuleren.UseVisualStyleBackColor = true;
            // 
            // StockBeheer
            // 
            this.StockBeheer.AutoSize = true;
            this.StockBeheer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StockBeheer.Location = new System.Drawing.Point(294, 137);
            this.StockBeheer.Name = "StockBeheer";
            this.StockBeheer.Size = new System.Drawing.Size(87, 17);
            this.StockBeheer.TabIndex = 75;
            this.StockBeheer.TabStop = false;
            this.StockBeheer.Text = "&Stockbeheer";
            this.StockBeheer.UseVisualStyleBackColor = true;
            // 
            // cmdSQLInfo
            // 
            this.cmdSQLInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSQLInfo.Location = new System.Drawing.Point(294, 15);
            this.cmdSQLInfo.Name = "cmdSQLInfo";
            this.cmdSQLInfo.Size = new System.Drawing.Size(66, 21);
            this.cmdSQLInfo.TabIndex = 74;
            this.cmdSQLInfo.TabStop = false;
            this.cmdSQLInfo.Text = "SQL &Info";
            this.cmdSQLInfo.UseVisualStyleBackColor = true;
            this.cmdSQLInfo.Visible = false;
            // 
            // AankoopOptie2
            // 
            this.AankoopOptie2.AutoSize = true;
            this.AankoopOptie2.Enabled = false;
            this.AankoopOptie2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AankoopOptie2.Location = new System.Drawing.Point(498, 109);
            this.AankoopOptie2.Name = "AankoopOptie2";
            this.AankoopOptie2.Size = new System.Drawing.Size(89, 17);
            this.AankoopOptie2.TabIndex = 57;
            this.AankoopOptie2.Text = "Leveringsbon";
            this.AankoopOptie2.UseVisualStyleBackColor = true;
            this.AankoopOptie2.Visible = false;
            // 
            // AankoopOptie1
            // 
            this.AankoopOptie1.AutoSize = true;
            this.AankoopOptie1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AankoopOptie1.Location = new System.Drawing.Point(498, 86);
            this.AankoopOptie1.Name = "AankoopOptie1";
            this.AankoopOptie1.Size = new System.Drawing.Size(75, 17);
            this.AankoopOptie1.TabIndex = 56;
            this.AankoopOptie1.Text = "CreditNota";
            this.AankoopOptie1.UseVisualStyleBackColor = true;
            // 
            // AankoopOptie0
            // 
            this.AankoopOptie0.AutoSize = true;
            this.AankoopOptie0.Checked = true;
            this.AankoopOptie0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AankoopOptie0.Location = new System.Drawing.Point(498, 64);
            this.AankoopOptie0.Name = "AankoopOptie0";
            this.AankoopOptie0.Size = new System.Drawing.Size(78, 17);
            this.AankoopOptie0.TabIndex = 55;
            this.AankoopOptie0.TabStop = true;
            this.AankoopOptie0.Text = "&Facturering";
            this.AankoopOptie0.UseVisualStyleBackColor = true;
            // 
            // AankoopDetail
            // 
            this.AankoopDetail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.AankoopDetail.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AankoopDetail.FormattingEnabled = true;
            this.AankoopDetail.ItemHeight = 15;
            this.AankoopDetail.Location = new System.Drawing.Point(12, 160);
            this.AankoopDetail.Name = "AankoopDetail";
            this.AankoopDetail.Size = new System.Drawing.Size(580, 169);
            this.AankoopDetail.TabIndex = 58;
            // 
            // TekstInfo4
            // 
            this.TekstInfo4.Location = new System.Drawing.Point(116, 452);
            this.TekstInfo4.Name = "TekstInfo4";
            this.TekstInfo4.Size = new System.Drawing.Size(284, 20);
            this.TekstInfo4.TabIndex = 64;
            // 
            // TekstInfo3
            // 
            this.TekstInfo3.Location = new System.Drawing.Point(376, 39);
            this.TekstInfo3.Name = "TekstInfo3";
            this.TekstInfo3.Size = new System.Drawing.Size(74, 20);
            this.TekstInfo3.TabIndex = 80;
            // 
            // TekstInfo6
            // 
            this.TekstInfo6.Location = new System.Drawing.Point(308, 370);
            this.TekstInfo6.Name = "TekstInfo6";
            this.TekstInfo6.Size = new System.Drawing.Size(95, 20);
            this.TekstInfo6.TabIndex = 70;
            // 
            // TekstInfo10
            // 
            this.TekstInfo10.Location = new System.Drawing.Point(497, 39);
            this.TekstInfo10.Name = "TekstInfo10";
            this.TekstInfo10.Size = new System.Drawing.Size(74, 20);
            this.TekstInfo10.TabIndex = 81;
            // 
            // TekstInfo2
            // 
            this.TekstInfo2.Location = new System.Drawing.Point(116, 395);
            this.TekstInfo2.Mask = "00/00/0000";
            this.TekstInfo2.Name = "TekstInfo2";
            this.TekstInfo2.Size = new System.Drawing.Size(78, 20);
            this.TekstInfo2.TabIndex = 62;
            this.TekstInfo2.ValidatingType = typeof(System.DateTime);
            // 
            // TekstInfo1
            // 
            this.TekstInfo1.Location = new System.Drawing.Point(116, 346);
            this.TekstInfo1.Mask = "00/00/0000";
            this.TekstInfo1.Name = "TekstInfo1";
            this.TekstInfo1.Size = new System.Drawing.Size(78, 20);
            this.TekstInfo1.TabIndex = 60;
            this.TekstInfo1.ValidatingType = typeof(System.DateTime);
            // 
            // TekstInfo0
            // 
            this.TekstInfo0.Location = new System.Drawing.Point(116, 370);
            this.TekstInfo0.Mask = "00/00/0000";
            this.TekstInfo0.Name = "TekstInfo0";
            this.TekstInfo0.Size = new System.Drawing.Size(78, 20);
            this.TekstInfo0.TabIndex = 61;
            this.TekstInfo0.ValidatingType = typeof(System.DateTime);
            // 
            // TekstInfo9
            // 
            this.TekstInfo9.Location = new System.Drawing.Point(483, 417);
            this.TekstInfo9.Name = "TekstInfo9";
            this.TekstInfo9.Size = new System.Drawing.Size(95, 20);
            this.TekstInfo9.TabIndex = 87;
            this.TekstInfo9.TabStop = false;
            this.TekstInfo9.Visible = false;
            // 
            // TekstInfo7
            // 
            this.TekstInfo7.Location = new System.Drawing.Point(308, 394);
            this.TekstInfo7.Name = "TekstInfo7";
            this.TekstInfo7.Size = new System.Drawing.Size(95, 20);
            this.TekstInfo7.TabIndex = 73;
            this.TekstInfo7.Visible = false;
            // 
            // TekstInfo5
            // 
            this.TekstInfo5.Location = new System.Drawing.Point(308, 341);
            this.TekstInfo5.Name = "TekstInfo5";
            this.TekstInfo5.Size = new System.Drawing.Size(95, 20);
            this.TekstInfo5.TabIndex = 67;
            // 
            // TekstInfo12
            // 
            this.TekstInfo12.Location = new System.Drawing.Point(116, 427);
            this.TekstInfo12.Name = "TekstInfo12";
            this.TekstInfo12.Size = new System.Drawing.Size(284, 20);
            this.TekstInfo12.TabIndex = 63;
            // 
            // TextInfoSellersIBAN
            // 
            this.TextInfoSellersIBAN.Enabled = false;
            this.TextInfoSellersIBAN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextInfoSellersIBAN.Location = new System.Drawing.Point(119, 478);
            this.TextInfoSellersIBAN.Name = "TextInfoSellersIBAN";
            this.TextInfoSellersIBAN.Size = new System.Drawing.Size(284, 20);
            this.TextInfoSellersIBAN.TabIndex = 91;
            // 
            // SSTab1
            // 
            this.SSTab1.Controls.Add(this.TabPageLeverancier);
            this.SSTab1.Controls.Add(this.TabPageBewerken);
            this.SSTab1.Controls.Add(this.TabPageHistoriek);
            this.SSTab1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SSTab1.Location = new System.Drawing.Point(12, 13);
            this.SSTab1.Name = "SSTab1";
            this.SSTab1.SelectedIndex = 0;
            this.SSTab1.Size = new System.Drawing.Size(355, 122);
            this.SSTab1.TabIndex = 54;
            this.SSTab1.TabStop = false;
            // 
            // TabPageLeverancier
            // 
            this.TabPageLeverancier.Controls.Add(this.LeverancierInfo);
            this.TabPageLeverancier.Location = new System.Drawing.Point(4, 22);
            this.TabPageLeverancier.Name = "TabPageLeverancier";
            this.TabPageLeverancier.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageLeverancier.Size = new System.Drawing.Size(347, 96);
            this.TabPageLeverancier.TabIndex = 0;
            this.TabPageLeverancier.Text = "Leverancier";
            this.TabPageLeverancier.UseVisualStyleBackColor = true;
            // 
            // LeverancierInfo
            // 
            this.LeverancierInfo.BackColor = System.Drawing.Color.LightYellow;
            this.LeverancierInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LeverancierInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeverancierInfo.Location = new System.Drawing.Point(6, 7);
            this.LeverancierInfo.Name = "LeverancierInfo";
            this.LeverancierInfo.Size = new System.Drawing.Size(335, 84);
            this.LeverancierInfo.TabIndex = 30;
            // 
            // TabPageBewerken
            // 
            this.TabPageBewerken.Controls.Add(this.cmdXLog);
            this.TabPageBewerken.Controls.Add(this.LabelInfoXlog);
            this.TabPageBewerken.Location = new System.Drawing.Point(4, 22);
            this.TabPageBewerken.Name = "TabPageBewerken";
            this.TabPageBewerken.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageBewerken.Size = new System.Drawing.Size(347, 96);
            this.TabPageBewerken.TabIndex = 1;
            this.TabPageBewerken.Text = "Bewerken";
            this.TabPageBewerken.UseVisualStyleBackColor = true;
            // 
            // cmdXLog
            // 
            this.cmdXLog.Enabled = false;
            this.cmdXLog.Location = new System.Drawing.Point(9, 55);
            this.cmdXLog.Name = "cmdXLog";
            this.cmdXLog.Size = new System.Drawing.Size(332, 32);
            this.cmdXLog.TabIndex = 40;
            this.cmdXLog.Text = "Geactiveerde leverancier Inkijken / Wijzigen";
            this.cmdXLog.UseVisualStyleBackColor = true;
            // 
            // LabelInfoXlog
            // 
            this.LabelInfoXlog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfoXlog.Location = new System.Drawing.Point(7, 7);
            this.LabelInfoXlog.Name = "LabelInfoXlog";
            this.LabelInfoXlog.Size = new System.Drawing.Size(334, 45);
            this.LabelInfoXlog.TabIndex = 42;
            this.LabelInfoXlog.Text = "Voor manuele verrichting activeer een leverancier via TAB \'Leverancier\' of TAB \'H" +
    "istoriek\'";
            this.LabelInfoXlog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TabPageHistoriek
            // 
            this.TabPageHistoriek.Controls.Add(this.ListView1);
            this.TabPageHistoriek.Location = new System.Drawing.Point(4, 22);
            this.TabPageHistoriek.Name = "TabPageHistoriek";
            this.TabPageHistoriek.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageHistoriek.Size = new System.Drawing.Size(347, 96);
            this.TabPageHistoriek.TabIndex = 2;
            this.TabPageHistoriek.Text = "Historiek";
            this.TabPageHistoriek.UseVisualStyleBackColor = true;
            // 
            // ListView1
            // 
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderNaam,
            this.columnHeaderIdCode});
            this.ListView1.FullRowSelect = true;
            this.ListView1.GridLines = true;
            this.ListView1.HideSelection = false;
            this.ListView1.Location = new System.Drawing.Point(6, 6);
            this.ListView1.Name = "ListView1";
            this.ListView1.Size = new System.Drawing.Size(335, 84);
            this.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ListView1.TabIndex = 41;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            this.ListView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderNaam
            // 
            this.columnHeaderNaam.Text = "Naam";
            this.columnHeaderNaam.Width = 210;
            // 
            // columnHeaderIdCode
            // 
            this.columnHeaderIdCode.Text = "IdCode";
            this.columnHeaderIdCode.Width = 90;
            // 
            // Label1_0
            // 
            this.Label1_0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_0.Location = new System.Drawing.Point(14, 478);
            this.Label1_0.Name = "Label1_0";
            this.Label1_0.Size = new System.Drawing.Size(93, 16);
            this.Label1_0.TabIndex = 92;
            this.Label1_0.Text = "Betaalrekening";
            // 
            // Label1_1
            // 
            this.Label1_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_1.Location = new System.Drawing.Point(14, 431);
            this.Label1_1.Name = "Label1_1";
            this.Label1_1.Size = new System.Drawing.Size(96, 16);
            this.Label1_1.TabIndex = 88;
            this.Label1_1.Text = "DocID Leveranc.";
            // 
            // OMSLabel
            // 
            this.OMSLabel.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OMSLabel.Location = new System.Drawing.Point(20, 138);
            this.OMSLabel.Name = "OMSLabel";
            this.OMSLabel.Size = new System.Drawing.Size(430, 16);
            this.OMSLabel.TabIndex = 83;
            this.OMSLabel.Text = "&Alg.Rek Naam rekening                                Totaal";
            // 
            // Label1_3
            // 
            this.Label1_3.Location = new System.Drawing.Point(480, 398);
            this.Label1_3.Name = "Label1_3";
            this.Label1_3.Size = new System.Drawing.Size(40, 16);
            this.Label1_3.TabIndex = 82;
            this.Label1_3.Text = "Koers";
            this.Label1_3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Label1_3.Visible = false;
            // 
            // Label2
            // 
            this.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(11, 346);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(99, 44);
            this.Label2.TabIndex = 59;
            this.Label2.Text = "&Document PeppolDatum  =  Boekdatum";
            // 
            // Label1_5
            // 
            this.Label1_5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_5.Location = new System.Drawing.Point(14, 397);
            this.Label1_5.Name = "Label1_5";
            this.Label1_5.Size = new System.Drawing.Size(69, 16);
            this.Label1_5.TabIndex = 84;
            this.Label1_5.Text = "Vervaldag";
            // 
            // Label1_7
            // 
            this.Label1_7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label1_7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_7.Location = new System.Drawing.Point(376, 18);
            this.Label1_7.Name = "Label1_7";
            this.Label1_7.Size = new System.Drawing.Size(196, 19);
            this.Label1_7.TabIndex = 79;
            this.Label1_7.Text = "Default&Rek. (Leverancier en Btw)";
            // 
            // Label1_8
            // 
            this.Label1_8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_8.Location = new System.Drawing.Point(204, 345);
            this.Label1_8.Name = "Label1_8";
            this.Label1_8.Size = new System.Drawing.Size(97, 16);
            this.Label1_8.TabIndex = 66;
            this.Label1_8.Text = "Btw Aftrekbaar";
            // 
            // Label1_11
            // 
            this.Label1_11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_11.Location = new System.Drawing.Point(204, 374);
            this.Label1_11.Name = "Label1_11";
            this.Label1_11.Size = new System.Drawing.Size(69, 16);
            this.Label1_11.TabIndex = 68;
            this.Label1_11.Text = "Totaal";
            // 
            // Label1_12
            // 
            this.Label1_12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_12.Location = new System.Drawing.Point(204, 398);
            this.Label1_12.Name = "Label1_12";
            this.Label1_12.Size = new System.Drawing.Size(101, 16);
            this.Label1_12.TabIndex = 69;
            this.Label1_12.Text = "Btw Verschuldigd";
            this.Label1_12.Visible = false;
            // 
            // Label1_14
            // 
            this.Label1_14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_14.Location = new System.Drawing.Point(14, 456);
            this.Label1_14.Name = "Label1_14";
            this.Label1_14.Size = new System.Drawing.Size(93, 16);
            this.Label1_14.TabIndex = 65;
            this.Label1_14.Text = "Betaalreferte";
            // 
            // TextWarningIBAN
            // 
            this.TextWarningIBAN.AutoSize = true;
            this.TextWarningIBAN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextWarningIBAN.ForeColor = System.Drawing.Color.Red;
            this.TextWarningIBAN.Location = new System.Drawing.Point(406, 481);
            this.TextWarningIBAN.Name = "TextWarningIBAN";
            this.TextWarningIBAN.Size = new System.Drawing.Size(11, 16);
            this.TextWarningIBAN.TabIndex = 95;
            this.TextWarningIBAN.Text = "!";
            this.TextWarningIBAN.Visible = false;
            // 
            // FormBuying
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.Annuleren;
            this.ClientSize = new System.Drawing.Size(604, 535);
            this.Controls.Add(this.TextWarningIBAN);
            this.Controls.Add(this.OMSproduct);
            this.Controls.Add(this.TextBoxWarningTestCompany);
            this.Controls.Add(this.CheckBoxAlwaysPeppolRefresh);
            this.Controls.Add(this.ButtonOptimize);
            this.Controls.Add(this.cbCheckTools);
            this.Controls.Add(this.cbImportUBL);
            this.Controls.Add(this.Schoonvegen);
            this.Controls.Add(this.ButtonControleIt);
            this.Controls.Add(this.ButtonBookIt);
            this.Controls.Add(this.Medekontraktant);
            this.Controls.Add(this.Annuleren);
            this.Controls.Add(this.StockBeheer);
            this.Controls.Add(this.cmdSQLInfo);
            this.Controls.Add(this.AankoopOptie2);
            this.Controls.Add(this.AankoopOptie1);
            this.Controls.Add(this.AankoopOptie0);
            this.Controls.Add(this.AankoopDetail);
            this.Controls.Add(this.TekstInfo4);
            this.Controls.Add(this.TekstInfo3);
            this.Controls.Add(this.TekstInfo6);
            this.Controls.Add(this.TekstInfo10);
            this.Controls.Add(this.TekstInfo2);
            this.Controls.Add(this.TekstInfo1);
            this.Controls.Add(this.TekstInfo0);
            this.Controls.Add(this.TekstInfo9);
            this.Controls.Add(this.TekstInfo7);
            this.Controls.Add(this.TekstInfo5);
            this.Controls.Add(this.TekstInfo12);
            this.Controls.Add(this.TextInfoSellersIBAN);
            this.Controls.Add(this.SSTab1);
            this.Controls.Add(this.Label1_0);
            this.Controls.Add(this.Label1_1);
            this.Controls.Add(this.OMSLabel);
            this.Controls.Add(this.Label1_3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1_5);
            this.Controls.Add(this.Label1_7);
            this.Controls.Add(this.Label1_8);
            this.Controls.Add(this.Label1_11);
            this.Controls.Add(this.Label1_12);
            this.Controls.Add(this.Label1_14);
            this.Controls.Add(this.LabelDocumentReference);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBuying";
            this.Text = "Direkte aankoopverrichting";
            this.SSTab1.ResumeLayout(false);
            this.TabPageLeverancier.ResumeLayout(false);
            this.TabPageBewerken.ResumeLayout(false);
            this.TabPageHistoriek.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label LabelDocumentReference;
        private Label OMSproduct;
        private TextBox TextBoxWarningTestCompany;
        private CheckBox CheckBoxAlwaysPeppolRefresh;
        private Button ButtonOptimize;
        private Button cbCheckTools;
        private Button cbImportUBL;
        private Button Schoonvegen;
        private Button ButtonControleIt;
        private Button ButtonBookIt;
        private CheckBox Medekontraktant;
        private Button Annuleren;
        private CheckBox StockBeheer;
        private Button cmdSQLInfo;
        private RadioButton AankoopOptie2;
        private RadioButton AankoopOptie1;
        private RadioButton AankoopOptie0;
        public ListBox AankoopDetail;
        private MaskedTextBox TekstInfo4;
        private TextBox TekstInfo3;
        private TextBox TekstInfo6;
        private TextBox TekstInfo10;
        private MaskedTextBox TekstInfo2;
        private MaskedTextBox TekstInfo1;
        private MaskedTextBox TekstInfo0;
        private TextBox TekstInfo9;
        private TextBox TekstInfo7;
        private TextBox TekstInfo5;
        private TextBox TekstInfo12;
        private TextBox TextInfoSellersIBAN;
        private TabControl SSTab1;
        private TabPage TabPageLeverancier;
        private Label LeverancierInfo;
        private TabPage TabPageBewerken;
        private Button cmdXLog;
        private Label LabelInfoXlog;
        private TabPage TabPageHistoriek;
        private ListView ListView1;
        private ColumnHeader columnHeaderNaam;
        private ColumnHeader columnHeaderIdCode;
        private Label Label1_0;
        private Label Label1_1;
        private Label OMSLabel;
        private Label Label1_3;
        private Label Label2;
        private Label Label1_5;
        private Label Label1_7;
        private Label Label1_8;
        private Label Label1_11;
        private Label Label1_12;
        private Label Label1_14;
        private Label TextWarningIBAN;
    }
}
