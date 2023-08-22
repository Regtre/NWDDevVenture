using Microsoft.AspNetCore.Http;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;

namespace NWDWebStandard.Extensions;

public static class NWDAccountInfoExtension
{
    public static void Save(this NWDAccountInfo sAccountInfo,HttpContext sHttpContext)
    {
        NWDWebDataManager.SaveData(sHttpContext,sAccountInfo);
    }
}