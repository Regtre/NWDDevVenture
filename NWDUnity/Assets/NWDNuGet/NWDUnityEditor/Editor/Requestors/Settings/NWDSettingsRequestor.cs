using NWDUnityEditor.Tools;

namespace NWDUnityEditor.Requestors
{
    public class NWDSettingsRequestor : NWDRequestor<NWDSettingsRequestor>
    {
        public NWDSettingsRequestor() : base()
        {
            RecurrentTasks.Add(new NWDAuthenticationRecurrentRequestor());
        }

        public NWDAuthenticationRecurrentRequestor GetRecurrentAuthentication()
        {
            return GetRecurrentTask(0) as NWDAuthenticationRecurrentRequestor;
        }
    }
}

