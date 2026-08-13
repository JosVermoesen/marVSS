namespace marVSS2028.MimMenu.Accounting
{
    partial class FormBalancePurchaseAndSales
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.CheckBoxFinanceDetail = new System.Windows.Forms.CheckBox();
            this.txtSubTitel = new System.Windows.Forms.TextBox();
            this.CheckBoxLedgerDetail = new System.Windows.Forms.CheckBox();
            this.CheckBoxExclude1994 = new System.Windows.Forms.CheckBox();
            this.CheckBoxNotPaid = new System.Windows.Forms.CheckBox();
            this.CheckBoxOnlyThisPeriod = new System.Windows.Forms.CheckBox();
            this.CheckBoxExcludeOutOfPeriod = new System.Windows.Forms.CheckBox();
            this.CheckBoxExpiryDate = new System.Windows.Forms.CheckBox();
            this.cmdStandaardBetaling = new System.Windows.Forms.Button();
            this.cmdBewaar = new System.Windows.Forms.Button();
            this.cmdStandaard = new System.Windows.Forms.Button();
            this.txtPeriode = new System.Windows.Forms.TextBox();
            this.txtDatum = new System.Windows.Forms.TextBox();
            this.txtVan = new System.Windows.Forms.TextBox();
            this.txtTot = new System.Windows.Forms.TextBox();
            this.btnDrukken = new System.Windows.Forms.Button();
            this.lblPeriode = new System.Windows.Forms.Label();
            this.lblSubTitel = new System.Windows.Forms.Label();
            this.lblDatum = new System.Windows.Forms.Label();
            this.lblVan = new System.Windows.Forms.Label();
            this.lblTot = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckBoxFinanceDetail
            // 
            this.CheckBoxFinanceDetail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxFinanceDetail.Checked = true;
            this.CheckBoxFinanceDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxFinanceDetail.Location = new System.Drawing.Point(300, 139);
            this.CheckBoxFinanceDetail.Name = "CheckBoxFinanceDetail";
            this.CheckBoxFinanceDetail.Size = new System.Drawing.Size(179, 16);
            this.CheckBoxFinanceDetail.TabIndex = 27;
            this.CheckBoxFinanceDetail.Text = "&Financieel Detail Journaal";
            this.CheckBoxFinanceDetail.CheckedChanged += new System.EventHandler(this.ChkFinancieelDetail_CheckedChanged);
            // 
            // txtSubTitel
            // 
            this.txtSubTitel.Location = new System.Drawing.Point(4, 20);
            this.txtSubTitel.Name = "txtSubTitel";
            this.txtSubTitel.Size = new System.Drawing.Size(251, 20);
            this.txtSubTitel.TabIndex = 1;
            this.txtSubTitel.Enter += new System.EventHandler(this.TekstLijn_Enter);
            // 
            // CheckBoxLedgerDetail
            // 
            this.CheckBoxLedgerDetail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxLedgerDetail.Checked = true;
            this.CheckBoxLedgerDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxLedgerDetail.Location = new System.Drawing.Point(294, 161);
            this.CheckBoxLedgerDetail.Name = "CheckBoxLedgerDetail";
            this.CheckBoxLedgerDetail.Size = new System.Drawing.Size(185, 16);
            this.CheckBoxLedgerDetail.TabIndex = 12;
            this.CheckBoxLedgerDetail.Text = "Betalingsjournaal &weergeven";
            // 
            // CheckBoxExclude1994
            // 
            this.CheckBoxExclude1994.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxExclude1994.Checked = true;
            this.CheckBoxExclude1994.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxExclude1994.Location = new System.Drawing.Point(294, 115);
            this.CheckBoxExclude1994.Name = "CheckBoxExclude1994";
            this.CheckBoxExclude1994.Size = new System.Drawing.Size(185, 16);
            this.CheckBoxExclude1994.TabIndex = 11;
            this.CheckBoxExclude1994.TabStop = false;
            this.CheckBoxExclude1994.Text = "Documenten -1994 &uitsluiten";
            this.CheckBoxExclude1994.CheckedChanged += new System.EventHandler(this.ChkSelektie4_CheckedChanged);
            // 
            // CheckBoxNotPaid
            // 
            this.CheckBoxNotPaid.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxNotPaid.Checked = true;
            this.CheckBoxNotPaid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxNotPaid.Location = new System.Drawing.Point(308, 90);
            this.CheckBoxNotPaid.Name = "CheckBoxNotPaid";
            this.CheckBoxNotPaid.Size = new System.Drawing.Size(171, 16);
            this.CheckBoxNotPaid.TabIndex = 10;
            this.CheckBoxNotPaid.Text = "&Niet betaalde documenten";
            // 
            // CheckBoxOnlyThisPeriod
            // 
            this.CheckBoxOnlyThisPeriod.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxOnlyThisPeriod.Checked = true;
            this.CheckBoxOnlyThisPeriod.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxOnlyThisPeriod.Location = new System.Drawing.Point(4, 139);
            this.CheckBoxOnlyThisPeriod.Name = "CheckBoxOnlyThisPeriod";
            this.CheckBoxOnlyThisPeriod.Size = new System.Drawing.Size(193, 17);
            this.CheckBoxOnlyThisPeriod.TabIndex = 15;
            this.CheckBoxOnlyThisPeriod.Text = "Enkel docum&enten deze periode";
            // 
            // CheckBoxExcludeOutOfPeriod
            // 
            this.CheckBoxExcludeOutOfPeriod.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxExcludeOutOfPeriod.Checked = true;
            this.CheckBoxExcludeOutOfPeriod.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxExcludeOutOfPeriod.Location = new System.Drawing.Point(256, 68);
            this.CheckBoxExcludeOutOfPeriod.Name = "CheckBoxExcludeOutOfPeriod";
            this.CheckBoxExcludeOutOfPeriod.Size = new System.Drawing.Size(223, 16);
            this.CheckBoxExcludeOutOfPeriod.TabIndex = 9;
            this.CheckBoxExcludeOutOfPeriod.Text = "Betalingen &buiten periode uitsluiten";
            // 
            // CheckBoxExpiryDate
            // 
            this.CheckBoxExpiryDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxExpiryDate.Location = new System.Drawing.Point(255, 46);
            this.CheckBoxExpiryDate.Name = "CheckBoxExpiryDate";
            this.CheckBoxExpiryDate.Size = new System.Drawing.Size(125, 16);
            this.CheckBoxExpiryDate.TabIndex = 8;
            this.CheckBoxExpiryDate.Text = "&Controle Vervaldag";
            this.CheckBoxExpiryDate.CheckedChanged += new System.EventHandler(this.ChkSelektie0_CheckedChanged);
            // 
            // cmdStandaardBetaling
            // 
            this.cmdStandaardBetaling.Location = new System.Drawing.Point(4, 191);
            this.cmdStandaardBetaling.Name = "cmdStandaardBetaling";
            this.cmdStandaardBetaling.Size = new System.Drawing.Size(171, 23);
            this.cmdStandaardBetaling.TabIndex = 20;
            this.cmdStandaardBetaling.TabStop = false;
            this.cmdStandaardBetaling.Text = "Standaard Betalingskontrole";
            this.cmdStandaardBetaling.Click += new System.EventHandler(this.CmdStandaardBetaling_Click);
            // 
            // cmdBewaar
            // 
            this.cmdBewaar.Location = new System.Drawing.Point(181, 191);
            this.cmdBewaar.Name = "cmdBewaar";
            this.cmdBewaar.Size = new System.Drawing.Size(119, 23);
            this.cmdBewaar.TabIndex = 19;
            this.cmdBewaar.TabStop = false;
            this.cmdBewaar.Text = "Bewaar instellingen";
            this.cmdBewaar.Click += new System.EventHandler(this.CmdBewaar_Click);
            // 
            // cmdStandaard
            // 
            this.cmdStandaard.Location = new System.Drawing.Point(4, 162);
            this.cmdStandaard.Name = "cmdStandaard";
            this.cmdStandaard.Size = new System.Drawing.Size(171, 23);
            this.cmdStandaard.TabIndex = 18;
            this.cmdStandaard.TabStop = false;
            this.cmdStandaard.Text = "Standaard Boekhoudkontrole";
            this.cmdStandaard.Click += new System.EventHandler(this.CmdStandaard_Click);
            // 
            // txtPeriode
            // 
            this.txtPeriode.Location = new System.Drawing.Point(4, 111);
            this.txtPeriode.Name = "txtPeriode";
            this.txtPeriode.Size = new System.Drawing.Size(193, 20);
            this.txtPeriode.TabIndex = 14;
            this.txtPeriode.Enter += new System.EventHandler(this.TekstLijn_Enter);
            this.txtPeriode.Leave += new System.EventHandler(this.TxtPeriode_Leave);
            // 
            // txtDatum
            // 
            this.txtDatum.Location = new System.Drawing.Point(296, 20);
            this.txtDatum.Name = "txtDatum";
            this.txtDatum.Size = new System.Drawing.Size(84, 20);
            this.txtDatum.TabIndex = 3;
            this.txtDatum.Enter += new System.EventHandler(this.TekstLijn_Enter);
            this.txtDatum.Leave += new System.EventHandler(this.TxtDatum_Leave);
            // 
            // txtVan
            // 
            this.txtVan.Location = new System.Drawing.Point(4, 64);
            this.txtVan.Name = "txtVan";
            this.txtVan.Size = new System.Drawing.Size(107, 20);
            this.txtVan.TabIndex = 5;
            this.txtVan.Enter += new System.EventHandler(this.TekstLijn_Enter);
            // 
            // txtTot
            // 
            this.txtTot.Location = new System.Drawing.Point(117, 64);
            this.txtTot.Name = "txtTot";
            this.txtTot.Size = new System.Drawing.Size(107, 20);
            this.txtTot.TabIndex = 6;
            this.txtTot.Enter += new System.EventHandler(this.TekstLijn_Enter);
            // 
            // btnDrukken
            // 
            this.btnDrukken.Location = new System.Drawing.Point(409, 8);
            this.btnDrukken.Name = "btnDrukken";
            this.btnDrukken.Size = new System.Drawing.Size(75, 42);
            this.btnDrukken.TabIndex = 7;
            this.btnDrukken.Text = "Rapport Genereren";
            this.btnDrukken.Click += new System.EventHandler(this.Drukken_Click);
            // 
            // lblPeriode
            // 
            this.lblPeriode.BackColor = System.Drawing.SystemColors.Control;
            this.lblPeriode.Location = new System.Drawing.Point(11, 95);
            this.lblPeriode.Name = "lblPeriode";
            this.lblPeriode.Size = new System.Drawing.Size(102, 14);
            this.lblPeriode.TabIndex = 13;
            this.lblPeriode.Text = "&Periode van - tot";
            // 
            // lblSubTitel
            // 
            this.lblSubTitel.Location = new System.Drawing.Point(8, 4);
            this.lblSubTitel.Name = "lblSubTitel";
            this.lblSubTitel.Size = new System.Drawing.Size(64, 16);
            this.lblSubTitel.TabIndex = 0;
            this.lblSubTitel.Text = "Sub&Titel";
            // 
            // lblDatum
            // 
            this.lblDatum.Location = new System.Drawing.Point(305, 4);
            this.lblDatum.Name = "lblDatum";
            this.lblDatum.Size = new System.Drawing.Size(41, 16);
            this.lblDatum.TabIndex = 2;
            this.lblDatum.Text = "Datu&m";
            // 
            // lblVan
            // 
            this.lblVan.Location = new System.Drawing.Point(8, 47);
            this.lblVan.Name = "lblVan";
            this.lblVan.Size = new System.Drawing.Size(32, 16);
            this.lblVan.TabIndex = 4;
            this.lblVan.Text = "&Van";
            // 
            // lblTot
            // 
            this.lblTot.Location = new System.Drawing.Point(117, 47);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(32, 16);
            this.lblTot.TabIndex = 17;
            this.lblTot.Text = "Tot";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(409, 191);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 28;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormBalancePurchaseAndSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(496, 219);
            this.ControlBox = false;
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.lblSubTitel);
            this.Controls.Add(this.txtSubTitel);
            this.Controls.Add(this.lblDatum);
            this.Controls.Add(this.txtDatum);
            this.Controls.Add(this.lblVan);
            this.Controls.Add(this.txtVan);
            this.Controls.Add(this.lblTot);
            this.Controls.Add(this.txtTot);
            this.Controls.Add(this.CheckBoxExpiryDate);
            this.Controls.Add(this.CheckBoxExcludeOutOfPeriod);
            this.Controls.Add(this.CheckBoxOnlyThisPeriod);
            this.Controls.Add(this.CheckBoxNotPaid);
            this.Controls.Add(this.CheckBoxExclude1994);
            this.Controls.Add(this.CheckBoxLedgerDetail);
            this.Controls.Add(this.CheckBoxFinanceDetail);
            this.Controls.Add(this.cmdBewaar);
            this.Controls.Add(this.cmdStandaard);
            this.Controls.Add(this.cmdStandaardBetaling);
            this.Controls.Add(this.lblPeriode);
            this.Controls.Add(this.txtPeriode);
            this.Controls.Add(this.btnDrukken);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBalancePurchaseAndSales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Balans";
            this.Load += new System.EventHandler(this.FormBalancePurchaseAndSales_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.CheckBox CheckBoxFinanceDetail;
        private System.Windows.Forms.TextBox  txtSubTitel;
        private System.Windows.Forms.CheckBox CheckBoxLedgerDetail;
        private System.Windows.Forms.CheckBox CheckBoxExclude1994;
        private System.Windows.Forms.CheckBox CheckBoxNotPaid;
        private System.Windows.Forms.CheckBox CheckBoxOnlyThisPeriod;
        private System.Windows.Forms.CheckBox CheckBoxExcludeOutOfPeriod;
        private System.Windows.Forms.CheckBox CheckBoxExpiryDate;
        private System.Windows.Forms.Button   cmdStandaardBetaling;
        private System.Windows.Forms.Button   cmdBewaar;
        private System.Windows.Forms.Button   cmdStandaard;
        private System.Windows.Forms.TextBox  txtPeriode;
        private System.Windows.Forms.TextBox  txtDatum;
        private System.Windows.Forms.TextBox  txtVan;
        private System.Windows.Forms.TextBox  txtTot;
        private System.Windows.Forms.Button   btnDrukken;
        private System.Windows.Forms.Label    lblPeriode;
        private System.Windows.Forms.Label    lblSubTitel;
        private System.Windows.Forms.Label    lblDatum;
        private System.Windows.Forms.Label    lblVan;
        private System.Windows.Forms.Label    lblTot;
        private System.Windows.Forms.Button ButtonClose;
    }
}
