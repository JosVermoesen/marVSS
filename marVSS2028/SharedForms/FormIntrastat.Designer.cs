using System.Windows.Forms;

namespace marVSS2028.SharedForms
{
    partial class FormIntrastat
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
            this.InfoDataPanel = new System.Windows.Forms.Panel();
            this.Eenheden = new System.Windows.Forms.TextBox();
            this.InfoTekst = new System.Windows.Forms.TextBox();
            this.KeuzeOpties0 = new System.Windows.Forms.ComboBox();
            this.KeuzeOpties1 = new System.Windows.Forms.ComboBox();
            this.KeuzeOpties2 = new System.Windows.Forms.ComboBox();
            this.KeuzeOpties3 = new System.Windows.Forms.ComboBox();
            this.KeuzeOpties4 = new System.Windows.Forms.ComboBox();
            this.TekstInfo0 = new System.Windows.Forms.TextBox();
            this.TekstInfo1 = new System.Windows.Forms.TextBox();
            this.TekstInfo2 = new System.Windows.Forms.TextBox();
            this.TekstInfo3 = new System.Windows.Forms.TextBox();
            this.TekstInfo4 = new System.Windows.Forms.TextBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Annuleren = new System.Windows.Forms.Button();
            this.Label1_11 = new System.Windows.Forms.Label();
            this.Label1_9 = new System.Windows.Forms.Label();
            this.LabelA6 = new System.Windows.Forms.Label();
            this.Label1_1 = new System.Windows.Forms.Label();
            this.Label1_2 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label1_4 = new System.Windows.Forms.Label();
            this.Label1_5 = new System.Windows.Forms.Label();
            this.Label1_6 = new System.Windows.Forms.Label();
            this.Label1_7 = new System.Windows.Forms.Label();
            this.LabelB6 = new System.Windows.Forms.Label();
            this.Label1_10 = new System.Windows.Forms.Label();
            this.NogToeTeWijzen = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InfoDataPanel
            // 
            this.InfoDataPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoDataPanel.Location = new System.Drawing.Point(0, 227);
            this.InfoDataPanel.Name = "InfoDataPanel";
            this.InfoDataPanel.Size = new System.Drawing.Size(526, 22);
            this.InfoDataPanel.TabIndex = 27;
            // 
            // Eenheden
            // 
            this.Eenheden.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Eenheden.Location = new System.Drawing.Point(418, 104);
            this.Eenheden.Multiline = true;
            this.Eenheden.Name = "Eenheden";
            this.Eenheden.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Eenheden.Size = new System.Drawing.Size(103, 53);
            this.Eenheden.TabIndex = 25;
            this.Eenheden.TextChanged += new System.EventHandler(this.Eenheden_TextChanged);
            // 
            // InfoTekst
            // 
            this.InfoTekst.Location = new System.Drawing.Point(4, 160);
            this.InfoTekst.Multiline = true;
            this.InfoTekst.Name = "InfoTekst";
            this.InfoTekst.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InfoTekst.Size = new System.Drawing.Size(517, 65);
            this.InfoTekst.TabIndex = 24;
            // 
            // KeuzeOpties0
            // 
            this.KeuzeOpties0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeuzeOpties0.Enabled = false;
            this.KeuzeOpties0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeuzeOpties0.FormattingEnabled = true;
            this.KeuzeOpties0.Location = new System.Drawing.Point(4, 16);
            this.KeuzeOpties0.Name = "KeuzeOpties0";
            this.KeuzeOpties0.Size = new System.Drawing.Size(232, 21);
            this.KeuzeOpties0.TabIndex = 0;
            this.KeuzeOpties0.SelectedIndexChanged += new System.EventHandler(this.KeuzeOpties0_SelectedIndexChanged);
            // 
            // KeuzeOpties1
            // 
            this.KeuzeOpties1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeuzeOpties1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeuzeOpties1.FormattingEnabled = true;
            this.KeuzeOpties1.Location = new System.Drawing.Point(92, 36);
            this.KeuzeOpties1.Name = "KeuzeOpties1";
            this.KeuzeOpties1.Size = new System.Drawing.Size(197, 21);
            this.KeuzeOpties1.TabIndex = 2;
            this.KeuzeOpties1.SelectedIndexChanged += new System.EventHandler(this.KeuzeOpties1_SelectedIndexChanged);
            // 
            // KeuzeOpties2
            // 
            this.KeuzeOpties2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeuzeOpties2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeuzeOpties2.FormattingEnabled = true;
            this.KeuzeOpties2.Location = new System.Drawing.Point(92, 56);
            this.KeuzeOpties2.Name = "KeuzeOpties2";
            this.KeuzeOpties2.Size = new System.Drawing.Size(198, 21);
            this.KeuzeOpties2.TabIndex = 3;
            // 
            // KeuzeOpties3
            // 
            this.KeuzeOpties3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeuzeOpties3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeuzeOpties3.FormattingEnabled = true;
            this.KeuzeOpties3.Location = new System.Drawing.Point(92, 76);
            this.KeuzeOpties3.Name = "KeuzeOpties3";
            this.KeuzeOpties3.Size = new System.Drawing.Size(322, 21);
            this.KeuzeOpties3.TabIndex = 4;
            this.KeuzeOpties3.SelectedIndexChanged += new System.EventHandler(this.KeuzeOpties3_SelectedIndexChanged);
            // 
            // KeuzeOpties4
            // 
            this.KeuzeOpties4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeuzeOpties4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeuzeOpties4.FormattingEnabled = true;
            this.KeuzeOpties4.Location = new System.Drawing.Point(92, 96);
            this.KeuzeOpties4.Name = "KeuzeOpties4";
            this.KeuzeOpties4.Size = new System.Drawing.Size(322, 21);
            this.KeuzeOpties4.TabIndex = 5;
            // 
            // TekstInfo0
            // 
            this.TekstInfo0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TekstInfo0.Location = new System.Drawing.Point(238, 16);
            this.TekstInfo0.Name = "TekstInfo0";
            this.TekstInfo0.Size = new System.Drawing.Size(51, 20);
            this.TekstInfo0.TabIndex = 1;
            // 
            // TekstInfo1
            // 
            this.TekstInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TekstInfo1.Location = new System.Drawing.Point(92, 116);
            this.TekstInfo1.Name = "TekstInfo1";
            this.TekstInfo1.Size = new System.Drawing.Size(104, 20);
            this.TekstInfo1.TabIndex = 6;
            // 
            // TekstInfo2
            // 
            this.TekstInfo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TekstInfo2.Location = new System.Drawing.Point(92, 136);
            this.TekstInfo2.Name = "TekstInfo2";
            this.TekstInfo2.Size = new System.Drawing.Size(104, 20);
            this.TekstInfo2.TabIndex = 7;
            // 
            // TekstInfo3
            // 
            this.TekstInfo3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TekstInfo3.Location = new System.Drawing.Point(310, 116);
            this.TekstInfo3.Name = "TekstInfo3";
            this.TekstInfo3.Size = new System.Drawing.Size(104, 20);
            this.TekstInfo3.TabIndex = 8;
            // 
            // TekstInfo4
            // 
            this.TekstInfo4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TekstInfo4.Location = new System.Drawing.Point(310, 136);
            this.TekstInfo4.Name = "TekstInfo4";
            this.TekstInfo4.Size = new System.Drawing.Size(104, 20);
            this.TekstInfo4.TabIndex = 9;
            // 
            // Ok
            // 
            this.Ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ok.Location = new System.Drawing.Point(422, 6);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(96, 24);
            this.Ok.TabIndex = 22;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Annuleren
            // 
            this.Annuleren.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Annuleren.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Annuleren.Location = new System.Drawing.Point(422, 34);
            this.Annuleren.Name = "Annuleren";
            this.Annuleren.Size = new System.Drawing.Size(96, 26);
            this.Annuleren.TabIndex = 10;
            this.Annuleren.Text = "Sluiten";
            this.Annuleren.UseVisualStyleBackColor = true;
            this.Annuleren.Click += new System.EventHandler(this.Annuleren_Click);
            // 
            // Label1_11
            // 
            this.Label1_11.AutoSize = true;
            this.Label1_11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_11.Location = new System.Drawing.Point(418, 86);
            this.Label1_11.Name = "Label1_11";
            this.Label1_11.Size = new System.Drawing.Size(96, 13);
            this.Label1_11.TabIndex = 26;
            this.Label1_11.Text = "Aanvullende eenh.";
            // 
            // Label1_9
            // 
            this.Label1_9.AutoSize = true;
            this.Label1_9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_9.Location = new System.Drawing.Point(304, 28);
            this.Label1_9.Name = "Label1_9";
            this.Label1_9.Size = new System.Drawing.Size(89, 13);
            this.Label1_9.TabIndex = 23;
            this.Label1_9.Text = "Nog toe te wijzen";
            // 
            // LabelA6
            // 
            this.LabelA6.AutoSize = true;
            this.LabelA6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelA6.Location = new System.Drawing.Point(6, 2);
            this.LabelA6.Name = "LabelA6";
            this.LabelA6.Size = new System.Drawing.Size(24, 13);
            this.LabelA6.TabIndex = 11;
            this.LabelA6.Text = "test";
            // 
            // Label1_1
            // 
            this.Label1_1.AutoSize = true;
            this.Label1_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_1.Location = new System.Drawing.Point(200, 118);
            this.Label1_1.Name = "Label1_1";
            this.Label1_1.Size = new System.Drawing.Size(96, 13);
            this.Label1_1.TabIndex = 12;
            this.Label1_1.Text = "Aanvullende eenh.";
            // 
            // Label1_2
            // 
            this.Label1_2.AutoSize = true;
            this.Label1_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_2.Location = new System.Drawing.Point(200, 138);
            this.Label1_2.Name = "Label1_2";
            this.Label1_2.Size = new System.Drawing.Size(86, 13);
            this.Label1_2.TabIndex = 13;
            this.Label1_2.Text = "Statisch. waarde";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(4, 56);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(24, 13);
            this.Label8.TabIndex = 14;
            this.Label8.Text = "test";
            // 
            // Label1_4
            // 
            this.Label1_4.AutoSize = true;
            this.Label1_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_4.Location = new System.Drawing.Point(4, 36);
            this.Label1_4.Name = "Label1_4";
            this.Label1_4.Size = new System.Drawing.Size(70, 13);
            this.Label1_4.TabIndex = 15;
            this.Label1_4.Text = "VervoerWijze";
            // 
            // Label1_5
            // 
            this.Label1_5.AutoSize = true;
            this.Label1_5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_5.Location = new System.Drawing.Point(4, 136);
            this.Label1_5.Name = "Label1_5";
            this.Label1_5.Size = new System.Drawing.Size(55, 13);
            this.Label1_5.TabIndex = 16;
            this.Label1_5.Text = "Netto (Kg)";
            // 
            // Label1_6
            // 
            this.Label1_6.AutoSize = true;
            this.Label1_6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_6.Location = new System.Drawing.Point(4, 118);
            this.Label1_6.Name = "Label1_6";
            this.Label1_6.Size = new System.Drawing.Size(79, 13);
            this.Label1_6.TabIndex = 17;
            this.Label1_6.Text = "GoederenKode";
            // 
            // Label1_7
            // 
            this.Label1_7.AutoSize = true;
            this.Label1_7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_7.Location = new System.Drawing.Point(4, 96);
            this.Label1_7.Name = "Label1_7";
            this.Label1_7.Size = new System.Drawing.Size(67, 13);
            this.Label1_7.TabIndex = 18;
            this.Label1_7.Text = "Transaktie B";
            // 
            // LabelB6
            // 
            this.LabelB6.AutoSize = true;
            this.LabelB6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelB6.Location = new System.Drawing.Point(168, 2);
            this.LabelB6.Name = "LabelB6";
            this.LabelB6.Size = new System.Drawing.Size(24, 13);
            this.LabelB6.TabIndex = 19;
            this.LabelB6.Text = "test";
            // 
            // Label1_10
            // 
            this.Label1_10.AutoSize = true;
            this.Label1_10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1_10.Location = new System.Drawing.Point(4, 76);
            this.Label1_10.Name = "Label1_10";
            this.Label1_10.Size = new System.Drawing.Size(67, 13);
            this.Label1_10.TabIndex = 20;
            this.Label1_10.Text = "Transaktie A";
            // 
            // NogToeTeWijzen
            // 
            this.NogToeTeWijzen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NogToeTeWijzen.Location = new System.Drawing.Point(304, 46);
            this.NogToeTeWijzen.Name = "NogToeTeWijzen";
            this.NogToeTeWijzen.Size = new System.Drawing.Size(107, 21);
            this.NogToeTeWijzen.TabIndex = 21;
            this.NogToeTeWijzen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormIntrastat
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Annuleren;
            this.ClientSize = new System.Drawing.Size(526, 249);
            this.Controls.Add(this.NogToeTeWijzen);
            this.Controls.Add(this.Label1_10);
            this.Controls.Add(this.LabelB6);
            this.Controls.Add(this.Label1_7);
            this.Controls.Add(this.Label1_6);
            this.Controls.Add(this.Label1_5);
            this.Controls.Add(this.Label1_4);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Label1_2);
            this.Controls.Add(this.Label1_1);
            this.Controls.Add(this.LabelA6);
            this.Controls.Add(this.Label1_9);
            this.Controls.Add(this.Label1_11);
            this.Controls.Add(this.Annuleren);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.TekstInfo4);
            this.Controls.Add(this.TekstInfo3);
            this.Controls.Add(this.TekstInfo2);
            this.Controls.Add(this.TekstInfo1);
            this.Controls.Add(this.TekstInfo0);
            this.Controls.Add(this.KeuzeOpties4);
            this.Controls.Add(this.KeuzeOpties3);
            this.Controls.Add(this.KeuzeOpties2);
            this.Controls.Add(this.KeuzeOpties1);
            this.Controls.Add(this.KeuzeOpties0);
            this.Controls.Add(this.InfoTekst);
            this.Controls.Add(this.Eenheden);
            this.Controls.Add(this.InfoDataPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormIntrastat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Intrastat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormIntrastat_FormClosed);
            this.Load += new System.EventHandler(this.FormIntrastat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel InfoDataPanel;
        private TextBox Eenheden;
        private TextBox InfoTekst;
        private ComboBox KeuzeOpties0;
        private ComboBox KeuzeOpties1;
        private ComboBox KeuzeOpties2;
        private ComboBox KeuzeOpties3;
        private ComboBox KeuzeOpties4;
        private TextBox TekstInfo0;
        private TextBox TekstInfo1;
        private TextBox TekstInfo2;
        private TextBox TekstInfo3;
        private TextBox TekstInfo4;
        private Button Ok;
        private Button Annuleren;
        private Label Label1_11;
        private Label Label1_9;
        private Label LabelA6;
        private Label Label1_1;
        private Label Label1_2;
        private Label Label8;
        private Label Label1_4;
        private Label Label1_5;
        private Label Label1_6;
        private Label Label1_7;
        private Label LabelB6;
        private Label Label1_10;
        private Label NogToeTeWijzen;
    }
}
