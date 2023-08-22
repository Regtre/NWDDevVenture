using NWDFoundation.Configuration.Environments;

namespace NWDFoundation.Facades
{
    public interface INWDProjectKey
    {
        #region interfaces
        public string GetProjectKeyInstanceName();
        public string GetProjectKey(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind);

        #endregion
    }
}