using Microsoft.AspNetCore.Http;
using NWDWebPlayerDemo.Models.CV;
using NWDWebRuntime.Managers;

namespace NWDWebPlayerDemo.Managers;

public class NWDCVManager
{
    public static void Save(HttpContext sHttpContext,CV sCv)
    {
        NWDWebDataManager.SaveData(sHttpContext,sCv);
    }
}