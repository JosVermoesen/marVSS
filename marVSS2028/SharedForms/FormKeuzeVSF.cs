using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;

namespace marVSS2028.PublicForms
{
    public partial class FormKeuzeVSF : Form
    {
        public FormKeuzeVSF()
        {
            InitializeComponent();
        }

        private void FormKeuzeVSF_Activated(object sender, System.EventArgs e)
        {
            string zoekTekst = "NTKB2";
            if (aIndex >= 1000)
                zoekTekst += (aIndex - 1000).ToString("000");
            else
                zoekTekst += "9" + aIndex.ToString("00");

            NTBoxLijst.Items.Clear();

            int keuze;
            ZoekEnPlaats(NTBoxLijst, zoekTekst, out int _, out keuze, GridText);

            if (keuze >= 0)
                NTBoxLijst.SelectedIndex = keuze;
        }

        private void FormKeuzeVSF_Resize(object sender, System.EventArgs e)
        {
            try
            {
                NTBoxLijst.Width  = Width  - 16;
                NTBoxLijst.Height = Height - 42;
            }
            catch { }
        }

        private void NTBoxLijst_DoubleClick(object sender, System.EventArgs e)
        {
            NTBoxLijst_KeyPress(sender, new KeyPressEventArgs('\r'));
        }

        private void NTBoxLijst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Escape)
                NTBoxLijst_KeyPress(sender, new KeyPressEventArgs((char)27));
        }

        private void NTBoxLijst_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                switch (e.KeyChar)
                {
                    case '\r':
                        GridText = NTBoxLijst.SelectedItem?.ToString() ?? string.Empty;
                        Close();
                        break;
                    case (char)27:
                        Close();
                        break;
                }
            }
            catch { }
        }
    }
}
