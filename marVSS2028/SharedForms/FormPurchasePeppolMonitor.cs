using ADODB;
using marVSS2028.Classes;
using System;
using System.IO;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.PeppolTools;
using static marVSS2028.Classes.ShellHelper;
using static PeppolDocumentRepository;

namespace marVSS2028.SharedForms
{
    public partial class FormPurchasePeppolMonitor : Form
    {
        // Column index that holds the filename in both grids (0-based, matching VB6 Col=5 => index 5)
        private const int FileNameColIndex = 5;
        // Column index for the document number (VB6 Col=1 => index 1)
        private const int DocNumberColIndex = 1;

        private string _fileUrlAsPdf = string.Empty;
        private string _selectedFile = string.Empty;

        private Recordset _rsBuyerUBL;

        public FormPurchasePeppolMonitor()
        {
            InitializeComponent();
            _rsBuyerUBL = new Recordset();
        }

        // -------------------------------------------------------------------------
        // Button handlers
        // -------------------------------------------------------------------------

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            uitwisselingOMS = string.Empty;
            uitwisselingDATA = string.Empty;
            documentLinesOMS = string.Empty;
            documentLinesDATA = string.Empty;
            Close();
        }

        private void ButtonLoadDocument_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgToBook, FileNameColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            XLogKey = LOCATION_COMPANYDATA + @"peppol\in\" + selectedRowItem;

            string path = XLogKey;
            string documentId = Path.GetFileNameWithoutExtension(selectedRowItem);

            string storedHash = GetStoredHash(LOCATION_COMPANYDATA + "marnt.mdv", documentId);
            if (storedHash == null)
            {
                var repo = new PeppolDocumentRepository(LOCATION_COMPANYDATA + "marnt.mdv");                                
                repo.RegisterIncomingDocument(documentId, path);
                storedHash = GetStoredHash(LOCATION_COMPANYDATA + "marnt.mdv", documentId);
            }
            string currentHash = PeppolHashHelper.ComputeSha256(path);
            bool isUnchanged = string.Equals(storedHash, currentHash, StringComparison.OrdinalIgnoreCase);

            if (!isUnchanged)
            {
                MessageBox.Show(
                    "Het Peppol-document werd gewijzigd sinds ontvangst en kan niet automatisch verwerkt worden.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                // verder verwerken...
                Hide();
            }
        }

        private void ButtonResponsesToSeller_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgBooked, DocNumberColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            string result = GetSentReceipt(selectedRowItem.Substring(0, Math.Min(11, selectedRowItem.Length)), 2);
            if (result.Contains("\"count\": 0"))
            {
                MessageBox.Show("Geen", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var dlg = new FormReactionsDialog();
                dlg.TextBoxReactions.Text = result;
                dlg.Text = "Reacties aan leverancier";
                dlg.ShowDialog(this);
            }
        }

        private void ButtonSentReceiptSeller_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgBooked, DocNumberColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            string result = GetSentReceipt(selectedRowItem.Substring(0, Math.Min(11, selectedRowItem.Length)), 1);
            if (result.Contains("\"count\": 0"))
            {
                MessageBox.Show("Nog te bevestigen. Vernieuw met MarSync", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {                
                var dlg = new FormReactionsDialog();
                dlg.TextBoxReactions.Text = result;
                dlg.Text = "Ontvangstbewijs";
                dlg.ShowDialog(this);
            }
        }

        private void ButtonShowBookedXML_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgBooked, FileNameColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            _selectedFile = LOCATION_COMPANYDATA + @"peppol\in\" + selectedRowItem;
            bool result = NoPdfPeppolViewer(_selectedFile);
            if (result)
            {
                if (!ShellExecuteWithFallback(LOCATION_COMPANYDATA + @"peppol\in\invoiceNoPdf.html"))
                    MessageBox.Show("Kon bestand niet openen. Raadpleeg ShellHelper.log voor details.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Iets ging verkeerd", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonShowPeppolDocTypes_Click(object sender, EventArgs e)
        {
            string documentTypesPDF = PROGRAM_LOCATION + @"Content\Def\PeppolDocTypes.pdf";
            if (!File.Exists(documentTypesPDF))
            {
                MessageBox.Show(
                    "PDF niet gevonden in " + Environment.NewLine + documentTypesPDF +
                    Environment.NewLine + Environment.NewLine + "Controleer correcte installatie MarIntegraal",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!ShellExecuteWithFallback(documentTypesPDF))
                MessageBox.Show("Kon bestand niet openen. Raadpleeg ShellHelper.log voor details.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ButtonShowToBookPDF_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgToBook, FileNameColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            _selectedFile = LOCATION_COMPANYDATA + @"peppol\in\" + selectedRowItem;
            ExtractPdfFromUBLDocument(_selectedFile);
            _fileUrlAsPdf = _selectedFile.Substring(0, _selectedFile.Length - 3) + "pdf";

            if (!File.Exists(_fileUrlAsPdf))
            {
                MessageBox.Show(
                    "Er is geen PDF beschikbaar in " + Environment.NewLine + _selectedFile +
                    Environment.NewLine + Environment.NewLine + "Opteer eventueel voor XML tonen in MarSync",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!ShellExecuteWithFallback(_fileUrlAsPdf))
                    MessageBox.Show("Kon bestand niet openen. Raadpleeg ShellHelper.log voor details.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonShowToBookXML_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgToBook, FileNameColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            _selectedFile = LOCATION_COMPANYDATA + @"peppol\in\" + selectedRowItem;
            bool result = NoPdfPeppolViewer(_selectedFile);
            if (result)
            {
                if (!ShellExecuteWithFallback(LOCATION_COMPANYDATA + @"peppol\in\invoiceNoPdf.html"))
                    MessageBox.Show("Kon bestand niet openen. Raadpleeg ShellHelper.log voor details.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Iets ging verkeerd", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonShowBookedPDF_Click(object sender, EventArgs e)
        {
            string selectedRowItem = GetGridCellText(mfgBooked, FileNameColIndex);
            if (string.IsNullOrEmpty(selectedRowItem)) return;

            _selectedFile = LOCATION_COMPANYDATA + @"peppol\in\" + selectedRowItem;
            ExtractPdfAttachments(_selectedFile, LOCATION_COMPANYDATA + @"peppol\in\");
        }
                
        // -------------------------------------------------------------------------
        // Helper: read the text of the selected row at the given column index
        // -------------------------------------------------------------------------

        public void ResetToBookGrid()
        {
            ResetGrid(mfgToBook, new[] { "Leverancier", "Ondernemingsnummer", "Type Doc.", "Datum Doc", "Vervaldag", "Bestand" });
        }

        public void ResetBookedGrid()
        {
            ResetGrid(mfgBooked, new[] { "Leverancier", "Nummer Doc.", "Datum Doc", "Vervaldag", "Status", "Bestand" });
        }

        public void AddToBookRow(params string[] values)
        {
            AddGridRow(mfgToBook, values);
        }

        public void AddBookedRow(params string[] values)
        {
            AddGridRow(mfgBooked, values);
        }

        public int ToBookRowCount
        {
            get { return mfgToBook.Rows.Count; }
        }

        private void ResetGrid(DataGridView grid, string[] headers)
        {
            grid.Columns.Clear();
            grid.Rows.Clear();
            grid.AutoGenerateColumns = false;
            grid.RowHeadersVisible = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            foreach (string header in headers)
            {
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = header,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = true
                });
            }
        }

        private void AddGridRow(DataGridView grid, string[] values)
        {
            if (grid.Columns.Count == 0)
                return;

            object[] row = new object[grid.Columns.Count];
            for (int i = 0; i < row.Length; i++)
            {
                row[i] = i < values.Length ? values[i] : string.Empty;
            }
            grid.Rows.Add(row);
        }

        private string GetGridCellText(DataGridView grid, int colIndex)
        {
            if (grid.CurrentRow == null) return string.Empty;
            if (colIndex >= grid.Columns.Count) return string.Empty;
            object val = grid.CurrentRow.Cells[colIndex].Value;
            return val == null ? string.Empty : val.ToString().Trim();
        }

        // -------------------------------------------------------------------------
        // PDF extraction from UBL XML (replaces VB6 ExtractPdfFromUBLDocument)
        // Uses System.Xml and System.Convert instead of MSXML/ADODB.Stream
        // -------------------------------------------------------------------------

        private void ExtractPdfFromUBLDocument(string ublFileUrl)
        {
            if (!File.Exists(ublFileUrl)) return;

            try
            {
                var xmlDoc = new System.Xml.XmlDocument();
                var nsMgr = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);
                nsMgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                xmlDoc.Load(ublFileUrl);

                System.Xml.XmlNode node = xmlDoc.SelectSingleNode("//cbc:EmbeddedDocumentBinaryObject", nsMgr);
                if (node == null) return;

                string base64Data = node.InnerText.Trim();
                byte[] byteData = Convert.FromBase64String(base64Data);

                string pdfPath = ublFileUrl.Substring(0, ublFileUrl.Length - 4) + ".pdf";
                File.WriteAllBytes(pdfPath, byteData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wegschrijven PDF is mislukt." + Environment.NewLine + Environment.NewLine + ex.Message,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // -------------------------------------------------------------------------
        // GetSentReceipt (replaces VB6 function of same name)
        // -------------------------------------------------------------------------

        private string GetSentReceipt(string vDocument, int asSeller)
        {
            try
            {
                if (_rsBuyerUBL != null && _rsBuyerUBL.State != (int)ObjectStateEnum.adStateClosed)
                    _rsBuyerUBL.Close();

                _rsBuyerUBL = new Recordset();
                _rsBuyerUBL.CursorLocation = CursorLocationEnum.adUseClient;

                string sSQL =
                    "SELECT Dokumenten.v033, Dokumenten.v406, Dokumenten.v408 " +
                    "FROM Dokumenten " +
                    "WHERE Dokumenten.v033 = '" + vDocument + "'";

                Cursor.Current = Cursors.WaitCursor;
                _rsBuyerUBL.Open(sSQL, adntDB, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                Cursor.Current = Cursors.Default;

                if (_rsBuyerUBL.EOF) return "Niets gevonden";

                string sentID = string.Empty;
                if (asSeller == 1)
                    sentID = _rsBuyerUBL.Fields["v406"].Value?.ToString().Trim() ?? string.Empty;
                else if (asSeller == 2)
                    sentID = _rsBuyerUBL.Fields["v408"].Value?.ToString().Trim() ?? string.Empty;

                return string.IsNullOrEmpty(sentID) ? "Geen" : sentID;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(
                    "Bron:" + Environment.NewLine + ex.Source +
                    Environment.NewLine + Environment.NewLine + "Detail:" + Environment.NewLine + ex.Message);
                return ex.Message;
            }
        }

        // -------------------------------------------------------------------------
        // mfgToBook grid events
        // -------------------------------------------------------------------------
        
        private void mfgToBook_GotFocus(object sender, EventArgs e)
        {
            SetButtonStatesForToBookGrid();
        }
        
        private void mfgToBook_SelectionChanged(object sender, EventArgs e)
        {
            string fileId = GetGridCellText(mfgToBook, FileNameColIndex);
            if (string.IsNullOrEmpty(fileId))
            {
                ButtonLoadDocument.Enabled = false;
                return;
            }

            ButtonLoadDocument.Enabled = true;
            ButtonShowToBookPDF.Visible = PeppolHasPdfAttachment(LOCATION_COMPANYDATA + @"peppol\in\" + fileId);
        }

        private void SetButtonStatesForToBookGrid()
        {
            ButtonLoadDocument.Enabled = mfgToBook.Rows.Count > 0;
            ButtonShowToBookPDF.Enabled = mfgToBook.Rows.Count > 0;
            ButtonShowToBookXML.Enabled = mfgToBook.Rows.Count > 0;
        }

        // -------------------------------------------------------------------------
        // mfgBooked grid events
        // -------------------------------------------------------------------------
        private void mfgBooked_GotFocus(object sender, EventArgs e)
        {
            SetButtonStatesForBookedGrid();            
        }

        private void mfgBooked_SelectionChanged(object sender, EventArgs e)
        {
            string fileId = GetGridCellText(mfgBooked, FileNameColIndex);
            if (!string.IsNullOrEmpty(fileId))
            {
                ButtonShowBookedPDF.Visible = PeppolHasPdfAttachment(LOCATION_COMPANYDATA + @"peppol\in\" + fileId);
            }

            string docNumber = GetGridCellText(mfgBooked, DocNumberColIndex);
            if (string.IsNullOrEmpty(docNumber))
            {
                ButtonSentReceiptSeller.Visible = false;
                ButtonResponsesToSeller.Visible = false;
            }
            else
            {
                ButtonSentReceiptSeller.Visible = true;
                string result = GetSentReceipt(docNumber.Substring(0, Math.Min(11, docNumber.Length)), 2);
                ButtonResponsesToSeller.Visible = !result.Contains("\"count\": 0");
            }
        }        

        private void SetButtonStatesForBookedGrid()
        {
            ButtonShowBookedPDF.Enabled = mfgBooked.Rows.Count > 0;
            ButtonShowBookedXML.Enabled = mfgBooked.Rows.Count > 0;
            ButtonResponsesToSeller.Enabled = mfgBooked.Rows.Count > 0;
            ButtonSentReceiptSeller.Enabled = mfgBooked.Rows.Count > 0;
        }

        private void mfgToBook_Enter(object sender, EventArgs e)
        {
            if (mfgToBook.Rows.Count > 0)
            {
                this.AcceptButton = ButtonLoadDocument;
                mfgToBook.KeyDown -= mfgToBook_KeyDown;  // prevent double-wiring
                mfgToBook.KeyDown += mfgToBook_KeyDown;
            }
        }

        private void mfgToBook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && mfgToBook.Rows.Count > 0)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ButtonLoadDocument.PerformClick();
            }
        }

        private void mfgToBook_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = null;
            mfgToBook.KeyDown -= mfgToBook_KeyDown;
        }
    }
}
