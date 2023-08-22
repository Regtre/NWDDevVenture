using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Razor;
using NWDFoundation.Models.Enums;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Exchanges;
using NWDFoundation.Facades;
using NWDWebRuntime.Facades;
using NWDWebRuntime.Models;
using NWDWebRuntime.Resources;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebRuntime.Tools;

namespace NWDWebRuntime.Configuration
{
    [Serializable]
    public class NWDWebRuntimeConfiguration : INWDConfiguration, INWDProjectKey, INWDSecretKey, INWDProjectInformation //, INWDSideBar
    {
        #region static properties

        public static NWDWebRuntimeConfiguration KConfig = new NWDWebRuntimeConfiguration();
        public static readonly HttpClient HttpClientShared = new HttpClient();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        [NonSerialized] public Dictionary<string, NWDRequestStatus> ConfigurationStatus = new Dictionary<string, NWDRequestStatus>();
        [NonSerialized] public string DateYear = string.Empty;
        [NonSerialized] public string SessionCookieName = "NODNS";
        [NonSerialized] public bool IsDevelopment = false;
        // [NonSerialized] public List<NWDPartialView> NavBarPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> AdminNavBarPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> AppNavBarPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> DebugNavBarPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> AnnexeSideBarPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> FooterOurMissionPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> FooterCompanyPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> FooterPrivacyPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> FooterOtherPartialList = new List<NWDPartialView>();
        // [NonSerialized] public List<NWDPartialView> AccountAnnexeMenuPartialList = new List<NWDPartialView>();

        [NonSerialized] public DateTime LaunchDateTime = DateTime.UtcNow;

        protected List<INWDSideBar> SideBarInterface { set; get; } = new List<INWDSideBar>();
        public void SideBarInterfaceAdd(INWDSideBar sSideBar)
        {
            SideBarInterface.Add(sSideBar);
        }
        public List<INWDSideBar> GetSideBarInterface()
        {
            return SideBarInterface;
        }
        protected List<INWDNavBar> NavBarInterface { set; get; } = new List<INWDNavBar>();
        public void NavBarInterfaceAdd(INWDNavBar sNavBar)
        {
            NavBarInterface.Add(sNavBar);
        }
        public List<INWDNavBar> GetNavBarInterface()
        {
            return NavBarInterface;
        }
        protected List<INWDNavFooter> NavFooterInterface { set; get; } = new List<INWDNavFooter>();
        public void NavFooterInterfaceAdd(INWDNavFooter sNavFooter)
        {
            NavFooterInterface.Add(sNavFooter);
        }
        public List<INWDNavFooter> GetNavFooterInterface()
        {
            return NavFooterInterface;
        }
        public NWDCaptchaParameters CaptchaParameters { set; get; } = new NWDCaptchaParameters();
        public string HubDns { set; get; } = "www.net-worked-data.com";
        public string ServerFormatDns { set; get; } = "ws??.net-worked-data.com";

        #region Page Set Up

        public bool SetUpPage { set; get; } = true;

        #endregion

        #region Project settings

        public NWDLogLevel LogLevel { set; get; } = NWDLogLevel.Trace;
        public ushort DataTrackActive { set; get; }
        public List<ushort> AvailableDataTracks { set; get; } = new List<ushort>();
        public ulong MyProjectId { set; get; } = NWDConstants.K_FAKE_PROJECT_ID;
        public NWDEnvironmentKind MyEnvironment { set; get; } = NWDConstants.K_FAKE_PROJECT_ENVIRONMENT;
        public string MyProjectKey { set; get; } = NWDConstants.K_FAKE_PROJECT_KEY;
        public string MySecretKey { set; get; } = NWDConstants.K_FAKE_SECRET_KEY;
        public bool Debug { set; get; } = false;

        [JsonIgnore] public NWDLicenseStatus LicenseValid = NWDLicenseStatus.Unknow;
        [JsonIgnore] public NWDNeedUpdate NeedUpdate = NWDNeedUpdate.Unknow;
        [JsonIgnore] public string NewVersion = string.Empty;

        #endregion

        #region Website settings

        public string Dns { set; get; } = "localhost:5001";
        // [NonSerialized] public string HttpsDns = string.Empty;
        [NonSerialized] private List<INWDWebMenu> _MenusAdmin = new List<INWDWebMenu>();
        public string BaseLanguage { set; get; } = "en-US";
        public string[] SupportLanguages { set; get; } = new string[] { }; // must stay empty else it's populated by adds
        // public long AdminService { set; get; } = -1;
        public ulong[] AdminAccountsId { set; get; } = new ulong[] { };

        #endregion

        #region GitLabInfos settings

        public string GitCommit { set; get; } = string.Empty; //"GIT_COMMIT";
        public string GitCommitShort { set; get; } = string.Empty; //"GIT_COMMIT_SHORT"; //string.Empty;
        public string PipelineDate { set; get; } = string.Empty; //"PIPELINE_DATE"; //string.Empty;
        public string PipelineJob { set; get; } = string.Empty; //"PIPELINE_DATE"; //string.Empty;

        #endregion

        #endregion


        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            NWDEmailConfiguration.LoadFromBuilder(sBuilder, sRuntimeCompileForDev);
            NWDDatabaseConnectorConfiguration.LoadFromBuilder(sBuilder, sRuntimeCompileForDev);
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebRuntimeConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebRuntimeConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }

                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebRuntimeConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebRuntimeStartupService>();
                sBuilder.Services.Configure<KestrelServerOptions>(sOptions =>
                {
                    if (sOptions == null)
                    {
                        throw new ArgumentNullException(nameof(sOptions));
                    }

                    sOptions.AllowSynchronousIO = true;
                });
                sBuilder.Services.AddSingleton<IValidationAttributeAdapterProvider, NWDCustomValidationAttributeAdapterProvider>();

                // sBuilder.Services.AddOutputCache();
                sBuilder.Services.AddDistributedMemoryCache();
                // app.UseSession();// OBLIGATORY
                sBuilder.Services.AddSession(sOptions =>
                {
                    sOptions.Cookie.Name = KConfig.SessionCookieName;
                    sOptions.IdleTimeout = TimeSpan.FromDays(365);
                    sOptions.Cookie.IsEssential = true;
                    sOptions.Cookie.MaxAge = TimeSpan.FromDays(365);
                });
                
                sBuilder.Services.AddMvc();
                sBuilder.Services.AddMvc().AddSessionStateTempDataProvider();
                sBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                sBuilder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
                // sBuilder.Services.AddControllers(sOptions => sOptions.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);


                // add localizations
                sBuilder.Services.AddLocalization(sOptions => sOptions.ResourcesPath = NWDConstants.K_RESOURCES);
                sBuilder.Services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(sOo =>
                {
                    Type tType = typeof(SharedResource);
                    string? tFullname = tType.GetTypeInfo().Assembly.FullName;
                    if (string.IsNullOrEmpty(tFullname) == false)
                    {
                        AssemblyName? tAssemblyName = new AssemblyName(tFullname);
                        if (tAssemblyName.Name != null)
                        {
                            var tFactory = sBuilder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                            if (tFactory != null)
                            {
                                var tLocalizer = tFactory.Create(nameof(SharedResource), tAssemblyName.Name);
                                sOo.DataAnnotationLocalizerProvider = (sType, sFactory) => tLocalizer;
                            }
                        }
                    }
                });

                // add Culture support
                sBuilder.Services.Configure<RequestLocalizationOptions>(sOptions => { sOptions.AddSupportedUICultures(NWDWebRuntimeConfiguration.KConfig.SupportLanguages.ToArray()); });


                KConfig.IsDevelopment = sBuilder.Environment.IsDevelopment();
                string? tEnvironment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrEmpty(tEnvironment) == false)
                {
                    if (tEnvironment == "Development"
                        || tEnvironment == "MariaDB"
                        || tEnvironment == "MySQL"
                        || tEnvironment == "NoSQL"
                        || tEnvironment == "SQLite"
                       )
                    {
                        KConfig.IsDevelopment = true;
                    }

                    NWDLogger.Information("ASPNETCORE_ENVIRONMENT = " + tEnvironment + " then IsDevelopment = " + KConfig.IsDevelopment.ToString());
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebRuntime), nameof(NWDWebRuntimeConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath,
                                "../NWDNuGet/", nameof(NWDWebRuntime)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebRuntime), nameof(NWDWebRuntimeConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebRuntime)));
                }
            }
        }

        public static void UseApp(WebApplication sApp)
        {
            sApp.UseCookiePolicy(); // OBLIGATORY
            sApp.UseSession(); // OBLIGATORY
            sApp.UseRequestLocalization();
            sApp.UseExceptionHandler("/Exception");
            sApp.UseStatusCodePagesWithReExecute("/Error/{0}");
        }

        #endregion

        #region insatnce methods

        public void LoadConfig(IConfiguration sConfiguration)
        {
            NWDWebRuntimeConfiguration? tConfig = sConfiguration.GetSection(nameof(NWDWebRuntimeConfiguration)).Get<NWDWebRuntimeConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebRuntimeConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebRuntimeConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebRuntimeConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebRuntimeConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebRuntimeConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            KConfig.Check();
            // KConfig.SideBarBlockInterface.Add(KConfig);
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDFoundation.NWDVersionDll));
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDRuntime.NWDVersionDll));
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDDatabaseAccess.NWDVersionDll));
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDStandardModels.NWDVersionDll));
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDCustomModels.NWDVersionDll));
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDLogger.SetWriter(new NWDConsoleLogger(KConfig.LogLevel));
            NWDWebDBDataManager.CreateAllTables();
        }

        public void Check()
        {
            DateYear = DateTime.Now.Year.ToString();
            SessionCookieName = Dns.Replace(":", "") + ".Session"; // dont change
        }
        
        public string GetDnsHttps()
        {
            return "https://" + Dns.Replace("https://", "").Replace("http://", "").TrimEnd('/');
        }

        #endregion

        #region interface methods
        

        public string GetHubDnsClean()
        {
            return HubDns.Replace("https://", "").Replace("http://", "").TrimEnd('/');
        }

        public string GetHubDnsHttps()
        {
            return "https://" + HubDns.Replace("https://", "").Replace("http://", "").TrimEnd('/');
        }

        public string GetServerFormatDnsClean()
        {
            return ServerFormatDns.Replace("https://", "").Replace("http://", "").TrimEnd('/');
        }
        public string GetBestUrlForServer()
        {
            // TODO CREATE REAL FONCTION!
            return "https://" + ServerFormatDns.Replace("https://", "").Replace("http://", "").Replace("??", "fr").TrimEnd('/');
        }

        public ulong GetProjectId()
        {
            return MyProjectId;
        }

        public NWDEnvironmentKind GetProjectEnvironment()
        {
            return MyEnvironment;
        }

        public string GetProjectKeyInstanceName()
        {
            return nameof(NWDWebRuntimeConfiguration);
        }

        public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            return MyProjectKey;
        }

        public string GetSecretKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            return MySecretKey;
        }

        public bool IsLoaded()
        {
            return Loaded;
        }

        public void RandomFake()
        {
            MyProjectId = NWDConstants.K_RANDOM_PROJECT_ID;
            MyEnvironment = NWDConstants.K_RANDOM_PROJECT_ENVIRONMENT;
            MyProjectKey = NWDConstants.K_RANDOM_PROJECT_KEY;
            MySecretKey = NWDConstants.K_RANDOM_SECRET_KEY;
        }

        public string GetSecretKeyInstanceName()
        {
            return nameof(NWDWebRuntimeConfiguration);
        }

        public string GetProjectInformationInstanceName()
        {
            return nameof(NWDWebRuntimeConfiguration);
        }

        #endregion
        //
        // public NWDSideBarBlock[]? AddSideBarBlock(NWDSideBarKind sSideBarKind)
        // {
        //     switch (sSideBarKind)
        //     {
        //         case NWDSideBarKind.Home:
        //             NWDSideBarBlock tBlock = new NWDSideBarBlock()
        //             {
        //                 Name = "WebRuntime ",
        //                 BadgeText = "Test",
        //                 Categories = new List<NWDSideBarCategory>()
        //                 {
        //                     new NWDSideBarCategory()
        //                     {
        //                         Name = "Title",
        //                         IconStyle = "far fa-comment-alt",
        //                         Elements = new List<NWDSideBarElement>()
        //                         {
        //                             new NWDSideBarElement()
        //                             {
        //                                 Name = "General", ActionName = "Index", ControllerName = "NWDWebHtmlLayout", UrlParameter = "",
        //                             },
        //                         }
        //                     },
        //                     new NWDSideBarCategory()
        //                     {
        //                         Name = "Whaoo",
        //                         IconStyle = "far fa-file-alt",
        //                         Elements = new List<NWDSideBarElement>()
        //                         {
        //                             new NWDSideBarElement()
        //                             {
        //                                 Name = "Estimates", ActionName = "Estimates", ControllerName = "NWDWebHtmlLayout", UrlParameter = ""
        //                             },
        //                         }
        //                     },
        //                 }
        //             };
        //
        //             return new[] { tBlock };
        //             break;
        //         case NWDSideBarKind.Tools:
        //             NWDSideBarBlock tBlockB = new NWDSideBarBlock()
        //             {
        //                 Name = "WebRuntime ",
        //                 BadgeText = "TOOOOLS",
        //                 Categories = new List<NWDSideBarCategory>()
        //                 {
        //                     new NWDSideBarCategory()
        //                     {
        //                         Name = "Title",
        //                         IconStyle = "far fa-comment-alt",
        //                         Elements = new List<NWDSideBarElement>()
        //                         {
        //                             new NWDSideBarElement()
        //                             {
        //                                 Name = "General", ActionName = "Index", ControllerName = "NWDWebHtmlLayout", UrlParameter = ""
        //                             },
        //                         }
        //                     }
        //                 }
        //             };
        //             return new[] { tBlockB };
        //             break;
        //         case NWDSideBarKind.None:
        //             return null;
        //             break;
        //     }
        //
        //     return null;
        // }
    }
}