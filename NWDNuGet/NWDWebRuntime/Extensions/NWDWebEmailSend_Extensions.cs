using Microsoft.AspNetCore.Http;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;

namespace NWDWebRuntime.Extensions
{
    public static class NWDWebEmailSendExtensions
    {
        public static void SendEmailSignUp(this NWDEmailManager sWebEmailSend, NWDAccountSign sAccountSign, HttpContext? sHttpContext)
        {
            try
            {
                Dictionary<string, string> tInfos = new Dictionary<string, string>();
                if (sHttpContext != null)
                {
                    tInfos.Add("HttpContext.TraceIdentifier", sHttpContext.TraceIdentifier);
                    if (sHttpContext.Connection.RemoteIpAddress != null)
                    {
                        tInfos.Add("HttpContext.Connection.RemoteIpAddress", sHttpContext.Connection.RemoteIpAddress.ToString());
                    }
                    foreach (string tKey in sHttpContext.Request.Headers.Keys)
                    {
                        string? tValue = sHttpContext.Request.Headers[tKey];
                        if (string.IsNullOrEmpty(tValue) == false)
                        {
                            tInfos.Add(tKey, tValue);
                        }
                    }
                }
                string tInformation = "<table>";
                foreach (KeyValuePair<string, string> tKeyValue in tInfos)
                {
                    tInformation = tInformation + "<tr><td><b>" + tKeyValue.Key + "</b></td><td>" + tKeyValue.Value + "</td></tr>";
                }
                tInformation = tInformation + "</table>";
                string tBody = "<div><h1>SignUp</h1><div>Use method "+sAccountSign.SignType.ToString()+" "+sAccountSign.Account.ToString()+"</div><div>" +"<div><h1>Information</h1>"+tInformation+"</div>";
                if (NWDEmailConfiguration.KConfig != null)
                {
                    sWebEmailSend.SendMailNow(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailNoReply,NWDEmailConfiguration.KConfig.EmailNoReply, NWDEmailConfiguration.KConfig.EmailWebsite,
                        "Inscription to " + NWDWebRuntimeConfiguration.KConfig.GetDnsHttps(), tBody);
                }
            }
            catch (Exception tException)
            {
                NWDLogger.Exception(tException);
            }
        }

        public static void SendEmailFromContact(this NWDEmailManager sWebEmailSend, NWDContactUsModel sModel, HttpContext sHttpContext)
        {
            try
            {
                Dictionary<string, string> tInfos = new Dictionary<string, string>();
                if (string.IsNullOrWhiteSpace(sModel.Subject))
                {
                    sModel.Subject = "Contact";
                }
                tInfos.Add("HttpContext.TraceIdentifier", sHttpContext.TraceIdentifier);
                if (sHttpContext.Connection.RemoteIpAddress != null)
                {
                    tInfos.Add("HttpContext.Connection.RemoteIpAddress", sHttpContext.Connection.RemoteIpAddress.ToString());
                }
                foreach (string tKey in sHttpContext.Request.Headers.Keys)
                {
                    string? tValue = sHttpContext.Request.Headers[tKey];
                    if (string.IsNullOrEmpty(tValue) == false)
                    {
                        tInfos.Add(tKey, tValue);
                    }
                }
                string tInformation = "<table>";
                foreach (KeyValuePair<string, string> tKeyValue in tInfos)
                {
                    tInformation = tInformation + "<tr><td><b>" + tKeyValue.Key + "</b></td><td>" + tKeyValue.Value + "</td></tr>";
                }
                tInformation = tInformation + "</table>";
                string tBody =
                    "<div>" +
                        "<h1>Email From</h1>" +
                        "<div>" + sModel.SenderName + " (" + sModel.SenderEmail + ")</div>";
                tBody = tBody +
                        "<h2>Message</h2>" +
                        "<h3>Subject : " + sModel.Subject + "</h3>";
                if (string.IsNullOrWhiteSpace(sModel.Category) == false)
                {
                    tBody = tBody + "<h3>Category : " + sModel.Category + "</h3>";
                }
                if (string.IsNullOrWhiteSpace(sModel.SubCategory) == false)
                {
                    tBody = tBody + "<h3>SubCategory : " + sModel.SubCategory + "</h3>";
                }
                tBody = tBody + "<div>" + sModel.Message + "</div>" +
                        "<h2>Information</h2>" +
                        "<div>" + tInformation + "</div>" +
                    "</div>" +
                    "";
                if (NWDEmailConfiguration.KConfig != null)
                {
                     sWebEmailSend.SendMailNow(NWDEmailConfiguration.KConfig, NWDEmailConfiguration.KConfig.EmailNoReply, sModel.SenderEmail, NWDEmailConfiguration.KConfig.EmailWebsite, sModel.Subject + " " + NWDWebRuntimeConfiguration.KConfig.GetDnsHttps(), tBody);
                }
            }
            catch (Exception tException)
            {
                NWDLogger.Exception(tException);
            }
        }
    }
}
