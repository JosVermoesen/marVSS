namespace marVSS2028.MimMenu.Accounting
{
    partial class FormManualLedgerEntries
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
            this.cmbSoortBoeking = new System.Windows.Forms.ComboBox();
            this.txtOmschrijving = new System.Windows.Forms.TextBox();
            this.dtpDatum = new System.Windows.Forms.DateTimePicker();
            this.optDCkeuze0 = new System.Windows.Forms.RadioButton();
            this.optDCkeuze1 = new System.Windows.Forms.RadioButton();
            this.chkTRvlag = new System.Windows.Forms.CheckBox();
            this.txtRekeningNummer = new System.Windows.Forms.TextBox();
            this.txtTegenrekening = new System.Windows.Forms.TextBox();
            this.txtBedrag = new System.Windows.Forms.TextBox();
            this.btnVolgendeLijn = new System.Windows.Forms.Button();
            this.btnAfsluiten = new System.Windows.Forms.Button();
            this.btnSluiten = new System.Windows.Forms.Button();
            this.btnSchoon = new System.Windows.Forms.Button();
            this.lstJournaalPost = new System.Windows.Forms.ListBox();
            this.lblNaamTegenRekening = new System.Windows.Forms.Label();
            this.lblNaamRekening = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblDatum = new System.Windows.Forms.Label();
            this.lblBedrag = new System.Windows.Forms.Label();
            this.lblRekening = new System.Windows.Forms.Label();
            this.lblOmschrijving = new System.Windows.Forms.Label();
            this.lblSaldoCaption = new System.Windows.Forms.Label();
            this.lblColNummer = new System.Windows.Forms.Label();
            this.lblColNaam = new System.Windows.Forms.Label();
            this.lblColBedrag = new System.Windows.Forms.Label();
            this.lblColTegenR = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbSoortBoeking
            // 
            this.cmbSoortBoeking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSoortBoeking.Location = new System.Drawing.Point(4, 16);
            this.cmbSoortBoeking.Name = "cmbSoortBoeking";
            this.cmbSoortBoeking.Size = new System.Drawing.Size(179, 21);
            this.cmbSoortBoeking.TabIndex = 0;
            this.cmbSoortBoeking.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CmbSoortBoeking_KeyPress);
            // 
            // txtOmschrijving
            // 
            this.txtOmschrijving.Location = new System.Drawing.Point(184, 16);
            this.txtOmschrijving.Name = "txtOmschrijving";
            this.txtOmschrijving.Size = new System.Drawing.Size(225, 20);
            this.txtOmschrijving.TabIndex = 2;
            this.txtOmschrijving.Enter += new System.EventHandler(this.txtOmschrijving_Enter);
            this.txtOmschrijving.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtOmschrijving_KeyDown);
            this.txtOmschrijving.Leave += new System.EventHandler(this.TxtOmschrijving_Leave);
            // 
            // dtpDatum
            // 
            this.dtpDatum.CustomFormat = "dd/MM/yyyy";
            this.dtpDatum.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDatum.Location = new System.Drawing.Point(420, 16);
            this.dtpDatum.Name = "dtpDatum";
            this.dtpDatum.Size = new System.Drawing.Size(97, 20);
            this.dtpDatum.TabIndex = 14;
            this.dtpDatum.Leave += new System.EventHandler(this.DtpDatum_Leave);
            // 
            // optDCkeuze0
            // 
            this.optDCkeuze0.Checked = true;
            this.optDCkeuze0.Location = new System.Drawing.Point(192, 36);
            this.optDCkeuze0.Name = "optDCkeuze0";
            this.optDCkeuze0.Size = new System.Drawing.Size(110, 17);
            this.optDCkeuze0.TabIndex = 3;
            this.optDCkeuze0.TabStop = true;
            this.optDCkeuze0.Text = "&Debiteren (+)";
            this.optDCkeuze0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OptDCkeuze_KeyPress);
            // 
            // optDCkeuze1
            // 
            this.optDCkeuze1.Location = new System.Drawing.Point(192, 52);
            this.optDCkeuze1.Name = "optDCkeuze1";
            this.optDCkeuze1.Size = new System.Drawing.Size(110, 17);
            this.optDCkeuze1.TabIndex = 4;
            this.optDCkeuze1.Text = "&Crediteren (-)";
            this.optDCkeuze1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OptDCkeuze_KeyPress);
            // 
            // chkTRvlag
            // 
            this.chkTRvlag.Location = new System.Drawing.Point(192, 68);
            this.chkTRvlag.Name = "chkTRvlag";
            this.chkTRvlag.Size = new System.Drawing.Size(110, 17);
            this.chkTRvlag.TabIndex = 5;
            this.chkTRvlag.TabStop = false;
            this.chkTRvlag.Text = "&Tegenrekening (/)";
            this.chkTRvlag.Click += new System.EventHandler(this.ChkTRvlag_Click);
            this.chkTRvlag.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChkTRvlag_KeyPress);
            // 
            // txtRekeningNummer
            // 
            this.txtRekeningNummer.Location = new System.Drawing.Point(4, 92);
            this.txtRekeningNummer.Name = "txtRekeningNummer";
            this.txtRekeningNummer.Size = new System.Drawing.Size(80, 20);
            this.txtRekeningNummer.TabIndex = 7;            
            this.txtRekeningNummer.DoubleClick += new System.EventHandler(this.TxtRekeningNummer_DoubleClick);
            this.txtRekeningNummer.GotFocus += new System.EventHandler(this.TxtRekeningNummer_GotFocus);
            this.txtRekeningNummer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtRekeningNummer_KeyDown);
            this.txtRekeningNummer.Leave += new System.EventHandler(this.TxtRekeningNummer_Leave);
            // 
            // txtTegenrekening
            // 
            this.txtTegenrekening.Location = new System.Drawing.Point(4, 117);
            this.txtTegenrekening.Name = "txtTegenrekening";
            this.txtTegenrekening.Size = new System.Drawing.Size(80, 20);
            this.txtTegenrekening.TabIndex = 8;
            this.txtTegenrekening.Visible = false;
            this.txtTegenrekening.DoubleClick += new System.EventHandler(this.TxtTegenrekening_DoubleClick);
            this.txtTegenrekening.GotFocus += new System.EventHandler(this.TxtTegenrekening_GotFocus);
            this.txtTegenrekening.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtTegenrekening_KeyDown);
            this.txtTegenrekening.Leave += new System.EventHandler(this.TxtTegenrekening_Leave);
            // 
            // txtBedrag
            // 
            this.txtBedrag.Location = new System.Drawing.Point(91, 68);
            this.txtBedrag.Name = "txtBedrag";
            this.txtBedrag.Size = new System.Drawing.Size(88, 20);
            this.txtBedrag.TabIndex = 10;
            this.txtBedrag.TextChanged += new System.EventHandler(this.TxtBedrag_TextChanged);
            this.txtBedrag.GotFocus += new System.EventHandler(this.TxtBedrag_GotFocus);
            this.txtBedrag.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBedrag_KeyPress);
            this.txtBedrag.Leave += new System.EventHandler(this.TxtBedrag_Leave);
            // 
            // btnVolgendeLijn
            // 
            this.btnVolgendeLijn.Enabled = false;
            this.btnVolgendeLijn.Location = new System.Drawing.Point(336, 42);
            this.btnVolgendeLijn.Name = "btnVolgendeLijn";
            this.btnVolgendeLijn.Size = new System.Drawing.Size(73, 23);
            this.btnVolgendeLijn.TabIndex = 11;
            this.btnVolgendeLijn.TabStop = false;
            this.btnVolgendeLijn.Text = "Bij&voegen";
            this.btnVolgendeLijn.Click += new System.EventHandler(this.BtnVolgendeLijn_Click);
            // 
            // btnAfsluiten
            // 
            this.btnAfsluiten.Enabled = false;
            this.btnAfsluiten.Location = new System.Drawing.Point(336, 66);
            this.btnAfsluiten.Name = "btnAfsluiten";
            this.btnAfsluiten.Size = new System.Drawing.Size(73, 23);
            this.btnAfsluiten.TabIndex = 12;
            this.btnAfsluiten.TabStop = false;
            this.btnAfsluiten.Text = "Boeken";
            this.btnAfsluiten.Click += new System.EventHandler(this.BtnAfsluiten_Click);
            // 
            // btnSluiten
            // 
            this.btnSluiten.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSluiten.Location = new System.Drawing.Point(336, 114);
            this.btnSluiten.Name = "btnSluiten";
            this.btnSluiten.Size = new System.Drawing.Size(73, 23);
            this.btnSluiten.TabIndex = 15;
            this.btnSluiten.TabStop = false;
            this.btnSluiten.Text = "Sluiten";
            this.btnSluiten.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSchoon
            // 
            this.btnSchoon.Location = new System.Drawing.Point(336, 90);
            this.btnSchoon.Name = "btnSchoon";
            this.btnSchoon.Size = new System.Drawing.Size(73, 23);
            this.btnSchoon.TabIndex = 17;
            this.btnSchoon.TabStop = false;
            this.btnSchoon.Text = "Schoo&n";
            this.btnSchoon.Click += new System.EventHandler(this.Schoon_Click);
            // 
            // lstJournaalPost
            // 
            this.lstJournaalPost.BackColor = System.Drawing.Color.White;
            this.lstJournaalPost.Font = new System.Drawing.Font("Courier New", 9F);
            this.lstJournaalPost.ItemHeight = 15;
            this.lstJournaalPost.Location = new System.Drawing.Point(0, 163);
            this.lstJournaalPost.Name = "lstJournaalPost";
            this.lstJournaalPost.Size = new System.Drawing.Size(514, 319);
            this.lstJournaalPost.TabIndex = 16;
            this.lstJournaalPost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LstJournaalPost_KeyPress);
            // 
            // lblNaamTegenRekening
            // 
            this.lblNaamTegenRekening.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNaamTegenRekening.Location = new System.Drawing.Point(91, 117);
            this.lblNaamTegenRekening.Name = "lblNaamTegenRekening";
            this.lblNaamTegenRekening.Size = new System.Drawing.Size(217, 20);
            this.lblNaamTegenRekening.TabIndex = 24;
            this.lblNaamTegenRekening.Visible = false;
            // 
            // lblNaamRekening
            // 
            this.lblNaamRekening.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNaamRekening.Location = new System.Drawing.Point(91, 92);
            this.lblNaamRekening.Name = "lblNaamRekening";
            this.lblNaamRekening.Size = new System.Drawing.Size(217, 20);
            this.lblNaamRekening.TabIndex = 25;
            // 
            // lblSaldo
            // 
            this.lblSaldo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblSaldo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSaldo.Location = new System.Drawing.Point(416, 92);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(99, 21);
            this.lblSaldo.TabIndex = 18;
            this.lblSaldo.Text = "0";
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDatum
            // 
            this.lblDatum.AutoSize = true;
            this.lblDatum.Location = new System.Drawing.Point(424, 0);
            this.lblDatum.Name = "lblDatum";
            this.lblDatum.Size = new System.Drawing.Size(38, 13);
            this.lblDatum.TabIndex = 26;
            this.lblDatum.Text = "Datu&m";
            // 
            // lblBedrag
            // 
            this.lblBedrag.AutoSize = true;
            this.lblBedrag.Location = new System.Drawing.Point(128, 52);
            this.lblBedrag.Name = "lblBedrag";
            this.lblBedrag.Size = new System.Drawing.Size(41, 13);
            this.lblBedrag.TabIndex = 27;
            this.lblBedrag.Text = "&Bedrag";
            // 
            // lblRekening
            // 
            this.lblRekening.AutoSize = true;
            this.lblRekening.Location = new System.Drawing.Point(8, 72);
            this.lblRekening.Name = "lblRekening";
            this.lblRekening.Size = new System.Drawing.Size(53, 13);
            this.lblRekening.TabIndex = 28;
            this.lblRekening.Text = "&Rekening";
            // 
            // lblOmschrijving
            // 
            this.lblOmschrijving.AutoSize = true;
            this.lblOmschrijving.Location = new System.Drawing.Point(190, 0);
            this.lblOmschrijving.Name = "lblOmschrijving";
            this.lblOmschrijving.Size = new System.Drawing.Size(67, 13);
            this.lblOmschrijving.TabIndex = 29;
            this.lblOmschrijving.Text = "&Omschrijving";
            // 
            // lblSaldoCaption
            // 
            this.lblSaldoCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSaldoCaption.Location = new System.Drawing.Point(416, 56);
            this.lblSaldoCaption.Name = "lblSaldoCaption";
            this.lblSaldoCaption.Size = new System.Drawing.Size(100, 33);
            this.lblSaldoCaption.TabIndex = 19;
            this.lblSaldoCaption.Text = "Saldo nog toe te wijzen";
            this.lblSaldoCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblColNummer
            // 
            this.lblColNummer.AutoSize = true;
            this.lblColNummer.Location = new System.Drawing.Point(1, 147);
            this.lblColNummer.Name = "lblColNummer";
            this.lblColNummer.Size = new System.Drawing.Size(46, 13);
            this.lblColNummer.TabIndex = 30;
            this.lblColNummer.Text = "Nummer";
            // 
            // lblColNaam
            // 
            this.lblColNaam.AutoSize = true;
            this.lblColNaam.Location = new System.Drawing.Point(67, 147);
            this.lblColNaam.Name = "lblColNaam";
            this.lblColNaam.Size = new System.Drawing.Size(35, 13);
            this.lblColNaam.TabIndex = 31;
            this.lblColNaam.Text = "Naam";
            // 
            // lblColBedrag
            // 
            this.lblColBedrag.AutoSize = true;
            this.lblColBedrag.Location = new System.Drawing.Point(389, 147);
            this.lblColBedrag.Name = "lblColBedrag";
            this.lblColBedrag.Size = new System.Drawing.Size(41, 13);
            this.lblColBedrag.TabIndex = 32;
            this.lblColBedrag.Text = "Bedrag";
            this.lblColBedrag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblColTegenR
            // 
            this.lblColTegenR.AutoSize = true;
            this.lblColTegenR.Location = new System.Drawing.Point(436, 147);
            this.lblColTegenR.Name = "lblColTegenR";
            this.lblColTegenR.Size = new System.Drawing.Size(49, 13);
            this.lblColTegenR.TabIndex = 33;
            this.lblColTegenR.Text = "TegenR.";
            // 
            // FormManualLedgerEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnSluiten;
            this.ClientSize = new System.Drawing.Size(522, 492);
            this.Controls.Add(this.cmbSoortBoeking);
            this.Controls.Add(this.txtOmschrijving);
            this.Controls.Add(this.dtpDatum);
            this.Controls.Add(this.optDCkeuze0);
            this.Controls.Add(this.optDCkeuze1);
            this.Controls.Add(this.chkTRvlag);
            this.Controls.Add(this.txtRekeningNummer);
            this.Controls.Add(this.txtTegenrekening);
            this.Controls.Add(this.txtBedrag);
            this.Controls.Add(this.btnVolgendeLijn);
            this.Controls.Add(this.btnAfsluiten);
            this.Controls.Add(this.btnSluiten);
            this.Controls.Add(this.btnSchoon);
            this.Controls.Add(this.lstJournaalPost);
            this.Controls.Add(this.lblNaamTegenRekening);
            this.Controls.Add(this.lblNaamRekening);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.lblSaldoCaption);
            this.Controls.Add(this.lblDatum);
            this.Controls.Add(this.lblBedrag);
            this.Controls.Add(this.lblRekening);
            this.Controls.Add(this.lblOmschrijving);
            this.Controls.Add(this.lblColNummer);
            this.Controls.Add(this.lblColNaam);
            this.Controls.Add(this.lblColBedrag);
            this.Controls.Add(this.lblColTegenR);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormManualLedgerEntries";
            this.Text = "Ctrl+F5 Diverse Posten";
            this.Load += new System.EventHandler(this.FormDiversePosten_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSoortBoeking;
        private System.Windows.Forms.TextBox txtOmschrijving;
        private System.Windows.Forms.DateTimePicker dtpDatum;
        private System.Windows.Forms.RadioButton optDCkeuze0;
        private System.Windows.Forms.RadioButton optDCkeuze1;
        private System.Windows.Forms.CheckBox chkTRvlag;
        private System.Windows.Forms.TextBox txtRekeningNummer;
        private System.Windows.Forms.TextBox txtTegenrekening;
        private System.Windows.Forms.TextBox txtBedrag;
        private System.Windows.Forms.Button btnVolgendeLijn;
        private System.Windows.Forms.Button btnAfsluiten;
        private System.Windows.Forms.Button btnSluiten;
        private System.Windows.Forms.Button btnSchoon;
        private System.Windows.Forms.ListBox lstJournaalPost;
        private System.Windows.Forms.Label lblNaamTegenRekening;
        private System.Windows.Forms.Label lblNaamRekening;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblDatum;
        private System.Windows.Forms.Label lblBedrag;
        private System.Windows.Forms.Label lblRekening;
        private System.Windows.Forms.Label lblOmschrijving;
        private System.Windows.Forms.Label lblSaldoCaption;
        private System.Windows.Forms.Label lblColNummer;
        private System.Windows.Forms.Label lblColNaam;
        private System.Windows.Forms.Label lblColBedrag;
        private System.Windows.Forms.Label lblColTegenR;
    }
}
