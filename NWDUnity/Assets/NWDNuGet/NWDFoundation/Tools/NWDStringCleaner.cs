using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace NWDFoundation.Tools
{
    public partial class NWDStringCleaner
    {
        #region static method

        public static string RemoveDiacritics(string sText)
        {
            string rReturn = string.Empty;
            if (string.IsNullOrWhiteSpace(sText))
            {
                rReturn = sText;
            }
            else
            {
                sText = sText.Normalize(NormalizationForm.FormD);
                var chars = sText.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                rReturn = new string(chars).Normalize(NormalizationForm.FormC);
                rReturn = AplhaNumericCleaner(rReturn);
            }
            return rReturn;
        }
        
        public static string CleanDNS(string sServerDNS)
        {
            string rServerDNS = sServerDNS;

            if (string.IsNullOrEmpty(sServerDNS) == false)
            {
                rServerDNS = rServerDNS.TrimEnd('/');
                if (rServerDNS.StartsWith("https://", StringComparison.Ordinal))
                {
                    rServerDNS = rServerDNS.Substring("https://".Length);
                }
                if (rServerDNS.StartsWith("http://", StringComparison.Ordinal))
                {
                    rServerDNS = rServerDNS.Substring("http://".Length);
                }
                if (rServerDNS.StartsWith("http://", StringComparison.Ordinal))
                {
                    rServerDNS = rServerDNS.Substring("http://".Length);
                }
            }
            return rServerDNS;
        }
        
        /// <summary>
        /// Protect the text for the separator usage.
        /// </summary>
        /// <returns>The protect text.</returns>
        /// <param name="sText">text.</param>
        public static string TextRemoveSeparator(string sText)
        {
            string rText = sText;
            if (string.IsNullOrEmpty(sText) == false)
            {
                rText = rText.Replace(NWDConstants.kFieldSeparatorA, string.Empty);
                rText = rText.Replace(NWDConstants.kFieldSeparatorB, string.Empty);
                rText = rText.Replace(NWDConstants.kFieldSeparatorC, string.Empty);
                // new adds
                rText = rText.Replace(NWDConstants.kFieldSeparatorD, string.Empty);
                rText = rText.Replace(NWDConstants.kFieldSeparatorE, string.Empty);
            }
            return rText;
        }
        
        /// <summary>
        /// Protect the text for the separator usage.
        /// </summary>
        /// <returns>The protect text.</returns>
        /// <param name="sText">text.</param>
        public static string TextProtect(string sText)
        {
            string rText = sText;
            if (string.IsNullOrEmpty(sText) == false)
            {
                rText = rText.Replace(NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorASubstitute);
                rText = rText.Replace(NWDConstants.kFieldSeparatorB, NWDConstants.kFieldSeparatorBSubstitute);
                rText = rText.Replace(NWDConstants.kFieldSeparatorC, NWDConstants.kFieldSeparatorCSubstitute);
                // new adds
                rText = rText.Replace(NWDConstants.kFieldSeparatorD, NWDConstants.kFieldSeparatorDSubstitute);
                rText = rText.Replace(NWDConstants.kFieldSeparatorE, NWDConstants.kFieldSeparatorESubstitute);
            }
            return rText;
        }
        
        /// <summary>
        /// Unprotect the text for the separator usage.
        /// </summary>
        /// <returns>The unprotect text.</returns>
        /// <param name="sText">text.</param>
        public static string TextUnprotect(string sText)
        {
            string rText = sText;
            if (string.IsNullOrEmpty(sText) == false)
            {
                rText = rText.Replace(NWDConstants.kFieldSeparatorASubstitute, NWDConstants.kFieldSeparatorA);
                rText = rText.Replace(NWDConstants.kFieldSeparatorBSubstitute, NWDConstants.kFieldSeparatorB);
                rText = rText.Replace(NWDConstants.kFieldSeparatorCSubstitute, NWDConstants.kFieldSeparatorC);
                // new adds
                rText = rText.Replace(NWDConstants.kFieldSeparatorDSubstitute, NWDConstants.kFieldSeparatorD);
                rText = rText.Replace(NWDConstants.kFieldSeparatorESubstitute, NWDConstants.kFieldSeparatorE);
            }
            return rText;
        }
        
        /// <summary>
        /// Protect the text for CSV export.
        /// </summary>
        /// <returns>The CSV protect.</returns>
        /// <param name="sText">S text.</param>
        public static string TextCSVProtect(string sText)
        {
            string rText = sText;
            rText = rText.Replace(NWDConstants.kStandardSeparator, NWDConstants.kStandardSeparatorSubstitute);

            return rText;
        }
        
        /// <summary>
        /// Unprotect the text from CSV import.
        /// </summary>
        /// <returns>The CSV unprotect.</returns>
        /// <param name="sText">S text.</param>
        public static string TextCSVUnprotect(string sText)
        {
            string rText = sText;
            rText = rText.Replace(NWDConstants.kStandardSeparatorSubstitute, NWDConstants.kStandardSeparator);
            return rText;
        }
        
        /// <summary>
        /// Bools to string.
        /// </summary>
        /// <returns>The value of sBoolean to numerical string "0" if false, "1" if true.</returns>
        /// <param name="sBoolean">If set to <c>true</c> s boolean.</param>
        public static string BoolToNumericalString(bool sBoolean)
        {
            if (sBoolean == false)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        static Regex URLCleanerRgx = new Regex("[^a-zA-Z0-9-_\\:\\/\\.]");
        
        public static string URLCleaner(string sString)
        {
            return URLCleanerRgx.Replace(sString, string.Empty);
        }
        static Regex EmailCleanerRgx = new Regex("[^a-zA-Z0-9-_@\\.]");
        
        public static string EmailCleaner(string sString)
        {
            return EmailCleanerRgx.Replace(sString, string.Empty);
        }
        
        static Regex UnixCleanerRgx = new Regex("[^a-zA-Z0-9_]");
        
        public static string UnixCleaner(string sString)
        {
            if (sString != null)
            {
                return UnixCleanerRgx.Replace(sString, string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }
        
        static Regex AplhaCleanerRgx = new Regex("[^a-zA-Z]");
        
        public static string AplhaCleaner(string sString)
        {
            return AplhaCleanerRgx.Replace(sString, string.Empty);
        }
        
        static Regex AplhaNumericCleanerRgx = new Regex("[^a-zA-Z0-9]");
        
        public static string AplhaNumericCleaner(string sString)
        {
            return AplhaNumericCleanerRgx.Replace(sString, string.Empty);
        }
        
        static Regex AplhaNumericToNumericRgx = new Regex("[^a-zA-Z0-9]");
        
        public static string AplhaNumericToNumeric(string sString)
        {
            string rReturn = AplhaNumericToNumericRgx.Replace(sString, string.Empty).ToUpper();
            rReturn = rReturn.Replace("A", "1");
            rReturn = rReturn.Replace("B", "2");
            rReturn = rReturn.Replace("C", "7");
            rReturn = rReturn.Replace("D", "8");
            rReturn = rReturn.Replace("E", "5");
            rReturn = rReturn.Replace("F", "4");
            rReturn = rReturn.Replace("G", "6");
            rReturn = rReturn.Replace("H", "9");
            rReturn = rReturn.Replace("I", "1");
            rReturn = rReturn.Replace("J", "4");
            rReturn = rReturn.Replace("K", "3");
            rReturn = rReturn.Replace("L", "5");
            rReturn = rReturn.Replace("M", "7");
            rReturn = rReturn.Replace("N", "8");
            rReturn = rReturn.Replace("O", "6");
            rReturn = rReturn.Replace("P", "5");
            rReturn = rReturn.Replace("Q", "6");
            rReturn = rReturn.Replace("R", "4");
            rReturn = rReturn.Replace("S", "9");
            rReturn = rReturn.Replace("T", "4");
            rReturn = rReturn.Replace("U", "1");
            rReturn = rReturn.Replace("V", "1");
            rReturn = rReturn.Replace("W", "4");
            rReturn = rReturn.Replace("X", "9");
            rReturn = rReturn.Replace("Y", "1");
            rReturn = rReturn.Replace("Z", "9");
            return rReturn;
        }
        
        static Regex SaltCleanerRgx = new Regex("[^a-zA-Z0-9 -_\\(\\)\\[\\]\\,\\;\\:\\!\\.]");
        
        public static string SaltCleaner(string sString)
        {
            //NWDBenchmark.Start();
            string rReturn = SaltCleanerRgx.Replace(sString, string.Empty);
            //NWDBenchmark.Finish();
            return rReturn;
        }
        
        /// <summary>
        /// Splits the string by Camel Case format.
        /// </summary>
        /// <returns>The camel case.</returns>
        /// <param name="input">Input.</param>
        public static string SplitCamelCase(string input)
        {
            string rReturn = Regex.Replace(input, "([A-Z])", " $1", RegexOptions.ECMAScript).Trim();
            rReturn = rReturn.Replace("_", string.Empty);
            return rReturn;
        }
        
        public static string NewLineUnixFix(string sString)
        {
            return sString.Replace("\r\n", "\n");
        }
        
        public static string CSharpFormat(string sString)
        {
            //NWDBenchmark.Start();
            StringBuilder rReturn = new StringBuilder();
            int tIndentCount = 0;
            string tString = NewLineUnixFix(sString);
            string[] tLines = tString.Split(new string[] { "\n", "\r" }, StringSplitOptions.None);
            foreach (string tLine in tLines)
            {
                if (tLine.Contains("{"))
                {
                    tIndentCount++;
                }
                if (tLine.Contains("}"))
                {
                    tIndentCount--;
                }
                for (int i = 0; i < tIndentCount; i++)
                {
                    rReturn.Append("\t");
                }
                //rReturn.Append(tLine.Replace("\t",""));
                rReturn.Append(tLine);
                rReturn.Append(NWDConstants.K_ReturnLine);
                if (tLine.Contains("{"))
                {
                    tIndentCount++;
                }
                if (tLine.Contains("}"))
                {
                    tIndentCount--;
                }
            }
            //NWDBenchmark.Finish();
            return rReturn.ToString().TrimEnd(new char[] { '\n', '\r' });
        }
        #endregion
    }
}