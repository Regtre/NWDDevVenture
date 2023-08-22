using Newtonsoft.Json;
using NWDFoundation.Tools;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    public partial class NWDToolbox
    {
        #region class method

        public static string JsonSerialize(object sObject)
        {
            return JsonConvert.SerializeObject(sObject, Formatting.Indented);
        }

        // Update is called once per frame
        public static T JsonDeserialize<T>(string sJSON)
        {
            return JsonConvert.DeserializeObject<T>(sJSON);
        }

        public static object CreateInstance (Type sType)
        {
            object rReturn = null;
            if (sType != null)
            {
                if (sType == typeof(string))
                {
                    rReturn = string.Empty;
                }
                else if (sType.IsArray)
                {
                    rReturn = Array.CreateInstance(sType.GetElementType(), 0);
                }
                else
                {
                    rReturn = Activator.CreateInstance(sType);
                }
            }
            return rReturn;
        }

        public static void EditorAndPlaying(string sWhere = "")
        {

#if UNITY_EDITOR
            //if (EditorApplication.isPlayingOrWillChangePlaymode == true)
            //{
            //    Debug.Log("<b>" + sWhere + "</b> <color=green>I AM IN EDITOR</color> BUT <color=green>MODE PLAYER IS OR WILL PLAYING</color>  ");
            //}
            //else
            //{
            //    Debug.Log("<b>" + sWhere + "</b> <color=green>I AM IN EDITOR</color> AND <color=red>MODE PLAYER IS NOT AND NOT SWITCH TO IT PLAYING</color> ");
            //}
            //if (EditorApplication.isPlaying == true)
            //{
            //    Debug.Log("<b>" + sWhere + "</b> <color=green>I AM IN EDITOR</color> BUT <color=green>MODE PLAYER IS PLAYING</color>  ");
            //}
            //else
            //{
            //    Debug.Log("<b>" + sWhere + "</b> <color=green>I AM IN EDITOR</color> AND <color=red>MODE PLAYER IS NOT PLAYING</color> ");
            //}
#endif

            if (Application.isEditor == true)
            {
                if (Application.isPlaying == true)
                {
                    Debug.Log("<b>" + sWhere + "</b> <color=green>I AM IN EDITOR</color> BUT <color=green>MODE PLAYER IS PLAYING</color>  ");
                }
                else
                {
                    Debug.Log("<b>" + sWhere + "</b> <color=green>I AM IN EDITOR</color> AND <color=red>MODE PLAYER IS NOT PLAYING</color> ");
                }
            }
            else
            {
                Debug.Log("<b>" + sWhere + "</b> <color=r-red>I AM NOT IN EDITOR</color>");
            }

        }

        public static Texture2D TextureFromColor(Color sColor)
        {
            Texture2D rResult = new Texture2D(1, 1);
            rResult.SetPixel(0, 0, sColor);
            rResult.Apply();
            return rResult;
        }

        public static Color MixColor(Color sColorA, Color sColorB)
        {
            Color rResult = new Color(
                Mathf.Max(sColorA.r + sColorB.r * sColorB.a, 1F),
                Mathf.Max(sColorA.g + sColorB.g * sColorB.a, 1F),
                Mathf.Max(sColorA.b + sColorB.b * sColorB.a, 1F),
                sColorA.a
            );
            return rResult;
        }

        public static Color Color255(int sR, int sG, int sB, int sA)
        {
            float tR = (float)Mathf.Max(Mathf.Min((float)sR, 255), 0) / 255F;
            float tG = (float)Mathf.Max(Mathf.Min((float)sG, 255), 0) / 255F;
            float tB = (float)Mathf.Max(Mathf.Min((float)sB, 255), 0) / 255F;
            float tA = (float)Mathf.Max(Mathf.Min((float)sA, 255), 0) / 255F;
            Color rResult = new Color(tR, tG, tB, tA);
            return rResult;
        }

        public static Color ColorWithAlpha(Color sColor, float sAlpha)
        {
            sAlpha = Mathf.Max(sAlpha, 0F);
            sAlpha = Mathf.Min(sAlpha, 1F);
            Color rResult = new Color(sColor.r, sColor.g, sColor.b, sAlpha);
            return rResult;
        }

        public static Color ColorPastelAlpha(Color sColor, float sPercent, float sAlpha)
        {
            float tPercent = Mathf.Abs(sPercent);
            float tAlpha = Mathf.Max(Mathf.Min(sAlpha, 1F), 0F);
            Color rResult = new Color(
            Mathf.Max(sColor.r * tPercent, 1F),
            Mathf.Max(sColor.g * tPercent, 1F),
            Mathf.Max(sColor.b * tPercent, 1F),
            tAlpha
        );
            return rResult;
        }

        public static Color ColorPercent(Color sColor, float sPercentR, float sPercentG, float sPercentB)
        {

            float tR = (float)Mathf.Max(Mathf.Min(sColor.r * sPercentR, 1F), 0F);
            float tG = (float)Mathf.Max(Mathf.Min(sColor.g * sPercentG, 1F), 0F);
            float tB = (float)Mathf.Max(Mathf.Min(sColor.b * sPercentB, 1F), 0F);
            float tA = (float)Mathf.Max(Mathf.Min(sColor.a, 1F), 0F);
            Color rResult = new Color(tR, tG, tB, tA);
            return rResult;
        }

        public static Color ColorInverse(Color sColor)
        {
            Color rResult = new Color(
            1F - sColor.r,
            1F - sColor.g,
            1F - sColor.b,
            sColor.a
        );
            return rResult;
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

        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_()[]{},;:!".
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomString(int sLength)
        {
            //string rReturn = string.Empty;
            StringBuilder rReturn = new StringBuilder();
            const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(tChars[UnityEngine.Random.Range(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        public static string RandomStringCypher(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string tChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            //"()[]{}" +
            //"+=_" +
            //"#$%&" +
            //"<^>" +
            //".!?:;" +
            "0123456789" +
            "";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(tChars[UnityEngine.Random.Range(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringAlpha(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string tChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(tChars[UnityEngine.Random.Range(0, tCharLenght)]);
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
            const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(tChars[UnityEngine.Random.Range(0, tCharLenght)]);
            }
            return rReturn.ToString();
        }

        public static string RandomStringNumeric(int sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string tChars = "0123456789";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(tChars[UnityEngine.Random.Range(0, tCharLenght)]);
            }
            return rReturn.ToString();
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

        //#if UNITY_EDITOR

        //        /// <summary>
        //        /// Copy all folder directories to destination
        //        /// </summary>
        //        /// <param name="sFromFolder">from folder.</param>
        //        /// <param name="sToFolder">to folder.</param>
        //        /// <param name="sImport">If set to <c>true</c> import in unity.</param>
        //        public static void CopyFolderDirectories(string sFromFolder, string sToFolder, bool sImport = true)
        //        {
        //            //Debug.Log ("copy folder from = " + sFromFolder + " to " + sToFolder);
        //            string[] tSubFoldersArray = AssetDatabase.GetSubFolders(sFromFolder);
        //            foreach (string tSubFolder in tSubFoldersArray)
        //            {
        //                string tSubFolderLast = tSubFolder.Replace(sFromFolder + "/", string.Empty);

        //                //Debug.Log ("copy sub folder from = " + tSubFolder + " to " + sToFolder + "/" + tSubFolderLast);

        //                if (AssetDatabase.IsValidFolder(sToFolder + "/" + tSubFolderLast) == false)
        //                {
        //                    AssetDatabase.CreateFolder(sToFolder, tSubFolderLast);
        //                }
        //                CopyFolderDirectories(sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
        //            }
        //        }

        //        /// <summary>
        //        /// Copy all folder files to destination
        //        /// </summary>
        //        /// <param name="sFromFolder">from folder.</param>
        //        /// <param name="sToFolder">to folder.</param>
        //        /// <param name="sImport">If set to <c>true</c> import in unity.</param>
        //        public static void CopyFolderFiles(string sFromFolder, string sToFolder, bool sImport = true)
        //        {
        //            //Debug.Log ("copy files from = " + sFromFolder + " to " + sToFolder);
        //            DirectoryInfo tDirectory = new DirectoryInfo(sFromFolder);
        //            FileInfo[] tInfo = tDirectory.GetFiles("*.*");
        //            foreach (FileInfo tFile in tInfo)
        //            {
        //                //Debug.Log ("find file = " + tFile.Name + " with extension = " + tFile.Extension);
        //                string tNewPath = sToFolder + "/" + tFile.Name;
        //                if (File.Exists(tNewPath))
        //                {
        //                    File.Delete(tNewPath);
        //                }
        //                if (tFile.Extension != ".meta" && (tFile.Name != ".DS_Store"))
        //                {
        //                    tFile.CopyTo(tNewPath);
        //                    if (sImport == true)
        //                    {
        //                        AssetDatabase.ImportAsset(tNewPath);
        //                    }
        //                }
        //            }
        //            DirectoryInfo[] tSubFoldersArray = tDirectory.GetDirectories();
        //            foreach (DirectoryInfo tSubFolder in tSubFoldersArray)
        //            {
        //                //Debug.Log ("find subfolder Name = " + tSubFolder.Name);
        //                string tSubFolderLast = tSubFolder.Name;
        //                if (AssetDatabase.IsValidFolder(sToFolder + "/" + tSubFolderLast) == false)
        //                {
        //                    AssetDatabase.CreateFolder(sToFolder, tSubFolderLast);
        //                }
        //                CopyFolderFiles(sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
        //            }
        //        }

        //        /// <summary>
        //        /// Exports the folder to destinations (use for web export)
        //        /// </summary>
        //        /// <param name="sFromFolder">from folder.</param>
        //        /// <param name="sToFolder">to folder.</param>
        //        public static void ExportCopyFolderFiles(string sFromFolder, string sToFolder)
        //        {
        //            //Debug.Log ("copy files from = " + sFromFolder + " to " + sToFolder);
        //            DirectoryInfo tDirectory = new DirectoryInfo(sFromFolder);
        //            FileInfo[] tInfo = tDirectory.GetFiles("*.*");
        //            foreach (FileInfo tFile in tInfo)
        //            {
        //                //Debug.Log ("find file = " + tFile.Name + " with extension = " + tFile.Extension);
        //                string tNewPath = sToFolder + "/" + tFile.Name.Replace(NWD.K_DOT_HTACCESS, NWD.K_HTACCESS);
        //                if (File.Exists(tNewPath))
        //                {
        //                    File.Delete(tNewPath);
        //                }
        //                if (tFile.Extension != ".meta" && tFile.Name != ".DS_Store" && tFile.Name != ".xcodeproj")
        //                {
        //                    tFile.CopyTo(tNewPath);
        //                }
        //            }
        //            DirectoryInfo[] tSubFoldersArray = tDirectory.GetDirectories();
        //            foreach (DirectoryInfo tSubFolder in tSubFoldersArray)
        //            {
        //                //Debug.Log ("find subfolder Name = " + tSubFolder.Name);
        //                string tSubFolderLast = tSubFolder.Name;
        //                // if (!tSubFolderLast.Contains(".xcodeproj"))
        //                {
        //                    if (Directory.Exists(sToFolder + "/" + tSubFolderLast) == false)
        //                    {
        //                        Directory.CreateDirectory(sToFolder + "/" + tSubFolderLast);
        //                    }
        //                    ExportCopyFolderFiles(sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
        //                }
        //            }
        //        }

        //#endif

        /// <summary>
        /// Generate unique Temporary USER ID for the device or the editor. Limit creation of account infos in editor
        /// </summary>
        //public static string GenerateUniqueAccountID(NWDAppEnvironment sAppEnvironment, bool isTemporaryAccount = true)
        //{
        //    string rReturn = string.Empty;

        //    int tTime = NWDToolbox.Timestamp() - 1492711200; // je compte depuis le 20 avril 2017 à 18h00:00

        //    string tReferenceMiddle = "9";
        //    string tReferenceEnd = tTime.ToString();


        //    switch (NWDLauncher.CompileAs())
        //    {
        //        case NWDCompileType.Editor:
        //            {
        //                tReferenceMiddle += AplhaNumericToNumeric(NWESecurityTools.GenerateSha("e5" + SystemInfo.deviceUniqueIdentifier + sAppEnvironment.Environment + "7ve").ToUpper().Substring(0, 9)); ;
        //                //tReferenceEnd += AplhaNumericToNumeric(NWESecurityTools.GenerateSha("7v5" + sAppEnvironment.Environment + "8m7").ToUpper().Substring(0, 6));
        //            }
        //            break;
        //        case NWDCompileType.PlayMode:
        //            {
        //                tReferenceMiddle += AplhaNumericToNumeric(NWESecurityTools.GenerateSha("475" + SystemInfo.deviceUniqueIdentifier + sAppEnvironment.Environment + "7u7").ToUpper().Substring(0, 9));
        //                //tReferenceEnd += AplhaNumericToNumeric(NWESecurityTools.GenerateSha("4r8" + sAppEnvironment.Environment + "6r8").ToUpper().Substring(0, 6));
        //            }
        //            break;
        //        case NWDCompileType.Runtime:
        //            {
        //                tReferenceMiddle += AplhaNumericToNumeric(NWESecurityTools.GenerateSha("4l5" + SystemInfo.deviceUniqueIdentifier + sAppEnvironment.Environment + "7t6").ToUpper().Substring(0, 9));
        //                //tReferenceEnd += AplhaNumericToNumeric(NWESecurityTools.GenerateSha("1h5" + sAppEnvironment.Environment + "4s5").ToUpper().Substring(0, 6));
        //            }
        //            break;
        //    }
        //    rReturn = NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + NWXFoundationConstants.K_MINUS + "00000" + NWXFoundationConstants.K_MINUS + tReferenceMiddle + NWXFoundationConstants.K_MINUS + tReferenceEnd;

        //    // I had temporary or new account
        //    if (isTemporaryAccount)
        //    {
        //        rReturn += NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE;
        //    }
        //    else
        //    {
        //        rReturn += NWDAccount.K_ACCOUNT_NEW_SUFFIXE;
        //    }

        //    return rReturn;
        //}

        /// <summary>
        /// Generate unique SALT
        /// </summary>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateSALT(int sFrequence)
        {
            long tUnixTimestamp = NWDTimestamp.Timestamp();
            if (sFrequence <= 0 || sFrequence >= tUnixTimestamp)
            {
                sFrequence = 600;
            }
            long rSalt = (tUnixTimestamp - (tUnixTimestamp % sFrequence));
            return rSalt.ToString();
        }

        /// <summary>
        /// Generate unique SALT
        /// </summary>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateSALTOutlined(int sFrequence, int sOutline = 0)
        {
            long tUnixTimestamp = NWDTimestamp.Timestamp();
            if (sFrequence <= 0 || sFrequence >= tUnixTimestamp)
            {
                sFrequence = 600;
            }
            long rSalt = (tUnixTimestamp - (tUnixTimestamp % sFrequence) - (sOutline % sFrequence));
            return rSalt.ToString();
        }

        /// <summary>
        /// Generate Admin hash
        /// </summary>
        /// <param name="sAdminKey">ADMIN ID</param>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateAdminHash(string sAdminKey, int sFrequence)
        {
            return NWDSecurityTools.GenerateSha(sAdminKey + GenerateSALT(sFrequence), NWDSecurityShaTypeEnum.Sha1);
        }

        public static string NewLineUnixFix(string sString)
        {
            return sString.Replace("\r\n", "\n"); // anti window bug
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
        //#if UNITY_EDITOR

        //        public static string FindOwnerServerFolder()
        //        {
        //            string tPath = NWDToolbox.FindClassFolder("NWDFindOwnerServer", "NetWorkedData_Server");
        //            if (AssetDatabase.IsValidFolder(tPath + "/Editor") == false)
        //            {
        //                AssetDatabase.CreateFolder(tPath, "Editor");
        //                AssetDatabase.ImportAsset(tPath + "/Editor");
        //            }
        //            return tPath + "/Editor";
        //        }

        //        public static string FindOwnerClassesFolder()
        //        {
        //            return NWDToolbox.FindClassFolder("NWDFindOwnerClasses", "NetWorkedData_Classes");
        //        }

        //        public static string FindOwnerConfigurationFolder()
        //        {
        //            return NWDToolbox.FindClassFolder("NWDFindOwnerConfiguration", "NetWorkedData_Configuration");
        //        }

        //        public static string FindPrivateConfigurationFolder()
        //        {
        //            return NWDToolbox.FindClassFolder("NetWorkedData_Private", "NetWorkedData_Private");
        //        }

        //        public static string FindCompileConfigurationFolder()
        //        {
        //            return NWDToolbox.FindClassFolder("NWDCompileConfiguration", "NetWorkedData_Compile");
        //        }

        //        /// <summary>
        //        /// Find the folder with FindClassName (ScriptableObject) or create the folder and the script FindClassName to find the folder if not found.
        //        /// </summary>
        //        /// <returns>The class folder.</returns>
        //        /// <param name="sFindClassName">S find class name.</param>
        //        /// <param name="sDefaultFolder">S default folder.</param>
        //        public static string FindClassFolder(string sFindClassName, string sDefaultFolder)
        //        {
        //            string tEngineRoot = "Assets";
        //            //tEngineRoot = Directory.GetParent(NWDFindPackage.SharedInstance().ScriptFolderFromAssets).ToString();
        //            string tFolder = sDefaultFolder;
        //            string tEngineRootFolder = tEngineRoot + "/" + tFolder;

        //            bool tFindClassesFolder = false;
        //            if (Type.GetType("NetWorkedData." + sFindClassName) != null)
        //            {
        //                // TODO : Change to remove invoke!
        //                tFindClassesFolder = true;
        //                Type tFindClassesType = Type.GetType("NetWorkedData." + sFindClassName);
        //                var tMethodInfo = tFindClassesType.GetMethod("PathOfPackage", BindingFlags.Public | BindingFlags.Static);
        //                if (tMethodInfo != null)
        //                {
        //                    tEngineRoot = tMethodInfo.Invoke(null, new object[] { string.Empty }) as string;
        //                }
        //                tEngineRootFolder = tEngineRoot; // root is directly the good path of final folder
        //            }
        //            if (AssetDatabase.IsValidFolder(tEngineRootFolder) == false)
        //            {
        //                //AssetDatabase.CreateFolder(tEngineRoot, tFolder);
        //                Directory.CreateDirectory(tEngineRootFolder);
        //                AssetDatabase.ImportAsset(tEngineRootFolder);
        //            }

        //            // TODO : rewrite with builderstring and NWD, NWXFoundationConstants
        //            if (tFindClassesFolder == false)
        //            {
        //                string tFindClassesClass = string.Empty +
        //                                           "using System.Collections;\n" +
        //                                           "using System.Collections.Generic;\n" +
        //                                           "using System.IO;\n" +
        //                                           "\n" +
        //                                           "using UnityEngine;\n" +
        //                                           "\n" +
        //                                           "#if UNITY_EDITOR\n" +
        //                                           "using UnityEditor;\n" +
        //                                           "\n" +
        //                                           "//=====================================================================================================================\n" +
        //                                           "namespace NetWorkedData\n" +
        //                                           "{\n" +
        //                                           "	/// <summary>\n" +
        //                                           "	/// Find package path class.\n" +
        //                                           "	/// Use the ScriptableObject to find the path of this package\n" +
        //                                           "	/// </summary>\n" +
        //                                           "	public class " + sFindClassName + " : ScriptableObject\n" +
        //                                           "	{\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// The script file path.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		public string ScriptFilePath;\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// The script folder.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		public string ScriptFolder;\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// The script folder from assets.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		public string ScriptFolderFromAssets;\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// The shared instance.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		private static " + sFindClassName + " kSharedInstance;\n" +
        //                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// Ascencor to shared instance.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		/// <returns>The shared instance.</returns>\n" +
        //                                           "		public static " + sFindClassName + " SharedInstance ()\n" +
        //                                           "		{\n" +
        //                                           "			if (kSharedInstance == null) {\n" +
        //                                           "				kSharedInstance = ScriptableObject.CreateInstance<" + sFindClassName + ">();\n" +
        //                                           "				kSharedInstance.ReadPaths ();\n" +
        //                                           "			}\n" +
        //                                           "			return kSharedInstance; \n" +
        //                                           "		}\n" +
        //                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// Reads the paths.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		public void ReadPaths ()\n" +
        //                                           "		{\n" +
        //                                           "			MonoScript tMonoScript = MonoScript.FromScriptableObject (this);\n" +
        //                                           "			ScriptFilePath = AssetDatabase.GetAssetPath (tMonoScript);\n" +
        //                                           "			FileInfo tFileInfo = new FileInfo (ScriptFilePath);\n" +
        //                                           "			ScriptFolder = tFileInfo.Directory.ToString ();\n" +
        //                                           "			ScriptFolder = ScriptFolder.Replace (\"\\\\\", \"/\");\n" +
        //                                           "			ScriptFolderFromAssets = \"Assets\"+ScriptFolder.Replace (Application.dataPath, NWEConstants.K_EMPTY_STRING);\n" +
        //                                           "		}\n" +
        //                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
        //                                           "		/// <summary>\n" +
        //                                           "		/// Packages the path.\n" +
        //                                           "		/// </summary>\n" +
        //                                           "		/// <returns>The path.</returns>\n" +
        //                                           "		/// <param name=\"sAddPath\">S add path.</param>\n" +
        //                                           "		public static string PathOfPackage (string sAddPath = NWEConstants.K_EMPTY_STRING)\n" +
        //                                           "		{\n" +
        //                                           "			return SharedInstance ().ScriptFolderFromAssets + sAddPath;\n" +
        //                                           "		}\n" +
        //                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
        //                                           "	}\n" +
        //                                           "}\n" +
        //                                           "//=====================================================================================================================\n" +
        //                                           "#endif";
        //                File.WriteAllText(tEngineRootFolder + "/" + sFindClassName + ".cs", tFindClassesClass);
        //                // write asmedf reference 
        //                File.WriteAllText(tEngineRootFolder + "/" + NWXFoundationConstants.NWDAssemblyReference + ".asmref", "{\n    \"reference\": \""+ NWXFoundationConstants.NWDAssembly + "\"\n}");
        //                // force to import this file by Unity3D
        //                AssetDatabase.ImportAsset(tEngineRootFolder + "/" + NWXFoundationConstants.NWDAssemblyReference + ".asmref");
        //                AssetDatabase.ImportAsset(tEngineRootFolder + "/" + sFindClassName + ".cs");
        //            }
        //            return tEngineRootFolder;
        //        }

        //#endif
        #endregion
    }
}