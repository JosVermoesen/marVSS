namespace marVSS2028.MimMenu.Accounting
{
    partial class FormLedgerBook
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
            this.ButtonClose = new System.Windows.Forms.Button();
            this.SelectedPeriodeLabel = new System.Windows.Forms.Label();
            this.LineCountLabel = new System.Windows.Forms.Label();
            this.ProcessingDateLabel = new System.Windows.Forms.Label();
            this.DefaultMailAddressLabel = new System.Windows.Forms.Label();
            this.MailAddressTextBox = new System.Windows.Forms.TextBox();
            this.ProcessingDate = new System.Windows.Forms.DateTimePicker();
            this.ButtonGenerateReport = new System.Windows.Forms.Button();
            this.LineCountTextBox = new System.Windows.Forms.TextBox();
            this.SelectedPeriodTextBox = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(427, 91);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(91, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // SelectedPeriodeLabel
            // 
            this.SelectedPeriodeLabel.AutoSize = true;
            this.SelectedPeriodeLabel.Location = new System.Drawing.Point(16, 22);
            this.SelectedPeriodeLabel.Name = "SelectedPeriodeLabel";
            this.SelectedPeriodeLabel.Size = new System.Drawing.Size(84, 13);
            this.SelectedPeriodeLabel.TabIndex = 1;
            this.SelectedPeriodeLabel.Text = "Periode Van Tot";
            // 
            // LineCountLabel
            // 
            this.LineCountLabel.AutoSize = true;
            this.LineCountLabel.Location = new System.Drawing.Point(384, 12);
            this.LineCountLabel.Name = "LineCountLabel";
            this.LineCountLabel.Size = new System.Drawing.Size(68, 13);
            this.LineCountLabel.TabIndex = 2;
            this.LineCountLabel.Text = "Aantal Lijnen";
            // 
            // ProcessingDateLabel
            // 
            this.ProcessingDateLabel.AutoSize = true;
            this.ProcessingDateLabel.Location = new System.Drawing.Point(170, 22);
            this.ProcessingDateLabel.Name = "ProcessingDateLabel";
            this.ProcessingDateLabel.Size = new System.Drawing.Size(83, 13);
            this.ProcessingDateLabel.TabIndex = 3;
            this.ProcessingDateLabel.Text = "Datum vandaag";
            // 
            // DefaultMailAddressLabel
            // 
            this.DefaultMailAddressLabel.AutoSize = true;
            this.DefaultMailAddressLabel.Location = new System.Drawing.Point(16, 94);
            this.DefaultMailAddressLabel.Name = "DefaultMailAddressLabel";
            this.DefaultMailAddressLabel.Size = new System.Drawing.Size(67, 13);
            this.DefaultMailAddressLabel.TabIndex = 4;
            this.DefaultMailAddressLabel.Text = "Mailen Naar:";
            // 
            // MailAddressTextBox
            // 
            this.MailAddressTextBox.Location = new System.Drawing.Point(89, 91);
            this.MailAddressTextBox.Name = "MailAddressTextBox";
            this.MailAddressTextBox.Size = new System.Drawing.Size(271, 20);
            this.MailAddressTextBox.TabIndex = 6;
            // 
            // ProcessingDate
            // 
            this.ProcessingDate.Location = new System.Drawing.Point(173, 38);
            this.ProcessingDate.Name = "ProcessingDate";
            this.ProcessingDate.Size = new System.Drawing.Size(187, 20);
            this.ProcessingDate.TabIndex = 7;
            // 
            // ButtonGenerateReport
            // 
            this.ButtonGenerateReport.Location = new System.Drawing.Point(427, 38);
            this.ButtonGenerateReport.Name = "ButtonGenerateReport";
            this.ButtonGenerateReport.Size = new System.Drawing.Size(91, 47);
            this.ButtonGenerateReport.TabIndex = 8;
            this.ButtonGenerateReport.Text = "Rapport Genereren";
            this.ButtonGenerateReport.UseVisualStyleBackColor = true;
            this.ButtonGenerateReport.Click += new System.EventHandler(this.ButtonGenerateReport_Click);
            // 
            // LineCountTextBox
            // 
            this.LineCountTextBox.Location = new System.Drawing.Point(458, 12);
            this.LineCountTextBox.Name = "LineCountTextBox";
            this.LineCountTextBox.Size = new System.Drawing.Size(60, 20);
            this.LineCountTextBox.TabIndex = 9;
            // 
            // SelectedPeriodTextBox
            // 
            this.SelectedPeriodTextBox.Location = new System.Drawing.Point(19, 38);
            this.SelectedPeriodTextBox.Mask = "00/00/0000 - 00/00/0000";
            this.SelectedPeriodTextBox.Name = "SelectedPeriodTextBox";
            this.SelectedPeriodTextBox.Size = new System.Drawing.Size(148, 20);
            this.SelectedPeriodTextBox.TabIndex = 10;
            this.SelectedPeriodTextBox.Leave += new System.EventHandler(this.SelectedPeriodTextBox_Leave);
            // 
            // FormLedgerBook
            // 
            this.AcceptButton = this.ButtonGenerateReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(530, 123);
            this.Controls.Add(this.SelectedPeriodTextBox);
            this.Controls.Add(this.LineCountTextBox);
            this.Controls.Add(this.ButtonGenerateReport);
            this.Controls.Add(this.ProcessingDate);
            this.Controls.Add(this.MailAddressTextBox);
            this.Controls.Add(this.DefaultMailAddressLabel);
            this.Controls.Add(this.ProcessingDateLabel);
            this.Controls.Add(this.LineCountLabel);
            this.Controls.Add(this.SelectedPeriodeLabel);
            this.Controls.Add(this.ButtonClose);
            this.Name = "FormLedgerBook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormLedgerBook";
            this.Load += new System.EventHandler(this.FormLedgerBook_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label SelectedPeriodeLabel;
        private System.Windows.Forms.Label LineCountLabel;
        private System.Windows.Forms.Label ProcessingDateLabel;
        private System.Windows.Forms.Label DefaultMailAddressLabel;
        private System.Windows.Forms.TextBox MailAddressTextBox;
        private System.Windows.Forms.DateTimePicker ProcessingDate;
        private System.Windows.Forms.Button ButtonGenerateReport;
        private System.Windows.Forms.TextBox LineCountTextBox;
        private System.Windows.Forms.MaskedTextBox SelectedPeriodTextBox;
    }
}