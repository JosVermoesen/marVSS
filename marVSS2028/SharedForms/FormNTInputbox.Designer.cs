namespace marVSS2028.SharedForms
{
    partial class FormNTInputbox
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
            this.Ok = new System.Windows.Forms.Button();
            this.Sluiten = new System.Windows.Forms.Button();
            this.Hernieuw = new System.Windows.Forms.Button();
            this.BtnForward = new System.Windows.Forms.Button();
            this.BtnBack = new System.Windows.Forms.Button();
            this.TekstInfo = new System.Windows.Forms.MaskedTextBox();
            this.MedeDeling = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(416, 10);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 3;
            this.Ok.Text = "OK";
            // 
            // Sluiten
            // 
            this.Sluiten.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Sluiten.Location = new System.Drawing.Point(416, 45);
            this.Sluiten.Name = "Sluiten";
            this.Sluiten.Size = new System.Drawing.Size(75, 23);
            this.Sluiten.TabIndex = 4;
            this.Sluiten.Text = "Sluiten";
            // 
            // Hernieuw
            // 
            this.Hernieuw.Location = new System.Drawing.Point(335, 45);
            this.Hernieuw.Name = "Hernieuw";
            this.Hernieuw.Size = new System.Drawing.Size(75, 23);
            this.Hernieuw.TabIndex = 5;
            this.Hernieuw.Text = "Zoek";
            this.Hernieuw.Visible = false;
            // 
            // BtnForward
            // 
            this.BtnForward.Location = new System.Drawing.Point(299, 45);
            this.BtnForward.Name = "BtnForward";
            this.BtnForward.Size = new System.Drawing.Size(30, 23);
            this.BtnForward.TabIndex = 6;
            this.BtnForward.Text = ">";
            this.BtnForward.Visible = false;
            // 
            // BtnBack
            // 
            this.BtnBack.Location = new System.Drawing.Point(11, 45);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(30, 23);
            this.BtnBack.TabIndex = 7;
            this.BtnBack.Text = "<";
            this.BtnBack.Visible = false;
            // 
            // TekstInfo
            // 
            this.TekstInfo.Location = new System.Drawing.Point(11, 13);
            this.TekstInfo.Name = "TekstInfo";
            this.TekstInfo.Size = new System.Drawing.Size(399, 20);
            this.TekstInfo.TabIndex = 0;
            // 
            // MedeDeling
            // 
            this.MedeDeling.Location = new System.Drawing.Point(0, 85);
            this.MedeDeling.Name = "MedeDeling";
            this.MedeDeling.Size = new System.Drawing.Size(500, 22);
            this.MedeDeling.TabIndex = 9;
            this.MedeDeling.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo.Location = new System.Drawing.Point(47, 45);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(246, 23);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormNTInputbox
            // 
            this.AcceptButton = this.Ok;
            this.CancelButton = this.Sluiten;
            this.ClientSize = new System.Drawing.Size(500, 107);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.MedeDeling);
            this.Controls.Add(this.TekstInfo);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Sluiten);
            this.Controls.Add(this.Hernieuw);
            this.Controls.Add(this.BtnForward);
            this.Controls.Add(this.BtnBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNTInputbox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Sluiten;
        public System.Windows.Forms.Button Hernieuw;
        public System.Windows.Forms.Button BtnForward;
        public System.Windows.Forms.Button BtnBack;
        public System.Windows.Forms.MaskedTextBox TekstInfo;
        private System.Windows.Forms.StatusStrip MedeDeling;
        public System.Windows.Forms.Label lblInfo;
    }
}