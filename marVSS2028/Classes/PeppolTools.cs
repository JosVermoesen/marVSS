using marVSS2028.SharedForms;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.ShellHelper;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.Classes
{
    internal static class PeppolTools
    {
        // ?? XML helper ??????????????????????????????????????????????????????????

        private static string GetNodeText(XmlNode parentNode, string xpath)
        {
            XmlNode node;
            XmlDocument xmlDoc = parentNode as XmlDocument ?? parentNode.OwnerDocument;

            if (xmlDoc != null)
            {
                var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                node = parentNode.SelectSingleNode(xpath, nsmgr);
            }
            else
            {
                node = parentNode.SelectSingleNode(xpath);
            }

            return node != null ? node.InnerText.Trim() : "";
        }

        // ?? File helpers ????????????????????????????????????????????????????????

        private static string MarReadUtf8File(string fileName)
        {
            if (!File.Exists(fileName)) return "";
            return File.ReadAllText(fileName, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        private static void MarWriteUtf8File(string fileName, string text)
        {
            File.WriteAllText(fileName, text, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        // ?? GetBbaDescription ???????????????????????????????????????????????????

        public static string GetBbaDescription(string bbaCode)
        {
            switch (bbaCode)
            {
                // SEPA Credit Transfers
                case "0101000": return "SEPA Credit Transfer (individual payment)";
                case "0102000": return "SEPA Credit Transfer (urgent)";
                case "0103000": return "SEPA Credit Transfer (international)";

                // SEPA Direct Debits
                case "0107000": return "SEPA Direct Debit (batch debit)";
                case "0108000": return "SEPA Direct Debit (individual debit)";
                case "0501000": return "SEPA Direct Debit CORE";
                case "0502000": return "SEPA Direct Debit B2B";

                // Card Payments
                case "0401000": return "Card Payment (Bancontact/Maestro)";
                case "0402000": return "ATM Cash Withdrawal";
                case "0403000": return "Credit Card Settlement";

                // Cheques
                case "0301000": return "Cheque Deposit";
                case "0302000": return "Cheque Payment";
                case "0307000": return "Unpaid Cheque";

                // Cash / Counter
                case "0901000": return "Cash Deposit";
                case "0902000": return "Cash Withdrawal";

                default: return "Unknown BBA Code (" + bbaCode + ")";
            }
        }

        // ?? DetectTransactionType ???????????????????????????????????????????????

        public static string DetectTransactionType(string bbaCode, string scor, string ustrd, string creditor, string debtor)
        {
            if (bbaCode.Length >= 2)
            {
                switch (bbaCode.Substring(0, 2))
                {
                    case "01":
                        return scor != "" ? "SEPA Transfer with Structured Communication" : "SEPA Transfer";
                    case "04":
                        return ustrd.IndexOf("Kaart", StringComparison.Ordinal) >= 0 ? "Card Payment" : "ATM Withdrawal";
                    case "05": return "Direct Debit";
                    case "02": return "Salary / Income / Incoming Transfer";
                    case "03": return "Cheque Operation";
                    case "09": return "Cash Operation";
                }
            }

            if (scor != "") return "Structured Payment";
            if (ustrd.IndexOf("Kaart", StringComparison.Ordinal) >= 0) return "Card Payment";
            if (creditor != "" && debtor == "") return "Outgoing Payment";
            if (debtor != "" && creditor == "") return "Incoming Payment";

            return "Unknown Transaction Type";
        }

        // ?? ReadCamt053XDA ??????????????????????????????????????????????????????

        public static bool ReadCamt053XDA(string fileName, bool showResult)
        {
            xdaOMS = "";
            xdaDATA = "";
            xdaLinesOMS = "";
            xdaLinesDATA = "";

            string txt = MarReadUtf8File(fileName);
            txt = txt.Replace("xmlns=\"urn:iso:std:iso:20022:tech:xsd:camt.053.001.02\"", "");

            var xml = new XmlDocument();
            try { xml.LoadXml(txt); }
            catch (XmlException ex)
            {
                MessageBox.Show("XML Parse Error: " + ex.Message);
                return false;
            }

            // Top-level info
            string msgId = xml.SelectSingleNode("//MsgId")?.InnerText ?? "";
            string stmtId = xml.SelectSingleNode("//Stmt/Id")?.InnerText ?? "";
            string elctrncSeqNb = GetNodeText(xml, "//Stmt/ElctrncSeqNb");
            string lglSeqNb = GetNodeText(xml, "//Stmt/LglSeqNb");
            string iban = xml.SelectSingleNode("//Acct/Id/IBAN")?.InnerText ?? "";
            string owner = xml.SelectSingleNode("//Acct/Ownr/Nm")?.InnerText ?? "";

            // Balances
            string openingBal = xml.SelectSingleNode("//Bal[Tp/CdOrPrtry/Cd='OPBD']/Amt")?.InnerText ?? "";
            string closingBal = xml.SelectSingleNode("//Bal[Tp/CdOrPrtry/Cd='CLBD']/Amt")?.InnerText ?? "";

            string result = "";

            xdaOMS += "MessageID\t"; xdaDATA += msgId + "\t";
            result += "MessageID: " + msgId + "\r\n";

            xdaOMS += "StatementID\t"; xdaDATA += stmtId + "\t";
            result += "StatementID: " + stmtId + "\r\n";

            xdaOMS += "ElectronicSeq\t"; xdaDATA += elctrncSeqNb + "\t";
            result += "Electronic Seq: " + elctrncSeqNb + "\r\n";

            xdaOMS += "LegalSeq\t"; xdaDATA += lglSeqNb + "\t";
            result += "Legal Sequence: " + lglSeqNb + "\r\n";

            xdaOMS += "IBAN\t"; xdaDATA += iban + "\t";
            result += "IBAN: " + iban + "\r\n";

            xdaOMS += "Owner\t"; xdaDATA += owner + "\t";
            result += "Owner: " + owner + "\r\n\r\n";

            xdaOMS += "OpeningBalance\t"; xdaDATA += openingBal + "\t";
            result += "Opening Balance: " + openingBal + "\r\n";

            xdaOMS += "ClosingBalance"; xdaDATA += closingBal;
            result += "Closing Balance: " + closingBal + "\r\n\r\n";

            result += "*** ALL TRANSACTIONS ***\r\n";

            string skipString = " - \t - \t - \t - \t - \t - \t - \t - \t - \t - \t - ";

            xdaLinesOMS = "Entry Ref\tEntry Amount\tEntry BBA Code\t";
            xdaLinesOMS += "Tx Ref\tTx Amount\tTx Creditor\tTx Debtor\tTx IBAN\tTx BIC\tTx SCOR\tTx Ustrd\tTx BBA Code\tTx Description\tTx Type";

            foreach (XmlNode nEntry in xml.SelectNodes("//Ntry"))
            {
                string entryRef = GetNodeText(nEntry, "NtryRef");
                string entryAmount = GetNodeText(nEntry, "Amt");
                string entryBBACode = GetNodeText(nEntry, "BkTxCd/Prtry/Cd");

                XmlNodeList txList = nEntry.SelectNodes("NtryDtls/TxDtls");
                if (txList == null || txList.Count == 0)
                {
                    xdaLinesDATA += skipString + "\r\n";
                    continue;
                }

                foreach (XmlNode nTx in txList)
                {
                    result += "---------------\r\n";
                    result += "EntryRef: " + entryRef + "\r\n"; xdaLinesDATA += entryRef + "\t";
                    result += "Entry Amount: " + entryAmount + "\r\n"; xdaLinesDATA += entryAmount + "\t";
                    result += "Entry BBA Code: " + entryBBACode + "\r\n"; xdaLinesDATA += entryBBACode + "\t";

                    string txCode = GetNodeText(nTx, "BkTxCd/Prtry/Cd");
                    if (txCode == "") txCode = GetNodeText(nEntry, "BkTxCd/Prtry/Cd");
                    string txDesc = GetBbaDescription(txCode);
                    string txRef = GetNodeText(nTx, "Refs/AcctSvcrRef");
                    string txAmount = GetNodeText(nTx, "AmtDtls/TxAmt/Amt");
                    string txCreditor = GetNodeText(nTx, "RltdPties/Cdtr/Nm");
                    string txDebtor = GetNodeText(nTx, "RltdPties/Dbtr/Nm");

                    string txIBAN = GetNodeText(nTx, "RltdPties/CdtrAcct/Id/IBAN");
                    if (txIBAN == "") txIBAN = GetNodeText(nTx, "RltdPties/DbtrAcct/Id/IBAN");

                    string txBIC = GetNodeText(nTx, "RltdAgts/CdtrAgt/FinInstnId/BIC");
                    if (txBIC == "") txBIC = GetNodeText(nTx, "RltdAgts/DbtrAgt/FinInstnId/BIC");

                    string txSCOR = GetNodeText(nTx, "RmtInf/Strd/CdtrRefInf/Ref");
                    string txUstrd = "";
                    foreach (XmlNode nU in nTx.SelectNodes("RmtInf/Ustrd"))
                    {
                        if (txUstrd != "") txUstrd += " | ";
                        txUstrd += nU.InnerText;
                    }

                    string txType = DetectTransactionType(txCode, txSCOR, txUstrd, txCreditor, txDebtor);

                    result += "--------- TxDtls ---------\r\n";
                    result += " -TxRef: " + txRef + "\r\n"; xdaLinesDATA += txRef + "\t";
                    result += " -TxAmount: " + txAmount + "\r\n"; xdaLinesDATA += txAmount + "\t";
                    result += " -Creditor: " + txCreditor + "\r\n"; xdaLinesDATA += txCreditor + "\t";
                    result += " -Debtor: " + txDebtor + "\r\n"; xdaLinesDATA += txDebtor + "\t";
                    result += " -IBAN: " + txIBAN + "\r\n"; xdaLinesDATA += txIBAN + "\t";
                    result += " -BIC: " + txBIC + "\r\n"; xdaLinesDATA += txBIC + "\t";
                    result += " -SCOR: " + txSCOR + "\r\n"; xdaLinesDATA += txSCOR + "\t";
                    result += " -Ustrd: " + txUstrd + "\r\n"; xdaLinesDATA += txUstrd + "\t";
                    result += " -BBA Code: " + txCode + "\r\n"; xdaLinesDATA += txCode + "\t";
                    result += " -Description: " + txDesc + "\r\n"; xdaLinesDATA += txDesc + "\t";
                    result += " -Tx Type: " + txType + "\r\n"; xdaLinesDATA += txType + "\r\n";
                }
            }

            if (showResult)
            {                
                var dlg = new FormReactionsDialog();
                dlg.TextBoxReactions.Text = result;
                dlg.Text = "SEPA Viewer - " + Path.GetFileName(fileName);
                dlg.ShowDialog();                
            }
            return true;
        }

        // ?? CheckforAmp ?????????????????????????????????????????????????????????

        public static string CheckforAmp(string toCheck)
        {
            return toCheck.Contains("&") ? toCheck.Replace("&", "&amp;") : toCheck;
        }

        // ?? GetCreationDateTime ??????????????????????????????????????????????????

        public static string GetCreationDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss");
        }

        // ?? CreateGUID ???????????????????????????????????????????????????????????

        public static string CreateGUID()
        {
            return "{" + Guid.NewGuid().ToString().ToUpper() + "}";
        }

        // ?? NoPdfPeppolViewer ????????????????????????????????????????????????????

        public static bool NoPdfPeppolViewer(string filePath)
        {
            string utf8Text = MarReadUtf8File(filePath);
            if (string.IsNullOrEmpty(utf8Text)) return false;

            var xml = new XmlDocument();
            try { xml.LoadXml(utf8Text); }
            catch (XmlException ex)
            {
                MessageBox.Show("XML parse error: " + ex.Message);
                return false;
            }

            var xsl = new System.Xml.Xsl.XslCompiledTransform();
            string xsltPath = PROGRAM_LOCATION + @"Content\xml-templates\peppol\peppol-invoice.xslt";
            try { xsl.Load(xsltPath); }
            catch (Exception ex)
            {
                MessageBox.Show("XSLT error: " + ex.Message);
                return false;
            }

            string outputPath = LOCATION_COMPANYDATA + @"peppol\in\invoiceNoPdf.html";
            using (var sw = new StringWriter())
            {
                xsl.Transform(xml, null, sw);
                MarWriteUtf8File(outputPath, sw.ToString());
            }

            return true;
        }

        // ?? PeppolHasPdfAttachment ???????????????????????????????????????????????

        public static bool PeppolHasPdfAttachment(string xmlPath)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(xmlPath);

            var nsmgr = new XmlNamespaceManager(xDoc.NameTable);
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");

            XmlNodeList nodeList = xDoc.SelectNodes(
                "//cac:AdditionalDocumentReference/cac:Attachment/cbc:EmbeddedDocumentBinaryObject", nsmgr);
            if (nodeList == null) return false;

            foreach (XmlNode node in nodeList)
            {
                string mime = (node.Attributes?["mimeCode"]?.Value ?? "").ToLower();
                if (mime == "application/pdf") return true;
            }

            return false;
        }

        // ?? ReadUblDocument ??????????????????????????????????????????????????????

        public static void ReadUblDocument(string filePath, bool showMessageBox, bool forBooking)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            uitwisselingOMS = "";
            uitwisselingDATA = "";

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                        
            string sb = "";
            string valueToUse;
            string documentId;
            string msg;

            // UBL VersionID
            uitwisselingOMS = "UBL VersionID";
            uitwisselingDATA = GetNodeText(xmlDoc, "//cbc:UBLVersionID");
            sb += "UBL VersionID: " + GetNodeText(xmlDoc, "//cbc:UBLVersionID") + "\r\n";

            // Document ID
            valueToUse = GetNodeText(xmlDoc, "//cbc:ID");
            documentId = valueToUse;
            uitwisselingOMS += "\tdocumentIdToCheck";
            uitwisselingDATA += "\t" + valueToUse;
            sb += "Document ID: " + valueToUse + "\r\n";

            // IssueDate
            valueToUse = GetNodeText(xmlDoc, "//cbc:IssueDate");
            uitwisselingOMS += "\tdateSellerDocumentToCheck";
            uitwisselingDATA += "\t" + valueToUse;
            sb += "IssueDate: " + valueToUse + "\r\n";

            // DueDate
            valueToUse = GetNodeText(xmlDoc, "//cbc:DueDate");
            uitwisselingOMS += "\tdateExpiringDocumentToCheck";
            uitwisselingDATA += "\t" + valueToUse;
            sb += "DueDate: " + valueToUse + "\r\n";

            // DocumentTypeCode � try InvoiceTypeCode first, then CreditNoteTypeCode
            bool isInvoiceToCheck = true;
            XmlNode invTypeNode = xmlDoc.SelectSingleNode("//cbc:InvoiceTypeCode", nsmgr);
            if (invTypeNode == null)
                invTypeNode = xmlDoc.SelectSingleNode("//cbc:CreditNoteTypeCode", nsmgr);

            if (invTypeNode != null)
            {
                double.TryParse(invTypeNode.InnerText, out double typeVal);
                string codeFormatted = Dec(typeVal, "000");
                switch (codeFormatted)
                {
                    case "071":
                    case "084":
                    case "380":
                    case "386":
                    case "575":
                        isInvoiceToCheck = true; break;
                    case "381":
                        isInvoiceToCheck = false; break;
                    default:
                        MessageBox.Show(
                            "Onbekende verwerkingscode " + codeFormatted + "\r\n\r\n" +
                            "Bezorg ons het document. Dank voor medewerking",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }

                uitwisselingOMS += "\tdocumentTypeCode";
                uitwisselingDATA += "\t" + codeFormatted;
                sb += "documentTypeCode: " + invTypeNode.InnerText + "\r\n";

                if (invTypeNode.Attributes != null)
                {
                    string listID = invTypeNode.Attributes["listID"]?.Value ?? "";
                    if (listID == "") listID = "not found";
                    sb += "document listID: " + listID + "\r\n";
                }
            }

            // OrderReference
            XmlNodeList orderList = xmlDoc.SelectNodes("//cac:OrderReference", nsmgr);
            if (orderList != null)
            {
                for (int i = 0; i < orderList.Count; i++)
                {
                    string orderId = GetNodeText(orderList[i], "cbc:ID");
                    if (orderId == "") orderId = "Order ID: not available";
                    sb += "Order ID: " + orderId + "\r\n";
                }
            }
            if (showMessageBox)
                MessageBox.Show(sb, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Supplier info
            string tmpSupplierId = string.Empty;
            string tmpSupplierName = string.Empty;

            XmlNode supplierNode = xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party", nsmgr);
            if (supplierNode != null)
            {
                msg = "Supplier info\r\n-------------\r\n";

                valueToUse = GetNodeText(supplierNode, "cbc:EndpointID");
                if (valueToUse.Length == 12 && valueToUse.Contains("BE"))
                    valueToUse = valueToUse.Substring(2);
                uitwisselingOMS += "\tsupplierCompanyIdToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                
                msg += "endpointOndernemingsnummer " + valueToUse + "\r\n";

                uitwisselingOMS += "\tsupplierID";
                uitwisselingDATA += "\t" + GetNodeText(supplierNode, "cac:PartyIdentification/cbc:ID");
                msg += "supplierID: " + GetNodeText(supplierNode, "cac:PartyIdentification/cbc:ID") + "\r\n";
                tmpSupplierId = valueToUse;

                valueToUse = GetNodeText(supplierNode, "cac:PartyLegalEntity/cbc:RegistrationName");
                uitwisselingOMS += "\tsupplierNameToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "tradingName: " + valueToUse + "\r\n";
                tmpSupplierName = valueToUse;

                valueToUse = GetNodeText(supplierNode, "cac:PostalAddress/cbc:StreetName");
                uitwisselingOMS += "\tsupplierStreetToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "street: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(supplierNode, "cac:PostalAddress/cbc:CityName");
                uitwisselingOMS += "\tsupplierCityToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "city: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(supplierNode, "cac:PostalAddress/cbc:PostalZone");
                uitwisselingOMS += "\tsupplierPostalCodeToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "postalZone: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(supplierNode, "cac:PostalAddress/cac:Country/cbc:IdentificationCode");
                uitwisselingOMS += "\tsupplierCountryCodeToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "countryCode: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(supplierNode, "cac:PartyTaxScheme/cbc:CompanyID");
                uitwisselingOMS += "\tsupplierVatNumberToCheck";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "vatNumber: " + valueToUse + "\r\n";

                if (showMessageBox)
                    MessageBox.Show(msg, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No AccountingSupplierParty element found.");
            }

            // PaymentMeans
            XmlNodeList pmNodes = xmlDoc.SelectNodes("//cac:PaymentMeans", nsmgr);
            if (pmNodes != null && pmNodes.Count > 0)
            {
                msg = "PaymentMeans\r\n------------\r\n";
                for (int i = 0; i < pmNodes.Count; i++)
                {
                    XmlNode pmNode = pmNodes[i];

                    uitwisselingOMS += "\tpaymentMeansCode";
                    uitwisselingDATA += "\t" + GetNodeText(pmNode, "cbc:PaymentMeansCode");
                    msg += "PaymentMeansCode: " + GetNodeText(pmNode, "cbc:PaymentMeansCode") + "\r\n";

                    valueToUse = GetNodeText(pmNode, "cbc:PaymentID");
                    if (valueToUse == "") valueToUse = documentId;
                    if (valueToUse.Contains("+") || valueToUse.Contains("/") || valueToUse.Contains(" "))
                        valueToUse = valueToUse.Replace("+", "").Replace("/", "").Replace(" ", "");
                    uitwisselingOMS += "\tpayReferenceToCheck";
                    uitwisselingDATA += "\t" + valueToUse;
                    msg += "PaymentID: " + valueToUse + "\r\n";

                    valueToUse = GetNodeText(pmNode, "cac:PayeeFinancialAccount/cbc:ID");
                    uitwisselingOMS += "\tsupplierIBANToCheck";
                    uitwisselingDATA += "\t" + valueToUse;
                    msg += "Payee IBAN: " + valueToUse + "\r\n";

                    valueToUse = GetNodeText(pmNode, "cac:PayeeFinancialAccount/cbc:Name");
                    uitwisselingOMS += "\tpaySupplierNameToCheck";
                    uitwisselingDATA += "\t" + valueToUse;
                    msg += "Account Name: " + valueToUse + "\r\n";

                    valueToUse = GetNodeText(pmNode, "cac:PayeeFinancialAccount/cac:FinancialInstitutionBranch/cbc:ID");
                    uitwisselingOMS += "\tsupplierBICToCheck";
                    uitwisselingDATA += "\t" + valueToUse;
                    msg += "BIC/Branch ID: " + valueToUse + "\r\n";

                    msg += "\r\nCard account (if present)\r\n";
                    msg += "Card Account ID: " + GetNodeText(pmNode, "cac:CardAccount/cbc:ID") + "\r\n";
                    msg += "Card Account Name: " + GetNodeText(pmNode, "cac:CardAccount/cbc:Name") + "\r\n";
                    msg += "\r\nDirect debit mandate (if present)\r\n";

                    valueToUse = GetNodeText(pmNode, "cac:PaymentMandate/cbc:ID");
                    uitwisselingOMS += "\tsupplierDomMandate";
                    uitwisselingDATA += "\t" + valueToUse;
                    msg += "Mandate ID: " + valueToUse + "\r\n";

                    valueToUse = GetNodeText(pmNode, "cac:PaymentMandate/cbc:PaymentMandateDate");
                    uitwisselingOMS += "\tsupplierDomDate";
                    uitwisselingDATA += "\t" + valueToUse;
                    msg += "Mandate Date: " + valueToUse + "\r\n";
                }
                if (showMessageBox)
                    MessageBox.Show(msg, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                uitwisselingOMS += "\tpaymentMeansCode\tpayReferenceToCheck\tsupplierIBANToCheck\tpaySupplierNameToCheck\tsupplierBICToCheck\tsupplierDomMandate\tsupplierDomDate";
                uitwisselingDATA += "\t\t\t\t\t\t\t";
            }

            // TaxTotal
            string msgTax = "TaxTotal\r\n--------\r\n";
            XmlNode taxAmountEl = xmlDoc.SelectSingleNode("//cac:TaxTotal/cbc:TaxAmount", nsmgr);
            string currencyID = taxAmountEl?.Attributes?["currencyID"]?.Value ?? "";
            if (taxAmountEl != null && currencyID == "")
                MessageBox.Show("Attribute currencyID is missing on <cbc:TaxAmount>");

            XmlNodeList taxTotals = xmlDoc.SelectNodes("//cac:TaxTotal", nsmgr);
            if (taxTotals != null)
            {
                for (int i = 0; i < taxTotals.Count; i++)
                {
                    XmlNode taxTotalElem = taxTotals[i];
                    msgTax += "TaxTotal: " + GetNodeText(taxTotalElem, "cbc:TaxAmount") + " " + currencyID + "\r\n";

                    XmlNodeList subtotals = taxTotalElem.SelectNodes("cac:TaxSubtotal", nsmgr);
                    for (int j = 0; j < subtotals.Count; j++)
                    {
                        XmlNode subElem = subtotals[j];
                        msgTax += "\r\nSubDetail\r\n";
                        msgTax += "TaxableAmount: " + GetNodeText(subElem, "cbc:TaxableAmount") + "\r\n";
                        msgTax += "TaxAmount: " + GetNodeText(subElem, "cbc:TaxAmount") + "\r\n";
                        msgTax += "Percent: " + GetNodeText(subElem, "cac:TaxCategory/cbc:Percent") + "%\r\n\r\n";
                    }
                }
                if (showMessageBox)
                    MessageBox.Show(msgTax, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // LegalMonetaryTotal
            bool allowanceChargeCheck = false;

            string msgMoney = "LegalMonetaryTotal\r\n------------------\r\n";
            XmlNode moneyTotalEl = xmlDoc.SelectSingleNode("//cac:LegalMonetaryTotal", nsmgr);
            if (moneyTotalEl != null)
            {
                // 1
                valueToUse = GetNodeText(moneyTotalEl, "cbc:LineExtensionAmount");
                msgMoney += "LineExtensionAmount: " + valueToUse + "\r\n";
                legalMonetaryTotalOMS = "LineExtensionAmount";
                legalMonetaryTotalDATA = valueToUse;

                // 2
                valueToUse = GetNodeText(moneyTotalEl, "cbc:TaxExclusiveAmount");
                msgMoney += "TaxExclusiveAmount: " + valueToUse + "\r\n";
                uitwisselingOMS += "\ttotalExclusiveVAT";
                uitwisselingDATA += "\t" + valueToUse;

                legalMonetaryTotalOMS += "\tTaxExclusiveAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                // 3
                valueToUse = GetNodeText(moneyTotalEl, "cbc:TaxInclusiveAmount");
                msgMoney += "TaxInclusiveAmount: " + valueToUse + "\r\n";
                uitwisselingOMS += "\ttotalInclusiveVAT";
                uitwisselingDATA += "\t" + valueToUse;

                legalMonetaryTotalOMS += "\tTaxInclusiveAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                // 4
                valueToUse = GetNodeText(moneyTotalEl, "cbc:AllowenceTotalAmount");                
                legalMonetaryTotalOMS += "\tAllowenceTotalAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                // 5
                valueToUse = GetNodeText(moneyTotalEl, "cbc:ChargeTotalAmount");
                legalMonetaryTotalOMS += "\tChargeTotalAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                // 6
                valueToUse = GetNodeText(moneyTotalEl, "cbc:PrepaidAmount");
                legalMonetaryTotalOMS += "\tPrepaidAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                // 7
                valueToUse = GetNodeText(moneyTotalEl, "cbc:PayableAmount");
                legalMonetaryTotalOMS += "\tPayableAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                msgMoney += "PayableAmount: " + valueToUse + " (" + currencyID + ")\r\n";
                if (showMessageBox)
                    MessageBox.Show(msgMoney, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 8
                valueToUse = GetNodeText(moneyTotalEl, "cbc:PayableRoundingAmount");
                legalMonetaryTotalOMS += "\tPayableRoundingAmount";
                legalMonetaryTotalDATA += "\t" + valueToUse;

                legalMonetaryTotalOMSArray = legalMonetaryTotalOMS.Split('\t');
                legalMonetaryTotalDATAArray = legalMonetaryTotalDATA.Split('\t');

                // TODO Set a flag for extra allowanceCharge data
                //      Is there a value for AllowanceTotalAmount?
                //      Is there a value for ChargeTotalAmount?
                double allowanceChargeAmount =
                    ParseOrZero(legalMonetaryTotalDATAArray, 3) +
                    ParseOrZero(legalMonetaryTotalDATAArray, 4) +
                    ParseOrZero(legalMonetaryTotalDATAArray, 7);

                allowanceChargeCheck = allowanceChargeAmount != 0d;
            }

            // Customer info
            XmlNode custNode = xmlDoc.SelectSingleNode("//cac:AccountingCustomerParty", nsmgr);
            if (custNode != null)
            {
                msg = "Customer info\r\n-------------\r\n";

                valueToUse = GetNodeText(custNode, "cbc:CustomerAssignedAccountID");
                uitwisselingOMS += "\tcustomerAccountID";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custAssignedAccountID: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cbc:EndpointID");
                if (valueToUse.Length == 12 && valueToUse.Contains("BE"))
                    valueToUse = valueToUse.Substring(2);
                uitwisselingOMS += "\tcustomerEndpointID";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custEndpointID: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PartyName/cbc:Name");
                uitwisselingOMS += "\tcustomerName";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custName: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PostalAddress/cbc:StreetName");
                uitwisselingOMS += "\tcustomerStreetName";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custStreet: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PostalAddress/cbc:CityName");
                uitwisselingOMS += "\tcustomerCityName";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custCity: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PostalAddress/cbc:PostalZone");
                uitwisselingOMS += "\tcustomerPostalZone";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custPostalZone: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode");
                uitwisselingOMS += "\tcustomerCountryCode";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custCountryCode: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PartyTaxScheme/cbc:CompanyID");
                uitwisselingOMS += "\tcustomerTaxID";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custTaxID: " + valueToUse + "\r\n";

                valueToUse = GetNodeText(custNode, "cac:Party/cac:PartyTaxScheme/cac:TaxScheme/cbc:ID");
                uitwisselingOMS += "\tcustomerTaxScheme";
                uitwisselingDATA += "\t" + valueToUse;
                msg += "custTaxScheme: " + valueToUse + "\r\n";

                if (showMessageBox)
                    MessageBox.Show(msg, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No AccountingCustomerParty element found.");
            }

            // DocumentLines
            documentLinesOMS = "";
            documentLinesDATA = "";
            string msgLines = "";

            XmlNodeList invoiceLines = isInvoiceToCheck
                ? xmlDoc.SelectNodes("//cac:InvoiceLine", nsmgr)
                : xmlDoc.SelectNodes("//cac:CreditNoteLine", nsmgr);

            documentLinesOMS = "LineID\tStandardItemID\tSellerID\tDescription\tName\tQuantity\tPriceAmount\tExtensionAmount\tTaxPercentage";

            if (invoiceLines != null)
            {
                for (int i = 0; i < invoiceLines.Count; i++)
                {
                    XmlNode lineNode = invoiceLines[i];

                    string lineID = GetNodeText(lineNode, ".//cbc:ID"); if (lineID == "") lineID = "-";
                    string desc = GetNodeText(lineNode, ".//cbc:Description"); if (desc == "") desc = "-";
                    string nameLine = GetNodeText(lineNode, ".//cbc:Name"); if (nameLine == "") nameLine = "-";
                    string sellerID = GetNodeText(lineNode, ".//cac:SellersItemIdentification"); if (sellerID == "") sellerID = "-";
                    string standardItemID = GetNodeText(lineNode, ".//cac:StandardItemIdentification"); if (standardItemID == "") standardItemID = "-";

                    string qty = isInvoiceToCheck
                        ? GetNodeText(lineNode, ".//cbc:InvoicedQuantity")
                        : GetNodeText(lineNode, ".//cbc:CreditedQuantity");
                    if (qty == "") qty = "-";

                    string extensionAmount = GetNodeText(lineNode, ".//cbc:LineExtensionAmount");
                    if (double.TryParse(extensionAmount, out double extVal) && extVal == 0) extensionAmount = "-";

                    string price = GetNodeText(lineNode, ".//cbc:PriceAmount");
                    if (double.TryParse(price, out double priceVal) && priceVal == 0) price = "-";

                    XmlNode percentNode = lineNode.SelectSingleNode("cac:Item/cac:ClassifiedTaxCategory/cbc:Percent", nsmgr);
                    string taxPercentage = percentNode != null ? percentNode.InnerText : "0";

                    if (extensionAmount != "-")
                    {
                        documentLinesDATA += lineID + "\t" + standardItemID + "\t" + sellerID + "\t" +
                                             desc + "\t" + nameLine + "\t" + qty + "\t" + price + "\t" +
                                             extensionAmount + "\t" + taxPercentage + "\r\n";
                    }

                    msgLines += "Item: " + desc + ", Quantity: " + qty + ", Price: " + price + "\r\n";
                }

                if (msgLines != "" && showMessageBox)
                    MessageBox.Show(msgLines, "Testing UBL DATA versie 0.01", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (allowanceChargeCheck && forBooking)
            {
                // For now only mention + and - amounts including rounding.
                msg = "Aankoopdocument met globale kosten en/of kortingen" + Environment.NewLine + Environment.NewLine;
                msg += "KBO Nummer: " + tmpSupplierId + Environment.NewLine;
                msg += "Bedrijf   : " + tmpSupplierName + Environment.NewLine + Environment.NewLine;

                var lm3 = ParseOrZero(legalMonetaryTotalDATAArray, 3);
                var lm4 = ParseOrZero(legalMonetaryTotalDATAArray, 4);

                if (lm3 != 0d)
                    msg += "Globale korting: " + Dec(lm3, "#######.##") + Environment.NewLine;

                if (lm4 != 0d)
                    msg += "Globale kosten : " + Dec(lm4, "#######.##") + Environment.NewLine;

                msg += Environment.NewLine;
                msg += "Bij inboeking wordt bedrag toegevoegd aan de eerste factuurlijn." + Environment.NewLine;

                MessageBox.Show(msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            documentLinesDATA = documentLinesOMS + "\r\n" + documentLinesDATA;
            
        }

        // ?? CheckPeppolRegistration ??????????????????????????????????????????????

        public static string CheckPeppolRegistration(string peppolID)
        {
            string url = "https://directory.peppol.eu/search/1.0/json?q=iso6523-actorid-upis:" + peppolID;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    return reader.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }

        // ?? ExtractPdfAttachments ????????????????????????????????????????????????

        public static void ExtractPdfAttachments(string ublFilePath, string xmlLocation)
        {
            var xml = new XmlDocument();
            xml.PreserveWhitespace = true;
            xml.Load(ublFilePath);

            var nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            XmlNodeList nodes = xml.SelectNodes(
                "//cac:AdditionalDocumentReference/cac:Attachment/cbc:EmbeddedDocumentBinaryObject", nsmgr);
            if (nodes == null || nodes.Count == 0)
            {
                MessageBox.Show("Geen bijlagen gevonden.");
                return;
            }

            foreach (XmlNode node in nodes)
            {
                string mimeCode = (node.Attributes?["mimeCode"]?.Value ?? "").ToLower();
                if (mimeCode != "application/pdf") continue;

                string fileName = node.Attributes?["filename"]?.Value ?? "attachment.pdf";
                string base64 = CleanBase64(node.InnerText);
                byte[] bytes = Convert.FromBase64String(base64);
                string pdfPath = ublFilePath.Substring(0, ublFilePath.Length - 4) + "_" + fileName;

                SaveBinary(pdfPath, bytes);

                if (!File.Exists(pdfPath))
                {
                    MessageBox.Show("Er is geen PDF beschikbaar in \r\n" + pdfPath + "\r\n\r\nOpteer XML tonen",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    string lookForPDF = Path.GetFileName(pdfPath);
                    if (!ShellExecuteWithFallback(xmlLocation + lookForPDF))
                        MessageBox.Show("Kon bestand niet openen. Raadpleeg ShellHelper.log voor details.",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        // ?? CleanBase64 ??????????????????????????????????????????????????????????

        public static string CleanBase64(string s)
        {
            return s.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
        }

        // ?? SaveBinary ???????????????????????????????????????????????????????????

        public static void SaveBinary(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }
    }
}

