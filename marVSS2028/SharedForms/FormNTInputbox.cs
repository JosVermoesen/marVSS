using System;
using System.Windows.Forms;
using static marVSS2028.Classes.Globals;

namespace marVSS2028.SharedForms
{
    public partial class FormNTInputbox : Form
    {
        // ── Public API used by MimEnvironment.VsfInputBox ─────────────────────

        /// <summary>The SQL prefix set by VsfInputBox before showing the dialog (e.g. "SELECT * FROM ISOLandKodes WHERE ISOLandNummer LIKE '").</summary>
        public string SQLBevel { get; set; } = string.Empty;

        /// <summary>Show / hide the lookup panel (Hernieuw + navigation + lblInfo).</summary>
        public bool LookupPanelVisible
        {
            set
            {
                Hernieuw.Visible  = value;
                BtnForward.Visible = value;
                BtnBack.Visible   = value;
                lblInfo.Visible   = value;
            }
        }

        public bool OkVisible        { set => Ok.Visible = value; }
        public bool SluitenIsDefault { set { if (value) { AcceptButton = Sluiten; } } }
        public bool OkIsDefault      { set { if (value) { AcceptButton = Ok; } } }

        /// <summary>The status bar text (VB6: MedeDeling.SimpleText).</summary>
        public string StatusText
        {
            get => _statusLabel?.Text ?? string.Empty;
            set { if (_statusLabel != null) _statusLabel.Text = value; }
        }

        /// <summary>The text in the input field (VB6: TekstInfo.text).</summary>
        public string InputText
        {
            get => TekstInfo.Text;
            set => TekstInfo.Text = value;
        }

        /// <summary>Returns a recordset field value by ordinal (VB6: DefaultData.Recordset(index)).</summary>
        public string GetRecordsetField(int fieldIndex)
        {
            try
            {
                if (_rs == null)
                    return string.Empty;

                object val = _rs.Fields[fieldIndex].Value;
                return (val == null || val is DBNull) ? string.Empty : val.ToString();
            }
            catch { return string.Empty; }
        }

        // ── Private state ──────────────────────────────────────────────────────

        private ADODB.Recordset _rs = null;
        private ToolStripStatusLabel _statusLabel;

        // ── Constructor ────────────────────────────────────────────────────────

        public FormNTInputbox()
        {
            InitializeComponent();

            // Add a ToolStripStatusLabel to the StatusStrip so we can write to it
            _statusLabel = new ToolStripStatusLabel { Spring = true, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            MedeDeling.Items.Add(_statusLabel);

            // Wire up events
            Ok.Click         += ButtonOk_Click;
            Sluiten.Click    += ButtonSluiten_Click;
            Hernieuw.Click   += ButtonHernieuw_Click;
            BtnForward.Click += ButtonVooruit_Click;
            BtnBack.Click    += ButtonAchteruit_Click;
            TekstInfo.GotFocus += TekstInfo_GotFocus;
            Activated        += FormNTInputbox_Activated;
        }

        // ── VB6: Form_Activate ─────────────────────────────────────────────────

        private void FormNTInputbox_Activated(object sender, EventArgs e)
        {
            // if (Hernieuw.Visible)
            //    ButtonHernieuw_Click(sender, e);
        }

        // ── VB6: Hernieuw_Click — open/refresh the ADODB recordset ────────────

        private void ButtonHernieuw_Click(object sender, EventArgs e)
        {
            try
            {
                // Close previous recordset if open
                if (_rs != null)
                {
                    try { _rs.Close(); } catch { }
                    _rs = null;
                }

                string sql = SQLBevel + TekstInfo.Text.TrimEnd() + "%';";
                StatusText = sql;

                string connStr = ADOJET_PROVIDER
                    + "Data Source=" + PROGRAM_LOCATION + @"MdX\default2022.mdb;"
                    + "Persist Security Info=False";

                _rs = new ADODB.Recordset { CursorLocation = ADODB.CursorLocationEnum.adUseClient };
                _rs.Open(
                    sql,
                    connStr,
                    ADODB.CursorTypeEnum.adOpenStatic,
                    ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText);

                if (!_rs.EOF)
                {
                    _rs.MoveFirst();
                    VernieuwInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij openen opzoektabel:\r\n" + ex.Message,
                    "FormNTInputbox", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── VB6: cmdVooruit_Click ──────────────────────────────────────────────

        private void ButtonVooruit_Click(object sender, EventArgs e)
        {
            if (_rs == null) return;
            try
            {
                _rs.MoveNext();
                if (_rs.EOF) _rs.MoveLast();
                VernieuwInfo();
            }
            catch { }
        }

        // ── VB6: cmdAchteruit_Click ────────────────────────────────────────────

        private void ButtonAchteruit_Click(object sender, EventArgs e)
        {
            if (_rs == null) return;
            try
            {
                _rs.MovePrevious();
                if (_rs.BOF) _rs.MoveFirst();
                VernieuwInfo();
            }
            catch { }
        }

        // ── VB6: VernieuwInfo — populate lblInfo and TekstInfo from current row ─

        private void VernieuwInfo()
        {
            if (_rs == null || _rs.EOF || _rs.BOF) return;

            // Derive case code from the SQL prefix
            string caseCode = "00";
            if (SQLBevel.Contains("PostKode LIKE"))       caseCode = "01";
            else if (SQLBevel.Contains("PlaatsNaam LIKE")) caseCode = "02";

            switch (caseCode)
            {
                case "00":
                    lblInfo.Text   = FieldStr("ISOLandNummer") + ", " + FieldStr("ISOLandkode")
                                   + ", " + FieldStr("ISOMuntKode") + ", " + FieldStr("LandNaam");
                    TekstInfo.Text = FieldStr("ISOLandNummer");
                    break;
                case "01":
                    lblInfo.Text   = FieldStr("PostKode") + ", " + FieldStr("PlaatsNaam");
                    TekstInfo.Text = FieldStr("PostKode");
                    break;
                case "02":
                    lblInfo.Text   = FieldStr("PostKode") + ", " + FieldStr("PlaatsNaam");
                    TekstInfo.Text = FieldStr("PlaatsNaam");
                    break;
            }
        }

        // ── VB6: Ok_Click ──────────────────────────────────────────────────────

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Hide();
        }

        // ── VB6: Sluiten_Click — cancel: set Chr$(255) sentinel ────────────────

        private void ButtonSluiten_Click(object sender, EventArgs e)
        {
            TekstInfo.Text = "\xFF";   // Chr$(255) = cancelled
            Hide();
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private void TekstInfo_GotFocus(object sender, EventArgs e)
        {
            TekstInfo.SelectAll();
        }

        private string FieldStr(string fieldName)
        {
            try
            {
                object val = _rs.Fields[fieldName].Value;
                return (val == null || val is DBNull) ? string.Empty : val.ToString();
            }
            catch { return string.Empty; }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            try { _rs?.Close(); } catch { }
            // Reset RecordSource sentinel so VsfInputBox_ProcessResult can detect no-lookup
            SQLBevel = string.Empty;
        }
    }
}

