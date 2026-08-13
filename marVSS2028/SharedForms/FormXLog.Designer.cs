namespace marVSS2028.SharedForms
{
    partial class FormXLog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageDefault = new System.Windows.Forms.TabPage();
            this.X = new System.Windows.Forms.DataGridView();
            this.TabPageAfbeelding = new System.Windows.Forms.TabPage();
            this.BtnCommand1 = new System.Windows.Forms.Button();
            this.BtnCommand2 = new System.Windows.Forms.Button();
            this.BtnCommand3 = new System.Windows.Forms.Button();
            this.BtnCommand4 = new System.Windows.Forms.Button();
            this.LblLabel1 = new System.Windows.Forms.Label();
            this.LblLabel2 = new System.Windows.Forms.Label();
            this.LblLabel3 = new System.Windows.Forms.Label();
            this.BtnAfsluiten = new System.Windows.Forms.Button();
            this.BtnAnnuleren = new System.Windows.Forms.Button();
            this.BtnAfbeelding = new System.Windows.Forms.Button();
            this.BtnWijzigenLijn = new System.Windows.Forms.Button();
            this.BtnDetailJournaal = new System.Windows.Forms.Button();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuBewerken = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuKopieren = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSelectie = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuGrafischAfdruk = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuHPPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuIBMPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPuurTekst = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuBewaarAls = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuStandaardGrootte = new System.Windows.Forms.ToolStripMenuItem();
            this.TabControl1.SuspendLayout();
            this.TabPageDefault.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X)).BeginInit();
            this.TabPageAfbeelding.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPageDefault);
            this.TabControl1.Controls.Add(this.TabPageAfbeelding);
            this.TabControl1.Location = new System.Drawing.Point(0, 24);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(668, 302);
            this.TabControl1.TabIndex = 0;
            this.TabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // TabPageDefault
            // 
            this.TabPageDefault.Controls.Add(this.X);
            this.TabPageDefault.Location = new System.Drawing.Point(4, 23);
            this.TabPageDefault.Name = "TabPageDefault";
            this.TabPageDefault.Size = new System.Drawing.Size(660, 275);
            this.TabPageDefault.TabIndex = 0;
            this.TabPageDefault.Text = "Default";
            // 
            // X
            // 
            this.X.AllowUserToAddRows = false;
            this.X.AllowUserToDeleteRows = false;
            this.X.BackgroundColor = System.Drawing.Color.Silver;
            this.X.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.X.Location = new System.Drawing.Point(0, 0);
            this.X.MultiSelect = false;
            this.X.Name = "X";
            this.X.ReadOnly = true;
            this.X.RowHeadersVisible = false;
            this.X.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.X.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.X.Size = new System.Drawing.Size(660, 275);
            this.X.TabIndex = 0;
            this.X.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.X_CellClick);
            this.X.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.X_CellDoubleClick);
            this.X.SelectionChanged += new System.EventHandler(this.X_SelectionChanged);
            this.X.KeyDown += new System.Windows.Forms.KeyEventHandler(this.X_KeyDown);
            this.X.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.X_KeyPress);
            this.X.KeyUp += new System.Windows.Forms.KeyEventHandler(this.X_KeyUp);
            // 
            // TabPageAfbeelding
            // 
            this.TabPageAfbeelding.Controls.Add(this.BtnCommand1);
            this.TabPageAfbeelding.Controls.Add(this.BtnCommand2);
            this.TabPageAfbeelding.Controls.Add(this.BtnCommand3);
            this.TabPageAfbeelding.Controls.Add(this.BtnCommand4);
            this.TabPageAfbeelding.Controls.Add(this.LblLabel1);
            this.TabPageAfbeelding.Controls.Add(this.LblLabel2);
            this.TabPageAfbeelding.Controls.Add(this.LblLabel3);
            this.TabPageAfbeelding.Location = new System.Drawing.Point(4, 23);
            this.TabPageAfbeelding.Name = "TabPageAfbeelding";
            this.TabPageAfbeelding.Size = new System.Drawing.Size(660, 275);
            this.TabPageAfbeelding.TabIndex = 1;
            this.TabPageAfbeelding.Text = "- Geen Bijlage";
            // 
            // BtnCommand1
            // 
            this.BtnCommand1.Location = new System.Drawing.Point(8, 8);
            this.BtnCommand1.Name = "BtnCommand1";
            this.BtnCommand1.Size = new System.Drawing.Size(120, 28);
            this.BtnCommand1.TabIndex = 0;
            this.BtnCommand1.Text = "Pdf Bewerken";
            this.BtnCommand1.Click += new System.EventHandler(this.BtnCommand1_Click);
            // 
            // BtnCommand2
            // 
            this.BtnCommand2.Enabled = false;
            this.BtnCommand2.Location = new System.Drawing.Point(8, 44);
            this.BtnCommand2.Name = "BtnCommand2";
            this.BtnCommand2.Size = new System.Drawing.Size(120, 28);
            this.BtnCommand2.TabIndex = 1;
            this.BtnCommand2.Text = "Pdf Opslaan";
            this.BtnCommand2.Click += new System.EventHandler(this.BtnCommand2_Click);
            // 
            // BtnCommand3
            // 
            this.BtnCommand3.Location = new System.Drawing.Point(8, 80);
            this.BtnCommand3.Name = "BtnCommand3";
            this.BtnCommand3.Size = new System.Drawing.Size(120, 28);
            this.BtnCommand3.TabIndex = 2;
            this.BtnCommand3.Text = "Tif Bewerken";
            this.BtnCommand3.Click += new System.EventHandler(this.BtnCommand3_Click);
            // 
            // BtnCommand4
            // 
            this.BtnCommand4.Enabled = false;
            this.BtnCommand4.Location = new System.Drawing.Point(8, 116);
            this.BtnCommand4.Name = "BtnCommand4";
            this.BtnCommand4.Size = new System.Drawing.Size(120, 28);
            this.BtnCommand4.TabIndex = 3;
            this.BtnCommand4.Text = "Pdf Opslaan";
            this.BtnCommand4.Visible = false;
            // 
            // LblLabel1
            // 
            this.LblLabel1.Location = new System.Drawing.Point(140, 80);
            this.LblLabel1.Name = "LblLabel1";
            this.LblLabel1.Size = new System.Drawing.Size(200, 20);
            this.LblLabel1.TabIndex = 4;
            this.LblLabel1.Text = "Label1";
            // 
            // LblLabel2
            // 
            this.LblLabel2.Location = new System.Drawing.Point(8, 220);
            this.LblLabel2.Name = "LblLabel2";
            this.LblLabel2.Size = new System.Drawing.Size(644, 48);
            this.LblLabel2.TabIndex = 5;
            this.LblLabel2.Text = "Label2";
            this.LblLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblLabel3
            // 
            this.LblLabel3.Location = new System.Drawing.Point(140, 28);
            this.LblLabel3.Name = "LblLabel3";
            this.LblLabel3.Size = new System.Drawing.Size(200, 40);
            this.LblLabel3.TabIndex = 6;
            this.LblLabel3.Text = "Label3";
            this.LblLabel3.Visible = false;
            // 
            // BtnAfsluiten
            // 
            this.BtnAfsluiten.Location = new System.Drawing.Point(4, 336);
            this.BtnAfsluiten.Name = "BtnAfsluiten";
            this.BtnAfsluiten.Size = new System.Drawing.Size(100, 26);
            this.BtnAfsluiten.TabIndex = 1;
            this.BtnAfsluiten.Text = "Ok";
            this.BtnAfsluiten.Click += new System.EventHandler(this.BtnAfsluiten_Click);
            // 
            // BtnAnnuleren
            // 
            this.BtnAnnuleren.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnAnnuleren.Location = new System.Drawing.Point(116, 336);
            this.BtnAnnuleren.Name = "BtnAnnuleren";
            this.BtnAnnuleren.Size = new System.Drawing.Size(100, 26);
            this.BtnAnnuleren.TabIndex = 2;
            this.BtnAnnuleren.TabStop = false;
            this.BtnAnnuleren.Text = "Sluiten";
            this.BtnAnnuleren.Click += new System.EventHandler(this.BtnAnnuleren_Click);
            // 
            // BtnAfbeelding
            // 
            this.BtnAfbeelding.Location = new System.Drawing.Point(224, 336);
            this.BtnAfbeelding.Name = "BtnAfbeelding";
            this.BtnAfbeelding.Size = new System.Drawing.Size(100, 26);
            this.BtnAfbeelding.TabIndex = 3;
            this.BtnAfbeelding.TabStop = false;
            this.BtnAfbeelding.Text = "Afdrukken";
            this.BtnAfbeelding.Click += new System.EventHandler(this.BtnAfbeelding_Click);
            // 
            // BtnWijzigenLijn
            // 
            this.BtnWijzigenLijn.Location = new System.Drawing.Point(332, 336);
            this.BtnWijzigenLijn.Name = "BtnWijzigenLijn";
            this.BtnWijzigenLijn.Size = new System.Drawing.Size(100, 26);
            this.BtnWijzigenLijn.TabIndex = 4;
            this.BtnWijzigenLijn.TabStop = false;
            this.BtnWijzigenLijn.Text = "Wijzigen";
            this.BtnWijzigenLijn.Click += new System.EventHandler(this.BtnWijzigenLijn_Click);
            // 
            // BtnDetailJournaal
            // 
            this.BtnDetailJournaal.Location = new System.Drawing.Point(440, 336);
            this.BtnDetailJournaal.Name = "BtnDetailJournaal";
            this.BtnDetailJournaal.Size = new System.Drawing.Size(110, 26);
            this.BtnDetailJournaal.TabIndex = 5;
            this.BtnDetailJournaal.TabStop = false;
            this.BtnDetailJournaal.Text = "Detail &Journaal";
            this.BtnDetailJournaal.Visible = false;
            this.BtnDetailJournaal.Click += new System.EventHandler(this.BtnDetailJournaal_Click);
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuBewerken});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(668, 24);
            this.MenuStrip1.TabIndex = 6;
            // 
            // MenuBewerken
            // 
            this.MenuBewerken.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuKopieren,
            this.MenuSelectie,
            this.MenuGrafischAfdruk,
            this.MenuSep1,
            this.MenuHPPrint,
            this.MenuIBMPrint,
            this.MenuPuurTekst,
            this.MenuSep2,
            this.MenuBewaarAls,
            this.MenuSep3,
            this.MenuStandaardGrootte});
            this.MenuBewerken.Name = "MenuBewerken";
            this.MenuBewerken.Size = new System.Drawing.Size(70, 20);
            this.MenuBewerken.Text = "&Bewerken";
            // 
            // MenuKopieren
            // 
            this.MenuKopieren.Name = "MenuKopieren";
            this.MenuKopieren.Size = new System.Drawing.Size(359, 22);
            this.MenuKopieren.Text = "&Kopiëren";
            this.MenuKopieren.Click += new System.EventHandler(this.MenuKopieren_Click);
            // 
            // MenuSelectie
            // 
            this.MenuSelectie.Name = "MenuSelectie";
            this.MenuSelectie.Size = new System.Drawing.Size(359, 22);
            this.MenuSelectie.Text = "&Selecteren mogelijk";
            this.MenuSelectie.Click += new System.EventHandler(this.MenuSelectie_Click);
            // 
            // MenuGrafischAfdruk
            // 
            this.MenuGrafischAfdruk.Name = "MenuGrafischAfdruk";
            this.MenuGrafischAfdruk.Size = new System.Drawing.Size(359, 22);
            this.MenuGrafischAfdruk.Text = "&Grafische afdruk";
            this.MenuGrafischAfdruk.Click += new System.EventHandler(this.MenuGrafischAfdruk_Click);
            // 
            // MenuSep1
            // 
            this.MenuSep1.Name = "MenuSep1";
            this.MenuSep1.Size = new System.Drawing.Size(356, 6);
            // 
            // MenuHPPrint
            // 
            this.MenuHPPrint.Name = "MenuHPPrint";
            this.MenuHPPrint.Size = new System.Drawing.Size(359, 22);
            this.MenuHPPrint.Text = "MsDos Editor Laserprinter Bestand (&HP Stuurcodes)";
            this.MenuHPPrint.Click += new System.EventHandler(this.MenuHPPrint_Click);
            // 
            // MenuIBMPrint
            // 
            this.MenuIBMPrint.Name = "MenuIBMPrint";
            this.MenuIBMPrint.Size = new System.Drawing.Size(359, 22);
            this.MenuIBMPrint.Text = "MsDos Editor Kettingprinter Bestand (&IBM Stuurcodes)";
            this.MenuIBMPrint.Click += new System.EventHandler(this.MenuIBMPrint_Click);
            // 
            // MenuPuurTekst
            // 
            this.MenuPuurTekst.Name = "MenuPuurTekst";
            this.MenuPuurTekst.Size = new System.Drawing.Size(359, 22);
            this.MenuPuurTekst.Text = "MsDos Editor Puur Tekstbestand";
            this.MenuPuurTekst.Click += new System.EventHandler(this.MenuPuurTekst_Click);
            // 
            // MenuSep2
            // 
            this.MenuSep2.Name = "MenuSep2";
            this.MenuSep2.Size = new System.Drawing.Size(356, 6);
            // 
            // MenuBewaarAls
            // 
            this.MenuBewaarAls.Name = "MenuBewaarAls";
            this.MenuBewaarAls.Size = new System.Drawing.Size(359, 22);
            this.MenuBewaarAls.Text = "Opslaan met scheidingstekens";
            this.MenuBewaarAls.Click += new System.EventHandler(this.MenuBewaarAls_Click);
            // 
            // MenuSep3
            // 
            this.MenuSep3.Name = "MenuSep3";
            this.MenuSep3.Size = new System.Drawing.Size(356, 6);
            // 
            // MenuStandaardGrootte
            // 
            this.MenuStandaardGrootte.Name = "MenuStandaardGrootte";
            this.MenuStandaardGrootte.Size = new System.Drawing.Size(359, 22);
            this.MenuStandaardGrootte.Text = "Standaard &Venstergrootte";
            this.MenuStandaardGrootte.Click += new System.EventHandler(this.MenuStandaardGrootte_Click);
            // 
            // FormXLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnAnnuleren;
            this.ClientSize = new System.Drawing.Size(668, 370);
            this.ControlBox = false;
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.BtnAfsluiten);
            this.Controls.Add(this.BtnAnnuleren);
            this.Controls.Add(this.BtnAfbeelding);
            this.Controls.Add(this.BtnWijzigenLijn);
            this.Controls.Add(this.BtnDetailJournaal);
            this.Controls.Add(this.MenuStrip1);
            this.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.MainMenuStrip = this.MenuStrip1;
            this.MinimizeBox = false;
            this.Name = "FormXLog";
            this.Text = "Log";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormXLog_FormClosed);
            this.Load += new System.EventHandler(this.FormXLog_Load);
            this.Resize += new System.EventHandler(this.FormXLog_Resize);
            this.TabControl1.ResumeLayout(false);
            this.TabPageDefault.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.X)).EndInit();
            this.TabPageAfbeelding.ResumeLayout(false);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Controls
        internal System.Windows.Forms.TabControl        TabControl1;
        private  System.Windows.Forms.TabPage           TabPageDefault;
        internal System.Windows.Forms.DataGridView      X;
        private  System.Windows.Forms.TabPage           TabPageAfbeelding;
        private  System.Windows.Forms.Button            BtnCommand1;
        private  System.Windows.Forms.Button            BtnCommand2;
        private  System.Windows.Forms.Button            BtnCommand3;
        private  System.Windows.Forms.Button            BtnCommand4;
        private  System.Windows.Forms.Label             LblLabel1;
        private  System.Windows.Forms.Label             LblLabel2;
        private  System.Windows.Forms.Label             LblLabel3;
        internal System.Windows.Forms.Button            BtnAfsluiten;
        internal System.Windows.Forms.Button            BtnAnnuleren;
        internal System.Windows.Forms.Button            BtnAfbeelding;
        internal System.Windows.Forms.Button            BtnWijzigenLijn;
        internal System.Windows.Forms.Button            BtnDetailJournaal;
        private  System.Windows.Forms.MenuStrip         MenuStrip1;
        private  System.Windows.Forms.ToolStripMenuItem MenuBewerken;
        private  System.Windows.Forms.ToolStripMenuItem MenuKopieren;
        internal System.Windows.Forms.ToolStripMenuItem MenuSelectie;
        private  System.Windows.Forms.ToolStripMenuItem MenuGrafischAfdruk;
        private  System.Windows.Forms.ToolStripSeparator MenuSep1;
        private  System.Windows.Forms.ToolStripMenuItem MenuHPPrint;
        private  System.Windows.Forms.ToolStripMenuItem MenuIBMPrint;
        private  System.Windows.Forms.ToolStripMenuItem MenuPuurTekst;
        private  System.Windows.Forms.ToolStripSeparator MenuSep2;
        private  System.Windows.Forms.ToolStripMenuItem MenuBewaarAls;
        private  System.Windows.Forms.ToolStripSeparator MenuSep3;
        private  System.Windows.Forms.ToolStripMenuItem MenuStandaardGrootte;
    }
}
