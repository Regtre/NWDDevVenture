using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools.Cookies
{
    public abstract class NWDCookieGlobal
    {
        public static readonly Dictionary<string, NWDCookieDefinition> KDictionary = new Dictionary<string, NWDCookieDefinition>();
        public static void DeleteAllCookie(HttpContext? sHttpContext)
        {
            foreach (KeyValuePair<string, NWDCookieDefinition> tCookieKeyValue in KDictionary)
            {
                if (tCookieKeyValue.Value.Group != NWDCookieDefinitionGroup.Functional)
                {
                    if (tCookieKeyValue.Value.Deletable == true)
                    {
                        tCookieKeyValue.Value.DeleteCookie(sHttpContext);
                    }
                }
            }
        }
    }

}