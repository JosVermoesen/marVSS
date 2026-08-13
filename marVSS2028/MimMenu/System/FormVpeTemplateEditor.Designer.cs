namespace marVSS2028.Forms
{
    partial class FormVpeTemplateEditor
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
            this.components = new System.ComponentModel.Container();
            this.TxtDemoTekst       = new System.Windows.Forms.TextBox();
            this.BtnFont            = new System.Windows.Forms.Button();
            this.TxtFont            = new System.Windows.Forms.TextBox();
            this.BtnKleurKiezen     = new System.Windows.Forms.Button();
            this.TxtKleur           = new System.Windows.Forms.TextBox();
            this.LblTekstHelper     = new System.Windows.Forms.Label();
            this.LblDemoTekst       = new System.Windows.Forms.Label();
            this.ToolTip1           = new System.Windows.Forms.ToolTip(this.components);
            this.MainMenu           = new System.Windows.Forms.MenuStrip();
            this.MenuBestand        = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBestandOpenen  = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBestandSep1    = new System.Windows.Forms.ToolStripSeparator();
            this.MenuViaKladblok    = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuViaKBTekst     = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBestandSep2    = new System.Windows.Forms.ToolStripSeparator();
            this.MenuBestandSluiten = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTaal           = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTaalFrans      = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTaalNederlands = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTaalEngels     = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTaalDuits      = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDocument       = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokFactuur     = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokLevering    = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokBestel      = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokOfferte     = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokBrief       = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokRekening    = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDokKwijting    = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            //
            // MainMenu
            //
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.MenuBestand, this.MenuTaal, this.MenuDocument });
            this.MainMenu.AllowMerge = false;
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(600, 24);
            //
            // MenuBestand
            //
            this.MenuBestand.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.MenuBestandOpenen, this.MenuBestandSep1,
                this.MenuViaKladblok,
                this.MenuBestandSep2, this.MenuBestandSluiten });
            this.MenuBestand.Name = "MenuBestand";
            this.MenuBestand.Text = "&Bestand";
            //
            // MenuBestandOpenen
            //
            this.MenuBestandOpenen.Name = "MenuBestandOpenen";
            this.MenuBestandOpenen.Text = "&Openen";
            this.MenuBestandOpenen.Click += new System.EventHandler(this.MenuBestandOpenen_Click);
            //
            // MenuBestandSep1
            //
            this.MenuBestandSep1.Name = "MenuBestandSep1";
            //
            // MenuViaKladblok
            //
            this.MenuViaKladblok.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.MenuViaKBTekst });
            this.MenuViaKladblok.Name = "MenuViaKladblok";
            this.MenuViaKladblok.Text = "Via kladblok";
            //
            // MenuViaKBTekst
            //
            this.MenuViaKBTekst.Name = "MenuViaKBTekst";
            this.MenuViaKBTekst.Text = "Tekst- en Lijnobjecten";
            this.MenuViaKBTekst.Click += new System.EventHandler(this.MenuViaKBTekst_Click);
            //
            // MenuBestandSep2
            //
            this.MenuBestandSep2.Name = "MenuBestandSep2";
            //
            // MenuBestandSluiten
            //
            this.MenuBestandSluiten.Name = "MenuBestandSluiten";
            this.MenuBestandSluiten.Text = "&Afsluiten";
            this.MenuBestandSluiten.Click += new System.EventHandler(this.MenuBestandSluiten_Click);
            //
            // MenuTaal
            //
            this.MenuTaal.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.MenuTaalFrans, this.MenuTaalNederlands,
                this.MenuTaalEngels, this.MenuTaalDuits });
            this.MenuTaal.Name = "MenuTaal";
            this.MenuTaal.Text = "&Taal";
            //
            // MenuTaalFrans
            //
            this.MenuTaalFrans.Name = "MenuTaalFrans";
            this.MenuTaalFrans.Text = "&Frans";
            this.MenuTaalFrans.Tag  = "1";
            this.MenuTaalFrans.Click += new System.EventHandler(this.MenuTaal_Click);
            //
            // MenuTaalNederlands
            //
            this.MenuTaalNederlands.Checked = true;
            this.MenuTaalNederlands.Name = "MenuTaalNederlands";
            this.MenuTaalNederlands.Text = "&Nederlands";
            this.MenuTaalNederlands.Tag  = "2";
            this.MenuTaalNederlands.Click += new System.EventHandler(this.MenuTaal_Click);
            //
            // MenuTaalEngels
            //
            this.MenuTaalEngels.Name = "MenuTaalEngels";
            this.MenuTaalEngels.Text = "&Engels";
            this.MenuTaalEngels.Tag  = "3";
            this.MenuTaalEngels.Click += new System.EventHandler(this.MenuTaal_Click);
            //
            // MenuTaalDuits
            //
            this.MenuTaalDuits.Name = "MenuTaalDuits";
            this.MenuTaalDuits.Text = "&Duits";
            this.MenuTaalDuits.Tag  = "4";
            this.MenuTaalDuits.Click += new System.EventHandler(this.MenuTaal_Click);
            //
            // MenuDocument
            //
            this.MenuDocument.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.MenuDokFactuur, this.MenuDokLevering, this.MenuDokBestel,
                this.MenuDokOfferte, this.MenuDokBrief, this.MenuDokRekening, this.MenuDokKwijting });
            this.MenuDocument.Name = "MenuDocument";
            this.MenuDocument.Text = "Document";
            //
            // MenuDokFactuur
            //
            this.MenuDokFactuur.Checked = true;
            this.MenuDokFactuur.Name = "MenuDokFactuur";
            this.MenuDokFactuur.Text = "Factuur/Creditnota";
            this.MenuDokFactuur.Tag  = "0";
            this.MenuDokFactuur.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // MenuDokLevering
            //
            this.MenuDokLevering.Name = "MenuDokLevering";
            this.MenuDokLevering.Text = "LeveringsBon";
            this.MenuDokLevering.Tag  = "1";
            this.MenuDokLevering.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // MenuDokBestel
            //
            this.MenuDokBestel.Name = "MenuDokBestel";
            this.MenuDokBestel.Text = "BestelBon";
            this.MenuDokBestel.Tag  = "2";
            this.MenuDokBestel.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // MenuDokOfferte
            //
            this.MenuDokOfferte.Name = "MenuDokOfferte";
            this.MenuDokOfferte.Text = "Offerte";
            this.MenuDokOfferte.Tag  = "3";
            this.MenuDokOfferte.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // MenuDokBrief
            //
            this.MenuDokBrief.Name = "MenuDokBrief";
            this.MenuDokBrief.Text = "Briefwisseling";
            this.MenuDokBrief.Tag  = "4";
            this.MenuDokBrief.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // MenuDokRekening
            //
            this.MenuDokRekening.Name = "MenuDokRekening";
            this.MenuDokRekening.Text = "Rekeninguitttreksel";
            this.MenuDokRekening.Tag  = "5";
            this.MenuDokRekening.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // MenuDokKwijting
            //
            this.MenuDokKwijting.Name = "MenuDokKwijting";
            this.MenuDokKwijting.Text = "Kwijting";
            this.MenuDokKwijting.Tag  = "6";
            this.MenuDokKwijting.Click += new System.EventHandler(this.MenuDokType_Click);
            //
            // TxtDemoTekst
            //
            this.TxtDemoTekst.Location = new System.Drawing.Point(10, 30);
            this.TxtDemoTekst.Name = "TxtDemoTekst";
            this.TxtDemoTekst.Size = new System.Drawing.Size(450, 23);
            this.TxtDemoTekst.TabIndex = 6;
            this.TxtDemoTekst.Text = "Demotekst";
            this.TxtDemoTekst.TextChanged += new System.EventHandler(this.TxtDemoTekst_TextChanged);
            //
            // BtnKleurKiezen
            //
            this.BtnKleurKiezen.Location = new System.Drawing.Point(10, 62);
            this.BtnKleurKiezen.Name = "BtnKleurKiezen";
            this.BtnKleurKiezen.Size = new System.Drawing.Size(140, 30);
            this.BtnKleurKiezen.TabIndex = 2;
            this.BtnKleurKiezen.Text = "Eerst Kleur Kiezen !";
            this.BtnKleurKiezen.Click += new System.EventHandler(this.BtnKleurKiezen_Click);
            //
            // BtnFont
            //
            this.BtnFont.Location = new System.Drawing.Point(160, 62);
            this.BtnFont.Name = "BtnFont";
            this.BtnFont.Size = new System.Drawing.Size(300, 30);
            this.BtnFont.TabIndex = 4;
            this.BtnFont.Text = "Vervolgens Font, Grootte en parameters";
            this.BtnFont.Click += new System.EventHandler(this.BtnFont_Click);
            //
            // TxtKleur
            //
            this.TxtKleur.Location = new System.Drawing.Point(10, 100);
            this.TxtKleur.Name = "TxtKleur";
            this.TxtKleur.ReadOnly = true;
            this.TxtKleur.Size = new System.Drawing.Size(140, 23);
            this.TxtKleur.TabIndex = 1;
            //
            // TxtFont
            //
            this.TxtFont.Location = new System.Drawing.Point(160, 100);
            this.TxtFont.Name = "TxtFont";
            this.TxtFont.ReadOnly = true;
            this.TxtFont.Size = new System.Drawing.Size(300, 23);
            this.TxtFont.TabIndex = 3;
            this.ToolTip1.SetToolTip(this.TxtFont, "Kies kleur, font en parameters en daarna selecteren en kopiëren voor uw eigen tekstlijnen");
            //
            // LblDemoTekst
            //
            this.LblDemoTekst.BackColor = System.Drawing.Color.White;
            this.LblDemoTekst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblDemoTekst.Location = new System.Drawing.Point(10, 132);
            this.LblDemoTekst.Name = "LblDemoTekst";
            this.LblDemoTekst.Size = new System.Drawing.Size(456, 88);
            this.LblDemoTekst.TabIndex = 0;
            this.LblDemoTekst.Text = "DemoTekst";
            this.LblDemoTekst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // LblTekstHelper
            //
            this.LblTekstHelper.Font = new System.Drawing.Font("MS Sans Serif", 13.5F, System.Drawing.FontStyle.Bold);
            this.LblTekstHelper.Location = new System.Drawing.Point(30, 230);
            this.LblTekstHelper.Name = "LblTekstHelper";
            this.LblTekstHelper.Size = new System.Drawing.Size(404, 30);
            this.LblTekstHelper.TabIndex = 5;
            this.LblTekstHelper.Text = "Teksthelper";
            this.LblTekstHelper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // FormVpeTemplateEditor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 272);
            this.Controls.Add(this.TxtDemoTekst);
            this.Controls.Add(this.BtnKleurKiezen);
            this.Controls.Add(this.BtnFont);
            this.Controls.Add(this.TxtKleur);
            this.Controls.Add(this.TxtFont);
            this.Controls.Add(this.LblDemoTekst);
            this.Controls.Add(this.LblTekstHelper);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVpeTemplateEditor";
            this.Text = "VPE-PDF AfdrukTester";
            this.Load += new System.EventHandler(this.FormVpeTemplateEditor_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormVpeTemplateEditor_FormClosing);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuBestand;
        private System.Windows.Forms.ToolStripMenuItem MenuBestandOpenen;
        private System.Windows.Forms.ToolStripSeparator MenuBestandSep1;
        private System.Windows.Forms.ToolStripMenuItem MenuViaKladblok;
        private System.Windows.Forms.ToolStripMenuItem MenuViaKBTekst;
        private System.Windows.Forms.ToolStripSeparator MenuBestandSep2;
        private System.Windows.Forms.ToolStripMenuItem MenuBestandSluiten;
        private System.Windows.Forms.ToolStripMenuItem MenuTaal;
        private System.Windows.Forms.ToolStripMenuItem MenuTaalFrans;
        private System.Windows.Forms.ToolStripMenuItem MenuTaalNederlands;
        private System.Windows.Forms.ToolStripMenuItem MenuTaalEngels;
        private System.Windows.Forms.ToolStripMenuItem MenuTaalDuits;
        private System.Windows.Forms.ToolStripMenuItem MenuDocument;
        private System.Windows.Forms.ToolStripMenuItem MenuDokFactuur;
        private System.Windows.Forms.ToolStripMenuItem MenuDokLevering;
        private System.Windows.Forms.ToolStripMenuItem MenuDokBestel;
        private System.Windows.Forms.ToolStripMenuItem MenuDokOfferte;
        private System.Windows.Forms.ToolStripMenuItem MenuDokBrief;
        private System.Windows.Forms.ToolStripMenuItem MenuDokRekening;
        private System.Windows.Forms.ToolStripMenuItem MenuDokKwijting;
        private System.Windows.Forms.TextBox TxtDemoTekst;
        private System.Windows.Forms.Button BtnKleurKiezen;
        private System.Windows.Forms.Button BtnFont;
        private System.Windows.Forms.TextBox TxtKleur;
        private System.Windows.Forms.TextBox TxtFont;
        private System.Windows.Forms.Label LblDemoTekst;
        private System.Windows.Forms.Label LblTekstHelper;
        private System.Windows.Forms.ToolTip ToolTip1;
    }
}
