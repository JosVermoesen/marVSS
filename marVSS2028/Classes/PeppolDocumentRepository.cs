using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class PeppolDocumentRepository
{
    private readonly string _connectionString;

    public void EnsurePeppolTableExists(string mdbPath)
    {
        string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={mdbPath};";
        using (var conn = new OleDbConnection(connStr))
        {
            conn.Open();

            // Check of tabel bestaat
            DataTable tables = conn.GetSchema("Tables");
            bool exists = false;

            foreach (DataRow row in tables.Rows)
            {
                object tableNameValue = row["TABLE_NAME"];
                if (tableNameValue != null && tableNameValue != DBNull.Value &&
                    string.Equals(tableNameValue.ToString(), "PeppolDocuments", StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                string ddl = @"
                CREATE TABLE PeppolDocuments
                (
                    DocumentId      TEXT(100)    NOT NULL,
                    FilePath        TEXT(255),
                    HashSha256      TEXT(64),
                    ReceivedOn      DATETIME,
                    CONSTRAINT PK_PeppolDocuments PRIMARY KEY (DocumentId)
                );
            ";

                using (var cmd = new OleDbCommand(ddl, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public PeppolDocumentRepository(string mdbPath)
    {
        // For classic .mdb (Jet)
        _connectionString =
            $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={mdbPath};Persist Security Info=False;";
        // For .accdb you should use ACE:
        // Provider=Microsoft.ACE.OLEDB.12.0;
    }

    public static class PeppolHashHelper
    {
        public static string ComputeSha256(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(stream);
                var sb = new StringBuilder(hashBytes.Length * 2);
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2")); // hex string

                return sb.ToString();
            }
        }
    }

    public static string GetStoredHash(string mdbPath, string documentId)
    {
        string resolvedMdbPath = mdbPath;
        if (Directory.Exists(resolvedMdbPath))
            resolvedMdbPath = Path.Combine(resolvedMdbPath, "marnt.mdv");

        string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={resolvedMdbPath};";
        string storedHash = null;

        using (var conn = new OleDbConnection(connStr))
        {
            conn.Open();

            string sql = "SELECT HashSha256 FROM PeppolDocuments WHERE DocumentId = ?";
            using (var cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@p1", (documentId ?? string.Empty).Trim());

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    storedHash = result.ToString();
            }
        }

        return storedHash;
    }

    // ---------- HASH BEREKENEN ----------
    public string ComputeSha256(string filePath)
    {
        using (var stream = File.OpenRead(filePath))
        using (var sha = SHA256.Create())
        {
            var hashBytes = sha.ComputeHash(stream);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    // ---------- REGISTREREN BIJ ONTVANGST ----------
    public void RegisterIncomingDocument(string documentId, string filePath)
    {
        string hash = ComputeSha256(filePath);

        using (var conn = new OleDbConnection(_connectionString))
        {
            conn.Open();

            // Eerst kijken of het document al bestaat
            string checkSql = "SELECT COUNT(*) FROM PeppolDocuments WHERE DocumentId = ?";
            using (var checkCmd = new OleDbCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@p1", documentId);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                string sql;
                if (count == 0)
                {
                    // Insert
                    sql = @"INSERT INTO PeppolDocuments
                            (DocumentId, FilePath, HashSha256, ReceivedOn)
                            VALUES (?, ?, ?, ?)";
                }
                else
                {
                    // Update (bijvoorbeeld bij herontvangst)
                    sql = @"UPDATE PeppolDocuments
                            SET FilePath = ?, HashSha256 = ?, ReceivedOn = ?
                            WHERE DocumentId = ?";
                }

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    if (count == 0)
                    {
                        cmd.Parameters.AddWithValue("@p1", documentId);
                        cmd.Parameters.AddWithValue("@p2", filePath);
                        cmd.Parameters.AddWithValue("@p3", hash);
                        var dateParamInsert = cmd.Parameters.Add("@p4", OleDbType.Date);
                        dateParamInsert.Value = DateTime.Now;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@p1", filePath);
                        cmd.Parameters.AddWithValue("@p2", hash);
                        var dateParamUpdate = cmd.Parameters.Add("@p3", OleDbType.Date);
                        dateParamUpdate.Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@p4", documentId);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
