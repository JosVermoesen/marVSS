namespace marVSS2028.MimMenu.Filing
{
    partial class FormProductGroups
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
            this.CbGroepDefinitie      = new System.Windows.Forms.ComboBox();
            this.BtnGroepToevoegen     = new System.Windows.Forms.Button();
            this.BtnItemsWijzigen      = new System.Windows.Forms.Button();
            this.TbGroepItem           = new System.Windows.Forms.TextBox();
            this.BtnGroepItemToevoegen = new System.Windows.Forms.Button();
            this.LbGroepItems          = new System.Windows.Forms.ListBox();
            this.BtnBewaren            = new System.Windows.Forms.Button();
            this.BtnSluiten            = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // CbGroepDefinitie
            this.CbGroepDefinitie.BackColor     = System.Drawing.Color.FromArgb(0xC0, 0xFF, 0xC0);
            this.CbGroepDefinitie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.CbGroepDefinitie.Location      = new System.Drawing.Point(8, 8);
            this.CbGroepDefinitie.Name          = "CbGroepDefinitie";
            this.CbGroepDefinitie.Size          = new System.Drawing.Size(225, 64);
            this.CbGroepDefinitie.TabIndex      = 1;
            this.CbGroepDefinitie.SelectedIndexChanged += new System.EventHandler(this.CbGroepDefinitie_SelectedIndexChanged);
            // BtnGroepToevoegen
            this.BtnGroepToevoegen.Location = new System.Drawing.Point(240, 8);
            this.BtnGroepToevoegen.Name     = "BtnGroepToevoegen";
            this.BtnGroepToevoegen.Size     = new System.Drawing.Size(129, 28);
            this.BtnGroepToevoegen.TabIndex = 2;
            this.BtnGroepToevoegen.Text     = "&Groep Bijvoegen";
            this.BtnGroepToevoegen.Visible  = false;
            this.BtnGroepToevoegen.Click   += new System.EventHandler(this.BtnGroepToevoegen_Click);
            // BtnItemsWijzigen
            this.BtnItemsWijzigen.Location = new System.Drawing.Point(240, 40);
            this.BtnItemsWijzigen.Name     = "BtnItemsWijzigen";
            this.BtnItemsWijzigen.Size     = new System.Drawing.Size(129, 28);
            this.BtnItemsWijzigen.TabIndex = 6;
            this.BtnItemsWijzigen.Text     = "Items &Wijzigen";
            this.BtnItemsWijzigen.Click   += new System.EventHandler(this.BtnItemsWijzigen_Click);
            // TbGroepItem
            this.TbGroepItem.Enabled  = false;
            this.TbGroepItem.Location = new System.Drawing.Point(8, 80);
            this.TbGroepItem.Name     = "TbGroepItem";
            this.TbGroepItem.Size     = new System.Drawing.Size(225, 20);
            this.TbGroepItem.TabIndex = 3;
            // BtnGroepItemToevoegen
            this.BtnGroepItemToevoegen.Enabled  = false;
            this.BtnGroepItemToevoegen.Location  = new System.Drawing.Point(240, 80);
            this.BtnGroepItemToevoegen.Name      = "BtnGroepItemToevoegen";
            this.BtnGroepItemToevoegen.Size      = new System.Drawing.Size(129, 28);
            this.BtnGroepItemToevoegen.TabIndex  = 4;
            this.BtnGroepItemToevoegen.TabStop   = false;
            this.BtnGroepItemToevoegen.Text      = "&Keuze Bijvoegen";
            this.BtnGroepItemToevoegen.Click    += new System.EventHandler(this.BtnGroepItemToevoegen_Click);
            // LbGroepItems
            this.LbGroepItems.BackColor  = System.Drawing.Color.FromArgb(0xE0, 0xE0, 0xE0);
            this.LbGroepItems.Enabled    = false;
            this.LbGroepItems.Location   = new System.Drawing.Point(8, 112);
            this.LbGroepItems.Name       = "LbGroepItems";
            this.LbGroepItems.Size       = new System.Drawing.Size(225, 95);
            this.LbGroepItems.TabIndex   = 0;
            this.LbGroepItems.KeyDown   += new System.Windows.Forms.KeyEventHandler(this.LbGroepItems_KeyDown);
            // BtnBewaren
            this.BtnBewaren.Enabled  = false;
            this.BtnBewaren.Location = new System.Drawing.Point(240, 136);
            this.BtnBewaren.Name     = "BtnBewaren";
            this.BtnBewaren.Size     = new System.Drawing.Size(129, 28);
            this.BtnBewaren.TabIndex = 5;
            this.BtnBewaren.Text     = "&Bewaren";
            this.BtnBewaren.Click   += new System.EventHandler(this.BtnBewaren_Click);
            // BtnSluiten
            this.BtnSluiten.CausesValidation = false;
            this.BtnSluiten.Location         = new System.Drawing.Point(240, 178);
            this.BtnSluiten.Name             = "BtnSluiten";
            this.BtnSluiten.Size             = new System.Drawing.Size(129, 33);
            this.BtnSluiten.TabIndex         = 7;
            this.BtnSluiten.TabStop          = false;
            this.BtnSluiten.Text             = "Sluiten";
            this.BtnSluiten.Click           += new System.EventHandler(this.BtnSluiten_Click);
            // FormProductGroups
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton        = this.BtnSluiten;
            this.ClientSize          = new System.Drawing.Size(376, 220);
            this.Controls.Add(this.CbGroepDefinitie);
            this.Controls.Add(this.BtnGroepToevoegen);
            this.Controls.Add(this.BtnItemsWijzigen);
            this.Controls.Add(this.TbGroepItem);
            this.Controls.Add(this.BtnGroepItemToevoegen);
            this.Controls.Add(this.LbGroepItems);
            this.Controls.Add(this.BtnBewaren);
            this.Controls.Add(this.BtnSluiten);
            this.FormBorderStyle   = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox       = false;
            this.MinimizeBox       = false;
            this.Name              = "FormProductGroups";
            this.ShowInTaskbar     = false;
            this.StartPosition     = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text              = "ProductGroepen";
            this.Load             += new System.EventHandler(this.FormProductGroups_Load);
            this.FormClosed       += new System.Windows.Forms.FormClosedEventHandler(this.FormProductGroups_FormClosed);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox CbGroepDefinitie;
        private System.Windows.Forms.Button   BtnGroepToevoegen;
        private System.Windows.Forms.Button   BtnItemsWijzigen;
        private System.Windows.Forms.TextBox  TbGroepItem;
        private System.Windows.Forms.Button   BtnGroepItemToevoegen;
        private System.Windows.Forms.ListBox  LbGroepItems;
        private System.Windows.Forms.Button   BtnBewaren;
        private System.Windows.Forms.Button   BtnSluiten;
    }
}