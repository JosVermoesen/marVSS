using System;
using System.Windows.Forms;

namespace marVSS2028.SharedForms
{
    public partial class FormReactionsDialog : Form
    {
        public FormReactionsDialog()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormReactionsDialog_Resize(object sender, EventArgs e)
        {
            try
            {
                TextBoxReactions.Width  = ClientSize.Width - 16;
                TextBoxReactions.Height = ClientSize.Height - 57;
                BtnSluiten.Top          = ClientSize.Height - 41;
            }
            catch { }
        }
    }
}
