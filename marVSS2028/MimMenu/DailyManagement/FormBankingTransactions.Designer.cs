namespace marVSS2028.MimMenu.DailyManagement
{
    partial class FormBankingTransactions
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl SSTab1;
        private System.Windows.Forms.TabPage TabManueel;
        private System.Windows.Forms.TabPage TabCoda;

        private System.Windows.Forms.Label LabelInfo3;
        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.Label lblInfo0;
        private System.Windows.Forms.Label lblInfo5;
        private System.Windows.Forms.Label LabelInfo0;
        private System.Windows.Forms.Label LabelInfo11;
        private System.Windows.Forms.Label LabelInfo1;
        private System.Windows.Forms.Label lblInfo6;
        private System.Windows.Forms.Label LabelInfo12;
        private System.Windows.Forms.Label LabelInfo13;
        private System.Windows.Forms.Label lblInfo7;
        private System.Windows.Forms.Label LabelInfo2;
        private System.Windows.Forms.Label lblInfo4;

        private System.Windows.Forms.DateTimePicker Datum;
        private System.Windows.Forms.ComboBox KeuzeInfo0;
        private System.Windows.Forms.Button Volgende;
        private System.Windows.Forms.Button Afsluiten;
        private System.Windows.Forms.ListBox FinancieelDetail;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button Struktuur;
        private System.Windows.Forms.TextBox TextBoxWarningTestCompany;
        private System.Windows.Forms.Button ButtonReadCamt053;
        private System.Windows.Forms.CheckBox CheckBoxSepaViewer;

        private System.Windows.Forms.Button ButtonAssign;
        private System.Windows.Forms.Button ButtonTransfer;
        private System.Windows.Forms.DataGridView mfgLijst;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLijn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBbaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocument;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTegenRek;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBedrag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOmschrijving;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCumulSaldo;
        private System.Windows.Forms.Label LabelCounter;
        private System.Windows.Forms.Label Label1;

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
            this.SSTab1 = new System.Windows.Forms.TabControl();
            this.TabManueel = new System.Windows.Forms.TabPage();
            this.CheckBoxSepaViewer = new System.Windows.Forms.CheckBox();
            this.ButtonReadCamt053 = new System.Windows.Forms.Button();
            this.TextBoxWarningTestCompany = new System.Windows.Forms.TextBox();
            this.Struktuur = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.FinancieelDetail = new System.Windows.Forms.ListBox();
            this.Afsluiten = new System.Windows.Forms.Button();
            this.Volgende = new System.Windows.Forms.Button();
            this.KeuzeInfo0 = new System.Windows.Forms.ComboBox();
            this.Datum = new System.Windows.Forms.DateTimePicker();
            this.lblInfo4 = new System.Windows.Forms.Label();
            this.LabelInfo2 = new System.Windows.Forms.Label();
            this.lblInfo7 = new System.Windows.Forms.Label();
            this.LabelInfo13 = new System.Windows.Forms.Label();
            this.LabelInfo12 = new System.Windows.Forms.Label();
            this.lblInfo6 = new System.Windows.Forms.Label();
            this.LabelInfo1 = new System.Windows.Forms.Label();
            this.LabelInfo11 = new System.Windows.Forms.Label();
            this.LabelInfo0 = new System.Windows.Forms.Label();
            this.lblInfo5 = new System.Windows.Forms.Label();
            this.lblInfo0 = new System.Windows.Forms.Label();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.LabelInfo3 = new System.Windows.Forms.Label();
            this.TabCoda = new System.Windows.Forms.TabPage();
            this.LabelCounter = new System.Windows.Forms.Label();
            this.ButtonAssign = new System.Windows.Forms.Button();
            this.ButtonTransfer = new System.Windows.Forms.Button();
            this.mfgLijst = new System.Windows.Forms.DataGridView();
            this.colLijn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBbaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocument = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTegenRek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBedrag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOmschrijving = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCumulSaldo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label1 = new System.Windows.Forms.Label();
            this.SSTab1.SuspendLayout();
            this.TabManueel.SuspendLayout();
            this.TabCoda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mfgLijst)).BeginInit();
            this.SuspendLayout();
            // 
            // SSTab1
            // 
            this.SSTab1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.SSTab1.Controls.Add(this.TabManueel);
            this.SSTab1.Controls.Add(this.TabCoda);
            this.SSTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSTab1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SSTab1.ItemSize = new System.Drawing.Size(140, 24);
            this.SSTab1.Location = new System.Drawing.Point(0, 0);
            this.SSTab1.Name = "SSTab1";
            this.SSTab1.SelectedIndex = 0;
            this.SSTab1.Size = new System.Drawing.Size(867, 495);
            this.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.SSTab1.TabIndex = 0;
            this.SSTab1.SelectedIndexChanged += new System.EventHandler(this.SSTab1_SelectedIndexChanged);
            // 
            // TabManueel
            // 
            this.TabManueel.BackColor = System.Drawing.SystemColors.Control;
            this.TabManueel.Controls.Add(this.CheckBoxSepaViewer);
            this.TabManueel.Controls.Add(this.ButtonReadCamt053);
            this.TabManueel.Controls.Add(this.TextBoxWarningTestCompany);
            this.TabManueel.Controls.Add(this.Struktuur);
            this.TabManueel.Controls.Add(this.ButtonClose);
            this.TabManueel.Controls.Add(this.FinancieelDetail);
            this.TabManueel.Controls.Add(this.Afsluiten);
            this.TabManueel.Controls.Add(this.Volgende);
            this.TabManueel.Controls.Add(this.KeuzeInfo0);
            this.TabManueel.Controls.Add(this.Datum);
            this.TabManueel.Controls.Add(this.lblInfo4);
            this.TabManueel.Controls.Add(this.LabelInfo2);
            this.TabManueel.Controls.Add(this.lblInfo7);
            this.TabManueel.Controls.Add(this.LabelInfo13);
            this.TabManueel.Controls.Add(this.LabelInfo12);
            this.TabManueel.Controls.Add(this.lblInfo6);
            this.TabManueel.Controls.Add(this.LabelInfo1);
            this.TabManueel.Controls.Add(this.LabelInfo11);
            this.TabManueel.Controls.Add(this.LabelInfo0);
            this.TabManueel.Controls.Add(this.lblInfo5);
            this.TabManueel.Controls.Add(this.lblInfo0);
            this.TabManueel.Controls.Add(this.lblInfo1);
            this.TabManueel.Controls.Add(this.LabelInfo3);
            this.TabManueel.Location = new System.Drawing.Point(4, 4);
            this.TabManueel.Name = "TabManueel";
            this.TabManueel.Padding = new System.Windows.Forms.Padding(3);
            this.TabManueel.Size = new System.Drawing.Size(859, 463);
            this.TabManueel.TabIndex = 0;
            this.TabManueel.Text = "Manueel";
            // 
            // CheckBoxSepaViewer
            // 
            this.CheckBoxSepaViewer.AutoSize = true;
            this.CheckBoxSepaViewer.Checked = true;
            this.CheckBoxSepaViewer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxSepaViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.CheckBoxSepaViewer.Location = new System.Drawing.Point(698, 107);
            this.CheckBoxSepaViewer.Name = "CheckBoxSepaViewer";
            this.CheckBoxSepaViewer.Size = new System.Drawing.Size(91, 17);
            this.CheckBoxSepaViewer.TabIndex = 23;
            this.CheckBoxSepaViewer.Text = ".XDA Inkijken";
            this.CheckBoxSepaViewer.UseVisualStyleBackColor = true;
            // 
            // ButtonReadCamt053
            // 
            this.ButtonReadCamt053.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ButtonReadCamt053.Location = new System.Drawing.Point(656, 71);
            this.ButtonReadCamt053.Name = "ButtonReadCamt053";
            this.ButtonReadCamt053.Size = new System.Drawing.Size(133, 30);
            this.ButtonReadCamt053.TabIndex = 22;
            this.ButtonReadCamt053.Text = "ReadCamt053";
            this.ButtonReadCamt053.UseVisualStyleBackColor = true;
            this.ButtonReadCamt053.Click += new System.EventHandler(this.ButtonReadCamt053_Click);
            // 
            // TextBoxWarningTestCompany
            // 
            this.TextBoxWarningTestCompany.BackColor = System.Drawing.Color.Red;
            this.TextBoxWarningTestCompany.Enabled = false;
            this.TextBoxWarningTestCompany.Location = new System.Drawing.Point(186, 365);
            this.TextBoxWarningTestCompany.Name = "TextBoxWarningTestCompany";
            this.TextBoxWarningTestCompany.Size = new System.Drawing.Size(265, 20);
            this.TextBoxWarningTestCompany.TabIndex = 21;
            this.TextBoxWarningTestCompany.TabStop = false;
            this.TextBoxWarningTestCompany.Text = "Opgelet: Dit is een testbedrijf!";
            this.TextBoxWarningTestCompany.Visible = false;
            // 
            // Struktuur
            // 
            this.Struktuur.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Struktuur.Location = new System.Drawing.Point(414, 417);
            this.Struktuur.Name = "Struktuur";
            this.Struktuur.Size = new System.Drawing.Size(280, 30);
            this.Struktuur.TabIndex = 9;
            this.Struktuur.TabStop = false;
            this.Struktuur.Text = "&Gestructureerde Verrichting";
            this.Struktuur.UseVisualStyleBackColor = true;
            this.Struktuur.Click += new System.EventHandler(this.Struktuur_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ButtonClose.Location = new System.Drawing.Point(702, 417);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(144, 30);
            this.ButtonClose.TabIndex = 8;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FinancieelDetail
            // 
            this.FinancieelDetail.Font = new System.Drawing.Font("Courier New", 9F);
            this.FinancieelDetail.FormattingEnabled = true;
            this.FinancieelDetail.ItemHeight = 15;
            this.FinancieelDetail.Location = new System.Drawing.Point(6, 173);
            this.FinancieelDetail.Name = "FinancieelDetail";
            this.FinancieelDetail.Size = new System.Drawing.Size(847, 229);
            this.FinancieelDetail.TabIndex = 4;
            this.FinancieelDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FinancieelDetail_KeyDown);
            // 
            // Afsluiten
            // 
            this.Afsluiten.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Afsluiten.Location = new System.Drawing.Point(506, 101);
            this.Afsluiten.Name = "Afsluiten";
            this.Afsluiten.Size = new System.Drawing.Size(144, 27);
            this.Afsluiten.TabIndex = 3;
            this.Afsluiten.TabStop = false;
            this.Afsluiten.Text = "&Boeken";
            this.Afsluiten.UseVisualStyleBackColor = true;
            this.Afsluiten.Click += new System.EventHandler(this.Afsluiten_Click);
            // 
            // Volgende
            // 
            this.Volgende.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Volgende.Location = new System.Drawing.Point(505, 71);
            this.Volgende.Name = "Volgende";
            this.Volgende.Size = new System.Drawing.Size(144, 25);
            this.Volgende.TabIndex = 7;
            this.Volgende.TabStop = false;
            this.Volgende.Text = "Ma&nueel";
            this.Volgende.UseVisualStyleBackColor = true;
            this.Volgende.Click += new System.EventHandler(this.Volgende_Click);
            // 
            // KeuzeInfo0
            // 
            this.KeuzeInfo0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeuzeInfo0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.KeuzeInfo0.FormattingEnabled = true;
            this.KeuzeInfo0.Location = new System.Drawing.Point(12, 35);
            this.KeuzeInfo0.Name = "KeuzeInfo0";
            this.KeuzeInfo0.Size = new System.Drawing.Size(372, 21);
            this.KeuzeInfo0.TabIndex = 0;
            this.KeuzeInfo0.SelectedIndexChanged += new System.EventHandler(this.KeuzeInfo0_SelectedIndexChanged);
            this.KeuzeInfo0.Leave += new System.EventHandler(this.KeuzeInfo0_Leave);
            // 
            // Datum
            // 
            this.Datum.CustomFormat = "dd/MM/yyyy";
            this.Datum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Datum.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Datum.Location = new System.Drawing.Point(506, 36);
            this.Datum.Name = "Datum";
            this.Datum.Size = new System.Drawing.Size(91, 20);
            this.Datum.TabIndex = 2;
            this.Datum.Leave += new System.EventHandler(this.Datum_Leave);
            // 
            // lblInfo4
            // 
            this.lblInfo4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfo4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblInfo4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo4.Location = new System.Drawing.Point(12, 71);
            this.lblInfo4.Name = "lblInfo4";
            this.lblInfo4.Size = new System.Drawing.Size(98, 27);
            this.lblInfo4.TabIndex = 28;
            this.lblInfo4.Text = "BeginSaldo EUR";
            this.lblInfo4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelInfo2
            // 
            this.LabelInfo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo2.Font = new System.Drawing.Font("Courier New", 9F);
            this.LabelInfo2.Location = new System.Drawing.Point(6, 149);
            this.LabelInfo2.Name = "LabelInfo2";
            this.LabelInfo2.Size = new System.Drawing.Size(847, 22);
            this.LabelInfo2.TabIndex = 20;
            this.LabelInfo2.Text = " Document           TegenR.       Bedrag Omschrijving                  Fin.Kort.";
            // 
            // lblInfo7
            // 
            this.lblInfo7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfo7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblInfo7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo7.Location = new System.Drawing.Point(257, 101);
            this.lblInfo7.Name = "lblInfo7";
            this.lblInfo7.Size = new System.Drawing.Size(98, 27);
            this.lblInfo7.TabIndex = 19;
            this.lblInfo7.Text = "EindSaldo BEF";
            this.lblInfo7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelInfo13
            // 
            this.LabelInfo13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.LabelInfo13.Location = new System.Drawing.Point(358, 101);
            this.LabelInfo13.Name = "LabelInfo13";
            this.LabelInfo13.Size = new System.Drawing.Size(131, 27);
            this.LabelInfo13.TabIndex = 18;
            this.LabelInfo13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelInfo12
            // 
            this.LabelInfo12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.LabelInfo12.Location = new System.Drawing.Point(115, 101);
            this.LabelInfo12.Name = "LabelInfo12";
            this.LabelInfo12.Size = new System.Drawing.Size(135, 27);
            this.LabelInfo12.TabIndex = 17;
            this.LabelInfo12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInfo6
            // 
            this.lblInfo6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfo6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblInfo6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo6.Location = new System.Drawing.Point(12, 101);
            this.lblInfo6.Name = "lblInfo6";
            this.lblInfo6.Size = new System.Drawing.Size(98, 27);
            this.lblInfo6.TabIndex = 16;
            this.lblInfo6.Text = "BeginSaldo BEF";
            this.lblInfo6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelInfo1
            // 
            this.LabelInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo1.Location = new System.Drawing.Point(506, 11);
            this.LabelInfo1.Name = "LabelInfo1";
            this.LabelInfo1.Size = new System.Drawing.Size(91, 24);
            this.LabelInfo1.TabIndex = 1;
            this.LabelInfo1.Text = "Datu&m uittreksel";
            this.LabelInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelInfo11
            // 
            this.LabelInfo11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo11.Location = new System.Drawing.Point(390, 35);
            this.LabelInfo11.Name = "LabelInfo11";
            this.LabelInfo11.Size = new System.Drawing.Size(99, 21);
            this.LabelInfo11.TabIndex = 15;
            this.LabelInfo11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelInfo0
            // 
            this.LabelInfo0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo0.Location = new System.Drawing.Point(18, 14);
            this.LabelInfo0.Name = "LabelInfo0";
            this.LabelInfo0.Size = new System.Drawing.Size(114, 24);
            this.LabelInfo0.TabIndex = 14;
            this.LabelInfo0.Text = "Financiële &Rekening";
            this.LabelInfo0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInfo5
            // 
            this.lblInfo5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfo5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblInfo5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo5.Location = new System.Drawing.Point(257, 69);
            this.lblInfo5.Name = "lblInfo5";
            this.lblInfo5.Size = new System.Drawing.Size(98, 27);
            this.lblInfo5.TabIndex = 13;
            this.lblInfo5.Text = "Eindsaldo EUR";
            this.lblInfo5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInfo0
            // 
            this.lblInfo0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfo0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblInfo0.Location = new System.Drawing.Point(115, 69);
            this.lblInfo0.Name = "lblInfo0";
            this.lblInfo0.Size = new System.Drawing.Size(135, 27);
            this.lblInfo0.TabIndex = 12;
            this.lblInfo0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInfo1
            // 
            this.lblInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblInfo1.Location = new System.Drawing.Point(358, 69);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(131, 27);
            this.lblInfo1.TabIndex = 11;
            this.lblInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelInfo3
            // 
            this.LabelInfo3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo3.Location = new System.Drawing.Point(399, 14);
            this.LabelInfo3.Name = "LabelInfo3";
            this.LabelInfo3.Size = new System.Drawing.Size(90, 21);
            this.LabelInfo3.TabIndex = 10;
            this.LabelInfo3.Text = "Uittreksel";
            this.LabelInfo3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TabCoda
            // 
            this.TabCoda.BackColor = System.Drawing.SystemColors.Control;
            this.TabCoda.Controls.Add(this.LabelCounter);
            this.TabCoda.Controls.Add(this.ButtonAssign);
            this.TabCoda.Controls.Add(this.ButtonTransfer);
            this.TabCoda.Controls.Add(this.mfgLijst);
            this.TabCoda.Location = new System.Drawing.Point(4, 4);
            this.TabCoda.Name = "TabCoda";
            this.TabCoda.Padding = new System.Windows.Forms.Padding(3);
            this.TabCoda.Size = new System.Drawing.Size(859, 463);
            this.TabCoda.TabIndex = 1;
            this.TabCoda.Text = "Full CODA (versie 2.1)";
            // 
            // LabelCounter
            // 
            this.LabelCounter.Location = new System.Drawing.Point(560, 429);
            this.LabelCounter.Name = "LabelCounter";
            this.LabelCounter.Size = new System.Drawing.Size(121, 25);
            this.LabelCounter.TabIndex = 27;
            this.LabelCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ButtonAssign
            // 
            this.ButtonAssign.Enabled = false;
            this.ButtonAssign.Location = new System.Drawing.Point(692, 424);
            this.ButtonAssign.Name = "ButtonAssign";
            this.ButtonAssign.Size = new System.Drawing.Size(135, 30);
            this.ButtonAssign.TabIndex = 25;
            this.ButtonAssign.Text = "Toewijzen";
            this.ButtonAssign.UseVisualStyleBackColor = true;
            this.ButtonAssign.Visible = false;
            this.ButtonAssign.Click += new System.EventHandler(this.ButtonAssign_Click);
            // 
            // ButtonTransfer
            // 
            this.ButtonTransfer.Enabled = false;
            this.ButtonTransfer.Location = new System.Drawing.Point(548, 424);
            this.ButtonTransfer.Name = "ButtonTransfer";
            this.ButtonTransfer.Size = new System.Drawing.Size(135, 30);
            this.ButtonTransfer.TabIndex = 24;
            this.ButtonTransfer.Text = "Overnemen";
            this.ButtonTransfer.UseVisualStyleBackColor = true;
            this.ButtonTransfer.Click += new System.EventHandler(this.ButtonTransfer_Click);
            // 
            // mfgLijst
            // 
            this.mfgLijst.AllowUserToAddRows = false;
            this.mfgLijst.AllowUserToDeleteRows = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mfgLijst.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.mfgLijst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mfgLijst.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLijn,
            this.colBbaCode,
            this.colDocument,
            this.colTegenRek,
            this.colBedrag,
            this.colOmschrijving,
            this.colCumulSaldo});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.mfgLijst.DefaultCellStyle = dataGridViewCellStyle10;
            this.mfgLijst.Location = new System.Drawing.Point(6, 6);
            this.mfgLijst.MultiSelect = false;
            this.mfgLijst.Name = "mfgLijst";
            this.mfgLijst.RowHeadersVisible = false;
            this.mfgLijst.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mfgLijst.Size = new System.Drawing.Size(821, 412);
            this.mfgLijst.TabIndex = 6;
            this.mfgLijst.SelectionChanged += new System.EventHandler(this.mfgLijst_SelectionChanged);
            // 
            // colLijn
            // 
            this.colLijn.HeaderText = "Lijn";
            this.colLijn.Name = "colLijn";
            this.colLijn.Width = 45;
            // 
            // colBbaCode
            // 
            this.colBbaCode.HeaderText = "BBA Code";
            this.colBbaCode.Name = "colBbaCode";
            this.colBbaCode.Width = 90;
            // 
            // colDocument
            // 
            this.colDocument.HeaderText = "Document";
            this.colDocument.Name = "colDocument";
            this.colDocument.Width = 120;
            // 
            // colTegenRek
            // 
            this.colTegenRek.HeaderText = "TegenRek.";
            this.colTegenRek.Name = "colTegenRek";
            this.colTegenRek.Width = 90;
            // 
            // colBedrag
            // 
            this.colBedrag.HeaderText = "Bedrag";
            this.colBedrag.Name = "colBedrag";
            this.colBedrag.Width = 90;
            // 
            // colOmschrijving
            // 
            this.colOmschrijving.HeaderText = "Omschrijving";
            this.colOmschrijving.Name = "colOmschrijving";
            this.colOmschrijving.Width = 170;
            // 
            // colCumulSaldo
            // 
            this.colCumulSaldo.HeaderText = "Cumul Saldo";
            this.colCumulSaldo.Name = "colCumulSaldo";
            this.colCumulSaldo.Width = 110;
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(372, 372);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(121, 49);
            this.Label1.TabIndex = 26;
            this.Label1.Text = "Label1";
            this.Label1.Visible = false;
            // 
            // FormBankingTransactions
            // 
            this.AcceptButton = this.Volgende;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(867, 495);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.SSTab1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormBankingTransactions";
            this.Text = "Ctrl+F3 Financiële verrichtingen";
            this.Load += new System.EventHandler(this.FormBankingTransactions_Load);
            this.SSTab1.ResumeLayout(false);
            this.TabManueel.ResumeLayout(false);
            this.TabManueel.PerformLayout();
            this.TabCoda.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mfgLijst)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
