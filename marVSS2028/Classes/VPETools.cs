using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using IDEALSoftware.VpeCommunity;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.ShellHelper;

namespace marVSS2028.Classes
{
    internal static class VPETools
    {
        // ── pdfPrintUserDef ──────────────────────────────────────────────────────

        public static void PdfPrintUserDef(string typeEnTaal, double pdfOVSStrook)
        {
            string defFile = LOCATION_COMPANYDATA + @"vpeSjbs\pdfDDEF" + typeEnTaal + ".Txt";
            if (!File.Exists(defFile))
                return;

            var report = Mim.Report;
            report.nTopMargin = 1;
            report.nLeftMargin = 0.5;
            report.nRightMargin = 20.8;
            report.nBottomMargin = 29.8;

            using (var reader = new StreamReader(defFile))
            {
                string pdfCmd;
                while ((pdfCmd = reader.ReadLine()) != null)
                {
                    if (pdfCmd.Length > 0 && pdfCmd[0] == '\'')
                        continue;

                    switch (pdfCmd.Trim().ToUpper())
                    {
                        case "CMD-VSOFTSPACE":
                            CmdVSoftSpace(reader);
                            break;

                        case "CMD-ADRESSPACE":
                            CmdAdresSpace(reader);
                            break;

                        case "CMD-WRITE":
                            CmdWrite(reader, report);
                            break;

                        case "CMD-WRITEBOX":
                            CmdWriteBox(reader, report);
                            break;

                        case "CMD-PRINT":
                            CmdPrint(reader, report, pdfOVSStrook);
                            break;

                        case "CMD-PICTURE":
                            CmdPicture(reader, report);
                            break;

                        default:
                            MessageBox.Show(pdfCmd + " nog niet voorzien", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            break;
                    }
                }
            }
        }

        // ── GenerateVpeDoc ───────────────────────────────────────────────────────

        public static bool GenerateVpeDoc(string location, string docName)
        {
            string fullPath = location + docName;

        VPE_ADD:
            if (!File.Exists(fullPath))
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(fullPath + "\r\nkan niet gevonden worden.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show(
                    "marIntegraal probeert een standaardversie te laden. Vergeet niet te wijzigen met uw bedrijfsinfo.\r\n" +
                    "De map: " + location + " wordt ter info geopend",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                string sourcePath = AppDomain.CurrentDomain.BaseDirectory + @"vpeSjbs\";
                if (!CopyFile(sourcePath, location, docName))
                {
                    MessageBox.Show(sourcePath + docName + "\r\nkan niet gevonden worden.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("Installeer marIntegraal opnieuw.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ShellExecuteWithFallback(location);
                goto VPE_ADD;
            }

            var report = Mim.Report;
            report.PageBreak();

            using (var reader = new StreamReader(fullPath))
            {
                string pdfCmd;
                while ((pdfCmd = reader.ReadLine()) != null)
                {
                    if (pdfCmd.Length > 0 && pdfCmd[0] == '\'')
                        continue;

                    switch (pdfCmd.Trim().ToUpper())
                    {
                        case "CMD-WRITE":
                            CmdWrite(reader, report);
                            break;

                        case "CMD-WRITEBOX":
                            CmdWriteBox(reader, report);
                            break;

                        case "CMD-PRINT":
                            CmdPrint(reader, report, 0);
                            break;

                        case "CMD-PICTURE":
                            CmdPicture(reader, report);
                            break;

                        default:
                            MessageBox.Show(pdfCmd + " niet voorzien in deze function", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            break;
                    }
                }
            }

            return true;
        }

        // ── Private command handlers (replace VB6 GoSub blocks) ─────────────────

        private static void CmdVSoftSpace(StreamReader reader)
        {
            string line = reader.ReadLine() ?? "";
            var parts = line.Split(',');
            if (parts.Length >= 2)
            {
                double.TryParse(parts[0].Trim(), out double vanaf);
                double.TryParse(parts[1].Trim(), out double tot);
                pdfVsoftVanaf = vanaf;
                pdfVsoftTot = tot;
            }
        }

        private static void CmdAdresSpace(StreamReader reader)
        {
            string line = reader.ReadLine() ?? "";
            var parts = line.Split(',');
            if (parts.Length >= 4)
            {
                double.TryParse(parts[0].Trim(), out double x);
                double.TryParse(parts[1].Trim(), out double y);
                double.TryParse(parts[2].Trim(), out double x2);
                double.TryParse(parts[3].Trim(), out double y2);
                pdfadresXpos = x;
                pdfadresYpos = y;
                pdfadresXpos2 = x2;
                pdfadresYpos2 = y2;
            }
        }

        private static void CmdPicture(StreamReader reader, VpeControl report)
        {
            string line = reader.ReadLine() ?? "";
            var parts = line.Split(',');
            if (parts.Length < 6) return;

            double.TryParse(parts[0].Trim(), out double xPos);
            double.TryParse(parts[1].Trim(), out double yPos);
            double.TryParse(parts[2].Trim(), out double xPos2);
            double.TryParse(parts[3].Trim(), out double yPos2);
            double.TryParse(parts[4].Trim(), out double penSize);
            string fileName = parts[5].Trim().Trim('"');

            report.PenSize = penSize;
            report.PictureBestFit = true;

            if (fileName.Length >= 4 && fileName.Substring(0, 4) == "[BL]")
                fileName = LOCATION_COMPANYDATA + fileName.Substring(4).TrimStart('\\', '/');
            else if (fileName.Length >= 4 && fileName.Substring(0, 4) == "[PL]")
                fileName = PROGRAM_LOCATION + fileName.Substring(4).TrimStart('\\', '/');

            report.Picture(xPos, yPos, xPos2, yPos2, fileName);
        }

        // Converts a VB6 OLE color long to a System.Drawing.Color
        private static Color OleColorToColor(double oleColor)
        {
            return ColorTranslator.FromOle((int)oleColor);
        }

        private static void ApplyFont(VpeControl report, string fontName, double fontSize,
            double color, double bold, double italic, double underline)
        {
            report.FontName = fontName;
            report.FontSize = (int)fontSize;
            report.TextColor = OleColorToColor(color);
            report.TextBold = bold != 0;
            report.TextItalic = italic != 0;
            report.TextUnderline = underline != 0;
        }

        private static void CmdWrite(StreamReader reader, VpeControl report)
        {
            string paramLine = reader.ReadLine() ?? "";
            var parts = paramLine.Split(',');
            if (parts.Length < 11) return;

            double.TryParse(parts[0].Trim(), out double xPos);
            double.TryParse(parts[1].Trim(), out double yPos);
            double.TryParse(parts[2].Trim(), out double xPos2);
            double.TryParse(parts[3].Trim(), out double yPos2);
            double.TryParse(parts[4].Trim(), out double fontSize);
            string fontName = parts[5].Trim();
            double.TryParse(parts[6].Trim(), out double color);
            double.TryParse(parts[7].Trim(), out double align);
            double.TryParse(parts[8].Trim(), out double bold);
            double.TryParse(parts[9].Trim(), out double italic);
            double.TryParse(parts[10].Trim(), out double underline);

            string textstring = reader.ReadLine() ?? "";
            string texttmp;
            while ((texttmp = reader.ReadLine()) != null)
            {
                if (texttmp == "CMD-ENDWRITE") break;
                textstring += "\r\n" + texttmp;
            }

            ApplyFont(report, fontName, fontSize, color, bold, italic, underline);
            report.TextAlignment = (TextAlignment)(int)align;
            report.Write(xPos, yPos, xPos2, yPos2, textstring);
        }

        private static void CmdWriteBox(StreamReader reader, VpeControl report)
        {
            string paramLine = reader.ReadLine() ?? "";
            var parts = paramLine.Split(',');
            if (parts.Length < 11) return;

            double.TryParse(parts[0].Trim(), out double xPos);
            double.TryParse(parts[1].Trim(), out double yPos);
            double.TryParse(parts[2].Trim(), out double xPos2);
            double.TryParse(parts[3].Trim(), out double yPos2);
            double.TryParse(parts[4].Trim(), out double fontSize);
            string fontName = parts[5].Trim();
            double.TryParse(parts[6].Trim(), out double color);
            double.TryParse(parts[7].Trim(), out double align);
            double.TryParse(parts[8].Trim(), out double bold);
            double.TryParse(parts[9].Trim(), out double italic);
            double.TryParse(parts[10].Trim(), out double underline);

            string textstring = reader.ReadLine() ?? "";
            string texttmp;
            while ((texttmp = reader.ReadLine()) != null)
            {
                if (texttmp == "CMD-ENDWRITE") break;
                textstring += "\r\n" + texttmp;
            }

            ApplyFont(report, fontName, fontSize, color, bold, italic, underline);
            report.TextAlignment = (TextAlignment)(int)align;
            report.WriteBox(xPos, yPos, xPos2, yPos2, textstring);
        }

        private static void CmdPrint(StreamReader reader, VpeControl report, double pdfOVSStrook)
        {
            string paramLine = reader.ReadLine() ?? "";
            var parts = paramLine.Split(',');
            if (parts.Length < 8) return;

            double.TryParse(parts[0].Trim(), out double xPos);
            double.TryParse(parts[1].Trim(), out double yPos);
            double.TryParse(parts[2].Trim(), out double fontSize);
            string fontName = parts[3].Trim();
            double.TryParse(parts[4].Trim(), out double color);
            double.TryParse(parts[5].Trim(), out double bold);
            double.TryParse(parts[6].Trim(), out double italic);
            double.TryParse(parts[7].Trim(), out double underline);

            string textstring = reader.ReadLine() ?? "";

            ApplyFont(report, fontName, fontSize, color, bold, italic, underline);

            double printY = (pdfOVSStrook > 0 && yPos > pdfVsoftTot)
                ? yPos - pdfOVSStrook
                : yPos;

            report.Print(xPos, printY, textstring);
        }
    }
}

