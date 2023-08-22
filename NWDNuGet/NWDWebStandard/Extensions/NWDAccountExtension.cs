using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDWebRuntime.Configuration;

namespace NWDWebStandard.Extensions;

public static class NWDAccountExtension
{
    public static string DecryptName(this NWDAccountSign? sSign)
    {
        string rReturn = string.Empty;
        if (string.IsNullOrEmpty(sSign?.Name) == false)
        {
                string? tName = NWDSecurityTools.DecryptAes(sSign.Name, NWDWebRuntimeConfiguration.KConfig.GetProjectId().ToString(), NWDWebRuntimeConfiguration.KConfig.GetProjectId().ToString());
                if (string.IsNullOrEmpty(tName) == false)
                {
                    rReturn = tName;
                }
        }
        return rReturn;
    }
}