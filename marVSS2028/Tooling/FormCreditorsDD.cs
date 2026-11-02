using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace marVSS2028.Tooling
{
    public partial class FormCreditorsDD : Form
    {
        private List<DomEntry> domEntries;
        private DomEntry selectedDomEntry;
        private DomCompany domCompany;
        private bool loaded;
        private int entryCount;
        private decimal totalAmount;

        public FormCreditorsDD()
        {
            InitializeComponent();
            domEntries = new List<DomEntry>();
            selectedDomEntry = new DomEntry();
            domCompany = new DomCompany();
            loaded = false;
            entryCount = 0;
            totalAmount = 0m;
        }

        private void FormCreditorsDD_Load(object sender, EventArgs e)
        {
            InitializeControls();
            LoadEntriesData();
        }

        private void InitializeControls()
        {
            dgvEntries.ColumnCount = 10;
            dgvEntries.Columns[0].Name = "ID";
            dgvEntries.Columns[0].Visible = false;
            dgvEntries.Columns[1].Name = "Amount";
            dgvEntries.Columns[2].Name = "End to End Ref.";
            dgvEntries.Columns[3].Name = "Communication";
            dgvEntries.Columns[4].Name = "Mandate ID";
            dgvEntries.Columns[5].Name = "Mandate Date";
            dgvEntries.Columns[6].Name = "Client Name";
            dgvEntries.Columns[7].Name = "Client IBAN";
            dgvEntries.Columns[8].Name = "Client BIC";
            dgvEntries.Columns[9].Name = "Client Town";

            dgvEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEntries.AllowUserToAddRows = false;
        }

        private void LoadEntriesData()
        {
            domEntries.Clear();
            string jsonData = LoadFromLocalStorage("cddEntries_Template");

            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    domEntries = DeserializeDomEntries(jsonData);
                }
                catch
                {
                    MessageBox.Show("Error loading entries data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    domEntries = new List<DomEntry>();
                }
            }

            loaded = true;
            totalAmount = 0m;
            entryCount = domEntries.Count;

            if (entryCount > 0)
            {
                for (int i = 0; i < domEntries.Count; i++)
                {
                    domEntries[i].EndToEndReference = (DateTime.Now.Ticks + i * 2).ToString();
                    totalAmount += domEntries[i].Amount;
                }
            }

            RefreshDataGrid();
            UpdateSummary();
        }

        private void RefreshDataGrid()
        {
            dgvEntries.Rows.Clear();

            foreach (var entry in domEntries)
            {
                dgvEntries.Rows.Add(
                    entry.Id,
                    entry.Amount.ToString("C2"),
                    entry.EndToEndReference,
                    entry.Communication,
                    entry.MandateId,
                    entry.MandateStartDate?.ToString("yyyy-MM-dd"),
                    entry.ClientName,
                    entry.ClientIban,
                    entry.ClientBic ?? "",
                    entry.ClientTown ?? ""
                );
            }
        }

        private void UpdateSummary()
        {
            lblEntryCount.Text = $"Count: {entryCount}";
            lblTotalAmount.Text = $"Total: {totalAmount:C2}";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var entry = new DomEntry
            {
                Id = GenerateGuid(),
                Amount = 0m,
                EndToEndReference = "",
                Communication = "",
                MandateId = "",
                MandateStartDate = null,
                ClientName = "",
                ClientIban = "",
                ClientBic = "",
                ClientStreet = "",
                ClientPostalcode = "",
                ClientTown = "",
                ClientCountry = ""
            };

            domEntries.Insert(0, entry);
            SaveToLocalStorage("cddEntries_Template", SerializeDomEntries(domEntries));
            LoadEntriesData();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEntries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an entry to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedIndex = dgvEntries.SelectedRows[0].Index;
            selectedDomEntry = domEntries[selectedIndex];

            using (var editForm = new FormCreditorsddEdit(selectedDomEntry))
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    domEntries[selectedIndex] = editForm.GetUpdatedEntry();
                    SaveToLocalStorage("cddEntries_Template", SerializeDomEntries(domEntries));
                    LoadEntriesData();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEntries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an entry to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this entry?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int selectedIndex = dgvEntries.SelectedRows[0].Index;
                domEntries.RemoveAt(selectedIndex);
                SaveToLocalStorage("cddEntries_Template", SerializeDomEntries(domEntries));
                LoadEntriesData();
            }
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            if (domEntries.Count == 0)
            {
                MessageBox.Show("No entries to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete ALL entries?", "Confirm Delete All", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RemoveFromLocalStorage("cddEntries_Template");
                LoadEntriesData();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "ZIP files (*.zip)|*.zip",
                FileName = $"creditors_dd_{DateTime.Now:yyyyMMdd_HHmmss}.zip"
            };

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    BackupDomEntriesToZip(saveFileDialog.FileName, domEntries);
                    MessageBox.Show("Backup saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving backup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "ZIP files (*.zip)|*.zip"
            };

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var loadedEntries = RestoreDomEntriesFromZip(openFileDialog.FileName);
                    if (loadedEntries.Count > 0)
                    {
                        domEntries = loadedEntries;
                        SaveToLocalStorage("cddEntries_Template", SerializeDomEntries(domEntries));
                        LoadEntriesData();
                        MessageBox.Show("Entries loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading backup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML files (*.xml)|*.xml",
                FileName = $"creditors_dd_{DateTime.Now:yyyyMMdd_HHmmss}.xml"
            };

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    ExportDomEntriesToXml(saveFileDialog.FileName, domEntries);
                    MessageBox.Show("Entries exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting entries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        private string LoadFromLocalStorage(string key)
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string storePath = System.IO.Path.Combine(localAppDataPath, "marVSS2028", "storage");
                string filePath = System.IO.Path.Combine(storePath, key + ".json");

                if (System.IO.File.Exists(filePath))
                {
                    return System.IO.File.ReadAllText(filePath);
                }
            }
            catch { }

            return string.Empty;
        }

        private void SaveToLocalStorage(string key, string value)
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string storePath = System.IO.Path.Combine(localAppDataPath, "marVSS2028", "storage");

                if (!System.IO.Directory.Exists(storePath))
                {
                    System.IO.Directory.CreateDirectory(storePath);
                }

                string filePath = System.IO.Path.Combine(storePath, key + ".json");
                System.IO.File.WriteAllText(filePath, value);
            }
            catch { }
        }

        private void RemoveFromLocalStorage(string key)
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string storePath = System.IO.Path.Combine(localAppDataPath, "marVSS2028", "storage");
                string filePath = System.IO.Path.Combine(storePath, key + ".json");

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch { }
        }

        private List<DomEntry> DeserializeDomEntries(string json)
        {
            var entries = new List<DomEntry>();
            // Simple JSON parsing - in production, use Newtonsoft.Json or System.Text.Json
            // This is a placeholder implementation
            return entries;
        }

        private string SerializeDomEntries(List<DomEntry> entries)
        {
            // Simple JSON serialization - in production, use Newtonsoft.Json or System.Text.Json
            // This is a placeholder implementation
            return "[]";
        }

        private void BackupDomEntriesToZip(string filePath, List<DomEntry> entries)
        {
            // Implementation for ZIP backup
            // Would use System.IO.Compression.ZipFile
        }

        private List<DomEntry> RestoreDomEntriesFromZip(string filePath)
        {
            // Implementation for ZIP restore
            // Would use System.IO.Compression.ZipFile
            return new List<DomEntry>();
        }

        private void ExportDomEntriesToXml(string filePath, List<DomEntry> entries)
        {
            // Implementation for XML export (SEPA Direct Debit format)
            // This would generate proper SEPA XML structure
        }
    }

    /// <summary>
    /// Represents a Direct Debit entry (Domiciliëring)
    /// </summary>
    public class DomEntry
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string EndToEndReference { get; set; }
        public string Communication { get; set; }
        public string MandateId { get; set; }
        public DateTime? MandateStartDate { get; set; }
        public string ClientName { get; set; }
        public string ClientIban { get; set; }
        public string ClientBic { get; set; }
        public string ClientStreet { get; set; }
        public string ClientPostalcode { get; set; }
        public string ClientTown { get; set; }
        public string ClientCountry { get; set; }
    }

    /// <summary>
    /// Represents the company/creditor information
    /// </summary>
    public class DomCompany
    {
        public string Name { get; set; }
        public string EnterpriseNumber { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PcPlace { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string DomId { get; set; }
    }
}
