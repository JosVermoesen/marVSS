namespace marVSS2028.Forms
{
    partial class FormCloudSetting
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
            this.ButtonDefaultResetForOneDrive = new System.Windows.Forms.Button();
            this.ButtonDefaultResetForMapMarnt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxCloudMarnt = new System.Windows.Forms.TextBox();
            this.TextBoxCloudMario = new System.Windows.Forms.TextBox();
            this.TextBoxCloudArchive = new System.Windows.Forms.TextBox();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.GroupBoxCloud = new System.Windows.Forms.GroupBox();
            this.ButtonToggle = new System.Windows.Forms.Button();
            this.ButtonCloudArchive = new System.Windows.Forms.Button();
            this.ButtonCloudMario = new System.Windows.Forms.Button();
            this.ButtonCloudMarnt = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonShowAlwaysBookingsInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonShowSomeBookingsInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonShowNoBookingsInfo = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ButtonMarntDataMap = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxMarntDataMap = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ButtonCodaIOMap = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxCodaIOMap = new System.Windows.Forms.TextBox();
            this.GroupBoxCloud.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonDefaultResetForOneDrive
            // 
            this.ButtonDefaultResetForOneDrive.Location = new System.Drawing.Point(11, 109);
            this.ButtonDefaultResetForOneDrive.Name = "ButtonDefaultResetForOneDrive";
            this.ButtonDefaultResetForOneDrive.Size = new System.Drawing.Size(139, 30);
            this.ButtonDefaultResetForOneDrive.TabIndex = 0;
            this.ButtonDefaultResetForOneDrive.Text = "AutoDefault OneDrive";
            this.ButtonDefaultResetForOneDrive.UseVisualStyleBackColor = true;
            this.ButtonDefaultResetForOneDrive.Click += new System.EventHandler(this.ButtonDefaultResetForOneDrive_Click);
            // 
            // ButtonDefaultResetForMapMarnt
            // 
            this.ButtonDefaultResetForMapMarnt.Location = new System.Drawing.Point(156, 109);
            this.ButtonDefaultResetForMapMarnt.Name = "ButtonDefaultResetForMapMarnt";
            this.ButtonDefaultResetForMapMarnt.Size = new System.Drawing.Size(128, 30);
            this.ButtonDefaultResetForMapMarnt.TabIndex = 1;
            this.ButtonDefaultResetForMapMarnt.Text = "AutoDefault Map Marnt";
            this.ButtonDefaultResetForMapMarnt.UseVisualStyleBackColor = true;
            this.ButtonDefaultResetForMapMarnt.Click += new System.EventHandler(this.ButtonDefaultResetForMapMarnt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MARNT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "MARIO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "ARCHIEF";
            // 
            // TextBoxCloudMarnt
            // 
            this.TextBoxCloudMarnt.Enabled = false;
            this.TextBoxCloudMarnt.Location = new System.Drawing.Point(77, 19);
            this.TextBoxCloudMarnt.Name = "TextBoxCloudMarnt";
            this.TextBoxCloudMarnt.Size = new System.Drawing.Size(601, 20);
            this.TextBoxCloudMarnt.TabIndex = 7;
            // 
            // TextBoxCloudMario
            // 
            this.TextBoxCloudMario.Enabled = false;
            this.TextBoxCloudMario.Location = new System.Drawing.Point(77, 45);
            this.TextBoxCloudMario.Name = "TextBoxCloudMario";
            this.TextBoxCloudMario.Size = new System.Drawing.Size(601, 20);
            this.TextBoxCloudMario.TabIndex = 8;
            // 
            // TextBoxCloudArchive
            // 
            this.TextBoxCloudArchive.Enabled = false;
            this.TextBoxCloudArchive.Location = new System.Drawing.Point(77, 71);
            this.TextBoxCloudArchive.Name = "TextBoxCloudArchive";
            this.TextBoxCloudArchive.Size = new System.Drawing.Size(601, 20);
            this.TextBoxCloudArchive.TabIndex = 9;
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(290, 109);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(113, 30);
            this.ButtonSave.TabIndex = 10;
            this.ButtonSave.Text = "Bewaren en Sluiten";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(640, 109);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(74, 27);
            this.ButtonClose.TabIndex = 11;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // GroupBoxCloud
            // 
            this.GroupBoxCloud.Controls.Add(this.ButtonToggle);
            this.GroupBoxCloud.Controls.Add(this.ButtonCloudArchive);
            this.GroupBoxCloud.Controls.Add(this.ButtonCloudMario);
            this.GroupBoxCloud.Controls.Add(this.ButtonCloudMarnt);
            this.GroupBoxCloud.Controls.Add(this.ButtonDefaultResetForOneDrive);
            this.GroupBoxCloud.Controls.Add(this.ButtonClose);
            this.GroupBoxCloud.Controls.Add(this.ButtonDefaultResetForMapMarnt);
            this.GroupBoxCloud.Controls.Add(this.ButtonSave);
            this.GroupBoxCloud.Controls.Add(this.TextBoxCloudArchive);
            this.GroupBoxCloud.Controls.Add(this.label2);
            this.GroupBoxCloud.Controls.Add(this.TextBoxCloudMario);
            this.GroupBoxCloud.Controls.Add(this.label3);
            this.GroupBoxCloud.Controls.Add(this.TextBoxCloudMarnt);
            this.GroupBoxCloud.Controls.Add(this.label4);
            this.GroupBoxCloud.Location = new System.Drawing.Point(12, 12);
            this.GroupBoxCloud.Name = "GroupBoxCloud";
            this.GroupBoxCloud.Size = new System.Drawing.Size(732, 152);
            this.GroupBoxCloud.TabIndex = 12;
            this.GroupBoxCloud.TabStop = false;
            this.GroupBoxCloud.Text = "Cloud";
            // 
            // ButtonToggle
            // 
            this.ButtonToggle.Location = new System.Drawing.Point(533, 109);
            this.ButtonToggle.Name = "ButtonToggle";
            this.ButtonToggle.Size = new System.Drawing.Size(101, 27);
            this.ButtonToggle.TabIndex = 17;
            this.ButtonToggle.Text = "Toggle Bewerken";
            this.ButtonToggle.UseVisualStyleBackColor = true;
            this.ButtonToggle.Click += new System.EventHandler(this.ButtonToggle_Click);
            // 
            // ButtonCloudArchive
            // 
            this.ButtonCloudArchive.Image = global::marVSS2028.Properties.Resources.OPENFOLD;
            this.ButtonCloudArchive.Location = new System.Drawing.Point(684, 68);
            this.ButtonCloudArchive.Name = "ButtonCloudArchive";
            this.ButtonCloudArchive.Size = new System.Drawing.Size(30, 23);
            this.ButtonCloudArchive.TabIndex = 16;
            this.ButtonCloudArchive.UseVisualStyleBackColor = true;
            this.ButtonCloudArchive.Click += new System.EventHandler(this.ButtonCloudArchive_Click);
            // 
            // ButtonCloudMario
            // 
            this.ButtonCloudMario.Image = global::marVSS2028.Properties.Resources.OPENFOLD;
            this.ButtonCloudMario.Location = new System.Drawing.Point(684, 42);
            this.ButtonCloudMario.Name = "ButtonCloudMario";
            this.ButtonCloudMario.Size = new System.Drawing.Size(30, 23);
            this.ButtonCloudMario.TabIndex = 15;
            this.ButtonCloudMario.UseVisualStyleBackColor = true;
            this.ButtonCloudMario.Click += new System.EventHandler(this.ButtonCloudMario_Click);
            // 
            // ButtonCloudMarnt
            // 
            this.ButtonCloudMarnt.Image = global::marVSS2028.Properties.Resources.OPENFOLD;
            this.ButtonCloudMarnt.Location = new System.Drawing.Point(684, 16);
            this.ButtonCloudMarnt.Name = "ButtonCloudMarnt";
            this.ButtonCloudMarnt.Size = new System.Drawing.Size(30, 23);
            this.ButtonCloudMarnt.TabIndex = 14;
            this.ButtonCloudMarnt.UseVisualStyleBackColor = true;
            this.ButtonCloudMarnt.Click += new System.EventHandler(this.ButtonCloudMarnt_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonShowAlwaysBookingsInfo);
            this.groupBox1.Controls.Add(this.radioButtonShowSomeBookingsInfo);
            this.groupBox1.Controls.Add(this.radioButtonShowNoBookingsInfo);
            this.groupBox1.Location = new System.Drawing.Point(487, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 132);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Journaal post";
            // 
            // radioButtonShowAlwaysBookingsInfo
            // 
            this.radioButtonShowAlwaysBookingsInfo.AutoSize = true;
            this.radioButtonShowAlwaysBookingsInfo.Location = new System.Drawing.Point(13, 85);
            this.radioButtonShowAlwaysBookingsInfo.Name = "radioButtonShowAlwaysBookingsInfo";
            this.radioButtonShowAlwaysBookingsInfo.Size = new System.Drawing.Size(146, 17);
            this.radioButtonShowAlwaysBookingsInfo.TabIndex = 2;
            this.radioButtonShowAlwaysBookingsInfo.TabStop = true;
            this.radioButtonShowAlwaysBookingsInfo.Text = "Altijd BoekingsInfo Tonen";
            this.radioButtonShowAlwaysBookingsInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonShowSomeBookingsInfo
            // 
            this.radioButtonShowSomeBookingsInfo.AutoSize = true;
            this.radioButtonShowSomeBookingsInfo.Location = new System.Drawing.Point(13, 62);
            this.radioButtonShowSomeBookingsInfo.Name = "radioButtonShowSomeBookingsInfo";
            this.radioButtonShowSomeBookingsInfo.Size = new System.Drawing.Size(203, 17);
            this.radioButtonShowSomeBookingsInfo.TabIndex = 1;
            this.radioButtonShowSomeBookingsInfo.TabStop = true;
            this.radioButtonShowSomeBookingsInfo.Text = "BoekingsInfo bij EUR <> BEF verschil";
            this.radioButtonShowSomeBookingsInfo.UseVisualStyleBackColor = true;
            // 
            // radioButtonShowNoBookingsInfo
            // 
            this.radioButtonShowNoBookingsInfo.AutoSize = true;
            this.radioButtonShowNoBookingsInfo.Location = new System.Drawing.Point(13, 39);
            this.radioButtonShowNoBookingsInfo.Name = "radioButtonShowNoBookingsInfo";
            this.radioButtonShowNoBookingsInfo.Size = new System.Drawing.Size(150, 17);
            this.radioButtonShowNoBookingsInfo.TabIndex = 0;
            this.radioButtonShowNoBookingsInfo.TabStop = true;
            this.radioButtonShowNoBookingsInfo.Text = "Geen BoekingsInfo Tonen";
            this.radioButtonShowNoBookingsInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ButtonMarntDataMap);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.TextBoxMarntDataMap);
            this.groupBox2.Location = new System.Drawing.Point(12, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(469, 69);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Locatie Bedrijfmappen marnt\\data";
            // 
            // ButtonMarntDataMap
            // 
            this.ButtonMarntDataMap.Image = global::marVSS2028.Properties.Resources.OPENFOLD;
            this.ButtonMarntDataMap.Location = new System.Drawing.Point(426, 31);
            this.ButtonMarntDataMap.Name = "ButtonMarntDataMap";
            this.ButtonMarntDataMap.Size = new System.Drawing.Size(30, 23);
            this.ButtonMarntDataMap.TabIndex = 17;
            this.ButtonMarntDataMap.UseVisualStyleBackColor = true;
            this.ButtonMarntDataMap.Click += new System.EventHandler(this.ButtonMarntDataMap_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "MAP";
            // 
            // TextBoxMarntDataMap
            // 
            this.TextBoxMarntDataMap.Enabled = false;
            this.TextBoxMarntDataMap.Location = new System.Drawing.Point(60, 34);
            this.TextBoxMarntDataMap.Name = "TextBoxMarntDataMap";
            this.TextBoxMarntDataMap.Size = new System.Drawing.Size(360, 20);
            this.TextBoxMarntDataMap.TabIndex = 9;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ButtonCodaIOMap);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.TextBoxCodaIOMap);
            this.groupBox3.Location = new System.Drawing.Point(12, 245);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(469, 60);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Coda XML en XDA";
            // 
            // ButtonCodaIOMap
            // 
            this.ButtonCodaIOMap.Image = global::marVSS2028.Properties.Resources.OPENFOLD;
            this.ButtonCodaIOMap.Location = new System.Drawing.Point(426, 24);
            this.ButtonCodaIOMap.Name = "ButtonCodaIOMap";
            this.ButtonCodaIOMap.Size = new System.Drawing.Size(30, 23);
            this.ButtonCodaIOMap.TabIndex = 17;
            this.ButtonCodaIOMap.UseVisualStyleBackColor = true;
            this.ButtonCodaIOMap.Click += new System.EventHandler(this.ButtonCodaIOMap_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "MAP";
            // 
            // TextBoxCodaIOMap
            // 
            this.TextBoxCodaIOMap.Enabled = false;
            this.TextBoxCodaIOMap.Location = new System.Drawing.Point(60, 27);
            this.TextBoxCodaIOMap.Name = "TextBoxCodaIOMap";
            this.TextBoxCodaIOMap.Size = new System.Drawing.Size(360, 20);
            this.TextBoxCodaIOMap.TabIndex = 9;
            // 
            // FormCloudSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(756, 314);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GroupBoxCloud);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCloudSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instellingen";
            this.Load += new System.EventHandler(this.FormCloudSetting_Load);
            this.GroupBoxCloud.ResumeLayout(false);
            this.GroupBoxCloud.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonDefaultResetForOneDrive;
        private System.Windows.Forms.Button ButtonDefaultResetForMapMarnt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxCloudMarnt;
        private System.Windows.Forms.TextBox TextBoxCloudMario;
        private System.Windows.Forms.TextBox TextBoxCloudArchive;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.GroupBox GroupBoxCloud;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonShowAlwaysBookingsInfo;
        private System.Windows.Forms.RadioButton radioButtonShowSomeBookingsInfo;
        private System.Windows.Forms.RadioButton radioButtonShowNoBookingsInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxMarntDataMap;
        private System.Windows.Forms.Button ButtonCloudArchive;
        private System.Windows.Forms.Button ButtonCloudMario;
        private System.Windows.Forms.Button ButtonCloudMarnt;
        private System.Windows.Forms.Button ButtonMarntDataMap;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ButtonCodaIOMap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxCodaIOMap;
        private System.Windows.Forms.Button ButtonToggle;
    }
}