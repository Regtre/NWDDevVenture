using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Sessions;

public abstract class NWDSessionGlobal
{
    public static Dictionary<string, NWDSessionDefinition> KDictionary = new Dictionary<string, NWDSessionDefinition>();

    public static void DeleteAllSession(HttpContext? sHttpContext)
    {
        foreach (KeyValuePair<string, NWDSessionDefinition> tSessionKv in NWDSessionGlobal.KDictionary)
        {
            tSessionKv.Value.DeleteFrom(sHttpContext);
        }
    }
}