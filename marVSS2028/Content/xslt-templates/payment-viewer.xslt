<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:pain="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03"
    exclude-result-prefixes="pain">

    <xsl:output method="html" encoding="UTF-8" indent="yes" doctype-public="-//W3C//DTD HTML 4.01//EN" doctype-system="http://www.w3.org/TR/html4/strict.dtd"/>

    <xsl:template match="/">
        <html>
            <head>
                <meta charset="UTF-8"/>
                <title>SEPA Payment Viewer</title>
                <style type="text/css">
                    * {
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }

                    body {
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
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
                        background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
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
                        color: #e74c3c;
                        border-bottom: 3px solid #e74c3c;
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
                        border-left: 4px solid #e74c3c;
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
                        border-bottom: 2px solid #e74c3c;
                    }

                    .transactions-table th {
                        padding: 15px;
                        text-align: left;
                        font-weight: bold;
                        color: #e74c3c;
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
                        color: #e74c3c;
                        text-align: right;
                        font-family: 'Courier New', monospace;
                    }

                    .currency {
                        font-size: 12px;
                        color: #999;
                    }

                    .summary {
                        background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
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
                        background: #fff3f0;
                        border-left: 4px solid #e74c3c;
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
                        background: #e74c3c;
                        color: white;
                        padding: 4px 12px;
                        border-radius: 20px;
                        font-size: 11px;
                        font-weight: bold;
                        text-transform: uppercase;
                    }
                </style>
            </head>
            <body>
                <div class="container">
                    <!-- Header Section -->
                    <div class="header">
                        <h1>💳 SEPA Credit Transfer Payment</h1>
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
                                <div class="info-box-label">Aantal transacties</div>
                                <div class="info-box-value">
                                    <xsl:value-of select="//pain:GrpHdr/pain:NbOfTxs"/>
                                </div>
                            </div>
                            <div class="info-box">
                                <div class="info-box-label">Totaal Bedrag</div>
                                <div class="info-box-value">
                                    €<xsl:value-of select="format-number(//pain:GrpHdr/pain:CtrlSum, '0.00')"/>
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
                                    <xsl:value-of select="//pain:InitgPty/pain:Nm"/>
                                </div>
                            </div>
                        </div>

                        <!-- Debtor Information Section -->
                        <div class="section">
                            <div class="section-title">💰 Betalersinformatie</div>
                            <div class="detail-grid">
                                <div class="detail-item">
                                    <div class="detail-label">Naam Debiteur</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:Dbtr/pain:Nm"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">IBAN</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:DbtrAcct/pain:Id/pain:IBAN"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Munteenheid</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:DbtrAcct/pain:Ccy"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Bank BIC</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:DbtrAgt/pain:FinInstnId/pain:BIC"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Betalingswijze</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:PmtMtd"/>
                                    </div>
                                </div>
                                <div class="detail-item">
                                    <div class="detail-label">Uitvoeringsdatum</div>
                                    <div class="detail-value">
                                        <xsl:value-of select="//pain:ReqdExctnDt"/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Transactions Section -->
                        <div class="section">
                            <div class="section-title">📤 Transactiedetails</div>
                            <table class="transactions-table">
                                <thead>
                                    <tr>
                                        <th>Nr.</th>
                                        <th>End-to-End ID</th>
                                        <th>Crediteur</th>
                                        <th>IBAN</th>
                                        <th>Bedrag</th>
                                        <th>Referte</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:for-each select="//pain:CdtTrfTxInf">
                                        <xsl:variable name="pos" select="position()"/>
                                        <tr>
                                            <td>
                                                <xsl:value-of select="$pos"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="pain:PmtId/pain:EndToEndId"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="pain:Cdtr/pain:Nm"/>
                                            </td>
                                            <td>
                                                <xsl:value-of select="pain:CdtrAcct/pain:Id/pain:IBAN"/>
                                            </td>
                                            <td class="amount">
                                                €<xsl:value-of select="format-number(pain:Amt/pain:InstdAmt, '0.00')"/>
                                                <div class="currency">
                                                    <xsl:value-of select="pain:Amt/pain:InstdAmt/@Ccy"/>
                                                </div>
                                            </td>
                                            <td>
                                                <xsl:choose>
                                                    <xsl:when test="pain:RmtInf/pain:Strd">
                                                        <div class="reference-info">
                                                            <strong>Gestructureerde Referte:</strong><br/>
                                                            <xsl:value-of select="pain:RmtInf/pain:Strd/pain:CdtrRefInf/pain:Ref"/>
                                                            <br/>
                                                            <small>(Type: <xsl:value-of select="pain:RmtInf/pain:Strd/pain:CdtrRefInf/pain:Tp/pain:CdOrPrtry/pain:Cd"/>)</small>
                                                        </div>
                                                    </xsl:when>
                                                    <xsl:when test="pain:RmtInf/pain:Ustrd">
                                                        <div class="reference-info">
                                                            <strong>Niet gestructureerde Referte:</strong><br/>
                                                            <xsl:value-of select="pain:RmtInf/pain:Ustrd"/>
                                                        </div>
                                                    </xsl:when>
                                                    <xsl:otherwise>
                                                        <em>Geen referte</em>
                                                    </xsl:otherwise>
                                                </xsl:choose>
                                            </td>
                                        </tr>
                                    </xsl:for-each>
                                </tbody>
                            </table>
                        </div>

                        <!-- Summary Section -->
                        <div class="summary">
                            <div class="summary-item">
                                <div class="summary-label">Aantal Transacties</div>
                                <div class="summary-value">
                                    <xsl:value-of select="//pain:PmtInf/pain:NbOfTxs"/>
                                </div>
                            </div>
                            <div class="summary-item">
                                <div class="summary-label">Controlesom</div>
                                <div class="summary-value">
                                    €<xsl:value-of select="format-number(//pain:PmtInf/pain:CtrlSum, '0.00')"/>
                                </div>
                            </div>
                            <div class="summary-item">
                                <div class="summary-label">Betaalstatuut</div>
                                <div class="summary-value" style="font-size: 16px;">
                                    <span class="status-badge">Verwerkt</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Footer -->
                    <div class="footer">
                        <p>SEPA Betalingsbestand Verkenner | Aangemaakt: <xsl:value-of select="//pain:GrpHdr/pain:CreDtTm"/> | Document Formaat: pain.001.001.03</p>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>

</xsl:stylesheet>