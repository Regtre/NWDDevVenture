using System.Collections.Generic;

namespace NWDFoundation.Localization
{
    public class NWDCountryISO
    {
        public string Name { get; private set; }
        public string TwoLetterCode { get; private set; }
        public string ThreeLetterCode { get; private set; }
        public string NumericCode { get; private set; }

        private NWDCountryISO(string sName, string sShortCode, string sCode, string sNumericCode)
        {
            Name = sName;
            TwoLetterCode = sShortCode;
            ThreeLetterCode = sCode;
            NumericCode = sNumericCode;
        }

        private static List<string> CountryListString;
        private static List<string> CountryListThreeLetterCode;
        private static List<string> CountryListTwoLetterCode;
        private static Dictionary<string, NWDCountryISO> TwoLetterCodeCountryDictionary;
        private static Dictionary<string, NWDCountryISO> ThreeLetterCodeCountryDictionary;

        public static NWDCountryISO GetFromTwoLetterCode(string sCode)
        {
            if (TwoLetterCodeCountryDictionary == null)
            {
                TwoLetterCodeCountryDictionary = new Dictionary<string, NWDCountryISO>();
                foreach (NWDCountryISO tCountry in List)
                {
                    TwoLetterCodeCountryDictionary.Add(tCountry.TwoLetterCode, tCountry);
                }
            }

            if (TwoLetterCodeCountryDictionary.ContainsKey(sCode))
            {
                return TwoLetterCodeCountryDictionary[sCode];
            }

            return new NWDCountryISO("","","","");
        }
        public static NWDCountryISO GetFromThreeLetterCode(string sCode)
        {
            if (ThreeLetterCodeCountryDictionary == null)
            {
                ThreeLetterCodeCountryDictionary = new Dictionary<string, NWDCountryISO>();
                foreach (NWDCountryISO tCountry in List)
                {
                    ThreeLetterCodeCountryDictionary.Add(tCountry.ThreeLetterCode, tCountry);
                }
            }

            if (ThreeLetterCodeCountryDictionary.ContainsKey(sCode))
            {
                return ThreeLetterCodeCountryDictionary[sCode];
            }

            return new NWDCountryISO("","","","");
        }
        public static List<string> GetCountries()
        {
            if (CountryListString == null)
            {
                CountryListString = new List<string>();
                foreach (NWDCountryISO tCountry in List)
                {
                    CountryListString.Add(tCountry.Name);
                }
            }

            return CountryListString;
        }
        public static List<string> GetThreeLetterCodeCountries()
        {
            if (CountryListThreeLetterCode == null)
            {
                CountryListThreeLetterCode = new List<string>();
                foreach (NWDCountryISO tCountry in List)
                {
                    CountryListThreeLetterCode.Add(tCountry.ThreeLetterCode);
                }
            }

            return CountryListThreeLetterCode;
        }
        public static List<string> GetTwoLetterCodeCountries()
        {
            if (CountryListTwoLetterCode == null)
            {
                CountryListTwoLetterCode = new List<string>();
                foreach (NWDCountryISO tCountry in List)
                {
                    CountryListTwoLetterCode.Add(tCountry.TwoLetterCode);
                }
            }

            return CountryListTwoLetterCode;
        }
        
        public static readonly NWDCountryISO[] List = new[]
        {
            new NWDCountryISO("Afghanistan", "AF", "AFG", "004"),
            new NWDCountryISO("Albania", "AL", "ALB", "008"),
            new NWDCountryISO("Algeria", "DZ", "DZA", "012"),
            new NWDCountryISO("American Samoa", "AS", "ASM", "016"),
            new NWDCountryISO("Andorra", "AD", "AND", "020"),
            new NWDCountryISO("Angola", "AO", "AGO", "024"),
            new NWDCountryISO("Anguilla", "AI", "AIA", "660"),
            new NWDCountryISO("Antarctica", "AQ", "ATA", "010"),
            new NWDCountryISO("Antigua and Barbuda", "AG", "ATG", "028"),
            new NWDCountryISO("Argentina", "AR", "ARG", "032"),
            new NWDCountryISO("Armenia", "AM", "ARM", "051"),
            new NWDCountryISO("Aruba", "AW", "ABW", "533"),
            new NWDCountryISO("Australia", "AU", "AUS", "036"),
            new NWDCountryISO("Austria", "AT", "AUT", "040"),
            new NWDCountryISO("Azerbaijan", "AZ", "AZE", "031"),
            new NWDCountryISO("Bahamas", "BS", "BHS", "044"),
            new NWDCountryISO("Bahrain", "BH", "BHR", "048"),
            new NWDCountryISO("Bangladesh", "BD", "BGD", "050"),
            new NWDCountryISO("Barbados", "BB", "BRB", "052"),
            new NWDCountryISO("Belarus", "BY", "BLR", "112"),
            new NWDCountryISO("Belgium", "BE", "BEL", "056"),
            new NWDCountryISO("Belize", "BZ", "BLZ", "084"),
            new NWDCountryISO("Benin", "BJ", "BEN", "204"),
            new NWDCountryISO("Bermuda", "BM", "BMU", "060"),
            new NWDCountryISO("Bhutan", "BT", "BTN", "064"),
            new NWDCountryISO("Bolivia, Plurinational State of", "BO", "BOL", "068"),
            new NWDCountryISO("Bonaire, Sint Eustatius and Saba", "BQ", "BES", "535"),
            new NWDCountryISO("Bosnia and Herzegovina", "BA", "BIH", "070"),
            new NWDCountryISO("Botswana", "BW", "BWA", "072"),
            new NWDCountryISO("Bouvet Island", "BV", "BVT", "074"),
            new NWDCountryISO("Brazil", "BR", "BRA", "076"),
            new NWDCountryISO("British Indian Ocean Territory", "IO", "IOT", "086"),
            new NWDCountryISO("Brunei Darussalam", "BN", "BRN", "096"),
            new NWDCountryISO("Bulgaria", "BG", "BGR", "100"),
            new NWDCountryISO("Burkina Faso", "BF", "BFA", "854"),
            new NWDCountryISO("Burundi", "BI", "BDI", "108"),
            new NWDCountryISO("Cabo Verde", "CV", "CPV", "132"),
            new NWDCountryISO("Cambodia", "KH", "KHM", "116"),
            new NWDCountryISO("Cameroon", "CM", "CMR", "120"),
            new NWDCountryISO("Canada", "CA", "CAN", "124"),
            new NWDCountryISO("Cayman Islands", "KY", "CYM", "136"),
            new NWDCountryISO("Central African Republic", "CF", "CAF", "140"),
            new NWDCountryISO("Chad", "TD", "TCD", "148"),
            new NWDCountryISO("Chile", "CL", "CHL", "152"),
            new NWDCountryISO("China", "CN", "CHN", "156"),
            new NWDCountryISO("Christmas Island", "CX", "CXR", "162"),
            new NWDCountryISO("Cocos (Keeling) Islands", "CC", "CCK", "166"),
            new NWDCountryISO("Colombia", "CO", "COL", "170"),
            new NWDCountryISO("Comoros", "KM", "COM", "174"),
            new NWDCountryISO("Congo", "CG", "COG", "178"),
            new NWDCountryISO("Congo, the Democratic Republic of the", "CD", "COD", "180"),
            new NWDCountryISO("Cook Islands", "CK", "COK", "184"),
            new NWDCountryISO("Costa Rica", "CR", "CRI", "188"),
            new NWDCountryISO("Côte d'Ivoire", "CI", "CIV", "384"),
            new NWDCountryISO("Croatia", "HR", "HRV", "191"),
            new NWDCountryISO("Cuba", "CU", "CUB", "192"),
            new NWDCountryISO("Curaçao", "CW", "CUW", "531"),
            new NWDCountryISO("Cyprus", "CY", "CYP", "196"),
            new NWDCountryISO("Czechia", "CZ", "CZE", "203"),
            new NWDCountryISO("Denmark", "DK", "DNK", "208"),
            new NWDCountryISO("Djibouti", "DJ", "DJI", "262"),
            new NWDCountryISO("Dominica", "DM", "DMA", "212"),
            new NWDCountryISO("Dominican Republic", "DO", "DOM", "214"),
            new NWDCountryISO("Ecuador", "EC", "ECU", "218"),
            new NWDCountryISO("Egypt", "EG", "EGY", "818"),
            new NWDCountryISO("El Salvador", "SV", "SLV", "222"),
            new NWDCountryISO("Equatorial Guinea", "GQ", "GNQ", "226"),
            new NWDCountryISO("Eritrea", "ER", "ERI", "232"),
            new NWDCountryISO("Estonia", "EE", "EST", "233"),
            new NWDCountryISO("Eswatini", "SZ", "SWZ", "748"),
            new NWDCountryISO("Ethiopia", "ET", "ETH", "231"),
            new NWDCountryISO("Falkland Islands (Malvinas)", "FK", "FLK", "238"),
            new NWDCountryISO("Faroe Islands", "FO", "FRO", "234"),
            new NWDCountryISO("Fiji", "FJ", "FJI", "242"),
            new NWDCountryISO("Finland", "FI", "FIN", "246"),
            new NWDCountryISO("France", "FR", "FRA", "250"),
            new NWDCountryISO("French Guiana", "GF", "GUF", "254"),
            new NWDCountryISO("French Polynesia", "PF", "PYF", "258"),
            new NWDCountryISO("French Southern Territories", "TF", "ATF", "260"),
            new NWDCountryISO("Gabon", "GA", "GAB", "266"),
            new NWDCountryISO("Gambia", "GM", "GMB", "270"),
            new NWDCountryISO("Georgia", "GE", "GEO", "268"),
            new NWDCountryISO("Germany", "DE", "DEU", "276"),
            new NWDCountryISO("Ghana", "GH", "GHA", "288"),
            new NWDCountryISO("Gibraltar", "GI", "GIB", "292"),
            new NWDCountryISO("Greece", "GR", "GRC", "300"),
            new NWDCountryISO("Greenland", "GL", "GRL", "304"),
            new NWDCountryISO("Grenada", "GD", "GRD", "308"),
            new NWDCountryISO("Guadeloupe", "GP", "GLP", "312"),
            new NWDCountryISO("Guam", "GU", "GUM", "316"),
            new NWDCountryISO("Guatemala", "GT", "GTM", "320"),
            new NWDCountryISO("Guernsey", "GG", "GGY", "831"),
            new NWDCountryISO("Guinea", "GN", "GIN", "324"),
            new NWDCountryISO("Guinea-Bissau", "GW", "GNB", "624"),
            new NWDCountryISO("Guyana", "GY", "GUY", "328"),
            new NWDCountryISO("Haiti", "HT", "HTI", "332"),
            new NWDCountryISO("Heard Island and McDonald Islands", "HM", "HMD", "334"),
            new NWDCountryISO("Holy See", "VA", "VAT", "336"),
            new NWDCountryISO("Honduras", "HN", "HND", "340"),
            new NWDCountryISO("Hong Kong", "HK", "HKG", "344"),
            new NWDCountryISO("Hungary", "HU", "HUN", "348"),
            new NWDCountryISO("Iceland", "IS", "ISL", "352"),
            new NWDCountryISO("India", "IN", "IND", "356"),
            new NWDCountryISO("Indonesia", "ID", "IDN", "360"),
            new NWDCountryISO("Iran, Islamic Republic of", "IR", "IRN", "364"),
            new NWDCountryISO("Iraq", "IQ", "IRQ", "368"),
            new NWDCountryISO("Ireland", "IE", "IRL", "372"),
            new NWDCountryISO("Isle of Man", "IM", "IMN", "833"),
            new NWDCountryISO("Israel", "IL", "ISR", "376"),
            new NWDCountryISO("Italy", "IT", "ITA", "380"),
            new NWDCountryISO("Jamaica", "JM", "JAM", "388"),
            new NWDCountryISO("Japan", "JP", "JPN", "392"),
            new NWDCountryISO("Jersey", "JE", "JEY", "832"),
            new NWDCountryISO("Jordan", "JO", "JOR", "400"),
            new NWDCountryISO("Kazakhstan", "KZ", "KAZ", "398"),
            new NWDCountryISO("Kenya", "KE", "KEN", "404"),
            new NWDCountryISO("Kiribati", "KI", "KIR", "296"),
            new NWDCountryISO("Korea, Democratic People's Republic of", "KP", "PRK", "408"),
            new NWDCountryISO("Korea, Republic of", "KR", "KOR", "410"),
            new NWDCountryISO("Kuwait", "KW", "KWT", "414"),
            new NWDCountryISO("Kyrgyzstan", "KG", "KGZ", "417"),
            new NWDCountryISO("Lao People's Democratic Republic", "LA", "LAO", "418"),
            new NWDCountryISO("Latvia", "LV", "LVA", "428"),
            new NWDCountryISO("Lebanon", "LB", "LBN", "422"),
            new NWDCountryISO("Lesotho", "LS", "LSO", "426"),
            new NWDCountryISO("Liberia", "LR", "LBR", "430"),
            new NWDCountryISO("Libya", "LY", "LBY", "434"),
            new NWDCountryISO("Liechtenstein", "LI", "LIE", "438"),
            new NWDCountryISO("Lithuania", "LT", "LTU", "440"),
            new NWDCountryISO("Luxembourg", "LU", "LUX", "442"),
            new NWDCountryISO("Macao", "MO", "MAC", "446"),
            new NWDCountryISO("Madagascar", "MG", "MDG", "450"),
            new NWDCountryISO("Malawi", "MW", "MWI", "454"),
            new NWDCountryISO("Malaysia", "MY", "MYS", "458"),
            new NWDCountryISO("Maldives", "MV", "MDV", "462"),
            new NWDCountryISO("Mali", "ML", "MLI", "466"),
            new NWDCountryISO("Malta", "MT", "MLT", "470"),
            new NWDCountryISO("Marshall Islands", "MH", "MHL", "584"),
            new NWDCountryISO("Martinique", "MQ", "MTQ", "474"),
            new NWDCountryISO("Mauritania", "MR", "MRT", "478"),
            new NWDCountryISO("Mauritius", "MU", "MUS", "480"),
            new NWDCountryISO("Mayotte", "YT", "MYT", "175"),
            new NWDCountryISO("Mexico", "MX", "MEX", "484"),
            new NWDCountryISO("Micronesia, Federated States of", "FM", "FSM", "583"),
            new NWDCountryISO("Moldova, Republic of", "MD", "MDA", "498"),
            new NWDCountryISO("Monaco", "MC", "MCO", "492"),
            new NWDCountryISO("Mongolia", "MN", "MNG", "496"),
            new NWDCountryISO("Montenegro", "ME", "MNE", "499"),
            new NWDCountryISO("Montserrat", "MS", "MSR", "500"),
            new NWDCountryISO("Morocco", "MA", "MAR", "504"),
            new NWDCountryISO("Mozambique", "MZ", "MOZ", "508"),
            new NWDCountryISO("Myanmar", "MM", "MMR", "104"),
            new NWDCountryISO("Namibia", "NA", "NAM", "516"),
            new NWDCountryISO("Nauru", "NR", "NRU", "520"),
            new NWDCountryISO("Nepal", "NP", "NPL", "524"),
            new NWDCountryISO("Netherlands", "NL", "NLD", "528"),
            new NWDCountryISO("New Caledonia", "NC", "NCL", "540"),
            new NWDCountryISO("New Zealand", "NZ", "NZL", "554"),
            new NWDCountryISO("Nicaragua", "NI", "NIC", "558"),
            new NWDCountryISO("Niger", "NE", "NER", "562"),
            new NWDCountryISO("Nigeria", "NG", "NGA", "566"),
            new NWDCountryISO("Niue", "NU", "NIU", "570"),
            new NWDCountryISO("Norfolk Island", "NF", "NFK", "574"),
            new NWDCountryISO("Northern Mariana Islands", "MP", "MNP", "580"),
            new NWDCountryISO("North Macedonia", "MK", "MKD", "807"),
            new NWDCountryISO("Norway", "NO", "NOR", "578"),
            new NWDCountryISO("Oman", "OM", "OMN", "512"),
            new NWDCountryISO("Pakistan", "PK", "PAK", "586"),
            new NWDCountryISO("Palau", "PW", "PLW", "585"),
            new NWDCountryISO("Palestine, State of", "PS", "PSE", "275"),
            new NWDCountryISO("Panama", "PA", "PAN", "591"),
            new NWDCountryISO("Papua New Guinea", "PG", "PNG", "598"),
            new NWDCountryISO("Paraguay", "PY", "PRY", "600"),
            new NWDCountryISO("Peru", "PE", "PER", "604"),
            new NWDCountryISO("Philippines", "PH", "PHL", "608"),
            new NWDCountryISO("Pitcairn", "PN", "PCN", "612"),
            new NWDCountryISO("Poland", "PL", "POL", "616"),
            new NWDCountryISO("Portugal", "PT", "PRT", "620"),
            new NWDCountryISO("Puerto Rico", "PR", "PRI", "630"),
            new NWDCountryISO("Qatar", "QA", "QAT", "634"),
            new NWDCountryISO("Réunion", "RE", "REU", "638"),
            new NWDCountryISO("Romania", "RO", "ROU", "642"),
            new NWDCountryISO("Russian Federation", "RU", "RUS", "643"),
            new NWDCountryISO("Rwanda", "RW", "RWA", "646"),
            new NWDCountryISO("Saint Barthélemy", "BL", "BLM", "652"),
            new NWDCountryISO("Saint Helena, Ascension and Tristan da Cunha", "SH", "SHN", "654"),
            new NWDCountryISO("Saint Kitts and Nevis", "KN", "KNA", "659"),
            new NWDCountryISO("Saint Lucia", "LC", "LCA", "662"),
            new NWDCountryISO("Saint Martin (French part)", "MF", "MAF", "663"),
            new NWDCountryISO("Saint Pierre and Miquelon", "PM", "SPM", "666"),
            new NWDCountryISO("Saint Vincent and the Grenadines", "VC", "VCT", "670"),
            new NWDCountryISO("Samoa", "WS", "WSM", "882"),
            new NWDCountryISO("San Marino", "SM", "SMR", "674"),
            new NWDCountryISO("Sao Tome and Principe", "ST", "STP", "678"),
            new NWDCountryISO("Saudi Arabia", "SA", "SAU", "682"),
            new NWDCountryISO("Senegal", "SN", "SEN", "686"),
            new NWDCountryISO("Serbia", "RS", "SRB", "688"),
            new NWDCountryISO("Seychelles", "SC", "SYC", "690"),
            new NWDCountryISO("Sierra Leone", "SL", "SLE", "694"),
            new NWDCountryISO("Singapore", "SG", "SGP", "702"),
            new NWDCountryISO("Sint Maarten (Dutch part)", "SX", "SXM", "534"),
            new NWDCountryISO("Slovakia", "SK", "SVK", "703"),
            new NWDCountryISO("Slovenia", "SI", "SVN", "705"),
            new NWDCountryISO("Solomon Islands", "SB", "SLB", "090"),
            new NWDCountryISO("Somalia", "SO", "SOM", "706"),
            new NWDCountryISO("South Africa", "ZA", "ZAF", "710"),
            new NWDCountryISO("South Georgia and the South Sandwich Islands", "GS", "SGS", "239"),
            new NWDCountryISO("South Sudan", "SS", "SSD", "728"),
            new NWDCountryISO("Spain", "ES", "ESP", "724"),
            new NWDCountryISO("Sri Lanka", "LK", "LKA", "144"),
            new NWDCountryISO("Sudan", "SD", "SDN", "729"),
            new NWDCountryISO("Suriname", "SR", "SUR", "740"),
            new NWDCountryISO("Svalbard and Jan Mayen", "SJ", "SJM", "744"),
            new NWDCountryISO("Sweden", "SE", "SWE", "752"),
            new NWDCountryISO("Switzerland", "CH", "CHE", "756"),
            new NWDCountryISO("Syrian Arab Republic", "SY", "SYR", "760"),
            new NWDCountryISO("Taiwan, Province of China", "TW", "TWN", "158"),
            new NWDCountryISO("Tajikistan", "TJ", "TJK", "762"),
            new NWDCountryISO("Tanzania, United Republic of", "TZ", "TZA", "834"),
            new NWDCountryISO("Thailand", "TH", "THA", "764"),
            new NWDCountryISO("Timor-Leste", "TL", "TLS", "626"),
            new NWDCountryISO("Togo", "TG", "TGO", "768"),
            new NWDCountryISO("Tokelau", "TK", "TKL", "772"),
            new NWDCountryISO("Tonga", "TO", "TON", "776"),
            new NWDCountryISO("Trinidad and Tobago", "TT", "TTO", "780"),
            new NWDCountryISO("Tunisia", "TN", "TUN", "788"),
            new NWDCountryISO("Turkey", "TR", "TUR", "792"),
            new NWDCountryISO("Turkmenistan", "TM", "TKM", "795"),
            new NWDCountryISO("Turks and Caicos Islands", "TC", "TCA", "796"),
            new NWDCountryISO("Tuvalu", "TV", "TUV", "798"),
            new NWDCountryISO("Uganda", "UG", "UGA", "800"),
            new NWDCountryISO("Ukraine", "UA", "UKR", "804"),
            new NWDCountryISO("United Arab Emirates", "AE", "ARE", "784"),
            new NWDCountryISO("United Kingdom of Great Britain and Northern Ireland", "GB", "GBR", "826"),
            new NWDCountryISO("United States of America", "US", "USA", "840"),
            new NWDCountryISO("United States Minor Outlying Islands", "UM", "UMI", "581"),
            new NWDCountryISO("Uruguay", "UY", "URY", "858"),
            new NWDCountryISO("Uzbekistan", "UZ", "UZB", "860"),
            new NWDCountryISO("Vanuatu", "VU", "VUT", "548"),
            new NWDCountryISO("Venezuela, Bolivarian Republic of", "VE", "VEN", "862"),
            new NWDCountryISO("Viet Nam", "VN", "VNM", "704"),
            new NWDCountryISO("Virgin Islands, British", "VG", "VGB", "092"),
            new NWDCountryISO("Virgin Islands, U.S.", "VI", "VIR", "850"),
            new NWDCountryISO("Wallis and Futuna", "WF", "WLF", "876"),
            new NWDCountryISO("Western Sahara", "EH", "ESH", "732"),
            new NWDCountryISO("Yemen", "YE", "YEM", "887"),
            new NWDCountryISO("Zambia", "ZM", "ZMB", "894"),
            new NWDCountryISO("Zimbabwe", "ZW", "ZWE", "716"),
            new NWDCountryISO("Åland Islands", "AX", "ALA", "248")
        };
    }
}
