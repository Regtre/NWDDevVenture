using NWDFoundation.Logger;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using NWDWebRuntime.Configuration;

#nullable enable
namespace NWDWebRuntime.Tools
{
    //  https://adaptivecards.io/samples/
    public class NWDTeamsWebHookLogger : INWDLogger
    {
        public string Hook { set; get; } = string.Empty;
        public static readonly HttpClient HttpClientShared = new HttpClient();
        private NWDLogLevel _Level = NWDLogLevel.None;

        private void Send(object sMessage)
        {
            Task.Run(() => SendAsync(sMessage));
        }

        private async Task SendAsync(object sMessage)
        {
            HttpClient tHttpClient = new HttpClient();
            HttpResponseMessage tResult = await tHttpClient.PostAsJsonAsync(Hook, sMessage);
//             string tMessage = ""+
//            " {" +
// "                \"type\":\"message\"," +
// "                \"attachments\":[" +
// "                {" +
// "                    \"contentType\":\"application/vnd.microsoft.card.adaptive\"," +
// "                    \"contentUrl\":null," +
// "                    \"content\":{" +
// "                        \"$schema\":\"http://adaptivecards.io/schemas/adaptive-card.json\"," +
// "                        \"type\":\"AdaptiveCard\"," +
// "                        \"version\":\"1.2\"," +
// "                        \"body\":[" +
// "                        {" +
//            "                 \"title\": \""+NWDWebRuntimeConfiguration.KConfig.Dns+"\"," +
// "                            \"type\": \"TextBlock\"," +
// "                            \"text\": \"For Samples and Templates, see [https://adaptivecards.io/samples](https://adaptivecards.io/samples)\"" +
// "                        }" +
// "                        ]" +
// "                    }" +
// "                }" +
// "                ]" +
// "            }" +
//             "";
            //HttpResponseMessage tResult = await tHttpClient.PostAsync(Hook, new StringContent(tMessage));
            if (!tResult.IsSuccessStatusCode)
            {
                throw new Exception("Task failed.");
            }
        }

        public NWDTeamsWebHookLogger(string sHook)
        {
            Hook = sHook;
            _Level = NWDLogLevel.Trace;
        }

        public void SetLogLevel(NWDLogLevel sLogLevel)
        {
            _Level = sLogLevel;
        }

        public void LoadLogLevel()
        {
        }

        public NWDLogLevel DefaultLogLevel()
        {
            return NWDLogLevel.Information;
        }

        public bool IsActivated()
        {
            return true;
        }

        public NWDLogLevel LogLevel()
        {
            return _Level;
        }

        private string WriteCategory(NWDLogCategory sLogCategory)
        {
            StringBuilder rReturn = new StringBuilder();
            switch (sLogCategory)
            {
                case NWDLogCategory.No:
                    rReturn.Append(" ");
                    break;
                case NWDLogCategory.Todo:
                    rReturn.Append("TODO");
                    break;
                case NWDLogCategory.Success:
                    rReturn.Append("SUCCESS");
                    break;
                case NWDLogCategory.Error:
                    rReturn.Append("ERROR");
                    break;
                case NWDLogCategory.Attention:
                    rReturn.Append("WARNING");
                    break;
                case NWDLogCategory.Exception:
                    rReturn.Append("EXCEPTION");
                    break;
                case NWDLogCategory.Failed:
                    rReturn.Append("FAILED");
                    break;
            }

            return rReturn.ToString();
        }

        private string WriteIcon(NWDLogLevel sLogLevel)
        {
            StringBuilder rReturn = new StringBuilder();
            switch (sLogLevel)
            {
                case NWDLogLevel.Trace:
                    rReturn.AppendLine("🤖 *trace*");
                    break;
                case NWDLogLevel.Debug:
                    rReturn.AppendLine("🤔 *debug*");
                    break;
                case NWDLogLevel.Information:
                    rReturn.AppendLine("🧐 *info*");
                    break;
                case NWDLogLevel.Warning:
                    rReturn.AppendLine("😰 *warning*");
                    break;
                case NWDLogLevel.Error:
                    rReturn.AppendLine("🤬 *error*");
                    break;
                case NWDLogLevel.Critical:
                    rReturn.AppendLine("💀 *critical*");
                    break;
                case NWDLogLevel.None:
                    rReturn.AppendLine(" ");
                    break;
            }

            return rReturn.ToString();
        }

        public string WriteObject(object? sObject)
        {
            StringBuilder rReturn = new StringBuilder();
            if (sObject != null)
            {
                Exception? tException = sObject as Exception;
                if (tException != null)
                {
                    rReturn.Append(tException.ToString());
                }
            }

            return rReturn.ToString();
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sString, object? sObject)
        {
            StringBuilder tMess = new StringBuilder();
            tMess.AppendLine(sString);
            if (sObject != null)
            {
                tMess.AppendLine(JsonConvert.SerializeObject(sObject, Formatting.Indented));
            }

            var tMessage = new { text = NWDWebRuntimeConfiguration.KConfig.Dns + "\n\n" + WriteIcon(sLogLevel) + " " + WriteCategory(sLogCategory) + " \n\n" + tMess.ToString() };
            Send(tMessage);
        }

        public void WriteLog(NWDLogLevel sLogLevel, NWDLogCategory sLogCategory, string sTitle, object? sObject, string[] sMessages)
        {
            StringBuilder tMess = new StringBuilder();
            tMess.AppendLine(string.Join("\n- ", sMessages) + "\n");
            if (sObject != null)
            {
                tMess.AppendLine(JsonConvert.SerializeObject(sObject, Formatting.Indented));
            }

            var tMessage = new { text = NWDWebRuntimeConfiguration.KConfig.Dns + "\n\n" + WriteIcon(sLogLevel) + " " + WriteCategory(sLogCategory) + " \n\n" + tMess.ToString() };
            Send(tMessage);
        }
    }
}
#nullable disable