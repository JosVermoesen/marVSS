namespace marVSS2028.MimMenu.Filing
{
    partial class FormVariousDataSheets
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
            this.CmbDokumentType = new System.Windows.Forms.ComboBox();
            this.TxtKey = new System.Windows.Forms.TextBox();
            this.BtnPrev = new System.Windows.Forms.Button();
            this.BtnNext = new System.Windows.Forms.Button();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.BtnNewSheet = new System.Windows.Forms.Button();
            this.BtnEdit = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.ButtonMinimize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CmbDokumentType
            // 
            this.CmbDokumentType.BackColor = System.Drawing.SystemColors.Control;
            this.CmbDokumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbDokumentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbDokumentType.Location = new System.Drawing.Point(6, 6);
            this.CmbDokumentType.Name = "CmbDokumentType";
            this.CmbDokumentType.Size = new System.Drawing.Size(364, 21);
            this.CmbDokumentType.TabIndex = 0;
            this.CmbDokumentType.SelectedIndexChanged += new System.EventHandler(this.CmbDokumentType_SelectedIndexChanged);
            // 
            // TxtKey
            // 
            this.TxtKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtKey.Location = new System.Drawing.Point(6, 33);
            this.TxtKey.MaxLength = 18;
            this.TxtKey.Name = "TxtKey";
            this.TxtKey.Size = new System.Drawing.Size(198, 20);
            this.TxtKey.TabIndex = 8;
            this.TxtKey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtKey_KeyPress);
            this.TxtKey.Leave += new System.EventHandler(this.TxtKey_Leave);
            // 
            // BtnPrev
            // 
            this.BtnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrev.Location = new System.Drawing.Point(238, 33);
            this.BtnPrev.Name = "BtnPrev";
            this.BtnPrev.Size = new System.Drawing.Size(42, 28);
            this.BtnPrev.TabIndex = 4;
            this.BtnPrev.TabStop = false;
            this.BtnPrev.Text = "&<<";
            this.BtnPrev.Click += new System.EventHandler(this.BtnPrev_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNext.Location = new System.Drawing.Point(284, 33);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(42, 28);
            this.BtnNext.TabIndex = 5;
            this.BtnNext.TabStop = false;
            this.BtnNext.Text = ">&>";
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnPrint
            // 
            this.BtnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrint.Location = new System.Drawing.Point(330, 33);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(40, 28);
            this.BtnPrint.TabIndex = 1;
            this.BtnPrint.Text = "...";
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnNewSheet
            // 
            this.BtnNewSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNewSheet.Location = new System.Drawing.Point(238, 67);
            this.BtnNewSheet.Name = "BtnNewSheet";
            this.BtnNewSheet.Size = new System.Drawing.Size(130, 28);
            this.BtnNewSheet.TabIndex = 2;
            this.BtnNewSheet.TabStop = false;
            this.BtnNewSheet.Text = "&Andere Fiche";
            this.BtnNewSheet.Click += new System.EventHandler(this.BtnNewSheet_Click);
            // 
            // BtnEdit
            // 
            this.BtnEdit.Enabled = false;
            this.BtnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEdit.Location = new System.Drawing.Point(6, 67);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(82, 28);
            this.BtnEdit.TabIndex = 6;
            this.BtnEdit.TabStop = false;
            this.BtnEdit.Text = "&Editeren";
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Enabled = false;
            this.BtnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSave.Location = new System.Drawing.Point(93, 67);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(118, 28);
            this.BtnSave.TabIndex = 7;
            this.BtnSave.TabStop = false;
            this.BtnSave.Text = "&Wegschrijven";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // ButtonMinimize
            // 
            this.ButtonMinimize.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonMinimize.Location = new System.Drawing.Point(12, 101);
            this.ButtonMinimize.Name = "ButtonMinimize";
            this.ButtonMinimize.Size = new System.Drawing.Size(93, 23);
            this.ButtonMinimize.TabIndex = 9;
            this.ButtonMinimize.Text = "&Minimaliseren";
            this.ButtonMinimize.UseVisualStyleBackColor = true;
            this.ButtonMinimize.Click += new System.EventHandler(this.ButtonMinimize_Click);
            // 
            // FormVariousDataSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonMinimize;
            this.ClientSize = new System.Drawing.Size(380, 136);
            this.Controls.Add(this.ButtonMinimize);
            this.Controls.Add(this.CmbDokumentType);
            this.Controls.Add(this.TxtKey);
            this.Controls.Add(this.BtnPrev);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.BtnPrint);
            this.Controls.Add(this.BtnNewSheet);
            this.Controls.Add(this.BtnEdit);
            this.Controls.Add(this.BtnSave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVariousDataSheets";
            this.Text = "xDokument";
            this.Load += new System.EventHandler(this.FormVariousDataSheets_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormVariousDataSheets_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox    CmbDokumentType;
        private System.Windows.Forms.TextBox     TxtKey;
        private System.Windows.Forms.Button      BtnPrev;
        private System.Windows.Forms.Button      BtnNext;
        private System.Windows.Forms.Button      BtnPrint;
        private System.Windows.Forms.Button      BtnNewSheet;
        private System.Windows.Forms.Button      BtnEdit;
        private System.Windows.Forms.Button      BtnSave;
        private System.Windows.Forms.Button ButtonMinimize;
    }
}
