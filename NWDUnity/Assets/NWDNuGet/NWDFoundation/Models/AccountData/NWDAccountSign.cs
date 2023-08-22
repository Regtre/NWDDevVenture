using System;
using System.Text;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDAccountSign : NWDAccountData
    {
        #region properties

        /// <summary>
        /// Name of sign (to show in list)
        /// </summary>
        public string Name { set; get; } = string.Empty;

        /// <summary>
        /// Sign Kind 
        /// </summary>
        public NWDAccountSignType SignType { set; get; } = NWDAccountSignType.None;

        /// <summary>
        /// hash for the real sign
        /// </summary>
        public string SignHash { set; get; } = string.Empty;

        /// <summary>
        /// hash for the email if email is use (to check duplicate)
        /// </summary>
        public string RescueHash { set; get; } = string.Empty;

        /// <summary>
        /// hash for the login if login is use (to check duplicate)
        /// </summary>
        public string LoginHash { set; get; } = string.Empty;

        /// <summary>
        /// if ok => Associated
        /// </summary>
        public NWDAccountSignAction SignStatus { set; get; } = NWDAccountSignAction.None;

        #region For futur rescue when use email/password by email rescue TokenRescue (limit in time by TokenRescueLimit)

        /// <summary>
        /// the token send if rescue asked
        /// </summary>
        public string TokenRescue { set; get; } = string.Empty;

        /// <summary>
        /// the dateTimeLimit to used the TokenRescue token (24h?)
        /// </summary>
        public int TokenRescueLimit { set; get; }

        #endregion

        #region For futur check when use email/password with check by email TokenVerif (limit in time by TokenVerifLimit)

        /// <summary>
        /// the token send if rescue asked
        /// </summary>
        // TODO : Rename to TokenVerification and rename column in Database by operation
        public string TokenVerif { set; get; } = string.Empty;

        /// <summary>
        /// the dateTimeLimit to used the TokenRescue token (24h?)
        /// </summary>
        // TODO : Rename to TokenVerificationLimit and rename column in Database by operation
        public int TokenVerifLimit { set; get; }

        /// <summary>
        /// hash for the real sign
        /// </summary>
        // TODO : Rename to SignVerificationHash and rename column in Database by operation
        public string SignVerifHash { set; get; } = string.Empty;

        /// <summary>
        /// hash for the email if email is use (to check duplicate)
        /// </summary>
        // TODO : Rename to RescueVerificationHash and rename column in Database by operation
        public string RescueVerifHash { set; get; } = string.Empty;

        /// <summary>
        /// hash for the login if login is use (to check duplicate)
        /// </summary>
        // TODO : Rename to LoginVerificationHash and rename column in Database by operation
        public string LoginVerifHash { set; get; } = string.Empty;

        #endregion

        #endregion


        #region Static methods

        public static string EmailToPartialString(string sOriginalEmail)
        {
            StringBuilder rReturn = new StringBuilder();

            if (sOriginalEmail.Length > 6)
            {
                rReturn.Append(sOriginalEmail[0]);
                rReturn.Append(sOriginalEmail[1]);
                for (int t = 2; t < sOriginalEmail.Length - 2; t++)
                {
                    if (sOriginalEmail[t] == '@')
                    {
                        rReturn.Append("@");
                    }
                    else if (sOriginalEmail[t] == ' ')
                    {
                        // It's a login email password case ... 
                        rReturn.Append(" / ");
                        if (sOriginalEmail.Length >= t + 4)
                        {
                            t++;
                            rReturn.Append(sOriginalEmail[t]);
                            t++;
                            rReturn.Append(sOriginalEmail[t]);
                        }
                    }
                    else if (sOriginalEmail[t] == '.')
                    {
                        rReturn.Append(".");
                    }
                    else
                    {
                        rReturn.Append("•");
                    }
                }

                rReturn.Append(sOriginalEmail[^2]);
                rReturn.Append(sOriginalEmail[^1]);
            }
            else
            {
                rReturn.Append(sOriginalEmail);
            }

            return rReturn.ToString();
        }

        #endregion


        #region Static Constructors

        /// <summary>
        /// Create a sign for device by device unique identifier (UDID) and associate device's name
        /// </summary>
        /// <param name="sDeviceId"></param>
        /// <param name="sProject"></param>
        /// <param name="sDeviceName"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateDeviceId(string sDeviceId, ulong sProject, string? sDeviceName = null)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.DeviceId);
            rReturn.ProjectId = sProject;
            if (string.IsNullOrEmpty(sDeviceName))
            {
                sDeviceName = NWDAccountSignType.DeviceId.ToString();
            }
            rReturn.Name = NWDSecurityTools.CryptAes(sDeviceName, sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.DeviceId;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sDeviceId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sDeviceId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sDeviceId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sDeviceId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sDeviceId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sDeviceId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with email and password
        /// </summary>
        /// <param name="sEmail"></param>
        /// <param name="sPassword"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateEmailPassword(string sEmail, string sPassword, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(EmailToPartialString(sEmail), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.EmailPassword;
            rReturn.LoginHash = NWDSecurityTools.GenerateSha(sEmail + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sEmail);
            rReturn.RescueHash = NWDSecurityTools.GenerateSha(sEmail + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sEmail);
            rReturn.SignHash = NWDSecurityTools.GenerateSha(sEmail + sPassword + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sPassword + sEmail);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with login, email and password
        /// </summary>
        /// <param name="sLogin"></param>
        /// <param name="sEmail"></param>
        /// <param name="sPassword"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateLoginEmailPassword(string sLogin, string sEmail, string sPassword, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(sLogin + " " + EmailToPartialString(sEmail), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.LoginEmailPassword;
            rReturn.LoginHash = NWDSecurityTools.GenerateSha(sLogin + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sLogin);
            rReturn.RescueHash = NWDSecurityTools.GenerateSha(sEmail + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sEmail);
            rReturn.SignHash = NWDSecurityTools.GenerateSha(sPassword + sEmail + sLogin + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sPassword + sLogin);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with with login and password
        /// </summary>
        /// <param name="sLogin"></param>
        /// <param name="sPassword"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateLoginPassword(string sLogin, string sPassword, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(sLogin, sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.LoginPassword;
            rReturn.LoginHash = NWDSecurityTools.GenerateSha(sLogin + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sLogin);
            rReturn.RescueHash = NWDSecurityTools.GenerateSha(sLogin + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sLogin);
            rReturn.SignHash = NWDSecurityTools.GenerateSha(sPassword + sLogin + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sPassword + sLogin);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with social id (facebook)
        /// </summary>
        /// <param name="sFacebookId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateFacebook(string sFacebookId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.Facebook);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.Facebook.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.Facebook;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sFacebookId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sFacebookId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sFacebookId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sFacebookId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sFacebookId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sFacebookId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }
        /// <summary>
        /// Create a sign with social id (facebook)
        /// </summary>
        /// <param name="sFacebookId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateDiscord(string sFacebookId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.Discord);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.Discord.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.Discord;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sFacebookId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sFacebookId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sFacebookId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sFacebookId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sFacebookId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sFacebookId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with social id (Google)
        /// </summary>
        /// <param name="sGoogleId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateGoogle(string sGoogleId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.Google);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.Google.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.Google;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sGoogleId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sGoogleId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sGoogleId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sGoogleId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sGoogleId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sGoogleId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with social id (AppleID)
        /// </summary>
        /// <param name="sAppleId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateApple(string sAppleId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.Apple);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.Apple.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.Apple;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sAppleId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sAppleId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sAppleId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sAppleId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sAppleId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sAppleId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with social id (Microsoft)
        /// </summary>
        /// <param name="sMicrosoftId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateMicrosoft(string sMicrosoftId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.Microsoft);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.Microsoft.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.Microsoft;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sMicrosoftId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sMicrosoftId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sMicrosoftId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sMicrosoftId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sMicrosoftId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sMicrosoftId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with social id (Twitter)
        /// </summary>
        /// <param name="sTwitterId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateTwitter(string sTwitterId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.Twitter);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.Twitter.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.Twitter;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sTwitterId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sTwitterId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sTwitterId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sTwitterId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sTwitterId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sTwitterId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        /// <summary>
        /// Create a sign with social id (LinkedIn)
        /// </summary>
        /// <param name="sLinkedInId"></param>
        /// <param name="sProject"></param>
        /// <returns></returns>
        public static NWDAccountSign CreateLinkedIn(string sLinkedInId, ulong sProject)
        {
            NWDAccountSign rReturn = new NWDAccountSign();
            string tType = DeviceTypeObfuscation(NWDAccountSignType.LinkedIn);
            rReturn.ProjectId = sProject;
            rReturn.Name = NWDSecurityTools.CryptAes(NWDAccountSignType.LinkedIn.ToString(), sProject.ToString(), sProject.ToString());
            rReturn.SignType = NWDAccountSignType.LinkedIn;
            rReturn.LoginHash = tType + "-" + NWDSecurityTools.GenerateSha(sLinkedInId + sProject) + "-" +
                                NWDSecurityTools.GenerateSha(sLinkedInId);
            rReturn.RescueHash = tType + "-" + NWDSecurityTools.GenerateSha(sLinkedInId + sProject) + "-" +
                                 NWDSecurityTools.GenerateSha(sLinkedInId);
            rReturn.SignHash = tType + "-" + NWDSecurityTools.GenerateSha(sLinkedInId + sProject) + "-" +
                               NWDSecurityTools.GenerateSha(sLinkedInId);
            rReturn.SignStatus = NWDAccountSignAction.None;
            return rReturn;
        }

        private static string DeviceTypeObfuscation(NWDAccountSignType sType)
        {
            string rResult = "";

            switch (sType)
            {
                case NWDAccountSignType.DeviceId:
                    rResult = "M";
                    break;
                case NWDAccountSignType.Facebook:
                    rResult = "E";
                    break;
                case NWDAccountSignType.Google:
                    rResult = "R";
                    break;
                case NWDAccountSignType.Apple:
                    rResult = "D";
                    break;
                case NWDAccountSignType.Microsoft:
                    rResult = "I";
                    break;
                case NWDAccountSignType.Twitter:
                    rResult = "Q";
                    break;
                case NWDAccountSignType.LinkedIn:
                    rResult = "U";
                    break;
                case NWDAccountSignType.Discord:
                    rResult = "A";
                    break;
                default:
                    break;
            }

            return rResult;
        }

        #endregion
    }
}