namespace NWDWebRuntime.Managers;


    public static class NWDEmailTemplateManager
    {
        public const string K_DEFAULT = "K_DEFAULT";
        public const string K_CONTACT_US = "K_CONTACT_US";
        public const string K_SIGN_UP = "K_SIGN_UP";
        public const string K_SIGN_IN = "K_SIGN_IN";
        public const string K_LOST_SIGN = "K_LOST_SIGN";
        static private Dictionary<string,NWDEmailTemplate> AllEmailTemplate = new Dictionary<string,NWDEmailTemplate>();
        static public NWDEmailTemplate Create(string sKey,string sSubject, string sMessage, string sHeader = "", string sFooter ="")
        {
            NWDEmailTemplate rReturn =  new NWDEmailTemplate(sSubject, sMessage);
            if (AllEmailTemplate.ContainsKey(sKey) == false)
            {
                AllEmailTemplate.Add(sKey, rReturn);
            }
            return rReturn;
        }

        static public Dictionary<string,NWDEmailTemplate> GetAll()
        {
            Dictionary<string, NWDEmailTemplate> rReturn = new Dictionary<string, NWDEmailTemplate>();
            foreach (KeyValuePair<string, NWDEmailTemplate> tItem in AllEmailTemplate)
            {
                rReturn.Add(tItem.Key, new NWDEmailTemplate(tItem.Value)); 
            }
            return rReturn;
        }
        static public NWDEmailTemplate GetCopyByKey(string sKey)
        {
            if (AllEmailTemplate.ContainsKey(sKey))
            {
                return new NWDEmailTemplate(AllEmailTemplate[sKey]);
            }
            return new NWDEmailTemplate(Default);
        }
        public static NWDEmailTemplate Default = Create(K_DEFAULT,NWDEmailTemplate.K_SUBJECT_TAG, "<h1>Hi!</h1><div>" + NWDEmailTemplate.K_MESSAGE_TAG + "</div>");
        public static NWDEmailTemplate ContactUs = Create(K_CONTACT_US, NWDEmailTemplate.K_SUBJECT_TAG, NWDEmailTemplate.K_MESSAGE_TAG);
        public static NWDEmailTemplate SignUp = Create(K_SIGN_UP,NWDEmailTemplate.K_SUBJECT_TAG, NWDEmailTemplate.K_MESSAGE_TAG);
        public static NWDEmailTemplate SignIn = Create(K_SIGN_IN,NWDEmailTemplate.K_SUBJECT_TAG, NWDEmailTemplate.K_MESSAGE_TAG);
        public static NWDEmailTemplate LostSign = Create(K_LOST_SIGN,NWDEmailTemplate.K_SUBJECT_TAG, NWDEmailTemplate.K_MESSAGE_TAG);
    }