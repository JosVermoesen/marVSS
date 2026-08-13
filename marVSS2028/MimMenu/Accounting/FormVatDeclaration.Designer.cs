namespace marVSS2028.MimMenu.Accounting
{
    partial class FormVatDeclaration
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.TvwBtwAangiftes = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.cbRestitution2025 = new System.Windows.Forms.CheckBox();
            this.cbPayment2025 = new System.Windows.Forms.CheckBox();
            this.btnXml2025 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cbAanvraagTerugbetaling = new System.Windows.Forms.CheckBox();
            this.cbAanvraagBetaalformulieren = new System.Windows.Forms.CheckBox();
            this.cbVergrendel = new System.Windows.Forms.CheckBox();
            this.btnInitialiseren = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.btnIntervat2025 = new System.Windows.Forms.Button();
            this.tbMailBtw = new System.Windows.Forms.TextBox();
            this.txtPeriodeNr = new System.Windows.Forms.TextBox();
            this.txtPeriodeTot = new System.Windows.Forms.TextBox();
            this.lblAktievePeriodeTot = new System.Windows.Forms.Label();
            this.lblAankopen = new System.Windows.Forms.Label();
            this.lblVerkopen = new System.Windows.Forms.Label();
            this.lblDoc0 = new System.Windows.Forms.Label();
            this.lblDoc1 = new System.Windows.Forms.Label();
            this.lblDoc2 = new System.Windows.Forms.Label();
            this.lblDoc3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TvwBtwAangiftes
            // 
            this.TvwBtwAangiftes.Location = new System.Drawing.Point(4, 77);
            this.TvwBtwAangiftes.Name = "TvwBtwAangiftes";
            this.TvwBtwAangiftes.Size = new System.Drawing.Size(155, 495);
            this.TvwBtwAangiftes.TabIndex = 0;
            this.TvwBtwAangiftes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvwBtwAangiftes_AfterSelect);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(165, 77);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(558, 495);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox2);
            this.tabPage3.Controls.Add(this.cbRestitution2025);
            this.tabPage3.Controls.Add(this.cbPayment2025);
            this.tabPage3.Controls.Add(this.btnXml2025);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(550, 469);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Intervat 2025";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox2.Location = new System.Drawing.Point(0, 32);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(550, 437);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            // 
            // cbRestitution2025
            // 
            this.cbRestitution2025.Location = new System.Drawing.Point(6, 9);
            this.cbRestitution2025.Name = "cbRestitution2025";
            this.cbRestitution2025.Size = new System.Drawing.Size(193, 17);
            this.cbRestitution2025.TabIndex = 1;
            this.cbRestitution2025.Text = "Aanvraag om Terugbetaling";
            this.cbRestitution2025.CheckedChanged += new System.EventHandler(this.CbRestitution2025_CheckedChanged);
            // 
            // cbPayment2025
            // 
            this.cbPayment2025.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbPayment2025.Location = new System.Drawing.Point(358, 9);
            this.cbPayment2025.Name = "cbPayment2025";
            this.cbPayment2025.Size = new System.Drawing.Size(177, 17);
            this.cbPayment2025.TabIndex = 2;
            this.cbPayment2025.Text = "Aanvraag van Betaalformulieren";
            this.cbPayment2025.CheckedChanged += new System.EventHandler(this.CbPayment2025_CheckedChanged);
            // 
            // btnXml2025
            // 
            this.btnXml2025.Location = new System.Drawing.Point(205, 3);
            this.btnXml2025.Name = "btnXml2025";
            this.btnXml2025.Size = new System.Drawing.Size(105, 23);
            this.btnXml2025.TabIndex = 3;
            this.btnXml2025.Text = "XML Bestand";
            this.btnXml2025.Click += new System.EventHandler(this.BtnXml2025_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Controls.Add(this.cbAanvraagTerugbetaling);
            this.tabPage2.Controls.Add(this.cbAanvraagBetaalformulieren);
            this.tabPage2.Controls.Add(this.cbVergrendel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(550, 469);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Intervat 2008";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(0, 35);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(550, 434);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // cbAanvraagTerugbetaling
            // 
            this.cbAanvraagTerugbetaling.Enabled = false;
            this.cbAanvraagTerugbetaling.Location = new System.Drawing.Point(6, 12);
            this.cbAanvraagTerugbetaling.Name = "cbAanvraagTerugbetaling";
            this.cbAanvraagTerugbetaling.Size = new System.Drawing.Size(193, 17);
            this.cbAanvraagTerugbetaling.TabIndex = 1;
            this.cbAanvraagTerugbetaling.Text = "Aanvraag om Terugbetaling";
            this.cbAanvraagTerugbetaling.CheckedChanged += new System.EventHandler(this.CbAanvraagTerugbetaling_CheckedChanged);
            // 
            // cbAanvraagBetaalformulieren
            // 
            this.cbAanvraagBetaalformulieren.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAanvraagBetaalformulieren.Enabled = false;
            this.cbAanvraagBetaalformulieren.Location = new System.Drawing.Point(358, 12);
            this.cbAanvraagBetaalformulieren.Name = "cbAanvraagBetaalformulieren";
            this.cbAanvraagBetaalformulieren.Size = new System.Drawing.Size(177, 17);
            this.cbAanvraagBetaalformulieren.TabIndex = 2;
            this.cbAanvraagBetaalformulieren.Text = "Aanvraag van Betaalformulieren";
            this.cbAanvraagBetaalformulieren.CheckedChanged += new System.EventHandler(this.CbAanvraagBetaalformulieren_CheckedChanged);
            // 
            // cbVergrendel
            // 
            this.cbVergrendel.Checked = true;
            this.cbVergrendel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVergrendel.Enabled = false;
            this.cbVergrendel.Location = new System.Drawing.Point(205, 12);
            this.cbVergrendel.Name = "cbVergrendel";
            this.cbVergrendel.Size = new System.Drawing.Size(105, 17);
            this.cbVergrendel.TabIndex = 3;
            this.cbVergrendel.Text = "Vergrendeld";
            this.cbVergrendel.CheckedChanged += new System.EventHandler(this.CbVergrendel_CheckedChanged);
            // 
            // btnInitialiseren
            // 
            this.btnInitialiseren.Enabled = false;
            this.btnInitialiseren.Location = new System.Drawing.Point(439, 43);
            this.btnInitialiseren.Name = "btnInitialiseren";
            this.btnInitialiseren.Size = new System.Drawing.Size(74, 21);
            this.btnInitialiseren.TabIndex = 3;
            this.btnInitialiseren.TabStop = false;
            this.btnInitialiseren.Text = "Initialiseren";
            this.btnInitialiseren.Click += new System.EventHandler(this.BtnInitialiseren_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(439, 7);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // btnIntervat2025
            // 
            this.btnIntervat2025.Location = new System.Drawing.Point(616, 3);
            this.btnIntervat2025.Name = "btnIntervat2025";
            this.btnIntervat2025.Size = new System.Drawing.Size(99, 28);
            this.btnIntervat2025.TabIndex = 205;
            this.btnIntervat2025.TabStop = false;
            this.btnIntervat2025.Text = "XML 04/2025";
            this.btnIntervat2025.Visible = false;
            this.btnIntervat2025.Click += new System.EventHandler(this.BtnIntervat2025_Click);
            // 
            // tbMailBtw
            // 
            this.tbMailBtw.Location = new System.Drawing.Point(522, 44);
            this.tbMailBtw.Name = "tbMailBtw";
            this.tbMailBtw.Size = new System.Drawing.Size(193, 20);
            this.tbMailBtw.TabIndex = 204;
            this.tbMailBtw.Text = "info@rv.be";
            // 
            // txtPeriodeNr
            // 
            this.txtPeriodeNr.Enabled = false;
            this.txtPeriodeNr.Location = new System.Drawing.Point(118, 11);
            this.txtPeriodeNr.Name = "txtPeriodeNr";
            this.txtPeriodeNr.Size = new System.Drawing.Size(27, 20);
            this.txtPeriodeNr.TabIndex = 5;
            // 
            // txtPeriodeTot
            // 
            this.txtPeriodeTot.Enabled = false;
            this.txtPeriodeTot.Location = new System.Drawing.Point(90, 31);
            this.txtPeriodeTot.Name = "txtPeriodeTot";
            this.txtPeriodeTot.Size = new System.Drawing.Size(55, 20);
            this.txtPeriodeTot.TabIndex = 6;
            // 
            // lblAktievePeriodeTot
            // 
            this.lblAktievePeriodeTot.Location = new System.Drawing.Point(4, 32);
            this.lblAktievePeriodeTot.Name = "lblAktievePeriodeTot";
            this.lblAktievePeriodeTot.Size = new System.Drawing.Size(80, 17);
            this.lblAktievePeriodeTot.TabIndex = 4;
            this.lblAktievePeriodeTot.Text = "Periode-Einde";
            this.lblAktievePeriodeTot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAankopen
            // 
            this.lblAankopen.BackColor = System.Drawing.Color.Lime;
            this.lblAankopen.Location = new System.Drawing.Point(262, 7);
            this.lblAankopen.Name = "lblAankopen";
            this.lblAankopen.Size = new System.Drawing.Size(80, 17);
            this.lblAankopen.TabIndex = 1;
            this.lblAankopen.Text = "Aankopen";
            this.lblAankopen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVerkopen
            // 
            this.lblVerkopen.BackColor = System.Drawing.Color.Blue;
            this.lblVerkopen.ForeColor = System.Drawing.Color.White;
            this.lblVerkopen.Location = new System.Drawing.Point(354, 7);
            this.lblVerkopen.Name = "lblVerkopen";
            this.lblVerkopen.Size = new System.Drawing.Size(80, 17);
            this.lblVerkopen.TabIndex = 2;
            this.lblVerkopen.Text = "Verkopen";
            this.lblVerkopen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDoc0
            // 
            this.lblDoc0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDoc0.Location = new System.Drawing.Point(254, 25);
            this.lblDoc0.Name = "lblDoc0";
            this.lblDoc0.Size = new System.Drawing.Size(88, 17);
            this.lblDoc0.TabIndex = 7;
            // 
            // lblDoc1
            // 
            this.lblDoc1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDoc1.Location = new System.Drawing.Point(254, 43);
            this.lblDoc1.Name = "lblDoc1";
            this.lblDoc1.Size = new System.Drawing.Size(88, 17);
            this.lblDoc1.TabIndex = 8;
            // 
            // lblDoc2
            // 
            this.lblDoc2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDoc2.Location = new System.Drawing.Point(346, 25);
            this.lblDoc2.Name = "lblDoc2";
            this.lblDoc2.Size = new System.Drawing.Size(88, 17);
            this.lblDoc2.TabIndex = 9;
            // 
            // lblDoc3
            // 
            this.lblDoc3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDoc3.Location = new System.Drawing.Point(346, 43);
            this.lblDoc3.Name = "lblDoc3";
            this.lblDoc3.Size = new System.Drawing.Size(88, 17);
            this.lblDoc3.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(7, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 21);
            this.label1.TabIndex = 206;
            this.label1.Text = "Klikken om weer te geven";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 207;
            this.label2.Text = "Periode-Code";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormVatDeclaration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(732, 584);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TvwBtwAangiftes);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.btnInitialiseren);
            this.Controls.Add(this.btnIntervat2025);
            this.Controls.Add(this.tbMailBtw);
            this.Controls.Add(this.txtPeriodeNr);
            this.Controls.Add(this.txtPeriodeTot);
            this.Controls.Add(this.lblAktievePeriodeTot);
            this.Controls.Add(this.lblAankopen);
            this.Controls.Add(this.lblVerkopen);
            this.Controls.Add(this.lblDoc0);
            this.Controls.Add(this.lblDoc1);
            this.Controls.Add(this.lblDoc2);
            this.Controls.Add(this.lblDoc3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVatDeclaration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BTW Aangifte België (Model EDIFACT X400)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormVatDeclaration_FormClosed);
            this.Load += new System.EventHandler(this.FormVatDeclaration_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TreeView TvwBtwAangiftes;
        private System.Windows.Forms.TabControl    tabControl1;
        private System.Windows.Forms.TabPage       tabPage2;
        private System.Windows.Forms.TabPage       tabPage3;
        private System.Windows.Forms.RichTextBox   richTextBox1;
        private System.Windows.Forms.RichTextBox   richTextBox2;
        private System.Windows.Forms.CheckBox      cbAanvraagTerugbetaling;
        private System.Windows.Forms.CheckBox      cbAanvraagBetaalformulieren;
        private System.Windows.Forms.CheckBox      cbVergrendel;
        private System.Windows.Forms.CheckBox      cbRestitution2025;
        private System.Windows.Forms.CheckBox      cbPayment2025;
        private System.Windows.Forms.Button        btnXml2025;
        private System.Windows.Forms.Button        btnInitialiseren;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button        btnIntervat2025;
        private System.Windows.Forms.TextBox       tbMailBtw;
        private System.Windows.Forms.TextBox       txtPeriodeNr;
        private System.Windows.Forms.TextBox       txtPeriodeTot;
        private System.Windows.Forms.Label         lblAktievePeriodeTot;
        private System.Windows.Forms.Label         lblAankopen;
        private System.Windows.Forms.Label         lblVerkopen;
        private System.Windows.Forms.Label         lblDoc0;
        private System.Windows.Forms.Label         lblDoc1;
        private System.Windows.Forms.Label         lblDoc2;
        private System.Windows.Forms.Label         lblDoc3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
