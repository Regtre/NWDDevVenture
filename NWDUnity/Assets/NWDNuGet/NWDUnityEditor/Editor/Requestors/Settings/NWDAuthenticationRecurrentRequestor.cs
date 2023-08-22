using NWDUnityEditor.Constants;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Services;
using NWDUnityEditor.Tools;

namespace NWDUnityEditor.Requestors
{
    public class NWDAuthenticationRecurrentRequestor : NWDRequestorRecurrentTask
    {
        public NWDAuthenticationRecurrentRequestor() : base (NWDEditorConstants.UpdateSettings, true, null)
        {
        }

        protected override void Execute()
        {
            string tPrivateToken = NWDUnityEngineEditor.Instance.GetConfig().GetPrivateRoleToken();
            string tPublicToken = NWDUnityEngineEditor.Instance.GetConfig().GetPublicRoleToken();
            NWDProjectService.Authenticate(tPrivateToken, tPublicToken, false);
        }

        public override string ProgressName()
        {
            return "NWD settings update";
        }

        public override string ProgressDescription()
        {
            return "Checking for NWD project configuration update with the server.";
        }
    }
}

