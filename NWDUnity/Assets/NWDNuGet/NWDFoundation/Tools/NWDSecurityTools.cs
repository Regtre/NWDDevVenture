using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Logger;

namespace NWDFoundation.Tools
{
    #region Enums
    // TODO : separate in another file
    public enum NWDSecurityShaTypeEnum
    {
        Sha1,
        Sha512,
    }

    // TODO : separate in another file
    public enum NWDSecurityAesTypeEnum
    {
        Aes128,
    }

    #endregion

    /// <summary>
    /// Security tools, use for SHA, AES (crypt, decrypt) and Base64 (encode, decode)
    /// All methods are static
    /// </summary>
    /// <example>
    /// How to use:
    /// <code>
    /// using NWDFoundation.Tools;
    /// string tEncoded = NWESecurityTools.Base64Encode("my string to encode!");
    /// </code>
    /// </example>
    public static class NWDSecurityTools
    {
        /// <summary>
        /// Encode string to Base64
        /// </summary>
        /// <param name="sPlainText">String to encode</param>
        /// <returns>A Base64 encoded string</returns>
        public static string Base64Encode(string sPlainText)
        {
            // TODO: verif if is that working?
            //sPlainText = UnityWebRequest.EscapeURL(sPlainText);
            sPlainText = Uri.EscapeUriString(sPlainText);
            var tPlainTextBytes = Encoding.UTF8.GetBytes(sPlainText); // convert to uft8 bt byte
            return Convert.ToBase64String(tPlainTextBytes, Base64FormattingOptions.None); // return the base64 text
        }

        /// <summary>
        /// Decode Base64 to string
        /// </summary>
        /// <param name="sBase64EncodedData">Base64 string</param>
        /// <returns>A string decoded</returns>
        public static string Base64Decode(string sBase64EncodedData)
        {
            var tBase64EncodedBytes = Convert.FromBase64String(sBase64EncodedData);
            return Encoding.UTF8.GetString(tBase64EncodedBytes);
        }

        public static string GenerateSha(string sPlainText, NWDSecurityShaTypeEnum sSha = NWDSecurityShaTypeEnum.Sha1)
        {
            var tData = Encoding.ASCII.GetBytes(sPlainText);
            if (sSha == NWDSecurityShaTypeEnum.Sha1)
            {
                using (SHA1 tShaManager = new SHA1Managed())
                {
                    byte[] tHash = tShaManager.ComputeHash(tData);
                    var tHashedInputStringBuilder = BitConverter.ToString(tHash);
                    return tHashedInputStringBuilder.Replace(NWDConstants.K_MINUS, string.Empty).ToLower();
                }
            }
            else
            {
                using (SHA512 tShaManager = new SHA512Managed())
                {
                    byte[] tHash = tShaManager.ComputeHash(tData);
                    var tHashedInputStringBuilder = BitConverter.ToString(tHash);
                    return tHashedInputStringBuilder.Replace(NWDConstants.K_MINUS, string.Empty).ToLower();
                }
            }
        }

        private static string KeyLengthFix(string sKey, int sSize)
        {
            string rReturn;
            if (string.IsNullOrEmpty(sKey))
            {
                sKey = string.Empty;
            }

            if (sKey.Length == sSize)
            {
                rReturn = sKey;
            }
            else if (sKey.Length > sSize)
            {
                rReturn = sKey.Substring(0, sSize);
            }
            else
            {
                rReturn = sKey;
                while (rReturn.Length < sSize)
                {
                    rReturn = rReturn + "A";
                }
            }

            return rReturn;
        }

        public static string CryptAes(string sText, string sKey, string sVector, NWDSecurityAesTypeEnum sAes = NWDSecurityAesTypeEnum.Aes128)
        {
            string rParamB64 = string.Empty;
            if (string.IsNullOrEmpty(sText) == false)
            {
                // Set AES bits size
                Int32 tAesSize = 128;
                string tKey = KeyLengthFix(sKey, 24);
                string tVector = KeyLengthFix(sVector, 16);
                // Encrypt the string to an array of bytes.
                byte[] tEncrypted = InternalCryptAes(sText, Encoding.ASCII.GetBytes(tKey), Encoding.ASCII.GetBytes(tVector), tAesSize);
                // Encode parameters
                rParamB64 = Convert.ToBase64String(tEncrypted);
            }

            return rParamB64;
        }

        public static string? DecryptAes(string sText, string sKey, string sVector, NWDSecurityAesTypeEnum sAes = NWDSecurityAesTypeEnum.Aes128)
        {
            // Decode parameters
            if (string.IsNullOrEmpty(sText) == false)
            {
                // Set AES bits size
                Int32 tAesSize = 128;
                string tKey = KeyLengthFix(sKey, 24);
                string tVector = KeyLengthFix(sVector, 16);
                //Debug.Log("sText = " + sText);
                byte[] tEncrypted = Convert.FromBase64String(sText);
                // Decrypt the string to an array of bytes.
                return InternalDecryptAes(tEncrypted, Encoding.ASCII.GetBytes(tKey), Encoding.ASCII.GetBytes(tVector), tAesSize);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Generate AES
        /// </summary>
        /// <param name="sPlainText">string Text</param>
        /// <param name="sKey">array byte Key</param>
        /// <param name="sIv">array byte Vector Key</param>
        /// <param name="sAesSize">AES size</param>
        /// <returns>A new array byte crypt from sPlainText using sKey, sIV and sAesSize</returns>
        private static byte[] InternalCryptAes(string sPlainText, byte[] sKey, byte[] sIv, Int32 sAesSize)
        {
            // Check arguments.
            if (sPlainText == null || sPlainText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(sPlainText));
            }

            if (sKey == null || sKey.Length <= 0)
            {
                throw new ArgumentNullException(nameof(sKey));
            }

            if (sIv == null || sIv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(sIv));
            }

            byte[] rEncrypted;
            using (Aes tAes = new AesManaged())
            {
                tAes.Mode = CipherMode.ECB;
                tAes.Padding = PaddingMode.PKCS7;
                tAes.KeySize = sAesSize;
                tAes.BlockSize = sAesSize;
                tAes.Key = sKey;
                tAes.IV = sIv;
                // Create a encrypt operator to perform the stream transform.
                ICryptoTransform tEncryptOperator = tAes.CreateEncryptor(tAes.Key, tAes.IV);
                // Create the streams used for encryption.
                using (MemoryStream tMsEncrypt = new MemoryStream())
                {
                    using (CryptoStream tCsEncrypt = new CryptoStream(tMsEncrypt, tEncryptOperator, CryptoStreamMode.Write))
                    {
                        using (StreamWriter tSwEncrypt = new StreamWriter(tCsEncrypt))
                        {
                            // Write all data to the stream.
                            tSwEncrypt.Write(sPlainText);
                        }

                        rEncrypted = tMsEncrypt.ToArray();
                    }
                }
            }

            return rEncrypted;
        }

        /// <summary>
        /// Decrypt AES
        /// </summary>
        /// <param name="sPlainText">string Text</param>
        /// <param name="sKey">array byte Key</param>
        /// <param name="sIv">array byte vector Key</param>
        /// <param name="sAesSize">AES size</param>
        /// <returns>A new string decrypted from sPlainText using sKey, sIV and sAesSize</returns>
        private static string? InternalDecryptAes(byte[] sPlainText, byte[] sKey, byte[] sIv, Int32 sAesSize)
        {
            // Check arguments.
            if (sPlainText == null || sPlainText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(sPlainText));
            }

            if (sKey == null || sKey.Length <= 0)
            {
                throw new ArgumentNullException(nameof(sKey));
            }

            if (sIv == null || sIv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(sIv));
            }

            string? rDecrypt = null;
            try
            {
                using (Aes tAes = new AesManaged())
                {
                    tAes.Mode = CipherMode.ECB;
                    tAes.Padding = PaddingMode.PKCS7;
                    tAes.KeySize = sAesSize;
                    tAes.BlockSize = sAesSize;
                    tAes.Key = sKey;
                    tAes.IV = sIv;
                    // Create a decrypt operator to perform the stream transform.
                    ICryptoTransform tDecryptOperator = tAes.CreateDecryptor(tAes.Key, tAes.IV);
                    // Create the streams used for decryption.
                    using (MemoryStream tMsDecrypt = new MemoryStream(sPlainText))
                    {
                        using (CryptoStream tCsDecrypt = new CryptoStream(tMsDecrypt, tDecryptOperator, CryptoStreamMode.Read))
                        {
                            using (StreamReader tSrDecrypt = new StreamReader(tCsDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                rDecrypt = tSrDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception tException)
            {
                NWDLogger.Critical(tException.ToString());
            }

            return rDecrypt;
        }

        public static string CryptSomething(string sRescueToken, ulong sProjectId, NWDEnvironmentKind sEnvironmentKind, INWDSecretKey sSecretKeyManager, INWDProjectKey sProjectKeyManager)
        {
            if (sSecretKeyManager != null && sProjectKeyManager !=null)
            {
                return NWDSecurityTools.CryptAes(sRescueToken, sSecretKeyManager.GetSecretKey(sProjectId, sEnvironmentKind),
                    sProjectKeyManager.GetProjectKey(sProjectId, sEnvironmentKind));
            }
            else
            {
                return sRescueToken;
            }
        }

       public static string HashMd5(string sInput)
        {
            using (MD5 md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sInput)))
                    .Replace("-", "");
            }
        }
       static string ComputeMd5(string sInput)
       {
           StringBuilder sb = new StringBuilder();
           // Initialize a MD5 hash object
           using (MD5 md5 = MD5.Create())
           {
               // Compute the hash of the given string
               byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(sInput));
               // Convert the byte array to string format
               foreach (byte b in hashValue) {
                   sb.Append($"{b:X2}");
               }
           }
           return sb.ToString();
       }
        
        public static string DecryptSomething(string sRescueTokenSecured, ulong sProjectId, NWDEnvironmentKind sEnvironmentKind, INWDSecretKey sSecretKeyManager, INWDProjectKey sProjectKeyManager)
        {
            if (sSecretKeyManager != null && sProjectKeyManager !=null)
            {
                string? tReturn = NWDSecurityTools.DecryptAes(sRescueTokenSecured, sSecretKeyManager.GetSecretKey(sProjectId, sEnvironmentKind), sProjectKeyManager.GetProjectKey(sProjectId, sEnvironmentKind));
                if (string.IsNullOrEmpty(tReturn))
                {
                    return sRescueTokenSecured;
                }
                else
                {
                    return tReturn;
                }
            }
            else
            {
                return sRescueTokenSecured;
            }
        }
    }
}