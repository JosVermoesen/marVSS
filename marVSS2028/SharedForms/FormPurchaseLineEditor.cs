using System;
using System.Collections.Generic;
using System.Globalization;
using System.Media;
using System.Windows.Forms;
using marVSS2028.Classes;
using marVSS2028.MimMenu.DailyManagement;
using marVSS2028.PublicForms;
using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.SharedForms
{
    public partial class FormPurchaseLineEditor : Form
    {
        private readonly TextBox[] _tekstInfo;
        private bool _ctrlFlag;
        private int _tabIndex;
        private bool _isB2BPeppol;
        private string _pctFilter = string.Empty;

        private string _veldRekening = string.Empty;
        private string _veldNaam = string.Empty;
        private string _veldBedrag = string.Empty;
        private string _veldAantal = string.Empty;
        private string _veldProdukt = string.Empty;
        private string _produktNaam = string.Empty;

        public bool StockBeheerChecked { get; set; }
        public bool OpenB2BDetailsOnLoad { get; set; }
        public int SelectedDocumentLineIndex { get; set; }

        public FormPurchaseLineEditor()
        {
            InitializeComponent();
            _tekstInfo = new[] { TekstInfo0, TekstInfo1, TekstInfo2, TekstInfo3, TekstInfo4, TekstInfo5, TekstInfo6 };
            cbEenheidsType.Items.AddRange(new object[] { "1: Eenheid", "2: Verpakking" });
            BindTekstInfoEvents();
        }

        private void BindTekstInfoEvents()
        {
            for (int i = 0; i < _tekstInfo.Length; i++)
            {
                _tekstInfo[i].Tag = i;
                _tekstInfo[i].TextChanged += TekstInfo_TextChanged;
                _tekstInfo[i].Enter += TekstInfo_Enter;
                _tekstInfo[i].KeyDown += TekstInfo_KeyDown;
                _tekstInfo[i].KeyUp += TekstInfo_KeyUp;
                _tekstInfo[i].Leave += TekstInfo_Leave;
            }
        }

        private void FormPurchaseLineEditor_Load(object sender, EventArgs e)
        {
            ConfigureB2BState();
            RestoreFilterState();

            if (StockBeheerChecked)
            {
                Labelstock0.Visible = true;
                cbEenheidsType.Visible = true;
                cbEenheidsType.SelectedIndex = 0;
                for (int i = 3; i <= 5; i++)
                {
                    _tekstInfo[i].Visible = true;
                }
            }

            if (!string.IsNullOrEmpty(GridText))
            {
                LoadFromGridText();
            }
        }

        private void ConfigureB2BState()
        {
            _isB2BPeppol = !string.IsNullOrEmpty(Globals.documentLinesDATA);
            Width = 404; 
            Height = 227;

            if (_isB2BPeppol)
            {
                _tekstInfo[2].Enabled = false;
                ButtonShowB2BDetails.Visible = true;

                if (StockBeheerChecked && OpenB2BDetailsOnLoad)
                {
                    ButtonShowB2BDetails_Click(this, EventArgs.Empty);
                }

                FillB2BLineItems();
            }
            else
            {
                _tekstInfo[2].Enabled = true;
                ButtonShowB2BDetails.Visible = false;
            }
        }

        private void FillB2BLineItems()
        {
            ListBoxLineItems.Items.Clear();

            string[] lines = documentLinesDATA.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 1)
            {
                return;
            }

            string[] headers = lines[0].Split('\t');
            int selectedIndex = SelectedDocumentLineIndex + 1;
            if (selectedIndex < 1 || selectedIndex >= lines.Length)
            {
                selectedIndex = 1;
            }

            string[] selectedValues = lines[selectedIndex].Split('\t');
            int max = Math.Min(headers.Length, selectedValues.Length);
            for (int i = 0; i < max; i++)
            {
                ListBoxLineItems.Items.Add(headers[i] + ": " + selectedValues[i]);
            }
        }

        private void RestoreFilterState()
        {
            string filterToggle = LaadTekst("Aankoopverrichting", "FilterToggle");
            menuFilterToggle.Checked = IsTrueSetting(filterToggle);
            _pctFilter = LaadTekst("Aankoopverrichting", "FilterPCT") ?? string.Empty;
            ApplyFilterUI();
        }

        private bool IsTrueSetting(string setting)
        {
            if (string.IsNullOrWhiteSpace(setting))
            {
                return false;
            }

            string value = setting.Trim();
            return value == "-1" || value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        private void ApplyFilterUI()
        {
            if (menuFilterToggle.Checked)
            {
                _tekstInfo[6].Visible = true;
                if (!_isB2BPeppol)
                {
                    _tekstInfo[2].Enabled = false;
                }
            }
            else
            {
                _tekstInfo[6].Visible = false;
                if (!_isB2BPeppol)
                {
                    _tekstInfo[2].Enabled = true;
                }
            }

            lPctFilter.Text = _pctFilter;
        }

        private void LoadFromGridText()
        {
            string[] parts = GridText.Split(new[] { '|' }, StringSplitOptions.None);
            int max = Math.Min(_tekstInfo.Length, parts.Length);
            for (int i = 0; i < max; i++)
            {
                _tekstInfo[i].Text = parts[i];
            }

            double quantity;
            if (double.TryParse(_tekstInfo[5].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity))
            {
                _tekstInfo[5].Text = Dec(quantity, "#####0.000");
            }

            if (!string.IsNullOrWhiteSpace(_tekstInfo[0].Text))
            {
                BGet(TABLE_LEDGERACCOUNTS, 0, VSet(_tekstInfo[0].Text, 7));
                if (Ktrl == 0)
                {
                    RecordToVeld(TABLE_LEDGERACCOUNTS);
                    _tekstInfo[0].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
                    _tekstInfo[1].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
                    if (!_isB2BPeppol)
                    {
                        _tekstInfo[2].TabIndex = 0;
                    }
                }
            }
        }

        private void Annuleren_Click(object sender, EventArgs e)
        {
            GridText = string.Empty;
            Close();
        }

        private void ButtonShowB2BDetails_Click(object sender, EventArgs e)
        {
            Width = Width <= 404 ? 753 : 404;
        }

        private void cbEenheidsType_Click(object sender, EventArgs e)
        {
            _tekstInfo[5].Focus();
        }

        private void cbFiche_Click(object sender, EventArgs e)
        {
            // Legacy frmProduktFiche is not available yet in this project.
            // Keep existing behavior close to VB6 by validating the product key.
            string productCode = _tekstInfo[3].Text;
            BGet(TABLE_PRODUCTS, 0, VSet(productCode, 13));
            _tekstInfo[3].Text = productCode.Trim();
            _tekstInfo[3].Focus();
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            if (menuFilterToggle.Checked)
            {
                _pctFilter = Microsoft.VisualBasic.Interaction.InputBox(
                    "Commissie Filter (vb. 25.75)",
                    "Filter aan",
                    string.IsNullOrEmpty(_pctFilter) ? "15" : _pctFilter);
            }
            else
            {
                _pctFilter = string.Empty;
            }

            ApplyFilterUI();
            BeWaarTekst("Aankoopverrichting", "FilterToggle", menuFilterToggle.Checked ? "-1" : "0");
            BeWaarTekst("Aankoopverrichting", "FilterPCT", _pctFilter);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            double hetBedrag;
            if (!double.TryParse(_tekstInfo[2].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out hetBedrag))
            {
                double.TryParse(_tekstInfo[2].Text, NumberStyles.Any, CultureInfo.CurrentCulture, out hetBedrag);
            }

            if (hetBedrag == 0 || string.IsNullOrWhiteSpace(_tekstInfo[1].Text))
            {
                SystemSounds.Beep.Play();
                return;
            }

            _veldRekening = VSet(_tekstInfo[0].Text, 7);
            _veldNaam = VSet(_tekstInfo[1].Text, 40);
            _veldBedrag = Dec(hetBedrag, MASK_EURBH);

            GridText = _veldRekening + "|" + _veldNaam + "|" + _veldBedrag + "|";

            if (_tekstInfo[3].Visible)
            {
                double aantal;
                if (string.IsNullOrWhiteSpace(_tekstInfo[4].Text) ||
                    !double.TryParse(_tekstInfo[5].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out aantal) ||
                    aantal == 0)
                {
                    SystemSounds.Beep.Play();
                    return;
                }

                _veldProdukt = VSet(_tekstInfo[3].Text, 13);
                _veldAantal = Dec(aantal, "#####0.000");
                _produktNaam = VSet(_tekstInfo[4].Text, 40);
                GridText += _veldProdukt + "|" + _produktNaam + "|" + _veldAantal + "|";
            }

            Close();
        }

        private void TekstInfo_TextChanged(object sender, EventArgs e)
        {
            int index = GetTekstInfoIndex(sender);
            if (index == 4)
            {
                cbFiche.Visible = true;
                return;
            }

            if (index == 6)
            {
                double bedragA;
                double filter;
                if (double.TryParse(_tekstInfo[6].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out bedragA) &&
                    double.TryParse(_pctFilter, NumberStyles.Any, CultureInfo.InvariantCulture, out filter))
                {
                    double bedragB = bedragA - (bedragA * filter / 100d);
                    if (!_isB2BPeppol)
                    {
                        _tekstInfo[2].Text = bedragB.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        private void TekstInfo_Enter(object sender, EventArgs e)
        {
            int index = GetTekstInfoIndex(sender);
            TextBox box = _tekstInfo[index];
            box.SelectionStart = 0;
            box.SelectionLength = box.Text.Length;
            _tabIndex = index;

            if (index == 0)
            {
                Ok.Enabled = true;
                SnelHelpPrint("[Ctrl] voor geïndexeerd zoeken", BL_LOGGING);
            }

            _ctrlFlag = false;
        }

        private void TekstInfo_KeyDown(object sender, KeyEventArgs e)
        {
            int index = GetTekstInfoIndex(sender);

            if (e.Control)
            {
                _ctrlFlag = true;
            }

            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            {
                if (index == 0 || index == 5)
                {
                    if (!_isB2BPeppol)
                    {
                        if (_tekstInfo[2].Enabled)
                        {
                            _tekstInfo[2].Focus();
                        }
                        else
                        {
                            _tekstInfo[6].Focus();
                        }
                    }
                }

                if (index == 2 || index == 6)
                {
                    Ok_Click(this, EventArgs.Empty);
                }
                else if (index == 3)
                {
                    _tekstInfo[5].Focus();
                }
            }

            if (e.KeyCode == Keys.ControlKey)
            {
                if (index == 0)
                {
                    OpenLedgerSearch();
                    e.Handled = true;
                }
                else if (index == 3)
                {
                    OpenProductSearch();
                    e.Handled = true;
                }
            }
        }

        private void OpenLedgerSearch()
        {
            SharedFl = TABLE_LEDGERACCOUNTS;
            aIndex = 0;
            GridText = _tekstInfo[0].Text;

            using (FormSearchSQL search = new FormSearchSQL())
            {
                search.ShowDialog(this);
            }

            if (Ktrl != 0)
            {
                _tekstInfo[1].Text = string.Empty;
                Ok.Enabled = false;
                return;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            _tekstInfo[0].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
            _tekstInfo[1].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
            Ok.Enabled = true;
            if (!_isB2BPeppol)
            {
                _tekstInfo[2].Focus();
            }
        }

        private void OpenProductSearch()
        {
            SharedFl = TABLE_PRODUCTS;
            aIndex = 0;
            GridText = _tekstInfo[3].Text;

            using (FormSearchSQL search = new FormSearchSQL())
            {
                search.ShowDialog(this);
            }

            if (Ktrl != 0)
            {
                _tekstInfo[4].Text = string.Empty;
                Ok.Enabled = false;
                return;
            }

            RecordToVeld(TABLE_PRODUCTS);
            _tekstInfo[3].Text = VBibText(TABLE_PRODUCTS, "#v102 #");
            _tekstInfo[4].Text = VBibText(TABLE_PRODUCTS, "#v105 #");
            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(VBibText(TABLE_PRODUCTS, "#v116 #"), 7));
            if (Ktrl != 0)
            {
                _tekstInfo[1].Text = string.Empty;
                Ok.Enabled = false;
                return;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            _tekstInfo[0].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
            _tekstInfo[1].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
            Ok.Enabled = true;
        }

        private void TekstInfo_KeyUp(object sender, KeyEventArgs e)
        {
            int index = GetTekstInfoIndex(sender);
            if (index != 2)
            {
                return;
            }

            if (e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract || e.KeyCode == Keys.Divide)
            {
                if (_tekstInfo[index].Text.Length == 1 || _isB2BPeppol)
                {
                    return;
                }

                string rekenHier = LineCalculating(_tekstInfo[2].Text);
                if (!string.IsNullOrEmpty(rekenHier))
                {
                    double result;
                    if (double.TryParse(rekenHier, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                    {
                        _tekstInfo[2].Text = Math.Round(result, 2).ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        private void TekstInfo_Leave(object sender, EventArgs e)
        {
            int index = GetTekstInfoIndex(sender);
            switch (index)
            {
                case 0:
                    ValidateLedgerOnLeave();
                    break;
                case 3:
                    ValidateProductOnLeave();
                    break;
                case 5:
                    RecalculateQuantityAndAmount();
                    break;
            }

            _ctrlFlag = false;
        }

        private void ValidateLedgerOnLeave()
        {
            if (_ctrlFlag)
            {
                return;
            }

            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(_tekstInfo[0].Text, 7));
            if (Ktrl != 0)
            {
                _tekstInfo[0].Text = string.Empty;
                _tekstInfo[1].Text = string.Empty;
                return;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            _tekstInfo[0].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
            _tekstInfo[1].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
        }

        private void ValidateProductOnLeave()
        {
            if (_ctrlFlag)
            {
                return;
            }

            BGet(TABLE_PRODUCTS, 0, VSet(_tekstInfo[3].Text, 13));
            if (Ktrl != 0)
            {
                DialogResult result = MessageBox.Show(
                    "Code " + _tekstInfo[3].Text + " bestaat niet." + Environment.NewLine + "Nieuw produkt aanmaken",
                    string.Empty,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Productfiche openen is nog niet gemigreerd in deze flow.");
                    _tekstInfo[3].Focus();
                }
                return;
            }

            RecordToVeld(TABLE_PRODUCTS);
            _tekstInfo[3].Text = VBibText(TABLE_PRODUCTS, "#v102 #");
            _tekstInfo[4].Text = VBibText(TABLE_PRODUCTS, "#v105 #");
            BGet(TABLE_LEDGERACCOUNTS, 0, VSet(VBibText(TABLE_PRODUCTS, "#v116 #"), 7));
            if (Ktrl != 0)
            {
                _tekstInfo[1].Text = string.Empty;
                Ok.Enabled = false;
                return;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);
            _tekstInfo[0].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v019 #");
            _tekstInfo[1].Text = VBibText(TABLE_LEDGERACCOUNTS, "#v020 #");
            Ok.Enabled = true;
        }

        private void RecalculateQuantityAndAmount()
        {
            float tmpAantal = 0f;
            string eenheidsType = cbEenheidsType.Text;

            if (eenheidsType.StartsWith("1", StringComparison.Ordinal))
            {
                float.TryParse(_tekstInfo[5].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpAantal);
            }
            else if (eenheidsType.StartsWith("2", StringComparison.Ordinal))
            {
                double qty;
                double factor;
                double.TryParse(_tekstInfo[5].Text, NumberStyles.Any, CultureInfo.InvariantCulture, out qty);
                double.TryParse(VBibText(TABLE_PRODUCTS, "#v107 #"), NumberStyles.Any, CultureInfo.InvariantCulture, out factor);
                tmpAantal = (float)(qty * factor);
                _tekstInfo[5].Text = Dec(tmpAantal, "####.000").Trim();
                cbEenheidsType.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("stop");
            }

            if (!_isB2BPeppol)
            {
                double productPrice;
                double.TryParse(VBibText(TABLE_PRODUCTS, "#e113 #"), NumberStyles.Any, CultureInfo.InvariantCulture, out productPrice);
                double amount = Math.Round(tmpAantal * productPrice, 2);
                _tekstInfo[2].Text = Dec(amount, MASK_EURBH);
                Application.DoEvents();
                _tekstInfo[2].Focus();
            }
        }

        private int GetTekstInfoIndex(object sender)
        {
            TextBox box = sender as TextBox;
            if (box == null || box.Tag == null)
            {
                return 0;
            }

            return (int)box.Tag;
        }
    }
}
