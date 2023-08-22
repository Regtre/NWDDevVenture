using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Controllers;
using NWDWebRuntime.Models;

namespace NWDWebStandard.Controllers
{
    public class NWDWebErrorController : NWDBasicController<NWDWebErrorController>
    {
        static public Dictionary<int, string> _ErrorDict = new Dictionary<int, string>();
        static public Dictionary<int, string> _InformationDict = new Dictionary<int, string>();
        static public Dictionary<int, string> _SuccesDict = new Dictionary<int, string>();
        static public Dictionary<int, string> _RedirectionDict = new Dictionary<int, string>();

        static NWDWebErrorController()
        {
            _InformationDict.Add(100, "Continue");
            _InformationDict.Add(101, "Switching");
            _InformationDict.Add(102, "Processing");
            _InformationDict.Add(103, "Early");

            _SuccesDict.Add(200, "OK");
            _SuccesDict.Add(201, "Created");
            _SuccesDict.Add(202, "Accepted");
            _SuccesDict.Add(203, "Non-Authoritative Information");
            _SuccesDict.Add(204, "No Content");
            _SuccesDict.Add(205, "Reset Content");
            _SuccesDict.Add(206, "Partial Content");
            _SuccesDict.Add(207, "Multi-Status");
            _SuccesDict.Add(208, "Already Reported");
            _SuccesDict.Add(210, "Content Different");
            _SuccesDict.Add(226, "IM Used");

            _RedirectionDict.Add(300, "Multiple Choices");
            _RedirectionDict.Add(301, "Moved Permanently");
            _RedirectionDict.Add(302, "Found");
            _RedirectionDict.Add(303, "See Other");
            _RedirectionDict.Add(304, "Not Modified");
            _RedirectionDict.Add(305, "Use Proxy (since HTTP/1.1)");
            _RedirectionDict.Add(306, "Switch Proxy");
            _RedirectionDict.Add(307, "Temporary Redirect");
            _RedirectionDict.Add(308, "Permanent Redirect");
            _RedirectionDict.Add(310, "Too many Redirects");

            _ErrorDict.Add(400, "Bad Request");
            _ErrorDict.Add(401, "Unauthorized");
            _ErrorDict.Add(402, "Payment Required");
            _ErrorDict.Add(403, "Forbidden");
            _ErrorDict.Add(404, "Not Found");
            _ErrorDict.Add(405, "Method Not Allowed");
            _ErrorDict.Add(406, "Not Acceptable");
            _ErrorDict.Add(407, "Proxy Authentication Required");
            _ErrorDict.Add(408, "Request Time-out");
            _ErrorDict.Add(409, "Conflict");
            _ErrorDict.Add(410, "Gone");
            _ErrorDict.Add(411, "Length Required");
            _ErrorDict.Add(412, "Precondition Failed");
            _ErrorDict.Add(413, "Request Entity Too Large");
            _ErrorDict.Add(414, "Request-URI Too Long");
            _ErrorDict.Add(415, "Unsupported Media Type");
            _ErrorDict.Add(416, "Requested range unsatisfiable");
            _ErrorDict.Add(417, "Expectation failed");
            _ErrorDict.Add(418, "I am a teapot");
            _ErrorDict.Add(421, "Bad mapping or Misdirected Request");
            _ErrorDict.Add(422, "Unprocessable entity");
            _ErrorDict.Add(423, "Locked");
            _ErrorDict.Add(424, "Method failure");
            _ErrorDict.Add(425, "Too Early");
            _ErrorDict.Add(426, "Upgrade Required");
            _ErrorDict.Add(428, "Precondition Required");
            _ErrorDict.Add(429, "Too Many Requests");
            _ErrorDict.Add(431, "Request Header Fields Too Large");
            _ErrorDict.Add(449, "Retry With");
            _ErrorDict.Add(450, "Blocked by Windows Parental Controls");
            _ErrorDict.Add(451, "Unavailable For Legal Reasons");
            _ErrorDict.Add(456, "Unrecoverable Error");
            _ErrorDict.Add(444, "No Response (Nginx)");
            _ErrorDict.Add(495, "SSL Certificate Error (Nginx)");
            _ErrorDict.Add(496, "SSL Certificate Required (Nginx)");
            _ErrorDict.Add(497, "HTTP Request Sent to HTTPS Port (Nginx)");
            _ErrorDict.Add(498, "Token expired or invalid (Nginx)");
            _ErrorDict.Add(500, "Internal Server Error");
            _ErrorDict.Add(501, "Not Implemented");
            _ErrorDict.Add(502, "Bad Gateway or Proxy Error");
            _ErrorDict.Add(503, "Service Unavailable");
            _ErrorDict.Add(504, "Gateway Time-out");
            _ErrorDict.Add(505, "HTTP Version not supported");
            _ErrorDict.Add(506, "Variant Also Negotiates");
            _ErrorDict.Add(507, "Insufficient storage");
            _ErrorDict.Add(508, "Loop detected");
            _ErrorDict.Add(509, "Bandwidth Limit Exceeded");
            _ErrorDict.Add(510, "Not extended");
            _ErrorDict.Add(511, "Network authentication required");
            _ErrorDict.Add(520, "Unknown Error (Cloudflare)");
            _ErrorDict.Add(521, "Web Server Is Down (Cloudflare)");
            _ErrorDict.Add(522, "Connection Timed Out (Cloudflare)");
            _ErrorDict.Add(523, "Origin Is Unreachable (Cloudflare)");
            _ErrorDict.Add(524, "A Timeout Occurred (Cloudflare)");
            _ErrorDict.Add(525, "SSL Handshake Failed (Cloudflare)");
            _ErrorDict.Add(526, "Invalid SSL Certificate (Cloudflare)");
            _ErrorDict.Add(527, "Railgun Error (Cloudflare)");
        }

        [Route("/Error/List")]
        public IActionResult Index()
        {
            NWDLogger.Information(nameof(NWDWebErrorController)+"."+nameof(Index)+"();");
            return View();
        }

        [HttpGet]
        public IActionResult Internal(NWDWebError sError)
        {
            NWDLogger.Information(nameof(NWDWebErrorController)+"."+nameof(Internal)+"();");
            AddViewDataObject(sError);
            return View("Error.cshtml");
        }
        [HttpGet]
        [Route("/Exception")]
        public IActionResult Exception()
        {
            NWDLogger.Information(nameof(NWDWebErrorController)+"."+nameof(Exception)+"();");
            NWDWebError tError = new NWDWebError();
            tError.Error = 500;
            if (_ErrorDict.ContainsKey(tError.Error))
            {
                tError.Title = tError.Error.ToString(); 
                tError.Description = _ErrorDict[tError.Error];
            }
            AddViewDataObject(tError);
            return View();
        }
        
        [HttpGet]
        [Route("/Error/{sError}")]
        public IActionResult Error(int sError)
        {
            NWDWebError tError = new NWDWebError();
            tError.Error = sError;
            var tStatusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (tStatusCodeResult != null)
            {
                tError.OriginalUrl = tStatusCodeResult.OriginalPath;
            }
            // tError.OriginalUrl = HttpContext.Request.GetDisplayUrl();
            if (_ErrorDict.ContainsKey(tError.Error))
            {
                tError.Title = tError.Error.ToString(); 
                tError.Description = _ErrorDict[tError.Error];
            }
            AddViewDataObject(tError);
            NWDLogger.Information(nameof(NWDWebErrorController)+"."+nameof(Error)+"("+sError+") for "+tError.OriginalUrl);
            return View();
        }

        [HttpGet]
        [Route("/Error-{sError}")]
        public IActionResult ErrorProxy(int sError)
        {
            NWDLogger.Information(nameof(NWDWebErrorController)+"."+nameof(ErrorProxy)+"();");
            ViewData.Add("DisplayUrl",HttpContext.Request.HttpContext.Request.GetDisplayUrl());
            return StatusCode(sError);
        }
    }
}