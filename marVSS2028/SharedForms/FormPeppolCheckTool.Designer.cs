namespace marVSS2028.SharedForms
{
    partial class FormPeppolCheckTool
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.TextBoxSupportedDocuments = new System.Windows.Forms.TextBox();
            this.mfgLijst = new System.Windows.Forms.DataGridView();
            this.CheckBoxOnlyRecent = new System.Windows.Forms.CheckBox();
            this.cbCopyToClipBoard = new System.Windows.Forms.Button();
            this.cbCheckAllPartners = new System.Windows.Forms.Button();
            this.cbCheckPeppolRegistration = new System.Windows.Forms.Button();
            this.tbPeppolID = new System.Windows.Forms.TextBox();
            this.cbCheckCompanyNumber = new System.Windows.Forms.Button();
            this.tbCompanyNumber = new System.Windows.Forms.TextBox();
            this.cbCheckVatNumber = new System.Windows.Forms.Button();
            this.tbVatNumber = new System.Windows.Forms.TextBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.lblPeppolId = new System.Windows.Forms.Label();
            this.lblCompanyNumber = new System.Windows.Forms.Label();
            this.lblVatNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mfgLijst)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBoxSupportedDocuments
            // 
            this.TextBoxSupportedDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSupportedDocuments.Location = new System.Drawing.Point(160, 88);
            this.TextBoxSupportedDocuments.Multiline = true;
            this.TextBoxSupportedDocuments.Name = "TextBoxSupportedDocuments";
            this.TextBoxSupportedDocuments.Size = new System.Drawing.Size(81, 33);
            this.TextBoxSupportedDocuments.TabIndex = 14;
            this.TextBoxSupportedDocuments.Visible = false;
            // 
            // mfgLijst
            // 
            this.mfgLijst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mfgLijst.AllowUserToAddRows = false;
            this.mfgLijst.AllowUserToDeleteRows = false;
            this.mfgLijst.AllowUserToResizeRows = false;
            this.mfgLijst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mfgLijst.Location = new System.Drawing.Point(8, 48);
            this.mfgLijst.MultiSelect = false;
            this.mfgLijst.Name = "mfgLijst";
            this.mfgLijst.ReadOnly = true;
            this.mfgLijst.RowHeadersVisible = false;
            this.mfgLijst.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mfgLijst.Size = new System.Drawing.Size(381, 393);
            this.mfgLijst.TabIndex = 10;
            this.mfgLijst.Visible = false;
            // 
            // CheckBoxOnlyRecent
            // 
            this.CheckBoxOnlyRecent.AutoSize = true;
            this.CheckBoxOnlyRecent.Checked = true;
            this.CheckBoxOnlyRecent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxOnlyRecent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxOnlyRecent.Location = new System.Drawing.Point(135, 17);
            this.CheckBoxOnlyRecent.Name = "CheckBoxOnlyRecent";
            this.CheckBoxOnlyRecent.Size = new System.Drawing.Size(155, 17);
            this.CheckBoxOnlyRecent.TabIndex = 13;
            this.CheckBoxOnlyRecent.Text = "met facturatie recente jaren";
            this.CheckBoxOnlyRecent.UseVisualStyleBackColor = true;
            // 
            // cbCopyToClipBoard
            // 
            this.cbCopyToClipBoard.Enabled = false;
            this.cbCopyToClipBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCopyToClipBoard.Location = new System.Drawing.Point(304, 8);
            this.cbCopyToClipBoard.Name = "cbCopyToClipBoard";
            this.cbCopyToClipBoard.Size = new System.Drawing.Size(81, 33);
            this.cbCopyToClipBoard.TabIndex = 11;
            this.cbCopyToClipBoard.TabStop = false;
            this.cbCopyToClipBoard.Text = "Kopie";
            this.cbCopyToClipBoard.UseVisualStyleBackColor = true;
            this.cbCopyToClipBoard.Click += new System.EventHandler(this.cbCopyToClipBoard_Click);
            // 
            // cbCheckAllPartners
            // 
            this.cbCheckAllPartners.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckAllPartners.Location = new System.Drawing.Point(8, 9);
            this.cbCheckAllPartners.Name = "cbCheckAllPartners";
            this.cbCheckAllPartners.Size = new System.Drawing.Size(121, 33);
            this.cbCheckAllPartners.TabIndex = 9;
            this.cbCheckAllPartners.TabStop = false;
            this.cbCheckAllPartners.Text = "Partijen &B2B oplijsten";
            this.cbCheckAllPartners.UseVisualStyleBackColor = true;
            this.cbCheckAllPartners.Click += new System.EventHandler(this.cbCheckAllPartners_Click);
            // 
            // cbCheckPeppolRegistration
            // 
            this.cbCheckPeppolRegistration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckPeppolRegistration.Location = new System.Drawing.Point(232, 160);
            this.cbCheckPeppolRegistration.Name = "cbCheckPeppolRegistration";
            this.cbCheckPeppolRegistration.Size = new System.Drawing.Size(145, 25);
            this.cbCheckPeppolRegistration.TabIndex = 7;
            this.cbCheckPeppolRegistration.TabStop = false;
            this.cbCheckPeppolRegistration.Text = "Controle &Peppol Registratie";
            this.cbCheckPeppolRegistration.UseVisualStyleBackColor = true;
            this.cbCheckPeppolRegistration.Click += new System.EventHandler(this.cbCheckPeppolRegistration_Click);
            // 
            // tbPeppolID
            // 
            this.tbPeppolID.Location = new System.Drawing.Point(8, 160);
            this.tbPeppolID.Name = "tbPeppolID";
            this.tbPeppolID.Size = new System.Drawing.Size(217, 20);
            this.tbPeppolID.TabIndex = 2;
            // 
            // cbCheckCompanyNumber
            // 
            this.cbCheckCompanyNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckCompanyNumber.Location = new System.Drawing.Point(232, 64);
            this.cbCheckCompanyNumber.Name = "cbCheckCompanyNumber";
            this.cbCheckCompanyNumber.Size = new System.Drawing.Size(145, 25);
            this.cbCheckCompanyNumber.TabIndex = 5;
            this.cbCheckCompanyNumber.TabStop = false;
            this.cbCheckCompanyNumber.Text = "&Onderneming Opzoeken";
            this.cbCheckCompanyNumber.UseVisualStyleBackColor = true;
            this.cbCheckCompanyNumber.Click += new System.EventHandler(this.cbCheckCompanyNumber_Click);
            // 
            // tbCompanyNumber
            // 
            this.tbCompanyNumber.Location = new System.Drawing.Point(8, 64);
            this.tbCompanyNumber.Name = "tbCompanyNumber";
            this.tbCompanyNumber.Size = new System.Drawing.Size(217, 20);
            this.tbCompanyNumber.TabIndex = 0;
            // 
            // cbCheckVatNumber
            // 
            this.cbCheckVatNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCheckVatNumber.Location = new System.Drawing.Point(232, 112);
            this.cbCheckVatNumber.Name = "cbCheckVatNumber";
            this.cbCheckVatNumber.Size = new System.Drawing.Size(145, 25);
            this.cbCheckVatNumber.TabIndex = 3;
            this.cbCheckVatNumber.TabStop = false;
            this.cbCheckVatNumber.Text = "&Btw Nummer Opzoeken";
            this.cbCheckVatNumber.UseVisualStyleBackColor = true;
            this.cbCheckVatNumber.Click += new System.EventHandler(this.cbCheckVatNumber_Click);
            // 
            // tbVatNumber
            // 
            this.tbVatNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVatNumber.Location = new System.Drawing.Point(8, 112);
            this.tbVatNumber.Name = "tbVatNumber";
            this.tbVatNumber.Size = new System.Drawing.Size(217, 20);
            this.tbVatNumber.TabIndex = 1;
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonClose.Location = new System.Drawing.Point(29, 13);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(81, 25);
            this.ButtonClose.TabIndex = 12;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Text = "ButtonClose";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // lblPeppolId
            // 
            this.lblPeppolId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeppolId.Location = new System.Drawing.Point(16, 144);
            this.lblPeppolId.Name = "lblPeppolId";
            this.lblPeppolId.Size = new System.Drawing.Size(105, 17);
            this.lblPeppolId.TabIndex = 8;
            this.lblPeppolId.Text = "Peppol ID (Europa)";
            // 
            // lblCompanyNumber
            // 
            this.lblCompanyNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyNumber.Location = new System.Drawing.Point(16, 48);
            this.lblCompanyNumber.Name = "lblCompanyNumber";
            this.lblCompanyNumber.Size = new System.Drawing.Size(185, 17);
            this.lblCompanyNumber.TabIndex = 6;
            this.lblCompanyNumber.Text = "Ondernemingsnummer (Enkel België!)";
            // 
            // lblVatNumber
            // 
            this.lblVatNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVatNumber.Location = new System.Drawing.Point(16, 96);
            this.lblVatNumber.Name = "lblVatNumber";
            this.lblVatNumber.Size = new System.Drawing.Size(113, 17);
            this.lblVatNumber.TabIndex = 4;
            this.lblVatNumber.Text = "Btw Nummer (Europa)";
            // 
            // FormPeppolCheckTool
            // 
            this.AcceptButton = this.ButtonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(398, 191);
            this.ControlBox = false;
            this.Controls.Add(this.TextBoxSupportedDocuments);
            this.Controls.Add(this.mfgLijst);
            this.Controls.Add(this.CheckBoxOnlyRecent);
            this.Controls.Add(this.cbCopyToClipBoard);
            this.Controls.Add(this.cbCheckAllPartners);
            this.Controls.Add(this.cbCheckPeppolRegistration);
            this.Controls.Add(this.tbPeppolID);
            this.Controls.Add(this.cbCheckCompanyNumber);
            this.Controls.Add(this.tbCompanyNumber);
            this.Controls.Add(this.cbCheckVatNumber);
            this.Controls.Add(this.tbVatNumber);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.lblPeppolId);
            this.Controls.Add(this.lblCompanyNumber);
            this.Controls.Add(this.lblVatNumber);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormPeppolCheckTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Peppol Tools";
            this.Load += new System.EventHandler(this.FormPeppolCheckTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mfgLijst)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxSupportedDocuments;
        private System.Windows.Forms.DataGridView mfgLijst;
        private System.Windows.Forms.CheckBox CheckBoxOnlyRecent;
        private System.Windows.Forms.Button cbCopyToClipBoard;
        private System.Windows.Forms.Button cbCheckAllPartners;
        private System.Windows.Forms.Button cbCheckPeppolRegistration;
        private System.Windows.Forms.TextBox tbPeppolID;
        private System.Windows.Forms.Button cbCheckCompanyNumber;
        private System.Windows.Forms.TextBox tbCompanyNumber;
        private System.Windows.Forms.Button cbCheckVatNumber;
        private System.Windows.Forms.TextBox tbVatNumber;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label lblPeppolId;
        private System.Windows.Forms.Label lblCompanyNumber;
        private System.Windows.Forms.Label lblVatNumber;
    }
}
