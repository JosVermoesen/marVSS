using System;
using System.Data.OleDb;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;

namespace marVSS2028.MimMenu.Filing
{
    public partial class FormProductGroups : Form
    {
        // ── Table / DDL ───────────────────────────────────────────────────────
        private const string TableName =
            "p_Groepen";

        private const string CreateDdl =
            "CREATE TABLE p_Groepen " +
            "( ID int IDENTITY (1,1), " +
            "  GroepsNaam varchar(60) UNIQUE, " +
            "  GroepItems MEMO, " +
            "  CONSTRAINT p_PK PRIMARY KEY (ID) )";

        private const string CreateIndex =
            "CREATE UNIQUE INDEX GroepsNaam ON p_Groepen (GroepsNaam)";

        // ── State: currently selected group name (used for Seek equivalent) ──
        private string _currentGroepNaam = "";

        public FormProductGroups()
        {
            InitializeComponent();
        }

        // ═════════════════════════════════════════════════════════════════════
        // Form events
        // ═════════════════════════════════════════════════════════════════════

        private void FormProductGroups_Load(object sender, EventArgs e)
        {
            EnsureTableExists();
            RefreshGroepen();
        }

        private void FormProductGroups_FormClosed(object sender, FormClosedEventArgs e)
        {
            // nothing to release — connections are opened/closed per-operation
        }

        // ═════════════════════════════════════════════════════════════════════
        // Button handlers
        // ═════════════════════════════════════════════════════════════════════

        private void BtnItemsWijzigen_Click(object sender, EventArgs e)
        {
            SetKnoppen(false);
        }

        private void BtnGroepToevoegen_Click(object sender, EventArgs e)
        {
            string naam = CbGroepDefinitie.Text.Trim();
            if (naam.Length == 0) return;

            if (MessageBox.Show(
                    "Nieuwe groep " + naam + " bijvoegen.  Bent U zeker",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand(
                        "INSERT INTO " + TableName + " (GroepsNaam, GroepItems) VALUES (?, ?)", conn))
                    {
                        cmd.Parameters.AddWithValue("?", naam);
                        cmd.Parameters.AddWithValue("?", "");
                        cmd.ExecuteNonQuery();
                    }
                }
                RefreshGroepen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGroepItemToevoegen_Click(object sender, EventArgs e)
        {
            if (TbGroepItem.Text.Length == 0) return;

            int idx = LbGroepItems.SelectedIndex;
            if (idx < 0)
                LbGroepItems.Items.Add(TbGroepItem.Text);
            else
                LbGroepItems.Items.Insert(idx, TbGroepItem.Text);

            TbGroepItem.Text = "";
            TbGroepItem.Focus();
        }

        private void BtnBewaren_Click(object sender, EventArgs e)
        {
            string groepItems = "";
            foreach (object item in LbGroepItems.Items)
                groepItems += item.ToString() + ";";

            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand(
                        "UPDATE " + TableName + " SET GroepItems = ? WHERE GroepsNaam = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("?", groepItems);
                        cmd.Parameters.AddWithValue("?", _currentGroepNaam);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            SetKnoppen(true);
        }

        private void BtnSluiten_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ═════════════════════════════════════════════════════════════════════
        // ComboBox
        // ═════════════════════════════════════════════════════════════════════

        private void CbGroepDefinitie_SelectedIndexChanged(object sender, EventArgs e)
        {
            LbGroepItems.Items.Clear();

            string naam = CbGroepDefinitie.SelectedItem?.ToString() ?? "";
            if (naam.Length == 0) return;

            _currentGroepNaam = naam;

            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand(
                        "SELECT GroepItems FROM " + TableName + " WHERE GroepsNaam = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("?", naam);
                        object val = cmd.ExecuteScalar();
                        if (val == null || val == DBNull.Value) return;

                        string raw = val.ToString();
                        string[] parts = raw.Split(';');
                        for (int i = 0; i < parts.Length - 1; i++) // last element after trailing ';' is empty
                            LbGroepItems.Items.Add(parts[i]);
                    }
                }
            }
            catch { /* ignore — list stays empty */ }
        }

        // ═════════════════════════════════════════════════════════════════════
        // ListBox keyboard: Delete key removes selected item
        // ═════════════════════════════════════════════════════════════════════

        private void LbGroepItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && LbGroepItems.SelectedIndex >= 0)
            {
                LbGroepItems.Items.RemoveAt(LbGroepItems.SelectedIndex);
                e.Handled = true;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        // Helpers
        // ═════════════════════════════════════════════════════════════════════

        private void EnsureTableExists()
        {
            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();
                    // Try to create the table; if it already exists the engine throws — we ignore that.
                    try
                    {
                        using (var cmd = new OleDbCommand(CreateDdl, conn))
                            cmd.ExecuteNonQuery();

                        // Table was just created — seed with defaults
                        using (var cmd = new OleDbCommand(
                            "INSERT INTO " + TableName + " (GroepsNaam, GroepItems) VALUES (?, ?)", conn))
                        {
                            cmd.Parameters.Add("@n", OleDbType.VarChar);
                            cmd.Parameters.Add("@v", OleDbType.VarChar);

                            cmd.Parameters["@n"].Value = "Categorie";
                            cmd.Parameters["@v"].Value = "zonder voorwerp";
                            cmd.ExecuteNonQuery();

                            cmd.Parameters["@n"].Value = "Merk";
                            cmd.Parameters["@v"].Value = "zonder voorwerp";
                            cmd.ExecuteNonQuery();
                        }

                        try
                        {
                            using (var cmd = new OleDbCommand(CreateIndex, conn))
                                cmd.ExecuteNonQuery();
                        }
                        catch { /* index may already exist */ }
                    }
                    catch { /* table already exists — nothing to do */ }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshGroepen()
        {
            CbGroepDefinitie.Items.Clear();

            try
            {
                using (var conn = new OleDbConnection(adntDB.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand(
                        "SELECT GroepsNaam FROM " + TableName + " ORDER BY GroepsNaam", conn))
                    using (var rs = cmd.ExecuteReader())
                    {
                        while (rs.Read())
                            CbGroepDefinitie.Items.Add(rs["GroepsNaam"].ToString());
                    }
                }
            }
            catch { /* table may not exist yet */ }

            if (CbGroepDefinitie.Items.Count > 0)
                CbGroepDefinitie.SelectedIndex = 0;
        }

        /// <summary>
        /// VB6 Knoppen(Vlag): True = view mode (combo/wijzigen enabled), False = edit mode.
        /// </summary>
        private void SetKnoppen(bool vlag)
        {
            BtnGroepToevoegen.Enabled     = vlag;
            CbGroepDefinitie.Enabled      = vlag;
            BtnItemsWijzigen.Enabled      = vlag;

            LbGroepItems.Enabled          = !vlag;
            TbGroepItem.Enabled           = !vlag;
            BtnGroepItemToevoegen.Enabled = !vlag;
            BtnBewaren.Enabled            = !vlag;
        }
    }
}

