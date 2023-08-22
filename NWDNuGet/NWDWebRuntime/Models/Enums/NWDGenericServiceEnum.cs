using NWDFoundation.Tools;

namespace NWDWebRuntime.Models.Enums
{
    public class NWDGenericServiceEnum 
    {
        private static Dictionary<long, NWDGenericServiceEnum> KList = new Dictionary<long, NWDGenericServiceEnum>();
        private static Dictionary<string, NWDGenericServiceEnum> KStringList = new Dictionary<string, NWDGenericServiceEnum>();
        
        public static NWDGenericServiceEnum Admin = NWDGenericServiceEnum.Create(-11, "Admin Generic");
        public static NWDGenericServiceEnum Marketing = NWDGenericServiceEnum.Create(-22, "Marketing Generic");
        
        static NWDGenericServiceEnum()
        {
            KList = new Dictionary<long, NWDGenericServiceEnum>();
            KStringList = new Dictionary<string, NWDGenericServiceEnum>();
        }

        /// <summary>
        /// The value of the static enum instance
        /// </summary>
        public long Value { set; get; }

        /// <summary>
        /// The name of this enum
        /// </summary>
        public string Name { set; get; } = string.Empty;
        
        public static NWDGenericServiceEnum? GetForValue(long sId)
        {
            NWDGenericServiceEnum? rReturn = null;
            if (KList.ContainsKey(sId))
            {
                rReturn = KList[sId];
            }

            return rReturn;
        }

        public static NWDGenericServiceEnum? GetForValue(string sName)
        {
            string tCleanedName = NWDStringCleaner.UnixCleaner(sName);
            NWDGenericServiceEnum? rReturn = null;
            if (KStringList.ContainsKey(tCleanedName))
            {
                rReturn = KStringList[tCleanedName];
            }

            return rReturn;
        }
        
        public static NWDGenericServiceEnum? Create(int sId, string sName)
        {
            string tCleanedName = NWDStringCleaner.UnixCleaner(sName);
            NWDGenericServiceEnum? rReturn = null;
            if (KList.ContainsKey(sId) == false && KStringList.ContainsKey(tCleanedName) == false)
            {
                rReturn = Add(sId, tCleanedName);
            }
            else
            {
                int tId = sId;
                string tName = tCleanedName;
                if (KList.ContainsKey(tId) == true)
                {
                    tId = 10 + tId;
                }

                if (KStringList.ContainsKey(tName) == true)
                {
                    tName = tName + "_" + tId;
                }

                rReturn = Create(tId, tName);
            }

            return rReturn;
        }
        
        protected static NWDGenericServiceEnum? Add(int sId, string sName)
        {
            string tCleanedName = NWDStringCleaner.UnixCleaner(sName);
            NWDGenericServiceEnum? rReturn = null;
            if (KList.ContainsKey(sId) == false && KStringList.ContainsKey(tCleanedName) == false)
            {
                rReturn = Activator.CreateInstance(typeof(NWDGenericServiceEnum)) as NWDGenericServiceEnum;
                if (rReturn != null)
                {
                    rReturn.Value = sId;
                    rReturn.Name = tCleanedName;
                    KList.Add(sId, rReturn);
                    KStringList.Add(tCleanedName, rReturn);
                }
            }
            else
            {
                if (KList.ContainsKey(sId))
                {
                    rReturn = KList[sId];
                }
                else if (KStringList.ContainsKey(tCleanedName))
                {
                    rReturn = KStringList[tCleanedName];
                }
            }

            return rReturn;
        }
        public NWDGenericServiceEnum()
        {
            
        }
    }
}