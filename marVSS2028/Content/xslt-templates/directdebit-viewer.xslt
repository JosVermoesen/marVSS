<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:pain="urn:iso:std:iso:20022:tech:xsd:pain.008.001.02"
    exclude-result-prefixes="pain">

    <xsl:output method="html" encoding="UTF-8" indent="yes" doctype-public="-//W3C//DTD HTML 4.01//EN" doctype-system="http://www.w3.org/TR/html4/strict.dtd"/>

    <xsl:template match="/">
        <html>
            <head>
                <meta charset="UTF-8"/>
                <title>SEPA Direct Debit Viewer</title>
                <style type="text/css">
                    * {
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }

                    body {
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        padding: 20px;
                        min-height: 100vh;
                    }

                    .container {
                        max-width: 1200px;
                        margin: 0 auto;
                        background: white;
                        border-radius: 8px;
                        box-shadow: 0 10px 40px rgba(0,0,0,0.2);
                        overflow: hidden;
                    }

                    .header {
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        color: white;
                        padding: 30px;
                        text-align: center;
                    }

                    .header h1 {
                        font-size: 28px;
                        margin-bottom: 10px;
                    }

                    .header-info {
                        display: grid;
                        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                        gap: 20px;
                        margin-top: 20px;
                    }

                    .info-box {
                        background: rgba(255,255,255,0.15);
                        padding: 15px;
                        border-radius: 5px;
                        backdrop-filter: blur(10px);
                    }

                    .info-box-label {
                        font-size: 12px;
                        text-transform: uppercase;
                        opacity: 0.9;
                        margin-bottom: 5px;
                    }

                    .info-box-value {
                        font-size: 16px;
                        font-weight: bold;
                    }

                    .content {
                        padding: 30px;
                    }

                    .section {
                        margin-bottom: 40px;
                    }

                    .section-title {
                        font-size: 20px;
                        font-weight: bold;
                        color: #667eea;
                        border-bottom: 3px solid #667eea;
                        padding-bottom: 10px;
                        margin-bottom: 20px;
                    }

                    .section-subtitle {
                        font-size: 16px;
                        font-weight: bold;
                        color: #333;
                        margin: 15px 0 10px 0;
                    }

                    .detail-grid {
                        display: grid;
                        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
                        gap: 20px;
                        margin-bottom: 20px;
                    }

                    .detail-item {
                        border-left: 4px solid #667eea;
                        padding-left: 15px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                    }

                    .detail-label {
                        font-size: 12px;
                        color: #999;
                        text-transform: uppercase;
                        margin-bottom: 3px;
                    }

                    .detail-value {
                        font-size: 14px;
                        color: #333;
                        font-weight: 500;
                    }

                    .transactions-table {
                        width: 100%;
                        border-collapse: collapse;
                        margin-top: 20px;
                        background: white;
                    }

                    .transactions-table thead {
                        background: #f8f9fa;
                        border-bottom: 2px solid #667eea;
                    }

                    .transactions-table th {
                        padding: 15px;
                        text-align: left;
                        font-weight: bold;
                        color: #667eea;
                        font-size: 12px;
                        text-transform: uppercase;
                    }

                    .transactions-table td {
                        padding: 15px;
                        border-bottom: 1px solid #e0e0e0;
                        color: #333;
                    }

                    .transactions-table tbody tr:hover {
                        background: #f8f9fa;
                    }

                    .amount {
                        font-weight: bold;
                        color: #27ae60;
                        text-align: right;
                        font-family: 'Courier New', monospace;
                    }

                    .currency {
                        font-size: 12px;
                        color: #999;
                    }

                    .summary {
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        color: white;
                        padding: 25px;
                        border-radius: 8px;
                        margin-top: 30px;
                        display: grid;
                        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                        gap: 20px;
                    }

                    .summary-item {
                        text-align: center;
                    }

                    .summary-label {
                        font-size: 12px;
                        text-transform: uppercase;
                        opacity: 0.9;
                        margin-bottom: 5px;
                    }

                    .summary-value {
                        font-size: 24px;
                        font-weight: bold;
                    }

                    .reference-info {
                        background: #f0f4ff;
                        border-left: 4px solid #667eea;
                        padding: 12px;
                        margin-top: 8px;
                        border-radius: 4px;
                        font-size: 12px;
                        color: #555;
                    }

                    .mandate-info {
                        background: #f0f4ff;
                        border-left: 4px solid #667eea;
                        padding: 12px;
                        margin-top: 8px;
                        border-radius: 4px;
                        font-size: 12px;
                        color: #555;
                    }

                    .footer {
                        background: #f8f9fa;
                        padding: 20px 30px;
                        text-align: center;
                        color: #999;
                        font-size: 12px;
                        border-top: 1px solid #e0e0e0;
                    }

                    .status-badge {
                        display: inline-block;
                        background: #667eea;
                        color: white;
                        padding: 4px 12px;
                        border-radius: 20px;
                        font-size: 11px;
                        font-weight: bold;
                        text-transform: uppercase;
                    }

                    .address-block {
                        line-height: 1.6;
                    }
                </style>
            </head>
            <body>
                <div class="container">
                    <!-- Header Section -->
                    <div class="header">
                        <h1>💳 SEPA Direct Debit Collection</h1>
                        <div class="header-info">
                            <div class="info-box">
                                <div class="info-box-label">Bericht-ID</div>
                                <div class="info-box-value">
                                    <xsl:value-of select="//pain:GrpHdr/pain:MsgId"/>
                                </div>
                            </div>
                            <div class="info-box">
                                <div class="info-box-label">Aanmaakdatum/Tijd</div>
                                <div class="info-box-value">
                                    <xsl:value-of select="//pain:GrpHdr/pain:CreDtTm"/>
                                </div>
                            </div>
                            <div class="info-box">
                                <div class="info-box-label">Aantal incasso's</div>
                                <div class="info-box-value">
                                    <xsl:value-of select="//pain:GrpHdr/pain:NbOfTxs"/>
                                </div>
                            </div>
                            <div class="info-box">
                                <div class="info-box-label">Totaal Bedrag</div>
                                <div class="info-box-value">
                                    €<xsl:value-of select="format-number(sum(//pain:DrctDbtTxInf/pain:InstdAmt), '0.00')"/>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="content">
                        <!-- Initiating Party Section -->
                        <div class="section">
                            <div class="section-title">📋 Opdrachtgever</div>
                            <div class="detail-item">
                                <div class="detail-label">Naam</div>
                                <div class="detail-value">
                                    <xsl:value-of select="//pain:GrpHdr/pain:InitgPty/pain:Nm"/>
                                </div>
                            </div>
                            <xsl:if test="//pain:GrpHdr/pain:InitgPty/pain:Id/pain:OrgId/pain:Othr/pain:Id">
                                <div class="detail-item" style="margin-top: 15px;">
                                    <div class="detail-label">KBO-Nummer</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:GrpHdr/pain:InitgPty/pain:Id/pain:OrgId/pain:Othr/pain:Id"/>
                                    </div>
                                </div>
                            </xsl:if>
                        </div>

                        <!-- Creditor Information Section -->
                        <div class="section">
                            <div class="section-title">💰 Incassantinformatie</div>
                            <div class="detail-grid">
                                <div class="detail-item">
                                    <div class="detail-label">Naam Incassant</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtInf/pain:Cdtr/pain:Nm"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">IBAN</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtInf/pain:CdtrAcct/pain:Id/pain:IBAN"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Bank BIC</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtInf/pain:CdtrAgt/pain:FinInstnId/pain:BIC"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Incasso Type</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtInf/pain:PmtTpInf/pain:LclInstrm/pain:Cd"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Gewenste incassodatum</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtInf/pain:ReqdColltnDt"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Sequence Type</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtInf/pain:PmtTpInf/pain:SeqTp"/>
                                    </div>
                                </div>
                            </div>
                            <xsl:if test="//pain:PmtInf/pain:Cdtr/pain:PstlAdr">
                                <div class="detail-item" style="margin-top: 15px;">
                                    <div class="detail-label">Adres</div>
                                    <div class="detail-value address-block">
                                        <xsl:value-of select="//pain:PmtInf/pain:Cdtr/pain:PstlAdr/pain:AdrLine[1]"/><br/>
                                        <xsl:value-of select="//pain:PmtInf/pain:Cdtr/pain:PstlAdr/pain:AdrLine[2]"/><br/>
                                        <xsl:value-of select="//pain:PmtInf/pain:Cdtr/pain:PstlAdr/pain:Ctry"/>
                                    </div>
                                </div>
                            </xsl:if>
                        </div>

                        <!-- Transactions Section -->
                        <div class="section">
                            <div class="section-title">📥 Incasso Details</div>
                            <table class="transactions-table">
                                <thead>
                                    <tr>
                                        <th>Nr.</th>
                                        <th>Betaler</th>
                                        <th>IBAN</th>
                                        <th>Bedrag</th>
                                        <th>Mandaat ID</th>
                                        <th>Referentie</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:for-each select="//pain:DrctDbtTxInf">
                                        <xsl:variable name="pos" select="position()"/>
                                        <tr>
                                            <td>
                                                <xsl:value-of select="$pos"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="pain:Dbtr/pain:Nm"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="pain:DbtrAcct/pain:Id/pain:IBAN"/>
                                            </td>
                                            <td class="amount">
                                                €<xsl:value-of select="format-number(pain:InstdAmt, '0.00')"/>
                                                <div class="currency">
                                                    <xsl:value-of select="pain:InstdAmt/@Ccy"/>
                                                </div>
                                            </td>
                                            <td>
                                                <xsl:value-of select="pain:DrctDbtTx/pain:MndtRltdInf/pain:MndtId"/>
                                            </td>
                                            <td>
                                                <xsl:variable name="mandateId" select="pain:DrctDbtTx/pain:MndtRltdInf/pain:MndtId"/>
                                                <xsl:variable name="mandateDate" select="pain:DrctDbtTx/pain:MndtRltdInf/pain:DtOfSgntr"/>
                                                <xsl:variable name="reference" select="pain:RmtInf/pain:Ustrd"/>
                                                <div class="mandate-info">
                                                    <strong>Mandaat:</strong><br/>
                                                    ID: <xsl:value-of select="$mandateId"/><br/>
                                                    Datum: <xsl:value-of select="$mandateDate"/>
                                                </div>
                                                <xsl:if test="$reference != ''">
                                                    <div class="reference-info">
                                                        <strong>Referentie:</strong><br/>
                                                        <xsl:value-of select="$reference"/>
                                                    </div>
                                                </xsl:if>
                                            </td>
                                        </tr>
                                    </xsl:for-each>
                                </tbody>
                            </table>
                        </div>

                        <!-- Summary Section -->
                        <div class="summary">
                            <div class="summary-item">
                                <div class="summary-label">Aantal Incasso's</div>
                                <div class="summary-value">
                                    <xsl:value-of select="//pain:PmtInf/pain:NbOfTxs"/>
                                </div>
                            </div>
                            <div class="summary-item">
                                <div class="summary-label">Controlesom</div>
                                <div class="summary-value">
                                    €<xsl:value-of select="format-number(sum(//pain:DrctDbtTxInf/pain:InstdAmt), '0.00')"/>
                                </div>
                            </div>
                            <div class="summary-item">
                                <div class="summary-label">Incasso Statuut</div>
                                <div class="summary-value" style="font-size: 16px;">
                                    <span class="status-badge">Verwerkt</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Footer -->
                    <div class="footer">
                        <p>SEPA Direct Debit Incasso Verkenner | Aangemaakt: <xsl:value-of select="//pain:GrpHdr/pain:CreDtTm"/> | Document Formaat: pain.008.001.02</p>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>

</xsl:stylesheet>
