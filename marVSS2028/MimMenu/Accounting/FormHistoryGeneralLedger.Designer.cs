namespace marVSS2028.MimMenu.Accounting
{
    partial class FormHistoryGeneralLedger
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
            this.ButtonGenerateReport = new System.Windows.Forms.Button();
            this.AccountFromLabel = new System.Windows.Forms.Label();
            this.AccountToLabel = new System.Windows.Forms.Label();
            this.SelectedPeriodLabel = new System.Windows.Forms.Label();
            this.ProcessingDateLabel = new System.Windows.Forms.Label();
            this.DefaultMailAddressLabel = new System.Windows.Forms.Label();
            this.AccountFromTextBox = new System.Windows.Forms.TextBox();
            this.AccountToTextBox = new System.Windows.Forms.TextBox();
            this.MailAddressTextBox = new System.Windows.Forms.TextBox();
            this.ProcessingDate = new System.Windows.Forms.DateTimePicker();
            this.SelectedPeriodTextBox = new System.Windows.Forms.MaskedTextBox();
            this.PeriodiekeTotalenCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(409, 113);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(100, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonGenerateReport
            // 
            this.ButtonGenerateReport.Location = new System.Drawing.Point(409, 22);
            this.ButtonGenerateReport.Name = "ButtonGenerateReport";
            this.ButtonGenerateReport.Size = new System.Drawing.Size(100, 36);
            this.ButtonGenerateReport.TabIndex = 1;
            this.ButtonGenerateReport.Text = "Rapport Genereren";
            this.ButtonGenerateReport.UseVisualStyleBackColor = true;
            this.ButtonGenerateReport.Click += new System.EventHandler(this.ButtonGenerateReport_Click);
            // 
            // AccountFromLabel
            // 
            this.AccountFromLabel.AutoSize = true;
            this.AccountFromLabel.Location = new System.Drawing.Point(16, 22);
            this.AccountFromLabel.Name = "AccountFromLabel";
            this.AccountFromLabel.Size = new System.Drawing.Size(84, 13);
            this.AccountFromLabel.TabIndex = 2;
            this.AccountFromLabel.Text = "Vanaf Rekening";
            // 
            // AccountToLabel
            // 
            this.AccountToLabel.AutoSize = true;
            this.AccountToLabel.Location = new System.Drawing.Point(180, 22);
            this.AccountToLabel.Name = "AccountToLabel";
            this.AccountToLabel.Size = new System.Drawing.Size(23, 13);
            this.AccountToLabel.TabIndex = 3;
            this.AccountToLabel.Text = "Tot";
            // 
            // SelectedPeriodLabel
            // 
            this.SelectedPeriodLabel.AutoSize = true;
            this.SelectedPeriodLabel.Location = new System.Drawing.Point(16, 62);
            this.SelectedPeriodLabel.Name = "SelectedPeriodLabel";
            this.SelectedPeriodLabel.Size = new System.Drawing.Size(90, 13);
            this.SelectedPeriodLabel.TabIndex = 4;
            this.SelectedPeriodLabel.Text = "Periode Van - Tot";
            // 
            // ProcessingDateLabel
            // 
            this.ProcessingDateLabel.AutoSize = true;
            this.ProcessingDateLabel.Location = new System.Drawing.Point(286, 62);
            this.ProcessingDateLabel.Name = "ProcessingDateLabel";
            this.ProcessingDateLabel.Size = new System.Drawing.Size(54, 13);
            this.ProcessingDateLabel.TabIndex = 5;
            this.ProcessingDateLabel.Text = "Lijstdatum";
            // 
            // DefaultMailAddressLabel
            // 
            this.DefaultMailAddressLabel.AutoSize = true;
            this.DefaultMailAddressLabel.Location = new System.Drawing.Point(16, 99);
            this.DefaultMailAddressLabel.Name = "DefaultMailAddressLabel";
            this.DefaultMailAddressLabel.Size = new System.Drawing.Size(39, 13);
            this.DefaultMailAddressLabel.TabIndex = 6;
            this.DefaultMailAddressLabel.Text = "MailTo";
            // 
            // AccountFromTextBox
            // 
            this.AccountFromTextBox.Location = new System.Drawing.Point(19, 38);
            this.AccountFromTextBox.MaxLength = 7;
            this.AccountFromTextBox.Name = "AccountFromTextBox";
            this.AccountFromTextBox.Size = new System.Drawing.Size(155, 20);
            this.AccountFromTextBox.TabIndex = 7;
            this.AccountFromTextBox.Leave += new System.EventHandler(this.AccountFromTextBox_Leave);
            // 
            // AccountToTextBox
            // 
            this.AccountToTextBox.Location = new System.Drawing.Point(184, 38);
            this.AccountToTextBox.MaxLength = 7;
            this.AccountToTextBox.Name = "AccountToTextBox";
            this.AccountToTextBox.Size = new System.Drawing.Size(95, 20);
            this.AccountToTextBox.TabIndex = 8;
            // 
            // MailAddressTextBox
            // 
            this.MailAddressTextBox.Location = new System.Drawing.Point(19, 113);
            this.MailAddressTextBox.Name = "MailAddressTextBox";
            this.MailAddressTextBox.Size = new System.Drawing.Size(234, 20);
            this.MailAddressTextBox.TabIndex = 9;
            // 
            // ProcessingDate
            // 
            this.ProcessingDate.Location = new System.Drawing.Point(289, 78);
            this.ProcessingDate.Name = "ProcessingDate";
            this.ProcessingDate.Size = new System.Drawing.Size(220, 20);
            this.ProcessingDate.TabIndex = 10;
            // 
            // SelectedPeriodTextBox
            // 
            this.SelectedPeriodTextBox.Location = new System.Drawing.Point(19, 78);
            this.SelectedPeriodTextBox.Mask = "00/00/0000 - 00/00/0000";
            this.SelectedPeriodTextBox.Name = "SelectedPeriodTextBox";
            this.SelectedPeriodTextBox.Size = new System.Drawing.Size(260, 20);
            this.SelectedPeriodTextBox.TabIndex = 11;
            this.SelectedPeriodTextBox.Leave += new System.EventHandler(this.SelectedPeriodTextBox_Leave);
            // 
            // PeriodiekeTotalenCheckBox
            // 
            this.PeriodiekeTotalenCheckBox.AutoSize = true;
            this.PeriodiekeTotalenCheckBox.Checked = true;
            this.PeriodiekeTotalenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PeriodiekeTotalenCheckBox.Location = new System.Drawing.Point(289, 115);
            this.PeriodiekeTotalenCheckBox.Name = "PeriodiekeTotalenCheckBox";
            this.PeriodiekeTotalenCheckBox.Size = new System.Drawing.Size(115, 17);
            this.PeriodiekeTotalenCheckBox.TabIndex = 12;
            this.PeriodiekeTotalenCheckBox.Text = "Periodieke Totalen";
            this.PeriodiekeTotalenCheckBox.UseVisualStyleBackColor = true;
            // 
            // FormHistoryGeneralLedger
            // 
            this.AcceptButton = this.ButtonGenerateReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(525, 145);
            this.Controls.Add(this.PeriodiekeTotalenCheckBox);
            this.Controls.Add(this.SelectedPeriodTextBox);
            this.Controls.Add(this.ProcessingDate);
            this.Controls.Add(this.MailAddressTextBox);
            this.Controls.Add(this.AccountToTextBox);
            this.Controls.Add(this.AccountFromTextBox);
            this.Controls.Add(this.DefaultMailAddressLabel);
            this.Controls.Add(this.ProcessingDateLabel);
            this.Controls.Add(this.SelectedPeriodLabel);
            this.Controls.Add(this.AccountToLabel);
            this.Controls.Add(this.AccountFromLabel);
            this.Controls.Add(this.ButtonGenerateReport);
            this.Controls.Add(this.ButtonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHistoryGeneralLedger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historiek Rekeningen (Grootboek)";
            this.Load += new System.EventHandler(this.FormHistoryGeneralLedger_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonGenerateReport;
        private System.Windows.Forms.Label AccountFromLabel;
        private System.Windows.Forms.Label AccountToLabel;
        private System.Windows.Forms.Label SelectedPeriodLabel;
        private System.Windows.Forms.Label ProcessingDateLabel;
        private System.Windows.Forms.Label DefaultMailAddressLabel;
        private System.Windows.Forms.TextBox AccountFromTextBox;
        private System.Windows.Forms.TextBox AccountToTextBox;
        private System.Windows.Forms.TextBox MailAddressTextBox;
        private System.Windows.Forms.DateTimePicker ProcessingDate;
        private System.Windows.Forms.MaskedTextBox SelectedPeriodTextBox;
        private System.Windows.Forms.CheckBox PeriodiekeTotalenCheckBox;
    }
}