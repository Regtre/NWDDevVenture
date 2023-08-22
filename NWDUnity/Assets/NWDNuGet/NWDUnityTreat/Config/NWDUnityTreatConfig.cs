using NWDFoundation.Configuration.Environments;
using NWDFoundation.Facades;
using NWDFoundation.Models;
using NWDUnityEditor.Engine;

namespace NWDUnityTreat.Config
{
    public class NWDUnityTreatConfig : INWDTreatKey
    {
        static private NWDUnityTreatConfig instance;
        public static NWDUnityTreatConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NWDUnityTreatConfig();
                }
                return instance;
            }
        }

        public string GetTreatInstanceName()
        {
            return NWDUnityEngineEditor.Instance.GetConfig().GetConfigName();
        }

        public string GetTreatKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind)
        {
            string rResult = string.Empty;

            NWDProjectCredentials tCredentials = NWDUnityEngineEditor.Instance.GetConfig().GetCredentials(sProjectId, sEnvironmentKind);
            if (tCredentials != null)
            {
                rResult = tCredentials.TreatKey;
            }

            return rResult;
        }
    }
}
