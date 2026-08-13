namespace marVSS2028.PrivateForms
{
    partial class FormFiscalYearSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SetupOption0 = new System.Windows.Forms.RadioButton();
            this.SetupOption1 = new System.Windows.Forms.RadioButton();
            this.SetupOption2 = new System.Windows.Forms.RadioButton();
            this.SetupOption3 = new System.Windows.Forms.RadioButton();
            this.SetupOption4 = new System.Windows.Forms.RadioButton();
            this.SetupOption5 = new System.Windows.Forms.RadioButton();
            this.SetupOption6 = new System.Windows.Forms.RadioButton();
            this.SetupOption7 = new System.Windows.Forms.RadioButton();
            this.SetupOption8 = new System.Windows.Forms.RadioButton();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SetupOption0
            // 
            this.SetupOption0.Location = new System.Drawing.Point(12, 12);
            this.SetupOption0.Name = "SetupOption0";
            this.SetupOption0.Size = new System.Drawing.Size(280, 20);
            this.SetupOption0.TabIndex = 0;
            this.SetupOption0.Text = "Boekingen en algemene instellingen";
            this.SetupOption0.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption1
            // 
            this.SetupOption1.Location = new System.Drawing.Point(12, 38);
            this.SetupOption1.Name = "SetupOption1";
            this.SetupOption1.Size = new System.Drawing.Size(280, 20);
            this.SetupOption1.TabIndex = 1;
            this.SetupOption1.Text = "Aankoopverrichtingen";
            this.SetupOption1.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption2
            // 
            this.SetupOption2.Location = new System.Drawing.Point(12, 64);
            this.SetupOption2.Name = "SetupOption2";
            this.SetupOption2.Size = new System.Drawing.Size(280, 20);
            this.SetupOption2.TabIndex = 2;
            this.SetupOption2.Text = "Verkoopverrichtingen";
            this.SetupOption2.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption3
            // 
            this.SetupOption3.Location = new System.Drawing.Point(12, 90);
            this.SetupOption3.Name = "SetupOption3";
            this.SetupOption3.Size = new System.Drawing.Size(280, 20);
            this.SetupOption3.TabIndex = 3;
            this.SetupOption3.Text = "BTW Default Rekeningen";
            this.SetupOption3.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption4
            // 
            this.SetupOption4.Location = new System.Drawing.Point(12, 116);
            this.SetupOption4.Name = "SetupOption4";
            this.SetupOption4.Size = new System.Drawing.Size(280, 20);
            this.SetupOption4.TabIndex = 4;
            this.SetupOption4.Text = "Default Collectieve Rekeningen";
            this.SetupOption4.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption5
            // 
            this.SetupOption5.Location = new System.Drawing.Point(12, 142);
            this.SetupOption5.Name = "SetupOption5";
            this.SetupOption5.Size = new System.Drawing.Size(280, 20);
            this.SetupOption5.TabIndex = 5;
            this.SetupOption5.Text = "Financieel en Rekeningen";
            this.SetupOption5.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption6
            // 
            this.SetupOption6.Checked = true;
            this.SetupOption6.Location = new System.Drawing.Point(12, 168);
            this.SetupOption6.Name = "SetupOption6";
            this.SetupOption6.Size = new System.Drawing.Size(280, 20);
            this.SetupOption6.TabIndex = 6;
            this.SetupOption6.TabStop = true;
            this.SetupOption6.Text = "Bedrijfsinformatie";
            this.SetupOption6.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption7
            // 
            this.SetupOption7.Location = new System.Drawing.Point(12, 194);
            this.SetupOption7.Name = "SetupOption7";
            this.SetupOption7.Size = new System.Drawing.Size(280, 20);
            this.SetupOption7.TabIndex = 7;
            this.SetupOption7.Text = "Status Boekjaar";
            this.SetupOption7.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // SetupOption8
            // 
            this.SetupOption8.Location = new System.Drawing.Point(12, 220);
            this.SetupOption8.Name = "SetupOption8";
            this.SetupOption8.Size = new System.Drawing.Size(280, 20);
            this.SetupOption8.TabIndex = 8;
            this.SetupOption8.Text = "Kassaverkoop";
            this.SetupOption8.DoubleClick += new System.EventHandler(this.SetupOption_DoubleClick);
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(310, 12);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(95, 28);
            this.BtnOk.TabIndex = 9;
            this.BtnOk.Text = "&Openen";
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnClose.Location = new System.Drawing.Point(310, 50);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(95, 28);
            this.BtnClose.TabIndex = 10;
            this.BtnClose.Text = "&Sluiten";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // FormFiscalYearSettings
            // 
            this.AcceptButton = this.BtnOk;
            this.CancelButton = this.BtnClose;
            this.ClientSize = new System.Drawing.Size(417, 252);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.SetupOption8);
            this.Controls.Add(this.SetupOption7);
            this.Controls.Add(this.SetupOption6);
            this.Controls.Add(this.SetupOption5);
            this.Controls.Add(this.SetupOption4);
            this.Controls.Add(this.SetupOption3);
            this.Controls.Add(this.SetupOption2);
            this.Controls.Add(this.SetupOption1);
            this.Controls.Add(this.SetupOption0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFiscalYearSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Setup Boekjaar En Parameters";
            this.Load += new System.EventHandler(this.FormFiscalYearSettings_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.RadioButton SetupOption0;
        private System.Windows.Forms.RadioButton SetupOption1;
        private System.Windows.Forms.RadioButton SetupOption2;
        private System.Windows.Forms.RadioButton SetupOption3;
        private System.Windows.Forms.RadioButton SetupOption4;
        private System.Windows.Forms.RadioButton SetupOption5;
        private System.Windows.Forms.RadioButton SetupOption6;
        private System.Windows.Forms.RadioButton SetupOption7;
        private System.Windows.Forms.RadioButton SetupOption8;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnClose;
    }
}