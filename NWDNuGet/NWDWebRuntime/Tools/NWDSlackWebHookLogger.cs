using NWDFoundation.Logger;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using NWDWebRuntime.Configuration;

#nullable enable
namespace NWDWebRuntime.Tools
{
    // https://app.slack.com/block-kit-builder/T9ZTQ05GX#%7B%22blocks%22:%5B%7B%22type%22:%22context%22,%22elements%22:%5B%7B%22type%22:%22mrkdwn%22,%22text%22:%22*%20🤖%20trace*%22%7D,%7B%22type%22:%22mrkdwn%22,%22text%22:%22FAILED%22%7D%5D%7D,%7B%22type%22:%22section%22,%22text%22:%7B%22type%22:%22mrkdwn%22,%22text%22:%22FAILED%22%7D%7D,%7B%22type%22:%22section%22,%22text%22:%7B%22type%22:%22mrkdwn%22,%22text%22:%22#%20This%20is%20a%20mrkdwn%20section%20block%20:ghost:%20*this%20is%20bold*,%20and%20~this%20is%20crossed%20out~,%20and%20%3Chttps://google.com%7Cthis%20is%20a%20link%3E%22%7D%7D%5D%7D
    public class NWDSlackWebHookLogger : INWDLogger
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
            //Console.WriteLine(JsonConvert.SerializeObject(sMessage, Formatting.Indented));
            if (!tResult.IsSuccessStatusCode)
            {
                throw new Exception("Task failed.");
            }
        }

        public NWDSlackWebHookLogger(string sHook)
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
            var tMessage = new
            {
                blocks = new object[]
                {
                    new
                    {
                        type = "header",
                        text = new
                        {
                            type = "plain_text",
                            text = NWDWebRuntimeConfiguration.KConfig.Dns,
                            emoji = true
                        }
                    },
                    // new
                    // {
                    //     type = "divider",
                    // },
                    new
                    {
                        type = "context",
                        elements = new object[]
                        {
                            new
                            {
                                type = "mrkdwn",
                                text = WriteIcon(sLogLevel)
                            },
                            new
                            {
                                type = "mrkdwn",
                                text = WriteCategory(sLogCategory)
                            },
                        },
                    },
                    new
                    {
                        type = "section",
                        text = new
                        {
                            type= "mrkdwn",
                            text= tMess.ToString(),
                        }
                    },
                    // new
                    // {
                    // type = "divider",
                    // },
                }
            };
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
                
                var tMessage = new
                {
                    blocks = new object[]
                    {
                        new
                        {
                            type = "header",
                            text = new
                            {
                                type = "plain_text",
                                text = NWDWebRuntimeConfiguration.KConfig.Dns,
                                emoji = true
                            }
                        },
                        // new
                        // {
                        //     type = "divider",
                        // },
                        new
                        {
                            type = "context",
                            elements = new object[]
                            {
                                new
                                {
                                    type = "mrkdwn",
                                    text = WriteIcon(sLogLevel)
                                },
                                new
                                {
                                    type = "mrkdwn",
                                    text = WriteCategory(sLogCategory)
                                },
                            },
                        },
                        new
                        {
                            type = "section",
                            text = new
                            {
                                type= "mrkdwn",
                                text= tMess.ToString(),
                            }
                        },
                        // new
                        // {
                        // type = "divider",
                        // },
                    }
                };
                Send(tMessage);
            }
        }
    }
#nullable disable