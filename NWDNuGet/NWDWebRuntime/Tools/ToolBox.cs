using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using NWDFoundation.Logger;
using NWDWebRuntime.Configuration;

namespace NWDWebRuntime.Tools
{
    /// <summary>
    /// Various tools allowing simple and standardized operations
    /// </summary>
    public static class NWDToolBox
    {
        public static string LimitToOneWord(string sWord)
        {
            string[] tWordsSplit = sWord.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tWordsSplit.Length <= 1)
            {
                return sWord;
            }
            else
            {
                return tWordsSplit[0];
            }
        }

        public static Dictionary<string, string?> GetPayLoad(HttpContext sHttpContext, HttpRequest sRequest)
        {
            Dictionary<string, string?> rPayLoad = new Dictionary<string, string?>();
            foreach (string? tKey in sRequest.Query.Keys)
            {
                if (rPayLoad.ContainsKey(tKey) == false && sHttpContext.Request.Query.ContainsKey(tKey))
                {
                    rPayLoad.Add(tKey, sHttpContext.Request.Query[tKey]!);
                }
                else
                {
                    NWDLogger.Warning(tKey + " already exists with value " + sHttpContext.Request.Query[tKey]);
                }
            }

            string? tPost;
            using (StreamReader tReader = new StreamReader(sRequest.Body, Encoding.ASCII))
            {
                tPost = tReader.ReadToEnd();
            }
            rPayLoad.Add("InitialPost", tPost);
            var tPostQuery = HttpUtility.ParseQueryString(tPost);
            foreach (string? tKey in tPostQuery.AllKeys)
            {
                if (tKey != null) rPayLoad.Add(tKey, tPostQuery.Get(tKey));
            }

            //foreach (string sKey in sHttpContext.Request.Form.Keys)
            //{
            //    if (rPayLoad.ContainsKey(sKey) == false)
            //    {
            //        rPayLoad.Add(sKey, sHttpContext.Request.Query[sKey]);
            //    }
            //    else
            //    {
            //        NWDLogger.WriteLine(sKey + " already exists with value " + sHttpContext.Request.Query[sKey]);
            //    }
            //}
            return rPayLoad;
        }

        public static string RemoveAccent(string sT)
        {
            string rReturn = sT;
            if (string.IsNullOrEmpty(rReturn))
            {
                return string.Empty;
            }
            rReturn = rReturn.Replace("Œ", "OE");
            rReturn = rReturn.Replace("œ", "oe");
            rReturn = rReturn.Replace("Æ", "AE");
            rReturn = rReturn.Replace("æ", "ae");

            rReturn = rReturn.Replace("Ç", "C");
            rReturn = rReturn.Replace("ç", "c");

            rReturn = rReturn.Replace("é", "e");
            rReturn = rReturn.Replace("è", "e");
            rReturn = rReturn.Replace("ê", "e");
            rReturn = rReturn.Replace("ë", "e");
            rReturn = rReturn.Replace("ì", "i");
            rReturn = rReturn.Replace("î", "i");
            rReturn = rReturn.Replace("ï", "i");
            rReturn = rReturn.Replace("ö", "o");
            rReturn = rReturn.Replace("ô", "o");
            rReturn = rReturn.Replace("ò", "o");
            rReturn = rReturn.Replace("ó", "o");
            rReturn = rReturn.Replace("ü", "u");
            rReturn = rReturn.Replace("û", "u");
            rReturn = rReturn.Replace("ù", "u");
            rReturn = rReturn.Replace("ú", "u");
            rReturn = rReturn.Replace("ä", "a");
            rReturn = rReturn.Replace("á", "a");
            rReturn = rReturn.Replace("â", "a");
            rReturn = rReturn.Replace("à", "a");


            rReturn = rReturn.Replace("É", "e");
            rReturn = rReturn.Replace("È", "e");
            rReturn = rReturn.Replace("Ê", "e");
            rReturn = rReturn.Replace("Ë", "e");
            rReturn = rReturn.Replace("Ì", "i");
            rReturn = rReturn.Replace("Î", "i");
            rReturn = rReturn.Replace("Ï", "i");
            rReturn = rReturn.Replace("Ö", "o");
            rReturn = rReturn.Replace("Ô", "o");
            rReturn = rReturn.Replace("Ò", "o");
            rReturn = rReturn.Replace("Ó", "o");
            rReturn = rReturn.Replace("Ü", "u");
            rReturn = rReturn.Replace("Û", "u");
            rReturn = rReturn.Replace("Ù", "u");
            rReturn = rReturn.Replace("Ú", "u");
            rReturn = rReturn.Replace("Ä", "a");
            rReturn = rReturn.Replace("Á", "a");
            rReturn = rReturn.Replace("Â", "a");
            rReturn = rReturn.Replace("À", "a");


            return rReturn;
        }

        public static string ExDicolatinStr(string sT)
        {
            string rReturn = sT;
            if (string.IsNullOrEmpty(rReturn))
            {
                return string.Empty;
            }
            rReturn = rReturn.Replace("\\r", "\n");
            rReturn = rReturn.Replace("\\n", "\n");
            rReturn = rReturn.Replace("\n\n", "\n");
            rReturn = rReturn.Replace("\n\n", "\n");
            rReturn = rReturn.Replace("\n\n", "\n");
            rReturn = rReturn.Replace("\n\n", "\n");

            rReturn = rReturn.Replace("Ã¦", "æ");
            rReturn = rReturn.Replace("Ã§", "ç");
            rReturn = rReturn.Replace("Ã©", "é");
            rReturn = rReturn.Replace("Ã¨", "è");
            rReturn = rReturn.Replace("Ãª", "ê");
            rReturn = rReturn.Replace("Ã«", "ë");
            rReturn = rReturn.Replace("Ã¬", "ì");
            rReturn = rReturn.Replace("Ã®", "î");
            rReturn = rReturn.Replace("Ã¯", "ï");
            rReturn = rReturn.Replace("Ã¶", "ö");
            rReturn = rReturn.Replace("Ã´", "ô");
            rReturn = rReturn.Replace("Ã²", "ò");
            rReturn = rReturn.Replace("Ã³", "ó");
            rReturn = rReturn.Replace("Ã¼", "ü");
            rReturn = rReturn.Replace("Ã»", "û");
            rReturn = rReturn.Replace("Ã¹", "ù");
            rReturn = rReturn.Replace("Ãº", "ú");
            rReturn = rReturn.Replace("Ã¦", "æ");
            rReturn = rReturn.Replace("Ã¤", "ä");
            rReturn = rReturn.Replace("Ã¡", "á");
            rReturn = rReturn.Replace("Ã¢", "â");
            rReturn = rReturn.Replace("Å", "œ");
            rReturn = rReturn.Replace("Ã ", "à");

            return rReturn;
        }
        public static string YearToRoman()
        {
            return ToRoman(DateTime.Now.Year);
        }

        private static string ToRoman(int sNumber)
        {
            if ((sNumber < 0) || (sNumber > 3999)) throw new ArgumentOutOfRangeException("sNumber");
            if (sNumber < 1) return string.Empty;
            if (sNumber >= 1000) return "M" + ToRoman(sNumber - 1000);
            if (sNumber >= 900) return "CM" + ToRoman(sNumber - 900);
            if (sNumber >= 500) return "D" + ToRoman(sNumber - 500);
            if (sNumber >= 400) return "CD" + ToRoman(sNumber - 400);
            if (sNumber >= 100) return "C" + ToRoman(sNumber - 100);
            if (sNumber >= 90) return "XC" + ToRoman(sNumber - 90);
            if (sNumber >= 50) return "L" + ToRoman(sNumber - 50);
            if (sNumber >= 40) return "XL" + ToRoman(sNumber - 40);
            if (sNumber >= 10) return "X" + ToRoman(sNumber - 10);
            if (sNumber >= 9) return "IX" + ToRoman(sNumber - 9);
            if (sNumber >= 5) return "V" + ToRoman(sNumber - 5);
            if (sNumber >= 4) return "IV" + ToRoman(sNumber - 4);
            return "I" + ToRoman(sNumber - 1);
        }

        public static string? GetAction(HttpContext sHttpContext)
        {
            return sHttpContext.Request.RouteValues["action"]?.ToString();
        }

        public static string? GetController(HttpContext sHttpContext)
        {
            return sHttpContext.Request.RouteValues["controller"]?.ToString();
        }

        public static int ToTimestampUnix(DateTime sDateTime)
        {
            return (int)sDateTime.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        private static int GetTimestampUnix()
        {
            return (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        }

        public static DateTime TimestampUnixToDatetime(int sTimeStamp)
        {
            return DateTime.UnixEpoch.AddSeconds(sTimeStamp);
        }

        public static string NewReference(string sTrigram)
        {
            //return sTrigram + "-" + RandomStringNumber(7) + "-" + RandomStringNumber(14);
            return sTrigram + "-" + GetTimestampUnix().ToString() + "-" + RandomStringNumber(14);
        }

        public static string RandomStringNumber(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "0123456789";
            int tCharLenght = cChars.Length;
            Random tRand = new Random();
            while (rReturn.Length < sLength)
            {
                int tR = tRand.Next(0, tCharLenght);
                rReturn.Append(cChars[tR]);
            }
            return rReturn.ToString();
        }

        public static string RandomString(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = cChars.Length;
            Random tRand = new Random();
            while (rReturn.Length < sLength)
            {
                int tR = tRand.Next(0, tCharLenght);
                rReturn.Append(cChars[tR]);
            }
            return rReturn.ToString();
        }

        private static int _UniqueCounter;
        public static string UniqueRandomString(int sLength)
        {
            _UniqueCounter++;
            if (_UniqueCounter < 1)
            {
                _UniqueCounter = 1;
            }
            StringBuilder rReturn = new StringBuilder();

            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = cChars.Length;
            Random tRand = new Random();

            rReturn.Append(cChars[_UniqueCounter % tCharLenght]);

            while (rReturn.Length < sLength)
            {
                int tR = tRand.Next(0, tCharLenght);
                rReturn.Append(cChars[tR]);
            }
            return rReturn.ToString();
        }

        public static string NoErrorRandomPassword(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "ACDEFHJKLMNPQRTUVWXY3479";
            int tCharLenght = cChars.Length;
            Random tRand = new Random();
            while (rReturn.Length < sLength)
            {
                int tR = tRand.Next(0, tCharLenght);
                rReturn.Append(cChars[tR]);
            }
            return rReturn.ToString();
        }

        public static string RandomKey(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "ACDEFHJKLMNPQRTUVWX";
            int tCharLenght = cChars.Length;
            Random tRand = new Random();
            while (rReturn.Length < sLength)
            {
                int tR = tRand.Next(0, tCharLenght);
                rReturn.Append(cChars[tR]);
            }
            return rReturn.ToString();
        }

        static List<string> _KCountryList = new List<string>();
        public static List<string> CountryList()
        {
            return _KCountryList;
        }

        private static Dictionary<string, string> _KCountryCode = new Dictionary<string, string>();

        private static Dictionary<string, string> CountryCode()
        {
            return _KCountryCode;
        }
        public static string GetCodeForCountry(string sCountry)
        {
            string rReturn = string.Empty;
            if (CountryCode().ContainsKey(sCountry))
            {
                rReturn = _KCountryCode[sCountry];
            }
            return rReturn;
        }
        static void GetCountryListB()
        {
            //create a new Generic list to hold the country names returned
            List<string> tCultureList = new List<string>();
            Dictionary<string, string> tCultureCode = new Dictionary<string, string>();

            //create an array of CultureInfo to hold all the cultures found, these include the users local culture, and all the
            //cultures installed with the .Net Framework
            CultureInfo[] tCultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

            //loop through all the cultures found
            foreach (CultureInfo tCulture in tCultures)
            {
                //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                //to the RegionInfo constructor to gain access to the information for that culture
                RegionInfo tRegion = new RegionInfo(tCulture.Name);

                //make sure out generic list doesnt already
                //contain this country
                if (!(tCultureList.Contains(tRegion.EnglishName)))
                {
                    //not there so add the EnglishName (http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.englishname.aspx)
                    //value to our generic list
                    tCultureList.Add(tRegion.EnglishName);
                    tCultureCode.Add(tRegion.EnglishName, tRegion.TwoLetterISORegionName);
                }
            }
            tCultureList.Sort();
            _KCountryList = tCultureList;
            _KCountryCode = tCultureCode;
        }

        // private const string K_Minus = "-";

        // public static string Hash(string sPlainText)
        // {
        //     var tData = Encoding.ASCII.GetBytes(sPlainText + NWDWebRuntimeConfiguration.KConfig.HashSalt);
        //     var tDataB = Encoding.ASCII.GetBytes(NWDWebRuntimeConfiguration.KConfig.HashSaltSecond + sPlainText);
        //     HashAlgorithm tSha = SHA256.Create();
        //     byte[] tHash = tSha.ComputeHash(tData);
        //     var rHashedInputStringBuilder = BitConverter.ToString(tHash).Replace(K_Minus, string.Empty).ToLower();
        //     byte[] tHashB = tSha.ComputeHash(tDataB);
        //     var rHashedInputStringBuilderB = BitConverter.ToString(tHashB).Replace(K_Minus, string.Empty).ToLower();
        //     return rHashedInputStringBuilder + rHashedInputStringBuilderB;
        // }
        //
        //
        // public static string Integrity(string sPlainText)
        // {
        //     var tData = Encoding.ASCII.GetBytes(sPlainText + NWDWebRuntimeConfiguration.KConfig.HashSalt);
        //     HashAlgorithm tSha = SHA256.Create();
        //     byte[] tHash = tSha.ComputeHash(tData);
        //     var rHashedInputStringBuilder = BitConverter.ToString(tHash).Replace(K_Minus, string.Empty).ToLower();
        //     return rHashedInputStringBuilder;
        // }
    }
}
