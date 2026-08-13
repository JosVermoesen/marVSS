namespace marVSS2028.MimMenu.Filing
{
    partial class FormProductReporting
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
            this.LblSubTitel = new System.Windows.Forms.Label();
            this.LblDatumDrukken = new System.Windows.Forms.Label();
            this.LblVanTot = new System.Windows.Forms.Label();
            this.TekstLijn0 = new System.Windows.Forms.TextBox();
            this.TekstLijn1 = new System.Windows.Forms.TextBox();
            this.Drukken = new System.Windows.Forms.Button();
            this.Annuleren = new System.Windows.Forms.Button();
            this.CmbLijstType = new System.Windows.Forms.ComboBox();
            this.Sortering = new System.Windows.Forms.ComboBox();
            this.TekstInfo0 = new System.Windows.Forms.TextBox();
            this.TekstInfo1 = new System.Windows.Forms.TextBox();
            this.CbNulWaarden = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LblSubTitel
            // 
            this.LblSubTitel.AutoSize = true;
            this.LblSubTitel.Location = new System.Drawing.Point(18, 4);
            this.LblSubTitel.Name = "LblSubTitel";
            this.LblSubTitel.Size = new System.Drawing.Size(46, 13);
            this.LblSubTitel.TabIndex = 2;
            this.LblSubTitel.Text = "&SubTitel";
            // 
            // LblDatumDrukken
            // 
            this.LblDatumDrukken.AutoSize = true;
            this.LblDatumDrukken.Location = new System.Drawing.Point(98, 4);
            this.LblDatumDrukken.Name = "LblDatumDrukken";
            this.LblDatumDrukken.Size = new System.Drawing.Size(82, 13);
            this.LblDatumDrukken.TabIndex = 0;
            this.LblDatumDrukken.Text = "Datu&m Drukken";
            // 
            // LblVanTot
            // 
            this.LblVanTot.AutoSize = true;
            this.LblVanTot.Location = new System.Drawing.Point(8, 92);
            this.LblVanTot.Name = "LblVanTot";
            this.LblVanTot.Size = new System.Drawing.Size(47, 13);
            this.LblVanTot.TabIndex = 6;
            this.LblVanTot.Text = "&Van - tot";
            // 
            // TekstLijn0
            // 
            this.TekstLijn0.BackColor = System.Drawing.Color.White;
            this.TekstLijn0.ForeColor = System.Drawing.Color.Black;
            this.TekstLijn0.Location = new System.Drawing.Point(8, 20);
            this.TekstLijn0.Name = "TekstLijn0";
            this.TekstLijn0.Size = new System.Drawing.Size(288, 20);
            this.TekstLijn0.TabIndex = 3;
            this.TekstLijn0.GotFocus += new System.EventHandler(this.TekstLijn0_GotFocus);
            this.TekstLijn0.Leave += new System.EventHandler(this.TekstLijn0_Leave);
            // 
            // TekstLijn1
            // 
            this.TekstLijn1.BackColor = System.Drawing.Color.White;
            this.TekstLijn1.ForeColor = System.Drawing.Color.Black;
            this.TekstLijn1.Location = new System.Drawing.Point(192, 0);
            this.TekstLijn1.Name = "TekstLijn1";
            this.TekstLijn1.Size = new System.Drawing.Size(104, 20);
            this.TekstLijn1.TabIndex = 1;
            this.TekstLijn1.GotFocus += new System.EventHandler(this.TekstLijn1_GotFocus);
            this.TekstLijn1.Leave += new System.EventHandler(this.TekstLijn1_Leave);
            // 
            // Drukken
            // 
            this.Drukken.Location = new System.Drawing.Point(302, 2);
            this.Drukken.Name = "Drukken";
            this.Drukken.Size = new System.Drawing.Size(96, 38);
            this.Drukken.TabIndex = 9;
            this.Drukken.Text = "Rapport Genereren";
            this.Drukken.Click += new System.EventHandler(this.Drukken_Click);
            // 
            // Annuleren
            // 
            this.Annuleren.Location = new System.Drawing.Point(302, 112);
            this.Annuleren.Name = "Annuleren";
            this.Annuleren.Size = new System.Drawing.Size(96, 23);
            this.Annuleren.TabIndex = 10;
            this.Annuleren.TabStop = false;
            this.Annuleren.Text = "&Sluiten";
            this.Annuleren.Click += new System.EventHandler(this.Annuleren_Click);
            // 
            // CmbLijstType
            // 
            this.CmbLijstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbLijstType.Location = new System.Drawing.Point(60, 42);
            this.CmbLijstType.Name = "CmbLijstType";
            this.CmbLijstType.Size = new System.Drawing.Size(237, 21);
            this.CmbLijstType.TabIndex = 4;
            this.CmbLijstType.SelectedIndexChanged += new System.EventHandler(this.CmbLijstType_SelectedIndexChanged);
            // 
            // Sortering
            // 
            this.Sortering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Sortering.Location = new System.Drawing.Point(60, 64);
            this.Sortering.Name = "Sortering";
            this.Sortering.Size = new System.Drawing.Size(237, 21);
            this.Sortering.TabIndex = 5;
            this.Sortering.SelectedIndexChanged += new System.EventHandler(this.Sortering_SelectedIndexChanged);
            // 
            // TekstInfo0
            // 
            this.TekstInfo0.Location = new System.Drawing.Point(60, 86);
            this.TekstInfo0.Name = "TekstInfo0";
            this.TekstInfo0.Size = new System.Drawing.Size(109, 20);
            this.TekstInfo0.TabIndex = 7;
            // 
            // TekstInfo1
            // 
            this.TekstInfo1.Location = new System.Drawing.Point(172, 86);
            this.TekstInfo1.Name = "TekstInfo1";
            this.TekstInfo1.Size = new System.Drawing.Size(125, 20);
            this.TekstInfo1.TabIndex = 8;
            this.TekstInfo1.Text = "zzzzzzzzzzzzzzzzzzzz";
            // 
            // CbNulWaarden
            // 
            this.CbNulWaarden.Checked = true;
            this.CbNulWaarden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbNulWaarden.Location = new System.Drawing.Point(64, 112);
            this.CbNulWaarden.Name = "CbNulWaarden";
            this.CbNulWaarden.Size = new System.Drawing.Size(233, 17);
            this.CbNulWaarden.TabIndex = 13;
            this.CbNulWaarden.TabStop = false;
            this.CbNulWaarden.Text = "Stockwaarden cijfers <= 0 uitsluiten";
            this.CbNulWaarden.Visible = false;
            // 
            // FormProductReporting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 140);
            this.ControlBox = false;
            this.Controls.Add(this.LblSubTitel);
            this.Controls.Add(this.LblDatumDrukken);
            this.Controls.Add(this.LblVanTot);
            this.Controls.Add(this.TekstLijn0);
            this.Controls.Add(this.TekstLijn1);
            this.Controls.Add(this.Drukken);
            this.Controls.Add(this.Annuleren);
            this.Controls.Add(this.CmbLijstType);
            this.Controls.Add(this.Sortering);
            this.Controls.Add(this.TekstInfo0);
            this.Controls.Add(this.TekstInfo1);
            this.Controls.Add(this.CbNulWaarden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProductReporting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Produktlijsten (rekenkundig)";
            this.Load += new System.EventHandler(this.FormProductReporting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label    LblSubTitel;
        private System.Windows.Forms.Label    LblDatumDrukken;
        private System.Windows.Forms.Label    LblVanTot;
        private System.Windows.Forms.TextBox  TekstLijn0;
        private System.Windows.Forms.TextBox  TekstLijn1;
        private System.Windows.Forms.Button   Drukken;
        private System.Windows.Forms.Button   Annuleren;
        private System.Windows.Forms.ComboBox CmbLijstType;
        private System.Windows.Forms.ComboBox Sortering;
        private System.Windows.Forms.TextBox  TekstInfo0;
        private System.Windows.Forms.TextBox  TekstInfo1;
        private System.Windows.Forms.CheckBox CbNulWaarden;
    }
}