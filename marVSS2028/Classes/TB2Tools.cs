using System;
using ADODB;

using static marVSS2028.Classes.Globals;

namespace marVSS2028.Classes
{
    internal static class TB2Tools
    {
        public static string Tb2Indent(string mapiString)
        {
            string[] xArray = mapiString.Split('\'');

            string strIndent = "";
            int tabIndent = 0;

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            for (int telIndent = 0; telIndent <= xArray.Length - 2; telIndent++)
            {
                // Maak eerst het aantal tabs klaar voor de volgende stringreeks
                for (int telTabIndent = 1; telTabIndent <= tabIndent; telTabIndent++)
                    strIndent += "\t";

                // TELEBIB2 extra info eventueel toe te voegen...
                if (xArray[telIndent].Length == 0)
                {
                    // Stop
                }
                else
                {
                    string strComment = TB2Commentaar(xArray[telIndent]);
                    strIndent += xArray[telIndent] + strComment + "\r\n";

                    // Aanpassen van de tabIndent voor de volgende tekenreeks
                    switch (xArray[telIndent].Substring(0, 3))
                    {
                        case "XGH":
                        case "XEH":
                        case "XRH":
                            tabIndent++;
                            break;

                        case "XGT":
                        case "XET":
                        case "XRT":
                            // niks doen, zie hieronder!
                            break;
                    }

                    // Aanpassen van de tabIndent voor afsluiting tekenreeks
                    if (xArray[telIndent + 1].Length >= 3)
                    {
                        switch (xArray[telIndent + 1].Substring(0, 3))
                        {
                            case "XGT":
                            case "XET":
                            case "XRT":
                                tabIndent--;
                                break;
                        }
                    }
                }
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            return strIndent;
        }

        public static string HeadComLists(string strDE, string strQualifiant, string strOOD)
        {
            var rsTB2Qualifiers = new Recordset();
            rsTB2Qualifiers.CursorLocation = CursorLocationEnum.adUseClient;

            string msg = "SELECT * From A_DE_QUALIFIANT WHERE DE = '" + strDE + "' AND Qualifiant = '" + strQualifiant + "'";
            rsTB2Qualifiers.Open(msg, adTBIB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

            if (rsTB2Qualifiers.RecordCount != 1)
            {
                rsTB2Qualifiers.Close();
                return " - ";
            }

            string result = rsTB2Qualifiers.Fields["Lbc-2"].Value?.ToString() ?? "";

            // Tot slot nog controleren of gebruiksdatum niet overschreden
            string datDel = rsTB2Qualifiers.Fields["Datdel"].Value?.ToString() ?? "";
            if (datDel != "")
            {
                // ok blijkbaar niet meer in gebruik!
                strOOD = datDel;
            }

            rsTB2Qualifiers.Close();
            return result;
        }

        public static string SubComLists(string strCode, string strValeur)
        {
            var rsTB2Lists = new Recordset();
            rsTB2Lists.CursorLocation = CursorLocationEnum.adUseClient;

            string msg = "SELECT * From VALEUR WHERE Code = '" + strCode + "' AND Valeur = '" + strValeur + "'";
            rsTB2Lists.Open(msg, adTBIB, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);

            string result = rsTB2Lists.RecordCount != 1
                ? ""
                : rsTB2Lists.Fields["Lbl-2"].Value?.ToString() ?? "";

            rsTB2Lists.Close();
            return result;
        }

        public static string TB2Commentaar(string tb2Code)
        {
            string[] yArray = tb2Code.Split('+');
            string[] zArray;

            string headCommentaar = "";
            string subCommentaar = "";
            string outOfDateSince = "";

            switch (yArray[0])
            {
                case "GIS":
                    if (yArray.Length > 2)
                        subCommentaar = SubComLists(yArray[1], yArray[2]);
                    // GIS= Process indicator= X021
                    headCommentaar = HeadComLists("X021", yArray[1], outOfDateSince);
                    break;

                case "IPD":
                    if (yArray.Length > 2)
                    {
                        zArray = yArray[2].Split(':');
                        subCommentaar = SubComLists(yArray[1], zArray[0]);
                    }
                    // IPD: Segment Product=X060
                    headCommentaar = HeadComLists("X060", yArray[1], outOfDateSince);
                    break;

                case "ATT":
                    if (yArray.Length > 2)
                        subCommentaar = SubComLists(yArray[1], yArray[2]);
                    // ATT: Attribute=X010
                    headCommentaar = HeadComLists("X010", yArray[1], outOfDateSince);
                    break;

                case "RFF":
                    // RFF: Reference qualifier=X050 én subReference Details=X032
                    zArray = yArray[1].Split(':');
                    if (zArray.Length > 2)
                        subCommentaar = HeadComLists("X032", zArray[2], outOfDateSince);
                    headCommentaar = HeadComLists("X050", zArray[0], outOfDateSince);
                    break;

                case "DTM":
                    // DTM: Date/Time qualifier=X016 én format qualifier X018
                    zArray = yArray[1].Split(':');
                    if (zArray.Length == 3)
                        subCommentaar = "=" + HeadComLists("X018", zArray[2], outOfDateSince);
                    headCommentaar = HeadComLists("X016", zArray[0], outOfDateSince);
                    break;

                case "QRS":
                    // QRS: Declaration qualifier=X045 én eventuele response code
                    zArray = yArray[1].Split(':');
                    if (yArray.Length == 3)
                        subCommentaar = "=" + HeadComLists("X046", yArray[2], outOfDateSince);
                    headCommentaar = HeadComLists("X045", zArray[0], outOfDateSince);
                    break;

                case "BIN":
                    // BIN: Boolean indicator=X069 én eventueel boolean indicator value X070
                    if (yArray.Length == 3)
                        subCommentaar = HeadComLists("X070", yArray[2], outOfDateSince);
                    headCommentaar = HeadComLists("X069", yArray[1], outOfDateSince);
                    break;

                case "QTY":
                    // QTY: Quantity qualifier=X047, cijfer zelf, aantaldecimalen en gecodeerde maateenheid X049
                    zArray = yArray[1].Split(':');
                    if (zArray.Length > 3)
                        subCommentaar = HeadComLists("X049", zArray[3], outOfDateSince);
                    headCommentaar = HeadComLists("X047", zArray[0], outOfDateSince);
                    break;

                case "COM":
                    // COM:
                    zArray = yArray[1].Split(':');
                    headCommentaar = HeadComLists("X013", zArray[0], outOfDateSince);
                    break;

                case "ICD":
                    // ICD: Waarborgen=X058
                    zArray = yArray[1].Split(':');
                    headCommentaar = HeadComLists("X058", zArray[0], outOfDateSince);
                    break;

                case "MOA":
                    // MOA: Monetairy Amount=X028
                    zArray = yArray[1].Split(':');
                    if (zArray.Length > 2)
                        subCommentaar = HeadComLists("X031", zArray[2], outOfDateSince);
                    headCommentaar = HeadComLists("X028", zArray[0], outOfDateSince);
                    break;

                case "PCD":
                    // Percentage qualifier= X038, cijfer zelf, aantal decimalen
                    zArray = yArray[1].Split(':');
                    headCommentaar = HeadComLists("X038", zArray[0], outOfDateSince);
                    break;

                case "PTY":
                    // PTY: Party Identification, party qualifier=X043
                    headCommentaar = HeadComLists("X043", yArray[1], outOfDateSince);
                    break;

                case "NME":
                    // NME: Name qualifier=X033
                    headCommentaar = HeadComLists("X033", yArray[1], outOfDateSince);
                    break;

                case "ADR":
                    // ADR: Adres qualifier=X001
                    headCommentaar = HeadComLists("X001", yArray[1], outOfDateSince);
                    break;

                case "DOC":
                    // DOC: Document=X015
                    headCommentaar = HeadComLists("X015", yArray[1], outOfDateSince);
                    break;

                case "PFN":
                    // PFN: Beroep
                    headCommentaar = HeadComLists("X040", yArray[1], outOfDateSince);
                    break;

                case "PER":
                    // PER: Period Qualifier=X072
                    headCommentaar = HeadComLists("X072", yArray[1], outOfDateSince);
                    break;

                case "ROD":
                    // ROD: Risico Object=X052
                    headCommentaar = HeadComLists("X052", yArray[1], outOfDateSince);
                    break;

                case "IFD":
                    break;

                default:
                    return "";
            }

            return "\t(" + headCommentaar + " " + subCommentaar + (" " + outOfDateSince).TrimEnd() + ")";
        }

        public static string RodCheck(string mapiString)
        {
            string[] rodArray = mapiString.Split(new[] { "ROD+" }, StringSplitOptions.None);
            string rodTmp = "";

            for (int telRod = 1; telRod <= rodArray.Length - 1; telRod++)
            {
                string rodHier = HeadComLists("X052", rodArray[telRod].Substring(0, 3), "");
                if (!rodTmp.Contains(rodHier))
                {
                    rodTmp += rodHier + (telRod == rodArray.Length - 1 ? "" : "; ");
                }
            }

            return rodTmp != "" ? "Verzekerd risico: " + rodTmp : "";
        }

        public static string IcdCheck(string mapiString)
        {
            string[] icdArray = mapiString.Split(new[] { "ICD+" }, StringSplitOptions.None);
            string icdTmp = "";

            for (int telIcd = 1; telIcd <= icdArray.Length - 1; telIcd++)
            {
                string icdHier = HeadComLists("X058", icdArray[telIcd].Substring(0, 3), "");
                if (!icdTmp.Contains(icdHier))
                {
                    icdTmp += icdHier + (telIcd == icdArray.Length - 1 ? "" : "; ");
                }
            }

            return icdTmp != "" ? "Waarborgen: " + icdTmp : "";
        }

        public static string FtxCheck(string mapiString)
        {
            string[] ftxArray = mapiString.Split(new[] { "FTX+018+" }, StringSplitOptions.None);
            string ftxTmp = "";

            for (int telFtx = 1; telFtx <= ftxArray.Length - 1; telFtx++)
            {
                int quotePos = ftxArray[telFtx].IndexOf('\'');
                if (quotePos > 0)
                    ftxTmp += " " + ftxArray[telFtx].Substring(0, quotePos);
            }

            // checken voor +08 en :
            while (ftxTmp.Contains("+08"))
                ftxTmp = ftxTmp.Replace("+08", "");

            while (ftxTmp.Contains(":"))
                ftxTmp = ftxTmp.Replace(":", "");

            return ftxTmp != "" ? "Bericht van de verzekeraar:\r\n" + ftxTmp : "";
        }
    }
}
