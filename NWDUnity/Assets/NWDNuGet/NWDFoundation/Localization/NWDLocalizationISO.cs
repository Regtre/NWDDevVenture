namespace NWDFoundation.Localization
{
    public class NWDLocalizationISO
    {
        public static NWDLocalizationISO[] LanguageDico;

        public string Name { get; private set; }
        public string ShortCode { get; private set; }
        public string LongCode { get; private set; }
        public NWDLangage Langage { get; private set; }

        public NWDLocalizationISO(string sName, string sShortCode, string sLongCode, NWDLangage sLangage)
        {
            Name = sName;
            ShortCode = sShortCode;
            LongCode = sLongCode;
            Langage = sLangage;
        }

        static NWDLocalizationISO()
        {
            LanguageDico = new NWDLocalizationISO[]
            {
                new NWDLocalizationISO("English (U.S.)", "en", "en_US", NWDLangage.English),
                //new NWDLocalizationISO("English (British)", "en", "en_GB", NWDLangage.English),
                //new NWDLocalizationISO("English (Australian)", "en", "en_AU", NWDLangage.English),
                //new NWDLocalizationISO("English (Canadian)", "en", "en_CA", NWDLangage.English),
                //new NWDLocalizationISO("English (Indian)", "en", "en_IN", NWDLangage.English),
                new NWDLocalizationISO("French", "fr", "fr_FR", NWDLangage.French),
                //new NWDLocalizationISO("French (Canadian)", "fr", "fr_CA", NWDLangage.French),
                new NWDLocalizationISO("Spanish", "es", "es_ES", NWDLangage.Spanish),
                //new NWDLocalizationISO("Spanish (Mexico)", "es", "es_MX", NWDLangage.Spanish),
                new NWDLocalizationISO("Portuguese", "pt", "pt_PT", NWDLangage.Portuguese),
                //new NWDLocalizationISO("Portuguese (Brazil)", "pt", "pt_BR", NWDLangage.Portuguese),
                new NWDLocalizationISO("Italian", "it", "it_IT", NWDLangage.Italian),
                new NWDLocalizationISO("German", "de", "de_DE", NWDLangage.German),
                new NWDLocalizationISO("Chinese (Simplified)", "zhs", "zh_CN", NWDLangage.Chinese_Simplified),
                new NWDLocalizationISO("Chinese (Traditional)", "zht", "zh_TW", NWDLangage.Chinese_Traditional),
                new NWDLocalizationISO("Dutch", "nl", "nl_NL", NWDLangage.Dutch),
                new NWDLocalizationISO("Japanese", "ja", "ja_JP", NWDLangage.Japanese),
                new NWDLocalizationISO("Korean", "ko", "ko_KR", NWDLangage.Korean),
                new NWDLocalizationISO("Vietnamese", "vi", "vi_VN", NWDLangage.Vietnamese),
                new NWDLocalizationISO("Russian", "ru", "ru_RU", NWDLangage.Russian),
                new NWDLocalizationISO("Swedish", "sv", "sv_SE", NWDLangage.Swedish),
                new NWDLocalizationISO("Danish", "da", "da_DK", NWDLangage.Danish),
                new NWDLocalizationISO("Finnish", "fi", "fi_FI", NWDLangage.Finnish),
                new NWDLocalizationISO("Norwegian (Bokmal)", "nb", "nb_NO", NWDLangage.Norwegian),
                //new NWDLocalizationISO("Norwegian (Nynorsk)", "nb", "nn_NO", NWDLangage.Norwegian),
                new NWDLocalizationISO("Turkish", "tr", "tr_TR", NWDLangage.Turkish),
                new NWDLocalizationISO("Greek", "el", "el_GR", NWDLangage.Greek),
                new NWDLocalizationISO("Indonesian", "id", "id_ID", NWDLangage.Indonesian),
                new NWDLocalizationISO("Malay", "ms", "ms_MY", NWDLangage.Malay),
                new NWDLocalizationISO("Thai", "th", "th_TH", NWDLangage.Thai),
                new NWDLocalizationISO("Hindi", "hi", "hi_IN", NWDLangage.Hindi),
                new NWDLocalizationISO("Hungarian", "hu", "hu_HU", NWDLangage.Hungarian),
                new NWDLocalizationISO("Polish", "pl", "pl_PL", NWDLangage.Polish),
                new NWDLocalizationISO("Czech", "cs", "cs_CZ", NWDLangage.Czech),
                new NWDLocalizationISO("Slovak", "sk", "sk_SK", NWDLangage.Slovak),
                new NWDLocalizationISO("Ukrainian", "uk", "uk_UA", NWDLangage.Ukrainian),
                new NWDLocalizationISO("Croatian", "hr", "hr_HR", NWDLangage.Croatian),
                new NWDLocalizationISO("Catalan", "ca", "ca_ES", NWDLangage.Catalan),
                new NWDLocalizationISO("Romanian", "ro", "ro_RO", NWDLangage.Romanian),
                new NWDLocalizationISO("Hebrew", "he", "he_IL", NWDLangage.Hebrew),
                new NWDLocalizationISO("Arabic", "ar", "ar_AR", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Algeria)", "ar", "ar_DZ", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Bahrain)", "ar", "ar_BH", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Egypt)", "ar", "ar_EG", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Iraq)", "ar", "ar_IQ", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Jordan)", "ar", "ar_JO", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Kuwait)", "ar", "ar_KW", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Lebanon)", "ar", "ar_LB", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Libya)", "ar", "ar_LY", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Morocco)", "ar", "ar_MA", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Oman)", "ar", "ar_OM", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Qatar)", "ar", "ar_QA", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Saudi Arabia)", "ar", "ar_SA", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Sudan)", "ar", "ar_SD", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Syria)", "ar", "ar_SY", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Tunisia)", "ar", "ar_TN", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Uae)", "ar", "ar_AE", NWDLangage.Arabic),
                //new NWDLocalizationISO("Arabic(Yemen)", "ar", "ar_YE", NWDLangage.Arabic),
            };
        }
    }
}
