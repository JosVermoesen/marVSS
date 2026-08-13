using System;
using System.IO;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MimEnvironment;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.TB2Tools;

namespace marVSS2028.Classes
{
    internal static class BrokerTools
    {
        // ── kt: VB6 zone-decimal conversion (Overpunch / EBCDIC trailing sign) ──

        public static string Kt(string fBedrag)
        {
            if (string.IsNullOrEmpty(fBedrag)) return fBedrag;
            char last = fBedrag[fBedrag.Length - 1];
            int asc = (int)last;
            string body = fBedrag.Substring(0, fBedrag.Length - 1);

            switch (asc)
            {
                case int n when (n >= 48 && n <= 57) || n == 32:
                    return fBedrag;

                case 232:
                case 233:
                    return body + "0";

                case int n when n >= 65 && n <= 73:
                    return body + (asc - 64).ToString("0");

                case int n when n >= 74 && n <= 82:
                    return "-" + body.Substring(1) + (asc - 73).ToString("0");

                default:
                    MessageBox.Show(
                        "Foutieve waarde in conversietafel voor '" + fBedrag + "'\r\n\r\nKontakteer onmiddellijk de maatschappij !!",
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return new string('0', fBedrag.Length);
            }
        }

        // ── Puur: strip .  /  - from a number string ────────────────────────────

        public static string Puur(string nummer)
        {
            foreach (char c in new[] { '.', '/', '-' })
            {
                int pos;
                while ((pos = nummer.IndexOf(c)) >= 0)
                    nummer = nummer.Remove(pos, 1);
            }
            return nummer;
        }

        // ── Tk: zero-padded formatted number ────────────────────────────────────

        public static string Tk(string fBedrag, int lengte)
        {
            return double.TryParse(fBedrag, out double val)
                ? val.ToString(new string('0', lengte))
                : new string('0', lengte);
        }

        // ── KTRLVerzoek ─────────────────────────────────────────────────────────

        public static bool KTRLVerzoek(string verzoekstring, string maatschappij)
        {
            BGet(TABLE_VARIOUS, 1, VSet("23CO" + maatschappij, 20));
            if (Ktrl != 0)
                return false;

            RecordToVeld(TABLE_VARIOUS);
            string v220 = VBibText(TABLE_VARIOUS, "#v220 #");
            int index = int.Parse(verzoekstring);
            if (index >= 1 && index <= v220.Length && v220[index - 1] == '1')
                return true;

            return false;
        }

        // ── SnippetXEH ──────────────────────────────────────────────────────────

        public static string SnippetXEH(string userArea, string polisNummer, bool blIndent)
        {
            string zoekXEH;
            string zoekXET;

            if (userArea.Contains("XET+03"))
            {
                zoekXEH = "XEH+03"; // termijnen en contanten via borderels
                zoekXET = "XET+03";
            }
            else if (userArea.Contains("XRT+2"))
            {
                zoekXET = "XRT+1"; // commissies via rekeninguittreksels
                zoekXEH = "XRH+1";
            }
            else
            {
                MessageBox.Show("geen XET te vinden!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }

            // termijnhernieuwing opzoeken voor dit contract
            string[] xehArray = userArea.Split(new[] { zoekXET }, StringSplitOptions.None);
            for (int countTo = 0; countTo <= xehArray.Length - 1; countTo++)
            {
                if (xehArray[countTo].Contains("RFF+001:" + polisNummer))
                {
                    if (!blIndent)
                    {
                        int pos = xehArray[countTo].IndexOf("'" + zoekXEH, StringComparison.Ordinal);
                        return xehArray[countTo].Substring(pos) + zoekXET + "'";
                    }
                    else
                    {
                        return Tb2Indent(xehArray[countTo] + zoekXET + "'");
                    }
                }
            }
            return "";
        }

        // ── MijStatistiek ───────────────────────────────────────────────────────

        public static void MijStatistiek()
        {
            string msg = "Beschikbare toepassingen maatschappijen opvragen ?\r\n\r\n(enkel nuttig bij eerste gebruik Assurnet !)";
            if (MessageBox.Show(msg, "Verzoek 912", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            char[] a = new string(' ', 140).ToCharArray();
            char[] aa = new string(' ', 420).ToCharArray();

            SetMid(a, 1, "AS1");
            SetMid(a, 16, "0");
            SetMid(a, 17, "912");
            SetMid(a, 20, "1");
            SetMid(a, 21, "1");
            SetMid(a, 22, VSet(ProducentNummer, 8));
            SetMid(a, 30, DateTime.Now.ToString("yyMMddHHmmss"));
            SetMid(a, 42, "1");
            SetMid(a, 43, "4");
            SetMid(a, 44, "XXXX");
            SetMid(a, 48, "01");
            SetMid(a, 50, "X");

            SetMid(aa, 1, VSet(ProducentNummer, 8));
            SetMid(aa, 9, "1"); // alle mijen

            using (var sw = new StreamWriter(LOCATION_ASWEB + "AS1.SND", append: true))
                sw.WriteLine(new string(a) + new string(aa));
        }

        // ── KTRLMijStatistiek ───────────────────────────────────────────────────

        public static void KTRLMijStatistiek(string verzoekTxt)
        {
            string msgTxt;
            int aaLen;
            switch (verzoekTxt)
            {
                case "912": msgTxt = "Beschikbare toepassingen "; aaLen = 420; break;
                case "915": msgTxt = "Bepalingen "; aaLen = 150; break;
                default: msgTxt = verzoekTxt + " "; aaLen = 150; break;
            }

            string prompt = msgTxt + "maatschappijen opvragen ?\r\n\r\n(enkel nuttig bij eerste gebruik Assurnet !)";
            if (MessageBox.Show(prompt, "Verzoek " + verzoekTxt, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            char[] a = new string(' ', 140).ToCharArray();
            char[] aa = new string(' ', aaLen).ToCharArray();

            SetMid(a, 1, "AS1");
            SetMid(a, 16, "0");
            SetMid(a, 17, verzoekTxt);
            SetMid(a, 20, "1");
            SetMid(a, 21, "1");
            SetMid(a, 22, VSet(ProducentNummer, 8));
            SetMid(a, 30, DateTime.Now.ToString("yyMMddHHmmss"));
            SetMid(a, 42, "1");
            SetMid(a, 43, "4");
            SetMid(a, 44, "XXXX");
            SetMid(a, 48, "01");
            SetMid(a, 50, "X");

            SetMid(aa, 1, VSet(ProducentNummer, 8));
            switch (verzoekTxt)
            {
                case "912":
                    SetMid(aa, 9, "1"); // alle mijen
                    break;
                case "915":
                    SetMid(aa, 9, "9"); // taalkode NL+FR
                    SetMid(aa, 10, "1"); // alle BVVO kodes
                    SetMid(aa, 61, "1"); // alle takken/ondertakken
                    SetMid(aa, 92, "1"); // verklarende tekst
                    SetMid(aa, 93, "1"); // verbindingen tussen gegevens
                    break;
            }

            using (var sw = new StreamWriter(LOCATION_ASWEB + "AS1.SND", append: true))
                sw.WriteLine(new string(a) + new string(aa));
        }

        // ── LogBoekUpdate ───────────────────────────────────────────────────────

        public static void LogBoekUpdate()
        {
            if (!File.Exists(LOCATION_ASWEB + "AS1.SND"))
                return;

            int recordOK = 0;
            int recordNOT = 0;
            StreamWriter swDum = null;

            BBegin();

            foreach (string lineA in File.ReadLines(LOCATION_ASWEB + "AS1.SND"))
            {
                string a = lineA;
                string as1VerzoekNummer = Mid1(a, 17, 3);
                if (as1VerzoekNummer == "912" || as1VerzoekNummer == "915")
                    continue;

                if (Mid1(a, 16, 1) != "1")
                {
                    recordNOT++;
                    swDum = EnsureDumWriter(swDum);
                    swDum.WriteLine(a);
                    continue;
                }

                TLB_RECORD[TABLE_VARIOUS] = "";
                string polisNummer;
                string as1VerzoekTekst;

                switch (as1VerzoekNummer)
                {
                    case "020":
                        polisNummer = Mid1(a, 92, 12);
                        as1VerzoekTekst = Mid1(a, 17, 3) + ":" + Mid1(a, 51, 40);
                        break;
                    case "022":
                        polisNummer = Mid1(a, 141, 12);
                        as1VerzoekTekst = Mid1(a, 17, 3) + ":Wijziging inlichtingen Klient";
                        break;
                    case "024":
                        polisNummer = Mid1(a, 141, 12);
                        as1VerzoekTekst = Mid1(a, 17, 3) + ":Wijzig. inlichtingen polis auto";
                        break;
                    case "027":
                    case "028":
                        polisNummer = Mid1(a, 141, 12);
                        as1VerzoekTekst = Mid1(a, 17, 3) + ":" + Mid1(a, 245, 2) + "/" + Mid1(a, 211, 9) + "/" + Mid1(a, 220, 8);
                        break;
                    case "913":
                        MessageBox.Show("913 *error*");
                        continue;
                    default:
                        MessageBox.Show("Niet beschikbare funktiekode : " + Mid1(a, 17, 3) + "\r\n\r\nRaadpleeg R&V 053/21.59.25");
                        recordNOT++;
                        swDum = EnsureDumWriter(swDum);
                        swDum.WriteLine(a);
                        continue;
                }

                recordOK++;
                TLB_RECORD[TABLE_VARIOUS] = "";
                VBib(TABLE_VARIOUS, Mid1(a, 4, 12), "v219");
                VBib(TABLE_VARIOUS, polisNummer, "A000");
                VBib(TABLE_VARIOUS, a, "v228");
                VBib(TABLE_VARIOUS, "26" + VBibText(TABLE_VARIOUS, "#v219 #"), "v005");
                BInsert(TABLE_VARIOUS, 1);
                if (Ktrl != 0)
                    MessageBox.Show("Fout bij invoegen 26-record");

                if (Mid1(a, 50, 1) == "X")
                {
                    VBib(TABLE_VARIOUS, as1VerzoekTekst, "v105");
                    VBib(TABLE_VARIOUS, polisNummer, "A000");
                    VBib(TABLE_VARIOUS, Mid1(a, 4, 12), "v219");
                    VBib(TABLE_VARIOUS, DateTime.Now.ToString("yyyy").Substring(0, 2) + Mid1(a, 30, 12), "v128");
                    VBib(TABLE_VARIOUS, "22" + VBibText(TABLE_VARIOUS, "#A000 #"), "v005");
                    BInsert(TABLE_VARIOUS, 1);
                    if (Ktrl != 0)
                    {
                        MessageBox.Show("Fout bij invoegen 22-record");
                        continue;
                    }
                }
            }

            swDum?.Close();
            BEnd();

            string msgResult =
                recordOK + " verzoeken werden in het logboek ingeschreven.\r\n\r\n" +
                "Nog " + recordNOT + " verzoeken te versturen naar ASSURNET\r\n" +
                "via de keuze [B]estandsoverdracht in de AS/1 menu.";
            MessageBox.Show(msgResult, "AS/1 Logboek bestandsoverdracht");

            File.Delete(LOCATION_ASWEB + "AS1.SND");
            string dumPath = LOCATION_ASWEB + "AS1.$$$";
            if (File.Exists(dumPath))
                File.Move(dumPath, LOCATION_ASWEB + "AS1.SND");
        }

        // ── EdiFactTERMIJN ──────────────────────────────────────────────────────
        // NOTE: This method contains many references to VB6 forms (BYPERDAT,
        // KwijtingBoeken) and legacy data-access helpers (BGet, VBib, RecordToVeld,
        // BInsert, BUpdate) that must be wired to their C# equivalents in the
        // calling context. The logic is faithfully preserved; UI/form calls are
        // kept as MessageBox stubs where the original used MsgBox.

        public static void EdiFactTERMIJN(string userArea)
        {
            string[] xArray = userArea.Split('\'');
            string[] xghArray = xArray[0].Split('+');
            string maatschappij = double.Parse(xghArray[2]).ToString("0000");

            string maskerHier = "######0.00";
            string gridText = "";
            int maandVerwerkingTermijn = 0;
            int xrhNiveau = 0;
            int lijnTeller = 0;
            bool isTotaalMOA = false;
            bool contanteFlag = false;
            bool inningmijFlag = false;
            bool mijWijzigen = false;
            int typekwijtingFlag = 0;

            decimal bedrag0 = 0, bedrag1 = 0, bedrag2 = 0, bedrag3 = 0, bedrag4 = 0;
            decimal bedrag5 = 0, bedrag7 = 0, bedrag8 = 0, bedrag9 = 0;
            decimal bedrag10 = 0, bedrag11 = 0, bedrag12 = 0;
            decimal totaalNettoPremie = 0, totaalLasten = 0;

            string polisNummer = "";
            string tempoNaamKlant = "";
            string huidigeIndex = "";
            string huidigeBM = "";
            string maandVerwerking = "";
            string boekjaarKontrole = "";
            string dagKwijting = "";
            string maandKwijting = "";
            string datumKwijting = "";
            string[] moaArray = new string[0];
            string[] attArray, xehArray, ptxArray, polArray, dtmArray;
            string check06 = "";
            string bedragPremie = "";

            BL_LOGGING = true;

            for (int telhier = 0; telhier <= xArray.Length - 2; telhier++)
            {
                string seg = xArray[telhier];
                if (seg.Length < 3) continue;
                string tag = seg.Substring(0, 3);

                switch (tag)
                {
                    case "XEH":
                        isTotaalMOA = true;
                        string xeh6 = seg.Length >= 6 ? seg.Substring(0, 6) : seg;
                        switch (xeh6)
                        {
                            case "XEH+01":
                                contanteFlag = true;
                                inningmijFlag = false;
                                check06 = "01";
                                break;
                            case "XEH+03":
                                lijnTeller++;
                                inningmijFlag = false;
                                check06 = "03";
                                break;
                            case "XEH+06":
                                check06 = xeh6;
                                break;
                            default:
                                System.Diagnostics.Debug.Assert(false, "Unexpected XEH: " + seg);
                                break;
                        }
                        break;

                    case "XRH":
                        isTotaalMOA = false;
                        break;

                    case "DTM":
                        dtmArray = seg.Split(':');
                        if (seg.StartsWith("DTM+005:"))
                        {
                            string fmt = dtmArray[dtmArray.Length - 1];
                            if (fmt == "010")
                            {
                                if (dtmArray[1].Length == 8)
                                {
                                    maandVerwerking = dtmArray[1].Substring(4, 2);
                                    boekjaarKontrole = dtmArray[1].Substring(0, 4);
                                }
                                else
                                    MessageBox.Show("stoppen en controleren a.u.b");
                            }
                            else
                            {
                                if (dtmArray[1].Length == 8)
                                {
                                    maandVerwerking = dtmArray[1].Substring(2, 2);
                                    boekjaarKontrole = dtmArray[1].Substring(4, 4);
                                }
                                else if (fmt == "001")
                                {
                                    maandVerwerking = Mid1(seg, 11, 2);
                                    boekjaarKontrole = Mid1(seg, 13, 4);
                                }
                                else if (fmt == "005")
                                {
                                    maandVerwerking = Mid1(seg, 9, 2);
                                    boekjaarKontrole = Mid1(seg, 11, 4);
                                }
                                else
                                    MessageBox.Show("stoppen en controleren a.u.b");
                            }
                        }
                        else if (seg.StartsWith("DTM+004:"))
                        {
                            dagKwijting = Mid1(dtmArray[1], 1, 2);
                            maandKwijting = Mid1(dtmArray[1], 3, 2);
                            datumKwijting = dagKwijting + "/" + maandKwijting + "/" + boekjaarKontrole;
                        }
                        else if (seg.StartsWith("DTM+041:"))
                        {
                            if (dtmArray[dtmArray.Length - 1] == "010")
                            {
                                dagKwijting = Mid1(dtmArray[1], 7, 2);
                                maandKwijting = Mid1(dtmArray[1], 5, 2);
                            }
                            else
                            {
                                dagKwijting = Mid1(dtmArray[1], 1, 2);
                                maandKwijting = Mid1(dtmArray[1], 3, 2);
                            }
                            datumKwijting = dagKwijting + "/" + maandKwijting + "/" + boekjaarKontrole;
                        }
                        break;

                    case "RFF":
                        if (seg.StartsWith("RFF+001:"))
                        {
                            polArray = seg.Split(':');
                            polisNummer = polArray[1];
                        }
                        break;

                    case "IND":
                        if (Mid1(seg, 1, 10) == "IND+002+1:")
                            huidigeIndex = (double.Parse(Mid1(seg, 11, 5)) / 100).ToString("000.00");
                        else
                            huidigeIndex = (double.Parse(Mid1(seg, 10, 5)) / 100).ToString("000.00");
                        break;

                    case "MOA":
                        if (isTotaalMOA)
                        {
                            moaArray = seg.Split(':');
                            string moaKey = moaArray[0];
                            switch (moaKey)
                            {
                                case "MOA+039": bedrag10 = ParseMOA(moaArray); break;
                                case "MOA+210": bedrag11 = ParseMOA(moaArray); break;
                                case "MOA+211": bedrag12 = ParseMOA(moaArray); break;
                                case "MOA+012": bedrag0 = ParseMOA(moaArray); break;
                                case "MOA+013":
                                    bedrag1 = ParseMOA(moaArray);
                                    totaalNettoPremie = bedrag1;
                                    break;
                                case "MOA+015":
                                    if (xrhNiveau > 0)
                                        SnelHelpPrint("XRHniveau is : " + xrhNiveau, BL_LOGGING);
                                    else
                                        bedrag9 = ParseMOA(moaArray);
                                    break;
                                case "MOA+016":
                                    bedrag2 = ParseMOA(moaArray);
                                    totaalLasten = bedrag2 + bedrag5;
                                    break;
                                case "MOA+017":
                                    bedrag2 += ParseMOA(moaArray);
                                    totaalLasten = bedrag2 + bedrag5;
                                    break;
                                case "MOA+097": bedrag4 = ParseMOA(moaArray); break;
                                case "MOA+098":
                                    bedrag5 = ParseMOA(moaArray);
                                    totaalLasten = bedrag2 + bedrag5;
                                    break;
                                case "MOA+100":
                                    bedrag2 += ParseMOA(moaArray);
                                    totaalLasten = bedrag2 + bedrag5;
                                    break;
                                case "MOA+105":
                                    break; // Andere aan netto premie toe te voegen kosten
                                default:
                                    SnelHelpPrint("MOA-stop voor " + moaKey, BL_LOGGING);
                                    break;
                            }
                        }
                        break;

                    case "NME":
                        tempoNaamKlant = seg.Substring(8);
                        break;

                    case "ATT":
                        attArray = seg.Split('+');
                        switch (attArray[1])
                        {
                            case "5300":
                                huidigeBM = attArray[2];
                                break;
                            case "B001":
                                typekwijtingFlag = int.Parse(seg[seg.Length - 1].ToString());
                                break;
                            case "B003":
                            case "A600":
                                if (attArray[2] == "3") // Inning Maatschappij
                                {
                                    inningmijFlag = true;
                                    bedrag0 = bedrag1 = bedrag2 = bedrag3 = bedrag4 = 0;
                                    bedrag5 = bedrag7 = bedrag8 = bedrag9 = 0;
                                    bedrag10 = bedrag11 = bedrag12 = 0;
                                    isTotaalMOA = true;
                                    huidigeIndex = "";
                                    huidigeBM = "";

                                    if (!contanteFlag)
                                    {
                                        MessageBox.Show("Kwitantie voor polis " + polisNummer.Trim() + " staat inning Maatschappij");
                                        // Skip segments until XET+03 or XET+06
                                        while (telhier < xArray.Length - 1)
                                        {
                                            telhier++;
                                            if (xArray[telhier] == "XET+03" || xArray[telhier] == "XET+06")
                                                break;
                                            if (telhier > 250)
                                                break;
                                            Application.DoEvents();
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case "XET":
                        if (seg.StartsWith("XET+03"))
                        {
                            bedrag7 = bedrag1 + bedrag4;
                            bedrag8 = bedrag2 + bedrag3 + bedrag5;

                            string moaCurrency = moaArray.Length > 2 ? moaArray[2] : "EUR";
                            if (bhEuro && moaCurrency == "EUR") { /* niets */ }
                            else if (!bhEuro && moaCurrency == "BEF") { /* niets */ }
                            else if (moaCurrency == "BEF")
                            {
                                bedrag7 = Math.Round(bedrag7 / (decimal)EURO, 2);
                                bedrag8 = Math.Round(bedrag8 / (decimal)EURO, 2);
                                bedrag9 = Math.Round(bedrag9 / (decimal)EURO, 2);
                                bedrag0 = Math.Round(bedrag0 / (decimal)EURO, 2);
                            }
                            else if (moaCurrency == "EUR")
                            {
                                bedrag7 = Math.Round(bedrag7 * (decimal)EURO);
                                bedrag8 = Math.Round(bedrag8 * (decimal)EURO);
                                bedrag9 = Math.Round(bedrag9 * (decimal)EURO);
                                bedrag0 = Math.Round(bedrag0 * (decimal)EURO);
                            }
                            else
                                MessageBox.Show("ONMOGELIJKE SITUATIE");

                            bedragPremie = bedrag0 == 0
                                ? Dec((double)(bedrag7 + bedrag8), maskerHier)
                                : Dec((double)bedrag0, maskerHier);

                            BGet(TABLE_CONTRACTS, 0, polisNummer);
                            bool polisNieuw = Ktrl != 0;
                            string dummy;
                            if (polisNieuw)
                            {
                                MessageBox.Show("polis niet aanwezig. EDIFACT nieuw nog in te brengen !!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                TLB_RECORD[TABLE_CONTRACTS] = "";
                                MessageBox.Show("Stop.  Polis bestaat nog niet :" + polisNummer);
                                VBib(TABLE_CONTRACTS, maandKwijting, "v164");
                                VBib(TABLE_CONTRACTS, "NONAME", "A110");
                                VBib(TABLE_CONTRACTS, maatschappij, "A010");
                                VBib(TABLE_CONTRACTS, polisNummer, "A000");
                                VBib(TABLE_CONTRACTS, tempoNaamKlant, "vs99");
                                BInsert(TABLE_CONTRACTS, 0);
                                if (Ktrl != 0) MessageBox.Show("onbekende soldaat ! STOP!!!");
                                dummy = "Kontroleer !!! " + tempoNaamKlant;
                            }
                            else
                            {
                                RecordToVeld(TABLE_CONTRACTS);
                                bool polisTeWijzigen = false;
                                string dagKtrl1 = VBibText(TABLE_CONTRACTS, "#v165 #");
                                string dagKtrl2 = Mid1(VBibText(TABLE_CONTRACTS, "#AW_2 #"), 7, 2);

                                string vs97 = rsMAR[TABLE_CONTRACTS].Fields["vs97"].Value?.ToString() ?? "";
                                if (string.Compare(vs97, "2") > 0)
                                {
                                    MessageBox.Show("Polis " + polisNummer + "\r\nActiecode staat op: " + vs97 + "\r\n\r\nWordt automatisch op 1 (Post) geplaatst", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    VBib(TABLE_CONTRACTS, "1", "vs97");
                                    polisTeWijzigen = true;
                                }
                                if (maandKwijting != VBibText(TABLE_CONTRACTS, "#v164 #"))
                                {
                                    VBib(TABLE_CONTRACTS, maandKwijting, "v164");
                                    polisTeWijzigen = true;
                                }
                                if (dagKtrl1 != dagKtrl2)
                                {
                                    if (MessageBox.Show(dagKtrl2 + " correctie vervalDAG", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        VBib(TABLE_CONTRACTS, dagKtrl2, "v165");
                                        polisTeWijzigen = true;
                                    }
                                }
                                dagKwijting = dagKtrl2;
                                if (maatschappij != VBibText(TABLE_CONTRACTS, "#A010 #"))
                                {
                                    VBib(TABLE_CONTRACTS, maatschappij, "A010");
                                    polisTeWijzigen = true;
                                }
                                if (polisTeWijzigen)
                                    BUpdate(TABLE_CONTRACTS, 0);

                                BGet(TABLE_CUSTOMERS, 0, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12));
                                if (Ktrl != 0)
                                    dummy = "Verbeter !!! " + tempoNaamKlant;
                                else
                                {
                                    RecordToVeld(TABLE_CUSTOMERS);
                                    dummy = VBibText(TABLE_CUSTOMERS, "#A100 #");
                                }
                            }

                            // TeleBibKTRL
                            BGet(TABLE_VARIOUS, 1, "25" + maatschappij + polisNummer);
                            if (Ktrl != 0) TLB_RECORD[TABLE_VARIOUS] = "";
                            else RecordToVeld(TABLE_VARIOUS);

                            if (huidigeIndex.Trim() != "") VBib(TABLE_VARIOUS, huidigeIndex, "AW.R");
                            if (huidigeBM.Trim() != "")
                            {
                                VBib(TABLE_VARIOUS, huidigeBM, "5315");
                                VBib(TABLE_VARIOUS, huidigeBM, "5300");
                            }
                            VBib(TABLE_VARIOUS, bedrag10.ToString(), "v400");
                            VBib(TABLE_VARIOUS, bedrag11.ToString(), "v401");
                            VBib(TABLE_VARIOUS, bedrag12.ToString(), "v402");
                            VBib(TABLE_VARIOUS, (bedrag7 + bedrag8).ToString(), "B010");
                            VBib(TABLE_VARIOUS, bedrag8.ToString(), "B011");
                            VBib(TABLE_VARIOUS, bedrag7.ToString(), "B013");
                            VBib(TABLE_VARIOUS, bedrag9.ToString(), "B014");
                            VBib(TABLE_VARIOUS, bedrag0.ToString(), "v390");
                            VBib(TABLE_VARIOUS, bedrag1.ToString(), "v391");
                            VBib(TABLE_VARIOUS, bedrag2.ToString(), "v392");
                            VBib(TABLE_VARIOUS, bedrag4.ToString(), "v393");
                            VBib(TABLE_VARIOUS, bedrag5.ToString(), "v394");
                            VBib(TABLE_VARIOUS, VSet("K" + VBibText(TABLE_CONTRACTS, "#A110 #"), 13), "v004");
                            VBib(TABLE_VARIOUS, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12), "A110");
                            VBib(TABLE_VARIOUS, maatschappij, "A010");
                            VBib(TABLE_VARIOUS, polisNummer, "A000");
                            VBib(TABLE_VARIOUS, VSet("25" + maatschappij + polisNummer, 20), "v005");

                            if (Ktrl != 0) BInsert(TABLE_VARIOUS, 1); else BUpdate(TABLE_VARIOUS, 1);
                            if (Ktrl != 0) MessageBox.Show("Stopkode " + Ktrl);

                            if (int.Parse(dagKwijting == "" ? "0" : dagKwijting) == 0)
                                dagKwijting = "01";

                            // Overwrite first 2 chars of datumKwijting with dagKwijting
                            if (datumKwijting.Length >= 2)
                                datumKwijting = dagKwijting + datumKwijting.Substring(2);

                            if (!inningmijFlag)
                            {
                                gridText += polisNummer + "\t";
                                gridText += datumKwijting + "\t";
                                gridText += bedragPremie + "\t";
                                gridText += Dec((double)bedrag9, maskerHier) + "\t";
                                gridText += dummy + "\t";
                                gridText += SnippetXEH(userArea, polisNummer.Trim(), false) + "\t";
                                gridText += totaalNettoPremie + "\t" + totaalLasten + "\r";
                            }

                            // Reset bedragen
                            bedrag0 = bedrag1 = bedrag2 = bedrag3 = bedrag4 = 0;
                            bedrag5 = bedrag7 = bedrag8 = bedrag9 = 0;
                            huidigeIndex = "";
                            huidigeBM = "";
                        }
                        break;

                    case "XGH" when tag == "XRH":
                        xrhNiveau++;
                        break;

                    case "XRT":
                        xrhNiveau--;
                        break;

                    case "PTY":
                        ptxArray = seg.Split('+');
                        if (ptxArray[1] == "006")
                        {
                            string newMij = double.Parse(ptxArray[2]).ToString("0000");
                            if (lijnTeller == 1)
                            {
                                if (newMij != maatschappij)
                                {
                                    if (MessageBox.Show("Maatschappijcode wijzigen\r\n\r\n" + newMij + " <> " + maatschappij, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    { maatschappij = newMij; mijWijzigen = true; }
                                    else
                                        mijWijzigen = false;
                                }
                            }
                            else if (mijWijzigen && newMij != maatschappij)
                                maatschappij = newMij;
                        }
                        break;

                        // IPD, FTX, QTY — voorlopig over te slaan
                }
            }

            // Afsluiting
            if (gridText != "")
            {
                string periodeVoor = boekjaarKontrole + maandVerwerking;
                // NOTE: BYPERDAT / KwijtingBoeken form interactions are kept as
                // comments — wire to the actual form instances in the calling context.
                // BYPERDAT.Boekjaar.ListIndex = 0;
                // ... period loop and KwijtingBoeken.Show(1) ...
                GridText = gridText;
                if (typekwijtingFlag == 2)
                    GridTextIs = "002\t" + maatschappij + "\tContante";
                else if (typekwijtingFlag == 3)
                    GridTextIs = "003\t" + maatschappij + "\tTeruggave";
                else
                    GridTextIs = "001\t" + maatschappij + "\tTermijn";
                BL_LOGGING = false;
            }
        }

        // ── EdiFactCONTANT ──────────────────────────────────────────────────────

        public static void EdiFactCONTANT(string userArea)
        {
            string[] xArray = userArea.Split('\'');

            string polisNummer = "";
            string bijvoegselNummer = "";
            string verzekeraar = "";
            string polisType = "";
            string hoofdVervaldag = "";
            string inningswijzeTermijn = "";
            string splitsingsCode = "";
            string statusPolis = "";
            string tempoNaamKlant = "";
            string infolijn = "";
            string boekDatum = "";
            string inningswijzeContant = "";
            string typeKwijting = "";
            decimal totaalNettoPremie = 0;
            decimal totaalTeBetalen = 0;
            decimal totaalCommissie = 0;
            decimal totaalLasten = 0;
            decimal totaalKosten = 0;
            string verzekerdVanDatum = "";
            string verzekerdTotDatum = "";
            string dummy = "";
            string boekjaarKontrole = "";
            string maandVerwerking = "";
            string maskerHier = "######0.00";

            string[] attArray, dtmArray, ipdArray, nmeArray, moaArray, ptyArray, rffArray;

            for (int telhier = 0; telhier <= xArray.Length - 2; telhier++)
            {
                string seg = xArray[telhier];
                if (seg.Length < 3) continue;
                string tag = seg.Substring(0, 3);

                switch (tag)
                {
                    case "ATT":
                        attArray = seg.Split('+');
                        switch (attArray[1])
                        {
                            case "A600": inningswijzeTermijn = attArray[2]; break;
                            case "A325": splitsingsCode = attArray[2]; break;
                            case "A003": statusPolis = attArray[2]; break;
                            case "A602": inningswijzeContant = attArray[2]; break;
                            case "B001": typeKwijting = attArray[2]; break;
                        }
                        break;

                    case "DTM":
                        dtmArray = seg.Split(':');
                        if (seg.StartsWith("DTM+004")) hoofdVervaldag = dtmArray[1];
                        else if (seg.StartsWith("DTM+005")) boekDatum = dtmArray[1];
                        else if (seg.StartsWith("DTM+041")) verzekerdVanDatum = dtmArray[1];
                        else if (seg.StartsWith("DTM+022")) verzekerdTotDatum = dtmArray[1];
                        break;

                    case "IPD":
                        ipdArray = seg.Split('+');
                        if (ipdArray[1] == "A502") polisType = ipdArray[2];
                        break;

                    case "NME":
                        nmeArray = seg.Split('+');
                        if (nmeArray[1] == "001")
                            tempoNaamKlant = nmeArray[2].Replace(":", " ");
                        break;

                    case "MOA":
                        moaArray = seg.Split(':');
                        switch (moaArray[0])
                        {
                            case "MOA+012": totaalTeBetalen = decimal.Parse(moaArray[1]) / 100; break;
                            case "MOA+013": totaalNettoPremie = decimal.Parse(moaArray[1]) / 100; break;
                            case "MOA+015": totaalCommissie = decimal.Parse(moaArray[1]) / 100; break;
                            case "MOA+016": totaalLasten = decimal.Parse(moaArray[1]) / 100; break;
                            case "MOA+017": totaalKosten = decimal.Parse(moaArray[1]) / 100; break;
                        }
                        break;

                    case "PTY":
                        ptyArray = seg.Split('+');
                        if (ptyArray[1] == "006") verzekeraar = ptyArray[2];
                        break;

                    case "RFF":
                        rffArray = seg.Split(':');
                        if (rffArray[0] == "RFF+001")
                        {
                            if (rffArray.Length > 2 && rffArray[2] == "001")
                                polisNummer = rffArray[1];
                            else
                                polisNummer = rffArray[1];
                        }
                        else if (rffArray[0] == "RFF+002")
                            bijvoegselNummer = rffArray[1];
                        break;

                        // XEH, Else — voorlopig over te slaan
                }
            }

            BGet(TABLE_CONTRACTS, 0, polisNummer);
            if (Ktrl != 0)
            {
                MessageBox.Show("polis " + polisNummer.Trim() + " niet aanwezig. EDIFACT nieuw nog in te brengen !!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TLB_RECORD[TABLE_CONTRACTS] = "";
                VBib(TABLE_CONTRACTS, Mid1(verzekerdVanDatum, 3, 2), "v164");
                VBib(TABLE_CONTRACTS, "NONAME", "A110");
                if (verzekeraar == "2652") { MessageBox.Show("2652: Nateus wordt nog weggeschreven onder nummer 0196"); verzekeraar = "0196"; }
                VBib(TABLE_CONTRACTS, verzekeraar, "A010");
                VBib(TABLE_CONTRACTS, polisNummer, "A000");
                VBib(TABLE_CONTRACTS, tempoNaamKlant, "vs99");
                BInsert(TABLE_CONTRACTS, 0);
                if (Ktrl != 0) MessageBox.Show("onbekende soldaat ! STOP!!!");
                dummy = "Kontroleer !!! " + tempoNaamKlant;
            }
            else
            {
                RecordToVeld(TABLE_CONTRACTS);
                BGet(TABLE_CUSTOMERS, 0, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12));
                if (Ktrl != 0)
                    dummy = "Verbeter !!! " + tempoNaamKlant;
                else
                {
                    RecordToVeld(TABLE_CUSTOMERS);
                    dummy = VBibText(TABLE_CUSTOMERS, "#A100 #");
                }

                if (typeKwijting == "2")
                {
                    BGet(TABLE_VARIOUS, 1, "25" + verzekeraar + polisNummer);
                    if (Ktrl != 0) TLB_RECORD[TABLE_VARIOUS] = "";
                    else RecordToVeld(TABLE_VARIOUS);

                    VBib(TABLE_VARIOUS, totaalTeBetalen.ToString(), "B010");
                    VBib(TABLE_VARIOUS, (totaalLasten + totaalKosten).ToString(), "B011");
                    VBib(TABLE_VARIOUS, totaalCommissie.ToString(), "B014");
                    VBib(TABLE_VARIOUS, VSet("K" + VBibText(TABLE_CONTRACTS, "#A110 #"), 13), "v004");
                    VBib(TABLE_VARIOUS, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12), "A110");
                    VBib(TABLE_VARIOUS, verzekeraar, "A010");
                    VBib(TABLE_VARIOUS, polisNummer, "A000");
                    VBib(TABLE_VARIOUS, VSet("25" + verzekeraar + polisNummer, 20), "v005");

                    if (Ktrl != 0) BInsert(TABLE_VARIOUS, 1); else BUpdate(TABLE_VARIOUS, 1);
                    if (Ktrl != 0) MessageBox.Show("Stopkode " + Ktrl);
                }
            }

            string newBoekdatum = Mid1(boekDatum, 1, 2) + "/" + Mid1(boekDatum, 3, 2) + "/" + Mid1(boekDatum, 5, 4);

            GridText = polisNummer + "\t" + newBoekdatum + "\t";
            GridText += Dec((double)totaalTeBetalen, maskerHier) + "\t";
            GridText += Dec((double)totaalCommissie, maskerHier) + "\t";
            GridText += dummy + "\t" + SnippetXEH(userArea, polisNummer.Trim(), false) + "\t" + totaalNettoPremie + "\t" + totaalLasten + "\r";

            boekjaarKontrole = Mid1(boekDatum, 5, 4);
            maandVerwerking = Mid1(boekDatum, 3, 2);

            if (typeKwijting == "2")
                GridTextIs = "002\t" + verzekeraar + "\tContant\t" + newBoekdatum;
            else if (typeKwijting == "3")
                GridTextIs = "003\t" + verzekeraar + "\tTeruggave\t" + newBoekdatum;
            else
                MessageBox.Show("controle type contante wat anders? " + typeKwijting);

            // NOTE: BYPERDAT / KwijtingBoeken period selection — wire in calling form.
        }

        // ── EdiFactREKENINGUITTREKSEL ───────────────────────────────────────────

        public static void EdiFactREKENINGUITTREKSEL(string userArea)
        {
            string[] xArray = userArea.Split('\'');
            string[] xghArray = xArray[0].Split('+');
            string maatschappij = double.Parse(xghArray[2]).ToString("0000");

            string maskerHier = bhEuro ? "######0.00" : "#########0";
            string boekLijn = "";
            string polisNummer = "";
            string tempoNaamKlant = "";
            string datumKwijting = "";
            string maandKwijting = "";
            string maandVerwerking = "";
            string boekjaarKontrole = "";
            decimal bedrag9 = 0;
            string[] moaArray = new string[0];
            string[] ptyArray;

            BL_LOGGING = true;
            GridText = "";

            for (int telhier = 0; telhier <= xArray.Length - 2; telhier++)
            {
                string seg = xArray[telhier];
                if (seg.Length < 3) continue;
                string tag = seg.Substring(0, 3);

                switch (tag)
                {
                    case "LIN":
                        if (boekLijn != "")
                            RuWegschrijven(ref boekLijn, ref bedrag9, moaArray, maatschappij, polisNummer, tempoNaamKlant, datumKwijting, maskerHier, ref GridText);

                        string linCode = Mid1(seg, 5, 3);
                        boekLijn = linCode + "\t";
                        if (linCode == "006")
                            boekLijn += datumKwijting + "\t";
                        else
                            SnelHelpPrint("Nog geen controle voor " + linCode, BL_LOGGING);
                        break;

                    case "DTM":
                        if (seg.StartsWith("DTM+069:"))
                        {
                            string dtmVal = seg.Substring(8);
                            maandKwijting = Mid1(dtmVal, 3, 2);
                            maandVerwerking = maandKwijting;
                            boekjaarKontrole = Mid1(dtmVal, 5, 4);
                            datumKwijting = Mid1(dtmVal, 1, 2) + "/" + Mid1(dtmVal, 3, 2) + "/" + Mid1(dtmVal, 5, 4);
                        }
                        break;

                    case "RFF":
                        if (seg.StartsWith("RFF+001:"))
                            polisNummer = seg.Substring(8);
                        break;

                    case "MOA":
                        moaArray = seg.Split(':');
                        string moaCode = moaArray[0].Length >= 8 ? moaArray[0].Substring(4, 3) : "";
                        if (moaCode == "088")
                            bedrag9 = ParseMOA(moaArray);
                        else if (moaCode != "083" && moaCode != "084" && moaCode != "087" && moaCode != "012")
                            SnelHelpPrint("MOA-stop voor " + moaArray[0], BL_LOGGING);
                        break;

                    case "NME":
                        tempoNaamKlant = seg.Substring(8);
                        break;

                    case "PTY":
                        ptyArray = seg.Split('+');
                        if (ptyArray[1] == "006")
                        {
                            SnelHelpPrint("maatschappij info" + seg, BL_LOGGING);
                            maatschappij = double.Parse(ptyArray[2]).ToString("0000");
                        }
                        break;

                    case "ATT":
                    case "XRH":
                    case "XRT":
                    case "XEH":
                    case "XET":
                    case "XGH":
                    case "XGT":
                    case "GIS":
                        // voorlopig over te slaan
                        break;

                    default:
                        SnelHelpPrint("nog niets voorzien voor " + tag, BL_LOGGING);
                        break;
                }
            }

            if (boekLijn.Length > 5)
                RuWegschrijven(ref boekLijn, ref bedrag9, moaArray, maatschappij, polisNummer, tempoNaamKlant, datumKwijting, maskerHier, ref GridText);

            if (GridText == "")
                SnelHelpPrint("Er zijn geen commissielonen te boeken", BL_LOGGING);
            else
            {
                // NOTE: BYPERDAT / KwijtingBoeken period selection — wire in calling form.
                string periodeVoor = boekjaarKontrole + maandVerwerking;
                GridTextIs = "006\t" + maatschappij + "\tCommissies";
                BL_LOGGING = false;
            }
        }

        // ── Borderel ────────────────────────────────────────────────────────────

        public static void Borderel(string maatschappij, string userArea)
        {
            string iolijn = userArea;
            string maskerHier = bhEuro ? "######0.00" : "#########0";

            decimal bedrag1 = 0, bedrag2 = 0, bedrag3 = 0, bedrag4 = 0;
            decimal bedrag5 = 0, bedrag7 = 0, bedrag8 = 0, bedrag9 = 0;
            string gridText = "";

            int maandVerwerkingTermijn = 0;
            string boekjaarKontroleTermijn = "";

            System.Diagnostics.Debug.Assert(false, "Borderel breakpoint");

            while (iolijn != new string(' ', iolijn.Length))
            {
                int maandVerwerking = int.Parse(Mid1(iolijn, 19, 2));
                string boekjaarKontrole = Mid1(iolijn, 21, 2);
                string polisNummer = Mid1(iolijn, 7, 12).TrimStart();
                int operatie = int.Parse(Mid1(iolijn, 4, 1));

                switch (int.Parse(Mid1(iolijn, 3, 1)))
                {
                    case 1: // Kwijting inning producent
                        if (operatie == 1) // Termijnhernieuwing
                        {
                            maandVerwerkingTermijn = maandVerwerking;
                            boekjaarKontroleTermijn = boekjaarKontrole;
                            char mid42 = Mid1(iolijn, 42, 1)[0];
                            bool isDigit = mid42 >= '0' && mid42 <= '9';
                            if (isDigit)
                            {
                                if (bhEuro)
                                {
                                    bedrag1 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 42, 6))) / (decimal)EURO, 2);
                                    bedrag2 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 48, 5))) / (decimal)EURO, 2);
                                    bedrag3 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 53, 4))) / (decimal)EURO, 2);
                                    bedrag4 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 57, 8))) / (decimal)EURO, 2);
                                    bedrag5 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 65, 7))) / (decimal)EURO, 2);
                                    bedrag9 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 72, 7))) / (decimal)EURO, 2);
                                }
                                else
                                {
                                    bedrag1 = decimal.Parse(Kt(Mid1(iolijn, 42, 6)));
                                    bedrag2 = decimal.Parse(Kt(Mid1(iolijn, 48, 5)));
                                    bedrag3 = decimal.Parse(Kt(Mid1(iolijn, 53, 4)));
                                    bedrag4 = decimal.Parse(Kt(Mid1(iolijn, 57, 8)));
                                    bedrag5 = decimal.Parse(Kt(Mid1(iolijn, 65, 7)));
                                    bedrag9 = decimal.Parse(Kt(Mid1(iolijn, 72, 7)));
                                }
                                bedrag7 = bedrag1 + bedrag4;
                                bedrag8 = bedrag2 + bedrag3 + bedrag5;
                            }
                            else
                            {
                                if (bhEuro)
                                {
                                    bedrag7 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 57, 8))) / (decimal)EURO, 2);
                                    bedrag8 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 65, 7))) / (decimal)EURO, 2);
                                    bedrag9 = Math.Round(decimal.Parse(Kt(Mid1(iolijn, 72, 7))) / (decimal)EURO, 2);
                                }
                                else
                                {
                                    bedrag7 = decimal.Parse(Kt(Mid1(iolijn, 57, 8)));
                                    bedrag8 = decimal.Parse(Kt(Mid1(iolijn, 65, 7)));
                                    bedrag9 = decimal.Parse(Kt(Mid1(iolijn, 72, 7)));
                                }
                            }
                            // Wegschrijven
                            BorderelWegschrijven(maatschappij, polisNummer, iolijn, boekjaarKontroleTermijn,
                                maandVerwerkingTermijn, bedrag1, bedrag2, bedrag3, bedrag7, bedrag8, bedrag9,
                                maskerHier, ref gridText);
                        }
                        else
                            MessageBox.Show("Kwijting inning producent, andere dan termijnhernieuwing: " + operatie);
                        break;

                    case 2: // Rekeninguittreksel
                        string datum2000;
                        switch (operatie)
                        {
                            case 0:
                                // Vorig saldo — informatie only
                                break;
                            case 1:
                                string msg1 = "Termijnborderel in rekeninguittreksel voor maatschappij " + maatschappij + "\r\n";
                                msg1 += "Boekmaand : " + maandVerwerking.ToString("00") + " Boekjaar : " + boekjaarKontrole + "\r\n\r\n";
                                msg1 += Kt(Mid1(iolijn, 57, 8)) + "\r\n" + Kt(Mid1(iolijn, 72, 7));
                                MessageBox.Show(msg1, "Rekeninguittreksel");
                                break;
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                string infoTekst2 = operatie == 2 ? "Kontante kwijting"
                                    : operatie == 3 ? "Terugbetaling van premie"
                                    : operatie == 4 ? "Vernietiging van verrichting"
                                    : "Terugzending van kwitantie";
                                datum2000 = "28/" + maandVerwerking.ToString("00") + "/" + boekjaarKontrole;
                                string msg2 = "Beweging: " + infoTekst2 + " " + maatschappij + "\r\n";
                                msg2 += "Boekmaand : " + maandVerwerking.ToString("00") + " Boekjaar : " + boekjaarKontrole + "\r\n\r\n";
                                msg2 += Kt(Mid1(iolijn, 57, 8)) + "\r\n" + Kt(Mid1(iolijn, 72, 7)) + "\r\n\r\n";
                                msg2 += "Polisnummer : " + polisNummer + "\r\n" + Mid1(iolijn, 30, 27);
                                MessageBox.Show(msg2, "Rekeninguittreksel");
                                GridTextPolis = (GridTextPolis?.ToString() ?? "") +
                                    maatschappij + "\t" + polisNummer + "\t" + datum2000 + "\t" +
                                    Kt(Mid1(iolijn, 57, 8)) + "\t" + Kt(Mid1(iolijn, 72, 7)) + "\t" +
                                    Mid1(iolijn, 30, 27) + "\t" + operatie + "\r\n";
                                break;
                            case 6:
                                datum2000 = "28/" + maandVerwerking.ToString("00") + "/" + boekjaarKontrole;
                                string msg6 = "Commissielonen, maatschappij: " + maatschappij + "\r\n";
                                msg6 += "Boekmaand : " + maandVerwerking.ToString("00") + " Boekjaar : " + boekjaarKontrole + "\r\n\r\n";
                                msg6 += Kt(Mid1(iolijn, 57, 8)) + "\r\n" + Kt(Mid1(iolijn, 72, 7)) + "\r\n\r\n";
                                msg6 += "Polisnummer : " + polisNummer + "\r\n" + Mid1(iolijn, 30, 27);
                                MessageBox.Show(msg6, "Rekeninguittreksel");
                                GridTextPolis = (GridTextPolis?.ToString() ?? "") +
                                    maatschappij + "\t" + polisNummer + "\t" + datum2000 + "\t" +
                                    Kt(Mid1(iolijn, 57, 8)) + "\t" + Kt(Mid1(iolijn, 72, 7)) + "\t" +
                                    Mid1(iolijn, 30, 27) + "\t" + operatie + "\r\n";
                                break;
                            case 7:
                                MessageBox.Show("Stop", "Schaderegelingen", MessageBoxButtons.OK);
                                break;
                        }
                        break;

                    case 3: // Kwijting Inning Maatschappij
                        if (!string.IsNullOrWhiteSpace(polisNummer))
                            MessageBox.Show("KWIJTING INNING MAATSCHAPPIJ");
                        break;

                    default:
                        MessageBox.Show("onbekende kode");
                        break;
                }

                iolijn = iolijn.Substring(Math.Min(80, iolijn.Length));
            }

            if (gridText != "")
            {
                string periodeVoor;
                if (string.Compare(boekjaarKontroleTermijn, "86") < 0)
                    periodeVoor = "20" + boekjaarKontroleTermijn + maandVerwerkingTermijn.ToString("00");
                else
                    periodeVoor = "19" + boekjaarKontroleTermijn + maandVerwerkingTermijn.ToString("00");

                GridText = gridText;
                // NOTE: BYPERDAT / KwijtingBoeken period selection — wire in calling form.
            }
        }

        // ── UpdatePolisDatabase ─────────────────────────────────────────────────

        public static bool UpdatePolisDatabase(int nummer, string bericht)
        {
            if (!TLBPag3("AS1" + nummer.ToString("000")))
                return false;

            string polisNummer = Mid1(bericht, TELEBIB_POS[0], TELEBIB_LENGTH[0]).Trim();
            string maatschappij = Mid1(bericht, TELEBIB_POS[1], TELEBIB_LENGTH[1]).TrimEnd();

            BGet(TABLE_VARIOUS, 1, "25" + maatschappij + polisNummer);
            int tempoKtrl = Ktrl;
            if (Ktrl != 0) TLB_RECORD[TABLE_VARIOUS] = "";
            else RecordToVeld(TABLE_VARIOUS);

            int t = 0;
            while (TELEBIB_CODE[t] != new string(' ', 10))
            {
                string tekst = Mid1(bericht, TELEBIB_POS[t], TELEBIB_LENGTH[t]).Trim();
                if (tekst != "")
                {
                    string code = Mid1(TELEBIB_CODE[t], 5, 5);
                    if (tempoKtrl != 0)
                    {
                        VBib(TABLE_VARIOUS, tekst, code);
                    }
                    else
                    {
                        string existing = VBibText(TABLE_VARIOUS, "#" + code + "#").Trim();
                        if (existing != tekst)
                        {
                            string msgBox = existing + " vervangen door\r\n" + tekst + "\r\n\r\nBent U zeker ?";
                            if (MessageBox.Show(msgBox, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                VBib(TABLE_VARIOUS, tekst, code);
                        }
                    }
                }
                t++;
            }
            return true;
        }

        // ── Private helpers ──────────────────────────────────────────────────────

        // Fixed-position Mid (1-based, like VB6 Mid$)
        private static string Mid1(string s, int start, int length)
        {
            if (s == null || start < 1 || start > s.Length) return "";
            int from = start - 1;
            int avail = s.Length - from;
            return s.Substring(from, Math.Min(length, avail));
        }

        // SetMid: write str into char array at 1-based position
        private static void SetMid(char[] buf, int pos, string val)
        {
            int idx = pos - 1;
            for (int i = 0; i < val.Length && idx + i < buf.Length; i++)
                buf[idx + i] = val[i];
        }

        // ParseMOA: parse MOAarray (split on ':') honouring decimals field
        private static decimal ParseMOA(string[] moaArray)
        {
            if (moaArray.Length < 4) return 0;
            string currency = moaArray[2];
            string decimals = moaArray[3];
            decimal raw = decimal.TryParse(moaArray[1], out decimal v) ? v : 0;

            if (currency == "EUR")
            {
                if (decimals.StartsWith("2")) return raw / 100;
                if (decimals == "4") return raw / 10000;
                if (decimals == "0") { MessageBox.Show("stop"); return raw; }
                MessageBox.Show("Stop"); return 0;
            }
            if (currency == "BEF") return raw;
            MessageBox.Show("Stop"); return 0;
        }

        private static StreamWriter EnsureDumWriter(StreamWriter sw)
        {
            if (sw == null)
                sw = new StreamWriter(LOCATION_ASWEB + "AS1.$$$", append: true);
            return sw;
        }

        private static void RuWegschrijven(
            ref string boekLijn, ref decimal bedrag9, string[] moaArray,
            string maatschappij, string polisNummer, string tempoNaamKlant,
            string datumKwijting, string maskerHier, ref string gridText)
        {
            if (!boekLijn.StartsWith("006")) { boekLijn = ""; return; }

            string moaCurrency = moaArray.Length > 2 ? moaArray[2] : "EUR";
            if (bhEuro && moaCurrency == "EUR") { }
            else if (!bhEuro && moaCurrency == "BEF") { }
            else if (moaCurrency == "BEF")
                bedrag9 = Math.Round(bedrag9 / (decimal)EURO, 2);
            else if (moaCurrency == "EUR")
                bedrag9 = Math.Round(bedrag9 * (decimal)EURO);
            else
                MessageBox.Show("ONMOGELIJKE SITUATIE");

            BGet(TABLE_CONTRACTS, 0, polisNummer);
            string dummy;
            if (Ktrl != 0)
            {
                MessageBox.Show("polis niet aanwezig. EDIFACT nieuw nog in te brengen !!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TLB_RECORD[TABLE_CONTRACTS] = "";
                VBib(TABLE_CONTRACTS, "NONAME", "A110");
                VBib(TABLE_CONTRACTS, maatschappij, "A010");
                VBib(TABLE_CONTRACTS, polisNummer, "A000");
                BInsert(TABLE_CONTRACTS, 0);
                if (Ktrl != 0) MessageBox.Show("onbekende soldaat ! STOP!!!");
                dummy = "Kontroleer !!! " + tempoNaamKlant;
            }
            else
            {
                RecordToVeld(TABLE_CONTRACTS);
                bool polisTeWijzigen = false;
                string dagKtrl2 = Mid1(VBibText(TABLE_CONTRACTS, "#AW_2 #"), 7, 2);
                if (maatschappij != VBibText(TABLE_CONTRACTS, "#A010 #"))
                {
                    VBib(TABLE_CONTRACTS, maatschappij, "A010");
                    polisTeWijzigen = true;
                }
                if (polisTeWijzigen) BUpdate(TABLE_CONTRACTS, 0);

                BGet(TABLE_CUSTOMERS, 0, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12));
                if (Ktrl != 0)
                    dummy = "Verbeter !!! " + tempoNaamKlant;
                else
                {
                    RecordToVeld(TABLE_CUSTOMERS);
                    dummy = VBibText(TABLE_CUSTOMERS, "#A100 #");
                }
            }

            // TeleBibKTRL
            BGet(TABLE_VARIOUS, 1, "25" + maatschappij + polisNummer);
            if (Ktrl != 0) TLB_RECORD[TABLE_VARIOUS] = "";
            else RecordToVeld(TABLE_VARIOUS);

            VBib(TABLE_VARIOUS, bedrag9.ToString(), "B014");
            VBib(TABLE_VARIOUS, VSet("K" + VBibText(TABLE_CONTRACTS, "#A110 #"), 13), "v004");
            VBib(TABLE_VARIOUS, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12), "A110");
            VBib(TABLE_VARIOUS, maatschappij, "A010");
            VBib(TABLE_VARIOUS, polisNummer, "A000");
            VBib(TABLE_VARIOUS, VSet("25" + maatschappij + polisNummer, 20), "v005");

            if (Ktrl != 0) BInsert(TABLE_VARIOUS, 1); else BUpdate(TABLE_VARIOUS, 1);
            if (Ktrl != 0) MessageBox.Show("Stopkode " + Ktrl);

            gridText += polisNummer + "\t" + datumKwijting + "\t";
            gridText += Dec(0d, maskerHier) + "\t";
            gridText += Dec((double)bedrag9, maskerHier) + "\t";
            gridText += dummy + "\t\r\n";

            bedrag9 = 0;
            boekLijn = "";
        }

        private static void BorderelWegschrijven(
            string maatschappij, string polisNummer, string iolijn,
            string boekjaarKontroleTermijn, int maandVerwerkingTermijn,
            decimal bedrag1, decimal bedrag2, decimal bedrag3,
            decimal bedrag7, decimal bedrag8, decimal bedrag9,
            string maskerHier, ref string gridText)
        {
            BGet(TABLE_CONTRACTS, 0, polisNummer);
            string dummy;
            string ddag;

            if (Ktrl != 0)
            {
                TLB_RECORD[TABLE_CONTRACTS] = "";
                MessageBox.Show("Stop.  Polis bestaat nog niet :" + polisNummer);
                VBib(TABLE_CONTRACTS, Mid1(iolijn, 23, 2), "v164");
                VBib(TABLE_CONTRACTS, "NONAME", "A110");
                VBib(TABLE_CONTRACTS, maatschappij, "A010");
                VBib(TABLE_CONTRACTS, polisNummer, "A000");
                VBib(TABLE_CONTRACTS, Mid1(iolijn, 30, 10), "vs99");
                BInsert(TABLE_CONTRACTS, 0);
                if (Ktrl != 0) MessageBox.Show("onbekende soldaat ! STOP!!!");
                dummy = "Kontroleer !!! " + Mid1(iolijn, 30, 10);
                ddag = "01";
            }
            else
            {
                RecordToVeld(TABLE_CONTRACTS);
                ddag = VBibText(TABLE_CONTRACTS, "#v165 #");
                if (maatschappij != VBibText(TABLE_CONTRACTS, "#A010 #"))
                {
                    VBib(TABLE_CONTRACTS, maatschappij, "A010");
                    BUpdate(TABLE_CONTRACTS, 0);
                }
                BGet(TABLE_CUSTOMERS, 0, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12));
                if (Ktrl != 0)
                    dummy = "Verbeter !!! " + Mid1(iolijn, 30, 10);
                else
                {
                    RecordToVeld(TABLE_CUSTOMERS);
                    dummy = VBibText(TABLE_CUSTOMERS, "#A100 #");
                }
            }

            // TeleBibKTRL
            BGet(TABLE_VARIOUS, 1, "25" + maatschappij + polisNummer);
            if (Ktrl != 0) TLB_RECORD[TABLE_VARIOUS] = "";
            else RecordToVeld(TABLE_VARIOUS);

            string mid42 = Mid1(iolijn, 42, 1);
            if (mid42[0] >= '0' && mid42[0] <= '9' && !(maatschappij == "0145" && Mid1(iolijn, 42, 6) == "000000"))
            {
                string vorigeBM = VBibText(TABLE_VARIOUS, "#5315 #");
                VBib(TABLE_VARIOUS, "P11", "AW00");
                VBib(TABLE_VARIOUS, "4", "AW06");
                VBib(TABLE_VARIOUS, (bedrag1 + bedrag2 + bedrag3).ToString(), "AW04");
                VBib(TABLE_VARIOUS, vorigeBM, "5310");
                VBib(TABLE_VARIOUS, Mid1(iolijn, 40, 2), "5315");
            }

            VBib(TABLE_VARIOUS, Mid1(iolijn, 25, 5), "AW.R");
            VBib(TABLE_VARIOUS, (bedrag7 + bedrag8).ToString(), "B010");
            VBib(TABLE_VARIOUS, bedrag8.ToString(), "B011");
            VBib(TABLE_VARIOUS, bedrag7.ToString(), "B013");
            VBib(TABLE_VARIOUS, bedrag9.ToString(), "B014");
            VBib(TABLE_VARIOUS, VSet("K" + VBibText(TABLE_CONTRACTS, "#A110 #"), 13), "v004");
            VBib(TABLE_VARIOUS, VSet(VBibText(TABLE_CONTRACTS, "#A110 #"), 12), "A110");
            VBib(TABLE_VARIOUS, maatschappij, "A010");
            VBib(TABLE_VARIOUS, polisNummer, "A000");
            VBib(TABLE_VARIOUS, VSet("25" + maatschappij + polisNummer, 20), "v005");

            if (Ktrl != 0) BInsert(TABLE_VARIOUS, 1); else BUpdate(TABLE_VARIOUS, 1);
            if (Ktrl != 0) MessageBox.Show("Stopkode " + Ktrl);

            string datumStr = boekjaarKontroleTermijn.CompareTo("86") < 0
                ? ddag + "/" + Mid1(iolijn, 23, 2) + "/20" + boekjaarKontroleTermijn
                : ddag + "/" + Mid1(iolijn, 23, 2) + "/19" + boekjaarKontroleTermijn;

            gridText += polisNummer + "\t" + datumStr + "\t";
            gridText += Dec((double)(bedrag7 + bedrag8), maskerHier) + "\t";
            gridText += Dec((double)bedrag9, maskerHier) + "\t";
            gridText += dummy + "\r";
        }
    }
}

