using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Models;

namespace NWDWebRuntime.Managers;

public class NWDAccountInfoManager
{
    public static NWDAccountInfo GetAccountInfo(HttpContext? sHttpContext)
    {
        if (sHttpContext == null)
        {
            return new NWDAccountInfo();
        }
        List<NWDAccountInfo>? tAccountInfo = NWDWebDataManager.GetDataForPlayerByClass<NWDAccountInfo>(sHttpContext);
        if (tAccountInfo != null && tAccountInfo.Count > 0) //  TODO Count ==1 (?)
        {
            return tAccountInfo.First();
        }
        else
        {
            return new NWDAccountInfo();
        }
    }

}