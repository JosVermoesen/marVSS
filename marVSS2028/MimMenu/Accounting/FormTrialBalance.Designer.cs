namespace marVSS2028.MimMenu.Accounting
{
    partial class FormTrialBalance
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
            this.SelectedPeriodLabel = new System.Windows.Forms.Label();
            this.ProcessingDateLabel = new System.Windows.Forms.Label();
            this.DefaultMailAddressLabel = new System.Windows.Forms.Label();
            this.AccountFromLabel = new System.Windows.Forms.Label();
            this.AccountToLabel = new System.Windows.Forms.Label();
            this.MailAddressTextBox = new System.Windows.Forms.TextBox();
            this.AccountFromTextBox = new System.Windows.Forms.TextBox();
            this.AccountToTextBox = new System.Windows.Forms.TextBox();
            this.ProcessingDate = new System.Windows.Forms.DateTimePicker();
            this.ButtonGenerateReport = new System.Windows.Forms.Button();
            this.SelectedPeriodTextBox = new System.Windows.Forms.MaskedTextBox();
            this.DetailJournalCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(379, 110);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(100, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // SelectedPeriodLabel
            // 
            this.SelectedPeriodLabel.AutoSize = true;
            this.SelectedPeriodLabel.Location = new System.Drawing.Point(16, 22);
            this.SelectedPeriodLabel.Name = "SelectedPeriodLabel";
            this.SelectedPeriodLabel.Size = new System.Drawing.Size(84, 13);
            this.SelectedPeriodLabel.TabIndex = 1;
            this.SelectedPeriodLabel.Text = "Periode Van Tot";
            // 
            // ProcessingDateLabel
            // 
            this.ProcessingDateLabel.AutoSize = true;
            this.ProcessingDateLabel.Location = new System.Drawing.Point(200, 22);
            this.ProcessingDateLabel.Name = "ProcessingDateLabel";
            this.ProcessingDateLabel.Size = new System.Drawing.Size(83, 13);
            this.ProcessingDateLabel.TabIndex = 2;
            this.ProcessingDateLabel.Text = "Datum vandaag";
            // 
            // DefaultMailAddressLabel
            // 
            this.DefaultMailAddressLabel.AutoSize = true;
            this.DefaultMailAddressLabel.Location = new System.Drawing.Point(16, 96);
            this.DefaultMailAddressLabel.Name = "DefaultMailAddressLabel";
            this.DefaultMailAddressLabel.Size = new System.Drawing.Size(39, 13);
            this.DefaultMailAddressLabel.TabIndex = 3;
            this.DefaultMailAddressLabel.Text = "MailTo";
            // 
            // AccountFromLabel
            // 
            this.AccountFromLabel.AutoSize = true;
            this.AccountFromLabel.Location = new System.Drawing.Point(16, 60);
            this.AccountFromLabel.Name = "AccountFromLabel";
            this.AccountFromLabel.Size = new System.Drawing.Size(57, 13);
            this.AccountFromLabel.TabIndex = 4;
            this.AccountFromLabel.Text = "Rekening Van";
            // 
            // AccountToLabel
            // 
            this.AccountToLabel.AutoSize = true;
            this.AccountToLabel.Location = new System.Drawing.Point(120, 60);
            this.AccountToLabel.Name = "AccountToLabel";
            this.AccountToLabel.Size = new System.Drawing.Size(57, 13);
            this.AccountToLabel.TabIndex = 5;
            this.AccountToLabel.Text = "Rekening Tot";
            // 
            // MailAddressTextBox
            // 
            this.MailAddressTextBox.Location = new System.Drawing.Point(19, 110);
            this.MailAddressTextBox.Name = "MailAddressTextBox";
            this.MailAddressTextBox.Size = new System.Drawing.Size(234, 20);
            this.MailAddressTextBox.TabIndex = 6;
            // 
            // AccountFromTextBox
            // 
            this.AccountFromTextBox.Location = new System.Drawing.Point(19, 75);
            this.AccountFromTextBox.Name = "AccountFromTextBox";
            this.AccountFromTextBox.Size = new System.Drawing.Size(90, 20);
            this.AccountFromTextBox.TabIndex = 7;
            // 
            // AccountToTextBox
            // 
            this.AccountToTextBox.Location = new System.Drawing.Point(120, 75);
            this.AccountToTextBox.Name = "AccountToTextBox";
            this.AccountToTextBox.Size = new System.Drawing.Size(90, 20);
            this.AccountToTextBox.TabIndex = 8;
            // 
            // ProcessingDate
            // 
            this.ProcessingDate.Location = new System.Drawing.Point(203, 38);
            this.ProcessingDate.Name = "ProcessingDate";
            this.ProcessingDate.Size = new System.Drawing.Size(200, 20);
            this.ProcessingDate.TabIndex = 9;
            // 
            // ButtonGenerateReport
            // 
            this.ButtonGenerateReport.Location = new System.Drawing.Point(259, 107);
            this.ButtonGenerateReport.Name = "ButtonGenerateReport";
            this.ButtonGenerateReport.Size = new System.Drawing.Size(110, 23);
            this.ButtonGenerateReport.TabIndex = 10;
            this.ButtonGenerateReport.Text = "Genereer Rapport";
            this.ButtonGenerateReport.UseVisualStyleBackColor = true;
            this.ButtonGenerateReport.Click += new System.EventHandler(this.ButtonGenerateReport_Click);
            // 
            // SelectedPeriodTextBox
            // 
            this.SelectedPeriodTextBox.Location = new System.Drawing.Point(19, 38);
            this.SelectedPeriodTextBox.Mask = "00/00/0000 - 00/00/0000";
            this.SelectedPeriodTextBox.Name = "SelectedPeriodTextBox";
            this.SelectedPeriodTextBox.Size = new System.Drawing.Size(148, 20);
            this.SelectedPeriodTextBox.TabIndex = 11;
            this.SelectedPeriodTextBox.Leave += new System.EventHandler(this.SelectedPeriodTextBox_Leave);
            // 
            // DetailJournalCheckBox
            // 
            this.DetailJournalCheckBox.AutoSize = true;
            this.DetailJournalCheckBox.Location = new System.Drawing.Point(260, 77);
            this.DetailJournalCheckBox.Name = "DetailJournalCheckBox";
            this.DetailJournalCheckBox.Size = new System.Drawing.Size(96, 17);
            this.DetailJournalCheckBox.TabIndex = 12;
            this.DetailJournalCheckBox.Text = "Algemeen Journaal";
            this.DetailJournalCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormTrialBalance
            // 
            this.AcceptButton = this.ButtonGenerateReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(498, 153);
            this.Controls.Add(this.DetailJournalCheckBox);
            this.Controls.Add(this.SelectedPeriodTextBox);
            this.Controls.Add(this.ButtonGenerateReport);
            this.Controls.Add(this.ProcessingDate);
            this.Controls.Add(this.AccountToTextBox);
            this.Controls.Add(this.AccountFromTextBox);
            this.Controls.Add(this.MailAddressTextBox);
            this.Controls.Add(this.AccountToLabel);
            this.Controls.Add(this.AccountFromLabel);
            this.Controls.Add(this.DefaultMailAddressLabel);
            this.Controls.Add(this.ProcessingDateLabel);
            this.Controls.Add(this.SelectedPeriodLabel);
            this.Controls.Add(this.ButtonClose);
            this.Name = "FormTrialBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Proef- en Saldibalans";
            this.Load += new System.EventHandler(this.FormTrialBalance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label SelectedPeriodLabel;
        private System.Windows.Forms.Label ProcessingDateLabel;
        private System.Windows.Forms.Label DefaultMailAddressLabel;
        private System.Windows.Forms.Label AccountFromLabel;
        private System.Windows.Forms.Label AccountToLabel;
        private System.Windows.Forms.TextBox MailAddressTextBox;
        private System.Windows.Forms.TextBox AccountFromTextBox;
        private System.Windows.Forms.TextBox AccountToTextBox;
        private System.Windows.Forms.DateTimePicker ProcessingDate;
        private System.Windows.Forms.Button ButtonGenerateReport;
        private System.Windows.Forms.MaskedTextBox SelectedPeriodTextBox;
        private System.Windows.Forms.CheckBox DetailJournalCheckBox;
    }
}