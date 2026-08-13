using ADODB;
using marVSS2028.Classes;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.PeppolTools;
using static marVSS2028.Classes.ShellHelper;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.SharedForms
{
    public partial class FormPeppolCheckTool : Form
    {
        private Recordset rsPartners;
        private string vatList = string.Empty;
        private string vatListAll = string.Empty;
        private string partnerIs = string.Empty;

        public FormPeppolCheckTool()
        {
            InitializeComponent();
            rsPartners = new Recordset();
        }

        private void FormPeppolCheckTool_Load(object sender, EventArgs e)
        {
            TextBoxSupportedDocuments.Text = string.Empty;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void cbCheckAllPartners_Click(object sender, EventArgs e)
        {
            if (Text.IndexOf("Klanten", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                partnerIs = "Klanten";
            }
            else
            {
                partnerIs = "Leveranciers";
            }

            string sql;
            if (partnerIs == "Klanten")
            {
                sql = "SELECT DISTINCT Klanten.A110, Klanten.v150, Klanten.A161, Klanten.A100, Klanten.v224 AS [eMail] " +
                      "FROM Klanten, Dokumenten WHERE Dokumenten.v034 = 'K'+Klanten.A110 " +
                      "AND Dokumenten.v033 Like 'V%' AND len(Klanten.A161) > 1";
            }
            else
            {
                sql = "SELECT DISTINCT Leveranciers.A110, Leveranciers.v150, Leveranciers.A161, Leveranciers.A100, Leveranciers.v224 AS [eMail] " +
                      "FROM Leveranciers, Dokumenten WHERE Dokumenten.v034 = 'L'+Leveranciers.A110 " +
                      "AND Dokumenten.v033 Like 'A%'";
            }

            if (CheckBoxOnlyRecent.Checked)
            {
                sql += " AND Dokumenten.v035 Like '202%'";
            }

            SnelHelpPrint(sql, BL_LOGGING);
            Cursor previousCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (rsPartners != null && rsPartners.State != (int)ObjectStateEnum.adStateClosed)
                {
                    rsPartners.Close();
                }

                rsPartners = new Recordset();
                rsPartners.CursorLocation = CursorLocationEnum.adUseClient;
                rsPartners.Open(sql, adntDB, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

                if (rsPartners.EOF)
                {
                    MessageBox.Show("Geen klanten te vinden met btw nummer.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                vatList = string.Empty;
                vatListAll = string.Empty;
                mfgLijst.Visible = false;
                mfgLijst.Columns.Clear();
                mfgLijst.Rows.Clear();
                mfgLijst.AutoGenerateColumns = false;
                mfgLijst.RowHeadersVisible = false;
                mfgLijst.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Btw Nummer", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                mfgLijst.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = partnerIs, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                mfgLijst.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ondernemingsnr.", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                mfgLijst.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "eMail", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

                while (!rsPartners.EOF)
                {
                    string a110 = FieldText(rsPartners, "A110");
                    string v150 = FieldText(rsPartners, "v150");
                    string a161 = FieldText(rsPartners, "A161");
                    string a100 = FieldText(rsPartners, "A100");
                    string eMail = FieldText(rsPartners, "eMail");

                    vatList += v150.Trim() + a161.Trim() + Environment.NewLine;
                    vatListAll += a110.Trim() + "\t\t" + v150.Trim() + a161.Trim() + "\t\t\t" + a100.Trim() + Environment.NewLine;
                    mfgLijst.Rows.Add(v150.Trim() + a161.Trim(), a100.Trim(), a110.Trim(), eMail.Trim());
                    rsPartners.MoveNext();
                }

                Height = 480;
                mfgLijst.Visible = true;
                cbCopyToClipBoard.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bron:" + Environment.NewLine + ex.Source + Environment.NewLine + Environment.NewLine +
                                "Foutnummer: " + ex.HResult.ToString(CultureInfo.InvariantCulture) + Environment.NewLine + Environment.NewLine +
                                "Detail:" + Environment.NewLine + ex.Message);
            }
            finally
            {
                Cursor.Current = previousCursor;
            }
        }

        private void cbCheckCompanyNumber_Click(object sender, EventArgs e)
        {
            if (InternetIsAvailable())
            {
                string url = "https://kbopub.economie.fgov.be/kbopub/zoeknummerform.html?nummer=" + tbCompanyNumber.Text + "&actionLu=Zoek";
                ShellExecuteWithFallback(url);
            }
            else
            {
                MessageBox.Show("Geen internet verbinding.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbCheckVatNumber_Click(object sender, EventArgs e)
        {
            if (InternetIsAvailable())
            {
                string vat = (tbVatNumber.Text ?? string.Empty).Trim();
                if (vat.Length >= 3)
                {
                    string url = "https://ec.europa.eu/taxation_customs/vies/rest-api/ms/" + vat.Substring(0, 2) + "/vat/" + vat.Substring(2);
                    ShellExecuteWithFallback(url);
                }
            }
            else
            {
                MessageBox.Show("Geen internet verbinding.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbCheckPeppolRegistration_Click(object sender, EventArgs e)
        {
            if (!InternetIsAvailable())
            {
                MessageBox.Show("Geen internet verbinding.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor previousCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string responseText = CheckPeppolRegistration(tbPeppolID.Text);
                TextBoxSupportedDocuments.Text = responseText;
                MessageBox.Show(TextBoxSupportedDocuments.Text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = previousCursor;
            }
        }

        private void cbCopyToClipBoard_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(vatList);

            string msg = "Plak de lijst btwnummers in een tekst venster en copy/paste per 25" + Environment.NewLine +
                         "in toolvenster van bijvoorbeeld https://app.peppolchecker.eu/" + Environment.NewLine + Environment.NewLine;

            if (partnerIs == "Leveranciers")
            {
                msg += "Wat leveranciers betreft, vooral uw aandacht:" + Environment.NewLine +
                       "Manuele controle van btw nummer en ondernemingsnummer";
            }

            MessageBox.Show(msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);

            string peppolCheckPath = Path.Combine(LOCATION_COMPANYDATA ?? string.Empty, "PeppolCheckControle.txt");
            string peppolVatPath = Path.Combine(LOCATION_COMPANYDATA ?? string.Empty, "PeppolEnkelBtwNummers.txt");

            try
            {
                ScrMaakTekstBestand(vatListAll, peppolCheckPath);
                ScrMaakTekstBestand(vatList, peppolVatPath);
            }
            catch
            {
                File.WriteAllText(peppolCheckPath, vatListAll, Encoding.UTF8);
                File.WriteAllText(peppolVatPath, vatList, Encoding.UTF8);
            }

            try
            {
                System.Diagnostics.Process.Start("notepad.exe", "\"" + peppolCheckPath + "\"");
                System.Diagnostics.Process.Start("notepad.exe", "\"" + peppolVatPath + "\"");
            }
            catch
            {
            }

            Hide();
        }

        private string FieldText(Recordset rs, string fieldName)
        {
            try
            {
                object value = rs.Fields[fieldName].Value;
                return value == null || value == DBNull.Value ? string.Empty : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
