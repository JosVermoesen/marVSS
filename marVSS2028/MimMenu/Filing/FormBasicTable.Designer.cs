using System.Windows.Forms;

namespace marVSS2028
{
    partial class FormBasicTable
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
            this.ComboBoxSearchOn = new System.Windows.Forms.ComboBox();
            this.ButtonFirst = new System.Windows.Forms.Button();
            this.MasketEditBoxInfo = new System.Windows.Forms.MaskedTextBox();
            this.ButtonLast = new System.Windows.Forms.Button();
            this.ButtonPrev = new System.Windows.Forms.Button();
            this.ButtonNext = new System.Windows.Forms.Button();
            this.ButtonMinimize = new System.Windows.Forms.Button();
            this.ButtonRelating = new System.Windows.Forms.Button();
            this.ButtonEdit = new System.Windows.Forms.Button();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.ButtonSearchOn = new System.Windows.Forms.Button();
            this.MaskedTextBoxDescription = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // ComboBoxSearchOn
            // 
            this.ComboBoxSearchOn.FormattingEnabled = true;
            this.ComboBoxSearchOn.Location = new System.Drawing.Point(112, 38);
            this.ComboBoxSearchOn.Name = "ComboBoxSearchOn";
            this.ComboBoxSearchOn.Size = new System.Drawing.Size(240, 21);
            this.ComboBoxSearchOn.TabIndex = 0;
            this.ComboBoxSearchOn.Enter += new System.EventHandler(this.ComboBoxSearchOn_GotFocus);
            // 
            // ButtonFirst
            // 
            this.ButtonFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonFirst.Location = new System.Drawing.Point(113, 91);
            this.ButtonFirst.Name = "ButtonFirst";
            this.ButtonFirst.Size = new System.Drawing.Size(63, 23);
            this.ButtonFirst.TabIndex = 1;
            this.ButtonFirst.Text = "&Eerste";
            this.ButtonFirst.UseVisualStyleBackColor = true;
            this.ButtonFirst.Click += new System.EventHandler(this.ButtonFirst_Click);
            // 
            // MasketEditBoxInfo
            // 
            this.MasketEditBoxInfo.Location = new System.Drawing.Point(113, 12);
            this.MasketEditBoxInfo.Name = "MasketEditBoxInfo";
            this.MasketEditBoxInfo.Size = new System.Drawing.Size(240, 20);
            this.MasketEditBoxInfo.TabIndex = 2;
            // 
            // ButtonLast
            // 
            this.ButtonLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonLast.Location = new System.Drawing.Point(113, 120);
            this.ButtonLast.Name = "ButtonLast";
            this.ButtonLast.Size = new System.Drawing.Size(63, 23);
            this.ButtonLast.TabIndex = 3;
            this.ButtonLast.Text = "&Laatste";
            this.ButtonLast.UseVisualStyleBackColor = true;
            this.ButtonLast.Click += new System.EventHandler(this.ButtonLast_Click);
            // 
            // ButtonPrev
            // 
            this.ButtonPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPrev.Location = new System.Drawing.Point(182, 120);
            this.ButtonPrev.Name = "ButtonPrev";
            this.ButtonPrev.Size = new System.Drawing.Size(63, 23);
            this.ButtonPrev.TabIndex = 5;
            this.ButtonPrev.Text = "&Vorige";
            this.ButtonPrev.UseVisualStyleBackColor = true;
            this.ButtonPrev.Click += new System.EventHandler(this.ButtonPrev_Click);
            // 
            // ButtonNext
            // 
            this.ButtonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonNext.Location = new System.Drawing.Point(182, 91);
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.Size = new System.Drawing.Size(63, 23);
            this.ButtonNext.TabIndex = 4;
            this.ButtonNext.Text = "&Volgende";
            this.ButtonNext.UseVisualStyleBackColor = true;
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // ButtonMinimize
            // 
            this.ButtonMinimize.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonMinimize.Location = new System.Drawing.Point(260, 120);
            this.ButtonMinimize.Name = "ButtonMinimize";
            this.ButtonMinimize.Size = new System.Drawing.Size(93, 23);
            this.ButtonMinimize.TabIndex = 7;
            this.ButtonMinimize.Text = "&Minimaliseren";
            this.ButtonMinimize.UseVisualStyleBackColor = true;
            this.ButtonMinimize.Click += new System.EventHandler(this.ButtonMinimize_Click);
            // 
            // ButtonRelating
            // 
            this.ButtonRelating.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonRelating.Location = new System.Drawing.Point(260, 91);
            this.ButtonRelating.Name = "ButtonRelating";
            this.ButtonRelating.Size = new System.Drawing.Size(93, 23);
            this.ButtonRelating.TabIndex = 6;
            this.ButtonRelating.Text = "&Relaties";
            this.ButtonRelating.UseVisualStyleBackColor = true;
            this.ButtonRelating.Click += new System.EventHandler(this.ButtonRelating_Click);
            // 
            // ButtonEdit
            // 
            this.ButtonEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonEdit.Location = new System.Drawing.Point(12, 12);
            this.ButtonEdit.Name = "ButtonEdit";
            this.ButtonEdit.Size = new System.Drawing.Size(93, 23);
            this.ButtonEdit.TabIndex = 8;
            this.ButtonEdit.Text = "&Bewerken";
            this.ButtonEdit.UseVisualStyleBackColor = true;
            this.ButtonEdit.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ButtonRemove.Enabled = false;
            this.ButtonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonRemove.Location = new System.Drawing.Point(12, 91);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(93, 23);
            this.ButtonRemove.TabIndex = 9;
            this.ButtonRemove.TabStop = false;
            this.ButtonRemove.Text = "Verwijderen";
            this.ButtonRemove.UseVisualStyleBackColor = false;
            this.ButtonRemove.Visible = false;
            this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // ButtonSearchOn
            // 
            this.ButtonSearchOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonSearchOn.Location = new System.Drawing.Point(12, 38);
            this.ButtonSearchOn.Name = "ButtonSearchOn";
            this.ButtonSearchOn.Size = new System.Drawing.Size(93, 23);
            this.ButtonSearchOn.TabIndex = 10;
            this.ButtonSearchOn.Text = "&Zoeken op ...";
            this.ButtonSearchOn.UseVisualStyleBackColor = true;
            this.ButtonSearchOn.Click += new System.EventHandler(this.ButtonSearchOn_Click);
            // 
            // MaskedTextBoxDescription
            // 
            this.MaskedTextBoxDescription.Enabled = false;
            this.MaskedTextBoxDescription.Location = new System.Drawing.Point(14, 65);
            this.MaskedTextBoxDescription.Name = "MaskedTextBoxDescription";
            this.MaskedTextBoxDescription.Size = new System.Drawing.Size(338, 20);
            this.MaskedTextBoxDescription.TabIndex = 11;
            // 
            // FormBasicTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonMinimize;
            this.ClientSize = new System.Drawing.Size(364, 154);
            this.ControlBox = false;
            this.Controls.Add(this.MaskedTextBoxDescription);
            this.Controls.Add(this.ButtonSearchOn);
            this.Controls.Add(this.ButtonRemove);
            this.Controls.Add(this.ButtonEdit);
            this.Controls.Add(this.ButtonMinimize);
            this.Controls.Add(this.ButtonRelating);
            this.Controls.Add(this.ButtonPrev);
            this.Controls.Add(this.ButtonNext);
            this.Controls.Add(this.ButtonLast);
            this.Controls.Add(this.MasketEditBoxInfo);
            this.Controls.Add(this.ButtonFirst);
            this.Controls.Add(this.ComboBoxSearchOn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormBasicTable";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "BasisFiche";
            this.Activated += new System.EventHandler(this.FormBasicFile_Activated);
            this.Load += new System.EventHandler(this.FormBasisFiche_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBoxSearchOn;
        private System.Windows.Forms.Button ButtonFirst;
        public System.Windows.Forms.MaskedTextBox MasketEditBoxInfo;
        private Button ButtonLast;
        private Button ButtonPrev;
        private Button ButtonNext;
        private Button ButtonMinimize;
        private Button ButtonRelating;
        public Button ButtonEdit;
        private Button ButtonRemove;
        private Button ButtonSearchOn;
        private MaskedTextBox MaskedTextBoxDescription;
    }
}