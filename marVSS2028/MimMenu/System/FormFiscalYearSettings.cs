using System;
using System.IO;
using System.Windows.Forms;

using marVSS2028.SharedForms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.MimEnvironment;

namespace marVSS2028.PrivateForms
{
    public partial class FormFiscalYearSettings : Form
    {
        public FormFiscalYearSettings()
        {
            InitializeComponent();
        }

        private void FormFiscalYearSettings_Load(object sender, EventArgs e)
        {           
            Top = 5;
            Left = 5;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetupOption_DoubleClick(object sender, EventArgs e)
        {
            BtnOk_Click(sender, e);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Determine which radio button is checked (VB6: For COUNT_TO = 0 To 8 ... If SetupOption(COUNT_TO).Value = True)
            RadioButton[] options = { SetupOption0, SetupOption1, SetupOption2, SetupOption3, SetupOption4,
                                      SetupOption5, SetupOption6, SetupOption7, SetupOption8 };
            int selectedIndex = -1;
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].Checked)
                {
                    selectedIndex = i;
                    COUNT_TO = i;
                    break;
                }
            }

            if (selectedIndex < 0) return;

            string sectionCaption = options[selectedIndex].Text;

            // VB6: TeleBibDEF(";" + SetupOption(COUNT_TO).Caption)
            if (!TeleBibDEF(";" + sectionCaption))
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Definitiebestand 099.DEF defekt ?", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // ── Build FormXLog grid ───────────────────────────────────────────
            var xlog = new FormXLog
            {
                Text = "Setup Boekjaar: " + sectionCaption
            };
            xlog.X.Columns.Clear();
            xlog.X.Columns.Add("colKode",     "vsfKode");
            xlog.X.Columns.Add("colOmschr",   "Veldomschrijving");
            xlog.X.Columns.Add("colGegevens", "Veldgegevens");
            xlog.X.Columns[0].Width = 64;   // VB6: 960 twips ≈ 64px
            xlog.X.Columns[1].Width = 227;  // VB6: 3405 twips ≈ 227px
            xlog.X.Columns[2].Width = 293;  // VB6: 4395 twips ≈ 293px

            // Populate grid from TELEBIB arrays
            int t = 0;
            while (t < TELEBIB_CODE.Length && TELEBIB_CODE[t] != new string(' ', 10))
            {
                string code = TELEBIB_CODE[t] ?? "";
                string crText = string.Empty;

                // VB6: bGet TABLE_COUNTERS, 0, Mid(TELEBIB_CODE(T), 5, 5)
                string keyField = code.Length >= 9 ? code.Substring(4, 5) : string.Empty;
                // string keyField = code.Substring(4, 4);
                BGet(TABLE_COUNTERS, 0, keyField);

                if (Ktrl == 0)
                {
                    RecordToVeld(TABLE_COUNTERS);
                    try
                    {
                        object val = rsMAR[TABLE_COUNTERS].Fields["v217"].Value;
                        crText = (val == null || val is DBNull) ? string.Empty : val.ToString();
                    }
                    catch { crText = string.Empty; }

                    string typeCode = code.Length >= 3 ? code.Substring(1, 2) : "  ";
                    switch (typeCode)
                    {
                        case "  ": case "K ": case "L ": case "LC":
                        case "R ": case "R3": case "R4": case "R6": case "R7":
                            break;
                        default:
                            char firstChar = code.Length > 0 ? code[0] : ' ';
                            if (firstChar != '@' && crText != string.Empty)
                            {
                                string boxMask = (firstChar == ' ') ? "00" : "000";
                                string mid3 = code.Length >= 3 ? code.Substring(0, 3) : "0";
                                int.TryParse(mid3, out int boxVal);
                                crText = FMarBoxText(boxVal.ToString(boxMask), "2", crText);
                            }
                            break;
                    }
                }

                xlog.X.Rows.Add(code, TELEBIB_TEXT[t], crText);
                t++;
            }

            if (xlog.X.Rows.Count > 0)
                xlog.X.Rows[0].Selected = true;

        XLogShow:
            xlog.BtnWijzigenLijn.TabStop = true;
            xlog.BtnAfsluiten.TabStop = true;
            XLogKey = string.Empty;
            xlog.TabControl1.TabPages[1].Visible = false;
            
            xlog.ShowDialog();

            if (XLogKey == string.Empty)
                return;

            // ── Write changed values back ──────────────────────────────────────
            Msg = "Boekjaarparameters overschrijven.  Bent U zeker ?";
            if (MessageBox.Show(Msg, "Bevestigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            t = 0;
            while (t < TELEBIB_CODE.Length && TELEBIB_CODE[t] != new string(' ', 10))
            {
                string code = TELEBIB_CODE[t] ?? "";
                string crText2 = t < xlog.X.Rows.Count
                    ? xlog.X.Rows[t].Cells[2].Value?.ToString() ?? string.Empty
                    : string.Empty;

                string keyField = code.Length >= 9 ? code.Substring(4, 5) : string.Empty;
                // string keyField = code.Substring(4, 4);
                BGet(TABLE_COUNTERS, 0, keyField);

                if (Ktrl != 0)
                {
                    TLB_RECORD[TABLE_COUNTERS] = string.Empty;
                    VBib(TABLE_COUNTERS, keyField, "v071");
                }
                else
                {
                    RecordToVeld(TABLE_COUNTERS);
                }

                // Strip ": description" suffix if not "@" type
                
                string typeCode = code.Length >= 3 ? code.Substring(1, 2) : "  ";
                if (typeCode != "  " && (code.Length == 0 || code[0] != '@'))
                {
                    int colonIdx = crText2.IndexOf(':');
                    if (colonIdx > 0)
                        crText2 = crText2.Substring(0, colonIdx);
                }

                VBib(TABLE_COUNTERS, crText2, "v217");

                if (Ktrl != 0)
                    BInsert(TABLE_COUNTERS, 0);
                else
                    BUpdate(TABLE_COUNTERS, 0);

                t++;
            }

            BtnClose.Focus();
        }

        /// <summary>
        /// VB6: Function TeleBibDEF — loads TELEBIB arrays from a section in 099.Def.
        /// The file is line-based: first find ";SectionName", then read Input #n records
        /// until a line starting with ";" is found.
        /// </summary>
        private bool TeleBibDEF(string teZoeken)
        {
            string defPath = PROGRAM_LOCATION + @"Content\Def\099.Def";
            if (!File.Exists(defPath))
            {
                MessageBox.Show("Geen TeleBib definitie 099.Def");
                return false;
            }

            try
            {
                using (var sr = new StreamReader(defPath))
                {
                    // Search for section marker (e.g. ";Bedrijfsinformatie")
                    string lokatieString = string.Empty;
                    while (!sr.EndOfStream)
                    {
                        lokatieString = sr.ReadLine() ?? string.Empty;
                        if (lokatieString == teZoeken)
                            break;
                    }

                    if (lokatieString != teZoeken)
                        return false;

                    // Read records until ";" or EOF
                    int t = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine() ?? string.Empty;
                        if (line.StartsWith(";"))
                            break;

                        string[] parts = ParseInputLine(line);
                        if (parts.Length >= 4)
                        {
                            TELEBIB_CODE[t]   = parts[0];
                            TELEBIB_TEXT[t]   = parts[1];
                            TELEBIB_TYPE[t]   = parts[2];
                            TELEBIB_LENGTH[t] = int.TryParse(parts[3], out int len) ? len : 0;
                            t++;
                        }
                    }
                    TELEBIB_CODE[t] = new string(' ', 10);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("VSBibinlaadfout: " + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }          
    }
}
