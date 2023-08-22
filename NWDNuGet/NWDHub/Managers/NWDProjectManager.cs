using Microsoft.AspNetCore.Http;
using NWDFoundation.Configuration.Environments;
using NWDHub.Models;
using NWDWebRuntime.Managers;

namespace NWDHub.Managers;

public class NWDProjectManager
{
    private static NWDProjectGlobalSettings CreateProjectGlobalSettingsFor(NWDProject sProject, HttpContext sHttpContext)
    {
        NWDProjectGlobalSettings tResult = new NWDProjectGlobalSettings
        {
            ProjectId = sProject.Reference,
            ProjectUniqueId = sProject.ProjectId
        };
        NWDWebDataManager.SaveData(sHttpContext, tResult);
        return tResult;
    }
    public static NWDProjectGlobalSettings GetProjectGlobalSettingsFor(NWDProject sProject, HttpContext sHttpContext)
    {
        NWDProjectGlobalSettings? tResult = null;
        foreach (NWDProjectGlobalSettings tItem in NWDWebDataManager.GetAllData<NWDProjectGlobalSettings>(sHttpContext))
        {
            if (tItem.ProjectId == sProject.Reference)
            {
                tResult = tItem;
                break;
            }
        }
        if (tResult == null)
        {
            tResult = CreateProjectGlobalSettingsFor(sProject, sHttpContext);
        }
        return tResult;
    }
    
    private static NWDProjectEnvironmentSettings CreateProjectEnvironmentSettingsFor(NWDProject sProject, NWDEnvironmentKind sEnvironmentKind, HttpContext sHttpContext)
    {
        NWDProjectEnvironmentSettings tResult = new NWDProjectEnvironmentSettings
        {
            ProjectId = sProject.Reference,
            ProjectUniqueId = sProject.ProjectId,
            EnvironmentKind = sEnvironmentKind
        };
        NWDWebDataManager.SaveData(sHttpContext, tResult);
        return tResult;
    }
    
    public static NWDProjectEnvironmentSettings GetProjectEnvironmentSettingsFor(NWDProject sProject, NWDEnvironmentKind sEnvironmentKind, HttpContext sHttpContext)
    {
        NWDProjectEnvironmentSettings? tResult = null;
        foreach (NWDProjectEnvironmentSettings tItem in NWDWebDataManager.GetAllData<NWDProjectEnvironmentSettings>(sHttpContext))
        {
            if (tItem.ProjectId == sProject.Reference && tItem.EnvironmentKind == sEnvironmentKind)
            {
                tResult = tItem;
                break;
            }
        }
        if (tResult == null)
        {
            tResult = CreateProjectEnvironmentSettingsFor(sProject, sEnvironmentKind, sHttpContext);
        }
        return tResult;
    }
}