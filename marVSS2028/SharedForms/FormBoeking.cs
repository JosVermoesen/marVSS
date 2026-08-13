using System;
using System.Windows.Forms;
using marVSS2028.Classes;

namespace marVSS2028.SharedForms
{
    public partial class FormBoeking : Form
    {
        public FormBoeking()
        {
            InitializeComponent();
        }

        private void FormBoeking_Load(object sender, EventArgs e)
        {
            dgvBoekLijst.Columns[0].HeaderText = "Rekening";
            dgvBoekLijst.Columns[1].HeaderText = "Boekingsomschrijving";
            dgvBoekLijst.Columns[1].Width = 185;
            dgvBoekLijst.Columns[2].HeaderText = "EUR Debet";
            dgvBoekLijst.Columns[3].HeaderText = "EUR Credit";
            dgvBoekLijst.Columns[4].HeaderText = "BEF Debet";
            dgvBoekLijst.Columns[5].HeaderText = "BEF Credit";
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (Globals.DKTRL_EUR != 0 || Globals.DKTRL_BEF != 0)
            {
                int lastRow = dgvBoekLijst.Rows.Count - 1;
                if (lastRow >= 0 && dgvBoekLijst.Rows[lastRow].Cells[0].Value?.ToString() == string.Empty)
                {
                    dgvBoekLijst.Rows[lastRow].Cells[1].Value = "Ter info BEF Rekenenverschil";

                    if (Globals.DKTRL_BEF != 0)
                    {
                        if (Globals.DKTRL_BEF < 0)
                            dgvBoekLijst.Rows[lastRow].Cells[4].Value = (-Globals.DKTRL_BEF).ToString("#,##0.00");
                        else
                            dgvBoekLijst.Rows[lastRow].Cells[5].Value = Globals.DKTRL_BEF.ToString("#,##0.00");
                    }
                    else
                    {
                        if (Globals.DKTRL_EUR < 0)
                            dgvBoekLijst.Rows[lastRow].Cells[2].Value = (-Globals.DKTRL_EUR).ToString("#,##0.00");
                        else
                            dgvBoekLijst.Rows[lastRow].Cells[3].Value = Globals.DKTRL_EUR.ToString("#,##0.00");
                    }
                }
            }
        }

        /// <summary>
        /// Ports VB6 mshfBoekLijst.AddItem: splits the tab-separated pipo string and
        /// adds a row to dgvBoekLijst (Rekening, Omschrijving, EurDebet, EurCredit, BefDebet, BefCredit).
        /// </summary>
        public void AddItem(string pipo)
        {
            if (string.IsNullOrEmpty(pipo)) return;

            string[] parts = pipo.Split('\t');
            // Ensure we always have exactly 6 elements
            string[] cells = new string[6];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = i < parts.Length ? parts[i] : string.Empty;

            dgvBoekLijst.Rows.Add(cells[0], cells[1], cells[2], cells[3], cells[4], cells[5]);
        }

        private void cmdBoeken_Click(object sender, EventArgs e)
        {           
            Close();
        }

        private void cmdNegeren_Click(object sender, EventArgs e)
        {
            Globals.DKTRL_CUMUL = 99;
            Close();
        }
    }
}
