using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Logger;

namespace NWDMauiRuntime.Configurations;

public class NWDMauiRuntimeConfiguration : INWDProjectKey
{
    public static NWDMauiRuntimeConfiguration KConfig = new NWDMauiRuntimeConfiguration();

    public string ServerFormatDns { set; get; } =  "https://localhost:2051";


    public NWDEnvironmentKind Environment { set; get; } = NWDEnvironmentKind.Dev;
    public ulong ProjectId { get; set; } = 51376;

    public string ProjectKey { get; set; } =
        "EUBDGTJI-RHNKTRFR-RLMXOWSK-BKNVGGNK-EWKVKNQD-DXVXQVZB-AEVXCFJF-WBYOZJAJ-MLASCGGG-JESBVREE";

    public string SecretKey { get; set; } =
        "JAXGILJQ-KHKYZYJU-WHGPXMXC-PQPLRSRC-ZFXWCGIP-GIPXCPPM-SYYAXRHA-NAWMGPWG-LIPTTJIN-HOFPSAIZ";  
    public static bool Loaded { get; set; }

    public static void LoadFromBuilder(MauiAppBuilder sBuilder, bool sRuntimeCompileForDev = false)
    {
        if (Loaded == true)
        {
            //NWDLogger.Warning(string.Format(NWDLogger.K_CONFIG_ALREADY_LOADED, nameof(NWDMauiRuntimeConfiguration)));
        }
        else
        {
            try
            {
                Assembly tAssembly = Assembly.GetExecutingAssembly();
                sBuilder.Configuration.AddJsonFile(new EmbeddedFileProvider(tAssembly),"appsettings.json", true, true);
            } catch (Exception tException)
            {
                //NWDLogger.Exception(tException);
            }
            
            KConfig.LoadConfig(sBuilder.Configuration);
        }
    }

    public void LoadConfig(IConfiguration sConfiguration)
    {
        NWDMauiRuntimeConfiguration? tConfig = sConfiguration.GetSection(nameof(NWDMauiRuntimeConfiguration))
            .Get<NWDMauiRuntimeConfiguration>();
        if (tConfig != null)
        {
            KConfig = tConfig;
            NWDLogger.TraceSuccess(
                string.Format(NWDLogger.K_FOUND_IN_APP_SETTINGS, nameof(NWDMauiRuntimeConfiguration)));
            //NWDLogger.Trace(nameof(NWDWebRuntimeConfiguration),  NWDLogger.SplitObjectSerializable(tConfig));
        }
        else
        {
           // NWDLogger.Warning(string.Format(NWDLogger.K_NOT_FOUND_IN_APP_SETTINGS,
                //nameof(NWDMauiRuntimeConfiguration)));
            //NWDLogger.Information(string.Format(NWDLogger.K_CONFIG_JSON_EXAMPLE, nameof(NWDMauiRuntimeConfiguration)),
                //NWDLogger.SplitObjectSerializable(new NWDMauiRuntimeConfiguration()));
        }

        PrepareAfterConfiguration();
    }

    public void PrepareAfterConfiguration()
    {
    }

    public string GetProjectKeyInstanceName()
    {
        return ProjectId.ToString();
    }

    public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
    {
        return ProjectKey;
    }

    public string GetBestUrlForServer()
    {
        return ServerFormatDns.Replace("??", "fr").TrimEnd('/');
    }
}