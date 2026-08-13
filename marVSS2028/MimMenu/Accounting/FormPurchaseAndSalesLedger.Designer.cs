namespace marVSS2028.MimMenu.Accounting
{
    partial class FormPurchaseAndSalesLedger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.RadioFacturen = new System.Windows.Forms.RadioButton();
            this.RadioCreditnotas = new System.Windows.Forms.RadioButton();
            this.DetailJournalCheckBox = new System.Windows.Forms.CheckBox();
            this.SubTitleLabel = new System.Windows.Forms.Label();
            this.SubTitleTextBox = new System.Windows.Forms.TextBox();
            this.ProcessingDateLabel = new System.Windows.Forms.Label();
            this.ProcessingDate = new System.Windows.Forms.DateTimePicker();
            this.PeriodFromLabel = new System.Windows.Forms.Label();
            this.DateFromLabel = new System.Windows.Forms.Label();
            this.PeriodToLabel = new System.Windows.Forms.Label();
            this.DateToLabel = new System.Windows.Forms.Label();
            this.DocRangeLabel = new System.Windows.Forms.Label();
            this.DocFromLabel = new System.Windows.Forms.Label();
            this.DocToLabel = new System.Windows.Forms.Label();
            this.MailAddressLabel = new System.Windows.Forms.Label();
            this.MailAddressTextBox = new System.Windows.Forms.TextBox();
            this.ButtonGenerateReport = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RadioFacturen
            // 
            this.RadioFacturen.AutoSize = true;
            this.RadioFacturen.Checked = true;
            this.RadioFacturen.Location = new System.Drawing.Point(16, 16);
            this.RadioFacturen.Name = "RadioFacturen";
            this.RadioFacturen.Size = new System.Drawing.Size(67, 17);
            this.RadioFacturen.TabIndex = 0;
            this.RadioFacturen.TabStop = true;
            this.RadioFacturen.Text = "Facturen";
            this.RadioFacturen.UseVisualStyleBackColor = true;
            this.RadioFacturen.CheckedChanged += new System.EventHandler(this.RadioFacturen_CheckedChanged);
            // 
            // RadioCreditnotas
            // 
            this.RadioCreditnotas.AutoSize = true;
            this.RadioCreditnotas.Location = new System.Drawing.Point(100, 16);
            this.RadioCreditnotas.Name = "RadioCreditnotas";
            this.RadioCreditnotas.Size = new System.Drawing.Size(80, 17);
            this.RadioCreditnotas.TabIndex = 1;
            this.RadioCreditnotas.Text = "Creditnota\'s";
            this.RadioCreditnotas.UseVisualStyleBackColor = true;
            this.RadioCreditnotas.CheckedChanged += new System.EventHandler(this.RadioCreditnotas_CheckedChanged);
            // 
            // DetailJournalCheckBox
            // 
            this.DetailJournalCheckBox.AutoSize = true;
            this.DetailJournalCheckBox.Checked = true;
            this.DetailJournalCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DetailJournalCheckBox.Location = new System.Drawing.Point(16, 44);
            this.DetailJournalCheckBox.Name = "DetailJournalCheckBox";
            this.DetailJournalCheckBox.Size = new System.Drawing.Size(96, 17);
            this.DetailJournalCheckBox.TabIndex = 2;
            this.DetailJournalCheckBox.Text = "Detail Journaal";
            this.DetailJournalCheckBox.UseVisualStyleBackColor = true;
            // 
            // SubTitleLabel
            // 
            this.SubTitleLabel.AutoSize = true;
            this.SubTitleLabel.Location = new System.Drawing.Point(16, 74);
            this.SubTitleLabel.Name = "SubTitleLabel";
            this.SubTitleLabel.Size = new System.Drawing.Size(46, 13);
            this.SubTitleLabel.TabIndex = 3;
            this.SubTitleLabel.Text = "SubTitel";
            // 
            // SubTitleTextBox
            // 
            this.SubTitleTextBox.Location = new System.Drawing.Point(80, 71);
            this.SubTitleTextBox.Name = "SubTitleTextBox";
            this.SubTitleTextBox.Size = new System.Drawing.Size(300, 20);
            this.SubTitleTextBox.TabIndex = 4;
            // 
            // ProcessingDateLabel
            // 
            this.ProcessingDateLabel.AutoSize = true;
            this.ProcessingDateLabel.Location = new System.Drawing.Point(16, 100);
            this.ProcessingDateLabel.Name = "ProcessingDateLabel";
            this.ProcessingDateLabel.Size = new System.Drawing.Size(54, 13);
            this.ProcessingDateLabel.TabIndex = 5;
            this.ProcessingDateLabel.Text = "Lijstdatum";
            // 
            // ProcessingDate
            // 
            this.ProcessingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ProcessingDate.Location = new System.Drawing.Point(100, 97);
            this.ProcessingDate.Name = "ProcessingDate";
            this.ProcessingDate.Size = new System.Drawing.Size(130, 20);
            this.ProcessingDate.TabIndex = 6;
            // 
            // PeriodFromLabel
            // 
            this.PeriodFromLabel.AutoSize = true;
            this.PeriodFromLabel.Location = new System.Drawing.Point(16, 127);
            this.PeriodFromLabel.Name = "PeriodFromLabel";
            this.PeriodFromLabel.Size = new System.Drawing.Size(65, 13);
            this.PeriodFromLabel.TabIndex = 7;
            this.PeriodFromLabel.Text = "Periode Van";
            // 
            // DateFromLabel
            // 
            this.DateFromLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DateFromLabel.Location = new System.Drawing.Point(100, 124);
            this.DateFromLabel.Name = "DateFromLabel";
            this.DateFromLabel.Size = new System.Drawing.Size(130, 20);
            this.DateFromLabel.TabIndex = 8;
            this.DateFromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PeriodToLabel
            // 
            this.PeriodToLabel.AutoSize = true;
            this.PeriodToLabel.Location = new System.Drawing.Point(16, 153);
            this.PeriodToLabel.Name = "PeriodToLabel";
            this.PeriodToLabel.Size = new System.Drawing.Size(62, 13);
            this.PeriodToLabel.TabIndex = 9;
            this.PeriodToLabel.Text = "Periode Tot";
            // 
            // DateToLabel
            // 
            this.DateToLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DateToLabel.Location = new System.Drawing.Point(100, 150);
            this.DateToLabel.Name = "DateToLabel";
            this.DateToLabel.Size = new System.Drawing.Size(130, 20);
            this.DateToLabel.TabIndex = 10;
            this.DateToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DocRangeLabel
            // 
            this.DocRangeLabel.AutoSize = true;
            this.DocRangeLabel.Location = new System.Drawing.Point(250, 127);
            this.DocRangeLabel.Name = "DocRangeLabel";
            this.DocRangeLabel.Size = new System.Drawing.Size(79, 13);
            this.DocRangeLabel.TabIndex = 11;
            this.DocRangeLabel.Text = "Dok. Van / Tot";
            // 
            // DocFromLabel
            // 
            this.DocFromLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DocFromLabel.Location = new System.Drawing.Point(340, 124);
            this.DocFromLabel.Name = "DocFromLabel";
            this.DocFromLabel.Size = new System.Drawing.Size(80, 20);
            this.DocFromLabel.TabIndex = 12;
            this.DocFromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DocToLabel
            // 
            this.DocToLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DocToLabel.Location = new System.Drawing.Point(340, 150);
            this.DocToLabel.Name = "DocToLabel";
            this.DocToLabel.Size = new System.Drawing.Size(80, 20);
            this.DocToLabel.TabIndex = 13;
            this.DocToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MailAddressLabel
            // 
            this.MailAddressLabel.AutoSize = true;
            this.MailAddressLabel.Location = new System.Drawing.Point(16, 182);
            this.MailAddressLabel.Name = "MailAddressLabel";
            this.MailAddressLabel.Size = new System.Drawing.Size(39, 13);
            this.MailAddressLabel.TabIndex = 14;
            this.MailAddressLabel.Text = "MailTo";
            // 
            // MailAddressTextBox
            // 
            this.MailAddressTextBox.Location = new System.Drawing.Point(60, 179);
            this.MailAddressTextBox.Name = "MailAddressTextBox";
            this.MailAddressTextBox.Size = new System.Drawing.Size(250, 20);
            this.MailAddressTextBox.TabIndex = 15;
            // 
            // ButtonGenerateReport
            // 
            this.ButtonGenerateReport.Location = new System.Drawing.Point(340, 206);
            this.ButtonGenerateReport.Name = "ButtonGenerateReport";
            this.ButtonGenerateReport.Size = new System.Drawing.Size(100, 26);
            this.ButtonGenerateReport.TabIndex = 16;
            this.ButtonGenerateReport.Text = "Genereren";
            this.ButtonGenerateReport.UseVisualStyleBackColor = true;
            this.ButtonGenerateReport.Click += new System.EventHandler(this.ButtonGenerateReport_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(450, 206);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(80, 26);
            this.ButtonClose.TabIndex = 17;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormPurchaseAndSalesLedger
            // 
            this.AcceptButton = this.ButtonGenerateReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(546, 248);
            this.Controls.Add(this.RadioFacturen);
            this.Controls.Add(this.RadioCreditnotas);
            this.Controls.Add(this.DetailJournalCheckBox);
            this.Controls.Add(this.SubTitleLabel);
            this.Controls.Add(this.SubTitleTextBox);
            this.Controls.Add(this.ProcessingDateLabel);
            this.Controls.Add(this.ProcessingDate);
            this.Controls.Add(this.PeriodFromLabel);
            this.Controls.Add(this.DateFromLabel);
            this.Controls.Add(this.PeriodToLabel);
            this.Controls.Add(this.DateToLabel);
            this.Controls.Add(this.DocRangeLabel);
            this.Controls.Add(this.DocFromLabel);
            this.Controls.Add(this.DocToLabel);
            this.Controls.Add(this.MailAddressLabel);
            this.Controls.Add(this.MailAddressTextBox);
            this.Controls.Add(this.ButtonGenerateReport);
            this.Controls.Add(this.ButtonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPurchaseAndSalesLedger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aankoop / Verkoop";
            this.Load += new System.EventHandler(this.FormPurchaseAndSalesLedger_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton  RadioFacturen;
        private System.Windows.Forms.RadioButton  RadioCreditnotas;
        private System.Windows.Forms.CheckBox     DetailJournalCheckBox;
        private System.Windows.Forms.Label        SubTitleLabel;
        private System.Windows.Forms.TextBox      SubTitleTextBox;
        private System.Windows.Forms.Label        ProcessingDateLabel;
        private System.Windows.Forms.DateTimePicker ProcessingDate;
        private System.Windows.Forms.Label        PeriodFromLabel;
        private System.Windows.Forms.Label        DateFromLabel;
        private System.Windows.Forms.Label        PeriodToLabel;
        private System.Windows.Forms.Label        DateToLabel;
        private System.Windows.Forms.Label        DocRangeLabel;
        private System.Windows.Forms.Label        DocFromLabel;
        private System.Windows.Forms.Label        DocToLabel;
        private System.Windows.Forms.Label        MailAddressLabel;
        private System.Windows.Forms.TextBox      MailAddressTextBox;
        private System.Windows.Forms.Button       ButtonGenerateReport;
        private System.Windows.Forms.Button       ButtonClose;
    }
}
