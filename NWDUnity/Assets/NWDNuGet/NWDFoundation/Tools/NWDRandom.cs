using System;
using System.Text;

namespace NWDFoundation.Tools
{
    public static class NWDRandom
    {
        #region static constants

        static readonly Random KRandom = new Random();

        #endregion

        #region static methods

        public static int Random(int sMin, int sMax)
        {
            return KRandom.Next(sMin, sMax);
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomString(int sLength)
        {
            //string rReturn = string.Empty;
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = cChars.Length;
            //Random r = new Random();
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }
        /// <summary>
        /// Return random email.
        /// </summary>
        /// <returns>The random email.</returns>
        public static string RandomEmail()
        {
            return RandomStringToken(12).ToLower() + "@" + RandomStringToken(8).ToLower() + ".com";
        }
        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringToken(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = cChars.Length;
            //Random r = new Random();
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }
        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringBase64(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/";
            int tCharLenght = cChars.Length;
            //Random r = new Random();
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringCypher(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                  //"()[]{}" +
                                  //"+=_" +
                                  //"#$%&" +
                                  //"<^>" +
                                  //".!?:;" +
                                  "0123456789" +
                                  "";
            int tCharLenght = cChars.Length;
            //Random r = new Random();
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringAlpha(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = cChars.Length;
            //Random r = new Random();
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringUnix(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "cdefhjkmnpqrtuvwxyCDEFHJKMNPQRTUVWXY379".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringNoMistake(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "cdefhjkmnpqrtuvwxyCDEFHJKMNPQRTUVWXY379";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }
        /// <summary>
        /// Return random string with length = sLength and char random in "cdefhjkmnpqrtwxyCDEFHJKMNPRTWXY379".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomCaptchaNoMistake(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "cdefhjkmnpqrtwxyCDEFHJKMNPRTWXY379";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "0123456789".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringNumeric(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "0123456789";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }
        
        public static ulong UnsignedLongNumeric(int sLength)
        {
            int tLength = Math.Max(1, Math.Min(18, sLength));
            string tReturn = RandomStringNumeric(tLength);
            ulong rReturn;
            ulong.TryParse(tReturn, out rReturn);
            return rReturn;
        }
        
        public static long LongNumeric(int sLength)
        {
            int tLength = Math.Max(1, Math.Min(18, sLength));
            string tReturn = RandomStringNumeric(tLength);
            long rReturn;
            long.TryParse(tReturn, out rReturn);
            return rReturn;
        }
        
        public static uint UnsignedIntNumeric(int sLength)
        {
            int tLength = Math.Max(1, Math.Min(9, sLength));
            string tReturn = RandomStringNumeric(tLength);
            uint rReturn;
            uint.TryParse(tReturn, out rReturn);
            return rReturn;
        }
        
        public static ushort UnsignedShortNumeric(int sLength)
        {
            int tLength = Math.Max(1, Math.Min(4, sLength));
            string tReturn = RandomStringNumeric(tLength);
            ushort rReturn;
            ushort.TryParse(tReturn, out rReturn);
            return rReturn;
        }
        public static short ShortNumeric(int sLength)
        {
            int tLength = Math.Max(1, Math.Min(4, sLength));
            string tReturn = RandomStringNumeric(tLength);
            short rReturn;
            short.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        public static int IntNumeric(int sLength)
        {
            int tLength = Math.Max(1, Math.Min(9, sLength));
            string tReturn = RandomStringNumeric(tLength);
            int rReturn;
            int.TryParse(tReturn, out rReturn);
            return rReturn;
        }
        /// <summary>
        /// Return random color in hex.
        /// </summary>
        /// <returns>The string unix.</returns>-
        public static string RandomHexadecimalColor(string sPrefix = "", bool sAlpha = false)
        {
            int tLength = 6;
            if (sAlpha == true)
            {
                tLength = 8;
            }
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "0123456789ABCDEF";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < tLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }
            return sPrefix + rReturn.ToString();
        }
        public static string RandomNetWorkedDataToken()
        {
            return RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper()+"-" + 
                   RandomStringToken(8).ToUpper();
            ;
        }

        #endregion
    }
}