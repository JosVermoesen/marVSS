namespace marVSS2028.MimMenu.DailyManagement
{
    partial class FormOGM
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Control declarations
        private System.Windows.Forms.ComboBox ComboGuidLabel;
        private System.Windows.Forms.GroupBox FrameChangeMemoDate;
        private System.Windows.Forms.TextBox TbBedrag;
        private System.Windows.Forms.TextBox MebRekening;
        private System.Windows.Forms.DateTimePicker dtpMemoDatum;
        private System.Windows.Forms.Label lbMemoDatum;
        private System.Windows.Forms.Label lbReferte;
        private System.Windows.Forms.Label lbBedrag;
        private System.Windows.Forms.CheckBox CheckBoxBookyearXOnly;
        private System.Windows.Forms.CheckBox CheckBoxForceExecutionDate;
        private System.Windows.Forms.CheckBox cbLeveranciers;
        private System.Windows.Forms.CheckBox chkAfdrukInVenster;
        private System.Windows.Forms.ComboBox ComboBoxSelectedBancAccount;
        private System.Windows.Forms.DataGridView FlexGridPayments;
        private System.Windows.Forms.Button CmdEmailNBB;
        private System.Windows.Forms.Button Samenstellen;
        private System.Windows.Forms.TextBox[] TekstLijn;
        private System.Windows.Forms.Button Drukken;
        private System.Windows.Forms.Button cmdSluiten;
        private System.Windows.Forms.DateTimePicker DatumVerwerking;
        private System.Windows.Forms.DateTimePicker DTPickerGlobalMemoDate;
        private System.Windows.Forms.DataGridView FlexGridGhost;
        private System.Windows.Forms.Label LabelMemoGroups;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label lblEUR;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label lbBank;
        private System.Windows.Forms.Label LabelGroupDate;
        private System.Windows.Forms.Label lbVanTot;

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
            this.ComboGuidLabel = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.lbBank = new System.Windows.Forms.Label();
            this.ComboBoxSelectedBancAccount = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.LabelMemoGroups = new System.Windows.Forms.Label();
            this.LabelGroupDate = new System.Windows.Forms.Label();
            this.DatumVerwerking = new System.Windows.Forms.DateTimePicker();
            this.CheckBoxForceExecutionDate = new System.Windows.Forms.CheckBox();
            this.DTPickerGlobalMemoDate = new System.Windows.Forms.DateTimePicker();
            this.CheckBoxBookyearXOnly = new System.Windows.Forms.CheckBox();
            this.cbLeveranciers = new System.Windows.Forms.CheckBox();
            this.chkAfdrukInVenster = new System.Windows.Forms.CheckBox();
            this.lbVanTot = new System.Windows.Forms.Label();
            this.Samenstellen = new System.Windows.Forms.Button();
            this.FlexGridPayments = new System.Windows.Forms.DataGridView();
            this.Drukken = new System.Windows.Forms.Button();
            this.CmdEmailNBB = new System.Windows.Forms.Button();
            this.cmdSluiten = new System.Windows.Forms.Button();
            this.FrameChangeMemoDate = new System.Windows.Forms.GroupBox();
            this.TbBedrag = new System.Windows.Forms.TextBox();
            this.MebRekening = new System.Windows.Forms.TextBox();
            this.dtpMemoDatum = new System.Windows.Forms.DateTimePicker();
            this.lbMemoDatum = new System.Windows.Forms.Label();
            this.lbReferte = new System.Windows.Forms.Label();
            this.lbBedrag = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.lblEUR = new System.Windows.Forms.Label();
            this.FlexGridGhost = new System.Windows.Forms.DataGridView();
            this.TextBoxFrom = new System.Windows.Forms.TextBox();
            this.TextBoxTo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.FlexGridPayments)).BeginInit();
            this.FrameChangeMemoDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FlexGridGhost)).BeginInit();
            this.SuspendLayout();
            // 
            // ComboGuidLabel
            // 
            this.ComboGuidLabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboGuidLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboGuidLabel.Location = new System.Drawing.Point(72, 4);
            this.ComboGuidLabel.Name = "ComboGuidLabel";
            this.ComboGuidLabel.Size = new System.Drawing.Size(253, 21);
            this.ComboGuidLabel.Sorted = true;
            this.ComboGuidLabel.TabIndex = 27;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(8, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(50, 13);
            this.Label1.TabIndex = 16;
            this.Label1.Text = "Groep ID";
            // 
            // lbBank
            // 
            this.lbBank.AutoSize = true;
            this.lbBank.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBank.Location = new System.Drawing.Point(468, 8);
            this.lbBank.Name = "lbBank";
            this.lbBank.Size = new System.Drawing.Size(32, 13);
            this.lbBank.TabIndex = 0;
            this.lbBank.Text = "Ban&k";
            // 
            // ComboBoxSelectedBancAccount
            // 
            this.ComboBoxSelectedBancAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxSelectedBancAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxSelectedBancAccount.Location = new System.Drawing.Point(512, 4);
            this.ComboBoxSelectedBancAccount.Name = "ComboBoxSelectedBancAccount";
            this.ComboBoxSelectedBancAccount.Size = new System.Drawing.Size(221, 21);
            this.ComboBoxSelectedBancAccount.TabIndex = 1;
            this.ComboBoxSelectedBancAccount.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSelectedBancAccount_SelectedIndexChanged);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(328, 8);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(78, 13);
            this.Label2.TabIndex = 28;
            this.Label2.Text = "Memogroepen:";
            // 
            // LabelMemoGroups
            // 
            this.LabelMemoGroups.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelMemoGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelMemoGroups.Location = new System.Drawing.Point(424, 8);
            this.LabelMemoGroups.Name = "LabelMemoGroups";
            this.LabelMemoGroups.Size = new System.Drawing.Size(29, 16);
            this.LabelMemoGroups.TabIndex = 29;
            this.LabelMemoGroups.Text = "0";
            this.LabelMemoGroups.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelGroupDate
            // 
            this.LabelGroupDate.AutoSize = true;
            this.LabelGroupDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelGroupDate.Location = new System.Drawing.Point(8, 32);
            this.LabelGroupDate.Name = "LabelGroupDate";
            this.LabelGroupDate.Size = new System.Drawing.Size(114, 13);
            this.LabelGroupDate.TabIndex = 6;
            this.LabelGroupDate.Text = "Datu&m van verwerking";
            // 
            // DatumVerwerking
            // 
            this.DatumVerwerking.CustomFormat = "dd/MM/yyyy";
            this.DatumVerwerking.Enabled = false;
            this.DatumVerwerking.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatumVerwerking.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DatumVerwerking.Location = new System.Drawing.Point(160, 32);
            this.DatumVerwerking.Name = "DatumVerwerking";
            this.DatumVerwerking.Size = new System.Drawing.Size(97, 20);
            this.DatumVerwerking.TabIndex = 13;
            // 
            // CheckBoxForceExecutionDate
            // 
            this.CheckBoxForceExecutionDate.AutoSize = true;
            this.CheckBoxForceExecutionDate.Checked = true;
            this.CheckBoxForceExecutionDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxForceExecutionDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxForceExecutionDate.Location = new System.Drawing.Point(8, 64);
            this.CheckBoxForceExecutionDate.Name = "CheckBoxForceExecutionDate";
            this.CheckBoxForceExecutionDate.Size = new System.Drawing.Size(115, 17);
            this.CheckBoxForceExecutionDate.TabIndex = 14;
            this.CheckBoxForceExecutionDate.TabStop = false;
            this.CheckBoxForceExecutionDate.Text = "Fixeer Memodatum";
            this.CheckBoxForceExecutionDate.CheckedChanged += new System.EventHandler(this.CheckBoxForceExecutionDate_CheckedChanged);
            // 
            // DTPickerGlobalMemoDate
            // 
            this.DTPickerGlobalMemoDate.CustomFormat = "dd/MM/yyyy";
            this.DTPickerGlobalMemoDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPickerGlobalMemoDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DTPickerGlobalMemoDate.Location = new System.Drawing.Point(160, 60);
            this.DTPickerGlobalMemoDate.Name = "DTPickerGlobalMemoDate";
            this.DTPickerGlobalMemoDate.Size = new System.Drawing.Size(97, 20);
            this.DTPickerGlobalMemoDate.TabIndex = 15;
            this.DTPickerGlobalMemoDate.ValueChanged += new System.EventHandler(this.DTPickerGlobalMemoDate_ValueChanged);
            // 
            // CheckBoxBookyearXOnly
            // 
            this.CheckBoxBookyearXOnly.AutoSize = true;
            this.CheckBoxBookyearXOnly.Checked = true;
            this.CheckBoxBookyearXOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxBookyearXOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxBookyearXOnly.Location = new System.Drawing.Point(8, 88);
            this.CheckBoxBookyearXOnly.Name = "CheckBoxBookyearXOnly";
            this.CheckBoxBookyearXOnly.Size = new System.Drawing.Size(126, 17);
            this.CheckBoxBookyearXOnly.TabIndex = 17;
            this.CheckBoxBookyearXOnly.TabStop = false;
            this.CheckBoxBookyearXOnly.Text = "Enkel actief boekjaar";
            // 
            // cbLeveranciers
            // 
            this.cbLeveranciers.AutoSize = true;
            this.cbLeveranciers.Checked = true;
            this.cbLeveranciers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLeveranciers.Enabled = false;
            this.cbLeveranciers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLeveranciers.Location = new System.Drawing.Point(420, 76);
            this.cbLeveranciers.Name = "cbLeveranciers";
            this.cbLeveranciers.Size = new System.Drawing.Size(117, 17);
            this.cbLeveranciers.TabIndex = 12;
            this.cbLeveranciers.Text = "Enkel Leveranciers";
            // 
            // chkAfdrukInVenster
            // 
            this.chkAfdrukInVenster.AutoSize = true;
            this.chkAfdrukInVenster.Checked = true;
            this.chkAfdrukInVenster.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAfdrukInVenster.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAfdrukInVenster.Location = new System.Drawing.Point(536, 80);
            this.chkAfdrukInVenster.Name = "chkAfdrukInVenster";
            this.chkAfdrukInVenster.Size = new System.Drawing.Size(73, 17);
            this.chkAfdrukInVenster.TabIndex = 11;
            this.chkAfdrukInVenster.Text = "In venster";
            this.chkAfdrukInVenster.Visible = false;
            // 
            // lbVanTot
            // 
            this.lbVanTot.AutoSize = true;
            this.lbVanTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVanTot.Location = new System.Drawing.Point(312, 32);
            this.lbVanTot.Name = "lbVanTot";
            this.lbVanTot.Size = new System.Drawing.Size(51, 13);
            this.lbVanTot.TabIndex = 2;
            this.lbVanTot.Text = "&Van - Tot";
            // 
            // Samenstellen
            // 
            this.Samenstellen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Samenstellen.Location = new System.Drawing.Point(308, 72);
            this.Samenstellen.Name = "Samenstellen";
            this.Samenstellen.Size = new System.Drawing.Size(88, 30);
            this.Samenstellen.TabIndex = 7;
            this.Samenstellen.TabStop = false;
            this.Samenstellen.Text = "&Samenstellen";
            this.Samenstellen.Click += new System.EventHandler(this.Samenstellen_Click);
            // 
            // FlexGridPayments
            // 
            this.FlexGridPayments.AllowUserToAddRows = false;
            this.FlexGridPayments.Location = new System.Drawing.Point(8, 108);
            this.FlexGridPayments.Name = "FlexGridPayments";
            this.FlexGridPayments.Size = new System.Drawing.Size(747, 237);
            this.FlexGridPayments.TabIndex = 5;
            // 
            // Drukken
            // 
            this.Drukken.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Drukken.Location = new System.Drawing.Point(536, 48);
            this.Drukken.Name = "Drukken";
            this.Drukken.Size = new System.Drawing.Size(77, 30);
            this.Drukken.TabIndex = 8;
            this.Drukken.TabStop = false;
            this.Drukken.Text = "Af&drukken";
            this.Drukken.Visible = false;
            // 
            // CmdEmailNBB
            // 
            this.CmdEmailNBB.Enabled = false;
            this.CmdEmailNBB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmdEmailNBB.Location = new System.Drawing.Point(624, 48);
            this.CmdEmailNBB.Name = "CmdEmailNBB";
            this.CmdEmailNBB.Size = new System.Drawing.Size(109, 49);
            this.CmdEmailNBB.TabIndex = 9;
            this.CmdEmailNBB.Text = "&Genereren";
            this.CmdEmailNBB.Click += new System.EventHandler(this.CmdEmailNBB_Click);
            // 
            // cmdSluiten
            // 
            this.cmdSluiten.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdSluiten.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSluiten.Location = new System.Drawing.Point(648, 400);
            this.cmdSluiten.Name = "cmdSluiten";
            this.cmdSluiten.Size = new System.Drawing.Size(93, 24);
            this.cmdSluiten.TabIndex = 10;
            this.cmdSluiten.TabStop = false;
            this.cmdSluiten.Text = "Sluiten";
            this.cmdSluiten.Click += new System.EventHandler(this.cmdSluiten_Click);
            // 
            // FrameChangeMemoDate
            // 
            this.FrameChangeMemoDate.Controls.Add(this.TbBedrag);
            this.FrameChangeMemoDate.Controls.Add(this.MebRekening);
            this.FrameChangeMemoDate.Controls.Add(this.dtpMemoDatum);
            this.FrameChangeMemoDate.Controls.Add(this.lbMemoDatum);
            this.FrameChangeMemoDate.Controls.Add(this.lbReferte);
            this.FrameChangeMemoDate.Controls.Add(this.lbBedrag);
            this.FrameChangeMemoDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrameChangeMemoDate.Location = new System.Drawing.Point(4, 352);
            this.FrameChangeMemoDate.Name = "FrameChangeMemoDate";
            this.FrameChangeMemoDate.Size = new System.Drawing.Size(537, 76);
            this.FrameChangeMemoDate.TabIndex = 18;
            this.FrameChangeMemoDate.TabStop = false;
            this.FrameChangeMemoDate.Text = "Controle op Memodatum";
            // 
            // TbBedrag
            // 
            this.TbBedrag.Enabled = false;
            this.TbBedrag.Location = new System.Drawing.Point(276, 44);
            this.TbBedrag.Name = "TbBedrag";
            this.TbBedrag.Size = new System.Drawing.Size(101, 20);
            this.TbBedrag.TabIndex = 19;
            this.TbBedrag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MebRekening
            // 
            this.MebRekening.Enabled = false;
            this.MebRekening.Location = new System.Drawing.Point(4, 44);
            this.MebRekening.Name = "MebRekening";
            this.MebRekening.Size = new System.Drawing.Size(177, 20);
            this.MebRekening.TabIndex = 20;
            // 
            // dtpMemoDatum
            // 
            this.dtpMemoDatum.CustomFormat = "dd/MM/yyyy";
            this.dtpMemoDatum.Enabled = false;
            this.dtpMemoDatum.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMemoDatum.Location = new System.Drawing.Point(184, 44);
            this.dtpMemoDatum.Name = "dtpMemoDatum";
            this.dtpMemoDatum.Size = new System.Drawing.Size(89, 20);
            this.dtpMemoDatum.TabIndex = 21;
            // 
            // lbMemoDatum
            // 
            this.lbMemoDatum.AutoSize = true;
            this.lbMemoDatum.Location = new System.Drawing.Point(188, 24);
            this.lbMemoDatum.Name = "lbMemoDatum";
            this.lbMemoDatum.Size = new System.Drawing.Size(65, 13);
            this.lbMemoDatum.TabIndex = 24;
            this.lbMemoDatum.Text = "&Memodatum";
            // 
            // lbReferte
            // 
            this.lbReferte.AutoSize = true;
            this.lbReferte.Location = new System.Drawing.Point(8, 24);
            this.lbReferte.Name = "lbReferte";
            this.lbReferte.Size = new System.Drawing.Size(42, 13);
            this.lbReferte.TabIndex = 23;
            this.lbReferte.Text = "&Referte";
            // 
            // lbBedrag
            // 
            this.lbBedrag.AutoSize = true;
            this.lbBedrag.Location = new System.Drawing.Point(320, 24);
            this.lbBedrag.Name = "lbBedrag";
            this.lbBedrag.Size = new System.Drawing.Size(41, 13);
            this.lbBedrag.TabIndex = 22;
            this.lbBedrag.Text = "&Bedrag";
            this.lbBedrag.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Label3
            // 
            this.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(548, 356);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(89, 21);
            this.Label3.TabIndex = 26;
            this.Label3.Text = "EUR Totaal";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEUR
            // 
            this.lblEUR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEUR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEUR.Location = new System.Drawing.Point(648, 356);
            this.lblEUR.Name = "lblEUR";
            this.lblEUR.Size = new System.Drawing.Size(93, 21);
            this.lblEUR.TabIndex = 25;
            this.lblEUR.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FlexGridGhost
            // 
            this.FlexGridGhost.AllowUserToAddRows = false;
            this.FlexGridGhost.Location = new System.Drawing.Point(24, 128);
            this.FlexGridGhost.Name = "FlexGridGhost";
            this.FlexGridGhost.Size = new System.Drawing.Size(711, 193);
            this.FlexGridGhost.TabIndex = 30;
            this.FlexGridGhost.Visible = false;
            // 
            // TextBoxFrom
            // 
            this.TextBoxFrom.Location = new System.Drawing.Point(308, 48);
            this.TextBoxFrom.Name = "TextBoxFrom";
            this.TextBoxFrom.Size = new System.Drawing.Size(100, 20);
            this.TextBoxFrom.TabIndex = 31;
            // 
            // TextBoxTo
            // 
            this.TextBoxTo.Location = new System.Drawing.Point(414, 48);
            this.TextBoxTo.Name = "TextBoxTo";
            this.TextBoxTo.Size = new System.Drawing.Size(100, 20);
            this.TextBoxTo.TabIndex = 32;
            // 
            // FormOGM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdSluiten;
            this.ClientSize = new System.Drawing.Size(761, 438);
            this.ControlBox = false;
            this.Controls.Add(this.TextBoxTo);
            this.Controls.Add(this.TextBoxFrom);
            this.Controls.Add(this.FlexGridGhost);
            this.Controls.Add(this.LabelMemoGroups);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lblEUR);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lbBank);
            this.Controls.Add(this.LabelGroupDate);
            this.Controls.Add(this.lbVanTot);
            this.Controls.Add(this.ComboBoxSelectedBancAccount);
            this.Controls.Add(this.ComboGuidLabel);
            this.Controls.Add(this.DatumVerwerking);
            this.Controls.Add(this.DTPickerGlobalMemoDate);
            this.Controls.Add(this.CheckBoxForceExecutionDate);
            this.Controls.Add(this.CheckBoxBookyearXOnly);
            this.Controls.Add(this.cbLeveranciers);
            this.Controls.Add(this.chkAfdrukInVenster);
            this.Controls.Add(this.Samenstellen);
            this.Controls.Add(this.FlexGridPayments);
            this.Controls.Add(this.Drukken);
            this.Controls.Add(this.CmdEmailNBB);
            this.Controls.Add(this.cmdSluiten);
            this.Controls.Add(this.FrameChangeMemoDate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOGM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Betalingen/Overschrijvingen";
            this.Load += new System.EventHandler(this.FormOGM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FlexGridPayments)).EndInit();
            this.FrameChangeMemoDate.ResumeLayout(false);
            this.FrameChangeMemoDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FlexGridGhost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxFrom;
        private System.Windows.Forms.TextBox TextBoxTo;
    }
}
