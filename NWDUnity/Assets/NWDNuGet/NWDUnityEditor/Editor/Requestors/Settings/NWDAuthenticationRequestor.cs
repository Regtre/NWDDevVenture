using NWDUnityEditor.Services;
using NWDUnityEditor.Tools;
using System;

namespace NWDUnityEditor.Requestors
{
    public class NWDAuthenticationRequestor : NWDRequestorTask
    {
        private string PrivateToken;
        private string PublicToken;
        private bool ForceUpdate;

        public NWDAuthenticationRequestor(string sPublicToken, string sPrivateToken, bool sForceUpdate, Action<NWDRequestorTask> sCallback) : base(sCallback)
        {
            PrivateToken = sPrivateToken;
            PublicToken = sPublicToken;
            ForceUpdate = sForceUpdate;
        }

        protected override void Execute()
        {
            NWDProjectService.Authenticate(PrivateToken, PublicToken, ForceUpdate);
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

