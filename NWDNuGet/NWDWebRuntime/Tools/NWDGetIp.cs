
using System.Net;
using Microsoft.AspNetCore.Http;

namespace NWDWebRuntime.Tools
{
    public static class NWDGetIp
    {
        public static IPAddress GetRemoteIpAddress(HttpContext? sContext, bool sAllowForwarded = true)
        {
            IPAddress rReturn = IPAddress.None;
            if (sContext != null)
            {
                IPAddress? tReturn = sContext.Connection.RemoteIpAddress;
                if (sAllowForwarded)
                {
                    if (sContext.Request.Headers.ContainsKey("CF-Connecting-IP"))
                    {
                        IPAddress.TryParse(sContext.Request.Headers["CF-Connecting-IP"], out tReturn);
                    }
                    if (sContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        IPAddress.TryParse(sContext.Request.Headers["X-Forwarded-For"], out tReturn);
                    }
                }
                if (tReturn != null)
                {
                    rReturn = tReturn;
                }
            }
            return rReturn;
        }
    }
}