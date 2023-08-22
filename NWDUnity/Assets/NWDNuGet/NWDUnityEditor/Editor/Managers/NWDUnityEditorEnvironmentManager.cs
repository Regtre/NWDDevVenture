using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDUnityEditor.Engine;
using NWDUnityShared.Facades;
using System;

namespace NWDUnityEditor.Managers
{
    public class NWDUnityEditorEnvironmentManager : INWDUnityEnvironmentManager
    {
        public int GetCurrentDataTrack()
        {
            int rResult = 0;
            NWDDataTrackDescription tDescription = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            if (tDescription != null)
            {
                rResult = tDescription.Track;
            }
            else
            {
                throw new Exception("No environment could be found !");
            }

            return rResult;
        }

        public NWDEnvironmentKind GetCurrentEnvironment()
        {
            NWDEnvironmentKind rResult;
            NWDDataTrackDescription tDescription = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            if (tDescription != null)
            {
                rResult = tDescription.Kind;
            }
            else
            {
                throw new Exception("No environment could be found !");
            }

            return rResult;
        }
    }
}
