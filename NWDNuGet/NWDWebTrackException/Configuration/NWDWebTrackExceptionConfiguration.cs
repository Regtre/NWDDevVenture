using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NWDFoundation.Configuration;
using NWDFoundation.Facades;
using NWDFoundation.Logger;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebTrackException.Services;

namespace NWDWebTrackException.Configuration
{
    [Serializable]
    public class NWDWebTrackExceptionConfiguration : INWDConfiguration
    {
        #region static properties

        public static NWDWebTrackExceptionConfiguration KConfig = new NWDWebTrackExceptionConfiguration();
        private static bool Loaded { set; get; } = false;

        #endregion

        #region instance properties

        public bool SetUpPage { set; get; } = true;
        public bool SendExceptionByEmail { set; get; } = true;

        #endregion

        #region static methods

        public static void UseApp(WebApplication sApp)
        {
            sApp.UseExceptionHandler(sErrorApp =>
            {
                sErrorApp.Run(async sContext =>
                {
                    var tExceptionHandlerPathFeature = sContext.Features.Get<IExceptionHandlerPathFeature>();
                    if (tExceptionHandlerPathFeature?.Error != null)
                    {
                        if (KConfig.SendExceptionByEmail == true)
                        {
                            NWDEmailManager tEmailer = new NWDEmailManager();
                                NWDEmailResult rResult = await tEmailer.SendMailAsync(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailNoReply, NWDEmailConfiguration.KConfig.EmailNoReply, NWDEmailConfiguration.KConfig.EmailWebmaster, "Exception " + NWDEmailConfiguration.KConfig.WebSite, NWDEmailManager.TransformToMessage(tExceptionHandlerPathFeature.Error, sContext));
                        }
                    }
                    NWDLogger.Exception(NWDWebRuntimeConfiguration.KConfig.Dns +" exception! ", tExceptionHandlerPathFeature.Error);
                    // TODO : Generate error page!
                    //await context.Response.WriteAsync("/Views/Shared/_Error.cshtml");
                });
            });
        }

        public static void LoadFromBuilder(WebApplicationBuilder sBuilder, bool sRuntimeCompileForDev = false)
        {
            if (Loaded == true)
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDWebTrackExceptionConfiguration)));
            }
            else
            {
                try
                {
                    sBuilder.Configuration.AddJsonFile(nameof(NWDWebTrackExceptionConfiguration) + ".json", true, true);
                }
                catch (Exception tException)
                {
                    NWDLogger.Exception(tException);
                }
                // load config
                KConfig.LoadConfig(sBuilder.Configuration);
                // add services
                sBuilder.Services.ConfigureOptions<NWDWebTrackExceptionConfigureOptions>();
                sBuilder.Services.AddHostedService<NWDWebTrackExceptionStartupService>();
                
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
                            NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_ENABLE, nameof(NWDWebTrackException), nameof(NWDWebTrackExceptionConfiguration)));
                            var tLibraryPath = Path.GetFullPath(Path.Combine(sBuilder.Environment.ContentRootPath, "../NWDNuGet/", nameof(NWDWebTrackException)));
                            sOptions.FileProviders.Add(new PhysicalFileProvider(tLibraryPath));
                        });
                    }
                    else
                    {
                        NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_RUNTIME_COMPILATION_DISABLE, nameof(NWDWebTrackException), nameof(NWDWebTrackExceptionConfiguration)));
                    }
                }
                else
                {
                    NWDLogger.TraceAttention(string.Format(NWDLogger.K_RAZOR_COMPILE_NOT_FOR_DEV, nameof(NWDWebTrackException)));
                }
            }
        }

        #endregion

        #region instance methods
        public void LoadConfig(IConfiguration sConfig)
        {
            NWDWebTrackExceptionConfiguration? tConfig = sConfig.GetSection(nameof(NWDWebTrackExceptionConfiguration)).Get<NWDWebTrackExceptionConfiguration>();
            if (tConfig != null)
            {
                KConfig = tConfig;
                NWDLogger.TraceSuccess(string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDWebTrackExceptionConfiguration)));
                //NWDLogger.Trace(nameof(NWDWebTrackExceptionConfiguration), NWDLogger.SplitObjectSerializable(tConfig));
            }
            else
            {
                NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS, nameof(NWDWebTrackExceptionConfiguration)));
                NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDWebTrackExceptionConfiguration)), NWDLogger.SplitObjectSerializable(new NWDWebTrackExceptionConfiguration()));
            }

            PrepareAfterConfiguration();
        }

        public void PrepareAfterConfiguration()
        {
            Loaded = true;
            NWDLibrariesInstalled.AddAssemblyByType(GetType(), true);
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