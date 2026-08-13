namespace marVSS2028.PublicForms
{
    partial class FormKeuzeVSF
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NTBoxLijst = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // NTBoxLijst
            // 
            this.NTBoxLijst.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NTBoxLijst.FormattingEnabled = true;
            this.NTBoxLijst.ItemHeight = 16;
            this.NTBoxLijst.Location = new System.Drawing.Point(0, 0);
            this.NTBoxLijst.Name = "NTBoxLijst";
            this.NTBoxLijst.Size = new System.Drawing.Size(269, 196);
            this.NTBoxLijst.TabIndex = 0;
            this.NTBoxLijst.DoubleClick += new System.EventHandler(this.NTBoxLijst_DoubleClick);
            this.NTBoxLijst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NTBoxLijst_KeyDown);
            this.NTBoxLijst.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NTBoxLijst_KeyPress);
            // 
            // FormKeuzeVSF
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(271, 204);
            this.ControlBox = false;
            this.Controls.Add(this.NTBoxLijst);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormKeuzeVSF";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KeuzeLijst";
            this.Activated += new System.EventHandler(this.FormKeuzeVSF_Activated);
            this.Resize += new System.EventHandler(this.FormKeuzeVSF_Resize);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ListBox NTBoxLijst;

        #endregion
    }
}