using System;
using System.Net;

namespace NWDFoundation.Exchanges
{
    [Serializable]
    public enum NWDExchangeResultCode
    {
        Error = HttpStatusCode.InternalServerError,
        OK = HttpStatusCode.OK,
        TimeOut = HttpStatusCode.RequestTimeout,
        FileError = HttpStatusCode.NotFound,
    }
}