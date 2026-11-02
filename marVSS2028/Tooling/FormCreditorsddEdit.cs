using System;
using System.Windows.Forms;

namespace marVSS2028.Tooling
{
    public partial class FormCreditorsddEdit : Form
    {
        private DomEntry entry;

        public FormCreditorsddEdit(DomEntry domEntry)
        {
            InitializeComponent();
            entry = domEntry;
        }

        private void FormCreditorsddEdit_Load(object sender, EventArgs e)
        {
            PopulateFields();
        }

        private void PopulateFields()
        {
            txtId.Text = entry.Id;
            txtId.ReadOnly = true;

            numAmount.Value = (decimal)entry.Amount;
            txtEndToEndRef.Text = entry.EndToEndReference ?? "";
            txtCommunication.Text = entry.Communication ?? "";
            txtMandateId.Text = entry.MandateId ?? "";
            dtpMandateDate.Value = entry.MandateStartDate ?? DateTime.Today;
            txtClientName.Text = entry.ClientName ?? "";
            txtClientIban.Text = entry.ClientIban ?? "";
            txtClientBic.Text = entry.ClientBic ?? "";
            txtClientStreet.Text = entry.ClientStreet ?? "";
            txtClientPostalcode.Text = entry.ClientPostalcode ?? "";
            txtClientTown.Text = entry.ClientTown ?? "";
            txtClientCountry.Text = entry.ClientCountry ?? "";
        }

        public DomEntry GetUpdatedEntry()
        {
            entry.Amount = (decimal)numAmount.Value;
            entry.EndToEndReference = txtEndToEndRef.Text;
            entry.Communication = txtCommunication.Text;
            entry.MandateId = txtMandateId.Text;
            entry.MandateStartDate = dtpMandateDate.Value;
            entry.ClientName = txtClientName.Text;
            entry.ClientIban = txtClientIban.Text;
            entry.ClientBic = txtClientBic.Text;
            entry.ClientStreet = txtClientStreet.Text;
            entry.ClientPostalcode = txtClientPostalcode.Text;
            entry.ClientTown = txtClientTown.Text;
            entry.ClientCountry = txtClientCountry.Text;

            return entry;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClientIban.Text))
            {
                MessageBox.Show("Client IBAN is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtClientName.Text))
            {
                MessageBox.Show("Client Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numAmount.Value <= 0)
            {
                MessageBox.Show("Amount must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
