namespace marVSS2028.Forms
{
    partial class FormSQLOperations
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
            this.LvDatabase = new System.Windows.Forms.ListView();
            this.TxtSQL = new System.Windows.Forms.TextBox();
            this.TxtPLUS = new System.Windows.Forms.TextBox();
            this.TxtWaarde = new System.Windows.Forms.TextBox();
            this.CmbSelect = new System.Windows.Forms.ComboBox();
            this.CbSQLBevel = new System.Windows.Forms.ComboBox();
            this.CbVelden = new System.Windows.Forms.ComboBox();
            this.CbOperatie = new System.Windows.Forms.ComboBox();
            this.LblRecordCount = new System.Windows.Forms.Label();
            this.ButtonSQL = new System.Windows.Forms.Button();
            this.ButtonExecute = new System.Windows.Forms.Button();
            this.ButtonSelectWegschrijven = new System.Windows.Forms.Button();
            this.ButtonKopij = new System.Windows.Forms.Button();
            this.ButtonOpenXML = new System.Windows.Forms.Button();
            this.ButtonSluiten = new System.Windows.Forms.Button();
            this.ButtonVersie = new System.Windows.Forms.Button();
            this.ButtonNet1 = new System.Windows.Forms.Button();
            this.ButtonBackup = new System.Windows.Forms.Button();
            this.PanelFilter = new System.Windows.Forms.Panel();
            this.SeparatorTop = new System.Windows.Forms.Label();
            this.SeparatorBottom = new System.Windows.Forms.Label();
            this.GridSQL = new System.Windows.Forms.DataGridView();
            this.PanelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridSQL)).BeginInit();
            this.SuspendLayout();
            // 
            // LvDatabase
            // 
            this.LvDatabase.FullRowSelect = true;
            this.LvDatabase.HideSelection = false;
            this.LvDatabase.Location = new System.Drawing.Point(783, 0);
            this.LvDatabase.Name = "LvDatabase";
            this.LvDatabase.Size = new System.Drawing.Size(113, 293);
            this.LvDatabase.TabIndex = 7;
            this.LvDatabase.UseCompatibleStateImageBehavior = false;
            this.LvDatabase.Click += new System.EventHandler(this.LvDatabase_Click);
            this.LvDatabase.DoubleClick += new System.EventHandler(this.LvDatabase_DoubleClick);
            // 
            // TxtSQL
            // 
            this.TxtSQL.Location = new System.Drawing.Point(4, 176);
            this.TxtSQL.Multiline = true;
            this.TxtSQL.Name = "TxtSQL";
            this.TxtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtSQL.Size = new System.Drawing.Size(687, 117);
            this.TxtSQL.TabIndex = 0;
            // 
            // TxtPLUS
            // 
            this.TxtPLUS.Enabled = false;
            this.TxtPLUS.Location = new System.Drawing.Point(188, 4);
            this.TxtPLUS.Name = "TxtPLUS";
            this.TxtPLUS.Size = new System.Drawing.Size(201, 20);
            this.TxtPLUS.TabIndex = 16;
            this.TxtPLUS.TextChanged += new System.EventHandler(this.TxtPLUS_TextChanged);
            // 
            // TxtWaarde
            // 
            this.TxtWaarde.Enabled = false;
            this.TxtWaarde.Location = new System.Drawing.Point(504, 4);
            this.TxtWaarde.Name = "TxtWaarde";
            this.TxtWaarde.Size = new System.Drawing.Size(101, 20);
            this.TxtWaarde.TabIndex = 15;
            this.TxtWaarde.TextChanged += new System.EventHandler(this.TxtWaarde_TextChanged);
            // 
            // CmbSelect
            // 
            this.CmbSelect.Location = new System.Drawing.Point(96, 154);
            this.CmbSelect.Name = "CmbSelect";
            this.CmbSelect.Size = new System.Drawing.Size(189, 21);
            this.CmbSelect.TabIndex = 1;
            this.CmbSelect.SelectedIndexChanged += new System.EventHandler(this.CmbSelect_SelectedIndexChanged);
            this.CmbSelect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmbSelect_KeyDown);
            // 
            // CbSQLBevel
            // 
            this.CbSQLBevel.Enabled = false;
            this.CbSQLBevel.Items.AddRange(new object[] {
            "SELECT",
            "DELETE",
            "UPDATE"});
            this.CbSQLBevel.Location = new System.Drawing.Point(0, 4);
            this.CbSQLBevel.Name = "CbSQLBevel";
            this.CbSQLBevel.Size = new System.Drawing.Size(89, 21);
            this.CbSQLBevel.TabIndex = 12;
            this.CbSQLBevel.SelectedIndexChanged += new System.EventHandler(this.CbSQLBevel_SelectedIndexChanged);
            // 
            // CbVelden
            // 
            this.CbVelden.Enabled = false;
            this.CbVelden.Location = new System.Drawing.Point(93, 4);
            this.CbVelden.Name = "CbVelden";
            this.CbVelden.Size = new System.Drawing.Size(93, 21);
            this.CbVelden.TabIndex = 13;
            this.CbVelden.SelectedIndexChanged += new System.EventHandler(this.CbVelden_SelectedIndexChanged);
            // 
            // CbOperatie
            // 
            this.CbOperatie.Enabled = false;
            this.CbOperatie.Items.AddRange(new object[] {
            "=",
            "<>",
            "LIKE",
            "<",
            ">",
            "<=",
            ">="});
            this.CbOperatie.Location = new System.Drawing.Point(395, 4);
            this.CbOperatie.Name = "CbOperatie";
            this.CbOperatie.Size = new System.Drawing.Size(105, 21);
            this.CbOperatie.TabIndex = 14;
            this.CbOperatie.SelectedIndexChanged += new System.EventHandler(this.CbOperatie_SelectedIndexChanged);
            // 
            // LblRecordCount
            // 
            this.LblRecordCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LblRecordCount.Location = new System.Drawing.Point(697, -1);
            this.LblRecordCount.Name = "LblRecordCount";
            this.LblRecordCount.Size = new System.Drawing.Size(79, 19);
            this.LblRecordCount.TabIndex = 10;
            this.LblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ButtonSQL
            // 
            this.ButtonSQL.Location = new System.Drawing.Point(4, 154);
            this.ButtonSQL.Name = "ButtonSQL";
            this.ButtonSQL.Size = new System.Drawing.Size(82, 20);
            this.ButtonSQL.TabIndex = 8;
            this.ButtonSQL.Text = "SQL &SELECT";
            this.ButtonSQL.Click += new System.EventHandler(this.ButtonSQL_Click);
            // 
            // ButtonExecute
            // 
            this.ButtonExecute.Location = new System.Drawing.Point(316, 152);
            this.ButtonExecute.Name = "ButtonExecute";
            this.ButtonExecute.Size = new System.Drawing.Size(90, 20);
            this.ButtonExecute.TabIndex = 6;
            this.ButtonExecute.TabStop = false;
            this.ButtonExecute.Text = "SQL &EXECUTE";
            this.ButtonExecute.Click += new System.EventHandler(this.ButtonExecute_Click);
            // 
            // ButtonSelectWegschrijven
            // 
            this.ButtonSelectWegschrijven.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ButtonSelectWegschrijven.Location = new System.Drawing.Point(288, 154);
            this.ButtonSelectWegschrijven.Name = "ButtonSelectWegschrijven";
            this.ButtonSelectWegschrijven.Size = new System.Drawing.Size(25, 20);
            this.ButtonSelectWegschrijven.TabIndex = 9;
            this.ButtonSelectWegschrijven.TabStop = false;
            this.ButtonSelectWegschrijven.Text = "▼";
            this.ButtonSelectWegschrijven.Click += new System.EventHandler(this.ButtonSelectWegschrijven_Click);
            // 
            // ButtonKopij
            // 
            this.ButtonKopij.Location = new System.Drawing.Point(697, 19);
            this.ButtonKopij.Name = "ButtonKopij";
            this.ButtonKopij.Size = new System.Drawing.Size(80, 46);
            this.ButtonKopij.TabIndex = 3;
            this.ButtonKopij.Text = "XML &Kopie";
            this.ButtonKopij.Click += new System.EventHandler(this.ButtonKopij_Click);
            // 
            // ButtonOpenXML
            // 
            this.ButtonOpenXML.Location = new System.Drawing.Point(697, 71);
            this.ButtonOpenXML.Name = "ButtonOpenXML";
            this.ButtonOpenXML.Size = new System.Drawing.Size(80, 46);
            this.ButtonOpenXML.TabIndex = 11;
            this.ButtonOpenXML.Text = "XML &Openen";
            this.ButtonOpenXML.Click += new System.EventHandler(this.ButtonOpenXML_Click);
            // 
            // ButtonSluiten
            // 
            this.ButtonSluiten.Location = new System.Drawing.Point(697, 123);
            this.ButtonSluiten.Name = "ButtonSluiten";
            this.ButtonSluiten.Size = new System.Drawing.Size(79, 20);
            this.ButtonSluiten.TabIndex = 4;
            this.ButtonSluiten.Text = "Sluiten";
            this.ButtonSluiten.Click += new System.EventHandler(this.ButtonSluiten_Click);
            // 
            // ButtonVersie
            // 
            this.ButtonVersie.Location = new System.Drawing.Point(697, 151);
            this.ButtonVersie.Name = "ButtonVersie";
            this.ButtonVersie.Size = new System.Drawing.Size(80, 20);
            this.ButtonVersie.TabIndex = 5;
            this.ButtonVersie.TabStop = false;
            this.ButtonVersie.Text = "&Versie";
            this.ButtonVersie.Click += new System.EventHandler(this.ButtonVersie_Click);
            // 
            // ButtonNet1
            // 
            this.ButtonNet1.Location = new System.Drawing.Point(697, 147);
            this.ButtonNet1.Name = "ButtonNet1";
            this.ButtonNet1.Size = new System.Drawing.Size(33, 25);
            this.ButtonNet1.TabIndex = 17;
            this.ButtonNet1.Text = "Net1";
            this.ButtonNet1.Click += new System.EventHandler(this.ButtonNet1_Click);
            // 
            // ButtonBackup
            // 
            this.ButtonBackup.Location = new System.Drawing.Point(733, 147);
            this.ButtonBackup.Name = "ButtonBackup";
            this.ButtonBackup.Size = new System.Drawing.Size(45, 25);
            this.ButtonBackup.TabIndex = 18;
            this.ButtonBackup.Text = "Backup";
            this.ButtonBackup.Click += new System.EventHandler(this.ButtonBackup_Click);
            // 
            // PanelFilter
            // 
            this.PanelFilter.Controls.Add(this.CbSQLBevel);
            this.PanelFilter.Controls.Add(this.CbVelden);
            this.PanelFilter.Controls.Add(this.TxtPLUS);
            this.PanelFilter.Controls.Add(this.CbOperatie);
            this.PanelFilter.Controls.Add(this.TxtWaarde);
            this.PanelFilter.Location = new System.Drawing.Point(4, 447);
            this.PanelFilter.Name = "PanelFilter";
            this.PanelFilter.Size = new System.Drawing.Size(605, 28);
            this.PanelFilter.TabIndex = 20;
            // 
            // SeparatorTop
            // 
            this.SeparatorTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SeparatorTop.Location = new System.Drawing.Point(4, 296);
            this.SeparatorTop.Name = "SeparatorTop";
            this.SeparatorTop.Size = new System.Drawing.Size(605, 2);
            this.SeparatorTop.TabIndex = 19;
            // 
            // SeparatorBottom
            // 
            this.SeparatorBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SeparatorBottom.Location = new System.Drawing.Point(4, 330);
            this.SeparatorBottom.Name = "SeparatorBottom";
            this.SeparatorBottom.Size = new System.Drawing.Size(605, 2);
            this.SeparatorBottom.TabIndex = 21;
            // 
            // GridSQL
            // 
            this.GridSQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridSQL.Location = new System.Drawing.Point(0, 0);
            this.GridSQL.Name = "GridSQL";
            this.GridSQL.Size = new System.Drawing.Size(674, 150);
            this.GridSQL.TabIndex = 22;
            // 
            // FormSQLOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 477);
            this.Controls.Add(this.GridSQL);
            this.Controls.Add(this.LvDatabase);
            this.Controls.Add(this.LblRecordCount);
            this.Controls.Add(this.ButtonKopij);
            this.Controls.Add(this.ButtonOpenXML);
            this.Controls.Add(this.ButtonSluiten);
            this.Controls.Add(this.ButtonNet1);
            this.Controls.Add(this.ButtonBackup);
            this.Controls.Add(this.CmbSelect);
            this.Controls.Add(this.ButtonSQL);
            this.Controls.Add(this.ButtonSelectWegschrijven);
            this.Controls.Add(this.ButtonExecute);
            this.Controls.Add(this.ButtonVersie);
            this.Controls.Add(this.TxtSQL);
            this.Controls.Add(this.SeparatorTop);
            this.Controls.Add(this.PanelFilter);
            this.Controls.Add(this.SeparatorBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSQLOperations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ANSI-92 SQL Database Beheer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSQLOperations_FormClosed);
            this.Load += new System.EventHandler(this.FormSQLOperations_Load);
            this.PanelFilter.ResumeLayout(false);
            this.PanelFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridSQL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView            LvDatabase;
        private System.Windows.Forms.TextBox             TxtSQL;
        private System.Windows.Forms.TextBox             TxtPLUS;
        private System.Windows.Forms.TextBox             TxtWaarde;
        private System.Windows.Forms.ComboBox            CmbSelect;
        private System.Windows.Forms.ComboBox            CbSQLBevel;
        private System.Windows.Forms.ComboBox            CbVelden;
        private System.Windows.Forms.ComboBox            CbOperatie;
        private System.Windows.Forms.Label               LblRecordCount;
        private System.Windows.Forms.Button              ButtonSQL;
        private System.Windows.Forms.Button              ButtonExecute;
        private System.Windows.Forms.Button              ButtonSelectWegschrijven;
        private System.Windows.Forms.Button              ButtonKopij;
        private System.Windows.Forms.Button              ButtonOpenXML;
        private System.Windows.Forms.Button              ButtonSluiten;
        private System.Windows.Forms.Button              ButtonVersie;
        private System.Windows.Forms.Button              ButtonNet1;
        private System.Windows.Forms.Button              ButtonBackup;
        private System.Windows.Forms.Panel               PanelFilter;
        private System.Windows.Forms.Label               SeparatorTop;
        private System.Windows.Forms.Label               SeparatorBottom;
        private System.Windows.Forms.DataGridView GridSQL;
    }
}
