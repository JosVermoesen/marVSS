using System;
using System.Windows.Forms;

namespace marVSS2028.Classes
{
    internal static class Globals // This class contains global variables and constants used throughout the application.
    {
        // Mijn dokumenten, ApplicatieData
        public const int CSIDL_PERSONAL = 0x5;
        public const int CSIDL_APPDATA = 0x1A;
        public const int CSIDL_PROGRAM_FILES = 0x26;

        public const int SND_ASYNC = 0x1;
        public const int SND_NODEFAULT = 0x2;
        public const int SND_MEMORY = 0x4;

        public static string SoundBuffer;

        public const string TABLE_STR = "Tabel";
        public const string ATTACHED_STR = "Verbonden";
        public const string QUERY_STR = "Opzoeking";
        public const string FIELD_STR = "Kolom";
        public const string FIELDS_STR = "Kolommen";
        public const string INDEX_STR = "Index";
        public const string INDEXES_STR = "Indexen";
        public const string PROPERTY_STR = "Eigenschap";
        public const string PROPERTIES_STR = "Eigenschappen";

        public const string ADOJET_PROVIDER = "Provider=Microsoft.Jet.OLEDB.4.0;";
        public const string OLEDBJET_PROVIDER = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=";

        // current database node in treeview
        public static TreeNode gnodDBNode;
        public static TreeNode gnodDBNode2;

        // marNT constanten
        public const int NUMBER_TABLES = 9;
        public const int TABLE_VARIOUS = 0;
        public const int TABLE_CUSTOMERS = 1;
        public const int TABLE_SUPPLIERS = 2;
        public const int TABLE_LEDGERACCOUNTS = 3;
        public const int TABLE_PRODUCTS = 4;
        public const int TABLE_CONTRACTS = 5;
        public const int TABLE_INVOICES = 6;
        public const int TABLE_JOURNAL = 7;
        public const int TABLE_DUMMY = 8;
        public const int TABLE_COUNTERS = 9;

        public const int PERIODAS_TEXT = 0;
        public const int BOOKYEARAS_TEXT = 1;
        public const int PERIODAS_KEY = 2;
        public const int BOOKYEAR_KEY = 3;
        public const string SISO = "001*002*002*003*004*005*006*007*008*009*010*011*030*032*038*046*053*054*055*060*061*063*064*091*600*";
        public const int MAX_TELEBIB = 150;

        public const bool READING = true;
        public const bool READING_LOCK = false;

        public const string MASK_EURX = "######0.0000";
        public const string MASK_EURBH = "########0.00";
        public const string MASK_BEF = "##########";
        public const string MASK_EUR = "######0.00";

        public const double EURO = 40.3399;

        public static string FileNameQR;
        public static bool PeppolFlag;

        public static string[] MASK_SY = new string[9];
        public static string MASK_2002;
        public static bool VSF_PRO;

        public static string[] SYS_VAR = new string[7];
        public static int[] FILE_NR = new int[NUMBER_TABLES + 1];
        public static string[] TLB_RECORD = new string[NUMBER_TABLES + 1];
        public static string[] KEY_BUF = new string[NUMBER_TABLES + 1];
        public static string[] TABLEDEF_ONT = new string[NUMBER_TABLES + 1];
        public static int[] KEY_INDEX = new int[NUMBER_TABLES + 1];
        public static int[] INSERT_FLAG = new int[NUMBER_TABLES + 1];

        public static int[] FL_NUMBEROFINDEXEN = new int[11];
        public static string[,] JETTABLEUSE_INDEX = new string[NUMBER_TABLES + 1, 11];
        public static int[,] FLINDEX_LEN = new int[NUMBER_TABLES + 1, 11];
        public static string[,] FLINDEX_CAPTION = new string[NUMBER_TABLES + 1, 11];
        public static string[,] FVT = new string[NUMBER_TABLES + 1, 11];

        public static int[] DAYS_IN_MONTH = new int[13];
        public static string[] MONTH_AS_TEXT = new string[13];

        public static string[] REPORT_FIELD = new string[24];
        public static int[] REPORT_TAB = new int[24];

        // VB6: TELEBIB_CODE(-1 To MAX_TELEBIB)
        public const int TELEBIB_CODE_LOWERBOUND = -1;
        public static string[] TELEBIB_CODE = new string[MAX_TELEBIB + 2];

        // VB6: ReDim ToolDef(3) As String — field-mapping array used by vsfInputBox
        public static string[] ToolDef = new string[4];

        public static string[] TELEBIB_TEXT = new string[MAX_TELEBIB + 1];
        public static string[] TELEBIB_TYPE = new string[MAX_TELEBIB + 1];
        public static int[] TELEBIB_LENGTH = new int[MAX_TELEBIB + 1];
        public static int[] TELEBIB_POS = new int[MAX_TELEBIB + 1];
        public static int TELEBIB_LAST;

        public static int FL99;
        public static string FL99_RECORD;
        public static int PRINTER_CURRENT_Y;
        public static int PAGE_COUNTER;
        public static string FULL_LINE;

        public static string MAR_VERSION;
        public static string LOG_PRINT;
        public static bool BL_LOGGING;

        public static double DKTRL_CUMUL;
        public static double DKTRL_BEF;
        public static double DKTRL_EUR;

        public static int B_MODUS;
        public static int COUNT_TO;

        public static string PERIOD_FROMTO;
        public static string BOOKYEAR_FROMTO;
        public static int ACTIVE_BOOKYEAR;
        public static string MIM_GLOBAL_DATE;
        public static bool VAT_BOBTHEBUILDERS;
        public static string DIRECTSELL_STRING;

        public static string LOCATION_DESKTOP;
        public static string LOCATION_COMPANYDATA;
        public static string LOCATION_NETDATA;
        public static string PROGRAM_LOCATION;
        public static string LOCATION_;
        public static string LOCATION_ASWEB;
        public static string LOCATION_MYDOCUMENTS;
        public static string SYSTEM_MYPERSONALDOCUMENTS;
        
        public static string ProducentNummer;
        public static string Eigenaar;
        public static int Fl;
        public static int SharedFl;
        public static int SharedScanFl;
        public static int Ktrl;
        public static int KtrlLong;
        public static int aIndex;
        public static int AktieveFiche;

        public static bool blMilieu;
        public static string MilieuGridText;
        public static string GridText;
        public static string GridTextIs;
        public static object GridTextPolis;
        public static string GridText9;
        public static int GridRows;
        public static string XLogKey;

        public static string XLogKassa;

        public static double dKtrCumul;
        public static int SetupVelden;
        public static string BedrijfKeuze;
        public static double dMuntL;
        public static string Msg;
        public static int KtrlBox;
        public static string SQLBevel;
        public static int DoEventsStatus;
        public static int VsoftLog;
        public static string ProgrammaVersie;
        public static int LockHold;

        // DAO is not referenced in this project; keep as object until an interop/reference is added.
        public static object ntDB;
        public static object[] ntRS = new object[10];
        public static object NTRuimte;

        public static ADODB.Connection adKBDB;
        public static ADODB.Recordset adKBTable;

        public static ADODB.Connection adntDB;
        public static ADODB.Connection adntDBSQLS;

        public static ADODB.Connection adTBIB;
        public static ADODB.Recordset rsWaarden;
        public static ADODB.Recordset rsJournaal;
        public static ADODB.Recordset[] rsMAR = new ADODB.Recordset[10];

        public static string jetConnect;
        public static string oleDbConnect;

        public static int XDoEvents;
        public static string[] bstNaam = new string[10];
        public static int[] AddNewStatus = new int[10];
        public static string[,] vBC = new string[10, 201];
        public static int BAModus;

        public static bool TestEuroModus;
        public static bool bhEuro;
        public static bool XisEuroWisBEF;

        public static DateTime TimerTijd;
        public static object RetVal;
        public static object Figuur1;
        public static object Figuur2;

        public static int LijstPrinterNr;
        public static int dokumentPrinterNr;
        public static int KassaPrinterNr;

        public static object FormReference;
        public static FormBasicTable[] BasisB = new FormBasicTable[5];
        public static object JumpVenster;

        public static object fs;

        public static double KasTicketTotaal;
        public static double KasTotaal;
        public static double KasBetalingBEF;
        public static double KasBetalingEUR;
        public static double KasTerugEUR;
        public static double KasTotaalBEF;
        public static double KasTotaalEUR;

        public static bool DecimalKTRL;

        // marIntegraal.NET
        public static string usrLicentieInfo;
        public static bool JournaalLocked;
        public static string usrMailAdres;
        public static string usrPW;

        public static double pdfVsoftVanaf;
        public static double pdfVsoftTot;
        public static double pdfadresXpos;
        public static double pdfadresYpos;
        public static double pdfadresXpos2;
        public static double pdfadresYpos2;

        public static string strTELEBIBIO;

        public static string uitwisselingOMS;
        public static string uitwisselingDATA;
        public static string documentLinesOMS;
        public static string documentLinesDATA;
        
        public static string[] uitwisselingOMSArray;
        public static string[] uitwisselingDATAArray;
        public static string[] documentLinesOMSArray;
        public static string[,] documentLinesDATAArray;

        // LegalMonetaryTotal
        public static string legalMonetaryTotalOMS;
        public static string legalMonetaryTotalDATA;
        public static string[] legalMonetaryTotalOMSArray;
        public static string[] legalMonetaryTotalDATAArray;

        // Added now in .NET, but not used yet.
        // TODO: First testing in VB6 version, then in .NET version.
        public static string allowanceChargeOMS;
        public static string allowanceChargeDATA;
        public static string[] allowanceChargeOMSArray;
        public static string[] allowanceChargeDATAArray;

        public static string xdaOMS;
        public static string xdaDATA;
        public static string xdaLinesOMS;
        public static string xdaLinesDATA;

        public static string[] xdaOMSArray;
        public static string[] xdaDATAArray;
        public static string[] xdaLinesOMSArray;
        public static string[] xdaLinesDATAArray;

        public static bool purchasePeppolTODOShowed;
        public static string appTitleAndVersion;
        public static FormMim Mim { get; set; }
        public static bool IsPreviewMode = true;
    }
}
