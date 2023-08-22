using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebDownloader.Managers;
using NWDWebDownloader.Services;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Models;
using NWDWebDownloader.Resources;

namespace NWDWebDownloader.Configuration
{
    [Serializable]
    public class NWDWebDownloaderConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebDownloaderConfiguration KConfig = new NWDWebDownloaderConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        public List<NWDDownloadConfig> Downloads { set; get; } = new List<NWDDownloadConfig>();
        public List<string> ReservedNames { set; get; } = new List<string>();
        public bool SetUpPage { set; get; } = true;
        public long MaxUploadSizeInBytes { set; get; } = 1000000000;

        #endregion

        #region static methods

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebDownloaderConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebDownloaderConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebDownloaderConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebDownloaderStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebDownloader), nameof(NWDWebDownloaderConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebDownloader)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebDownloader), nameof(NWDWebDownloaderConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebDownloader)));
                }

            }
        }

        #endregion

        #region instance methods

        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebDownloaderConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebDownloaderConfiguration)).Get<NWDWebDownloaderConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebDownloaderConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebDownloaderConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebDownloaderConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebDownloaderConfiguration)),  NWDLogger.SplitObjectSerializable(new NWDWebDownloaderConfiguration()));
            }
            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
            NWDConfigurationInstalled.AddConfiguration(KConfig);
            NWDWebRuntimeConfiguration.KConfig.NavBarInterfaceAdd(new NWDWebDownloaderNavBar());
            NWDWebRuntimeConfiguration.KConfig.SideBarInterfaceAdd(new NWDWebDownloaderSideBar());
            NWDWebRuntimeConfiguration.KConfig.NavFooterInterfaceAdd(new NWDWebDownloaderNavFooter());
            // NWDWebRuntimeConfiguration.KConfig.AppNavBarPartialList.Add(new NWDPartialView() { PartialPath = "_DownloaderRightNavBar.cshtml" });
            // NWDWebRuntimeConfiguration.KConfig.AdminNavBarPartialList.Add(new NWDPartialView() { PartialPath = "_DownloaderRightNavBarAdmin.cshtml" });
            
            foreach (NWDDownloadConfig tKConfigDownload in KConfig.Downloads)
            {
                if (!Directory.Exists(tKConfigDownload.RootPath))
                {
                    Directory.CreateDirectory(tKConfigDownload.RootPath);
                }

                if (string.IsNullOrEmpty(tKConfigDownload.DescriptionFileName) == false)
                {
                    KConfig.ReservedNames.Add(tKConfigDownload.DescriptionFileName);
                }

                KConfig.ReservedNames.Add(tKConfigDownload.FooterFileName);
                KConfig.ReservedNames.Add(tKConfigDownload.HeaderFileName);
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