namespace marVSS2028.SharedForms
{
    partial class DetailInfo
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip toolTip1;

        private System.Windows.Forms.Button cmdBank1;
        private System.Windows.Forms.Button cmdBank0;
        private System.Windows.Forms.TextBox tbBank1;
        private System.Windows.Forms.TextBox tbBank0;
        private System.Windows.Forms.Button Balans;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.TextBox TekstInfo0;
        private System.Windows.Forms.TextBox TekstInfo2;
        private System.Windows.Forms.TextBox TekstInfo1;
        private System.Windows.Forms.TextBox TekstInfo3;
        private System.Windows.Forms.TextBox TekstInfo5;
        private System.Windows.Forms.CheckBox Bewerking;
        private System.Windows.Forms.CheckBox Dokument;
        private System.Windows.Forms.CheckBox Partij;
        private System.Windows.Forms.Button ZoekDokument;
        private System.Windows.Forms.GroupBox Shape1;
        private System.Windows.Forms.Label LabelInfo5;
        private System.Windows.Forms.Label LabelInfo0;
        private System.Windows.Forms.Label LabelInfo6;
        private System.Windows.Forms.Label LabelInfo2;
        private System.Windows.Forms.Label LabelInfo1;
        private System.Windows.Forms.Label LabelInfo3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdBank1 = new System.Windows.Forms.Button();
            this.cmdBank0 = new System.Windows.Forms.Button();
            this.tbBank1 = new System.Windows.Forms.TextBox();
            this.tbBank0 = new System.Windows.Forms.TextBox();
            this.Balans = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.TekstInfo0 = new System.Windows.Forms.TextBox();
            this.TekstInfo2 = new System.Windows.Forms.TextBox();
            this.TekstInfo1 = new System.Windows.Forms.TextBox();
            this.TekstInfo3 = new System.Windows.Forms.TextBox();
            this.TekstInfo5 = new System.Windows.Forms.TextBox();
            this.Bewerking = new System.Windows.Forms.CheckBox();
            this.Dokument = new System.Windows.Forms.CheckBox();
            this.Partij = new System.Windows.Forms.CheckBox();
            this.ZoekDokument = new System.Windows.Forms.Button();
            this.Shape1 = new System.Windows.Forms.GroupBox();
            this.LabelInfo5 = new System.Windows.Forms.Label();
            this.LabelInfo0 = new System.Windows.Forms.Label();
            this.LabelInfo6 = new System.Windows.Forms.Label();
            this.LabelInfo2 = new System.Windows.Forms.Label();
            this.LabelInfo1 = new System.Windows.Forms.Label();
            this.LabelInfo3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdBank1
            // 
            this.cmdBank1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmdBank1.Location = new System.Drawing.Point(332, 124);
            this.cmdBank1.Name = "cmdBank1";
            this.cmdBank1.Size = new System.Drawing.Size(21, 21);
            this.cmdBank1.TabIndex = 21;
            this.cmdBank1.TabStop = false;
            this.cmdBank1.Text = "...";
            this.toolTip1.SetToolTip(this.cmdBank1, "Sepa Test");
            this.cmdBank1.UseVisualStyleBackColor = true;
            this.cmdBank1.Click += new System.EventHandler(this.CmdBank_Click);
            // 
            // cmdBank0
            // 
            this.cmdBank0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmdBank0.Location = new System.Drawing.Point(156, 124);
            this.cmdBank0.Name = "cmdBank0";
            this.cmdBank0.Size = new System.Drawing.Size(21, 21);
            this.cmdBank0.TabIndex = 20;
            this.cmdBank0.TabStop = false;
            this.cmdBank0.Text = "...";
            this.toolTip1.SetToolTip(this.cmdBank0, "Sepa Test");
            this.cmdBank0.UseVisualStyleBackColor = true;
            this.cmdBank0.Click += new System.EventHandler(this.CmdBank_Click);
            // 
            // tbBank1
            // 
            this.tbBank1.BackColor = System.Drawing.Color.White;
            this.tbBank1.Enabled = false;
            this.tbBank1.Location = new System.Drawing.Point(180, 124);
            this.tbBank1.Name = "tbBank1";
            this.tbBank1.Size = new System.Drawing.Size(148, 20);
            this.tbBank1.TabIndex = 19;
            this.tbBank1.TabStop = false;
            // 
            // tbBank0
            // 
            this.tbBank0.BackColor = System.Drawing.Color.White;
            this.tbBank0.Enabled = false;
            this.tbBank0.Location = new System.Drawing.Point(4, 124);
            this.tbBank0.Name = "tbBank0";
            this.tbBank0.Size = new System.Drawing.Size(148, 20);
            this.tbBank0.TabIndex = 18;
            this.tbBank0.TabStop = false;
            // 
            // Balans
            // 
            this.Balans.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Balans.Location = new System.Drawing.Point(6, 6);
            this.Balans.Name = "Balans";
            this.Balans.Size = new System.Drawing.Size(121, 25);
            this.Balans.TabIndex = 0;
            this.Balans.Text = "&Balanscontrole";
            this.Balans.UseVisualStyleBackColor = true;
            this.Balans.Click += new System.EventHandler(this.Balans_Click);
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.OK.Location = new System.Drawing.Point(360, 4);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(76, 22);
            this.OK.TabIndex = 9;
            this.OK.Text = "&Ok";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.Ok_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ButtonClose.Location = new System.Drawing.Point(360, 96);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(76, 22);
            this.ButtonClose.TabIndex = 14;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // TekstInfo0
            // 
            this.TekstInfo0.BackColor = System.Drawing.Color.White;
            this.TekstInfo0.Location = new System.Drawing.Point(260, 18);
            this.TekstInfo0.Name = "TekstInfo0";
            this.TekstInfo0.Size = new System.Drawing.Size(93, 20);
            this.TekstInfo0.TabIndex = 5;
            this.TekstInfo0.GotFocus += new System.EventHandler(this.TekstInfo_GotFocus);
            this.TekstInfo0.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TekstInfo_KeyDown);
            this.TekstInfo0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TekstInfo_KeyPress);
            this.TekstInfo0.LostFocus += new System.EventHandler(this.TekstInfo_LostFocus);
            // 
            // TekstInfo2
            // 
            this.TekstInfo2.BackColor = System.Drawing.Color.White;
            this.TekstInfo2.Location = new System.Drawing.Point(244, 58);
            this.TekstInfo2.Name = "TekstInfo2";
            this.TekstInfo2.Size = new System.Drawing.Size(109, 20);
            this.TekstInfo2.TabIndex = 6;
            this.TekstInfo2.GotFocus += new System.EventHandler(this.TekstInfo_GotFocus);
            this.TekstInfo2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TekstInfo_KeyDown);
            this.TekstInfo2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TekstInfo_KeyPress);
            this.TekstInfo2.LostFocus += new System.EventHandler(this.TekstInfo_LostFocus);
            // 
            // TekstInfo1
            // 
            this.TekstInfo1.BackColor = System.Drawing.Color.White;
            this.TekstInfo1.Location = new System.Drawing.Point(244, 78);
            this.TekstInfo1.Name = "TekstInfo1";
            this.TekstInfo1.Size = new System.Drawing.Size(109, 20);
            this.TekstInfo1.TabIndex = 7;
            this.TekstInfo1.GotFocus += new System.EventHandler(this.TekstInfo_GotFocus);
            this.TekstInfo1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TekstInfo_KeyDown);
            this.TekstInfo1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TekstInfo_KeyPress);
            this.TekstInfo1.LostFocus += new System.EventHandler(this.TekstInfo_LostFocus);
            // 
            // TekstInfo3
            // 
            this.TekstInfo3.BackColor = System.Drawing.Color.White;
            this.TekstInfo3.Location = new System.Drawing.Point(144, 100);
            this.TekstInfo3.Name = "TekstInfo3";
            this.TekstInfo3.Size = new System.Drawing.Size(209, 20);
            this.TekstInfo3.TabIndex = 8;
            this.TekstInfo3.GotFocus += new System.EventHandler(this.TekstInfo_GotFocus);
            this.TekstInfo3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TekstInfo_KeyDown);
            this.TekstInfo3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TekstInfo_KeyPress);
            this.TekstInfo3.LostFocus += new System.EventHandler(this.TekstInfo_LostFocus);
            // 
            // TekstInfo5
            // 
            this.TekstInfo5.BackColor = System.Drawing.Color.White;
            this.TekstInfo5.Location = new System.Drawing.Point(140, 18);
            this.TekstInfo5.Name = "TekstInfo5";
            this.TekstInfo5.Size = new System.Drawing.Size(116, 20);
            this.TekstInfo5.TabIndex = 4;
            this.TekstInfo5.GotFocus += new System.EventHandler(this.TekstInfo_GotFocus);
            this.TekstInfo5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TekstInfo_KeyDown);
            this.TekstInfo5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TekstInfo_KeyPress);
            this.TekstInfo5.LostFocus += new System.EventHandler(this.TekstInfo_LostFocus);
            // 
            // Bewerking
            // 
            this.Bewerking.AutoSize = true;
            this.Bewerking.Checked = true;
            this.Bewerking.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Bewerking.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bewerking.Location = new System.Drawing.Point(14, 40);
            this.Bewerking.Name = "Bewerking";
            this.Bewerking.Size = new System.Drawing.Size(84, 17);
            this.Bewerking.TabIndex = 1;
            this.Bewerking.Text = "= &Ontvangst";
            this.Bewerking.UseVisualStyleBackColor = true;
            this.Bewerking.CheckedChanged += new System.EventHandler(this.Bewerking_CheckedChanged);
            // 
            // Dokument
            // 
            this.Dokument.AutoSize = true;
            this.Dokument.Checked = true;
            this.Dokument.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Dokument.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dokument.Location = new System.Drawing.Point(14, 56);
            this.Dokument.Name = "Dokument";
            this.Dokument.Size = new System.Drawing.Size(84, 17);
            this.Dokument.TabIndex = 2;
            this.Dokument.Text = "= &Document";
            this.Dokument.UseVisualStyleBackColor = true;
            this.Dokument.CheckedChanged += new System.EventHandler(this.Dokument_CheckedChanged);
            this.Dokument.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Dokument_KeyPress);
            // 
            // Partij
            // 
            this.Partij.AutoSize = true;
            this.Partij.Checked = true;
            this.Partij.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Partij.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Partij.Location = new System.Drawing.Point(14, 72);
            this.Partij.Name = "Partij";
            this.Partij.Size = new System.Drawing.Size(59, 17);
            this.Partij.TabIndex = 3;
            this.Partij.Text = "= &Klant";
            this.Partij.UseVisualStyleBackColor = true;
            this.Partij.CheckedChanged += new System.EventHandler(this.Partij_CheckedChanged);
            // 
            // ZoekDokument
            // 
            this.ZoekDokument.BackColor = System.Drawing.Color.Silver;
            this.ZoekDokument.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ZoekDokument.Location = new System.Drawing.Point(216, 176);
            this.ZoekDokument.Name = "ZoekDokument";
            this.ZoekDokument.Size = new System.Drawing.Size(128, 16);
            this.ZoekDokument.TabIndex = 17;
            this.ZoekDokument.TabStop = false;
            this.ZoekDokument.Text = "ZoekDokument";
            this.ZoekDokument.UseVisualStyleBackColor = false;
            this.ZoekDokument.Visible = false;
            this.ZoekDokument.Click += new System.EventHandler(this.ZoekDokument_Click);
            // 
            // Shape1
            // 
            this.Shape1.Location = new System.Drawing.Point(8, 36);
            this.Shape1.Name = "Shape1";
            this.Shape1.Size = new System.Drawing.Size(120, 56);
            this.Shape1.TabIndex = 22;
            this.Shape1.TabStop = false;
            // 
            // LabelInfo5
            // 
            this.LabelInfo5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo5.Location = new System.Drawing.Point(140, 0);
            this.LabelInfo5.Name = "LabelInfo5";
            this.LabelInfo5.Size = new System.Drawing.Size(82, 18);
            this.LabelInfo5.TabIndex = 3;
            this.LabelInfo5.Text = "Doc. nummer";
            // 
            // LabelInfo0
            // 
            this.LabelInfo0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo0.Location = new System.Drawing.Point(260, 0);
            this.LabelInfo0.Name = "LabelInfo0";
            this.LabelInfo0.Size = new System.Drawing.Size(95, 18);
            this.LabelInfo0.TabIndex = 5;
            this.LabelInfo0.Text = "&TegenRekening";
            // 
            // LabelInfo6
            // 
            this.LabelInfo6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.LabelInfo6.Location = new System.Drawing.Point(140, 38);
            this.LabelInfo6.Name = "LabelInfo6";
            this.LabelInfo6.Size = new System.Drawing.Size(213, 20);
            this.LabelInfo6.TabIndex = 15;
            // 
            // LabelInfo2
            // 
            this.LabelInfo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo2.Location = new System.Drawing.Point(142, 60);
            this.LabelInfo2.Name = "LabelInfo2";
            this.LabelInfo2.Size = new System.Drawing.Size(95, 17);
            this.LabelInfo2.TabIndex = 7;
            this.LabelInfo2.Text = "Betaling";
            // 
            // LabelInfo1
            // 
            this.LabelInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo1.Location = new System.Drawing.Point(142, 80);
            this.LabelInfo1.Name = "LabelInfo1";
            this.LabelInfo1.Size = new System.Drawing.Size(95, 17);
            this.LabelInfo1.TabIndex = 9;
            this.LabelInfo1.Text = "Financ. Korting";
            // 
            // LabelInfo3
            // 
            this.LabelInfo3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelInfo3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInfo3.Location = new System.Drawing.Point(56, 100);
            this.LabelInfo3.Name = "LabelInfo3";
            this.LabelInfo3.Size = new System.Drawing.Size(79, 17);
            this.LabelInfo3.TabIndex = 11;
            this.LabelInfo3.Text = "Omschrijvin&G";
            // 
            // DetailInfo
            // 
            this.AcceptButton = this.Balans;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(442, 152);
            this.ControlBox = false;
            this.Controls.Add(this.LabelInfo3);
            this.Controls.Add(this.LabelInfo1);
            this.Controls.Add(this.LabelInfo2);
            this.Controls.Add(this.LabelInfo6);
            this.Controls.Add(this.LabelInfo0);
            this.Controls.Add(this.LabelInfo5);
            this.Controls.Add(this.ZoekDokument);
            this.Controls.Add(this.Partij);
            this.Controls.Add(this.Dokument);
            this.Controls.Add(this.Bewerking);
            this.Controls.Add(this.TekstInfo5);
            this.Controls.Add(this.TekstInfo3);
            this.Controls.Add(this.TekstInfo1);
            this.Controls.Add(this.TekstInfo2);
            this.Controls.Add(this.TekstInfo0);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Balans);
            this.Controls.Add(this.tbBank0);
            this.Controls.Add(this.tbBank1);
            this.Controls.Add(this.cmdBank0);
            this.Controls.Add(this.cmdBank1);
            this.Controls.Add(this.Shape1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DetailInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
