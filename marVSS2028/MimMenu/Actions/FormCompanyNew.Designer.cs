namespace marVSS2028.MimMenu.Actions
{
    partial class FormCompanyNew
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtStartMaand = new System.Windows.Forms.TextBox();
            this.TypeBoekjaar0 = new System.Windows.Forms.RadioButton();
            this.TypeBoekjaar1 = new System.Windows.Forms.RadioButton();
            this.TypeBoekjaar2 = new System.Windows.Forms.RadioButton();
            this.TypeBoekjaar3 = new System.Windows.Forms.RadioButton();
            this.Maanden = new System.Windows.Forms.TextBox();
            this.CmbBedrijfsType = new System.Windows.Forms.ComboBox();
            this.BedrijfsNaam = new System.Windows.Forms.TextBox();
            this.Installeren = new System.Windows.Forms.Button();
            this.Negeren = new System.Windows.Forms.Button();
            this.Makelaar = new System.Windows.Forms.CheckBox();
            this.Boekjaar = new System.Windows.Forms.TextBox();
            this.LabelNaam = new System.Windows.Forms.Label();
            this.LabelType = new System.Windows.Forms.Label();
            this.LabelMaanden = new System.Windows.Forms.Label();
            this.LabelBoekjaar = new System.Windows.Forms.Label();
            this.LabelData = new System.Windows.Forms.Label();
            this.LocationMarntData = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtStartMaand
            // 
            this.txtStartMaand.BackColor = System.Drawing.Color.White;
            this.txtStartMaand.Enabled = false;
            this.txtStartMaand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartMaand.Location = new System.Drawing.Point(115, 116);
            this.txtStartMaand.Name = "txtStartMaand";
            this.txtStartMaand.Size = new System.Drawing.Size(56, 20);
            this.txtStartMaand.TabIndex = 17;
            this.txtStartMaand.Text = "01/01";
            this.txtStartMaand.Leave += new System.EventHandler(this.txtStartMaand_Leave);
            // 
            // TypeBoekjaar0
            // 
            this.TypeBoekjaar0.AutoSize = true;
            this.TypeBoekjaar0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TypeBoekjaar0.Checked = true;
            this.TypeBoekjaar0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeBoekjaar0.Location = new System.Drawing.Point(9, 53);
            this.TypeBoekjaar0.Name = "TypeBoekjaar0";
            this.TypeBoekjaar0.Size = new System.Drawing.Size(100, 17);
            this.TypeBoekjaar0.TabIndex = 1;
            this.TypeBoekjaar0.TabStop = true;
            this.TypeBoekjaar0.Text = "Aanvang 01/0&1";
            this.TypeBoekjaar0.Click += new System.EventHandler(this.TypeBoekjaar_Click);
            // 
            // TypeBoekjaar1
            // 
            this.TypeBoekjaar1.AutoSize = true;
            this.TypeBoekjaar1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TypeBoekjaar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeBoekjaar1.Location = new System.Drawing.Point(55, 74);
            this.TypeBoekjaar1.Name = "TypeBoekjaar1";
            this.TypeBoekjaar1.Size = new System.Drawing.Size(54, 17);
            this.TypeBoekjaar1.TabIndex = 6;
            this.TypeBoekjaar1.Text = "01/0&7";
            this.TypeBoekjaar1.Click += new System.EventHandler(this.TypeBoekjaar_Click);
            // 
            // TypeBoekjaar2
            // 
            this.TypeBoekjaar2.AutoSize = true;
            this.TypeBoekjaar2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TypeBoekjaar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeBoekjaar2.Location = new System.Drawing.Point(55, 95);
            this.TypeBoekjaar2.Name = "TypeBoekjaar2";
            this.TypeBoekjaar2.Size = new System.Drawing.Size(54, 17);
            this.TypeBoekjaar2.TabIndex = 7;
            this.TypeBoekjaar2.Text = "01/1&0";
            this.TypeBoekjaar2.Click += new System.EventHandler(this.TypeBoekjaar_Click);
            // 
            // TypeBoekjaar3
            // 
            this.TypeBoekjaar3.AutoSize = true;
            this.TypeBoekjaar3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TypeBoekjaar3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeBoekjaar3.Location = new System.Drawing.Point(50, 119);
            this.TypeBoekjaar3.Name = "TypeBoekjaar3";
            this.TypeBoekjaar3.Size = new System.Drawing.Size(59, 17);
            this.TypeBoekjaar3.TabIndex = 16;
            this.TypeBoekjaar3.Text = "Andere";
            this.TypeBoekjaar3.Click += new System.EventHandler(this.TypeBoekjaar_Click);
            // 
            // Maanden
            // 
            this.Maanden.BackColor = System.Drawing.Color.White;
            this.Maanden.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Maanden.Location = new System.Drawing.Point(299, 20);
            this.Maanden.Name = "Maanden";
            this.Maanden.Size = new System.Drawing.Size(31, 20);
            this.Maanden.TabIndex = 15;
            this.Maanden.Text = "12";
            // 
            // CmbBedrijfsType
            // 
            this.CmbBedrijfsType.BackColor = System.Drawing.Color.White;
            this.CmbBedrijfsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBedrijfsType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBedrijfsType.Location = new System.Drawing.Point(175, 74);
            this.CmbBedrijfsType.Name = "CmbBedrijfsType";
            this.CmbBedrijfsType.Size = new System.Drawing.Size(233, 21);
            this.CmbBedrijfsType.TabIndex = 13;
            // 
            // BedrijfsNaam
            // 
            this.BedrijfsNaam.BackColor = System.Drawing.Color.White;
            this.BedrijfsNaam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BedrijfsNaam.Location = new System.Drawing.Point(8, 20);
            this.BedrijfsNaam.Name = "BedrijfsNaam";
            this.BedrijfsNaam.Size = new System.Drawing.Size(161, 20);
            this.BedrijfsNaam.TabIndex = 0;
            this.BedrijfsNaam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BedrijfsNaam_KeyPress);
            // 
            // Installeren
            // 
            this.Installeren.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Installeren.Location = new System.Drawing.Point(348, 2);
            this.Installeren.Name = "Installeren";
            this.Installeren.Size = new System.Drawing.Size(93, 24);
            this.Installeren.TabIndex = 3;
            this.Installeren.Text = "&Installeren";
            this.Installeren.Click += new System.EventHandler(this.Installeren_Click);
            // 
            // Negeren
            // 
            this.Negeren.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Negeren.Location = new System.Drawing.Point(349, 166);
            this.Negeren.Name = "Negeren";
            this.Negeren.Size = new System.Drawing.Size(92, 24);
            this.Negeren.TabIndex = 8;
            this.Negeren.Text = "Sluiten";
            this.Negeren.Click += new System.EventHandler(this.Negeren_Click);
            // 
            // Makelaar
            // 
            this.Makelaar.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Makelaar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Makelaar.Location = new System.Drawing.Point(260, 101);
            this.Makelaar.Name = "Makelaar";
            this.Makelaar.Size = new System.Drawing.Size(148, 17);
            this.Makelaar.TabIndex = 4;
            this.Makelaar.Text = "&Verzekeringsbemiddelaar";
            // 
            // Boekjaar
            // 
            this.Boekjaar.BackColor = System.Drawing.Color.White;
            this.Boekjaar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Boekjaar.Location = new System.Drawing.Point(175, 18);
            this.Boekjaar.Name = "Boekjaar";
            this.Boekjaar.Size = new System.Drawing.Size(56, 20);
            this.Boekjaar.TabIndex = 2;
            this.Boekjaar.Leave += new System.EventHandler(this.Boekjaar_Leave);
            // 
            // LabelNaam
            // 
            this.LabelNaam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelNaam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelNaam.Location = new System.Drawing.Point(10, 2);
            this.LabelNaam.Name = "LabelNaam";
            this.LabelNaam.Size = new System.Drawing.Size(90, 17);
            this.LabelNaam.TabIndex = 9;
            this.LabelNaam.Text = "&Naam Bedrijf";
            // 
            // LabelType
            // 
            this.LabelType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelType.Location = new System.Drawing.Point(175, 52);
            this.LabelType.Name = "LabelType";
            this.LabelType.Size = new System.Drawing.Size(39, 19);
            this.LabelType.TabIndex = 14;
            this.LabelType.Text = "&Type";
            // 
            // LabelMaanden
            // 
            this.LabelMaanden.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelMaanden.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelMaanden.Location = new System.Drawing.Point(241, 0);
            this.LabelMaanden.Name = "LabelMaanden";
            this.LabelMaanden.Size = new System.Drawing.Size(89, 17);
            this.LabelMaanden.TabIndex = 10;
            this.LabelMaanden.Text = "Aantal &maanden";
            // 
            // LabelBoekjaar
            // 
            this.LabelBoekjaar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelBoekjaar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelBoekjaar.Location = new System.Drawing.Point(175, 0);
            this.LabelBoekjaar.Name = "LabelBoekjaar";
            this.LabelBoekjaar.Size = new System.Drawing.Size(57, 17);
            this.LabelBoekjaar.TabIndex = 11;
            this.LabelBoekjaar.Text = "&Boekjaar";
            // 
            // LabelData
            // 
            this.LabelData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelData.Location = new System.Drawing.Point(12, 150);
            this.LabelData.Name = "LabelData";
            this.LabelData.Size = new System.Drawing.Size(85, 17);
            this.LabelData.TabIndex = 12;
            this.LabelData.Text = "&Data Locatie";
            // 
            // LocationMarntData
            // 
            this.LocationMarntData.BackColor = System.Drawing.Color.White;
            this.LocationMarntData.Enabled = false;
            this.LocationMarntData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocationMarntData.Location = new System.Drawing.Point(10, 170);
            this.LocationMarntData.Name = "LocationMarntData";
            this.LocationMarntData.Size = new System.Drawing.Size(320, 20);
            this.LocationMarntData.TabIndex = 18;
            // 
            // FormCompanyNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.Negeren;
            this.ClientSize = new System.Drawing.Size(448, 196);
            this.Controls.Add(this.LocationMarntData);
            this.Controls.Add(this.LabelNaam);
            this.Controls.Add(this.BedrijfsNaam);
            this.Controls.Add(this.LabelData);
            this.Controls.Add(this.Installeren);
            this.Controls.Add(this.Negeren);
            this.Controls.Add(this.LabelBoekjaar);
            this.Controls.Add(this.LabelMaanden);
            this.Controls.Add(this.TypeBoekjaar0);
            this.Controls.Add(this.Boekjaar);
            this.Controls.Add(this.Maanden);
            this.Controls.Add(this.Makelaar);
            this.Controls.Add(this.TypeBoekjaar1);
            this.Controls.Add(this.TypeBoekjaar2);
            this.Controls.Add(this.LabelType);
            this.Controls.Add(this.TypeBoekjaar3);
            this.Controls.Add(this.txtStartMaand);
            this.Controls.Add(this.CmbBedrijfsType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCompanyNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nieuw bedrijf";
            this.Load += new System.EventHandler(this.FormCompanyNew_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox    txtStartMaand;
        private System.Windows.Forms.RadioButton TypeBoekjaar0;
        private System.Windows.Forms.RadioButton TypeBoekjaar1;
        private System.Windows.Forms.RadioButton TypeBoekjaar2;
        private System.Windows.Forms.RadioButton TypeBoekjaar3;
        private System.Windows.Forms.TextBox    Maanden;
        private System.Windows.Forms.ComboBox   CmbBedrijfsType;
        private System.Windows.Forms.TextBox    BedrijfsNaam;
        private System.Windows.Forms.Button     Installeren;
        private System.Windows.Forms.Button     Negeren;
        private System.Windows.Forms.CheckBox   Makelaar;
        private System.Windows.Forms.TextBox    Boekjaar;
        private System.Windows.Forms.Label      LabelNaam;
        private System.Windows.Forms.Label      LabelType;
        private System.Windows.Forms.Label      LabelMaanden;
        private System.Windows.Forms.Label      LabelBoekjaar;
        private System.Windows.Forms.Label      LabelData;
        private System.Windows.Forms.TextBox LocationMarntData;
    }
}
