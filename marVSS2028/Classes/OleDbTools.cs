using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace marVSS2028.Classes
{
    internal class OleDbTools
    {
        // COUNTER AND SETTINGS TOOLING
        /// <summary>
        /// Returns the current counters table name (e.g. "jr2027") from the global table-name array.
        /// </summary>
        private static string CountersTable => Globals.bstNaam[Globals.TABLE_COUNTERS];

        /// <summary>
        /// Persists FL99_RECORD into the v217 field of the counter record keyed by "sNNN ".
        /// Inserts the record when it does not yet exist (BAModus = 1 only).
        /// </summary>
        internal static void SetString99(int nummerSleutel)
        {
            string keyBase = "s" + nummerSleutel.ToString("D3");
            string keyFull = keyBase + " "; // 5-char padded form for INSERT

            try
            {
                using (var conn = new OleDbConnection(Globals.oleDbConnect))
                {
                    conn.Open();

                    if (Globals.BAModus == 1)
                    {
                        // Step 1: fetch the exact stored key (trailing spaces vary in Access/JET)
                        string exactKey = null;
                        string selectSql =
                            "SELECT v071 FROM [" + CountersTable + "] WHERE v071 LIKE ?";
                        using (var cmd = new OleDbCommand(selectSql, conn))
                        {
                            cmd.Parameters.AddWithValue("?", keyBase + "%");
                            object val = cmd.ExecuteScalar();
                            if (val != null && val != DBNull.Value)
                                exactKey = val.ToString();
                        }

                        if (exactKey != null)
                        {
                            // Step 2a: UPDATE using the exact stored key value
                            string updateSql =
                                "UPDATE [" + CountersTable + "] SET v217 = ? WHERE v071 = ?";
                            using (var cmd = new OleDbCommand(updateSql, conn))
                            {
                                cmd.Parameters.AddWithValue("?", Globals.FL99_RECORD);
                                cmd.Parameters.AddWithValue("?", exactKey);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Step 2b: record does not exist yet — INSERT
                            // If a concurrent call already inserted it, fall back to UPDATE
                            string insertSql =
                                "INSERT INTO [" + CountersTable + "] (v071, v217) VALUES (?, ?)";
                            try
                            {
                                using (var cmd = new OleDbCommand(insertSql, conn))
                                {
                                    cmd.Parameters.AddWithValue("?", keyFull);
                                    cmd.Parameters.AddWithValue("?", Globals.FL99_RECORD);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (OleDbException oleEx) when (oleEx.Errors[0].SQLState == "3022")
                            {
                                // Duplicate key: another call beat us to the INSERT — UPDATE instead
                                string fallbackSql =
                                    "UPDATE [" + CountersTable + "] SET v217 = ? WHERE v071 LIKE ?";
                                using (var cmd = new OleDbCommand(fallbackSql, conn))
                                {
                                    cmd.Parameters.AddWithValue("?", Globals.FL99_RECORD);
                                    cmd.Parameters.AddWithValue("?", keyBase + "%");
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Illogical, formerly 1990 in Btrieve version!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Stores stringInhoud into FL99_RECORD and persists it via SetString99ForOleDb.
        /// </summary>
        internal static void SS99(string stringInhoud, int nummerRec)
        {
            Globals.FL99_RECORD = stringInhoud;
            SetString99(nummerRec);
        }

        /// <summary>
        /// Reads the v217 field of the setup counter record identified by "sNNN" and returns it.
        /// Returns an empty string when the record is not found or on error.
        /// </summary>
        internal static string String99(int szNummer)
        {
            // Key stored as TEXT(5): "sNNN " — use LIKE to match regardless of trailing spaces
            string key = "s" + szNummer.ToString("D3");

            try
            {
                using (var conn = new OleDbConnection(Globals.oleDbConnect))
                {
                    conn.Open();

                    string selectSql =
                        "SELECT v217 FROM [" + CountersTable + "] WHERE v071 LIKE ?";
                    using (var cmd = new OleDbCommand(selectSql, conn))
                    {
                        cmd.Parameters.AddWithValue("?", key + "%");
                        object val = cmd.ExecuteScalar();
                        return val == null || val == DBNull.Value
                            ? string.Empty
                            : val.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Tellers fout voor setup-tellersleutel " + key + "\r\n\r\n"
                    + "Controleer setup instellingen vooraleer op te starten of verder te werken!\r\n"
                    + "Wij staan tot uw beschikking om U hierbij te helpen.\r\n\r\n"
                    + ex.Message);
                return string.Empty;
            }
        }
    }
}
