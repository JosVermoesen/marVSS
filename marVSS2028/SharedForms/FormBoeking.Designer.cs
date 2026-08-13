namespace marVSS2028.SharedForms
{
    partial class FormBoeking
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmdBoeken = new System.Windows.Forms.Button();
            this.cmdNegeren = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabJournaal = new System.Windows.Forms.TabPage();
            this.dgvBoekLijst = new System.Windows.Forms.DataGridView();
            this.colRekening = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOmschrijving = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEurDebet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEurCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBefDebet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBefCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDocument = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabJournaal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBoekLijst)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdBoeken
            // 
            this.cmdBoeken.Location = new System.Drawing.Point(416, 8);
            this.cmdBoeken.Name = "cmdBoeken";
            this.cmdBoeken.Size = new System.Drawing.Size(129, 25);
            this.cmdBoeken.TabIndex = 1;
            this.cmdBoeken.TabStop = false;
            this.cmdBoeken.Text = "&Boeking laten doorgaan";
            this.cmdBoeken.Click += new System.EventHandler(this.cmdBoeken_Click);
            // 
            // cmdNegeren
            // 
            this.cmdNegeren.Location = new System.Drawing.Point(551, 8);
            this.cmdNegeren.Name = "cmdNegeren";
            this.cmdNegeren.Size = new System.Drawing.Size(128, 25);
            this.cmdNegeren.TabIndex = 2;
            this.cmdNegeren.TabStop = false;
            this.cmdNegeren.Text = "Boeking Terugzetten";
            this.cmdNegeren.Click += new System.EventHandler(this.cmdNegeren_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabJournaal);
            this.tabControl1.Controls.Add(this.tabDocument);
            this.tabControl1.Location = new System.Drawing.Point(0, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(687, 227);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabJournaal
            // 
            this.tabJournaal.Controls.Add(this.dgvBoekLijst);
            this.tabJournaal.Location = new System.Drawing.Point(4, 22);
            this.tabJournaal.Name = "tabJournaal";
            this.tabJournaal.Size = new System.Drawing.Size(679, 201);
            this.tabJournaal.TabIndex = 0;
            this.tabJournaal.Text = "Journaal";
            // 
            // dgvBoekLijst
            // 
            this.dgvBoekLijst.AllowUserToAddRows = false;
            this.dgvBoekLijst.AllowUserToDeleteRows = false;
            this.dgvBoekLijst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBoekLijst.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRekening,
            this.colOmschrijving,
            this.colEurDebet,
            this.colEurCredit,
            this.colBefDebet,
            this.colBefCredit});
            this.dgvBoekLijst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBoekLijst.Location = new System.Drawing.Point(0, 0);
            this.dgvBoekLijst.Name = "dgvBoekLijst";
            this.dgvBoekLijst.ReadOnly = true;
            this.dgvBoekLijst.RowHeadersVisible = false;
            this.dgvBoekLijst.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBoekLijst.Size = new System.Drawing.Size(679, 201);
            this.dgvBoekLijst.TabIndex = 1;
            // 
            // colRekening
            // 
            this.colRekening.Name = "colRekening";
            this.colRekening.ReadOnly = true;
            this.colRekening.Width = 70;
            // 
            // colOmschrijving
            // 
            this.colOmschrijving.Name = "colOmschrijving";
            this.colOmschrijving.ReadOnly = true;
            this.colOmschrijving.Width = 185;
            // 
            // colEurDebet
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colEurDebet.DefaultCellStyle = dataGridViewCellStyle9;
            this.colEurDebet.Name = "colEurDebet";
            this.colEurDebet.ReadOnly = true;
            // 
            // colEurCredit
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colEurCredit.DefaultCellStyle = dataGridViewCellStyle10;
            this.colEurCredit.Name = "colEurCredit";
            this.colEurCredit.ReadOnly = true;
            // 
            // colBefDebet
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colBefDebet.DefaultCellStyle = dataGridViewCellStyle11;
            this.colBefDebet.Name = "colBefDebet";
            this.colBefDebet.ReadOnly = true;
            // 
            // colBefCredit
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colBefCredit.DefaultCellStyle = dataGridViewCellStyle12;
            this.colBefCredit.Name = "colBefCredit";
            this.colBefCredit.ReadOnly = true;
            // 
            // tabDocument
            // 
            this.tabDocument.Location = new System.Drawing.Point(4, 22);
            this.tabDocument.Name = "tabDocument";
            this.tabDocument.Size = new System.Drawing.Size(540, 167);
            this.tabDocument.TabIndex = 1;
            this.tabDocument.Text = "Document";
            // 
            // FormBoeking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 264);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdBoeken);
            this.Controls.Add(this.cmdNegeren);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBoeking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BoekingsDetail";
            this.Load += new System.EventHandler(this.FormBoeking_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabJournaal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBoekLijst)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdBoeken;
        private System.Windows.Forms.Button cmdNegeren;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabJournaal;
        private System.Windows.Forms.TabPage tabDocument;
        private System.Windows.Forms.DataGridView dgvBoekLijst;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRekening;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOmschrijving;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEurDebet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEurCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBefDebet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBefCredit;
    }
}
