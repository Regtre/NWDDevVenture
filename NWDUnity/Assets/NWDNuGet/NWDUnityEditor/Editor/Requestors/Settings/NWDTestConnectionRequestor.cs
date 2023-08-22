using NWDUnityEditor.Services;
using NWDUnityEditor.Tools;
using System;

namespace NWDUnityEditor.Requestors
{
    public class NWDTestConnectionRequestor : NWDRequestorTask
    {
        private string URL;

        public NWDTestConnectionRequestor(string sURL, Action<NWDRequestorTask> sCallback) : base(sCallback)
        {
            URL = sURL;
        }

        protected override void Execute()
        {
            NWDProjectService.TestConnexion(URL);
        }

        public override string ProgressName()
        {
            return "NWD connection test";
        }

        public override string ProgressDescription()
        {
            return "Testing connection to the NWD server.";
        }
    }
}

