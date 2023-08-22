using Microsoft.AspNetCore.Http;
using NWDWebGitLabReport.Models;
using NWDWebRuntime.Managers;

namespace NWDWebGitLabReport.Managers;

public class NWDSprintInfoManager
{

    public static NWDSprintInfo? GetSprintInfoForMilestone(string sMilestoneName,HttpContext sHttpContext)
    {
        NWDSprintInfo? rResult = null;
        List<NWDSprintInfo> tSprintInfos = NWDWebDataManager.GetDataForPlayerByClass<NWDSprintInfo>(sHttpContext);
        if(tSprintInfos.Count > 0)
        {
            rResult = tSprintInfos.Find(sSprintInfo => sSprintInfo.MilestoneName == sMilestoneName);
        }

        return rResult;
    }

    public static void Save(HttpContext sHttpContext,NWDSprintInfo sSprintInfo)
    {
        NWDWebDataManager.SaveData(sHttpContext,sSprintInfo);
    }
}