using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDWebStandard.Managers;
using Microsoft.Extensions.Localization;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebRuntime.Tools;
using NWDWebStandard.Models;
using NWDWebStandard.Services;

// using Wkhtmltopdf.NetCore;

namespace NWDWebStandard.Configuration
{
    public enum NWDSignOutMethodInMenu
    {
        SignOut = 0,
        LogOut = 1,
    }

    [Serializable]
    public class NWDWebStandardConfiguration : INWDConfiguration
    {
        #region static members

        public static NWDWebStandardConfiguration KConfig = new NWDWebStandardConfiguration();
        public static readonly HttpClient HttpClientShared = new HttpClient();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance members

        public bool SetUpPage { set; get; } = true;

        public NWDSignOutMethodInMenu SignOutMethodInMenu { set; get; } = NWDSignOutMethodInMenu.SignOut;
        public bool ShowIdemobiEngine { set; get; } = false;

        public bool ActiveHtmlToPdf { set; get; } = false;

        public NWDSocialShareableGlobalStyle ShareableStyle { set; get; } = NWDSocialShareableGlobalStyle.Toolbar;

        #region Society

        public string SocietyName { set; get; } = "idéMobi";
        public string SocietyAddress { set; get; } = "43, rue d'Atlanta";
        public string SocietyTown { set; get; } = "Marcq-En-Barœul";
        public string SocietyZipCode { set; get; } = "59700";
        public string SocietyCountry { set; get; } = "FRANCE";
        public string SocietySiret { set; get; } = "44424955100011";
        public string SocietyApe { set; get; } = "7722C";
        public string SocietyRcs { set; get; } = "LILLE";
        public string SocietyTva { set; get; } = "FR93444249551";

        #endregion


        #region Authentification settings

        public string SignHook { set; get; } = string.Empty;

        public string ContactHook { set; get; } = string.Empty;

        public string SignInSuccessAction { set; get; } = string.Empty;
        public string SignUpSuccessAction { set; get; } = string.Empty;
        public string SignOutSuccessAction { set; get; } = string.Empty;

        #region EmailPassword authentification

        public bool AddAccountSignEmailPassword { set; get; } = true;
        public bool AddAccountSignEmailPasswordSendEmail { set; get; } = true;

        #endregion

        #region LoginPassword authentification

        public bool AddAccountSignLoginPassword { set; get; } = true;
        public bool AccountSignInSendEmail { set; get; } = false;
        public bool AccountSignUpSendEmail { set; get; } = false;

        #endregion

        #region LoginEmailPassword authentification

        public bool AddAccountSignLoginPasswordEmail { set; get; } = true;
        public bool AddAccountSignLoginPasswordEmailSendEmail { set; get; } = true;

        #endregion

        #region Google authentification

        public bool AddGoogleSign { set; get; }
        public string GoogleClientId { set; get; } = string.Empty;
        public string GoogleClientSecret { set; get; } = string.Empty;

        #endregion

        #region Facebook authentification

        public bool AddFacebookSign { set; get; }
        public string FacebookClientId { set; get; } = string.Empty;
        public string FacebookClientSecret { set; get; } = string.Empty;

        #endregion

        #region Discord authentification

        public bool AddDiscordSign { set; get; }
        public string DiscordClientId { set; get; } = string.Empty;
        public string DiscordClientSecret { set; get; } = string.Empty;

        #endregion

        #region Apple authentification

        public bool AddAppleSign { set; get; }

        public string AppleServiceId { set; get; } = string.Empty;

        // public string AppleSecretKey { set; get; } = string.Empty;
        public string AppleTeamId { set; get; } = string.Empty;
        public string[] AppleKeyValue { set; get; } = new[] { string.Empty };
        public string AppleKeyId { set; get; } = string.Empty;

        #endregion

        #region Microsoft authentification

        public bool AddMicrosoftSign { set; get; }
        public string MicrosoftClientId { set; get; } = string.Empty;
        public string MicrosoftClientSecret { set; get; } = string.Empty;

        #endregion

        #region Twitter authentification

        public bool AddTwitterSign { set; get; }
        public string TwitterClientId { set; get; } = string.Empty;
        public string TwitterClientSecret { set; get; } = string.Empty;

        #endregion

        #region LinkedIn authentification

        public bool AddLinkedInSign { set; get; }
        public string LinkedInClientId { set; get; } = string.Empty;
        public string LinkedInClientSecret { set; get; } = string.Empty;

        #endregion

        #endregion


        #region Social interaction

        public bool ShareableByEmail { set; get; } = false;
        public bool ShareableOnFacebook { set; get; } = false;
        public bool ShareableOnGooglePlus { set; get; } = false;
        public bool ShareableOnWhatsApp { set; get; } = false;
        public bool ShareableOnInstagram { set; get; } = false;
        public bool ShareableOnWeibo { set; get; } = false;
        public bool ShareableOnRenren { set; get; } = false;
        public bool ShareableOnBaidu { set; get; } = false;
        public bool ShareableOnReddIt { set; get; } = false;
        public bool ShareableOnTwitter { set; get; } = false;
        public bool ShareableOnPinterest { set; get; } = false;
        public bool ShareableOnLinkedIn { set; get; } = false;
        public bool ShareableOnDiscord { set; get; } = false;
        public bool ShareableOnTwitch { set; get; } = false;
        public bool ShareableOnTumblr { set; get; } = false;

        public string PageUrlOnFacebook { set; get; } = string.Empty;
        public string PageUrlOnGooglePlus { set; get; } = string.Empty;
        public string PageUrlOnInstagram { set; get; } = string.Empty;
        public string PageUrlOnWeibo { set; get; } = string.Empty;
        public string PageUrlOnReddIt { set; get; } = string.Empty;
        public string PageUrlOnTwitter { set; get; } = string.Empty;
        public string PageUrlOnPinterest { set; get; } = string.Empty;
        public string PageUrlOnLinkedIn { set; get; } = string.Empty;
        public string PageUrlOnDiscord { set; get; } = string.Empty;
        public string PageUrlOnTwitch { set; get; } = string.Empty;

        #endregion

        #region Website settings

        public string WebSiteName { set; get; } = "MyWebSite";
        public string WebSiteShortName { set; get; } = "MyWeb";
        public string WebSiteInitials { set; get; } = "MWS";

        #endregion

        #region Shop
        public bool ShopIsEnabled { set; get; } = false;
        public string ShopCurrency { set; get; } = "EUR";
        public string ShopCurrencySymbol { set; get; } = "€";
        public string ShopCartEndPoint { set; get; } = "/cart";
        #endregion
        public bool NotificationIsEnabled { set; get; } = false;

        public int NotificationRefreshTimeInMilliseconds { set; get; } = 10000;
        public bool ApplicationsAreEnabled { set; get; } = false;

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebStandardConfiguration)));
            }
            else
            {
                NWDWebRuntimeConfiguration.LoadFromBuilder(sBuilder, sRuntimeCompileForDev);
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebStandardConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }

                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebStandardConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebStandardStartupService>();

                // add localizations
                // sBuilder.Services.AddLocalization(sOptions => sOptions.ResourcesPath = NWDConstants.K_RESOURCES);
                // sBuilder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(sOo =>
                // {
                //     Type tType = typeof(SharedResource);
                //     string? tFullname = tType.GetTypeInfo().Assembly.FullName;
                //     if (string.IsNullOrEmpty(tFullname) == false)
                //     {
                //         AssemblyName? tAssemblyName = new AssemblyName(tFullname);
                //         if (tAssemblyName.Name != null)
                //         {
                //             var tFactory = sBuilder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                //             if (tFactory != null)
                //             {
                //                 var tLocalizer = tFactory.Create(nameof(SharedResource), tAssemblyName.Name);
                //                 sOo.DataAnnotationLocalizerProvider = (sType, sFactory) => tLocalizer;
                //             }
                //         }
                //     }
                // });

                // add view render
                sBuilder.Services.AddScoped<NWDViewRenderer, NWDViewRenderer>();
                if (KConfig.ActiveHtmlToPdf == true)
                {
                    NWDLogger.Error("⚠️ NEED INSTALL ROTATIVA DIRECTORY! ⚠️");
                    NWDLogger.Error("⚠️ NEED ALWAYS COPY ALL .SO, .DLL, LIB AND EXE IN ROTATIVA DIRECTORY! ⚠️");

                    // sBuilder.Services.AddWkhtmltopdf();
                }

                // add razor
                if (sRuntimeCompileForDev)
                {
                    if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
                    {
                        var tMvcBuilder = sBuilder.Services.AddRazorPages();
                        tMvcBuilder.AddRazorRuntimeCompilation();
                        sBuilder.Services.Configure<MvcRazorRuntimeCompilationOptions>(sOptions =>
                        {
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebStandard), nameof(NWDWebRuntimeConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebStandard)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebStandard), nameof(NWDWebRuntimeConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebStandard)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebStandardConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebStandardConfiguration)).Get<NWDWebStandardConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebStandardConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebStandardConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebStandardConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebStandardConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebStandardConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            KConfig.Check();
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDWebRuntimeConfiguration.KConfig.SideBarInterfaceAdd(new NWDWebStandardSideBar());
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDWebStandardNavBar());
            NWDWebRuntimeConfiguration.KConfig.NavFooterInterfaceAdd(new NWDWebStandardNavFooter());

            NWDStatisticsConsolidated.CreateTable();
        }
        private void Check()
        {
            if (string.IsNullOrEmpty(LinkedInClientId) || string.IsNullOrEmpty(LinkedInClientSecret))
            {
                AddLinkedInSign = false;
            }

            if (string.IsNullOrEmpty(TwitterClientId) || string.IsNullOrEmpty(TwitterClientSecret))
            {
                AddTwitterSign = false;
            }

            if (string.IsNullOrEmpty(AppleServiceId)
                || string.IsNullOrEmpty(AppleKeyId)
                // || string.IsNullOrEmpty(AppleKeyValue)
                || AppleKeyValue.Length == 0
                || string.IsNullOrEmpty(AppleTeamId)
               )
            {
                AddAppleSign = false;
            }

            if (string.IsNullOrEmpty(MicrosoftClientId) || string.IsNullOrEmpty(MicrosoftClientSecret))
            {
                AddMicrosoftSign = false;
            }

            if (string.IsNullOrEmpty(FacebookClientId) || string.IsNullOrEmpty(FacebookClientSecret))
            {
                AddFacebookSign = false;
            }

            if (string.IsNullOrEmpty(DiscordClientId) || string.IsNullOrEmpty(DiscordClientSecret))
            {
                AddDiscordSign = false;
            }

            if (string.IsNullOrEmpty(GoogleClientId) || string.IsNullOrEmpty(GoogleClientSecret))
            {
                AddGoogleSign = false;
            }
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