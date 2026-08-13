namespace marVSS2028.SharedForms
{
    partial class FormReactionsDialog
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
            this.TextBoxReactions = new System.Windows.Forms.TextBox();
            this.BtnSluiten = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxReactions
            // 
            this.TextBoxReactions.Location = new System.Drawing.Point(8, 8);
            this.TextBoxReactions.Multiline = true;
            this.TextBoxReactions.Name = "TextBoxReactions";
            this.TextBoxReactions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxReactions.Size = new System.Drawing.Size(496, 232);
            this.TextBoxReactions.TabIndex = 1;
            // 
            // BtnSluiten
            // 
            this.BtnSluiten.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSluiten.Location = new System.Drawing.Point(19, 248);
            this.BtnSluiten.Name = "BtnSluiten";
            this.BtnSluiten.Size = new System.Drawing.Size(97, 30);
            this.BtnSluiten.TabIndex = 0;
            this.BtnSluiten.Text = "Sluiten";
            this.BtnSluiten.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormReactionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnSluiten;
            this.ClientSize = new System.Drawing.Size(513, 289);
            this.Controls.Add(this.TextBoxReactions);
            this.Controls.Add(this.BtnSluiten);
            this.MinimizeBox = false;
            this.Name = "FormReactionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reacties";
            this.Resize += new System.EventHandler(this.FormReactionsDialog_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox TextBoxReactions;
        private System.Windows.Forms.Button BtnSluiten;
    }
}