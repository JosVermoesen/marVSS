namespace marVSS2028.MimMenu.Accounting
{
    partial class FormFinancialBook
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
            this.LabelPeriod = new System.Windows.Forms.Label();
            this.PeriodTextBox = new System.Windows.Forms.TextBox();
            this.LabelPrintDate = new System.Windows.Forms.Label();
            this.ProcessingDateTextBox = new System.Windows.Forms.TextBox();
            this.LabelPeriodExtracts = new System.Windows.Forms.Label();
            this.AccountComboBox = new System.Windows.Forms.ComboBox();
            this.ExtractsListBox = new System.Windows.Forms.ListBox();
            this.ButtonGenerateReport = new System.Windows.Forms.Button();
            this.ButtonManualJournal = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonCtrl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelPeriod
            // 
            this.LabelPeriod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelPeriod.Location = new System.Drawing.Point(10, 5);
            this.LabelPeriod.Name = "LabelPeriod";
            this.LabelPeriod.Size = new System.Drawing.Size(185, 23);
            this.LabelPeriod.TabIndex = 0;
            this.LabelPeriod.Text = "Afdrukperiode Van - &Tot";
            // 
            // PeriodTextBox
            // 
            this.PeriodTextBox.BackColor = System.Drawing.Color.White;
            this.PeriodTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PeriodTextBox.Location = new System.Drawing.Point(8, 30);
            this.PeriodTextBox.Name = "PeriodTextBox";
            this.PeriodTextBox.Size = new System.Drawing.Size(300, 20);
            this.PeriodTextBox.TabIndex = 1;
            this.PeriodTextBox.Enter += new System.EventHandler(this.PeriodTextBox_Enter);
            this.PeriodTextBox.Leave += new System.EventHandler(this.PeriodTextBox_Leave);
            // 
            // LabelPrintDate
            // 
            this.LabelPrintDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelPrintDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelPrintDate.Location = new System.Drawing.Point(312, 5);
            this.LabelPrintDate.Name = "LabelPrintDate";
            this.LabelPrintDate.Size = new System.Drawing.Size(118, 23);
            this.LabelPrintDate.TabIndex = 2;
            this.LabelPrintDate.Text = "Datu&m Drukken";
            // 
            // ProcessingDateTextBox
            // 
            this.ProcessingDateTextBox.BackColor = System.Drawing.Color.White;
            this.ProcessingDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessingDateTextBox.Location = new System.Drawing.Point(312, 30);
            this.ProcessingDateTextBox.Name = "ProcessingDateTextBox";
            this.ProcessingDateTextBox.Size = new System.Drawing.Size(132, 20);
            this.ProcessingDateTextBox.TabIndex = 3;
            this.ProcessingDateTextBox.Enter += new System.EventHandler(this.ProcessingDateTextBox_Enter);
            this.ProcessingDateTextBox.Leave += new System.EventHandler(this.ProcessingDateTextBox_Leave);
            // 
            // LabelPeriodExtracts
            // 
            this.LabelPeriodExtracts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelPeriodExtracts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelPeriodExtracts.Location = new System.Drawing.Point(5, 70);
            this.LabelPeriodExtracts.Name = "LabelPeriodExtracts";
            this.LabelPeriodExtracts.Size = new System.Drawing.Size(90, 37);
            this.LabelPeriodExtracts.TabIndex = 5;
            this.LabelPeriodExtracts.Text = "&Uittreksels periode";
            // 
            // AccountComboBox
            // 
            this.AccountComboBox.BackColor = System.Drawing.Color.White;
            this.AccountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccountComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountComboBox.Location = new System.Drawing.Point(108, 80);
            this.AccountComboBox.Name = "AccountComboBox";
            this.AccountComboBox.Size = new System.Drawing.Size(328, 21);
            this.AccountComboBox.TabIndex = 6;
            this.AccountComboBox.SelectedIndexChanged += new System.EventHandler(this.AccountComboBox_SelectedIndexChanged);
            // 
            // ExtractsListBox
            // 
            this.ExtractsListBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExtractsListBox.ItemHeight = 14;
            this.ExtractsListBox.Location = new System.Drawing.Point(8, 112);
            this.ExtractsListBox.Name = "ExtractsListBox";
            this.ExtractsListBox.Size = new System.Drawing.Size(586, 242);
            this.ExtractsListBox.Sorted = true;
            this.ExtractsListBox.TabIndex = 8;
            this.ExtractsListBox.DoubleClick += new System.EventHandler(this.ExtractsListBox_DoubleClick);
            this.ExtractsListBox.GotFocus += new System.EventHandler(this.ExtractsListBox_GotFocus);
            this.ExtractsListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExtractsListBox_KeyDown);
            // 
            // ButtonGenerateReport
            // 
            this.ButtonGenerateReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonGenerateReport.Location = new System.Drawing.Point(466, 5);
            this.ButtonGenerateReport.Name = "ButtonGenerateReport";
            this.ButtonGenerateReport.Size = new System.Drawing.Size(120, 46);
            this.ButtonGenerateReport.TabIndex = 4;
            this.ButtonGenerateReport.Text = "Rapport Genereren";
            this.ButtonGenerateReport.Click += new System.EventHandler(this.ButtonGenerateReport_Click);
            // 
            // ButtonManualJournal
            // 
            this.ButtonManualJournal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonManualJournal.Location = new System.Drawing.Point(466, 62);
            this.ButtonManualJournal.Name = "ButtonManualJournal";
            this.ButtonManualJournal.Size = new System.Drawing.Size(120, 45);
            this.ButtonManualJournal.TabIndex = 7;
            this.ButtonManualJournal.Text = "&Journalen Manueel Zoeken";
            this.ButtonManualJournal.Click += new System.EventHandler(this.ButtonManualJournal_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonClose.Location = new System.Drawing.Point(466, 369);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(120, 32);
            this.ButtonClose.TabIndex = 9;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonCtrl
            // 
            this.ButtonCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCtrl.Location = new System.Drawing.Point(382, 372);
            this.ButtonCtrl.Name = "ButtonCtrl";
            this.ButtonCtrl.Size = new System.Drawing.Size(78, 26);
            this.ButtonCtrl.TabIndex = 10;
            this.ButtonCtrl.TabStop = false;
            this.ButtonCtrl.Text = "cKTRL";
            this.ButtonCtrl.Visible = false;
            this.ButtonCtrl.Click += new System.EventHandler(this.ButtonCtrl_Click);
            // 
            // FormFinancialBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(606, 408);
            this.ControlBox = false;
            this.Controls.Add(this.LabelPeriod);
            this.Controls.Add(this.PeriodTextBox);
            this.Controls.Add(this.LabelPrintDate);
            this.Controls.Add(this.ProcessingDateTextBox);
            this.Controls.Add(this.ButtonGenerateReport);
            this.Controls.Add(this.LabelPeriodExtracts);
            this.Controls.Add(this.AccountComboBox);
            this.Controls.Add(this.ButtonManualJournal);
            this.Controls.Add(this.ExtractsListBox);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonCtrl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFinancialBook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Financiële Boeken";
            this.Load += new System.EventHandler(this.FormFinancialBook_Load);
            this.Shown += new System.EventHandler(this.FormFinancialBook_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelPeriod;
        private System.Windows.Forms.TextBox PeriodTextBox;
        private System.Windows.Forms.Label LabelPrintDate;
        private System.Windows.Forms.TextBox ProcessingDateTextBox;
        private System.Windows.Forms.Label LabelPeriodExtracts;
        private System.Windows.Forms.ComboBox AccountComboBox;
        private System.Windows.Forms.ListBox ExtractsListBox;
        private System.Windows.Forms.Button ButtonGenerateReport;
        private System.Windows.Forms.Button ButtonManualJournal;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonCtrl;
    }
}
