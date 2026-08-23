namespace marVSS2028.MimMenu.Filing
{
    partial class FormProductBasicTable
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.Opties = new System.Windows.Forms.ToolStripMenuItem();
            this.LijstRap = new System.Windows.Forms.ToolStripMenuItem();
            this.VerwijderenMogelijk = new System.Windows.Forms.ToolStripMenuItem();
            this.Groepen = new System.Windows.Forms.ToolStripMenuItem();
            this.v = new System.Windows.Forms.TabControl();
            this.tabDefault = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkFilter0 = new System.Windows.Forms.CheckBox();
            this.chkFilter1 = new System.Windows.Forms.CheckBox();
            this.chkFilter2 = new System.Windows.Forms.CheckBox();
            this.chkFilter3 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdRBAcontrole = new System.Windows.Forms.Button();
            this.txtMilieu = new System.Windows.Forms.TextBox();
            this.cmdTonen = new System.Windows.Forms.Button();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.cmdSwitch = new System.Windows.Forms.Button();
            this.cbCategorie = new System.Windows.Forms.ComboBox();
            this.cbMerk = new System.Windows.Forms.ComboBox();
            this.txtEindeReeks = new System.Windows.Forms.TextBox();
            this.ButtonTab = new System.Windows.Forms.Button();
            this.Alfa = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonHigher = new System.Windows.Forms.Button();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.CmdPrinterAfdruk = new System.Windows.Forms.Button();
            this.ButtonLower = new System.Windows.Forms.Button();
            this.CmdVerwijderFiche = new System.Windows.Forms.Button();
            this.ButtonNew = new System.Windows.Forms.Button();
            this.lbJournaal = new System.Windows.Forms.Label();
            this.lblCijfers0 = new System.Windows.Forms.Label();
            this.lblCijfers1 = new System.Windows.Forms.Label();
            this.lblCijfers2 = new System.Windows.Forms.Label();
            this.lblCijfers3 = new System.Windows.Forms.Label();
            this.lblCijfers4 = new System.Windows.Forms.Label();
            this.lblCijfers5 = new System.Windows.Forms.Label();
            this.lblCijfers6 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlTxtInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tabSql = new System.Windows.Forms.TabPage();
            this.cmdKopij = new System.Windows.Forms.Button();
            this.cmdSQL = new System.Windows.Forms.Button();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.msfSQL = new System.Windows.Forms.DataGridView();
            this.tabFtp = new System.Windows.Forms.TabPage();
            this.tabJournaal = new System.Windows.Forms.TabPage();
            this.msfJournaal = new System.Windows.Forms.DataGridView();
            this.MainMenu.SuspendLayout();
            this.v.SuspendLayout();
            this.tabDefault.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msfSQL)).BeginInit();
            this.tabJournaal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msfJournaal)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Opties});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(838, 24);
            this.MainMenu.TabIndex = 0;
            // 
            // Opties
            // 
            this.Opties.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LijstRap,
            this.VerwijderenMogelijk,
            this.Groepen});
            this.Opties.Name = "Opties";
            this.Opties.Size = new System.Drawing.Size(53, 20);
            this.Opties.Text = "&Opties";
            // 
            // LijstRap
            // 
            this.LijstRap.Name = "LijstRap";
            this.LijstRap.Size = new System.Drawing.Size(223, 22);
            this.LijstRap.Text = "&Lijstrapportage";
            this.LijstRap.Click += new System.EventHandler(this.LijstRap_Click);
            // 
            // VerwijderenMogelijk
            // 
            this.VerwijderenMogelijk.Name = "VerwijderenMogelijk";
            this.VerwijderenMogelijk.Size = new System.Drawing.Size(223, 22);
            this.VerwijderenMogelijk.Text = "&Verwijderen mogelijk maken";
            this.VerwijderenMogelijk.Click += new System.EventHandler(this.VerwijderenMogelijk_Click);
            // 
            // Groepen
            // 
            this.Groepen.Name = "Groepen";
            this.Groepen.Size = new System.Drawing.Size(223, 22);
            this.Groepen.Text = "&Groepen";
            this.Groepen.Click += new System.EventHandler(this.Groepen_Click);
            // 
            // v
            // 
            this.v.Controls.Add(this.tabDefault);
            this.v.Controls.Add(this.tabSql);
            this.v.Controls.Add(this.tabFtp);
            this.v.Controls.Add(this.tabJournaal);
            this.v.Dock = System.Windows.Forms.DockStyle.Fill;
            this.v.Location = new System.Drawing.Point(0, 24);
            this.v.Name = "v";
            this.v.SelectedIndex = 0;
            this.v.Size = new System.Drawing.Size(838, 500);
            this.v.TabIndex = 1;
            this.v.SelectedIndexChanged += new System.EventHandler(this.v_SelectedIndexChanged);
            // 
            // tabDefault
            // 
            this.tabDefault.Controls.Add(this.label9);
            this.tabDefault.Controls.Add(this.label8);
            this.tabDefault.Controls.Add(this.label7);
            this.tabDefault.Controls.Add(this.groupBox1);
            this.tabDefault.Controls.Add(this.label6);
            this.tabDefault.Controls.Add(this.label5);
            this.tabDefault.Controls.Add(this.label4);
            this.tabDefault.Controls.Add(this.label3);
            this.tabDefault.Controls.Add(this.label2);
            this.tabDefault.Controls.Add(this.label1);
            this.tabDefault.Controls.Add(this.cmdRBAcontrole);
            this.tabDefault.Controls.Add(this.txtMilieu);
            this.tabDefault.Controls.Add(this.cmdTonen);
            this.tabDefault.Controls.Add(this.txtLink);
            this.tabDefault.Controls.Add(this.cmdSwitch);
            this.tabDefault.Controls.Add(this.cbCategorie);
            this.tabDefault.Controls.Add(this.cbMerk);
            this.tabDefault.Controls.Add(this.txtEindeReeks);
            this.tabDefault.Controls.Add(this.ButtonTab);
            this.tabDefault.Controls.Add(this.Alfa);
            this.tabDefault.Controls.Add(this.ButtonClose);
            this.tabDefault.Controls.Add(this.ButtonHigher);
            this.tabDefault.Controls.Add(this.ButtonSave);
            this.tabDefault.Controls.Add(this.CmdPrinterAfdruk);
            this.tabDefault.Controls.Add(this.ButtonLower);
            this.tabDefault.Controls.Add(this.CmdVerwijderFiche);
            this.tabDefault.Controls.Add(this.ButtonNew);
            this.tabDefault.Controls.Add(this.lbJournaal);
            this.tabDefault.Controls.Add(this.lblCijfers0);
            this.tabDefault.Controls.Add(this.lblCijfers1);
            this.tabDefault.Controls.Add(this.lblCijfers2);
            this.tabDefault.Controls.Add(this.lblCijfers3);
            this.tabDefault.Controls.Add(this.lblCijfers4);
            this.tabDefault.Controls.Add(this.lblCijfers5);
            this.tabDefault.Controls.Add(this.lblCijfers6);
            this.tabDefault.Controls.Add(this.lblInfo);
            this.tabDefault.Controls.Add(this.pnlTxtInfo);
            this.tabDefault.Location = new System.Drawing.Point(4, 22);
            this.tabDefault.Name = "tabDefault";
            this.tabDefault.Padding = new System.Windows.Forms.Padding(3);
            this.tabDefault.Size = new System.Drawing.Size(830, 474);
            this.tabDefault.TabIndex = 0;
            this.tabDefault.Text = "Default";
            this.tabDefault.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(378, 359);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 55;
            this.label9.Text = "Link";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(378, 314);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 54;
            this.label8.Text = "&Y. Recupel/Bebat/Auvibel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(579, 386);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Merk";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkFilter0);
            this.groupBox1.Controls.Add(this.chkFilter1);
            this.groupBox1.Controls.Add(this.chkFilter2);
            this.groupBox1.Controls.Add(this.chkFilter3);
            this.groupBox1.Location = new System.Drawing.Point(543, 186);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 113);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "InputFilter Bedragen";
            // 
            // chkFilter0
            // 
            this.chkFilter0.Location = new System.Drawing.Point(8, 21);
            this.chkFilter0.Name = "chkFilter0";
            this.chkFilter0.Size = new System.Drawing.Size(113, 21);
            this.chkFilter0.TabIndex = 28;
            this.chkFilter0.Text = "Per Verpakking";
            this.chkFilter0.UseVisualStyleBackColor = true;
            // 
            // chkFilter1
            // 
            this.chkFilter1.Location = new System.Drawing.Point(8, 43);
            this.chkFilter1.Name = "chkFilter1";
            this.chkFilter1.Size = new System.Drawing.Size(113, 21);
            this.chkFilter1.TabIndex = 29;
            this.chkFilter1.Text = "Winstberekening";
            this.chkFilter1.UseVisualStyleBackColor = true;
            // 
            // chkFilter2
            // 
            this.chkFilter2.Location = new System.Drawing.Point(8, 70);
            this.chkFilter2.Name = "chkFilter2";
            this.chkFilter2.Size = new System.Drawing.Size(113, 21);
            this.chkFilter2.TabIndex = 30;
            this.chkFilter2.Text = "Aankoop Inclusief";
            this.chkFilter2.UseVisualStyleBackColor = true;
            // 
            // chkFilter3
            // 
            this.chkFilter3.Location = new System.Drawing.Point(8, 92);
            this.chkFilter3.Name = "chkFilter3";
            this.chkFilter3.Size = new System.Drawing.Size(113, 21);
            this.chkFilter3.TabIndex = 31;
            this.chkFilter3.Text = "Verkoop Inclusief";
            this.chkFilter3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(548, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 29);
            this.label6.TabIndex = 50;
            this.label6.Text = "Actuele Aankoopwaarde";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(548, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 30);
            this.label5.TabIndex = 49;
            this.label5.Text = "Gemiddelde Aankoopwaarde";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(548, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "EUR incl. Btw";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(548, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "EUR excl. Btw";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(676, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 31);
            this.label2.TabIndex = 46;
            this.label2.Text = "Einde reeks korting %";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(548, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Actuele Stock";
            // 
            // cmdRBAcontrole
            // 
            this.cmdRBAcontrole.Location = new System.Drawing.Point(748, 329);
            this.cmdRBAcontrole.Name = "cmdRBAcontrole";
            this.cmdRBAcontrole.Size = new System.Drawing.Size(57, 20);
            this.cmdRBAcontrole.TabIndex = 30;
            this.cmdRBAcontrole.Text = "&Controle";
            this.cmdRBAcontrole.UseVisualStyleBackColor = true;
            this.cmdRBAcontrole.Click += new System.EventHandler(this.cmdRBAcontrole_Click);
            // 
            // txtMilieu
            // 
            this.txtMilieu.Location = new System.Drawing.Point(377, 330);
            this.txtMilieu.Name = "txtMilieu";
            this.txtMilieu.Size = new System.Drawing.Size(365, 20);
            this.txtMilieu.TabIndex = 29;
            // 
            // cmdTonen
            // 
            this.cmdTonen.Location = new System.Drawing.Point(748, 356);
            this.cmdTonen.Name = "cmdTonen";
            this.cmdTonen.Size = new System.Drawing.Size(57, 20);
            this.cmdTonen.TabIndex = 32;
            this.cmdTonen.Text = "&Tonen";
            this.cmdTonen.UseVisualStyleBackColor = true;
            this.cmdTonen.Click += new System.EventHandler(this.cmdTonen_Click);
            // 
            // txtLink
            // 
            this.txtLink.Location = new System.Drawing.Point(411, 356);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(331, 20);
            this.txtLink.TabIndex = 31;
            // 
            // cmdSwitch
            // 
            this.cmdSwitch.Location = new System.Drawing.Point(718, 440);
            this.cmdSwitch.Name = "cmdSwitch";
            this.cmdSwitch.Size = new System.Drawing.Size(85, 27);
            this.cmdSwitch.TabIndex = 6;
            this.cmdSwitch.Text = "Ingave in EUR";
            this.cmdSwitch.UseVisualStyleBackColor = true;
            this.cmdSwitch.Visible = false;
            this.cmdSwitch.Click += new System.EventHandler(this.cmdSwitch_Click);
            // 
            // cbCategorie
            // 
            this.cbCategorie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategorie.FormattingEnabled = true;
            this.cbCategorie.Location = new System.Drawing.Point(377, 403);
            this.cbCategorie.Name = "cbCategorie";
            this.cbCategorie.Size = new System.Drawing.Size(193, 21);
            this.cbCategorie.TabIndex = 33;
            // 
            // cbMerk
            // 
            this.cbMerk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMerk.FormattingEnabled = true;
            this.cbMerk.Location = new System.Drawing.Point(576, 402);
            this.cbMerk.Name = "cbMerk";
            this.cbMerk.Size = new System.Drawing.Size(169, 21);
            this.cbMerk.TabIndex = 34;
            // 
            // txtEindeReeks
            // 
            this.txtEindeReeks.Location = new System.Drawing.Point(690, 229);
            this.txtEindeReeks.Name = "txtEindeReeks";
            this.txtEindeReeks.Size = new System.Drawing.Size(59, 20);
            this.txtEindeReeks.TabIndex = 8;
            // 
            // ButtonTab
            // 
            this.ButtonTab.Location = new System.Drawing.Point(379, 443);
            this.ButtonTab.Name = "ButtonTab";
            this.ButtonTab.Size = new System.Drawing.Size(72, 20);
            this.ButtonTab.TabIndex = 35;
            this.ButtonTab.Text = "Tab";
            this.ButtonTab.UseVisualStyleBackColor = true;
            this.ButtonTab.Click += new System.EventHandler(this.ButtonTab_Click);
            // 
            // Alfa
            // 
            this.Alfa.Location = new System.Drawing.Point(377, 160);
            this.Alfa.Name = "Alfa";
            this.Alfa.Size = new System.Drawing.Size(160, 31);
            this.Alfa.TabIndex = 16;
            this.Alfa.TabStop = false;
            this.Alfa.Text = "SQL &Zoeken";
            this.Alfa.UseVisualStyleBackColor = true;
            this.Alfa.Click += new System.EventHandler(this.Alfa_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(377, 232);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(160, 31);
            this.ButtonClose.TabIndex = 19;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonHigher
            // 
            this.ButtonHigher.Location = new System.Drawing.Point(377, 84);
            this.ButtonHigher.Name = "ButtonHigher";
            this.ButtonHigher.Size = new System.Drawing.Size(160, 31);
            this.ButtonHigher.TabIndex = 13;
            this.ButtonHigher.TabStop = false;
            this.ButtonHigher.Text = "&Hoger";
            this.ButtonHigher.UseVisualStyleBackColor = true;
            this.ButtonHigher.Click += new System.EventHandler(this.ButtonHigher_Click);
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(377, 196);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(160, 31);
            this.ButtonSave.TabIndex = 18;
            this.ButtonSave.Text = "&Bewaren";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // CmdPrinterAfdruk
            // 
            this.CmdPrinterAfdruk.Location = new System.Drawing.Point(377, 268);
            this.CmdPrinterAfdruk.Name = "CmdPrinterAfdruk";
            this.CmdPrinterAfdruk.Size = new System.Drawing.Size(160, 31);
            this.CmdPrinterAfdruk.TabIndex = 20;
            this.CmdPrinterAfdruk.TabStop = false;
            this.CmdPrinterAfdruk.Text = "&Printer Afdruk";
            this.CmdPrinterAfdruk.UseVisualStyleBackColor = true;
            // 
            // ButtonLower
            // 
            this.ButtonLower.Location = new System.Drawing.Point(377, 47);
            this.ButtonLower.Name = "ButtonLower";
            this.ButtonLower.Size = new System.Drawing.Size(160, 31);
            this.ButtonLower.TabIndex = 12;
            this.ButtonLower.TabStop = false;
            this.ButtonLower.Text = "&Lager";
            this.ButtonLower.UseVisualStyleBackColor = true;
            this.ButtonLower.Click += new System.EventHandler(this.ButtonLower_Click);
            // 
            // CmdVerwijderFiche
            // 
            this.CmdVerwijderFiche.Enabled = false;
            this.CmdVerwijderFiche.Location = new System.Drawing.Point(377, 121);
            this.CmdVerwijderFiche.Name = "CmdVerwijderFiche";
            this.CmdVerwijderFiche.Size = new System.Drawing.Size(160, 31);
            this.CmdVerwijderFiche.TabIndex = 14;
            this.CmdVerwijderFiche.TabStop = false;
            this.CmdVerwijderFiche.Text = "Verwijderen";
            this.CmdVerwijderFiche.UseVisualStyleBackColor = true;
            this.CmdVerwijderFiche.Click += new System.EventHandler(this.CmdVerwijderFiche_Click);
            // 
            // ButtonNew
            // 
            this.ButtonNew.Location = new System.Drawing.Point(377, 10);
            this.ButtonNew.Name = "ButtonNew";
            this.ButtonNew.Size = new System.Drawing.Size(160, 31);
            this.ButtonNew.TabIndex = 11;
            this.ButtonNew.TabStop = false;
            this.ButtonNew.Text = "&Nieuw";
            this.ButtonNew.UseVisualStyleBackColor = true;
            this.ButtonNew.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // lbJournaal
            // 
            this.lbJournaal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbJournaal.Location = new System.Drawing.Point(648, 160);
            this.lbJournaal.Name = "lbJournaal";
            this.lbJournaal.Size = new System.Drawing.Size(101, 20);
            this.lbJournaal.TabIndex = 36;
            this.lbJournaal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCijfers0
            // 
            this.lblCijfers0.BackColor = System.Drawing.Color.LightYellow;
            this.lblCijfers0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers0.Location = new System.Drawing.Point(648, 8);
            this.lblCijfers0.Name = "lblCijfers0";
            this.lblCijfers0.Size = new System.Drawing.Size(101, 20);
            this.lblCijfers0.TabIndex = 37;
            this.lblCijfers0.Text = " ";
            this.lblCijfers0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCijfers1
            // 
            this.lblCijfers1.BackColor = System.Drawing.Color.LightYellow;
            this.lblCijfers1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers1.Location = new System.Drawing.Point(648, 95);
            this.lblCijfers1.Name = "lblCijfers1";
            this.lblCijfers1.Size = new System.Drawing.Size(101, 20);
            this.lblCijfers1.TabIndex = 38;
            this.lblCijfers1.Text = " ";
            this.lblCijfers1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCijfers2
            // 
            this.lblCijfers2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers2.Location = new System.Drawing.Point(456, 443);
            this.lblCijfers2.Name = "lblCijfers2";
            this.lblCijfers2.Size = new System.Drawing.Size(124, 20);
            this.lblCijfers2.TabIndex = 39;
            this.lblCijfers2.Text = " ";
            this.lblCijfers2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCijfers2.Visible = false;
            // 
            // lblCijfers3
            // 
            this.lblCijfers3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers3.Location = new System.Drawing.Point(588, 443);
            this.lblCijfers3.Name = "lblCijfers3";
            this.lblCijfers3.Size = new System.Drawing.Size(124, 20);
            this.lblCijfers3.TabIndex = 40;
            this.lblCijfers3.Text = " ";
            this.lblCijfers3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCijfers3.Visible = false;
            // 
            // lblCijfers4
            // 
            this.lblCijfers4.BackColor = System.Drawing.Color.LightYellow;
            this.lblCijfers4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers4.Location = new System.Drawing.Point(648, 58);
            this.lblCijfers4.Name = "lblCijfers4";
            this.lblCijfers4.Size = new System.Drawing.Size(101, 20);
            this.lblCijfers4.TabIndex = 41;
            this.lblCijfers4.Text = " ";
            this.lblCijfers4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCijfers5
            // 
            this.lblCijfers5.BackColor = System.Drawing.Color.LightYellow;
            this.lblCijfers5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers5.Location = new System.Drawing.Point(648, 34);
            this.lblCijfers5.Name = "lblCijfers5";
            this.lblCijfers5.Size = new System.Drawing.Size(101, 20);
            this.lblCijfers5.TabIndex = 42;
            this.lblCijfers5.Text = " ";
            this.lblCijfers5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCijfers6
            // 
            this.lblCijfers6.BackColor = System.Drawing.Color.LightYellow;
            this.lblCijfers6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCijfers6.Location = new System.Drawing.Point(648, 132);
            this.lblCijfers6.Name = "lblCijfers6";
            this.lblCijfers6.Size = new System.Drawing.Size(101, 20);
            this.lblCijfers6.TabIndex = 43;
            this.lblCijfers6.Text = " ";
            this.lblCijfers6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(380, 387);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(52, 13);
            this.lblInfo.TabIndex = 44;
            this.lblInfo.Text = "&Categorie";
            // 
            // pnlTxtInfo
            // 
            this.pnlTxtInfo.AutoScroll = true;
            this.pnlTxtInfo.ColumnCount = 2;
            this.pnlTxtInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.pnlTxtInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlTxtInfo.Location = new System.Drawing.Point(8, 10);
            this.pnlTxtInfo.Name = "pnlTxtInfo";
            this.pnlTxtInfo.RowCount = 12;
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlTxtInfo.Size = new System.Drawing.Size(352, 461);
            this.pnlTxtInfo.TabIndex = 10;
            // 
            // tabSql
            // 
            this.tabSql.Controls.Add(this.cmdKopij);
            this.tabSql.Controls.Add(this.cmdSQL);
            this.tabSql.Controls.Add(this.lblRecordCount);
            this.tabSql.Controls.Add(this.txtSQL);
            this.tabSql.Controls.Add(this.msfSQL);
            this.tabSql.Location = new System.Drawing.Point(4, 22);
            this.tabSql.Name = "tabSql";
            this.tabSql.Padding = new System.Windows.Forms.Padding(3);
            this.tabSql.Size = new System.Drawing.Size(835, 533);
            this.tabSql.TabIndex = 1;
            this.tabSql.Text = "SQL Query";
            this.tabSql.UseVisualStyleBackColor = true;
            // 
            // cmdKopij
            // 
            this.cmdKopij.Location = new System.Drawing.Point(770, 10);
            this.cmdKopij.Name = "cmdKopij";
            this.cmdKopij.Size = new System.Drawing.Size(128, 50);
            this.cmdKopij.TabIndex = 3;
            this.cmdKopij.Text = "XML &Kopie";
            this.cmdKopij.UseVisualStyleBackColor = true;
            this.cmdKopij.Click += new System.EventHandler(this.ButtonCopy_Click);
            // 
            // cmdSQL
            // 
            this.cmdSQL.Location = new System.Drawing.Point(8, 156);
            this.cmdSQL.Name = "cmdSQL";
            this.cmdSQL.Size = new System.Drawing.Size(145, 28);
            this.cmdSQL.TabIndex = 1;
            this.cmdSQL.Text = "SQL &SELECT";
            this.cmdSQL.UseVisualStyleBackColor = true;
            this.cmdSQL.Click += new System.EventHandler(this.ButtonSQL_Click);
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRecordCount.Location = new System.Drawing.Point(160, 156);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(120, 28);
            this.lblRecordCount.TabIndex = 4;
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQL
            // 
            this.txtSQL.Location = new System.Drawing.Point(8, 10);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQL.Size = new System.Drawing.Size(756, 140);
            this.txtSQL.TabIndex = 0;
            // 
            // msfSQL
            // 
            this.msfSQL.AllowUserToAddRows = false;
            this.msfSQL.AllowUserToDeleteRows = false;
            this.msfSQL.AllowUserToOrderColumns = true;
            this.msfSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msfSQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.msfSQL.Location = new System.Drawing.Point(8, 190);
            this.msfSQL.Name = "msfSQL";
            this.msfSQL.ReadOnly = true;
            this.msfSQL.Size = new System.Drawing.Size(890, 353);
            this.msfSQL.TabIndex = 2;
            // 
            // tabFtp
            // 
            this.tabFtp.Location = new System.Drawing.Point(4, 22);
            this.tabFtp.Name = "tabFtp";
            this.tabFtp.Padding = new System.Windows.Forms.Padding(3);
            this.tabFtp.Size = new System.Drawing.Size(835, 533);
            this.tabFtp.TabIndex = 2;
            this.tabFtp.Text = "eCommerce FTP";
            this.tabFtp.UseVisualStyleBackColor = true;
            // 
            // tabJournaal
            // 
            this.tabJournaal.Controls.Add(this.msfJournaal);
            this.tabJournaal.Location = new System.Drawing.Point(4, 22);
            this.tabJournaal.Name = "tabJournaal";
            this.tabJournaal.Padding = new System.Windows.Forms.Padding(3);
            this.tabJournaal.Size = new System.Drawing.Size(835, 533);
            this.tabJournaal.TabIndex = 3;
            this.tabJournaal.Text = "Journaal";
            this.tabJournaal.UseVisualStyleBackColor = true;
            // 
            // msfJournaal
            // 
            this.msfJournaal.AllowUserToAddRows = false;
            this.msfJournaal.AllowUserToDeleteRows = false;
            this.msfJournaal.AllowUserToOrderColumns = true;
            this.msfJournaal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.msfJournaal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msfJournaal.Location = new System.Drawing.Point(3, 3);
            this.msfJournaal.Name = "msfJournaal";
            this.msfJournaal.ReadOnly = true;
            this.msfJournaal.Size = new System.Drawing.Size(829, 527);
            this.msfJournaal.TabIndex = 0;
            // 
            // FormProductBasicTable
            // 
            this.AcceptButton = this.ButtonTab;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(838, 524);
            this.Controls.Add(this.v);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProductBasicTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProduktFiche";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormProductBasicTable_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.v.ResumeLayout(false);
            this.tabDefault.ResumeLayout(false);
            this.tabDefault.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabSql.ResumeLayout(false);
            this.tabSql.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msfSQL)).EndInit();
            this.tabJournaal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.msfJournaal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem Opties;
        private System.Windows.Forms.ToolStripMenuItem LijstRap;
        private System.Windows.Forms.ToolStripMenuItem VerwijderenMogelijk;
        private System.Windows.Forms.ToolStripMenuItem Groepen;
        private System.Windows.Forms.TabControl v;
        private System.Windows.Forms.TabPage tabDefault;
        private System.Windows.Forms.TabPage tabSql;
        private System.Windows.Forms.TabPage tabFtp;
        private System.Windows.Forms.TabPage tabJournaal;
        private System.Windows.Forms.Button cmdRBAcontrole;
        private System.Windows.Forms.TextBox txtMilieu;
        private System.Windows.Forms.Button cmdTonen;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.Button cmdSwitch;
        private System.Windows.Forms.ComboBox cbCategorie;
        private System.Windows.Forms.ComboBox cbMerk;
        private System.Windows.Forms.TextBox txtEindeReeks;
        private System.Windows.Forms.Button ButtonTab;
        private System.Windows.Forms.Button Alfa;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonHigher;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button CmdPrinterAfdruk;
        private System.Windows.Forms.Button ButtonLower;
        private System.Windows.Forms.Button CmdVerwijderFiche;
        private System.Windows.Forms.Button ButtonNew;
        private System.Windows.Forms.Label lbJournaal;
        private System.Windows.Forms.Label lblCijfers0;
        private System.Windows.Forms.Label lblCijfers1;
        private System.Windows.Forms.Label lblCijfers2;
        private System.Windows.Forms.Label lblCijfers3;
        private System.Windows.Forms.Label lblCijfers4;
        private System.Windows.Forms.Label lblCijfers5;
        private System.Windows.Forms.Label lblCijfers6;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TableLayoutPanel pnlTxtInfo;
        private System.Windows.Forms.Button cmdKopij;
        private System.Windows.Forms.Button cmdSQL;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.DataGridView msfSQL;
        private System.Windows.Forms.DataGridView msfJournaal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkFilter0;
        private System.Windows.Forms.CheckBox chkFilter1;
        private System.Windows.Forms.CheckBox chkFilter2;
        private System.Windows.Forms.CheckBox chkFilter3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}
