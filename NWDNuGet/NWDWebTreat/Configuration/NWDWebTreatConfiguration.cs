using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebTreat.Services;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebTreat.Managers;
using NWDWebTreat.Models;

namespace NWDWebTreat.Configuration
{
    [Serializable]
    public class NWDWebTreatConfiguration : INWDConfiguration, INWDTreatKey
    {
        #region static properties

        public static NWDWebTreatConfiguration KConfig = new NWDWebTreatConfiguration();
        public static readonly HttpClient HttpClientShared = new HttpClient();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        public bool SetUpPage { set; get; } = true;
        public string MyTreatKey { set; get; } = NWDConstants.K_FAKE_TREAT_KEY;

        public List<NWDVoucherServiceName> VoucherServiceList { set; get; } = new List<NWDVoucherServiceName>()
        {
            //new NWDVoucherServiceName("test",78),
            //new NWDVoucherServiceName("Marketing",45)
        };

        public long VoucherServiceMarketing { set; get; } = 45;

        public List<NWDVoucherServiceName> VoucherServiceMarketingList { set; get; } = new List<NWDVoucherServiceName>()
        {
            //new NWDVoucherServiceName("download",1234565),
            //new NWDVoucherServiceName("other",98887),
            //new NWDVoucherServiceName("press kit",145214),
        };

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebTreatConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebTreatConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }

                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebTreatConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebTreatStartupService>();

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

                // add razor
                if (sRuntimeCompileForDev)
                {
                    if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
                    {
                        var tMvcBuilder = sBuilder.Services.AddRazorPages();
                        tMvcBuilder.AddRazorRuntimeCompilation();
                        sBuilder.Services.Configure<MvcRazorRuntimeCompilationOptions>(sOptions =>
                        {
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebTreat), nameof(NWDWebTreatConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebTreat)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebTreat), nameof(NWDWebTreatConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebTreat)));
                }
            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebTreatConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebTreatConfiguration)).Get<NWDWebTreatConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebTreatConfiguration)));
                NWDLogger.Trace(nameof(NWDWebTreatConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebTreatConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebTreatConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebTreatConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(typeof(NWDTreat.Facades.INWDTreatManager));
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDWebTreatNavBar());
        }

        public bool IsLoaded()
        {
            return Loaded;
        }

        public void RandomFake()
        {
            MyTreatKey = NWDConstants.K_RANDOM_TREAT_KEY;
            VoucherServiceList = new List<NWDVoucherServiceName>()
            {
                new NWDVoucherServiceName("test", 78),
                new NWDVoucherServiceName("Marketing", 45)
            };
            VoucherServiceMarketing = 45;
            VoucherServiceMarketingList = new List<NWDVoucherServiceName>()
            {
                new NWDVoucherServiceName("download", 1234565),
                new NWDVoucherServiceName("other", 98887),
                new NWDVoucherServiceName("press kit", 145214),
            };
        }

        public string GetTreatKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            return MyTreatKey;
        }

        public string GetTreatInstanceName()
        {
            return nameof(NWDWebTreatConfiguration);
        }

        #endregion
    }
}