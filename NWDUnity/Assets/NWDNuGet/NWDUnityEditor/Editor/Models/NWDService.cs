using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDUnityEditor.Engine;
using NWDUnityShared.Tools;
using System;
using NWDToolbox = NWDUnityShared.Tools.NWDToolbox;

namespace NWDUnityEditor.Models
{
    [Serializable]
    public class NWDService
    {
        public long Reference = NWDRandom.LongNumeric(32);
        public string Name = "Account Service";
        public string Message = "Unity Editor account service";
        public bool UniqueService = false;

        static public implicit operator NWDAccountService(NWDService sService)
        {
            ulong tProjectId = NWDUnityEngineEditor.Instance.Config.GetProjectId();
            NWDEnvironmentKind tEnvironment = NWDUnityEngineEditor.Instance.EnvironmentManager.GetCurrentEnvironment();
            DateTime tStart = DateTime.Now;
            DateTime tEnd = new DateTime(tStart.Year + 1, tStart.Month, tStart.Day);

            return new NWDAccountService
            {
                Service = sService.Reference,
                Name = sService.Name,
                Message = sService.Message,
                ProjectId = tProjectId,
                ServiceKind = NWDAccountServiceKind.Original,
                Status = NWDAccountServiceStatus.IsActive,
                UniqueService = sService.UniqueService,
                Active = true,
                OfflineCounterDown = 10000,
                EnvironmentKind = tEnvironment,
                Start = (int)NWDTimestamp.Timestamp(tStart),
                End = (int)NWDTimestamp.Timestamp(tEnd)
            };
        }
    }
}
