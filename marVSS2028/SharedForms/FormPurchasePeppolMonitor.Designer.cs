using System.Windows.Forms;

namespace marVSS2028.SharedForms
{
    partial class FormPurchasePeppolMonitor
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
            this.ButtonShowToBookXML = new System.Windows.Forms.Button();
            this.ButtonShowBookedXML = new System.Windows.Forms.Button();
            this.ButtonShowPeppolDocTypes = new System.Windows.Forms.Button();
            this.ButtonSentReceiptSeller = new System.Windows.Forms.Button();
            this.ButtonResponsesToSeller = new System.Windows.Forms.Button();
            this.ButtonLoadDocument = new System.Windows.Forms.Button();
            this.ButtonShowBookedPDF = new System.Windows.Forms.Button();
            this.ButtonShowToBookPDF = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.mfgToBook = new System.Windows.Forms.DataGridView();
            this.mfgBooked = new System.Windows.Forms.DataGridView();
            this.LabelBooked = new System.Windows.Forms.Label();
            this.LabelToBook = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mfgToBook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mfgBooked)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonShowToBookXML
            // 
            this.ButtonShowToBookXML.Enabled = false;
            this.ButtonShowToBookXML.Location = new System.Drawing.Point(160, 8);
            this.ButtonShowToBookXML.Name = "ButtonShowToBookXML";
            this.ButtonShowToBookXML.Size = new System.Drawing.Size(81, 25);
            this.ButtonShowToBookXML.TabIndex = 12;
            this.ButtonShowToBookXML.Text = "XML Tonen";
            this.ButtonShowToBookXML.UseVisualStyleBackColor = true;
            this.ButtonShowToBookXML.Click += new System.EventHandler(this.ButtonShowToBookXML_Click);
            // 
            // ButtonShowBookedXML
            // 
            this.ButtonShowBookedXML.Enabled = false;
            this.ButtonShowBookedXML.Location = new System.Drawing.Point(16, 448);
            this.ButtonShowBookedXML.Name = "ButtonShowBookedXML";
            this.ButtonShowBookedXML.Size = new System.Drawing.Size(81, 25);
            this.ButtonShowBookedXML.TabIndex = 11;
            this.ButtonShowBookedXML.Text = "XML Tonen";
            this.ButtonShowBookedXML.UseVisualStyleBackColor = true;
            this.ButtonShowBookedXML.Click += new System.EventHandler(this.ButtonShowBookedXML_Click);
            // 
            // ButtonShowPeppolDocTypes
            // 
            this.ButtonShowPeppolDocTypes.Location = new System.Drawing.Point(384, 8);
            this.ButtonShowPeppolDocTypes.Name = "ButtonShowPeppolDocTypes";
            this.ButtonShowPeppolDocTypes.Size = new System.Drawing.Size(169, 25);
            this.ButtonShowPeppolDocTypes.TabIndex = 10;
            this.ButtonShowPeppolDocTypes.Text = "Peppol Document types tonen";
            this.ButtonShowPeppolDocTypes.UseVisualStyleBackColor = true;
            this.ButtonShowPeppolDocTypes.Click += new System.EventHandler(this.ButtonShowPeppolDocTypes_Click);
            // 
            // ButtonSentReceiptSeller
            // 
            this.ButtonSentReceiptSeller.Enabled = false;
            this.ButtonSentReceiptSeller.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonSentReceiptSeller.Location = new System.Drawing.Point(224, 448);
            this.ButtonSentReceiptSeller.Name = "ButtonSentReceiptSeller";
            this.ButtonSentReceiptSeller.Size = new System.Drawing.Size(137, 25);
            this.ButtonSentReceiptSeller.TabIndex = 9;
            this.ButtonSentReceiptSeller.Text = "Ontvangstbewijs";
            this.ButtonSentReceiptSeller.UseVisualStyleBackColor = true;
            this.ButtonSentReceiptSeller.Click += new System.EventHandler(this.ButtonSentReceiptSeller_Click);
            // 
            // ButtonResponsesToSeller
            // 
            this.ButtonResponsesToSeller.Enabled = false;
            this.ButtonResponsesToSeller.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonResponsesToSeller.Location = new System.Drawing.Point(368, 448);
            this.ButtonResponsesToSeller.Name = "ButtonResponsesToSeller";
            this.ButtonResponsesToSeller.Size = new System.Drawing.Size(185, 25);
            this.ButtonResponsesToSeller.TabIndex = 8;
            this.ButtonResponsesToSeller.Text = "Reacties (aan leverancier)";
            this.ButtonResponsesToSeller.UseVisualStyleBackColor = true;
            this.ButtonResponsesToSeller.Click += new System.EventHandler(this.ButtonResponsesToSeller_Click);
            // 
            // ButtonLoadDocument
            // 
            this.ButtonLoadDocument.Enabled = false;
            this.ButtonLoadDocument.Location = new System.Drawing.Point(16, 8);
            this.ButtonLoadDocument.Name = "ButtonLoadDocument";
            this.ButtonLoadDocument.Size = new System.Drawing.Size(129, 25);
            this.ButtonLoadDocument.TabIndex = 4;
            this.ButtonLoadDocument.Text = "Document Inladen";
            this.ButtonLoadDocument.UseVisualStyleBackColor = true;
            this.ButtonLoadDocument.Click += new System.EventHandler(this.ButtonLoadDocument_Click);
            // 
            // ButtonShowBookedPDF
            // 
            this.ButtonShowBookedPDF.Enabled = false;
            this.ButtonShowBookedPDF.Location = new System.Drawing.Point(112, 448);
            this.ButtonShowBookedPDF.Name = "ButtonShowBookedPDF";
            this.ButtonShowBookedPDF.Size = new System.Drawing.Size(81, 25);
            this.ButtonShowBookedPDF.TabIndex = 3;
            this.ButtonShowBookedPDF.Text = "PDF Tonen";
            this.ButtonShowBookedPDF.UseVisualStyleBackColor = true;
            this.ButtonShowBookedPDF.Click += new System.EventHandler(this.ButtonShowBookedPDF_Click);
            // 
            // ButtonShowToBookPDF
            // 
            this.ButtonShowToBookPDF.Enabled = false;
            this.ButtonShowToBookPDF.Location = new System.Drawing.Point(256, 8);
            this.ButtonShowToBookPDF.Name = "ButtonShowToBookPDF";
            this.ButtonShowToBookPDF.Size = new System.Drawing.Size(81, 25);
            this.ButtonShowToBookPDF.TabIndex = 2;
            this.ButtonShowToBookPDF.Text = "PDF Tonen";
            this.ButtonShowToBookPDF.UseVisualStyleBackColor = true;
            this.ButtonShowToBookPDF.Click += new System.EventHandler(this.ButtonShowToBookPDF_Click);
            // 
            // ButtonClose
            // 
            this.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonClose.Location = new System.Drawing.Point(560, 448);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(81, 25);
            this.ButtonClose.TabIndex = 5;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // mfgToBook
            // 
            this.mfgToBook.AllowUserToAddRows = false;
            this.mfgToBook.AllowUserToDeleteRows = false;
            this.mfgToBook.AllowUserToResizeRows = false;
            this.mfgToBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mfgToBook.Location = new System.Drawing.Point(16, 40);
            this.mfgToBook.MultiSelect = false;
            this.mfgToBook.Name = "mfgToBook";
            this.mfgToBook.ReadOnly = true;
            this.mfgToBook.RowHeadersVisible = false;
            this.mfgToBook.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mfgToBook.Size = new System.Drawing.Size(625, 185);
            this.mfgToBook.TabIndex = 0;
            this.mfgToBook.SelectionChanged += new System.EventHandler(this.mfgToBook_SelectionChanged);
            this.mfgToBook.Enter += new System.EventHandler(this.mfgToBook_Enter);
            this.mfgToBook.GotFocus += new System.EventHandler(this.mfgToBook_GotFocus);
            // 
            // mfgBooked
            // 
            this.mfgBooked.AllowUserToAddRows = false;
            this.mfgBooked.AllowUserToDeleteRows = false;
            this.mfgBooked.AllowUserToResizeRows = false;
            this.mfgBooked.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mfgBooked.Location = new System.Drawing.Point(16, 256);
            this.mfgBooked.MultiSelect = false;
            this.mfgBooked.Name = "mfgBooked";
            this.mfgBooked.ReadOnly = true;
            this.mfgBooked.RowHeadersVisible = false;
            this.mfgBooked.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mfgBooked.Size = new System.Drawing.Size(625, 185);
            this.mfgBooked.TabIndex = 1;
            this.mfgBooked.SelectionChanged += new System.EventHandler(this.mfgBooked_SelectionChanged);
            this.mfgBooked.GotFocus += new System.EventHandler(this.mfgBooked_GotFocus);
            // 
            // LabelBooked
            // 
            this.LabelBooked.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelBooked.Location = new System.Drawing.Point(568, 232);
            this.LabelBooked.Name = "LabelBooked";
            this.LabelBooked.Size = new System.Drawing.Size(73, 17);
            this.LabelBooked.TabIndex = 7;
            this.LabelBooked.Text = "Ingeboekt";
            // 
            // LabelToBook
            // 
            this.LabelToBook.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelToBook.Location = new System.Drawing.Point(560, 16);
            this.LabelToBook.Name = "LabelToBook";
            this.LabelToBook.Size = new System.Drawing.Size(81, 17);
            this.LabelToBook.TabIndex = 6;
            this.LabelToBook.Text = "In te boeken";
            // 
            // FormPurchasePeppolMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(658, 485);
            this.ControlBox = false;
            this.Controls.Add(this.LabelToBook);
            this.Controls.Add(this.LabelBooked);
            this.Controls.Add(this.mfgBooked);
            this.Controls.Add(this.mfgToBook);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonShowToBookPDF);
            this.Controls.Add(this.ButtonShowBookedPDF);
            this.Controls.Add(this.ButtonLoadDocument);
            this.Controls.Add(this.ButtonResponsesToSeller);
            this.Controls.Add(this.ButtonSentReceiptSeller);
            this.Controls.Add(this.ButtonShowPeppolDocTypes);
            this.Controls.Add(this.ButtonShowBookedXML);
            this.Controls.Add(this.ButtonShowToBookXML);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPurchasePeppolMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Monitor B2B Aankoopdocumenten";
            ((System.ComponentModel.ISupportInitialize)(this.mfgToBook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mfgBooked)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button ButtonShowToBookXML;
        private Button ButtonShowBookedXML;
        private Button ButtonShowPeppolDocTypes;
        private Button ButtonSentReceiptSeller;
        private Button ButtonResponsesToSeller;
        private Button ButtonLoadDocument;
        private Button ButtonShowBookedPDF;
        private Button ButtonShowToBookPDF;
        private Button ButtonClose;
        private DataGridView mfgToBook;
        private DataGridView mfgBooked;
        private Label LabelBooked;
        private Label LabelToBook;
    }
}
