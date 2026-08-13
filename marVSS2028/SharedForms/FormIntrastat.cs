using System;
using System.Collections.Generic;
using System.Globalization;
using System.Media;
using System.Windows.Forms;

using static marVSS2028.Classes.Globals;
using static marVSS2028.Classes.MdvDataTools;
using static marVSS2028.Classes.TextTools;

namespace marVSS2028.SharedForms
{
    public partial class FormIntrastat : Form
    {
        private readonly string[] _sIInput = new string[14];
        private string _intraType = string.Empty;
        private ComboBox[] _keuzeOpties;
        private TextBox[] _tekstInfo;

        public FormIntrastat()
        {
            InitializeComponent();
            InitializeControlArrays();
            InitializeInfoDataPlaceholder();
        }

        private void InitializeControlArrays()
        {
            _keuzeOpties = new[]
            {
                KeuzeOpties0,
                KeuzeOpties1,
                KeuzeOpties2,
                KeuzeOpties3,
                KeuzeOpties4
            };

            _tekstInfo = new[]
            {
                TekstInfo0,
                TekstInfo1,
                TekstInfo2,
                TekstInfo3,
                TekstInfo4
            };

            for (int i = 0; i < _tekstInfo.Length; i++)
            {
                _tekstInfo[i].Tag = i;
                _tekstInfo[i].Leave += TekstInfo_Leave;
            }
        }

        private void InitializeInfoDataPlaceholder()
        {
            InfoDataPanel.Tag = ADOJET_PROVIDER + "Data Source=" + PROGRAM_LOCATION + "Default2022.mdb;Persist Security Info=False";
        }

        private void FormIntrastat_Load(object sender, EventArgs e)
        {
            FillComboData();

            _intraType = aIndex.ToString("00", CultureInfo.InvariantCulture);
            Text += " " + _intraType;

            switch (_intraType)
            {
                case "19":
                    LabelA6.Text = "LidStaat Herkomst";
                    LabelB6.Text = "Land van Oorsprong";
                    Label8.Text = "Plaats lossen";
                    break;

                case "29":
                    LabelA6.Text = "LidStaat bestemming";
                    LabelB6.Text = string.Empty;
                    Label8.Text = "Plaats laden";
                    TekstInfo0.Enabled = false;
                    break;
            }

            KeuzeOpties0.Enabled = true;
            SetNogToeTeWijzen(SafeLeft(GridText ?? string.Empty, 9));
            _sIInput[6] = VBibText(Fl, "#v149 #").TrimEnd();

            SelectCountryFromCode(_sIInput[6]);

            if (KeuzeOpties2.Items.Count > 0)
            {
                KeuzeOpties2.SelectedIndex = 0;
            }

            if (KeuzeOpties1.Items.Count > 2)
            {
                KeuzeOpties1.SelectedIndex = 2;
            }

            if (KeuzeOpties3.Items.Count > 0)
            {
                KeuzeOpties3.SelectedIndex = 0;
            }
        }

        private void FormIntrastat_FormClosed(object sender, FormClosedEventArgs e)
        {
            BClose(TABLE_VARIOUS);
        }

        private void FillComboData()
        {
            AddItems(KeuzeOpties0, new[]
            {
                "001: Frankrijk",
                "002: G.H. Luxemburg",
                "003: Nederland",
                "004: Duitsland",
                "005: Italië",
                "006: Verenigd Koninkrijk",
                "007: Ierland",
                "008: Denemarken",
                "009: Griekenland",
                "010: Portugal",
                "011: Spanje en Canarische Eilanden",
                "030: Zweden",
                "032: Finland",
                "038: Oostenrijk",
                "046: Malta",
                "053: Estland",
                "054: Letland",
                "055: Litouwen",
                "060: Polen",
                "061: Tsjechië",
                "063: Slowakije",
                "064: Hongarije",
                "091: Slovenië",
                "600: Cyprus"
            });

            AddItems(KeuzeOpties1, new[]
            {
                "1: Vervoer over zee",
                "2: Vervoer per spoor",
                "3: Wegvervoer",
                "4: Luchtvervoer",
                "5: Postzendingen",
                "7: Vaste transportinrichtingen",
                "8: Binnenwateren",
                "9: Eigen kracht"
            });

            AddItems(KeuzeOpties2, new[]
            {
                "X: Andere dan zee",
                "1: Zee via Antwerpen 2000-2070/9120-9130",
                "2: Zee via Gent 9000-9060",
                "3: Zee via Zeebrugge 8000-8380",
                "4: Zee via Oostende 8400",
                "5: Zee via Brussel 1020-1210",
                "6: Noorden Brussel 1800-1980/2830-2880",
                "7: Agglomeratie Luik 4000-4684",
                "8: Nieuwpoort 8620",
                "9: Overige"
            });

            AddItems(KeuzeOpties3, new[]
            {
                "1: Eigendomsoverdracht met financiële compensatie",
                "2: Retourzendingen",
                "3: Eigendomsoverdracht zonder fin. compensatie",
                "4: Loonveredeling, herstelling intergouvern. programma's",
                "5: Loonveredel., herstell. buiten intergouvern. prog.",
                "7: Gecoördineerde defensieprojecten e.a. (vb. Airbus)",
                "8: Bouwmaterialen en uitrusting burgerlijke bouwkunde",
                "9: Andere, niet elders geregistreerde transacties"
            });
        }

        private static void AddItems(ComboBox combo, IEnumerable<string> items)
        {
            combo.Items.Clear();
            foreach (string item in items)
            {
                combo.Items.Add(item);
            }
        }

        private void SelectCountryFromCode(string code)
        {
            for (int i = KeuzeOpties0.Items.Count - 1; i >= 0; i--)
            {
                KeuzeOpties0.SelectedIndex = i;
                string text = KeuzeOpties0.Text ?? string.Empty;
                if (SafeLeft(text, 3) == code)
                {
                    return;
                }
            }

            if (KeuzeOpties0.Items.Count > 0)
            {
                KeuzeOpties0.SelectedIndex = 0;
            }
        }

        private void Annuleren_Click(object sender, EventArgs e)
        {
            const string msg = "Aangifte voor deze faktuur negeren !  Bent U zeker ?";
            DialogResult result = MessageBox.Show(
                msg,
                "INSTRAT 19 overslaan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void Eenheden_TextChanged(object sender, EventArgs e)
        {
            switch ((Eenheden.Text ?? string.Empty).Trim())
            {
                case "":
                case "-":
                    TekstInfo3.Enabled = false;
                    break;
                default:
                    TekstInfo3.Enabled = true;
                    break;
            }

            TekstInfo3.Text = string.Empty;
        }

        private void KeuzeOpties0_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_intraType == "19")
            {
                TekstInfo0.Text = SafeLeft(KeuzeOpties0.Text ?? string.Empty, 3);
            }
            else
            {
                TekstInfo0.Text = string.Empty;
            }
        }

        private void KeuzeOpties1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SafeLeft(KeuzeOpties1.Text ?? string.Empty, 1) == "1")
            {
                KeuzeOpties2.Enabled = true;
                if (KeuzeOpties2.Items.Count > 1)
                {
                    KeuzeOpties2.SelectedIndex = 1;
                }
            }
            else
            {
                KeuzeOpties2.Enabled = false;
                if (KeuzeOpties2.Items.Count > 0)
                {
                    KeuzeOpties2.SelectedIndex = 0;
                }
            }
        }

        private void KeuzeOpties3_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeuzeOpties4.Items.Clear();

            switch (SafeLeft(KeuzeOpties3.Text ?? string.Empty, 1))
            {
                case "1":
                    AddItems(KeuzeOpties4, new[]
                    {
                        "1: Definitieve aankoop/verkoop",
                        "2: Zichtzending/op proef of via commissionair",
                        "3: Ruilhandel (compensatie in natura)",
                        "4: Persoonlijke aankopen door reiziger",
                        "5: Financiële leasing"
                    });
                    break;

                case "2":
                    AddItems(KeuzeOpties4, new[]
                    {
                        "1: Terugzending van goederen",
                        "2: Vervanging van teruggezonden goederen",
                        "3: Vervanging goederen die niet teruggezonden zijn"
                    });
                    break;

                case "3":
                    AddItems(KeuzeOpties4, new[]
                    {
                        "1: Door E.U. gefinancierde hulpprogramma's",
                        "2: Andere algemene regeringshulp",
                        "3: Andere hulp (particuliere, niet gouvern. organ.)"
                    });
                    break;

                case "4":
                case "5":
                    AddItems(KeuzeOpties4, new[]
                    {
                        "1: Loonveredeling",
                        "2: Onderhoud en herstelling tegen betaling",
                        "3: Onderhoud en herstelling kosteloos"
                    });
                    break;

                default:
                    AddItems(KeuzeOpties4, new[] { "0: zonder meer..." });
                    break;
            }

            if (KeuzeOpties4.Items.Count > 0)
            {
                KeuzeOpties4.SelectedIndex = 0;
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if ((TekstInfo1.Text ?? string.Empty).Trim().Length != 8 || ParseVbVal(TekstInfo2.Text) == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            if (string.IsNullOrWhiteSpace(TekstInfo3.Text) && TekstInfo3.Enabled)
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Aanvullende eenheden a.u.b.", "Intrastat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string msg = "Informatielijn opslaan...\r\nBent U zeker ?";
            DialogResult result = MessageBox.Show(
                msg,
                "Intrastat 19",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                return;
            }

            string landCode = SafeLeft(KeuzeOpties0.Text ?? string.Empty, 3);
            string oorsprongCode = SafeLeft(TekstInfo0.Text ?? string.Empty, 3);
            string vervoerWijze = SafeLeft(KeuzeOpties1.Text ?? string.Empty, 1);
            string plaatsCode = SafeLeft(KeuzeOpties2.Text ?? string.Empty, 1);
            string transA = SafeLeft(KeuzeOpties3.Text ?? string.Empty, 1);
            string transB = SafeLeft(KeuzeOpties4.Text ?? string.Empty, 1);
            string goederenKode = (TekstInfo1.Text ?? string.Empty).Trim();

            TLB_RECORD[TABLE_VARIOUS] = string.Empty;
            FVT[TABLE_VARIOUS, 1] = _intraType + landCode;

            VBib(TABLE_VARIOUS, landCode, "v072");
            FVT[TABLE_VARIOUS, 1] += VSet(oorsprongCode, 3);
            VBib(TABLE_VARIOUS, oorsprongCode, "v073");

            FVT[TABLE_VARIOUS, 1] += vervoerWijze;
            VBib(TABLE_VARIOUS, vervoerWijze, "v074");

            FVT[TABLE_VARIOUS, 1] += plaatsCode;
            VBib(TABLE_VARIOUS, plaatsCode, "v075");

            FVT[TABLE_VARIOUS, 1] += transA;
            VBib(TABLE_VARIOUS, transA, "v076");

            FVT[TABLE_VARIOUS, 1] += transB;
            VBib(TABLE_VARIOUS, transB, "v077");

            FVT[TABLE_VARIOUS, 1] += goederenKode;
            VBib(TABLE_VARIOUS, goederenKode, "v078");

            VBib(TABLE_VARIOUS, FVT[TABLE_VARIOUS, 1], "v005");
            VBib(TABLE_VARIOUS, Dec(ParseVbVal(TekstInfo2.Text), MASK_SY[0]), "v079");
            VBib(TABLE_VARIOUS, Dec(ParseVbVal(TekstInfo3.Text), MASK_SY[0]), "v080");
            VBib(TABLE_VARIOUS, Dec(ParseVbVal(TekstInfo4.Text), MASK_SY[0]), "v081");
            VBib(TABLE_VARIOUS, VBibText(TABLE_INVOICES, "#v033 #"), "v033");
            VBib(TABLE_VARIOUS, VBibText(TABLE_INVOICES, "#v035 #"), "v035");

            BInsert(TABLE_VARIOUS, 1);
            if (Ktrl != 0)
            {
                return;
            }

            double nieuwSaldo = ParseVbVal(NogToeTeWijzen.Text) - ParseVbVal(TekstInfo4.Text);
            SetNogToeTeWijzen(Dec(nieuwSaldo, MASK_SY[0]));

            if (ParseVbVal(NogToeTeWijzen.Text) == 0)
            {
                Close();
            }
            else
            {
                TekstInfo2.Text = string.Empty;
                TekstInfo3.Text = string.Empty;
            }
        }

        private void TekstInfo_Leave(object sender, EventArgs e)
        {
            int index = sender is Control c && c.Tag is int i ? i : -1;
            if (index < 0)
            {
                return;
            }

            switch (index)
            {
                case 0:
                    _tekstInfo[index].Text = ParseVbVal(_tekstInfo[index].Text).ToString("000", CultureInfo.InvariantCulture);
                    break;

                case 1:
                    if (string.IsNullOrWhiteSpace(TekstInfo1.Text))
                    {
                        return;
                    }
                    break;

                case 4:
                    double invoer = ParseVbVal(_tekstInfo[index].Text);
                    double nog = ParseVbVal(NogToeTeWijzen.Text);
                    if (invoer > nog || invoer == 0)
                    {
                        SystemSounds.Beep.Play();
                        _tekstInfo[index].Text = NogToeTeWijzen.Text;
                    }
                    break;
            }
        }

        private void SetNogToeTeWijzen(string value)
        {
            NogToeTeWijzen.Text = value ?? string.Empty;
            TekstInfo4.Text = NogToeTeWijzen.Text;
        }

        private static double ParseVbVal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            string raw = text.Trim();
            if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            if (double.TryParse(raw, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
            {
                return value;
            }

            return 0;
        }
    }
}
