namespace marVSS2028.PublicForms
{
    partial class FormSearchSQL
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
            this.stbSnelHelp = new System.Windows.Forms.StatusStrip();
            this.stbSnelHelpLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Sortering = new System.Windows.Forms.ComboBox();
            this.rtbSQLTekst = new System.Windows.Forms.RichTextBox();
            this.cmdZoeken = new System.Windows.Forms.Button();
            this.cmdSluiten = new System.Windows.Forms.Button();
            this.cmdBewaar = new System.Windows.Forms.Button();
            this.cmbExternedatabase = new System.Windows.Forms.ComboBox();
            this.mfgLijst = new System.Windows.Forms.DataGridView();
            this.chkExterneDatabase = new System.Windows.Forms.CheckBox();
            this.txtTeZoeken = new System.Windows.Forms.TextBox();
            this.lblTekst1 = new System.Windows.Forms.Label();
            this.lblTekst0 = new System.Windows.Forms.Label();
            this.stbSnelHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mfgLijst)).BeginInit();
            this.SuspendLayout();
            // 
            // stbSnelHelp
            // 
            this.stbSnelHelp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stbSnelHelpLabel});
            this.stbSnelHelp.Location = new System.Drawing.Point(0, 297);
            this.stbSnelHelp.Name = "stbSnelHelp";
            this.stbSnelHelp.Size = new System.Drawing.Size(660, 22);
            this.stbSnelHelp.TabIndex = 10;
            this.stbSnelHelp.Text = "stbSnelHelp";
            // 
            // stbSnelHelpLabel
            // 
            this.stbSnelHelpLabel.Name = "stbSnelHelpLabel";
            this.stbSnelHelpLabel.Size = new System.Drawing.Size(645, 17);
            this.stbSnelHelpLabel.Spring = true;
            this.stbSnelHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Sortering
            // 
            this.Sortering.BackColor = System.Drawing.Color.White;
            this.Sortering.Location = new System.Drawing.Point(0, 32);
            this.Sortering.Name = "Sortering";
            this.Sortering.Size = new System.Drawing.Size(297, 21);
            this.Sortering.TabIndex = 2;
            this.Sortering.SelectedIndexChanged += new System.EventHandler(this.Sortering_Click);
            this.Sortering.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Sortering_KeyDown);
            this.Sortering.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Sortering_KeyPress);
            // 
            // rtbSQLTekst
            // 
            this.rtbSQLTekst.BackColor = System.Drawing.Color.White;
            this.rtbSQLTekst.Location = new System.Drawing.Point(1, 199);
            this.rtbSQLTekst.Name = "rtbSQLTekst";
            this.rtbSQLTekst.Size = new System.Drawing.Size(544, 93);
            this.rtbSQLTekst.TabIndex = 8;
            this.rtbSQLTekst.TabStop = false;
            this.rtbSQLTekst.Text = "";
            this.rtbSQLTekst.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbSQLTekst_KeyPress);
            // 
            // cmdZoeken
            // 
            this.cmdZoeken.Location = new System.Drawing.Point(304, 8);
            this.cmdZoeken.Name = "cmdZoeken";
            this.cmdZoeken.Size = new System.Drawing.Size(81, 25);
            this.cmdZoeken.TabIndex = 7;
            this.cmdZoeken.TabStop = false;
            this.cmdZoeken.Text = "Zoeken";
            this.cmdZoeken.Click += new System.EventHandler(this.cmdZoeken_Click);
            // 
            // cmdSluiten
            // 
            this.cmdSluiten.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdSluiten.Location = new System.Drawing.Point(551, 267);
            this.cmdSluiten.Name = "cmdSluiten";
            this.cmdSluiten.Size = new System.Drawing.Size(81, 25);
            this.cmdSluiten.TabIndex = 6;
            this.cmdSluiten.TabStop = false;
            this.cmdSluiten.Text = "Sluiten";
            this.cmdSluiten.Click += new System.EventHandler(this.cmdSluiten_Click);
            // 
            // cmdBewaar
            // 
            this.cmdBewaar.Enabled = false;
            this.cmdBewaar.Location = new System.Drawing.Point(551, 199);
            this.cmdBewaar.Name = "cmdBewaar";
            this.cmdBewaar.Size = new System.Drawing.Size(81, 25);
            this.cmdBewaar.TabIndex = 5;
            this.cmdBewaar.TabStop = false;
            this.cmdBewaar.Text = "Bewaren";
            this.cmdBewaar.Visible = false;
            this.cmdBewaar.Click += new System.EventHandler(this.cmdBewaar_Click);
            // 
            // cmbExternedatabase
            // 
            this.cmbExternedatabase.Location = new System.Drawing.Point(0, 32);
            this.cmbExternedatabase.Name = "cmbExternedatabase";
            this.cmbExternedatabase.Size = new System.Drawing.Size(297, 21);
            this.cmbExternedatabase.TabIndex = 4;
            this.cmbExternedatabase.Visible = false;
            this.cmbExternedatabase.SelectedIndexChanged += new System.EventHandler(this.cmbExterneDatabase_Click);
            // 
            // mfgLijst
            // 
            this.mfgLijst.AllowUserToAddRows = false;
            this.mfgLijst.AllowUserToDeleteRows = false;
            this.mfgLijst.AllowUserToResizeRows = false;
            this.mfgLijst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mfgLijst.Location = new System.Drawing.Point(0, 56);
            this.mfgLijst.MultiSelect = false;
            this.mfgLijst.Name = "mfgLijst";
            this.mfgLijst.ReadOnly = true;
            this.mfgLijst.RowHeadersVisible = false;
            this.mfgLijst.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mfgLijst.Size = new System.Drawing.Size(632, 137);
            this.mfgLijst.TabIndex = 3;
            this.mfgLijst.TabStop = false;
            this.mfgLijst.DoubleClick += new System.EventHandler(this.mfgLijst_DblClick);
            this.mfgLijst.GotFocus += new System.EventHandler(this.mfgLijst_GotFocus);
            this.mfgLijst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mfgLijst_KeyDown);
            // 
            // chkExterneDatabase
            // 
            this.chkExterneDatabase.Location = new System.Drawing.Point(304, 36);
            this.chkExterneDatabase.Name = "chkExterneDatabase";
            this.chkExterneDatabase.Size = new System.Drawing.Size(113, 17);
            this.chkExterneDatabase.TabIndex = 11;
            this.chkExterneDatabase.TabStop = false;
            this.chkExterneDatabase.Text = "E&xterne database";
            this.chkExterneDatabase.Visible = false;
            this.chkExterneDatabase.CheckedChanged += new System.EventHandler(this.chkExterneDatabase_Click);
            // 
            // txtTeZoeken
            // 
            this.txtTeZoeken.BackColor = System.Drawing.Color.White;
            this.txtTeZoeken.ForeColor = System.Drawing.Color.Black;
            this.txtTeZoeken.Location = new System.Drawing.Point(72, 8);
            this.txtTeZoeken.Name = "txtTeZoeken";
            this.txtTeZoeken.Size = new System.Drawing.Size(225, 20);
            this.txtTeZoeken.TabIndex = 1;
            this.txtTeZoeken.TextChanged += new System.EventHandler(this.txtTeZoeken_Change);
            this.txtTeZoeken.GotFocus += new System.EventHandler(this.txtTeZoeken_GotFocus);
            this.txtTeZoeken.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTeZoeken_KeyDown);
            this.txtTeZoeken.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTeZoeken_KeyPress);
            // 
            // lblTekst1
            // 
            this.lblTekst1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTekst1.Location = new System.Drawing.Point(543, 33);
            this.lblTekst1.Name = "lblTekst1";
            this.lblTekst1.Size = new System.Drawing.Size(89, 17);
            this.lblTekst1.TabIndex = 9;
            this.lblTekst1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTekst0
            // 
            this.lblTekst0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTekst0.Location = new System.Drawing.Point(0, 8);
            this.lblTekst0.Name = "lblTekst0";
            this.lblTekst0.Size = new System.Drawing.Size(65, 17);
            this.lblTekst0.TabIndex = 0;
            this.lblTekst0.Text = " &Zoek zoals";
            // 
            // FormSearchSQL
            // 
            this.AcceptButton = this.cmdZoeken;
            this.CancelButton = this.cmdSluiten;
            this.ClientSize = new System.Drawing.Size(660, 319);
            this.ControlBox = false;
            this.Controls.Add(this.lblTekst0);
            this.Controls.Add(this.lblTekst1);
            this.Controls.Add(this.txtTeZoeken);
            this.Controls.Add(this.chkExterneDatabase);
            this.Controls.Add(this.mfgLijst);
            this.Controls.Add(this.cmbExternedatabase);
            this.Controls.Add(this.cmdBewaar);
            this.Controls.Add(this.cmdSluiten);
            this.Controls.Add(this.cmdZoeken);
            this.Controls.Add(this.rtbSQLTekst);
            this.Controls.Add(this.Sortering);
            this.Controls.Add(this.stbSnelHelp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSearchSQL";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ANSI-92 SQL GeSELECTeerd zoeken";
            this.Load += new System.EventHandler(this.FormSearchSQL_Load);
            this.stbSnelHelp.ResumeLayout(false);
            this.stbSnelHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mfgLijst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.StatusStrip stbSnelHelp;
        private System.Windows.Forms.ToolStripStatusLabel stbSnelHelpLabel;
        private System.Windows.Forms.ComboBox Sortering;
        private System.Windows.Forms.RichTextBox rtbSQLTekst;
        private System.Windows.Forms.Button cmdZoeken;
        private System.Windows.Forms.Button cmdSluiten;
        private System.Windows.Forms.Button cmdBewaar;
        private System.Windows.Forms.ComboBox cmbExternedatabase;
        private System.Windows.Forms.DataGridView mfgLijst;
        private System.Windows.Forms.CheckBox chkExterneDatabase;
        private System.Windows.Forms.TextBox txtTeZoeken;
        private System.Windows.Forms.Label lblTekst1;
        private System.Windows.Forms.Label lblTekst0;

        #endregion
    }
}