using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NWDFoundation.Logger;
using NWDServerMiddle.Configuration;

namespace NWDServerMiddle.Managers
{
    public class NWDServerHookSlack
    {
        public void Send(string sMessage)
        {
            Task.Run(() => SendAsync(sMessage));
        }

        public async Task SendAsync(string sMessage)
        {
            NWDServerMiddleConfiguration? tSharedInstance = NWDServerMiddleConfiguration.KConfig;
            if (tSharedInstance != null)
            {
                HttpClient tHttpClient = new HttpClient();
                HttpResponseMessage tResult = await tHttpClient.PostAsJsonAsync(tSharedInstance.SlackWebHook, new { text = sMessage });
                if (!tResult.IsSuccessStatusCode)
                {
                    throw new Exception("Task failed.");
                }
            }
            else
            {
                NWDLogger.Error(nameof(NWDServerMiddleConfiguration) + " " + nameof(NWDServerMiddleConfiguration.KConfig) + " is not instantiated!");
            }
        }


        public void SendNotificationException(Exception sException, HttpContext? sHttpContext = null)
        {
            Task.Run(() => SendAsyncNotificationException(sException, sHttpContext));
        }

        public async Task SendAsyncNotificationException(Exception sException, HttpContext? sHttpContext = null)
        {
            NWDServerMiddleConfiguration? tSharedInstance = NWDServerMiddleConfiguration.KConfig;
            if (tSharedInstance != null)
            {
                StringBuilder tMessage = new StringBuilder();
                tMessage.AppendLine("<div><h2>" + sException.Message + "</h2> \n\n\n " + sException.StackTrace + "</div>");
                if (sHttpContext != null)
                {
                    string tBodyStr;
                    using (StreamReader tReader = new StreamReader(sHttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                    {
                        tBodyStr = await tReader.ReadToEndAsync();
                    }
                    tMessage.AppendLine("<div><h3>HttpContext</h3>" + "Request : \n " + sHttpContext.Request.Host + " \n" + sHttpContext.Request.Path + " \n" + " body : \n " + tBodyStr + "</div>");
                }
                else
                {
                    tMessage.AppendLine("<div><h3>HttpContext is null</h3></div>");
                }
                await SendAsync(tMessage.ToString());
            }
            else
            {
                NWDLogger.Error(nameof(NWDServerMiddleConfiguration) + " " + nameof(NWDServerMiddleConfiguration.KConfig) + " is not instantiated!");
            }
        }

    }
}

