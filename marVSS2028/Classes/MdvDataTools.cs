using ADODB;
using System;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.TextTools;
using static marVSS2028.Classes.OleDbTools;

namespace marVSS2028.Classes
{
    internal static class MdvDataTools // This class contains methods for database operations, including opening/closing recordsets, seeking records, updating records, and handling tagged field values in a custom format. It also includes methods for journal entry validation and dynamic column addition.
    {
        /// <summary>
        /// Diagnostic: dumps the column names and object type of MinimumIndeling
        /// (or any table/query name) to a MessageBox so you can verify whether
        /// the field names used in DbKontrole match the actual schema.
        /// Call this once from ButtonEdit_Click (or a test button) to inspect the DB.
        /// </summary>
        public static void DiagnoseMinimumIndeling()
        {
            try
            {
                using (var con = new OleDbConnection(adKBDB.ConnectionString))
                {
                    con.Open();

                    var sb = new StringBuilder();

                    // 1. Is "MinimumIndeling" a table or a saved query?
                    var tables = con.GetOleDbSchemaTable(
                        System.Data.OleDb.OleDbSchemaGuid.Tables,
                        new object[] { null, null, "MinimumIndeling", null });
                    sb.AppendLine("=== Object type ===");
                    if (tables != null && tables.Rows.Count > 0)
                        sb.AppendLine("TABLE_TYPE: " + tables.Rows[0]["TABLE_TYPE"]);
                    else
                        sb.AppendLine("(not found in Tables schema — may be a saved query)");

                    // 2. Actual column names in MinimumIndeling
                    var cols = con.GetOleDbSchemaTable(
                        System.Data.OleDb.OleDbSchemaGuid.Columns,
                        new object[] { null, null, "MinimumIndeling", null });
                    sb.AppendLine();
                    sb.AppendLine("=== Columns in MinimumIndeling ===");
                    if (cols != null && cols.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in cols.Rows)
                            sb.AppendLine($"  {row["COLUMN_NAME"]}  ({row["DATA_TYPE"]})");
                    }
                    else
                    {
                        sb.AppendLine("(no columns found — object may not exist)");
                    }

                    // 3. Try a parameterless SELECT * to catch any unresolved query parameters
                    sb.AppendLine();
                    sb.AppendLine("=== SELECT * FROM MinimumIndeling (TOP 1) ===");
                    try
                    {
                        using (var cmd = new OleDbCommand(
                            "SELECT TOP 1 * FROM MinimumIndeling", con))
                        using (var dr = cmd.ExecuteReader())
                        {
                            sb.Append("Columns returned: ");
                            for (int i = 0; i < dr.FieldCount; i++)
                                sb.Append(dr.GetName(i) + (i < dr.FieldCount - 1 ? ", " : ""));
                            sb.AppendLine();
                            sb.AppendLine("(SELECT * succeeded — no unresolved query parameters)");
                        }
                    }
                    catch (Exception exSel)
                    {
                        sb.AppendLine("SELECT * FAILED: " + exSel.Message);
                        sb.AppendLine("=> MinimumIndeling is likely a saved Access query");
                        sb.AppendLine("   with unresolved parameters (e.g. Forms!... references).");
                    }

                    MessageBox.Show(sb.ToString(), "DbKontrole — MinimumIndeling diagnostics",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DiagnoseMinimumIndeling fout:\r\n" + ex.Message);
            }
        }

        public static void DbKontrole(string stringTeZoeken, int flNr)
        {
            switch (flNr)
            {
                case int fl when fl == TABLE_LEDGERACCOUNTS:
                    {
                        var sb = new StringBuilder();

                        using (var rsKBRecord = new OleDbCommand())
                        using (var con = new OleDbConnection(adKBDB.ConnectionString))
                        {
                            con.Open();

                            // Seek BeforeEQ: find records before or equal to search value
                            string sqlBefore =
                                "SELECT TOP 11 RekeningNummer, Omschrijving " +
                                "FROM MinimumIndeling " +
                                "WHERE RekeningNummer <= ? " +
                                "ORDER BY RekeningNummer DESC";

                            using (var cmd = new OleDbCommand(sqlBefore, con))
                            {
                                cmd.Parameters.AddWithValue("?", stringTeZoeken.Trim());
                                try
                                {
                                    using (var dr = cmd.ExecuteReader())
                                    {
                                        int teller = 0;
                                        var before = new StringBuilder();
                                        while (dr.Read() && teller < 11)
                                        {
                                            before.Insert(0,
                                                dr[0] + " " + dr[1] + System.Environment.NewLine);
                                            teller++;
                                        }
                                        sb.Append(before);
                                    }
                                }
                                catch (System.Data.OleDb.OleDbException)
                                {
                                    // Schema mismatch or unresolved query parameters detected —
                                    // run the diagnostic and abort this query.
                                    // DiagnoseMinimumIndeling();
                                    return;
                                }
                            }

                            // Seek AfterEQ: find records after the search value (excluding equal)
                            string sqlAfter =
                                "SELECT TOP 15 RekeningNummer, Omschrijving " +
                                "FROM MinimumIndeling " +
                                "WHERE RekeningNummer > ? " +
                                "ORDER BY RekeningNummer ASC";

                            using (var cmd = new OleDbCommand(sqlAfter, con))
                            {
                                cmd.Parameters.AddWithValue("?", stringTeZoeken.Trim());
                                try
                                {
                                    using (var dr = cmd.ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            sb.Append(dr[0] + " " + dr[1] + System.Environment.NewLine);
                                        }
                                    }
                                }
                                catch (System.Data.OleDb.OleDbException)
                                {
                                    DiagnoseMinimumIndeling();
                                    return;
                                }
                            }
                        }

                        Globals.Mim.InfoData.Visible = true;
                        Globals.Mim.InfoData.Text = sb.ToString();
                        break;
                    }

                default:
                    MessageBox.Show("stop");
                    break;
            }
        }
        /// <summary>
        /// VB6: Sub bClose(Fl As Integer)
        /// Closes one recordset (Fl) or all recordsets when Fl = 99.
        /// </summary>
        internal static void BClose(int fl)
        {
            if (fl == 99)
            {
                for (int i = 0; i <= Globals.NUMBER_TABLES; i++)
                {
                    Globals.TLB_RECORD[i] = "";
                    CloseTable(i);
                }
            }
            else
            {
                CloseTable(fl);
            }
        }

        private static void CloseTable(int fl)
        {
            try
            {
                if (Globals.rsMAR[fl] == null ||
                    Globals.rsMAR[fl].State == (int)ObjectStateEnum.adStateClosed)
                    return;

                Globals.rsMAR[fl].Close();
                Globals.Ktrl = 0;
            }
            catch (Exception ex)
            {
                Globals.Ktrl = ex.HResult;
                if (ex.HResult == unchecked((int)0x80040E5C)) // 3420 – object invalid or no longer set
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// VB6: Function bOpen(Fl As Integer) As Integer
        /// Opens rsMAR(Fl) against the active ADO connection (SQL Server or Jet).
        /// Returns 0 on success; sets Globals.Ktrl on error.
        /// </summary>
        internal static int BOpen(int fl)
        {
            if (Globals.rsMAR[fl] == null)
                Globals.rsMAR[fl] = new Recordset();

            if (Globals.rsMAR[fl].State != (int)ObjectStateEnum.adStateClosed)
                return 0;

            try
            {
                Globals.rsMAR[fl].CursorLocation = CursorLocationEnum.adUseServer;
                Globals.rsMAR[fl].Open(
                Globals.bstNaam[fl],
                Globals.adntDB,
                CursorTypeEnum.adOpenKeyset,
                LockTypeEnum.adLockOptimistic,
                (int)CommandTypeEnum.adCmdTableDirect);

                Globals.Ktrl = 0;
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Globals.Ktrl = ex.HResult;
                return ex.HResult;
            }
        }

        /// <summary>
        /// VB6: Sub bGet(Fl, fIndex, fSleutel)
        /// Seeks a record in rsMAR(Fl) by index and key; sets Globals.Ktrl (0 = found, 4 = not found, 99 = abort).
        /// </summary>
        internal static void BGet(int fl, int fIndex, string fSleutel)
        {
            int probeerTellertje = 0;

        bGetNogEens:
            try
            {
                if (Globals.rsMAR[fl] == null || Globals.rsMAR[fl].State == (int)ObjectStateEnum.adStateClosed)
                    Globals.Ktrl = BOpen(fl);


                fSleutel = VSet(fSleutel, Globals.FLINDEX_LEN[fl, fIndex]);

                try
                {
                    if (Globals.rsMAR[fl].Index != Globals.FLINDEX_CAPTION[fl, fIndex])
                        Globals.rsMAR[fl].Index = Globals.FLINDEX_CAPTION[fl, fIndex];
                }
                catch (Exception ex)
                {
                    MimEnvironment.SnelHelpPrint(ex.Message, Globals.BL_LOGGING);
                    BClose(fl);
                    probeerTellertje++;
                    if (probeerTellertje > 5)
                    {
                        Globals.Ktrl = 99;
                        return;
                    }
                    goto bGetNogEens;
                }

                try
                {
                    Globals.rsMAR[fl].Seek(fSleutel, SeekEnum.adSeekFirstEQ);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\r" + "Hierna wordt foutcode 4 doorgegeven",
                        "bGet routine", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Globals.Ktrl = 4;
                    return;
                }

                Globals.Ktrl = Globals.rsMAR[fl].EOF ? 4 : 0;
                Globals.KEY_BUF[fl] = VSet(fSleutel, Globals.FLINDEX_LEN[fl, fIndex]);
                Globals.KEY_INDEX[fl] = fIndex;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Globals.Ktrl = 4;
            }
        }

        /// <summary>
        /// VB6: Sub bUpdate(Fl, fIndex)
        /// Writes vBC fields back to rsMAR(Fl) and calls Update; retries on concurrency error 3197.
        /// </summary>
        internal static void BUpdate(int fl, int fIndex)
        {
            try
            {
                VeldToRecord(fl);
                if (Globals.Ktrl == 32000) return;

                Globals.KEY_BUF[fl] = Globals.FVT[fl, fIndex];
                Globals.KEY_INDEX[fl] = fIndex;

                if (fl != TABLE_COUNTERS)
                    Globals.rsMAR[fl].Fields["dnnsync"].Value = false;

            doUpdate:
                try
                {
                    Globals.rsMAR[fl].Update();
                }
                catch (Exception ex)
                {
                    int errNr = ex.HResult & 0xFFFF; // low word = DAO/ADO error number
                    if (errNr == 3197)
                    {
                        MessageBox.Show("Andere gebruiker heeft bewerking uitgevoerd !");
                        BGet(fl, fIndex, Globals.KEY_BUF[fl]);
                        if (Globals.Ktrl == 0)
                            RecordToVeld(fl);
                    }
                    else
                    {
                        Globals.Msg = "Database stopkode " + errNr + "\r\n\r\n"
                                    + "Mededeling:\r\n"
                                    + ex.Message + "\r\n\r\n"
                                    + "Steeds opnieuw proberen ?";
                        if (MessageBox.Show(Globals.Msg, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            goto doUpdate;
                        else
                            Globals.Ktrl = 99;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>        
        /// Writes TLB_RECORD values back into rsMAR(Fl) fields and updates composite index keys.
        /// Sets Globals.Ktrl on error; returns 0 on success.
        /// </summary>
        internal static void VeldToRecord(int fl)
        {
            if (Globals.rsMAR[fl] == null || Globals.rsMAR[fl].State == (int)ObjectStateEnum.adStateClosed)
                Globals.Ktrl = BOpen(fl);
                        
            // Build composite index fields before writing
            if (fl == Globals.TABLE_CONTRACTS)
            {
                // MaandKlantMaatschappijPolis = v164(2) + A110(12) + A010(4) + A000(12)
                VBib(fl,
                    VSet(VBibText(fl, "#v164 #"), 2) +
                    VSet(VBibText(fl, "#A110 #"), 12) +
                    VSet(VBibText(fl, "#A010 #"), 4) +
                    VSet(VBibText(fl, "#A000 #"), 12),
                    "v167");
            }
            else if (fl == Globals.TABLE_JOURNAL)
            {
                VBib(fl,
                    VSet(VBibText(fl, "#v019 #"), 7) + VBibText(fl, "#v066 #"),
                    "v070");
            }

            // Cache all index key values into FVT, padded to their defined lengths
            for (int t = 0; t <= Globals.FL_NUMBEROFINDEXEN[fl]; t++)
                Globals.FVT[fl, t] = VSet(
                    VBibText(fl, "#" + Globals.JETTABLEUSE_INDEX[fl, t] + "#"),
                    Globals.FLINDEX_LEN[fl, t]);

            // Write primary key field back
            VBib(fl, Globals.FVT[fl, 0], Globals.JETTABLEUSE_INDEX[fl, 0]);

            // Write all mapped fields back to the recordset
            int i = 0;
            while (!string.IsNullOrEmpty(Globals.vBC[fl, i]))
            {
                string fieldCode = Globals.vBC[fl, i];                
                SetFields(fl, fieldCode, VBibText(fl, "#" + fieldCode + " #"));
                i++;
            }

            // Special case: TABLE_VARIOUS — write A000 field directly
            try
            {
                if (fl == Globals.TABLE_VARIOUS)
                {
                    Globals.rsMAR[Globals.TABLE_VARIOUS].Fields["A000"].Value =
                        VBibText(Globals.TABLE_VARIOUS, "#A000 #");

                    rsMAR[fl].Fields["MEMO"].Value = TLB_RECORD[fl];
                }                    
            }
            catch { }
        }

        /// <summary>
        /// VB6: Sub RecordToVeld(Fl)
        /// Reads the current rsMAR(Fl) record into TLB_RECORD / vBC fields via vBib,
        /// then caches all index key values into FVT.
        /// </summary>
        internal static void RecordToVeld(int fl)
        {
            TLB_RECORD[fl] = "";
            try
            {
                if (fl == TABLE_VARIOUS)
                {
                    TLB_RECORD[fl] = rsMAR[fl].Fields["MEMO"].Value?.ToString() ?? "";
                }
                else
                {
                    int t = 0;
                    while (!string.IsNullOrEmpty(vBC[fl, t]))
                    {
                        string fieldName = vBC[fl, t];
                        string fieldVal = rsMAR[fl].Fields[fieldName].Value?.ToString() ?? "";
                        VBib(fl, fieldVal, fieldName);
                        t++;
                    }
                }
            }
            catch { }

            try
            {
                for (int t = 0; t <= FL_NUMBEROFINDEXEN[fl]; t++)
                    FVT[fl, t] = VBibText(fl, "#" + JETTABLEUSE_INDEX[fl, t] + "#");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// VB6: Sub vBib(Fl, StringTekst1, StringTekst2)
        /// Stores a tagged value (StringTekst2 = field code, StringTekst1 = value) in TLB_RECORD(Fl).
        /// The tag format is "#CCCCC#value#" where CCCCC is a 5-character field code.
        /// </summary>
        internal static void VBib(int fl, string value, string fieldCode)
        {
            string tbCode = "#" + fieldCode.PadRight(5) + "#";

            if (string.IsNullOrEmpty(value))
                value = " ";
            else if (value.IndexOf('#') >= 0)
            {
                MimEnvironment.SnelHelpPrint("U gebruikte het verboden '#' teken !!!", false);
                return;
            }

        jump:
            int pos = Globals.TLB_RECORD[fl].IndexOf(tbCode, StringComparison.Ordinal);
            if (pos < 0)
            {
                Globals.TLB_RECORD[fl] += tbCode + value + "#";
            }
            else
            {
                string valueToCheck = VBibText(fl, tbCode).TrimEnd();
                if (valueToCheck == value)
                    return;

                int tbLen = Globals.TLB_RECORD[fl].Length;
                int tbStart = pos;
                int tbStop = Globals.TLB_RECORD[fl].IndexOf('#', tbStart + 7);
                Globals.TLB_RECORD[fl] = Globals.TLB_RECORD[fl].Substring(0, tbStart)
                                        + Globals.TLB_RECORD[fl].Substring(tbStop + 1);
                goto jump;
            }
        }

        /// <summary>
        /// VB6: Function vBibTekst(Fl, TBS) As String
        /// Retrieves the value stored for a tag in TLB_RECORD(Fl).
        /// TBS may be a raw 7-char tag "#CCCCC#" or a 5-char field code.
        /// </summary>
        internal static string VBibText(int fl, string tbs)
        {            
            string tbsHier;
            if (tbs.Length > 0 && tbs[0] == '#')
                tbsHier = tbs.PadRight(7).Substring(0, 7);
            else
                tbsHier = ("#" + tbs).PadRight(7).Substring(0, 7);

            // Console.WriteLine("Table: " + fl + ", code: " + tbs + " becomes: " + tbsHier);

            try
            {
                string rec = Globals.TLB_RECORD[fl];
                if (string.IsNullOrEmpty(rec)) return "";

                int start = rec.IndexOf(tbsHier, StringComparison.Ordinal);
                if (start < 0) return "";

                int extractFrom = start + 7;
                int hashPos = rec.IndexOf('#', extractFrom);
                if (hashPos < 0) return "";

                return rec.Substring(extractFrom, hashPos - extractFrom);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// VB6: Function ADOBIB_TEXT(adoField As ADODB.Field, TBS As String) As String
        /// Extracts a sub-string from an ADO field value using a tag-based protocol.
        /// Returns "" when the field is empty or the tag is not found.
        /// </summary>
        public static string ADOBIB_TEXT(Field adoField, string tbs)
        {
            try
            {
                if (adoField == null) return "";
                string fieldValue = adoField.Value?.ToString() ?? "";
                if (fieldValue == "") return "";

                int startPos = fieldValue.IndexOf(tbs, StringComparison.Ordinal);
                if (startPos < 0) return "";

                int extractFrom = startPos + 7;
                int hashPos = fieldValue.IndexOf('#', extractFrom);
                if (hashPos < 0) return "";

                return fieldValue.Substring(extractFrom, hashPos - extractFrom);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// VB6: Function ADO_GET(iTabel, iIndex, sZoals, sZoek) As Boolean
        /// Seeks a record in rsMAR(iTabel) using the specified index and seek operator.
        /// </summary>
        public static bool ADO_GET(int iTabel, int iIndex, string sZoals, object sZoek)
        {
            try
            {
                if (Globals.rsMAR[iTabel].State == (int)ObjectStateEnum.adStateClosed)
                    Globals.Ktrl = BOpen(iTabel);

                Globals.rsMAR[iTabel].Index = Globals.FLINDEX_CAPTION[iTabel, iIndex];

                if (sZoals == "=")
                    Globals.rsMAR[iTabel].Seek(sZoek, SeekEnum.adSeekFirstEQ);
                else if (sZoals == ">=")
                    Globals.rsMAR[iTabel].Seek(sZoek, SeekEnum.adSeekAfterEQ);
                else
                    MessageBox.Show(sZoals + " nog niet beschikbaar");

                return !Globals.rsMAR[iTabel].EOF;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// VB6: Function RV(adoRecord As ADODB.Recordset, TBS As String) As Variant
        /// Returns the field value from a recordset, or "" when null/empty.
        /// </summary>
        public static object RV(Recordset adoRecord, string tbs)
        {
            try
            {
                object val = adoRecord.Fields[tbs].Value;
                if (val == null || val == DBNull.Value || val is DBNull)
                    return "";
                string s = val.ToString();
                return s == "" ? (object)"" : val;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>        
        /// Validates and posts a journal entry: updates cumulative totals, ledger account
        /// balances, and commits the rsJournaal record.
        /// </summary>
        public static bool ADOJOURNAL_OK(SharedForms.FormBoeking boekingForm = null)
        {
            double v068 = double.TryParse(RV(rsJournaal, "v068")?.ToString(), out double parsedV068)
                ? parsedV068
                : 0.0;

            if (v068 == 0)
            {
                MimEnvironment.SnelHelpPrint("BoekBedrag is 0", BL_LOGGING);
                return false;
            }

            DKTRL_CUMUL += v068;
            DKTRL_BEF += Math.Round(v068 * EURO, 0);
            DKTRL_EUR += Math.Round(v068, 2);

            try { rsJournaal.Fields["dece068"].Value = v068; } catch { }

            // Build grid line (tab-separated)
            string pipo = rsJournaal.Fields["v019"].Value?.ToString() + "\t"
                        + rsJournaal.Fields["v067"].Value?.ToString() + "\t";

            if (v068 < 0)
            {
                pipo += "\t" + (-v068).ToString("#,##0.00");
                pipo += "\t" + "\t" + Math.Round(-v068 * EURO).ToString("#,##0.00");
            }
            else
            {
                pipo += v068.ToString("#,##0.00") + "\t" + "";
                pipo += "\t" + Math.Round(v068 * EURO).ToString("#,##0.00") + "\t" + "";
            }

            // Port of VB6: frmBoeking.mshfBoekLijst.AddItem pipo
            boekingForm?.AddItem(pipo);

            // Compose sort/search key for journal
            string v019 = Globals.rsJournaal.Fields["v019"].Value?.ToString() ?? "";
            string v066 = Globals.rsJournaal.Fields["v066"].Value?.ToString() ?? "";
            Globals.rsJournaal.Fields["v070"].Value = VSet(v019, 7) + v066;

            // Update ledger account balance
            BGet(TABLE_LEDGERACCOUNTS, 0, rsJournaal.Fields["v019"].Value?.ToString() ?? "");
            if (Ktrl != 0)
            {
                MessageBox.Show("Rekening " + v019 + " niet te vinden.\r\nEerst SETUPrekening inbrengen a.u.b. !");
                DKTRL_CUMUL += 99;
                return false;
            }

            RecordToVeld(TABLE_LEDGERACCOUNTS);

            double dece068 = double.TryParse(RV(rsJournaal, "dece068")?.ToString(), out double parsedDece068)
                ? parsedDece068
                : 0.0;

            if (ACTIVE_BOOKYEAR != 0)
            {
                VBib(TABLE_LEDGERACCOUNTS,
                    ((double.TryParse(VBibText(TABLE_LEDGERACCOUNTS, "#e023 #"), out double e023Val) ? e023Val : 0.0) + v068).ToString(),
                    "e023");

                double dece023 = double.TryParse(rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece023"].Value?.ToString(), out double parsedDece023)
                    ? parsedDece023
                    : 0.0;
                rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece023"].Value = dece023 + dece068;
            }
            else
            {
                VBib(TABLE_LEDGERACCOUNTS,
                    ((double.TryParse(VBibText(TABLE_LEDGERACCOUNTS, "#e022 #"), out double e022Val) ? e022Val : 0.0) + v068).ToString(),
                    "e022");

                double dece022 = double.TryParse(rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value?.ToString(), out double parsedDece022)
                    ? parsedDece022
                    : 0.0;
                rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value = dece022 + dece068;
            }

            rsMAR[TABLE_LEDGERACCOUNTS].Fields["dnnsync"].Value = false;
            BUpdate(TABLE_LEDGERACCOUNTS, 0);

            try
            {
                rsJournaal.Update();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
               
        /// <summary>        
        /// Writes a single value to the named field of rsMAR(Fl).
        /// Trims string values to the field DefinedSize to match VB6 behavior.
        /// </summary>
        private static void SetFields(int fl, string fieldCode, string value)
        {
            string vBCode = (fieldCode ?? string.Empty).TrimEnd();

            try
            {
                Field field = Globals.rsMAR[fl].Fields[vBCode];
                if (field == null)
                {
                    return;
                }

                if (string.Equals(vBCode, "MEMO", StringComparison.OrdinalIgnoreCase))
                {
                    field.Value = TLB_RECORD[fl];
                    return;
                }

                string stringData = value ?? string.Empty;
                int definedSize = 0;
                try
                {
                    definedSize = field.DefinedSize;
                }
                catch
                {
                    definedSize = 0;
                }

                string truncatedValue = definedSize > 0 && stringData.Length > definedSize
                    ? stringData.Substring(0, definedSize)
                    : stringData;

                object currentValue = field.Value;
                string currentText = currentValue == null || currentValue is DBNull ? string.Empty : currentValue.ToString();

                if (string.Equals(currentText, truncatedValue, StringComparison.Ordinal))
                {
                    return;
                }

                if (stringData.Length == 0)
                {
                    return;
                }

                field.Value = truncatedValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(vBCode + "\r\n" + ex.Message + "\r\n\r\nGeef deze foutcode melding door a.u.b. 053/21.59.25.  Dank U",
                    "Velden in rijen plaatsen (SetFields)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        
        /// <summary>        
        /// Adds a new column to an existing table via ALTER TABLE … ADD COLUMN DDL
        /// executed on the active ADO connection (no ADOX dependency required).
        ///
        /// clType values (ADOX DataTypeEnum):
        ///   adDate         =   7  → DATETIME
        ///   adInteger      =   3  → INTEGER
        ///   adCurrency     =   6  → CURRENCY
        ///   adSingle       =   4  → SINGLE
        ///   adDouble       =   5  → DOUBLE
        ///   adBoolean      =  11  → BIT
        ///   adVarWChar     = 202  → TEXT(clLengte)
        ///   adLongVarWChar = 203  → MEMO
        /// Returns true when the column was added successfully.
        /// </summary>
        internal static bool AdxKolom(string tbNaam, string clNaam, long clType, long clLengte)
        {
            string sqlType;
            switch (clType)
            {
                case 7: sqlType = "DATETIME"; break; // adDate
                case 3: sqlType = "INTEGER"; break; // adInteger
                case 6: sqlType = "CURRENCY"; break; // adCurrency
                case 4: sqlType = "SINGLE"; break; // adSingle
                case 5: sqlType = "DOUBLE"; break; // adDouble
                case 11: sqlType = "BIT"; break; // adBoolean
                case 202: sqlType = clLengte > 0 ? "TEXT(" + clLengte + ")" : "TEXT"; break; // adVarWChar
                case 203: sqlType = "MEMO"; break; // adLongVarWChar
                default: sqlType = "TEXT"; break;
            }

            string sql = "ALTER TABLE " + tbNaam + " ADD COLUMN " + clNaam + " " + sqlType;

            try
            {
                object rAffected = Type.Missing;
                Globals.adntDB.Execute(sql, out rAffected, (int)ADODB.CommandTypeEnum.adCmdText);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Foutmelding bron: " + ex.Source + "\r\n" +
                    "Foutkodenummer: " + ex.HResult + "\r\n\r\n" +
                    "Foutmelding omschrijving:\r\n" + ex.Message);
                MessageBox.Show("Aanmaak Kolom " + clNaam + " zonder succes.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>        
        /// Looks up ZoekTekst in adKBTable, splits the "splitDefinitie" field on ";" and
        /// populates DeKontrol (ListBox). Sets aLijnen to the item count and optieNr to the
        /// 0-based index of the item matching optieTxt. Returns the matching item text, or "".
        /// </summary>
        internal static string ZoekEnPlaats(
            ListBox deKontrol,
            string zoekTekst,
            out int aLijnen,
            out int optieNr,
            string optieTxt)
        {
            aLijnen = 0;
            optieNr = 0;
            string result = string.Empty;

            try
            {
                Globals.adKBTable.Seek(zoekTekst, SeekEnum.adSeekFirstEQ);
                if (Globals.adKBTable.EOF)
                {
                    MessageBox.Show("Stop !  Keuzebox " + zoekTekst + " niet te vinden...");
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }

            string joinStringHier;
            try
            {
                object val = Globals.adKBTable.Fields["splitDefinitie"].Value;
                joinStringHier = val == null || val == DBNull.Value
                    ? string.Empty
                    : val.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }

            if (!joinStringHier.EndsWith(";"))
                joinStringHier += ";";

            int optieLen = optieTxt.Length;
            deKontrol.Items.Clear();

            int puntKommaLokatie = 0; // 0-based index into the string
            while (true)
            {
                int semiIdx = joinStringHier.IndexOf(';', puntKommaLokatie);
                if (semiIdx < 0)
                    break;

                string deString = joinStringHier.Substring(puntKommaLokatie, semiIdx - puntKommaLokatie);
                deKontrol.Items.Add(deString);
                aLijnen++;

                if (optieLen > 0 &&
                    puntKommaLokatie + optieLen <= joinStringHier.Length &&
                    string.Compare(joinStringHier, puntKommaLokatie, optieTxt, 0, optieLen, StringComparison.Ordinal) == 0)
                {
                    optieNr = aLijnen - 1;
                    result = deString;
                }

                puntKommaLokatie = semiIdx + 1;
            }

            if (optieLen == 0)
                optieNr = 0;

            return result;
        }

        /// <summary>        
        /// Builds the adKBTable seek key from marBoxNumber + Taal, populates a temporary
        /// ListBox via ZoekEnPlaats and returns the item that matches marBoxOption.
        /// </summary>
        internal static string FMarBoxText(string marBoxNumber, string taal, string marBoxOption)
        {
            string zoekTekst;
            switch (marBoxNumber.Length)
            {
                case 2:
                    zoekTekst = "NTKB" + taal + "9";
                    break;
                case 3:
                    zoekTekst = "NTKB" + taal;
                    break;
                case 4:
                    MessageBox.Show("Stop");
                    zoekTekst = "NT";
                    break;
                default:
                    MessageBox.Show("fmarBoxText fout");
                    return string.Empty;
            }
            zoekTekst += marBoxNumber;

            // Use a temporary off-screen ListBox (replaces VB6 KeuzeVSF.NTBoxLijst)
            using (var tempList = new ListBox())
            {
                int aLijnen, optieNr;
                return ZoekEnPlaats(tempList, zoekTekst, out aLijnen, out optieNr, marBoxOption);
            }
        }

        /// <summary>        
        /// Checks if a field exists in rsMAR(flHier). If not, and VeldDef is provided,
        /// prompts the user to add the column via ALTER TABLE. Returns 0 when field exists,
        /// 99 when user cancels, or the error code otherwise.
        /// </summary>
        internal static long VeldOK(int flHier, string veldNaam, string veldDef = "")
        {
            if (Globals.rsMAR[flHier] == null ||
                Globals.rsMAR[flHier].State == (int)ObjectStateEnum.adStateClosed)
                Globals.Ktrl = BOpen(flHier);

            // Check whether the field exists
            try
            {
                string _ = Globals.rsMAR[flHier].Fields[veldNaam].Name;
                return 0;
            }
            catch { }

            // Field does not exist
            if (string.IsNullOrEmpty(veldDef))
                return 1;

            BClose(flHier);

            Globals.Msg = "ALTER TABLE " + Globals.bstNaam[flHier]
                        + " ADD COLUMN " + veldNaam + " " + veldDef + ";";

            if (MessageBox.Show(
                    Globals.Msg + "\r\r" + "SQL-instructie uitvoeren",
                    string.Empty,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    object recordsAffected = Type.Missing;
                    Globals.adntDB.Execute(Globals.Msg, out recordsAffected, (int)CommandTypeEnum.adCmdText);
                    MessageBox.Show(Globals.Msg + " met succes uitgevoerd", string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Foutmelding bron: " + ex.Source + "\r\n" +
                        "Foutkodenummer: " + ex.HResult + "\r\n\r\n" +
                        "Foutmelding omschrijving:\r\n" + ex.Message);
                    return ex.HResult;
                }
            }
            else
            {
                return 99;
            }
        }

        /// <summary>        
        /// Opens the company Marnt.MDV via a dedicated Jet connection
        /// definitions and renames any table whose name is a year in the range "1900"–"2004"
        /// by prepending "jr" (e.g. "2001" → "jr2001").
        /// Returns false (matches VB6 default — the return value was never set to True).
        /// ADOX is not referenced; DAO TableDefs are accessed via dynamic to match the
        /// existing codebase pattern for Globals.ntDB / Globals.NTRuimte.
        /// </summary>
        internal static bool TabelKontrole()
        {
            string dbPath = Globals.LOCATION_COMPANYDATA + "marnt.mdv";
            string connectString = Globals.ADOJET_PROVIDER + "Data Source=" + dbPath + ";";

            dynamic dao = null;
            ADODB.Connection cnn = null;
            try
            {
                cnn = new ADODB.Connection();
                cnn.Open(connectString);

                // Open the same MDB via DAO so we can rename TableDefs
                // (ADODB/ADO cannot rename tables; DAO TableDef.Name is writeable)
                if (Globals.NTRuimte == null)
                    return false;
                dao = ((dynamic)Globals.NTRuimte).OpenDatabase(dbPath, false, false);

                dynamic tableDefs = dao.TableDefs;
                int count = (int)tableDefs.Count;

                for (int i = 0; i < count; i++)
                {
                    string name = (string)tableDefs[i].Name;
                    if (int.TryParse(name, out int year) && year >= 1900 && year <= 2004)
                    {
                        tableDefs[i].Name = "jr" + name;
                    }
                }

                tableDefs.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("TabelKontrole fout:\r\n" + ex.Message);
            }
            finally
            {
                try { dao?.Close(); } catch { }
                try
                {
                    if (cnn != null &&
                        cnn.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                        cnn.Close();
                }
                catch { }
            }

            return false;
        }

        /// <summary>VB6: bNext — move to next record and update KEY_BUF.</summary>
        internal static void BNext(int fl)
        {
            if (rsMAR[fl].State == (int)ADODB.ObjectStateEnum.adStateClosed)
            {
                if (fl == TABLE_VARIOUS)
                    BLast(fl, 1);
                else
                    BLast(fl, 0);
            }

        AccessNext:
            if (rsMAR[fl].BOF || rsMAR[fl].EOF)
            {
                Ktrl = 9;
                MessageBox.Show("Er is geen record (meer).");
                return;
            }

            try
            {
                rsMAR[fl].MoveNext();
                if (rsMAR[fl].EOF)
                {
                    Ktrl = 9;
                }
                else
                {
                    Ktrl = 0;
                    KEY_BUF[fl] = rsMAR[fl].Fields[
                        JETTABLEUSE_INDEX[fl, KEY_INDEX[fl]].Substring(0, 4)].Value?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                string msg =
                    "Database stopkode " + ex.HResult + "\r\n\r\n" +
                    "Mededeling :\r\n" +
                    ex.Message + "\r\n\r\n" +
                    "Steeds opnieuw proberen ?";
                if (MessageBox.Show(msg, string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    goto AccessNext;
            }
        }

        /// <summary>VB6: bGetOrGreater — seek to key or first record >= key.</summary>
        internal static void BGetOrGreater(int fl, int fIndex, string fSleutel)
        {
        opnieuwGOG:
            if (rsMAR[fl].State == (int)ObjectStateEnum.adStateClosed)
                Ktrl = BOpen(fl);

            try
            {
                fSleutel = VSet(fSleutel, FLINDEX_LEN[fl, fIndex]);

                if (rsMAR[fl].Index != FLINDEX_CAPTION[fl, fIndex])
                {
                    try
                    {
                        rsMAR[fl].Index = FLINDEX_CAPTION[fl, fIndex];
                    }
                    catch (Exception indexEx)
                    {
                        // Error -2147217883 = index not found — reopen and retry
                        if ((indexEx.HResult & 0xFFFF) == 0x7763 || indexEx.HResult == -2147217883)
                        {
                            BClose(fl);
                            Application.DoEvents();
                            goto opnieuwGOG;
                        }
                        throw;
                    }
                }

                rsMAR[fl].Seek(fSleutel, (SeekEnum)(int)SeekEnum.adSeekAfterEQ);

                Ktrl = rsMAR[fl].EOF ? 4 : 0;
                KEY_BUF[fl] = rsMAR[fl].Fields[JETTABLEUSE_INDEX[fl, fIndex].TrimEnd()].Value?.ToString() ?? string.Empty;
                KEY_INDEX[fl] = fIndex;
            }
            catch { }
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BDelete(int fl)
        {
            Ktrl = 0;

            try
            {
                rsMAR[fl].Delete();
            }
            catch (Exception ex)
            {
                Ktrl = ex.HResult & 0xFFFF;
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>VB6: Editmogelijk — always returns true (error-handling path is dead code in original VB6).</summary>
        internal static bool Editmogelijk(int fl)
        {
            return true;
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BAbort()
        {
            try
            {
                Globals.adntDB.RollbackTrans();
                Ktrl = 0;
            }
            catch (Exception ex)
            {
                Ktrl = ex.HResult;
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BBegin()
        {
            try
            {
                Globals.adntDB.BeginTrans();
                Ktrl = 0;
            }
            catch (Exception ex)
            {
                Ktrl = ex.HResult;
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BEnd()
        {
            try
            {
                Globals.adntDB.CommitTrans();
                Ktrl = 0;
            }
            catch (Exception ex)
            {
                Ktrl = ex.HResult;
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BFirst(int fl, int fIndex)
        {
        MoveFirstNogEens:
            if (rsMAR[fl].State == (int)ADODB.ObjectStateEnum.adStateClosed)
                Ktrl = BOpen(fl);

            try
            {
                rsMAR[fl].Index = FLINDEX_CAPTION[fl, fIndex];
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147217883)
                {
                    MessageBox.Show(ex.Message, "Gecontroleerde foutopvang...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BClose(fl);
                    goto MoveFirstNogEens;
                }
                Ktrl = 9;
                return;
            }

            if (rsMAR[fl].RecordCount == 0)
            {
                Ktrl = 9;
                return;
            }

            rsMAR[fl].MoveFirst();
            Ktrl = 0;
            KEY_INDEX[fl] = fIndex;

            try
            {
                KEY_BUF[fl] = string.Empty;
                object fieldVal = rsMAR[fl].Fields[JETTABLEUSE_INDEX[fl, fIndex].TrimEnd()].Value;
                if (fieldVal == null || fieldVal is DBNull)
                {
                    // leave KEY_BUF empty
                }
                else if (FLINDEX_LEN[fl, fIndex] == 0)
                {
                    KEY_BUF[fl] = rsMAR[fl].Fields[JETTABLEUSE_INDEX[fl, KEY_INDEX[fl]].Substring(0, 4)].Value?.ToString()?.Trim() ?? string.Empty;
                }
                else
                {
                    KEY_BUF[fl] = VSet(
                        rsMAR[fl].Fields[JETTABLEUSE_INDEX[fl, fIndex].TrimEnd()].Value?.ToString() ?? string.Empty,
                        FLINDEX_LEN[fl, fIndex]);
                }
            }
            catch
            {
                KEY_BUF[fl] = string.Empty;
            }
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BLast(int fl, int fIndex)
        {
        MoveLastNogEens:
            if (rsMAR[fl].State == (int)ADODB.ObjectStateEnum.adStateClosed)
                Ktrl = BOpen(fl);

            try
            {
                rsMAR[fl].Index = FLINDEX_CAPTION[fl, fIndex];
            }
            catch
            {
                try
                {
                    rsMAR[fl].MoveLast();
                }
                catch
                {
                    MessageBox.Show("Stop");
                    return;
                }
                BClose(fl);
                goto MoveLastNogEens;
            }

            if (rsMAR[fl].RecordCount == 0)
            {
                Ktrl = 9;
                return;
            }

            try
            {
                rsMAR[fl].MoveLast();
                Ktrl = 0;
            }
            catch (Exception ex)
            {
                Ktrl = ex.HResult;
                MessageBox.Show(ex.Message);
            }

            KEY_INDEX[fl] = fIndex;
            KEY_BUF[fl] = string.Empty;
            try
            {
                object fieldVal = rsMAR[fl].Fields[JETTABLEUSE_INDEX[fl, fIndex].TrimEnd()].Value;
                if (fieldVal == null || fieldVal is DBNull)
                {
                    BPrev(fl);
                }
                else if (FLINDEX_LEN[fl, fIndex] == 0)
                {
                    KEY_BUF[fl] = "...";
                }
                else
                {
                    KEY_BUF[fl] = VSet(fieldVal.ToString(), FLINDEX_LEN[fl, fIndex]);
                }
            }
            catch
            {
                KEY_BUF[fl] = string.Empty;
            }
        }

        /// <summary>
        /// Came before from NOVELL BTRIEVE
        /// Adds a new record to rsMAR(Fl), writes fields via VeldToRecord, calls Update,
        /// and — for TABLE_JOURNAL — updates cumulative totals and the ledger account balance.
        /// </summary>
        internal static void BInsert(int fl, int fIndex, SharedForms.FormBoeking boekingForm = null)
        {
            if (fl == TABLE_INVOICES)
            {
                // TABLE_INVOICES: AddNew is handled externally (VB6: no-op branch)
            }
            else
            {
                if (rsMAR[fl].State == (int)ObjectStateEnum.adStateClosed)
                    Ktrl = BOpen(fl);
                rsMAR[fl].AddNew();
            }

            VeldToRecord(fl);
            if (Ktrl == 32000) return;

            KEY_INDEX[fl] = fIndex;
            KEY_BUF[fl] = FVT[fl, fIndex];

            if (fl == TABLE_JOURNAL)
            {
                double v068 = Convert.ToDouble(rsMAR[TABLE_JOURNAL].Fields["v068"].Value);
                DKTRL_CUMUL += v068;

                if (bhEuro)
                {
                    DKTRL_BEF += Math.Round(v068 * EURO, 0);
                    DKTRL_EUR += Math.Round(v068, 2);
                    try { rsMAR[TABLE_JOURNAL].Fields["dece068"].Value = v068; } catch { }
                }
                else
                {
                    DKTRL_BEF += Math.Round(v068, 0);
                    DKTRL_EUR += Math.Round(v068 / EURO, 2);
                }

                double v068d = v068;
                string pipo = rsMAR[fl].Fields["v019"].Value?.ToString() + "\t"
                            + rsMAR[fl].Fields["v067"].Value?.ToString() + "\t";

                if (bhEuro)
                {
                    if (v068d < 0)
                    {
                        pipo += "" + "\t" + (-v068d).ToString("#,##0.00");
                        pipo += "\t" + "" + "\t" + Math.Round(-v068d * EURO, 0).ToString("#,##0.00");
                    }
                    else
                    {
                        pipo += v068d.ToString("#,##0.00") + "\t" + "";
                        pipo += "\t" + Math.Round(v068d * EURO, 0).ToString("#,##0.00") + "\t" + "";
                    }
                }
                else
                {
                    if (v068d < 0)
                    {
                        pipo += "" + "\t" + (-v068d / EURO).ToString("#,##0.00");
                        pipo += "\t" + "" + "\t" + (-v068d).ToString("#,##0.00");
                    }
                    else
                    {
                        pipo += (v068d / EURO).ToString("#,##0.00") + "\t" + "";
                        pipo += "\t" + v068d.ToString("#,##0.00") + "\t" + "";
                    }
                }
                // Port of VB6: frmBoeking.mshfBoekLijst.AddItem pipo
                // BInsert has no FormBoeking reference; pass boekingForm via the optional parameter if needed.
                boekingForm?.AddItem(pipo);
            }

            try
            {
                Ktrl = 0;
                rsMAR[fl].Update();
            }
            catch (Exception ex)
            {
                int errNr = ex.HResult & 0xFFFF;
                switch (errNr)
                {
                    case 3022:
                        MessageBox.Show("Unieke sleutel reeds aanwezig in bestand : " + bstNaam[fl]
                            + "\r\n\r\n" + "Mogelijke sleutel : " + FVT[fl, fIndex]);
                        Ktrl = errNr;
                        break;
                    default:
                        bool isNull = false;
                        try { isNull = rsMAR[fl].Fields[fIndex].Value == null || rsMAR[fl].Fields[fIndex].Value is DBNull; } catch { isNull = true; }
                        if (isNull)
                            MessageBox.Show(ex.Message + "\r\n\r\n" + "Bestand : " + bstNaam[fl]
                                + "\r\n\r\n" + "De sleutel heeft 'null' waarde",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                            MessageBox.Show(ex.Message + "\r\n\r\n" + "Bestand : " + bstNaam[fl]
                                + "\r\n\r\n" + "Mogelijke sleutel : " + FVT[fl, fIndex],
                                "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Ktrl = errNr;
                        break;
                }
            }

            if (fl == TABLE_JOURNAL)
            {
                if (Ktrl != 0)
                {
                    MessageBox.Show("bInsert journaal stopkode " + Ktrl);
                }
                else
                {
                    string rekening = (FVT[TABLE_JOURNAL, 0] ?? "").Length >= 7
                        ? FVT[TABLE_JOURNAL, 0].Substring(0, 7)
                        : FVT[TABLE_JOURNAL, 0];

                    BGet(TABLE_LEDGERACCOUNTS, 0, rekening);
                    if (Ktrl != 0)
                    {
                        MessageBox.Show("Rekening " + rekening + " niet te vinden." + "\r\n"
                            + "Eerst SETUPrekening inbrengen a.u.b. !");
                        DKTRL_CUMUL += 99;
                        return;
                    }
                    else if (ACTIVE_BOOKYEAR != 0)
                    {
                        RecordToVeld(TABLE_LEDGERACCOUNTS);

                        double e023Val = double.TryParse(VBibText(TABLE_LEDGERACCOUNTS, "#e023 #"), out double parsedE023)
                            ? parsedE023
                            : 0.0;
                        double v068Val = double.TryParse(VBibText(TABLE_JOURNAL, "#v068 #"), out double parsedV068)
                            ? parsedV068
                            : 0.0;

                        VBib(TABLE_LEDGERACCOUNTS, (e023Val + v068Val).ToString(), "e023");

                        double dece023Val = double.TryParse(rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece023"].Value?.ToString(), out double parsedDece023)
                            ? parsedDece023
                            : 0.0;
                        double dece068Val = double.TryParse(rsMAR[TABLE_JOURNAL].Fields["dece068"].Value?.ToString(), out double parsedDece068)
                            ? parsedDece068
                            : 0.0;
                        rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece023"].Value = dece023Val + dece068Val;
                    }
                    else
                    {
                        RecordToVeld(TABLE_LEDGERACCOUNTS);

                        double e022Val = double.TryParse(VBibText(TABLE_LEDGERACCOUNTS, "#e022 #"), out double parsedE022)
                            ? parsedE022
                            : 0.0;
                        double v068Val = double.TryParse(VBibText(TABLE_JOURNAL, "#v068 #"), out double parsedV068)
                            ? parsedV068
                            : 0.0;

                        VBib(TABLE_LEDGERACCOUNTS, (e022Val + v068Val).ToString(), "e022");

                        double dece022Val = double.TryParse(rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value?.ToString(), out double parsedDece022)
                            ? parsedDece022
                            : 0.0;
                        double dece068Val = double.TryParse(rsMAR[TABLE_JOURNAL].Fields["dece068"].Value?.ToString(), out double parsedDece068)
                            ? parsedDece068
                            : 0.0;
                        rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value = dece022Val + dece068Val;
                    }
                    BUpdate(TABLE_LEDGERACCOUNTS, 0);
                }
            }

            switch (Ktrl)
            {
                case 0:
                    break;
                case 5:
                    MessageBox.Show("Dergelijke ID.Kode Bestaat reeds : " + KEY_BUF[fl] + " : " + fl);
                    break;
                case 46:
                    MessageBox.Show("Bestand werd geopend in LEES-modus.\r\nSchrijven is niet mogelijk...",
                        "Database beveiliging");
                    break;
                default:
                    MessageBox.Show("Stopkode " + Ktrl + " tijdens invoegen nieuwe record.");
                    break;
            }
        }

        /// <summary>Came before from NOVELL BTRIEVE.</summary>
        internal static void BPrev(int fl)
        {
            if (rsMAR[fl].State == (int)ADODB.ObjectStateEnum.adStateClosed)
            {
                if (fl == TABLE_VARIOUS)
                    BFirst(fl, 1);
                else
                    BFirst(fl, 0);
                return;
            }

            if (rsMAR[fl].BOF || rsMAR[fl].EOF)
            {
                BFirst(fl, 0);
                return;
            }

            rsMAR[fl].MovePrevious();
            if (rsMAR[fl].BOF)
            {
                Ktrl = 9;
            }
            else
            {
                Ktrl = 0;
                try
                {
                    KEY_BUF[fl] = rsMAR[fl].Fields[JETTABLEUSE_INDEX[fl, KEY_INDEX[fl]].TrimEnd()].Value?.ToString() ?? string.Empty;
                }
                catch { }
            }
        }

        /// <summary>
        /// Initialises a blank record for the given table with default field values.
        /// Came before from NOVELL BTRIEVE.
        /// </summary>
        public static bool DaoBlankoRecord(int fl)
        {
            TLB_RECORD[fl] = string.Empty;

            switch (fl)
            {
                case int f when f == TABLE_CUSTOMERS || f == TABLE_SUPPLIERS:
                    VBib(fl, "2", "A10C");    //Taalkode
                    VBib(fl, "002", "v149");    //Landnummer  ISO kode
                    VBib(fl, "B  ", "A109");    //Landkode Postkantoor
                    VBib(fl, "BE", "v150");    //Landkode    ISO kode
                    if (bhEuro)
                        VBib(fl, "EUR", "vs03"); //Munteenheid ISO kode
                    else
                        VBib(fl, "BEF", "vs03"); //Munteenheid ISO kode
                    VBib(fl, "1", "vs07");    //exemplaren dokumenten
                    break;

                case int f when f == TABLE_LEDGERACCOUNTS:
                    VBib(fl, "O", "v032");      //Budgetcode
                    break;

                case int f when f == TABLE_PRODUCTS:
                    VBib(fl, FMarBoxText("004", "2", "0"), "v106");
                    VBib(fl, Dec(1, "#####.00"), "v107");
                    VBib(fl, FMarBoxText("022", "2", "N"), "v108");
                    VBib(fl, FMarBoxText("002", "2", String99(183)), "v111");
                    VBib(fl, String99(77), "v116");
                    VBib(fl, String99(78), "v117");
                    VBib(fl, String99(79), "v118");
                    break;
            }

            return true;
        }
                
        /// <summary>
        /// VB6: Function SQLPopUp — looks up a SQL definition in TABLE_VARIOUS by key,
        /// ensures the link field/record exists, then opens FormSearchSQL for inline editing.
        /// Returns true when at least one key was processed successfully.
        /// </summary>
        public static bool SqlPopUp(string opzoekReeks, string tbNaam, string vldNaam, string idKode)
        {
            while (opzoekReeks != string.Empty)
            {
                string zoekstring;
                int semi = opzoekReeks.IndexOf(';');
                if (semi < 0)
                {
                    zoekstring = opzoekReeks;
                    opzoekReeks = string.Empty;
                }
                else
                {
                    zoekstring = opzoekReeks.Substring(0, semi);
                    opzoekReeks = opzoekReeks.Substring(semi + 1);
                }

                BGet(TABLE_VARIOUS, 1, "29" + zoekstring);
                if (Ktrl != 0)
                    continue;

                RecordToVeld(TABLE_VARIOUS);
                string fullText = VBibText(TABLE_VARIOUS, "#v132 #");
                int colWidthPos = fullText.IndexOf("[Colwidth]", StringComparison.Ordinal);
                string sqlString = colWidthPos >= 0
                    ? fullText.Substring(0, colWidthPos)
                    : fullText;

                int fromPos = sqlString.IndexOf("FROM ", StringComparison.OrdinalIgnoreCase);
                string sqlPopString = fromPos >= 0
                    ? sqlString.Substring(fromPos + 5)
                    : string.Empty;

                if (string.IsNullOrEmpty(sqlPopString))
                {
                    MessageBox.Show("Ongeldige syntax in : " + sqlString, string.Empty,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string linkField = tbNaam.Substring(0, 1) + vldNaam;
                string selectSQL = "SELECT * FROM " + sqlPopString +
                                   " WHERE " + linkField + " Like '" + idKode + "%'";

                var rs = new Recordset { CursorLocation = CursorLocationEnum.adUseClient };
                try
                {
                    rs.Open(selectSQL, adntDB, CursorTypeEnum.adOpenStatic,
                        LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Verbindingsveld bestaat nog niet.  Voeg eerst een veld toe: " + linkField +
                        "\r\n\r\n" + ex.Message);
                    try { rs.Close(); } catch { }
                    continue;
                }

                if (rs.RecordCount == 0)
                {
                    string insertSQL = "INSERT INTO " + sqlPopString +
                                       " (" + linkField + ") VALUES ('" + idKode + "');";
                    try
                    {
                        object affected = Type.Missing;
                        adntDB.Execute(insertSQL, out affected, (int)CommandTypeEnum.adCmdText);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                try { rs.Close(); } catch { }

                string formSQL = sqlString + " WHERE " + linkField + " Like '" + idKode + "%'";
                using (var frm = new marVSS2028.PublicForms.FormSearchSQL())
                {
                    GridText = formSQL;
                    frm.Text = sqlString;
                    frm.ShowDialog();
                }
            }

            return true;
        }

        /// <summary>
        /// VB6: Function OpenSchemeAsString — returns a newline-separated list of user table names.
        /// </summary>
        public static string OpenSchemeAsString()
        {
            var sb = new System.Text.StringBuilder();
            try
            {
                Recordset rstSchema = adntDB.OpenSchema(
                    SchemaEnum.adSchemaTables,
                    new object[] { null, null, null, "TABLE" },
                    Type.Missing);

                while (!rstSchema.EOF)
                {
                    sb.Append(rstSchema.Fields["TABLE_NAME"].Value?.ToString() ?? string.Empty);
                    sb.Append('\r');
                    rstSchema.MoveNext();
                }
                rstSchema.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sb.ToString();
        }

        /// <summary>
        /// VB6: Function vBT — retrieves the value of a tagged field from a raw record string.
        /// Equivalent to VBibText but operates on an arbitrary record string instead of TLB_RECORD.
        /// </summary>
        public static string VBT(string tlbr, string tbs)
        {
            if (string.IsNullOrEmpty(tlbr)) return string.Empty;
            string tbsHier = ("#" + tbs).PadRight(7).Substring(0, 7);
            try
            {
                int start = tlbr.IndexOf(tbsHier, StringComparison.Ordinal);
                if (start < 0) return string.Empty;
                int valueStart = start + 7;
                int hashPos = tlbr.IndexOf('#', valueStart);
                if (hashPos < 0) return string.Empty;
                return tlbr.Substring(valueStart, hashPos - valueStart);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// VB6: Sub GetAllIndexes — fills a ComboBox with index entries for the given table.
        /// Each item is formatted as "+COLUMN_NAME; INDEX_NAME".
        /// </summary>
        public static void GetAllIndexes(string tbNaam, ComboBox combo)
        {
            combo.Items.Clear();
            try
            {
                Recordset rstSchema = adntDB.OpenSchema(SchemaEnum.adSchemaIndexes, Type.Missing, Type.Missing);
                while (!rstSchema.EOF)
                {
                    string tableName = rstSchema.Fields["TABLE_NAME"].Value?.ToString() ?? string.Empty;
                    if (string.Compare(tbNaam, tableName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        string colName = rstSchema.Fields["COLUMN_NAME"].Value?.ToString() ?? string.Empty;
                        string idxName = rstSchema.Fields["INDEX_NAME"].Value?.ToString() ?? string.Empty;
                        combo.Items.Add("+" + colName + "; " + idxName);
                    }
                    rstSchema.MoveNext();
                }
                rstSchema.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// VB6: Function adxMaakDatabase — creates a new Jet (.mdb) database at the given path.
        /// Uses ADOX.Catalog via COM late-binding (no direct ADOX reference required).
        /// Returns true on success.
        /// </summary>
        public static bool AdxMaakDatabase(string dbNaam, string dbPath)
        {
            try
            {
                Type catalogType = Type.GetTypeFromProgID("ADOX.Catalog");
                if (catalogType == null)
                    throw new InvalidOperationException("ADOX.Catalog is niet geregistreerd op dit systeem.");

                object cat = Activator.CreateInstance(catalogType);
                string connectionString = ADOJET_PROVIDER +
                    "Data Source=" + dbPath + @"\" + dbNaam + ".mdb";

                catalogType.InvokeMember("Create",
                    System.Reflection.BindingFlags.InvokeMethod, null, cat,
                    new object[] { connectionString });

                // Release ActiveConnection
                catalogType.GetProperty("ActiveConnection")
                    ?.SetValue(cat, null);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(cat);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Foutmelding bron: " + ex.Source +
                    "\r\nFoutkodenummer: " + ex.HResult +
                    "\r\n\r\nFoutmelding omschrijving:\r\n" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// VB6: Function adxMaakTabel — appends a new table with an auto-increment ID column
        /// to the currently open database (SQL Server or Jet), using ADOX via COM late-binding.
        /// Returns true on success.
        /// </summary>
        public static bool AdxMaakTabel(string tbNaam)
        {
            try
            {
                Type catalogType = Type.GetTypeFromProgID("ADOX.Catalog");
                if (catalogType == null)
                    throw new InvalidOperationException("ADOX.Catalog is niet geregistreerd op dit systeem.");

                object cat = Activator.CreateInstance(catalogType);

                // Open catalog against the active connection
                bool isSqlServer = adntDB.Properties["DBMS Name"].Value?.ToString()
                    ?.IndexOf("SQL Server", StringComparison.OrdinalIgnoreCase) >= 0;

                string connectStr = isSqlServer
                    ? adntDB.ConnectionString
                    : jetConnect;

                catalogType.GetProperty("ActiveConnection")
                    ?.SetValue(cat, connectStr);

                // Create table object
                Type tableType = Type.GetTypeFromProgID("ADOX.Table");
                object tbl = Activator.CreateInstance(tableType);
                tableType.GetProperty("Name")?.SetValue(tbl, tbNaam);

                // tbl.ParentCatalog = cat
                tableType.GetProperty("ParentCatalog")?.SetValue(tbl, cat);

                // tbl.Columns.Append "ID", adInteger (3), adBigInt (16)
                object columns = tableType.GetProperty("Columns")?.GetValue(tbl);
                Type columnsType = columns?.GetType();
                columnsType?.InvokeMember("Append",
                    System.Reflection.BindingFlags.InvokeMethod, null, columns,
                    new object[] { "ID", 3 /*adInteger*/, 0 });

                // tbl.Columns("ID").Properties("AutoIncrement") = True
                object idColumn = columnsType?.InvokeMember("Item",
                    System.Reflection.BindingFlags.GetProperty, null, columns,
                    new object[] { "ID" });
                if (idColumn != null)
                {
                    object props = idColumn.GetType().GetProperty("Properties")?.GetValue(idColumn);
                    object autoIncProp = props?.GetType().InvokeMember("Item",
                        System.Reflection.BindingFlags.GetProperty, null, props,
                        new object[] { "AutoIncrement" });
                    autoIncProp?.GetType().GetProperty("Value")?.SetValue(autoIncProp, true);
                }

                // cat.Tables.Append tbl
                object tables = catalogType.GetProperty("Tables")?.GetValue(cat);
                tables?.GetType().InvokeMember("Append",
                    System.Reflection.BindingFlags.InvokeMethod, null, tables,
                    new object[] { tbl });

                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                catalogType.GetProperty("ActiveConnection")?.SetValue(cat, null);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cat);

                MessageBox.Show("Aanmaak tabel " + tbNaam + " met succes.", string.Empty,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Foutmelding bron: " + ex.Source +
                    "\r\nFoutkodenummer: " + ex.HResult +
                    "\r\n\r\nFoutmelding omschrijving:\r\n" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// VB6: Sub NieuwBoekjaar — installs a new financial year:
        ///   1. Carries over ledger-account balances (InstelSaldos)
        ///   2. Resets product stock counters (InstelVoorraad)
        ///   3. Creates a new counter table for the new year via ADOX
        ///   4. Renames/rotates DEF*.OCT files
        ///   5. Copies counter records into the new table
        ///   6. Resets counter strings (SetString99)
        ///   7. Updates DEF00.OCT period records
        /// </summary>
        public static bool NewBookYear()
        {
            if (ACTIVE_BOOKYEAR != 0)
            {
                MessageBox.Show("Enkel logisch met hoogste boekjaar actief !  Probeer opnieuw...");
                return false;
            }

            string tempoBestand = TABLEDEF_ONT[TABLE_COUNTERS];
            string tempoBstNaam = bstNaam[TABLE_COUNTERS];

            // Check previous year's DEF01.OCT existence
            if (File.Exists(LOCATION_COMPANYDATA + "DEF01.OCT"))
            {
                BClose(TABLE_COUNTERS);
                TABLEDEF_ONT[TABLE_COUNTERS] = "01.ONT";
                string prevYearNaam = "jr" + (int.Parse(bstNaam[TABLE_COUNTERS].Substring(2)) - 1).ToString("0000");
                bstNaam[TABLE_COUNTERS] = prevYearNaam;

                if (double.Parse(String99(63)) + double.Parse(String99(64)) != 2)
                {
                    MessageBox.Show("Eerst vorig boekjaar in orde brengen !");
                    BClose(TABLE_COUNTERS);
                    TABLEDEF_ONT[TABLE_COUNTERS] = tempoBestand;
                    bstNaam[TABLE_COUNTERS] = tempoBstNaam;
                    return false;
                }

                if (double.Parse(String99(62)) != 1)
                {
                    if (MessageBox.Show(
                            "Eindinventaris van vorig boekjaar overslaan en stock-roulatie vernietigen !  Bent U zeker ?",
                            string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        BClose(TABLE_COUNTERS);
                        TABLEDEF_ONT[TABLE_COUNTERS] = tempoBestand;
                        bstNaam[TABLE_COUNTERS] = tempoBstNaam;
                        return false;
                    }
                }

                BClose(TABLE_COUNTERS);
                TABLEDEF_ONT[TABLE_COUNTERS] = tempoBestand;
                bstNaam[TABLE_COUNTERS] = tempoBstNaam;
            }

            if (MessageBox.Show(
                    "Een nieuw boekjaar wordt geïnstalleerd hierna !  Bent U zeker ?",
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return false;

            // ── 1. Ledger-account balance carry-over ──────────────────────────────
            BClose(TABLE_LEDGERACCOUNTS);
            BFirst(TABLE_LEDGERACCOUNTS, 0);
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Voortijdige stop...");
                return false;
            }

            MessageBox.Show("De lijst algemene rekeningen wordt hersamengesteld.");
            BBegin();
            InstelSaldos();

            while (true)
            {
                BNext(TABLE_LEDGERACCOUNTS);
                if (Ktrl != 0) break;
                InstelSaldos();
            }
            BEnd();
            BClose(TABLE_LEDGERACCOUNTS);

            // ── 2. Product stock reset ────────────────────────────────────────────
            BClose(TABLE_PRODUCTS);
            BFirst(TABLE_PRODUCTS, 0);
            if (Ktrl != 0)
            {
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Er zijn geen produkten...");
            }
            else
            {
                MessageBox.Show("De stockaankoop/verkooptellers van het vorig boekjaar worden op nul gezet.");
                BBegin();
                InstelVoorraad();

                while (true)
                {
                    BNext(TABLE_PRODUCTS);
                    if (Ktrl != 0) break;
                    InstelVoorraad();
                }
                BEnd();
                BClose(TABLE_PRODUCTS);
            }

            // ── 3. Create new counter table via ADOX ──────────────────────────────
            var byperdat = Application.OpenForms["FormBYPERDAT"] as marVSS2028.FormBYPERDAT;
            if (byperdat == null)
            {
                MessageBox.Show("FormBYPERDAT is niet geopend.");
                return false;
            }

            int newYear = int.Parse(byperdat.CmbBoekjaar.Text) + 1;
            string newTableName = "jr" + newYear.ToString("0000");

            try
            {
                object rAffected = Type.Missing;

                // Create the new counter table via DDL
                adntDB.Execute(
                    "CREATE TABLE [" + newTableName + "] (v071 TEXT(5), v217 TEXT(255))",
                    out rAffected,
                    (int)ADODB.CommandTypeEnum.adCmdText);

                // Create a unique index on the primary key column
                string indexName = FLINDEX_CAPTION[TABLE_COUNTERS, 0];
                string indexField = JETTABLEUSE_INDEX[TABLE_COUNTERS, 0].TrimEnd();
                adntDB.Execute(
                    "CREATE UNIQUE INDEX [" + indexName + "] ON [" + newTableName + "] ([" + indexField + "])",
                    out rAffected,
                    (int)ADODB.CommandTypeEnum.adCmdText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout aanmaak tabel " + newTableName + ":\r\n" + ex.Message);
                return false;
            }

            Application.DoEvents();

            // ── 4. Rotate DEF*.OCT files ──────────────────────────────────────────
            for (int i = 9; i >= 1; i--)
            {
                string vroeger2 = "DEF" + (i - 1).ToString("00") + ".OCT";
                string nu2 = "DEF" + i.ToString("00") + ".OCT";

                if (File.Exists(LOCATION_COMPANYDATA + vroeger2))
                {
                    if (File.Exists(LOCATION_COMPANYDATA + nu2))
                        File.Delete(LOCATION_COMPANYDATA + nu2);
                    File.Move(LOCATION_COMPANYDATA + vroeger2, LOCATION_COMPANYDATA + nu2);
                }
            }

            Application.DoEvents();

            // Copy DEF01.OCT → LOCATION_COMPANYDATA\DEF00.OCT
            if (!File.Exists(LOCATION_COMPANYDATA + "DEF01.OCT"))
            {
                MessageBox.Show("DEF01.OCT niet gevonden — stop.");
                return false;
            }
            File.Copy(LOCATION_COMPANYDATA + "DEF01.OCT", PROGRAM_LOCATION + "DEF01.OCT", overwrite: true);
            if (File.Exists(LOCATION_COMPANYDATA + "DEF00.OCT"))
                File.Delete(LOCATION_COMPANYDATA + "DEF00.OCT");
            File.Move(PROGRAM_LOCATION + "DEF01.OCT", LOCATION_COMPANYDATA + "DEF00.OCT");

            // ── 5. Copy counter records into new table ────────────────────────────
            var rsTempo = new ADODB.Recordset();
            while (true)
            {
                try
                {
                    rsTempo.Open("SELECT * FROM " + newTableName, adntDB,
                        ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic,
                        (int)ADODB.CommandTypeEnum.adCmdText);
                    break;
                }
                catch
                {
                    MessageBox.Show("Even wachten...");
                }
            }

            bstNaam[TABLE_COUNTERS] = tempoBstNaam;
            BFirst(TABLE_COUNTERS, 0);
            if (Ktrl != 0)
                MessageBox.Show("Er gaat iets fout bij overdracht van tellers");

            while (true)
            {
                RecordToVeld(TABLE_COUNTERS);
                rsTempo.AddNew();
                rsTempo.Fields["v071"].Value = VBibText(TABLE_COUNTERS, "#v071 #");
                rsTempo.Fields["v217"].Value = VBibText(TABLE_COUNTERS, "#v217 #");
                rsTempo.Update();
                BNext(TABLE_COUNTERS);
                if (Ktrl != 0) break;
            }
            BClose(TABLE_COUNTERS);
            rsTempo.Close();

            bstNaam[TABLE_COUNTERS] = newTableName;
            FL99_RECORD = "0";

            // ── 6. Reset counter strings ──────────────────────────────────────────
            SetString99(62);
            SetString99(63);
            SetString99(64);

            if (!BOOKYEAR_FROMTO.Substring(12, 2).Equals("12", StringComparison.Ordinal))
            {
                MessageBox.Show(
                    "Boekjaar eindigt niet in december.  U dient zelf te beslissen of de tellers van " +
                    "aankoop-, verkoop- en financiële documenten dienen op nul gebracht te worden.");
                goto TotSlot;
            }

            if (MessageBox.Show("Aankooptellers op 0 zetten", string.Empty, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SetString99(1); SetString99(2); SetString99(3); SetString99(4);
            }
            SetString99(15); SetString99(205);

            if (MessageBox.Show("Verkooptellers op 0 zetten", string.Empty, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SetString99(11); SetString99(12); SetString99(13); SetString99(14);
                SetString99(73); SetString99(59); SetString99(188);
            }

            SetString99(31); SetString99(32); SetString99(33); SetString99(34); SetString99(35);
            SetString99(38); SetString99(215); SetString99(216); SetString99(217); SetString99(218);

        TotSlot:

            // ── 7. Update DEF00.OCT period records ───────────────────────────────
            int periodeCount = byperdat.CmbPeriodeBoekjaar.Items.Count;
            string def00 = LOCATION_COMPANYDATA + "DEF00.OCT";

            if (periodeCount > 12)
            {
                MessageBox.Show(
                    "Boekjaar bestaande uit " + periodeCount + " periodes wordt hierna tot 12 periodes gebracht.  " +
                    "Kontroleer na installatie a.u.b. !");

                using (var fs = new FileStream(def00, FileMode.Open, FileAccess.ReadWrite))
                {
                    int recNr = 0;
                    for (int i = periodeCount - 12; i < periodeCount; i++)
                    {
                        recNr++;
                        byperdat.CmbPeriodeBoekjaar.SelectedIndex = i;
                        string bStr = PERIOD_FROMTO.PadRight(16).Substring(0, 16);
                        // Increment start year (+1)
                        string y1 = (int.Parse(bStr.Substring(0, 4)) + 1).ToString("0000");
                        string y2 = (int.Parse(bStr.Substring(8, 4)) + 1).ToString("0000");
                        byte[] buf = Encoding.Default.GetBytes(y1 + bStr.Substring(4, 4) + y2 + bStr.Substring(12, 4));
                        fs.Seek((recNr - 1) * 16L, SeekOrigin.Begin);
                        fs.Write(buf, 0, 16);
                    }
                    // Clear remaining records
                    byte[] blank = new byte[16];
                    for (int i = 13; i <= 99; i++)
                    {
                        fs.Seek((i - 1) * 16L, SeekOrigin.Begin);
                        fs.Write(blank, 0, 16);
                    }
                }
            }
            else
            {
                using (var fs = new FileStream(def00, FileMode.Open, FileAccess.ReadWrite))
                {
                    byte[] buf = new byte[16];
                    for (int recNr = 1; recNr <= 99; recNr++)
                    {
                        fs.Seek((recNr - 1) * 16L, SeekOrigin.Begin);
                        int read = fs.Read(buf, 0, 16);
                        if (read < 16) break;

                        string bStr = Encoding.Default.GetString(buf);
                        if (bStr.Trim() == string.Empty) continue;

                        string y1 = (int.Parse(bStr.Substring(0, 4)) + 1).ToString("0000");
                        string y2 = (int.Parse(bStr.Substring(8, 4)) + 1).ToString("0000");
                        byte[] updated = Encoding.Default.GetBytes(y1 + bStr.Substring(4, 4) + y2 + bStr.Substring(12, 4));
                        fs.Seek((recNr - 1) * 16L, SeekOrigin.Begin);
                        fs.Write(updated, 0, 16);
                    }
                }

                // Remove any .OXT compatibility files
                foreach (string f in Directory.GetFiles(LOCATION_COMPANYDATA, "DEF*.OXT"))
                    File.Delete(f);
            }

            Ktrl = 100;
            MimEnvironment.AutoUnLoadCompany();
            return true;
        }

        // ── InstelSaldos: carry over ledger-account balances for TABLE_LEDGERACCOUNTS ──
        private static void InstelSaldos()
        {
            RecordToVeld(TABLE_LEDGERACCOUNTS);
            for (int i = 30; i >= 22; i--)
            {
                string src = "v" + i.ToString("000");
                string dest = "v" + (i + 1).ToString("000");
                VBib(TABLE_LEDGERACCOUNTS, VBibText(TABLE_LEDGERACCOUNTS, "#" + src + " #"), dest);

                string esrc = "e" + i.ToString("000");
                string edest = "e" + (i + 1).ToString("000");
                VBib(TABLE_LEDGERACCOUNTS, VBibText(TABLE_LEDGERACCOUNTS, "#" + esrc + " #"), edest);

                try
                {
                    decimal saldo = 0m;
                    object val = rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece" + i.ToString("000")].Value;
                    if (val != null && !(val is DBNull))
                        decimal.TryParse(val.ToString(), out saldo);
                    rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece" + (i + 1).ToString("000")].Value = saldo;
                }
                catch { }
            }

            VBib(TABLE_LEDGERACCOUNTS, Dec(0, MASK_SY[0]), "v022");
            VBib(TABLE_LEDGERACCOUNTS, Dec(0, MASK_SY[0]), "e022");
            rsMAR[TABLE_LEDGERACCOUNTS].Fields["dece022"].Value = 0;

            MimEnvironment.SnelHelpPrint(
                rsMAR[TABLE_LEDGERACCOUNTS].Fields["v019"].Value?.ToString() + ", " +
                rsMAR[TABLE_LEDGERACCOUNTS].Fields["v020"].Value?.ToString(), BL_LOGGING);

            BUpdate(TABLE_LEDGERACCOUNTS, 0);
        }

        // ── InstelVoorraad: reset product stock counters for TABLE_PRODUCTS ──────
        private static void InstelVoorraad()
        {
            RecordToVeld(TABLE_PRODUCTS);

            double beginEenheden = double.Parse(VBibText(TABLE_PRODUCTS, "#v114 #").Trim().Length > 0 ? VBibText(TABLE_PRODUCTS, "#v114 #") : "0")
                                 + double.Parse(VBibText(TABLE_PRODUCTS, "#v119 #").Trim().Length > 0 ? VBibText(TABLE_PRODUCTS, "#v119 #") : "0")
                                 - double.Parse(VBibText(TABLE_PRODUCTS, "#v120 #").Trim().Length > 0 ? VBibText(TABLE_PRODUCTS, "#v120 #") : "0");

            decimal beginBedrag = decimal.Parse(VBibText(TABLE_PRODUCTS, "#e123 #").Trim().Length > 0 ? VBibText(TABLE_PRODUCTS, "#e123 #") : "0")
                                + decimal.Parse(VBibText(TABLE_PRODUCTS, "#e121 #").Trim().Length > 0 ? VBibText(TABLE_PRODUCTS, "#e121 #") : "0")
                                - decimal.Parse(VBibText(TABLE_PRODUCTS, "#e122 #").Trim().Length > 0 ? VBibText(TABLE_PRODUCTS, "#e122 #") : "0");

            VBib(TABLE_PRODUCTS, Dec(0, MASK_SY[2]), "v119");
            VBib(TABLE_PRODUCTS, Dec(0, MASK_SY[2]), "v120");
            VBib(TABLE_PRODUCTS, Dec(beginEenheden, MASK_SY[2]), "v114");

            VBib(TABLE_PRODUCTS, Dec(0, MASK_EURX), "e121");
            VBib(TABLE_PRODUCTS, Dec(0, MASK_EURX), "e122");
            VBib(TABLE_PRODUCTS, Dec((double)beginBedrag, MASK_EURX), "e123");

            MimEnvironment.SnelHelpPrint(
                VBibText(TABLE_PRODUCTS, "#v102 #") + ", " + VBibText(TABLE_PRODUCTS, "#v105 #"),
                BL_LOGGING);

            BUpdate(TABLE_PRODUCTS, 0);
        }              
    }
}
