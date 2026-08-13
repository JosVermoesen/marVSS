using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace marVSS2028.Classes
{
    internal static class TextTools // This class provides utility methods for text manipulation, including email validation, number formatting, file reading/writing, and string padding/truncation.
    {
        private static readonly System.Drawing.Color FocusColor = System.Drawing.Color.LightYellow;
        private static readonly System.Drawing.Color NormalColor = System.Drawing.SystemColors.Window;
        private static readonly System.Drawing.Color FocusForeColor = System.Drawing.Color.DarkBlue;
        private static readonly System.Drawing.Color NormalForeColor = System.Drawing.SystemColors.ControlText;

        /// <summary>
        /// Recursively wires Enter/Leave highlight events on all input controls of a form.
        /// - TextBox, ComboBox, DateTimePicker, ListBox  → BackColor highlight
        /// - RadioButton, CheckBox, Button               → ForeColor highlight
        /// Also applies the highlight immediately to the focused control on first show.
        /// Call from the form constructor after InitializeComponent().
        /// </summary>
        public static void WireHighlightEvents(Control parent)
        {
            WireHighlightEventsRecursive(parent);

            // Apply initial highlight once the form is fully shown
            if (parent is Form form)
            {
                form.Shown += (s, e) => ApplyHighlightToFocused(form);
            }
        }

        private static void WireHighlightEventsRecursive(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox || ctrl is ComboBox || ctrl is DateTimePicker || ctrl is ListBox)
                {
                    ctrl.Enter += (s, e) => ((Control)s).BackColor = FocusColor;
                    ctrl.Leave += (s, e) => ((Control)s).BackColor = NormalColor;
                }
                else if (ctrl is RadioButton || ctrl is CheckBox || ctrl is Button)
                {
                    ctrl.Enter += (s, e) => ((Control)s).ForeColor = FocusForeColor;
                    ctrl.Leave += (s, e) => ((Control)s).ForeColor = NormalForeColor;
                }
                if (ctrl.HasChildren)
                    WireHighlightEventsRecursive(ctrl);
            }
        }

        private static void ApplyHighlightToFocused(Control parent)
        {
            Control focused = (parent is Form f) ? f.ActiveControl : GetFocusedControl(parent);
            if (focused == null) return;

            if (focused is TextBox || focused is ComboBox || focused is DateTimePicker || focused is ListBox)
                focused.BackColor = FocusColor;
            else if (focused is RadioButton || focused is CheckBox || focused is Button)
                focused.ForeColor = FocusForeColor;
        }

        private static Control GetFocusedControl(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Focused) return ctrl;
                if (ctrl.HasChildren)
                {
                    Control found = GetFocusedControl(ctrl);
                    if (found != null) return found;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the single FormBYPERDAT MDI child, creates it if absent,
        /// then activates it and sets focus on CmbPeriodeBoekjaar.
        /// </summary>
        public static void OpenBYPERDAT(Form caller)
        {
            System.Media.SystemSounds.Beep.Play();
            if (caller?.MdiParent == null) return;

            FormBYPERDAT byperdat = null;
            foreach (Form child in caller.MdiParent.MdiChildren)
            {
                if (child is FormBYPERDAT bp)
                {
                    byperdat = bp;
                    break;
                }
            }
            if (byperdat == null)
            {
                byperdat = new FormBYPERDAT();
                byperdat.MdiParent = caller.MdiParent;
                byperdat.Show();
            }
            byperdat.Activate();
            byperdat.WindowState = FormWindowState.Normal;
            byperdat.CmbPeriodeBoekjaar.Focus();
        }

        // -----------------------------------------------------------------------
        // Helper: VB6 vSet — left-pads / truncates a string to a fixed length
        // -----------------------------------------------------------------------
        public static string VSet(string value, int length)
        {
            if (value == null) value = "";
            if (value.Length >= length) return value.Substring(0, length);
            return value.PadRight(length);
        }

        public static double ParseOrZero(string[] values, int index)
        {
            if (index < 0 || index >= values.Length)
                return 0d;

            return double.TryParse(
                values[index],
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var value)
                ? value
                : 0d;
        }

        /// <summary>        
        /// Returns true when sEmail matches a basic e-mail pattern.
        /// </summary>
        public static bool IsValidEmail(string sEmail)
        {
            if (string.IsNullOrEmpty(sEmail))
                return false;

            try
            {
                return Regex.IsMatch(
                    sEmail.Trim(),
                    @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        // Helper: VB6 Mid(s, start, length) — 1-based, safe

        public static string SafeMid(string s, int start, int length)
        {
            if (s == null || start < 1 || start > s.Length) return string.Empty;
            int idx = start - 1;
            int available = s.Length - idx;
            return s.Substring(idx, Math.Min(length, available));
        }

        // Helper: VB6 Left(s, n) — safe
        public static string SafeLeft(string s, int n)
        {
            if (s == null || n <= 0) return string.Empty;
            return s.Length >= n ? s.Substring(0, n) : s;
        }

        // Helper: VB6 Right(s, n) — safe
        public static string SafeRight(string s, int n)
        {
            if (s == null || n <= 0) return string.Empty;
            return s.Length >= n ? s.Substring(s.Length - n) : s;
        }

        /// <summary>        
        /// Formats a number using the given VB6-style mask, right-pads to mask length,
        /// and replaces a comma decimal separator with a period.
        /// </summary>
        public static string Dec(double fGetal, string fMasker)
        {
            int maskerLengte = fMasker.Length;
            string tempoString = fGetal.ToString(ConvertVb6FormatMask(fMasker));

            if (maskerLengte - tempoString.Length > 0)
                tempoString = tempoString.PadLeft(maskerLengte);

            tempoString = tempoString.Replace(',', '.');
            return tempoString;
        }

        // Converts a VB6 numeric format mask to a .NET format string.
        private static string ConvertVb6FormatMask(string vb6Mask)
        {
            // VB6 masks use '#' for optional digit and '0' for required digit.
            // .NET numeric format strings use the same characters, so pass through directly.
            return vb6Mask;
        }

        /// <summary>        
        /// Deletes an existing file (if any) and writes TekstZelf as UTF-8.
        /// Returns true on success.
        /// </summary>
        public static bool ScrMaakTekstBestand(string tekstZelf, string bestandsnaam)
        {
            try
            {
                if (File.Exists(bestandsnaam))
                    File.Delete(bestandsnaam);

                if (Application.OpenForms["FormMim"] is FormMim mim)
                    mim.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                MarWriteUtf8File(bestandsnaam, tekstZelf);

                if (Application.OpenForms["FormMim"] is FormMim mimEnd)
                    mimEnd.Cursor = System.Windows.Forms.Cursors.Default;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>        
        /// Reads the UTF-8 file into tekstZelf (returned as out parameter).
        /// Returns true on success.
        /// </summary>
        public static bool ScrLeesTekstBestand(out string tekstZelf, string bestandsnaam)
        {
            try
            {
                tekstZelf = MarReadUtf8File(bestandsnaam);
                return true;
            }
            catch
            {
                tekstZelf = string.Empty;
                return false;
            }
        }

        /// <summary>        
        /// Reads the UTF-8 file into tekstZelf (returned as out parameter).
        /// Returns true on success.
        /// </summary>
        public static bool ScrLeesBestandAlleTekst(out string tekstZelf, string bestandsnaam)
        {
            try
            {
                tekstZelf = MarReadUtf8File(bestandsnaam);
                return true;
            }
            catch
            {
                tekstZelf = string.Empty;
                return false;
            }
        }

        private static void MarWriteUtf8File(string path, string content)
        {
            File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        private static string MarReadUtf8File(string path)
        {
            return File.ReadAllText(path, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        }

        /// <summary>        
        /// Sets MASK_2002 based on the active currency (EUR or BEF) and initialises
        /// the MASK_SY numeric format mask array.
        /// </summary>
        public static void Cijfermaskers()
        {
            Globals.MASK_2002 = Globals.bhEuro ? Globals.MASK_EUR : Globals.MASK_BEF;

            Globals.MASK_SY[0] = "#########";
            Globals.MASK_SY[1] = "###0";
            Globals.MASK_SY[2] = "######0.00";
            Globals.MASK_SY[3] = "##0.00000000";
            Globals.MASK_SY[4] = "#######0.00";
            Globals.MASK_SY[5] = "##0";
            Globals.MASK_SY[6] = "#0";
            Globals.MASK_SY[7] = "#####0.0";
            Globals.MASK_SY[8] = "#######0";
        }

        /// <summary>
        /// VB6: Function scrFolderBestaat — returns true when the given folder exists.
        /// </summary>
        public static bool ScrFolderBestaat(string folder)
        {
            return Directory.Exists(folder);
        }

        /// <summary>
        /// VB6: Function LineCalculating — interactive single-line calculator.
        /// Prompts the user for an expression, evaluates it with DataTable.Compute,
        /// and returns the result string, or null when the user cancels.
        /// Type "CLR" in the prompt to reset the accumulated start value.
        /// </summary>
        public static string LineCalculating(string startWith)
        {
            string startWithHere = startWith ?? string.Empty;

            while (true)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    "Rekenen met '" + startWithHere.Trim() + "'" +
                    "\r\n(voer in CLR de startgegevens te verwijderen)",
                    "1-Lijn Rekenen");

                // User cancelled (empty string returned by InputBox on Cancel)
                if (input == null)
                    return null;

                if (input.ToUpperInvariant().Contains("CLR"))
                {
                    startWithHere = string.Empty;
                    continue;
                }

                string expression = startWithHere + input;
                try
                {
                    object result = new System.Data.DataTable().Compute(expression, null);
                    string resultStr = result?.ToString() ?? string.Empty;
                    if (resultStr != string.Empty)
                        return resultStr;
                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show("Deling door nul", string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception ex) when (ex is System.Data.EvaluateException ||
                                            ex is System.Data.SyntaxErrorException)
                {
                    MessageBox.Show("Schrijffout of ongeldige notering", string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// VB6: Function BankOk — validates a Belgian bank account number (12 or 14 chars).
        /// </summary>
        public static bool BankOk(string rekString)
        {
            rekString = rekString ?? string.Empty;
            if (rekString.Trim().Length == 14)
                rekString = rekString.Substring(0, 3) + rekString.Substring(4, 7) + rekString.Substring(12, 2);
            else if (rekString.Length != 12)
                return false;

            if (!double.TryParse(rekString.Substring(0, 3) + rekString.Substring(3, 7), out double dPip))
                return false;

            string check = rekString.Substring(10, 2);
            if (check == "00")
                return false;

            double remainder = dPip - Math.Truncate(dPip / 97) * 97;
            if (remainder == 0 && check == "97")
                return true;
            if (remainder == double.Parse(check))
                return true;

            return false;
        }

        /// <summary>
        /// VB6: Function Mod97 — computes mod-97 of a numeric string digit-by-digit.
        /// </summary>
        public static long Mod97(string s)
        {
            long value = 0;
            foreach (char ch in s)
            {
                int c = ch - '0';
                value = (value * 10 + c) % 97;
            }
            return value;
        }

        /// <summary>
        /// VB6: Function FormatDummy — zero-pads num to size digits.
        /// </summary>
        public static string FormatDummy(long num, int size)
        {
            string s = "000000000" + num.ToString();
            return s.Substring(s.Length - size);
        }

        /// <summary>
        /// VB6: Function IbanCheck — validates and optionally converts a Belgian bank account to/from IBAN.
        /// Returns the formatted account or "invalid".
        /// </summary>
        public static string IbanCheck(string anyRekString, bool sepaFlag, bool returnFormatted)
        {
            int rekLength = anyRekString?.Length ?? 0;
            string rekOld = string.Empty;
            string rekSepa = string.Empty;
            bool inputIsSepa = false;

            switch (rekLength)
            {
                case 12:
                    inputIsSepa = false;
                    rekOld = anyRekString;
                    break;
                case 14:
                    inputIsSepa = false;
                    rekOld = anyRekString.Substring(0, 3) +
                             anyRekString.Substring(4, 7) +
                             anyRekString.Substring(12);
                    break;
                case 16:
                    inputIsSepa = true;
                    rekSepa = anyRekString;
                    rekOld = rekSepa.Substring(4);
                    break;
                case 19:
                    inputIsSepa = true;
                    rekSepa = anyRekString.Substring(0, 4) +
                              anyRekString.Substring(5, 4) +
                              anyRekString.Substring(10, 4) +
                              anyRekString.Substring(15);
                    rekOld = rekSepa.Substring(4);
                    break;
                default:
                    return "invalid";
            }

            string dPip = rekOld.Substring(0, 10);
            long dPip2 = long.Parse(rekOld.Substring(10, 2));
            long calcPip = Mod97(dPip);

            if (rekOld.Substring(10, 2) == "00")
                return "invalid";
            else if (calcPip == 0 && rekOld.Substring(10, 2) == "97")
            { /* OK */ }
            else if (calcPip == dPip2)
            { /* OK */ }
            else
                return "invalid";

            if (!sepaFlag)
            {
                if (!returnFormatted)
                    return rekOld;
                return rekOld.Substring(0, 3) + " " +
                       rekOld.Substring(3, 7) + " " +
                       rekOld.Substring(10);
            }

            if (!inputIsSepa)
                rekSepa = "BE00" + rekOld;
            else if (!rekSepa.StartsWith("BE"))
                return "invalid";

            string longString = rekOld.Substring(10) + rekOld.Substring(10) + "111400";
            calcPip = Mod97(longString);

            if (!inputIsSepa)
                rekSepa = rekSepa.Replace("BE00", "BE" + FormatDummy(98 - calcPip, 2));

            dPip2 = long.Parse(rekSepa.Substring(2, 2));
            if ((98 - calcPip) != dPip2)
                return "invalid";

            if (!returnFormatted)
                return rekSepa;

            return rekSepa.Substring(0, 4) + " " +
                   rekSepa.Substring(4, 4) + " " +
                   rekSepa.Substring(8, 4) + " " +
                   rekSepa.Substring(12);
        }

        /// <summary>
        /// VB6: Function BtwKontrole — validates a Belgian VAT number string.
        /// Returns the original string if valid, empty string otherwise.
        /// </summary>
        public static string BtwKontrole(string btwString, bool bStrip)
        {
            try
            {
                double dPip1 = double.Parse(btwString.Substring(0, 8));
                double dPip2 = dPip1 / 97;
                double dPip3 = dPip2 - Math.Truncate(dPip2);
                int ipip = 97 - (int)(dPip3 * 97);
                if (ipip != int.Parse(btwString.Substring(btwString.Length - 2, 2)))
                    return string.Empty;
                return btwString;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// VB6: Function CopyFile — copies one or more files (supports wildcards) from sourcePath to targetPath.
        /// Returns true on success.
        /// </summary>
        public static bool CopyFile(string sourcePath, string targetPath, string fileToCopy)
        {
            try
            {
                string[] files;
                if (fileToCopy.Contains("?") || fileToCopy.Contains("*"))
                {
                    files = Directory.GetFiles(sourcePath, fileToCopy);
                    if (files.Length == 0)
                    {
                        MessageBox.Show("Stop tijdens het kopieren.  Bestand niet te vinden: \"" + fileToCopy + "\"",
                            "SETUP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                else
                {
                    string src = Path.Combine(sourcePath, fileToCopy);
                    if (!File.Exists(src))
                    {
                        MessageBox.Show("Bestand niet te vinden: \"" + fileToCopy + "\"",
                            "SETUP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    files = new[] { src };
                }

                foreach (string srcFile in files)
                {
                    string destFile = Path.Combine(targetPath, Path.GetFileName(srcFile));
                    File.Copy(srcFile, destFile, overwrite: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stop tijdens het kopieren van " + fileToCopy + "\r\n" + ex.Message,
                    "SETUP", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        /// <summary>
        /// VB6: Function CreatePath — creates all directories in the given path.
        /// Returns true on success.
        /// </summary>
        public static bool CreatePath(string destPath)
        {
            try
            {
                Directory.CreateDirectory(destPath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stop tijdens aanmaak van inhoudsopgaves op de doeldisk.\r\n" + ex.Message,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>
        /// VB6: Function DATE_INVALID — returns true when fDatum is not a valid dd/mm/yyyy date.
        /// </summary>
        public static bool DateInvalid(string fDatum)
        {
            if (fDatum?.Length != 10)
                return true;

            int dag = int.Parse(fDatum.Substring(0, 2));
            int maand = int.Parse(fDatum.Substring(3, 2));
            int jaar = int.Parse(fDatum.Substring(6, 4));

            if (dag < 1 || dag > 31 || maand < 1 || maand > 12 || jaar <= 1985 || jaar >= 2062)
            {
                System.Media.SystemSounds.Beep.Play();
                return true;
            }
            return false;
        }

        /// <summary>
        /// VB6: Function DATE_KEY — converts dd/mm/yyyy to yyyymmdd sort key.
        /// </summary>
        public static string DateKey(string datumfTXT)
        {
            string dag = datumfTXT.Substring(0, 2);
            string maand = datumfTXT.Substring(3, 2);
            string jaar = datumfTXT.Substring(6, 4);
            return jaar + maand + dag;
        }

        /// <summary>
        /// VB6: Function DATE_TEXT — converts yyyymmdd key to dd/mm/yyyy display text.
        /// </summary>
        public static string DateText(string dateAsKey)
        {
            if (string.IsNullOrWhiteSpace(dateAsKey) || dateAsKey.Length < 8)
                return string.Empty;

            string day = dateAsKey.Substring(6, 2);
            string month = dateAsKey.Substring(4, 2);
            string year = dateAsKey.Substring(0, 4);
            return day + "/" + month + "/" + year;
        }

        /// <summary>
        /// VB6: Function FileExists — returns true when the given file exists.
        /// </summary>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// VB6: Function DATE_CHECK — validates a dd/mm/yyyy date string against the active
        /// period or book-year range stored in Globals.PERIOD_FROMTO / BOOKYEAR_FROMTO.
        /// fVlag: PERIODAS_TEXT=0, BOOKYEARAS_TEXT=1, PERIODAS_KEY=2, BOOKYEAR_KEY=3.
        /// </summary>
        public static bool DateCheck(string fDatum, int fVlag)
        {
            string gDatum = fDatum ?? string.Empty;
            while (gDatum.Contains("/"))
            {
                int pos = gDatum.IndexOf('/');
                gDatum = gDatum.Substring(0, pos) + gDatum.Substring(pos + 1);
            }

            string dag, maand, jaar;
            switch (fVlag)
            {
                case Globals.PERIODAS_TEXT:
                case Globals.BOOKYEARAS_TEXT:
                    dag = gDatum.Length >= 2 ? gDatum.Substring(0, 2) : "00";
                    maand = gDatum.Length >= 4 ? gDatum.Substring(2, 2) : "00";
                    jaar = gDatum.Length >= 8 ? gDatum.Substring(4, 4) : "0000";
                    break;
                case Globals.PERIODAS_KEY:
                case Globals.BOOKYEAR_KEY:
                    jaar = gDatum.Length >= 4 ? gDatum.Substring(0, 4) : "0000";
                    maand = gDatum.Length >= 6 ? gDatum.Substring(4, 2) : "00";
                    dag = gDatum.Length >= 8 ? gDatum.Substring(6, 2) : "00";
                    break;
                default:
                    MessageBox.Show("Datum onjuist !");
                    return false;
            }

            string key = jaar + maand + dag;
            switch (fVlag)
            {
                case Globals.PERIODAS_TEXT:
                case Globals.PERIODAS_KEY:
                    string pFrom = Globals.PERIOD_FROMTO?.Length >= 8 ? Globals.PERIOD_FROMTO.Substring(0, 8) : "00000000";
                    string pTo = Globals.PERIOD_FROMTO?.Length >= 16 ? Globals.PERIOD_FROMTO.Substring(8, 8) : "99999999";
                    return string.Compare(key, pFrom, StringComparison.Ordinal) >= 0
                        && string.Compare(key, pTo, StringComparison.Ordinal) <= 0;
                case Globals.BOOKYEARAS_TEXT:
                case Globals.BOOKYEAR_KEY:
                    string bFrom = Globals.BOOKYEAR_FROMTO?.Length >= 8 ? Globals.BOOKYEAR_FROMTO.Substring(0, 8) : "00000000";
                    string bTo = Globals.BOOKYEAR_FROMTO?.Length >= 16 ? Globals.BOOKYEAR_FROMTO.Substring(8, 8) : "99999999";
                    return string.Compare(key, bFrom, StringComparison.Ordinal) >= 0
                        && string.Compare(key, bTo, StringComparison.Ordinal) <= 0;
                default:
                    return false;
            }
        }

        /// <summary>
        /// VB6: Function GetFileSize — returns the size in bytes of the given file.
        /// </summary>
        public static long GetFileSize(string source)
        {
            return new FileInfo(source).Length;
        }

        /// <summary>
        /// VB6: Function SleutelDok — generates an 11-character document key
        /// from a record number, the fiscal year, and a counter value.
        /// Format: PP + YYYY + NNNNN (e.g., "A02025" + first 4 of PERIOD_FROMTO + incremented counter)
        /// </summary>
        public static string SleutelDok(int fRecordNr)
        {
            string VoorLetter;

            // Map record number to 2-letter prefix (voorletter)
            switch (fRecordNr)
            {
                case 1:
                    VoorLetter = "A0";
                    break;
                case 3:
                    VoorLetter = "A1";
                    break;
                case 11:
                    VoorLetter = "V0";
                    break;
                case 13:
                    VoorLetter = "V1";
                    break;
                case 73:
                    VoorLetter = "B0";
                    break;
                case 59:
                    VoorLetter = "F0";
                    break;
                case 121:
                    VoorLetter = "Q0";
                    break;
                case 188:
                    VoorLetter = "PF";
                    break;
                default:
                    MessageBox.Show("Ongeldige record : " + fRecordNr.ToString());
                    return string.Empty;
            }

            // Retrieve the counter value from the fiscal settings
            string FL99_RECORD = OleDbTools.String99(fRecordNr);

            // Build document key: prefix + fiscal year (first 4 chars) + counter (5 digits, incremented)
            int counterValue = int.Parse(FL99_RECORD) + 1;
            return VoorLetter + Globals.PERIOD_FROMTO.Substring(0, 4) + counterValue.ToString("D5");
        }

        /// <summary>
        /// VB6: Function VValdag — adds avd days to a dd/mm/yyyy date string.
        /// If rvv contains 'E', snaps to end of month.
        /// Returns empty string on error.
        /// </summary>
        public static string VValdag(string rDat1, string rvv)
        {
            try
            {
                int irdg43 = int.Parse(rDat1.Substring(0, 2));
                int irmd43 = int.Parse(rDat1.Substring(3, 2));
                int irjr43 = int.Parse(rDat1.Substring(6, 4));
                bool isEndOfMonth = rvv.ToUpperInvariant().Contains("E");
                string numericPart = rvv.ToUpperInvariant().Replace("E", "");
                int avd43 = int.Parse(numericPart);

                if (avd43 == 0)
                    return rDat1;

                int adm1 = DateTime.DaysInMonth(irjr43, irmd43);
                while (irdg43 + avd43 > adm1)
                {
                    avd43 -= (adm1 - irdg43);
                    irdg43 = 0;
                    if (irmd43 == 12)
                    {
                        irmd43 = 1;
                        irjr43++;
                    }
                    else
                        irmd43++;
                    adm1 = DateTime.DaysInMonth(irjr43, irmd43);
                }

                irdg43 += avd43;
                if (isEndOfMonth)
                    irdg43 = adm1;

                return irdg43.ToString("00") + "/" + irmd43.ToString("00") + "/" + irjr43.ToString("0000");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// VB6: Function ValidateNumeric — returns true when strText is empty, a sign/decimal stub, or numeric.
        /// </summary>
        public static bool ValidateNumeric(string strText)
        {
            return strText == string.Empty ||
                   strText == "-" ||
                   strText == "-." ||
                   strText == "." ||
                   double.TryParse(strText, out _);
        }

        /// <summary>
        /// VB6: Function IsSchrikkelJaar — returns true when intJaar is a leap year.
        /// </summary>
        public static bool IsSchrikkelJaar(int intJaar)
        {
            return DateTime.IsLeapYear(intJaar);
        }

        /// <summary>
        /// VB6: Function objectValue — returns the value, or empty string if null/DBNull.
        /// </summary>
        public static object ObjectValue(object dbValue)
        {
            return (dbValue == null || dbValue is DBNull) ? (object)"" : dbValue;
        }
    }
}

