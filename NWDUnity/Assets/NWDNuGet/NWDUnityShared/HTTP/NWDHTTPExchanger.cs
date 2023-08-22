using Newtonsoft.Json;
using NWDUnityShared.Constants;
using NWDUnityShared.Engine;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace NWDUnityShared.Exchanges
{
    public abstract class NWDHTTPExchanger
    {
        protected HttpWebRequest PrepareHTTPRequest (string sURL, string tBody = null)
        {
            HttpWebRequest tHttpRequest = (HttpWebRequest)WebRequest.Create(sURL);
            tHttpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            tHttpRequest.ContentType = "application/json; charset=utf-8";
            tHttpRequest.Method = "POST";
            tHttpRequest.Timeout = NWDConstantsUnityRuntime.K_ServerRequestTimeout * 1000;

            if (!string.IsNullOrWhiteSpace(tBody))
            {
                using (var tStreamWriter = new StreamWriter(tHttpRequest.GetRequestStream()))
                {
                    tStreamWriter.Write(tBody);
                }
            }

            return tHttpRequest;
        }

        protected string GetResponseBody(HttpWebResponse sHttpResponse)
        {
            if (sHttpResponse.StatusCode == HttpStatusCode.OK)
            {
                NWDUnityEngine.Instance.ConnectionState = Enumerations.NWDConnectionState.Online;
            }
            else
            {
                if (sHttpResponse.StatusCode == HttpStatusCode.RequestTimeout)
                {
                    NWDUnityEngine.Instance.ConnectionState = Enumerations.NWDConnectionState.Offline;
                }

                throw new Exception("HTTP error: " + sHttpResponse.StatusCode);
            }

            string tResponseText = string.Empty;
            using (var rReader = new StreamReader(sHttpResponse.GetResponseStream(), Encoding.UTF8))
            {
                tResponseText = rReader.ReadToEnd();
            }

            return tResponseText;
        }
    }

    public abstract class NWDHTTPExchanger<REQ, RES> : NWDHTTPExchanger, INWDExchanger<REQ, RES>
    {
        public abstract string DefaultURI { get; }

        public RES SendSync(REQ sRequest, string sURL)
        {
            string tBody = JsonConvert.SerializeObject(sRequest, Formatting.Indented);
            HttpWebRequest tHttpRequest = PrepareHTTPRequest(sURL, tBody);
            using (HttpWebResponse tHttpResponse = (HttpWebResponse)tHttpRequest.GetResponse())
            {
                string tResponseBody = GetResponseBody(tHttpResponse);

                return ParseResponseBody(tResponseBody);
            }
        }

        public RES SendAsync(REQ sRequest, string sURL)
        {
            string tBody = JsonConvert.SerializeObject(sRequest, Formatting.Indented);
            HttpWebRequest tHttpRequest = PrepareHTTPRequest(sURL, tBody);
            using (HttpWebResponse tHttpResponse = (HttpWebResponse)tHttpRequest.GetResponseAsync().Result)
            {
                string tResponseBody = GetResponseBody(tHttpResponse);

                return ParseResponseBody(tResponseBody);
            }
        }

        private RES ParseResponseBody(string sResponseBody)
        {
            return JsonConvert.DeserializeObject<RES>(sResponseBody);
        }
    }
}

