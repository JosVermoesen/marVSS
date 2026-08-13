namespace marVSS2028.MimMenu.Filing
{
    partial class FormLedgerSQL
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
            this.grdJournaalDetail = new System.Windows.Forms.DataGridView();
            this.sluiten = new System.Windows.Forms.Button();
            this.cmdHoger = new System.Windows.Forms.Button();
            this.cmdLager = new System.Windows.Forms.Button();
            this.cbKlembord = new System.Windows.Forms.Button();
            this.txtLijnen = new System.Windows.Forms.TextBox();
            this.lblTussenstop = new System.Windows.Forms.Label();
            this.lblSaldoCaption = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblCreditSaldo = new System.Windows.Forms.Label();
            this.lblLijnen = new System.Windows.Forms.Label();
            this.gansePeriode = new System.Windows.Forms.CheckBox();
            this.lblRekening = new System.Windows.Forms.Label();
            this.rekening = new System.Windows.Forms.TextBox();
            this.zoeken = new System.Windows.Forms.Button();
            this.lblPeriode = new System.Windows.Forms.Label();
            this.tekstLijn = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdJournaalDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // grdJournaalDetail
            // 
            this.grdJournaalDetail.AllowUserToAddRows = false;
            this.grdJournaalDetail.AllowUserToDeleteRows = false;
            this.grdJournaalDetail.AllowUserToResizeColumns = false;
            this.grdJournaalDetail.AllowUserToResizeRows = false;
            this.grdJournaalDetail.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.grdJournaalDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdJournaalDetail.Location = new System.Drawing.Point(0, 22);
            this.grdJournaalDetail.Name = "grdJournaalDetail";
            this.grdJournaalDetail.ReadOnly = true;
            this.grdJournaalDetail.RowHeadersVisible = false;
            this.grdJournaalDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdJournaalDetail.Size = new System.Drawing.Size(567, 391);
            this.grdJournaalDetail.TabIndex = 14;
            // 
            // sluiten
            // 
            this.sluiten.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sluiten.Location = new System.Drawing.Point(576, 21);
            this.sluiten.Name = "sluiten";
            this.sluiten.Size = new System.Drawing.Size(90, 23);
            this.sluiten.TabIndex = 5;
            this.sluiten.TabStop = false;
            this.sluiten.Text = "Sluiten";
            this.sluiten.UseVisualStyleBackColor = true;
            this.sluiten.Click += new System.EventHandler(this.Sluiten_Click);
            // 
            // cmdHoger
            // 
            this.cmdHoger.Location = new System.Drawing.Point(576, 50);
            this.cmdHoger.Name = "cmdHoger";
            this.cmdHoger.Size = new System.Drawing.Size(90, 23);
            this.cmdHoger.TabIndex = 6;
            this.cmdHoger.TabStop = false;
            this.cmdHoger.Text = "&Volgende";
            this.cmdHoger.UseVisualStyleBackColor = true;
            this.cmdHoger.Click += new System.EventHandler(this.CmdHoger_Click);
            // 
            // cmdLager
            // 
            this.cmdLager.Location = new System.Drawing.Point(576, 79);
            this.cmdLager.Name = "cmdLager";
            this.cmdLager.Size = new System.Drawing.Size(90, 23);
            this.cmdLager.TabIndex = 7;
            this.cmdLager.TabStop = false;
            this.cmdLager.Text = "&Vorige";
            this.cmdLager.UseVisualStyleBackColor = true;
            this.cmdLager.Click += new System.EventHandler(this.CmdLager_Click);
            // 
            // cbKlembord
            // 
            this.cbKlembord.Location = new System.Drawing.Point(576, 108);
            this.cbKlembord.Name = "cbKlembord";
            this.cbKlembord.Size = new System.Drawing.Size(90, 23);
            this.cbKlembord.TabIndex = 17;
            this.cbKlembord.Text = "Naar Klembord";
            this.cbKlembord.UseVisualStyleBackColor = true;
            this.cbKlembord.Click += new System.EventHandler(this.CbKlembord_Click);
            // 
            // txtLijnen
            // 
            this.txtLijnen.BackColor = System.Drawing.Color.White;
            this.txtLijnen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLijnen.Location = new System.Drawing.Point(576, 338);
            this.txtLijnen.MaxLength = 4;
            this.txtLijnen.Name = "txtLijnen";
            this.txtLijnen.Size = new System.Drawing.Size(49, 20);
            this.txtLijnen.TabIndex = 9;
            this.txtLijnen.TabStop = false;
            // 
            // lblTussenstop
            // 
            this.lblTussenstop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTussenstop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTussenstop.Location = new System.Drawing.Point(576, 273);
            this.lblTussenstop.Name = "lblTussenstop";
            this.lblTussenstop.Size = new System.Drawing.Size(91, 30);
            this.lblTussenstop.TabIndex = 8;
            this.lblTussenstop.Text = "Tussenstop &Melden na:";
            // 
            // lblSaldoCaption
            // 
            this.lblSaldoCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSaldoCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldoCaption.Location = new System.Drawing.Point(576, 162);
            this.lblSaldoCaption.Name = "lblSaldoCaption";
            this.lblSaldoCaption.Size = new System.Drawing.Size(93, 33);
            this.lblSaldoCaption.TabIndex = 11;
            this.lblSaldoCaption.Text = "Saldo huidige selektie";
            // 
            // lblSaldo
            // 
            this.lblSaldo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblSaldo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSaldo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.Location = new System.Drawing.Point(576, 206);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(93, 17);
            this.lblSaldo.TabIndex = 12;
            this.lblSaldo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCreditSaldo
            // 
            this.lblCreditSaldo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCreditSaldo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditSaldo.Location = new System.Drawing.Point(576, 232);
            this.lblCreditSaldo.Name = "lblCreditSaldo";
            this.lblCreditSaldo.Size = new System.Drawing.Size(93, 30);
            this.lblCreditSaldo.TabIndex = 13;
            this.lblCreditSaldo.Text = "Creditsaldo = bedrag in min !";
            // 
            // lblLijnen
            // 
            this.lblLijnen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLijnen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLijnen.Location = new System.Drawing.Point(576, 314);
            this.lblLijnen.Name = "lblLijnen";
            this.lblLijnen.Size = new System.Drawing.Size(49, 21);
            this.lblLijnen.TabIndex = 10;
            this.lblLijnen.Text = "Lijnen";
            // 
            // gansePeriode
            // 
            this.gansePeriode.Checked = true;
            this.gansePeriode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gansePeriode.Enabled = false;
            this.gansePeriode.Location = new System.Drawing.Point(576, 373);
            this.gansePeriode.Name = "gansePeriode";
            this.gansePeriode.Size = new System.Drawing.Size(74, 17);
            this.gansePeriode.TabIndex = 15;
            this.gansePeriode.Text = "&Boekjaar";
            this.gansePeriode.UseVisualStyleBackColor = true;
            this.gansePeriode.Click += new System.EventHandler(this.GansePeriode_Click);
            // 
            // lblRekening
            // 
            this.lblRekening.AutoSize = true;
            this.lblRekening.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRekening.Location = new System.Drawing.Point(364, 423);
            this.lblRekening.Name = "lblRekening";
            this.lblRekening.Size = new System.Drawing.Size(53, 13);
            this.lblRekening.TabIndex = 0;
            this.lblRekening.Text = "&Rekening";
            // 
            // rekening
            // 
            this.rekening.BackColor = System.Drawing.Color.White;
            this.rekening.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rekening.Location = new System.Drawing.Point(475, 421);
            this.rekening.Name = "rekening";
            this.rekening.Size = new System.Drawing.Size(85, 20);
            this.rekening.TabIndex = 1;
            this.rekening.Enter += new System.EventHandler(this.Rekening_Enter);
            this.rekening.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Rekening_KeyDown);
            this.rekening.Leave += new System.EventHandler(this.Rekening_Leave);
            // 
            // zoeken
            // 
            this.zoeken.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoeken.Location = new System.Drawing.Point(576, 418);
            this.zoeken.Name = "zoeken";
            this.zoeken.Size = new System.Drawing.Size(75, 23);
            this.zoeken.TabIndex = 4;
            this.zoeken.Text = "&Zoek";
            this.zoeken.UseVisualStyleBackColor = true;
            this.zoeken.Click += new System.EventHandler(this.Zoeken_Click);
            // 
            // lblPeriode
            // 
            this.lblPeriode.AutoSize = true;
            this.lblPeriode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriode.Location = new System.Drawing.Point(5, 425);
            this.lblPeriode.Name = "lblPeriode";
            this.lblPeriode.Size = new System.Drawing.Size(90, 13);
            this.lblPeriode.TabIndex = 2;
            this.lblPeriode.Text = "&Periode Van - Tot";
            // 
            // tekstLijn
            // 
            this.tekstLijn.BackColor = System.Drawing.Color.White;
            this.tekstLijn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tekstLijn.Location = new System.Drawing.Point(133, 422);
            this.tekstLijn.Name = "tekstLijn";
            this.tekstLijn.Size = new System.Drawing.Size(169, 20);
            this.tekstLijn.TabIndex = 3;
            this.tekstLijn.Leave += new System.EventHandler(this.TekstLijn_Leave);
            // 
            // FormLedgerSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.sluiten;
            this.ClientSize = new System.Drawing.Size(679, 454);
            this.Controls.Add(this.grdJournaalDetail);
            this.Controls.Add(this.sluiten);
            this.Controls.Add(this.cmdHoger);
            this.Controls.Add(this.cmdLager);
            this.Controls.Add(this.cbKlembord);
            this.Controls.Add(this.lblTussenstop);
            this.Controls.Add(this.txtLijnen);
            this.Controls.Add(this.lblLijnen);
            this.Controls.Add(this.lblSaldoCaption);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.lblCreditSaldo);
            this.Controls.Add(this.gansePeriode);
            this.Controls.Add(this.lblRekening);
            this.Controls.Add(this.rekening);
            this.Controls.Add(this.zoeken);
            this.Controls.Add(this.lblPeriode);
            this.Controls.Add(this.tekstLijn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormLedgerSQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historiek";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormLedgerSQL_FormClosed);
            this.Load += new System.EventHandler(this.FormLedgerSQL_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdJournaalDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView  grdJournaalDetail;
        private System.Windows.Forms.Button        sluiten;
        private System.Windows.Forms.Button        cmdHoger;
        private System.Windows.Forms.Button        cmdLager;
        private System.Windows.Forms.Button        cbKlembord;
        private System.Windows.Forms.TextBox       txtLijnen;
        private System.Windows.Forms.Label         lblTussenstop;
        private System.Windows.Forms.Label         lblSaldoCaption;
        private System.Windows.Forms.Label         lblSaldo;
        private System.Windows.Forms.Label         lblCreditSaldo;
        private System.Windows.Forms.Label         lblLijnen;
        private System.Windows.Forms.CheckBox      gansePeriode;
        private System.Windows.Forms.Label         lblRekening;
        private System.Windows.Forms.TextBox       rekening;
        private System.Windows.Forms.Button        zoeken;
        private System.Windows.Forms.Label         lblPeriode;
        private System.Windows.Forms.TextBox       tekstLijn;
    }
}
