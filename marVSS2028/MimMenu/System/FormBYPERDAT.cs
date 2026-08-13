using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

using marVSS2028.Classes;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;

namespace marVSS2028
{
    public partial class FormBYPERDAT : Form
    {
        public FormBYPERDAT()
        {
            InitializeComponent();
            TextTools.WireHighlightEvents(this);

            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Size = new System.Drawing.Size(327, 149);            
        }

        private void BtnVerkleinen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            FL99 = 0;
        }

        private void FormBJPERDAT_Load(object sender, EventArgs e)
        {
            Top = 50;
            Left = 50;
        }

        private void FormBJPERDAT_Activated(object sender, EventArgs e)
        {
            if (DateTime.Now.ToString("dd/MM/yyyy") != DatumVerwerking.Value.ToString("dd/MM/yyyy"))
            {
                DatumVerwerking.Value = DateTime.Now;
                DatumVerwerking_ValueChanged(sender, e);
            }

            if (CmbPeriodeBoekjaar.SelectedIndex >= 0)
            {
                CmbPeriodeBoekjaar.Focus();
            }
        }

        private void DatumVerwerking_ValueChanged(object sender, EventArgs e)
        {
            MIM_GLOBAL_DATE = DatumVerwerking.Value.ToString("dd/MM/yyyy");
            if (Application.OpenForms["FormMim"] is FormMim mim)
            {
                mim.ToolStripBookingDate.Text = MIM_GLOBAL_DATE;
            }
        }

        internal void CmbBoekjaar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string xx = string.Empty;
            int periodeMeestLogisch = -1;

            if (ACTIVE_BOOKYEAR != CmbBoekjaar.SelectedIndex)
            {
                BClose(99);
                TABLEDEF_ONT[TABLE_COUNTERS] = CmbBoekjaar.SelectedIndex.ToString("D2") + ".ONT";
                bstNaam[TABLE_COUNTERS] = "jr" + CmbBoekjaar.Text;
                // Close all MDI children except persistent forms (VB6: CloseOpenWindows)
                if (this.MdiParent != null)
                {
                    foreach (Form child in this.MdiParent.MdiChildren)
                    {
                        if (child is FormBYPERDAT || child is FormBasicTable)
                            continue;
                        child.Close();
                    }
                }
                ACTIVE_BOOKYEAR = CmbBoekjaar.SelectedIndex;
                int aktievePeriode = 1;
                CmbPeriodeBoekjaar.Items.Clear();

                string octPath = LOCATION_COMPANYDATA + "DEF" + ACTIVE_BOOKYEAR.ToString("D2") + ".OCT";
                try
                {
                    using (var fs = new FileStream(octPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buf = new byte[16];
                        for (int t = 1; t <= 99; t++)
                        {
                            fs.Seek((t - 1) * 16L, SeekOrigin.Begin);
                            int bytesRead = fs.Read(buf, 0, 16);
                            if (bytesRead < 16)
                                break;

                            string a = Encoding.Default.GetString(buf);
                            if (a == new string(' ', 16))
                            {
                                if (CmbPeriodeBoekjaar.Items.Count > 0)
                                    CmbPeriodeBoekjaar.SelectedIndex = 0;
                                string yy = CmbPeriodeBoekjaar.Text;
                                BOOKYEAR_FROMTO =
                                    SafeMid(yy, 7, 4) + SafeMid(yy, 4, 2) + SafeLeft(yy, 2)
                                    + SafeMid(xx, 20, 4) + SafeMid(xx, 17, 2) + SafeLeft(xx.Length >= 14 ? xx.Substring(13) : "", 2);
                                break;
                            }
                            else
                            {
                                xx = a.Substring(6, 2) + "/" + a.Substring(4, 2) + "/" + a.Substring(0, 4)
                                   + " - "
                                   + a.Substring(14, 2) + "/" + a.Substring(12, 2) + "/" + a.Substring(8, 4);
                                CmbPeriodeBoekjaar.Items.Add(xx);

                                if (string.Compare(DateKey(MIM_GLOBAL_DATE), DateKey(xx.Substring(0, 10)), StringComparison.Ordinal) >= 0
                                    && string.Compare(DateKey(MIM_GLOBAL_DATE), DateKey(xx.Substring(xx.Length - 10)), StringComparison.Ordinal) <= 0)
                                {
                                    periodeMeestLogisch = t - 1;
                                }
                            }
                        }
                    }
                }
                catch { }

                if (periodeMeestLogisch != -1)
                    CmbPeriodeBoekjaar.SelectedIndex = periodeMeestLogisch;
                else if (CmbPeriodeBoekjaar.Items.Count > 0)
                    CmbPeriodeBoekjaar.SelectedIndex = aktievePeriode - 1;

                FL99 = 0;
            }

            xx = CmbPeriodeBoekjaar.Text;
            PERIOD_FROMTO = SafeMid(xx, 7, 4) + SafeMid(xx, 4, 2) + SafeLeft(xx, 2)
                          + SafeRight(xx, 4) + SafeMid(xx, 17, 2) + SafeMid(xx, 14, 2);

            // Currency panel check (VB6: Mim.SnelHelp.Panels(2).Text)
            if (Mim != null && Mim.ToolStripLabel2.Text != "---")
            {
                if (Mim.ToolStripLabel2.Text == "EUR")
                    XisEuroWisBEF = true;

                string curr296 = String99(296);
                if (curr296.Length == 0)
                {
                    MessageBox.Show(
                        "Gelieve Setup Boekingen en algemene instellingen : munt van de Boekhouding in te stellen a.u.b.  " +
                        "Pér bedrijf, pér boekjaar.  Hierna wordt voorlopig verder gewerkt in BEF.");
                    bhEuro = false;
                }
                else if (curr296 == "BEF")
                {
                    bhEuro = false;
                }
                else if (curr296 == "EUR")
                {
                    bhEuro = true;
                }
                else
                {
                    SnelHelpPrint("Onlogische situatie", BL_LOGGING);
                    bhEuro = false;
                }

                if (bhEuro)
                {
                    XisEuroWisBEF = false;
                    Mim.ToolStripLabel2.Text = "EUR";
                }
                else
                {
                    Mim.ToolStripLabel2.Text = "BEF";
                    if (!(XisEuroWisBEF == true && CmbBoekjaar.SelectedIndex == 1))
                        XisEuroWisBEF = false;
                }

                SnelHelpPrint("XisEuroWisBEF = " + XisEuroWisBEF, BL_LOGGING);

                if (CmbBoekjaar.SelectedIndex == 1 && bhEuro == false)
                {
                    Msg = "Dit bedrijf heeft een boekjaar -1 met verwerking in BEF.  " +
                          "Indien U nog boekingen in BEF wenst uit te voeren voor het zopas aangeduide boekjaar, " +
                          "gelieve een vorige versie (6.5.300 of lager) opnieuw te installeren a.u.b.";
                    MessageBox.Show(Msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        internal void CmbPeriodeBoekjaar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = CmbPeriodeBoekjaar.Text;
            PERIOD_FROMTO = SafeMid(a, 7, 4) + SafeMid(a, 4, 2) + SafeLeft(a, 2)
                          + SafeRight(a, 4) + SafeMid(a, 17, 2) + SafeMid(a, 14, 2);

            // string octPath = PROGRAM_LOCATION + "9999.OCT";
            string octPath = LOCATION_COMPANYDATA + "9999.OCT";
            try
            {
                using (var fs = new FileStream(octPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] buf = new byte[4];

                    // Record 1: ACTIVE_BOOKYEAR as string
                    WriteFixed4(fs, 1, ACTIVE_BOOKYEAR.ToString());
                    // Record 2: Boekjaar text
                    WriteFixed4(fs, 2, CmbBoekjaar.Text);
                    // Record 3: active period index (1-based)
                    WriteFixed4(fs, 3, (CmbPeriodeBoekjaar.SelectedIndex + 1).ToString());
                }
            }
            catch { }

            this.Text = "(" + CmbBoekjaar.Text + ") (" + CmbPeriodeBoekjaar.Text.Substring(0, Math.Min(10, CmbPeriodeBoekjaar.Text.Length)) + ") BoekPeriode";
        }
                
        // VB6: DATE_KEY(dateStr) — converts dd/MM/yyyy to yyyyMMdd for comparison
        private static string DateKey(string ddmmyyyy)
        {
            if (string.IsNullOrEmpty(ddmmyyyy) || ddmmyyyy.Length < 10)
                return string.Empty;
            // dd/MM/yyyy → yyyyMMdd
            return ddmmyyyy.Substring(6, 4) + ddmmyyyy.Substring(3, 2) + ddmmyyyy.Substring(0, 2);
        }

        // Writes a fixed 4-byte record at the given 1-based record position
        private static void WriteFixed4(FileStream fs, int recordNumber, string value)
        {
            byte[] buf = new byte[4];
            byte[] src = Encoding.Default.GetBytes(value);
            int len = Math.Min(src.Length, 4);
            Array.Copy(src, buf, len);
            fs.Seek((recordNumber - 1) * 4L, SeekOrigin.Begin);
            fs.Write(buf, 0, 4);
        }
    }
}
