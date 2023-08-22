using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDWebEmailSender.Models;
using NWDWebRuntime.Models.Enums;

namespace NWDWebRuntime.Configuration
{
    [Serializable]
    public class NWDEmailConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDEmailConfiguration KConfig = new NWDEmailConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties
        public bool SetUpPage { set; get; } = true;
        public NWDEmailConfigurationKind Kind { set; get; } = NWDEmailConfigurationKind.SmtpClient;
        public NWDSecureSocketOptions Secure { set; get; } = NWDSecureSocketOptions.StartTls;
        public string WebSite { set; get; } = "www.example.com";
        public string EmailWebsite { set; get; } = "contact@example.com";
        public string User { set; get; } = "contact@example.com";
        public string Password { set; get; } = "PASSWORD";
        public string Server { set; get; } = "smtp.example.com";
        public int Port { set; get; } = 587;
        public string EmailToReceiptTest { set; get; } = "test@example.com";
        public bool CopyToWebmaster { set; get; } = true;
        public string EmailWebmaster { set; get; } = "webmaster@example.com";
        public string EmailNoReply { set; get; } = "no_reply@example.com";

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDEmailConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDEmailConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                KConfig.LoadConfig(sBuilder.Configuration);
            }
        }

        #endregion

        #region methods
        public void LoadConfig(IConfiguration sConfig)
        {
            NWDEmailConfiguration? tConfig = sConfig.GetSection(nameof(NWDEmailConfiguration)).Get<NWDEmailConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDEmailConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebEmailSenderConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDEmailConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDEmailConfiguration)), NWDLogger.SplitObjectSerializable(new NWDEmailConfiguration()));
            }
            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(this.GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
        }

        public bool IsLoaded()
        {
            return Loaded;
        }
        public void RandomFake()
        {
        }

        #endregion
    }
}