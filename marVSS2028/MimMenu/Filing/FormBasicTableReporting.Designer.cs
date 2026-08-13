namespace marVSS2028.MimMenu.Filing
{
    partial class FormBasicTableReporting
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
            this.cmbTabel = new System.Windows.Forms.ComboBox();
            this.cmbSortering = new System.Windows.Forms.ComboBox();
            this.cmbRapportDefinitie = new System.Windows.Forms.ComboBox();
            this.cmbFormattering = new System.Windows.Forms.ComboBox();
            this.lstTabelVelden = new System.Windows.Forms.ListBox();
            this.lstRapportVelden = new System.Windows.Forms.ListBox();
            this.txtVan = new System.Windows.Forms.TextBox();
            this.txtTot = new System.Windows.Forms.TextBox();
            this.txtRapportnaam = new System.Windows.Forms.TextBox();
            this.txtTitelEdit = new System.Windows.Forms.TextBox();
            this.txtTabPosEdit = new System.Windows.Forms.TextBox();
            this.txtKeyLen = new System.Windows.Forms.TextBox();
            this.lblAantalInSelektie = new System.Windows.Forms.Label();
            this.btnAfdrukken = new System.Windows.Forms.Button();
            this.btnDefinitie = new System.Windows.Forms.Button();
            this.btnToevoegen = new System.Windows.Forms.Button();
            this.btnTitel = new System.Windows.Forms.Button();
            this.btnFormattering = new System.Windows.Forms.Button();
            this.btnTabPositie = new System.Windows.Forms.Button();
            this.btnToonSQL = new System.Windows.Forms.Button();
            this.btnSQLOvername = new System.Windows.Forms.Button();
            this.lblTabel = new System.Windows.Forms.Label();
            this.lblRapportDefinitie = new System.Windows.Forms.Label();
            this.lblSortering = new System.Windows.Forms.Label();
            this.lblVan = new System.Windows.Forms.Label();
            this.lblTot = new System.Windows.Forms.Label();
            this.lblLengteSorteer = new System.Windows.Forms.Label();
            this.lblAantal = new System.Windows.Forms.Label();
            this.lblRapportnaam = new System.Windows.Forms.Label();
            this.lblTabelvelden = new System.Windows.Forms.Label();
            this.lblRapportvelden = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbTabel
            // 
            this.cmbTabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTabel.Location = new System.Drawing.Point(8, 32);
            this.cmbTabel.Name = "cmbTabel";
            this.cmbTabel.Size = new System.Drawing.Size(161, 21);
            this.cmbTabel.TabIndex = 1;
            this.cmbTabel.SelectedIndexChanged += new System.EventHandler(this.CmbTabel_SelectedIndexChanged);
            // 
            // cmbSortering
            // 
            this.cmbSortering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSortering.Location = new System.Drawing.Point(8, 72);
            this.cmbSortering.Name = "cmbSortering";
            this.cmbSortering.Size = new System.Drawing.Size(161, 21);
            this.cmbSortering.TabIndex = 5;
            this.cmbSortering.SelectedIndexChanged += new System.EventHandler(this.CmbSortering_SelectedIndexChanged);
            // 
            // cmbRapportDefinitie
            // 
            this.cmbRapportDefinitie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRapportDefinitie.Location = new System.Drawing.Point(168, 32);
            this.cmbRapportDefinitie.Name = "cmbRapportDefinitie";
            this.cmbRapportDefinitie.Size = new System.Drawing.Size(240, 21);
            this.cmbRapportDefinitie.Sorted = true;
            this.cmbRapportDefinitie.TabIndex = 3;
            this.cmbRapportDefinitie.SelectedIndexChanged += new System.EventHandler(this.CmbRapportDefinitie_SelectedIndexChanged);
            this.cmbRapportDefinitie.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmbRapportDefinitie_KeyDown);
            // 
            // cmbFormattering
            // 
            this.cmbFormattering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormattering.Location = new System.Drawing.Point(362, 199);
            this.cmbFormattering.Name = "cmbFormattering";
            this.cmbFormattering.Size = new System.Drawing.Size(270, 21);
            this.cmbFormattering.TabIndex = 24;
            this.cmbFormattering.Visible = false;
            this.cmbFormattering.LostFocus += new System.EventHandler(this.CmbFormattering_LostFocus);
            // 
            // lstTabelVelden
            // 
            this.lstTabelVelden.Enabled = false;
            this.lstTabelVelden.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lstTabelVelden.ItemHeight = 14;
            this.lstTabelVelden.Location = new System.Drawing.Point(10, 199);
            this.lstTabelVelden.Name = "lstTabelVelden";
            this.lstTabelVelden.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTabelVelden.Size = new System.Drawing.Size(265, 130);
            this.lstTabelVelden.TabIndex = 17;
            this.lstTabelVelden.SelectedIndexChanged += new System.EventHandler(this.LstTabelVelden_SelectedIndexChanged);
            this.lstTabelVelden.GotFocus += new System.EventHandler(this.LstTabelVelden_GotFocus);
            // 
            // lstRapportVelden
            // 
            this.lstRapportVelden.Enabled = false;
            this.lstRapportVelden.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.lstRapportVelden.ItemHeight = 14;
            this.lstRapportVelden.Location = new System.Drawing.Point(362, 199);
            this.lstRapportVelden.Name = "lstRapportVelden";
            this.lstRapportVelden.Size = new System.Drawing.Size(270, 130);
            this.lstRapportVelden.TabIndex = 20;
            this.lstRapportVelden.GotFocus += new System.EventHandler(this.LstRapportVelden_GotFocus);
            this.lstRapportVelden.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LstRapportVelden_KeyDown);
            // 
            // txtVan
            // 
            this.txtVan.Location = new System.Drawing.Point(214, 72);
            this.txtVan.Name = "txtVan";
            this.txtVan.Size = new System.Drawing.Size(109, 20);
            this.txtVan.TabIndex = 7;
            this.txtVan.GotFocus += new System.EventHandler(this.Txt_GotFocus);
            this.txtVan.LostFocus += new System.EventHandler(this.TxtVanTot_LostFocus);
            // 
            // txtTot
            // 
            this.txtTot.Location = new System.Drawing.Point(214, 96);
            this.txtTot.Name = "txtTot";
            this.txtTot.Size = new System.Drawing.Size(109, 20);
            this.txtTot.TabIndex = 9;
            this.txtTot.GotFocus += new System.EventHandler(this.Txt_GotFocus);
            this.txtTot.LostFocus += new System.EventHandler(this.TxtVanTot_LostFocus);
            // 
            // txtRapportnaam
            // 
            this.txtRapportnaam.Enabled = false;
            this.txtRapportnaam.Location = new System.Drawing.Point(104, 159);
            this.txtRapportnaam.Name = "txtRapportnaam";
            this.txtRapportnaam.Size = new System.Drawing.Size(395, 20);
            this.txtRapportnaam.TabIndex = 15;
            // 
            // txtTitelEdit
            // 
            this.txtTitelEdit.Location = new System.Drawing.Point(362, 199);
            this.txtTitelEdit.Name = "txtTitelEdit";
            this.txtTitelEdit.Size = new System.Drawing.Size(270, 20);
            this.txtTitelEdit.TabIndex = 25;
            this.txtTitelEdit.Visible = false;
            this.txtTitelEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtTitelEdit_KeyDown);
            this.txtTitelEdit.LostFocus += new System.EventHandler(this.TxtTitelEdit_LostFocus);
            // 
            // txtTabPosEdit
            // 
            this.txtTabPosEdit.Location = new System.Drawing.Point(362, 199);
            this.txtTabPosEdit.Name = "txtTabPosEdit";
            this.txtTabPosEdit.Size = new System.Drawing.Size(75, 20);
            this.txtTabPosEdit.TabIndex = 26;
            this.txtTabPosEdit.Visible = false;
            this.txtTabPosEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtTabPosEdit_KeyDown);
            this.txtTabPosEdit.LostFocus += new System.EventHandler(this.TxtTabPosEdit_LostFocus);
            // 
            // txtKeyLen
            // 
            this.txtKeyLen.Location = new System.Drawing.Point(136, 120);
            this.txtKeyLen.Name = "txtKeyLen";
            this.txtKeyLen.Size = new System.Drawing.Size(75, 20);
            this.txtKeyLen.TabIndex = 13;
            this.txtKeyLen.GotFocus += new System.EventHandler(this.Txt_GotFocus);
            this.txtKeyLen.LostFocus += new System.EventHandler(this.TxtKeyLen_LostFocus);
            // 
            // lblAantalInSelektie
            // 
            this.lblAantalInSelektie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAantalInSelektie.Location = new System.Drawing.Point(333, 123);
            this.lblAantalInSelektie.Name = "lblAantalInSelektie";
            this.lblAantalInSelektie.Size = new System.Drawing.Size(75, 17);
            this.lblAantalInSelektie.TabIndex = 31;
            this.lblAantalInSelektie.Text = " ";
            this.lblAantalInSelektie.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAfdrukken
            // 
            this.btnAfdrukken.Location = new System.Drawing.Point(416, 30);
            this.btnAfdrukken.Name = "btnAfdrukken";
            this.btnAfdrukken.Size = new System.Drawing.Size(108, 23);
            this.btnAfdrukken.TabIndex = 10;
            this.btnAfdrukken.Text = "Raport Genereren";
            this.btnAfdrukken.Click += new System.EventHandler(this.BtnAfdrukken_Click);
            // 
            // btnDefinitie
            // 
            this.btnDefinitie.Location = new System.Drawing.Point(498, 115);
            this.btnDefinitie.Name = "btnDefinitie";
            this.btnDefinitie.Size = new System.Drawing.Size(125, 23);
            this.btnDefinitie.TabIndex = 14;
            this.btnDefinitie.Text = "&Nieuwe Definitie maken";
            this.btnDefinitie.Visible = false;
            // 
            // btnToevoegen
            // 
            this.btnToevoegen.Enabled = false;
            this.btnToevoegen.Location = new System.Drawing.Point(282, 199);
            this.btnToevoegen.Name = "btnToevoegen";
            this.btnToevoegen.Size = new System.Drawing.Size(75, 23);
            this.btnToevoegen.TabIndex = 19;
            this.btnToevoegen.Text = "Toevoegen";
            this.btnToevoegen.Click += new System.EventHandler(this.BtnToevoegen_Click);
            // 
            // btnTitel
            // 
            this.btnTitel.Enabled = false;
            this.btnTitel.Location = new System.Drawing.Point(282, 227);
            this.btnTitel.Name = "btnTitel";
            this.btnTitel.Size = new System.Drawing.Size(75, 23);
            this.btnTitel.TabIndex = 21;
            this.btnTitel.Text = "Titel";
            this.btnTitel.Click += new System.EventHandler(this.BtnTitel_Click);
            // 
            // btnFormattering
            // 
            this.btnFormattering.Enabled = false;
            this.btnFormattering.Location = new System.Drawing.Point(282, 255);
            this.btnFormattering.Name = "btnFormattering";
            this.btnFormattering.Size = new System.Drawing.Size(75, 23);
            this.btnFormattering.TabIndex = 22;
            this.btnFormattering.Text = "Formattering";
            this.btnFormattering.Click += new System.EventHandler(this.BtnFormattering_Click);
            // 
            // btnTabPositie
            // 
            this.btnTabPositie.Enabled = false;
            this.btnTabPositie.Location = new System.Drawing.Point(282, 283);
            this.btnTabPositie.Name = "btnTabPositie";
            this.btnTabPositie.Size = new System.Drawing.Size(75, 23);
            this.btnTabPositie.TabIndex = 27;
            this.btnTabPositie.Text = "TabPositie";
            this.btnTabPositie.Click += new System.EventHandler(this.BtnTabPositie_Click);
            // 
            // btnToonSQL
            // 
            this.btnToonSQL.Location = new System.Drawing.Point(327, 56);
            this.btnToonSQL.Name = "btnToonSQL";
            this.btnToonSQL.Size = new System.Drawing.Size(165, 23);
            this.btnToonSQL.TabIndex = 30;
            this.btnToonSQL.Text = "&Toon SQL SELECT Definitie";
            this.btnToonSQL.Click += new System.EventHandler(this.BtnToonSQL_Click);
            // 
            // btnSQLOvername
            // 
            this.btnSQLOvername.Location = new System.Drawing.Point(327, 84);
            this.btnSQLOvername.Name = "btnSQLOvername";
            this.btnSQLOvername.Size = new System.Drawing.Size(165, 23);
            this.btnSQLOvername.TabIndex = 32;
            this.btnSQLOvername.Text = "SQL Resultaat via Generator";
            this.btnSQLOvername.Click += new System.EventHandler(this.BtnSQLOvername_Click);
            // 
            // lblTabel
            // 
            this.lblTabel.AutoSize = true;
            this.lblTabel.Location = new System.Drawing.Point(10, 16);
            this.lblTabel.Name = "lblTabel";
            this.lblTabel.Size = new System.Drawing.Size(34, 13);
            this.lblTabel.TabIndex = 33;
            this.lblTabel.Text = "&Tabel";
            // 
            // lblRapportDefinitie
            // 
            this.lblRapportDefinitie.AutoSize = true;
            this.lblRapportDefinitie.Location = new System.Drawing.Point(170, 16);
            this.lblRapportDefinitie.Name = "lblRapportDefinitie";
            this.lblRapportDefinitie.Size = new System.Drawing.Size(115, 13);
            this.lblRapportDefinitie.TabIndex = 34;
            this.lblRapportDefinitie.Text = "Aktieve &rapportdefinitie";
            // 
            // lblSortering
            // 
            this.lblSortering.AutoSize = true;
            this.lblSortering.Location = new System.Drawing.Point(10, 56);
            this.lblSortering.Name = "lblSortering";
            this.lblSortering.Size = new System.Drawing.Size(49, 13);
            this.lblSortering.TabIndex = 35;
            this.lblSortering.Text = "&Sortering";
            // 
            // lblVan
            // 
            this.lblVan.AutoSize = true;
            this.lblVan.Location = new System.Drawing.Point(188, 56);
            this.lblVan.Name = "lblVan";
            this.lblVan.Size = new System.Drawing.Size(26, 13);
            this.lblVan.TabIndex = 36;
            this.lblVan.Text = "&Van";
            // 
            // lblTot
            // 
            this.lblTot.AutoSize = true;
            this.lblTot.Location = new System.Drawing.Point(188, 80);
            this.lblTot.Name = "lblTot";
            this.lblTot.Size = new System.Drawing.Size(23, 13);
            this.lblTot.TabIndex = 37;
            this.lblTot.Text = "&Tot";
            // 
            // lblLengteSorteer
            // 
            this.lblLengteSorteer.AutoSize = true;
            this.lblLengteSorteer.Location = new System.Drawing.Point(10, 122);
            this.lblLengteSorteer.Name = "lblLengteSorteer";
            this.lblLengteSorteer.Size = new System.Drawing.Size(107, 13);
            this.lblLengteSorteer.TabIndex = 38;
            this.lblLengteSorteer.Text = "&Lengte Sorteersleutel";
            // 
            // lblAantal
            // 
            this.lblAantal.AutoSize = true;
            this.lblAantal.Location = new System.Drawing.Point(236, 125);
            this.lblAantal.Name = "lblAantal";
            this.lblAantal.Size = new System.Drawing.Size(87, 13);
            this.lblAantal.TabIndex = 39;
            this.lblAantal.Text = "Aantal in selectie";
            // 
            // lblRapportnaam
            // 
            this.lblRapportnaam.AutoSize = true;
            this.lblRapportnaam.Location = new System.Drawing.Point(12, 162);
            this.lblRapportnaam.Name = "lblRapportnaam";
            this.lblRapportnaam.Size = new System.Drawing.Size(71, 13);
            this.lblRapportnaam.TabIndex = 40;
            this.lblRapportnaam.Text = "Rapportnaam";
            // 
            // lblTabelvelden
            // 
            this.lblTabelvelden.AutoSize = true;
            this.lblTabelvelden.Location = new System.Drawing.Point(10, 183);
            this.lblTabelvelden.Name = "lblTabelvelden";
            this.lblTabelvelden.Size = new System.Drawing.Size(66, 13);
            this.lblTabelvelden.TabIndex = 41;
            this.lblTabelvelden.Text = "Tabelvelden";
            // 
            // lblRapportvelden
            // 
            this.lblRapportvelden.AutoSize = true;
            this.lblRapportvelden.Location = new System.Drawing.Point(362, 183);
            this.lblRapportvelden.Name = "lblRapportvelden";
            this.lblRapportvelden.Size = new System.Drawing.Size(77, 13);
            this.lblRapportvelden.TabIndex = 42;
            this.lblRapportvelden.Text = "Rapportvelden";
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(548, 30);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 44;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormBasicTableReporting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(644, 340);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.cmbTabel);
            this.Controls.Add(this.cmbSortering);
            this.Controls.Add(this.cmbRapportDefinitie);
            this.Controls.Add(this.cmbFormattering);
            this.Controls.Add(this.lstTabelVelden);
            this.Controls.Add(this.lstRapportVelden);
            this.Controls.Add(this.txtVan);
            this.Controls.Add(this.txtTot);
            this.Controls.Add(this.txtRapportnaam);
            this.Controls.Add(this.txtTitelEdit);
            this.Controls.Add(this.txtTabPosEdit);
            this.Controls.Add(this.txtKeyLen);
            this.Controls.Add(this.lblAantalInSelektie);
            this.Controls.Add(this.btnAfdrukken);
            this.Controls.Add(this.btnDefinitie);
            this.Controls.Add(this.btnToevoegen);
            this.Controls.Add(this.btnTitel);
            this.Controls.Add(this.btnFormattering);
            this.Controls.Add(this.btnTabPositie);
            this.Controls.Add(this.btnToonSQL);
            this.Controls.Add(this.btnSQLOvername);
            this.Controls.Add(this.lblTabel);
            this.Controls.Add(this.lblRapportDefinitie);
            this.Controls.Add(this.lblSortering);
            this.Controls.Add(this.lblVan);
            this.Controls.Add(this.lblTot);
            this.Controls.Add(this.lblLengteSorteer);
            this.Controls.Add(this.lblAantal);
            this.Controls.Add(this.lblRapportnaam);
            this.Controls.Add(this.lblTabelvelden);
            this.Controls.Add(this.lblRapportvelden);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormBasicTableReporting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ANSI-92 SQL Lijst rapportage";
            this.Load += new System.EventHandler(this.FormBasicTableReporting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox    cmbTabel;
        private System.Windows.Forms.ComboBox    cmbSortering;
        private System.Windows.Forms.ComboBox    cmbRapportDefinitie;
        private System.Windows.Forms.ComboBox    cmbFormattering;
        private System.Windows.Forms.ListBox     lstTabelVelden;
        private System.Windows.Forms.ListBox     lstRapportVelden;
        private System.Windows.Forms.TextBox     txtVan;
        private System.Windows.Forms.TextBox     txtTot;
        private System.Windows.Forms.TextBox     txtRapportnaam;
        private System.Windows.Forms.TextBox     txtTitelEdit;
        private System.Windows.Forms.TextBox     txtTabPosEdit;
        private System.Windows.Forms.TextBox     txtKeyLen;
        private System.Windows.Forms.Label       lblAantalInSelektie;
        private System.Windows.Forms.Button      btnAfdrukken;
        private System.Windows.Forms.Button      btnDefinitie;
        private System.Windows.Forms.Button      btnToevoegen;
        private System.Windows.Forms.Button      btnTitel;
        private System.Windows.Forms.Button      btnFormattering;
        private System.Windows.Forms.Button      btnTabPositie;
        private System.Windows.Forms.Button      btnToonSQL;
        private System.Windows.Forms.Button      btnSQLOvername;
        private System.Windows.Forms.Label       lblTabel;
        private System.Windows.Forms.Label       lblRapportDefinitie;
        private System.Windows.Forms.Label       lblSortering;
        private System.Windows.Forms.Label       lblVan;
        private System.Windows.Forms.Label       lblTot;
        private System.Windows.Forms.Label       lblLengteSorteer;
        private System.Windows.Forms.Label       lblAantal;
        private System.Windows.Forms.Label       lblRapportnaam;
        private System.Windows.Forms.Label       lblTabelvelden;
        private System.Windows.Forms.Label       lblRapportvelden;
        private System.Windows.Forms.Button ButtonClose;
    }
}
