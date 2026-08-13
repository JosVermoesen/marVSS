namespace marVSS2028.Forms
{
    partial class FormSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplash));
            this.cmdLeesMij = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.LabelProductInfo = new System.Windows.Forms.Label();
            this.lblCloud = new System.Windows.Forms.Label();
            this.LabelCopyRight = new System.Windows.Forms.Label();
            this.AppInfo0 = new System.Windows.Forms.Label();
            this.LabelProductName = new System.Windows.Forms.Label();
            this.LabelInfo2 = new System.Windows.Forms.Label();
            this.Image1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdLeesMij
            // 
            this.cmdLeesMij.BackColor = System.Drawing.Color.White;
            this.cmdLeesMij.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cmdLeesMij.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.cmdLeesMij.Location = new System.Drawing.Point(115, 317);
            this.cmdLeesMij.Name = "cmdLeesMij";
            this.cmdLeesMij.Size = new System.Drawing.Size(174, 30);
            this.cmdLeesMij.TabIndex = 7;
            this.cmdLeesMij.Text = "LeesMij";
            this.cmdLeesMij.UseVisualStyleBackColor = false;
            this.cmdLeesMij.Click += new System.EventHandler(this.CmdLeesMij_Click);
            // 
            // Ok
            // 
            this.Ok.BackColor = System.Drawing.Color.White;
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Ok.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Ok.Location = new System.Drawing.Point(298, 317);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(68, 30);
            this.Ok.TabIndex = 0;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = false;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // LabelProductInfo
            // 
            this.LabelProductInfo.BackColor = System.Drawing.SystemColors.Highlight;
            this.LabelProductInfo.ForeColor = System.Drawing.Color.Black;
            this.LabelProductInfo.Location = new System.Drawing.Point(70, 138);
            this.LabelProductInfo.Name = "LabelProductInfo";
            this.LabelProductInfo.Size = new System.Drawing.Size(364, 72);
            this.LabelProductInfo.TabIndex = 2;
            this.LabelProductInfo.Text = "Label5";
            this.LabelProductInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCloud
            // 
            this.lblCloud.BackColor = System.Drawing.SystemColors.Highlight;
            this.lblCloud.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblCloud.ForeColor = System.Drawing.Color.Black;
            this.lblCloud.Location = new System.Drawing.Point(67, 250);
            this.lblCloud.Name = "lblCloud";
            this.lblCloud.Size = new System.Drawing.Size(354, 52);
            this.lblCloud.TabIndex = 8;
            this.lblCloud.Text = "marIntegraal 2028";
            this.lblCloud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelCopyRight
            // 
            this.LabelCopyRight.BackColor = System.Drawing.SystemColors.Highlight;
            this.LabelCopyRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelCopyRight.ForeColor = System.Drawing.Color.Black;
            this.LabelCopyRight.Location = new System.Drawing.Point(84, 210);
            this.LabelCopyRight.Name = "LabelCopyRight";
            this.LabelCopyRight.Size = new System.Drawing.Size(323, 40);
            this.LabelCopyRight.TabIndex = 1;
            this.LabelCopyRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AppInfo0
            // 
            this.AppInfo0.BackColor = System.Drawing.SystemColors.Highlight;
            this.AppInfo0.ForeColor = System.Drawing.Color.Black;
            this.AppInfo0.Location = new System.Drawing.Point(346, 58);
            this.AppInfo0.Name = "AppInfo0";
            this.AppInfo0.Size = new System.Drawing.Size(94, 16);
            this.AppInfo0.TabIndex = 3;
            this.AppInfo0.Text = "Label2";
            this.AppInfo0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelProductName
            // 
            this.LabelProductName.BackColor = System.Drawing.SystemColors.Highlight;
            this.LabelProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.LabelProductName.ForeColor = System.Drawing.Color.Black;
            this.LabelProductName.Location = new System.Drawing.Point(10, 19);
            this.LabelProductName.Name = "LabelProductName";
            this.LabelProductName.Size = new System.Drawing.Size(450, 54);
            this.LabelProductName.TabIndex = 4;
            this.LabelProductName.Text = "Produktnaam";
            this.LabelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelProductName.Click += new System.EventHandler(this.LabelInfo_Click);
            // 
            // LabelInfo2
            // 
            this.LabelInfo2.BackColor = System.Drawing.SystemColors.Highlight;
            this.LabelInfo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Bold);
            this.LabelInfo2.ForeColor = System.Drawing.Color.Black;
            this.LabelInfo2.Location = new System.Drawing.Point(58, 67);
            this.LabelInfo2.Name = "LabelInfo2";
            this.LabelInfo2.Size = new System.Drawing.Size(349, 83);
            this.LabelInfo2.TabIndex = 5;
            this.LabelInfo2.Text = "-";
            this.LabelInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelInfo2.Click += new System.EventHandler(this.LabelInfo_Click);
            // 
            // Image1
            // 
            this.Image1.Image = global::marVSS2028.Properties.Resources.Windows_11_scaled;
            this.Image1.Location = new System.Drawing.Point(470, 0);
            this.Image1.Name = "Image1";
            this.Image1.Size = new System.Drawing.Size(546, 357);
            this.Image1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Image1.TabIndex = 0;
            this.Image1.TabStop = false;
            this.Image1.Click += new System.EventHandler(this.Image1_Click);
            // 
            // FormSplash
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.CancelButton = this.Ok;
            this.ClientSize = new System.Drawing.Size(1014, 357);
            this.ControlBox = false;
            this.Controls.Add(this.cmdLeesMij);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.LabelProductInfo);
            this.Controls.Add(this.lblCloud);
            this.Controls.Add(this.LabelCopyRight);
            this.Controls.Add(this.AppInfo0);
            this.Controls.Add(this.Image1);
            this.Controls.Add(this.LabelProductName);
            this.Controls.Add(this.LabelInfo2);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info Licentie";
            this.Load += new System.EventHandler(this.FormSplash_Load);
            this.Click += new System.EventHandler(this.FormSplash_Click);
            this.DoubleClick += new System.EventHandler(this.FormSplash_DblClick);
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdLeesMij;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Label LabelProductInfo;
        private System.Windows.Forms.Label lblCloud;
        private System.Windows.Forms.Label LabelCopyRight;
        private System.Windows.Forms.Label AppInfo0;
        private System.Windows.Forms.PictureBox Image1;
        private System.Windows.Forms.Label LabelProductName;
        private System.Windows.Forms.Label LabelInfo2;
    }
}