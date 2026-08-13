using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;

namespace marVSS2028.Forms
{
    public partial class FormSplash : Form
    {
        bool forceExit = false;

        public FormSplash()
        {
            InitializeComponent();
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();
            var asmName = asm.GetName();

            string productName = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyProductAttribute)))?.Product ?? asmName.Name;
            string legalCopyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCopyrightAttribute)))?.Copyright ?? string.Empty;
            string fileDescription = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyDescriptionAttribute)))?.Description ?? string.Empty;

            LabelProductName.Text = productName;
            LabelCopyRight.Text = legalCopyright;
            LabelProductInfo.Text = fileDescription;

            int vIdx = appTitleAndVersion != null ? appTitleAndVersion.IndexOf("v.") : -1;
            AppInfo0.Text = vIdx >= 0 ? appTitleAndVersion.Substring(vIdx) : string.Empty;
                 
            string sVar1, sVar5;
                            
            sVar1 = "Servicejaar 2026";
            sVar5 = "01/12/2024 - 31/01/2030";

            ProgrammaVersie = sVar1;            
            Msg = sVar1 + "\r\n";
            Msg += "(Geldig van/tot)\r\n" + sVar5;
            LabelInfo2.Text = Msg;

            string geldigVanTot = sVar5.Substring(0, 10);
            string geldigVan = geldigVanTot.Substring(6, 4) + geldigVanTot.Substring(3, 2) + geldigVanTot.Substring(0, 2);
            geldigVanTot = sVar5.Substring(sVar5.Length - 10, 10);
            string geldigTot = geldigVanTot.Substring(6, 4) + geldigVanTot.Substring(3, 2) + geldigVanTot.Substring(0, 2);

            string today = DateTime.Now.ToString("yyyyMMdd");
            if (string.Compare(today, geldigVan) < 0 || string.Compare(today, geldigTot) > 0)
            {
                MessageBox.Show(
                    "Datum van vandaag valt buiten de geldigheidsduur van dit programma." + "\n\n" +
                    "Voor meer inlichtingen mail uw MarIntegraal software verantwoordelijke.",                    
                    "Controle van licentie !", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                forceExit = true;
                return;
            }

            // TODO: check service year and show message if not correct
            // "Datum van vandaag valt buiten de geldigheidsduur van het servicejaar." + "\r" +
            // "Het programma kan hierna verder gebruikt worden zonder service.",

        }

        private void FormSplash_Click(object sender, EventArgs e)
        {
            Ok_Click(sender, e);            
        }

        private void FormSplash_DblClick(object sender, EventArgs e)
        {
            Ok_Click(sender, e);
        }

        private void CmdLeesMij_Click(object sender, EventArgs e)
        {
            string url = "https://rv.be/accounting";
            try
            {
                Process.Start(url);
            }
            catch { }
            Application.DoEvents();
            Ok_Click(sender, e);
        }
                
        private void Image1_Click(object sender, EventArgs e)
        {
            Ok_Click(sender, e);
        }

        private void LabelInfo_Click(object sender, EventArgs e)
        {
            Ok_Click(sender, e);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (forceExit)
            {
                Environment.Exit(0);
            }
            Close();
        }
    }
}
