using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NWDFoundation.Exchanges;
using NWDFoundation.Logger;
using NWDFoundation.Models.Enums;
using NWDRuntime.Exchanges;
using NWDRuntime.Facades;
using NWDServerMiddle.Configuration;
using NWDServerMiddle.Managers;
using NWDServerShared.Configuration;
using JsonElement = System.Text.Json.JsonElement;

namespace NWDServerFront.Controllers
{
    [ApiController]
    [Route("NWDRuntime")]
    // [Route("[controller]")]
    public class NWDRuntimeController : ControllerBase
    {
        #region properties

        private readonly INWDRuntimeManager _Manager;

        #endregion

        #region constructor

        public NWDRuntimeController(NWDRuntimeManager sManager)
        {
            _Manager = sManager;
        }

        #endregion

        #region methods

        // [HttpPost]
        // public NWDResponseRuntime TestPost()
        // {
        //     NWDServerDebug.WriteLine(" TestPost () ok ");
        //     return new NWDResponseRuntime();
        // }
        // [HttpGet(Name = "GetSync")]
        // public NWDResponseRuntime Get()
        // {
        //     NWDServerDebug.WriteLine(" GetSync () ok ");
        //     return new NWDResponseRuntime();
        // }

        [HttpPost(Name = "PostSync")]
        public NWDResponseRuntime Post([FromBody] NWDRequestRuntime sRequestRuntime)
        {
            //NWDLogger.Trace(" NWDRuntimeController Receipt POST", NWDLogger.SplitObjectSerializable(sRequestRuntime.PlayerToken));
            if (NWDServerConfiguration.KConfig.Status == NWDServerStatus.Active)
            {
                NWDResponseRuntime tResponse = _Manager.Process(sRequestRuntime);
                //NWDLogger.Trace(" NWDRuntimeController Generate PlayerToken for "+tResponse.Status.ToString(), NWDLogger.SplitObjectSerializable(tResponse.PlayerToken));
                return tResponse;
            }
            else
            {
                NWDResponseRuntime tResponse = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sRequestRuntime.PlayerToken, NWDExchangeRuntimeKind.None, null, NWDRequestStatus.ServerIsDisabled);
                //NWDLogger.Warning(" SERVER IS DISABLED!");
                //NWDLogger.Trace(" NWDRuntimeController Generate PlayerToken for "+tResponse.Status.ToString(), NWDLogger.SplitObjectSerializable(tResponse.PlayerToken));
                return tResponse;
            }
        }
        
        // [HttpPost(Name = "PostSyncJava")]
        // public NWDResponseRuntime PostJava([FromBody] JsonElement element)
        // {
        //     
        //     string sJson = element.GetRawText();
        //     NWDRequestRuntime sRequestRuntime =  JsonConvert.DeserializeObject<NWDRequestRuntime>(sJson);
        //     //NWDLogger.Trace(" NWDRuntimeController Receipt POST", NWDLogger.SplitObjectSerializable(sRequestRuntime.PlayerToken));
        //     if (NWDServerConfiguration.KConfig.Status == NWDServerStatus.Active)
        //     {
        //         NWDResponseRuntime tResponse = _Manager.Process(sRequestRuntime);
        //         //NWDLogger.Trace(" NWDRuntimeController Generate PlayerToken for "+tResponse.Status.ToString(), NWDLogger.SplitObjectSerializable(tResponse.PlayerToken));
        //         return tResponse;
        //     }
        //     else
        //     {
        //         NWDResponseRuntime tResponse = new NWDResponseRuntime(NWDServerMiddleConfiguration.KConfig, sRequestRuntime.PlayerToken, NWDExchangeRuntimeKind.None, null, NWDRequestStatus.ServerIsDisabled);
        //         //NWDLogger.Warning(" SERVER IS DISABLED!");
        //         //NWDLogger.Trace(" NWDRuntimeController Generate PlayerToken for "+tResponse.Status.ToString(), NWDLogger.SplitObjectSerializable(tResponse.PlayerToken));
        //         return tResponse;
        //     }
        // }


        #endregion
    }
}